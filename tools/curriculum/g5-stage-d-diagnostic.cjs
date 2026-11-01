#!/usr/bin/env node
'use strict';
// Grade 5 Stage D diagnostic. A successful preflight or test job never implies academic certification.
const fs=require('node:fs');
const path=require('node:path');
const cp=require('node:child_process');
const ROOT=path.resolve(__dirname,'../..');
const rel='artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
const read=p=>JSON.parse(fs.readFileSync(path.join(ROOT,p),'utf8'));
const exists=p=>typeof p==='string' && !path.isAbsolute(p) && !p.includes('..') && fs.existsSync(path.join(ROOT,p));
const units=[12,17,20,21,26,21,13,18];
function inventory(){
  const ledger=read(rel+'g5-abc-lesson-ledger-v1.json');
  const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json');
  const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json');
  const errors=[];
  const a=ledger.lessons,b=blueprint.Lessons,c=pack.lessons;
  if(!Array.isArray(a)||a.length!==148||!Array.isArray(b)||b.length!==148||!Array.isArray(c)||c.length!==148)
    errors.push('EXACT_148_LESSON_DENOMINATOR_MISMATCH');
  const seen=new Set(); const records=[];
  for(let i=0;i<148;i++){
    const row=a[i], bp=b[i], pc=c[i];
    if(!row||!bp||!pc){errors.push('MISSING_ROW_'+(i+1));continue;}
    const code=row.LessonCode, expectedCode='PED:US-CCSS-MATH:G5:U'+String(units.findIndex((_,u)=>i<units.slice(0,u+1).reduce((p,n)=>p+n,0))+1).padStart(2,'0')+':L'+String(i-units.slice(0,units.findIndex((_,u)=>i<units.slice(0,u+1).reduce((p,n)=>p+n,0))).reduce((p,n)=>p+n,0)+1).padStart(2,'0');
    if(code!==expectedCode)errors.push('LESSON_ORDER_'+(i+1)+':'+code);
    if(bp.LessonCode!==code||pc.lessonCode!==code)errors.push('BLUEPRINT_CONTENT_LEDGER_CODE_MISMATCH_'+(i+1));
    if(seen.has(code))errors.push('DUPLICATE_'+code);seen.add(code);
    if(!row.READY_FOR_STAGE_D)errors.push('NO_A_C_HANDOFF_'+code);
    const test=row.testPath||row.testFile,ev=row.closureEvidencePath;
    if(!exists(test))errors.push('TEST_SOURCE_MISSING_'+code);
    if(!exists(ev))errors.push('SOURCE_CLOSURE_MISSING_'+code);
    if(!pc.translations?.length)errors.push('PACK_TRANSLATION_MISSING_'+code);
    records.push({ordinal:i+1,code,test,evidence:ev});
  }
  return {errors,records,blueprintCount:b?.length,packCount:c?.length,ledgerCount:a?.length};
}
function preflight(){
  const q=inventory();
  console.log('G5_STAGE_D_MANIFEST '+JSON.stringify({expected:148,blueprint:q.blueprintCount,pack:q.packCount,ledger:q.ledgerCount,linkage:q.records.length,errors:q.errors.slice(0,30),errorCount:q.errors.length}));
  if(q.errors.length)process.exitCode=1;
}
function shard(name){
  const ranges={D1:[1,50],D2:[51,100],D3:[101,148]};
  if(!ranges[name])throw Error('Bad shard name');
  const q=inventory();
  if(q.errors.length)throw Error('Preflight failed: '+q.errors.slice(0,6));
  const [lo,hi]=ranges[name], rows=q.records.filter(x=>x.ordinal>=lo&&x.ordinal<=hi);
  if(rows.length!==hi-lo+1)throw Error('Shard denominator mismatch');
  const node=[...new Set(rows.filter(x=>x.test.endsWith('.cjs')).map(x=>x.test))].sort();
  const cs=rows.filter(x=>x.test.endsWith('.cs')).length;
  console.log('G5_STAGE_D_SHARD '+JSON.stringify({name,range:[lo,hi],lessons:rows.length,nodeFileCount:node.length,dotnetLinkedLessons:cs,tests:node}));
  if(node.length){
    const p=cp.spawnSync(process.execPath,['--test',...node],{cwd:ROOT,stdio:'inherit',timeout:25*60*1000});
    if(p.error)throw p.error;
    if(p.status!==0)process.exitCode=p.status||1;
  }
  console.log('G5_STAGE_D_SHARD_END '+name+' node='+node.length+' cs='+cs+' nodeResult='+process.exitCode);
}
function academic(){
  const q=inventory(); const blockers=[...q.errors];
  for(const row of q.records){
    const source=read(row.evidence);
    if(source.status==='A_C_EXPLICIT_GAP'||source.academicAccepted===false||source.sourceCoverage100Percent===false)
      blockers.push(row.code+':A_C_SOURCE_ACCEPTANCE_NOT_PROVEN');
    // The source lacks verified per-element artwork/rights and authorized human signoffs.
    if(source.sourceRights?.status==='RIGHTS_RESTRICTED')blockers.push(row.code+':RIGHTS_RESTRICTED');
  }
  // No Stage D test evidence or authorized human reviewer in the handoff; never promote on static files alone.
  blockers.push('G5_TECHNICAL_AND_AUTHENTICATED_E2E_CANDIDATE_EVIDENCE_NOT_COLLECTED');
  blockers.push('G5_ORIGINAL_IMAGE_PDF_RIGHTS_AND_TEACHER_APPROVAL_NOT_CERTIFIED');
  console.error('G5_STAGE_D_ACADEMIC_NO_GO '+JSON.stringify({decision:'NO_GO_TECHNICAL',blockerCount:blockers.length,sample:blockers.slice(0,35)}));
  process.exitCode=2;
}
const mode=process.argv[2]||'preflight';
if(mode==='preflight')preflight();
else if(mode==='shard')shard(process.argv[3]);
else if(mode==='academic')academic();
else throw Error('Unknown mode '+mode);
