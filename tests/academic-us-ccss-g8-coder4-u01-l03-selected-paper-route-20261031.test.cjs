// Grade 8 U01:L03 selected offline paper assessment fixtures — AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l03-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031;
const l=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const original=[[1,1],[3,1],[2,3]];
const T=original.map(([x,y])=>[x+2,y-1]);
const R=original.map(([x,y])=>[-y,x]);
const F=original.map(([x,y])=>[-x,y]);
const axi=[[1,0],[0,1],[1,1]].map(([q,s])=>[-s,q+s]);
const area=p=>Math.abs((p[1][0]-p[0][0])*(p[2][1]-p[0][1])-(p[1][1]-p[0][1])*(p[2][0]-p[0][0]));
const lengths=p=>[[0,1],[1,2],[0,2]].map(([i,j])=>(p[i][0]-p[j][0])**2+(p[i][1]-p[j][1])**2);
test('exact L03 first-party sheet linked from student Rich V2, separate teacher rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L03');assert.equal(r.machineGenerated,false);assert.equal(r.teacherGradePersistenceVerified,false);
 assert.equal(r.onlineSubmission,false);assert.equal(r.teacherSelectedOffline,true);
 assert(rich.Lessons[2].ExplanationParagraphs.some(x=>x.includes(r.learnerWorksheetUrl)));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const x of ['iso','square','cool'])assert(html.includes('id="'+x+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(c.gates.D,'NOT_RUN');assert.equal(c.gates.READY_FOR_STAGE_D,true);
});
test('independent square-grid figures exact, congruent and not erroneously composed',()=>{
 assert.deepEqual(T,r.privateKey.translation);assert.deepEqual(R,r.privateKey.rotationCCW90);assert.deepEqual(F,r.privateKey.reflectionYAxis);
 const base=lengths(original);for(const p of [T,R,F]){assert.deepEqual(lengths(p),base);assert.equal(area(p),area(original));}
 assert.notDeepEqual(R,T.map(([x,y])=>[-y,x]),'rotation must start from original, not translated image');
});
test('isometric basis and 60° axial oracle preserve nonorthogonal radial distances',()=>{
 const e1=[1,0],e2=[0.5,Math.sqrt(3)/2];
 assert.ok(Math.abs(e1[0]*e2[0]+e1[1]*e2[1]-.5)<1e-12);
 assert.deepEqual(axi,r.privateKey.axialCCW60);
 const norm=([q,s])=>q*q+q*s+s*s;
 for(let i=0;i<3;i++)assert.equal(norm([[1,0],[0,1],[1,1]][i]),norm(axi[i]));
});
test('translation vs nontranslation cooldown and authored negative boundaries',()=>{
 const imageI=r.privateKey.cooldownImageI,imageII=r.privateKey.cooldownImageII;
 const moves=p=>p.map(([x,y],i)=>[x-original[i][0],y-original[i][1]]);
 assert.deepEqual(moves(imageI),[[2,-1],[2,-1],[2,-1]]);
 assert.deepEqual(moves(imageII),[[2,-1],[2,-1],[0,-1]]);
 for(const key of ['correct','equivalent','wrong','malformed','boundary'])assert.ok(r.fixtures[key].length>=3);
 assert.equal(l.abcCounters.ABC_READY_FOR_D,3);assert.equal(l.abcCounters.D_VERIFIED,0);
 assert.equal(l.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L04');
});
