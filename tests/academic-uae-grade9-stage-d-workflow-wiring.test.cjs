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
 'tests/academic-uae-grade9*.test.cjs'
 ];
 for(const glob of globs)assert.equal(workflow.split(glob).length-1,2,glob);
 assert.equal(workflow.split('src/Edulytics.Core/Curriculum/LessonContent/**').length-1,1);
 assert.match(workflow,/pull_request:/);assert.match(workflow,/push:/);
});
test('Stage D concurrency cancels stale same-event runs, while source gate and zero-test failures remain fail-closed',()=>{
 assert.ok(workflow.includes('uae-g9-stage-d-${{ github.event_name }}-'));
 assert.ok(workflow.includes('github.event.pull_request.number || github.ref'));
 assert.ok(workflow.includes('cancel-in-progress: true'));
 assert.ok(workflow.includes('node --test tests/academic-uae-grade9-strict-source-index-gate.test.cjs'));
 assert.ok(workflow.includes('dotnet test tests/Edulytics.Tests/Edulytics.Tests.csproj'));
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
