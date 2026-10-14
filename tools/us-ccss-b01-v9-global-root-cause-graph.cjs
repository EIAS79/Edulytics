#!/usr/bin/env node
'use strict';
// Unified V9 B01 FULL-BATCH, read-only, root-cause impact graph.
// A repeated template, missing text cue, or shared standard is NOT proof of
// a pedagogical defect. Only academic source/effective runtime verification
// can authorize bounded repairs or L1-L6 evidence. Never modifies pack data.
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const {buildIndex}=require('./us-ccss-b01-v9-math-element-index.cjs');
const {summarize}=require('./us-ccss-b01-v9-shared-root-cause-ledger.cjs');

const root=path.resolve(__dirname,'..');
const sourcePrefix='artifacts/academic-us-ccss/batches/';
const manifestPath=sourcePrefix+'B01-lesson-manifest.json';
const sourceIndexPath=sourcePrefix+'B01-book-source-reference-index-500.json';
const teacherActivityIndexPath=sourcePrefix+'B01-original-teacher-activity-local-index-500.json';
const sourceGoalMemoryPath=sourcePrefix+'B01-v9-source-goal-signal-memory-500.json';
const packPaths=[1,2,3,4].map(g=>
 'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json');
const reviewedPaths=[
 sourcePrefix+'B01-v9-fraction-purpose-reviewed-metadata.json',
 sourcePrefix+'B01-v9-placevalue-purpose-reviewed-metadata.json',
 sourcePrefix+'B01-v9-unit-weight-ten-thousand-purpose-reviewed-metadata.json',
 sourcePrefix+'B01-v9-nonlength-ruler-cohort-metadata.json',
 sourcePrefix+'B01-v9-source-verified-nonmeasure-23-cohort.json',
 sourcePrefix+'B01-v9-learner-voice-2-cohort.json'
];
const sha=s=>crypto.createHash('sha256').update(s).digest('hex');
const assert=(v,msg)=>{if(!v)throw Error(msg)};
const isSha=s=>typeof s==='string'&&/^[0-9a-f]{64}$/i.test(s);
const fields=['stepByStepSolutions','commonMistakes'];
function standardDomain(s){
 const match=String(s).match(/^CCSS:(?:\d+)\.([A-Z]+)\./);
 return match?match[1]:'OTHER';
}
function groupCandidate(p,field,content){
 return {rootCauseId:'REPEATED_'+field.toUpperCase()+'_'+sha(field+'\0'+content).slice(0,20),
  kind:'REPEATED_IDENTICAL_LEARNER_TEMPLATE',
  affectedComponent:'CanonicalRichV2:'+field,
  learnerTemplateSha256:sha(content),
  excerpt:content.slice(0,105),affectedLessonCodes:[],
  sourceSemanticSha256ByLesson:{},standardDomains:{},sourcePurposeSignalCounts:{},gradeSet:[],
  conclusion:'SUSPECTED_ISSUE_REQUIRES_SOURCE_AND_EFFECTIVE_CONTENT_REVIEW',
  approvedToModify:false,academicPass:false};
}
function buildGraph({manifest,packs,sourceIndex,teacherActivityIndex,reviewed,sourceGoalMemory}){
 assert(sourceGoalMemory?.planId==='EDULYTICS-US-CCSS-UNIFIED-V9'&&
  sourceGoalMemory.batchId==='B01'&&sourceGoalMemory.coverage?.manifestExpected===500&&
  sourceGoalMemory.coverage.sourceFilesRead===500&&sourceGoalMemory.rows?.length===500,
  'Missing immutable 500-row V9 teacher-goal source memory');
 // Reuse the immutable, separately tested V9 math/source and root-cause indexes.
 const math=buildIndex({manifest,packs,sourceIndex,teacherActivityIndex});
 const triage=summarize({manifest,packs,reviewed});
 assert(math.coverage.observed===500&&math.coverage.unique===500&&
  triage.coverage.actual===500&&triage.coverage.unique===500,
  'All V9 indexes must cover EXACTLY the same 500');
 const learner=new Map();
 for(const p of packs) for(const l of p.lessons){
  assert(!learner.has(l.lessonCode),'Duplicate learner identity '+l.lessonCode);
  learner.set(l.lessonCode,l);
 }
 const verified=new Map();
 for(const e of reviewed){
  assert(e.batchId==='B01','Out-of-batch source evidence');
  for(const r of e.rows||e.reviewedObjectives||[]){
   assert(isSha(r.sourceSemanticSha256)&&!verified.has(r.lessonCode),
    'Duplicate/stale source review evidence '+r.lessonCode);
   verified.set(r.lessonCode,r.sourceSemanticSha256);
  }
 }
 const repeated=new Map(),nodes=[];
 const flagged=new Map(Object.keys(triage.groups).map(k=>[k,[]]));
 const cueGap=new Map(teacherActivityIndex.representationCueKeys.map(k=>[k,[]]));
 const sourceRulerContext={ORIGINAL_LENGTH:0,ORIGINAL_WEIGHT_MASS:0,
  ORIGINAL_NUMBER_LINE:0,ORIGINAL_OTHER_OR_UNRESOLVED:0};
 const unresolvedSourceGoalCodes=[];
 for(let i=0;i<500;i++){
  const m=manifest.lessons[i],base=learner.get(m.lessonCode),
   mathRow=math.rows[i],triageRow=triage.rows[i],source=sourceIndex.rows[i];
  assert(m.ordinal===i+1 && mathRow.lessonCode===m.lessonCode &&
   triageRow.lessonCode===m.lessonCode&&source.lessonCode===m.lessonCode &&
   isSha(m.sourceSemanticSha256)&&isSha(base.canonicalBodySha256),
   'V9 manifest/math/root/source identity or SHA drift '+m.lessonCode);
  const memory=sourceGoalMemory.rows[i];
  assert(memory?.ordinal===m.ordinal&&memory.lessonCode===m.lessonCode&&
   memory.sourceSemanticSha256===m.sourceSemanticSha256&&
   memory.sourceFileInventorySha256===source.localFileByteSha256&&
   memory.originalTeacherRelativePath===source.localRelativePath&&
   memory.originalTeacherSourceUrl===m.sourceUrl&&
   Array.isArray(memory.teacherFacingPurposeSignals),
   'Stale, unverified or cross-linked V9 original teacher-goal memory '+m.lessonCode);
  if(!memory.teacherGoalSectionFound)unresolvedSourceGoalCodes.push(m.lessonCode);
  const originalTags=memory.teacherFacingPurposeSignals;
  const t=base.translations.find(x=>x.cultureCode==='en');
  assert(t,'Missing English learner translation '+m.lessonCode);
  const sourceReviewed=verified.has(m.lessonCode);
  if(sourceReviewed)assert(verified.get(m.lessonCode)===m.sourceSemanticSha256,
   'Source evidence fingerprint drift '+m.lessonCode);
  const domains=[...new Set((m.officialOutcomeCodes||[]).map(standardDomain))].sort();
  const templateIds=[];
  for(const f of fields){
   const text=t[f];
   if(typeof text!=='string'||text.length<80)continue;
   const key=f+'\0'+text;
   if(!repeated.has(key))repeated.set(key,groupCandidate(base,f,text));
   const g=repeated.get(key);
   g.affectedLessonCodes.push(m.lessonCode);
   g.sourceSemanticSha256ByLesson[m.lessonCode]=m.sourceSemanticSha256;
   for(const d of domains)g.standardDomains[d]=(g.standardDomains[d]||0)+1;
   for(const tag of originalTags)
    g.sourcePurposeSignalCounts[tag]=(g.sourcePurposeSignalCounts[tag]||0)+1;
   if(!g.gradeSet.includes(m.gradeOrCourse))g.gradeSet.push(m.gradeOrCourse);
   templateIds.push(g.rootCauseId);
  }
  const patternFamilies=[];
  for(const f of triageRow.rootCauseCandidates){
   assert(flagged.has(f.rootCauseFamily),'Unexpected root family '+f.rootCauseFamily);
   flagged.get(f.rootCauseFamily).push(m.lessonCode);
   if(f.rootCauseFamily==='SHARED_RULER_METHOD_TEMPLATE'){
    const ctx=originalTags.includes('LENGTH')?'ORIGINAL_LENGTH':
     originalTags.includes('WEIGHT_MASS')?'ORIGINAL_WEIGHT_MASS':
     originalTags.includes('NUMBER_LINE')?'ORIGINAL_NUMBER_LINE':
     'ORIGINAL_OTHER_OR_UNRESOLVED';
    sourceRulerContext[ctx]++;
   }
   patternFamilies.push({rootCauseId:f.rootCauseFamily,fields:f.fieldHits,
    goalContext:f.goalContext||null,sourceReviewState:f.reviewState});
  }
  // Teacher cue absence from raw prose NEVER means Rich V2 is missing a visual.
  // It's a correlation queue only, awaiting effective runtime + browser evidence.
  const joined=['explanation','workedExamples','stepByStepSolutions','keyConceptsAndRules']
   .map(k=>t[k]||'').join(' ');
  for(const cue of mathRow.originalTeacherActivitySnapshot.representationCueCandidates){
   const look={
    DOT_IMAGES:/\b(?:dots?|dot card|cubes?|ten.frame)\b/i,
    NUMBER_LINE:/\bnumber[\s-]line\b/i,
    FRACTION_PARTS:/\b(?:fraction|numerator|denominator|equal parts)\b/i,
    CLOCK_FACE:/\b(?:clock|hour|minute|time)\b/i,
    PLACE_VALUE:/\b(?:place value|tens|hundreds|thousands)\b/i,
    RECTANGULAR_ARRAY:/\b(?:array|rows|columns)\b/i,
    MEASUREMENT_SCALE:/\b(?:scale|ruler|measurement|weigh|length)\b/i,
    DATA_GRAPH:/\b(?:graph|chart|plot|data)\b/i
   }[cue];
   if(look&&!look.test(joined))cueGap.get(cue).push(m.lessonCode);
  }
  nodes.push({
   ordinal:m.ordinal,lessonCode:m.lessonCode,
   sourceSemanticSha256:m.sourceSemanticSha256,rawLearnerBodySha256:base.canonicalBodySha256,
   originalTeacherFileSha256:source.localFileByteSha256,
   standards:m.officialOutcomeCodes,standardDomains:domains,
   academicGoalSourceReview:sourceReviewed?'REVIEWED_PURPOSE_ONLY_NOT_L1_L6':'NOT_SOURCE_GOAL_VERIFIED',
   originalTeacherGoalSectionCaptured:memory.teacherGoalSectionFound,
   sourceTeacherPurposeSignals:originalTags,
   sourcePurposeMemoryState:'EXTRACTED_SIGNALS_NOT_SEMANTIC_L1_CERTIFICATION',
   originalTeacherActivitySignals:mathRow.originalTeacherActivitySnapshot.representationCueCandidates,
   mathElementCandidateFamilies:mathRow.mathematicalElements.map(x=>x.family),
   rawRootCauseCandidates:patternFamilies,
   rawExactTemplateIds:templateIds,
   effectiveRichV2RuntimeState:'NOT_VERIFIED_BY_STATIC_GRAPH',
   academicDecision:'NOT_CERTIFIED_BY_THIS_GRAPH'
  });
 }
 const templateGroups=[...repeated.values()]
  .filter(g=>g.affectedLessonCodes.length>=8)
  .map(g=>({
   ...g,candidateLessonCount:g.affectedLessonCodes.length,
   distinctStandardsDomains:Object.keys(g.standardDomains).length,
   gradeSet:g.gradeSet.sort(),
   priorityScore:g.affectedLessonCodes.length*Math.max(1,Object.keys(g.standardDomains).length)
  }))
  .sort((a,b)=>b.priorityScore-a.priorityScore ||
   a.rootCauseId.localeCompare(b.rootCauseId));
 const rootFamilies=Object.fromEntries([...flagged].map(([id,codes])=>{
  assert(triage.groups[id].rawCandidateCount===codes.length,
   'Inconsistent V9 root cause count '+id);
  return [id,{rootCauseId:id,affectedLessonCodes:codes,candidateLessonCount:codes.length,
   sourceReviewedCandidates:triage.groups[id].sourceGoalReviewedCandidates,
   byGoalContext:id==='SHARED_RULER_METHOD_TEMPLATE'?
    triage.groups[id].byGoalContext:null,
   byOriginalTeacherGoalContext:id==='SHARED_RULER_METHOD_TEMPLATE'?
    sourceRulerContext:null,
   verifiedDefects:'NOT_ESTABLISHED_BY_PATTERN_INDEX',
   automaticFixAllowed:false}];
 }));
 const sourceVisualTextCues=Object.fromEntries([...cueGap].map(([id,codes])=>
  [id,{sourceCueKey:id,rawTextGapCandidates:codes.length,lessonCodes:codes,
   actualRenderedVisualStatus:'UNVERIFIED',automaticFixAllowed:false}]));
 const covered=new Set(nodes.map(x=>x.lessonCode));
 assert(covered.size===500&&templateGroups.every(g=>
  new Set(g.affectedLessonCodes).size===g.candidateLessonCount),
  '500-lesson graph or repeated root impact cardinality mismatch');
 assert(unresolvedSourceGoalCodes.length===sourceGoalMemory.coverage.sourceGoalSectionsUnresolved,
  'Source goal missing-section count drift');
 const domainSummary=Object.fromEntries(Object.entries(math.byFamily)
  .map(([k,v])=>[k,v.candidateLessons]));
 return {
  schemaVersion:1,planId:'EDULYTICS-US-CCSS-UNIFIED-V9',
  kind:'B01_GLOBAL_SHARED_ROOT_CAUSE_IMPACT_GRAPH',
  batchId:'B01',batchManifest:'ONE_IMMUTABLE_500_LESSON_SCOPE',
  coverage:{expected:500,observed:nodes.length,unique:covered.size,missing:0},
  inputs:{manifestPath,sourceIndexPath,teacherActivityIndexPath,sourceGoalMemoryPath,
   packPaths,reviewedPaths},
  automaticSourceGoalSignalCapture:{teacherGoalsFound:500-unresolvedSourceGoalCodes.length,
   unresolvedLessonCodes:unresolvedSourceGoalCodes,
   provenance:'All 500 local teacher HTML read read-only; 499/500 section lexical extracts',
   academicVerification:'NOT_GRANTED_BY_LEXICAL_EXTRACTION'},

  academicSourcePurposeReviews:{
   verifiedPurposeOnly:verified.size,stillNotPurposeReviewed:500-verified.size,
   clarification:'Purpose review and source hash indexing are NOT complete source element cards or L1–L6'},
  mathElementCandidatesByFamily:domainSummary,
  rootCauseSignals:rootFamilies,
  repeatedExactLearnerTemplates:{
   minimumOccurrences:8,fields,
   groups:templateGroups,uniqueGroups:templateGroups.length,
   identification:'SHA256 of field plus full unchanged text; no guess based on title',
   overlapWarning:'One lesson can match multiple fields and candidate families',
   noSemanticVerdict:'Shared original text can be valid in unrelated lessons; source evidence required'
  },
  originalTeacherRepresentationTextCorrelation:sourceVisualTextCues,
  nodes,
  repairDecision:'NO_AUTOMATIC_REPAIR_AUTHORITY',
  verificationDecision:'FULL_SOURCE_AND_EFFECTIVE_RICH_V2_COMPARISON_OUTSTANDING',
  certificationDecision:'BATCH_NO-GO',
  releaseEligible:false,
  nextRootCauseOperation:'Validate full-batch clusters against source-verified goals and effective Rich V2 runtime, confirm cause at component level, then bounded cohort-wide repair + full regression; never use per-lesson default repair or inferred PASS'
 };
}
function load(){
 const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
 return {manifest:read(manifestPath),sourceIndex:read(sourceIndexPath),
  teacherActivityIndex:read(teacherActivityIndexPath),
  sourceGoalMemory:read(sourceGoalMemoryPath),
  packs:packPaths.map(read),reviewed:reviewedPaths.map(read)};
}
if(require.main===module){
 const graph=buildGraph(load());
 const output=process.argv.find(x=>x.startsWith('--out='));
 if(output)fs.writeFileSync(output.slice(6),JSON.stringify(graph,null,2)+'\n');
 else process.stdout.write(JSON.stringify(graph,null,2)+'\n');
 process.stderr.write('B01 V9 global root cause coverage '+graph.coverage.observed+
  '/500; exact repeated template groups '+graph.repeatedExactLearnerTemplates.uniqueGroups+
  '; academic certifications newly granted 0; NO-GO\n');
}
module.exports={buildGraph,standardDomain};
