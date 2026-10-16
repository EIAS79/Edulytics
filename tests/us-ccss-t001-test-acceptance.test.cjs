'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const {evaluateChecks,fetchChecks,required}=require('../tools/us-ccss-t001-test-acceptance.cjs');
const sha='a'.repeat(40);
const result = (name, conclusion = 'success', id = 1) => ({
  name, head_sha:sha, status:'completed', conclusion, id,
  app:{slug:'github-actions'},
  html_url:'https://github.com/EIAS79/Edulytics/actions/runs/123'
});
test('accepts exactly three genuine, successful, same-SHA checks',()=>{
  const outcome=evaluateChecks(required.map(name=>result(name)),sha);
  assert.equal(outcome.state,'BATCH_TESTS_ACCEPTED');
  assert.equal(outcome.requiresHumanAcademicReview,false);
  assert.equal(outcome.isAcademicCertification,false);
});
test('missing/pending/failed/wrong-SHA/non-Actions checks block acceptance',()=>{
  for(const change of ['missing','pending','failure','sha','app','url']){
    const checks=required.map(name=>result(name));
    if(change==='missing')checks.pop();
    if(change==='pending'){checks[0].status='in_progress';checks[0].conclusion=null;}
    if(change==='failure')checks[0].conclusion='failure';
    if(change==='sha')checks[0].head_sha='b'.repeat(40);
    if(change==='app')checks[0].app.slug='untrusted-runner';
    if(change==='url')checks[0].html_url='https://example.com/fake-check';
    assert.equal(evaluateChecks(checks,sha).state,'BATCH_TESTS_BLOCKED',change);
  }
});
test('latest rerun failure overrides an earlier green run',()=>{
  const checks=required.map(name=>result(name,'success',1));
  checks.push(result(required[0],'failure',2));
  assert.equal(evaluateChecks(checks,sha).state,'BATCH_TESTS_BLOCKED');
});
test('rejects invalid SHA and malformed checks responses',()=>{
  assert.throws(()=>evaluateChecks([], 'short'), /SHA/);
  assert.throws(()=>evaluateChecks(null, sha), /list/);
});
test('GitHub provider transport rejects HTTP errors and malformed payloads',async()=>{
  await assert.rejects(fetchChecks('EIAS79/Edulytics',sha,null,
    async()=>({ok:false,status:403})),/403/);
  await assert.rejects(fetchChecks('EIAS79/Edulytics',sha,null,
    async()=>({ok:true,json:async()=>({foo:[]})})),/payload/);
});
