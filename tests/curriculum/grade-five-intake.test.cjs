'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {validate}=require('../../tools/curriculum/grade-five-intake.cjs');
test('Grade 1 source, canonical body, teacher and student indexes reconcile to fixed 146',()=>{
 const result=validate();
 assert.equal(result.sourceCount,146);
 assert.equal(result.dbObservedCount,146);
 assert.equal(result.checkedBatch,'US-CCSS-G1-003');
 assert.equal(result.lessonCodes.length,5);
 assert.equal(result.status,'REVIEW_STARTED_NOT_ACCEPTED');
});
