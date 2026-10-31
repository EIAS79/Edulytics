// Grade 8 U01:L15 source exact paper/teacher route and math fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l15-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const page=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('L15 exact Rich V2 selected worksheet and four-part human teacher 8-point rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L15');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(p=>p.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(page,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const sec of ['classification','three-angles','tear','cool'])assert(page.includes('id="'+sec+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((sum,x)=>sum+x.MaxPoints,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.machineGenerated,false);assert.equal(r.teacherGradePersistenceVerified,false);
});
test('nine triangle category cells respect three angles sum and restrictive equilateral case',()=>{
 for(const type of ['scalene','isosceles'])for(const name of ['acute','right','obtuse'])assert.equal(k[type][name].reduce((sum,v)=>sum+v,0),180);
 assert.deepEqual(k.equilateral.acute,[60,60,60]);
 assert.equal(k.equilateral.right,null);assert.equal(k.equilateral.obtuse,null);
 const acute=v=>v.every(x=>x<90),right=v=>v.includes(90),obtuse=v=>v.some(x=>x>90);
 assert(acute(k.scalene.acute));assert(right(k.scalene.right));assert(obtuse(k.scalene.obtuse));
 assert(acute(k.isosceles.acute));assert(right(k.isosceles.right));assert(obtuse(k.isosceles.obtuse));
});
test('interior/exterior vertex C are distinct and wedge construction proves straight180 and quad360',()=>{
 assert.equal(k.givenAngles.reduce((sum,v)=>sum+v,0)+k.remainingInterior,k.triangleSum);
 assert.equal(k.remainingInterior+k.adjacentExterior,180);
 assert.equal(2*k.triangleSum,k.quadrilateralSum);
 assert.notEqual(k.adjacentExterior,k.remainingInterior);
 assert.equal(r.sourceActivityMap.length,4);
});
test('correct/equivalent/wrong/malformed/boundary fixtures authored; no runtime PASS',()=>{
 for(const kind of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[kind].length>=3);
 assert.equal(r.originalPublisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,15);assert.equal(ledger.abcCounters.D_VERIFIED,0);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L16');
});