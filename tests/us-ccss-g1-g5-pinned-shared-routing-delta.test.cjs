'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {execFileSync}=require('node:child_process');
const root=require('node:path').resolve(__dirname,'..');
const git=(...args)=>execFileSync('git',args,{cwd:root,encoding:'utf8'}).trim();
const grade4PublishedMain='39c757630d2bd67217a1468fce099653ed0f3f53';
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
test('G1 shared compatibility: exact 4-file additive G5 routing and diagnostic deltas, no G1 code edits',()=>{
 const num=git('diff','--numstat',grade4PublishedMain,'HEAD','--',...shared.map(x=>x[0]));
 const rows=new Map(num.split(/\r?\n/).map(x=>{
  const [added,removed,p]=x.split('\t');
  return [p,[Number(added),Number(removed)]];
 }));
 assert.equal(rows.size,shared.length,'unexpected shared Grade 1 code delta');
 for(const [p,added,removed,oldBlob,newBlob] of shared){
  assert.deepEqual(rows.get(p),[added,removed],p+' must be the audited change only');
  assert.equal(git('rev-parse',grade4PublishedMain+':'+p),oldBlob);
  assert.equal(git('rev-parse','HEAD:'+p),newBlob);
 }
 const grade1Names=git('diff','--name-only',grade4PublishedMain,'HEAD','--',
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
  const diff=git('diff','--unified=0',grade4PublishedMain,'HEAD','--',p);
  const removedCode=diff.split('\n').filter(x=>x.startsWith('-')&&!x.startsWith('---'));
  assert.deepEqual(removedCode,[],p+' must only add new grade-specific functionality');
  assert.ok(diff.includes('G5')||diff.includes('g5'),p+' must add G5-specific functionality');
 }
});
