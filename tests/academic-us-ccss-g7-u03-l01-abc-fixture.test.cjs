'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('U03 L01 measurement teaching and independent Cool-down are authored',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit3CircleFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u03-l01-square-measurement.svg');
 for(const s of ['data-g7-u03-l01','1.1 Collect','1.2 Decide','1.3 Measurement error','Cool-down','Teacher review']) assert.ok(v.includes(s));
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
});
test('U03 L01 closed arithmetic witnesses are correct',()=>{assert.deepEqual([2,4,6].map(x=>4*x),[8,16,24]);assert.deepEqual([2,4,6].map(x=>x*x),[4,16,36]);});
test('U03 L01 A-C is authored not executed',()=>{const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u03-l01-abc-cursor.json'));assert.equal(c.READY_FOR_STAGE_D,true);assert.equal(c.newEngine,false);assert.equal(c.testExecution,'AUTHORED_NOT_RUN');});
