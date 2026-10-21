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
const get=k=>readSourceConstant(source,k);
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
  const versionV1=source.includes('public const string '+p.key+'VersionV1 = ');
  assert.equal(get(p.key+(versionV1?'VersionV1':'Version')),r.contentVersion);
  if(versionV1){
   assert.match(get(p.key+'Version'),/-v2$/);
   assert.notEqual(get(p.key+'Version'),r.contentVersion);
  }
  assert.ok(source.includes('['+p.key+'LessonCode] = new Repair('));
  for(const v of [l.sourceUrl,l.canonicalBodySha256,r.oldEnglishSha256,e.stepByStepSolutions]){
   assert.ok(source.includes(JSON.stringify(v)),p.short+': SHA/URL/old-steps pin');
  }
  const examples=get(p.key+'Examples'),steps=get(p.key+'Steps'),concepts=get(p.key+'Concepts'),mistakes=get(p.key+'Mistakes');
  // Preserve every original V1 teaching field byte-for-byte in its pinned
  // constant, even when a later V2 adds source-corroborated learner tasks.
  for(const [suffix,historical] of Object.entries({
   Concepts:p.concepts,Examples:p.examples,Steps:p.steps,Mistakes:p.mistakes
  })){
   const oldKey=p.key+suffix+'V1';
   if(source.includes('public const string '+oldKey+' = ')){
    assert.equal(get(oldKey),historical,p.short+': V1 '+suffix+' changed');
    assert.ok(get(p.key+suffix).startsWith(historical),
      p.short+': V2 must preserve its historical '+suffix+' prefix');
   }else if(p.key!=='RectanglesSquaresAttributes'){
    assert.equal(get(p.key+suffix),historical,p.short+': '+suffix);
   }
  }
  if(p.key==='RectanglesSquaresAttributes'){
    // The newer original-pupil task was substantively corrected, not just
    // appended. Keep the original pack SHA pins and enforce all four drawing
    // sets rather than misleadingly comparing against an obsolete proposal.
    assert.match(concepts,/DRAW FIVE rectangles and THREE shapes that are NOT rectangles/);
    assert.match(concepts,/DRAW FIVE squares and THREE shapes that are NOT squares/);
    assert.match(concepts,/TWO distinct sets of eight/);
    assert.match(concepts,/teacher must review authentic drawings/i);
    assert.match(examples,/FIVE rectangles, THREE non-rectangles, FIVE squares and THREE non-squares/);
    assert.match(steps,/Step 2: Draw FIVE different valid rectangles/);
    assert.match(steps,/Step 5: Separately draw FIVE squares and THREE figures that are NOT squares/);
    assert.match(mistakes,/Do not omit the SECOND source task/);
  }
  assert.equal((examples.match(/Example \d:/g)||[]).length,4,p.short);
  const stepNumbers=[...steps.matchAll(/Step (\d+):/g)].map(x=>Number(x[1]));
  assert.ok(stepNumbers.length>=5,p.short);
  assert.deepEqual(stepNumbers,stepNumbers.map((_,i)=>i+1),
    p.short+': complete ordered task steps');
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