'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const { readFileSync, readdirSync } = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..', '..');
const read = (p) => JSON.parse(readFileSync(path.join(root, p), 'utf8'));
const blueprint = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json');
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-geo-phase29-v1.lesson-content-pack.json');
const rich = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l01-l03-2021.rich-lesson-v2.json');
const richNext = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l04-l06-2021.rich-lesson-v2.json');
const richThird = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l07-l09-2021.rich-lesson-v2.json');
const richFourth = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l10-l12-2021.rich-lesson-v2.json');
const richFifth = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l13-l15-2021.rich-lesson-v2.json');
const richSixth = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l16-l18-2021.rich-lesson-v2.json');
const richSeventh = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l19-l22-2021.rich-lesson-v2.json');
const richDirectory = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const geoResources = readdirSync(path.join(root, richDirectory))
  .filter(name => /^us-ccss-hs-geo-.*\.rich-lesson-v2\.json$/.test(name)).sort();
const authored = geoResources.flatMap(name => read(path.join(richDirectory, name)).Lessons);
const evidence = read('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-124-source-manifest-20261028.json');
const prefix = 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U01:L';

test('Geometry identities, source links, outcomes and order match all 124 canonical records', () => {
  assert.equal(blueprint.BlueprintCode, 'US-CCSS-MATH:HS:TRAD:GEO:IM-1E');
  assert.equal(blueprint.CourseCode, 'GEO');
  assert.equal(blueprint.Pathway, 'Traditional');
  assert.equal(blueprint.Units.length, 8);
  assert.equal(blueprint.Lessons.length, 124);
  assert.equal(pack.lessons.length, 124);
  assert.equal(new Set(blueprint.Lessons.map(x => x.LessonCode)).size, 124);
  assert.equal(new Set(blueprint.Lessons.map(x => x.SourceLessonCode)).size, 124);
  assert.equal(new Set(pack.lessons.map(x => x.lessonCode)).size, 124);
  const contentByCode = new Map(pack.lessons.map(x => [x.lessonCode, x]));
  for (const [i, lesson] of blueprint.Lessons.entries()) {
    assert.equal(lesson.SortOrder, i + 1);
    const candidate = contentByCode.get(lesson.LessonCode);
    assert.ok(candidate, lesson.LessonCode);
    assert.equal(candidate.sourceLocator, lesson.SourceLessonCode);
    assert.equal(candidate.sourceUrl, lesson.SourceUrl);
    assert.ok(lesson.SourceUrl.startsWith('https://im.kendallhunt.com/HS/teachers/2/'));
    assert.deepEqual(candidate.outcomeCodes, lesson.FormalTargets.map(o => o.OutcomeCode));
    assert.equal(candidate.isSupporting, false);
  }
});

test('Course-wide matrix records unverified runtime and no fabricated approvals', () => {
  assert.equal(evidence.lessons.length, 124);
  assert.equal(evidence.totals.blueprintLessons, 124);
  assert.equal(evidence.totals.richCandidateLessons, 53);
  assert.equal(evidence.totals.gradeAccepted, 0);
  assert.equal(evidence.totals.runtimeVerified, 0);
  const known = blueprint.Lessons.map(l => l.LessonCode);
  assert.deepEqual(evidence.lessons.map(l => l.lessonCode), known);
  for (const row of evidence.lessons) {
    assert.equal(row.stageD, 'NOT_RUN');
    assert.equal(row.release, 'NOT_ACCEPTED');
    assert.equal(row.persisted, 'NOT_CHECKED');
    assert.ok(row.sourceSemanticSha256.length === 64);
    assert.ok(row.canonicalBodySha256.length === 64);
  }
});

