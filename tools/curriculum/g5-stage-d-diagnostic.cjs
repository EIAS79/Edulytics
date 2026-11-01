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
    // Node's standard exit code is zero even for unimplemented TODO assertions.
    // A nonempty TODO count must not be reported as a Stage D success.
    const p=cp.spawnSync(process.execPath,['--test',...node],{cwd:ROOT,encoding:'utf8',timeout:25*60*1000,maxBuffer:32*1024*1024});
    if(p.stdout)process.stdout.write(p.stdout);
    if(p.stderr)process.stderr.write(p.stderr);
    if(p.error)throw p.error;
    if(p.status!==0)process.exitCode=p.status||1;
    const summary=(p.stdout||'').match(/(?:^|\\n)(?:[^\\n]*?)(?:ℹ todo|# todo)\\s+(\\d+)\\s*(?:\\n|$)/);
    const todos=summary?Number(summary[1]):null;
    if(todos===null){
      console.error('G5_STAGE_D_NODE_SUMMARY_MISSING: cannot prove there are zero TODO tests');
      process.exitCode=2;
    }else if(todos!==0){
      console.error('G5_STAGE_D_NODE_TODO_BLOCKED '+JSON.stringify({batch:name,todo:todos,reason:'Outstanding runtime/source cases are not passing tests'}));
      process.exitCode=2;
    }
  }else console.log('G5_STAGE_D_DOTNET_ONLY: Node count 0; C# unit tests are executed in the separate shared .NET job');
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

function sourceEvidenceAudit(){
  const q=inventory();
  const rows=[];
  for(const item of q.records){
    const evidence=read(item.evidence);
    const sourceElements=Array.isArray(evidence.lessonElements)?evidence.lessonElements:
      Array.isArray(evidence.elements)?evidence.elements:null;
    const identified=sourceElements?.length ?? (Number.isInteger(evidence.sourceElementsKnown)?evidence.sourceElementsKnown:null);
    const matched=sourceElements?.filter(x=>x.Status==='MATCHED_DIGITAL'||x.Status==='MATCHED_TEACHER_LED').length ?? null;
    const statusCounts={};
    if(sourceElements)for(const e of sourceElements){
      const status=typeof e.Status==='string'?e.Status:'UNKNOWN';
      statusCounts[status]=(statusCounts[status]||0)+1;
    }
    const blockers=[];
    if(evidence.status==='A_C_EXPLICIT_GAP')blockers.push('A_C_EXPLICIT_GAP');
    if(evidence.sourceCoverage100Percent!==true)blockers.push('SOURCE_PARITY_NOT_INDEPENDENTLY_PROVEN');
    if(evidence.academicAccepted!==true)blockers.push('ACADEMIC_SIGNOFF_NOT_PROVEN');
    if(sourceElements===null)blockers.push('SOURCE_ITEMIZED_DENOMINATOR_UNKNOWN');
    if(sourceElements&&Number.isInteger(evidence.sourceElementsKnown)&&evidence.sourceElementsKnown!==sourceElements.length)blockers.push('SOURCE_ITEM_COUNT_MISMATCH');
    if(sourceElements&&matched!==sourceElements.length)blockers.push('UNMATCHED_IDENTIFIED_SOURCE_ELEMENTS');
    if(sourceElements?.some(x=>typeof x.SourceSnapshotId!=='string'||!x.SourceSnapshotId.trim()))blockers.push('MISSING_ELEMENT_SOURCE_SNAPSHOT');
    const acceptedReviewer=typeof evidence.academicReviewerSignature==='string'&&evidence.academicReviewerSignature.trim().length>0;
    if(!acceptedReviewer)blockers.push('AUTHORIZED_HUMAN_REVIEWER_NOT_EVIDENCED');
    rows.push({ordinal:item.ordinal,lessonCode:item.code,sourceEvidencePath:item.evidence,
      sourceSnapshotId:evidence.sourceSnapshotId||null,identifiedElementCount:identified,
      itemizedElementCount:sourceElements?.length??null,matchedElementCount:matched,
      sourceElementStatusCounts:statusCounts,sourceParity:evidence.sourceCoverage100Percent===true?'CLAIMED_IN_AC_REQUIRES_INDEPENDENT_VALIDATION':'UNKNOWN',
      academicApproval:evidence.academicAccepted===true?'CLAIMED_IN_AC_REQUIRES_SIGNED_REVIEW':'NOT_APPROVED',
      blockers});
  }
  const result={schemaVersion:1,reportType:'SOURCE_ELEMENT_EVIDENCE_DIAGNOSTIC_NOT_ACADEMIC_CERTIFICATION',
    gitCandidateSha:cp.execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim(),
    scopeLessons:148,lessonCount:rows.length,
    itemizedLessonCount:rows.filter(x=>x.itemizedElementCount!==null).length,
    unknownSourceDenominatorLessons:rows.filter(x=>x.itemizedElementCount===null).length,
    sourceParityVerifiedLessons:0,academicApprovedLessons:0,
    lessonsWithExplicitACGap:rows.filter(x=>x.blockers.includes('A_C_EXPLICIT_GAP')).map(x=>x.lessonCode),
    blockedLessons:rows.filter(x=>x.blockers.length).length,rows};
  fs.mkdirSync(path.join(ROOT,'stage-d-results'),{recursive:true});
  fs.writeFileSync(path.join(ROOT,'stage-d-results/grade5-source-evidence.json'),JSON.stringify(result,null,2)+'\\n');
  console.log('G5_SOURCE_EVIDENCE_AUDIT '+JSON.stringify({candidateSha:result.gitCandidateSha,scope:result.scopeLessons,
    itemized:result.itemizedLessonCount,unknownDenominators:result.unknownSourceDenominatorLessons,
    explicitGaps:result.lessonsWithExplicitACGap,blocked:result.blockedLessons,
    artifact:'stage-d-results/grade5-source-evidence.json'}));
  if(q.errors.length||result.blockedLessons)process.exitCode=2;
}

const mode=process.argv[2]||'preflight';
if(mode==='preflight')preflight();
else if(mode==='shard')shard(process.argv[3]);
else if(mode==='academic')academic();
else if(mode==='source-evidence')sourceEvidenceAudit();
else throw Error('Unknown mode '+mode);
