// Grade 8 U01:L09 selected first-party teacher paper route math and source modality fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l09-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const paper=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('L09 exact selected paper linked to RichV2 with separate teacher rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L09');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(p=>p.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(paper,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const part of ['line-moves','parallel','vertical','cool'])assert(paper.includes('id="'+part+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.onlineSubmission,false);
});
test('line translation and point images preserve one directed vector',()=>{
 const t=([x,y])=>[x+k.lineMove.vector[0],y+k.lineMove.vector[1]];
 assert.deepEqual(t(k.lineMove.A),k.lineMove.AImage);assert.deepEqual(t(k.lineMove.B),k.lineMove.BImage);
 assert.deepEqual(k.translatedLines,k.initialLines.map(y=>y+2));assert.equal(k.translatedLines[1]-k.translatedLines[0],k.separation);
});
test('half turn and reflection preserve same horizontal line equations but DIFFER point correspondence',()=>{
 assert.deepEqual(k.halfTurnLines,k.initialLines.map(y=>-y));
 assert.deepEqual(k.reflectedLines,k.initialLines.map(y=>-y));
 const point=[1,1],half=([x,y])=>[-x,-y],refl=([x,y])=>[x,-y];
 assert.deepEqual(half(point),k.halfTurnPoint);assert.deepEqual(refl(point),k.reflectedPoint);
 assert.notDeepEqual(k.halfTurnPoint,k.reflectedPoint);
 assert.equal(Math.abs(k.reflectedLines[1]-k.reflectedLines[0]),k.separation);
});
test('vertical acute angle correspondence and no parallel/intersection contradiction',()=>{
 assert.equal(k.acuteVerticalAngleDegrees,45);assert.equal(k.obtuseVerticalAngleDegrees,135);
 assert.equal(k.acuteVerticalAngleDegrees+k.obtuseVerticalAngleDegrees,180);
 assert.deepEqual(r.sourceActivityMap.map(x=>x.worksheet),['line-moves','parallel','vertical','cool']);
 for(const type of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[type].length>=3);
 assert.equal(r.originalPublisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,9);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L10');
});
