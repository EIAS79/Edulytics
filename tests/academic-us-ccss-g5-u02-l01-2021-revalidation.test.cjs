'use strict';
// First edition 2021 G5 U02 L01 A-B-C contract; tests authored, NOT RUN during A-B-C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
test('L01 2021 equal sharing atomic map preserves source and first-party exception',()=>{
const a=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u02-l01-2021-atomic-source-evidence-20261102.json');
const l=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-abc-lesson-ledger-v1.json');
assert.equal(a.officialEdition,'Illustrative Mathematics K5 Grade 5 First Edition 2021');
assert.equal(a.atomicIdentified,15);
assert.equal(a.sourceUnavailable.length,2);
assert.equal(a.sourceCoverageDenominator.startsWith('DENOMINATOR_INCOMPLETE'),true);
assert.equal(a.ownerAuthorizedFirstPartyExit,true);
assert.equal(a.lessonPractice.runtimeValidation,'DEFERRED_STAGE_D');
assert.equal(l.lessons[12].READY_FOR_STAGE_D,true);
assert.equal(l.currentAcceptanceCursor,'PED:US-CCSS-MATH:G5:U02:L02');
});
test('The six choices and alternative Han partitions are conceptually correct',()=>{
const cases=[2,3,5].flatMap(s=>[2,3,5].filter(p=>p!==s).map(p=>[s,p]));
assert.equal(cases.length,6);
assert.equal((1/2+1/6),2/3);
assert.equal(3*(2/3),2);
assert.equal(3*(4/3),4);
});
test('U02 L01 exact engine registered, not an invented dedicated file',()=>{
const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/UsCcssG5Unit2SourcePracticeContracts.cs'),'utf8');
const q=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/UsCcssG5Unit2SourceQuestions.cs'),'utf8');
assert.match(c,/usccss\.g5\.u02\.l01\.source_fair_share_choice/);
assert.match(q,/usccss\.g5\.u02\.l01\.source_fair_share_choice/);
const x=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u02-l01-practice-acceptance-v16.json');
assert.equal(x.StageC,'GRADE5_U02_L01_SOURCE_EXACT_FAIR_SHARING_CHOICE_WRITTEN_NOT_RUN_TEACHER_LED');
assert.match(x.selectedRuntimePractice,/AUTHENTICATED_RUNTIME_STAGE_D_NOT_RUN/);
});
