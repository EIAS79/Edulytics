'use strict';
// Stage D: do not hide duplicate Rich V2 lesson/culture registrations by
// preferring one arbitrary embedded resource. Runtime index is strictly unique.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const dir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
test('all embedded rich V2 lessons have unique lessonCode:cultureCode',()=>{
 const files=fs.readdirSync(dir).filter(n=>n.endsWith('.rich-lesson-v2.json')).sort();
 assert.ok(files.length>300,'unexpectedly incomplete Rich V2 source inventory');
 const seen=new Map(),duplicates=[];
 for(const name of files){
  const doc=JSON.parse(fs.readFileSync(path.join(dir,name),'utf8'));
  assert.ok(Array.isArray(doc.Lessons)&&doc.Lessons.length>0,name);
  for(const lesson of doc.Lessons){
   assert.ok(lesson.LessonCode&&lesson.CultureCode,name);
   const key=lesson.LessonCode+':'+lesson.CultureCode.toLowerCase();
   if(seen.has(key))duplicates.push({key,first:seen.get(key),second:name});
   else seen.set(key,name);
  }
 }
 assert.deepEqual(duplicates,[],'Runtime would throw Duplicate Rich Lesson Content V2 entry');
 assert.equal(seen.get('PED:US-CCSS-MATH:G5:U02:L01:en'),
   'us-ccss-g5-u02-l01-2021.rich-lesson-v2.json',
   'preserve the richer canonical dedicated original-source G5 U02:L01');
 assert.equal(seen.get('PED:US-CCSS-MATH:G5:U03:L01:en'),
   'us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json');
 assert.equal(seen.get('PED:US-CCSS-MATH:G5:U07:L01:en'),
   'us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json');
});

