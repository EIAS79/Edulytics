'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const workflow=fs.readFileSync(path.join(root,'.github/workflows/uae-grade9-general-stage-d.yml'),'utf8');
test('Grade 9 Stage D path filters cover original lesson HTML, content, native and Node tests on both events',()=>{
 const globs=[
 'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/**',
 'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/**',
 'tests/Edulytics.Tests/MathematicsIntelligence/UaeG9*.cs',
 'tests/Edulytics.Tests/MathematicsIntelligence/UaeGrade9ArabicPathwayPolicyTests.cs',
 'tests/academic-uae-grade9*.test.cjs',
 'tools/curriculum/uae_g9_stage_d_semantic_gate.py'
 ];
 const pushPaths=workflow.split('  push:')[1].split('  pull_request:')[0];
 const pullPaths=workflow.split('  pull_request:')[1].split('  workflow_dispatch:')[0];
 for(const glob of globs){
  assert.ok(pushPaths.includes("'"+glob+"'"),'push '+glob);
  assert.ok(pullPaths.includes("'"+glob+"'"),'PR '+glob);
 }
 assert.equal(workflow.split('src/Edulytics.Core/Curriculum/LessonContent/**').length-1,1);
 assert.match(workflow,/pull_request:/);assert.match(workflow,/push:/);
});
test('Stage D concurrency cancels stale same-event runs, while source gate and zero-test failures remain fail-closed',()=>{
 assert.ok(workflow.includes('uae-g9-stage-d-${{ github.event_name }}-'));
 assert.ok(workflow.includes('github.event.pull_request.number || github.ref'));
 assert.ok(workflow.includes('cancel-in-progress: true'));
 assert.ok(workflow.includes('node --test tests/academic-uae-grade9-strict-source-index-gate.test.cjs'));
 assert.ok(workflow.includes('python3 tools/curriculum/uae_g9_stage_d_semantic_gate.py --json'));
 assert.ok(workflow.includes('grade9-python-source-parity'));
 assert.ok(workflow.includes('dotnet test tests/Edulytics.Tests/Edulytics.Tests.csproj'));
 assert.ok(workflow.includes('FullyQualifiedName~UaeGrade9ArabicPathwayPolicyTests'));
 assert.ok(workflow.includes('FullyQualifiedName~UaeGrade7ArabicPathwayPolicyTests'));
 const push=workflow.split('  push:')[1].split('  pull_request:')[0];
 const pr=workflow.split('  pull_request:')[1].split('  workflow_dispatch:')[0];
 for(const file of [
   'tests/Edulytics.Tests/MathematicsIntelligence/UaeGrade9ArabicPathwayPolicyTests.cs'
 ]) {
   assert.ok(push.includes("'"+file+"'"));
   assert.ok(pr.includes("'"+file+"'"));
 }
 for(const file of [
   'src/Edulytics.Services/LessonContent/LessonContentPolicy.cs',
   'src/Edulytics.Services/LessonContent/LessonContentService.cs'
 ]) assert.ok(push.includes("'"+file+"'"));

 for(const x of ['NO_G9_TRX','ZERO_G9_TESTS','FAILED_G9_TESTS'])assert.ok(workflow.includes(x));
});
test('Grade 9 pull-request source gate does not inadvertently block unrelated shared-engine or other grade content changes',()=>{
 const pr=workflow.split('  pull_request:')[1]?.split('  workflow_dispatch:')[0] || '';
 assert.ok(pr);
 for(const shared of [
  'src/Edulytics.Core/Curriculum/LessonContent/**',
  'src/Edulytics.Core/Mathematics/**',
  'src/Edulytics.Services/Mathematics/**'
 ]) assert.ok(!pr.includes(shared),shared);
 for(const specific of [
  'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-*',
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-*',
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g9-general-*'
 ]) assert.ok(pr.includes(specific),specific);
 const push=workflow.split('  push:')[1]?.split('  pull_request:')[0] || '';
 assert.ok(push.includes('src/Edulytics.Services/Mathematics/**'));
 assert.ok(push.includes('src/Edulytics.Core/Mathematics/**'));
});


test('Grade 7 owner-approved first-party path is release-required; exact publisher parity remains genuinely red but separately reported',()=>{
 const firstParty=workflow.split('      - name: G7-parity owner first-party source acceptance')[1].split('      - name: Upload Grade 9 first-party source acceptance evidence')[0];
 assert.ok(firstParty.includes('--phase structural'));
 assert.ok(firstParty.includes('--phase source'));
 assert.ok(firstParty.includes('FIRST_PARTY_'));
 assert.ok(!firstParty.includes('continue-on-error'));
 const node=workflow.split('      - name: Run prepared Grade 9 Arabic tests')[1].split('      - name: Upload prepared')[0];
 assert.ok(!node.includes('continue-on-error'));
 const literalNode=workflow.split('      - name: Strict source-index lock')[1].split('      - name: Independent Python')[0];
 const literalPython=workflow.split('      - name: Independent Python source-parity evidence gate')[1].split('      - name: Verify independently reported publisher parity')[0];
 assert.ok(literalNode.includes('id: publisher_literal_node'));
 assert.ok(literalPython.includes('id: publisher_literal_python'));
 for(const body of [literalNode,literalPython])assert.ok(body.includes('continue-on-error: true'));
 const proof=workflow.split('      - name: Verify independently reported publisher parity')[1].split('      - name: Upload independent Python')[0];
 assert.ok(!proof.includes('continue-on-error'));
 for(const marker of ['SOURCE_DIAGNOSTIC_INVALID_EVIDENCE_NOT_WAIVED','SOURCE_DIAGNOSTIC_FALSE_PASS','SOURCE_DIAGNOSTIC_INCONSISTENT_RED_STATUS','GITHUB_STEP_SUMMARY'])assert.ok(proof.includes(marker));
});

test('All three Grade9 Node/Python CI pipelines propagate test errors despite tee log collection',()=>{
 const node=workflow.split('      - name: Run prepared Grade 9 Arabic tests')[1].split('      - name: Upload prepared')[0];
 const strict=workflow.split('      - name: Strict source-index lock')[1].split('      - name: Independent Python')[0];
 const python=workflow.split('      - name: Independent Python source-parity evidence gate')[1].split('      - name: Upload independent')[0];
 for(const body of [node,strict,python])assert.ok(body.includes('set -o pipefail'),body);
 assert.ok(node.includes('node --test'));
 assert.ok(strict.includes('academic-uae-grade9-strict-source-index-gate.test.cjs'));
 assert.ok(python.includes('uae_g9_stage_d_semantic_gate.py --json'));
});
