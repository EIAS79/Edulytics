'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const v10=read('docs/curriculum/us-ccss-g8-coder4-v10-acceptance-ledger-20261029.json');
const figures=read('docs/curriculum/us-ccss-g8-coder4-v11-original-geometry-figures-20261029.json');
const by=new Map(figures.scenes.map(s=>[s.lessonCode,s]));
const rows=v10.rows.map(row=>{
 const figure=by.get(row.lessonCode);
 const lessonMockServiceVerified=row.lessonCode==='PED:US-CCSS-MATH:G8:U04:L05';
 const next=[];
 if(row.figures.hasPublisherActivityFigures&&!figure)next.push('SOURCE_FIGURE_ORIGINAL_REPRESENTATION_STILL_REQUIRED_OR_CLASSIFY_REUSE');
 if(figure)next.push('SVG_ASSET_NOT_YET_ATTACHED_TO_AUTHENTICATED_RICH_V2_RENDERER');
 if(row.source.teacherOnlyActivityTitles.length)next.push('SOURCE_COOLDOWN_TEACHER_RUBRIC_AND_PERSISTED_SCORE_NOT_VERIFIED');
 if(row.standardsRoles.hasDiscrepancy)next.push('AUTHORITATIVE_STANDARDS_ROLE_OWNER_APPROVAL_PENDING');
 if(row.lessonCode==='PED:US-CCSS-MATH:G8:U03:L12')next.push('SUPERVISOR2_SHARED_PRACTICE_PR597_TECHNICAL_REVIEW_INTEGRATION_PENDING');
 next.push('LESSON_EXACT_AUTHENTICATED_PRACTICE_AND_TWO_SCHOOL_DATABASE_VERIFICATION_REQUIRED');
 return {lessonCode:row.lessonCode,sourceLessonCode:row.sourceLessonCode,
  originalSourceActivityCount:row.source.activityCount,
  teacherOnlyCooldowns:row.source.teacherOnlyActivityTitles.length,
  sourceFigureInActivities:row.figures.hasPublisherActivityFigures,
  verifiedCourseOwnedSvgFile:figure?.assetPath??null,
  verifiedCourseOwnedSvgSha256:figure?.sha256??null,
  svgHasIndependentMathUnitChecks:!!figure,
  svgInAuthenticatedStudentRenderer:false,
  localSyntheticServiceGenerationAndFakeAttemptVerified:lessonMockServiceVerified,
  localSyntheticServiceTwoSchoolFakeIdentityGateVerified:lessonMockServiceVerified,
  actualPostgresqlAttemptPersistedAndReadBack:false,
  studentTeacherAuthenticatedSessionVerified:false,
  teacherScoringPersistenceVerified:false,
  A:'NOT_ACCEPTED',B:'NOT_ACCEPTED',C:'NOT_VERIFIED',D:'NOT_VERIFIED',
  nextChecks:next};
});
const counts={lessons:rows.length,
 assets:figures.scenes.length,
 sourceFigurePriority:rows.filter(r=>r.sourceFigureInActivities).length,
 sourceFigurePriorityWithOriginalAsset:rows.filter(r=>r.sourceFigureInActivities&&r.verifiedCourseOwnedSvgFile).length,
 sourceFigurePriorityWithoutOriginalAsset:rows.filter(r=>r.sourceFigureInActivities&&!r.verifiedCourseOwnedSvgFile).length,
 teacherOnlyCooldowns:rows.reduce((a,r)=>a+r.teacherOnlyCooldowns,0),
 grade8LessonsWithServiceOnlySyntheticPracticeHarness:rows.filter(r=>r.localSyntheticServiceGenerationAndFakeAttemptVerified).length,
 AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,
 authenticatedDatabaseFixtureRun:false,release:'NO_GO'};
if(counts.lessons!==131||counts.assets!==30||counts.sourceFigurePriority!==113||
 counts.sourceFigurePriorityWithOriginalAsset!==28||counts.sourceFigurePriorityWithoutOriginalAsset!==85||
 counts.teacherOnlyCooldowns!==117||counts.grade8LessonsWithServiceOnlySyntheticPracticeHarness!==1)
 throw Error('V11 exact evidence mismatch '+JSON.stringify(counts));
const output='docs/curriculum/us-ccss-g8-coder4-v11-implementation-ledger-20261029.json';
fs.writeFileSync(path.join(root,output),JSON.stringify({schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 purpose:'Live V11 course-owned original SVG asset and actual private Practice service synthetic harness per-lesson coverage; NOT an A/B/C/D promotion.',
 limitations:'Assets are not connected to the shared authenticated Rich V2 UI, and the two-school fake repository tests do not prove real DB or role-scoped authorization. Grade 8 exact student/teacher human source parity and assessment remain unverified.',
 counts,rows},null,2)+'\n');
console.log('V11_IMPL '+JSON.stringify({output,counts}));
