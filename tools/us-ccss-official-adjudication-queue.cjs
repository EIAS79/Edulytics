#!/usr/bin/env node
'use strict';
// PHASE 02 source-by-source review queue: never approve a text mismatch by similarity.
const fs=require('node:fs'),path=require('node:path'),cp=require('node:child_process'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),writing=process.argv.includes('--write');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
// Baseline SHA identifies source inputs. Final candidate SHA is separately
// checked at the independent academic/release signoff gate.
const sha=JSON.parse(fs.readFileSync(path.join(root,
 'artifacts/academic-us-ccss/00-source-manifest.json'),'utf8')).sourceCommitSha;
const norm=s=>String(s||'').replace(/^CCSS:/,'').replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.');
const hash=t=>crypto.createHash('sha256').update(String(t||'')).digest('hex');
const snapshot=read('docs/curriculum/reference/us-ccss-official-codes.snapshot.json');
const diff=read('docs/curriculum/us-ccss-official-text-diff.json');
const pack=read('src/Edulytics.Core/Curriculum/Packs/us-ccss-math.curriculum-pack.json');
const manifests=[
 read('src/Edulytics.Core/Curriculum/Packs/us-ccss-math.authority-text-repairs.json'),
 read('src/Edulytics.Core/Curriculum/Packs/us-ccss-math.exponent-text-repair.json'),
 read('src/Edulytics.Core/Curriculum/Packs/us-ccss-math.hs-notation-text-repairs.json')
];
const fixes=[...manifests[0].replacements,manifests[1],...manifests[2].replacements];
if(fixes.length!==9||diff.nonIdenticalResults.length!==107||diff.fetchErrors.length)
  throw Error('Expected 107 original discrepancies, nine reviewed patches, and zero official fetch failures');
const officialPages=new Map();
for(const p of snapshot.pages)for(const c of p.codes)officialPages.set(norm(c),p.url);
const sourceByCode=new Map(pack.Nodes.filter(x=>x.Kind==='Standard').map(x=>[norm(x.Code),x]));
const byFix=new Map(fixes.map(x=>[norm(x.code),x]));
if(byFix.size!==9)throw Error('Duplicate official repair code');
const pending=[],recorded=[];
for(const row of diff.nonIdenticalResults){
 const code=norm(row.code),source=sourceByCode.get(code);
 if(!source)throw Error('Missing source text '+code);
 const correction=byFix.get(code);
 const base={standardCode:'CCSS:'+code,authorityUrl:officialPages.get(code)||null,
  comparisonSource:'Original accepted source pack versus official HTML; not the current live Neon prose',
  sourceTextSha256:hash(source.OfficialText),
  sourceOriginalTail:row.sourceTail,officialPageTail:row.officialTail,
  sourceLength:row.sourceLength,officialHtmlExtractLength:row.officialLength,
  lexicalSimilarity:row.similarity,
  possibleMathOrFootnoteMarkupLoss:true};
 if(!base.authorityUrl)throw Error('Missing official publisher page for '+code);
 if(correction){recorded.push({...base,disposition:'HASH_GUARDED_CORRECTION_RECORDED',
  correctedTextSha256:correction.correctedTextSha256||null,
  correctionManifestTargetContentHash:correction.correctedContentHash||null,
  implementationReview:'CORRECTION_MANIFEST_PRESENT__LIVE_VERIFICATION_SEPARATE'});
  continue;}
 const note=/(\d+(?:see|students|glossary|sizes|a footnote))/i.test(row.sourceTail||'') ||
   /footnote/i.test((row.sourceTail||'')+' '+(row.officialTail||''));
 const priority=row.similarity<0.90?'P1_MATH_OR_TRUNCATION_REVIEW':
  row.similarity<0.95?'P2_CONTENT_DELTA_REVIEW':'P3_POSSIBLE_FORMAT_OR_FOOTNOTE_REVIEW';
 pending.push({...base,triagePriority:priority,
  tentativeReviewerHint:note?'POSSIBLE_FOOTNOTE_ARTIFACT__VERIFY_FULL_SOURCE':'COMPARE_COMPLETE_STANDARD_IN_CONTEXT',
  disposition:'NEEDS_EXTERNAL_REVIEW',humanReviewerId:null,
  reviewEvidenceId:null,approvedSourceEdition:null,exactBeforeAfterEvidenceId:null,
  officialMeaningPreserved:null,academicDecision:null});
}
if(recorded.length!==9||pending.length!==98)throw Error('Nine-fix/98-unreviewed partition changed');
pending.sort((a,b)=>a.triagePriority.localeCompare(b.triagePriority) ||
  a.lexicalSimilarity-b.lexicalSimilarity || a.standardCode.localeCompare(b.standardCode));
