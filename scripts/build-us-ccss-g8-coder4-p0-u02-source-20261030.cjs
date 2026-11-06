
'use strict';
const fs=require('node:fs'), path=require('node:path'), crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const richRel='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json';
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const source=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json')));
const cross=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json')));
const lessonPrompts={
L01:[
 'Source activity 1.1, independent numeric check: 6 1/4 ÷ 2 = 25/8, 10 1/7 ÷ 5 = 71/35, and 8 1/2 ÷ 11 = 17/22. Explain how fractions or mixed numbers represent each quotient; the warm-up addresses fraction division before new scale factors.',
 'Source activity 1.2: repeatedly halve an 8.5-by-11-inch page, producing first-party dimensions A(8.5,11), B(5.5,8.5), C(4.25,5.5), D(2.75,4.25), E(2.125,2.75), each written (short,long). A,C,E are similar with scale factors 1, 1/2, 1/4; B,D form a different shape family. Stack at a shared corner and compare diagonals. Do not claim that every halved rectangle is similar to the original.',
 'Source activity 1.3: if rectangle R is split into four equal similar rectangles, B has 1/2 the length of each corresponding edge of R. If C is one of nine similar cells inside another quarter of R, C is 1/3 of B in length and 1/6 of R, so multiplying the two scale factors confirms the R-to-C relationship. Student sorting, measuring and reasoning need observation and teacher review.'
],
L02:[
 'Source activity 2.1 asks students to notice and wonder about concentric-circle grids: all circles have the same center, and the radial distance is independent of ray direction. An open observation is teacher-reviewed, not a numeric answer.',
 'Source activity 2.2 asks for four points on a small circle, their center-to-point rays, intersections with a larger circle and a measured S/L table. In an independent example with radii 2 and 5, every corresponding distance quotient L/S is 5/2, not a direction-dependent scale factor. The original GeoGebra point/ray/intersection/distance tools have not been integrated into Edulytiks.',
 'Source activities 2.3 and 2.4: trace every quadrilateral vertex along its ray from the common center and use one constant factor for the entire polygon. Preserve labels and compare corresponding edges. Teacher review is required for a student-constructed polygon, rather than reporting that a static RatioTable is interactive.'
],
L03:[
 'Source activity 3.1 constructs points twice and half as far along a ray from its starting point. On original ray A(0,0), B(2,0), use C(4,0) and D(1,0); C must be beyond B, D between A and B.',
 'Source activity 3.2 asks which labeled point is an image after a dilation and which chosen point can serve as center; reverse maps use reciprocal scale factors, and dilation with factor 1 maps any point to itself. On an independent example A(0,0), B(2,0), factor 5 gives B′(10,0).',
 'Source activity 3.3 uses a dilation applet without a grid: constructing projected rays and distances must be student-demonstrated and teacher-checked or implemented as an accessible drawing task; a single numeric distance is not an equivalent graphical activity.'
],
L04:[
 'Source activity 4.1 requires estimating scale factor from points A, B, C on one ray; the estimate must be the quotient AC/AB rather than a guess or the ratio BC/AB.',
 'Source activity 4.2: an original coordinate reconstruction of the stated rectangle A(2,9), B(8,9), C(8,5), D(2,5), center P(3,7), factor 2 gives A′(1,11), B′(13,11), C′(13,3), D′(1,3). Explicitly use P+2(X−P); multiplying raw coordinates by 2 would be wrong because P is not the origin.',
 'Source activity 4.3 asks students to match original and image figures in a card sort and explain the center/scale factor for each. A static original diagram illustrates one valid case but is not a working accessible card-sort submission or teacher mark.'
],
L05:[
 'Source activity 5.1 varies one dilation factor through an interactive triangular construction. A first-party triangle with side 14 has image side 7 when k=1/2 and side 28 when k=2; all corresponding angles stay fixed, while triangle area scales by k squared.',
 'Source activity 5.2 is a two-person information gap: one partner has a problem card and the other has data and must supply requested values only. Learners must justify which center, original length and factor are needed; a fabricated automatically graded conversation would not verify the source communication task.'
],
L06:[
 'Source activity 6.1 uses equivalent expressions as a warm-up. For an independent check, 10(2+3)−8·3=26, also 50−24 and 2·13, using distributive or arithmetic identities without changing the expression value.',
 'Source activities 6.2 and 6.3 require describing actual sequences of dilations and rigid moves that transform corresponding labeled triangles and polygons; one equal side ratio without angle/correspondence checks does not prove similarity for a general quadrilateral.',
 'Source activity 6.4 asks for methods describing translations and dilations, which are not interchangeable: a translation applies one vector and a dilation multiplies center-to-vertex distances by the same positive factor. A student-created overlay and proof require a visual/teacher check.'
],
L07:[
 'Source activity 7.1 classifies propositions as all, some or none: every congruent pair is similar (all); a similar pair is congruent only when its scale factor is 1 (some); dilation centered at an angle vertex does not change the angle measure (none).',
 'Source activity 7.2 provides a square with side 2 and rhombus with all sides 2 but unequal 60°/120° angles: they are not similar. Rectangles 4-by-2 and 6-by-4 have different aspect ratios, so no single uniform scale maps one to the other. Adding 2 units or independently using factors 1.5 and 2 does not constitute a dilation.',
 'Source activity 7.3 is a partner search for similar polygons; match all angles and one consistent set of corresponding side quotients, with independent student labels. Such open comparison needs teacher review.'
],
L08:[
 'Source activity 8.1 asks for distinct expressions using the supplied numbers 4, −2, 10 to obtain 20, and comparisons are teacher-reviewed; equivalent expression creation is not a fixed multiple-choice test.',
 'Source activity 8.2 creates pasta triangles with one given angle and then with all three angles. Equal one angle alone does not guarantee similarity; agreeing in two independent corresponding angles establishes triangle similarity by AA. Measuring and comparing physical triangles is a human-observed construction.',
 'Source activity 8.3 asks students to reason about similar figures in a regular pentagon using equal angle measures and corresponding sides. Preserve geometric labels and check the actual correspondence, not a statement that any pentagon is similar to any other.'
],
L09:[
 'Source activity 9.1 compares side sets (2,3,4) and (4,5,6): factors 2, 5/3 and 3/2 differ, so the two triangles are not similar despite both having three listed lengths.',
 'Source activity 9.2 takes the original side triple (4,5,7) and scale factors 2, 3 and 1/2 to obtain (8,10,14), (12,15,21), and (2,2.5,3.5). Within each triangle the long/short quotient is 7/4, the long/medium is 7/5, and medium/short is 5/4, independent of scale.',
 'Source activity 9.3 requires identifying corresponding sides consistently before solving an unknown quotient. An answer that swaps source/image or short/long denominators is incorrect even if a coincidental number matches.'
],
L10:[
 'Source activity 10.1 invites equivalent forms for 15÷12, such as 5/4 and 1.25; a single decimal is not the only mathematical representation.',
 'Source activity 10.2 draws three right triangles on the same line with (run,rise) values (4,3), (8,6), and (2,1.5). Their common slope is 3/4 because equal acute angles make the triangles similar. A static coordinate example alone cannot verify a learner has constructed the assigned pair.',
 'Source activities 10.3 and 10.4 contrast lines with equal and unequal slopes. Two lines with slope 3 but different intercepts are parallel, a slope 1/2 line is less steep, and a vertical line has undefined slope because run=0, not slope 0.'
],
L11:[
 'Source activity 11.1 uses a point on a line through A(0,2), D(4,7), with vertical rise 5 over horizontal run 4. For E above x=2, E=(2,4.5) and segment BE from B(2,2) has length 2.5; justify with a similar slope triangle, not visual guessing.',
 'Source activity 11.2 checks the line through origin, (4,3) and (8,6), whose equation is y=3x/4: (20,15) and (100,75) lie on the line; (90,68) does not because 3·90/4=67.5. Treat wrong values as rejected examples.',
 'Source activity 11.3 asks learners to derive a linear equation from similar slope triangles and to explain how every point on the entire line satisfies it; automatic evaluation of a single coordinate is only partial assessment.'
],
L12:[
 'Source activity 12.1: a positive dilation of scale 2 taking A to B has center C on their common line with C=2A−B. In an independent coordinate example A(2,1), B(5,4), the unique center is C(−1,−2). Do not treat that original coordinate example as the source image.',
 'Source activity 12.2: the source line through (5,3) and (7,7) has slope 2 and satisfies y−3=2(x−5), or y=2x−7. Both (9,11) and (100,193) are on the line; (9,10) is an intentionally wrong point. Derive through a slope triangle, not only by reading a y-intercept omitted from the source diagram.',
 'Source activity 12.3: for original triangle A(0,1), B(2,1), C(2,2), dilation centered at A with factor s maps C to (2s,1+s). Factor 2.5 sends C to (5,3.5), and factor 4.5 sends C to (9,5.5). Students must construct and label their image triangles; a static SVG is not an interactive dilation tool.'
],
L13:[
 'Source activity 13.1 contrasts nearby-lamp shadows, whose ray angles can differ, with parallel rays from distant sunlight. Do not infer equal ratios from a point light and objects at different distances.',
 'Source activity 13.2 provides rounded field measurements (height in inches, shadow length in inches): (43,29), (72,48), (51,34), and lamp shadow 114. The first ratio 43/29≈1.48276 differs from 72/48=51/34=1.5. Predict lamp height approximately 114·1.5=171 inches, not as an exactly known measured height.',
 'Source activities 13.3 and 13.4 ask students to justify the proportional model with parallel sun rays, right triangles and same-angle correspondence, then estimate a tall object. Real data rounded to whole inches must be treated as approximately proportional; teacher reviews the physical model and reasoning.'
]
};
let text=fs.readFileSync(path.join(root,richRel),'utf8');
const pack=JSON.parse(text),changed=[];
for(const [id,ps] of Object.entries(lessonPrompts)){
 const l=pack.Lessons.find(x=>x.LessonCode.endsWith(':'+id));if(!l)throw Error('Missing lesson '+id);
 if(text.includes(ps[0]))continue;
 const anchor='        '+JSON.stringify(l.ExplanationParagraphs[1])+',';
 if(!text.includes(anchor))throw Error('No unique anchor '+id);
 text=text.replace(anchor,anchor+'\n'+ps.map(x=>'        '+JSON.stringify(x)+',').join('\n'));
 changed.push(id);
}
fs.writeFileSync(path.join(root,richRel),text);
const updatedHash=sha(richRel);
const paths=[];
const n=x=>Number(x.toFixed(5));
const esc=x=>String(x).replace(/&/g,'&amp;').replace(/</g,'&lt;');
function svg(name,description,body){return '<svg xmlns="http://www.w3.org/2000/svg" width="920" height="600" viewBox="0 0 920 600" role="img" aria-labelledby="title desc"><title id="title">'+esc(name)+'</title><desc id="desc">'+esc(description)+'</desc><rect width="920" height="600" fill="#fff"/><rect x="10" y="78" width="900" height="502" fill="#f8fafc" stroke="#cbd5e1"/><text x="25" y="40" font-size="22" fill="#0f172a">'+esc(name)+'</text>'+body+'</svg>\n';}
function pathRect(id,w,h,x,y,s,col){return '<rect id="'+id+'" x="'+n(x)+'" y="'+n(y-h*s)+'" width="'+n(w*s)+'" height="'+n(h*s)+'" fill="'+col+'" fill-opacity=".14" stroke="'+col+'" stroke-width="2"/>';}
function line(id,a,b,c){return '<line id="'+id+'" x1="'+n(a[0])+'" y1="'+n(a[1])+'" x2="'+n(b[0])+'" y2="'+n(b[1])+'" stroke="'+c+'" stroke-width="2.5"/>';}
function txt(x,y,t){return '<text x="'+x+'" y="'+y+'" font-size="15" fill="#0f172a">'+esc(t)+'</text>';}
const graphics=[
 {id:'L01',name:'Rectangle scaling from a 8.5-by-11 page',desc:'Independent diagram with rectangles A (8.5,11), C (4.25,5.5), E (2.125,2.75) aligned at the same lower left corner with shared diagonal, and B (5.5,8.5), D (2.75,4.25) forming a distinct second diagonal family. Source activity requires student sorting, measuring and explanation. Static candidate diagram, not publisher artwork.',kind:'rectangles'},
 {id:'L04',name:'Dilation factor 2 around nonzero center P (3,7)',desc:'First-party coordinate quadrilateral A(2,9), B(8,9), C(8,5), D(2,5) dilates about P(3,7), factor two, to A prime(1,11), B prime(13,11), C prime(13,3), D prime(1,3). Source drawing and card sort are not automatically implemented.',kind:'dilation'},
 {id:'L12',name:'Slope triangle and dilation on coordinate plane',desc:'Independent two-panel drawing. Left line through (5,3), (7,7), (9,11) has slope two and equation y=2x-7; (9,10) is a negative candidate not on the line. Right triangle A(0,1),B(2,1),C(2,2) scaled by 2.5 about A gives A prime(0,1),B prime(5,1),C prime(5,3.5). Image is not interactive student work.',kind:'slope'},
 {id:'L13',name:'Sunlight shadow height approximation',desc:'Original two similar-right-triangle model: 72-inch person casts 48-inch shadow; 171-inch estimated lamppost casts 114-inch shadow. Both modeled ratios are 1.5; field data 43/29 differs due to measured rounding and physical approximation. Diagram is not a verified real image or teacher observation.',kind:'shadow'}
];
const points=(coords,o,scale)=>coords.map(p=>[n(o[0]+p[0]*scale),n(o[1]-p[1]*scale)]);
const polygon=(id,coords,o,scale,color)=>'<polygon id="'+id+'" points="'+points(coords,o,scale).map(p=>p.join(',')).join(' ')+'" fill="'+color+'" fill-opacity=".14" stroke="'+color+'" stroke-width="3"/>';
for(const g of graphics){
 let b='';
 if(g.kind==='rectangles'){
  const family1=[['A',8.5,11],['C',4.25,5.5],['E',2.125,2.75]],family2=[['B',5.5,8.5],['D',2.75,4.25]],s=22;
  for(const [id,w,h] of family1)b+=pathRect('rect-'+id,w,h,105,455,s,'#2563eb');
  for(const [id,w,h] of family2)b+=pathRect('rect-'+id,w,h,553,455,s,'#b91c1c');
  b+=line('family-ACE-diagonal',[105,455],[105+8.5*s,455-11*s],'#2563eb')+line('family-BD-diagonal',[553,455],[553+5.5*s,455-8.5*s],'#b91c1c');
  b+=txt(90,505,'A, C, E: scale 1, 1/2, 1/4')+txt(518,505,'B, D: other aspect ratio');
 }
 if(g.kind==='dilation'){
  const o=[180,510],s=28,orig=[[2,9],[8,9],[8,5],[2,5]],img=[[1,11],[13,11],[13,3],[1,3]];
  for(let i=0;i<15;i++)b+=line('grid-x-'+i,[o[0]+i*s,180],[o[0]+i*s,535],'#e2e8f0');
  for(let i=0;i<13;i++)b+=line('grid-y-'+i,[160,o[1]-i*s],[570,o[1]-i*s],'#e2e8f0');
  b+=polygon('source-quad',orig,o,s,'#2563eb')+polygon('dilated-quad',img,o,s,'#b91c1c');
  const p=points([[3,7]],o,s)[0];b+='<circle id="center-P" cx="'+p[0]+'" cy="'+p[1]+'" r="6" fill="#16a34a"/>';
  b+=txt(590,205,'P=(3,7), k=2')+txt(590,250,'A prime=(1,11)')+txt(590,280,'B prime=(13,11)')+txt(590,310,'C prime=(13,3)')+txt(590,340,'D prime=(1,3)');
 }
 if(g.kind==='slope'){
  const o=[60,490],s=33,ln=[[4,1],[9.5,12]];b+=line('line-y=2x-7',points([ln[0]],o,s)[0],points([ln[1]],o,s)[0],'#2563eb');
  const on=[[5,3],[7,7],[9,11]],off=[[9,10]];
  for(const [id,x,y] of on.map((p,i)=>['on-'+i,...p])){const p=points([[x,y]],o,s)[0];b+='<circle id="'+id+'" cx="'+p[0]+'" cy="'+p[1]+'" r="5" fill="#15803d"/>';}
  for(const p of off){const a=points([p],o,s)[0];b+='<circle id="off-line" cx="'+a[0]+'" cy="'+a[1]+'" r="5" fill="#b91c1c"/>';}
  b+=txt(40,520,'Blue y=2x-7; red (9,10) is off line');
  const origin=[625,440],sc=36;
  b+=polygon('triangle-source',[[0,1],[2,1],[2,2]],origin,sc,'#2563eb')+polygon('triangle-scaled',[[0,1],[5,1],[5,3.5]],origin,sc,'#b91c1c');
  b+=txt(595,526,'Center=(0,1); factor 2.5');
 }
 if(g.kind==='shadow'){
  const people=[{id:'person',x:70,base:480,shadow:48,height:72,s:2},{id:'lamppost',x:470,base:480,shadow:114,height:171,s:1.3}];
  for(const p of people){
   const h=p.height*p.s,w=p.shadow*p.s;
   b+=line(p.id+'-height',[p.x,p.base],[p.x,p.base-h],'#2563eb')+line(p.id+'-shadow',[p.x,p.base],[p.x+w,p.base],'#15803d')+line(p.id+'-sunray',[p.x,p.base-h],[p.x+w,p.base],'#b91c1c');
   b+=txt(p.x,p.base+30,p.id+' height '+p.height+'; shadow '+p.shadow+' inches');
  }
  b+=txt(80,540,'All heights are estimates; 43/29 differs from 1.5.');
 }
 const assetName='u02-'+g.id.toLowerCase()+(g.id==='L04'?'-off-origin':'')+'.svg';
 const file='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/'+assetName;
 if(fs.existsSync(path.join(root,file)) && !assetName.includes('-off-origin')){
   const stored=fs.readFileSync(path.join(root,file),'utf8');
   if(!stored.includes('Independent') && !stored.includes('first-party') && !stored.includes('Original')) throw Error('Refuse potential pre-existing asset overwrite: '+file);
 }
 fs.writeFileSync(path.join(root,file),svg(g.name,g.desc,b));
 paths.push({lessonCode:'PED:US-CCSS-MATH:G8:U02:'+g.id,assetPath:'/curriculum/us-ccss-g8/figures/'+assetName,sha256:sha(file),desc:g.desc,kind:g.kind,studentRenderedAuthenticated:false});
}
const by=x=>new Map(x.map(a=>[a.lessonCode,a]));
const S=by(source.rows),C=by(cross.rows);
const evidence={schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',sourceEdition:'IM / Open Up Resources 2017–2019 via Kendall Hunt',kind:'U02 public student lesson activity read and source-specific first-party candidate changes, NOT full parity or teacher-source-certified review',sourceLessonStudentPagesOpened:13,teacherSourcePagesIndividuallyOpened:[1,4,7,10,12,13],reviewedLessons:Object.entries(lessonPrompts).map(([id,paragraphs])=>{
 const lessonCode='PED:US-CCSS-MATH:G8:U02:'+id,s=S.get(lessonCode),c=C.get(lessonCode);
 return {lessonCode,studentUrl:s.sourceStudentUrl,teacherUrl:s.sourceTeacherUrl,sourceActivities:s.activities.map(x=>x.title),sourceActivityCount:s.studentActivityCount,sourceStatus:'PUBLIC_STUDENT_SOURCE_PAGE_READ_ACTIVITY_ALIGNED_CANDIDATE_NOT_COMPLETE_TEACHER_ACTIVITY_PARITY',teacherRoleEvidenceLevel:[1,4,7,10,12,13].includes(Number(id.slice(1)))?'TEACHER_ROLE_SPOT_READ':'EARLIER_TEACHER_ROLE_CROSSWALK_ONLY',blueprintExtraRoles:c.blueprintExtraRoles,sourceRoles:c.sourceRoles,roleIssueAcceptedByIndependentReviewer:false,authoredCourseAdditions:paragraphs,fullAcademicSourceParityAccepted:false,actualStudentTaskSubmitted:false,actualTeacherCoolDownAssessed:false};
}),figures:paths,sourceActivityReviewStatus:'PARTIAL',richFileSha256:updatedHash,sourceSpecialChecks:{L01:'Fraction division and partitioned rectangle similarity with alternating aspect ratios',L04:'Off-origin dilation factor two',L12:'Slope triangles and reciprocal center formula',L13:'Approximate height/shadow ratios with actual rounded data'},approvals:{A:0,B:0,C:0,D:0}};
const evid='docs/curriculum/us-ccss-g8-coder4-p0-u02-source-fixes-20261030.json';
fs.writeFileSync(path.join(root,evid),JSON.stringify(evidence,null,2)+'\n');
const courseRel='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
const course=JSON.parse(fs.readFileSync(path.join(root,courseRel),'utf8'));
course.hashes.richCandidates[path.basename(richRel)]=updatedHash;
fs.writeFileSync(path.join(root,courseRel),JSON.stringify(course,null,2)+'\n');
console.log(JSON.stringify({changed,richSha:updatedHash,svg:paths.map(x=>x.assetPath),evidence:evid},null,2));
