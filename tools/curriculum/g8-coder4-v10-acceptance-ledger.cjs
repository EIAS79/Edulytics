'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'../..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const v9=read('docs/curriculum/us-ccss-g8-coder4-v9-acceptance-ledger-20261029.json');
const activity=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const newer=read('docs/curriculum/us-ccss-g8-coder4-v10-original-math-representations-20261029.json');
const lessons=fs.readdirSync(path.join(root,dir)).filter(x=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(x))
.flatMap(x=>read(dir+'/'+x).Lessons);
const by=new Map(lessons.map(x=>[x.LessonCode,x]));
const sourceBy=new Map(activity.rows.map(x=>[x.lessonCode,x]));
const v10By=new Set(newer.rows.map(x=>x.lessonCode));
const rows=v9.rows.map(r=>{
 const l=by.get(r.lessonCode),p=sourceBy.get(r.lessonCode);
 if(!l||!p)throw Error('Missing lesson '+r.lessonCode);
 const sourceFigures=p.uniqueMathFiguresInActivities>0;
 return {
  ...r,figures:{
   ...r.figures,currentRichV2VisualCount:l.Visuals.length,
   currentRichV2VisualKinds:l.Visuals.map(v=>v.Kind),
   courseOwnedV10OriginalTableAdded:v10By.has(r.lessonCode),
   hasPublisherActivityFigures:sourceFigures,
   v10Decision:sourceFigures?
    (l.Visuals.length?'ORIGINAL_NUMERIC_TABLE_PRESENT_BUT_SOURCE_ART_PARITY_AND_REAL_GRAPH_NOT_VERIFIED':'SOURCE_ACTIVITY_FIGURES_AWAIT_PROVEN_ORIGINAL_ACCESSIBLE_REPRESENTATION'):
    (l.Visuals.length?'ORIGINAL_TABLE_PRESENT_SOURCE_ACTIVITY_GRAPH_REVIEW_STILL_REQUIRED':'NO_CMS_FIGURE_IN_STUDENT_ACTIVITY_REVIEW_TABLES_AND_INTERACTIONS'),
   effectiveBrowserFigureRenderingVerified:false,independentlyAccessibleSourceParityVerified:false
  },
  practice:{
   ...r.practice,exactCourseRuntimeAttemptGenerationVerified:false,
   independentCorrectWrongEquivalentMalformedBoundaryVerified:false,
   securePersistedStudentAttemptVerified:false,teacherReviewedRubricPersisted:false
  },
  stageBActualStudentLearnerRouteVerified:false,
  stageDTwoSchoolRoleIsolationVerified:false,
  A:'NOT_ACCEPTED',B:'NOT_ACCEPTED',C:'NOT_VERIFIED',D:'NOT_VERIFIED',
  status:'BLOCKED_WITH_CAUSE'
 };
});
const counts={authoredLessonCandidates:lessons.length,
 firstPartyWorkedExamples:lessons.reduce((n,l)=>n+l.WorkedExamples.length,0),
 lessonsWithVisuals:rows.filter(r=>r.figures.currentRichV2VisualCount>0).length,
 v10NewOriginalVisualLessons:v10By.size,
 sourceFigurePriorityDecisions:rows.length,
 studentSourceActivityFigurePriority:rows.filter(r=>r.figures.hasPublisherActivityFigures).length,
 sourceStudentActivities:rows.reduce((n,r)=>n+r.source.activityCount,0),
 teacherOnlyCoolDowns:rows.reduce((n,r)=>n+r.source.teacherOnlyActivityTitles.length,0),
 roleDecisions:rows.filter(r=>r.standardsRoles.hasDiscrepancy).length,
 sourceTables:activity.counts.totalActivityTables,
 studentSourceFigureOccurrences:activity.counts.totalActivityFigureOccurrences,
 AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,teacherAssessmentVerified:0};
if(counts.authoredLessonCandidates!==131||counts.firstPartyWorkedExamples!==393||
 counts.lessonsWithVisuals!==67||counts.v10NewOriginalVisualLessons!==66||
 counts.studentSourceActivityFigurePriority!==113||counts.sourceTables!==79||
 counts.sourceStudentActivities!==411||counts.teacherOnlyCoolDowns!==117||counts.roleDecisions!==22)
 throw Error('V10 ledger count mismatch '+JSON.stringify(counts));
const output='docs/curriculum/us-ccss-g8-coder4-v10-acceptance-ledger-20261029.json';
fs.writeFileSync(path.join(root,output),JSON.stringify({schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 methodology:'Reconcile actual Rich V2 embedded resources against real source student/teacher lesson HTML activity structure and per-lesson C/D risk. Original numeric tables are teaching assets, NOT source task fidelity or runtime acceptance.',
 sourceBasis:'2017–2019 IM Open Up Resources',
 security:'No live database mutation, no real student/teacher access, no unverified owner approval.',
 releaseDecision:'NO_GO',counts,rows},null,2)+'\n');
console.log('V10_ACCEPTANCE_LEDGER '+JSON.stringify({output,...counts}));