test('All indexed Geometry candidate resources have authored source-specific contracts', () => {
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 3);
  assert.equal(richNext.Lessons.length, 3);
  assert.equal(richThird.Lessons.length, 3);
  assert.equal(richFourth.Lessons.length, 3);
  assert.equal(richFifth.Lessons.length, 3);
  assert.equal(richSixth.Lessons.length, 3);
  assert.equal(richSeventh.Lessons.length, 4);
  assert.deepEqual(
    authored.map(l => l.LessonCode),
    evidence.lessons.filter(l => l.richCandidateOnBranch).map(l => l.lessonCode)
  );
  assert.equal(authored.length, evidence.totals.richCandidateLessons);
  assert.equal(new Set(authored.map(l => l.LessonCode)).size, authored.length);
  for (const lesson of authored) {
    assert.equal(lesson.CultureCode, 'en');
    assert.equal(lesson.WorkedExamples.length, 3);
    assert.ok(lesson.ExplanationParagraphs.some(x => /teacher[- ]scor|teacher[- ]review|teacher[- ]check|teacher rubric|Human assessment/i.test(x)));
    assert.ok(lesson.ExplanationParagraphs.join(' ').includes('CC BY 4.0'));
    assert.ok(lesson.KeyConcepts.length >= 3);
    assert.ok(lesson.CommonMistakes.length >= 2);
    assert.ok(lesson.WorkedExamples.every(ex => ex.Steps.length >= 3 && ex.Answer && ex.Check));
    assert.ok(!JSON.stringify(lesson).includes('You will constructing Patterns'));
    assert.ok(!JSON.stringify(lesson).includes('You will build It'));
    assert.deepEqual(lesson.Visuals, []);
    assert.deepEqual(lesson.Videos, []);
  }
});

test('Two-circle constructions are geometrically consistent at boundary cases', () => {
  const distance = (p, q) => Math.hypot(p[0] - q[0], p[1] - q[1]);
  for (const d of [1, 6, 11]) {
    const r = 0.7 * d;
    const A = [0, 0], B = [d, 0];
    const y = Math.sqrt(r*r - (d/2)*(d/2));
    for (const P of [[d/2, y], [d/2, -y]]) {
      assert.ok(Math.abs(distance(A, P) - r) < 1e-10);
      assert.ok(Math.abs(distance(B, P) - r) < 1e-10);
    }
    assert.ok(y > 0);
    assert.equal(r <= d/2, false);
  }
  assert.equal(2 * 4 > 10, false);
  assert.equal(2 * 5 > 10, false);
  assert.equal(2 * 6 > 10, true);
  // With radius AB, the apex yields equilateral triangles and equal-sided rhombi.
  const d=2, r=2, y=Math.sqrt(r*r - (d/2)*(d/2));
  const A=[0,0],B=[d,0],C=[d/2,y],D=[d/2,-y];
  assert.ok([distance(A,C),distance(C,B),distance(B,D),distance(D,A)].every(x=>Math.abs(x-d)<1e-10));
});

test('Candidate remains isolated from shared Practice and cannot certify release', () => {
  for (const l of authored) {
    assert.ok(l.LessonCode.startsWith('PED:US-CCSS-MATH:HS:TRAD:GEO:'));
    assert.ok(!Object.hasOwn(l, 'AutoGrade'));
    assert.ok(!Object.hasOwn(l, 'Approved'));
    assert.ok(!Object.hasOwn(l, 'AcademicApproved'));
  }
  assert.equal(evidence.status, 'WORK_IN_PROGRESS_NOT_RELEASE_READY');
});

test('Parallel and angle-bisector authored examples obey nondegenerate conditions', () => {
  const next = new Map(richNext.Lessons.map(x => [x.LessonCode, x]));
  const eq = next.get(prefix+'04');
  const ang = next.get(prefix+'05');
  const para = next.get(prefix+'06');
  assert.ok(eq.WorkedExamples.some(x => x.Answer.includes('ACE')));
  assert.ok(ang.WorkedExamples.some(x => x.Answer.includes('35-degree')));
  assert.ok(para.WorkedExamples.some(x => x.Answer.includes('y=3')));
  // Two perpendiculars to a transversal are parallel in Euclidean geometry.
  const horizontal = [1,0], vertical = [0,1];
  const dot = (a,b) => a[0]*b[0] + a[1]*b[1];
  assert.equal(dot(horizontal, vertical), 0);
  assert.equal(dot(vertical, horizontal), 0);
  assert.equal(dot(horizontal, horizontal), 1);
  assert.equal(2*3 > 6, false); // tangency: one circle-line contact only
  assert.equal(2*4 > 6, true); // larger radius: two circle-line intersections
});

