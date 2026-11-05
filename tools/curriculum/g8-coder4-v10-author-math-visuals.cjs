'use strict';
// Grade 8 course-owned original worked-example representations. No publisher art.
const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'../..'),dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const source=new Map(read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json').rows.map(x=>[x.lessonCode,x]));
const sha=x=>crypto.createHash('sha256').update(x).digest('hex');
const n=x=>Number.isInteger(x)?String(x):String(Number(x.toPrecision(10)));
const specs=[];
function add(u,l,anchor,title,desc,labels,rows){
 const code='PED:US-CCSS-MATH:G8:U'+String(u).padStart(2,'0')+':L'+String(l).padStart(2,'0');
 specs.push({code,anchor,visual:{Kind:'RatioTable',Title:title,Description:desc,Rows:labels.map((label,i)=>({Label:label,Cells:rows[i].map(String)}))}});
}
function linear(u,l,anchor,title,m,b,xs){
 add(u,l,anchor,title,'Original lesson-example input-output table. Each row uses y = '+n(m)+'x '+(b<0?'− ':'+ ')+n(Math.abs(b))+'. Plotting this relation, when required, is a separate graphical task.', ['x','y'],[xs,xs.map(x=>n(m*x+b))]);
}
function shift(u,l,anchor,title,pts,dx,dy){
 add(u,l,anchor,title,'Original rigid-motion table: translate every point by ('+n(dx)+','+n(dy)+'). All corresponding distances and angles must stay invariant. This table is not an interactive coordinate-grid construction.', ['vertex','x','y','new x','new y'],[pts.map((_,i)=>String.fromCharCode(65+i)),pts.map(x=>x[0]),pts.map(x=>x[1]),pts.map(x=>x[0]+dx),pts.map(x=>x[1]+dy)]);
}
function power(u,l,anchor,title,base,exps){
 add(u,l,anchor,title,'Original exponent evaluation table. The nonzero base is '+n(base)+'; negative powers mean reciprocals, never a logarithm approximation.',['exponent','value'],[exps,exps.map(e=>n(base**e))]);
}
function py(u,l,anchor,title,pts){
 add(u,l,anchor,title,'Original right-triangle numeric verification of a²+b²=c². It supplements but does not replace a labeled proof diagram.',['case','a','b','a²+b²','c'],[pts.map((_,i)=>i+1),pts.map(x=>x[0]),pts.map(x=>x[1]),pts.map(x=>x[0]*x[0]+x[1]*x[1]),pts.map(x=>n(Math.hypot(...x)))]);
}
// Rigid transformations and dilation
shift(1,1,'slides four squares','Triangle translated right four and up two',[[1,1],[3,1],[1,3]],4,2);
shift(1,2,'three squares left','Translation left three and up two',[[1,1],[3,1],[1,3]],-3,2);
shift(1,3,'shift four units right','Triangle translated right four and down three',[[1,1],[3,1],[1,2]],4,-3);
shift(1,4,'two units right','Quadrilateral translated two units',[[1,1],[4,1],[3,3],[1,2]],2,0);
add(1,5,'Reflect P(3,2)','Coordinate-axis reflection comparisons','Original P=(3,2); reflecting over the x-axis changes only y, and reflecting over the y-axis changes only x.',['state','x','y'],[['P','x-axis image','y-axis image'],[3,3,-3],[2,-2,2]]);
add(1,6,'translated right two','Order matters for compositions','Starting at (1,1), translation by (2,0) then reflection in y-axis ends at (−3,1); the reverse order ends at (1,1).',['order','x','y'],[['translate then reflect','reflect then translate'],[-3,1],[1,1]]);
add(1,9,'images of y=1 and y=4','Parallel lines after translation','Moving both horizontal lines two units up preserves their parallel nature.',['line','initial y','new y'],[['A','B'],[1,4],[3,6]]);
add(2,2,'Original radii are 2,3,4','Circular grid distances at scale 2','Multiply each distance from the dilation center by 2; rays are unchanged.',['original radius','image radius'],[[2,3,4],[4,6,8]]);
add(2,4,'Dilate P(2,−3)','Origin-centered dilations in coordinates','For k positive, both coordinates multiply by the same factor, including negative coordinates.',['point','x','y','factor k','image x','image y'],[['P','Q'],[2,-8],[-3,6],[2,0.5],[4,-4],[-6,3]]);
add(2,5,'A 2-by-3 rectangle','Dilation scales area by k²','A 2 by 3 rectangle dilated by k=2 becomes 4 by 6; its area goes from 6 to 24.',['state','length','width','area'],[['original','image'],[2,4],[3,6],[6,24]]);
linear(2,10,'slope through (1,2)','Line with slope two',2,0,[0,1,4,5]);
linear(2,11,'line through (1,3)','Line through two given points',2,1,[0,1,3,4]);
linear(2,12,'V=30−2t','Remaining water at constant loss rate',-2,30,[0,7,10,15]);
// Proportionality and linear models
linear(3,1,'(0,0),(1,3),(2,6)','Proportional input-output relationship',3,0,[0,1,2,3]);
linear(3,3,'At 4 km/h','Distance as a function of time',4,0,[0,1,2,3]);
linear(3,5,'y=3x+4','Linear but not proportional',3,4,[0,1,2,3]);
linear(3,7,'C=2d+6','Trip cost with a fixed fee',2,6,[0,1,5,8]);
linear(3,9,'y=20−4x','Decreasing height with negative slope',-4,20,[0,1,2,3]);
linear(3,10,'(1,2) and (4,8)','Rise and run on a line with slope two',2,0,[0,1,4,5]);
add(3,13,'x-intercept of 2x+3y=12','Ordered pairs on a two-variable line','All listed points satisfy 2x+3y=12. A complete coordinate graph remains a separate task.',['x','y','2x+3y'],[[0,3,6],[4,2,0],[12,12,12]]);
// Balancing equations, simultaneous systems
add(4,2,'Two equal bags plus three','Which candidate balances 2x+3=11?','A solution has equal left and right sides; x=4 is the only solution shown.',['x','left','right','equal?'],[[0,4,5],[3,11,13],[11,11,11],['No','Yes','No']]);
add(4,4,'Solve 5x+2=2x+14','Variables on both sides','The two expressions equal each other only when x=4 among the candidates shown.',['x','5x+2','2x+14','equal?'],[[0,4,5],[2,22,27],[14,22,24],['No','Yes','No']]);
add(4,7,'2(x+3)=2x+6','Identity versus contradiction','The first equation is true for all real x; the second equation 2x+1=2x+5 is never true.',['x','2(x+3)','2x+6','2x+1','2x+5'],[[-2,0,3],[2,6,12],[2,6,12],[-3,1,7],[1,5,11]]);
add(4,9,'Plan A is 6+3m','Find the break-even month','Plan A=6+3m; plan B=18+m. They have equal price at month 6.',['months','A cost','B cost'],[[0,2,6,10],[6,12,24,36],[18,20,24,28]]);
add(4,11,'Solve y=x+1 and y=−x+5','Intersection of two lines','For the same x, both y-values agree at (2,3). A graph must be represented separately.',['x','y=x+1','y=−x+5','intersection?'],[[0,2,4],[1,3,5],[5,3,1],['No','Yes','No']]);
add(4,12,'y=2x+1 and y=2x−3','Parallel and distinct equations','Two lines with slope 2 and different intercepts never intersect.',['x','y=2x+1','y=2x−3'],[[0,1,2],[1,3,5],[-3,-1,1]]);
add(4,13,'Solve y=2x+1, x+y=10','Check both parts of a system','Only (3,7) satisfies y=2x+1 and x+y=10 in these choices.',['x','y=2x+1','x+y','valid?'],[[1,3,4],[3,7,9],[4,10,13],['No','Yes','No']]);
add(4,14,'Solve x+y=7 and x−y=1','Check the difference constraint','All listed pairs sum to 7, but only (4,3) also has x−y=1.',['x','y','x+y','x−y'],[[3,4,5],[4,3,2],[7,7,7],[-1,1,3]]);
add(4,16,'Eight fruits','Check a two-price fruit system','Eight fruits total; apples cost $2, pears $3. Five apples and three pears cost $19.',['apples','pears','number','cost'],[[0,5,8],[8,3,0],[8,8,8],[24,19,16]]);
// Function tables and volumes
linear(5,1,'f(x)=2x+1','Inputs and outputs of f(x)=2x+1',2,1,[0,1,4,5]);
linear(5,3,'f(x)=2x−5','Evaluation of a linear function',2,-5,[0,3,7,8]);
add(5,4,'Find x² for x=−2','Quadratic values are not linear','Opposite inputs share the same square. Plotting the curve remains a separate graphic task.',['x','x²'],[[-2,-1,0,1,2],[4,1,0,1,4]]);
linear(5,7,'f(n)=5+2n','A fixed term plus rate',2,5,[0,1,2,3]);
linear(5,8,'f(x)=3x+2','A linear function with constant rate',3,2,[0,1,2,3]);
add(5,10,'f(t)=2t for','Two parts of one function','Rule 2t applies for 0≤t≤2; rule 4 applies for 2<t≤4. No jump occurs at t=2.',['t','rule','f(t)'],[[0,1,2,3,4],['2t','2t','2t','4','4'],[0,2,4,4,4]]);
add(5,11,'base 20 cm²','Volume and height in a right prism','Height is volume divided by constant base area 20 cm².',['volume (cm³)','height (cm)'],[[0,40,100],[0,2,5]]);
add(5,13,'Radius 3 cm','Cylinder volume and height','For fixed radius 3, V=9πh, so the table shows the exact coefficient V/π.',['height','V/π'],[[0,1,5],[0,9,45]]);
add(5,20,'Sphere radius 3','Sphere volume proportional to radius cubed','V=(4/3)πr³. The table shows exact volume coefficients, not decimals.',['radius','V/π'],[[1,2,3],['4/3','32/3','36']]);
add(5,22,'Cylinder radius 2 cm','Cylinder water volume as height rises','With fixed radius 2, water volume V=4πh.',['height h','V/π'],[[0,1,3,5],[0,4,12,20]]);
// Statistics and association
add(6,4,'At (4,11)','Line prediction and residual','At x=4, the fit y=2x+2 predicts 10; observed 11 gives residual +1.',['x','observed','predicted','residual'],[[4],[11],[10],[1]]);
linear(6,5,'(1,8),(2,6),(3,4),(4,2)','A negative linear trend',-2,10,[1,2,3,4]);
add(6,7,'(−2,4),(−1,1)','U-shaped nonlinear association','The squared-input data do not have constant first differences.',['x','y=x²'],[[-2,-1,0,1,2],[4,1,0,1,4]]);
linear(6,8,'Across (1,3) to (5,9)','Rate of increase between two observations',1.5,1.5,[1,3,5]);
add(6,9,'12 of 20 in A','Conditional percentage of a group','Divide by the full group count 20, not by another category.',['choice','count','percentage'],[['bus','other','total'],[12,8,20],['60%','40%','100%']]);
// Integer exponents and scientific notation
power(7,2,'10²×10³','Multiply powers of ten',10,[2,3,5]);
power(7,4,'10⁵/10²','Divide powers of ten',10,[2,3,5]);
power(7,5,'10⁻³','Negative powers mean reciprocals',10,[-3,-2,-1,0]);
power(7,6,'2³×2⁻⁵','Positive and negative exponents of two',2,[-5,-2,0,3]);
add(7,9,'50,000','Decimal and scientific notation','Scientific notation has a leading factor from 1 to below 10.',['decimal','scientific'],[['50000','62000','300000'],['5×10⁴','6.2×10⁴','3×10⁵']]);
add(7,11,'intervals are 0.0001','Equally spaced small decimals','Every number-line tick represents 0.0001, not 0.001.',['tick','value'],[[0,1,2,3,4],['0','0.0001','0.0002','0.0003','0.0004']]);
add(7,13,'62000','Scientific notation of 62,000','62000=6.2×10⁴ after moving the decimal four places.',['representation','value'],[['decimal','coefficient','power','product'],['62000','6.2','10000','6.2×10⁴']]);
add(7,15,'3×10⁵+2×10⁵','Add like powers of ten','Both terms have scale 10⁵, so add coefficients: 3+2=5.',['coefficient','scale','value'],[[3,2,5],['10⁵','10⁵','10⁵'],[300000,200000,500000]]);
// Squares, roots and right triangles
add(8,1,'Square side length 4','Side length versus square area','A square of edge s has area s².',['edge','area'],[[1,2,4],[1,4,16]]);
add(8,2,'Square side 2 becomes','Doubling length quadruples square area','From edge 2 to 4, area rises from 4 to 16, factor four.',['state','edge','area'],[['original','doubled'],[2,4],[4,16]]);
py(8,6,'Legs 3 and 4','Exact right triangle triples',[[3,4],[5,12]]);
add(8,7,'Subtract four triangles','Numerical decomposition for Pythagoras','For a=3, b=4: outer square side 7 has area 49, four triangles total area 24, inner c²=25. This is NOT the actual visual proof.',['measure','value'],[['a','b','outer side','outer area','four triangles','center c²'],[3,4,7,49,24,25]]);
py(8,8,'legs 8 and 15','Determine unknown hypotenuses',[[8,15],[3,4]]);
py(8,10,'6 m east','Shortest path after perpendicular travel',[[6,8],[5,12]]);
add(8,11,'(−2,1) and (4,9)','Distance by coordinate differences','From (−2,1) to (4,9), Δx=6 and Δy=8, so distance is 10. A coordinate grid is separately required.',['measure','value','square'],[['Δx','Δy','distance'],[6,8,10],[36,64,100]]);
add(8,12,'cube edge 3','Cubic volume changes with edge length','A cube with side length s has three equal dimensions and exact volume s³ cubic units.',['edge','volume'],[[1,2,3],[1,8,27]]);
add(8,13,'∛(−27)','Real cube roots preserve negative signs','Each cube root r satisfies r³=x even for negative x.',['x','cube root'],[[-27,-8,0,8,27],[-3,-2,0,2,3]]);
// Tessellation and mathematical modeling
add(9,1,'90° square corners','Fill 360 degrees around a vertex','Each indicated regular tile shape can meet without gaps when vertex angles sum to 360°.',['regular polygon','corner angle','number','sum'],[['square','hexagon','triangle'],[90,120,60],[4,3,6],[360,360,360]]);
add(9,2,'120° regular-hexagon','Hexagonal corners around a point','Exactly three hexagon angles of 120° sum to 360°.',['number','degrees total'],[[1,2,3,4],[120,240,360,480]]);
linear(9,4,'(20,30),(40,20),(60,10)','Negative association with latitude',-0.5,40,[20,40,60]);
linear(9,5,'T≈40−0.5L','Latitude as an input to a model',-0.5,40,[0,30,40,60]);
linear(9,6,'T=40−0.5L','Use a linear model without extrapolation claims',-0.5,40,[0,30,45,60]);
if(new Set(specs.map(s=>s.code)).size!==specs.length)throw Error('Duplicate lesson codes');
const specBy=new Map(specs.map(s=>[s.code,s]));
const files=fs.readdirSync(path.join(root,dir)).filter(x=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(x));
const pending=[],evidence=read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const installed=[];
for(const file of files){
 const rel=dir+'/'+file,original=fs.readFileSync(path.join(root,rel),'utf8'),docs=JSON.parse(original);
 let contents=original;
 for(const lesson of docs.Lessons){
  const spec=specBy.get(lesson.LessonCode);if(!spec)continue;
  if(lesson.Visuals.length && (lesson.Visuals.length!==1 || lesson.Visuals[0].Title!==spec.visual.Title))throw Error('Unexpected different lesson visual '+spec.code);
  if(!lesson.WorkedExamples.some(x=>x.Question.includes(spec.anchor)))throw Error('Worked-example anchor not found '+spec.code+': '+spec.anchor);
  if(!source.get(spec.code)?.studentActivityCount)throw Error('No student activity provenance '+spec.code);
  const start=contents.indexOf('"LessonCode": "'+spec.code+'"'),at=contents.indexOf('"Visuals":',start);
  const videos=contents.indexOf('"Videos":',start);
  if(start<0||at<0||videos<at)throw Error('Visual slot not found '+spec.code);
  if(JSON.stringify(lesson.Visuals)!==JSON.stringify([spec.visual])){
   const pretty=JSON.stringify([spec.visual],null,2).split('\n').map((line,i)=>i?'      '+line:line).join('\n');
   contents=contents.slice(0,at)+'"Visuals": '+pretty+',\n      '+contents.slice(videos);
  }
  installed.push({lessonCode:spec.code,sourceLessonCode:source.get(spec.code).sourceLessonCode,
   sourceStudentUrl:source.get(spec.code).sourceStudentUrl,authoredExampleAnchor:spec.anchor,
   visuals:[spec.visual],testedInBrowser:false,publisherVisualParityAccepted:false});
 }
 if(contents!==original){JSON.parse(contents);pending.push({rel,contents,file})}
}
if(installed.length!==specs.length)throw Error('Not all specs installed '+installed.length+'/'+specs.length);
for(const item of pending)evidence.hashes.richCandidates[item.file]=sha(Buffer.from(item.contents,'utf8'));
const report='docs/curriculum/us-ccss-g8-coder4-v10-original-math-representations-20261029.json';
for(const item of pending)fs.writeFileSync(path.join(root,item.rel),item.contents);
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json'),JSON.stringify(evidence,null,2)+'\n');
fs.writeFileSync(path.join(root,report),JSON.stringify({schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 status:'COURSE_OWNED_ORIGINAL_MATH_REPRESENTATIONS_NOT_A_B_C_D_ACCEPTANCE',
 method:'Explicit Grade 8 worked-example anchors and original numeric tables for the existing Rich V2 table renderer; no publisher pixels, graph renderer or automatic grading added.',
 count:installed.length,rows:installed},null,2)+'\n');
console.log('V10_MATH_REPRESENTATIONS '+JSON.stringify({total:installed.length,files:pending.map(p=>p.file),report}));
