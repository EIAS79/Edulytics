
'use strict';
const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const file='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json';
const lessonCode='PED:US-CCSS-MATH:G8:U03:L12';
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const additions=[
 'Source activity 12.1: three equal-area candidate square diagrams illustrate shading as fractions, not linear equations. Independent first-party fraction model: A and B each shade 12 of 16 equal cells (3/4) while C shades 13 of 16 (13/16), so C is larger. The source original square diagrams are not reproduced.',
 'Source activity 12.2: apples cost $1 and oranges $2 each. The four sample purchases (6,3), (4,4), (5,4) and (8,2) cost $12, $12, $13 and $12, respectively. A $10 budget gives a+2o=10 with a,o whole nonnegative; the six exact-cost combinations are (10,0), (8,1), (6,2), (4,3), (2,4), (0,5). (7,2) costs 11 and must be rejected. The graph of the algebraic line also includes fractional points, which do not necessarily correspond to buyable whole fruit.',
 'Source activity 12.3: for two unrestricted real numbers x,y, the statement double the first and add the second gives 2x+y=10. Solutions include (0,10), (1,8), (5,0), (−3,16), (7,−4), (3.5,3); non-solutions include (1,7), (2,4), (5,1). Graph and label several points in both sets, explain that all real solutions lie on one line, and distinguish a continuous line from the finite nonnegative integer purchase possibilities.',
 'Teacher-scored contract: student-generated labeled coordinate graph with at least five valid and ten invalid ordered pairs, shaded-fraction rationale, budget units, and a written explanation of why a point on the real line is or is not an allowed whole-fruit purchase. A correct numeric yes/no ordered-pair item alone does not demonstrate these graphical, domain, or teacher-review outcomes. Supervisor 2 owns shared Practice PR #597 (source-matched Ax+By=C membership); no shared generator or renderer changes are made here.'
];
let data=fs.readFileSync(path.join(root,file),'utf8');
const pack=JSON.parse(data),l=pack.Lessons.find(x=>x.LessonCode===lessonCode);
if(!l)throw Error('Missing U03 L12');
if(!data.includes(additions[0])){
 const anchor='        '+JSON.stringify(l.ExplanationParagraphs[1])+',';
 if(!data.includes(anchor))throw Error('Missing unique lesson anchor');
 data=data.replace(anchor,anchor+'\n'+additions.map(x=>'        '+JSON.stringify(x)+',').join('\n'));
 fs.writeFileSync(path.join(root,file),data);
}
const origin=(o,s,p)=>[Number((o[0]+p[0]*s).toFixed(5)),Number((o[1]-p[1]*s).toFixed(5))];
const tx=(x,y,s)=>'<text x="'+x+'" y="'+y+'" font-size="13" fill="#0f172a">'+s+'</text>';
let body='';
const colors=['#2563eb','#15803d','#9333ea'];
for(let g=0;g<3;g++){
 const x=35+g*87,y=160,cell=13,n=g===2?13:12;
 for(let k=0;k<16;k++)body+='<rect id="shade-'+g+'-'+k+'" x="'+(x+cell*(k%4))+'" y="'+(y+cell*Math.floor(k/4))+'" width="'+cell+'" height="'+cell+'" stroke="#64748b" stroke-width=".5" fill="'+(k<n?colors[g]:'#fff')+'"/>';
 body+=tx(x,245,`${['A','B','C'][g]}: ${n}/16`);
}
body+=tx(45,290,'Source fraction idea: 13/16 > 12/16');
function drawAxes(prefix,o,s,xs,ys){
 for(let i=0;i<=xs;i++){const v=origin(o,s,[i,0]);body+='<line x1="'+v[0]+'" y1="'+(o[1]-ys*s)+'" x2="'+v[0]+'" y2="'+o[1]+'" stroke="#e2e8f0"/>';}
 for(let j=0;j<=ys;j++){const v=origin(o,s,[0,j]);body+='<line x1="'+o[0]+'" y1="'+v[1]+'" x2="'+(o[0]+xs*s)+'" y2="'+v[1]+'" stroke="#e2e8f0"/>';}
}
const budgetO=[330,475],budgetS=20;
drawAxes('fruit',budgetO,budgetS,10,5);
const fruit=[0,1,2,3,4,5].map(o=>[10-2*o,o]);
const budgetLine=[origin(budgetO,budgetS,[0,5]),origin(budgetO,budgetS,[10,0])];
body+='<line id="fruit-budget-line" x1="'+budgetLine[0][0]+'" y1="'+budgetLine[0][1]+'" x2="'+budgetLine[1][0]+'" y2="'+budgetLine[1][1]+'" stroke="#94a3b8" stroke-dasharray="5 4" stroke-width="2"/>';
fruit.forEach((p,i)=>{const [x,y]=origin(budgetO,budgetS,p);body+='<circle id="fruit-valid-'+i+'" cx="'+x+'" cy="'+y+'" r="5" fill="#15803d"/>';});
body+=tx(326,300,'a+2o=10: six whole-fruit pairs')+tx(327,512,'a = apples, o = oranges');
const realO=[665,480],realS=18,valid=[[-3,16],[0,10],[1,8],[2,6],[3,4],[3.5,3],[5,0],[7,-4]],invalid=[[1,7],[2,4],[5,1]];
const a=origin(realO,realS,[-3,16]),b=origin(realO,realS,[7,-4]);
body+='<line id="real-solution-line" x1="'+a[0]+'" y1="'+a[1]+'" x2="'+b[0]+'" y2="'+b[1]+'" stroke="#2563eb" stroke-width="3"/>';
valid.forEach((p,i)=>{const [x,y]=origin(realO,realS,p);body+='<circle id="real-valid-'+i+'" cx="'+x+'" cy="'+y+'" r="4" fill="#15803d"/>';});
invalid.forEach((p,i)=>{const [x,y]=origin(realO,realS,p);body+='<circle id="real-invalid-'+i+'" cx="'+x+'" cy="'+y+'" r="5" fill="#b91c1c"/>';});
body+=tx(635,300,'2x+y=10: real-valued line')+tx(634,522,'Green valid; red invalid');
const desc='Original Edulytiks three-panel illustration of source lesson 8.3.12 concepts. Left shaded fractions A 12/16, B 12/16, C 13/16. Middle finite nonnegative integer apples and oranges solutions to a+2o=10, with apples on horizontal axis. Right line 2x+y=10 with several real-number satisfying green points and red nonsolutions. Graphs are static explanatory assets, not a student-constructed or machine-graded graph. Publisher diagrams not copied.';
const svg='<svg xmlns="http://www.w3.org/2000/svg" width="930" height="590" viewBox="0 0 930 590" role="img" aria-labelledby="title desc"><title id="title">Grade 8 solutions to two-variable equations: domains and graphs</title><desc id="desc">'+desc+'</desc><rect width="930" height="590" fill="white"/><rect x="14" y="102" width="902" height="455" rx="8" fill="#f8fafc" stroke="#cbd5e1"/><text x="25" y="44" font-size="23" fill="#0f172a">U03 L12 — exact solutions and domain restrictions</text>'+body+'</svg>\n';
const asset='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/u03-l12-source-context.svg';
if(fs.existsSync(path.join(root,asset)) && !fs.readFileSync(path.join(root,asset),'utf8').includes('Original Edulytiks'))throw Error('Potential overwrite');
fs.writeFileSync(path.join(root,asset),svg);
const evidence={schemaVersion:1,lessonCode,sourceStudentUrl:'https://im.kendallhunt.com/MS/students/3/3/12/index.html',sourceTeacherUrl:'https://im.kendallhunt.com/MS/teachers/3/3/12/index.html',activityTitles:['12.1: Estimate Area','12.2: Apples and Oranges','12.3: Solutions and Everything Else'],sourceTeacherRoleSpotReview:['BuildingOn:6.G.A.1','Addressing:8.EE.B','BuildingTowards:8.EE.C'],license:'Open Up Resources / IM 2017–2019 CC BY 4.0; no copied source graphics',sourceComparison:'PUBLIC_STUDENT_AND_TEACHER_ACTIVITY_SPOT_REVIEW_NOT_FULL_PARITY',
courseFixes:additions,graphMath:{fruitSolutions:fruit,fruitEquation:'a+2o=10 nonnegative integers',rejectedFruit:[7,2],fruitCost:11,realEquation:'2x+y=10',validReal:valid,invalidReal:invalid,initialShading:[12/16,12/16,13/16]},courseSvg:{assetPath:'/curriculum/us-ccss-g8/figures/u03-l12-source-context.svg',sha256:sha(asset),authenticatedStudentRendered:false},
existingCourseSvgPreserved:true,sharedPracticePR:{number:597,head:'b0026ed90720025789dfe7c3fdb5632e5c3ed59d',mergeVerified:false,selectedRuntimeFamilyVerified:false,studentGraphDeliveredBySharedPR:false},teacherRubricSaved:false,fullSourceParityAccepted:false,stages:{A:'NOT_ACCEPTED',B:'NOT_ACCEPTED',C:'NOT_VERIFIED',D:'NOT_VERIFIED'},updatedRichCandidateHash:sha(file)};
const evidencePath='docs/curriculum/us-ccss-g8-coder4-p0-u03-l12-source-graph-contract-20261030.json';
fs.writeFileSync(path.join(root,evidencePath),JSON.stringify(evidence,null,2)+'\n');
const coursePath='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
const course=JSON.parse(fs.readFileSync(path.join(root,coursePath),'utf8'));course.hashes.richCandidates[path.basename(file)]=evidence.updatedRichCandidateHash;
fs.writeFileSync(path.join(root,coursePath),JSON.stringify(course,null,2)+'\n');
console.log(JSON.stringify({lessonCode,updatedRichSha:evidence.updatedRichCandidateHash,svgSha:evidence.courseSvg.sha256,evidencePath},null,2));