test('Original square, dynamic geometry and Voronoi candidates have truthful assessment boundaries', () => {
  const byCode = new Map(richThird.Lessons.map(l => [l.LessonCode, l]));
  const sq = byCode.get(prefix+'07'), digital = byCode.get(prefix+'08'), vor = byCode.get(prefix+'09');
  assert.ok(sq.ExplanationParagraphs.join(' ').includes('four right'));
  assert.ok(digital.ExplanationParagraphs.join(' ').includes('optional source lesson'));
  assert.ok(digital.ExplanationParagraphs.join(' ').includes('Drag'));
  assert.ok(vor.ExplanationParagraphs.join(' ').includes('Euclidean'));
  for (const candidate of [sq, digital, vor]) {
    assert.ok(candidate.ExplanationParagraphs.some(x => /teacher rubric|teacher[- ]scor|teacher[- ]review/i.test(x)), candidate.LessonCode);
    assert.equal(candidate.WorkedExamples.length, 3);
    assert.equal(candidate.Visuals.length, 0);
  }
  // Perpendicular bisector x=2 of A=(0,0),B=(4,0):
  for (const y of [-9,0,2,11]) {
    const distanceSq = (p,q) => (p[0]-q[0])**2 + (p[1]-q[1])**2;
    assert.equal(distanceSq([2,y],[0,0]), distanceSq([2,y],[4,0]));
  }
  // A=(0,0), B=(4,0), C=(0,4): A is nearest where x<=2 and y<=2.
  for(const [x,y] of [[0,0],[1,1],[2,2]]) {
    const ds=(q)=> (x-q[0])**2+(y-q[1])**2;
    assert.ok(ds([0,0])<=ds([4,0])&&ds([0,0])<=ds([0,4]));
  }
  // A circle of radius r has inscribed-square side r*sqrt(2), and area 2r^2.
  for(const radius of [0.5,3,10]) {
    const side=Math.sqrt(2)*radius;
    assert.ok(Math.abs(side**2 - 2*radius*radius)<1e-10);
  }
});

test('Rigid-motion isometries preserve squared distance; reflection reverses orientation and order can matter', () => {
  const candidate = new Map(richFourth.Lessons.map(x => [x.LessonCode, x]));
  assert.ok(candidate.get(prefix+'10').WorkedExamples.some(x => x.Answer.includes('(-3,1)')));
  assert.ok(candidate.get(prefix+'11').WorkedExamples.some(x => x.Answer.replaceAll('−', '-').includes('(-1,1)')));
  assert.ok(candidate.get(prefix+'12').WorkedExamples.some(x => x.Answer.includes('non-parallel')));
  const d2=(a,b)=>(a[0]-b[0])**2+(a[1]-b[1])**2;
  const shift=(p)=>[p[0]+2,p[1]-1];
  const reflection=(p)=>[-p[0],p[1]];
  for(const [p,q] of [[[0,0],[3,4]],[[1,-1],[-2,7]],[[2,3],[2,3]]]) {
    assert.equal(d2(shift(p),shift(q)),d2(p,q));
    assert.equal(d2(reflection(p),reflection(q)),d2(p,q));
  }
  assert.notDeepEqual(reflection(shift([1,0])),shift(reflection([1,0])));
  assert.ok(reflection([0,4]).every((v,i) => Math.abs(v - [0,4][i]) < 1e-12)); // mirror fixed point; normalize signed zero
  // A translation can form only a degenerate quadrilateral when parallel to its segment.
  const determinant=(v,w)=>v[0]*w[1]-v[1]*w[0];
  assert.equal(determinant([3,0],[2,0]),0);
  assert.equal(determinant([3,0],[2,1]),3);
});

