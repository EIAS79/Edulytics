'use strict';
// Grade 4 #578 precedent: an owner-authorized FIRST-PARTY digital release is
// not an independent IM publisher-source parity or media-rights certificate.
// Never change the original source evidence or rubber-stamp D_VERIFIED.
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const cp=require('node:child_process');
const ROOT=path.resolve(__dirname,'../..');
const {inventory}=require('./g5-stage-d-diagnostic.cjs');
const read=p=>JSON.parse(fs.readFileSync(path.join(ROOT,p),'utf8'));
const stateDir=path.join(ROOT,'stage-d-results');
const evidencePath=path.join(stateDir,'grade5-source-evidence.json');
const ownerPath='artifacts/curriculum-delivery/US-CCSS-MATH/G5/grade5-owner-academic-release-authorization-20261103.json';
const reviewPath='artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-2021-second-pass-review-ledger-v1.json';
const g5='PED:US-CCSS-MATH:G5:';

function auditFirstPartyRelease(){
 const error=[];
 const candidateSha=cp.execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim();
 const owner=read(ownerPath),review=read(reviewPath),index=inventory();
 if(index.errors.length)error.push('INVALID_G5_CATALOGUE_OR_AB_C_HANDOFF');
 if(index.records.length!==148||index.blueprintCount!==148||index.packCount!==148)
  error.push('G5_148_LESSON_DENOMINATOR_CHANGED');
 if(owner.scope?.repository!=='EIAS79/Edulytics'||
    owner.scope?.grade!=='G5'||owner.scope?.lessons!==148||owner.scope?.units!==8||
    owner.ownerDecision?.academicApprovalOfEdulyticsAuthoredMathematics!==true||
    owner.ownerDecision?.productionPublicationAuthorized!==true||
    owner.ownerDecision?.originalMathContentAuthorshipAssertedByOwner!==true)
  error.push('OWNER_FIRST_PARTY_CONTENT_RELEASE_NOT_AUTHORIZED');
 const contentPack=owner.authorizedContentPack?.path;
 if(contentPack!=='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json')
  error.push('OWNER_CONTENT_SCOPE_DRIFT');
 else{
  const bytes=fs.readFileSync(path.join(ROOT,contentPack));
  const blob=crypto.createHash('sha1').update('blob '+bytes.length+'\0').update(bytes).digest('hex');
  if(owner.authorizedContentPack.gitBlobSha!==blob)error.push('OWNER_AUTHORIZED_G5_CONTENT_PACK_DRIFT');
 }
 if(owner.nonClaims?.independentExactPublisherSourceParityVerified!==false||
    owner.nonClaims?.individualThirdPartyMediaRightsVerified!==false||
    owner.decisionSeparation?.independentSourceParity!=='UNVERIFIED_NOT_CLAIMED'||
    owner.decisionSeparation?.thirdPartyAssetLicenseCompliance!=='NOT_YET_INDEPENDENTLY_AUDITED')
  error.push('FALSE_PUBLISHER_OR_RIGHTS_CERTIFICATION');
 if(review.reviewComplete2021Count!==148||review.existingStageDReadyCount!==148||
    review.lessons?.length!==148||review.lessons.some(x=>
     x.reviewState!=='CLOSED'||x.REVIEW_COMPLETE_2021!==true||
     x.readyForStageDPreviousPass!==true))
  error.push('SECOND_PASS_2021_HANDOFF_UNCLOSED');

 const visualDir=path.join(ROOT,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
 const files=fs.readdirSync(visualDir).filter(x=>
  /^us-ccss-g5-.*\.rich-lesson-v2\.json$/.test(x));
 const lessons=new Map();
 let visualCount=0,externalVisuals=0,videoCount=0,sourceSummaryLinks=0;
 for(const file of files){
  const doc=read(path.relative(ROOT,path.join(visualDir,file)));
  for(const lesson of doc.Lessons||[]){
   if(!lesson.LessonCode?.startsWith(g5)||lessons.has(lesson.LessonCode)||
      lesson.CultureCode!=='en')error.push('DUPLICATE_OR_WRONG_EDITION_RICH_G5_'+lesson.LessonCode);
   lessons.set(lesson.LessonCode,lesson);
   visualCount+=(lesson.Visuals||[]).length;
   videoCount+=(lesson.Videos||[]).length;
   for(const visual of lesson.Visuals||[]){
    if(/https?:\/\/|data:image\/|<img\b|<image\b|cms-assets\./i.test(JSON.stringify(visual)))
     externalVisuals++;
   }
   sourceSummaryLinks+=(lesson.SummaryPoints||[]).filter(x=>/https?:\/\//.test(x)).length;
  }
 }
 if(lessons.size!==148||visualCount!==286||externalVisuals!==0||videoCount!==0)
  error.push('UNEXPECTED_FIRST_PARTY_LESSON_VISUAL_OR_EMBED_DRIFT');
 if(sourceSummaryLinks<95)error.push('INTERNAL_SOURCE_CITATIONS_DELETED');
 const view=fs.readFileSync(path.join(ROOT,'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml'),'utf8');
 if(!view.includes('RichLessonSummaryVisibility.ForReader(Model.LessonCode, Model.SummaryPoints)'))
  error.push('STUDENT_TEACHER_SOURCE_NOTES_MAY_LEAK');

 if(!fs.existsSync(evidencePath))error.push('SOURCE_DIAGNOSTIC_NOT_COLLECTED');
 let source=null;
 if(fs.existsSync(evidencePath)){
  source=JSON.parse(fs.readFileSync(evidencePath,'utf8'));
  if(source.gitCandidateSha!==candidateSha||source.scopeLessons!==148||source.lessonCount!==148||
     source.ownerAcademicApprovedOriginalLessons!==148||source.ownerReleasePermissionGranted!==true||
     source.reviewed2021SecondPassLessons!==148||
     source.original2021TeacherStudentFacingLessons!==148||
     source.original2021TeacherSectionHashLinkedLessons!==148||
     source.lessonsWithExplicitACGap?.length!==0)
   error.push('G5_SOURCE_PROVENANCE_REPORT_INCOMPLETE');
  if(source.sourceParityVerifiedLessons!==0||
     source.independentlySourceApprovedLessons!==0||
     source.rows?.some(x=>x.independentElementReviewPassed===true)||
     source.blockedLessons!==148)
   error.push('G5_PUBLISHER_PARITY_OR_HUMAN_SOURCE_CERTIFICATION_FABRICATED');
 }
 const result={
  schemaVersion:1,gitCandidateSha:candidateSha,
  reportType:'OWNER_AUTHORIZED_G5_FIRST_PARTY_DIGITAL_SCOPE_NOT_PUBLISHER_PARITY_CERTIFICATION',
  scopeLessons:148,ownerAcademicDecision:'GRANTED_FOR_FIRST_PARTY_CONTENT',
  ownerProductionPublicationPermission:'GRANTED',
  authentic2021ReviewCompleteLessons:review.reviewComplete2021Count,
  richV2LessonCount:lessons.size,richV2FirstPartyVisualDescriptions:visualCount,
  copiedOrExternallyLinkedVisualsDetected:externalVisuals,
  externalVideoEmbedsDetected:videoCount,
  sourceReferencesPreservedInternally:sourceSummaryLinks,
  independentPublisherSourceElementCertification:'NOT_CLAIMED',
  thirdPartyMediaRightsCertificate:'NOT_CLAIMED',
  teacherLedSourceTaskCertification:'NOT_CLAIMED',
  sourceDiagnosticBlockedLessons:source?.blockedLessons??null,
  sourceDiagnosticUnknownDenominators:source?.unknownSourceDenominatorLessons??null,
  fullProductCICandidateResult:'CHECK_SEPARATELY_BEFORE_MERGE',
  firstPartyDigitalScopePreflightPassed:error.length===0,
  errors:error
 };
 fs.mkdirSync(stateDir,{recursive:true});
 fs.writeFileSync(path.join(stateDir,'grade5-owner-first-party-digital-release.json'),JSON.stringify(result,null,2)+'\n');
 console.log('G5_FIRST_PARTY_DIGITAL_RELEASE '+JSON.stringify(result));
 if(error.length)process.exitCode=1;
 return result;
}
if(require.main===module)auditFirstPartyRelease();
module.exports={auditFirstPartyRelease};
