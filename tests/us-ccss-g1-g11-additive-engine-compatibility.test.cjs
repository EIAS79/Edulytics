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
const nowLines=file=>fs.readFileSync(path.join(root,file),'utf8').replace(/\r\n/g,'\n').split('\n');
// Owner-directed retirement of one obsolete UAE G7 General English
// supporting contract is the sole permitted historical contract removal.
// Preserve exact old G1/G11 routes and permit only narrowly scoped G9 additions.
const retiredG7GeneralContractBlock=[
  '        new(',
  '            "PED:UAE-MOE-MATH:L7:GENERAL:01:06:RATES-AND-UNIT-RATES",',
  '            "ratio.unit_rate",',
  '            "UNIT_RATE",',
  '            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],',
  '            "SupportingLesson",',
  '            "READY_VERIFIED",',
  '            Version),'
];
// One additional owner-retired UAE G8 Advanced English unit-rate declaration.
const retiredG8AdvancedContractBlock=[
  '        new(',
  '            "PED:UAE-MOE-MATH:L8:ADVANCED:01:06:RATES-AND-UNIT-RATES",',
  '            "ratio.unit_rate",',
  '            "UNIT_RATE",',
  '            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],',
  '            "SupportingLesson",',
  '            "READY_VERIFIED",',
  '            Version),'
];
// Owner-retired UAE Grade9 removes two orphan unit-rate declarations;
// new first-party G9 exact-family branches are separately tested and scoped.
const retiredGrade9ContractBlocks=['ADVANCED','GENERAL'].map(pathway=>[
  '        new(',
  '            "PED:UAE-MOE-MATH:L9:'+pathway+':01:06:RATES-AND-UNIT-RATES",',
  '            "ratio.unit_rate",',
  '            "UNIT_RATE",',
  '            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],',
  '            "SupportingLesson",',
  '            "READY_VERIFIED",',
  '            Version),'
]);
function stripOnlyRetiredGrade9Contracts(lines){
  let result=lines;
  for(const block of retiredGrade9ContractBlocks){
    const start=result.findIndex((line,i)=>
      block.every((expected,offset)=>result[i+offset]===expected));
    assert.ok(start>=0,'Expected old UAE Grade9 contract missing from pinned original baseline');
    assert.equal(result.filter(x=>x===block[1]).length,1,
      'Retired Grade9 contract declaration must occur exactly once');
    result=[...result.slice(0,start),...result.slice(start+block.length)];
  }
  return result;
}
function stripOnlyRetiredG8AdvancedContract(lines){
  const start=lines.findIndex((line,i)=>
    retiredG8AdvancedContractBlock.every((expected,offset)=>
      lines[i+offset]===expected));
  assert.ok(start>=0,'Expected retired Grade8 Advanced contract missing from immutable baseline');
  assert.equal(lines.filter(x=>x===retiredG8AdvancedContractBlock[1]).length,1);
  return [...lines.slice(0,start),
          ...lines.slice(start+retiredG8AdvancedContractBlock.length)];
}
function stripOnlyRetiredG7Contract(lines){
  const start=lines.findIndex((line,i)=>
    retiredG7GeneralContractBlock.every((expected,offset)=>
      lines[i+offset]===expected));
  assert.ok(start>=0,'Expected old Grade7 English contract block missing from immutable baseline');
  assert.equal(lines.filter(line=>line===retiredG7GeneralContractBlock[1]).length,1);
  return [...lines.slice(0,start),
          ...lines.slice(start+retiredG7GeneralContractBlock.length)];
}
// PR #640: owner-directed G8 U12L04 post-release P1 review fix only.
// Exact new contract blob is pinned; all non-G8 G1/G11 lines remain compared to historical sources.
const oldEngineClass='public sealed class ExactSkillContractQuestionEngine';
const newEngineClass='public sealed partial class ExactSkillContractQuestionEngine';
const oldSharedIntegerParser='        if (!int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))';
const newSharedIntegerParser='        if (!int.TryParse(integerAnswer, NumberStyles.Integer,';
function withoutGrade8ScopedPracticeAssignments(lines){
  // All Grade8 GENERAL Practice rewrites are isolated by their
  // byLesson[...] deterministic contract key; compare every other line
  // against the original G1/G11 baseline. Never exempt shared G1 paths.
  const result=[];let skipping=false;let count=0;
  for(const line of lines){
    if(!skipping && line.includes('byLesson["PED:UAE-MOE-MATH:L8:GENERAL:')){
      skipping=true;count++;
    }
    if(skipping){
      if(line.includes(';'))skipping=false;
      continue;
    }
    result.push(line);
  }
  assert.equal(skipping,false,'G8 contract declaration not terminated');
  // The new candidate has 76 deterministic scoped declarations. The
  // pre-G5 historical source may have a different G8 count.
  assert.ok(count<=77,'G8 scoped contract selector unexpectedly broad');
  return result;
}
// A verified UAE-G1 arithmetic correction: the generator legitimately emits
// 19+1, and 20 is the correct result. Allow exactly this verifier bound,
 // not arbitrary edits in the US G1/G11 shared Mathematics engine.
