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
const preUaeG1Release='dc76fa4b2bc30dcb1a8af6a9b184ca4c381e25fb';
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
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs':['5bb7e5e356c5422144d20d5ac0b6d632a18feedc','c1b1898e7c5d27af8cb086ba6403f8073600279e'],
  [engine]:['e24c62e02c7474cbd7badf0b3c36951faec4b60d','ea6989494b3190676b7343e29da659bb3bf07afe'],
  'tools/phase29/b001_practice_attempt_probe.py':['aea34e8bf12c74e823ab16564451c36a7a4c9c86','db9a3f92f18bf7b0b14c9448ddd3e62657d54f8d']
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
test('Algebra 2 additive engine proof preserved and Grade12 changes never touch original G1',()=>{
  assert.equal(git('rev-parse',courseBase+':'+engine),
    '0dc7edcdf94afe823f852a2803fdfbf6a834adba');
  // Preserve the independently accepted G11 additive-only snapshot despite
  // subsequent Grade12 and shared-trigonometry improvements. The historical
  // 210-line insertion contains no G1 family edits or deletions.
  const preG12='0557784611797e6a45d5219556bd630e642530e7';
  const sourceBase=courseBase+':'+engine;
  assert.equal(git('diff','--numstat',sourceBase,preG12).split('\t').slice(0,2).join('\t'),'210\t0');
  const oldDiff=git('diff','--unified=0',sourceBase,preG12);
  assert.ok(!oldDiff.split('\n').some(x=>x.startsWith('-')&&!x.startsWith('---')));
  const oldAdded=oldDiff.split('\n').filter(x=>x.startsWith('+')&&!x.startsWith('+++')).map(x=>x.slice(1));
  assert.equal(oldAdded.length,210);
  assert.ok(oldAdded.every(x=>!x.includes('usccss.g1.')&&!x.includes('UsCcssG1')));
  // Compare against the immutable pre-G1 production commit rather than
  // the moving origin/main ref, which contains the accepted Grade 1 changes
  // AFTER merge. Preserve the original US G1 and G11 source hashes.
  // UAE G1 and G2 add 1,871 reviewed lines total, delete zero and
  // change no US G1 dispatch. The pre-G1 production anchor remains fixed.
  assert.equal(git('rev-parse',preUaeG1Release+':'+engine),
    '19ca4300c3a8c772a4624831987ae164489026e8');
  assert.equal(git('rev-parse','HEAD:'+engine),
    'ea6989494b3190676b7343e29da659bb3bf07afe');
  assert.equal(git('diff','--numstat',preUaeG1Release,'HEAD','--',engine),
    '1871\t0\t'+engine);
  const delta=git('diff','--unified=0',preUaeG1Release,'HEAD','--',engine);
  const changed=delta.split('\n').filter(x=>
    (x.startsWith('+')&&!x.startsWith('+++'))||
    (x.startsWith('-')&&!x.startsWith('---')));
  assert.ok(changed.every(x=>!x.includes('usccss.g1.')&&!x.includes('UsCcssG1')));
  const deleted=changed.filter(x=>x.startsWith('-'));
  assert.deepEqual(deleted,[]);
  assert.ok(changed.some(x=>x.includes('uae.g1.u01.l01.read_write_1_5')));
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
