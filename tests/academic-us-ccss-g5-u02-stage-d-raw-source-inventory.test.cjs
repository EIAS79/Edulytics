'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const path=require('node:path');
const fs=require('node:fs');

test('all 17 U02 original 2021 local teacher HTML lesson/source section identities are inventoried but NOT source-parity approved',()=>{
  const root=path.resolve(__dirname,'..','artifacts','curriculum-delivery','US-CCSS-MATH','G5');
  const parts=['l01-l05','l06-l10','l11-l15','l16-l17'];
  const rows=parts.flatMap(part=>{
    const obj=JSON.parse(fs.readFileSync(path.join(root,'source-u02-html-headings-'+part+'.json'),'utf8'));
    assert.equal(obj.unit,'U02');
    assert.match(obj.kind,/NOT_1_TO_1_CONTENT_APPROVAL/);
    return obj.lessons;
  });
  assert.equal(rows.length,17);
  const unique=new Set();
  let sectionCount=0,imageTags=0;
  for(let i=0;i<rows.length;i++){
    const row=rows[i],n=i+1;
    assert.equal(row.lessonCode,'PED:US-CCSS-MATH:G5:U02:L'+String(n).padStart(2,'0'));
    assert.match(row.originalTeacherLesson,new RegExp('unit-2\\\\lesson-'+n+'\\\\lesson\\.html$'));
    assert.ok(row.sections.length>=10,'incomplete teacher HTML section extraction');
    assert.ok(Array.isArray(row.images));
    for(const section of row.sections){
      assert.ok(!unique.has(section.sourceElementId),'duplicate source section id');
      unique.add(section.sourceElementId);
      assert.equal(section.status,'SOURCE_LOCATED_IMPLEMENTATION_UNVERIFIED');
      assert.match(section.locator,/heading-ordinal-\d+$/);
      assert.ok(section.title.length>0);
    }
    assert.equal(row.academicApproval,'NOT_GRANTED');
    assert.equal(row.sourceAssetRights,'NOT_REVIEWED');
    assert.equal(row.studentPdfCoverage,'UNKNOWN');
    assert.equal(row.matchedEdulyticsItems,null);
    sectionCount+=row.sections.length;
    imageTags+=row.images.length;
  }
  assert.equal(sectionCount,499);
  assert.equal(imageTags,209);
});