// Grade 5 first-edition licensing: educational mathematics can be independently
// re-illustrated, but never silently import third-party artwork into learner UI.
test('Grade5 first-party visual inventory and 2021 CC BY source attribution are intact',()=>{
 const files=fs.readdirSync(dir).filter(n=>/^us-ccss-g5-.*\.rich-lesson-v2\.json$/.test(n));
 const codes=new Set();
 let figures=0, videos=0;
 for(const name of files){
  const doc=JSON.parse(fs.readFileSync(path.join(dir,name),'utf8'));
  for(const lesson of doc.Lessons||[]){
   assert.match(lesson.LessonCode,/^PED:US-CCSS-MATH:G5:U\d{2}:L\d{2}$/);
   assert.equal(lesson.CultureCode,'en');
   assert.ok(!codes.has(lesson.LessonCode),'Duplicate Grade 5 lesson '+lesson.LessonCode);
   codes.add(lesson.LessonCode);
   assert.ok(Array.isArray(lesson.Visuals),name+' missing visuals array');
   assert.ok(Array.isArray(lesson.Videos),name+' missing videos array');
   figures+=lesson.Visuals.length;
   videos+=lesson.Videos.length;
   for(const visual of lesson.Visuals){
    const raw=JSON.stringify(visual);
    assert.doesNotMatch(raw,/https?:\/\/|data:image\/|<img\b|<image\b|cms-assets\./i,
      lesson.LessonCode+' visual must not embed or link to external publisher artwork');
   }
  }
 }
 assert.equal(codes.size,148,'Grade 5 first-edition lesson count');
 assert.equal(figures,284,'Grade5 first-party visual inventory, including U01:L01 and U01:L02 SVG models');
 assert.equal(videos,0,'no unreviewed external Grade5 video embeds');
 const grade5Intro=JSON.parse(fs.readFileSync(path.join(dir,
  'us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
 const first=grade5Intro.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L01');
 assert.equal(first.Visuals.filter(x=>x.Kind==='Grade5Unit1CubeComparison').length,1,
  'one independently drawn original 2021 WODB four-model illustration');
 const second=grade5Intro.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L02');
 assert.equal(second.Visuals.filter(x=>x.Kind==='Grade5Unit1CubeModelWodb').length,1,
  'one independent 2021 lesson 2 four-model cube comparison SVG');

 const metadata=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json'),'utf8'));
 assert.equal(metadata.lessons.length,148);
 assert.match(metadata.pedagogicalSourceRightsNote,/CC BY 4\.0.*attribution/i);
 assert.match(metadata.pedagogicalSourceRightsNote,/independently authored/i);
 assert.equal(metadata.pedagogicalSourceEdition,'1st Edition (2021)');
 for(const row of metadata.lessons){
  assert.ok(codes.has(row.lessonCode),'Original source metadata missing a tested lesson '+row.lessonCode);
  assert.equal(row.sourceRights,'CC BY 4.0');
  assert.match(row.sourcePublisher,/Illustrative Mathematics/);
  assert.match(row.sourceUrl,/^https:\/\/im\.kendallhunt\.com\//);
 }
});


test('Grade5 original 2021 teacher source titles exclude non-title PLC badges',()=>{
 const blueprint=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json'),'utf8'));
 const pack=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json'),'utf8'));
 const canonical={
  "PED:US-CCSS-MATH:G5:U01:L04": "Use Layers to Determine Volume",
  "PED:US-CCSS-MATH:G5:U01:L05": "Side Lengths of Rectangular Prisms",
  "PED:US-CCSS-MATH:G5:U01:L10": "Represent Volume with Expressions",
  "PED:US-CCSS-MATH:G5:U02:L03": "Interpret Equations",
  "PED:US-CCSS-MATH:G5:U02:L07": "Divide to Multiply Unit Fractions",
  "PED:US-CCSS-MATH:G5:U02:L11": "Fractional Side Lengths Greater Than 1",
  "PED:US-CCSS-MATH:G5:U03:L03": "Multiply Unit Fractions",
  "PED:US-CCSS-MATH:G5:U03:L12": "Represent Division of Unit Fractions by Whole Numbers",
  "PED:US-CCSS-MATH:G5:U03:L17": "Fraction Multiplication and Division Situations",
  "PED:US-CCSS-MATH:G5:U04:L06": "Standard Algorithm: Multi-digit Numbers with Composing",
  "PED:US-CCSS-MATH:G5:U04:L11": "Different Partial Quotients",
  "PED:US-CCSS-MATH:G5:U04:L19": "Trash Talk",
  "PED:US-CCSS-MATH:G5:U05:L07": "Round Doubloons",
  "PED:US-CCSS-MATH:G5:U05:L11": "Make Sense of Decimal Addition",
  "PED:US-CCSS-MATH:G5:U05:L20": "Products in the Hundredths Place",
  "PED:US-CCSS-MATH:G5:U05:L22": "Divide Whole Numbers by 0.1 and 0.01",
  "PED:US-CCSS-MATH:G5:U06:L02": "Powers of 10",
  "PED:US-CCSS-MATH:G5:U06:L08": "Add and Subtract Fractions",
  "PED:US-CCSS-MATH:G5:U06:L19": "Compare to 1",
  "PED:US-CCSS-MATH:G5:U07:L02": "Points on the Coordinate Grid",
  "PED:US-CCSS-MATH:G5:U07:L05": "Trapezoids",
  "PED:US-CCSS-MATH:G5:U07:L11": "Patterns and Ordered Pairs",
  "PED:US-CCSS-MATH:G5:U08:L01": "Find the Largest Product",
  "PED:US-CCSS-MATH:G5:U08:L06": "Revisit Volume",
  "PED:US-CCSS-MATH:G5:U08:L13": "Multiply Fractions Game Day",
  "PED:US-CCSS-MATH:G5:U08:L16": "Number Talk"
};
 assert.equal(Object.keys(canonical).length,26);
 assert.equal(blueprint.Lessons.length,148);
 // 26 known mismatches remain pinned in the consented original pack;
 // fail-closed academic source parity remains an independent gate.
 assert.equal(Object.keys(canonical).filter(code=>
  blueprint.Lessons.find(x=>x.LessonCode===code)?.Title!==canonical[code]).length,26);
 for(const entry of blueprint.Lessons){
  const locale=pack.lessons.find(x=>x.lessonCode===entry.LessonCode)?.translations.find(x=>x.cultureCode==='en');
  assert.ok(locale,entry.LessonCode);
  assert.equal(locale.title,entry.Title,'Live Grade5 title drift '+entry.LessonCode);
  // Immutable owner-consented blueprint/pack intentionally retain 26 legacy
  // title badges. Record this unsolved source mismatch, never certify parity.
  if(canonical[entry.LessonCode])
   assert.equal(entry.Title,canonical[entry.LessonCode]+' PLC Activity',
    'Pinned legacy title changed without renewed content authorization');
 }
 const sourceTitleOverrides={
  'PED:US-CCSS-MATH:G5:U07:L02':'Points on the Coordinate Grid',
  'PED:US-CCSS-MATH:G5:U07:L03':'Plot More Points',
  'PED:US-CCSS-MATH:G5:U07:L09':'Generate Patterns',
  'PED:US-CCSS-MATH:G5:U07:L10':'Interpret Relationships',
  'PED:US-CCSS-MATH:G5:U07:L11':'Patterns and Ordered Pairs',
  'PED:US-CCSS-MATH:G5:U07:L12':'Represent Problems on the Coordinate Grid',
  'PED:US-CCSS-MATH:G5:U07:L13':'Perimeter and Area of Rectangles'
 };
 const canonicalTitle=new Map(blueprint.Lessons.map(x=>[
   x.LessonCode,canonical[x.LessonCode]||sourceTitleOverrides[x.LessonCode]||x.Title
 ]));
 const richFiles=fs.readdirSync(dir).filter(n=>/^us-ccss-g5-.*\.rich-lesson-v2\.json$/.test(n));
 for(const file of richFiles){
  const doc=JSON.parse(fs.readFileSync(path.join(dir,file),'utf8'));
  for(const lesson of doc.Lessons||[]){
   assert.ok(!/ PLC Activity$/.test(lesson.Title),'Rich V2 publisher source title drift '+lesson.LessonCode);
   assert.equal(lesson.Title,canonicalTitle.get(lesson.LessonCode),'Original first-edition student Rich V2 title misaligned: '+lesson.LessonCode);
  }
 }
});

// Existing parity audit tests above cover all 148 lesson metadata and all 148 Rich V2 titles.
test('student title preserves immutable owner-consented data but renders 2021 source title for Grade5 only',()=>{
 const view=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml'),'utf8');
 assert.match(view,/Model\.LessonCode\.StartsWith\("PED:US-CCSS-MATH:G5:"/);
 assert.match(view,/Model\.Title\.EndsWith\(" PLC Activity"/);
 assert.match(view,/ViewData\["Title"\]\s*=\s*grade5OriginalDisplayTitle/);
 assert.match(view,/<h1>@grade5OriginalDisplayTitle<\/h1>/);
 assert.match(view,/Model\.LessonCode,\s*grade5OriginalDisplayTitle,\s*Model\.GradeName/,
  'Lesson media header also uses the canonical first-edition title');
 const official=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json'),'utf8'));
 const pack=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json'),'utf8'));
 let displayCorrected=0;
 for(const entry of official.Lessons){
  const legacy=pack.lessons.find(row=>row.lessonCode===entry.LessonCode).translations.find(t=>t.cultureCode==='en').title;
  const displayed=entry.LessonCode.startsWith('PED:US-CCSS-MATH:G5:')&&
     legacy.endsWith(' PLC Activity') ? legacy.slice(0,-' PLC Activity'.length):legacy;
  if(displayed!==legacy)displayCorrected++;
  assert.ok(displayed.length>0);
 }
 assert.equal(displayCorrected,26,'26 previously source-mismatched Grade5 displays');
});


// Stage D diagnostic: these are 215 distinct lesson/outcome projections, not
// 232 resolved alignment records (a lesson can repeat an outcome via roles).
// Keep the owner's exact approved *content pack* blob untouched: this checks
// the separately maintained blueprint evidence, not asset licenses or parity.
test('Grade5 canonical lesson-outcome projection counts match source-lock diagnostics',()=>{
 const blueprint=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json'),'utf8'));
 assert.equal(blueprint.Lessons.length,148);
 assert.equal(blueprint.Units.length,8);
 const pairs=new Set();
 const covered=new Set();
 for(const lesson of blueprint.Lessons){
  const fromAlignments=new Set((lesson.Alignments||[])
   .filter(x=>typeof x.OutcomeCode==='string'&&x.OutcomeCode.length)
   .map(x=>x.OutcomeCode));
  const outcomes=new Set(lesson.OutcomeCodes||[]);
  assert.deepEqual([...outcomes].sort(),[...fromAlignments].sort(),
   'Resolved outcomes must agree with lesson projection: '+lesson.LessonCode);
  for(const outcome of outcomes)pairs.add(lesson.LessonCode+'|'+outcome);
  for(const mapping of lesson.Alignments||[]){
   if(mapping.Role==='Addressing'&&mapping.ReferenceKind==='NumberedStandard')
    covered.add(mapping.ReferenceCode);
  }
 }
 assert.equal(pairs.size,215,'148 lessons must retain 215 unique formal lesson/outcome mappings');
 assert.equal(covered.size,27,'27 uniquely addressed numbered Grade 5 standards');
 assert.equal(blueprint.AcquisitionDiagnostics.FormalMappingCount,pairs.size);
 assert.equal(blueprint.AcquisitionDiagnostics.OfficialStandardCount,covered.size);
 assert.equal(blueprint.AcquisitionDiagnostics.AddressingCoverageCount,covered.size);
 assert.match(blueprint.SourceSelectionEvidence,
  /215 formal mappings, 27\/27 accepted Grade 5 Standards/);
});
