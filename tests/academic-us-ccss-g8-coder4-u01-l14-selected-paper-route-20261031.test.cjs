// Grade 8 U01:L14 selected source-aligned geometry and boundary fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l14-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json'),page=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('exact L14 student worksheet linked to selected Rich V2 and private manual teacher rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L14');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(p=>p.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(page,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const id of ['pairs','transversal','rotation','cool'])assert(page.includes('id="'+id+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.onlineSubmission,false);
});
test('vertical and adjacent angles mathematically preserve exact acute and supplementary measures',()=>{
 assert.equal(k.intersecting.acute,k.intersecting.vertical);for(const s of k.intersecting.supplements)assert.equal(s+k.intersecting.acute,180);
 assert.equal(k.parallelAngles.given,k.parallelAngles.alternate);assert.equal(k.parallelAngles.corresponding,k.parallelAngles.alternate);
 assert.equal(k.parallelAngles.adjacent+k.parallelAngles.given,180);
});
test('half turn swaps y=0 and y=3 at two transversal intersections and preserves 45°',()=>{
 const h=k.halfTurn;
 assert.deepEqual([(h.P[0]+h.Q[0])/2,(h.P[1]+h.Q[1])/2],h.M);
 assert.deepEqual([2*h.M[0]-h.P[0],2*h.M[1]-h.P[1]],h.Q);
 assert.equal(h.acuteAngle,45);
 assert.equal(2*h.M[1]-0,3);
});
test('nonparallel counterexample intersection and angle differ from 45°',()=>{
 const n=k.nonparallel;
 assert.equal(n.intersection[1],n.intersection[0]);
 assert.equal(n.intersection[1],n.intersection[0]*0.5+3);
 assert.ok(n.differenceDegrees>18&&n.differenceDegrees<19);
 assert.notEqual(n.differenceDegrees,45);
 for(const kind of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[kind].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,14);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L15');
});
