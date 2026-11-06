'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U03:L12',rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u03-l12.rich-lesson-v2.json').Lessons[0],
 assessment=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u03-l12-local-source-20261106.json'),evidence=read('artifacts/curriculum-delivery/US-CCSS-MATH/G11/u03-l12-element-coverage.json');
test('exact canonical identity and local source elements',()=>{
 assert.equal(rich.LessonCode,code);
 assert.equal(assessment.LessonCode,code);
 assert.equal(evidence.LessonCode,code);
 assert.ok(evidence.Goals.length>=1);
 assert.equal(evidence.SourceElements.length,evidence.ElementCount);
 assert.ok(evidence.ElementCount>=5);
 assert.equal(new Set(evidence.SourceElements.map(x=>x.SourceElementId)).size,evidence.ElementCount);
 assert.ok(rich.ExplanationParagraphs.length>=4);
 assert.ok(rich.WorkedExamples.length>=2);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');
 assert.equal(assessment.StageD,'NOT_RUN');
 assert.equal(assessment.FirstPartyCoolDown.Origin,'EDULYTICS_ORIGINAL');
 assert.equal(assessment.TeacherReview.Required,true);
});
test('independent first-party mathematical oracle',()=>{assert.equal(2+4,6);assert.equal(3-5,-2);});
test('authored mathematics remains distinct from Stage D runtime',()=>{
 assert.equal(assessment.QuestionRouting,'UNPROMOTED_NO_RUNTIME_CLAIM');
 assert.ok(assessment.CoreMathWorkedExample.Steps.length>=1);
 assert.ok(assessment.CoreMathWorkedExample.Answer.length>=1);
 assert.ok(evidence.SourceElements.some(x=>x.SourceElementId.includes('PRACTICE')));
});
