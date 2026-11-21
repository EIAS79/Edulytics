'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
test('Grade9 canonical and 82 fully structured original Arabic RichV2 bodies embed while source parity stays unknown',()=>{
 const csproj=fs.readFileSync(path.join(root,'src/Edulytics.Core/Edulytics.Core.csproj'),'utf8');
 assert.ok(!csproj.includes('Exclude="Curriculum/LessonContent/Packs/uae-g9-general-*.lesson-content-pack.json"'));
 assert.ok(!csproj.includes('Exclude="Curriculum/LessonContent/RichV2/uae-g9-general-*.rich-lesson-v2.json"'));
 assert.ok(csproj.includes('Include="Curriculum/LessonContent/RichV2/*.rich-lesson-v2.json"'));
 assert.ok(csproj.includes('Include="Curriculum/LessonContent/Packs/*.lesson-content-pack.json"'));
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 assert.equal(pack.Status,'Published');
 assert.equal(pack.Lessons.length,82);
 const rootRich=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
 assert.equal(fs.readdirSync(rootRich).filter(n=>/^uae-g9-general-.*\.rich-lesson-v2\.json$/.test(n)).length,82);
 const index=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-82-target-index.json');
 assert.equal(index.SOURCE_INDEX_COMPLETE,false);
 const richFiles=fs.readdirSync(rootRich).filter(n=>/^uae-g9-general-.*\.rich-lesson-v2\.json$/.test(n));
 assert.equal(richFiles.length,82);
 for(const file of richFiles){
  const doc=JSON.parse(fs.readFileSync(path.join(rootRich,file),'utf8'));
  assert.equal(doc.SchemaVersion,1,file);
  assert.equal(doc.PackCode,'UAE-MOE-MATH',file);
  assert.equal(doc.Lessons.length,1,file);
  const l=doc.Lessons[0];
  assert.equal(l.CultureCode,'ar',file);
  assert.ok(l.LessonCode.startsWith('PED:UAE-MOE-MATH:L9:GENERAL:'),file);
  for(const [k,n] of Object.entries({ExplanationParagraphs:2,KeyConcepts:3,WorkedExamples:3,CommonMistakes:2,SummaryPoints:3})){
   assert.ok(l[k].length>=n,file+':'+k);
  }
  for(const x of l.WorkedExamples) {
   assert.ok(x.Method && x.Steps.length>=3,file+':worked-example incomplete');
  }
 }
});
test('RichV2 repaired schema 1 is accepted while incompatible schema 2 is still rejected by runtime',()=>{
 const rootRich='src/Edulytics.Core/Curriculum/LessonContent/RichV2/';
 const p=read(rootRich+'uae-g9-general-u01-l01-ar.rich-lesson-v2.json');
 assert.equal(p.SchemaVersion,1);
 assert.equal(p.PackCode,'UAE-MOE-MATH');
 const visual=fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/RichLessonContentV2.cs'),'utf8');
 assert.ok(visual.includes('if (document.SchemaVersion != 1)'));
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml'),'utf8');
 for(const kind of ['VennDiagram','CoordinatePlane']){
  assert.ok(visual.includes(kind+' = '));
  assert.ok(html.includes('RichLessonVisualKind.'+kind));
 }
 assert.ok(html.includes('RichLessonCoordinateDiagram.ParseLines'));
 assert.ok(html.includes('role="img"'));
 const audit=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-82-production-import-candidate-NO_GO.json');
 assert.equal(audit.allowProductionWrites,false);
 assert.equal(audit.productionImportExecuted,false);
});
test('Grade9 draft exact Practice families are excluded from publicly READY_VERIFIED runtime registry until academic release',()=>{
 const file=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(file.includes('string.Equals(x.Readiness, "READY_VERIFIED"'));
 assert.ok(file.includes('!x.LessonCode.StartsWith('));
 assert.ok(file.includes('"PED:UAE-MOE-MATH:L9:GENERAL:", StringComparison.Ordinal'));
 assert.ok(file.includes('return byLesson.Values'));
 assert.ok(file.includes('ByLessonCode.TryGetValue(lessonCode.Trim(), out contract)'));
});
