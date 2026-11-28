'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
const evidence=read(base+'lesson-001-stage-d-numbered-source-review.json');
test('G10 L001 numbered inventory is traceable, but not atomic parity',()=>{
 assert.equal(evidence.numberedQuestionsIndexed,91);
 assert.deepEqual(evidence.page23to25Rows.map(x=>x.number),Array.from({length:65},(_,i)=>i+1));
 assert.equal(new Set(evidence.page23to25Rows.map(x=>x.sourceElementId)).size,65);
 for(const row of evidence.page23to25Rows){
   assert.equal(row.status,'DEDICATED_ORIGINAL_EQUIVALENT_AUTHORED_NOT_RUNTIME_VERIFIED');
   assert.ok(row.originalArtifact && row.originalArtifact.workedExampleTitle===row.representativeArtifactTitle);
   assert.ok(rich.WorkedExamples.some(e=>e.Title===row.representativeArtifactTitle));
 }
 const p26=read(base+'lesson-001-stage-d-page-026-review.json');
 assert.deepEqual(p26.rows.map(r=>r.sourceQuestionNumber),Array.from({length:26},(_,i)=>i+66));
 assert.equal(evidence.coverage,'UNKNOWN');assert.equal(evidence.productionRouting,false);
 assert.equal(rich.WorkedExamples.length,evidence.currentExampleCount);
});
test('G10 L001 revised arithmetic and signed/closed figures have valid data',()=>{
 for(let x=-10;x<=10;x++){
   assert.equal((2*x**3+3*x*x-4)+(-2*x**3+5*x-1)-(x*x-2*x+3),2*x*x+7*x-8);
   assert.equal((2*x+5)*(x+2),2*x*x+9*x+10);
   assert.equal(2*(2*x+5+x+2),6*x+14);
   assert.equal((2*x-1)+(2*x+1)+(2*x+3),6*x+3);
   assert.equal((2*x*x+5*x+40)-(x*x+2*x+15),x*x+3*x+25);
 }
 for(let a=-3;a<=3;a++)for(let b=-3;b<=3;b++)for(let c=-3;c<=3;c++){
   assert.equal((4*a*a*b+3*b*c-2*a)+(-a*a*b+5*a*c-7*b*c)-(2*a*c-a),3*a*a*b-4*b*c+3*a*c-a);
 }
 const polys=rich.Visuals.filter(v=>v.Kind==='ClosedCoordinatePolygon');
 assert.equal(polys.length,2);
 assert.deepEqual(polys.map(p=>p.Rows.map(r=>r.Cells.map(Number))),[[[-4,2],[2,-1],[1,3]],[[-3,-2],[2,-2],[3,2],[-2,3]]]);
 const inequality=rich.Visuals.find(v=>v.Kind==='CoordinatePlane');
 assert.ok(inequality&&inequality.Description.includes('ص≥س−١')&&inequality.Description.includes('ص<−س+٣'));
 const area=rich.Visuals.find(v=>v.Title==='نقاط من منحنى مساحة المستطيل');
 assert.equal(area.Rows.length,19);
 for(const r of area.Rows){const [x,y]=r.Cells.map(Number);assert.equal(y,x*(20-x));assert.ok(x>0&&x<20);assert.equal(y,100-(x-10)**2);}
 assert.equal(Math.max(...area.Rows.map(r=>Number(r.Cells[1]))),100);
});
test('G10 L001 fallback digest agrees with the current Arabic body and evidence',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const l=pack.Lessons.find(l=>l.LessonCode===rich.LessonCode);
 const a=l.Translations.find(t=>t.CultureCode==='ar');
 const ordered=Object.fromEntries(Object.keys(a).sort().map(k=>[k,a[k]]));
 const sha=crypto.createHash('sha256').update(JSON.stringify(ordered)).digest('hex');
 assert.equal(l.CanonicalBodySha256,sha);
 assert.equal(evidence.canonicalBodySha256,sha);
 assert.equal(read(base+'lesson-001-stage-d-checkpoint.json').canonicalBodySha256,sha);
 assert.ok(rich.WorkedExamples.every(e=>e.Steps.length>=3));
 assert.equal(pack.Lessons.length,148);
});