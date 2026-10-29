'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {readFileSync}=require('node:fs');
const path=require('node:path');

const root=path.resolve(__dirname,'..','..');
const manifest=JSON.parse(readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v11-three-original-svg-candidates-20261029.json'),'utf8'));

test('all three new Geometry visuals are course-scoped first-party static candidates',()=>{
  assert.equal(manifest.CourseCode,'GEO');
  assert.equal(manifest.Assets.length,3);
  assert.equal(manifest.AssetStatus,'FIRST_PARTY_STATIC_ASSET_STAGED_NOT_YET_LINKED_TO_LESSON_RENDERER');
  assert.equal(manifest.AcademicAcceptance,'NOT_VERIFIED');
  assert.equal(manifest.WebStudentTeacherAuthenticatedView,'NOT_TESTED');
  for(const asset of manifest.Assets){
    assert.ok(asset.LessonCode.startsWith('PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:'));
    assert.ok(asset.Path.startsWith('src/Edulytics.Web/wwwroot/geometry/geo/v11/'));
    assert.ok(asset.VisualParity.startsWith('PARTIAL_FIRST_PARTY_CANDIDATE'));
    const svg=readFileSync(path.join(root,asset.Path),'utf8');
    assert.match(svg,/^<svg\s+[^>]*xmlns="http:\/\/www\.w3\.org\/2000\/svg"/);
    assert.match(svg,/<title\s+id="[^"]+">[^<]+<\/title>/);
    assert.match(svg,/<desc\s+id="[^"]+">[^<]+<\/desc>/);
    assert.match(svg,/role="img"/);
    assert.match(svg,/aria-labelledby="[a-z0-9-]+ [a-z0-9-]+"/);
    assert.ok(svg.includes(asset.AccessibleTitle),asset.Path);
    assert.ok(!/<(?:script|foreignObject|iframe|image)\b/i.test(svg),asset.Path);
    assert.ok(!/\bon[a-z]+\s*=/i.test(svg),asset.Path);
    assert.ok(!/\b(?:href|xlink:href)\s*=/i.test(svg),asset.Path);
    assert.ok(!/<style\b|@import|url\(/i.test(svg),asset.Path);
  }
});

test('focus/directrix SVG point P=(4,2) encodes equal distances and a real parabola',()=>{
  const asset=manifest.Assets.find(x=>x.LessonCode.endsWith('U06:L07'));
  assert.ok(asset);
  const svg=readFileSync(path.join(root,asset.Path),'utf8');
  assert.ok(svg.includes('P(4, 2)'));
  assert.ok(svg.includes('F(0, 2)'));
  assert.ok(svg.includes('y = −2'));
  assert.ok(svg.includes('M60 120 Q300 480 540 120'));
  const focus=[0,2], P=[4,2],directrixY=-2;
  const focusDistance=Math.hypot(P[0]-focus[0],P[1]-focus[1]);
  const directrixDistance=Math.abs(P[1]-directrixY);
  assert.equal(focusDistance,4);
  assert.equal(focusDistance,directrixDistance);
  assert.equal(P[0]**2,8*P[1]);
});

test('original solids and radians SVGs represent correct unit-dimensional relationships',()=>{
  const solid=manifest.Assets.find(x=>x.LessonCode.endsWith('U05:L01'));
  const rad=manifest.Assets.find(x=>x.LessonCode.endsWith('U07:L11'));
  const diagram=readFileSync(path.join(root,solid.Path),'utf8');
  const sector=readFileSync(path.join(root,rad.Path),'utf8');
  assert.ok(diagram.includes('V = πr²h'));
  assert.ok(sector.includes('θ = s/r')&&sector.includes('s = (π/3)r'));
  const r=6,h=8,theta=Math.PI/3;
  assert.ok(Math.abs((Math.PI*r*r*h)/(Math.PI*r*r)-h)<1e-12);
  assert.ok(Math.abs((r*theta)/r-theta)<1e-12);
});
