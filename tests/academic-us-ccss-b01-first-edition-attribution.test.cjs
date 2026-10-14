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

test('The public programme source page does not disclose internal IM academic-reference sources',()=>{
 const h=html();
 assert.match(h,/<h1>Content sources &amp; licences<\/h1>/);
 assert.match(h,/Cambridge International Mathematics/);
 assert.doesNotMatch(h,/Illustrative Mathematics/i);
 assert.doesNotMatch(h,/im\.kendallhunt\.com/i);
 assert.doesNotMatch(h,/CCSS B01|2024 IM v\.360|ccss-b01-first-edition/i);
});
test('The student lesson has source-specific licensed CC BY attribution only through the B01 Rich V2 renderer',()=>{
 const view=fs.readFileSync(path.join(base,'src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml'),'utf8');
 const rich=fs.readFileSync(path.join(base,'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml'),'utf8');
 const credit=fs.readFileSync(path.join(base,'src/Edulytics.Web/Views/Shared/_RichLessonFirstEditionAttribution.cshtml'),'utf8');
 assert.doesNotMatch(view,/Illustrative Mathematics|im\.kendallhunt\.com|lesson-source-attribution|Source inspiration/i);
 assert.match(view,/partial name="_RichLessonContentV2"/);
 assert.match(rich,/Model\.LessonCode\.StartsWith\("PED:US-CCSS-MATH:"/);
 assert.match(rich,/partial name="_RichLessonFirstEditionAttribution"/);
 assert.match(credit,/https:\/\/creativecommons\.org\/licenses\/by\/4\.0\//);
 assert.match(credit,/2019–2021/);
 assert.match(credit,/Separately attributed third-party images/);
});
test('General public curriculum register keeps official CCSS authority, not IM textbook references',()=>{
 const reg=fs.readFileSync(path.join(base,'src/Edulytics.Core/Curriculum/MathematicsCurriculumPackRegistry.cs'),'utf8');
 const start=reg.indexOf('            CommonCoreCode,');
 const end=reg.indexOf('\n        new(\n',start+50);
 assert.ok(start>0&&end>start,'Could not isolate Common Core pack configuration');
 const us=reg.slice(start,end);
 assert.match(us,/Common Core State Standards Initiative/);
 assert.doesNotMatch(us,/Illustrative Mathematics|im\.kendallhunt\.com/i);
});
test('500 internal edition/provenance records stay intact and do not imply rights signoff',()=>{
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
