'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
const registryPath='src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';
const sourcePath='src/Edulytics.Services/Mathematics/UsCcssG5Unit2SourceQuestions.cs';
const corePath='src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs';
const enginePath='src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';
const contractPath='src/Edulytics.Core/Mathematics/Practice/UsCcssG5Unit2SourcePracticeContracts.cs';
function checkLesson(id,family,correct,wrong){
 const source=read(sourcePath),core=read(corePath),engine=read(enginePath);
 const contracts=read(contractPath);
 const registry=JSON.parse(read(registryPath));
 assert.ok(registry.families.some(x=>x.id===family&&x.lessonPracticeRouting===true));
 assert.ok(contracts.includes(id));assert.ok(contracts.includes(family));
 assert.ok(core.includes('UsCcssG5Unit2SourcePracticeContracts.Reviewed'));
 for(const m of ['Supports','Build','Solve','Verify'])assert.ok(engine.includes('UsCcssG5Unit2SourceQuestions.'+m));
 for(const c of correct)assert.ok(source.includes(c),'required correct source option missing '+c);
 for(const w of wrong)assert.ok(source.includes(w),'required negative example missing '+w);
 assert.ok(source.includes('teacher review'));
}
test('G5 U02 L01 source fair-share options and teacher-review boundary',()=>{
 checkLesson('PED:US-CCSS-MATH:G5:U02:L01','usccss.g5.u02.l01.source_fair_share_choice',
 ['2/5 sandwich','2 1/2 sandwiches','1 1/2 sandwiches','2/3 sandwich'],
 ['5/2 sandwiches','2/5 sandwich']);
});
test('U02:L02 source-specific digital choice and teacher-led assessment',()=>{
 checkLesson('PED:US-CCSS-MATH:G5:U02:L02','usccss.g5.u02.l02.unit_share_equivalence',["1/2 sandwich","1/3 sandwich","1/4 sandwich"],["2 sandwiches","3 sandwiches"]);
});
test('U02:L03 exact source fraction-choice and teacher-scoring boundary',()=>{
 checkLesson('PED:US-CCSS-MATH:G5:U02:L03','usccss.g5.u02.l03.fraction_division_direction',["2/3 liter","Each receives 2/3 liter","3/2 liters"],["3/2 liters","Each receives 3/2 liters"]);
});
test('G5 U02 L04 source exact-choices and teacher evidence',()=>{
checkLesson('PED:US-CCSS-MATH:G5:U02:L04','usccss.g5.u02.l04.blueberry_division_context',["4/6 pound = 2/3 pound","4÷6=4/6","2/3 pound each"],["6/4 pound","6÷4=4/6"]);
});
test('G5 U02 L05 source exact-choices and teacher evidence',()=>{
checkLesson('PED:US-CCSS-MATH:G5:U02:L05','usccss.g5.u02.l05.fraction_division_generalization',["a÷b=a/b","5/2 pounds","2/5 pound"],["a÷b=b/a","2/5 pound"]);
});
test('G5 U02 L06 source fraction multiplication and divided-whole correspondence',()=>{
checkLesson('PED:US-CCSS-MATH:G5:U02:L06','usccss.g5.u02.l06.relay_division_unit_fraction',["3/2 miles","3×(1/2) miles","3 half-mile parts","3/2 miles"],["2/3 mile","2×(1/3) miles"]);
});
test('G5 U02 L07 source fraction multiplication and divided-whole correspondence',()=>{
checkLesson('PED:US-CCSS-MATH:G5:U02:L07','usccss.g5.u02.l07.unit_fraction_race_distances',["9/4 miles","7/4 miles","3/4 mile","9×1/4"],["4/9 mile","4/7 mile"]);
});
test('G5 U02 L08 source fraction geometry plus teacher models',()=>{
checkLesson('PED:US-CCSS-MATH:G5:U02:L08','usccss.g5.u02.l08.nonunit_fraction_of_whole',["6/5 unit squares","3/5","9/5","2×(3÷5)"],["2/15 unit square","5/3"]);
});
// NEXT_LESSON_TEST
