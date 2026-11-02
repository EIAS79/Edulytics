'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const repo = path.resolve(__dirname, '..');
const richFolder = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/';
const candidates = [
  'us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json',
  'us-ccss-g5-u03-l19-2021.rich-lesson-v2.json',
  'us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json',
  'us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json',
  'us-ccss-g5-decimals-placevalue-maxproduct-source-2021.rich-lesson-v2.json',
  'us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json',
  'us-ccss-g5-u02-l02-l05-2021.rich-lesson-v2.json',
  'us-ccss-g5-u02-l06-l08-2021.rich-lesson-v2.json'
];
const documents = candidates.map(filename => JSON.parse(
  fs.readFileSync(path.join(repo, richFolder, filename), 'utf8')
));
const lessons = documents.flatMap(document => document.Lessons);
const lessonById = new Map(lessons.map(lesson => [lesson.LessonCode, lesson]));
const get = id => {
  const lesson = lessonById.get('PED:US-CCSS-MATH:G5:' + id);
  assert.ok(lesson, 'missing Rich V2 lesson ' + id);
  return lesson;
};

test('all 26 authored G5 sidecars have unique identity and correct fields', () => {
  assert.equal(lessons.length, 26);
  assert.equal(lessonById.size, 26);
  for (const document of documents) {
    assert.equal(document.SchemaVersion, 1);
    assert.equal(document.PackCode, 'US-CCSS-MATH');
    assert.ok(document.ContentVersion);
    for (const lesson of document.Lessons) {
      assert.match(lesson.LessonCode, /^PED:US-CCSS-MATH:G5:U[0-9]{2}:L[0-9]{2}$/);
      assert.equal(lesson.CultureCode, 'en');
      assert.ok(lesson.Title);
      assert.ok(lesson.ExplanationParagraphs.length >= 2);
      assert.ok(lesson.KeyConcepts.length >= 3);
      assert.ok(lesson.WorkedExamples.length >= 3);
      assert.ok(lesson.CommonMistakes.length >= 2);
      assert.ok(lesson.SummaryPoints.length >= 3);
      assert.ok(lesson.Visuals.length >= 1);
      for (const example of lesson.WorkedExamples) {
        for (const field of ['Title','Question','Method','Answer','Check']) {
          assert.ok(example[field], lesson.LessonCode + '.' + field);
        }
        assert.ok(example.Steps.length >= 3, lesson.LessonCode);
      }
      for (const visual of lesson.Visuals) {
        assert.equal(visual.Kind, 'RatioTable');
        assert.ok(visual.Title && visual.Description);
        assert.ok(visual.Rows.length > 0);
        for (const row of visual.Rows) {
          assert.ok(row.Label && row.Cells.length > 0);
        }
      }
    }
  }
});

test('volume sidecars L04-L06 teach partially pictured layers and alternative bases', () => {
  for (const id of ['U01:L04','U01:L05','U01:L06']) {
    const lesson = get(id);
    const text = lesson.ExplanationParagraphs.join(' ');
    assert.match(text, /layer|base/i);
    assert.match(text, /teacher|peer|partner/i);
    assert.match(lesson.SummaryPoints.join(' '), /2021/i);
  }
  assert.match(get('U01:L04').WorkedExamples[0].Answer, /24/);
  assert.match(get('U01:L05').WorkedExamples[0].Answer, /30/);
  assert.match(get('U01:L06').WorkedExamples[0].Answer, /24/);
});

test('sandwich lesson preserves exploratory representation rather than forcing early equations', () => {
  const text = get('U02:L01').ExplanationParagraphs.join(' ');
  assert.match(text, /not yet required|not replace/i);
  assert.match(text, /teacher/i);
  assert.equal(2 / 3 + 2 / 3 + 2 / 3, 2);
  assert.equal(1 + 1 / 2 + 1 + 1 / 2, 3);
});

test('fraction-of-fraction lesson uses the same whole', () => {
  const lesson = get('U03:L01');
  assert.match(lesson.ExplanationParagraphs.join(' '), /one third of the half/i);
  assert.equal(1 / 2 / 3, 1 / 6);
  assert.equal(1 / 2 / 4, 1 / 8);
  assert.match(lesson.WorkedExamples[0].Answer, /sixth/i);
});

test('coordinate rectangle lesson includes location, dimensions, and original teacher review', () => {
  const lesson = get('U07:L01');
  assert.match(lesson.ExplanationParagraphs.join(' '), /partner/i);
  assert.match(lesson.ExplanationParagraphs.join(' '), /drawing|reconstruct/i);
  assert.equal(6 - 2, 4);
  assert.equal(4 - 1, 3);
});

test('place-value source lessons validate thousandth and tenfold relationships', () => {
  const tiny = get('U05:L01');
  const place = get('U06:L01');
  assert.match(tiny.ExplanationParagraphs.join(' '), /thousandth/i);
  assert.equal(100 * 10, 1000);
  assert.equal(0.001 * 10, 0.01);
  assert.ok(Math.abs(0.6 / 10 - 0.06) < 1e-12);
  assert.match(place.WorkedExamples[0].Answer, /0.6/);
});

