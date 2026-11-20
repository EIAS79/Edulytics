'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lessons=[
  ['01','01','VARIABLES-AND-EXPRESSIONS'],
  ['01','02','ORDER-OF-OPERATIONS'],
  ['01','03','PROPERTIES-OF-NUMBERS'],
  ['01','04','DISTRIBUTIVE-PROPERTY'],
  ['01','05','EQUATIONS'],
  ['01','06','RELATIONS'],
  ['01','07','FUNCTIONS'],
  ['01','08','INTERPRETING-FUNCTION-GRAPHS'],
  ['02','01','WRITING-EQUATIONS'],
  ['02','02','SOLVING-ONE-STEP-EQUATIONS'],
  ['02','03','SOLVING-MULTI-STEP-EQUATIONS'],
  ['02','04','VARIABLES-ON-BOTH-SIDES']
];
test('All twelve historic Grade 9 General A-C lessons retain canonical Arabic pack and explicit practice mapping',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 assert.equal(pack.AcademicLanguage,'ar');
 for(const [unit,lesson,slug] of lessons){
  const code='PED:UAE-MOE-MATH:L9:GENERAL:'+unit+':'+lesson+':'+slug;
  const source=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(source,code);
  assert.ok(source.Translations.some(x=>x.CultureCode==='ar'),code);
  assert.ok(mappings.mappings.some(x=>x.lessonCode===code),code);
 }
});
test('Twelve source-aligned Arabic teacher Cool-down components are student-facing and do not send or grade',()=>{
 for(const [unit,lesson,slug] of lessons){
  const file='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u'+unit+'-l'+lesson+'-source-cooldown-ar.html';
  const html=fs.readFileSync(path.join(root,file),'utf8');
  assert.match(html,/lang="ar" dir="rtl"/,file);
  for(let q=1;q<=3;q++)assert.ok(html.includes('id="q'+q+'"'),file+' missing Q'+q);
  assert.ok(html.includes('PED:UAE-MOE-MATH:L9:GENERAL:'+unit+':'+lesson+':'+slug),file);
  assert.match(html,/TEACHER_REVIEW_REQUIRED/);
  assert.match(html,/graded:false,submitted:false/);
  assert.match(html,/لم تُرسل البيانات إلى Edulytics/);
 }
});
test('Prior A-C evidence remains source-specific and never pretends Stage D executed',()=>{
 const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
 const f=read(base+'u01l01-stage-a-c-checkpoint.json');
 assert.equal(f.delivery.lessonClosed,true);assert.equal(f.delivery.testsRun,'NOT_RUN');
 for(const [unit,lesson] of lessons.slice(1)){
  const evidence=read(base+'u'+unit+'l'+lesson+'-a-c-evidence-and-prepared-tests.json');
  assert.ok(Array.isArray(evidence.cooldown) && evidence.cooldown.length===3);
 }
});
