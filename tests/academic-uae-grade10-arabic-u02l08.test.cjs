'use strict';
// Grade10 General owner-approved U02L08. Written in A-B-C; NOT RUN until Stage D.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U02:L08:C024';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l08-ar.rich-lesson-v2.json');
const source=read(base+'lesson-024-source-elements.json');
const contract=read(base+'lesson-024-stage-c-exact-contract.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('U02L08 is the unique 24th canonical Arabic Grade10 lesson with two approved book sources',()=>{
 assert.equal(bp.Lessons.length,148);
 assert.equal(bp.Units.length,18);
 const b=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);
 assert.equal(b.SortOrder,24);
 assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,8);
 assert.equal(b.Title,'حل المعادلات الجذرية');
 assert.equal(bp.Lessons.filter(x=>x.LessonCode===code).length,1);
 const l=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.ok(l.Translations.some(x=>x.CultureCode==='ar'&&x.Explanation.length>250));
 assert.match(l.SourceLocator,/318–324.*177–181/);
});
test('All 29 source elements have individual advanced/applied book and teaching targets',()=>{
 assert.equal(source.sourceElements.length,29);
 assert.deepEqual(source.sourcePdfPages,[318,319,320,321,322,323,324]);
 assert.deepEqual(source.appliedSource.pdfPages,[177,178,179,180,181]);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,29);
 assert.ok(source.sourceElements.every(x=>x.sourceBook&&x.sourcePdfPage&&x.appLocation&&x.coverageStatus));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(source.originalPublisherContentCopied,false);
});
test('Arabic lesson distinguishes valid, extraneous and no-real-root cases and inequalities',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,14);
 assert.equal(l.Visuals.length,5);
 assert.ok(l.WorkedExamples.some(x=>x.Question.includes('√(س+٥)=٤')));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('لا يوجد حل')));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('−١≤س<٨')));
 assert.equal(Math.sqrt(11+5),4);
 assert.notEqual(Math.sqrt(11+5),-4);
 assert.equal(Math.sqrt(5+4),5-2);
 assert.notEqual(Math.sqrt(0+4),0-2);
 assert.ok([-1,0,1,7].every(x=>Math.sqrt(x+1)<3));
 assert.equal(Math.sqrt(8+1),3);
});
test('Exact Grade10 radical-equation families use source matching primary skill and stay fail-closed',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.radical.solve_equation']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 for(const id of contract.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===id);
  assert.ok(f,id);
  assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.answerType,'exact_integer');
  assert.equal(f.productionRouting,false);
 }
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const e=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RadicalEquation.cs'),'utf8');
 assert.match(e,/BuildG10RadicalEquation/);
 assert.match(e,/root\*root==radicand/);
 assert.match(e,/signedRightSide/);
});
