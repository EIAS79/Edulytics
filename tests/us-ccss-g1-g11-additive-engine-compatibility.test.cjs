'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const cp=require('node:child_process');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const git=(...args)=>cp.execFileSync('git',args,{cwd:root,encoding:'utf8'}).trim();
const g1Proof='e702b36328ff90cbb01e7ac81a4a90c06e165989';
const preUaeG1Release='dc76fa4b2bc30dcb1a8af6a9b184ca4c381e25fb';
const preG5='50f89c7276c7d44b8a0a527096d3d590222ec7f6';
const engine='src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';
const contracts='src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs';
const familyRegistry='src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json';
const priorLines=(ref,file)=>cp.execFileSync('git',['show',ref+':'+file],
  {cwd:root,encoding:'utf8'}).split('\n');
const nowLines=file=>fs.readFileSync(path.join(root,file),'utf8').split('\n');
function assertOriginalLinesUnchanged(ref,file){
  const baseline=priorLines(ref,file);
  const current=nowLines(file);
  let originalIndex=0;
  const added=[];
  for(const line of current){
    if(originalIndex<baseline.length && line===baseline[originalIndex]) originalIndex++;
    else added.push(line);
  }
  assert.equal(originalIndex,baseline.length,
    'Deleted/edited a previously released engine/contract line: '+file);
  return added;
}
test('Original G1 family semantics and G11 routes are preserved after authorized G6 additions',()=>{
  assert.equal(git('merge-base','--is-ancestor',g1Proof,'HEAD'),'');
  assert.equal(git('rev-parse',preUaeG1Release+':'+engine),
    '19ca4300c3a8c772a4624831987ae164489026e8');
  assert.equal(git('rev-parse',preG5+':'+engine),
    'd84a61ebc37e8ca026f95906720bfaef3ea56081');
  const engineAdded=assertOriginalLinesUnchanged(preG5,engine);
  assert.ok(engineAdded.length>=30,'Grade5 and Grade6 must be additive to historical engine');
  assert.ok(engineAdded.some(x=>x.includes('UaeGrade5')),'Grade5 branches were lost');
  assert.ok(engineAdded.some(x=>x.includes('UaeG6')),'Grade6 branches are missing');
  const contractAdded=assertOriginalLinesUnchanged(preG5,contracts);
  assert.ok(contractAdded.some(x=>x.includes('PED:UAE-MOE-MATH:L6:GENERAL:')),
    'The 128 scoped G6 contracts are missing');
  const diff=git('diff','--numstat',preG5,'HEAD','--',engine,contracts);
  for(const row of diff.split('\n').filter(Boolean)){
    const [added,removed,file]=row.split('\t');
    assert.equal(removed,'0','An older published line was removed in '+file);
    assert.ok(Number(added)>0);
  }
  const data=JSON.parse(fs.readFileSync(path.join(root,familyRegistry),'utf8'));
  const g11=data.families.filter(x=>x.id?.startsWith('usccss.g11.alg2.'));
  assert.equal(g11.length,8);
  for(const x of g11){
    assert.equal(x.status,'Shadow');
    assert.equal(x.productionRouting,false);
    assert.ok(nowLines(engine).join('\n').includes(x.id),'G11 engine route missing '+x.id);
  }
});
test('Other grades and cross-school Practice negative-answer profiles remain explicit',()=>{
  const probe=fs.readFileSync(path.join(root,'tools/phase29/b001_practice_attempt_probe.py'),'utf8');
  assert.ok(probe.includes('wrong_answer_profile in ("uae-g2", "uae-g3", "uae-g4", "uae-g5", "uae-g6")'),
    'The G6 profile must be added without replacing G2-G5 negative controls');
  assert.ok(probe.includes('wrong_answer_profile=="uae-g2"') ||
    probe.includes('wrong_answer_profile in ("uae-g2", "uae-g3", "uae-g4", "uae-g5", "uae-g6")'));
  for(const file of [engine,contracts]){
    const changes=git('diff','--unified=0',preG5,'HEAD','--',file);
    const deleted=changes.split('\n').filter(x=>x.startsWith('-')&&!x.startsWith('---'));
    assert.deepEqual(deleted,[],'No historical Grade1-5/US engine or contract deletion: '+file);
  }
});
