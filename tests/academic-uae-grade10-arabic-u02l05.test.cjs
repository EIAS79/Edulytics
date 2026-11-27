'use strict';
// UAE Grade 10 Stage A-B-C source-skill guards; execute ONLY in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:02:05:SIMPLIFYING-RADICAL-EXPRESSIONS';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l05-ar.rich-lesson-v2.json');
const source=read(base+'lesson-021-source-elements.json');
const contract=read(base+'lesson-021-stage-c-exact-contract.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const families=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Owner 148 index preserves original canonical U02L05 identity and Arabic lesson',()=>{
 assert.equal(bp.Lessons.length,148);const b=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.SortOrder,21);assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,5);
 assert.equal(b.Title,'تبسيط التعابير الجذرية');
 const p=content.Lessons.find(x=>x.LessonCode===code);assert.ok(p);
 assert.match(p.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.match(p.SourceLocator,/130–136.*296–301/);
 const ar=p.Translations.find(x=>x.CultureCode==='ar');assert.ok(ar);
 assert.ok(ar.Explanation.length>300);assert.ok(ar.WorkedExamples.length>1000);
});
test('Arabic 12 original worked examples distinguish square roots, nth roots and real domain',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,16);assert.equal(l.Visuals.length,5);
 assert.ok(l.WorkedExamples.some(x=>x.Answer==='٦√٢'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('|س|')));
 for(const [a,b,r] of [[72,6,2],[50,5,2],[48,4,3]])assert.equal(a,b*b*r);
 assert.ok(Math.abs(Math.cbrt(54)-3*Math.cbrt(2))<1e-9);
 assert.ok(Math.abs(162**.25-3*2**.25)<1e-9);
});
test('Original 7-page General and 6-page Advanced source contain 29 accountable categories',()=>{
 assert.deepEqual(source.sourcePdfPages,[130,131,132,133,134,135,136]);
 assert.deepEqual(source.advancedSource.pdfPages,[296,297,298,299,300,301]);
 assert.equal(source.sourceElements.length,29);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,29);
 assert.ok(source.sourceElements.every(x=>x.appLocation));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Exact grade10 radical families are mapped to registered skill with fail-closed routing',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.radical.simplify']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(m.forbiddenQuestionFamilies.includes('uae.g9.general.u08.l06.like_radical_coefficient'));
 for(const f of contract.allowedQuestionFamilies){const row=families.families.find(x=>x.id===f);assert.ok(row,f);assert.equal(row.skillId,m.primarySkills[0]);assert.equal(row.productionRouting,false);assert.equal(row.answerType,'exact_integer');}
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const helper=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RadicalSimplification.cs'),'utf8');
 assert.match(helper,/class\s+UaeG10RadicalSimplificationResponse\b/);
 assert.match(helper,/static\s+bool\s+Verify\s*\(/);
 assert.match(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8'),/return\s+UaeG10RadicalSimplificationResponse\.Verify\(family,\s*parameters,\s*answer\)/);
});

 test('G10 L021 primary-source repair examples and arithmetic',()=>{const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));const e=load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-021-stage-d-source-review.json'),l=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l05-ar.rich-lesson-v2.json').Lessons[0],p=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===l.LessonCode),a=p.Translations.find(t=>t.CultureCode==='ar');for(const row of e.rows){assert.ok(l.WorkedExamples.some(x=>x.Title===row.originalWorkedExampleTitle&&x.Answer===row.expectedAnswer));assert.ok(a.WorkedExamples.includes(row.originalWorkedExampleTitle));assert.ok(a.StepByStepSolutions.includes(row.originalWorkedExampleTitle));}assert.equal(p.CanonicalBodySha256,crypto.createHash('sha256').update(JSON.stringify(Object.fromEntries(Object.keys(a).sort().map(k=>[k,a[k]])))).digest('hex'));let count=0;const eq=(x,y)=>{assert.ok(Number.isFinite(x)&&Number.isFinite(y)&&Math.abs(x-y)<=1e-10*Math.max(1,Math.abs(x),Math.abs(y)));count++;};eq((3*Math.sqrt(70)/14)**2,45/14);eq(4/(3+Math.sqrt(5)),3-Math.sqrt(5));eq(Math.sqrt(96/6),4);eq(Math.sqrt(2*126/7),6);eq(Math.sqrt(3)-Math.sqrt(3),0);eq(Math.sqrt(3)**2,3);assert.equal(count,e.arithmeticProbes);});
