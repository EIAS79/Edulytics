// Geometry U02:L07: static academic authoring fixtures only. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l07-asa-parallelogram-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l07-l09-2021.rich-lesson-v2.json');
test('exact L07 offline teacher-selected assessment and seven source-indexed practice tasks',()=>{
 assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert(a.LessonCode.endsWith('U02:L07'));
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.Practice.length,7);
 assert.equal(a.HumanRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
 assert.equal(r.Lessons.find(x=>x.LessonCode===a.LessonCode).WorkedExamples.length,3);
});
test('paper alternative is explicitly allowed for source optional technology warmup',()=>{
 assert.equal(a.SelectedPracticeRoute.OriginalGeoGebraApplet,'SOURCE_OPTIONAL_NOT_RECREATED');
 assert(a.SourceTaskCrosswalk.some(x=>x.Source==='7.1'));
 assert(a.SourceTaskCrosswalk.some(x=>x.Source==='7.3 OPTIONAL'));
});
test('ASA must use unique intersection, not unspecified SAS radius',()=>{
 assert.equal(a.IndependentOracles.ASA_ThirdVertex,'UNIQUE_INTERSECTION_OF_TWO_DISTINCT_NONPARALLEL_LINES');
 assert(a.AuthoredFixtures.Wrong.some(x=>x.includes('unknown distances')));
});
test('P3 uses GE as included side and only same-side squares universally congruent',()=>{
 assert(a.IndependentOracles.P3.includes('ASA'));
 assert.equal(a.IndependentOracles.P5_OnlyUniversallyCongruent,'ALL_SQUARES_WITH_SIDE_5');
});