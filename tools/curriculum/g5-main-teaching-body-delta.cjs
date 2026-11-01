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
const consentPath='artifacts/curriculum-delivery/US-CCSS-MATH/G5/grade5-owner-academic-consent-20261101.json';
const ownerConsent=JSON.parse(fs.readFileSync(path.join(root,consentPath),'utf8'));
const approvedPath='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json';
const approvedBytes=fs.readFileSync(path.join(root,approvedPath));
const approvedContentBlobSha=crypto.createHash('sha1')
  .update('blob '+approvedBytes.length+'\0').update(approvedBytes).digest('hex');
const ownerConsentValid=
  ownerConsent.ownerDecision==='OWNER_ACADEMIC_CONSENT_GRANTED' &&
  ownerConsent.scope?.grade==='G5' && ownerConsent.scope?.prNumber===596 &&
  ownerConsent.scope?.stage==='D' && ownerConsent.scope?.lessonCount===148 &&
  ownerConsent.scope?.requiredEntryState==='READY_FOR_STAGE_D' &&
  ownerConsent.authorizationClaims?.ownerConsentsTo107IdentifiedTextChanges===true &&
  ownerConsent.publicDecisionEvidence==='https://github.com/EIAS79/Edulytics/issues/619#issuecomment-6564702967' &&
  ownerConsent.approvedContentPackPath===approvedPath &&
  ownerConsent.approvedContentPackGitBlobSha===approvedContentBlobSha &&
  ownerConsent.ownerSignedCryptographicDocument===false &&
  ownerConsent.certificationExclusions?.individualSourceElementsIndependentlyCompared===false &&
  ownerConsent.certificationExclusions?.releaseApproved===false;

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
      reviewStatus:'OWNER_CONTENT_CONSENT_RECORDED_SOURCE_PARITY_PENDING',
      approvalEvidence:ownerConsentValid?ownerConsent.publicDecisionEvidence:null,
      approvalSignature:null,originalElementParity:null});
  }
}
for(const code of baseline.keys())if(!currentMap.has(code))errors.push('MAIN_LESSON_DELETED:'+code);
const changed=review.filter(x=>x.translations.length);
const metadata=review.filter(x=>x.changedMetadata.length);
const approvalIssues=[];
if(!ownerConsentValid)approvalIssues.push('GRADE5_OWNER_CONTENT_CONSENT_INVALID_OR_MISSING');
if(changed.length!==107)approvalIssues.push('OWNER_CONSENT_WAS_FOR_EXACTLY_107_TEACHING_EDITS');
if(metadata.length!==0)approvalIssues.push('UNAPPROVED_NON_TRANSLATION_METADATA_CHANGES');
if(review.length!==107)approvalIssues.push('OWNER_APPROVAL_SCOPE_DOES_NOT_COVER_EXTRA_CHANGES');
if(changed.some(x=>!x.sourceShaUnchanged||!x.canonicalShaUnchanged))
  approvalIssues.push('UNAPPROVED_PUBLISHER_SOURCE_OR_CANONICAL_HASH_CHANGED');
if(changed.some(x=>x.translations.some(t=>t.cultureCode!=='en')))
  approvalIssues.push('OWNER_APPROVAL_CANNOT_COVER_NON_ENGLISH_EDITS');

const languageCounts={};
for(const r of review)for(const tr of r.translations)languageCounts[tr.cultureCode]=(languageCounts[tr.cultureCode]||0)+1;
const result={schemaVersion:1,kind:'G5_MAIN_TO_CANDIDATE_CONTENT_DIFFERENCE_EVIDENCE_NOT_ACADEMIC_APPROVAL',
  baselineBranch:'origin/main',candidateSha:git('rev-parse','HEAD'),
  baselinePackLessons:old?.length,candidatePackLessons:current?.length,
  changedTeachingLessonCount:changed.length,changedMetadataLessonCount:metadata.length,
  changedCultureCounts:languageCounts,sourceHashPreservedOnAllChangedLessons:changed.every(x=>x.sourceShaUnchanged),
  canonicalHashPreservedOnAllChangedLessons:changed.every(x=>x.canonicalShaUnchanged),
  ownerContentApprovedLessonCount:ownerConsentValid && approvalIssues.length===0 ? changed.length:0,
  ownerContentApprovalEvidence:ownerConsentValid?ownerConsent.publicDecisionEvidence:null,
  independentlySourceVerifiedLessonCount:0,
  approvalStatus:approvalIssues.length ? 'OWNER_APPROVAL_OUTSIDE_VERIFIED_SCOPE' :
    'OWNER_CONTENT_CONSENT_GRANTED_SOURCE_PARITY_INDEPENDENTLY_UNVERIFIED',
  errors:[...errors,...approvalIssues],lessons:review};
fs.mkdirSync(outputDir,{recursive:true});
fs.writeFileSync(path.join(outputDir,'grade5-main-teaching-body-delta.json'),JSON.stringify(result,null,2)+'\n');
const lines=[
  '# Grade 5 — candidate versus main exact teaching-field differences',
  '',
  'Candidate '+result.candidateSha+', main baseline '+git('rev-parse','origin/main')+'.',
  '148 source lesson identities must remain identical. This is a SHA-based change inventory, **not source-parity acceptance**.',
  'Changed teaching lessons: '+changed.length+'; metadata-changed lessons: '+metadata.length+
  '; owner academic content approval evidenced: **'+result.ownerContentApprovedLessonCount+'**; independently verified source parity: **0**.',
  'Original publisher source hash unchanged for every changed lesson: '+result.sourceHashPreservedOnAllChangedLessons+
  '; canonical body SHA unchanged for every changed lesson: '+result.canonicalHashPreservedOnAllChangedLessons+
  '. These are byte-level metadata comparisons, not proof that the corrected text is correct.',
  '',
  '| Lesson | Changed English fields | Source SHA preserved | Content consent |',
  '|---|---|---|---|'
];
for(const row of review){
  const t=row.translations.find(x=>x.cultureCode==='en');
  lines.push('| '+row.lessonCode+' | '+(t?.changes.map(x=>x.field).join(', ')||'none')+
    ' | '+(row.sourceShaUnchanged?'yes':'NO')+' | '+(ownerConsentValid?'OWNER CONSENT — source parity pending':'NOT APPROVED')+' |');
}
lines.push('','No canonical lesson body, original pack or generated source text was rewritten by this audit.','');
fs.writeFileSync(path.join(outputDir,'grade5-main-teaching-body-delta.md'),lines.join('\n'));
console.log('G5_MAIN_CONTENT_DELTA '+JSON.stringify({candidate:result.candidateSha,sourceCount:old?.length,
  targetCount:current?.length,changedTeaching:changed.length,changedMetadata:metadata.length,
  languages:languageCounts,sourceHashesUnchanged:result.sourceHashPreservedOnAllChangedLessons,
  canonicalHashesUnchanged:result.canonicalHashPreservedOnAllChangedLessons,
  ownerApproved:result.ownerContentApprovedLessonCount,sourceVerified:0,
  outsideOwnerConsent:approvalIssues.length,errors:result.errors}));
if(result.errors.length)process.exitCode=2; // Owner consents to the 107 exact content edits; source parity remains a separate required Stage D gate.
