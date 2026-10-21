'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const rd=p=>fs.readFileSync(path.join(root,p),'utf8');
const evidence=JSON.parse(rd('artifacts/academic-us-ccss/grade-reports/G01-six-local-source-verified-corrections.json'));
const pack=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json'));
const cls=rd('src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs');
const seed=rd('src/Edulytics.Data/Seeding/MathematicsCanonicalLessonContentSeeder.cs');
function readSourceConstant(source,key) {
 const declaration='public const string '+key+' = ';
 const start=source.indexOf(declaration);
 assert.ok(start>=0,'Missing source-guarded constant '+key);
 const rhs=source.slice(start+declaration.length);
 const direct=rhs.match(/^("(?:[^"\\]|\\.)*")/);
 if(direct)return JSON.parse(direct[1]);
 const appended=rhs.match(/^([A-Za-z_]\w*)\s*\+\s*("(?:[^"\\]|\\.)*")/);
 assert.ok(appended,'Unrecognized source-guarded constant '+key);
 return readSourceConstant(source,appended[1])+JSON.parse(appended[2]);
}
const constants=name=>readSourceConstant(cls,name);
const hash=en=>crypto.createHash('sha256').update([en.cultureCode,en.title,en.explanation,en.keyConceptsAndRules,en.workedExamples,en.stepByStepSolutions,en.commonMistakes,en.quickSummary].join('\n')+'\n').digest('hex');
test('all six Grade 1 fixes exactly source/English-SHA bound and unapproved',()=>{
 assert.equal(evidence.lessons.length,6);assert.equal(new Set(evidence.lessons.map(x=>x.lessonCode)).size,6);
 assert.equal(evidence.independentAcademicSignoffs,0);
 assert.match(seed,/expectedSourceBoundCorrectionCount = 12 \+ 19 \+ 3 \+ 7 \+ 6(?: \+ \d+)*;/);
 for(const e of evidence.lessons){
  const l=pack.lessons.find(x=>x.lessonCode===e.lessonCode);assert.ok(l,e.lessonCode);
  const en=l.translations.find(t=>t.cultureCode==='en');
  assert.equal(hash(en),e.originalEnglishBodySha256);assert.equal(l.canonicalBodySha256,e.canonicalBodySha256);
  assert.equal(constants(e.correctionKey+'LessonCode'),e.lessonCode);
  const historicalVersion=e.correctionKey==='ChooseIndirectLengthComparator'
   ?e.correctionKey+'VersionV1':e.correctionKey+'Version';
  assert.equal(constants(historicalVersion),e.contentVersion);
  if(historicalVersion!==e.correctionKey+'Version'){
   assert.match(constants(e.correctionKey+'Version'),/-v2$/);
   assert.notEqual(constants(e.correctionKey+'Version'),e.contentVersion);
   assert.ok(constants(e.correctionKey+'Concepts').startsWith(
      constants(e.correctionKey+'ConceptsV1')));
  }
  assert.ok(cls.includes('['+e.correctionKey+'LessonCode] = new Repair('));
  assert.ok(cls.includes(JSON.stringify(l.sourceUrl)));assert.ok(cls.includes(JSON.stringify(l.canonicalBodySha256)));
  assert.ok(cls.includes(JSON.stringify(e.originalEnglishBodySha256)));
  assert.ok(cls.includes(JSON.stringify(en.stepByStepSolutions)));
  assert.match(e.localOfficialTeacherPreparationSha256,/^[a-f0-9]{64}$/);
  const example=constants(e.correctionKey+'Examples'),steps=constants(e.correctionKey+'Steps'),concepts=constants(e.correctionKey+'Concepts');
  assert.ok(example.length>230&&steps.length>190&&concepts.length>100,e.lessonCode);
  const stepNumbers=[...steps.matchAll(/Step (\d+):/g)].map(x=>Number(x[1]));
  assert.ok(stepNumbers.length>=5,e.lessonCode);
  assert.deepEqual(stepNumbers,stepNumbers.map((_,i)=>i+1),e.lessonCode+': ordered instructional steps');
  assert.ok(!/<(?:svg|path)\b/i.test(example));
  for(const x of (example+'\n'+steps).matchAll(/(\d+)\s*([+-])\s*(\d+)(?:\s*([+-])\s*(\d+))?\s*=\s*(\d+)(?!\d)/g)){
   const first=x[2]==='+'?Number(x[1])+Number(x[3]):Number(x[1])-Number(x[3]);
   const result=x[4]?(x[4]==='+'?first+Number(x[5]):first-Number(x[5])):first;
   assert.equal(result,Number(x[6]),e.lessonCode+': '+x[0]);
  }
 }
});
test('content matches six actual task types rather than teacher guidance fragments',()=>{
 assert.match(constants('SortMathToolsExamples'),/pattern blocks.*counters.*inch tiles/is);
 assert.match(constants('ClassPetSurveyStatementsExamples'),/true.*false|true statement[\s\S]*false statement/i);
 assert.match(constants('ChooseIndirectLengthComparatorExamples'),/string.*shelf|shelf.*string/i);
 assert.match(constants('ChooseIndirectLengthComparatorConcepts'),/Mai and Clare/i);
 assert.match(constants('ChooseIndirectLengthComparatorSteps'),/Step 6:/);
 assert.match(constants('ClockHourIntroductionExamples'),/5:00/);
 assert.match(constants('DrawFlatShapesOnDotsExamples'),/non-collinear dots/);
 assert.match(constants('ComposeFlatPatternShapesExamples'),/two identical square tiles/i);
});
