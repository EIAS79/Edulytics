
'use strict';
const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const target='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json';
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const source=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json')));
const prompts={
L01:[
 'Source activity 1.1 presents distance horizontally and elapsed time vertically: line g passes through (6 cm,3 s), line f through (8 cm,2 s), so slopes as labeled are 1/2 s/cm and 1/4 s/cm. The inverse speeds are 2 cm/s and 4 cm/s, respectively; do not call the vertical-to-horizontal slope itself a speed in cm/s.',
 'Source activities 1.2 and 1.3 follow position tick marks for a moving ladybug and ant and ask learners to compare rate across representations, including what doubling speed changes. A physical movement diagram and student explanation require observer or teacher assessment beyond a single quotient.'
],
L02:[
 'Source activity 2.1 supplies a graph through origin and (8,14): y=(7/4)x, with graph scale changed in a second coordinate frame. Points (8,14) and (16,28) share the constant quotient 7/4, so relabeling axes does not change the underlying relationship.',
 'Source activity 2.2 supplies a teacher-mediated twelve-card sort of proportional graphs; students identify which lines share proportionality constants and write equations. Source activity 2.3 compares graph scales. Neither a static sample chart nor the general sentence y=kx implements the card sort.'
],
L03:[
 'Source activity 3.1 multiplication warm-up includes 15·2=30, 15·0.5=7.5, 15·0.25=3.75 and 15·2.25=33.75.',
 'Source activity 3.2 says Jada takes 8 steps where Noah takes 10, and Jada 12 where Noah takes 15: if x is Jada steps and y Noah steps then y=(5/4)x; corresponding table points (0,0),(8,10),(12,15) lie on one ray through the origin. Source activity 3.3 uses private information cards and an explanation, which require partner interaction and human review.'
],
L04:[
 'Source activity 4.1 starts from proportional rule y=4.2x and asks the learner to invent a coherent context, label the table and graph and interpret x and y. A context must retain the units and constant rate rather than treating 4.2 as an arbitrary intercept.',
 'Source activity 4.2 compares wages of $8.40 per hour against $7 per hour. After 12 hours the earnings are $100.80 and $84, a $16.80 difference; the $150 target requires 150/8.4≈17.86 hours and 150/7≈21.43 hours. The first-party line comparison is a static explanatory diagram, not a verified student graph.'
],
L05:[
 'Source activity 5.1 divides mixed number 2 5/8 by 1/2, obtaining 21/4=5.25.',
 'Source activity 5.2 uses stacks of 6 cups at 15 cm and 12 cups at 23 cm. The slope is (23−15)/(12−6)=4/3 cm per additional cup; the original affine relationship is h=7+(4/3)n, not a proportional relation through the origin. Solving h=50 gives 32.25 cups, which is not a whole cup count; 33 whole cups reach or exceed 50 cm in this idealized model.',
 'Source activity 5.3 asks students to construct a graph and explain how the constant additive cup height 7 cm acts as the intercept and the added height per nested cup is the slope. An actual physical stack and graph still require human or interactive verification.'
],
L06:[
 'Source activity 6.1 displays a colored growing arrangement with yellow squares n², blue tiles 3n, and red tiles n in diagram n. At n=4 there are 16 yellow, 12 blue and 4 red; at n=10 there are 100, 30 and 10. The total n²+4n is not linear even though blue and red counts are linear.',
 'Source activities 6.2 and 6.3 require matching six situation cards to six graphs and interpreting slope/intercept in a summer-reading context. The learner must distinguish proportional y=mx from nonproportional y=mx+b and interpret the units of both parameters; static lesson text does not fulfill the source card interaction.'
],
L07:[
 'Source activity 7.1 compares which of three glasses holds more, a visual estimation that cannot be decided solely from height because cross-section matters. Source activity 7.2 adjusts the initial water level, adds identical objects and plots volume/depth data with varying intercept.',
 'For an independently chosen starting amount V0 and displacement per object d, the linear model is V(n)=V0+dn. In particular V(3)=V0+3d and V(7)=V0+7d. Since the source interactive initial water level is adjustable, there is no one globally correct fixed numerical intercept.',
 'Source activity 7.3 asks students to calculate the slope from multiple triangles on their experiment graph. Compare the ratios for two runs and explain why slope does not depend on the chosen two points on the line; the experiment and plotted graph require observable learner work.'
],
L08:[
 'Source activity 8.1 asks which graphed lines can be translations of line f. A translation keeps slope unchanged, whereas a changed slope cannot result from pure translation. The actual source diagram must be interpreted with lines labelled f,h,i,j,g and not a generic same-intercept equation.',
 'Source activity 8.2 compares Diego babysitting at $10/hour with savings y=10x from zero and y=10x+30 with $30 saved initially. The two lines have equal slope 10 and vertical displacement 30. Source activity 8.3 describes translating arbitrary lines; specifying a horizontal translation can also change the intercept unless the slope is zero.'
],
L09:[
 'Source activity 9.1 is an open odd-one-out among four differently sloped lines; several well-justified features may distinguish a line and teacher review is needed for the explanation.',
 'Source activity 9.2 models a $40 transportation card losing $2.50 each ride: B(r)=40−2.5r for nonnegative whole-number r from 0 to 16; B(0)=40, B(1)=37.5, B(2)=35 and B(16)=0. Using r=17 creates negative balance and is outside the original prepaid-card domain. Source activities 9.3 and 9.4 compare travel/payment contexts and signed rates, not just a generic negative-slope calculation.'
],
L10:[
 'Source activity 10.1 asks for examples of a/b=−2, a/b=2 and a−b=−2; rational values with b≠0 may be used, so the answer need not be a single integer pair.',
 'Source activity 10.2 plots (1,11) and (8,2). Its slope is (2−11)/(8−1)=−9/7, not +9/7. With the source extension, the required missing coordinates are k=5 for (k,2),(11,14), slope 2; k=7 for (1,k),(4,1), slope −2; k=11 for (3,5),(k,9), slope 1/2; k=5 for (−1,4),(−3,k), slope −1/2.',
 'Source activity 10.3 includes the design of connected slope segments. A drawn line and geometric explanation require a student graph or teacher-reviewed construction; testing the numerical value −9/7 alone is partial.'
],
L11:[
 'Source activity 11.1 groups different line pairs into odd-one-out cases. Source activity 11.2 asks students to plot ten points whose y-coordinate is −4 (horizontal line y=−4), and ten points whose x-coordinate is 3 (vertical line x=3); distinguish these equations from y=−4x and x+y=−4.',
 'The line y=−4 has slope zero, while the line x=3 has undefined slope because its run is zero; neither is an ordinary y=mx+b graph with finite m for the vertical line. Source activity 11.3 extends this distinction to geometry and same-perimeter contexts; pupil-generated graphs must be reviewed.'
],
L13:[
 'Source activity 13.1 asks for coordinate solutions to 6x=7y, 5x+3y=9 and y+5−x/3=7. Illustrations include (7,6), (0,3), and (6,4), respectively, which satisfy the equations when substituted.',
 'Source activities 13.2 and 13.3 examine overlapping solution lines and requested coordinates. A point on two crossing lines solves both equations; a graph alone is insufficient if the coordinates are only estimated by eye. Teacher checks labels and the full explanation, including whether a statement names a pair or just one coordinate.'
],
L14:[
 'Source activity 14.1 gives grapes at $2.39/lb and bananas at $0.59/lb with a $15 budget: 2.39g+0.59b=15, with g,b nonnegative real measured pounds. It also gives savings starting at $50 with $20 added weekly, y=50+20x for nonnegative whole weeks x.',
 'Source activity 14.2 presents five account lines with (slope,intercept) pairs a:(20,50), c:(−5,110), d:(0,30), e:(−10,80), h:(20,−80). Interpret negative initial balances as debts where appropriate rather than assume every model is an account starting positive. Source activity 14.3 uses a fish-related linear model whose actual numbers and interpretation must be matched before grading.'
]};
let raw=fs.readFileSync(path.join(root,target),'utf8'),pack=JSON.parse(raw),newIDs=[];
for(const [id,pp] of Object.entries(prompts)){
 const l=pack.Lessons.find(x=>x.LessonCode.endsWith(':'+id));if(!l)throw Error('No lesson '+id);
 if(raw.includes(pp[0]))continue;
 const anchor='        '+JSON.stringify(l.ExplanationParagraphs[1])+',';
 if(!raw.includes(anchor))throw Error('No paragraph anchor '+id);
 raw=raw.replace(anchor,anchor+'\n'+pp.map(p=>'        '+JSON.stringify(p)+',').join('\n'));
 newIDs.push(id);
}
fs.writeFileSync(path.join(root,target),raw);
const n=v=>Number(v.toFixed(5));
const point=(o,s,p)=>[n(o[0]+p[0]*s),n(o[1]-p[1]*s)];
const axis=(id,o,s,p,q,c)=>{const a=point(o,s,p),b=point(o,s,q);return '<line id="'+id+'" x1="'+a[0]+'" y1="'+a[1]+'" x2="'+b[0]+'" y2="'+b[1]+'" stroke="'+c+'" stroke-width="3"/>';};
const circle=(id,o,s,p,color)=>{const a=point(o,s,p);return '<circle id="'+id+'" cx="'+a[0]+'" cy="'+a[1]+'" r="5" fill="'+color+'"/>';};
const graph=(title,desc,content)=>'<svg xmlns="http://www.w3.org/2000/svg" width="920" height="580" viewBox="0 0 920 580" role="img" aria-labelledby="title desc"><title id="title">'+title+'</title><desc id="desc">'+desc+'</desc><rect width="920" height="580" fill="white"/><rect x="15" y="82" width="889" height="468" rx="10" fill="#f8fafc" stroke="#cbd5e1"/><text x="28" y="45" font-size="23" fill="#0f172a">'+title+'</text>'+content+'</svg>\n';
const figures=[];
{
 const o=[110,490],sx=23,sy=2;
 const rawPoint=p=>[n(o[0]+p[0]*sx),n(o[1]-p[1]*sy)];
 const line=(id,rate,col)=>{const a=rawPoint([0,0]),b=rawPoint([20,20*rate]);return '<line id="'+id+'" x1="'+a[0]+'" y1="'+a[1]+'" x2="'+b[0]+'" y2="'+b[1]+'" stroke="'+col+'" stroke-width="3"/>';};
 let b=line('wage-840',8.4,'#2563eb')+line('wage-700',7,'#be123c');
 for(const [id,y,col] of [['wage-at-12-high',100.8,'#2563eb'],['wage-at-12-low',84,'#be123c']]){
  const p=rawPoint([12,y]);b+='<circle id="'+id+'" cx="'+p[0]+'" cy="'+p[1]+'" r="6" fill="'+col+'"/>';
 }
 b+='<text x="580" y="200" font-size="17" fill="#0f172a">After 12 hours:</text><text x="580" y="242" font-size="16" fill="#0f172a">$100.80 versus $84.00</text><text x="580" y="281" font-size="16" fill="#0f172a">Difference = $16.80</text>';
 const file='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/u03-l04-wage-comparison.svg';
 const desc='Original first-party comparison of linear wages y=8.4x and y=7x for nonnegative worked hours. At x=12 lines give 100.8 and 84 dollars. No source graph artwork is copied and this static graph is not a student-completed or authenticated figure.';
 fs.writeFileSync(path.join(root,file),graph('U03 L04: two hourly wages',desc,b));
 figures.push({lessonCode:'PED:US-CCSS-MATH:G8:U03:L04',assetPath:'/curriculum/us-ccss-g8/figures/u03-l04-wage-comparison.svg',sha256:sha(file),model:{rates:[8.4,7],hours:12,totals:[100.8,84],origin:o,xscale:sx,yscale:sy},authenticatedStudentRendered:false});
}
{
 const o=[110,515],sx=15,sy=7.6,model=n=>7+(4/3)*n;
 let b=axis('cup-line',o,1,[0,0],[0,0],'#e2e8f0');
 const p=(n,h)=>[o[0]+n*sx,o[1]-h*sy];
 const a=p(0,7),c=p(35,model(35));
 b+='<line id="cup-height-line" x1="'+a[0]+'" y1="'+n(a[1])+'" x2="'+c[0]+'" y2="'+n(c[1])+'" stroke="#2563eb" stroke-width="3"/>';
 for(const [id,cups,cm] of [['six',6,15],['twelve',12,23],['thirty-two',32,7+4*32/3],['thirty-three',33,51]]){
  const pos=p(cups,cm);b+='<circle id="cup-'+id+'" cx="'+n(pos[0])+'" cy="'+n(pos[1])+'" r="5" fill="'+(id==='thirty-three'?'#15803d':'#b91c1c')+'"/>';
 }
 b+='<text x="625" y="210" font-size="18" fill="#0f172a">h = 7 + (4/3)n</text><text x="625" y="248" font-size="16" fill="#0f172a">6 cups = 15 cm</text><text x="625" y="281" font-size="16" fill="#0f172a">12 cups = 23 cm</text><text x="625" y="321" font-size="16" fill="#0f172a">At least 50 cm: 33 cups</text>';
 const file='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures/u03-l05-stacking-cups.svg';
 const desc='First-party analytic plot of 6 cups 15 cm and 12 cups 23 cm. Slope is four thirds centimeters per cup and intercept is seven centimeters. For modeled height at least 50 cm, 33 whole cups are required, because 32 cups are below 50 cm and 33 cups are 51 cm. Diagram is not sourced publisher pixels or physical classroom verification.';
 fs.writeFileSync(path.join(root,file),graph('U03 L05: stack height is affine',desc,b));
 figures.push({lessonCode:'PED:US-CCSS-MATH:G8:U03:L05',assetPath:'/curriculum/us-ccss-g8/figures/u03-l05-stacking-cups.svg',sha256:sha(file),model:{observations:[[6,15],[12,23]],intercept:7,slope:4/3,target:50,firstWholeCupsAtOrAboveTarget:33,origin:o,xscale:sx,yscale:sy},authenticatedStudentRendered:false});
}
const by=new Map(source.rows.map(x=>[x.lessonCode,x]));
const evidence={schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',sourceEdition:'IM/Open Up Resources 2017–2019',sourceMethod:'Directly opened student lesson activity pages U03 1–14, direct teacher detail only U03:L12; all other teacher role entries from existing V8/V9 publisher crosswalk. Partial student-page activity check, not complete A-D parity.',studentLessonPagesRead:14,sourceTeacherDetailedPagesRead:[12],reviewedLessons:Object.entries(prompts).map(([id,paragraphs])=>{
 const code='PED:US-CCSS-MATH:G8:U03:'+id,s=by.get(code);
 return {lessonCode:code,studentSourceUrl:s.sourceStudentUrl,sourceStudentActivities:s.activities.map(x=>x.title),newOriginalTeachingParagraphs:paragraphs,sourceTeacherDetailedReview:false,fullAcademicSourceParity:false,studentGraphSubmissionSeen:false,teacherGradeSaved:false};
}),figures,richFileSha256:sha(target),sourceIssues:['Unobserved card-sort and interactive water-level experiment','Teacher cooldown and roles need independent source/human review','Shared Practice selection and real persistence not established'],stages:{A:0,B:0,C:0,D:0}};
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-p0-u03-remaining-source-fixes-20261030.json'),JSON.stringify(evidence,null,2)+'\n');
const ev='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
const course=JSON.parse(fs.readFileSync(path.join(root,ev),'utf8'));course.hashes.richCandidates[path.basename(target)]=evidence.richFileSha256;
fs.writeFileSync(path.join(root,ev),JSON.stringify(course,null,2)+'\n');
console.log(JSON.stringify({newIDs,richSha:evidence.richFileSha256,svg:figures.map(x=>x.assetPath)},null,2));