const oldRegroupVerifierLine='            5 => a is >= 11 and <= 19 && b is >= 2 and <= 9 &&';
const newRegroupVerifierLine='            5 => a is >= 11 and <= 19 && b is >= 1 and <= 9 &&';
function assertOriginalLinesUnchanged(ref,file){
  const baseline=file===contracts
    ? withoutGrade8ScopedPracticeAssignments(
        stripOnlyRetiredGrade9Contracts(
          stripOnlyRetiredG8AdvancedContract(
            stripOnlyRetiredG7Contract(priorLines(ref,file)))))
    : file===engine
      ? priorLines(ref,file).map(line=>
          line===oldRegroupVerifierLine ? newRegroupVerifierLine :
          line===oldEngineClass ? newEngineClass :
          line===oldSharedIntegerParser ? newSharedIntegerParser : line)
      : priorLines(ref,file);
  const current=file===contracts
    ? withoutGrade8ScopedPracticeAssignments(nowLines(file))
    : nowLines(file);
  if(file===engine){
    const oldLines=priorLines(ref,file);
    assert.equal(oldLines.filter(x=>x===oldRegroupVerifierLine).length,1,
      'Exactly one previous G1 regrouping verifier bound was present');
    assert.equal(current.filter(x=>x===newRegroupVerifierLine).length,1,
      'Only reviewed G1 19+1 correction is accepted');
    assert.ok(!current.includes(oldRegroupVerifierLine),
      'Prior G1 regrouping false negative must not return');
    assert.ok(current.includes(newEngineClass),'G9 engine partial class required');
    assert.ok(current.includes(newSharedIntegerParser));
    assert.ok(current.some(line=>line.includes('"uae.g9.general.u02.l06.two_sided_proportion"')),
      'Arabic digit normalization must be confined to the G9 exact family');
    assert.ok(current.some(line=>line.includes(': answer;')),
      'Existing curricula must retain the original integer parser');
  }
  if(file===contracts) {
    assert.ok(!current.includes(retiredG7GeneralContractBlock[1]),
      'Retired English G7 contract accidentally reinstated');
    assert.ok(!current.includes(retiredG8AdvancedContractBlock[1]),
      'Retired English G8 Advanced contract accidentally reinstated');
    for(const block of retiredGrade9ContractBlocks)
      assert.ok(!current.includes(block[1]),
        'Owner-retired UAE Grade9 contract accidentally reinstated');
  }
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
// Git's hunk alignment is not a reliable deletion detector when a large
// additive dispatch block repeats existing braces and return statements.
// Check actual historical line multiplicities AND ordered subsequence instead.
// The only permitted lost historical lines remain the exact three below.
function assertExactHistoricalEngineRetirement(ref){
  const before=priorLines(ref,engine), after=nowLines(engine);
  const remaining=new Map();
  for(const line of after)remaining.set(line,(remaining.get(line)||0)+1);
  const lost=[];
  for(const line of before){
    const count=remaining.get(line)||0;
    if(count)remaining.set(line,count-1);
    else lost.push(line);
  }
  assert.deepEqual(lost,[oldEngineClass,oldSharedIntegerParser,oldRegroupVerifierLine],
    'Only the three pinned historical engine lines may be retired');
  assertOriginalLinesUnchanged(ref,engine);
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
    if(file===contracts){
      assert.equal(nowLines(contracts).filter(x=>x.includes(
        'byLesson["PED:UAE-MOE-MATH:L9:GENERAL:') &&
        x.includes('= new LessonPracticeContract(')).length,80,
        'Exactly 80 new G9 digital contract declarations are authorized');
      assert.ok(Number(removed)>=retiredG7GeneralContractBlock.length,
        'The owner-approved G8 code replacement may delete only its pinned legacy declarations');
    }else{
      assertExactHistoricalEngineRetirement(preG5);
    }
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
  assert.ok(probe.includes('if wrong_answer_profile == "uae-g7":'),
    'Grade7 must have its own first-party wrong-answer branch');
  assert.ok(probe.includes('grade7_syntactically_valid_wrong_answer(answer,family)'),
    'Grade7 must retain its scoped category/number response helper');
  assert.ok(probe.includes('elif wrong_answer_profile in ("uae-g2", "uae-g3", "uae-g4", "uae-g5", "uae-g6"):'),
    'Existing Grade2-6 profiles must remain unchanged and explicit');
  assert.ok(probe.includes('grade6_syntactically_valid_wrong_answer(answer,family)'),
    'Existing Grade2-6 negative control helper must remain intact');
  assert.ok(probe.includes('answer=syntactically_valid_wrong_answer(answer,family)'),
    'The independent default/US negative control must not be replaced');
  for(const file of [engine,contracts]){
    if(file===contracts)
      assert.equal(nowLines(contracts).filter(x=>x.includes(
        'byLesson["PED:UAE-MOE-MATH:L9:GENERAL:') &&
        x.includes('= new LessonPracticeContract(')).length,80,
        'G9 may add 80 independent digital declarations without changing older contracts');
    else
      assertExactHistoricalEngineRetirement(preG5);
  }
});

