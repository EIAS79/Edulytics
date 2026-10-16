#!/usr/bin/env node
'use strict';

/**
 * Source-grounded B001 500-row P2/P3 remediation ledger.
 *
 * DO NOT mistake RAW source-pack template signals for learner-visible effective
 * composition. This audit identifies risk/candidates; it performs no grading,
 * browser UAT, independent math verification, publisher-asset licensing approval,
 * or release acceptance.
 */
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const batch=path.join(root,'docs/curriculum/batches');
const read=filename=>JSON.parse(fs.readFileSync(filename,'utf8'));
const source=name=>read(path.join(batch,name));
const files=[1,2,3,4].map(n=>path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs',
  'us-ccss-math-g'+n+'-phase29-v1.lesson-content-pack.json'));

const normalized=s=>String(s||'').toLowerCase()
  .replace(/[0-9]+/g,'#').replace(/\s+/g,' ').trim();

function build(){
  const catalogue=source('US-CCSS-B001-CATALOGUE.json');
  const original=[
    ...source('US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json').lessons,
    ...source('US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-2-OF-2.json').lessons
  ];
  const packs=files.map(read);
  const canonical=packs.flatMap(x=>x.lessons);
  const byCode=new Map(canonical.map(x=>[x.lessonCode,x]));
  if(catalogue.batchId!=='US-CCSS-B001'||catalogue.total!==500||
    catalogue.lessons.length!==500||original.length!==500||
    byCode.size!==canonical.length||new Set(catalogue.lessons.map(x=>x.lessonCode)).size!==500)
    throw Error('Refusing incomplete or duplicated B001 source inventory');

  const duplicates=new Map();
  for(const item of catalogue.lessons){
    const pack=byCode.get(item.lessonCode);
    const en=pack?.translations?.find(t=>t.cultureCode==='en');
    if(!en)throw Error('B001 original embedded learner English missing: '+item.lessonCode);
    const key=item.gradeOrCourse+'|'+normalized(en.stepByStepSolutions);
    duplicates.set(key,(duplicates.get(key)||0)+1);
  }

  const rows=catalogue.lessons.map((item,index)=>{
    const packet=original[index],pack=byCode.get(item.lessonCode);
    if(!packet||packet.code!==item.lessonCode||packet.ordinal!==index+1||
      item.batchOrdinal!==index+1||!pack||
      pack.sourceUrl!==item.sourceUrl ||
      pack.canonicalBodySha256!==item.storedContentSha256||
      !Array.isArray(pack.outcomeCodes)||!Array.isArray(item.outcomeCodes)||
      JSON.stringify(pack.outcomeCodes)!==JSON.stringify(item.outcomeCodes)||
      !item.sourceEdition||!item.recordedLicense||!packet.teacherPurposeExcerpt)
      throw Error('B001 lesson/source/hash/outcome mismatch at ordinal '+(index+1));

    const en=pack.translations.find(t=>t.cultureCode==='en');
    const activities=packet.originalActivities||[];
    const studentExcerpts=activities.filter(x=>x.hasStudentFacing&&
      typeof x.studentFacingExcerpt==='string'&&x.studentFacingExcerpt.trim());
    const activityText=activities.map(x=>(x.name||'')+' '+(x.studentFacingExcerpt||'')).join(' ');
    const rawGenericExample=/the source task (combines|compares|removes)/i.test(en.workedExamples||'');
    const rawGenericStep=/step 1: (represent the quantities or objects|identify what the task asks you to find or show)/i
      .test(en.stepByStepSolutions||'');
    const stepTemplateCount=duplicates.get(item.gradeOrCourse+'|'+normalized(en.stepByStepSolutions))||1;
    const sourceGoalMissing=!String(packet.studentFacingGoalExcerpt||'').trim();
    // The original first-edition publisher identifies this as an optional
    // applied-modeling lesson and supplies no separate teacher/student goal.
    // Preserve the blank source fields; do not fabricate a goal to silence a
    // generic extraction warning. Activity parity is still unverified.
    const optionalWithoutSeparateGoal=sourceGoalMissing &&
      item.lessonCode==='PED:US-CCSS-MATH:G2:U02:L16' &&
      item.title==='Our Market’s Inventory (optional)' &&
      !String(packet.teacherLearningGoalsExcerpt||'').trim() &&
      String(packet.teacherPurposeExcerpt||'').includes(
        'apply strategies for adding and subtracting within 100 within the context of a store') &&
      activities.length===3;
    const taskNeedsWrittenOrVisualEvidence=/draw|drawing|sketch|diagram|picture|graph|chart|table|plot|explain|show your thinking|tell how|justify|center|game|sort|build|create|make a story|write a story|survey|partner/i
      .test(activityText);
    const sourceActivityTextNotCaptured=activities.length>0 &&
      studentExcerpts.length===0;
    const reviewFlags=[
      ...(sourceGoalMissing?[optionalWithoutSeparateGoal?'OPTIONAL_LESSON_NO_SEPARATE_STUDENT_GOAL_IN_SOURCE':'STUDENT_GOAL_EXCERPT_MISSING']:[]),
      ...(sourceActivityTextNotCaptured?['NO_STUDENT_FACING_ACTIVITY_EXCERPT']:[]),
      ...(rawGenericExample?['RAW_PACK_GENERIC_EXAMPLES']:[]),
      ...(rawGenericStep?['RAW_PACK_GENERIC_STEPS']:[]),
      ...(stepTemplateCount>=5?['RAW_PACK_REPEATED_STEP_TEMPLATE']:[]),
      ...(taskNeedsWrittenOrVisualEvidence?['SOURCE_ACTIVITY_NEEDS_INTERACTION_PARITY_REVIEW']:[])
    ];
    const urgency=sourceGoalMissing&&!optionalWithoutSeparateGoal?'P0_SOURCE_GOAL':
      rawGenericExample||rawGenericStep||stepTemplateCount>=5?
      'P1_LEARNER_BODY_TEMPLATE':taskNeedsWrittenOrVisualEvidence?
      'P2_STUDENT_ACTIVITY_PARITY':'P3_OTHER_SOURCE_PARITY';
    return {
      ordinal:index+1,lessonCode:item.lessonCode,grade:item.gradeOrCourse,
      gradeOrdinal:item.gradeOrdinal,title:item.title,sourceUrl:item.sourceUrl,
      sourceEdition:item.sourceEdition,licenceRecorded:item.recordedLicense,
      originalStudentGoalExcerptPresent:!sourceGoalMissing,
      originalGoalAbsenceVerifiedOptional:optionalWithoutSeparateGoal,
      originalTeacherPurpose:packet.teacherPurposeExcerpt,
      sourceActivityCount:activities.length,
      originalStudentFacingActivityExcerptCount:studentExcerpts.length,
      originalActivityNames:activities.map(a=>a.name).filter(Boolean),
      sourceActivityNeedsInteractionParityReview:taskNeedsWrittenOrVisualEvidence,
      rawEmbeddedGenericExample:rawGenericExample,
      rawEmbeddedGenericSteps:rawGenericStep,
      rawEmbeddedNormalizedStepTemplateUsesInGrade:stepTemplateCount,
      rawContentSha256:item.storedContentSha256,
      sourceSemanticSha256:item.semanticSha256,
      historicalFirst15PreviouslyPublished:index<15,
      reviewPriority:urgency,reviewFlags,
      // No contents of this source-level ledger warrant a TESTED/PASS/APPROVED assertion.
      currentRenderedSourceParity:'NOT_VERIFIED_BY_SOURCE_RISK_AUDIT',
      lessonIndependentMathProof:'NOT_VERIFIED_BY_SOURCE_RISK_AUDIT',
      lessonAuthenticatedProductionUat:'NOT_VERIFIED_BY_SOURCE_RISK_AUDIT',
      internalAcceptance:'NOT_GRANTED_BY_SOURCE_RISK_AUDIT',
      nextAction:sourceGoalMissing&&!optionalWithoutSeparateGoal?'Recover missing original student-goal evidence':
        optionalWithoutSeparateGoal?'Verify full market inventory, buying/selling and sales-report activity parity with publisher source and live learner interaction':
        rawGenericExample||rawGenericStep||stepTemplateCount>=5?
          'Compare EFFECTIVE learner-rendered explanation/examples/steps with original lesson purpose and repair only genuine misalignment':
          taskNeedsWrittenOrVisualEvidence?
            'Verify original student activity representation and authenticated interaction, not just numerical grading':
            'Verify source pedagogical parity and source-specific mathematics with real reproducible evidence'
    };
  });

  const grades=Object.create(null);
  const priorities=Object.create(null);
  for(const row of rows){
    if(!grades[row.grade])grades[row.grade]={
      total:0,rawGenericExamples:0,rawGenericSteps:0,repeatedRawStepTemplates:0,
      activityInteractionParityReview:0,missingStudentGoal:0,verifiedOptionalWithoutGoal:0,historicalPrior15:0
    };
    const g=grades[row.grade];
    g.total++;
    if(row.rawEmbeddedGenericExample)g.rawGenericExamples++;
    if(row.rawEmbeddedGenericSteps)g.rawGenericSteps++;
    if(row.rawEmbeddedNormalizedStepTemplateUsesInGrade>=5)g.repeatedRawStepTemplates++;
    if(row.sourceActivityNeedsInteractionParityReview)g.activityInteractionParityReview++;
    if(!row.originalStudentGoalExcerptPresent)g.missingStudentGoal++;
    if(row.originalGoalAbsenceVerifiedOptional)g.verifiedOptionalWithoutGoal++;
    if(row.historicalFirst15PreviouslyPublished)g.historicalPrior15++;
    priorities[row.reviewPriority]=(priorities[row.reviewPriority]||0)+1;
  }
  if(rows.length!==500||Object.values(grades).reduce((n,g)=>n+g.total,0)!==500)
    throw Error('B001 group counts must exactly partition 500 source lessons');
  return {
    schemaVersion:1,batchId:'US-CCSS-B001',scope:500,
    scopeKind:'ORIGINAL_SOURCE_AND_RAW_EMBEDDED_PACK_RISK_AUDIT',
    // Distinguish source screening from actual post-correction effective learner output.
    method:'Exact canonical/source/embedded pack crosswalk, original 2021 teaching packets, heuristic raw pack duplication and student-activity signals. DOES NOT execute the Rich V2 materializer or academic acceptance gate.',
    historical15:15,grades,priorityCounts:priorities,
    independentAcademicAcceptedInThisAudit:0,
    fullBatchPublishAuthorization:false,
    releaseDecision:'NO_GO',
    caveat:'The original G2 U02 L16 optional market lesson contains activities but no separately stated goals; absence remains recorded, not fabricated. Repeated RAW generic prose is a prioritization flag, not proof the effective student view is wrong. No external teacher signoff is needed for internal V2 acceptance; independent source and mathematical evidence for all 500 is still required.',
    rows
  };
}

function main(){
  const data=build();
  const out=process.argv.find(x=>x.startsWith('--out='))?.slice(6)||
    path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/B001');
  fs.mkdirSync(out,{recursive:true});
  fs.writeFileSync(path.join(out,'source-aligned-500-lesson-remediation-ledger.json'),
    JSON.stringify(data,null,2)+'\n');
  process.stdout.write(JSON.stringify({
    batch:data.batchId,scope:data.scope,grades:data.grades,
    priorities:data.priorityCounts,approved:data.independentAcademicAcceptedInThisAudit,
    decision:data.releaseDecision,out
  })+'\n');
}
if(require.main===module)main();
module.exports={build};
