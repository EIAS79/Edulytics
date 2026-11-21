'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
const read=name=>JSON.parse(fs.readFileSync(path.join(root,base,name),'utf8'));
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json'),'utf8'));
const first=pack.Lessons[0],ar=first.Translations.find(x=>x.CultureCode==='ar');

test('Printed textbook explicitly has exactly two objectives, while Edulytics four competencies are labelled derived',()=>{
 const contract=read('u01l01-source-objectives-practice-contract.json');
 assert.equal(contract.sourceTextbookExplicitObjectiveCount,2);
 assert.equal(contract.sourceTextbookExplicitObjectives.length,2);
 assert.equal(contract.edulyticsExpandedPracticeCompetencyObjectiveCount,4);
 assert.deepEqual(
   contract.sourceTextbookExplicitObjectives.flatMap(x=>x.mappedDerivedOutcomes).sort(),
   ['O1','O2','O3','O4']);
 const text=ar.KeyConceptsAndRules;
 assert.ok(text.startsWith('هدفا الدرس المطبوعان في الكتاب:'));
 assert.ok(text.includes('كتابة التعابير اللفظية للتعابير الجبرية'));
 assert.ok(text.includes('كتابة التعابير الجبرية للتعابير اللفظية'));
 assert.ok(text.includes('كفايات Edulytics التدريبية المشتقة من هذين الهدفين'));
 assert.ok(text.includes('نمذجة موقف واقعي'));
 assert.ok(!text.startsWith('الأهداف في المصدر:'));
});

test('Correcting U01L01 provenance preserves all lesson identities and does not certify source D parity',()=>{
 const index=read('grade9-82-target-index.json');
 const ledger=read('grade9-stage-d-execution-ledger.json');
 assert.equal(pack.Status,'Published');
 assert.equal(read('grade9-release-validation.json').deployed,false);
 assert.equal(pack.Lessons.length,82);
 const codes=pack.Lessons.map(x=>x.LessonCode);
 assert.equal(new Set(codes).size,82);
 assert.equal(first.LessonCode,'PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS');
 assert.equal(first.SourceSha256,index.records[0].SourcePdfSha256);
 assert.ok(first.AdaptationStatus.includes('not Stage D certified'));
 assert.equal(ledger.firstIncompleteLessonOrdinal,1);
 assert.equal(ledger.sourceSemanticLock.rawLessonAtomicAuditPassed,0);
 assert.equal(ledger.sourceSemanticLock.targetLessonElementParityPassed,0);
 assert.ok(ledger.releaseDecision.startsWith('NO_GO_'));
 assert.ok(ar.WorkedExamples.includes('المساحة'));
 assert.ok(ar.KeyConceptsAndRules.includes('إذا جاءت عبارة'));
});
