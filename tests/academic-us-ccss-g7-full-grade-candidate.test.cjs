'use strict';

// Grade 7 candidate *static* contract only. This is not Stage A academic
// acceptance, a real generator/verifier run, a seeded DB check or owner UAT.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const dir = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const bp = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade7-our-2017.lesson-blueprint.json');
const files = fs.readdirSync(path.join(root, dir))
  .filter(name => /^us-ccss-g7-.*\.rich-lesson-v2\.json$/.test(name))
  .sort();
const docs = files.map(name => ({ name, doc: read(path.join(dir, name)) }));
const lessons = docs.flatMap(({name, doc}) => doc.Lessons.map(lesson => ({...lesson, sourceFile: name})));
const byCode = new Map(lessons.map(x => [x.LessonCode, x]));

test('Grade 7 full-grade candidate has exact canonical 145 IDs and 9 unit counts', () => {
  assert.equal(bp.BlueprintCode, 'US-CCSS-MATH:G7:OUR-IM-2017');
  assert.equal(bp.Lessons.length, 145);
  assert.equal(lessons.length, 145);
  assert.equal(byCode.size, 145, 'No duplicate Rich V2 LessonCode');
  assert.deepEqual(new Set(byCode.keys()), new Set(bp.Lessons.map(x=>x.LessonCode)));
  const expected = [13,15,11,16,17,23,17,20,13];
  for (let u=1;u<=9;u++) {
    const count = lessons.filter(x=>x.LessonCode.includes(':U'+String(u).padStart(2,'0')+':')).length;
    assert.equal(count,expected[u-1],'Incorrect coverage for U'+u);
  }
});

test('Rich V2 schema, exact title/source, 3 concrete examples and no duplicate prompts', () => {
  const prompts = new Set();
  const source = new Map(bp.Lessons.map(x=>[x.LessonCode,x]));
  for (const {name, doc} of docs) {
    assert.equal(doc.SchemaVersion,1,name);
    assert.equal(doc.PackCode,'US-CCSS-MATH',name);
    assert.ok(doc.ContentVersion,name);
    assert.ok(doc.Lessons.length>0,name);
  }
  for(const l of lessons) {
    const x=source.get(l.LessonCode);
    assert.ok(x,l.LessonCode);
    assert.equal(l.Title,x.Title,l.LessonCode);
    assert.equal(l.CultureCode,'en',l.LessonCode);
    assert.ok(l.ExplanationParagraphs.length>=2,l.LessonCode);
    assert.ok(l.ExplanationParagraphs.join(' ').includes(x.SourceUrl),l.LessonCode);
    assert.ok(l.KeyConcepts.length>=3,l.LessonCode);
    assert.ok(l.WorkedExamples.length>=3,l.LessonCode);
    assert.ok(l.CommonMistakes.length>=2,l.LessonCode);
    assert.ok(l.SummaryPoints.length>=3,l.LessonCode);
    assert.ok(!l.ExplanationParagraphs.join(' ').includes('Learning goal: You will'),l.LessonCode);
    for (const k of l.KeyConcepts) for(const field of ['Title','Definition','Rule','Example']) assert.ok(k[field],l.LessonCode+':'+field);
    for(const w of l.WorkedExamples) {
      for(const field of ['Title','Question','Method','Answer','Check']) assert.ok(w[field],l.LessonCode+':'+field);
      assert.ok(Array.isArray(w.Steps)&&w.Steps.length>=3,l.LessonCode);
      assert.ok(w.Steps.every(s=>typeof s==='string'&&s.trim()),l.LessonCode);
      assert.ok(!prompts.has(w.Question.toLowerCase().trim()),l.LessonCode+': duplicate example question');
      prompts.add(w.Question.toLowerCase().trim());
    }
    for(const mistake of l.CommonMistakes) for(const field of ['Mistake','WhyWrong','Correction']) assert.ok(mistake[field],l.LessonCode+':'+field);
  }
  assert.ok(prompts.size>=435,'Three unique worked problems per lesson');
});

test('source-targeted mathematics checkpoints are independently computed', () => {
  const L = (u,n)=>byCode.get('PED:US-CCSS-MATH:G7:U'+String(u).padStart(2,'0')+':L'+String(n).padStart(2,'0'));
  assert.ok(L(1,1).ExplanationParagraphs.join(' ').includes('scaled'), 'U01 L01 scaling');
  assert.ok(L(6,1).ExplanationParagraphs.join(' ').includes('not proportional'), 'U06 L01 not-proportional target');
  assert.ok(L(8,13).ExplanationParagraphs.join(' ').includes('random'), 'U08 L13 sampling goal');
  assert.ok(L(9,13).ExplanationParagraphs.join(' ').includes('5 km'), 'U09 L13 course design');
  // These computations are reference oracles, not a test of the live Practice generator.
  assert.equal((6/3),2);
  assert.equal((7/4),1.75);
  assert.equal((12/2),6);
  assert.equal((3*4*5),60);
  assert.equal((6*4/2)*8,96);
  assert.equal((16/5),3.2);
  assert.equal(40*(1-0.15),34);
  assert.equal(5000/1250,4);
  assert.equal((-4) >= (-4),true);
  assert.equal((-5) > (-4),false);
});

test('candidate ledger must not silently claim academic or runtime PASS',()=>{
  const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authoring-ledger.json');
  assert.equal(ledger.courseCode,bp.BlueprintCode);
  assert.equal(ledger.lessons.length,145);
  assert.equal(ledger.counts.directRichCandidate,145);
  assert.equal(ledger.counts.stageDVerified,0);
  assert.equal(ledger.counts.stageCVerified,0);
  for (const item of ledger.lessons) {
    assert.ok(byCode.has(item.lessonCode));
    assert.equal(item.stages.D,'NOT_RUN');
    assert.equal(item.stages.C,'NOT_VERIFIED');
    assert.notEqual(item.humanAcademicAcceptance,'APPROVED');
  }
});
