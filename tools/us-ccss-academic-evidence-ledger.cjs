#!/usr/bin/env node
'use strict';
// Strict, deterministic US Common Core academic evidence register.
// It maps exact publisher child-code evidence separately from candidate
// Practice lesson routes. It NEVER converts source references or passing
// numerical tests into independent academic/constructed-response certification.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const input={
  clauses:'docs/curriculum/us-ccss-121-clause-evidence-corrected.json',
  diff:'docs/curriculum/us-ccss-official-text-diff.json',
  mappings:'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json',
  repairs:'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.authority-text-repairs.json'
};
const normalize=s=>String(s||'').replace(/^CCSS:/,'')
  .replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.').toUpperCase();
const clauses=read(input.clauses), diff=read(input.diff);
const mappings=read(input.mappings).mappings;
const repairs=read(input.repairs);
const ready=mappings.filter(x=>x.lessonCode?.startsWith('PED:US-CCSS-MATH:')
  &&x.practiceReadiness==='READY_VERIFIED'&&x.allowedQuestionFamilies?.length>0);
const byRepair=new Map(repairs.replacements.map(x=>[normalize(x.code),x]));
const rows=clauses.clauses.map(c=>{
 const parent=normalize(c.parent),code=normalize(c.clause);
 const candidates=ready.filter(m=>m.officialOutcomeCodes?.some(v=>
   normalize(v)===parent||normalize(v)===code)).map(m=>({
    lessonCode:m.lessonCode,sourceType:m.sourceType,
    allowedQuestionFamilies:m.allowedQuestionFamilies,
    alignmentLimit:'Parent standard or declared lesson mapping; not independent child-clause grading proof'
   }));
 return {
  clause:c.clause,parent:c.parent,sourceReferenceStatus:c.sourceReferenceStatus,
  exactSourceReferenceCount:c.exactSourceReferenceCount,
  examplePublisherReference:c.examplePublisherReference||null,
  exampleParentLesson:c.exampleParentLesson||null,
  candidateReadyLessonRoutes:candidates,
  assessmentEvidenceStatus:candidates.length
   ?'CANDIDATE_NUMERIC_OR_RECOGNITION_PRACTICE_REVIEW_REQUIRED'
   :'NO_CANDIDATE_READY_ROUTED_FAMILY',
  certificationStatus:'NOT_FULLY_CERTIFIED',
  missingEvidence:[
   'Independent lesson example and skill-to-child-clause review',
   'Question-answer and grading rubric against full clause, including constructed/graphical/written skill if required'
  ]
 };
});
const textRows=diff.nonIdenticalResults.map(row=>({
 code:row.code,similarity:row.similarity,sourceLength:row.sourceLength,
 officialLength:row.officialLength,sourceTail:row.sourceTail,officialTail:row.officialTail,
 disposition:byRepair.has(normalize(row.code))
  ?'HASH_GUARDED_CORRECTION_RECORDED__POST_REPAIR_COMPARISON_PENDING'
  :'OFFICIAL_SOURCE_DISCREPANCY_REVIEW_REQUIRED',
 correctionManifest:byRepair.has(normalize(row.code))?{
  code:byRepair.get(normalize(row.code)).code,
  correctedTextSha256:byRepair.get(normalize(row.code)).correctedTextSha256,
  authorityUrl:byRepair.get(normalize(row.code)).authorityUrl
 }:null,
 warning:'Snapshot compares original accepted pack text, not repaired runtime standard prose; no automatic typo/footnote dismissal.'
}));
const summary={
 clauses:rows.length,
 exactPublisherChildCodeEvidence:rows.filter(x=>x.exactSourceReferenceCount>0).length,
 parentOnlyChildCodeEvidence:rows.filter(x=>x.exactSourceReferenceCount===0).length,
 clausesWithAnyCandidateReadyLesson:rows.filter(x=>x.candidateReadyLessonRoutes.length>0).length,
 academicallyCertified:0,
 officialTextDiffsInBaseline:textRows.length,
 hashGuardedOfficialCorrections:byRepair.size,
 textDifferencesPendingIndependentDisposition:textRows.filter(x=>x.correctionManifest===null).length,
 authenticatedDisposableUSSchoolWorkflow:'PASSED_PR_452',
 authenticatedLiveStagingWorkflow:'NOT_VERIFIED',
 releaseReady:false
};
if(summary.clauses!==121||summary.exactPublisherChildCodeEvidence!==106
  ||summary.parentOnlyChildCodeEvidence!==15||summary.officialTextDiffsInBaseline!==107
  ||summary.hashGuardedOfficialCorrections!==5||summary.academicallyCertified!==0)
 throw Error('Academic evidence source count or verdict drift: '+JSON.stringify(summary));
const output={
 schemaVersion:1,kind:'US_CCSS_ACADEMIC_EVIDENCE_AND_DISPOSITION_REGISTER',
 sourceBasis:{
  clauses:input.clauses,officialTextDiff:input.diff,
  mappings:input.mappings,authorityRepairManifest:input.repairs
 },
 reviewMethod:'Offline committed source and checked-in SHA-guarded amendments; no new publisher assertions; active lesson-contract routes are candidates, never semantic certification.',
 summary,clauseEvidence:rows,officialTextDiscrepancies:textRows
};
const file=path.join(root,'docs/curriculum/us-ccss-academic-evidence-ledger.json');
const content=JSON.stringify(output,null,2)+'\n';
if(process.argv.includes('--write'))fs.writeFileSync(file,content);
else if(fs.readFileSync(file,'utf8')!==content)throw Error('US CCSS academic register drift; run node tools/us-ccss-academic-evidence-ledger.cjs --write and review the diff');
console.log(JSON.stringify(summary));
