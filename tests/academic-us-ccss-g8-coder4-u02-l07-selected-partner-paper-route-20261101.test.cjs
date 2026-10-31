// Grade8 U02:L07 first-party selected polygon/partner paper with precise geometric fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u02-l07-academic-assessment-contract-20261101.json'),r=c.selectedOfflinePaperRoute20261101,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'),unit=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
test('L07 exact selected source-bound A-B-C paper, seven actually separable cards and private rubric',()=>{
 assert.equal(c.lessonCode,'PED:US-CCSS-MATH:G8:U02:L07');
 assert.equal(c.sourceActivityToFirstPartyCrosswalk.length,3);assert.equal(c.sourcePracticeToFirstPartyCrosswalk.length,6);
 assert(rich.Lessons.some(x=>x.LessonCode===c.lessonCode&&x.ExplanationParagraphs.some(p=>p.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const id of r.stableTaskIds)assert(html.includes('id="'+id+'"'));
 assert.equal((html.match(/class="card break" id="partner-card-/g)||[]).length,7);
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.savedTeacherGradeVerified,false);
});
test('quantifier logic and square-rhombus angles refute mistaken side-only similarity',()=>{
 assert.deepEqual(k.quantifiers,['ALL','SOME','NONE']);
 assert.notDeepEqual(k.squareRhombus.squareAngles,k.squareRhombus.rhombusAngles);
 assert.equal(k.squareRhombus.similar,false);
 assert.notEqual(k.rectangles.RtoSRatios[0],k.rectangles.RtoSRatios[1]);
 assert.equal(k.rectangles.RtoTFactor,1.5);
});
test('all three source partner card pairs have factor2 and angle correspondence; square G unmatched',()=>{
 const q=k.partnerCards;
 assert.deepEqual(q.pairs,[['A','B'],['C','D'],['E','F']]);
 for(let i=0;i<2;i++)assert.equal(q.B.dimensions[i]/q.A.dimensions[i],q.pairFactor);
 for(let i=0;i<3;i++)assert.ok(Math.abs(q.D.sides[i]/q.C.sides[i]-q.pairFactor)<1e-12);
 assert.deepEqual(q.C.angles,q.D.angles);
 assert.equal(q.F.side/q.E.side,q.pairFactor);assert.deepEqual(q.E.angles,q.F.angles);
 assert.notDeepEqual(q.G.angles,q.E.angles);
 assert.equal(q.teacherCardsPrintedSeparately,true);
});
test('six practice families and first-party Cool-down verify positive/negative geometry boundary',()=>{
 assert.equal(k.practice1.angleImage,k.practice1.angleOriginal);
 assert.deepEqual(k.practice2.sidesOriginal.map(x=>x*k.practice2.factor),k.practice2.sidesImage);
 assert.notEqual(k.practice3.scaleRatios[0],k.practice3.scaleRatios[1]);assert.equal(k.practice3.similar,false);
 assert.deepEqual(k.practice4.original.map(x=>x*2),k.practice4.image);
 const v=k.practice5;
 assert.equal(v.angleOriginal,90);assert.ok(v.angleImageDegrees>71&&v.angleImageDegrees<72);assert.equal(v.similar,false);
 const z=k.practice6;assert.deepEqual(z.upperLeft,[0,2]);assert.deepEqual(z.upperRight,[2,2]);
 assert.deepEqual(k.cooldown.original.map(x=>x*k.cooldown.factor),k.cooldown.similar);
 assert.notEqual(k.cooldown.unequalRatios[0],k.cooldown.unequalRatios[1]);
 for(const type of ['correct','equivalent','wrong','missing','malformed','boundary'])assert(r.fixtures[type].length>=4);
 assert.equal(c.StageD,'NOT_RUN');assert.equal(led.abcCounters.ABC_READY_FOR_D,24);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(unit.academicABCReadyForD,7);assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L08');
});