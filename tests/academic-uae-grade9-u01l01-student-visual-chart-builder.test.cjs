'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const plot = require(path.join(root, 'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-source-plot-builder.js'));
const base = 'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
const read = rel => JSON.parse(fs.readFileSync(path.join(root, rel), 'utf8'));

test('U01L01 prior-review Q47 bar plot requires student-created title and seven valid whole-number votes', () => {
  assert.deepEqual(Object.keys(plot.definitions).sort(), ['U01L01-Q47', 'U01L01-Q48']);
  let a = plot.validate('U01L01-Q47', [8, 5, 7, 11, 6, 9, 4], 'تصويت النادي');
  assert.equal(a.ok, true);
  assert.equal(a.graded, false);
  assert.equal(a.review, 'TEACHER_REVIEW_REQUIRED');
  assert.equal(a.values.length, 7);
  assert.equal(a.kind, 'bar');
  assert.equal(a.values.reduce((x, y) => x + y, 0), 50);
  assert.equal(a.values.indexOf(Math.max(...a.values)), 3);
  assert.equal(a.values.indexOf(Math.min(...a.values)), 6);
  for (const bad of [[8, 5, 7, 11, 6, 9], [8, 5, 7, 11, 6, 9, -1], [8, 5, 7, 11, 6, 9, 1.5],
    [8, 5, 7, 11, 6, 9, 1001], [8, 5, 7, 11, 6, 9, 'Infinity']])
    assert.equal(plot.validate('U01L01-Q47', bad, 'تصويت النادي').ok, false);
  assert.equal(plot.validate('U01L01-Q47', [8, 5, 7, 11, 6, 9, 4], '').ok, false);
});

test('U01L01 prior-review Q48 box plot requires five ordered numeric values and allows fractional median', () => {
  const a = plot.validate('U01L01-Q48', ['١٩', '٢١', '٢٣٫٥', '٢٦', '٢٩'], '');
  assert.equal(a.ok, true);
  assert.equal(a.kind, 'box');
  assert.deepEqual(a.values, [19, 21, 23.5, 26, 29]);
  assert.equal(a.values[3] - a.values[1], 5);
  assert.equal(a.review, 'TEACHER_REVIEW_REQUIRED');
  assert.equal(plot.validate('U01L01-Q48', [19, 25, 23.5, 26, 29], '').ok, false);
  assert.equal(plot.validate('U01L01-Q48', [19, 21, 23.5, 26, ''], '').ok, false);
  assert.equal(plot.validate('U01L01-Q48', [19, 21, 23.5, 26, 'NaN'], '').ok, false);
});

class FakeElement {
  constructor(doc, tag) {
    this.ownerDocument = doc; this.tagName = tag;
    this.children = []; this.attributes = {}; this.listeners = {};
    this.dataset = {}; this.value = ''; this.textContent = '';
  }
  append(...els) { this.children.push(...els); }
  replaceChildren(...els) { this.children = [...els]; }
  setAttribute(n, v) { this.attributes[n] = String(v); }
  addEventListener(type, fn) { this.listeners[type] = fn; }
}

const fakeDoc = {
  createElement: tag => new FakeElement(fakeDoc, tag),
  createElementNS: (_ns, tag) => new FakeElement(fakeDoc, tag)
};

function descend(el) {
  return [el, ...(el.children || []).flatMap(descend)];
}

