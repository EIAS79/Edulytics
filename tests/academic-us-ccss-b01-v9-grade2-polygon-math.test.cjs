'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const renderer=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml'),'utf8');
const source=fs.readFileSync(path.join(root,'src/Edulytics.Services/LessonContent/RichLessonContentV2RuntimeComposer.cs'),'utf8');
const polygons={triangle:3,quadrilateral:4,square:4,pentagon:5,hexagon:6};
const parsePoints=name=>{
 const needle='"'+name+'" => "';
 const i=renderer.indexOf(needle);
 assert(i>=0,'Polygon kind not rendered: '+name);
 const tail=renderer.slice(i+needle.length);
 const t=tail.slice(0,tail.indexOf('"'));
 return t.trim().split(/\s+/).map(x=>x.split(',').map(Number));
};
test('Real renderer geometry has exactly 3/4/4/5/6 corners, closed polygon paths, distinct vertices and non-zero area',()=>{
 assert(renderer.includes('<polygon points="@polygonPoints"'));
 for(const [name,count] of Object.entries(polygons)){
  const pts=parsePoints(name);
  assert.equal(pts.length,count,name+' vertices');
  assert.equal(new Set(pts.map(x=>x.join(','))).size,count,name+' unique corners');
  assert(pts.every(([x,y])=>Number.isFinite(x)&&Number.isFinite(y)&&x>=0&&x<=220&&y>=0&&y<=175));
  let cross=0;
  for(let i=0;i<count;i++){
   const [x1,y1]=pts[i],[x2,y2]=pts[(i+1)%count];
   assert(x1!==x2||y1!==y2,'Degenerate edge');
   cross+=x1*y2-x2*y1;
  }
  assert(Math.abs(cross)>100,name+' nonzero polygon area');
 }
 const square=parsePoints('square');
 const edgeLengths=square.map(([x,y],i)=>Math.hypot(x-square[(i+1)%4][0],y-square[(i+1)%4][1]));
 assert(edgeLengths.every(x=>x===edgeLengths[0]),'Square must have four equal sides');
});
test('G2 U06 L02 first-edition purpose guard requires side/corner math; artwork color/SHA never decides shape identity',()=>{
 const label='PED:US-CCSS-MATH:G2:U06:L02';
 assert(source.includes('if (lessonCode == "'+label+'")'));
 assert(source.includes('body.Explanation.Contains("number of sides or corners"'));
 assert(source.includes('body.KeyConceptsAndRules.Contains("pentagons"'));
 for(const [name] of Object.entries(polygons))assert(source.includes('Items = ["'+name+'"]'));
 assert(source.includes('An OPEN chain is not a pentagon'));
});