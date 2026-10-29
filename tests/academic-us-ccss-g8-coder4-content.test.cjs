'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g8-phase29-v1.lesson-content-pack.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const rich2=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const matrix=read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const by=new Map([...rich.Lessons,...rich2.Lessons].map(x=>[x.LessonCode,x]));
const plus=(p,v)=>[p[0]+v[0],p[1]+v[1]],ccw=([x,y])=>[-y,x],cw=([x,y])=>[y,-x];
const mx=([x,y])=>[x,-y],my=([x,y])=>[-x,y],half=([x,y])=>[-x,-y];
const d2=(p,q)=>(p[0]-q[0])**2+(p[1]-q[1])**2;
test('source and content list all 131 canonical Grade 8 lessons, exact identity/order',()=>{
 assert.equal(bp.BlueprintCode,'US-CCSS-MATH:G8:OUR-IM-2017');
 assert.equal(bp.LogicalLevel,9);assert.equal(bp.NativeLevel,'Grade 8');assert.equal(bp.Units.length,9);
 assert.deepEqual(bp.Units.map(x=>x.LessonCount),[17,13,14,16,22,11,16,16,6]);
 assert.equal(bp.Lessons.length,131);assert.equal(pack.lessons.length,131);assert.equal(source.rows.length,131);assert.equal(matrix.rows.length,131);
 assert.equal(new Set(bp.Lessons.map(x=>x.LessonCode)).size,131);
 assert.equal(new Set(bp.Lessons.map(x=>x.SourceLessonCode)).size,131);
 assert.deepEqual(matrix.rows.map(x=>x.lessonCode),bp.Lessons.map(x=>x.LessonCode));
 assert.deepEqual(matrix.rows.map(x=>x.sourceUrl),bp.Lessons.map(x=>x.SourceUrl));
 assert.equal(matrix.rows[0].lessonCode,'PED:US-CCSS-MATH:G8:U01:L01');
 assert.equal(matrix.rows[130].lessonCode,'PED:US-CCSS-MATH:G8:U09:L06');
});
test('131 distinct public teacher preparation pages were read; source roles are not silently overridden',()=>{
 assert.equal(source.retrieved,131);assert.equal(source.missing,0);
 assert.equal(source.roleDiscrepancies,22);
 for(const r of source.rows){assert.equal(r.academicAcceptance,undefined);assert.ok(r.sourceStudentGoal?.length>10);assert.ok(/^https:\/\/im\.kendallhunt\.com\//.test(r.url));}
 const l1=source.rows[0];
 assert.ok(l1.blueprintExtraRoles.includes('Addressing:8.G.A.1'));
 assert.ok(l1.sourceRoles.some(x=>x.role==='BuildingTowards'&&x.reference==='8.G.A.1'));
 assert.equal(matrix.rows[0].academicAccepted,false);
});
test('exact Rich V2 has 8 unique first-party lessons and valid schema/field minima',()=>{
 assert.equal(rich.SchemaVersion,1);assert.equal(rich.PackCode,'US-CCSS-MATH');assert.equal(rich.Lessons.length,8);assert.equal(rich2.Lessons.length,9);
 for(let n=1;n<=17;n++){const code='PED:US-CCSS-MATH:G8:U01:L'+String(n).padStart(2,'0');assert.ok(by.has(code));}
 const core=fs.readFileSync(path.join(root,'src/Edulytics.Core/Edulytics.Core.csproj'),'utf8');
 assert.match(core,/RichV2\/\*\.rich-lesson-v2\.json/);
 const directory=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2'),ids=new Set();
 for(const file of fs.readdirSync(directory).filter(x=>x.endsWith('.rich-lesson-v2.json'))){
   const doc=JSON.parse(fs.readFileSync(path.join(directory,file),'utf8'));
   for(const x of doc.Lessons){const key=x.LessonCode+':'+x.CultureCode;assert.ok(!ids.has(key),'Duplicate Rich V2: '+key);ids.add(key);}
 }
 for(const lesson of [...rich.Lessons,...rich2.Lessons]){
  assert.equal(lesson.CultureCode,'en');
  assert.ok(lesson.ExplanationParagraphs.length>=2);assert.ok(lesson.KeyConcepts.length>=3);
  assert.ok(lesson.WorkedExamples.length>=3);assert.ok(lesson.CommonMistakes.length>=2);assert.ok(lesson.SummaryPoints.length>=3);
  assert.deepEqual(lesson.Videos,[]);assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes('CC BY 4.0')));
  assert.ok(lesson.ExplanationParagraphs.some(x=>/teacher/i.test(x)),'Transparent scoring boundary missing');
  for(const x of lesson.WorkedExamples){assert.ok(x.Title&&x.Question&&x.Method&&x.Answer&&x.Check);assert.ok(x.Steps.length>=3);}
  assert.ok(!lesson.ExplanationParagraphs.some(x=>x.includes('Learning goal: You will ')));
 }
});
test('all eight lessons have source-specific focus and nonidentical mathematical worked examples',()=>{
 const all=[...rich.Lessons,...rich2.Lessons].flatMap(x=>x.WorkedExamples);
 assert.equal(all.length,51);
 assert.equal(new Set(all.map(x=>x.Question)).size,51);
 assert.match(by.get('PED:US-CCSS-MATH:G8:U01:L03').WorkedExamples[1].Answer,/\(−1,2\).*?\(−1,3\)/);
 assert.match(by.get('PED:US-CCSS-MATH:G8:U01:L05').WorkedExamples[1].Answer,/−1,−4/);
 assert.match(by.get('PED:US-CCSS-MATH:G8:U01:L07').WorkedExamples[0].Answer,/3, 4 and 5/);
 assert.match(by.get('PED:US-CCSS-MATH:G8:U01:L08').WorkedExamples[0].Answer,/60°/);
});
test('independent transformations satisfy positive, negative, inverse and boundary invariants',()=>{
 let seed=20261029;const rnd=()=>((seed=(Math.imul(1664525,seed)+1013904223)>>>0)%1025)-512;
 for(let i=0;i<4096;i++){
  const p=[rnd(),rnd()],q=[rnd(),rnd()],v=[rnd(),rnd()];
  assert.equal(d2(plus(p,v),plus(q,v)),d2(p,q));
  for(const fn of [ccw,cw,mx,my,half])assert.equal(d2(fn(p),fn(q)),d2(p,q));
  assert.deepEqual(cw(ccw(p)),p);assert.deepEqual(mx(mx(p)),p);assert.deepEqual(my(my(p)),p);assert.deepEqual(half(half(p)),p);
  assert.deepEqual(plus(plus(p,v),[-v[0],-v[1]]),p);
 }
 assert.notDeepEqual(ccw([2,1]),cw([2,1]),'Wrong turn direction must be distinguishable');
 assert.notDeepEqual(mx([5,-3]),my([5,-3]),'Wrong mirror axis must be distinguishable');
 assert.deepEqual([[1,1],[3,1],[1,3]].map(p=>plus(p,[4,2])),[[5,3],[7,3],[5,5]]);
 assert.deepEqual([[1,1],[3,1],[1,2]].map(p=>plus(p,[4,-3])),[[5,-2],[7,-2],[5,-1]]);
 assert.deepEqual([[2,1],[3,1],[2,2]].map(ccw),[[-1,2],[-1,3],[-2,2]]);
 assert.deepEqual(my([3,-2]),[-3,-2]);
 assert.equal(3**2+4**2,5**2);
});
test('unreviewed lesson corpus and Practice cannot be promoted into academic approval',()=>{
 assert.equal(matrix.counts.richV2AuthoredCandidates,17);
 assert.equal(matrix.counts.exactPracticeVerified,0);
 assert.equal(matrix.counts.humanAssessmentVerified,0);
 assert.equal(matrix.counts.academicAccepted,0);
 for(const r of matrix.rows){assert.equal(r.exactPracticeChecked,false);assert.equal(r.academicAccepted,false);assert.equal(r.stageD,'NOT_RUN');assert.ok(r.unresolved.length>=5);}
});

