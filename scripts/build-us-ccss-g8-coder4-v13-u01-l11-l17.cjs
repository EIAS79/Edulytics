
'use strict';
const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const rich='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json';
const source=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json'),'utf8'));
const added={
'L11':[
'Source-aligned 11.1: compare left- and right-handed asymmetric tracings. A reflection reverses handedness, whereas translating or rotating a labeled tracing does not. Explain which orientation difference must be accounted for by the rigid mapping.',
'Source-aligned 11.2: inspect multiple shape pairs and distinguish size changes, altered angles, and reflection-plus-translation congruence. Similar-looking shapes with a changed angle or length cannot be established as congruent just by matching areas.',
'Source-aligned 11.3: compare rectangles R (2 by 3), S (1 by 6), and T (1 by 4). R and S have equal area 6 but perimeters 10 and 14; R and T have the same perimeter 10 but areas 6 and 4. Neither equal area nor equal perimeter alone proves congruence.'
],
'L12':[
'Source-aligned 12.1: draw multiple congruent triangle copies; select only those mapped by a translation. A translation preserves orientation and moves every labeled vertex through exactly the same vector, unlike reflected or rotated copies.',
'Source-aligned 12.2 and 12.3: for each polygon pair, test the whole labeled boundary, not just approximate shape. A different angle, different edge length, or change from convex to nonconvex is sufficient to reject congruence even when several vertices align.',
'Source-aligned 12.4: build an original quadrilateral and specify a reproducible sequence of permitted rigid moves mapping all corresponding points to a second figure; teacher evaluates the labeled construction and reasoning.'
],
'L13':[
'Source-aligned 13.1: after translating a trapezoid, track points that lie halfway or another fixed fraction along an edge, not only its four vertices. If P=A+t(D−A), then under a translation by vector v, P′=(A+v)+t(D−A).',
'Source-aligned 13.2: compare ovals whose long and short axes have the same dimensions but are turned 90°; orientation alone does not change congruence. A genuinely larger oval cannot be matched by a rigid motion.',
'Source-aligned 13.3 and 13.4: identify interior/edge point correspondences and explain congruence using actual geometric evidence. Points that are only close in a drawing are not necessarily corresponding.'
],
'L14':[
'Source-aligned 14.1: supplementary angles add to 180° and vertical angles are equal. If one angle of intersecting lines is 30°, each adjacent angle is 150°, while its vertical opposite also measures 30°.',
'Source-aligned 14.2: when a transversal meets two truly parallel lines and one acute intersection angle is 63°, alternate interior and corresponding acute angles also measure 63°, while their supplements measure 117°. In the deliberately nonparallel comparison diagram, equal alternate interior angles must not be assumed; a visually similar sketch is not a proof.',
'Source-aligned 14.3: identify the correct pair by locating both angles between the parallel lines on opposite sides of the transversal. A half-turn about the midpoint between intersection points can map the lines and prove equality, provided the lines are actually parallel.'
],
'L15':[
'Source-aligned 15.1: construct a 3-by-3 classification table of scalene, isosceles, and equilateral triangles versus acute, right, and obtuse angles. The inclusive isosceles definition means equilateral triangles have at least two equal sides; equilateral-right and equilateral-obtuse cases are impossible in Euclidean geometry.',
'Source-aligned 15.2: compare three differently labeled views of the same congruent triangle and reconcile all three interior angle measures before posting a combined result. Do not mistake an exterior angle for a triangle interior angle.',
'Source-aligned 15.3: cut out or draw three independent angle sectors, fit them along one straight line, and explain why their sum must equal 180°. Numeric substitution alone does not demonstrate the source construction.'
],
'L16':[
'Source-aligned 16.1: justify true/false using properties of operations: 62−28=34 differs from 60−30=30 (false); 3×(−8)=2×(−8)−8 (true); 16/(−2)+24/(−2)=40/(−2) (true). This warm-up is not interchangeable with a generic triangle-only question.',
'Source-aligned 16.2: rotate a triangle 180° about the midpoints of AC and AB to create a pair of exterior images. Angle EAB plus angle BAC plus angle CAD forms one straight angle; the other two pieces correspond to the original base angles.',
'Source-aligned 16.3 and 16.4: construct a line through the upper vertex parallel to the opposite triangle side, label equal alternate interior angles and explain why all three consecutive angles sum to 180°. For a quadrilateral, drawing a diagonal reduces its angle sum to two triangle sums (360°).'
],
'L17':[
'Source-aligned 17.1: test angular tilings around a shared vertex. Six equilateral 60° angles fit exactly; four right 90° angles fit exactly; three regular 108° pentagon angles leave 36° and four exceed 360°, so copies of a regular pentagon alone cannot meet at a single vertex without a gap or overlap.',
'Source-aligned 17.2: create a first-party periodic triangular or rectangular tiling and ask a partner to name an exact translation taking the full repeating pattern to itself. Verify adjacent edges match without overlaps and the chosen motion preserves tile dimensions.',
'Source-aligned 17.3: make a design with rotational symmetry and identify an actual nonzero rotation below 360° that maps the entire pattern, not merely one tile, to itself. Assess student drawing and explanation with a teacher rather than inventing numerical auto-grading.'
]};
let raw=fs.readFileSync(path.join(root,rich),'utf8');
const lessonData=JSON.parse(raw).Lessons;
for(const [id,lines] of Object.entries(added)){
 if(raw.includes(lines[0]))continue;
 const lesson=lessonData.find(l=>l.LessonCode.endsWith(':'+id));
 if(!lesson)throw Error('Missing lesson '+id);
 const anchor='        '+JSON.stringify(lesson.ExplanationParagraphs[1])+',';
 if(!raw.includes(anchor))throw Error('Missing anchor '+id);
 raw=raw.replace(anchor,anchor+'\n'+lines.map(x=>'        '+JSON.stringify(x)+',').join('\n'));
}
fs.writeFileSync(path.join(root,rich),raw);
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const pi=(p,o,s)=>[Number((o.x+p.x*s).toFixed(5)),Number((o.y-p.y*s).toFixed(5))];
const line=(id,a,b,o,s,col)=>{const x=pi(a,o,s),y=pi(b,o,s);return '<line id="'+id+'" x1="'+x[0]+'" y1="'+x[1]+'" x2="'+y[0]+'" y2="'+y[1]+'" stroke="'+col+'" stroke-width="3"/>';};
function create(id,desc,parts){return '<svg xmlns="http://www.w3.org/2000/svg" width="860" height="560" viewBox="0 0 860 560" role="img" aria-labelledby="title desc"><title id="title">Grade 8 U01 '+id+' original geometric proof</title><desc id="desc">'+desc+'</desc><rect width="860" height="560" fill="white"/><rect x="18" y="92" width="824" height="425" rx="10" stroke="#cbd5e1" fill="#f8fafc"/><text x="29" y="44" font-size="22" fill="#0f172a">U01 '+id+' — first-party coordinate model</text>'+parts+'</svg>\n';}
const fig=[];
{
 const id='L14',angle=63,m=Math.tan(angle*Math.PI/180),o={x:280,y:405},s=75;
 const ends=[{x:-3,y:0},{x:4,y:0},{x:-3,y:2},{x:4,y:2}];
 let parts=line('parallel-lower',ends[0],ends[1],o,s,'#2563eb')+line('parallel-upper',ends[2],ends[3],o,s,'#2563eb');
 const lo={x:-1,y:-m},hi={x:3,y:3*m};parts+=line('transversal',lo,hi,o,s,'#be123c');
 parts+='<text x="42" y="137" font-size="18" fill="#0f172a">63° acute angle, 117° supplementary; two parallel lines</text>';
 const description='Original model: horizontal parallel lines y=0 and y=2, red transversal y=tan(63 degrees)x, intersection at (0,0) and (2/tan(63 degrees),2). Matching acute alternate interior angles are 63 degrees; adjacent supplements are 117 degrees. This is not publisher art.';
 const p='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/u01-l14.svg';fs.writeFileSync(path.join(root,p),create(id,description,parts));
 fig.push({lessonCode:'PED:US-CCSS-MATH:G8:U01:L14',assetPath:'/curriculum/us-ccss-g8/figures/u01-l14.svg',sha256:sha(p),description,data:{angle,m,lowerY:0,upperY:2,origin:o,scale:s},authenticatedStudentRendered:false,sourceParityAccepted:false});
}
{
 const id='L16',triangle=[{x:0,y:0},{x:6,y:0},{x:2,y:3}],o={x:200,y:430},s=71;
 let parts=line('base',triangle[0],triangle[1],o,s,'#2563eb')+line('leg-a',triangle[0],triangle[2],o,s,'#2563eb')+line('leg-b',triangle[1],triangle[2],o,s,'#2563eb');
 parts+=line('parallel-through-C',{x:-1,y:3},{x:7,y:3},o,s,'#16a34a');
 parts+='<text x="35" y="138" font-size="18" fill="#0f172a">Transferred base angles + vertex angle = 180°</text>';
 const description='Original triangle A(0,0), B(6,0), C(2,3), with a line through C at y=3 parallel to base AB at y=0. Alternate interior angles at C equal the two base angles. Three consecutive angles at C lie on a straight 180-degree line. Static visual: teacher reviews construction and proof.';
 const p='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/u01-l16.svg';fs.writeFileSync(path.join(root,p),create(id,description,parts));
 fig.push({lessonCode:'PED:US-CCSS-MATH:G8:U01:L16',assetPath:'/curriculum/us-ccss-g8/figures/u01-l16.svg',sha256:sha(p),description,data:{triangle,upperY:3,baseY:0,origin:o,scale:s},authenticatedStudentRendered:false,sourceParityAccepted:false});
}
const sourceEvidence={schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',sourceEdition:'IM/Open Up Resources 2017–2019',scope:'U01 L11-L17 source student AND teacher content spot review, not full per-lesson academic acceptance',lessons:Object.entries(added).map(([id,lines])=>({lessonCode:'PED:US-CCSS-MATH:G8:U01:'+id,sourceStudentUrl:'https://im.kendallhunt.com/MS/students/3/1/'+Number(id.slice(1))+'/index.html',sourceTeacherUrl:'https://im.kendallhunt.com/MS/teachers/3/1/'+Number(id.slice(1))+'/index.html',sourceActivityTitles:source.rows.find(x=>x.lessonCode.endsWith('U01:'+id)).activities.map(x=>x.title),courseOwnedAdditions:lines,sourceParityAccepted:false,interactiveEquivalenceVerified:false,humanScorePersisted:false})),figures:fig,updatedRichCandidateHash:sha(rich),adoptionApproved:false,accepted:{A:0,B:0,C:0,D:0}};
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json'),JSON.stringify(sourceEvidence,null,2)+'\n');
const coursePath=path.join(root,'docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const course=JSON.parse(fs.readFileSync(coursePath));course.hashes.richCandidates[path.basename(rich)]=sha(rich);
fs.writeFileSync(coursePath,JSON.stringify(course,null,2)+'\n');
console.log('U01 L11-L17 updated, original SVGs:',fig.map(x=>x.assetPath),'new rich hash:',sha(rich));
