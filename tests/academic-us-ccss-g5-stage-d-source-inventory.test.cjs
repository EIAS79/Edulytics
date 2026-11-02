'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {spawnSync}=require('node:child_process');
const {analyzeElementInventory,analyzeEditionPair,grade5EditionManifests}=
  require('../tools/curriculum/g5-stage-d-diagnostic.cjs');

const root=path.resolve(__dirname,'..');
const gradePath='artifacts/curriculum-delivery/US-CCSS-MATH/G5';
const code=n=>'PED:US-CCSS-MATH:G5:U01:L'+String(n).padStart(2,'0');
const fixture=n=>JSON.parse(fs.readFileSync(path.join(root,gradePath,
  'u01-l'+String(n).padStart(2,'0')+'-abc-element-ledger-20261030.json'),'utf8'));

test('all existing U01 L04-L07 source-element array schemas are itemized',()=>{
  // The 2021 second pass retained each unavailable publisher item explicitly;
  // these are not part of the matched count or academic approval denominator.
  const counts=new Map([[4,[21,20]],[5,[25,24]],[6,[21,19]],[7,[17,16]]]);
  for(const [lesson,[total,matched]] of counts){
    const source=fixture(lesson);
    const audit=analyzeElementInventory(source,code(lesson));
    assert.equal(audit.sourceElements.length,total,'lesson '+lesson);
    assert.equal(audit.identified,total);
    assert.equal(audit.matched,matched);
    assert.equal(source.sourceElementsAccepted,matched);
    assert.ok(audit.blockers.includes('UNMATCHED_IDENTIFIED_SOURCE_ELEMENTS'),
     'unavailable publisher source items must remain blockers '+lesson);
    assert.equal(source.status,'READY_FOR_STAGE_D');
    assert.equal(source.readyForStageD,true);
    assert.equal(source.academicAccepted,false);
    assert.equal(source.sourceCoverage100Percent,false);
  }
});
test('unstructured descriptive source outline is never counted as itemized parity',()=>{
  const source=JSON.parse(fs.readFileSync(path.join(root,gradePath,
    'u03-l04-abc-sequential-closure-20261101.json'),'utf8'));
  const audit=analyzeElementInventory(source,'PED:US-CCSS-MATH:G5:U03:L04');
  assert.equal(audit.hasSourceElementArray,true);
  assert.equal(audit.descriptiveOutlineCount,5);
  assert.equal(audit.sourceElements,null);
  assert.equal(audit.matched,null);
  assert.ok(audit.blockers.includes('SOURCE_ELEMENT_ARRAY_IS_NOT_STRUCTURED_1_TO_1_EVIDENCE'));
});
test('structured element identities with missing snapshots remain blocked',()=>{
  const source=JSON.parse(fs.readFileSync(path.join(root,gradePath,
    'u01-l02-abc-element-ledger-20261030.json'),'utf8'));
  const audit=analyzeElementInventory(source,code(2));
  assert.ok(audit.sourceElements.length>0);
  assert.ok(audit.blockers.includes('MISSING_ELEMENT_SOURCE_SNAPSHOT'));
  assert.equal(source.academicAccepted,false);
});
test('unknown element inventory stays unknown, not successful',()=>{
  const audit=analyzeElementInventory({lessonCode:code(1)},code(1));
  assert.equal(audit.identified,null);
  assert.equal(audit.matched,null);
  assert.ok(audit.blockers.includes('SOURCE_ITEMIZED_DENOMINATOR_UNKNOWN'));
});
test('declarative count alone does not create itemized proof',()=>{
  const audit=analyzeElementInventory({sourceElementsCount:12},code(1));
  assert.equal(audit.identified,12);
  assert.equal(audit.sourceElements,null);
  assert.ok(audit.blockers.includes('SOURCE_ITEMIZED_DENOMINATOR_UNKNOWN'));
});
test('reject duplicated source ID, wrong lesson, missing snapshot and incorrect accepted totals',()=>{
  const audit=analyzeElementInventory({
    sourceElementsCount:3,sourceElementsAccepted:3,
    sourceElements:[
      {SourceElementId:'duplicate',LessonCode:code(4),SourceSnapshotId:'source-A',Status:'MATCHED_DIGITAL'},
      {SourceElementId:'duplicate',LessonCode:code(5),SourceSnapshotId:'',Status:'UNKNOWN'}
    ]
  },code(4));
  for(const failure of ['SOURCE_ITEM_COUNT_MISMATCH','ACCEPTED_ELEMENT_COUNT_MISMATCH',
    'DUPLICATE_SOURCE_ELEMENT_ID','SOURCE_ELEMENT_LESSON_CODE_MISMATCH',
    'MISSING_ELEMENT_SOURCE_SNAPSHOT','UNMATCHED_IDENTIFIED_SOURCE_ELEMENTS'])
    assert.ok(audit.blockers.includes(failure),failure);
  assert.equal(audit.matched,1);
});

