// Grade 8 U01:L10 selected first-party paper assessment / geometry invariants. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l10-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const paper=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8'),sq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
test('selected exact L10 paper links to RichV2 and all four source activities plus first-party Cool-down',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L10');
 assert(pack.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(p=>p.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(paper,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 assert.deepEqual(r.sourceActivityMap.map(x=>x.worksheet),['isosceles','plus-one','plus-two','one-plus','cool']);
 for(const id of ['isosceles','plus-one','plus-two','one-plus','cool'])assert(paper.includes('id="'+id+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.teacherGradePersistenceVerified,false);
});
test('equal isosceles side lengths; half-turn midpoint creates true parallelogram and exact areas',()=>{
 assert.equal(sq(k.isosceles.A,k.isosceles.B),k.isosceles.equalSideSquared);
 assert.equal(sq(k.isosceles.A,k.isosceles.C),k.isosceles.equalSideSquared);
 const h=k.halfTurn;
 assert.deepEqual([(h.A[0]+h.C[0])/2,(h.A[1]+h.C[1])/2],h.M);
 assert.deepEqual([2*h.M[0]-h.B[0],2*h.M[1]-h.B[1]],h.D);
 assert.deepEqual(h.AB,h.DC);assert.deepEqual(h.BC,h.AD);
 assert.equal(h.triangleArea,3);assert.equal(h.parallelogramArea,6);
});
test('independent rigid scalene images preserve side lengths while reflection reverses orientation',()=>{
 const p=k.scaleneOriginal,T=p.map(([x,y])=>[x+2,y-1]),F=p.map(([x,y])=>[-x,y]);
 assert.deepEqual(T,k.translated);assert.deepEqual(F,k.reflected);
 const lens=a=>[[0,1],[0,2],[1,2]].map(([i,j])=>sq(a[i],a[j]));
 assert.deepEqual(lens(p),lens(T));assert.deepEqual(lens(p),lens(F));
});
test('two ordered 30-degree reflections give exact -30/-60 rays of constant radius 5',()=>{
 const d=Math.PI/180,E=[5*Math.cos(30*d),5*Math.sin(30*d)],M=[E[0],-E[1]],T=[5*Math.cos(-60*d),5*Math.sin(-60*d)];
 for(const p of [E,M,T])assert.ok(Math.abs(Math.hypot(...p)-k.rays.length)<1e-10);
 assert.deepEqual(k.rays.angleDegrees,[30,0,-30,-60]);
 assert.ok(Math.abs(M[1]+2.5)<1e-10);assert.ok(Math.abs(T[0]-2.5)<1e-10);
});
test('joined rectangles area/perimeter exclude shared interior segment; cases authored but NOT_RUN',()=>{
 const z=k.rectangles;assert.equal(z.combinedWidth,2*z.widthEach);assert.equal(z.combinedArea,z.combinedWidth*z.height);
 assert.equal(z.combinedPerimeter,2*(z.combinedWidth+z.height));assert.equal(z.combinedPerimeter,2*2*(z.widthEach+z.height)-2*z.sharedInteriorSide);
 for(const key of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[key].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,10);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L11');
});
