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
 'src/Edulytics.Core/Curriculum/LessonContent/**',
 'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/**',
 'tests/Edulytics.Tests/MathematicsIntelligence/UaeG9*.cs',
 'tests/academic-uae-grade9*.test.cjs'
 ];
 for(const glob of globs)assert.equal(workflow.split(glob).length-1,2,glob);
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