test('Unit 1 L09-L17 independent angles, rigid-copy geometry and tiling oracles',()=>{
 const lesson=n=>by.get('PED:US-CCSS-MATH:G8:U01:L'+String(n).padStart(2,'0'));
 assert.equal(lesson(9).WorkedExamples[0].Answer.includes('y=3 and y=6'),true);
 assert.equal(lesson(10).WorkedExamples[0].Answer,'16 units.');
 assert.equal(lesson(10).WorkedExamples[2].Answer,'12 square units.');
 assert.equal(2*(6+2),16);assert.equal(2*3*2,12);
 assert.equal(lesson(11).WorkedExamples[1].Answer.includes('not the same size'),true);
 assert.equal(lesson(12).WorkedExamples[0].Answer.includes('A′(−4,3)'),true);
 assert.equal(lesson(13).WorkedExamples[1].Answer.includes('Equal sides alone are insufficient'),true);
 assert.equal(180-65,115);assert.equal(180-50-60,70);
 assert.equal(180-42-73,65);
 assert.equal(360/6,60);assert.equal(360/4,90);
 assert.equal(360/108,10/3);
 assert.equal(lesson(17).WorkedExamples[2].Answer.includes('regular pentagons'),true);
 assert.notEqual(3*108,360);
 for(let n=9;n<=17;n++)assert.equal(lesson(n).WorkedExamples.length,3);
});
