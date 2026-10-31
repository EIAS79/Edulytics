// Grade 8 U01:L04 exact first-party selected paper/human route fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const contract=read('docs/curriculum/us-ccss-g8-coder4-u01-l04-academic-assessment-contract-20261030.json'),r=contract.selectedOfflinePaperRoute20261031;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const page=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const ref=([x,y])=>[-x,y],half=([x,y])=>[-x,-y],slide=([x,y])=>[x+2,y];
test('student Rich V2 selects source-linked L04 paper, teacher uses separate private eight-point rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L04');
 assert(rich.Lessons[3].ExplanationParagraphs.some(x=>x.includes(r.learnerWorksheetUrl)));
 for(const id of ['quick','partner','composition','cool'])assert(page.includes('id="'+id+'"'));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(page,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
 assert.equal(r.machineGenerated,false);assert.equal(r.onlineSubmission,false);assert.equal(r.savedTeacherGradeVerified,false);
 assert.equal(contract.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
});
test('teacher-operated flash reconstruction and partner figures are exact',()=>{
 const key=r.privateKey;
 assert.deepEqual([[1,1],[3,1],[2,3]].map(ref),key.quickImage);
 assert.deepEqual(key.quadrilateralOriginal.map(slide),key.quadrilateralTranslated);
 assert.deepEqual(key.quadrilateralOriginal.map(half),key.quadrilateralHalfTurn);
 assert.deepEqual(key.quadrilateralOriginal.map(ref),key.quadrilateralReflection);
});
test('noncommuting composition preserves all vertices and yields different results',()=>{
 const k=r.privateKey;
 assert.deepEqual(k.triangleOriginal.map(slide),k.translated);
 assert.deepEqual(k.triangleOriginal.map(slide).map(ref),k.translateThenReflect);
 assert.deepEqual(k.triangleOriginal.map(ref).map(slide),k.reflectThenTranslate);
 assert.notDeepEqual(k.translateThenReflect,k.reflectThenTranslate);
 const sq=(u,v)=>(u[0]-v[0])**2+(u[1]-v[1])**2;
 const lengths=p=>[[0,1],[0,2],[1,2]].map(([a,b])=>sq(p[a],p[b]));
 for(const p of [k.translated,k.translateThenReflect,k.reflectThenTranslate])assert.deepEqual(lengths(p),lengths(k.triangleOriginal));
});
test('positive/equivalent/wrong/malformed/boundary fixtures authored and no Stage D claims',()=>{
 for(const key of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[key].length>=3);
 assert.equal(r.publisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(led.rows[3].readyForStageD,true);assert.equal(led.abcCounters.ABC_READY_FOR_D,4);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L05');
 assert.equal(led.abcCounters.D_VERIFIED,0);
});
