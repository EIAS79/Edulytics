'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const cp=require('node:child_process');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const git=(...args)=>cp.execFileSync('git',args,{cwd:root,encoding:'utf8'}).trim();
const g1Proof='e702b36328ff90cbb01e7ac81a4a90c06e165989';
const courseBase='4c02c262c5b52feae4b0c2209231acdfc1310d83';
const engine='src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';
const familyRegistry='src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';
const legacyG1Paths=new Set(JSON.parse(fs.readFileSync(
  path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json'),'utf8'))
  .newLessons.flatMap(x=>x.evidence.affectedCodePaths));
const g1Critical=[
  engine,
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs',
  'src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.cs',
  'tools/phase29/b001_practice_attempt_probe.py',
  'tools/phase29/acceptance_two_school_https.py'
];
const expectedOld={
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs':['5bb7e5e356c5422144d20d5ac0b6d632a18feedc','1a21ed278a0a1956a82b67bf4be2e1218b200fb3'],
  [engine]:['e24c62e02c7474cbd7badf0b3c36951faec4b60d','0557784611797e6a45d5219556bd630e642530e7'],
  'tools/phase29/b001_practice_attempt_probe.py':['aea34e8bf12c74e823ab16564451c36a7a4c9c86','0d5cd3d3f294decd4d36d158563606b3f4708823']
};
test('Only the existing eight audited shared G1 paths differ from original proof',()=>{
  assert.equal(git('merge-base','--is-ancestor',g1Proof,'HEAD'),'');
  const names=git('diff','--name-only',g1Proof,'HEAD','--',...new Set([...legacyG1Paths,...g1Critical]))
    .split(/\r?\n/).filter(Boolean);
  assert.equal(names.length,8);
  assert.ok(names.every(x=>!x.includes('/G1')&&!x.includes('/g1')));
  for(const [file,[oldSha,newSha]] of Object.entries(expectedOld)){
    assert.ok(names.includes(file));
    assert.equal(git('rev-parse',g1Proof+':'+file),oldSha);
    assert.equal(git('rev-parse','HEAD:'+file),newSha);
  }
});
test('Algebra 2 shared engine delta is additive-only from verified Grade 10 base',()=>{
  assert.equal(git('rev-parse',courseBase+':'+engine),
    '0dc7edcdf94afe823f852a2803fdfbf6a834adba');
  assert.equal(git('rev-parse','HEAD:'+engine),
    '0557784611797e6a45d5219556bd630e642530e7');
  assert.equal(git('diff','--numstat',courseBase,'HEAD','--',engine),
    '210\t0\t'+engine);
  const diff=git('diff','--unified=0',courseBase,'HEAD','--',engine);
  assert.ok(!diff.split('\n').some(x=>x.startsWith('-')&&!x.startsWith('---')));
  const added=diff.split('\n').filter(x=>x.startsWith('+')&&!x.startsWith('+++')).map(x=>x.slice(1));
  assert.equal(added.length,210);
  assert.ok(added.every(x=>!x.includes('usccss.g1.')&&!x.includes('UsCcssG1')));
  const source=fs.readFileSync(path.join(root,engine),'utf8');
  const entries=JSON.parse(fs.readFileSync(path.join(root,familyRegistry),'utf8'));
  const newFamilies=entries.families.filter(x=>x.id?.startsWith('usccss.g11.alg2.'));
  assert.equal(newFamilies.length,8);
  for(const f of newFamilies){
    assert.equal(f.status,'Shadow');
    assert.equal(f.productionRouting,false);
    assert.ok(source.includes(f.id),'Missing G11-only routing for '+f.id);
  }
});
