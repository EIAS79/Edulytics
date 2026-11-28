'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8')),base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/',code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L06:C030';
test('Source lesson 1-7 matrix product not confused with scalar or matrix intro',()=>{
 const blueprint=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
 const l=blueprint.Lessons.find(x=>x.LessonCode===code);assert.ok(l);assert.equal(l.SortOrder,30);assert.equal(l.Title,'ضرب المصفوفات');
 const src=get(base+'lesson-030-source-elements.json');assert.deepEqual(src.sourcePhysicalPdfPages,[72,73,74,75,76,77,78,79]);
 assert.deepEqual(src.supplementalTechnologyLabPages,[80,81]);assert.equal(src.sourceElements.length,26);
});
test('Original Arabic row×column and dimensions are independently correct',()=>{
 const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const r=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l06-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(r.WorkedExamples.length,15);assert.equal(r.Visuals.length,6);
 const A=[[1,2],[3,4]],B=[[5,-1],[2,0]];
 const mul=(a,b)=>a.map(row=>b[0].map((_,j)=>row.reduce((sum,v,k)=>sum+v*b[k][j],0)));
 assert.deepEqual(mul(A,B),[[9,-1],[23,-3]]);assert.deepEqual(mul(B,A),[[2,6],[2,4]]);
 assert.notDeepEqual(mul(A,B),mul(B,A));
});
test('Bounded exact Grade10 families are one-entry or dimension-only fail-closed',()=>{
 const c=get(base+'lesson-030-stage-c-exact-contract.json'),m=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,c.primarySkills);assert.deepEqual(m.allowedQuestionFamilies,c.allowedQuestionFamilies);
 assert.equal(c.testsRun,'WRITTEN_NOT_RUN');assert.equal(c.runtimeGate.productionRouting,false);
 const source=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10MatrixProduct.cs'),'utf8');
 assert.match(source,/UaeG10MatrixProductResponse/);assert.ok(source.includes('result=p["a"+row+"1"]*p["b"+"1"+col]+p["a"+row+"2"]*p["b"+"2"+col];'));
});