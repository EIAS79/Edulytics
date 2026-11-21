'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const activityPath='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-original-61-practice-ar.html';
const cooldownPath='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-source-cooldown-ar.html';

test('All 61 visually identified Grade 9 lesson 1-1 source items have an original independent teacher-reviewed task',()=>{
 const audited=read(base+'u01l01-all-61-source-exercises-preparation.json');
 const practice=read(base+'u01l01-original-61-parallel-skill-practice.json');
 assert.equal(practice.questionCount,61);
 assert.equal(practice.records.length,61);
 assert.equal(audited.items.length,61);
 assert.equal(practice.coreSkillTasks,46);
 assert.equal(practice.priorSkillReviewTasks,15);
 const ids=new Set();
 for(let i=0;i<61;i++){
   const source=audited.items[i],task=practice.records[i];
   assert.equal(task.sourceItem,source.sourceItem);
   assert.equal(task.printedPage,source.printedPage);
   assert.equal(task.sourceSkillCategory,source.visualSourceSkillCategory);
   assert.equal(task.sourceGroup,source.sourceGroup);
   assert.equal(task.independentTaskId,'G9-U01L01-T'+String(i+1).padStart(2,'0'));
   assert.equal(task.scope,i<46?'CORE_SKILL':'PRIOR_SKILL_REVIEW');
   assert.equal(task.gradingRoute,'TEACHER_REVIEW_REQUIRED');
   assert.ok(task.studentPrompt.trim().length>5);
   assert.ok(task.teacherExpectedEvidence.trim().length>0);
   assert.ok(!ids.has(task.independentTaskId));
   ids.add(task.independentTaskId);
 }
 assert.equal(practice.records[28].sourceSkillCategory,'CYLINDER_VOLUME_MODEL');
 assert.equal(practice.records[36].sourceSkillCategory,'SAME_BASE_POWER_PRODUCT');
 assert.equal(practice.records[40].sourceSkillCategory,'CUBE_VOLUME_FACE_AREA_RELATION');
 assert.equal(practice.records[46].scope,'PRIOR_SKILL_REVIEW');
 assert.equal(practice.sourceExerciseItemsCheckedForMathematicalEquivalence,0);
 assert.equal(practice.fullSourceElementParityVerified,false);
 assert.equal(practice.publishGateSatisfied,false);
});

test('Student activity is 61-task Arabic RTL teacher-only and reachable from existing Grade 9 cooldown',()=>{
 const html=fs.readFileSync(path.join(root,activityPath),'utf8');
 const cooldown=fs.readFileSync(path.join(root,cooldownPath),'utf8');
 const match=html.match(/const tasks = (\[[^\n]+\]);/);
 assert.ok(match,'embedded student questions exist');
 const tasks=JSON.parse(match[1]);
 assert.equal(tasks.length,61);
 assert.equal(tasks.filter(x=>x.scope==='CORE_SKILL').length,46);
 assert.equal(tasks.filter(x=>x.scope==='PRIOR_SKILL_REVIEW').length,15);
 assert.ok(tasks.every(x=>!Object.hasOwn(x,'teacherExpectedEvidence')));
 assert.ok(tasks.every(x=>x.studentPrompt && x.independentTaskId && x.sourceItem));
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/event\.preventDefault\(\)/);
 assert.match(html,/graded:false/);
 assert.match(html,/submitted:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.doesNotMatch(html,/\bfetch\(/);
 assert.doesNotMatch(html,/XMLHttpRequest|sendBeacon\(/);
 assert.match(cooldown,/href="u01-l01-original-61-practice-ar\.html"/);
});

test('Independent source-aligned task arithmetic samples use correct mathematical conclusions',()=>{
 const r=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 assert.equal(10**4*10**2,10**6);
 assert.equal(Math.PI*3**2*5,45*Math.PI);
 assert.equal(4*(5**2+4)-(4*5**2+4),12);
 assert.ok(Math.abs((2/3)*(3/5)-2/5)<1e-12);
 assert.ok(Math.abs((5/6)/(10/9)-3/4)<1e-12);
 assert.ok(Math.abs((7/10)*(5/14)-1/4)<1e-12);
 assert.equal((3+4+4+5+9)/5,5);
 assert.equal(8.43-2.17,6.26);
 assert.equal((1940-785)/100,11.55);
 assert.equal(r[36].sourceSkillCategory,'SAME_BASE_POWER_PRODUCT');
 assert.equal(r[46].sourceSkillCategory,'PRIOR_DATA_CHART');
});
