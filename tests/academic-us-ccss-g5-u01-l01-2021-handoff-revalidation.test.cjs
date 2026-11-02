'use strict';
// Grade5 First Edition 2021 A-B-C guarded exception; authored only, NOT RUN under A-B-C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pathG='artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
test('L01 author-approved first-party cooldown closes A-C, without inventing original publisher exact prompt',()=>{
 const e=read(pathG+'u01-l01-abc-element-ledger-20261030.json');
 const grade=read(pathG+'g5-abc-lesson-ledger-v1.json');
 const c=e.lessonElements.find(x=>x.SourceElementId==='G5-U01-L01-CD-SOURCE-Q01');
 assert.ok(c);assert.equal(c.Status,'SOURCE_UNAVAILABLE');
 assert.equal(c.OriginalPublisherContentStatus,'SOURCE_UNAVAILABLE_GATED');
 assert.equal(c.OriginalPublisherExactTextMatched,false);
 assert.equal(c.OriginalSourceParityClaimed,false);
 assert.equal(c.OwnerAuthorizedFirstPartyAlternative,'G5-U01-L01-CD-AUTHOR-Q01');
 assert.equal(e.sourceCoverage100Percent,false);
 assert.equal(e.sourceFunctionallyEquivalentWithAuthorizedExceptions,true);
 assert.equal(e.sourceExceptionDecision20261102.ownerAuthorized,true);
 assert.equal(e.sourceExceptionDecision20261102.stageDTests,'NOT_RUN');
 assert.equal(e.status,'READY_FOR_STAGE_D');assert.equal(e.readyForStageD,true);
 assert.equal(grade.lessons[0].READY_FOR_STAGE_D,true);
 assert.equal(grade.gradeTotals.READY_FOR_D,148);
 assert.equal(grade.currentAcceptanceCursor,'PED:US-CCSS-MATH:G5:U01:L02');
});
test('L01 2021 teacher HTML is original source; v.360 not counted as student evidence',()=>{
 const e=read(pathG+'u01-l01-abc-element-ledger-20261030.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json').Lessons[0];
 assert.equal(e.sourceReference.studentHtml,e.sourceReference.teacherHtml);
 assert.match(e.sourceReference.studentFacingEvidence,/2021 Kendall Hunt teacher lesson/i);
 assert.match(e.sourceReference.excludedWrongEditionMirror,/Illustrative_Mathematics_v360/);
 assert.doesNotMatch(rich.ExplanationParagraphs[1],/including the local AccessIM student mirror/i);
 assert.match(rich.ExplanationParagraphs.join(' '),/Edulytiks-original five-minute exit check/i);
});
