
'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..'),dir='docs/curriculum/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,dir+p),'utf8'));
const write=(p,v)=>fs.writeFileSync(path.join(root,dir+p),JSON.stringify(v,null,2)+'\n');
const prev=read('us-ccss-g8-coder4-p0-cumulative-execution-ledger-20261030.json');
const before=read('us-ccss-g8-coder4-p0-cumulative-source-practice-teacher-audit-20261030.json');
const evidence=read('us-ccss-g8-coder4-p0-u03-remaining-source-fixes-20261030.json');
const rich=read('us-ccss-g8-coder4-p0-u03-l12-source-graph-contract-20261030.json');
const figureMap=new Map(evidence.figures.map(f=>[f.lessonCode,f]));
const inspected=new Map(evidence.reviewedLessons.map(x=>[x.lessonCode,x]));
const rows=prev.rows.map(old=>{
 const src=inspected.get(old.lessonCode),f=figureMap.get(old.lessonCode);
 if(!src)return old;
 let r={...old,p0U03SourceEvidence:'docs/curriculum/us-ccss-g8-coder4-p0-u03-remaining-source-fixes-20261030.json',p0U03ReviewLevel:'STUDENT_ACTIVITY_SOURCE_PAGE_SPOT_REVIEW_TEACHER_DETAILED_NOT_DONE',
 nextChecks:[...new Set(['U03_STUDENT_GRAPH_OR_CARD_EXPERIMENT_INTERACTION_UNVERIFIED','TEACHER_COOLDOWN_REVIEW_AND_RUBRIC_PERSISTENCE_UNVERIFIED',...old.nextChecks])]};
 if(f){if(r.verifiedCourseOwnedSvgFile)r.additionalCourseOwnedSvgFiles=[...(r.additionalCourseOwnedSvgFiles||[]),{path:f.assetPath,sha256:f.sha256}];else{r.verifiedCourseOwnedSvgFile=f.assetPath;r.verifiedCourseOwnedSvgSha256=f.sha256;}r.svgHasIndependentMathUnitChecks=true;}
 return r;
});
const ledger={...prev,rows,
 purpose:'P0 source-based candidate changes for U02 all 13 and U03 all 14, preserving 131 canonical identities/old 36 figures; independent original geometry/graphs, no A-D promotion',
 limitations:'44 lesson-level activity spot reviews total (U01 17, U02 13, U03 14); U02 6 detailed teacher pages + U03 L12 one detailed teacher page. Others rely on existing publisher crosswalk. No full academic parity accepted, real authenticated Practice, student graph submission, two-school Postgres or teacher saved score.',
 counts:{...prev.counts,assets:43,sourceFigurePriorityWithOriginalAsset:38,sourceFigurePriorityWithoutOriginalAsset:75,sourceActivitySpotReviewedLessons:44,fullAcademicSourceParityAcceptedLessons:0,AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,authenticatedSvgRenderedLessons:0,teacherScoredLessons:0,release:'NO_GO'}};
const audit={...before,
 scope:'U01=17, U02=13, U03=14 source student lesson-page activity spot reviews: 44/131. Existing remaining 87 lessons only static retrieved source inventory. Stage A-D 0/131.',
 unitSummary:{...before.unitSummary,U03:{...before.unitSummary.U03,sourceStudentLessonPagesCheckedInP0:14,sourceTeacherLessonPagesCheckedInP0:1,candidateSvgLessons:before.unitSummary.U03.candidateSvgLessons+2}},
 counts:{...before.counts,sourceActivitySpotReviewedLessons:44,candidateSvgAssets:43,sourceFigurePriorityWithOriginalAsset:38,sourceFigurePriorityWithoutOriginalAsset:75},
 rows:before.rows.map(x=>{
  const src=inspected.get(x.lessonCode),f=figureMap.get(x.lessonCode);if(!src)return x;
  const visuals={...x.visuals};
  if(f){if(visuals.candidateSvg)visuals.additionalCandidateSvg=f.assetPath;else visuals.candidateSvg=f.assetPath;}
  return {...x,reviewScope:'U03 source student activity page read; teacher lesson page not deeply reviewed for this lesson; no parity acceptance',p0SourceReview:'docs/curriculum/us-ccss-g8-coder4-p0-u03-remaining-source-fixes-20261030.json',visuals};
 })
};
if(ledger.rows.length!==131||audit.rows.length!==131||new Set(ledger.rows.map(r=>r.lessonCode)).size!==131)throw Error('Grade8 identities changed');
if(ledger.rows.some(r=>r.A!=='NOT_ACCEPTED'||r.B!=='NOT_ACCEPTED'||r.C!=='NOT_VERIFIED'||r.D!=='NOT_VERIFIED'))throw Error('Academic approval falsely promoted');
write('us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json',ledger);
write('us-ccss-g8-coder4-p0-current-source-practice-teacher-audit-20261030.json',audit);
const issues=read('us-ccss-g8-coder4-source-crosswalk-20261029.json').rows.filter(r=>r.blueprintExtraRoles?.length||r.blueprintMissingRoles?.length);
write('us-ccss-g8-coder4-p0-standards-role-resolution-queue-20261030.json',{
 schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 decision:'AUTHORITATIVE_SOURCE_V8_CROSSWALK_HAS_FLAGS_REVIEW_REQUIRED_NO_UNAUTHORIZED_ROLE_EDIT',
 totalFlagged:issues.length,sourceEdition:'IM / OUR 2017–2019 Kendall Hunt',scope:'Source discrepancy triage, NOT independent standards reviewer approval',
 cases:issues.map(r=>({lessonCode:r.lessonCode,sourceLessonCode:r.sourceLessonCode,sourceRoles:r.sourceRoles,blueprintExtraRoles:r.blueprintExtraRoles,blueprintMissingRoles:r.blueprintMissingRoles,officialStudentPage:`https://im.kendallhunt.com/MS/students/3/${Number(r.lessonCode.match(/:U(\d+)/)[1])}/${Number(r.lessonCode.match(/:L(\d+)/)[1])}/index.html`,publisherTeacherRolesInspectedInThisP0:(r.lessonCode.match(/:U(\d+)/)?.[1]==='02'&&[1,4,7,10,12,13].includes(Number(r.lessonCode.match(/:L(\d+)/)[1])))||r.lessonCode.endsWith('U03:L12'),resolution:'REQUIRES_SUPERVISOR2_OR_INDEPENDENT_ACADEMIC_DECISION_SOURCE_ROLE_CANNOT_BE_SILENTLY_PROMOTED',certified:false})),
 approvedResolutions:0,pendingResolutions:issues.length
});
console.log(JSON.stringify({lessons:ledger.rows.length,counts:ledger.counts,standardsRolePending:issues.length},null,2));
