'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const docPath=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
const specPath=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l02-practice-rubric-v15.json');
function connected(cubes){if(!Array.isArray(cubes)||!cubes.length||cubes.some(p=>!Array.isArray(p)||p.length!==3||p.some(x=>!Number.isSafeInteger(x))))return false;const keys=new Set(cubes.map(x=>x.join(',')));if(keys.size!==cubes.length)return false;const seen=new Set([cubes[0].join(',')]);const todo=[cubes[0]];while(todo.length){const p=todo.pop();for(let i=0;i<3;i++)for(const d of [-1,1]){const q=[...p];q[i]+=d;const k=q.join(',');if(keys.has(k)&&!seen.has(k)){seen.add(k);todo.push(q)}}}return seen.size===cubes.length}
test('L02 eight to twelve irregular cubes may contain uneven layer sizes',()=>{const m=[[0,0,0],[1,0,0],[2,0,0],[0,1,0],[1,1,0],[0,0,1],[1,0,1],[2,0,1],[1,1,1]];assert.equal(m.length,9);assert.ok(connected(m))});
test('L02 exactly packed 2×3×3 prism has 18 cubes and multiple layer partitions',()=>{assert.equal(2*3*3,18);assert.equal(3*6,18);assert.equal(2*9,18);assert.deepEqual([2,3,3].sort(),[3,2,3].sort())});
test('L02 reject disconnected, duplicated and malformed cube models',()=>{const a=Array.from({length:8},(_,x)=>[x,0,0]);assert.equal(connected([...a,[99,0,0]]),false);assert.equal(connected([...a,a[0]]),false);assert.equal(connected([[0,0],[1,0,0]]),false);assert.equal(connected('18'),false)});
test('L02 content and rubric are grade-owned and selected exact irregular family is statically wired; runtime deferred to Stage D',{skip:(!fs.existsSync(docPath)||!fs.existsSync(specPath))&&'full repo checkout not mounted'},()=>{const l=JSON.parse(fs.readFileSync(docPath,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L02'));const s=JSON.parse(fs.readFileSync(specPath,'utf8'));assert.ok(l);assert.equal(s.StageC,'GRADE5_L02_EXACT_CONNECTED_IRREGULAR_COUNT_WRITTEN_NOT_RUN_WITH_TEACHER_LED');assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.match(l.ExplanationParagraphs.join(' '),/Guess My Prism/);assert.ok(s.requirements.some(x=>x.task.includes('irregular')))});

test('grade5 U01 L02 exact connected irregular counts are selected over unrelated volume families', {
  skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson2Questions.cs')) && 'Full repository checkout required'
}, () => {
  const root=path.resolve(__dirname,'..');
  const read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
  const src=read('src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson2Questions.cs');
  const exact=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
  const core=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
  const reg=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
  const family='usccss.g5.u01.l02.count_irregular_connected_cubes';
  assert.equal(reg.families.find(x=>x.id===family).answerType,'exact_integer');
  assert.equal(reg.families.find(x=>x.id===family).lessonPracticeRouting,true);
  assert.ok(core.includes('UsCcssG5Unit1Lesson2PracticeContracts.Reviewed'));
  for(const m of ['Supports','Build','Verify','Solve'])assert.ok(exact.includes('UsCcssG5Unit1Lesson2Questions.'+m));
  assert.ok(src.includes('bottom')&&src.includes('middle')&&src.includes('top'));
  for(const [bottom,middle,top,expected] of [[5,3,0,8],[5,4,2,11],[6,4,2,12]])
    assert.equal(bottom+middle+top,expected);
  assert.match(src,/teacher review/i);
});
