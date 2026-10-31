'use strict';

// Grade 6 U01:L01 browser-only learning visualization. It is NOT the source
// of truth for selected Practice, not the PR #607 geometry kernel, and does
// not score/persist learner work. Free constructions require teacher review.
(() => {
  const kinds = Object.freeze({
    square: { label: 'Blue square · 1 unit', pieceArea: 1, cover: 1, squares: 1, triangles: 0 },
    pair: { label: 'Two yellow half-triangles · 1 unit', pieceArea: 1, cover: 1, squares: 0, triangles: 2 },
    half: { label: 'One yellow half-triangle · ½ unit', pieceArea: 0.5, cover: 0.5, squares: 0, triangles: 1 },
    gap: { label: 'Empty · 0 units', pieceArea: 0, cover: 0, squares: 0, triangles: 0 },
    overlap: { label: 'Overlapping blue square + yellow half-triangle', pieceArea: 1.5, cover: 1, squares: 1, triangles: 1 }
  });
  const presets = Object.freeze({
    a: ['square', 'square', 'square', 'square'],
    b: ['pair', 'pair', 'pair', 'pair'],
    v: ['square', 'pair', 'square', 'pair'],
    h: ['square', 'square', 'pair', 'pair'],
    gap: ['square', 'pair', 'square', 'gap'],
    trick: ['square', 'pair', 'half', 'overlap'] // piece-area sum 4, but gap AND overlap
  });
  const names = ['Top-left', 'Top-right', 'Bottom-left', 'Bottom-right'];
  const mosaic = document.getElementById('mosaic');
  const editors = document.getElementById('editors');
  if (!mosaic || !editors) return;
  let selection = [...presets.v];
  const selects = [];

  function makeElement(tag, className, text) {
    const e = document.createElement(tag);
    if (className) e.className = className;
    if (text !== undefined) e.textContent = text;
    return e;
  }
  function paintCell(name, kindId) {
    const cell = makeElement('div', 'mosaic-cell');
    cell.dataset.kind = kindId;
    cell.append(makeElement('span', 'cell-tag', name));
    const tile = (cls) => cell.append(makeElement('div', cls));
    if (kindId === 'square' || kindId === 'overlap') tile('blue-square');
    if (kindId === 'pair' || kindId === 'half' || kindId === 'overlap') tile('triangle triangle-a');
    if (kindId === 'pair') tile('triangle triangle-b');
    return cell;
  }
  names.forEach((name, i) => {
    const id = 'cell-' + i;
    const wrapper = makeElement('div');
    const label = makeElement('label', '', name + ' cell');
    label.htmlFor = id;
    const select = makeElement('select');
    select.id = id;
    select.dataset.cell = String(i);
    for (const [key, item] of Object.entries(kinds)) {
      const opt = document.createElement('option');
      opt.value = key;
      opt.textContent = item.label;
      select.append(opt);
    }
    select.addEventListener('change', () => {
      selection[i] = select.value;
      document.querySelectorAll('[data-preset]').forEach(b => b.setAttribute('aria-pressed', 'false'));
      render();
    });
    wrapper.append(label, select);
    editors.append(wrapper);
    selects.push(select);
  });

  const fmt = n => Number.isInteger(n) ? String(n) : n.toFixed(1);
  const output = (id, value) => { document.getElementById(id).textContent = String(value); };
  function render() {
    const pieces = selection.map(s => kinds[s]);
    let squareCount = 0, triangleCount = 0, pieceArea = 0, covered = 0;
    for (const x of pieces) {
      squareCount += x.squares;
      triangleCount += x.triangles;
      pieceArea += x.pieceArea;
      covered += x.cover;
    }
    const gaps = 4 - covered;
    const overlaps = pieceArea - covered;
    mosaic.replaceChildren(...selection.map((kind, i) => paintCell(names[i], kind)));
    mosaic.setAttribute('aria-label', names.map((n, i) => n + ': ' + kinds[selection[i]].label).join('; '));
    selects.forEach((s, i) => { s.value = selection[i]; });
    output('square-count', squareCount + ' pieces');
    output('triangle-count', triangleCount + ' pieces');
    output('area-sum', fmt(pieceArea) + ' square units');
    output('covered-area', fmt(covered) + ' of 4 square units');
    output('gap-area', fmt(gaps) + ' square units');
    output('overlap-area', fmt(overlaps) + ' square units');

    const status = document.getElementById('coverage-status');
    const complete = gaps === 0 && overlaps === 0;
    status.className = 'status ' + (complete ? 'valid' : 'invalid');
    status.textContent = complete
      ? 'This constrained cell arrangement covers the 2 × 2 target without a gap or overlap.'
      : 'Not a complete tiling: ' +
        (gaps ? 'uncovered area ' + fmt(gaps) + '; ' : '') +
        (overlaps ? 'overlapping area ' + fmt(overlaps) + '; ' : '') +
        'a piece-area sum alone cannot prove coverage.';
    const requiredPieces = squareCount === 2 && triangleCount === 4;
    output('inventory-status', requiredPieces
      ? 'Required challenge inventory: 2 squares + 4 triangles. Teacher must still assess the construction and explanation.'
      : 'This example does not use the challenge inventory of 2 squares + 4 triangles. That does not automatically invalidate the separate A/B comparison task.');
  }
  function setPreset(id) {
    const values = presets[id];
    if (!values) return;
    selection = [...values];
    document.querySelectorAll('[data-preset]').forEach(b =>
      b.setAttribute('aria-pressed', b.dataset.preset === id ? 'true' : 'false'));
    render();
  }
  document.querySelectorAll('[data-preset]').forEach(button =>
    button.addEventListener('click', () => setPreset(button.dataset.preset)));
  document.querySelectorAll('.rubric').forEach(input =>
    input.addEventListener('change', () => {
      const scores = [...document.querySelectorAll('.rubric')].map(s => s.value);
      const entered = scores.filter(s => s !== '');
      output('review-status', entered.length === 0
        ? 'Not reviewed'
        : entered.length !== scores.length
          ? 'Human review incomplete (' + entered.length + ' of 4 criteria). No automatic score.'
          : 'Teacher-entered rubric: ' + entered.map(Number).reduce((a, b) => a + b, 0) +
            ' of 8 points. Manually assigned, not submitted or persisted.');
    }));
  document.getElementById('print-work').addEventListener('click', () => window.print());
  setPreset('v');
})();
