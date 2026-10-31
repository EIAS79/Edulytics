// G8 U01:L17 selected first-party tessellation paper and whole-unit academic closure fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l17-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const unit=read('docs/curriculum/us-ccss-g8-coder4-u01-strict-unit-gate-20261030.json');
const report=read('docs/curriculum/us-ccss-g8-coder4-u01-unit-abc-closure-report-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const paper=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('selected source-bound L17 student page and exact private 8-point human rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L17');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(v=>v.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(paper,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const x of ['angle-tiles','tessellate','rotate','cool'])assert(paper.includes('id="'+x+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((sum,q)=>sum+q.MaxPoints,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.machineGenerated,false);assert.equal(r.teacherGradePersistenceVerified,false);
});
test('triangle, square, hexagon regular angular tilings exactly 360; pentagon insufficient/overlap',()=>{
 for(const entry of k.regularTilings){assert.equal(entry.copies*entry.angle,entry.sum);assert.equal(entry.sum,360);}
 const q=k.pentagon;assert.equal(q.threeSum,3*q.angle);assert.equal(q.fourSum,4*q.angle);
 assert.equal(360-q.threeSum,q.threeGap);assert.equal(q.fourSum-360,q.fourOverlap);
 assert.equal(q.threeGap,36);assert.equal(q.fourOverlap,72);
});
test('unit square infinite tiling translates to itself with two independent generators',()=>{
 const q=k.squareTiling;assert.equal(q.minimumDrawn,12);assert.deepEqual(q.translationVectors,[[1,0],[0,1]]);
 const cells=[[0,0],[0,1],[3,-2]];
 for(const [x,y]of cells)for(const [dx,dy]of q.translationVectors){
   assert.equal(Number.isInteger(x+dx)&&Number.isInteger(y+dy),true);
   assert.equal(Math.abs((x+dx+1)-(x+dx)),q.tileSide);
 }
});
test('four asymmetric triangle copies are mapped bijectively by a nonidentity 90 degree turn',()=>{
 const t=k.fourfoldTriangle;const rot=([x,y])=>[-y,x];let copies=[t.original];
 for(let i=1;i<4;i++)copies.push(copies[i-1].map(rot));
 assert.deepEqual(t.turnsCCWDegrees,[0,90,180,270]);assert.equal(t.wholePatternSmallestPositiveSymmetryDegrees,90);
 assert.deepEqual(copies[3].map(rot),copies[0]);
 assert.deepEqual(t.center,[0,0]);
});
test('six sectors Cool-down, all five response categories and full U01 source academic closure',()=>{
 assert.equal(k.cooldown.triangleCount*k.cooldown.eachAngle,k.cooldown.totalAtVertex);
 assert.equal(k.cooldown.nonidentityRotation,60);
 for(const key of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[key].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(ledger.rows.slice(0,17).filter(q=>q.readyForStageD).length,17);
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,17);assert.equal(ledger.abcCounters.D_VERIFIED,0);assert.equal(ledger.abcCounters.UNITS_ABC_CLOSED,1);
 assert.equal(unit.unitReadyForStageD,true);assert.equal(report.academicABCSelectedPaperClosures,17);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L01');
 assert.equal(report.stageD,'NOT_RUN');
});