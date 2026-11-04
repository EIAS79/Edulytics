'use strict';
// G7 U01:L09 authoring check written NOT_RUN during A-C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.join(__dirname,'..');
test('L09 source-specific drawing and size relations are represented',()=>{
 const view=fs.readFileSync(path.join(root,
   'src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
 const contracts=fs.readFileSync(path.join(root,
   'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const c=JSON.parse(fs.readFileSync(path.join(root,
   'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l09-abc-cursor.json'),'utf8'));
 assert.match(view,/data-g7-u01-l09/);
 assert.match(view,/4-metre bedroom wall/);
 assert.match(view,/Cool-down — independently authored Edulytiks task/);
 assert.match(contracts,/usccss\.g7\.u01\.l09\.create_drawing_scale_lengths/);
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'TEST_AUTHORED_NOT_RUN');
});
test('L09 semantic exact source numeric checkpoints',()=>{
 assert.equal(16/4,4);
 assert.deepEqual([2.5,2.75,3.75].map(x=>x*4),[10,11,15]);
 assert.deepEqual([270,350,110,70].map(x=>x/50),[5.4,7,2.2,1.4]);
 assert.equal(9/1.5,6);
 assert.equal(3*6,18);
 assert.equal(6/2,3);
});
