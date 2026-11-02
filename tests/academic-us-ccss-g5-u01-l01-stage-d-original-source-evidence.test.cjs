'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');

test('G5 ordinal 1 actual 2021 source geometry and missing publisher cool-down fail closed at D while 148 A–B–C handoffs are preserved',()=>{
 const root=path.resolve(__dirname,'..');
 const base='artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
 const evidence=JSON.parse(fs.readFileSync(path.join(root,base,'u01-l01-abc-element-ledger-20261030.json'),'utf8'));
 const handoff=JSON.parse(fs.readFileSync(path.join(root,base,'g5-abc-lesson-ledger-v1.json'),'utf8'));
 const source=evidence.stageDOriginalSourceAudit20261102;
 assert.equal(handoff.gradeTotals.READY_FOR_D,148);
 assert.equal(handoff.lessons.length,148);
 assert.equal(handoff.lessons[0].READY_FOR_STAGE_D,true);
 assert.equal(evidence.readyForStageD,true);
 assert.equal(source.academicAtoCHandoffPreserved,true);
 assert.equal(source.stageDVerified,false);
 assert.match(source.sourceSha256,/^[a-f0-9]{64}$/);
 assert.equal(source.sourceStudentFacingSections,3);
 assert.deepEqual(source.sourceVerifiedGeometry.map(x=>x.figure),
   [147995,147996,147997,147998,147635,147636]);
 assert.deepEqual(source.sourceVerifiedGeometry.map(x=>x.cubes),[8,8,8,10,8,8]);
 const a=evidence.lessonElements.filter(x=>x.Status==='MATCHED_TEACHER_LED').length;
 const b=evidence.lessonElements.filter(x=>x.Status==='MATCHED_DIGITAL').length;
 const missing=evidence.lessonElements.filter(x=>x.Status==='SOURCE_UNAVAILABLE');
 assert.equal(evidence.lessonElements.length,25);
 assert.equal(a,23);assert.equal(b,1);
 assert.deepEqual(missing.map(x=>x.SourceElementId),
   ['G5-U01-L01-CD-SOURCE-Q01']);
 assert.equal(a+b,24);
 assert.equal(source.matchedOriginalSourceElements,24);
 assert.equal(source.lessonElementDenominator,25);
 assert.equal(source.sourceCoverage100Percent,false);
 assert.equal(source.firstPartyReplacementIsNotPublisherSourceMatch,true);
 assert.equal(evidence.sourceCoverage100Percent,false);
 assert.equal(evidence.academicAccepted,false);
 assert.deepEqual(evidence.sourceElementStatusCounts,
   {MATCHED_TEACHER_LED:23,MATCHED_DIGITAL:1,SOURCE_UNAVAILABLE:1});
 const original=missing[0];
 const alternative=evidence.lessonElements.find(x=>
   x.SourceElementId===original.OwnerAuthorizedFirstPartyAlternative);
 assert.ok(alternative);
 assert.equal(alternative.Status,'MATCHED_TEACHER_LED');
 assert.equal(original.AcademicSourceParityCertified,false);
});
