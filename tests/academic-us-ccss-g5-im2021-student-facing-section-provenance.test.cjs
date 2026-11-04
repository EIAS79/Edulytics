'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {grade5EditionManifests,original2021StudentFacingLocator,
  validateOriginal2021StudentFacingUnit,original2021StudentFacingSectionProvenance}=
  require('../tools/curriculum/g5-stage-d-diagnostic.cjs');
const manifest=JSON.parse(fs.readFileSync(path.join(__dirname,
  '../artifacts/curriculum-delivery/US-CCSS-MATH/G5/im2021-student-facing-section-provenance-u01.json'),'utf8'));

test('all 148 original first-edition teacher files provide 483 indexed independently hashed Student Facing source sections',()=>{
 const sources=original2021StudentFacingSectionProvenance(
   grade5EditionManifests(),original2021StudentFacingLocator());
 assert.equal(sources.size,148);
 const sections=[...sources.values()].flatMap(x=>x.studentFacingSections);
 assert.equal(sections.length,483);
 assert.ok(sections.every(s=>/^[a-f0-9]{64}$/.test(s.sourceSectionSha256)));
 assert.ok(sections.every(s=>Array.isArray(s.embeddedFigureRecordIds)));
 assert.deepEqual(sources.get('PED:US-CCSS-MATH:G5:U01:L01')
   .studentFacingSections.map(s=>s.embeddedFigureRecordIds),[
     ['147995','147996','147997','147998'],
     ['147995','147996','147635','147636'],
     ['36946']]);
 console.log('GRADE5_2021_SECTION_HASHES_LINKED 148 lessons / 483 original teacher source sections; no publisher artwork or parity certification');
});
test('original teacher hash, section count, source element IDs and order cannot be forged',()=>{
 const editions=grade5EditionManifests(),legacy=original2021StudentFacingLocator();
 assert.equal(validateOriginal2021StudentFacingUnit(manifest,1,editions,legacy).size,12);
 const corrupt=(fn)=>{
   const fake=structuredClone(manifest);fn(fake);
   assert.throws(()=>validateOriginal2021StudentFacingUnit(fake,1,editions,legacy),
     /G5_2021_PROVENANCE/);
 };
 corrupt(m=>m.lessons[0].sourceHtmlSha256='0'.repeat(64));
 corrupt(m=>m.lessons[0].studentFacingSections[0].sourceSectionSha256='NOT_A_HASH');
 corrupt(m=>m.lessons[0].studentFacingSections[0].ordinal=3);
 corrupt(m=>m.lessons[0].studentFacingSections[1].sourceHtmlStartOffset=0);
 corrupt(m=>m.lessons[0].lessonCode='PED:US-CCSS-MATH:G6:U01:L01');
 corrupt(m=>m.studentFacingSectionTotal+=1);
 corrupt(m=>m.fullSourceItemParityVerified=true);
});
