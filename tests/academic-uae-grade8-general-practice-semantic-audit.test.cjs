'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const audit=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G8/general-practice-semantic-audit.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json');
const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
const exact=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/UaeGrade8GeneralSemanticQuestions.cs'),'utf8');
const fixed=['05:02','06:03','11:06','12:01','12:02','12:03'];
test('G8 76 scoped semantic audit rows preserve Arabic academic identity and do not invent broad 100% certification',()=>{
 assert.equal(audit.lessonsCount,76);
 assert.equal(audit.lessons.length,76);
 assert.equal(pack.Lessons.length,76);
 assert.equal(audit.summary.fullPedagogicalCoverageCertified,0);
 assert.equal(audit.summary.publisherPageItemParityCertified,0);
 assert.equal(audit.summary.lessonScopedAutomatedSubsetRemediationsCommitted,6);
 assert.equal(audit.summary.awaitingSemanticsInspection,70);
 assert.equal(audit.academicLanguage,'ar');
 const used=new Set();
 for(let i=0;i<76;i++){
  const a=audit.lessons[i],p=pack.Lessons[i],code=p.LessonCode;
  assert.equal(a.ordinal,i+1);
  assert.equal(a.lessonCode,code);
  assert.equal(a.arabicTitle,p.Translations[0].Title);
  assert.ok(!used.has(code));used.add(code);
  assert.ok(fs.existsSync(path.join(root,a.richV2Path)),a.richV2Path);
  const rich=read(a.richV2Path);
  assert.equal(rich.Lessons[0].LessonCode,code);
  assert.equal(rich.Lessons[0].CultureCode,'ar');
  const needle='byLesson["'+code+'"]';
  const at=contracts.indexOf(needle);
  assert.ok(at>=0,'registered '+code);
  const statement=contracts.slice(at,contracts.indexOf(';',at)+1);
  const descriptor=statement.slice(statement.indexOf('new LessonPracticeContract'));
  const actual=descriptor.match(/new LessonPracticeContract\(\s*"[^"]+"\s*,\s*"[^"]+"\s*,\s*"([^"]+)"\s*,\s*\[([^\]]*)\]/);
  assert.ok(actual,'registered family array '+code);
  assert.equal(a.mechanic,actual[1]);
  assert.deepEqual(a.allowedFamilies,[...actual[2].matchAll(/"([^"]+)"/g)].map(m=>m[1]));
  assert.ok(a.allowedFamilies.length>0);
  const id=code.split(':').slice(4,6).join(':');
  if(fixed.includes(id)){
   assert.equal(a.semanticAuditStatus,'PARTIAL_TEACHER_REVIEW');
   assert.match(a.verifiedAutoGradeSubset,/^[A-Z0-9_]+$/);
   assert.ok(a.teacherReviewRequired.length>35);
   assert.ok(a.allowedFamilies.every(f=>f.startsWith('uae.g8.')));
  }else{
   assert.equal(a.semanticAuditStatus,'PENDING_SEMANTIC_REVIEW');
   assert.equal(a.sampledLearnerPromptVerified,false);
  }
 }
});
test('the six formerly mismapped G8 lessons have no unrelated arithmetic, chance or reflection Practice',()=>{
 const forbidden=[
  ['05:02:GEOMETRIC-PROOF','supporting.algebra.equations.mixed'],
  ['06:03:ROTATIONS','uae.g6.u05.l11.reflect_both_axes'],
  ['11:06:TRAPEZOIDS-AND-KITES','uae.g6.u09.l06.trapezoid_area'],
  ['12:01:INTRODUCTION-TO-SET-THEORY','supporting.sets.union_count'],
  ['12:02:SUBSETS-AND-OPERATIONS-ON-SETS','probability.theoretical.complement'],
  ['12:03:USING-VENN-DIAGRAMS-TO-STUDY-SET-OPERATIONS','probability.theoretical.complement']
 ];
 for(const [id,badFamily] of forbidden){
  const code=audit.lessons.find(l=>l.lessonCode.includes(':'+id)).lessonCode;
  const a=audit.lessons.find(l=>l.lessonCode===code);
  assert.ok(!a.allowedFamilies.includes(badFamily),code);
 }
 assert.match(exact,/UaeGrade8GeneralSemanticQuestions\.Verify\(family, parameters, answer\)/);
 assert.match(exact,/UaeGrade8GeneralSemanticQuestions\.Solve\(problem.Family, p\)/);
 assert.match(exact,/UaeGrade8GeneralSemanticQuestions\.Build\(family, random\)/);
 assert.match(engine,/internal const string Rotation = "uae\.g8\.u06\.l03\.rotation_origin_coordinate"/);
 assert.match(engine,/internal const string VennRegions = "uae\.g8\.u12\.l03\.three_set_venn_region_count"/);
});
