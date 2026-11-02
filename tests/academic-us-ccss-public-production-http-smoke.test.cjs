'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {checkPublicEndpoint,base,endpoints}=require('../tools/us-ccss-public-production-http-smoke.cjs');
function response(status=200,url='https://edulytics-4346.onrender.com/health/live'){
 return {status,url,body:{cancel:async()=>{}}};
}
test('exact four allowlisted anonymous GET endpoints and fixed production origin',async()=>{
 assert.equal(base.origin,'https://edulytics-4346.onrender.com');
 assert.deepEqual(endpoints,['/health/live','/health/ready',
  '/account/login?culture=en','/']);
 for(const path of endpoints){
  let called=0;
  const f=async(url,options)=>{
   called++;
   assert.equal(new URL(url).origin,base.origin);
   assert.equal(options.method,'GET');
   assert.equal(options.redirect,'follow');
   return response(200,url);
  };
  assert.equal((await checkPublicEndpoint(f,path)).status,200);
  assert.equal(called,1);
 }
});
test('wrong status, redirect and non-allowlisted endpoints fail closed',async()=>{
 await assert.rejects(()=>checkPublicEndpoint(async url=>response(503,url),
  '/health/ready'),/Unexpected HTTP 503/);
 await assert.rejects(()=>checkPublicEndpoint(async()=>response(200,'https://other.example/'),
  '/health/live'),/Cross-origin redirect/);
 let invoked=false;
 await assert.rejects(()=>checkPublicEndpoint(async()=>{invoked=true},
  '/admin/user-management'),/Unapproved anonymous endpoint/);
 assert.equal(invoked,false);
});
