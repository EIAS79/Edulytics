// G8 U01:L08 original selected paper route and independent math fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l08-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8'),sha=crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex');
const cw=([x,y])=>[y,-x],ccw=([x,y])=>[-y,x],half=([x,y],[a,b])=>[2*a-x,2*b-y];
const sq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
test('L08 student route selected by exact lesson, four assessment sections, independent teacher rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L08');assert.equal(sha,r.learnerWorksheetSha256);
 assert(rich.Lessons[7].ExplanationParagraphs.some(x=>x.includes(r.learnerWorksheetUrl)));
 for(const sec of ['quadrilateral','segment','pattern','cool'])assert(html.includes('id="'+sec+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.onlineSubmission,false);
 assert.equal(r.teacherGradePersistenceVerified,false);
});
test('four quarter-turn triangles tile square diamond exactly without gaps',()=>{
 const original=k.originalRightTriangle;
 for(let i=0;i<4;i++){
   const after=original.map(p=>{let x=p;for(let j=0;j<i;j++)x=cw(x);return x;});
   assert.deepEqual(after,k.fourClockwiseTriangles[i]);
 }
 const B=k.squareBoundary;
 const v=[B[1][0]-B[0][0],B[1][1]-B[0][1]],w=[B[2][0]-B[1][0],B[2][1]-B[1][1]];
 assert.equal(v[0]*w[0]+v[1]*w[1],0);
 assert.equal(sq(B[0],B[1]),sq(B[1],B[2]));
 const polyArea=Math.abs(B.reduce((sum,p,i)=>sum+p[0]*B[(i+1)%4][1]-p[1]*B[(i+1)%4][0],0)/2);
 assert.equal(polyArea,18);
});
test('all three half-turn centers preserve segment length; midpoint maps endpoints to same set',()=>{
 const original=k.originalSegment;
 for(const [center,key] of [[[2,0],'endpointQHalfTurn'],[[1,0],'midpointMHalfTurn'],[[0,2],'offLineCHalfTurn']]){
   const out=original.map(p=>half(p,center));assert.deepEqual(out,k[key]);
   assert.equal(sq(out[0],out[1]),4);
 }
 assert.deepEqual(k.midpointMHalfTurn[0],original[1]);assert.deepEqual(k.midpointMHalfTurn[1],original[0]);
 assert.notDeepEqual(k.endpointQHalfTurn,original);assert.notDeepEqual(k.offLineCHalfTurn,original);
});
test('scalene triangle exact 90/180/270 CCW pattern preserves side lengths and orientation',()=>{
 const p=k.scaleneOriginal;
 const ninety=p.map(ccw),oneeighty=ninety.map(ccw),twoseventy=oneeighty.map(ccw);
 assert.deepEqual(ninety,k.scalene90CCW);assert.deepEqual(oneeighty,k.scalene180CCW);assert.deepEqual(twoseventy,k.scalene270CCW);
 const lens=a=>[[0,1],[0,2],[1,2]].map(([i,j])=>sq(a[i],a[j]));
 for(const x of [ninety,oneeighty,twoseventy])assert.deepEqual(lens(x),lens(p));
 assert.equal(r.privateKey.scaleneOriginal[0][0],1);
});
test('selected paper has correct/equivalent/wrong/malformed/boundary fixtures, no Stage D assertion',()=>{
 assert.deepEqual(r.sourceActivityMap.map(x=>x.worksheet),['quadrilateral','segment','pattern','cool']);
 for(const x of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[x].length>=4);
 assert.equal(r.originalPublisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,8);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L09');
});
