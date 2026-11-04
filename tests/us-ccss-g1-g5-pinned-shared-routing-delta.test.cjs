'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {execFileSync}=require('node:child_process');
const root=require('node:path').resolve(__dirname,'..');
const git=(...args)=>execFileSync('git',args,{cwd:root,encoding:'utf8'}).trim();
const grade4PublishedMain='39c757630d2bd67217a1468fce099653ed0f3f53';
const grade5PublishedMain='ab942b5abb279610b16771a9eb02f5e1bc9c714d';
const shared=[
 ['src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs',1033,0,
  '0f2d85373b379177961c58b8c041b84e98b42af1','75a917f003bb4d423471c29cf5862157afb029f3'],
 ['src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs',294,0,
  '18c7c65da8606354002e37a78f846e8e0d30563d','cc9809a6a788b43a2c6b7cbbf2c1e990f5c481a3'],
 ['src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.cs',3,0,
  '5667da84beba2344498ca462b1fa5af1dc948bd0','c3df782c9e124e9ad5f1011eb6d2ab23c2aa61a8'],
 ['tools/phase29/b001_practice_attempt_probe.py',13,1,
  'aea34e8bf12c74e823ab16564451c36a7a4c9c86','26edc116fbe9e6f788b9a94c209a938d7db5a69a']
];

test('the Grade 4 live shared engine baseline is an ancestor of this Grade 5 candidate',()=>{
 assert.equal(execFileSync('git',['merge-base','--is-ancestor',grade4PublishedMain,'HEAD'],{cwd:root}).length,0);
});
test('historical G5 published audit: exact 4-file additive routing and diagnostic deltas',()=>{
 const num=git('diff','--numstat',grade4PublishedMain,grade5PublishedMain,'--',...shared.map(x=>x[0]));
 const rows=new Map(num.split(/\r?\n/).map(x=>{
  const [added,removed,p]=x.split('\t');
  return [p,[Number(added),Number(removed)]];
 }));
 assert.equal(rows.size,shared.length,'unexpected shared Grade 1 code delta');
 for(const [p,added,removed,oldBlob,newBlob] of shared){
  assert.deepEqual(rows.get(p),[added,removed],p+' must be the audited change only');
  assert.equal(git('rev-parse',grade4PublishedMain+':'+p),oldBlob);
  assert.equal(git('rev-parse',grade5PublishedMain+':'+p),newBlob);
 }
 const grade1Names=git('diff','--name-only',grade4PublishedMain,grade5PublishedMain,'--',
  'src/Edulytics.Services/Mathematics/UsCcssG1*',
  'src/Edulytics.Core/Mathematics/Practice/UsCcssG1*',
  'src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.G1*')
  .split(/\r?\n/).filter(Boolean);
 assert.deepEqual(grade1Names,[],'Grade 1 specific code must remain untouched');
});
test('G5 source-to-QuestionFamily additions cannot replace original G1 solver branches',()=>{
 const critical=shared.slice(0,3);
 for(const [p,added,removed] of critical){
  assert.equal(removed,0,p);
  const diff=git('diff','--unified=0',grade4PublishedMain,grade5PublishedMain,'--',p);
  const removedCode=diff.split('\n').filter(x=>x.startsWith('-')&&!x.startsWith('---'));
  assert.deepEqual(removedCode,[],p+' must only add new grade-specific functionality');
  assert.ok(diff.includes('G5')||diff.includes('g5'),p+' must add G5-specific functionality');
 }
});


// The historical G5 proof above stays pinned to its RELEASED SHA. New Grade 6
// source-scoped extensions are a separate, exact-blob compatibility audit.
// Neither the G1 canonical lessons nor their G1-specific engines may change.
test('historical G6 shared math routes preserve the published G5 baseline and G1-specific code',()=>{
 const g6PublishedMain='b146b9c35d5aa12b2f7efb59a3e4842c356f2713';
 assert.equal(execFileSync('git',['merge-base','--is-ancestor',grade5PublishedMain,g6PublishedMain],{cwd:root}).length,0);
 const g6=[
  [shared[0][0],2088,0,'ebb8108f080421a7078245644d490ec69fb2aeb7'],
  [shared[1][0],356,6,'558dbbd75cf1dbf274ee9f047f32ac42971ed779'],
  [shared[2][0],0,0,'c3df782c9e124e9ad5f1011eb6d2ab23c2aa61a8'],
  [shared[3][0],47,0,'28cc417956722287265a43d740eafb9db1ff2d25']
 ];
 const raw=git('diff','--numstat',grade5PublishedMain,g6PublishedMain,'--',...g6.map(x=>x[0]));
 const changed=new Map(raw.split(/\r?\n/).filter(Boolean).map(x=>{
  const [added,removed,path]=x.split('\t');
  return [path,[Number(added),Number(removed)]];
 }));
 assert.equal(changed.size,3,'historical G6 baseline includes exactly the three reviewed shared additions');
 for(const [path,added,removed,blob] of g6){
  assert.deepEqual(changed.get(path)||[0,0],[added,removed],path+' has an unreviewed delta');
  assert.equal(git('rev-parse',g6PublishedMain+':'+path),blob,path+' is not the tested G6 blob');
 }
 const engineDiff=git('diff','--unified=0',grade5PublishedMain,g6PublishedMain,'--',shared[1][0]);
 const removals=engineDiff.split('\n')
  .filter(x=>x.startsWith('-')&&!x.startsWith('---'))
  .map(x=>x.slice(1).trim()).sort();
 const expected=[
  'UsCcssG6Unit1Lesson1TilingQuestions.Supports(family.Trim()) ||',
  'requestedVariant);',
  'if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(family))',
  'return UsCcssG6Unit1Lesson1TilingQuestions.Verify(family, parameters, answer);',
  'if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(problem.Family))',
  'return UsCcssG6Unit1Lesson1TilingQuestions.Solve(problem.Family, p);'
 ].sort();
 assert.deepEqual(removals,expected,'the G6 extension must not delete other solver branches');
 const grade1=git('diff','--name-only',grade5PublishedMain,g6PublishedMain,'--',
  'src/Edulytics.Services/Mathematics/UsCcssG1*',
  'src/Edulytics.Core/Mathematics/Practice/UsCcssG1*',
  'src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.G1*')
  .split(/\r?\n/).filter(Boolean);
 assert.deepEqual(grade1,[],'G1-specific implementation remains unchanged');
});
