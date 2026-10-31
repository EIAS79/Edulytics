
'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const write=(p,v)=>fs.writeFileSync(path.join(root,p),JSON.stringify(v,null,2)+'\n');
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const standards=read('docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const practice=read('docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json');
const rubrics=read('docs/curriculum/us-ccss-g8-coder4-teacher-rubric-candidates-20261029.json');
const prev=read('docs/curriculum/us-ccss-g8-coder4-v12-implementation-ledger-20261029.json');
const artwork=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-source-geometry-evidence-20261029.json');
const index=a=>new Map(a.map(x=>[x.lessonCode,x]));
const S=index(source.rows),T=index(standards.rows),P=index(practice.rows),R=index(rubrics.lessons),F=index(artwork.figures);
const countByUnit={};
const pendingSupervisor2={};
const rowEvidence=[];
const rows=prev.rows.map(prevRow=>{
 const key=prevRow.lessonCode,s=S.get(key),t=T.get(key),p=P.get(key),r=R.get(key),f=F.get(key);
 if(!s||!t||!p||!r)throw Error('Missing authoritative V7-V12 course audit record for '+key);
 const unit=Number(key.match(/:U(\d{2}):/)[1]),u='U'+String(unit).padStart(2,'0');
 const a=countByUnit[u]||(countByUnit[u]={lessons:0,studentActivities:0,studentTables:0,teacherCooldowns:0,sourceRoleDiscrepancies:0,staticUnmappedLessons:0,multipleFamilyLessons:0,candidateSvgLessons:0,activitySpotCheckedInV13:0});
 a.lessons++;a.studentActivities+=s.studentActivityCount;a.studentTables+=s.activities.reduce((z,x)=>z+x.tables,0);
 if(s.teacherOnlyActivityTitles.length)a.teacherCooldowns++;
 const roleMismatch=t.blueprintExtraRoles.length>0||t.blueprintMissingRoles.length>0;if(roleMismatch)a.sourceRoleDiscrepancies++;
 const notMapped=p.officialOutcomeTargets.filter(x=>x.resolvedStatic===false);
 if(notMapped.length)a.staticUnmappedLessons++;
 const families=[...new Set(p.officialOutcomeTargets.filter(x=>x.resolvedStatic).map(x=>x.targetRuleId))];
 if(families.length>1)a.multipleFamilyLessons++;
 const row={...prevRow};
 if(f){
  if(prevRow.verifiedCourseOwnedSvgFile)throw Error('Asset collision: '+key);
  row.verifiedCourseOwnedSvgFile=f.assetPath;
  row.verifiedCourseOwnedSvgSha256=f.sha256;
  row.svgHasIndependentMathUnitChecks=true;
  row.v13ActivitySpotCheck='docs/curriculum/us-ccss-g8-coder4-v13-u01-source-geometry-evidence-20261029.json';
  row.nextChecks=prevRow.nextChecks.filter(x=>x!=='SOURCE_FIGURE_ORIGINAL_REPRESENTATION_STILL_REQUIRED_OR_CLASSIFY_REUSE');
  row.nextChecks.unshift('SOURCE_CONSTRUCTION_DIGITAL_EQUIVALENCE_UNVERIFIED','ORIGINAL_SVG_NOT_AUTHENTICATED_RICH_V2_RENDERED');
 }
 if(row.verifiedCourseOwnedSvgFile)a.candidateSvgLessons++;
 if(f)a.activitySpotCheckedInV13++;
 rowEvidence.push({
 lessonCode:key,unit,sourceLessonCode:s.sourceLessonCode,sourceStudentUrl:s.sourceStudentUrl,sourceTeacherUrl:s.sourceTeacherUrl,
 studentActivityTitles:s.activities.map(x=>x.title),
 originalStudentActivityCount:s.studentActivityCount,
 sourceFigurePresent:s.uniqueMathFiguresInActivities>0,
 originalStudentTables:s.activities.map(x=>({activity:x.title,count:x.tables})).filter(x=>x.count),
 sourceActivityImageAlt:s.activities.flatMap(x=>x.uniqueMathFigures.map(z=>z.alt)),
 sourceInteractiveMarkers:s.activities.reduce((z,x)=>z+x.publisherInteractiveElementMarkers,0),
 teacherOnlyCooldowns:s.teacherOnlyActivityTitles,
 standardsRoleAudit:{sourceRoles:t.sourceRoles,extraBlueprint:t.blueprintExtraRoles,missingBlueprint:t.blueprintMissingRoles,discrepancy:roleMismatch,approvedResolution:false},
 practiceContractAudit:{candidateStaticFamilies:families,staticUnmappedStandards:notMapped.map(x=>x.code),actualSelectedPracticeFamilyTested:false,generatorNegativeAndWrongLessonCasesTested:false,attemptSavedUnderRealIdentityAndPostgres:false,lessonContractMatchVerified:false},
 teacherAssessmentAudit:{rubricPresent:!!r.rubric,rubricCriteria:r.rubric.criteria.map(x=>x.description),maxPoints:r.rubric.maximum,draftOnly:true,teacherScoringRuntimeVerified:false,studentSubmissionAndFeedbackPersisted:false},
 visuals:{candidateSvg:row.verifiedCourseOwnedSvgFile,authenticatedStudentRenderVerified:false,requiresStudentSourceFigure:s.uniqueMathFiguresInActivities>0},
 reviewScope:f?'U01 V13: source student+teacher lesson activity spot-check, targeted content addition & original math asset':'Previous existing source title/activity/figure inventory only; full text/activity parity not reviewed under V13',
 perLessonAcademicAcceptance:{A:'NOT_ACCEPTED',B:'NOT_ACCEPTED',C:'NOT_VERIFIED',D:'NOT_VERIFIED'},
 integrationNeeds:['Supervisor 2: renderer-owned secure lesson-local SVG attachment/accessibility','Supervisor 2: current selected Practice families and generator/verifier/authorization','Supervisor 2: authenticated Postgres two-school lesson attempt and teacher score/feedback'],
 });
 return row;
});
if(rows.length!==131||new Set(rows.map(x=>x.lessonCode)).size!==131)throw Error('Grade inventory corruption');
const ledger={...prev,purpose:'V13 source-matched correction for Grade8 U01 L08-L10, original math SVG assets, and 131-lesson gap triage; still A-D NOT accepted',limitations:'The V13 per-lesson audit is an evidence-index from existing V7-V12 publisher activity inventory, static registries and draft rubrics. Only three new lessons were independently spot-checked against source activities. No authenticated generated Practice, teacher scoring, student/tenant database, or interactive construction has been verified.',counts:{...prev.counts,assets:34,sourceFigurePriorityWithOriginalAsset:32,sourceFigurePriorityWithoutOriginalAsset:81,sourceActivitySpotReviewedLessons:4,fullAcademicSourceParityAcceptedLessons:0,authenticatedSvgRenderedLessons:0,teacherScoredLessons:0,AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,release:'NO_GO'},rows};
write('docs/curriculum/us-ccss-g8-coder4-v13-implementation-ledger-20261029.json',ledger);
write('docs/curriculum/us-ccss-g8-coder4-v13-course-source-practice-teacher-audit-20261029.json',{
 schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 scope:'Full 131-row source inventory and role/Practice static gap triage, NOT 131 source-text reviews or approvals',
 unitSummary:countByUnit,counts:{total:131,sourceActivityCount:source.counts.studentActivities,sourceActivityTables:source.counts.totalActivityTables,teacherCooldowns:source.counts.teacherOnlyActivityHeadings,roleDiscrepancies:standards.roleDiscrepancies,staticUnmappedLessons:practice.unmappedLessonCodes.length,multipleTargetCandidates:practice.potentialMultipleTargetLessonCodes.length,authenticatedPracticeLessonVerifications:0,authenticatedTeacherScoreVerifications:0,stages:{A:0,B:0,C:0,D:0}},
 rows:rowEvidence,sharedHandoff:{owner:'Supervisor 2',affectedAllCourses:false,priorityLessonCodes:[...new Set([...practice.unmappedLessonCodes,'PED:US-CCSS-MATH:G8:U01:L08','PED:US-CCSS-MATH:G8:U01:L09','PED:US-CCSS-MATH:G8:U01:L10'])],scope:'Review current Practice selected families and routing without broad standards-based fallbacks; check geometric construction and reasoning need teacher scored/accessible interactions, not fake automatic scoring. Attach originals through shared secure RichV2 manifest and version adoption. Two-school real Postgres teacher graded attempts needed.'},release:'NO_GO'});
console.log('V13 ledger 131; U01 source corrections 3; total candidate SVG 34; priority remaining 81; summary',JSON.stringify(countByUnit));
