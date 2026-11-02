'use strict';
// Grade 5 U01:L02 2021 source parity guard; authored NOT RUN during A-B-C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),a='artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
const read=p=>JSON.parse(fs.readFileSync(path.resolve(root,p),'utf8'));
test('L02 first-party cooldown replaces gated source item without falsely claiming publisher parity',()=>{
const x=read(a+'u01-l02-abc-element-ledger-20261030.json'),grade=read(a+'g5-abc-lesson-ledger-v1.json');
const cooldown=x.lessonElements.find(e=>e.SourceElementId==='G5-U01-L02-COOLDOWN');
assert.equal(cooldown.Status,'SOURCE_UNAVAILABLE');
assert.equal(cooldown.OwnerAuthorizedReplacement,'G5-U01-L02-EXIT-CHECK');
assert.equal(cooldown.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
assert.equal(x.sourceCoverage100Percent,false);
assert.equal(x.readyForStageD,true);
assert.equal(grade.lessons[1].READY_FOR_STAGE_D,true);
assert.equal(grade.currentAcceptanceCursor,'PED:US-CCSS-MATH:G5:U01:L03');
});
test('L02 selected source-specific digital family is not coded as blocked',()=>{
const x=read(a+'u01-l02-abc-element-ledger-20261030.json');
const item=x.lessonElements.find(e=>e.SourceElementId==='G5-U01-L02-PRACTICE');
assert.equal(item.ImplementationType,'DIGITAL');
assert.equal(item.SourceSpecificFamily,'usccss.g5.u01.l02.count_irregular_connected_cubes');
assert.equal(item.RuntimeValidation,'DEFERRED_STAGE_D');
assert.equal(x.acRevalidation20261102.noNewEngine,true);
});
