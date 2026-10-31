'use strict';
// Evidence inventory only. No academic grade or runtime success is inferred.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
const inventory=read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const practice=read('docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json');
const rubrics=read('docs/curriculum/us-ccss-g8-coder4-teacher-rubric-candidates-20261029.json');
const byPractice=new Map(practice.rows.map(x=>[x.lessonCode,x]));
const byRubric=new Map(rubrics.lessons.map(x=>[x.lessonCode,x]));
const visualTerms=/draw|sketch|construct|geometry|geometric|graph|scatter|grid|coordinate|diagram|figure|triangle|polygon|angle|reflection|rotation|translation|dilation|tessellat|solid|cylinder|sphere|cone|line of fit|number line/i;
const rows=inventory.rows.map(row=>{
 const p=byPractice.get(row.lessonCode),r=byRubric.get(row.lessonCode);
 if(!p||!r||!row.authoredRichV2Asset)throw Error('Missing evidence '+row.lessonCode);
 const targets=p.officialOutcomeTargets||[];
 const unresolved=targets.filter(t=>!t.resolvedStatic);
 const nonMp=unresolved.filter(t=>!t.code.startsWith('CCSS:MP.'));
 const mpOnly=targets.length>0&&targets.every(t=>t.code.startsWith('CCSS:MP.'));
 const families=[...new Set(targets.filter(t=>t.resolvedStatic).map(t=>t.targetRuleId))];
 const visibleNeed=visualTerms.test(row.publisherStudentGoal+' '+r.authoredTaskPrompt);
 const oneVariableFamilyOnTwoVariableLesson=row.lessonCode==='PED:US-CCSS-MATH:G8:U03:L12' && (p.explicitLessonSkill?.approvedFamilies||[]).includes('algebra.linear.ax_plus_b_equals_c');
 const classifications=['SOURCE_REVIEW_BLOCKED','TEACHER_ASSESSMENT'];
 if(mpOnly||nonMp.length||families.length>1||oneVariableFamilyOnTwoVariableLesson)classifications.push('PRACTICE_REPAIR');
 if(visibleNeed)classifications.push('CONTENT_REPAIR');
 classifications.push('SHARED_CAPABILITY_REQUIRED');
 return {lessonCode:row.lessonCode,sourceLessonCode:row.sourceLessonCode,unit:row.unit,title:row.title,
  sourceUrl:row.sourceUrl,sourceGoal:row.publisherStudentGoal,
  classification:classifications,classificationConfidence:'TRIAGE_ONLY_NOT_AN_ACADEMIC_DECISION',
  evidence:{richV2Candidate:row.authoredRichV2Asset,sourcePreparationPageFetched:true,
   fullStudentActivitySourceReview:false,visualScaffoldingReviewNeeded:visibleNeed,
   standardRoleDiscrepancy:row.blueprintRoleReviewNeeded,
   mappedPracticeFamilies:families,mathematicalPracticeOnlyOutcomes:mpOnly,
   unmappedMathematicalPracticeOutcomeCodes:unresolved.filter(t=>t.code.startsWith('CCSS:MP.')).map(t=>t.code),
   unmappedFormalOutcomeCodes:nonMp.map(t=>t.code),
   multipleMappedPracticeFamilies:families.length>1,
   semanticPracticeMismatchCandidate:oneVariableFamilyOnTwoVariableLesson,
   semanticPracticeMismatchReason:oneVariableFamilyOnTwoVariableLesson?'Publisher goal is solving a two-variable linear equation, but explicit Practice family asks to solve one-variable ax+b=c. Requires course- and shared-registry repair.':null,
   draftTeacherScoringRubric:true,integratedTeacherScoringRubric:false,
   actualAuthLearnerRendering:false,actualPracticeGenerated:false,practiceSubmissionScored:false,
   savedAttemptVerified:false,wrongLessonAccessDeniedVerified:false,twoSchoolIsolationVerified:false,
   accessibleStudentFiguresVerified:false,academicA:false,academicB:false,academicC:false,academicD:false},
  nextEvidence:['Publisher student activity-by-activity and drawing verification',
   'Inspect the actual authenticated student-rendered Rich V2 lesson and accessibility',
   'Run a lesson-exact Practice question with independent oracle and wrong-answer feedback',
   'Check saved attempts and tenant/course/role negative access controls',
   'Observe teacher-scored original task where automated assessment is insufficient',
   'Attach frozen-SHA CI and independent A-D academic acceptance']
 };
});
const count=label=>rows.filter(x=>x.classification.includes(label)).length;
const report={schemaVersion:1,scope:'US-CCSS-MATH:G8:OUR-IM-2017',
 checkpointSha:'e644d5e264e078a55e37803aa8aac25763b0bd5c',
 status:'TRIAGE_ONLY_NOT_RELEASE_READY',totalLessons:rows.length,
 coverage:{richCandidates:131,sourcePreparationRetrieved:131,
 sourceRoleDiscrepancy:rows.filter(x=>x.evidence.standardRoleDiscrepancy).length,
 needsVisualScaffoldingInspection:rows.filter(x=>x.evidence.visualScaffoldingReviewNeeded).length,
 mathematicalPracticeOnlyOutcomes:rows.filter(x=>x.evidence.mathematicalPracticeOnlyOutcomes).length,
 unmappedFormalOutcomes:rows.filter(x=>x.evidence.unmappedFormalOutcomeCodes.length).length,
 multiPracticeFamilyTargets:rows.filter(x=>x.evidence.multipleMappedPracticeFamilies).length,
 explicitSkillSemanticMismatchCandidates:rows.filter(x=>x.evidence.semanticPracticeMismatchCandidate).length,
 readyReuse:count('READY_REUSE'),contentRepairReview:count('CONTENT_REPAIR'),
 practiceRepairReview:count('PRACTICE_REPAIR'),teacherAssessment:count('TEACHER_ASSESSMENT'),
 sharedCapabilityRequired:count('SHARED_CAPABILITY_REQUIRED'),sourceReviewBlocked:count('SOURCE_REVIEW_BLOCKED'),
 acceptedA:0,acceptedB:0,verifiedPracticeC:0,verifiedRuntimeD:0},
 caveat:'Classification flags are review priorities based on extant metadata and candidate tasks; missing verification is not equivalent to a proved broken feature. No 131 lessons are academically accepted and no production release is authorized.',
 rows};
if(rows.length!==131||new Set(rows.map(x=>x.lessonCode)).size!==131)throw Error('Wrong count');
const out='docs/curriculum/us-ccss-g8-coder4-v7-lesson-gates-20261029.json';
fs.writeFileSync(path.join(root,out),JSON.stringify(report,null,2)+'\n');
console.log('V7_GATES '+JSON.stringify({file:out,coverage:report.coverage}));
