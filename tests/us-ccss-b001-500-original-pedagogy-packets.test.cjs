'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const loc=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const catalog=loc('docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');
const part1=loc('docs/curriculum/batches/US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json');
const part2=loc('docs/curriculum/batches/US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-2-OF-2.json');
const studentSource=loc('docs/curriculum/batches/US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json');
const records=[...part1.lessons,...part2.lessons];

test('One complete original 2021 source-grounded teaching triage encompasses exactly 500 B001 IDs',()=>{
 assert.equal(catalog.lessons.length,500);
 assert.equal(part1.lessons.length,250);
 assert.equal(part2.lessons.length,250);
 assert.equal(part1.firstOrdinal,1);
 assert.equal(part1.lastOrdinal,250);
 assert.equal(part2.firstOrdinal,251);
 assert.equal(part2.lastOrdinal,500);
 assert.equal(records.length,500);
 assert.equal(new Set(records.map(x=>x.code)).size,500);
 assert.equal(studentSource.lessons.length,500);
 const counts=[0,0,0,0];
 for(let i=0;i<500;i++){
   const x=records[i],canonical=catalog.lessons[i],student=studentSource.lessons[i];
   assert.equal(x.ordinal,i+1);
   assert.equal(x.code,canonical.lessonCode);
   assert.equal(x.title,canonical.title);
   assert.equal(student.lessonCode,x.code);
   assert.equal(x.sourceEdition,'IM 2021 teacher+student-facing full HTML');
   assert.ok(x.teacherPurposeExcerpt.length>10,'Missing purpose: '+x.code);
   assert.ok(Array.isArray(x.originalActivities),'Missing activities: '+x.code);
   assert.ok(Array.isArray(x.reviewFlags));
   assert.equal(x.sourceTaskPdfReviewed,false);
   assert.equal(x.sourceToLearnerPedagogicalParityApproved,false);
   assert.equal(x.practiceFamilySourceGoalApproved,false);
   assert.equal(x.independentTeacherReviewApproved,false);
   assert.equal(x.studentAuthenticatedUatApproved,false);
   counts[Number(x.code.match(/:G([1-4]):/)[1])-1]++;
 }
 assert.deepEqual(counts,[146,146,143,65]);
});

test('Source-goal and student task collection is never mistaken for independent academic signoff',()=>{
 for(const source of [part1,part2]){
  assert.equal(source.lessonCount,250);
  assert.match(source.assurance,/No source was graded for conceptual pedagogy/i);
  assert.match(source.attribution,/CC BY 4\.0/);
 }
 const counts=f=>records.filter(x=>x.reviewFlags.includes(f)).length;
 assert.equal(counts('RAW_GENERIC_EXAMPLE'),97);
 assert.equal(counts('RAW_WORKED_EXAMPLES_SHORT'),39);
 assert.equal(counts('RAW_STEPS_SHORT'),47);
 assert.equal(counts('SOURCE_ACTIVITIES_NOT_PARSED'),2);
 assert.equal(counts('SOURCE_TEACHER_GOAL_NOT_PARSED'),1);
 assert.equal(counts('REGROUPING_EXAMPLE_CONTRADICTS_SOURCE_GOAL'),1);
 assert.equal(records.filter(x=>x.originalActivities.length>0).length,498);
 assert.equal(records.filter(x=>x.originalActivities.some(a=>a.hasStudentFacing)).length,492);
 assert.equal(records.filter(x=>x.studentFacingGoalExcerpt.length>10).length,499);
 assert.ok(records.some(x=>x.code==='PED:US-CCSS-MATH:G2:U02:L01' &&
   x.reviewFlags.includes('REGROUPING_EXAMPLE_CONTRADICTS_SOURCE_GOAL')));
});
