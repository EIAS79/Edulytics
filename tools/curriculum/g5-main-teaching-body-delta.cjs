#!/usr/bin/env node
'use strict';
// Read-only source-preserving exact Git-object diff. Produces review candidates, NEVER academic approvals.
const fs = require('node:fs');
const path = require('node:path');
const cp = require('node:child_process');
const crypto = require('node:crypto');
const root = path.resolve(__dirname,'../..');
const relative = 'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json';
const outputDir = path.join(root,'stage-d-results');
const sha = value => crypto.createHash('sha256').update(String(value),'utf8').digest('hex');
const git = (...args) => cp.execFileSync('git',args,{cwd:root,encoding:'utf8',maxBuffer:32*1024*1024}).trim();
const previous = JSON.parse(git('show','origin/main:'+relative));
const next = JSON.parse(fs.readFileSync(path.join(root,relative),'utf8'));
const old = previous.lessons, current = next.lessons;
const errors=[], review=[];
if(!Array.isArray(old)||!Array.isArray(current)||old.length!==148||current.length!==148){
  errors.push('SOURCE_PACK_148_LESSONS_IDENTITY_MISMATCH');
}
const baseline = new Map((old||[]).map(x=>[x.lessonCode,x]));
const currentMap = new Map((current||[]).map(x=>[x.lessonCode,x]));
if(baseline.size!==old?.length||currentMap.size!==current?.length)errors.push('DUPLICATE_LESSON_CODE');
const fields=['title','explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
const other = [];
for(const [code,item] of currentMap){
  const original=baseline.get(code);
  if(!original){errors.push('LESSON_NOT_IN_MAIN:'+code);continue;}
  const changedMetadata=[...new Set([...Object.keys(original),...Object.keys(item)])].filter(k=>k!=='translations'&&JSON.stringify(original[k])!==JSON.stringify(item[k]));
  if(changedMetadata.length)other.push({lessonCode:code,changedMetadata});
  const oldTr = new Map((original.translations||[]).map(x=>[x.cultureCode,x]));
  const newTr = new Map((item.translations||[]).map(x=>[x.cultureCode,x]));
  if(oldTr.size!==newTr.size||[...oldTr.keys()].some(k=>!newTr.has(k)))errors.push('TRANSLATION_CULTURE_MISMATCH:'+code);
  const translations=[];
  for(const [culture,t] of newTr){
    const prior=oldTr.get(culture);
    if(!prior)continue;
    const changed=[...new Set([...Object.keys(prior),...Object.keys(t)])].filter(k=>JSON.stringify(prior[k])!==JSON.stringify(t[k]));
    if(changed.length){
      if(culture!=='en')errors.push('NON_ENGLISH_UNREVIEWED_EDIT:'+code+':'+culture);
      if(changed.some(k=>!fields.includes(k)))errors.push('NON_TEACHING_TRANSLATION_FIELD:'+code+':'+changed.join(','));
      translations.push({cultureCode:culture,changes:changed.map(k=>({
        field:k,oldSha256:sha(JSON.stringify(prior[k])),newSha256:sha(JSON.stringify(t[k])),
        oldBytes:Buffer.byteLength(JSON.stringify(prior[k])||'','utf8'),
        newBytes:Buffer.byteLength(JSON.stringify(t[k])||'','utf8')
      }))});
    }
  }
  if(translations.length || changedMetadata.length){
    review.push({lessonCode:code,
      baselineSourceSha256:original.sourceSha256,
      candidateSourceSha256:item.sourceSha256,
      sourceShaUnchanged:original.sourceSha256===item.sourceSha256,
      baselineCanonicalSha256:original.canonicalBodySha256,
      candidateCanonicalSha256:item.canonicalBodySha256,
      canonicalShaUnchanged:original.canonicalBodySha256===item.canonicalBodySha256,
      changedMetadata,translations,
      reviewStatus:'PENDING_AUTHORIZED_HUMAN_SOURCE_REVIEW',
      approvalSignature:null,originalElementParity:null});
  }
}
for(const code of baseline.keys())if(!currentMap.has(code))errors.push('MAIN_LESSON_DELETED:'+code);
const changed=review.filter(x=>x.translations.length);
const metadata=review.filter(x=>x.changedMetadata.length);
const languageCounts={};
for(const r of review)for(const tr of r.translations)languageCounts[tr.cultureCode]=(languageCounts[tr.cultureCode]||0)+1;
const result={schemaVersion:1,kind:'G5_MAIN_TO_CANDIDATE_CONTENT_DIFFERENCE_EVIDENCE_NOT_ACADEMIC_APPROVAL',
  baselineBranch:'origin/main',candidateSha:git('rev-parse','HEAD'),
  baselinePackLessons:old?.length,candidatePackLessons:current?.length,
  changedTeachingLessonCount:changed.length,changedMetadataLessonCount:metadata.length,
  changedCultureCounts:languageCounts,sourceHashPreservedOnAllChangedLessons:changed.every(x=>x.sourceShaUnchanged),
  canonicalHashPreservedOnAllChangedLessons:changed.every(x=>x.canonicalShaUnchanged),
  externallyApprovedLessonCount:0,approvalStatus:'PENDING_SOURCE_REVIEW',errors,lessons:review};
fs.mkdirSync(outputDir,{recursive:true});
fs.writeFileSync(path.join(outputDir,'grade5-main-teaching-body-delta.json'),JSON.stringify(result,null,2)+'\n');
const lines=[
  '# Grade 5 — candidate versus main exact teaching-field differences',
  '',
  'Candidate '+result.candidateSha+', main baseline '+git('rev-parse','origin/main')+'.',
  '148 source lesson identities must remain identical. This is a SHA-based change inventory, **not source-parity acceptance**.',
  'Changed teaching lessons: '+changed.length+'; metadata-changed lessons: '+metadata.length+
  '; external academic approvals evidenced: **0**.',
  'Original publisher source hash unchanged for every changed lesson: '+result.sourceHashPreservedOnAllChangedLessons+
  '; canonical body SHA unchanged for every changed lesson: '+result.canonicalHashPreservedOnAllChangedLessons+
  '. These are byte-level metadata comparisons, not proof that the corrected text is correct.',
  '',
  '| Lesson | Changed English fields | Source SHA preserved | Academic approval |',
  '|---|---|---|---|'
];
for(const row of review){
  const t=row.translations.find(x=>x.cultureCode==='en');
  lines.push('| '+row.lessonCode+' | '+(t?.changes.map(x=>x.field).join(', ')||'none')+
    ' | '+(row.sourceShaUnchanged?'yes':'NO')+' | PENDING |');
}
lines.push('','No canonical lesson body, original pack or generated source text was rewritten by this audit.','');
fs.writeFileSync(path.join(outputDir,'grade5-main-teaching-body-delta.md'),lines.join('\n'));
console.log('G5_MAIN_CONTENT_DELTA '+JSON.stringify({candidate:result.candidateSha,sourceCount:old?.length,
  targetCount:current?.length,changedTeaching:changed.length,changedMetadata:metadata.length,
  languages:languageCounts,sourceHashesUnchanged:result.sourceHashPreservedOnAllChangedLessons,
  canonicalHashesUnchanged:result.canonicalHashPreservedOnAllChangedLessons,unapproved:review.length,errors}));
if(errors.length||review.length)process.exitCode=2; // Never turn undocumented source changes green.
