'use strict';
// Original, source-contextual Grade 8 mathematics diagrams. No publisher SVG/image is copied.
// Course-owned standalone assets; rendering in the authenticated student Rich V2 UI
// REQUIRES separately coordinated shared renderer integration (NOT certified here).
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const outDir='src/Edulytics.Web/wwwroot/curriculum/us-ccss-g8/figures';
const report='docs/curriculum/us-ccss-g8-coder4-v11-original-geometry-figures-20261029.json';
const richDir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const source=require(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json'));
const srcIndex=new Map(source.rows.map(x=>[x.lessonCode,x]));
const lessons=new Map(fs.readdirSync(path.join(root,richDir))
 .filter(f=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(f))
 .flatMap(f=>JSON.parse(fs.readFileSync(path.join(root,richDir,f),'utf8')).Lessons)
 .map(l=>[l.LessonCode,l]));
const diagrams=[];
const xml=s=>String(s).replace(/[&<>"']/g,x=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&apos;'}[x]));
const id=(u,l)=>'PED:US-CCSS-MATH:G8:U'+String(u).padStart(2,'0')+':L'+String(l).padStart(2,'0');
function add(u,l,anchor,kind,title,description,data){
 const lessonCode=id(u,l),lesson=lessons.get(lessonCode),source=srcIndex.get(lessonCode);
 if(!lesson||!source?.studentActivityCount)throw Error('Missing source/lesson '+lessonCode);
 if(!lesson.WorkedExamples.some(w=>w.Question.includes(anchor)))throw Error('No precise authored example '+lessonCode+': '+anchor);
 if(diagrams.some(x=>x.lessonCode===lessonCode))throw Error('Duplicate '+lessonCode);
 if(!description.includes('.')||description.length<65)throw Error('Inadequate semantic text alternative '+lessonCode);
 diagrams.push({lessonCode,sourceLessonCode:source.sourceLessonCode,sourceStudentUrl:source.sourceStudentUrl,
  sourceTeacherUrl:source.sourceTeacherUrl,authoredWorkedExampleAnchor:anchor,
  type:kind,title,description,data});
}
const P=(label,x,y)=>({label,x,y});
function moving(u,l,anchor,title,description,original,image){
 add(u,l,anchor,'transformation',title,description,{original,image});
}
function plot(u,l,anchor,title,description,points,lines=[],xmin=-6,xmax=10,ymin=-6,ymax=12){
 add(u,l,anchor,'coordinate-graph',title,description,{points,lines,xmin,xmax,ymin,ymax});
}
function triangle(u,l,anchor,title,description,a,b,c){
 add(u,l,anchor,'right-triangle',title,description,{a,b,c});
}
moving(1,1,'slides four squares','Translation of a 3-vertex triangle',
 'Original A(1,1), B(3,1), C(1,3) are each translated four right and two up to A′(5,3), B′(7,3), C′(5,5). Side lengths and angle measures are unchanged.',
 [P('A',1,1),P('B',3,1),P('C',1,3)],[P("A′",5,3),P("B′",7,3),P("C′",5,5)]);
moving(1,3,'shift four units right','Triangle translated right and downward',
 'Original A(1,1), B(3,1), C(1,2) are translated by (4,−3). Their images A′(5,−2), B′(7,−2), C′(5,−1) preserve all corresponding side lengths.',
 [P('A',1,1),P('B',3,1),P('C',1,2)],[P("A′",5,-2),P("B′",7,-2),P("C′",5,-1)]);
moving(1,4,'two units right','Translated quadrilateral',
 'The quadrilateral with vertices (1,1), (4,1), (3,3), (1,2) moves two units right. Each image coordinate has unchanged y and x increased by two.',
 [P('A',1,1),P('B',4,1),P('C',3,3),P('D',1,2)],[P("A′",3,1),P("B′",6,1),P("C′",5,3),P("D′",3,2)]);
plot(1,5,'Reflect P(3,2)','Reflections across the coordinate axes',
 'The original point P(3,2) is reflected in the x-axis to X(3,−2) and separately in the y-axis to Y(−3,2). Only the coordinate perpendicular to the mirror changes.',
 [P('P',3,2),P('X',3,-2),P('Y',-3,2)],[], -5,5,-4,4);
plot(1,6,'translated right two','Order of two coordinate transformations',
 'Starting from P(1,1): translate by (2,0) then reflect across the y-axis to A(−3,1), or reflect first then translate right two to B(1,1). The final points differ.',
 [P('P',1,1),P('A',-3,1),P('B',1,1)],[], -5,5,-3,4);
triangle(1,7,'side lengths of triangle','A 3-4-5 right triangle',
 'Triangle A(0,0), B(3,0), C(0,4) has legs of length three and four. The hypotenuse BC has length five by the Pythagorean theorem.',
 P('A',0,0),P('B',3,0),P('C',0,4));
moving(1,12,'Translate A(1,1)','Congruent rectangles under translation',
 'Rectangle A(1,1), B(4,1), C(4,3), D(1,3) translates by (−5,2); corresponding points move to (−4,3), (−1,3), (−1,5), (−4,5).',
 [P('A',1,1),P('B',4,1),P('C',4,3),P('D',1,3)],
 [P("A′",-4,3),P("B′",-1,3),P("C′",-1,5),P("D′",-4,5)]);
plot(2,4,'Dilate P(2,−3)','Origin-centered dilation',
 'The factor-two image of P(2,−3) is P′(4,−6). The factor-one-half image of Q(−8,6) is Q′(−4,3). Both image points lie on the original rays from the origin.',
 [P('P',2,-3),P("P′",4,-6),P('Q',-8,6),P("Q′",-4,3)],[], -10,7,-8,8);
plot(2,10,'slope through (1,2)','Slope two from two points',
 'The line through A(1,2) and B(4,8) has rise 6 and run 3, hence slope 2. The plotted line represents y=2x.',
 [P('A',1,2),P('B',4,8)],[{label:'y = 2x',m:2,b:0}],-2,6,-2,10);
plot(2,11,'line through (1,3)','Equation of a straight line',
 'The line y=2x+1 passes through A(1,3) and B(3,7). Its vertical intercept is (0,1) and its slope is 2.',
 [P('A',1,3),P('B',3,7),P('C',0,1)],[{label:'y = 2x + 1',m:2,b:1}],-3,6,-3,10);
plot(3,1,'(0,0),(1,3),(2,6)','Proportional relationship y=3x',
 'The plotted points (0,0), (1,3), (2,6) lie on y=3x. Each unit increase in input produces three units of output, and the line passes through the origin.',
 [P('O',0,0),P('A',1,3),P('B',2,6)],[{label:'y = 3x',m:3,b:0}],-1,4,-2,9);
plot(3,3,'At 4 km/h','Distance versus time at constant speed',
 'Distance d in kilometers equals four times t in hours. The points (0,0), (1,4), (2,8), (3,12) lie on a straight line through the origin.',
 [P('O',0,0),P('A',1,4),P('B',2,8),P('C',3,12)],[{label:'d = 4t',m:4,b:0}],-1,5,-2,15);
plot(3,9,'y=20−4x','Line with negative slope',
 'The straight line y=20−4x decreases four vertical units for each horizontal unit. It contains (0,20), (2,12), and (3,8).',
 [P('A',0,20),P('B',2,12),P('C',3,8)],[{label:'y = 20 − 4x',m:-4,b:20}],-1,6,-2,24);
plot(3,10,'Slope between (1,2)','Slope measurement from known points',
 'Line y=2x passes through (1,2) and (4,8); its rise is six and its run is three. A vertical line has undefined slope, not zero.',
 [P('A',1,2),P('B',4,8)],[{label:'y = 2x',m:2,b:0}],-2,6,-2,10);
plot(3,12,'Does (2,3)','Membership in one two-variable linear equation',
 'For 2x+y=7, A(0,7), B(1,5), C(2,3) lie on y=7−2x; point D(2,4) does not lie on this line. The source also requires reasoning about points and constraints.',
 [P('A',0,7),P('B',1,5),P('C',2,3),P('D',2,4)],[{label:'2x + y = 7',m:-2,b:7}],-2,6,-4,10);
plot(3,13,'x-intercept of 2x+3y=12','Intercepts of a two-variable line',
 'The line 2x+3y=12 has x-intercept (6,0) and y-intercept (0,4), with a third solution (3,2). All three points are on y=4−(2/3)x.',
 [P('A',6,0),P('B',0,4),P('C',3,2)],[{label:'2x + 3y = 12',m:-2/3,b:4}],-2,9,-2,7);
plot(4,10,'Is (3,10)','Verify a linear model graph',
 'The point (3,10) satisfies F=4+2d and belongs to the line, while (3,11) does not. A negative d may be algebraic but outside this practical domain.',
 [P('yes',3,10),P('no',3,11),P('intercept',0,4)],[{label:'F = 4 + 2d',m:2,b:4}],-1,6,-2,15);
plot(4,11,'Solve y=x+1','Intersection of two linear functions',
 'The lines y=x+1 and y=−x+5 meet at P(2,3); this ordered pair satisfies both equations, unlike (0,1).',
 [P('P',2,3),P('Q',0,1)],[{label:'y = x + 1',m:1,b:1},{label:'y = −x + 5',m:-1,b:5}],-2,7,-2,9);
plot(4,12,'Solve y=2x+1','Distinct parallel lines',
 'The parallel lines y=2x+1 and y=2x−3 have equal slope and different intercepts, so they never intersect. No ordered pair can satisfy both simultaneously.',
 [P('A',0,1),P('B',0,-3)],[{label:'y = 2x + 1',m:2,b:1},{label:'y = 2x − 3',m:2,b:-3}],-3,5,-7,9);
plot(4,13,'Solve y=2x+1','Solve a two-equation system graphically',
 'The lines y=2x+1 and x+y=10 intersect at (3,7). The point satisfies both equations, not merely one.',
 [P('intersection',3,7)],[{label:'y = 2x + 1',m:2,b:1},{label:'x + y = 10',m:-1,b:10}],-2,8,-2,14);
plot(4,14,'Solve x+y=7','Sum and difference constraints',
 'The lines x+y=7 and x−y=1 intersect at (4,3). The point passes both the sum and difference checks.',
 [P('intersection',4,3)],[{label:'x + y = 7',m:-1,b:7},{label:'x − y = 1',m:1,b:-1}],-2,9,-2,10);
plot(5,4,'Find x² for x=−2','The parabola y=x²',
 'The points (−2,4), (−1,1), (0,0), (1,1), (2,4) form a U-shaped quadratic graph. The equal outputs at −2 and 2 do not mean the relation is not a function.',
 [P('A',-2,4),P('B',-1,1),P('O',0,0),P('C',1,1),P('D',2,4)],[{label:'y = x²',quadratic:true}],-4,4,-2,9);
plot(5,7,'f(n)=5+2n','Function with base value and rate',
 'The line f(n)=5+2n contains points (0,5), (1,7), (2,9), and (3,11). The vertical intercept is five; the rate per unit is two.',
 [P('A',0,5),P('B',1,7),P('C',2,9),P('D',3,11)],[{label:'f(n) = 5 + 2n',m:2,b:5}],-1,5,-1,14);
plot(6,5,'(1,8),(2,6)','Negative association in paired measurements',
 'The four observations (1,8), (2,6), (3,4), (4,2) have constant slope minus two. This is a perfect negative linear trend in the authored example.',
 [P('A',1,8),P('B',2,6),P('C',3,4),P('D',4,2)],[{label:'y = −2x + 10',m:-2,b:10}],-1,6,-2,12);
plot(6,7,'(−2,4),(−1,1)','Nonlinear U-shaped scatter association',
 'The scatter points (−2,4), (−1,1), (0,0), (1,1), (2,4) are nonlinear and U-shaped; a straight line is not a faithful fit.',
 [P('A',-2,4),P('B',-1,1),P('O',0,0),P('C',1,1),P('D',2,4)],[], -4,4,-2,8);
triangle(8,6,'Legs 3 and 4','Right triangle with integer side lengths',
 'A right triangle with horizontal leg three and vertical leg four has hypotenuse five. Its squared legs sum to nine plus sixteen, equalling twenty-five.',
 P('A',0,0),P('B',3,0),P('C',0,4));
add(8,7,'Subtract four triangles','area-proof','Two area descriptions of a square',
 'For positive a and b, the outer square has side a+b and area (a+b)². Four right triangles each have area ab/2; subtracting them leaves c² and yields a²+b²=c².',
 {a:3,b:4,c:5});
triangle(8,10,'6 m east','East and north paths versus direct distance',
 'From origin A(0,0), move six east to B(6,0), then eight north to C(6,8). The direct distance AC is ten by the Pythagorean theorem.',
 P('A',0,0),P('B',6,0),P('C',6,8));
triangle(8,11,'(−2,1) and (4,9)','Distance on a coordinate plane',
 'Points A(−2,1) and C(4,9) differ by six horizontally and eight vertically. With B(4,1), the right triangle has hypotenuse AC of length ten.',
 P('A',-2,1),P('B',4,1),P('C',4,9));
plot(9,4,'(20,30),(40,20)','A decreasing weather-data model',
 'The points (20,30), (40,20), and (60,10) lie on the illustrative line T=40−0.5L. The model decreases half a degree per latitude degree in this example.',
 [P('A',20,30),P('B',40,20),P('C',60,10)],[{label:'T = 40 − 0.5L',m:-0.5,b:40}],0,80,0,45);
const WIDTH=800,HEIGHT=650,frame={x:68,y:72,w:640,h:500};
function svgText(x,y,text,attrs=''){return '<text x="'+x+'" y="'+y+'" '+attrs+'>'+xml(text)+'</text>'}
function coord(scene){
 const d=scene.data;const points=scene.type==='transformation'?[...d.original,...d.image]:scene.type==='right-triangle'?[d.a,d.b,d.c]:d.points;
 const minX=scene.type==='coordinate-graph'?d.xmin:Math.min(-2,...points.map(p=>p.x))-1;
 const maxX=scene.type==='coordinate-graph'?d.xmax:Math.max(2,...points.map(p=>p.x))+1;
 const minY=scene.type==='coordinate-graph'?d.ymin:Math.min(-2,...points.map(p=>p.y))-1;
 const maxY=scene.type==='coordinate-graph'?d.ymax:Math.max(2,...points.map(p=>p.y))+1;
 if(!(minX<maxX&&minY<maxY))throw Error('Bad viewport '+scene.lessonCode);
 const X=x=>frame.x+(x-minX)/(maxX-minX)*frame.w;
 const Y=y=>frame.y+(maxY-y)/(maxY-minY)*frame.h;
 let s='';
 const strideX=Math.max(1,Math.ceil((maxX-minX)/16));
 const strideY=Math.max(1,Math.ceil((maxY-minY)/14));
 for(let i=Math.ceil(minX/strideX)*strideX;i<=maxX;i+=strideX){
  s+='<line x1="'+X(i)+'" x2="'+X(i)+'" y1="'+frame.y+'" y2="'+(frame.y+frame.h)+'" stroke="#e5e7eb" stroke-width="1"/>';
  s+=svgText(X(i)+2,frame.y+frame.h+18,i,'font-size="12" fill="#444"');
 }
 for(let j=Math.ceil(minY/strideY)*strideY;j<=maxY;j+=strideY){
  s+='<line x1="'+frame.x+'" x2="'+(frame.x+frame.w)+'" y1="'+Y(j)+'" y2="'+Y(j)+'" stroke="#e5e7eb" stroke-width="1"/>';
  s+=svgText(frame.x-40,Y(j)-2,j,'font-size="12" fill="#444"');
 }
 if(minY<=0&&maxY>=0)s+='<line x1="'+frame.x+'" x2="'+(frame.x+frame.w)+'" y1="'+Y(0)+'" y2="'+Y(0)+'" stroke="#111827" stroke-width="2"/>';
 if(minX<=0&&maxX>=0)s+='<line x1="'+X(0)+'" x2="'+X(0)+'" y1="'+frame.y+'" y2="'+(frame.y+frame.h)+'" stroke="#111827" stroke-width="2"/>';
 if(scene.type==='transformation'){
  for(const [pts,color] of [[d.original,'#1d4ed8'],[d.image,'#be123c']]){
   s+='<polygon points="'+pts.map(p=>X(p.x)+','+Y(p.y)).join(' ')+'" fill="'+color+'" fill-opacity="0.12" stroke="'+color+'" stroke-width="4"/>';
  }
  for(let i=0;i<d.original.length;i++){
   const a=d.original[i],b=d.image[i];
   s+='<path d="M'+X(a.x)+' '+Y(a.y)+' L'+X(b.x)+' '+Y(b.y)+'" fill="none" stroke="#16a34a" stroke-width="2" stroke-dasharray="5 7"/>';
  }
 } else if(scene.type==='right-triangle'){
  const [a,b,c]=[d.a,d.b,d.c];
  const dot=(v,p,q)=>(p.x-v.x)*(q.x-v.x)+(p.y-v.y)*(q.y-v.y);
  if(![dot(a,b,c),dot(b,a,c),dot(c,a,b)].some(v=>Math.abs(v)<1e-9))throw Error('Non-right angle '+scene.lessonCode);
  s+='<polygon points="'+[a,b,c].map(p=>X(p.x)+','+Y(p.y)).join(' ')+'" stroke="#1d4ed8" fill="#dbeafe" stroke-width="4"/>';
  // Numeric angle validity is separately checked by the right-angle test; no decorative mark is drawn.
 } else if(scene.type==='coordinate-graph'){
  for(let i=0;i<d.lines.length;i++){
   const line=d.lines[i],color=i?'#be123c':'#1d4ed8';
   const pts=[];
   for(let x=minX;x<=maxX;x+=(maxX-minX)/110){const y=line.quadratic?x*x:line.m*x+line.b;if(y>=minY&&y<=maxY)pts.push(X(x)+','+Y(y))}
   if(pts.length)s+='<polyline points="'+pts.join(' ')+'" stroke="'+color+'" stroke-width="4" fill="none"/>';
  }
 }
 for(const point of points){
  const color=point.label.includes('′')?'#be123c':'#1d4ed8';
  s+='<circle cx="'+X(point.x)+'" cy="'+Y(point.y)+'" r="6" fill="'+color+'" stroke="#fff" stroke-width="1.5"/>';
  s+=svgText(X(point.x)+8,Y(point.y)-12,point.label+' ('+point.x+', '+point.y+')','font-weight="600" font-size="13" fill="#111827"');
 }
 return s;
}
function area(scene){
 const {a,b,c}=scene.data;const side=400,originX=200,originY=135;
 const inner=side*a/(a+b),outer=side*b/(a+b),n=n=>Math.round(n*100)/100;
 const square='<rect x="'+originX+'" y="'+originY+'" width="'+side+'" height="'+side+'" fill="#f0f9ff" stroke="#1d4ed8" stroke-width="3"/>';
 const A=[originX,originY],B=[originX+side,originY],C=[originX+side,originY+side],D=[originX,originY+side];
 const P=[originX+inner,originY],Q=[originX+side,originY+inner],R=[originX+outer,originY+side],S=[originX,originY+outer];
 let t=square;
 for(const tri of [[A,P,S],[P,B,Q],[Q,C,R],[S,R,D]])t+='<polygon points="'+tri.map(p=>p.map(n).join(',')).join(' ')+'" fill="#fdba74" fill-opacity="0.85" stroke="#9a3412" stroke-width="2"/>';
 t+='<polygon points="'+[P,Q,R,S].map(p=>p.join(',')).join(' ')+'" fill="#bfdbfe" stroke="#1d4ed8" stroke-width="3"/>';
 t+=svgText(280,350,'c² = '+c*c,'font-size="23" font-weight="bold" fill="#1d4ed8"');
 t+=svgText(220,115,'outer side = a+b = '+(a+b),'font-size="19" fill="#111827"');
 t+=svgText(160,570,'4 triangles × (ab/2) = '+(2*a*b),'font-size="19" fill="#111827"');
 t+=svgText(160,604,'(a+b)² − 2ab = c²; '+(a+b)**2+' − '+(2*a*b)+' = '+c*c,'font-size="19" fill="#111827"');
 return t;
}
const header=s=>'<svg xmlns="http://www.w3.org/2000/svg" width="'+WIDTH+'" height="'+HEIGHT+'" viewBox="0 0 '+WIDTH+' '+HEIGHT+'" role="img" aria-labelledby="title desc"><title id="title">'+xml(s.title)+'</title><desc id="desc">'+xml(s.description)+'</desc><rect width="'+WIDTH+'" height="'+HEIGHT+'" fill="white"/>'+svgText(40,39,s.title,'font-size="25" font-weight="bold" fill="#0f172a"');
for(const d of diagrams){
 const code=d.lessonCode.slice(-7).toLowerCase().replace(':','-');d.fileName=code+'.svg';
 const content=header(d)+(d.type==='area-proof'?area(d):coord(d))+'</svg>\n';
 if(/<script|<foreignObject|http:\/\//i.test(content.replace('http://www.w3.org/2000/svg','')))throw Error('Unsafe SVG content '+d.lessonCode);
 d.sha256=crypto.createHash('sha256').update(content).digest('hex');
 d.assetPath='/curriculum/us-ccss-g8/figures/'+d.fileName;
 d.attachedToStudentRenderer=false;
 d.verifiedInAuthenticatedBrowser=false;
 fs.mkdirSync(path.join(root,outDir),{recursive:true});
 fs.writeFileSync(path.join(root,outDir,d.fileName),content);
}
fs.writeFileSync(path.join(root,report),JSON.stringify({schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
status:'ORIGINAL_GRADE_8_SVG_ASSETS_IMPLEMENTED_NOT_ATTACHED_TO_SHARED_RICH_V2_RENDERER',
method:'Mathematically specified original coordinate graphs, transformations and triangle/area representations selected from first-party Grade 8 worked examples with source provenance. SVG has title/desc and labels; CSS/keyboard/browser/student delivery not certified.',
count:diagrams.length,scenes:diagrams},null,2)+'\n');
console.log('GRADE8_V11_ORIGINAL_GEOMETRY '+JSON.stringify({count:diagrams.length,byType:diagrams.reduce((r,d)=>(r[d.type]=(r[d.type]||0)+1,r),{}),report}));
