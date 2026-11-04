'use strict';

// Genuine Grade 7 *static fixture* test. Run with:
// node --test tests/academic-us-ccss-g7-v9-u02-u07-geometry.test.cjs
// Passing cannot certify an authenticated lesson screen or teacher scoring.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const base = 'src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/';
const evidence = JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-v9-u02-u07-source-geometry-and-rubrics.json'), 'utf8'));
const partial = fs.readFileSync(path.join(root,
  'src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml'), 'utf8');
const text = filename => fs.readFileSync(path.join(root,base,filename),'utf8');
const expected = [
  'PED:US-CCSS-MATH:G7:U02:L01',
  'PED:US-CCSS-MATH:G7:U07:L01',
  'PED:US-CCSS-MATH:G7:U07:L02'
];
const angle = (x1,y1,x2,y2) => Math.atan2(y1-y2,x2-x1)*180/Math.PI;
const close = (actual,want,label,tolerance=0.05) =>
  assert.ok(Math.abs(actual-want)<tolerance, label+': '+actual+' should be '+want);

test('V9 contains three accessible original SVGs with distinct teacher-only rubric drafts',()=>{
  assert.equal(evidence.lessons.length,3);
  assert.deepEqual(evidence.lessons.map(l=>l.lessonCode),expected);
  assert.equal(evidence.release,'RELEASE_BLOCKED');
  assert.equal(evidence.evidence.authenticatedStudentRender,'NOT_RUN');
  assert.equal(evidence.evidence.teacherScoringPersistence,'NOT_RUN');
  for(const gate of ['stageA','stageB','stageC','stageD'])
    assert.equal(evidence.evidence[gate],'NOT_VERIFIED');
  for(const row of evidence.lessons){
    assert.equal(row.sourceCompared,true);
    assert.equal(row.academicAcceptance,'NOT_VERIFIED');
    assert.equal(row.studentAssetRenderedInRichV2,false);
    assert.equal(row.teacherRubricIntegratedInUI,false);
    assert.equal(row.studentArtifactPersisted,false);
    assert.equal(row.verifiedInTwoSchools,false);
    assert.equal(row.assessmentMode,'TEACHER_REVIEW_REQUIRED_NOT_AUTOGRADED');
    assert.equal(row.teacherRubric.criteria.length,4);
    assert.equal(row.teacherRubric.maxScore,12);
    assert.deepEqual(new Set(row.cases.map(c=>c.kind)).has('correct'),true);
    assert.ok(row.cases.some(c=>c.kind==='incorrect'));
    assert.ok(row.cases.some(c=>c.kind==='incomplete'));
    assert.match(row.sourceStudentUrl,/^https:\/\/im\.kendallhunt\.com\/MS\/students\/2\//);
    assert.equal(row.publicAssetPath,
      '/curriculum/us-ccss-g7/geometry/'+path.basename(row.staticFirstPartyAsset));
    const svg = text(path.basename(row.staticFirstPartyAsset));
    assert.match(svg,/<svg[^>]+role="img"/);
    assert.match(svg,/aria-labelledby="[^"]+"/);
    assert.match(svg,/<title id="[^"]+">/);
    assert.match(svg,/<desc id="[^"]+">/);
    assert.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
    assert.ok(row.accessibleDescription.length>40);
    assert.ok(partial.includes(row.lessonCode));
    assert.ok(partial.includes(row.publicAssetPath));
  }
});

test('U02 crescent B is uniform factor 1.5 while C is only horizontally stretched',()=>{
  const svg=text('u02-l01-scaled-crescents.svg');
  const groups = [...svg.matchAll(/<g id="([^"]+)" transform="translate\([\d.]+ [\d.]+\) scale\(([\d.]+) ([\d.]+)\)">\s*<path class="[^"]+" d="([^"]+)"/g)];
  assert.equal(groups.length,3);
  assert.deepEqual(groups.map(g=>g[1]),['original-crescent','uniform-crescent','stretched-crescent']);
  assert.deepEqual(groups.map(g=>[+g[2],+g[3]]),[[1,1],[1.5,1.5],[1.5,1]]);
  assert.equal(new Set(groups.map(g=>g[4])).size,1,'Same curve coordinates must be scaled rather than redrawn');
  close(+groups[1][2]/+groups[0][2],1.5,'Copied x length');
  close(+groups[1][3]/+groups[0][3],1.5,'Copied y length');
  assert.notEqual(+groups[2][2],+groups[2][3],'Distorted crescent is not uniform');
});