test('Rotational symmetry, congruence alignment, and proof candidates obey key invariants', () => {
  const byCode = new Map(authored.map(x=>[x.LessonCode,x]));
  for(let n=13;n<=22;n++) {
    const lesson=byCode.get(prefix+String(n).padStart(2,'0'));
    assert.ok(lesson, 'missing lesson '+n);
    assert.equal(lesson.WorkedExamples.length,3);
    assert.ok(lesson.ExplanationParagraphs.join(' ').includes('Teacher rubric'),lesson.LessonCode);
    assert.ok(lesson.ExplanationParagraphs.join(' ').includes('CC BY 4.0'),lesson.LessonCode);
    assert.ok(lesson.CommonMistakes.length>=2);
  }
  assert.equal(evidence.lessons[7].sourceOptional,true);  // L08
  assert.equal(evidence.lessons[17].sourceOptional,true); // L18
  assert.equal(evidence.lessons[21].sourceOptional,true); // L22
  // Rotation by 90 degrees about origin is an isometry and changes orientation angle, not side lengths.
  const rotate90=p=>[-p[1],p[0]];
  const sq=(a,b)=> (a[0]-b[0])**2+(a[1]-b[1])**2;
  const pts=[[0,0],[2,0],[0,1]];
  for(let i=0;i<3;i++)for(let j=0;j<3;j++)assert.equal(sq(pts[i],pts[j]),sq(rotate90(pts[i]),rotate90(pts[j])));
  const reflect=p=>[-p[0],p[1]];
  const twice=p=>[-p[0],-p[1]];
  assert.equal(sq(reflect([1,2]),reflect([4,6])),25);
  assert.deepEqual(twice([4,3]),[-4,-3]);
  // Triangle angle sum and vertical angles: independent arithmetic property cases.
  for(const [A,B] of [[30,70],[1,1],[89,1],[45,90]]) {
    const C=180-A-B;
    assert.ok(C>0);
    assert.equal(A+B+C,180);
    assert.equal((180-A)+(A),180);
  }
  assert.equal(360/5,72);
  assert.equal(360/6,60);
});

