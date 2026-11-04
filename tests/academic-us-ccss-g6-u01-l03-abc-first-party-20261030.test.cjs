'use strict';
// Grade6 U01:L03 independent paper-model mathematics and source authoring; NOT_RUN
const {test}=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');const root=path.resolve(__dirname,'..');const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json');const l=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G6:U01:L03');const evidence=load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l03-abc-first-party-20261030.json');const ledger=load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json');
test('three independent source off-grid shaded areas and correct square units',()=>{
 const a=2*(5*3/2),b=4*(2*4/2),c=5*5-(2*2);assert.deepEqual([a,b,c],[15,16,21]);assert.equal(5*5-2,23);assert.notEqual(23,c);assert.equal(6*6-3*3,27);
});
test('source-conserving paper move maintains shaded region under moving same piece',()=>{
 assert.equal(4*4-1+1,16);assert.equal(8*5-3*2+3*2,40);assert.notEqual(8*5-3*2,40);
 assert.ok(l.Visuals.every(v=>v.Kind!=='ShapeDecomposition'||v.Rows.length>0));
 assert.match(l.WorkedExamples.find(e=>/Edulytiks-original Cool-down/.test(e.Title)).Answer,/both 40 square units/);
});
test('malformed, incorrect and equivalently expressed numeric claims are not a drawing review',()=>{
 const grade=(v,unit)=>typeof v==='number'&&Number.isFinite(v)&&v===16&&unit==='cm²';
 assert.equal(grade(16,'cm²'),true);assert.equal(grade(4*(2*4/2),'cm²'),true);assert.equal(grade(16,'cm'),false);assert.equal(grade(25-2*2,'cm²'),false);assert.equal(grade('16','cm²'),false);assert.equal(grade(null,'cm²'),false);
 assert.match(evidence.C_STATUS,/BLOCKED/);assert.equal(evidence.READY_FOR_STAGE_D,false);assert.equal(evidence.testExecution,'AUTHORED_NOT_RUN');assert.equal(ledger.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U01:L03').academicAcceptance,'NOT_ACCEPTED');
});
