// Grade8 U01:L16 first-party selected paper and independent math fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l16-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json'),paper=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('exact L16 Rich V2 selects first-party student sheet and private 8-point human assessment',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L16');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(v=>v.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(paper,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const id of ['relationships','midpoint-rotations','every-triangle','four-triangles','cool'])assert(paper.includes('id="'+id+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.onlineSubmission,false);
});
test('all three source computational relationships preserve signed arithmetic',()=>{
 assert.deepEqual(k.computations.map(q=>q.lhs===q.rhs),k.computations.map(q=>q.true));
 assert.deepEqual(k.computations.map(q=>q.true),[false,true,true]);
 assert.equal(62-28,34);assert.equal(60-30,30);assert.equal(3*(-8),2*(-8)-8);
 assert.equal((16+24)/(-2),16/(-2)+24/(-2));
});
test('midpoints of AC and AB anchor independent 180-degree rotations',()=>{
 const p=k.triangleOriginal,half=(x,c)=>[2*c[0]-x[0],2*c[1]-x[1]];
 assert.deepEqual([(p[0][0]+p[2][0])/2,(p[0][1]+p[2][1])/2],k.midpointAC);
 assert.deepEqual([(p[0][0]+p[1][0])/2,(p[0][1]+p[1][1])/2],k.midpointAB);
 assert.deepEqual(p.map(x=>half(x,k.midpointAC)),k.rotatedAboutMidpointAC);
 assert.deepEqual(p.map(x=>half(x,k.midpointAB)),k.rotatedAboutMidpointAB);
});
test('auxiliary parallel angle theorem and quadrilateral extension have exact bounds',()=>{
 assert.equal(k.generalTriangleAngleSum,180);
 assert.equal(k.fourTriangleCentralSum,k.fourTriangleRotations.length*90);
 assert.equal(k.quadrilateralAngleSum,2*k.generalTriangleAngleSum);
 assert.equal(k.cooldown.firstBaseAngle+k.cooldown.secondBaseAngle+k.cooldown.apex,180);
 assert.equal(k.cooldown.apex,65);
 for(const key of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[key].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,16);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L17');
});