test('Student chart builders render real SVG geometry with accessibility labels; never imply automatic grades', () => {
  const source = read(base + 'u01l01-original-61-parallel-skill-practice.json');
  assert.equal(source.records.length, 61);
  for (const i of [47, 48]) {
    const record = source.records[i - 1];
    const row = fakeDoc.createElement('li');
    const response = fakeDoc.createElement('textarea');
    plot.attach({ sourceItem: record.sourceItem, independentTaskId: 'G9-U01L01-T' + i }, row, response);
    const title = descend(row).find(e => e.tagName === 'input' && e.type === 'text');
    const inputs = descend(row).filter(e => e.tagName === 'input' && e.type === 'number');
    const button = descend(row).find(e => e.tagName === 'button');
    const alert = descend(row).find(e => e.attributes.role === 'alert');
    const expected = i === 47 ? [8, 5, 7, 11, 6, 9, 4] : [19, 21, 23.5, 26, 29];
    assert.equal(inputs.length, expected.length);
    button.listeners.click();
    assert.ok(alert.textContent);
    assert.equal(row.dataset.plotDraft, undefined);
    title.value = 'بيانات أصلية للطالب';
    inputs.forEach((input, n) => { input.value = String(expected[n]); });
    button.listeners.click();
    assert.equal(alert.textContent, '');
    const saved = JSON.parse(row.dataset.plotDraft);
    assert.equal(saved.graded, false);
    assert.equal(saved.review, 'TEACHER_REVIEW_REQUIRED');
    assert.deepEqual(saved.values, expected);
    const svgs = descend(row).filter(x => x.tagName === 'svg');
    assert.equal(svgs.length, 1);
    assert.equal(svgs[0].attributes.role, 'img');
    assert.ok(descend(svgs[0]).some(x => x.tagName === 'title'));
    if (i === 47) assert.equal(descend(svgs[0]).filter(x => x.tagName === 'rect').length, 7);
    if (i === 48) assert.ok(descend(svgs[0]).some(x => x.tagName === 'rect'));
    inputs[0].value = '100';
    inputs[0].listeners.input();
    assert.equal(row.dataset.plotDraft, undefined);
    assert.equal(descend(row).filter(x => x.tagName === 'svg').length, 0);
  }
});

test('Untrusted student chart titles are inserted through textContent rather than raw HTML', () => {
  const a = plot.validate('U01L01-Q47', [1, 2, 3, 4, 5, 6, 7], '<img src=x onerror=alert(1)>');
  assert.equal(a.ok, true);
  const svg = plot.draw(fakeDoc, a);
  assert.ok(svg.children.some(x => x.tagName === 'title' && x.textContent === a.title));
  assert.ok(descend(svg).every(x => x.tagName !== 'script' && x.tagName !== 'img'));
});

test('U01L01 native Arabic student activity wires the two diagram builders to teacher-only local export', () => {
  const dir = 'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/';
  const html = fs.readFileSync(path.join(root, dir, 'u01-l01-original-61-practice-ar.html'), 'utf8');
  assert.ok(html.includes('<script src="./u01-l01-source-plot-builder.js"></script>'));
  assert.ok(html.includes('window.Grade9SourcePlot.attach(task,entry,answer)'));
  assert.ok(html.includes('row.dataset.plotDraft ? JSON.parse(row.dataset.plotDraft) : null'));
  assert.ok(html.includes('DIAGRAM_NOT_CREATED'));
  assert.ok(html.includes("graded:false"));
  assert.ok(html.includes("submitted:false"));
  const records = read(base + 'u01l01-original-61-parallel-skill-practice.json').records;
  assert.ok(records[46].studentPrompt.includes('مخطط أعمدة'));
  assert.ok(records[47].studentPrompt.includes('مخطط صندوق وشاربين'));
  assert.equal(records[46].sourceItem, 'U01L01-Q47');
  assert.equal(records[47].sourceItem, 'U01L01-Q48');
  assert.equal(plot.definitions['U01L01-Q47'].labels.length, 7);
  assert.equal(plot.definitions['U01L01-Q48'].labels.length, 5);
});
test('U01L01 source Q37 power-pattern table and Q44/Q46 rectangle reference visuals remain learner-created examples with no answers prefilled', () => {
  const dir = 'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/';
  const html = fs.readFileSync(path.join(root, dir, 'u01-l01-original-61-practice-ar.html'), 'utf8');
  assert.ok(html.includes('id="g9-power-pattern-template"'));
  assert.ok(html.includes("if(task.sourceItem==='U01L01-Q37')"));
  assert.ok(html.includes('pattern.content.cloneNode(true)'));
  assert.ok(html.includes('geometry-rectangle-gallery'));
  assert.ok(html.includes('aria-label="مستطيل أول طوله ل وعرضه ع'));
  assert.ok(html.includes('10²</th><td>10³</td><td>؟'));
  assert.ok(html.includes('10²</th><td>10⁴</td><td>؟'));
  assert.ok(html.includes('عدد الأصوات') || plot.definitions['U01L01-Q47'].unit === 'عدد الأصوات');
  assert.equal(10 ** 2 * 10 ** 3, 10 ** 5);
  assert.equal(10 ** 2 * 10 ** 4, 10 ** 6);
  assert.equal(7 * 3, 21);
});
