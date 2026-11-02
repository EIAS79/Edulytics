'use strict';
// First Edition 2021 A-B-C guard; authored only, deliberately NOT RUN in A-B-C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const grade='artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
test('Grade 5 L01 cannot be READY when the original 2021 publisher cooldown is unverified',()=>{
 const evidence=read(grade+'u01-l01-abc-element-ledger-20261030.json');
 const ledger=read(grade+'g5-abc-lesson-ledger-v1.json');
 const original=evidence.lessonElements.find(x=>x.SourceElementId==='G5-U01-L01-CD-SOURCE-Q01');
 assert.ok(original);assert.equal(original.Status,'SOURCE_UNAVAILABLE');
 assert.equal(evidence.sourceCoverage100Percent,false);
 assert.equal(evidence.readyForStageD,false);
 assert.equal(ledger.lessons[0].READY_FOR_STAGE_D,false);
 assert.equal(ledger.lessons[0].A_STATUS,'A_C_EXPLICIT_GAP');
 assert.equal(ledger.gradeTotals.READY_FOR_D,147);
});
test('Original 2021 Student Facing evidence is not a v.360 AccessIM mirror',()=>{
 const evidence=read(grade+'u01-l01-abc-element-ledger-20261030.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json').Lessons[0];
 assert.equal(evidence.sourceReference.studentHtml,evidence.sourceReference.teacherHtml);
 assert.match(evidence.sourceReference.studentFacingEvidence,/2021 Kendall Hunt teacher lesson/i);
 assert.doesNotMatch(rich.ExplanationParagraphs[1],/including the local AccessIM student mirror/i);
 assert.match(rich.ExplanationParagraphs[1],/embedded Student Facing/i);
 assert.match(rich.ExplanationParagraphs.join(' '),/Edulytiks-original five-minute exit check/i);
});
