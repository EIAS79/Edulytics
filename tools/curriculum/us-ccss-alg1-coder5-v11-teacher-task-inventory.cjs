#!/usr/bin/env node
'use strict';
// Index existing source-authored teacher task/rubric prose; don't invent grades.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const ledgerPath=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json');
const rubricPath=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json');
const output=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V11_ALL_TEACHER_TASK_INVENTORY_20261029.json');
const ledger=JSON.parse(fs.readFileSync(ledgerPath,'utf8'));
const reviews=JSON.parse(fs.readFileSync(rubricPath,'utf8'));
const reviewed=new Map(reviews.teacherRubricCandidates.map(x=>[x.lessonCode,x]));
const tasks=[];
for(const lesson of ledger.lessons){
  const json=JSON.parse(fs.readFileSync(path.join(root,lesson.candidatePath),'utf8'));
  const paragraphs=json.Lessons?.[0]?.ExplanationParagraphs||[];
  const rubricTexts=paragraphs.filter(p=>/rubric/i.test(p));
  if(rubricTexts.length===0)throw Error('No existing rubric candidate '+lesson.lessonCode);
  let activityTexts=paragraphs.filter(p=>/\bstudent\b|\bactivity\b|\btask\b|\bpartner\b/i.test(p));
  const taskNarrativeExplicit=activityTexts.length>0;
  // Preserve existing mathematical explanation as a review candidate when
  // wording lacks task/activity/partner; do not invent a classroom task.
  if(!activityTexts.length)activityTexts=paragraphs.slice(1,3);
  if(!activityTexts.length)throw Error('No source narrative candidate '+lesson.lessonCode);
  const reviewedCandidate=reviewed.get(lesson.lessonCode);
  tasks.push({
    lessonCode:lesson.lessonCode,title:lesson.title,
    status:reviewedCandidate?'STRUCTURED_RUBRIC_SOURCE_SPOT_CHECKED_TEACHER_SIGNOFF_PENDING'
      :'EXISTING_RUBRIC_PROSE_UNSTRUCTURED_SOURCE_REVIEW_REQUIRED',
    studentActivityAndTeacherTaskCandidates:activityTexts.slice(-3),
    explicitTaskOrPartnerActivityTextFound:taskNarrativeExplicit,
    needsIndividualSourceReview:true,
    existingNarrativeRubricDrafts:rubricTexts,
    sourceTeacherUrl:lesson.sourceTeacherPreparationUrl,
    sourceStudentUrl:lesson.sourceStudentActivityUrlTemplate,
    structuredTeacherRubricAvailable:Boolean(reviewedCandidate),
    structuredRubricManifest:reviewedCandidate
      ?'docs/curriculum/US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json':null,
    independentlyAcademicallyAccepted:false,
    authenticatedTeacherSubmissionVerified:false,
    verifiedTeacherScore:false,
    privateFeedbackPersistenceVerified:false,
    safeguards:['do not auto-grade a graph or explanation','never claim synthetic data as authentic student evidence',
      'respect exact course and school enrollment','teacher must review actual student work']
  });
}
if(tasks.length!==132||new Set(tasks.map(t=>t.lessonCode)).size!==132)throw Error('Course task count drift');
const manifest={
 schemaVersion:1,asOfDate:'2026-10-29',courseKey:'US-CCSS-MATH:HS:TRAD:ALG1:IM-1E',
 classification:'DRAFT_TEXT_INVENTORY_NOT_TEACHER_GRADING_OR_ACADEMIC_ACCEPTANCE',
 existingNarrativeRubrics:tasks.length,structuredSourceSpotCheckedRubricDrafts:reviewed.size,
 teacherRubricsApproved:0,realTeacherScoresPersisted:0,
 assessedByHumanTeachers:0,lessons:tasks
};
fs.writeFileSync(output,JSON.stringify(manifest,null,2)+'\n');
ledger.v11.teacherTaskNarrativeInventory='docs/curriculum/US_CCSS_ALG1_CODER5_V11_ALL_TEACHER_TASK_INVENTORY_20261029.json';
ledger.v11.teacherRubricsInAuthoredNarratives=tasks.length;
ledger.v11.structuredRubricsWithSourceSpotChecks=reviewed.size;
fs.writeFileSync(ledgerPath,JSON.stringify(ledger,null,2)+'\n');
console.log(JSON.stringify({lessons:tasks.length,narrativeRubrics:tasks.length,
 structurallyReviewedDrafts:reviewed.size,teacherGradingPassed:0}));