const output={schemaVersion:1,scope:'US-CCSS-MATH/CCSSM-2010/G1-G12',gitSha:sha,
 capturedAuthorityAtUtc:diff.retrievedUtc,sourceAuditFetchErrors:0,
 historicalOriginalDifferences:107,recordedHashGuardedRepairs:9,
 pendingIndependentDispositions:98,academicGate:'NO-GO',
 warning:'Ordering and hints are machine triage only; not approval, live text comparison, or license determination.',
 recordedCorrections:recorded,unreviewedStandards:pending};
if(manifests[1].previousPatchedContentDigest!==manifests[0].patchedContentDigest ||
   manifests[2].previousPatchedContentDigest!==manifests[1].patchedContentDigest)
 throw Error('Five -> six -> nine hash-guarded correction chain broken');
const reviewManifest={schemaVersion:1,scope:output.scope,sourceCommitSha:sha,
 kind:'PREVIOUSLY_SOURCE_REVIEWED_HASH_GUARDED_REPLACEMENTS_NOT_NEW_SIGNOFFS',
 decision:'PENDING_INDEPENDENT_ACADEMIC_RELEASE_APPROVAL',
 repairs:fixes.map(p=>({standardCode:p.code,officialAuthorityUrl:p.authorityUrl,
  expectedOldContentHash:p.expectedOldContentHash,expectedOldTextSha256:p.expectedOldTextSha256,
  proposedCorrectedContentHash:p.correctedContentHash,
  proposedCorrectedTextSha256:p.correctedTextSha256,
  acceptedManifestExists:true,liveApplicationAndAcademicSignoff:'VERIFY_SEPARATELY'}))};
const transition={schemaVersion:1,scope:output.scope,sourceCommitSha:sha,
 originalToFiveFix:manifests[0].patchedContentDigest,
 fiveToSixFix:manifests[1].patchedContentDigest,
 sixToNineFix:manifests[2].patchedContentDigest,
 expectedCurrentPredecessorDigest:manifests[2].previousPatchedContentDigest,
 targetDigest:manifests[2].patchedContentDigest,
 targetedHighSchoolCodes:manifests[2].replacements.map(x=>x.code),
 source:'Committed guarded seeder and correction manifests; disposable PostgreSQL CI evidence in PR #459',
 actualLiveStatus:'SEE_02_LIVE_NEON_READONLY_PROOF',
 productionMutationApproved:false,
 releaseGate:'NO-GO',
 constraints:['Independent production snapshot and rehearsed restore','Exact predecessor import digest and each old standard ContentHash/text SHA','Three affected standards only','Exact target canonical node/link counts','Post-write digest and individual row verification','Authorized release owner and rollback decision']};
function emit(file,data){
 const dst=path.join(root,'artifacts/academic-us-ccss',file);
 const content=JSON.stringify(data,null,2)+'\n';
 if(writing){fs.mkdirSync(path.dirname(dst),{recursive:true});fs.writeFileSync(dst,content);}
 else if(!fs.existsSync(dst)||fs.readFileSync(dst,'utf8')!==content)
   throw Error('Artifact missing or drift '+file);
}
emit('02-official-text-diff-dispositions.json',output);
emit('02-reviewed-authority-repairs.json',reviewManifest);
emit('02-digest-transition-proof.json',transition);
console.log(JSON.stringify({pending:pending.length,recorded:recorded.length,
 p1:pending.filter(x=>x.triagePriority.startsWith('P1')).length,
 p2:pending.filter(x=>x.triagePriority.startsWith('P2')).length,
 p3:pending.filter(x=>x.triagePriority.startsWith('P3')).length,
 reviewerDecision:'REQUIRED'},null,2));
