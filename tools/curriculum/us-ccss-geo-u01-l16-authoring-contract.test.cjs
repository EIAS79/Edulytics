const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U01:L16";
const checkpoint=read("artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-u01-l16-abc-checkpoint-20261030.json");
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/GEO_ABC_INDEPENDENT_COURSE_LEDGER_20261030.json');
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json');
const richDir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const rich=fs.readdirSync(richDir).filter(x=>x.startsWith('us-ccss-hs-geo-u01-')&&x.endsWith('.json')).flatMap(x=>read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/'+x).Lessons);
const entry=ledger.LessonEntries.find(x=>x.LessonCode===code);
const b=blueprint.Lessons.find(x=>x.LessonCode===code);
const lesson=rich.find(x=>x.LessonCode===code);
test('U01 source identity/order and checkpoint belong to the same lesson',()=>{
 assert.ok(entry);assert.ok(b);assert.ok(lesson);
 assert.equal(checkpoint.LessonCode,code);
 assert.equal(entry.Ordinal,16);
 assert.equal(b.SortOrder,16);
 assert.equal(b.UnitNumber,1);
 assert.equal(b.LessonNumber,16);
 assert.equal(entry.SourceCheckpoint,"artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-u01-l16-abc-checkpoint-20261030.json");
});
test('RichV2 retains lesson-specific teaching structures, not an empty placeholder',()=>{
 assert.equal(lesson.Title,b.Title);
 assert.ok(Array.isArray(lesson.ExplanationParagraphs)&&lesson.ExplanationParagraphs.length>1);
 assert.ok(lesson.ExplanationParagraphs.some(x=>typeof x==='string'&&x.trim().length>20));
 assert.ok(Array.isArray(lesson.WorkedExamples)&&lesson.WorkedExamples.length>=1);
 assert.ok(Array.isArray(lesson.KeyConcepts)&&lesson.KeyConcepts.length>=1);
});
test('A-B-C authoring checkpoint does not claim Stage D success',()=>{
 assert.equal(entry.ABCReadyForD,true);
 assert.match(entry.A,/^A_CLOSED/);
 assert.match(entry.B,/^B_CLOSED/);
 assert.match(entry.C,/^C_CLOSED/);
 assert.equal(entry.D,'NOT_RUN');
 assert.equal(checkpoint.ABCReadyForD,true);
});
