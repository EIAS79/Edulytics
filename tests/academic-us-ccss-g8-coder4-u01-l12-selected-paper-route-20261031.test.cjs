// Grade 8 U01:L12 source-specific selected paper route. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u01-l12-academic-assessment-contract-20261030.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const paper=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8'),sq=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
test('exact selected L12 sheet covers all four source activities with private teacher rubric',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U01:L12');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(y=>y.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(paper,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 assert.deepEqual(r.sourceActivityMap.map(x=>x.worksheet),['translations','pairs-one','pairs-two','build','cool']);
 for(const id of ['translations','pairs-one','pairs-two','build','cool'])assert(paper.includes('id="'+id+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((v,x)=>v+x.MaxPoints,0),8);
});
test('only two of four figure images are true translations with uniform vertex displacement',()=>{
 const p=k.triangleOriginal;
 assert.deepEqual(k.translationT1,p.map(([x,y])=>[x+3,y+1]));
 assert.deepEqual(k.translationT4,p.map(([x,y])=>[x-2,y+2]));
 assert.deepEqual(k.halfTurnT2,p.map(([x,y])=>[-x,-y]));
 assert.deepEqual(k.reflectionT3,p.map(([x,y])=>[-x,y]));
 const uniform=q=>q.map(([x,y],i)=>[x-p[i][0],y-p[i][1]]).every((v,_,a)=>v[0]===a[0][0]&&v[1]===a[0][1]);
 assert(uniform(k.translationT1));assert(uniform(k.translationT4));assert(!uniform(k.halfTurnT2));assert(!uniform(k.reflectionT3));
});
test('full quadrilateral correspondence distinguishes congruent reflection/translation from changed sides',()=>{
 assert.deepEqual(k.rectangleOriginal.map(([x,y])=>[x-5,y+2]),k.rectangleU);
 assert.equal(sq(k.rectangleOriginal[1],k.rectangleOriginal[2]),4);
 assert.equal(sq(k.rectangleV[1],k.rectangleV[2]),9);
 assert.deepEqual(k.quadOriginal.map(([x,y])=>[-x,y]),k.quadM);
 assert.equal(sq(k.quadOriginal[3],k.quadOriginal[0]),10);
 assert.equal(sq(k.quadN[3],k.quadN[0]),2);
});
test('equal-area four-unit-square composites can have unequal perimeters; original Cool-down exact',()=>{
 assert.equal(k.tiles.count*k.tiles.unitArea,k.tiles.square.area);
 assert.equal(k.tiles.square.area,k.tiles.strip.area);
 assert.notEqual(k.tiles.square.perimeter,k.tiles.strip.perimeter);
 assert.deepEqual(k.rectangleOriginal.map(([x,y])=>[x-5,y+2]),k.cooldownImages);
 for(const category of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[category].length>=3);
 assert.equal(c.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,12);assert.equal(ledger.abcCounters.D_VERIFIED,0);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L13');
});
