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
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs':['5bb7e5e356c5422144d20d5ac0b6d632a18feedc','7fcc5777093ea8c53e4abf88f100d5ca42430f6a'],
  [engine]:['e24c62e02c7474cbd7badf0b3c36951faec4b60d','02748e9f3b6f56d5ab0b9e3b4a08777fd9bb2c88'],
  'tools/phase29/b001_practice_attempt_probe.py':['aea34e8bf12c74e823ab16564451c36a7a4c9c86','ece5ce8b368f8cb07f5c4be0992d45de0d4ba96a']
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
  // UAE G1/G2 add 1,871 verified lines; approved G3 adds 300 more
  // engine lines and 623 lesson-contract lines, deleting zero. Preserve
  // US G1/G11 dispatch and the immutable pre-G1 production anchor.
  assert.equal(git('rev-parse',preUaeG1Release+':'+engine),
    '19ca4300c3a8c772a4624831987ae164489026e8');
  assert.equal(git('rev-parse','HEAD:'+engine),
    '02748e9f3b6f56d5ab0b9e3b4a08777fd9bb2c88');
  assert.equal(git('diff','--numstat',preUaeG1Release,'HEAD','--',engine),
    '2871\t0\t'+engine);
  // Explicitly constrain the G3-only additive changes, rather than blindly
  // accepting a moving SHA or dropping the original US G1 negative control.
  const grade2Published='d4448d58b43d46a73b2f8bed6128524f6c003caf';
  assert.equal(git('diff','--numstat',grade2Published,'HEAD','--',engine),
    '1000\t0\t'+engine);
  assert.equal(git('diff','--numstat',grade2Published,'HEAD','--',
    'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),
    '1451\t0\tsrc/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
  // The legacy HTTPS probe accepts an additional opt-in Grade 3 wrong-answer
  // profile; preserve the exact Grade 2/US/G1 branches and guard the only
  // permitted one-line replacement with a reviewed Git blob digest.
  const probe='tools/phase29/b001_practice_attempt_probe.py';
  assert.equal(git('diff','--numstat',grade2Published,'HEAD','--',probe),
    '1\t1\t'+probe);
  const probeDiff=git('diff','--unified=0',grade2Published,'HEAD','--',probe);
  assert.ok(probeDiff.includes('-            if wrong_answer_profile=="uae-g2":'));
  assert.ok(probeDiff.includes('+            if wrong_answer_profile in ("uae-g2", "uae-g3", "uae-g4", "uae-g5"):'));
  // UAE Grade 4 contributes only reviewed additive registrations.
  // Preserve the entire published main-branch engine and contract lines;
  // a changed, deleted or replaced pre-existing US/G1 path fails here.
  const preG4='c0970107123a3e94ca5d0928b45a58c577162d17';
  for(const [file,expectedDelta] of [
    [engine,700],
    ['src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs',828]
  ]){
    const changes=git('diff','--numstat',preG4,'HEAD','--',file).split('\t');
    assert.equal(changes[0],String(expectedDelta));
    assert.equal(changes[1],'0');
    const baseline=cp.execFileSync('git',['show',preG4+':'+file],{cwd:root,encoding:'utf8'}).split('\n');
    const current=fs.readFileSync(path.join(root,file),'utf8').split('\n');
    let oldIndex=0;
    for(const line of current) if(oldIndex<baseline.length && line===baseline[oldIndex]) oldIndex++;
    assert.equal(oldIndex,baseline.length,'Published non-Grade4 lines must survive unchanged: '+file);
  }
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


test('Grade 5 additions preserve every published G1-G4 and US/G11 engine line',()=>{
  // Release baseline is immutable. Exactly 30 first-party G5 registrations
  // are appended without replacing any previously published instruction.
  const preG5='50f89c7276c7d44b8a0a527096d3d590222ec7f6';
  assert.equal(git('rev-parse',preG5+':'+engine),
    'd84a61ebc37e8ca026f95906720bfaef3ea56081');
  assert.equal(git('diff','--numstat',preG5,'HEAD','--',engine),
    '30\t0\t'+engine);
  const prior=cp.execFileSync('git',['show',preG5+':'+engine],
    {cwd:root,encoding:'utf8'}).split('\n');
  const now=fs.readFileSync(path.join(root,engine),'utf8').split('\n');
  let matched=0;
  const added=[];
  for(const line of now){
    if(matched<prior.length && line===prior[matched]) matched++;
    else added.push(line);
  }
  assert.equal(matched,prior.length,'G5 must preserve all pre-existing engine lines');
  assert.equal(added.length,30,'Exactly thirty G5 registration lines are permitted');
  assert.equal(added.filter(x=>/UaeGrade5/.test(x)).length,21);
  assert.equal(added.filter(x=>/return new ExactProblem\(q\.Family/.test(x)).length,3);
  assert.ok(added.every(x=>/UaeGrade5/.test(x) ||
    /return new ExactProblem\(q\.Family/.test(x) ||
    x.trim()==='{' || x.trim()==='}'));
  // Negative control: the shared US/G1 wrong-answer predicate is modified
  // on a single pinned line solely to add the G5 fixture profile.
  const probe='tools/phase29/b001_practice_attempt_probe.py';
  assert.equal(git('rev-parse','HEAD:'+probe),
    'ece5ce8b368f8cb07f5c4be0992d45de0d4ba96a');
  assert.equal(git('diff','--numstat',preG5,'HEAD','--',probe),
    '1\t1\t'+probe);
});
