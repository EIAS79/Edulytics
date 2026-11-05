'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const v8=read('docs/curriculum/us-ccss-g8-coder4-v8-acceptance-ledger-20261029.json');
const activity=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const rich=fs.readdirSync(path.join(root,dir)).filter(f=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(f))
 .flatMap(f=>read(dir+'/'+f).Lessons);
const index=new Map(rich.map(x=>[x.LessonCode,x]));
const sourceBy=new Map(source.rows.map(x=>[x.lessonCode,x]));
const activityBy=new Map(activity.rows.map(x=>[x.lessonCode,x]));
function roleVerdict(sourceRow){
 const extra=sourceRow.blueprintExtraRoles;
 const nonMP=extra.filter(x=>!x.startsWith('Addressing:MP.'));
 if(nonMP.length)return {
  decision:'KEEP_BLUEPRINT_UNCHANGED_PENDING_COORDINATED_SAME_EDITION_ROLE_REVIEW',
  class:'CONTENT_STANDARD_ROLE_OR_SPECIFICITY_DISCREPANCY',
  rationale:'The source and blueprint differ on a content-standard role or granularity. A preparation-page role alone does not prove the entire teacher/student sequence should be remapped.'
 };
 return {
  decision:'RETAIN_ADDITIONAL_BLUEPRINT_MP_AS_PEDAGOGY_PENDING_OWNER_REVIEW',
  class:'MATHEMATICAL_PRACTICE_ONLY_NOT_EXPLICIT_IN_SOURCE_PREPARATION',
  rationale:'The additional MP standards appear in the blueprint but not the selected teacher preparation roles. Do not fabricate an outcome or delete pedagogy without owner approval.'
 };
}
const rows=v8.rows.map(r=>{
 const id=r.lessonCode,p=activityBy.get(id),s=sourceBy.get(id),lesson=index.get(id);
 if(!p||!s||!lesson)throw Error('Missing V9 evidence: '+id);
 const roleMismatch=!!(s.blueprintExtraRoles.length||s.blueprintMissingRoles.length);
 const figureDecision=p.uniqueMathFiguresInActivities>0
  ?'SOURCE_FIGURES_CATALOGUED_ORIGINAL_ACCESSIBLE_REPRESENTATION_NOT_CERTIFIED'
  :'NO_CMS_IMAGE_IN_STUDENT_ACTIVITIES_CHECK_TABLES_AND_INTERACTIONS';
 const remaining=['AUTHENTICATED_RICH_V2_ROUTE_NOT_VERIFIED','STUDENT_SOURCE_ACTIVITY_SEMANTIC_PARITY_NOT_APPROVED',
  'LESSON_EXACT_GENERATED_PRACTICE_NOT_ACCEPTED','WRONG_MALFORMED_AND_EDGE_ANSWERS_NOT_AUTHENTICATED',
  'ATTEMPT_STORAGE_AND_TWO_SCHOOL_REJECTION_NOT_VERIFIED','TEACHER_SCORE_FEEDBACK_PERSISTENCE_NOT_VERIFIED',
  'EXTERNAL_ACADEMIC_SIGNOFF_AND_OWNER_UAT_MISSING'];
 if(p.uniqueMathFiguresInActivities>0)remaining.push('SOURCE_FIGURES_REQUIRE_ORIGINAL_ACCESSIBLE_VISUAL_REVIEW');
 if(p.teacherOnlyActivityTitles.length>0)remaining.push('TEACHER_COOLDOWN_ASSESSMENT_NOT_INTEGRATED');
 if(roleMismatch)remaining.push('ROLE_PROVENANCE_DECISION_NOT_OWNER_APPROVED');
 if(id==='PED:US-CCSS-MATH:G8:U03:L12')remaining.push('ISSUE_595_ONE_VARIABLE_PRACTICE_WRONG_FOR_TWO_VARIABLE_GOAL');
 return {lessonCode:id,sourceLessonCode:s.sourceLessonCode,unit:s.unit,
  source:{studentPage:p.sourceStudentUrl,teacherPage:p.sourceTeacherUrl,
   studentPageSha256:p.studentPageSha256,teacherPageSha256:p.teacherPageSha256,
   activityCount:p.studentActivityCount,activityEvidence:p.activities.map(a=>({
    title:a.title,bodySha256:a.bodySha256,figureCount:a.mathFigureCount,
    figures:a.uniqueMathFigures.map(i=>({sourceUrl:i.sourceAssetUrl,alt:i.alt})),
    tableCount:a.tables,mathMarkupCount:a.inlineMathTags
   })),teacherOnlyActivityTitles:p.teacherOnlyActivityTitles},
  figures:{uniqueSourceFigureCount:p.uniqueMathFiguresInActivities,originalVisualInspectionDecision:figureDecision,
   currentRichV2VisualCount:lesson.Visuals.length,
   currentRichV2VisualKinds:lesson.Visuals.map(v=>v.Kind),
   effectiveBrowserFigureRenderingVerified:false,independentlyAccessibleSourceParityVerified:false},
  standardsRoles:roleMismatch?{hasDiscrepancy:true,publisherPreparationRoles:s.sourceRoles,
   blueprintExtraRoles:s.blueprintExtraRoles,blueprintMissingRoles:s.blueprintMissingRoles,...roleVerdict(s),
   ownerApproved:false}:{hasDiscrepancy:false,decision:'NO_ROLE_DISCREPANCY_RECORDED',ownerApproved:null},
  practice:{staticOutcomeTargets:r.practice.outcomeTargets,
   explicitMapping:r.practice.explicitSkill,
   multiMappedFamilies:r.practice.multiMappedFamilies,
   mathematicalPracticeOnlyOutcomes:r.practice.mathematicalPracticeOnlyOutcomes,
   explicitKnownMismatch:r.practice.knownSemanticMappingMismatch,
   questionGeneratorExactlySelectedForThisLessonVerified:false,
   graderPositiveNegativeMalformedEquivalentBoundaryVerified:false,
   securePersistedStudentAttemptVerified:false,
   teacherReviewedRubricPersisted:false},
  status:'BLOCKED_WITH_CAUSE',A:'NOT_ACCEPTED',B:'NOT_ACCEPTED',C:'NOT_VERIFIED',D:'NOT_VERIFIED',
  unresolvedEvidence:remaining};
});
const counts={
 authoredLessons:rich.length,sourceStudentLessonPages:rows.length,sourceTeacherLessonPages:rows.length,
 sourceStudentActivities:rows.reduce((n,r)=>n+r.source.activityCount,0),
 sourceTeacherOnlyActivities:rows.reduce((n,r)=>n+r.source.teacherOnlyActivityTitles.length,0),
 sourceFigurePriorityLessons:rows.filter(r=>r.figures.uniqueSourceFigureCount>0).length,
 sourceFigurePriorityDecisions:rows.length,sourceImageOccurrences:rows.reduce((n,r)=>n+r.source.activityEvidence.reduce((m,a)=>m+a.figureCount,0),0),
 lessonsWithFirstPartyRichV2Visuals:rows.filter(r=>r.figures.currentRichV2VisualCount>0).length,
 sourceRoleDiscrepancyDecisions:rows.filter(r=>r.standardsRoles.hasDiscrepancy).length,
 ownerApprovedRoleChanges:0,
 AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,teacherRuntimeVerified:0};
if(counts.authoredLessons!==131||counts.sourceStudentActivities!==411||
 counts.sourceTeacherOnlyActivities!==117||counts.sourceFigurePriorityLessons!==113||
 counts.sourceRoleDiscrepancyDecisions!==22||counts.lessonsWithFirstPartyRichV2Visuals!==1)
 throw Error('V9 ledger sources failed consistency: '+JSON.stringify(counts));
const output='docs/curriculum/us-ccss-g8-coder4-v9-acceptance-ledger-20261029.json';
fs.writeFileSync(path.join(root,output),JSON.stringify({schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 method:'Real teacher and student lesson source activity headings, HTML hashes, de-duplicated source figure URLs, and current first-party embedded Rich V2 state; no academic approval inferred.',
 limitations:'Teacher-only headings include cool-down assessments absent from the student lesson public HTML; their absence from that page does not imply an actual student product defect. Original diagrams may need coordinated renderer integration. No authentic isolated two-school persisted assessment or UAT yet.',
 counts,rows},null,2)+'\n');
console.log('V9_ACCEPTANCE_LEDGER '+JSON.stringify({file:output,...counts}));
