'use strict';
// Authoring only; NO Stage D test execution.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('Grade7 U02 L03 source pages represented in guarded view',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l03','3.1 Equivalent','3.2 Centimeters',
    '3.3 A plane','Cool-down','36 min'])
 assert.ok(v.includes(s),s);
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L03"'));
});
test('Independent reciprocal & scale-area arithmetic',()=>{
 assert.equal(88.49*10,884.9);
 assert.equal(699.1/10,69.91);
 assert.equal(550*1.7,935);
 assert.equal(330/550*60,36);
 assert.equal(1/10,0.1);
 assert.equal(10*10,100);
 assert.equal(Math.sqrt(25000/10),50);
 assert.equal(14*2.5,35);
});
test('Readiness remains authoring-only',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l03-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.D_STATUS,'NOT_RUN');
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
});
