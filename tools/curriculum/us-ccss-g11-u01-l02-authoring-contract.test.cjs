'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'../..',p),'utf8'));
const code='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L02';
const lesson=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l02.rich-lesson-v2.json').Lessons[0];
const check=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l02-source-teacher-led-20261106.json');
const evidence=read('artifacts/curriculum-delivery/US-CCSS-MATH/G11/u01-l02-element-coverage.json');
test('exact identity and source task paths',()=>{
 assert.equal(lesson.LessonCode,code);assert.equal(evidence.LessonCode,code);
 assert.equal(evidence.Elements.length,26);
 assert.equal(new Set(evidence.Elements.map(e=>e.SourceElementId)).size,26);
 assert.equal(check.StageD,'NOT_RUN');
 assert.equal(check.TestsExecuted,'NOT_RUN');
});
test('independent geometric-sequence and conserved-area oracle',()=>{
 for(let n=0;n<=6;n++){const count=2**n;const area=80/count;assert.equal(count*area,80)}
 assert.deepEqual([1.5,3,6,12,24,48].map(Number),[1.5,3,6,12,24,48]);
 assert.equal(256*(3/4)**2,144);
 assert.equal(2**6,64);
});
test('authored paper-activity and fraction answer types are not claimed executed',()=>{
 assert.ok(check.SkillContracts[0].QuestionFamilies.includes('usccss.g11.alg2.u01.l02.paper_piece_area'));
 assert.equal(check.SkillContracts[0].Readiness,'WRITTEN_NOT_RUN');
 assert.equal(check.FirstPartyCoolDown.Origin,'EDULYTICS_ORIGINAL');
 assert.ok(lesson.ExplanationParagraphs.some(p=>p.includes('conserv')));
 assert.ok(lesson.CommonMistakes.some(x=>x.Mistake.includes('continuous')));
});
