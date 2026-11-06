'use strict';

const assert = require('node:assert/strict');
const crypto = require('node:crypto');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '../..');
const docsDir = path.join(root, 'docs', 'curriculum');
const testsDir = path.join(root, 'tests');
const blueprintPath = path.join(
  root,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json'
);
const contentPackPath = path.join(
  root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g8-phase29-v1.lesson-content-pack.json'
);
const closurePath = path.join(
  docsDir,
  'us-ccss-g8-coder4-u09-unit-abc-closure-report-20261105.json'
);

const unitCounts = [17, 13, 14, 16, 22, 11, 16, 16, 6];
const bounds = { D1: [1, 44], D2: [45, 88], D3: [89, 131] };
const lessonCode = (unit, lesson) =>
  'PED:US-CCSS-MATH:G8:U' + String(unit).padStart(2, '0') +
  ':L' + String(lesson).padStart(2, '0');
const readJson = p => JSON.parse(fs.readFileSync(p, 'utf8'));
const slash = p => p.replaceAll('\\\\', '/');

function expectedLessons() {
  const out = [];
  let ordinal = 0;
  for (let unit = 1; unit <= unitCounts.length; unit++) {
    for (let lesson = 1; lesson <= unitCounts[unit - 1]; lesson++) {
      out.push({
        ordinal: ++ordinal,
        unit,
        lesson,
        lessonCode: lessonCode(unit, lesson),
      });
    }
  }
  assert.equal(out.length, 131);
  return out;
}

function checkpointFiles() {
  const files = fs.readdirSync(docsDir)
    .filter(name =>
      /^us-ccss-g8-coder4-u\d{2}-l\d{2}-.+stage-c-checkpoint-\d{8}\.json$/i.test(name))
    .sort();
  assert.equal(files.length, 131, 'expected exactly 131 Grade 8 Stage C checkpoint files');
  return files;
}

function resolveTestPath(data, checkpointName, unit, lesson) {
  const explicit = data.focusedTestPath || data.testsAuthored;
  if (typeof explicit === 'string' && explicit.trim()) {
    const normalized = slash(explicit.trim());
    assert.ok(normalized.startsWith('tests/'), 'test path must remain under tests/: ' + normalized);
    assert.ok(fs.existsSync(path.join(root, normalized)), 'missing authored lesson test: ' + normalized);
    return normalized;
  }

  const prefix =
    'academic-us-ccss-g8-coder4-u' + String(unit).padStart(2, '0') +
    '-l' + String(lesson).padStart(2, '0') + '-';
  const matches = fs.readdirSync(testsDir)
    .filter(name => name.startsWith(prefix) && /-route-\d{8}\.test\.cjs$/i.test(name))
    .sort();

  assert.equal(
    matches.length,
    1,
    'checkpoint has no explicit test path and cannot be mapped uniquely: ' + checkpointName +
      ' matches=' + JSON.stringify(matches)
  );
  return 'tests/' + matches[0];
}

