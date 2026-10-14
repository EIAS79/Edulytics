#!/usr/bin/env node
'use strict';
// V9 B01: exact 500-way join of original teacher-purpose signal memory,
// canonical source/lesson fingerprints, shared raw-template graph and ACTUAL
// post-materialization Rich V2 structural evidence. Analysis ONLY, no repair.
const fs=require('node:fs');
const assert=require('node:assert/strict');

function expectedSubject(excerpt) {
 if(/Identify what needs to be measured|correct starting point|equal.size units/i.test(excerpt))
  return ['LENGTH','WEIGHT_MASS'];
 if(/whole and the part being removed|subtraction relationship/i.test(excerpt))
  return ['SUBTRACTION'];
 if(/quantities being combined|addition relationship/i.test(excerpt))
  return ['ADDITION'];
 if(/quantities or expressions to compare|greater, less, or equal/i.test(excerpt))
  return ['COMPARISON'];
 if(/title, labels, categories, and scale|wrong category or scale/i.test(excerpt))
  return ['DATA_GRAPH'];
 if(/figure and the attributes|shape by appearance/i.test(excerpt))
  return ['GEOMETRY_SHAPES'];
 if(/equal groups, rate, or scaling|multiplication relationship/i.test(excerpt))
  return ['MULTIPLICATION','DIVISION'];
 if(/what the task asks you to find or show/i.test(excerpt))
  return []; // deliberately generic - no domain assumption
 return []; // never invent a mathematical requirement from weak text
}
function reconcile(graph,effective) {
 assert.equal(graph?.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(effective?.planId,graph.planId);
 assert.equal(graph?.batchId,'B01');
 assert.equal(effective?.batchId,'B01');
 assert.deepEqual(graph?.coverage,{expected:500,observed:500,unique:500,missing:0});
 assert.deepEqual(effective?.coverage,{expected:500,materialized:500,unique:500});
 assert.equal(graph?.nodes?.length,500);
 assert.equal(effective?.rows?.length,500);
 assert.equal(graph.repairDecision,'NO_AUTOMATIC_REPAIR_AUTHORITY');
 assert.equal(effective.academicDecision,'BATCH_NO-GO');
 const byCode=new Map(), joined=[];
 for(const [i,e] of effective.rows.entries()){
  const g=graph.nodes[i];
  assert.equal(g?.ordinal,i+1);
  assert.equal(e?.ordinal,i+1);
  assert.equal(e.lessonCode,g.lessonCode);
  assert.match(e.effectiveBodyFingerprint,/^[a-f0-9]{64}$/i);
  assert.match(e.exactEffectiveFieldSha256.stepByStepSolutions,/^[a-f0-9]{64}$/i);
  assert.match(e.exactEffectiveFieldSha256.commonMistakes,/^[a-f0-9]{64}$/i);
  assert.equal(e.richV2.composed,true,'Unusable Rich V2 for '+e.lessonCode);
  assert.equal(e.academicDisposition,'EFFECTIVE_RUNTIME_EVIDENCE_ONLY_NOT_CERTIFIED');
  assert.equal(g.academicDecision,'NOT_CERTIFIED_BY_THIS_GRAPH');
  assert.ok(!byCode.has(e.lessonCode),'Duplicate effective learner '+e.lessonCode);
  byCode.set(e.lessonCode,{raw:g,effective:e});
  joined.push({
   lessonCode:e.lessonCode,ordinal:e.ordinal,
   originalSourceSemanticSha256:g.sourceSemanticSha256,
   rawLearnerBodySha256:g.rawLearnerBodySha256,
   effectiveLearnerFingerprint:e.effectiveBodyFingerprint,
   effectiveContentVersion:e.effectiveContentVersion,
   sourceTeacherPurposeSignals:g.sourceTeacherPurposeSignals,
   originalTeacherGoalSectionCaptured:g.originalTeacherGoalSectionCaptured,
   richV2Source:e.richV2.source,richV2VisualCount:e.richV2.visualCount,
   richV2VisualKinds:e.richV2.visualKinds,
   effectivePatternSignals:e.effectiveRawPatternSignals,
   certificationState:'NOT_CERTIFIED_BY_THIS_JOIN'
  });
 }
 const groups=graph.repeatedExactLearnerTemplates.groups.map(group=>{
  assert.match(group.learnerTemplateSha256,/^[a-f0-9]{64}$/);
  const field=group.affectedComponent.split(':')[1];
  assert.ok(['stepByStepSolutions','commonMistakes'].includes(field),
   'Unexpected mutation root field');
  const requirements=expectedSubject(group.excerpt);
  const effectiveStillSame=[],changedByMaterializer=[],contextGap=[],sourcePurposeUnresolved=[];
  const sourceTagCounts={};
  for(const code of group.affectedLessonCodes){
   const match=byCode.get(code);assert.ok(match,'Unknown group learner '+code);
   const {raw,effective:e}=match;
   assert.ok(raw.rawExactTemplateIds.includes(group.rootCauseId),
    'Raw group membership drift '+code);
   const tags=raw.sourceTeacherPurposeSignals||[];
   for(const tag of tags)sourceTagCounts[tag]=(sourceTagCounts[tag]||0)+1;
   const unchanged=e.exactEffectiveFieldSha256[field]===group.learnerTemplateSha256;
   (unchanged?effectiveStillSame:changedByMaterializer).push(code);
   if(!raw.originalTeacherGoalSectionCaptured)sourcePurposeUnresolved.push(code);
   else if(requirements.length&&!requirements.some(tag=>tags.includes(tag)))
    contextGap.push(code);
  }
  assert.equal(new Set(group.affectedLessonCodes).size,group.candidateLessonCount);
  assert.equal(effectiveStillSame.length+changedByMaterializer.length,group.candidateLessonCount);
  return {
   rootCauseId:group.rootCauseId,
   affectedComponent:group.affectedComponent,
   candidateLessonCount:group.candidateLessonCount,
   rawTemplateSha256:group.learnerTemplateSha256,
   exactMaterializedTemplateStillUsed:effectiveStillSame.length,
   exactMaterializedTemplateReplaced:changedByMaterializer.length,
   exactMaterializedTemplateLessonCodes:effectiveStillSame,
   materializerOverrodeLessonCodes:changedByMaterializer,
   expectedSourcePurposeSignalAnyOf:requirements,
   lexicalSourcePurposeMismatchCandidates:contextGap,
   unresolvedSourcePurposeCodes:sourcePurposeUnresolved,
   originalTeacherPurposeSignalCounts:sourceTagCounts,
   topicSignalEvidence:'LEXICAL_CANDIDATES_ONLY_NOT_SEMANTIC_GOAL_PROOF',
   priorityScore:contextGap.filter(x=>effectiveStillSame.includes(x)).length*
    Math.max(1,group.distinctStandardsDomains),
   confirmedDefect:false,canAutoRepair:false,
   disposition:'SOURCE_AND_EFFECTIVE_SEMANTIC_VALIDATION_REQUIRED'
  };
 });
 const markerField={
  SHARED_RULER_METHOD_TEMPLATE:'rulerTemplate',
  SOURCE_FIGURE_CAPTION_PLACEHOLDER:'figureCaptionPlaceholder',
  LEARNER_TEACHER_PRONOUN_DRIFT:'learnerTeacherPronoun'
 };
 const roots={};
 for(const [name,g] of Object.entries(graph.rootCauseSignals)){
  const signal=markerField[name];
  assert.ok(signal,'Unrecognized root-cause signal '+name);
  const same=[],cleared=[],withVisual=[],withoutVisual=[],sidecar=[];
  for(const code of g.affectedLessonCodes){
   const m=byCode.get(code);assert.ok(m,'Unknown raw family member '+code);
   if(m.effective.effectiveRawPatternSignals[signal])same.push(code);
   else cleared.push(code);
   if(m.effective.richV2.visualCount>0)withVisual.push(code);
   else withoutVisual.push(code);
   if(m.effective.richV2.source==='EXPLICIT_SIDECAR')sidecar.push(code);
  }
  assert.equal(same.length+cleared.length,g.candidateLessonCount);
  roots[name]={
   rawCandidates:g.candidateLessonCount,effectiveMarkerStillPresent:same.length,
   effectiveMarkerAbsent:cleared.length,effectiveMarkerLessonCodes:same,
   resolvedOrTransformedBeforeRendering:cleared,
   withComposedVisual:withVisual.length,withoutComposedVisual:withoutVisual.length,
   explicitRichSidecar:sidecar.length,
   renderedSemanticCorrectness:'NOT_VERIFIED_BY_STRUCTURAL_COMPOSITION',
   classification:'CANDIDATES_NOT_CONFIRMED_DEFECTS'
  };
 }
 const noVisual=joined.filter(r=>r.richV2VisualCount===0).map(r=>r.lessonCode);
 const done=groups.reduce((n,g)=>n+g.exactMaterializedTemplateStillUsed,0);
 const changed=groups.reduce((n,g)=>n+g.exactMaterializedTemplateReplaced,0);
 return {
  schemaVersion:1,planId:graph.planId,batchId:'B01',
  kind:'V9_B01_ALL_500_EFFECTIVE_ROOT_CAUSE_RECONCILIATION',
  coverage:{expected:500,joined:joined.length,unique:byCode.size,missing:0},
  resultOf:'Read-only cohort-level post-Materializer canonical and Rich V2 structural analysis',
  sourceGoalEvidence:'499/500 lexical signals + separately verified purpose-only subset; not publisher-certified',
  groupCount:groups.length,
  groupCandidateMemberships:{effectiveExactRepeated:done,materializerTransformed:changed,
   overlapsAllowed:true},
  repeatedTemplateGroups:groups.sort((a,b)=>b.priorityScore-a.priorityScore||
   a.rootCauseId.localeCompare(b.rootCauseId)),
  sharedPatternRoots:roots,
  richV2WithoutStructuralVisuals:{count:noVisual.length,lessonCodes:noVisual,
   note:'A missing visual is NOT automatically a failure for all lesson objectives'},
  rows:joined,
  confirmedSemanticDefectsByThisTool:0,
  automaticFixAuthorization:false,
  L1toL6Decision:'UNPROVEN',
  academicDecision:'BATCH_NO-GO',
  nextOperation:'Validate all high-impact mismatches against complete source-element cards, actual worked examples, independent math oracle and rendered Rich V2 before one shared-root repair; do not choose arbitrary lessons'
 };
}
function run(){
 const graphPath=process.argv.find(x=>x.startsWith('--graph='))?.slice(8)||
  'b01-v9-global-root-cause-500.json';
 const effectPath=process.argv.find(x=>x.startsWith('--effective='))?.slice(12)||
  'b01-v9-effective-root-cause-500.json';
 const out=process.argv.find(x=>x.startsWith('--out='))?.slice(6);
 const a=reconcile(JSON.parse(fs.readFileSync(graphPath,'utf8')),
  JSON.parse(fs.readFileSync(effectPath,'utf8')));
 const serialized=JSON.stringify(a,null,2)+'\n';
 if(out)fs.writeFileSync(out,serialized);else process.stdout.write(serialized);
 process.stderr.write('V9 B01 effective root comparison 500/500, '+
  a.groupCount+' global template groups; 0 semantic approvals; BATCH_NO-GO\n');
}
if(require.main===module)run();
module.exports={reconcile,expectedSubject};
