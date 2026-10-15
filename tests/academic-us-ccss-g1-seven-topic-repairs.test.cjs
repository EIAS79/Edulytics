'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {createHash}=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
const pack=JSON.parse(read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json'));
const record=JSON.parse(read('artifacts/academic-us-ccss/grade-reports/G01-seven-topic-mismatch-corrections.json'));
const source=read('src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs');
const selector=read('src/Edulytics.Data/Seeding/MathematicsCanonicalLessonContentSeeder.cs');
const materializer=read('src/Edulytics.Data/Seeding/CanonicalLessonContentMaterializer.cs');
const mapping={
 'G1:U04:L21':'CompareRepresentedNumbers',
 'G1:U06:L02':'CompareLengthsIndirectly',
 'G1:U06:L05':'MeasureConnectingCubeLength',
 'G1:U07:L02':'ComposeThreeDimensionalShapes',
 'G1:U07:L14':'ReadHalfPastClock',
 'G1:U07:L15':'WriteClockTimes',
 'G1:U07:L16':'ReadClockDailySchedule'
};
function constant(key) {
 const match=source.match(new RegExp('public const string '+key+' = ("(?:[^"\\\\]|\\\\.)*");'));
 assert.ok(match,'Missing Edulytics-authored constant '+key);
 return JSON.parse(match[1]);
}
function originalSha(en) {
 return createHash('sha256').update([
  en.cultureCode,en.title,en.explanation,en.keyConceptsAndRules,
  en.workedExamples,en.stepByStepSolutions,en.commonMistakes,en.quickSummary
 ].join('\n')+'\n').digest('hex');
}
test('the seven new Grade 1 repairs are exactly bounded and fully source/SHA guarded',()=>{
 assert.equal(record.lessons.length,7);
 assert.equal(new Set(record.lessons.map(x=>x.lessonCode)).size,7);
 assert.equal(record.academicSignoff,'NOT_GRANTED');
 assert.equal(record.releaseDecision,'NO-GO');
 assert.equal(record.sourcePackImmutable,true);
  assert.match(selector,/const int expectedSourceBoundCorrectionCount = 181;/);
 assert.match(materializer,/CommonCoreG1SourceGroundedInstructionalCorrections\s*\.Apply\(document\)/);
 assert.match(source,/Refusing G1 instruction correction of unexpected prose/);
 assert.match(source,/Refusing to overwrite unrecognized G1 persisted text/);
 assert.match(source,/G1 instruction source\/official-standard drift/);
 for(const r of record.lessons){
   const key=mapping[r.lessonCode.replace('PED:US-CCSS-MATH:','')];
   assert.ok(key,r.lessonCode);
   const lesson=pack.lessons.find(x=>x.lessonCode===r.lessonCode);
   assert.ok(lesson,r.lessonCode);
   const en=lesson.translations.find(x=>x.cultureCode==='en');
   assert.equal(r.title,en.title);
   assert.deepEqual(r.officialOutcomeCodes,lesson.outcomeCodes);
   assert.equal(r.publisherSourceUrl,lesson.sourceUrl);
   assert.equal(r.sourceSha256,lesson.sourceSha256);
   assert.equal(r.oldCanonicalBodySha256,lesson.canonicalBodySha256);
   assert.equal(r.oldFullEnglishBodySha256,originalSha(en));
   assert.equal(constant(key+'LessonCode'),r.lessonCode);
   assert.equal(constant(key+'Version'),r.newContentVersion);
   assert.ok(source.includes('['+key+'LessonCode] = new Repair('));
   for(const str of [lesson.sourceUrl,lesson.canonicalBodySha256,r.oldFullEnglishBodySha256,en.stepByStepSolutions]){
    assert.ok(source.includes(JSON.stringify(str)),r.lessonCode+': missing strict pin');
   }
   for(const c of lesson.outcomeCodes)assert.ok(source.includes(JSON.stringify(c)));
 }
});
test('new Grade 1 text is learner-facing with worked applications and mathematical consistency',()=>{
 let equations=0;
 for(const [short,key] of Object.entries(mapping)){
  const concepts=constant(key+'Concepts'),examples=constant(key+'Examples'),steps=constant(key+'Steps'),mistakes=constant(key+'Mistakes');
  assert.ok(concepts.length>100,short);
  assert.ok(examples.length>200,short);
  assert.ok(steps.length>180 && (steps.match(/Step \d:/g)||[]).length===5,short);
  assert.ok(mistakes.length>100,short);
  assert.ok(!/<(?:svg|path)\b/i.test(examples),short);
  assert.ok(!/Step 1: Represent the quantities or objects/i.test(steps),short);
  for(const m of (examples+'\n'+steps).matchAll(/(\d+)\s*([+-])\s*(\d+)\s*=\s*(\d+)(?!\d)/g)){
   const left=Number(m[1]),right=Number(m[3]),expected=Number(m[4]);
   assert.equal(m[2]==='+'?left+right:left-right,expected,short+': '+m[0]);equations++;
  }
 }
 assert.ok(equations>=7,'Unexpectedly few independent numeric checks: '+equations);
 const represented=constant('CompareRepresentedNumbersExamples');
 assert.match(represented,/42/); assert.match(represented,/>/); assert.match(represented,/</);
 assert.match(constant('CompareLengthsIndirectlyExamples'),/reference|same pencil|marked string/i);
 assert.match(constant('MeasureConnectingCubeLengthExamples'),/cube lengths|cubes/);
 assert.match(constant('ComposeThreeDimensionalShapesExamples'),/stack|join/i);
 assert.match(constant('ComposeThreeDimensionalShapesExamples'),/rectangular-prism shape/i);
 assert.match(constant('ComposeThreeDimensionalShapesExamples'),/sketch the two layers/i);
 for(const key of ['ReadHalfPastClock','WriteClockTimes','ReadClockDailySchedule']){
  assert.match(constant(key+'Examples'),/\d+:30/);
  assert.match(constant(key+'Examples'),/minute hand/i);
 }
});
