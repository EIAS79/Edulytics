'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {analyzeElementInventory}=require('../tools/curriculum/g5-stage-d-diagnostic.cjs');

const root=path.resolve(__dirname,'..');
const gradePath='artifacts/curriculum-delivery/US-CCSS-MATH/G5';
const code=n=>'PED:US-CCSS-MATH:G5:U01:L'+String(n).padStart(2,'0');
const fixture=n=>JSON.parse(fs.readFileSync(path.join(root,gradePath,
  'u01-l'+String(n).padStart(2,'0')+'-abc-element-ledger-20261030.json'),'utf8'));

test('all existing U01 L04-L07 source-element array schemas are itemized',()=>{
  const counts=new Map([[4,20],[5,24],[6,19],[7,15]]);
  for(const [lesson,count] of counts){
    const source=fixture(lesson);
    const audit=analyzeElementInventory(source,code(lesson));
    assert.equal(audit.sourceElements.length,count,'lesson '+lesson);
    assert.equal(audit.identified,count);
    assert.equal(audit.matched,count);
    assert.deepEqual(audit.blockers,[],'documented element structure '+lesson);
    // These remain academic/source blockers: itemization never authorizes 100% parity.
    assert.equal(source.status,'A_C_EXPLICIT_GAP');
    assert.equal(source.academicAccepted,false);
    assert.equal(source.sourceCoverage100Percent,false);
  }
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
