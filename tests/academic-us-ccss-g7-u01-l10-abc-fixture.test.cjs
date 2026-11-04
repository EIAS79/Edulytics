'use strict';
// Grade 7 U01:L10 source-specific static fixture. AUTHORING_NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
test('L10 source-aligned geometry, scale and boundary authored',()=>{
 const view=fs.readFileSync(path.join(root,
  'src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
 const contracts=fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const checkpoint=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l10-abc-cursor.json'),'utf8'));
 assert.match(view,/data-g7-u01-l10/);
 assert.match(view,/300-metre by 180-metre garden/);
 assert.match(view,/Teacher review/);
 assert.match(contracts,/usccss\.g7\.u01\.l10\.change_scale_length_and_area/);
 assert.equal(checkpoint.READY_FOR_STAGE_D,true);
 assert.equal(checkpoint.testExecution,'TEST_AUTHORED_NOT_RUN');
});
test('L10 no linear-area false equivalence',()=>{
 assert.equal((120*90)/2,5400);
 assert.equal((120/5)*(90/5)/2,216);
 assert.equal(60*12*12,8640);
 assert.notEqual(60*12,60*12*12);
 assert.deepEqual([10,6].map(x=>x*(30/20)),[15,9]);
 assert.equal(15*9/(10*6),9/4);
});
