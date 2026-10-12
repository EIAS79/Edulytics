#!/usr/bin/env node
'use strict';
/*
 * Phase 09: independent, unauthenticated public HTTP production smoke.
 * This does NOT prove the responding Render app's commit SHA, authenticated
 * student/teacher/supervisor behavior, or academic curriculum signoff.
 * Checks fixed known public endpoints only; does not send any credentials.
 */
const base=new URL('https://edulytics-4346.onrender.com/');
const endpoints=[
 '/health/live',
 '/health/ready',
 '/account/login?culture=en',
 '/'
];
const must=(value,message)=>{if(!value)throw Error(message)};
async function checkPublicEndpoint(fetcher,endpoint){
 must(endpoints.includes(endpoint),'Unapproved anonymous endpoint '+endpoint);
 const expected=new URL(endpoint,base);
 const res=await fetcher(expected.toString(),{
  method:'GET',redirect:'follow',cache:'no-store',
  signal:AbortSignal.timeout(35000),
  headers:{'Accept':'text/html, application/json, text/plain;q=0.9'}
 });
 must(res && Number.isInteger(res.status),'No numeric HTTP response from '+endpoint);
 must(new URL(res.url).origin===base.origin,
  'Cross-origin redirect on '+endpoint+'; refusing to certify destination');
 must(res.status===200,
  'Unexpected HTTP '+res.status+' at '+endpoint);
 if(typeof res.body?.cancel==='function')await res.body.cancel();
 return {endpoint,status:res.status,origin:base.origin};
}
async function runLive(){
 const results=[];
 for(const endpoint of endpoints){
  let lastError;
  for(let attempt=1;attempt<=2;attempt++){
   try{
    const result=await checkPublicEndpoint(fetch,endpoint);
    results.push(result);
    console.log('PASS '+endpoint+' HTTP '+result.status);
    lastError=null;
    break;
   } catch(err){
    lastError=err;
    if(attempt<2)await new Promise(resolve=>setTimeout(resolve,1500));
   }
  }
  if(lastError)throw new Error(endpoint+': '+lastError.message);
 }
 console.log('ANONYMOUS_HTTP_SMOKE_ONLY: '+results.length+
  ' paths. No authenticated role acceptance and no deployed SHA attestation.');
 return results;
}
if(require.main===module){
 runLive().catch(e=>{console.error('FAIL public HTTP smoke: '+e.message);process.exitCode=1;});
}
module.exports={base,endpoints,checkPublicEndpoint,runLive};