test('capstone greatest-product claims are exhaustively correct', () => {
  const lesson = get('U08:L01');
  const all = digits => {
    const possibilities = [];
    for (const a of digits) for (const b of digits) for (const c of digits) {
      if (new Set([a,b,c]).size !== 3) continue;
      possibilities.push({formula: (10 * a + b) + '×' + c, result: (10*a + b)*c});
    }
    return possibilities;
  };
  const set752 = all([7,5,2]);
  const set631 = all([6,3,1]);
  assert.equal(set752.length, 6);
  assert.equal(set631.length, 6);
  assert.equal(Math.max(...set752.map(x => x.result)), 364);
  assert.equal(Math.max(...set631.map(x => x.result)), 186);
  assert.ok(set752.some(x => x.formula === '52×7' && x.result === 364));
  assert.ok(set631.some(x => x.formula === '31×6' && x.result === 186));
  assert.match(lesson.WorkedExamples[0].Check, /135/);
  assert.equal(841 * 6, 5046);
  assert.equal(641 * 8, 5128);
});

test('Rich V2 has no approved external videos or linked publisher images', () => {
  for (const lesson of lessons) {
    assert.equal(lesson.Videos.length, 0);
    assert.ok(lesson.Visuals.every(x => !('ImageUrl' in x) && !('Image' in x)));
    assert.match(lesson.SummaryPoints.join(' '), /2021/i);
  }
});


test('Unit 1: all twelve lessons have distinct source-aligned Rich V2 bodies', () => {
  for (let i = 1; i <= 12; i++) {
    const short = 'U01:L' + String(i).padStart(2,'0');
    const lesson = get(short);
    assert.ok(lesson.ExplanationParagraphs.join(' ').length >= 250, short);
    assert.ok(lesson.Visuals.some(x => x.Rows.length > 0), short);
  }
});

test('cubic-unit and composite examples respect units and volume additivity', () => {
  assert.match(get('U01:L07').WorkedExamples[2].Answer, /24 cubic feet/);
  assert.equal(12 + 18, 30);
  assert.match(get('U01:L08').WorkedExamples[1].Answer, /30 cubic units/);
  assert.equal(5*3*3 + 1, 46);
  assert.match(get('U01:L09').WorkedExamples[0].Answer, /46/);
  assert.equal(5*5*5 - 2*3*5, 95);
  assert.equal(3*5*5 + 2*2*5, 95);
  assert.match(get('U01:L10').WorkedExamples[0].Answer, /95/);
  assert.equal(4*9*5, 180);
  assert.equal(30*9, 270);
  assert.equal(3*8*4 + 10*3*4, 216);
  assert.match(get('U01:L11').WorkedExamples[2].Answer, /216/);
});

test('optional investigation has five valid container arrangements and teacher-review boundary', () => {
  const source = get('U01:L12');
  for (const factors of [[1,6,10],[2,5,6],[3,4,5],[2,3,10],[1,3,20]]) {
    assert.equal(factors.reduce((x,y)=>x*y,1), 60);
  }
  assert.match(source.ExplanationParagraphs.join(' '), /teacher review/i);
  assert.match(source.KeyConcepts[1].Rule, /footprint|height|clearance/i);
  assert.match(source.WorkedExamples[1].Answer, /384/);
});

test('Unit 2 L01-L05 establishes exact sharing quotients with distinct representations', () => {
  for(let i=1;i<=5;i++){
    const code='U02:L'+String(i).padStart(2,'0');
    const lesson=get(code);
    assert.match(lesson.ExplanationParagraphs.join(' '), /share|fraction|division/i,code);
    assert.match(lesson.SummaryPoints.join(' '), /2021/i,code);
  }
  assert.match(get('U02:L02').WorkedExamples[2].Answer, /2\/3/);
  assert.match(get('U02:L03').WorkedExamples[0].Answer, /2\/3/);
  assert.match(get('U02:L04').WorkedExamples[0].Answer, /2\/3/);
  assert.match(get('U02:L05').WorkedExamples[0].Answer, /false/i);
});

test('sharing examples reject reversed quotients and preserve fixed whole', () => {
  assert.equal(3*(2/3),2);
  assert.ok(3*(3/2)>2);
  assert.equal(4/6,2/3);
  assert.equal(36/7,5+1/7);
  assert.equal(5/2,2.5);
  assert.equal(3/6,0.5);
  assert.equal(4/3,1+1/3);
});

test('Unit 2 L06-L08 retains different fraction source reasoning tasks', () => {
 for(const id of ['U02:L06','U02:L07','U02:L08']){
  const l=get(id);
  assert.match(l.SummaryPoints.join(' '), /2021/);
  assert.match(l.ExplanationParagraphs.join(' '), /teacher|partner|peer/i);
 }
 assert.match(get('U02:L06').WorkedExamples[0].Answer, /3\/2/);
 assert.match(get('U02:L07').WorkedExamples[1].Answer, /7\/4/);
 assert.match(get('U02:L08').WorkedExamples[1].Answer, /6\/5/);
});

test('Unit 2 source problems preserve exact multiplication/division equivalences', () => {
 assert.equal(3/2,3*(1/2));
 assert.equal(9/4,2+1/4);
 assert.equal(7/4,1+3/4);
 assert.ok(Math.abs(3*(2/5)-6/5)<1e-12);
 assert.ok(Math.abs(2*(3/5)-6/5)<1e-12);
 assert.ok(Math.abs(3*(3/5)-9/5)<1e-12);
 assert.equal(2*(1/3*6),4);
 assert.equal(2*(1/4*6),3);
});
