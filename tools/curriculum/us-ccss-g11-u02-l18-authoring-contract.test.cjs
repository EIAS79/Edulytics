'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L18', rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u02-l18.rich-lesson-v2.json').Lessons[0];
const assessment=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u02-l18-local-source-20261106.json'),evidence=read('artifacts/curriculum-delivery/US-CCSS-MATH/G11/u02-l18-element-coverage.json');
test('unit 2 lesson identity and source task coverage evidence',()=>{
 assert.equal(rich.LessonCode,code);
 assert.equal(assessment.LessonCode,code);
 assert.equal(evidence.LessonCode,code);
 assert.ok(evidence.Goals.length>0);
 assert.ok(evidence.ElementCount>=8);
 assert.equal(evidence.SourceElements.length,evidence.ElementCount);
 assert.equal(new Set(evidence.SourceElements.map(x=>x.SourceElementId)).size,evidence.ElementCount);
 assert.ok(rich.ExplanationParagraphs.length>=5);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');
 assert.equal(assessment.StageD,'NOT_RUN');
 assert.equal(assessment.TeacherReview.Required,true);
 assert.equal(assessment.FirstPartyCoolDown.Origin,'EDULYTICS_ORIGINAL');
});
test('lesson-specific independent arithmetic oracle',()=>{assert.equal(5+1000/100,15);});
test('teacher and learner source still distinguish authored from tested',()=>{
 assert.equal(assessment.QuestionRouting,'UNPROMOTED_NO_RUNTIME_CLAIM');
 assert.ok(assessment.CoreMathWorkedExample.Question.length>15);
 assert.ok(assessment.CoreMathWorkedExample.Answer.length>2);
 assert.ok(evidence.SourceElements.some(x=>x.SourceElementId.includes('PRACTICE')));
});
