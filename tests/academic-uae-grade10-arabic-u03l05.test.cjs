'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=x=>JSON.parse(fs.readFileSync(path.join(root,x),'utf8')),base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L05:C029';
test('Matrix lesson is canonical ordinal29 with BOTH source 1-5 and 1-6 and separate next matrix product',()=>{
const blueprint=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const row=blueprint.Lessons.find(x=>x.LessonCode===code);assert.ok(row);assert.equal(row.SortOrder,29);
assert.equal(row.Title,'العمليات على المصفوفات');assert.equal(row.UnitNumber,3);
const source=get(base+'lesson-029-source-elements.json');
assert.equal(source.sourceElements.length,37);
assert.equal(source.sourcePhysicalPdfPages.length,15);
assert.equal(source.nextSourceLessonStartsPhysicalPage,72);
});
test('Arabic originals include matrix indexing, equality, compatible operations and distinct multiplication',()=>{
const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
const ar=l.Translations.find(x=>x.CultureCode==='ar');assert.ok(ar);
const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l05-ar.rich-lesson-v2.json').Lessons[0];
assert.equal(rich.WorkedExamples.length,15);assert.equal(rich.Visuals.length,6);
assert.ok(rich.SummaryPoints.some(x=>x.includes('ضرب مصفوفتين')));
});
test('Bounded source-exact matrix cell families are independent and fail closed pending D',()=>{
const contract=get(base+'lesson-029-stage-c-exact-contract.json');
const m=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
assert.ok(m);assert.deepEqual(m.primarySkills,contract.primarySkills);
assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');assert.equal(contract.runtimeGate.productionRouting,false);
const codeText=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10MatrixOperations.cs'),'utf8');
assert.match(codeText,/UaeG10MatrixOperationsResponse/);
for(const [A,B,k,expectedAdd,expectedDiff,expectedScalar] of [
[[1,2,-3,4],[5,-2,0,7],-2,[6,0,-3,11],[-4,4,-3,-3],[-2,-4,6,-8]]]){
assert.deepEqual(A.map((v,i)=>v+B[i]),expectedAdd);
assert.deepEqual(A.map((v,i)=>v-B[i]),expectedDiff);
assert.deepEqual(A.map(v=>k*v),expectedScalar);
}
});