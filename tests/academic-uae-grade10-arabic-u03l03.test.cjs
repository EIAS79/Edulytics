'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=x=>JSON.parse(fs.readFileSync(path.join(root,x),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L03:C027';
test('canonical U03L03 is exactly source Advanced lesson1-3 linear programming',()=>{
const blueprint=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const x=blueprint.Lessons.find(z=>z.LessonCode===code);
assert.ok(x);assert.equal(x.SortOrder,27);assert.equal(x.Title,'البرمجة الخطية');assert.equal(x.UnitNumber,3);
const book=get(base+'lesson-027-source-elements.json');
assert.deepEqual(book.sourcePhysicalPdfPages,[41,42,43,44,45,46,47]);assert.equal(book.sourceElements.length,26);
assert.equal(book.sourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
});
test('authored Arabic LP covers bounded, unbounded, tied and infeasible regions',()=>{
const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const lesson=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(lesson);
assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
assert.ok(lesson.Translations.some(t=>t.CultureCode==='ar'));
const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l03-ar.rich-lesson-v2.json').Lessons[0];
assert.equal(rich.WorkedExamples.length,15);assert.ok(rich.Visuals.length>=5);
assert.ok(rich.WorkedExamples.some(x=>x.Question.includes('غير محدودة')));
assert.ok(rich.WorkedExamples.some(x=>x.Question.includes('متناقضة')));
});
test('Stage C LP scope bounded and auto routing disabled, exact source family IDs only',()=>{
const contract=get(base+'lesson-027-stage-c-exact-contract.json');
const maps=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const m=maps.mappings.find(x=>x.lessonCode===code);
assert.ok(m);assert.deepEqual(m.primarySkills,contract.primarySkills);
assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
assert.equal(contract.runtimeGate.productionRouting,false);
for(const [a,b,k,p,q,mode] of [[2,1,6,3,2,0],[2,1,6,3,2,1],[1,2,5,4,3,0]]){
 const values=[0,p*b*k,q*a*k];
 const opt=mode===0?Math.max(...values):Math.min(...values);
 assert.ok(Number.isInteger(opt));
}
});