(function () {
  'use strict';

  var root = document.querySelector('[data-lesson-grounded-game]');
  if (!root) return;

  var mechanic = root.dataset.mechanic || '';
  var locale = root.dataset.lessonLanguage || 'en';
  var lessonTitle = root.dataset.lessonTitle || '';
  var unitTitle = root.dataset.unitTitle || '';
  var supported = ['TWO_UNKNOWNS', 'SCALE_READING', 'FRACTION_COMPARE_UNLIKE', 'FRACTION_EQUIVALENT'];
  if (supported.indexOf(mechanic) < 0) {
    root.innerHTML = '<div class="gw-error">This lesson does not have an exact skill practice model.</div>';
    return;
  }

  var langIndex = locale === 'pl' ? 1 : (locale === 'ar' ? 2 : 0);
  var guide = '/images/game/v9/eddy-hint.webp';
  var state = { round: 0, count: 8, score: 0, locked: false };

  function t(en, pl, ar) { return [en, pl, ar][langIndex]; }
  function esc(value) {
    return String(value).replace(/[&<>'\"]/g, function (c) {
      return {'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','\"':'&quot;'}[c];
    });
  }
  function rnd(min, max) { return Math.floor(Math.random() * (max - min + 1)) + min; }
  function shuffle(values) {
    var a = values.slice();
    for (var i = a.length - 1; i > 0; i -= 1) {
      var j = Math.floor(Math.random() * (i + 1));
      var temp = a[i]; a[i] = a[j]; a[j] = temp;
    }
    return a;
  }
  function unique(values) {
    return values.filter(function (value, index, array) { return array.indexOf(value) === index; });
  }

  root.innerHTML = '<section class="gw-game gw-universal" data-exact-lesson-game>' +
    '<header class="gw-hud"><div class="gw-brand"><span class="gw-brand-mark">◆</span><span><strong>' + esc(t('EXACT-SKILL PRACTICE', 'ĆWICZENIE DOKŁADNEJ UMIEJĘTNOŚCI', 'تدريب المهارة المحددة')) + '</strong><small>' + esc(unitTitle) + '</small></span></div>' +
    '<div class="gw-hud-right"><div class="gw-pill">★ <span data-score>0</span></div><div class="gw-pill"><span data-round>1 / 8</span></div></div></header>' +
    '<div class="gw-progress"><span data-progress></span></div>' +
    '<main class="gw-stage"><div class="gw-mission"><span>' + esc(t('YOUR MISSION', 'TWOJE ZADANIE', 'مهمتك')) + '</span><strong data-question></strong><small data-subquestion></small></div><div class="gw-runtime-board" data-board></div></main>' +
    '<div class="gw-eddy"><img src="' + guide + '" alt="Eddy"><div><span>EDDY</span><p data-eddy></p></div></div>' +
    '<div class="gw-complete" data-complete hidden><div class="gw-complete-card"><img src="' + guide + '" alt="Eddy"><p>' + esc(lessonTitle) + '</p><h2>' + esc(t('Lesson practice complete!', 'Ćwiczenie ukończone!', 'اكتمل تدريب الدرس!')) + '</h2><strong data-final></strong><br><button class="gw-primary" type="button" data-replay>' + esc(t('Practice again', 'Ćwicz ponownie', 'تدرّب مرة أخرى')) + '</button></div></div>' +
    '</section>';

  var board = root.querySelector('[data-board]');
  var question = root.querySelector('[data-question]');
  var sub = root.querySelector('[data-subquestion]');
  var eddy = root.querySelector('[data-eddy]');
  var scoreNode = root.querySelector('[data-score]');
  var roundNode = root.querySelector('[data-round]');
  var progress = root.querySelector('[data-progress]');
  var complete = root.querySelector('[data-complete]');
  var finalNode = root.querySelector('[data-final]');

  function hud() {
    roundNode.textContent = (state.round + 1) + ' / ' + state.count;
    scoreNode.textContent = state.score;
    progress.style.width = ((state.round / state.count) * 100) + '%';
  }

  function mission(main, detail, guidance) {
    question.textContent = main;
    sub.textContent = detail || '';
    eddy.textContent = guidance || '';
  }

  function success(button, explanation) {
    state.locked = true;
    state.score += 200;
    if (button) button.classList.add('is-correct');
    eddy.textContent = t('Correct. ', 'Dobrze. ', 'صحيح. ') + explanation;
    hud();
    window.setTimeout(next, 750);
  }

  function failure(button, hint) {
    if (button) button.classList.add('is-wrong');
    eddy.textContent = t('Try again. ', 'Spróbuj ponownie. ', 'حاول مرة أخرى. ') + hint;
    window.setTimeout(function () {
      if (button) button.classList.remove('is-wrong');
    }, 500);
  }

  function next() {
    state.round += 1;
    if (state.round >= state.count) {
      progress.style.width = '100%';
      finalNode.textContent = t('Stars: ', 'Gwiazdki: ', 'النجوم: ') + state.score;
      complete.hidden = false;
      return;
    }
    render();
  }

  function modelCard(html) {
    var card = document.createElement('div');
    card.className = 'gw-operation-model';
    card.innerHTML = html;
    board.appendChild(card);
  }

  function choiceRow(items, correct, explanation, hint) {
    var row = document.createElement('div');
    row.className = 'gw-choice-row gw-runtime-choices';
    shuffle(unique(items)).forEach(function (item) {
      var button = document.createElement('button');
      button.type = 'button';
      button.className = 'gw-choice';
      button.textContent = String(item);
      button.dataset.answer = String(item);
      row.appendChild(button);
    });
    row.addEventListener('click', function (event) {
      if (state.locked) return;
      var button = event.target.closest('[data-answer]');
      if (!button) return;
      if (String(button.dataset.answer) === String(correct)) success(button, explanation);
      else failure(button, hint);
    });
    board.appendChild(row);
  }

  // Every TWO_UNKNOWNS round requires two linked unknown quantities and two
  // independent relationships. Direct arithmetic and one-unknown equations are
  // deliberately excluded from this mechanic.
  function twoUnknownsQuestion() {
    var smaller = rnd(8, 34);
    var difference = rnd(3, 16);
    var larger = smaller + difference;
    var total = smaller + larger;
    var correct = 'x=' + smaller + ', y=' + larger;
    var wrong1 = 'x=' + (smaller + 1) + ', y=' + (larger - 1);
    var wrong2 = 'x=' + smaller + ', y=' + (larger + difference);
    var wrong3 = 'x=' + difference + ', y=' + (total - difference);
    var mode = state.round % 4;

    if (mode === 0) {
      mission(
        'Two unknowns satisfy x + y = ' + total + ' and y − x = ' + difference + '. Find x and y.',
        'Both relationships must be true at the same time.',
        'Use the total and the difference together; checking only one equation is not enough.');
    } else if (mode === 1) {
      mission(
        'Two boxes contain ' + total + ' counters altogether. Box B has ' + difference + ' more counters than Box A. How many are in each box?',
        'Let x be Box A and y be Box B.',
        'Model the situation with x + y = ' + total + ' and y = x + ' + difference + '.');
    } else if (mode === 2) {
      mission(
        'Choose the pair that solves both relationships: x + y = ' + total + ' and y = x + ' + difference + '.',
        'A valid pair must satisfy both equations.',
        'Substitute each proposed pair into both relationships.');
    } else {
      mission(
        'A student says x=' + (smaller + 1) + ' and y=' + (larger - 1) + ' solve x + y = ' + total + ' and y − x = ' + difference + '. Which pair is actually correct?',
        'The total alone cannot prove the answer.',
        'Check the total and the difference independently.');
    }

    modelCard('<div class="gw-equation">x + y = ' + total + '</div><div class="gw-equation">y − x = ' + difference + '</div>');
    choiceRow(
      [correct, wrong1, wrong2, wrong3],
      correct,
      'The pair gives the required total and the required difference.',
      'Test the pair in both equations, not just one.');
  }

  function scaleModel(start, step, intervals, pointer) {
    var wrap = document.createElement('div');
    wrap.className = 'gw-operation-model';
    wrap.style.padding = '1rem';
    var line = document.createElement('div');
    line.style.display = 'grid';
    line.style.gridTemplateColumns = 'repeat(' + (intervals + 1) + ', 1fr)';
    line.style.alignItems = 'end';
    line.style.borderBottom = '3px solid currentColor';
    line.style.gap = '0';
    for (var i = 0; i <= intervals; i += 1) {
      var tick = document.createElement('div');
      tick.style.textAlign = 'center';
      tick.style.minWidth = '0';
      tick.innerHTML = '<div style="font-size:1.25rem;min-height:1.5rem">' + (i === pointer ? '▼' : '') + '</div>' +
        '<div style="border-left:2px solid currentColor;height:14px;margin:0 auto;width:0"></div>' +
        '<small>' + (i === 0 || i === intervals ? (start + i * step) : '•') + '</small>';
      line.appendChild(tick);
    }
    wrap.appendChild(line);
    board.appendChild(wrap);
  }

  // The scale drawing and the answer are built from the same interval data.
  function scaleReadingQuestion() {
    var intervalOptions = [2, 4, 5, 10];
    var intervals = intervalOptions[state.round % intervalOptions.length];
    var step = rnd(2, 9);
    var start = rnd(0, 6) * step;
    var end = start + intervals * step;
    var pointer = intervals === 2 ? 1 : rnd(1, intervals - 1);
    var pointerValue = start + pointer * step;
    var mode = state.round % 3;

    if (mode === 1) {
      mission(
        'A scale runs from ' + start + ' to ' + end + ' in ' + intervals + ' equal intervals. What is the value of one interval?',
        'Equal intervals divide the total change evenly.',
        'Calculate (' + end + ' − ' + start + ') ÷ ' + intervals + '.');
      scaleModel(start, step, intervals, -1);
      choiceRow(
        [step, step + 1, Math.max(1, step - 1), intervals],
        step,
        'Each interval is (' + end + ' − ' + start + ') ÷ ' + intervals + ' = ' + step + '.',
        'Find the total change, then divide by the number of equal intervals.');
      return;
    }

    mission(
      'Read the marked value on this scale.',
      'The scale has ' + intervals + ' equal intervals from ' + start + ' to ' + end + '.',
      'First find the interval value, then count from the start to the pointer.');
    scaleModel(start, step, intervals, pointer);
    choiceRow(
      [pointerValue, pointerValue + step, Math.max(start, pointerValue - step), start + pointer],
      pointerValue,
      'One interval is ' + step + '; after ' + pointer + ' intervals the value is ' + pointerValue + '.',
      'Do not use the number of tick marks as the interval value.');
  }

  function gcd(a, b) {
    while (b) { var temp = a % b; a = b; b = temp; }
    return Math.abs(a);
  }
  function lcm(a, b) { return Math.abs(a * b) / gcd(a, b); }

  function fractionBars(n1, d1, n2, d2) {
    function makeBar(n, d) {
      var bar = document.createElement('div');
      bar.style.display = 'grid';
      bar.style.gridTemplateColumns = 'repeat(' + d + ', 1fr)';
      bar.style.width = '100%';
      bar.style.maxWidth = '520px';
      bar.style.margin = '.5rem auto';
      for (var i = 0; i < d; i += 1) {
        var cell = document.createElement('span');
        cell.style.border = '1px solid currentColor';
        cell.style.height = '30px';
        cell.style.opacity = i < n ? '1' : '.25';
        cell.textContent = i < n ? '■' : '';
        cell.style.textAlign = 'center';
        bar.appendChild(cell);
      }
      return bar;
    }
    var card = document.createElement('div');
    card.className = 'gw-operation-model';
    card.appendChild(document.createTextNode(n1 + '/' + d1));
    card.appendChild(makeBar(n1, d1));
    card.appendChild(document.createTextNode(n2 + '/' + d2));
    card.appendChild(makeBar(n2, d2));
    board.appendChild(card);
  }

  function unlikeFractionPair(requireEqual) {
    var denominators = [3, 4, 5, 6, 8, 10, 12];
    if (requireEqual) {
      var baseD = [3, 4, 5][rnd(0, 2)];
      var baseN = rnd(1, baseD - 1);
      var factor = rnd(2, 3);
      return { n1: baseN, d1: baseD, n2: baseN * factor, d2: baseD * factor };
    }

    for (var attempt = 0; attempt < 50; attempt += 1) {
      var d1 = denominators[rnd(0, denominators.length - 1)];
      var d2 = denominators[rnd(0, denominators.length - 1)];
      if (d1 === d2) continue;
      var n1 = rnd(1, d1 - 1);
      var n2 = rnd(1, d2 - 1);
      if (n1 * d2 !== n2 * d1) return { n1: n1, d1: d1, n2: n2, d2: d2 };
    }
    return { n1: 2, d1: 3, n2: 3, d2: 4 };
  }

  // Every generated item here compares fractions with unlike denominators.
  function fractionCompareQuestion() {
    var requireEqual = state.round === 2 || state.round === 6;
    var pair = unlikeFractionPair(requireEqual);
    var left = pair.n1 + '/' + pair.d1;
    var right = pair.n2 + '/' + pair.d2;
    var crossLeft = pair.n1 * pair.d2;
    var crossRight = pair.n2 * pair.d1;
    var sign = crossLeft === crossRight ? '=' : (crossLeft > crossRight ? '>' : '<');
    var common = lcm(pair.d1, pair.d2);
    var eqLeft = pair.n1 * (common / pair.d1);
    var eqRight = pair.n2 * (common / pair.d2);
    var mode = state.round % 4;

    if (mode === 1 && sign !== '=') {
      var greater = sign === '>' ? left : right;
      mission(
        'Which fraction is greater: ' + left + ' or ' + right + '?',
        'The denominators are different.',
        'Compare equivalent values, not the denominator digits by themselves.');
      fractionBars(pair.n1, pair.d1, pair.n2, pair.d2);
      choiceRow(
        [left, right, 'They are equal'],
        greater,
        'Using denominator ' + common + ': ' + left + ' = ' + eqLeft + '/' + common + ' and ' + right + ' = ' + eqRight + '/' + common + '.',
        'Rewrite both fractions with a common denominator or compare cross-products.');
      return;
    }

    if (mode === 3) {
      mission(
        'A student compares ' + left + ' and ' + right + ' by looking only at the denominators. Which symbol is actually correct?',
        left + '  ?  ' + right,
        'A larger denominator does not automatically mean a larger fraction.');
    } else {
      mission(
        'Choose the correct comparison: ' + left + '  ?  ' + right,
        'The fractions have different denominators.',
        'Use equivalence or a common denominator without changing either value.');
    }
    fractionBars(pair.n1, pair.d1, pair.n2, pair.d2);
    choiceRow(
      ['<', '>', '='],
      sign,
      'With common denominator ' + common + ', compare ' + eqLeft + '/' + common + ' and ' + eqRight + '/' + common + '.',
      'Compare the values of the whole fractions, not isolated numerator or denominator digits.');
  }

  // Every FRACTION_EQUIVALENT round preserves value by multiplying or
  // dividing numerator and denominator by the same non-zero whole-number factor.
  // Distractors are rejected by cross-product equivalence before rendering.
  function fractionEquivalentQuestion() {
    var baseD = rnd(3, 9);
    var baseN = rnd(1, baseD - 1);
    var factor = rnd(2, 4);
    var eqN = baseN * factor;
    var eqD = baseD * factor;
    var base = baseN + '/' + baseD;
    var equivalent = eqN + '/' + eqD;
    var mode = state.round % 4;

    function isEquivalent(n, d) {
      return d !== 0 && baseN * d === n * baseD;
    }

    function fractionDistractors() {
      var candidates = [
        [eqN + 1, eqD],
        [eqN, eqD + factor],
        [baseN, eqD],
        [baseN + factor, baseD * factor],
        [Math.max(1, eqN - 1), eqD]
      ];
      var values = [];
      candidates.forEach(function (pair) {
        if (!isEquivalent(pair[0], pair[1])) values.push(pair[0] + '/' + pair[1]);
      });
      return unique(values);
    }

    if (mode === 1) {
      mission(
        'Complete the equivalent fraction: ' + base + ' = ?/' + eqD,
        'The denominator was multiplied by ' + factor + '.',
        'Multiply the numerator by the same factor.');
      fractionBars(baseN, baseD, eqN, eqD);
      choiceRow(
        [eqN, eqN + 1, baseN, Math.max(1, eqN - factor)],
        eqN,
        'Both numerator and denominator are multiplied by ' + factor + ', so the value stays the same.',
        'Whatever happens to the denominator must also happen to the numerator.');
      return;
    }

    if (mode === 2) {
      mission(
        'Complete the equivalent fraction: ' + base + ' = ' + eqN + '/?',
        'The numerator was multiplied by ' + factor + '.',
        'Multiply the denominator by the same factor.');
      fractionBars(baseN, baseD, eqN, eqD);
      choiceRow(
        [eqD, eqD + 1, baseD, Math.max(1, eqD - factor)],
        eqD,
        'Both parts are scaled by the same factor, giving ' + equivalent + '.',
        'Use the same scale factor on numerator and denominator.');
      return;
    }

    mission(
      mode === 3
        ? 'Which fraction has exactly the same value as ' + base + '?'
        : 'Choose an equivalent fraction for ' + base + '.',
      'Equivalent fractions name the same value.',
      'Multiply numerator and denominator by the same whole-number factor.');
    fractionBars(baseN, baseD, eqN, eqD);
    var distractors = fractionDistractors();
    choiceRow(
      [equivalent].concat(distractors.slice(0, 3)),
      equivalent,
      base + ' × ' + factor + '/' + factor + ' = ' + equivalent + ', so the value is unchanged.',
      'Check equivalence with cross-products or scale both numerator and denominator by the same factor.');
  }

  function render() {
    state.locked = false;
    board.innerHTML = '';
    hud();
    if (mechanic === 'TWO_UNKNOWNS') twoUnknownsQuestion();
    else if (mechanic === 'SCALE_READING') scaleReadingQuestion();
    else if (mechanic === 'FRACTION_EQUIVALENT') fractionEquivalentQuestion();
    else fractionCompareQuestion();
  }

  root.querySelector('[data-replay]').addEventListener('click', function () {
    state.round = 0;
    state.score = 0;
    complete.hidden = true;
    render();
  });

  render();
}());
