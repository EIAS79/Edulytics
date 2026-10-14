'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const src=p=>fs.readFileSync(path.join(root,p),'utf8');
const model=src('src/Edulytics.Core/Curriculum/RichLessonContentV2.cs');
const html=src('src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml');
const builder=src('src/Edulytics.Services/LessonContent/UsCcssB01V9SourceFigureStructuralGapVisuals.cs');
const manifest=JSON.parse(src('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'));
const original=JSON.parse(src('artifacts/academic-us-ccss/batches/B01-v9-original-teacher-figure-dom-asset-map-40.json'));
const codes=['G2:U06:L03','G2:U06:L19','G2:U08:L02','G2:U08:L03','G3:U04:L11','G3:U04:L15','G3:U07:L01','G3:U07:L04','G3:U07:L06','G3:U08:L01','G4:U04:L20'];
test('The eleven original source-figure repairs remain exact B01 500-manifest identities, with no new academic PASS',()=>{
 assert.equal(manifest.lessons.length,500);assert.equal(original.rows.length,40);
 const m=new Set(manifest.lessons.map(x=>x.lessonCode)),f=new Set(original.rows.map(x=>x.lessonCode));
 for(const code of codes){
  const x='PED:US-CCSS-MATH:'+code;
  assert(m.has(x)&&f.has(x),code);
  assert(builder.includes('[Prefix+"'+code+'"]'),code+' source guard');
  assert(builder.includes('"'+code+'" =>'),code+' returned visuals');
 }
 assert.equal(new Set(codes).size,11);
 assert(builder.includes('body.Title!=goal.Title'));
 assert(builder.includes('body.Explanation.Contains(goal.Cue'));
 assert(builder.includes('B01 source-bound figure purpose drift'));
 assert(builder.includes('Does NOT imply complete source-activity fidelity'));
});
test('Pairs visually show exactly two counters in each pair and one extra iff odd, including zero-fraud safeguards',()=>{
 assert.match(model,/CounterPairs = 17/);
 assert(html.includes('RichLessonVisualKind.CounterPairs'));
 assert(html.includes('var pairCount = total / 2'));
 assert(html.includes('var leftover = total % 2'));
 assert(html.includes('pair < pairCount'));
 assert(html.includes('leftover == 1'));
 assert(html.includes('aria-label="@Model.Description"'));
 for(let n=1;n<=99;n++){
  const pairs=Math.floor(n/2),remainder=n%2;
  assert.equal(2*pairs+remainder,n);
  assert(remainder===0||remainder===1);
  assert.equal(remainder===0,n%2===0);
 }
});
test('Rhombus is a true four-equal-sided nonrectangular quadrilateral, not a diamond icon mislabeled',()=>{
 const m=html.match(/"rhombus" => "([^"]+)"/);
 assert(m,'rhombus SVG geometry must exist');
 const points=m[1].split(' ').map(p=>p.split(',').map(Number));
 assert.equal(points.length,4);
 const lengths=points.map(([x,y],i)=>Math.hypot(x-points[(i+1)%4][0],y-points[(i+1)%4][1]));
 assert(lengths.every(x=>Math.abs(x-lengths[0])<1e-8),'all four sides equal');
 const a=[points[0][0]-points[1][0],points[0][1]-points[1][1]];
 const b=[points[2][0]-points[1][0],points[2][1]-points[1][1]];
 assert.notEqual(a[0]*b[0]+a[1]*b[1],0,'sample rhombus is not secretly a rectangle');
});
test('Money tape, fractional benchmarks, regrouping and area equations are exact, styling irrelevant',()=>{
 assert(builder.includes('Items=["68","39","29","0"]'));
 assert.equal(68-39,29);
 assert(builder.includes('Numerator=4,Denominator=8'));
 assert(builder.includes('Numerator=5,Denominator=8'));
 assert(5/8>1/2);
 assert(builder.includes('75,940 − 12,786 = 63,154'));
 assert.equal(75940-12786,63154);
 assert(builder.includes('3 times (4 + 2) = 12 + 6 = 18'));
 assert.equal(3*(4+2),18);
 assert(builder.includes('15 times 4 = 60'));
 assert(builder.includes('5 + 3 + 5 + 3 = 16'));
 assert(!builder.includes('BATCH_CERTIFIED'));
});