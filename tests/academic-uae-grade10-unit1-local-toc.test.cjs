'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const manifest=load('docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json');
const inventory=load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-inventory.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
test('The first nine original General source headings are the same lessons in the approved 16-lesson unified Unit1',()=>{
 const expected=['جمع كثيرات الحدود وطرحها','ضرب كثيرة حدود في أحادية حد','ضرب كثيرات الحدود','نواتج الضرب الخاصة','استخدام خاصية التوزيع','حل x² + bx + c = 0','حل ax² + bx + c = 0','الفرق بين مربعين','المربعات الكاملة'];
 assert.equal(manifest.Units[0].ls.length,16);
 assert.deepEqual(manifest.Units[0].ls.slice(0,9),expected);
 assert.equal(pack.Lessons.length,148);
 assert.equal(new Set(pack.Lessons.map(x=>x.LessonCode)).size,148);
 assert.ok(pack.Lessons.some(x=>x.LessonCode.startsWith('PED:UAE-MOE-MATH:L10:GENERAL:01:01:')));
});
test('Seven source files are still audited separately from the owner-approved 148-lesson index',()=>{
 assert.equal(inventory.fileCount,7);
 assert.equal(inventory.allSevenFileHashesMatched,true);
 assert.equal(manifest.LessonCount,148);
 assert.equal(manifest.CanonicalUnifiedIndexLocked,true);
 assert.equal(manifest.FullLessonLevelSemanticAuditComplete,false);
 assert.equal(inventory.sourceLock,false,'source-body coverage still requires academic verification');
});
