// G8 U01:L13 independent source-aligned student paper and non-vertex math fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l13-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const page=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const sq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
test('exact L13 first-party paper selected by RichV2, all four source activities and private rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L13');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(v=>v.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(page,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 assert.deepEqual(r.sourceActivityMap.map(x=>x.worksheet),['nonvertices','ovals','points','faces','cool']);
 for(const x of ['nonvertices','ovals','points','faces','cool'])assert(page.includes('id="'+x+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
});
test('fractional edge points translate with all trapezoid vertices and preserve parameters',()=>{
 assert.deepEqual(k.trapezoidOriginal.map(([x,y])=>[x+2,y+3]),k.trapezoidImage);
 assert.deepEqual([k.E[0]+2,k.E[1]+3],k.EImage);
 assert.deepEqual([k.F[0]+2,k.F[1]+3],k.FImage);
 assert.deepEqual(k.E,[2,0]);assert.deepEqual(k.F,[0.25,0.5]);
});
test('oval rigid 90-degree rotation preserves full axis lengths while resize does not',()=>{
 assert.deepEqual(k.ovals.originalAxes.slice().reverse(),k.ovals.rotatedAxes);
 assert.notDeepEqual(k.ovals.originalAxes.slice().sort(),k.ovals.enlargedAxes.slice().sort());
});
test('nonvertex centroid and midpoint obey same translated reflection and preserve correspondence',()=>{
 const f=([x,y])=>[-(x+2),y+1];
 assert.deepEqual(k.triangleOriginal.map(f),k.triangleImage);
 assert.deepEqual(f(k.midpointOriginal),k.midpointImage);
 const center=k.triangleOriginal.reduce(([a,b],[x,y])=>[a+x/3,b+y/3],[0,0]);
 assert.ok(Math.abs(center[0]-k.centroidOriginal[0])<1e-10);
 assert.deepEqual(f(k.centroidOriginal),k.centroidImage);
});
test('changed face cheek cannot be a reflection while all other landmarks correspond',()=>{
 const f=k.face;assert.deepEqual([-f.cheek[0],f.cheek[1]],f.correctMirroredCheek);
 assert.equal(sq(f.nose,f.correctMirroredCheek),f.correctSquaredDistance);
 assert.equal(sq(f.nose,f.changedCheek),f.changedSquaredDistance);
 assert.notEqual(f.correctSquaredDistance,f.changedSquaredDistance);
 const z=k.cooldown;assert.deepEqual([z.AImage[0]+z.BImage[0],z.AImage[1]+z.BImage[1]],[2*z.EImage[0],2*z.EImage[1]]);
 for(const kind of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[kind].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(led.abcCounters.ABC_READY_FOR_D,13);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L14');
});
