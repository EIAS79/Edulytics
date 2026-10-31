// G8 U01:L11 student-selected paper and congruence mathematics fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l11-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const page=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const sq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2,lens=p=>[[0,1],[0,2],[1,2]].map(([i,j])=>sq(p[i],p[j]));
test('L11 selected exact paper and teacher-only rubric preserve source activity modalities',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L11');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(y=>y.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(page,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const x of ['hands','pairs','rectangles','cool'])assert(page.includes('id="'+x+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.machineGenerated,false);
});
test('handedness tracing reverses x but preserves lengths under reflection',()=>{
 assert.deepEqual(k.handOriginal.map(([x,y])=>[-x,y]),k.handMirror);
 for(let i=0;i<k.handOriginal.length;i++){const j=(i+1)%k.handOriginal.length;assert.equal(sq(k.handOriginal[i],k.handOriginal[j]),sq(k.handMirror[i],k.handMirror[j]));}
});
test('X and Z are rigid triangle images but doubled Y is not',()=>{
 const p=k.triangleOriginal;
 assert.deepEqual(p.map(([x,y])=>[x+2,y]),k.X);
 assert.deepEqual(p.map(([x,y])=>[3-x,y]),k.Z);
 assert.deepEqual(p.map(([x,y])=>[1+2*(x-1),1+2*(y-1)]),k.Y);
 assert.deepEqual(lens(p),lens(k.X));assert.deepEqual(lens(p),lens(k.Z));
 assert.notDeepEqual(lens(p),lens(k.Y));assert.deepEqual(k.congruentImages,['X','Z']);
});
test('equal area or equal perimeter is not sufficient to show rectangles congruent',()=>{
 const {R,S,T}=k.rectangles;
 for(const q of [R,S,T]){assert.equal(q.area,q.width*q.height);assert.equal(q.perimeter,2*(q.width+q.height));}
 assert.equal(R.area,S.area);assert.notEqual(R.perimeter,S.perimeter);
 assert.equal(R.perimeter,T.perimeter);assert.notEqual(R.area,T.area);
 for(const z of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[z].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,11);assert.equal(ledger.abcCounters.D_VERIFIED,0);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L12');
});
