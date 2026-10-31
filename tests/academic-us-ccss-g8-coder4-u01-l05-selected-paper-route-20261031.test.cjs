// Grade 8 U01:L05 first-party selected offline teacher paper-route fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l05-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const cw=([x,y])=>[y,-x],ccw=([x,y])=>[-y,x],reflectX=([x,y])=>[x,-y],reflectY=([x,y])=>[-x,y];
const about=(p,center,fn)=>{const t=fn([p[0]-center[0],p[1]-center[1]]);return [t[0]+center[0],t[1]+center[1]];};
test('exact linked L05 first-party paper and private teacher-only manual route',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L05');
 assert(rich.Lessons[4].ExplanationParagraphs.some(x=>x.includes(r.learnerWorksheetUrl)));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const id of ['vectors','reflections','segment','cool'])assert(html.includes('id="'+id+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.onlineSubmission,false);assert.equal(r.teacherGradePersistenceVerified,false);
});
test('source translation vector cards accept all correct and reject reverse/different',()=>{
 const pairs=[[[-3,0],[1,2]],[[2,1],[-2,-1]],[[-4,-3],[0,-1]],[[1,2],[2,1]]];
 const delta=pairs.map(([p,q])=>[q[0]-p[0],q[1]-p[1]]);
 assert.deepEqual(delta,r.privateKey.vectorCardDisplacements);
 assert.deepEqual(delta.map((v,i)=>v[0]===4&&v[1]===2?i+1:null).filter(x=>x!==null),[1,3]);
});
test('large-value reflections, different centers and original segment length are exact',()=>{
 assert.deepEqual([[13,10],[13,-20],[13,570]].map(reflectX).concat([reflectY([3,2])]),r.privateKey.reflections);
 const k=r.privateKey.segment;
 assert.deepEqual(about(k.A,k.B,ccw),k.C);
 assert.deepEqual(about(k.B,k.A,ccw),k.D);
 assert.deepEqual(cw(k.A),k.E);assert.deepEqual(cw(k.B),k.F);
 const sq=(p,q)=>(p[0]-q[0])**2+(p[1]-q[1])**2;
 assert.equal(sq(k.A,k.B),4);assert.equal(sq(k.B,k.C),4);assert.equal(sq(k.A,k.D),4);assert.equal(sq(k.E,k.F),4);
});
test('original authored cooldown distinguishes rotation from reflection, 5 fixture categories',()=>{
 const k=r.privateKey.coolDown;
 assert.deepEqual(k.original.map(ccw),k.rotation90CCW);
 assert.deepEqual(k.original.map(reflectY),k.reflectionY);
 for(const category of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[category].length>=3);
 assert.equal(r.publisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,5);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L06');
});
