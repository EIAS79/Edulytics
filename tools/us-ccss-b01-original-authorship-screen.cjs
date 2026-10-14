#!/usr/bin/env node
'use strict';
// Review candidates only. This does NOT prove copying or grant publication rights.
// Source books are read-only references on Our-CS; only learner texts and fingerprints
// already present on GitHub are examined here.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const manifestFile='artifacts/academic-us-ccss/batches/B01-lesson-manifest.json';
const packFiles=[1,2,3,4].map(n=>
 'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+n+'-phase29-v1.lesson-content-pack.json');
const cues=[
 {id:'SOURCE_NARRATIVE_SEQUENCE',re:/\b(?:in previous lessons|in the previous lesson|in this lesson, you|in the first activity)\b/i,
  fields:['explanation','quickSummary']},
 {id:'LEARNER_TEACHER_PRONOUN_DRIFT',re:/\b(?:students|the teacher|they are observing|support them in|tools they have used|as they learn)\b/i,
  fields:['explanation','stepByStepSolutions','workedExamples']},
 {id:'PUBLISHER_ASSET_PLACEHOLDER',re:/\b(?:Caption\s*:|Display\s*:|Image\s*:|alt text\s*:)\b/i,
  fields:['workedExamples','explanation']},
 {id:'CLASSROOM_ACTIVITY_TRANSCRIPTION',re:/\b(?:in Activity\s+[1-9]|in the lesson synthesis|in the warm-up|in this center)\b/i,
  fields:['explanation','workedExamples']},
 {id:'SOURCE_SPECIFIC_WORKED_PROMPT',re:/\b(?:which one doesn.t belong|which representations best help|represent this number with words|fill in the blanks to represent)\b/i,
  fields:['workedExamples']}
];
const check=(cond,msg)=>{if(!cond)throw Error(msg)};
const sha=x=>typeof x==='string'&&/^[a-f0-9]{64}$/i.test(x);
function screen({manifest,packs}){
 check(manifest?.batchId==='B01'&&manifest.expectedLessonCount===500&&
  manifest.lessons?.length===500,'Exactly 500 B01 manifest items required');
 check(packs?.length===4,'Expected four first-edition CCSS K5 learner pack files');
 const packLessons=new Map();
 for(const pack of packs){
  check(pack.packCode==='US-CCSS-MATH'&&pack.versionCode==='CCSSM-2010'&&
    Array.isArray(pack.lessons),'Unexpected learner pack version');
  for(const l of pack.lessons){
   check(l.lessonCode&&!packLessons.has(l.lessonCode),'Duplicate learner lesson '+l.lessonCode);
   packLessons.set(l.lessonCode,l);
  }
 }
 const rows=[];
 const categories=Object.fromEntries(cues.map(x=>[x.id,{candidateCount:0,lessonCodes:[]}]));
 const seen=new Set();
 for(const entry of manifest.lessons){
  const code=entry.lessonCode;
  check(typeof code==='string'&&!seen.has(code),'Duplicate source lesson identity');
  seen.add(code);
  check(sha(entry.sourceSemanticSha256),'Invalid source semantic SHA: '+code);
  const packLesson=packLessons.get(code);
  check(packLesson&&sha(packLesson.canonicalBodySha256),'Missing learner lesson: '+code);
  const en=packLesson.translations?.find(x=>x.cultureCode==='en');
  check(en,'Missing English learner text: '+code);
  const found=[];
  for(const cue of cues){
   const fields=cue.fields.filter(k=>typeof en[k]==='string'&&cue.re.test(en[k]));
   if(fields.length){
    found.push({candidate:cue.id,fields,disposition:'REVIEW_REQUIRED_NOT_COPY_PROOF'});
    categories[cue.id].candidateCount++;
    categories[cue.id].lessonCodes.push(code);
   }
  }
  rows.push({
   lessonCode:code,
   sourceSemanticSha256:entry.sourceSemanticSha256,
   learnerBodySha256:packLesson.canonicalBodySha256,
   contentVersion:"UNRESOLVED_EFFECTIVE_BODY_USE_RUNTIME_EVIDENCE",
   originalAuthorshipReview:found.length?'CANDIDATE_NOT_CLEARED':'NO_HEURISTIC_CUE_NOT_RIGHTS_PROOF',
   cues:found,
   sourceTextCompared:'NO_SOURCE_TEXT_IN_THIS_SCAN',
   licenceDisposition:'UNDECIDED',
   academicDecision:'NOT_CERTIFIED_BY_THIS_SCAN'
  });
 }
 check(rows.length===500&&seen.size===500,'Missing B01 index coverage');
 return {
  schemaVersion:1,batchId:'B01',kind:'REFERENCE_ONLY_ELIGIBILITY_HEURISTIC_SCREEN',
  readOnlySourcePolicy:'No original publisher book text or graphics copied into repository',
  coverage:{expected:500,found:rows.length,unique:seen.size},
  byCue:categories,
  flagged:rows.filter(x=>x.cues.length).length,
  unflagged:rows.filter(x=>!x.cues.length).length,
  rows,
  proofPolicy:'Neither a flagged nor unflagged heuristic row determines copying, original authorship, attribution necessity or legal approval. Compare source against effective learner content only where relevant; no student release/attribution removal until verified.',
  batchCertification:'NOT_CERTIFIED',
  removePublicAttributionAuthorized:false
 };
}
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
if(require.main===module){
 const out=screen({manifest:read(manifestFile),packs:packFiles.map(read)});
 const json=JSON.stringify(out,null,2)+'\n';
 const file=process.argv.find(x=>x.startsWith('--out='));
 if(file) fs.writeFileSync(file.slice(6),json);else process.stdout.write(json);
 process.stderr.write('B01 original authorship candidate screen: '+out.coverage.found+
 '/500; '+out.flagged+' review candidates; ZERO rights claims certified.\n');
}
module.exports={screen,cues};
