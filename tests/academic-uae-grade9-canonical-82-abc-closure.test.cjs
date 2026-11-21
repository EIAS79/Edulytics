'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
test('Grade9 General canonical pack and mathematical skill mapping contain 82 unique unit-lesson pairs',()=>{
const idx=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-82-target-index.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const rows=mappings.mappings.filter(x=>x.lessonCode.startsWith('PED:UAE-MOE-MATH:L9:GENERAL:'));
const position=code=>{const m=code.match(/:GENERAL:(\d\d):(\d\d):/);assert.ok(m,code);return m[1]+':'+m[2];};
assert.equal(idx.records.length,82);assert.equal(new Set(idx.records.map(x=>x.UnitOrdinal)).size,13);
assert.equal(pack.AcademicLanguage,'ar');assert.equal(pack.Lessons.length,82);assert.equal(rows.length,82);
assert.equal(new Set(pack.Lessons.map(x=>position(x.LessonCode))).size,82);
assert.equal(new Set(rows.map(x=>position(x.lessonCode))).size,82);
const byPosition=new Map(pack.Lessons.map(x=>[position(x.LessonCode),x]));
for(const source of idx.records){const code=String(source.UnitOrdinal).padStart(2,'0')+':'+String(source.LessonOrdinal).padStart(2,'0');const lesson=byPosition.get(code);assert.ok(lesson,'missing lesson '+code);assert.ok(lesson.Translations.some(t=>t.CultureCode==='ar'));}
});
test('Every numeric practice mapping has a contract; two open geometry proof lessons are teacher-reviewed',()=>{
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const rows=mappings.mappings.filter(x=>x.lessonCode.startsWith('PED:UAE-MOE-MATH:L9:GENERAL:'));
const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
const teacher=rows.filter(x=>x.allowedQuestionFamilies.length===0);
assert.deepEqual(teacher.map(x=>x.lessonCode).sort(),[
'PED:UAE-MOE-MATH:L9:GENERAL:13:07:CONGRUENCE-TRANSFORMATIONS',
'PED:UAE-MOE-MATH:L9:GENERAL:13:08:TRIANGLES-AND-COORDINATE-PROOF'
].sort());
for(const x of rows.filter(x=>x.allowedQuestionFamilies.length))assert.ok(c.includes('byLesson["'+x.lessonCode+'"]'),x.lessonCode);
});
test('82 sequential A-B-C authoring closure records are prepared while Stage D remains unrun',()=>{
const early=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/first-12-stage-a-c-closure.json');
assert.equal(early.items.length,12);assert.deepEqual(early.items.map(x=>x.ordinal),Array.from({length:12},(_,i)=>i+1));
for(const x of early.items){assert.equal(x.closed,true);assert.equal(x.closeDecision,'READY_FOR_STAGE_D');assert.equal(x.testsRun,'WRITTEN_NOT_RUN');}
for(let n=13;n<=82;n++){const v=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-'+n+'-stage-a-c-checkpoint.json');assert.equal(v.closed,true,'lesson '+n);assert.equal(v.closeDecision,'READY_FOR_STAGE_D','lesson '+n);}
const progress=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/full-grade-progress.json');
assert.notEqual(progress.stageD,'PASSED');assert.notEqual(progress.productionPublish,'PUBLISHED');
});

test('Grade 9 full-year composite pack must disclose mixed source editions and distinguish owner approval from D certification',()=>{
const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
const idx=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-82-target-index.json');
assert.equal(p.Lessons.length,82);
assert.equal(p.AcademicLanguage,'ar');
assert.equal(p.TargetCurriculumPeriod,'2026-2027');
assert.equal(p.SourceCurriculumPeriod,'2025-2026');
assert.match(p.SourceVersionLabel,/2019-2020/);
assert.match(p.SourceVersionLabel,/02-1\.pdf/);
assert.match(p.SourceVersionLabel,/03-1\.pdf/);
assert.match(p.ReviewedBy,/program owner/);
assert.match(p.ReviewEvidence,/SOURCE_INDEX_COMPLETE=false/);
assert.equal(p.Status,'Draft','Never claim Published before real final verification');
assert.deepEqual([...new Set(idx.records.map(x=>x.SourceEdition))].sort(),['2019-2020','2025-2026']);
});
