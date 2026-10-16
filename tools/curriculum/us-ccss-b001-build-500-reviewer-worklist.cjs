#!/usr/bin/env node
'use strict';

/**
 * Single B001 (all 500) human reviewer worklist, never a release approval.
 * Sources are local publisher-sourced dossiers and manifest snapshots.
 * No publisher PDF bytes, full teacher lessons or personal pupil data included.
 */
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const batches=path.join(root,'docs','curriculum','batches');
const read=name=>JSON.parse(fs.readFileSync(path.join(batches,name),'utf8'));
const catalogue=read('US-CCSS-B001-CATALOGUE.json');
const pdfs=read('US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json');
const packets=[
  ...read('US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json').lessons,
  ...read('US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-2-OF-2.json').lessons
];
const selected=read('US-CCSS-B001-G1-U02-L04-L10-ORIGINAL-STUDENT-PDF-TEXT-AND-REVIEW-BLOCKERS.json');
const lessonMapping=JSON.parse(fs.readFileSync(path.join(root,
 'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'),'utf8'));
const byMapping=new Map(lessonMapping.mappings.map(x=>[x.lessonCode,x]));
const bySelected=new Map(selected.lessons.map(x=>[x.lessonCode,x]));
const fields=[
 'batchOrdinal','lessonCode','grade','title','teacherSourceUrl','studentTaskPdfUrl',
 'sourceEdition','teacherPurpose','studentGoal','originalActivityNames',
 'baselineSkillFamilies','sourceReviewFlags','pdfTextInspection',
 'graphicsVisualReview','realClassroomActivityUat','independentMathOracleApproval',
 'teacherSignoff','releaseDecision','specificReviewGaps'
];
const csvValue=x=>'"'+String(x==null?'':Array.isArray(x)?x.join(' | '):x)
 .replace(/\r?\n/g,' ').replace(/"/g,'""')+'"';
function build(){
  if(catalogue.batchId!=='US-CCSS-B001'||catalogue.total!==500||
     pdfs.count!==500||packets.length!==500||selected.lessons.length!==7||
     catalogue.lessons.length!==500||pdfs.lessons.length!==500)
    throw Error('B001 exact canonical 500 source/PDF packet completeness failed');
  const rows=catalogue.lessons.map((c,i)=>{
    const p=packets[i],f=pdfs.lessons[i],s=bySelected.get(c.lessonCode);
    if(c.lessonCode!==p.code||c.lessonCode!==f.lessonCode||
       c.batchOrdinal!==i+1||f.ordinal!==i+1||p.ordinal!==i+1||
       c.sourceUrl!==f.sourceTeacherPreparationUrl||
       !f.originalStudentTaskStatementPdfUrl||
       !p.teacherPurposeExcerpt)
      throw Error('B001 source/PDF identity drift at ordinal '+(i+1));
    if(s&&(s.ordinal!==i+1||s.sourceStudentPdfUrl!==f.originalStudentTaskStatementPdfUrl||
       s.academicDecision!=='NO_GO'||s.independentTeacherDecision!=='UNSIGNED'))
      throw Error('Seven PDF source text observations must remain unsigned and source-locked');
    const mapping=byMapping.get(c.lessonCode);
    const row={
      batchOrdinal:i+1,lessonCode:c.lessonCode,grade:c.gradeOrCourse,
      title:c.title,teacherSourceUrl:c.sourceUrl,
      studentTaskPdfUrl:f.originalStudentTaskStatementPdfUrl,
      sourceEdition:c.sourceEdition,
      teacherPurpose:p.teacherPurposeExcerpt,
      studentGoal:p.studentFacingGoalExcerpt||'[NOT PRESENT IN ORIGINAL STUDENT GOAL EXTRACTION — REVIEW REQUIRED]',
      originalActivityNames:p.originalActivities.map(x=>x.name).filter(Boolean),
      baselineSkillFamilies:mapping?.allowedQuestionFamilies||[],
      sourceReviewFlags:p.reviewFlags||[],
      pdfTextInspection:s?'TEXT_EXTRACTED_AND_COMPARED_TO_CANDIDATE':'NOT_INDIVIDUALLY_INSPECTED',
      graphicsVisualReview:'PENDING',
      realClassroomActivityUat:'PENDING',
      independentMathOracleApproval:'PENDING_HUMAN_REVIEW',
      teacherSignoff:'UNSIGNED',
      releaseDecision:'NO_GO',
      specificReviewGaps:[
        ...(s?s.blockingFindings:[
          'Independently compare the original student PDF page content and visual diagrams with the learner body and exact generated Practice family.',
          'Verify real student activities and mathematical correctness; obtain independent qualified-teacher signoff.'
        ]),
        ...(!p.studentFacingGoalExcerpt?[
          'The original student-facing learning-goal excerpt was absent; inspect full 2021 student task and teacher goal, do not synthesize it.'
        ]:[])
      ]
    };
    if(row.baselineSkillFamilies.some(x=>typeof x!=='string'))
      throw Error('Malformed baseline SkillContract families: '+c.lessonCode);
    return row;
  });
  if(new Set(rows.map(x=>x.lessonCode)).size!==500||
     rows.filter(x=>x.pdfTextInspection==='TEXT_EXTRACTED_AND_COMPARED_TO_CANDIDATE').length!==7)
    throw Error('B001 review dossier shape/review count mismatch');
  return {
    schemaVersion:1,batchId:'US-CCSS-B001',total:500,
    observedOriginalStudentPdfTextInspectionCount:7,
    missingOriginalStudentFacingGoalExcerptCount:rows.filter(x=>x.studentGoal.startsWith('[NOT PRESENT')).length,
    independentGraphicsApprovals:0,independentMathApprovals:0,
    qualifiedTeacherApprovals:0,productionLearnerUatApprovals:0,
    academicReleaseDecision:'NO_GO',
    assurance:'Reviewer worklist, not publisher fidelity, independent solver proof, actual classroom UAT or a teacher certification. Baseline family mappings can be overridden by source-specific runtime contracts and must be rechecked at the exact candidate SHA.',
    rows
  };
}
function main(){
  const dossier=build();
  const out=process.argv.find(x=>x.startsWith('--out='))?.slice('--out='.length)||
    path.join(root,'artifacts','curriculum-delivery','US-CCSS-MATH','B001');
  fs.mkdirSync(out,{recursive:true});
  fs.writeFileSync(path.join(out,'b001-500-source-pdf-math-human-review-worklist.json'),
    JSON.stringify(dossier,null,2)+'\n');
  fs.writeFileSync(path.join(out,'b001-500-source-pdf-math-human-review-worklist.csv'),
    fields.join(',')+'\n'+dossier.rows.map(row=>fields.map(x=>csvValue(row[x])).join(',')).join('\n')+'\n');
  process.stdout.write(JSON.stringify({batch:dossier.batchId,rows:dossier.total,
    textInspected:dossier.observedOriginalStudentPdfTextInspectionCount,
    qualifiedTeacherApprovals:0,release:dossier.academicReleaseDecision,out})+'\n');
}
if(require.main===module)main();
module.exports={build};
