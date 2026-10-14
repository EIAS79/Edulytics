#!/usr/bin/env node
'use strict';
// V4.3: reuse immutable GitHub Actions evidence, never repeat the publisher scan.
// This report is triage; it neither edits the certification ledger nor marks PASS.
const fs=require('node:fs');
const assert=(ok,message)=>{if(!ok)throw Error(message)};
const sha64=x=>typeof x==='string'&&/^[0-9a-f]{64}$/i.test(x);
function join({manifest,visual,activity,unified,progress,baselineRunId,baselineSha}) {
  assert(manifest.batchId==='B01'&&manifest.lessons?.length===500,'Wrong B01 manifest');
  assert(visual.expected===500&&visual.academicDecision==='NOT_CERTIFIED','Unverified visual baseline');
  assert(activity.expected===500&&activity.academicDecision==='NOT_CERTIFIED','Unverified teacher baseline');
  assert(unified.batchId==='B01'&&unified.coverage?.joined===500&&unified.coverage?.unique===500
    &&unified.batchDecision==='BATCH_NO-GO','Unverified disposition baseline');
  assert(progress.batchId==='B01'&&progress.expected===500&&progress.observed===500
    &&progress.state==='BATCH_NO-GO','Unverified ledger evidence baseline');
  assert(Number.isInteger(baselineRunId)&&baselineRunId>0
    &&/^[a-f0-9]{40}$/i.test(baselineSha),'Missing immutable baseline provenance');
  const sources=[['visual',visual.rows],['activity',activity.rows],
    ['unified',unified.lessons],['progress',progress.lessons]];
  for(const [name,rows] of sources)
    assert(Array.isArray(rows)&&rows.length===500,'Expected 500 '+name+' rows');
  const groups=new Map(),queue=[],missingByGate={};
  let publisherOnly=0,teacherOnly=0,overlap=0,neither=0,confirmed=0,missing=0,classifier=0;
  function group(kind,type,code) {
    const key=kind+':'+type;
    if(!groups.has(key))groups.set(key,{key,kind,issueType:type,affectedLessonCodes:[],
      status:kind==='CONFIRMED_DEFECT'?'CONFIRMED_DEFECT':'REVIEW_CUE_NOT_DEFECT'});
    groups.get(key).affectedLessonCodes.push(code);
    return key;
  }
  for(let i=0;i<500;i++){
    const m=manifest.lessons[i],v=visual.rows[i],a=activity.rows[i],
      u=unified.lessons[i],p=progress.lessons[i],code=m.lessonCode;
    assert(m.ordinal===i+1&&sha64(m.sourceSemanticSha256),'Bad manifest row '+i);
    for(const [name,rows] of sources)
      assert(rows[i].ordinal===i+1&&rows[i].lessonCode===code,
        name+' missing/reordered lesson '+code);
    assert(m.sourceSemanticSha256===u.sourceSemanticSha256
      &&m.sourceSemanticSha256===a.sourceSemanticSha256
      &&m.sourceSemanticSha256===p.sourceSemanticSha256,
      'Source semantic SHA drift '+code);
    const body=u.effectiveContentSha256;
    assert(sha64(body)&&body===v.sourceEffectiveContentSha256
      &&body===v.richV2EffectiveContentSha256
      &&body===a.effectiveContentSha256&&body===p.effectiveContentSha256
      &&v.sameEffectiveContent===true,'Effective content SHA drift '+code);
    const cues=[...new Set(v.publisherCueVisualReviewTypes||[])].sort();
    const teacher=[...new Set(a.candidateMissingRepresentations||[])].sort();
    const defects=u.confirmedDefects||[];
    assert(JSON.stringify(defects)===JSON.stringify(p.confirmedDefects||[]),
      'Defect disagreement '+code);
    if(cues.length&&teacher.length)overlap++;
    else if(cues.length)publisherOnly++;
    else if(teacher.length)teacherOnly++;
    else neither++;
    if(defects.length)confirmed++;
    if(!p.evidenceReady)missing++;
    for(const [gate,rec] of Object.entries(p.sixGateEvidence||{}))
      if(rec.status!=='LEDGER_PASS')missingByGate[gate]=(missingByGate[gate]||0)+1;
    const groupKeys=[];
    for(const cue of cues)groupKeys.push(group('PUBLISHER_VISUAL_REVIEW',cue,code));
    for(const cue of teacher)groupKeys.push(group('TEACHER_ACTIVITY_REVIEW',cue,code));
    for(const defect of defects){
      assert(defect.code,'Defect without code '+code);
      groupKeys.push(group('CONFIRMED_DEFECT',defect.code,code));
    }
    const kinds=v.actualRichV2VisualKinds||[];
    const possibleClassifierOvermatch=
      (cues.includes('GRAPH_AND_SCALE')&&kinds.includes('PictureGraph'))||
      (cues.includes('FRACTION_PARTS')&&kinds.includes('LinePlot')&&
       /fractional measurements on line plots/i.test(v.publisherTitle||''));
    if(possibleClassifierOvermatch)classifier++;
    if(groupKeys.length)queue.push({ordinal:i+1,lessonCode:code,title:m.title,
      sourceUrl:m.sourceUrl,sourceSemanticSha256:m.sourceSemanticSha256,
      effectiveContentSha256:body,visualKinds:kinds,
      publisherVisualCues:cues,teacherActivityCues:teacher,
      confirmedDefects:defects,groupKeys,
      possibleClassifierOvermatch,
      reviewState:'REVIEW_REQUIRED_NOT_CERTIFIED'});
  }
  const counts={publisherOnly,teacherOnly,overlap,neither,
    publisherCues:publisherOnly+overlap,teacherCues:teacherOnly+overlap,
    reviewUnion:500-neither,confirmedDefectLessons:confirmed,
    missingEvidenceLessons:missing,potentialClassifierOvermatches:classifier};
  assert(counts.publisherCues===visual.lessonsNeedingPedagogicalVisualReview,
    'Publisher review count drift');
  assert(counts.teacherCues===activity.withReviewCues,'Teacher review count drift');
  assert(missing===progress.remainingEvidence,'Evidence gap count drift');
  const rootCauseGroups=[...groups.values()].map(g=>({...g,count:g.affectedLessonCodes.length}))
    .sort((a,b)=>(a.kind==='CONFIRMED_DEFECT'?-1:0)-
      (b.kind==='CONFIRMED_DEFECT'?-1:0)||b.count-a.count||
      a.key.localeCompare(b.key));
  return {schemaVersion:1,kind:'B01_V4_3_REUSED_ROOT_CAUSE_QUEUE',
    mode:'REUSE_EXISTING_GITHUB_ACTIONS_ARTIFACTS_NO_500_RESCAN',
    baseline:{runId:baselineRunId,codeSha:baselineSha,exactLessonCount:500,
      reusedArtifacts:['b01-rich-v2-publisher-visual-gaps',
      'b01-original-teacher-activities-500',
      'b01-unified-eight-dimension-report-500',
      'b01-collective-evidence-progress-500']},
    counts,missingByGate,rootCauseGroups,queue,
    releaseEligible:false,academicDecision:'BATCH_NO-GO',
    warning:'Review cues and possible classifier errors are NOT confirmed defects or L4 PASS. Lessons without cues still lack per-lesson L1-L6 evidence. This tool never writes PASS.'};
}
function run(argv=process.argv.slice(2)){
  const val=k=>{const i=argv.indexOf('--'+k);assert(i>=0&&argv[i+1],'Missing --'+k);return argv[i+1]};
  const read=k=>JSON.parse(fs.readFileSync(val(k),'utf8'));
  const r=join({manifest:read('manifest'),visual:read('visual'),activity:read('activity'),
    unified:read('unified'),progress:read('progress'),baselineRunId:Number(val('run-id')),
    baselineSha:val('baseline-sha')});
  const out=val('output');
  fs.writeFileSync(out+'.json',JSON.stringify(r,null,2)+'\n');
  const c=r.counts;
  const md=['# B01 — V4.3 reusable-evidence repair queue','',
    'Reused previous successful full-batch run '+r.baseline.runId+
      ' at code commit '+r.baseline.codeSha+'; no new 500-lesson scan.','',
    '| Metric | Lessons |','|---|---:|',
    '| Publisher visual review | '+c.publisherCues+' |',
    '| Teacher source activity review | '+c.teacherCues+' |',
    '| Overlapping review | '+c.overlap+' |',
    '| Unique lesson review queue | '+c.reviewUnion+' |',
    '| Possible classifier overmatch (requires source check) | '+c.potentialClassifierOvermatches+' |',
    '| Confirmed defects | '+c.confirmedDefectLessons+' |',
    '| Still missing full evidence | '+c.missingEvidenceLessons+' |','',
    '## Root-cause review groups','','| Signal | Lessons |',
    '|---|---:|',...r.rootCauseGroups.map(x=>'| '+x.key+' | '+x.count+' |'),
    '', 'See JSON for exact affected LessonCodes, source/content SHAs and overlapping causes.',
    '', '**BATCH_NO-GO** — no release, certification or ledger mutation.'];
  fs.writeFileSync(out+'.md',md.join('\n')+'\n');
  console.log(JSON.stringify({mode:r.mode,counts:r.counts,groups:r.rootCauseGroups.map(g=>({key:g.key,count:g.count})),decision:r.academicDecision},null,2));
}
if(require.main===module){try{run()}catch(e){console.error('B01 delta triage: '+e.message);process.exitCode=1}}
module.exports={join,run};