test('U07 hexagon vertices make regular 120° interior corners, 360° full turn and 40° protractor ray',()=>{
  const svg=text('u07-l01-hexagon-angles-and-protractor.svg');
  const groups = [...svg.matchAll(/<g id="hexagon-(\d+)" transform="rotate\((\d+)\)"><polygon class="[^"]+" points="([^"]+)"/g)];
  assert.equal(groups.length,3);
  assert.deepEqual(groups.map(g=>+g[2]),[0,120,240]);
  assert.equal(new Set(groups.map(g=>g[3])).size,1);
  const vertices=groups[0][3].split(/\s+/).map(p=>p.split(',').map(Number));
  assert.equal(vertices.length,6);
  const [o,a,,,,f]=vertices;
  const va=[a[0]-o[0],a[1]-o[1]],vb=[f[0]-o[0],f[1]-o[1]];
  const dot=va[0]*vb[0]+va[1]*vb[1];
  const norm=Math.hypot(...va)*Math.hypot(...vb);
  const interior=Math.acos(dot/norm)*180/Math.PI;
  close(interior,120,'Hexagon interior corner');
  close(3*interior,360,'Three hexagons full-turn',0.06);
  assert.equal(360-3*108,36,'Regular pentagon leaves 36 degree gap');
  const ray=svg.match(/<line id="protractor-angle-40-ray" class="ray" x1="([\d.]+)" y1="([\d.]+)" x2="([\d.]+)" y2="([\d.]+)" data-angle-deg="40"/);
  assert.ok(ray,'Protractor ray must be independently testable');
  close(angle(...ray.slice(1,5).map(Number)),40,'Ray measured from right-hand zero');
});

test('U07 adjacent ray endpoints enforce 35+55=90 and 125+55=180',()=>{
  const svg=text('u07-l02-adjacent-right-and-straight-angles.svg');
  for(const [id,want] of [['right-angle-35-ray',35],['straight-angle-125-ray',125]]){
    const re=new RegExp('<line id="'+id+'" class="divider" x1="([\\d.]+)" y1="([\\d.]+)" x2="([\\d.]+)" y2="([\\d.]+)" data-angle-deg="'+want+'"');
    const match=svg.match(re);
    assert.ok(match,'Expected divider ray '+id);
    close(angle(...match.slice(1,5).map(Number)),want,id);
  }
  assert.equal(35+55,90);
  assert.equal(125+55,180);
  assert.equal(70+110,180,'Publisher analogous straight-angle measurements');
  assert.equal(53+37,90,'Publisher analogous right-angle measurements');
});

test('New Grade 7-only presentation partial cannot self-approve or mutate Practice',()=>{
  assert.match(partial,/@model Edulytics\.Core\.Curriculum\.RichLessonContentV2Lesson/);
  assert.match(partial,/@if \(figure is not null\)/);
  assert.match(partial,/alt="@description"/);
  assert.match(partial,/asp-append-version="true"/);
  assert.match(partial,/teacher must inspect|Teacher-only assessment/i);
  assert.doesNotMatch(partial,/Grade[345689]|G[345689]:/);
  assert.doesNotMatch(partial,/PracticeService|SaveChanges|HttpClient|fetch\(/);
});

test('Existing 145-lesson academic stages must remain fail-closed',()=>{
  const ledger=JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authoring-ledger.json'),'utf8'));
  assert.equal(ledger.counts.catalogue,145);
  assert.equal(ledger.counts.stageAVerified,0);
  assert.equal(ledger.counts.stageBEffectiveRuntimeVerified,0);
  assert.equal(ledger.counts.stageCVerified,0);
  assert.equal(ledger.counts.stageDVerified,0);
  const byCode=new Map(ledger.lessons.map(l=>[l.lessonCode,l]));
  for(const code of expected){
    assert.ok(byCode.has(code));
    assert.notEqual(byCode.get(code).humanAcademicAcceptance,'APPROVED');
    assert.equal(byCode.get(code).stages.C,'NOT_VERIFIED');
    assert.equal(byCode.get(code).stages.D,'NOT_RUN');
  }
});
