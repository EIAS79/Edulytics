#!/usr/bin/env node
'use strict';
// Conservative acceptance evidence: source-link presence is not academic review.
// These counters MUST NOT be promoted by the existence of static Rich V2 files.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const ledgerFile=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json');
const progressFile=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json');
const progress=JSON.parse(fs.readFileSync(progressFile,'utf8'));
const codes=new Set();
const unitCounts={};
const visualKindCounts={};
const rows=[];
for(const lesson of progress.lessons){
  const code=lesson.lessonCode;
  if(!code.startsWith('PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:'))
    throw Error('Foreign course in Algebra 1 ledger: '+code);
  if(codes.has(code))throw Error('Duplicate canonical lesson code: '+code);
  codes.add(code);
  const filename=lesson.authoredRichV2Sidecar;
  if(!filename || !filename.startsWith('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg1-'))
    throw Error('Missing isolated Algebra 1 candidate: '+code);
  const doc=JSON.parse(fs.readFileSync(path.join(root,filename),'utf8'));
  if(doc.Lessons.length!==1 || doc.Lessons[0].LessonCode!==code ||
     doc.Lessons[0].Title!==lesson.title)
    throw Error('Exact identity/title mismatch: '+code);
  const authored=doc.Lessons[0];
  const text=authored.ExplanationParagraphs.join(' ');
  const teacher=lesson.officialTeacherPreparationUrl;
  const student=teacher.replace('/teachers/','/students/').replace('/preparation.html','/index.html');
  if(!text.includes(teacher))
    throw Error('Source teacher reference missing: '+code);
  if(authored.WorkedExamples.length<3 || authored.KeyConcepts.length<3 ||
     authored.SummaryPoints.length<3)
    throw Error('Source candidate structural deficiency: '+code);
  unitCounts[lesson.unitNumber]=(unitCounts[lesson.unitNumber]||0)+1;
  for(const visual of authored.Visuals)
    visualKindCounts[visual.Kind]=(visualKindCounts[visual.Kind]||0)+1;
  rows.push({
    lessonCode:code,unitNumber:lesson.unitNumber,lessonNumber:lesson.lessonNumber,
    title:lesson.title,sourceLessonCode:lesson.sourceLessonCode,
    sourceTeacherPreparationUrl:teacher,
    sourceStudentActivityUrlTemplate:student,
    studentActivityUrlExplicitlyInAuthoredExplanation:text.includes(student),
    sourceLinksPresent:true,
    contentVersion:doc.ContentVersion,
    candidatePath:filename,
    authoringCandidatePresent:true,
    humanTeacherAssessmentWorkflowVerified:false,
    A:{accepted:false,code:'PENDING_INDEPENDENT_TEACHER_AND_STUDENT_TASK_ALIGNMENT'},
    B:{accepted:false,code:'PENDING_EFFECTIVE_AUTHENTICATED_RENDER_AND_CONTENT_VERSION'},
    C:{verified:false,code:'PENDING_EXACT_PRACTICE_OR_TEACHER_RUBRIC_GRADE_AND_PERSISTENCE'},
    D:{verified:false,code:'PENDING_AUTHORIZED_TWO_SCHOOL_HTTPS_DATABASE_PROOF'}
  });
}
if(rows.length!==132 || Object.values(unitCounts).reduce((a,b)=>a+b,0)!==132)
  throw Error('Wrong canonical 132/7 unit inventory.');
const report={
  schemaVersion:1,
  courseKey:progress.courseKey,
  asOfDate:'2026-10-29',
  scope:'V10 course-local source-identity evidence only; explicitly NOT an academic approval',
  sourceTeacherLinkCount:rows.filter(x=>x.sourceLinksPresent).length,
  explicitStudentActivityLinkCount:rows.filter(x=>x.studentActivityUrlExplicitlyInAuthoredExplanation).length,
  candidateCount:rows.length,
  unitCounts,
  visualKindCounts,
  independentSourceSpotChecks:[
    {
      lessonCode:'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L01',
      studentUrl:'https://im.kendallhunt.com/HS/students/1/1/1/index.html',
      teacherUrl:'https://im.kendallhunt.com/HS/teachers/1/1/1/preparation.html',
      finding:'Original student activity surveys 15 classmates with four questions and retains data; first-party candidate amended to preserve task and protect respondent privacy.',
      acceptance:'PRELIMINARY_SOURCE_COMPARISON_ONLY'
    },
    {
      lessonCode:'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L24',
      studentUrl:'https://im.kendallhunt.com/HS/students/1/7/24/index.html',
      teacherUrl:'https://im.kendallhunt.com/HS/teachers/1/7/24/preparation.html',
      finding:'Source asks students to independently model diver height and reason about a parabola-line intersection; first-party candidate uses a separate quadratic/line model but lacks verified interactive plotted graph.',
      acceptance:'PRELIMINARY_SOURCE_COMPARISON_ONLY'
    }
  ],
  gates:{A_ACCEPTED:0,B_ACCEPTED:0,C_VERIFIED:0,D_VERIFIED:0,
    TEACHER_ASSESSMENT_VERIFIED:0,AUTH_TWO_SCHOOL_VERIFIED:0},
  verificationLimitations:[
    'Static source links do not demonstrate that the teacher goal and each student activity was independently audited.',
    'A Rich V2 registry sidecar is not evidence of served content, canonical publication, content-version selection, or authenticated user access.',
    'An authored teacher rubric is not evidence of implemented authenticated teacher grade/feedback/persistence.',
    'Exact Practice readiness and assessment grading require live contract/engine tests, not title or official-outcome fallback.',
    'All current Algebra 1 Rich V2 visuals are RatioTable objects; these do not prove actual graph, geometric diagram, accessible figure, or plotting-task fidelity for graph-intensive lessons.',
    'A synthetic fake repository test is not a two-school HTTPS/disposable-database integration test.',
    'Do not merge or deploy while unresolved.'
  ],
  lessons:rows
};
fs.writeFileSync(ledgerFile,JSON.stringify(report,null,2)+'\n');
progress.acceptanceGates={
  ...(progress.acceptanceGates||{}),
  asOfDate:'2026-10-29',
  ledgerPath:'docs/curriculum/US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json',
  A_ACCEPTED:0,B_ACCEPTED:0,C_VERIFIED:0,D_VERIFIED:0,
  teacherAssessmentVerified:0,authenticatedTwoSchoolVerified:0,
  note:'Historical source reviewed authoring flags are not stage A source-activity acceptance; static links do not prove A-D. Do not promote until exact per-lesson evidence.'
};
fs.writeFileSync(progressFile,JSON.stringify(progress,null,2)+'\n');
console.log(JSON.stringify({candidates:132,explicitStudentLinks:report.explicitStudentActivityLinkCount,unitCounts,gates:report.gates}));
