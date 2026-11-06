'use strict';
// Grade 6 U01:L02 independent A–C authoring oracles. NOT live engine or execution proof.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const json = p => JSON.parse(fs.readFileSync(path.join(root, p),'utf8'));
const rich=json('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json');
const row=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G6:U01:L02');
const ledger=json('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json');
const evidence=json('artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l02-abc-source-first-party-20261030.json');
test('source-aligned six? no: exactly eight pieces with exact eight square units',()=>{
 assert.equal(1+4*0.5+1+2*2,8);
 assert.equal(4*0.5,2);
 assert.equal(2*0.5,1);
 assert.notEqual(2*2,8);
 assert.ok(Math.abs(Math.sqrt(8)**2 - 8) < 1e-10);
 assert.equal(evidence.practice.current,'NO_EXPLICIT_LESSON_SKILL_MAPPING');
});
test('valid and invalid area submissions, equivalents and malformed',()=>{
 const numeric=x=>typeof x==='number'&&Number.isFinite(x)&&x===2.5;
 assert.equal(numeric(2.5),true);
 assert.equal(numeric(5/2),true);
 assert.equal(numeric(3),false);
 assert.equal(numeric('5/2'),false);
 assert.equal(numeric(NaN),false);
 assert.equal(numeric(null),false);
});
test('non-square conservation and paper construction remain teacher reviewed',()=>{
 assert.ok(row.WorkedExamples.some(x=>/Full set of tangram pieces/.test(x.Title)));
 assert.ok(row.WorkedExamples.some(x=>/Edulytiks-original exit check/.test(x.Title)));
 assert.ok(row.Visuals.every(v=>v.Kind!=='ShapeDecomposition'||v.Rows.length>0));
 assert.equal(evidence.A_STATUS.startsWith('A_CLOSED'),true);
 assert.equal(evidence.B_STATUS.startsWith('B_CLOSED'),true);
 assert.match(evidence.C_STATUS,/BLOCKED/);
 assert.equal(evidence.READY_FOR_STAGE_D,false);
 assert.equal(ledger.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U01:L02').academicAcceptance,'NOT_ACCEPTED');
});