function loadInventory() {
  const expected = expectedLessons();
  const expectedCodes = new Set(expected.map(x => x.lessonCode));
  const files = checkpointFiles();
  const byCode = new Map();

  for (const name of files) {
    const full = path.join(docsDir, name);
    const data = readJson(full);
    const code = data.lessonCode || data.LessonCode;
    assert.ok(expectedCodes.has(code), 'non-canonical or unexpected lessonCode in ' + name + ': ' + code);
    assert.ok(!byCode.has(code), 'duplicate Stage C checkpoint for ' + code);

    const match = /:U(\d{2}):L(\d{2})$/.exec(code);
    assert.ok(match, 'invalid lesson code ' + code);
    const unit = Number(match[1]);
    const lesson = Number(match[2]);
    const expectedRow = expected.find(x => x.lessonCode === code);
    assert.ok(expectedRow, 'missing expected row for ' + code);

    const statusText = String(data.status || data.state || '');
    const ready =
      data.READY_FOR_STAGE_D === true ||
      data.readyForStageD === true ||
      statusText.includes('READY_FOR_STAGE_D');
    assert.equal(ready, true, 'A-B-C handoff is not READY_FOR_STAGE_D: ' + code);

    const dStatus = data.D ?? data.stageD ?? data.D_STATUS;
    assert.ok(
      dStatus === undefined || dStatus === null || dStatus === 'NOT_RUN',
      'stale/non-entry Stage D status in A-B-C checkpoint: ' + code + ' = ' + dStatus
    );

    const testExecution = data.testsExecuted ?? data.tests ?? data.testExecution;
    assert.ok(
      testExecution === undefined ||
        testExecution === null ||
        ['NOT_RUN', 'AUTHORED_NOT_RUN', 'TEST_AUTHORED_NOT_RUN', 'WRITTEN_NOT_RUN'].includes(testExecution),
      'unexpected pre-D test state: ' + code + ' = ' + testExecution
    );

    const testPath = resolveTestPath(data, name, unit, lesson);
    const assessmentPath =
      data.studentPaper?.path ||
      data.paperPath ||
      (typeof data.worksheetUrl === 'string' && data.worksheetUrl.startsWith('/curriculum/')
        ? 'src/Edulytics.Web/wwwroot' + data.worksheetUrl
        : null);

    if (assessmentPath) {
      const normalizedAsset = slash(assessmentPath);
      const assetFullPath = path.join(root, normalizedAsset);
      assert.ok(
        fs.existsSync(assetFullPath),
        'checkpoint references missing first-party assessment asset: ' + normalizedAsset
      );
      if (data.studentPaper?.sha256) {
        const actualSha256 = crypto
          .createHash('sha256')
          .update(fs.readFileSync(assetFullPath))
          .digest('hex');
        assert.equal(
          actualSha256,
          data.studentPaper.sha256,
          'checkpoint assessment SHA-256 drift: ' + code
        );
      }
    }

    byCode.set(code, {
      ordinal: expectedRow.ordinal,
      unit,
      lesson,
      lessonCode: code,
      checkpoint: 'docs/curriculum/' + name,
      testPath,
      assessmentPath: assessmentPath ? slash(assessmentPath) : null,
      handoffStatus: data.status || data.state || 'READY_FOR_STAGE_D',
      ownerAcademicApproval:
        data.ownerAcademicApproval === true ||
        data.ownerApprovedAcademicPublication === true ||
        data.ownerAcademicApproval === 'true',
      preDTestState: testExecution ?? 'UNRECORDED_NOT_RUN',
    });
  }

  assert.equal(byCode.size, 131, 'checkpoint lessonCode coverage must be 131/131');
  const rows = expected.map(x => byCode.get(x.lessonCode));
  assert.ok(rows.every(Boolean), 'one or more official lessons are missing Stage C handoff evidence');

  const blueprint = readJson(blueprintPath);
  assert.equal(blueprint.Lessons.length, 131, 'Grade 8 blueprint must contain exactly 131 lessons');
  assert.equal(
    new Set(blueprint.Lessons.map(x => x.LessonCode)).size,
    131,
    'Grade 8 blueprint contains duplicate lesson codes'
  );
  const blueprintCodes = new Set(blueprint.Lessons.map(x => x.LessonCode));
  assert.deepEqual(
    [...blueprintCodes].sort(),
    expected.map(x => x.lessonCode).sort(),
    'Grade 8 blueprint lesson identity differs from official 131-lesson scope'
  );

  const pack = readJson(contentPackPath);
  assert.ok(Array.isArray(pack.lessons), 'Grade 8 content pack has no lessons[]');
  assert.equal(pack.lessons.length, 131, 'Grade 8 content pack must contain exactly 131 lessons');
  const packCodes = pack.lessons.map(x => x.lessonCode || x.LessonCode);
  assert.equal(new Set(packCodes).size, 131, 'Grade 8 content pack contains duplicate lesson codes');
  assert.deepEqual(
    [...packCodes].sort(),
    expected.map(x => x.lessonCode).sort(),
    'Grade 8 content pack lesson identity differs from official 131-lesson scope'
  );

  const closure = readJson(closurePath);
  assert.equal(closure.courseLessons, 131, 'A-B-C closure report courseLessons mismatch');
  assert.equal(closure.totalReadyForStageD, 131, 'A-B-C closure report not 131/131 ready');
  assert.equal(closure.remaining, 0, 'A-B-C closure report still has remaining lessons');
  assert.equal(
    closure.state,
    'ABC_COMPLETE_ALL_LESSONS_READY_FOR_STAGE_D',
    'A-B-C closure state mismatch'
  );

  return rows;
}

function shard(rows, batch) {
  assert.ok(Object.hasOwn(bounds, batch), 'unknown batch ' + batch);
  const [start, end] = bounds[batch];
  const selected = rows.filter(x => x.ordinal >= start && x.ordinal <= end);
  assert.equal(selected.length, end - start + 1, 'batch cardinality mismatch: ' + batch);
  return selected;
}

const rows = loadInventory();
const command = process.argv[2] || 'verify';

if (command === 'verify') {
  console.log('G8 Stage D inventory PASS: 131/131 official A-B-C handoffs, blueprint, content pack and lesson tests mapped.');
  console.log('No Stage D runtime, authenticated E2E, visual/accessibility, rights or release approval is implied by inventory PASS.');
} else if (command === 'inventory') {
  process.stdout.write(JSON.stringify({
    schemaVersion: 1,
    course: 'US-CCSS-MATH:G8:OUR-IM-2017',
    lessonCount: 131,
    unitCount: 9,
    batches: bounds,
    rows,
  }, null, 2) + '\n');
} else if (command === 'shard') {
  const batch = process.argv[3];
  process.stdout.write(shard(rows, batch).map(x => x.testPath).join('\n') + '\n');
} else if (command === 'shard-json') {
  const batch = process.argv[3];
  process.stdout.write(JSON.stringify(shard(rows, batch), null, 2) + '\n');
} else {
  throw new Error('unknown command: ' + command);
}
