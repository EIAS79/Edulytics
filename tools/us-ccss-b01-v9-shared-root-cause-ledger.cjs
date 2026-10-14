#!/usr/bin/env node
'use strict';
// Full 500-lesson grouping by shared faulty templates; source verification
// is explicitly separate. A cue alone is not a defect or certificate.
// Materialized fixes must be inspected via runtime tests, not assumed from raw packs.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const sourceFile='artifacts/academic-us-ccss/batches/B01-lesson-manifest.json';
const packFiles=[1,2,3,4].map(n=>'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+n+'-phase29-v1.lesson-content-pack.json');
const evidenceFiles=[
 'artifacts/academic-us-ccss/batches/B01-v9-fraction-purpose-reviewed-metadata.json',
 'artifacts/academic-us-ccss/batches/B01-v9-placevalue-purpose-reviewed-metadata.json',
 'artifacts/academic-us-ccss/batches/B01-v9-unit-weight-ten-thousand-purpose-reviewed-metadata.json',
 'artifacts/academic-us-ccss/batches/B01-v9-nonlength-ruler-cohort-metadata.json',
 'artifacts/academic-us-ccss/batches/B01-v9-source-verified-nonmeasure-23-cohort.json',
 'artifacts/academic-us-ccss/batches/B01-v9-learner-voice-2-cohort.json',
 'artifacts/academic-us-ccss/batches/B01-v9-optional-source-purpose-fallback-reviewed-metadata.json',
 'artifacts/academic-us-ccss/batches/B01-v9-shared-comparison-procedure-44-source-reviewed-metadata.json'
];
const SHA=/^[a-f0-9]{64}$/i;
const definitions=[
 {id:'SHARED_RULER_METHOD_TEMPLATE',severity:'SOURCE_GOAL_REVIEW_REQUIRED',
  matching(t){
   const phrases=[
    ['workedExamples',/Approach:\s*Choose the required unit, align equal units from the endpoint/i],
    ['stepByStepSolutions',/Step 1: Identify what needs to be measured\./i],
    ['keyConceptsAndRules',/Measurement units must be equal in size and placed or interpreted without gaps/i],
    ['commonMistakes',/Common mistake: using unequal units, leaving gaps, overlapping units/i]
   ];
   return phrases.filter(([field,re])=>re.test(t[field]||'')).map(x=>x[0]);
  }},
 {id:'SOURCE_FIGURE_CAPTION_PLACEHOLDER',severity:'REVIEW_ACTUAL_RICH_RENDERING',
  matching(t){return ['workedExamples','explanation'].filter(f=>/\bCaption:\s*[A-Z]\b/.test(t[f]||''));}},
 {id:'LEARNER_TEACHER_PRONOUN_DRIFT',severity:'REVIEW_WORDING_NOT_COPYRIGHT_CLAIM',
  matching(t){return ['explanation','workedExamples'].filter(f=>/\b(?:support them in adding|as they learn a new stage|they are observing)\b/i.test(t[f]||''));}}
];
// Sort by explicit learner goal, never classify a candidate as a confirmed error.
// A number-line interval is deliberately distinct from physical length/ruler work.
function rulerGoalContext(en){
 const goal=(en.explanation||'').match(/Learning goal:\s*([^\n]+)/i)?.[1]||'';
 if(!goal)return 'GOAL_UNRESOLVED';
 if(/\b(?:lengths?|centimeters?|met(?:ers?|res?)|inches?|feet|foot|yardsticks?|rulers?|measuring lengths?)\b/i.test(goal))
  return 'LENGTH_CONTEXT_POSSIBLY_RELEVANT';
 if(/\b(?:number line|tick marks|interval from|locate .* fractions?)\b/i.test(goal))
  return 'NUMBER_LINE_CONTEXT_REVIEW';
 return 'NON_LENGTH_GOAL_SOURCE_REVIEW';
}
const assert=(condition,msg)=>{if(!condition)throw Error(msg)};
function summarize({manifest,packs,reviewed}){
 assert(manifest?.batchId==='B01'&&manifest.expectedLessonCount===500&&manifest.lessons?.length===500,
  'Required one 500 lesson B01 manifest');
 assert(Array.isArray(packs)&&packs.length===4,'Required four curricular packs');
 const contents=new Map();
 for(const p of packs){
  assert(p.packCode==='US-CCSS-MATH'&&p.versionCode==='CCSSM-2010','Unexpected pack identity');
  for(const l of p.lessons){
   assert(l.lessonCode&&!contents.has(l.lessonCode),'Duplicate pack identity: '+l.lessonCode);
   contents.set(l.lessonCode,l);
  }
 }
 assert(Array.isArray(reviewed)&&reviewed.length===evidenceFiles.length,'Need three source-reviewed evidence inputs');
 const verified=new Map();
 for(const e of reviewed){
  const items=e.rows || e.reviewedObjectives;
  assert(e.batchId==='B01'&&Array.isArray(items),'Unexpected evidence group');
  for(const row of items){
   assert(row.lessonCode&&SHA.test(row.sourceSemanticSha256),'Missing evidence/source fingerprint');
   assert(!verified.has(row.lessonCode),'Duplicate verified source-goal identity: '+row.lessonCode);
   verified.set(row.lessonCode,row.sourceSemanticSha256);
  }
 }
 const groups=new Map(definitions.map(x=>[x.id,{id:x.id,classification:'ROOT_CAUSE_CANDIDATES_NOT_CERTIFICATION',
  rawCandidateCount:0,sourceGoalReviewedCandidates:0,unverifiedCandidates:0,
  matchingLessons:[],sourceGoalReviewed:[],stillNeedsSourceVerification:[],
  byGoalContext:{LENGTH_CONTEXT_POSSIBLY_RELEVANT:0,NUMBER_LINE_CONTEXT_REVIEW:0,NON_LENGTH_GOAL_SOURCE_REVIEW:0,GOAL_UNRESOLVED:0}}]));
 const rows=[],seen=new Set();
 for(const entry of manifest.lessons){
  assert(entry.lessonCode&&!seen.has(entry.lessonCode),'Duplicate B01 manifest identity');
  seen.add(entry.lessonCode);
  const l=contents.get(entry.lessonCode);
  assert(l&&SHA.test(entry.sourceSemanticSha256)&&SHA.test(l.canonicalBodySha256),
   'Missing source/content fingerprints: '+entry.lessonCode);
  const en=l.translations?.find(x=>x.cultureCode==='en');
  assert(en,'Missing English learner text: '+entry.lessonCode);
  const flags=[];
  for(const family of definitions){
   const fields=family.matching(en);
   if(!fields.length)continue;
   const group=groups.get(family.id),proof=verified.get(entry.lessonCode);
   const sourceMatched=proof===entry.sourceSemanticSha256;
   assert(!proof||proof===entry.sourceSemanticSha256,
    'Stale academic source-goal fingerprint: '+entry.lessonCode);
   const outcome=sourceMatched?'SOURCE_GOAL_REVIEWED_REQUIRES_RUNTIME_RETEST':'SOURCE_GOAL_NOT_VERIFIED';
   const goalContext=family.id==='SHARED_RULER_METHOD_TEMPLATE'?rulerGoalContext(en):null;
   flags.push({rootCauseFamily:family.id,fieldHits:fields,reviewState:outcome,
    ...(goalContext?{goalContext}:{} )});
   if(goalContext)group.byGoalContext[goalContext]++;
   group.rawCandidateCount++;
   group.matchingLessons.push(entry.lessonCode);
   if(sourceMatched){
    group.sourceGoalReviewedCandidates++;group.sourceGoalReviewed.push(entry.lessonCode);
   }else{
    group.unverifiedCandidates++;group.stillNeedsSourceVerification.push(entry.lessonCode);
   }
  }
  rows.push({lessonCode:entry.lessonCode,sourceSemanticSha256:entry.sourceSemanticSha256,
   rawLearnerBodySha256:l.canonicalBodySha256,rootCauseCandidates:flags,
   finalEffectiveBodyVerified:false,independentL1L6Evidence:'NOT_IN_THIS_AUDIT',
   certifiedByThisIndex:false});
 }
 assert(rows.length===500&&seen.size===500,'Incomplete B01 coverage');
 for(const group of groups.values())
  if(group.id==='SHARED_RULER_METHOD_TEMPLATE')
   assert(Object.values(group.byGoalContext).reduce((a,b)=>a+b,0)===group.rawCandidateCount,
    'Incomplete ruler-purpose partition');
 return {schemaVersion:1,kind:'B01_V9_COLLECTIVE_SHARED_ROOT_CAUSE_TRIAGE',
  scope:'B01 500 lessons one execution unit',coverage:{expected:500,actual:rows.length,unique:seen.size},
  source:{manifest:sourceFile,packFiles,verifiedSourceGoalEvidence:evidenceFiles},
  groups:Object.fromEntries(groups),rows,automatedFixAuthority:false,
  sourceGoalVerifiedDoesNotProveRootCauseOrDelivery:true,academicDecision:'BATCH_NO-GO'};
}
function load(){
 const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
 return {manifest:read(sourceFile),packs:packFiles.map(read),reviewed:evidenceFiles.map(read)};
}
if(require.main===module){
 const result=summarize(load());
 const value=JSON.stringify(result,null,2)+'\n';
 const out=process.argv.find(x=>x.startsWith('--out='));
 if(out) fs.writeFileSync(out.slice(6),value);else process.stdout.write(value);
 process.stderr.write('B01 root cause group triage '+result.coverage.actual+
  '/500, 0 new certifications. Groups: '+definitions.map(x=>x.id+'='+
  result.groups[x.id].rawCandidateCount).join(', ')+'\n');
}
module.exports={summarize,definitions};
