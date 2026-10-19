'use strict';
const {test}=require('node:test'), assert=require('node:assert/strict'), fs=require('node:fs'), path=require('node:path'), {createHash}=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
const pack=JSON.parse(read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json'));
const proofs=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-six-source-goal-content-repairs.json'));
const proposals=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-six-source-goal-content-proposals.json'));
const archive=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-146-local-teacher-source-evidence.json'));
const source=read('src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs');
const selector=read('src/Edulytics.Data/Seeding/MathematicsCanonicalLessonContentSeeder.cs');
const get=k=>{const m=source.match(new RegExp('public const string '+k+' = ("(?:[^"\\\\]|\\\\.)*");'));assert.ok(m,k);return JSON.parse(m[1]);};
const digest=e=>createHash('sha256').update([e.cultureCode,e.title,e.explanation,e.keyConceptsAndRules,e.workedExamples,e.stepByStepSolutions,e.commonMistakes,e.quickSummary].join('\n')+'\n').digest('hex');
test('six source-verified G1 repairs preserve all pinned before-state guarantees',()=>{
 assert.equal(proofs.lessons.length,6);
 assert.equal(new Set(proofs.lessons.map(x=>x.lessonCode)).size,6);
 assert.equal(proofs.academicApproval,'PENDING');
 assert.equal(proofs.originalPacksUnchanged,true);
 // Preserve the exact original V1 count PREFIX and independently pinned
 // source and learner-content SHA checks below. Later full-grade corrections
 // legitimately add extra source-bound lessons without rewriting old six.
 const sourceBound=selector.match(/const int expectedSourceBoundCorrectionCount = ([^;]+);/);
 assert.ok(sourceBound,'Missing source-bound correction count assertion');
 const terms=sourceBound[1].split(/\s*\+\s*/).map(x=>Number(x));
 assert.ok(terms.length>=9 && terms.every(x=>Number.isSafeInteger(x)&&x>0),
   'Source-bound correction count must stay a sum of positive integers');
 assert.deepEqual(terms.slice(0,9),[12,19,3,7,6,6,1,2,2],
   'The original V1 source-bound correction cohort must remain unchanged');
 assert.ok(selector.includes('codes.Distinct(StringComparer.Ordinal).Count() != expectedSourceBoundCorrectionCount'),
   'Source-bound correction count must still fail on duplicate lesson codes');
 assert.ok(selector.includes(
  'CommonCoreT001MixedAddSubtractContentCorrection.IsTarget(document, lesson) ||'));
 assert.ok(selector.includes(
  'CommonCoreT001FindThePairContentCorrection.IsTarget(document, lesson)'));

 assert.ok(selector.includes('CommonCoreG1CountAndAddPilotCorrection.IsTarget(document, lesson) ||'),
  'Only the separately documented G1 U01 L01 pilot may extend the prior source-bound set');
 for(const p of proposals.lessons){
  const l=pack.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:'+p.short);
  const r=proofs.lessons.find(x=>x.lessonCode===l?.lessonCode);
  const original=archive.records.find(x=>x.lessonCode===l?.lessonCode);
  assert.ok(l&&r&&original,p.short);
  const e=l.translations.find(x=>x.cultureCode==='en');
  assert.equal(digest(e),r.oldEnglishSha256,p.short);
  assert.equal(l.canonicalBodySha256,r.oldCanonicalSha256,p.short);
  assert.equal(original.teacherPreparationSha256,r.localTeacherPreparationSha256);
  assert.deepEqual(r.officialOutcomeCodes,l.outcomeCodes);
  assert.equal(get(p.key+'LessonCode'),l.lessonCode);
  assert.equal(get(p.key+'Version'),r.contentVersion);
  assert.ok(source.includes('['+p.key+'LessonCode] = new Repair('));
  for(const v of [l.sourceUrl,l.canonicalBodySha256,r.oldEnglishSha256,e.stepByStepSolutions]){
   assert.ok(source.includes(JSON.stringify(v)),p.short+': SHA/URL/old-steps pin');
  }
  const examples=get(p.key+'Examples'),steps=get(p.key+'Steps'),concepts=get(p.key+'Concepts'),mistakes=get(p.key+'Mistakes');
  assert.equal(examples,p.examples);
  assert.equal(steps,p.steps);
  assert.equal(concepts,p.concepts);
  assert.equal(mistakes,p.mistakes);
  assert.equal((examples.match(/Example \d:/g)||[]).length,4,p.short);
  assert.equal((steps.match(/Step \d:/g)||[]).length,5,p.short);
  assert.ok(!/<(?:path|svg)\b/i.test(examples));
  for(const z of (examples+'\n'+steps).matchAll(/(\d+)\s*([+-])\s*(\d+)(?:\s*([+-])\s*(\d+))?\s*=\s*(\d+)(?!\d)/g)){
   let a=z[2]==='+'?Number(z[1])+Number(z[3]):Number(z[1])-Number(z[3]);
   if(z[4]) a=z[4]==='+'?a+Number(z[5]):a-Number(z[5]);
   assert.equal(a,Number(z[6]),p.short+': '+z[0]);
  }
 }
});
test('each repair teaches the publisher goal rather than only generic steps',()=>{
 assert.match(get('ShapeCardCountsExamples'),/categor|Triangle|rectangle/i);
 assert.match(get('ClassFavoriteSurveyExamples'),/picture graph|tally/i);
 assert.match(get('AroundRoomStoryProblemsExamples'),/4 \+ 3 \+ 5 = 12/);
 assert.match(get('MeasuredShoesStoriesExamples'),/cube lengths/i);
 assert.match(get('RectanglesSquaresAttributesExamples'),/it is also a rectangle/i);
 assert.match(get('CompositeShapesAndFactsCenterExamples'),/larger flat rectangle|combined solid/i);
});