test('archived v360 student mirror is detected but never substitutes for active IM2021 source',()=>{
  const entries=grade5EditionManifests();
  assert.equal(entries.size,148);
  for(const [lessonCode,rows] of entries){
    const result=analyzeEditionPair(lessonCode,rows);
    assert.equal(result.teacherTitle,'Illustrative Mathematics | Kendall Hunt');
    assert.equal(result.studentTitle,'Illustrative Mathematics | v.360 Curriculum');
    assert.ok(result.blockers.includes('SOURCE_EDITION_MISMATCH_V360_STUDENT'));
    assert.ok(result.blockers.includes('STUDENT_IM2021_EDITION_NOT_VERIFIED'));
    assert.ok(result.blockers.includes('TEACHER_STUDENT_SEMANTIC_PARITY_NOT_CERTIFIED'));
    assert.match(result.teacherSha256,/^[a-f0-9]{64}$/);
    assert.match(result.studentSha256,/^[a-f0-9]{64}$/);
  }
});
test('missing, duplicate, forged and cross-edition student evidence cannot pass source gate',()=>{
  const lessonCode='PED:US-CCSS-MATH:G5:U01:L01';
  const entries=grade5EditionManifests().get(lessonCode);
  const teacher=entries.find(x=>x.role==='Teacher');
  const student=entries.find(x=>x.role==='Student');
  assert.ok(teacher&&student,'both source roles must be identified by role, never array order');
  const valid=analyzeEditionPair(lessonCode,[teacher,student]);
  assert.ok(valid.blockers.includes('SOURCE_EDITION_MISMATCH_V360_STUDENT'));
  const dropped=analyzeEditionPair(lessonCode,[teacher]);
  assert.ok(dropped.blockers.includes('SOURCE_EDITION_MISSING_OR_DUPLICATE_ROLE'));
  const duplicated=analyzeEditionPair(lessonCode,[teacher,student,student]);
  assert.ok(duplicated.blockers.includes('SOURCE_EDITION_MISSING_OR_DUPLICATE_ROLE'));
  const fake=analyzeEditionPair(lessonCode,[teacher,{...student,sha256:'0'}]);
  assert.ok(fake.blockers.includes('SOURCE_EDITION_HASH_MISSING_OR_MALFORMED'));
  const titleOnly=analyzeEditionPair(lessonCode,[teacher,{
    ...student,htmlDocumentTitle:teacher.htmlDocumentTitle,
    editionMatch:'IM2021_STUDENT_CANDIDATE_SOURCE'
  }]);
  assert.ok(titleOnly.blockers.includes('TEACHER_STUDENT_SEMANTIC_PARITY_NOT_CERTIFIED'));
});

// Regression: obsolete v360 Student_Public_Mirror snapshots are kept for audit,
// never used to re-open 148 closed 2021 second-pass Grade 5 authoring handoffs.
test('Stage D consumes 148 closed 2021 second-pass reviews and isolates all archival v360 mirrors',()=>{
  const p=spawnSync(process.execPath,['tools/curriculum/g5-stage-d-diagnostic.cjs','source-evidence'],
    {cwd:root,encoding:'utf8',timeout:30000});
  assert.ok([0,2].includes(p.status),'audit must exit successfully only with no real blockers');
  const result=JSON.parse(fs.readFileSync(path.join(root,'stage-d-results/grade5-source-evidence.json'),'utf8'));
  assert.equal(result.scopeLessons,148);
  assert.equal(result.reviewed2021SecondPassLessons,148);
  assert.equal(result.archivedV360StudentMirrors,148);
  assert.equal(result.wrongEditionStudentLessons,0,
    'historic student mirrors cannot invalidate the 2021 source-referenced lesson');
  assert.equal(result.original2021TeacherStudentFacingLessons,148);
  assert.equal(result.original2021TeacherSectionHashLinkedCount,483);
  assert.equal(result.independentlySourceApprovedLessons,
    result.rows.filter(x=>x.independentElementReviewPassed).length);
  assert.equal(result.blockedLessons,result.rows.filter(x=>x.blockers.length>0).length,
    'only real remaining blockers can fail the source gate');
  assert.equal(p.status,result.blockedLessons===0?0:2);
  for(const lesson of result.rows){
    assert.equal(lesson.reviewed2021SecondPass?.status,'REVIEW_COMPLETE_2021',lesson.lessonCode);
    assert.equal(lesson.editionEvidence.scope,'ARCHIVAL_DIAGNOSTIC_ONLY_NOT_ACTIVE_2021_STUDENT_SOURCE');
    assert.ok(lesson.editionEvidence.blockers.includes('SOURCE_EDITION_MISMATCH_V360_STUDENT'));
    assert.ok(!lesson.blockers.includes('SOURCE_EDITION_MISMATCH_V360_STUDENT'),
      'obsolete archival v360 is not an active lesson source blocker');
    if(!lesson.independentElementReviewPassed)
      assert.ok(lesson.blockers.includes('INDEPENDENT_SOURCE_ELEMENT_REVIEW_NOT_EVIDENCED'),
        'teacher review must not be forged by 2021 handoff');
  }
});
