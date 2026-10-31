// Grade 8 U02:L01 original source-aligned first-party paper route fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const contract=read('docs/curriculum/us-ccss-g8-coder4-u02-l01-academic-assessment-contract-20261031.json'),r=contract.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'),unit=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('selected exact U02 L01 source & all five Practice modalities presented as first-party offline teacher paper',()=>{
 assert.equal(contract.lessonCode,'PED:US-CCSS-MATH:G8:U02:L01');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(v=>v.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const id of ['warmup','sorting','scaled','project','practice','cool'])assert(html.includes('id="'+id+'"'));
 assert.equal(r.sourceActivityMap.length,3);assert.equal(r.sourcePracticeMap.length,5);
 assert.equal(r.privateTeacherRubric.Criteria.reduce((sum,x)=>sum+x.MaxPoints,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.machineGenerated,false);assert.equal(r.savedTeacherGradeVerified,false);
});
test('fraction quotient warm-up source quantities exact and equivalent mixed formats',()=>{
 assert.deepEqual(k.fractionQuotients.map(x=>x.improper),['25/8','71/35','17/22']);
 assert.equal(25/8,3+1/8);assert.equal(71/35,2+1/35);assert.equal((8+1/2)/11,17/22);
});
test('halved rectangle aspect ratios classify A C E only and B D separate family',()=>{
 const a=k.halvedRectangles.find(x=>x.label==='A').dimensions;
 const yes=k.halvedRectangles.filter(x=>x.similar).map(x=>x.label);
 assert.deepEqual(yes,['A','C','E']);
 for(const q of k.halvedRectangles){
  const h=q.dimensions[0]/a[0],w=q.dimensions[1]/a[1];
  assert.equal(Math.abs(h-w)<1e-12,q.similar);
  if(q.similar)assert.ok(Math.abs(q.relativeFactor-h)<1e-12);
 }
 assert.equal(k.nestedScaling.RtoB*k.nestedScaling.BtoC,k.nestedScaling.RtoC);
});
test('dilation center-to-point scaling factor, polygon copies and wrong shape options',()=>{
 const d=k.dilation;
 for(let i=0;i<3;i++)assert.deepEqual(d.original[i].map(x=>x*d.factor),d.image[i]);
 assert.deepEqual(k.practice1Correct,['A','C','D']);assert.deepEqual(k.practice4Correct,['B','C']);
 assert.deepEqual(k.practice3.A,[[0,0],[2,0],[0,2]]);
 assert.deepEqual(k.practice3.B,[[0,0],[4,0],[4,2],[0,2]]);
 assert.deepEqual(k.practice3.C,[[0,0],[2,0],[1,1],[0,1]]);
});
test('source scope retrieval, cooldown and five fixture families honest Stage D',()=>{
 assert.equal(k.practice2.forward*k.practice2.inverse,1);
 assert.equal(k.practice5.acute+k.practice5.obtuse,180);
 assert.equal(k.cooldown.correctFactor,2);assert.equal(k.cooldown.A[0]*2,k.cooldown.B[0]);
 assert.notEqual(k.cooldown.A[1]*2,k.cooldown.C[1]);
 for(const kind of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[kind].length>=3);
 assert.equal(contract.StageA,'A_CLOSED_SOURCE_AND_PRACTICE_SCOPE_PARITY');
 assert.equal(contract.StageD,'NOT_RUN');
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,18);assert.equal(ledger.abcCounters.D_VERIFIED,0);
 assert.equal(unit.academicABCReadyForD,1);assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L02');
});
