'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const base = path.resolve(__dirname,'..');
const html=()=>fs.readFileSync(path.join(base,'src/Edulytics.Web/wwwroot/content-sources-licenses.html'),'utf8');
const manifest=()=>JSON.parse(fs.readFileSync(path.join(base,
 'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),'utf8'));
const packs=()=>[1,2,3,4].flatMap(g=>JSON.parse(fs.readFileSync(path.join(base,
 'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+
 '-phase29-v1.lesson-content-pack.json'),'utf8')).lessons);

test('B01 attribution is transparent about source, edition, licence and adaptation',()=>{
 const h=html();
 assert.match(h,/id="ccss-b01-first-edition"/);
 assert.match(h,/Illustrative Mathematics/);
 assert.match(h,/2019–2021 first edition/);
 assert.match(h,/https:\/\/creativecommons\.org\/licenses\/by\/4\.0\//);
 assert.match(h,/Changes and independent material:/);
 assert.match(h,/not imply that Illustrative Mathematics sponsors,/);
 assert.match(h,/2024 IM v\.360 edition has a different, non-commercial licence/);
 for(let g=1;g<=4;g++)assert.ok(h.includes('https://im.kendallhunt.com/k5/teachers/grade-'+g+'/'));
 assert.doesNotMatch(h,/src="[^"]*(?:im\.kendallhunt|accessim)/i);
});
test('All 500 B01 source declarations match first edition and approved licence; declarations do not equal signoff',()=>{
 const m=manifest();
 assert.equal(m.lessons.length,500);
 const ids=new Set(m.lessons.map(l=>l.lessonCode));
 assert.equal(ids.size,500);
 const byCode=new Map(packs().map(l=>[l.lessonCode,l]));
 const grouped={};
 for(const ref of m.lessons){
  const l=byCode.get(ref.lessonCode);
  assert.ok(l,'Missing pack for '+ref.lessonCode);
  assert.match(l.sourceEdition,/1st Edition.*2021/i,ref.lessonCode);
  assert.equal(l.sourceRights,'CC BY 4.0',ref.lessonCode);
  assert.match(l.sourceUrl,/^https:\/\/im\.kendallhunt\.com\/k5\/teachers\/grade-[1-4]\//);
  assert.ok(!l.sourceUrl.includes('accessim.org'));
  const grade=ref.lessonCode.match(/:G([1-4]):/)[1];
  grouped[grade]=(grouped[grade]||0)+1;
 }
 assert.deepEqual(grouped,{'1':146,'2':146,'3':143,'4':65});
 const rights=JSON.parse(fs.readFileSync(path.join(base,'artifacts/academic-us-ccss/01-edition-specific-licence-evidence.json'),'utf8'));
 assert.notEqual(rights.reviewStatus,'COMPLETE');
 assert.notEqual(rights.academicGate,'PASS');
 assert.equal(rights.independentReviewerId,null);
 assert.equal(rights.rightsApprovalEvidenceId,null);
});
