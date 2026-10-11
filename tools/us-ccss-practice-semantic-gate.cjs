// Verified evidence gate for US CCSS. Never overwrite release reports
// with stale "no authenticated tests" or "unused question family" claims.
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;
const clauses=read('docs/curriculum/us-ccss-121-clause-evidence-corrected.json');
const ledger=read('docs/curriculum/us-ccss-academic-evidence-ledger.json');
const required=[
 'PED:US-CCSS-MATH:G3:U02:L08',
 'PED:US-CCSS-MATH:G5:U01:L01',
 'PED:US-CCSS-MATH:G6:U08:L17',
 'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10',
 'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15',
 'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16',
 'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17',
 'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17'
];
const explicit=mappings.filter(x=>x.lessonCode.startsWith('PED:US-CCSS-MATH:')
  &&x.sourceType==='ReviewedPublisherLessonExactPractice');
if(explicit.length!==35||required.some(c=>!explicit.some(e=>e.lessonCode===c)))
 throw Error('Required 35 reviewed source routes unavailable');
if(clauses.summary.allClauses!==121||clauses.summary.withPublisherExactDirectCode!==106
  ||clauses.summary.withoutExactDirectCode!==15)
 throw Error('121-child clause source evidence drift');
const s=ledger.summary;
if(s.clauses!==121||s.exactPublisherChildCodeEvidence!==106
  ||s.parentOnlyChildCodeEvidence!==15||s.officialTextDiffsInBaseline!==107
  ||s.hashGuardedOfficialCorrections!==5||s.academicallyCertified!==0
  ||s.releaseReady!==false||ledger.clauseEvidence.some(c=>c.certificationStatus!=='NOT_FULLY_CERTIFIED'))
 throw Error('US academic truthfulness release gate breached');
const report={
 schemaVersion:4,productionReady:false,sourceReferencedLessons:35,targetedNewLessonRoutes:8,
 sourceEvidence:{childClauses:121,exactPublisherCodeReferences:106,
  withoutExactPublisherChildCodeReference:15},
 numericPracticeFamiliesAdded:11,
 originalEdulyticsExtensions:[{"clause":"HSF-BF.A.1.c","lessonCode":"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10","bodyPolicy":"ORIGINAL_NOT_PUBLISHER_ATTRIBUTED","scopedFamily":"usccss.hs.function_composition","fullSemanticCertification":false},{"clause":"HSF-BF.B.4.d","lessonCode":"PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15","bodyPolicy":"ORIGINAL_NOT_PUBLISHER_ATTRIBUTED","scopedFamily":"usccss.hs.restricted_inverse","fullSemanticCertification":false}],
 independentlyCertifiedChildClauses:0,
 authenticatedDisposableUsSchoolFlow:'PASSED_PR_452',
 authenticatedLiveStagingFlow:'NOT_VERIFIED',
 sourceTextDiscrepancyAudit:{baselineFlagged:107,confirmedHashGuardedRepairManifest:5,
  pendingReviewOrDisposition:102,baselineComparisonNotUpdatedForRuntimeRepairs:true},
 remainingReleaseBlockers:[
  '121 child clauses require independent full educational explanation, question-family and grading-rubric signoff; 106 exact publisher child references are not proof of mastery',
  '15 parent-only child-code alignments require clause-specific provenance review; do not invent publisher links',
  '107 baseline text differences were detected; 5 confirmed correction entries have SHA-guarded manifests, 102 still need independently documented disposition',
  'Graph construction, graphical vectors, and free-form mathematical justification require assessment/rubric proof beyond existing numeric and recognition Practice',
  'American high-school adoption still needs explicit course/pathway configuration (issue #453), not the disposable CI fixture workaround',
  'Live staging student and teacher browser acceptance has not yet been verified; disposable CI HTTPS acceptance passed in PR #452'
 ],
 notice:'Technical bugfixes and authenticated disposable school Practice pass. US 121-clause full academic certification remains NO-GO; do not infer failure of all 121 clauses.'
};
const outfile=path.join(root,'docs/curriculum/us-ccss-practice-semantic-blockers.json');
const content=JSON.stringify(report,null,2)+'\n';
if(process.argv.includes('--check')){
 if(fs.readFileSync(outfile,'utf8')!==content)throw Error('Semantic blocker report drift; run node tools/us-ccss-practice-semantic-gate.cjs and review');
}else fs.writeFileSync(outfile,content);
console.log(JSON.stringify({productionReady:false,independentlyCertifiedChildClauses:0,
 clauses:s.clauses,sourceUnmatched:s.parentOnlyChildCodeEvidence,sourceTextReview:102,
 authenticatedDisposableUsSchoolFlow:report.authenticatedDisposableUsSchoolFlow}));
