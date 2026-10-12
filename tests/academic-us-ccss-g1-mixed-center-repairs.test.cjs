'use strict';
const {test}=require('node:test'), assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),{createHash}=require('node:crypto');
const root=path.resolve(__dirname,'..'), read=p=>fs.readFileSync(path.join(root,p),'utf8');
const pack=JSON.parse(read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json'));
const proofs=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-mixed-center-source-guarded-repairs.json'));
const authored=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-mixed-center-repair-proposals.json'));
const teacher=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-146-local-teacher-source-evidence.json'));
const cls=read('src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs');
const seed=read('src/Edulytics.Data/Seeding/MathematicsCanonicalLessonContentSeeder.cs');
const con=k=>{const m=cls.match(new RegExp('public const string '+k+' = ("(?:[^"\\\\]|\\\\.)*");'));assert.ok(m,'Missing '+k);return JSON.parse(m[1]);};
const digest=e=>createHash('sha256').update([e.cultureCode,e.title,e.explanation,e.keyConceptsAndRules,e.workedExamples,e.stepByStepSolutions,e.commonMistakes,e.quickSummary].join('\n')+'\n').digest('hex');
test('four remaining G1 mixed-center lesson corrections target exact original source identity',()=>{
 assert.equal(proofs.count,4);
 assert.equal(new Set(proofs.lessons.map(x=>x.lessonCode)).size,4);
 assert.equal(proofs.academicL7,'PENDING');
 assert.ok(seed.includes('expectedSourceBoundCorrectionCount = 12 + 19 + 3 + 7 + 6 + 6 + 4;'));
 for(const authoredLesson of authored.lessons){
  const id='PED:US-CCSS-MATH:'+authoredLesson.short,raw=pack.lessons.find(x=>x.lessonCode===id),
   pinned=proofs.lessons.find(x=>x.lessonCode===id),local=teacher.records.find(x=>x.lessonCode===id);
  assert.ok(raw&&pinned&&local,id);
  const en=raw.translations.find(x=>x.cultureCode==='en');
  assert.equal(pinned.priorEnglishBodySha256,digest(en));
  assert.equal(pinned.canonicalBodySha256,raw.canonicalBodySha256);
  assert.equal(pinned.localTeacherSourceSha256,local.teacherPreparationSha256);
  assert.deepEqual(pinned.officialOutcomes,raw.outcomeCodes);
  assert.equal(con(authoredLesson.key+'LessonCode'),id);
  assert.equal(con(authoredLesson.key+'Version'),pinned.newVersion);
  assert.ok(cls.includes('['+authoredLesson.key+'LessonCode] = new Repair('));
  for(const v of [raw.sourceUrl,raw.canonicalBodySha256,pinned.priorEnglishBodySha256,en.stepByStepSolutions])
   assert.ok(cls.includes(JSON.stringify(v)),id+': missing fail-closed guard');
  for(const v of raw.outcomeCodes)assert.ok(cls.includes(JSON.stringify(v)),id+': standard guard');
 }
});
test('worked exercises include independently verifiable equalities and mixed-center skills',()=>{
 let count=0;
 for(const l of authored.lessons){
  const examples=con(l.key+'Examples'),steps=con(l.key+'Steps'),concepts=con(l.key+'Concepts'),mistakes=con(l.key+'Mistakes');
  assert.equal(examples,l.examples);assert.equal(steps,l.steps);assert.equal(concepts,l.concepts);assert.equal(mistakes,l.mistakes);
  assert.equal((examples.match(/Example \d:/g)||[]).length,4,l.short);
  assert.equal((steps.match(/Step \d:/g)||[]).length,5,l.short);
  assert.ok(examples.length>350&&mistakes.length>150,l.short);
  for(const m of (examples+'\n'+steps).matchAll(/(\d+)\s*([+-])\s*(\d+)(?:\s*([+-])\s*(\d+))?\s*=\s*(\d+)(?!\d)/g)){
   let a=m[2]==='+'?Number(m[1])+Number(m[3]):Number(m[1])-Number(m[3]);
   if(m[4])a=m[4]==='+'?a+Number(m[5]):a-Number(m[5]);
   assert.equal(a,Number(m[6]),l.short+': '+m[0]);count++;
  }
 }
 assert.ok(count>=12,'Arithmetic checks unexpectedly few '+count);
 const measure=con('EstimateMeasureAndAddCenterNextExamples');
 assert.match(measure,/cube lengths/); assert.match(measure,/24 \+ 30 = 54/);
 assert.match(con('CountTo120MeasureAndRecordExamples'),/112/);
 assert.match(con('DrawSolidShapesAndNumbersCenterExamples'),/L shape|cylinder/i);
 assert.match(con('SolidFlatShapesClockArithmeticCenterExamples'),/4:30/);
 assert.ok(!cls.includes('ACADEMICALLY_CERTIFIED'));
});