test('UAE G10 additive engine preserves every pinned released line and routes only G10 families',()=>{
  const base='83c31a13ffc173c7be9d1d85b24f4fa83c222aee';
  assert.equal(git('rev-parse',base+':'+engine),'fa4d40a57669f766e4cf2ce69ed6d573c6736d24');
  const before=priorLines(base,engine), after=nowLines(engine);
  const added=[];
  let i=0;
  for(const line of after){
    if(i<before.length && line===before[i]) i++;
    else added.push(line);
  }
  assert.equal(i,before.length,'Released engine source lines must survive in order');
  if(added.length===0)return; // historic released G9 snapshot
  const hash=git('rev-parse','HEAD:'+engine);
  assert.ok(['05a9ad87b9674a60aa3063318aaf79d8ce64b4c6','bba658ea67ad9acaf7889b082d7ea359ae7776b4'].includes(hash));
  assert.equal(added.length,hash==='bba658ea67ad9acaf7889b082d7ea359ae7776b4'?1330:1309);
  // All historical lines and the exact prior G10 engine are still
  // checked in order; the only additional edit is the known normalization.
  if(hash==='bba658ea67ad9acaf7889b082d7ea359ae7776b4'){
    const prior=priorLines('c1e567d943230d60806256db670a44ae95280173',engine).join('\n');
    const exactNewBlock="        // G10 D.2: these eight reused numeric families rejected Arabic\n        // digits in ten declared lesson/family cases. Normalize digits only,\n        // retaining the original parser, parameter bounds and math verifier.\n        if (family is \"geometry.coordinate.gradient_between_points\" or\n            \"geometry.angles.parallel_lines\" or\n            \"geometry.coordinate.evaluate_linear_rule\" or\n            \"geometry.right_triangle.pythagorean.exact\" or\n            \"geometry.right_triangle.pythagorean.find_leg_exact\" or\n            \"supporting.functions.inverse_linear\" or\n            \"supporting.functions.evaluate\" or\n            \"supporting.functions.composite\")\n        {\n            answer = string.Concat((answer ?? string.Empty).Select(ch => ch switch\n            {\n                >= '٠' and <= '٩' => (char)('0' + ch - '٠'),\n                >= '۰' and <= '۹' => (char)('0' + ch - '۰'),\n                '−' => '-',\n                _ => ch\n            }));\n        }\n\n";
    assert.equal(after.join('\n'),prior.replace("        if (family==\"uae.g9.general.u07.l03.multiply_coefficient\")",exactNewBlock+"        if (family==\"uae.g9.general.u07.l03.multiply_coefficient\")"));
  }
  assert.ok(added.every(line=>!(/USCCSS|UsCcss|usccss|UaeG[1-9](?![0-9])/.test(line))));
  assert.ok(added.filter(line=>/Supports\(|family ==/.test(line)).every(line=>/G10|g10/.test(line)));
});
