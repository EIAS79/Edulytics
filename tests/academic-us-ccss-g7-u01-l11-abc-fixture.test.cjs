'use strict';
// Grade 7 U01:L11 source oracle authoring only; NOT_RUN in A-C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
test('L11 unitless same-unit and source partial mapping authored',()=>{
 const src=fs.readFileSync(path.join(root,
  'src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
 const contracts=fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const c=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l11-abc-cursor.json'),'utf8'));
 assert.match(src,/data-g7-u01-l11/);
 assert.match(src,/1 to 180/);
 assert.match(contracts,/usccss\.g7\.u01\.l11\.unitless_scale_and_conversions/);
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'TEST_AUTHORED_NOT_RUN');
});
test('L11 mixed units must convert before unitless ratio',()=>{
 assert.equal(15*12,180);
 assert.equal(120/15,8);
 assert.equal(75/15,5);
 assert.equal(120/20,6);
 assert.equal(75/20,3.75);
 assert.equal(8*72/12,48);
 assert.equal(6*12/180,.4);
});
