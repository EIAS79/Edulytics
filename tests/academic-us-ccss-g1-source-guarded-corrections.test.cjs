'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { createHash } = require('node:crypto');

const root = path.resolve(__dirname, '..');
const sourcePath = 'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json';
const classPath = 'src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs';
const materializerPath = 'src/Edulytics.Data/Seeding/CanonicalLessonContentMaterializer.cs';
const pack = JSON.parse(fs.readFileSync(path.join(root, sourcePath), 'utf8'));
const source = fs.readFileSync(path.join(root, classPath), 'utf8');
const materializer = fs.readFileSync(path.join(root, materializerPath), 'utf8');
const defs = [
  ['G1:U01:L04', 'CountBackOneOrTwo', 'us-ccss-g1-u01-l04-count-back-one-two-v1',
   '5041bfe615b64551b0fc59735b02065bcc4a6036ebea865faaf769e608f17eec'],
  ['G1:U01:L10', 'CenterAddSubtract', 'us-ccss-g1-u01-l10-center-add-subtract-v1',
   '287e0dcb25fc726798717075fc858e5f4cf529eead1b35837835ccfc2f7a20b8'],
  ['G1:U06:L17', 'PuppiesTulipsCompare', 'us-ccss-g1-u06-l17-puppies-tulips-compare-v1',
   '39bce6dd35872fc984af0b9017b8c7c61be8abc23c678db5b0b10f93c462e9dd']
];
function constant(key) {
  const match = source.match(new RegExp('public const string ' + key + ' = ("(?:[^"\\\\]|\\\\.)*");'));
  assert.ok(match, 'Missing constant: ' + key);
  return JSON.parse(match[1]);
}
const oldBody = en => createHash('sha256').update([
  en.cultureCode, en.title, en.explanation, en.keyConceptsAndRules,
  en.workedExamples, en.stepByStepSolutions, en.commonMistakes, en.quickSummary
].join('\n') + '\n').digest('hex');

test('exact three Grade 1 original bodies, source locations, standard arrays and persistent hashes are pinned', () => {
  assert.ok(materializer.includes('CommonCoreG1SourceGroundedInstructionalCorrections.Apply(document)'));
  assert.match(materializer, /CommonCoreG1SourceGroundedInstructionalCorrections\s*\.GetExpectedContentVersion/);
  assert.match(materializer, /CommonCoreG1SourceGroundedInstructionalCorrections\s*\.CanUpgradeExisting/);
  assert.ok(source.includes('Refusing G1 instruction correction of unexpected prose'));
  assert.ok(source.includes('G1 instruction source/official-standard drift'));
  assert.ok(source.includes('Refusing to overwrite unrecognized G1 persisted text'));
  for (const [short, name, version, old] of defs) {
    const lesson = pack.lessons.find(r => r.lessonCode === 'PED:US-CCSS-MATH:' + short);
    assert.ok(lesson, short);
    assert.equal(oldBody(lesson.translations.find(x => x.cultureCode === 'en')), old);
    assert.equal(constant(name + 'LessonCode'), lesson.lessonCode);
    assert.equal(constant(name + 'Version'), version);
    assert.ok(source.includes(JSON.stringify(lesson.sourceUrl)), 'URL pin: '+short);
    assert.ok(source.includes(JSON.stringify(lesson.canonicalBodySha256)), 'Canonical fingerprint pin: '+short);
    assert.ok(source.includes(JSON.stringify(old)), 'Stored body pin: '+short);
    for (const outcome of lesson.outcomeCodes) {
      assert.ok(source.includes(JSON.stringify(outcome)), 'Standard pin: '+short+'/'+outcome);
    }
    assert.ok(source.includes('[' + name + 'LessonCode] = new Repair('));
  }
});

test('corrected learning bodies are topic-specific and all numeric equalities are mathematically true', () => {
  let checked = 0;
  for (const [, name] of defs) {
    const examples = constant(name + 'Examples');
    const steps = constant(name + 'Steps');
    const concepts = constant(name + 'Concepts');
    const mistakes = constant(name + 'Mistakes');
    assert.equal((examples.match(/Example \d+/g) || []).length, 4, name);
    assert.equal((steps.match(/Step \d+/g) || []).length, 5, name);
    assert.ok(concepts.length > 130 && mistakes.length > 150, name);
    assert.ok(!/<(?:path|svg)\b/i.test(examples), name);
    assert.ok(!/=-\d+/.test(examples), name);
    assert.ok(!/Example 1:\s*“/.test(examples), name);
    for (const match of (examples + '\n' + steps).matchAll(/(\d+)\s*([+-])\s*(\d+)\s*=\s*(\d+)\b/g)) {
      const a = Number(match[1]), b = Number(match[3]), rhs = Number(match[4]);
      assert.equal(match[2] === '+' ? a + b : a - b, rhs, name + ': ' + match[0]);
      checked++;
    }
  }
  assert.ok(checked >= 20, 'Independent equality checks were unexpectedly reduced: ' + checked);
  assert.match(constant('CountBackOneOrTwoExamples'), /6 - 2 = 4/);
  assert.match(constant('CenterAddSubtractExamples'), /10 - 4 = 6/);
  assert.match(constant('PuppiesTulipsCompareExamples'), /7 - 4 = 3/);
});

test('changed materializer stays scoped to the three exact new G1 lesson identities', () => {
  for (const [, name] of defs) {
    const code = constant(name + 'LessonCode');
    assert.match(code, /^PED:US-CCSS-MATH:G1:/);
    assert.ok(source.includes('[' + name + 'LessonCode] = new Repair('));
  }
  assert.equal(pack.lessons.length, 146);
  assert.ok(!source.includes('ACADEMICALLY_CERTIFIED'));
});