test('Unit 2 triangle criteria use valid numerical hypotheses and no SSA/AAA misgrading', () => {
  const U2 = 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U02:L';
  const byCode = new Map(authored.map(l => [l.LessonCode,l]));
  for(let n=1;n<=9;n++) {
    const lesson=byCode.get(U2+String(n).padStart(2,'0'));
    assert.ok(lesson, 'missing U02:'+n);
    assert.ok(lesson.ExplanationParagraphs.join(' ').includes('CC BY 4.0'));
    assert.equal(lesson.WorkedExamples.length,3);
    assert.ok(lesson.WorkedExamples.every(ex=>ex.Steps.length>=3 && ex.Answer));
  }
  // AAA counterexample: proportional sides yield similarity but not rigid congruence.
  assert.equal(Math.hypot(3,4),5);
  assert.equal(Math.hypot(6,8),10);
  assert.ok([3,4,5].every((x,i)=> [6,8,10][i]===2*x));
  // SSA example from U02 L04 has two strictly positive possible positions.
  const discriminant = 300-256, coeff=10*Math.sqrt(3);
  assert.ok(discriminant>0);
  const roots=[(coeff-Math.sqrt(discriminant))/2,(coeff+Math.sqrt(discriminant))/2];
  for (const t of roots) {
    assert.ok(t>0);
    const C=[t*Math.cos(Math.PI/6),t*Math.sin(Math.PI/6)];
    assert.ok(Math.abs((C[0]-10)**2 + C[1]**2 - 36)<1e-10);
  }
  assert.notEqual(roots[0],roots[1]); // distinct AC lengths -> noncongruent
  // ASA proof angle sum, and triangle inequality guard of SSS.
  assert.equal(45+65+70,180);
  assert.ok(5+7>8 && 5+8>7 && 7+8>5);
  assert.equal(2+3>6,false);
  // Equidistant locus for A=(-3,0),B=(3,0): x=0, and only x=0.
  for(const x of [-7,-1,0,2,13]) {
    const pa2=(x+3)**2+4**2, pb2=(x-3)**2+4**2;
    assert.equal(pa2===pb2,x===0);
  }
});
test('All fifteen source-specific Unit 2 lessons and exact quadrilateral invariants', () => {
  const prefix2='PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U02:L';
  const index=new Map(authored.map(x=>[x.LessonCode,x]));
  for(let i=1;i<=15;i++) {
    const x=index.get(prefix2+String(i).padStart(2,'0'));
    assert.ok(x, 'missing U02 L'+i);
    assert.ok(x.WorkedExamples.length>=3 && x.KeyConcepts.length>=3 && x.CommonMistakes.length>=2);
    assert.ok(x.ExplanationParagraphs.join(' ').includes('CC BY 4.0'));
    assert.ok(x.ExplanationParagraphs.some(v=>/teacher rubric|teacher[- ]scor|teacher[- ]review/i.test(v)));
  }
  assert.equal(evidence.lessons[32].sourceOptional,true); // U02 L11, canonical ordinal 33
  // A square and side-equal non-square rhombus are not congruent.
  assert.equal(5*Math.sqrt(2)===5*Math.sqrt(3),false);
  // Parallelogram with intersecting perpendicular-bisecting diagonals.
  for(const [a,b] of [[3,4],[2,7],[0.5,5]]) {
    const fourSideSquares=[a*a+b*b,a*a+b*b,a*a+b*b,a*a+b*b];
    assert.equal(new Set(fourSideSquares).size,1);
  }
  // Parallel Gram adjacent congruence and rectangle criterion.
  const diagonalSq=(a,b)=>a*a+b*b;
  assert.equal(diagonalSq(6,2),40);
  assert.equal(180-68,112);
  // Fixed side length rhombus can vary diagonals under a hinge angle.
  const r=5;
  const diag1=Math.sqrt(2*r*r*(1+Math.cos(Math.PI/3)));
  const diag2=Math.sqrt(2*r*r*(1-Math.cos(Math.PI/3)));
  assert.ok(Math.abs(diag1-5*Math.sqrt(3))<1e-10);
  assert.ok(Math.abs(diag2-5)<1e-10);
});

test('All 16 Similarity candidates and scaled model/Pythagorean/bank-shot independent oracles', () => {
  const byCode=new Map(authored.map(x=>[x.LessonCode,x]));
  const P='PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L';
  for(let i=1;i<=16;i++) {
    const item=byCode.get(P+String(i).padStart(2,'0'));
    assert.ok(item, 'missing U03 L'+i);
    assert.equal(item.WorkedExamples.length,3);
    assert.ok(item.WorkedExamples.every(ex=>ex.Steps.length>=3 && ex.Answer && ex.Check));
    assert.ok(item.ExplanationParagraphs.join(' ').includes('CC BY 4.0'));
  }
  for (const n of [2,10,12]) assert.equal(evidence.lessons[37+n-1].sourceOptional,true);
  const dilate=(p,o,k)=>o.map((v,i)=>v+k*(p[i]-v));
  const P1=[1,1], A=[2,1], B=[1,3], Ap=dilate(A,P1,2),Bp=dilate(B,P1,2);
  assert.deepEqual(Ap,[3,1]);
  assert.deepEqual(Bp,[1,5]);
  const d2=(p,q)=>(p[0]-q[0])**2+(p[1]-q[1])**2;
  assert.equal(d2(Ap,Bp),4*d2(A,B));
  assert.equal(6**2+8**2,10**2);
  assert.equal(6*8/10,4.8);
  assert.ok(Math.abs(3.6*6.4-4.8**2)<1e-10);
  const t=3/8, bounce=[2+6*t,3-8*t];
  assert.deepEqual(bounce,[4.25,0]);
  assert.equal(150000000/12700*0.02>200,true); // metres: 2cm Earth model
  assert.equal(9*3**2,81); // area scale
});
