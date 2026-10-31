// G8 U01:L07 original source-aligned paper/teacher route fixtures; AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l07-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const distanceSq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
const lengths=p=>[[0,1],[0,2],[1,2]].map(([x,y])=>Math.sqrt(distanceSq(p[x],p[y])));
const original=[[1,1],[4,1],[1,5]],T=original.map(([x,y])=>[x+2,y+1]),R=original.map(([x,y])=>[-y,x]),F=original.map(([x,y])=>[-x,y]);
test('exact L07 source-selected paper contains three first-party tick rulers and four task sections',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L07');
 assert(rich.Lessons.find(x=>x.LessonCode===r.lessonCode).ExplanationParagraphs.some(x=>x.includes(r.learnerWorksheetUrl)));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const s of ['measure','invariants','which','cool'])assert(html.includes('id="'+s+'"'));
 assert.equal((html.match(/<svg /g)||[]).length,3);
 assert.equal(r.privateTeacherRubric.Criteria.reduce((sum,x)=>sum+x.MaxPoints,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.teacherGradePersistenceVerified,false);
});
test('ruler measurements use exact eighths and tenths with proper precision',()=>{
 const rulers=r.privateAnswerKey.rulers;
 assert.deepEqual(rulers.map(x=>x.exact),[4.625,4.7,3.75]);
 assert.equal(rulers[0].exact*8,37);assert.equal(rulers[1].exact*10,47);assert.equal(rulers[2].exact*8,30);
 assert.equal(rulers[0].fraction,'4 5/8');assert.equal(rulers[2].fraction,'3 3/4');
 assert(html.includes('nearest 1/8'));assert(html.includes('nearest 0.1'));
});
test('true rigid images preserve three lengths and right angle, rotated candidate X valid',()=>{
 assert.deepEqual(T,r.privateAnswerKey.translation);assert.deepEqual(R,r.privateAnswerKey.rotationCCW90);assert.deepEqual(F,r.privateAnswerKey.reflectionYAxis);
 assert.deepEqual(lengths(original),[3,4,5]);for(const p of [T,R,F])assert.deepEqual(lengths(p),[3,4,5]);
 assert.equal((original[1][0]-original[0][0])*(original[2][0]-original[0][0])+(original[1][1]-original[0][1])*(original[2][1]-original[0][1]),0);
 assert.equal(r.privateAnswerKey.validCandidate,'X');assert.equal(r.privateAnswerKey.invalidCandidate,'Y');
 assert.notDeepEqual([3,4,5],[3,4,6]);
});
test('source 7.1, 7.2, 7.3 and first-party cooldown assessed; human boundary categories authored',()=>{
 assert.deepEqual(r.sourceActivityMap.map(x=>x.worksheet),['measure','invariants','which','cool']);
 for(const kind of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[kind].length>=3);
 assert.equal(r.originalPublisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,7);assert.equal(ledger.abcCounters.D_VERIFIED,0);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L08');
});
