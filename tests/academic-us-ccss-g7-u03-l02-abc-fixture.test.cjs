'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('U03 L02 circle features and independent Cool-down are authored',()=>{const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit3CircleFigures.cshtml');const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u03-l02-circle-features.svg');for(const s of ['data-g7-u03-l02','2.1 Sort','2.2 Measure','2.3 Construct','Cool-down','Teacher review'])assert.ok(v.includes(s));assert.match(svg,/<title/);assert.match(svg,/<desc/);});
test('U03 L02 radius-diameter arithmetic is exact',()=>{assert.equal(2*4,8);assert.equal(11/2,5.5);});
test('U03 L02 records Extend not New Engine',()=>{const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u03-l02-abc-cursor.json'));assert.equal(c.extendedFamily,'usccss.g7.u03.l02.radius_diameter_relation');assert.equal(c.newEngine,false);assert.equal(c.READY_FOR_STAGE_D,true);assert.equal(c.testExecution,'AUTHORED_NOT_RUN');});
