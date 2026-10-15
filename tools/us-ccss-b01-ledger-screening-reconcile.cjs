#!/usr/bin/env node
'use strict';
// Reconcile actually generated B01 learner-body fingerprints with the canonical
// audit ledger. Produces a PROPOSAL: no L1-L6 PASS, no new dimension PASS,
// no academic certification, no mutation of the checked-in ledger by default.
const fs=require('node:fs');
const path=require('node:path');
const {execFileSync}=require('node:child_process');
const {loadSource,validateLedger}=require('./us-ccss-lesson-certification-records.cjs');
const root=path.resolve(__dirname,'..');
const isSha=x=>typeof x==='string'&&/^[a-f0-9]{64}$/.test(x);
const isCommit=x=>typeof x==='string'&&/^[a-f0-9]{40}$/.test(x);
const assert=(c,msg)=>{if(!c)throw Error(msg)};
const read=p=>JSON.parse(fs.readFileSync(path.resolve(root,p),'utf8'));
function reconcile({manifest,source,ledger,runtime,headSha,runId,atUtc}){
 assert(isCommit(headSha),'Actual audited HEAD SHA required');
 assert(/^[1-9][0-9]*$/.test(String(runId)),'Positive GitHub run ID required');
 assert(!Number.isNaN(Date.parse(atUtc))&&typeof atUtc==='string'&&atUtc.endsWith('Z'),
    'UTC screening timestamp required');
 assert(manifest.batchId==='B01'&&manifest.lessons?.length===500,
    'Exactly 500 B01 manifest lessons required');
 assert(source.records?.length===1560&&ledger.records?.length===1560,
    'Exactly 1,560 original audit and ledger records required');
 assert(runtime.kind==='B01_500_EFFECTIVE_CONTENT_AND_REAL_PRACTICE_RUNTIME_PROBE' &&
   runtime.inspected===500&&runtime.unique===500&&runtime.runtimeFailures===0 &&
   runtime.lessonRows?.length===500,'Successful complete 500-row runtime required');
 const output=structuredClone(ledger);
 const changes=[],skipped=[];
 let preservedCertified=0;
 for(let i=0;i<500;i++){
  const original=source.records[i],m=manifest.lessons[i],r=runtime.lessonRows[i];
  const l=output.records[i];
  assert(m?.ordinal===i+1&&r?.ordinal===i+1&&
    m.lessonCode===r.lessonCode&&original.lessonCode===r.lessonCode&&
    l.lessonCode===r.lessonCode,'Unordered/missing source lesson '+(i+1));
  assert(m.sourceSemanticSha256===original.sourceSemanticSha256,
    'Original teacher source SHA drift: '+l.lessonCode);
  assert(isSha(r.effectiveContentSha256)&&
    typeof r.effectiveContentVersion==='string'&&r.effectiveContentVersion.length>0 &&
    typeof r.sourcePreparationUrl==='string'&&
    r.sourcePreparationUrl.startsWith('https://im.kendallhunt.com/') &&
    r.generatedCases>0&&(r.engineIssues?.length??-1)===0,
    'Incomplete actual student-content and practice probe: '+l.lessonCode);
  if(l.status==='INTERNALLY_CERTIFIED'){
   assert(l.academicDecision==='INTERNALLY_CERTIFIED'&&
     l.contentSha256===r.effectiveContentSha256,
     'Historic certified lesson must not be invalidated: '+l.lessonCode);
   preservedCertified++;skipped.push(l.lessonCode);continue;
  }
  assert(l.academicDecision==='NOT_CERTIFIED'&&
    (l.status==='QUEUED'||l.status==='AUTOMATED_SCREENED')&&
    l.reviewDimensions===null&&l.unresolvedDefects.length===0&&
    Object.values(l.gates).every(x=>x.status==='NOT_STARTED'),
    'Screening cannot overwrite started academic review: '+l.lessonCode);
  assert(l.contentSha256===null||l.contentSha256===r.effectiveContentSha256,
    'Effective learner-body fingerprint changed since review: '+l.lessonCode);
  l.contentSha256=r.effectiveContentSha256;
  l.contentVersion=r.effectiveContentVersion;
  l.status='AUTOMATED_SCREENED';
  l.screeningProvenance={
   type:'RUNTIME_MATH_AND_CONTENT_SCREEN_ONLY_NOT_CERTIFICATION',
   githubRunId:String(runId),auditedHeadSha:headSha,recordedAtUtc:atUtc,
   runtimeArtifact:'b01-effective-content-and-practice',
   learnerContentSha256:r.effectiveContentSha256
  };
  changes.push(l.lessonCode);
 }
 return {ledger:output,summary:{
  kind:'B01_500_LEDGER_SCREENING_RECONCILIATION_PROPOSAL',
  auditedHeadSha:headSha,githubRunId:String(runId),
  checked:500,screened:changes.length,preservedCertified,
  refusedToAwardPass:true,academicDecision:'NOT_500_BATCH_CERTIFIED',
  stillNeedFullL1L6AndEightDimensions:changes.length,
  unchangedOtherCurriculumRows:output.records.length-500,
  note:'A 500-row successful runtime does not prove academic correctness or browser L6. Review and apply the ledger proposal separately; do NOT use it as release approval.'
 }};
}
function main(){
 const args=process.argv.slice(2);
 const arg=(n,defaultValue)=>args.find(x=>x.startsWith('--'+n+'='))?.slice(n.length+3)??defaultValue;
 const headSha=arg('head-sha', '');
 assert(isCommit(headSha),'--head-sha=40hex is required');
 const checkedOut=execFileSync('git',['rev-parse','HEAD'],{cwd:root,encoding:'utf8'}).trim();
 assert(checkedOut===headSha,'Runtime must be generated from the actual audited checkout SHA');
 const runId=arg('run-id','');
 const manifest=read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json');
 const source=loadSource();
 const ledger=read('artifacts/academic-us-ccss/04-lesson-certification-records.json');
 validateLedger(source,ledger);
 const runtime=read(arg('runtime','b01-effective-content-and-practice.json'));
 const result=reconcile({manifest,source,ledger,runtime,headSha,runId,
   atUtc:new Date().toISOString().replace(/\.[0-9]{3}Z$/,'Z')});
 validateLedger(source,result.ledger);
 const out=arg('proposal','b01-ledger-screening-proposal-1560.json');
 const summaryFile=arg('summary','b01-ledger-screening-proposal-summary.json');
 // Deliberately refuse to overwrite the canonical ledger or any source file.
 assert(path.resolve(root,out)!==path.resolve(root,
   'artifacts/academic-us-ccss/04-lesson-certification-records.json'),
   'Refuse to overwrite live certification ledger; proposal must be reviewed');
 fs.writeFileSync(path.resolve(root,out),JSON.stringify(result.ledger,null,2)+'\n');
 fs.writeFileSync(path.resolve(root,summaryFile),
   JSON.stringify(result.summary,null,2)+'\n');
 console.log(JSON.stringify(result.summary));
}
if(require.main===module){try{main()}catch(e){console.error('B01 screening reconciliation: '+e.message);process.exitCode=1}}
module.exports={reconcile};
