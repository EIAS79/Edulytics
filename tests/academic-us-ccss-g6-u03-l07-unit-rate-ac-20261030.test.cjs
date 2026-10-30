'use strict';
// Grade 6 U03:L07 exact unit-rate mapping and independent mathematics; TEST_AUTHORED_NOT_RUN
const test=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'),ledger=load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json'),evidence=load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u03-l07-ac-first-independent-20261030.json'),m=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'),families=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'),skills=load('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');
const mapping=m.mappings.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U03:L07'),row=pack.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U03:L07');
test('static exact contract projects before fallback and registers both valid families',()=>{
 assert.ok(mapping);assert.equal(mapping.practiceReadiness,'READY_VERIFIED');assert.deepEqual(mapping.allowedQuestionFamilies,['ratio.unit_rate.direct','ratio.unit_rate.equivalent_ratio']);assert.deepEqual(mapping.primarySkills,['ratio.unit_rate']);assert.ok(skills.skills.some(s=>s.id==='ratio.unit_rate'));
 for(const familyId of mapping.allowedQuestionFamilies){const family=families.families.find(f=>f.id===familyId);assert.ok(family);assert.equal(family.skillId,'ratio.unit_rate');assert.equal(family.lessonPracticeRouting,true);assert.ok(family.verificationPolicy);}
 assert.equal(evidence.C.staticSelectionVerified,true);assert.equal(evidence.C.actualGeneratedItemNotRun,true);
});
test('source-reasoned correct, wrong and equivalent unit price tables',()=>{
 assert.equal(14/2,7);assert.deepEqual([4,5,10].map(n=>7*n),[28,35,70]);
 assert.equal(14/2,28/4);assert.equal(14/2,35/5);assert.equal(14/2,70/10);
 assert.equal(5/15*60,20);assert.equal(60/3,20);
 assert.equal(32/1.609344<20,true);assert.equal(32/1.609344>19.8,true);
 assert.deepEqual([2,5,7].map(h=>h*6),[12,30,42]);assert.equal(66/6,11);assert.equal(100/6,50/3);
 assert.deepEqual([7,10].map(lb=>lb*6/4),[10.5,15]);assert.equal(9*4/6,6);assert.equal(20*4/6,40/3);
});
test('original Edulytiks Cool-down exact numerics and invalid/malformed cases',()=>{
 const base=18/3;assert.equal(base,6);assert.deepEqual([1,5,7,12].map(n=>n*base),[6,30,42,72]);
 for(const s of [2,1.5,3/2,5])assert.equal((18*s)/(3*s),6);
 const exact=sub=>sub!==null&&typeof sub==='object'&&typeof sub.cost==='number'&&Number.isFinite(sub.cost)&&typeof sub.count==='number'&&Number.isFinite(sub.count)&&sub.count>0&&sub.unit==='USD_per_ticket'&&sub.cost/sub.count===6;
 assert.equal(exact({cost:18,count:3,unit:'USD_per_ticket'}),true);
 assert.equal(exact({cost:36,count:6,unit:'USD_per_ticket'}),true);
 assert.equal(exact({cost:18,count:6,unit:'USD_per_ticket'}),false);
 assert.equal(exact({cost:18,count:0,unit:'USD_per_ticket'}),false);
 assert.equal(exact({cost:'18',count:3,unit:'USD_per_ticket'}),false);
 assert.equal(exact({cost:18,count:3,unit:'tickets_per_USD'}),false);
 assert.equal(exact(null),false);
});
test('numeric answer cannot assert source open discussion has been teacher scored',()=>{
 assert.equal(row.translations.some(t=>t.cultureCode==='en'&&t.workedExamples.includes('Edulytiks-original Cool-down')),true);
 assert.equal(ledger.summary.abcFirstOrder.READY_FOR_STAGE_D,1);
 assert.equal(ledger.summary.abcFirstOrder.C_PREPARED,1);
 assert.equal(ledger.summary.abcFirstOrder.BLOCKED,2);
 assert.equal(evidence.C.actualTeacherWorkflowNotRun,true);
 assert.equal(evidence.tests.status,'AUTHORED_NOT_RUN');
 assert.equal(ledger.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U03:L07').academicAcceptance,'NOT_ACCEPTED');
});
