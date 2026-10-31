// Grade 8 U01:L06 selected first-party paper/teacher and separate info-gap data-card fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const contract=read('docs/curriculum/us-ccss-g8-coder4-u01-l06-academic-assessment-contract-20261030.json'),route=contract.selectedOfflinePaperRoute20261031;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const student=fs.readFileSync(path.join(root,route.learnerWorksheetPath),'utf8');
const data=fs.readFileSync(path.join(root,route.teacherDistributedSeparateDataCardPath),'utf8');
const sha=s=>crypto.createHash('sha256').update(Buffer.from(s,'utf8')).digest('hex');
const move=([x,y],[dx,dy])=>[x+dx,y+dy],mirror=([x,y])=>[-x,y];
const rotCCW90=(p,[cx,cy])=>[cx-(p[1]-cy),cy+(p[0]-cx)];
const sq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
test('exact selected L06 student paper, separate teacher-distributed data card and private rubric',()=>{
 assert.equal(route.lessonCode,'PED:US-CCSS-MATH:G8:U01:L06');
 assert(rich.Lessons[5].ExplanationParagraphs.some(x=>x.includes(route.learnerWorksheetUrl)));
 for(const k of ['center','info-gap','problem-card','sequence','cool'])assert(student.includes('id="'+k+'"'));
 assert(data.includes('id="data-card"'));assert(!student.includes('id="data-card"'));
 assert.equal(sha(student),route.learnerWorksheetSha256);assert.equal(sha(data),route.dataCardSha256);
 assert.equal(route.privateTeacherRubric.Criteria.reduce((s,r)=>s+r.MaxPoints,0),8);
 assert.equal(route.onlinePractice,false);assert.equal(route.generatedByProduction,false);assert.equal(route.teacherGradePersistenceVerified,false);
 assert.equal(contract.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
});
test('all three original polygon vertices have same radii and CCW90 about center O(0,1)',()=>{
 const k=route.privateAnswerKey,p=k.rotationOriginal,q=k.rotationCCW90,c=k.rotationCenter;
 assert.deepEqual(p.map(x=>rotCCW90(x,c)),q);
 assert.deepEqual(p.map(x=>sq(x,c)),k.rotationRadiiSquared);
 assert.deepEqual(q.map(x=>sq(x,c)),k.rotationRadiiSquared);
 assert.notDeepEqual(p.map(x=>rotCCW90(x,[0,0])),q,'wrong center must not be accepted');
});
test('source 6.2 problem and data holder roles actually represented by separate first-party files',()=>{
 assert(student.includes('Problem-holder card'));assert(data.includes('Data Holder card'));
 assert(data.includes('90° counterclockwise'));assert(data.includes('O=(0,1)'));
 assert(student.includes('Do not reveal both cards at once'));
 assert(route.sourceToPaper.some(x=>x.source.startsWith('6.2')&&x.response.includes('SEPARATE')));
});
test('composition is noncommutative and each step preserves pairwise lengths',()=>{
 const k=route.privateAnswerKey;const t=p=>move(p,[2,0]);
 assert.deepEqual(k.compositionOriginal.map(t),k.afterTranslation);
 assert.deepEqual(k.compositionOriginal.map(t).map(mirror),k.afterTranslationThenReflection);
 assert.deepEqual(k.compositionOriginal.map(mirror).map(t),k.afterReflectionThenTranslation);
 assert.notDeepEqual(k.afterTranslationThenReflection,k.afterReflectionThenTranslation);
 const d=a=>[[0,1],[1,2],[0,2]].map(([x,y])=>sq(a[x],a[y]));
 for(const x of [k.afterTranslation,k.afterTranslationThenReflection,k.afterReflectionThenTranslation])assert.deepEqual(d(x),d(k.compositionOriginal));
});
test('positive equivalent negative malformed boundary fixtures authored, no Stage D claim',()=>{
 for(const x of ['correct','equivalent','wrong','malformed','boundary'])assert(route.fixtures[x].length>=3);
 assert.equal(route.publisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,6);assert.equal(ledger.abcCounters.D_VERIFIED,0);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L07');
});
