
'use strict';
const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2/';
const additions={
 L08:{
 file:dir+'us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json',
 anchor:'A pattern with six equally spaced repeats uses a rotation of 360°/6=60°. With four repeats it uses 90°; with three, 120°. The center and the exact number of turns matter; a rotation of 360° always returns the shape to its start.',
 lines:[
 'Activity 8.1: create an original right isosceles triangle OAB with OA=OB=3 and angle AOB=90°. Draw its images after clockwise rotations of 90°, 180°, and 270° about O; four copies exactly form a square. Explain why the equal legs fit without gaps.',
 'Activity 8.2: rotate segment PQ through 180° about endpoint Q, midpoint M, and off-line point C. For a half-turn about center C, each image X′ satisfies X′=2C−X. Endpoint rotation extends the original line; midpoint rotation exchanges segment endpoints; off-line rotation makes a parallel segment at the same length.',
 'Activity 8.3: use four rigid copies of an asymmetric triangle to build and label a pattern. Identify corresponding lengths and transformations; a drawn construction and its explanation require teacher or accessible graphical assessment.'
 ]},
 L09:{
 file:dir+'us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json',
 anchor:'Draw horizontal lines y=1 and y=4, then translate both upward two units. Compare their separation and slopes. Next rotate a pair of horizontal line segments a quarter-turn about the origin and observe how their directions change together.',
 lines:[
 'Activity 9.1: for a line ℓ and its image ℓ′, identify a possible single rigid motion and then plot corresponding images A′ and B′. Mapping only the infinite line without its labeled points does not complete the source task.',
 'Activity 9.2: apply a common translation, half-turn about K, and mirror reflection across h to the same two parallel lines. Explain why their distance and parallelism remain unchanged, even when both line directions reverse.',
 'Activity 9.3: under a half-turn around D on a line, every point exchanges position with its image across D and the full line maps to itself. For intersecting lines rotated together about O, reason from corresponding angles and vertical angles rather than appearance alone.'
 ]},
 L10:{
 file:dir+'us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json',
 anchor:'Original task: build a 6-by-2 rectangle from two identical 3-by-2 rectangles. Translate one piece next to the other and ask what happens to the shared interior boundary. Discuss which lengths become exterior edges.',
 lines:[
 'Activity 10.1: start from isosceles triangle ABC with AB=AC. Reflect ABC over side AB, then rotate around A to align the image of C with B; corresponding base angles B and C are congruent because reflections and rotations preserve angle measure.',
 'Activity 10.2: mark M as the midpoint of side AC of triangle ABC, then rotate the triangle 180° around M. The image vertices are C,D,A, and quadrilateral ABCD is a parallelogram because opposite corresponding sides are parallel. Two congruent triangles fill it, so each has half its area.',
 'Activity 10.3: construct and label two other rigid images of an original scalene triangle. Identify at least two corresponding segment pairs and two equal angle pairs by matching vertices; teacher review is required for the constructed diagram and explanation.',
 'Activity 10.4: begin with triangle ONE where ON=OE=5 and angle NOE=30°. Reflect across ON and then across OM, marking the successive images M and T. Justify the new angles and all 5-unit image rays; numerical repetition is not sufficient evidence of a constructed sequence.'
 ]}
};
for(const [id,info] of Object.entries(additions)){
 const p=path.join(root,info.file);let txt=fs.readFileSync(p,'utf8');
 if(txt.includes(info.lines[0]))continue;
 const target='        '+JSON.stringify(info.anchor)+',';
 if(!txt.includes(target))throw Error('No source-preserving insert anchor for '+id);
 txt=txt.replace(target,target+'\n'+info.lines.map(x=>'        '+JSON.stringify(x)+',').join('\n'));
 fs.writeFileSync(p,txt);
}
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const snap=[
 {lesson:'L08',file:'u01-l08.svg',description:'Four original right isosceles triangles rotate by 0, -90, -180 and -270 degrees around O(0,0), with OA=OB=3. Separate half-turn images of segment P(0,0), Q(4,0) about endpoint Q(4,0), midpoint M(2,0), off-line C(2,2).',data:{triangle:[{x:0,y:0},{x:3,y:0},{x:0,y:3}],turns:[0,-90,-180,-270],segment:[{x:0,y:0},{x:4,y:0}],centers:[{name:'endpoint',x:4,y:0},{name:'midpoint',x:2,y:0},{name:'off-line',x:2,y:2}]}},
 {lesson:'L09',file:'u01-l09.svg',description:'Horizontal parallel lines y=1 and y=4; a common translation by (2,3) maps them to y=4 and y=7; a half-turn about (0,0) maps them to y=-1 and y=-4; reflection in y=0 produces the same line equations but does not reverse x. Distance remains three.',data:{lines:[1,4],translation:{x:2,y:3},rotationCenter:{x:0,y:0},reflectionAxis:'y=0'}},
 {lesson:'L10',file:'u01-l10.svg',description:'Triangle A(0,0), B(1,2), C(5,2) and 180-degree image C(5,2), D(4,0), A(0,0) around M(2.5,1) form parallelogram ABCD. Original triangle area 4 and parallelogram area 8. Isosceles triangle with B(-1,0), C(1,0), A(0,sqrt(8)) has AB=AC=3.',data:{triangle:[{x:0,y:0},{x:1,y:2},{x:5,y:2}],midpoint:{x:2.5,y:1},isosceles:{base:[{x:-1,y:0},{x:1,y:0}],apex:{x:0,y:Math.sqrt(8)}}}}
];
const r=n=>Number(n.toFixed(5));
function rotate(p,degrees){const t=degrees*Math.PI/180;return{x:r(p.x*Math.cos(t)-p.y*Math.sin(t)),y:r(p.x*Math.sin(t)+p.y*Math.cos(t))};}
function half(p,c){return{x:2*c.x-p.x,y:2*c.y-p.y};}
function pix(p,o,s){return (r(o.x+p.x*s))+','+(r(o.y-p.y*s));}
function poly(id,vertices,o,s,color){return '<polygon id="'+id+'" points="'+vertices.map(p=>pix(p,o,s)).join(' ')+'" fill="'+color+'" fill-opacity=".13" stroke="'+color+'" stroke-width="3"/>';}
function line(id,p,q,o,s,color){return '<line id="'+id+'" x1="'+r(o.x+p.x*s)+'" y1="'+r(o.y-p.y*s)+'" x2="'+r(o.x+q.x*s)+'" y2="'+r(o.y-q.y*s)+'" stroke="'+color+'" stroke-width="3"/>';}
function title(x,y,txt,size=16){return '<text x="'+x+'" y="'+y+'" font-size="'+size+'" fill="#0f172a">'+txt+'</text>';}
function svg(scene){
 let shapes='';
 if(scene.lesson==='L08'){
 const data=scene.data,o={x:223,y:323},s=55;
 data.turns.forEach((d,i)=>{shapes+=poly('quarter-'+i,data.triangle.map(p=>rotate(p,d)),o,s,['#2563eb','#be123c','#16a34a','#7e22ce'][i]);});
 shapes+=title(40,155,'Four 90° clockwise quarter-turns around O')+title(232,326,'O');
 const origin={x:600,y:395},sc=35;
 shapes+=line('original-segment',...data.segment,origin,sc,'#2563eb');
 data.centers.forEach((c,i)=>{let p=data.segment.map(v=>half(v,c));shapes+=line('half-'+c.name,p[0],p[1],origin,sc,['#be123c','#16a34a','#7e22ce'][i]);});
 shapes+=title(490,155,'180° segment rotations: three centers')+title(480,493,'Red endpoint; green midpoint; purple off-line',13);
 }
 if(scene.lesson==='L09'){
 const mapCases=[{name:'translate (+2,+3)',fn:p=>({x:p.x+2,y:p.y+3}),col:'#be123c'},{name:'rotate 180° about O',fn:p=>({x:-p.x,y:-p.y}),col:'#16a34a'},{name:'reflect across y=0',fn:p=>({x:p.x,y:-p.y}),col:'#7e22ce'}];
 mapCases.forEach((v,i)=>{const o={x:142+i*297,y:365},sc=28,ys=[1,4];
 ys.forEach((y,j)=>{const a={x:-3,y},b={x:4,y};shapes+=line('source-'+i+'-'+j,a,b,o,sc,'#2563eb');shapes+=line('image-'+i+'-'+j,v.fn(a),v.fn(b),o,sc,v.col);});
 shapes+=title(30+i*297,175,v.name,16)+title(30+i*297,492,'Parallel; distance = 3',14);
 });
 }
 if(scene.lesson==='L10'){
 const a=scene.data.triangle,c=scene.data.midpoint,o={x:115,y:390},s=65;
 const second=a.map(p=>half(p,c));
 shapes+=poly('triangle-original',a,o,s,'#2563eb')+poly('triangle-halfturn',second,o,s,'#be123c');
 shapes+=title(35,155,'ABC and its 180° image CDA')+title(275,330,'M')+title(40,493,'Area triangle = 4; parallelogram = 8',14);
 const iso=scene.data.isosceles,oo={x:690,y:395},ss=75;
 shapes+=poly('isosceles-original',[...iso.base,iso.apex],oo,ss,'#16a34a');
 shapes+=title(520,155,'Isosceles: equal legs 3, base 2')+title(680,475,'AB = AC = 3',15);
 }
 return '<svg xmlns="http://www.w3.org/2000/svg" width="960" height="560" viewBox="0 0 960 560" role="img" aria-labelledby="title desc"><title id="title">Original Grade 8 U01 '+scene.lesson+' geometry representations</title><desc id="desc">'+scene.description+'</desc><rect width="960" height="560" fill="#fff"/><rect x="16" y="95" width="925" height="440" rx="12" fill="#f8fafc" stroke="#cbd5e1"/>'+title(25,43,'U01 '+scene.lesson+' — independent first-party geometry',24)+shapes+'</svg>\n';
}
const figures=snap.map(s=>{
 const p='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/'+s.file;
 fs.writeFileSync(path.join(root,p),svg(s));
 return {lessonCode:'PED:US-CCSS-MATH:G8:U01:'+s.lesson,sourceStudentUrl:'https://im.kendallhunt.com/MS/students/3/1/'+Number(s.lesson.slice(1))+'/index.html',sourceTeacherUrl:'https://im.kendallhunt.com/MS/teachers/3/1/'+Number(s.lesson.slice(1))+'/index.html',assetPath:'/curriculum/us-ccss-g8/figures/'+s.file,sha256:sha(p),description:s.description,data:s.data,sourceParityAccepted:false,authenticatedStudentRendered:false,independentMathTestsRequired:true};
});
const hashfiles=[additions.L08.file,additions.L09.file],hashes={};for(const f of hashfiles)hashes[path.basename(f)]=sha(f);
const evidence={schemaVersion:1,method:'Public source lesson activity review (U01:L08-L10) with first-party mathematical representations. Does not verify authenticated rendering, teacher scoring, card-sort/GeoGebra equivalence, or publisher art.',sourceEdition:'IM/Open Up Resources 2017–2019',status:'SOURCE_ALIGNED_CANDIDATE_NOT_A_D_ACCEPTED',lessons:Object.keys(additions).map(id=>({lessonCode:'PED:US-CCSS-MATH:G8:U01:'+id,addedTeacherStudentActivityPrompts:additions[id].lines,fullSourceParityAccepted:false,teacherRubricPersisted:false,interactiveConstructionImplemented:false})),figures,updatedRichCandidateHashes:hashes,approval:{A:0,B:0,C:0,D:0}};
const out='docs/curriculum/us-ccss-g8-coder4-v13-u01-source-geometry-evidence-20261029.json';
fs.writeFileSync(path.join(root,out),JSON.stringify(evidence,null,2)+'\n');
const course='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
let text=fs.readFileSync(path.join(root,course),'utf8');
for(const [file,hash] of Object.entries(hashes)){let re=new RegExp('("'+file.replace(/[.*+?^${}()|[\]\\]/g,'\\$&')+'": ")[a-f0-9]{64}(")');if(!re.test(text))throw Error('Missing hash '+file);text=text.replace(re,'$1'+hash+'$2');}
fs.writeFileSync(path.join(root,course),text);
console.log('Updated academic lessons',Object.keys(additions),'candidate figures',figures.map(f=>f.assetPath),'updated hashes',hashes);
