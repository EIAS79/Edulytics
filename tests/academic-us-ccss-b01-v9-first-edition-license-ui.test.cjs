'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const parent=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml'),'utf8');
const partial=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_RichLessonFirstEditionAttribution.cshtml'),'utf8');
const style=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/css/site.css'),'utf8');
const manifest=JSON.parse(fs.readFileSync(path.join(root,'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),'utf8'));
const re=/^PED:US-CCSS-MATH:G([1-4]):U(\d{2}):L(\d{2})$/;
test('All 500 B01 first-edition IM K5 source links are exact, without needing an image-identity test',()=>{
 assert.equal(manifest.lessons.length,500);
 for(const row of manifest.lessons){
  const m=row.lessonCode.match(re);
  assert(m,row.lessonCode);
  const url='https://im.kendallhunt.com/k5/teachers/grade-'+m[1]+'/unit-'+Number(m[2])+'/lesson-'+Number(m[3])+'/preparation.html';
  assert.equal(url,row.sourceUrl,row.lessonCode);
 }
 assert(!re.test('PED:CAMBRIDGE-MATH:G1:U01:L01'));
 assert(!re.test('PED:US-CCSS-MATH:G5:U01:L01'));
 assert(!re.test('PED:US-CCSS-MATH:G2:U01:L03<script>'));
});
test('Only original US CCSS B01 Rich V2 lessons show visible author, exact teacher-source and adaptation attribution',()=>{
 assert(parent.includes('Model.LessonCode.StartsWith("PED:US-CCSS-MATH:", StringComparison.Ordinal)'));
 assert.equal(parent.split('name="_RichLessonFirstEditionAttribution"').length,2);
 assert(partial.includes('Regex.Match('));
 assert(partial.includes('sourceCode.Success'));
 assert(partial.includes('int.Parse('));
 assert(partial.includes('https://im.kendallhunt.com/k5/teachers/grade-'));
 assert(partial.includes('https://illustrativemathematics.org/'));
 assert(partial.includes('https://creativecommons.org/licenses/by/4.0/'));
 assert(partial.includes('CC BY 4.0'));
 assert(partial.includes('2019–2021'));
 assert(partial.includes('Illustrative Mathematics®'));
 assert(partial.includes('does not imply endorsement or affiliation'));
 assert(partial.includes('third-party images may have their own licenses'));
 assert(partial.includes('rel="noopener noreferrer external"'));
 assert(!partial.includes('accessim.org'));
 assert(!partial.includes('CC BY-NC 4.0'));
 assert(!partial.includes('<img'));
 assert(!partial.includes('<svg'));
});
test('Identical-looking images allowed when mathematically equivalent and credited; different drawings allowed if data agrees',()=>{
 assert(style.includes('.rich-lesson-source-attribution'));
 const example={sourceMath:{quantities:[2,3],relation:'2+3=5',units:'counters',parts:5},
   candidateMath:{quantities:[2,3],relation:'2+3=5',units:'counters',parts:5},
   originalGraphicSha:'a'.repeat(64),candidateGraphicSha:'b'.repeat(64),
   curriculumEdition:'IM_K5_FIRST_EDITION_2021',license:'CC BY 4.0'};
 function mathEqual(x){return JSON.stringify(x.sourceMath)===JSON.stringify(x.candidateMath);}
 assert(mathEqual(example));
 assert.notEqual(example.originalGraphicSha,example.candidateGraphicSha);
 assert(mathEqual({...example,candidateGraphicSha:example.originalGraphicSha}));
 assert(!mathEqual({...example,candidateMath:{...example.sourceMath,parts:4}}));
 assert(!mathEqual({...example,candidateMath:{...example.sourceMath,units:'dollars'}}));
 assert(!mathEqual({...example,candidateMath:{...example.sourceMath,relation:'2+3=4'}}));
});