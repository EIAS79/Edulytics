'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const obj=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/general-2025-volume1-48-verified-book-toc.json'),'utf8'));
const main=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-193-main-toc-raw-entry-ledger.json'),'utf8'));
test('First General volume has exactly 48 independently named source lessons and seven units',()=>{
 assert.equal(obj.sourceLessonCount,48);assert.equal(obj.unitCount,7);
 assert.deepEqual([...new Set(obj.lessons.map(x=>x.unit))],[1,2,3,4,5,6,7]);
 assert.equal(new Set(obj.lessons.map(x=>x.sourceEntryId)).size,48);
 assert.ok(obj.lessons.every(x=>x.sourceSha256===obj.pdfSha256));
 assert.ok(obj.lessons.every(x=>x.printedOpeningPageVerified>0));
 assert.ok(obj.lessons.every(x=>x.sourceTocPdfPage>=5&&x.sourceTocPdfPage<=11));
 assert.equal(obj.canonicalUnifiedLessonCount,null,'Volume1 cannot certify annual curriculum');
 assert.equal(obj.sourceLock,false);
});
test('Correct printed index typo 587 for final lesson to actual verified body p487',()=>{
 const last=obj.lessons.at(-1);
 assert.equal(last.lessonNumber,'7-6');assert.equal(last.titleArabicSourceToc,'شبه المنحرف وشكل الطائرة الورقية');
 assert.equal(last.printedPageInOriginalToc,587);
 assert.equal(last.printedOpeningPageVerified,487);
 assert.equal(last.sourceBodyPdfOpeningPageVerified,500);
 const row=main.rows.find(x=>x.sourceEntryId===last.sourceEntryId);
 assert.equal(row.sourcePrintedOpeningPageVerified,487);
 assert.equal(row.sourceTocOriginalPrintedPage,587);
 assert.equal(obj.testsRun,'WRITTEN_NOT_RUN');
});
