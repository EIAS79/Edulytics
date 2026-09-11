(function () {
  'use strict';

  var root = document.querySelector('[data-curriculum-workspace-game]');
  if (!root) return;

  var mechanic = root.dataset.mechanic || '';
  var locale = root.dataset.lessonLanguage || 'en';
  var unitTitle = root.dataset.unitTitle || '';
  var langIndex = locale === 'pl' ? 1 : (locale === 'ar' ? 2 : 0);

  function t(en, pl, ar) { return [en, pl, ar][langIndex]; }
  function rnd(min, max) { return Math.floor(Math.random() * (max - min + 1)) + min; }
  function shuffle(values) {
    var result = values.slice();
    for (var i = result.length - 1; i > 0; i--) {
      var j = Math.floor(Math.random() * (i + 1));
      var temp = result[i];
      result[i] = result[j];
      result[j] = temp;
    }
    return result;
  }
  function uniq(values) { return values.filter(function (value, index, array) { return array.indexOf(value) === index; }); }

  var copy = {
    mission: t('YOUR MISSION', 'TWOJE ZADANIE', 'مهمتك'),
    check: t('Check', 'Sprawdź', 'تحقق'),
    reset: t('Reset amount', 'Wyzeruj kwotę', 'إعادة ضبط المبلغ'),
    correct: t('Correct!', 'Dobrze!', 'صحيح!'),
    retry: t('Try again.', 'Spróbuj ponownie.', 'حاول مرة أخرى.'),
    complete: t('Mission complete!', 'Misja ukończona!', 'اكتملت المهمة!'),
    again: t('Play again', 'Zagraj ponownie', 'العب مرة أخرى'),
    sound: t('Sound', 'Dźwięk', 'الصوت'),
    fullscreen: t('Fullscreen', 'Pełny ekran', 'ملء الشاشة')
  };

  var state = { round: 0, count: 8, score: 0, locked: false, sound: true };
  var guide = '/images/game/v9/eddy-guide.webp';
  var title = mechanic === 'PERIMETER_TRACE'
    ? t('Perimeter Workshop', 'Pracownia obwodu', 'ورشة المحيط')
    : mechanic === 'AREA_TILE'
      ? t('Area Tile Lab', 'Laboratorium pola', 'مختبر المساحة')
      : t('Money Builder', 'Budowanie kwoty', 'تكوين المبلغ');

  root.innerHTML =
    '<section class="gw-game gw-universal" data-specialized-practice>' +
      '<header class="gw-hud">' +
        '<div class="gw-brand"><span class="gw-brand-mark">◆</span><span><strong>' + title.toUpperCase() + '</strong><small></small></span></div>' +
        '<div class="gw-hud-right"><div class="gw-pill">★ <span data-score>0</span></div><div class="gw-pill"><span data-round>1 / 8</span></div><button class="gw-icon" type="button" data-sound aria-label="' + copy.sound + '">🔊</button><button class="gw-icon" type="button" data-fullscreen aria-label="' + copy.fullscreen + '">⛶</button></div>' +
      '</header>' +
      '<div class="gw-progress"><span data-progress></span></div>' +
      '<main class="gw-stage"><div class="gw-mission"><span>' + copy.mission + '</span><strong data-question></strong><small data-subquestion></small></div><div class="gw-runtime-board" data-board></div></main>' +
      '<div class="gw-eddy"><img src="' + guide + '" alt="Eddy"><div><span>EDDY</span><p data-eddy></p></div></div>' +
      '<div class="gw-complete" data-complete hidden><div class="gw-complete-card"><img src="' + guide + '" alt="Eddy"><h2>' + copy.complete + '</h2><p>' + t('Practice is feedback, not an official grade.', 'Ćwiczenie daje informację zwrotną, nie jest oficjalną oceną.', 'التدريب للتغذية الراجعة وليس درجة رسمية.') + '</p><strong data-final></strong><br><button class="gw-primary" type="button" data-replay>' + copy.again + '</button></div></div>' +
    '</section>';

  root.querySelector('.gw-brand small').textContent = unitTitle;

  var game = root.querySelector('[data-specialized-practice]');
  var board = root.querySelector('[data-board]');
  var question = root.querySelector('[data-question]');
  var subquestion = root.querySelector('[data-subquestion]');
  var eddy = root.querySelector('[data-eddy]');
  var scoreNode = root.querySelector('[data-score]');
  var roundNode = root.querySelector('[data-round]');
  var progress = root.querySelector('[data-progress]');
  var complete = root.querySelector('[data-complete]');
  var finalNode = root.querySelector('[data-final]');

  function speak(text) {
    if (!state.sound || !window.speechSynthesis || !window.SpeechSynthesisUtterance) return;
    try {
      window.speechSynthesis.cancel();
      var utterance = new SpeechSynthesisUtterance(text);
      utterance.lang = locale === 'pl' ? 'pl-PL' : (locale === 'ar' ? 'ar-AE' : 'en-GB');
      utterance.rate = 0.92;
      window.speechSynthesis.speak(utterance);
    } catch (_) { }
  }

  function updateHud() {
    scoreNode.textContent = state.score;
    roundNode.textContent = (state.round + 1) + ' / ' + state.count;
    progress.style.width = ((state.round / state.count) * 100) + '%';
  }

  function setMission(main, detail, hint) {
    question.textContent = main;
    subquestion.textContent = detail || '';
    eddy.textContent = hint || '';
    speak(main);
  }

  function success(button, message) {
    if (state.locked) return;
    state.locked = true;
    state.score += 200;
    if (button) button.classList.add('is-correct');
    updateHud();
    eddy.textContent = copy.correct + ' ' + message;
    speak(eddy.textContent);
    window.setTimeout(next, 750);
  }

  function failure(button, message) {
    if (button) button.classList.add('is-wrong');
    eddy.textContent = copy.retry + ' ' + message;
    speak(eddy.textContent);
    window.setTimeout(function () {
      if (button) button.classList.remove('is-wrong');
    }, 550);
  }

  function next() {
    state.round++;
    if (state.round >= state.count) {
      progress.style.width = '100%';
      finalNode.textContent = t('Stars: ', 'Gwiazdki: ', 'النجوم: ') + state.score;
      complete.hidden = false;
      speak(copy.complete);
      return;
    }
    render();
  }

  function choiceRow(values, correct, successMessage) {
    var row = document.createElement('div');
    row.className = 'gw-choice-row gw-runtime-choices';
    shuffle(uniq(values)).forEach(function (value) {
      var button = document.createElement('button');
      button.type = 'button';
      button.className = 'gw-choice';
      button.dataset.answer = value;
      button.textContent = value;
      row.appendChild(button);
    });
    row.addEventListener('click', function (event) {
      if (state.locked) return;
      var button = event.target.closest('[data-answer]');
      if (!button) return;
      if (Number(button.dataset.answer) === correct) {
        success(button, successMessage);
      } else {
        failure(button, t('Check the measurements and calculate again.', 'Sprawdź wymiary i policz ponownie.', 'تحقق من القياسات واحسب مرة أخرى.'));
      }
    });
    board.appendChild(row);
  }

  function renderPerimeter() {
    var width = rnd(2, 10);
    var height = rnd(2, 8);
    var answer = 2 * (width + height);
    setMission(
      t('Find the perimeter of the rectangle.', 'Oblicz obwód prostokąta.', 'احسب محيط المستطيل.'),
      width + ' × ' + height,
      t('Add all four sides: 2 × (length + width).', 'Dodaj cztery boki: 2 × (długość + szerokość).', 'اجمع الأضلاع الأربعة: 2 × (الطول + العرض).'));

    var model = document.createElement('div');
    model.className = 'gw-operation-model';
    model.innerHTML = '<div style="display:grid;place-items:center;min-height:180px"><div style="position:relative;width:220px;height:120px;border:5px solid currentColor;border-radius:12px"><strong style="position:absolute;left:50%;top:-36px;transform:translateX(-50%)">' + width + '</strong><strong style="position:absolute;right:-34px;top:50%;transform:translateY(-50%)">' + height + '</strong></div></div>';
    board.appendChild(model);
    choiceRow([answer, width * height, answer + 2, Math.max(1, answer - 2)], answer, t('All four sides were included.', 'Uwzględniono wszystkie cztery boki.', 'تم احتساب الأضلاع الأربعة.'));
  }

  function renderArea() {
    var columns = rnd(2, 7);
    var rows = rnd(2, 6);
    var answer = columns * rows;
    setMission(
      t('Find the area covered by the tiles.', 'Oblicz pole pokryte kafelkami.', 'احسب المساحة المغطاة بالمربعات.'),
      columns + ' × ' + rows,
      t('Count equal square units: rows × columns.', 'Policz równe jednostki kwadratowe: wiersze × kolumny.', 'عد الوحدات المربعة المتساوية: الصفوف × الأعمدة.'));

    var grid = document.createElement('div');
    grid.className = 'gw-operation-model';
    var tiles = document.createElement('div');
    tiles.setAttribute('data-area-tiles', '');
    tiles.style.display = 'grid';
    tiles.style.gridTemplateColumns = 'repeat(' + columns + ', minmax(26px, 42px))';
    tiles.style.gap = '4px';
    tiles.style.justifyContent = 'center';
    tiles.style.padding = '18px';
    for (var i = 0; i < answer; i++) {
      var tile = document.createElement('span');
      tile.style.aspectRatio = '1';
      tile.style.border = '2px solid currentColor';
      tile.style.borderRadius = '5px';
      tiles.appendChild(tile);
    }
    grid.appendChild(tiles);
    board.appendChild(grid);
    choiceRow([answer, 2 * (columns + rows), answer + columns, Math.max(1, answer - rows)], answer, t('Rows times columns gives the area.', 'Liczba wierszy razy liczba kolumn daje pole.', 'عدد الصفوف مضروبًا في الأعمدة يعطي المساحة.'));
  }

  function renderMoney() {
    var target = shuffle([20, 30, 40, 50, 60, 75, 90])[0];
    var coins = [5, 10, 20, 25, 50];
    var total = 0;

    setMission(
      t('Make exactly ' + target + ' using the coins.', 'Ułóż dokładnie kwotę ' + target + ' z monet.', 'كوّن المبلغ ' + target + ' بالضبط باستخدام العملات.'),
      t('Tap coins, then check the total.', 'Klikaj monety, potem sprawdź sumę.', 'اضغط العملات ثم تحقق من المجموع.'),
      t('If you go over the target, reset the amount and try again.', 'Jeśli przekroczysz kwotę, wyzeruj ją i spróbuj ponownie.', 'إذا تجاوزت المبلغ المطلوب فأعد ضبط المبلغ وحاول مجددًا.'));

    var card = document.createElement('div');
    card.className = 'gw-money-card';
    var tray = document.createElement('div');
    tray.className = 'gw-coin-tray';
    var output = document.createElement('output');
    output.className = 'gw-money-sum';
    output.textContent = '0';
    output.setAttribute('aria-live', 'polite');

    coins.forEach(function (value) {
      var button = document.createElement('button');
      button.type = 'button';
      button.className = 'gw-coin';
      button.textContent = value;
      button.addEventListener('click', function () {
        if (state.locked) return;
        total += value;
        output.textContent = total;
      });
      tray.appendChild(button);
    });

    var controls = document.createElement('div');
    controls.className = 'gw-choice-row';

    var reset = document.createElement('button');
    reset.type = 'button';
    reset.className = 'gw-choice';
    reset.setAttribute('data-money-reset', '');
    reset.textContent = copy.reset;
    reset.addEventListener('click', function () {
      if (state.locked) return;
      total = 0;
      output.textContent = '0';
      eddy.textContent = t('Amount reset. Build the target again.', 'Kwota wyzerowana. Ułóż ją ponownie.', 'تمت إعادة ضبط المبلغ. كوّنه من جديد.');
    });

    var check = document.createElement('button');
    check.type = 'button';
    check.className = 'gw-primary';
    check.textContent = copy.check;
    check.addEventListener('click', function () {
      if (total === target) {
        success(check, t('Exact target amount made.', 'Ułożono dokładną kwotę.', 'تم تكوين المبلغ المطلوب بالضبط.'));
      } else {
        failure(check, total > target
          ? t('The total is too high. Reset it or build a smaller total.', 'Suma jest za duża. Wyzeruj ją i spróbuj ponownie.', 'المجموع أكبر من المطلوب. أعد ضبطه وحاول من جديد.')
          : t('The total is still below the target.', 'Suma jest jeszcze za mała.', 'المجموع ما زال أقل من المطلوب.'));
      }
    });

    controls.appendChild(reset);
    controls.appendChild(check);
    card.appendChild(tray);
    card.appendChild(output);
    card.appendChild(controls);
    board.appendChild(card);
  }

  function render() {
    state.locked = false;
    board.innerHTML = '';
    updateHud();

    if (mechanic === 'PERIMETER_TRACE') {
      renderPerimeter();
      return;
    }
    if (mechanic === 'AREA_TILE') {
      renderArea();
      return;
    }
    renderMoney();
  }

  root.querySelector('[data-sound]').addEventListener('click', function () {
    state.sound = !state.sound;
    this.textContent = state.sound ? '🔊' : '🔇';
    if (!state.sound && window.speechSynthesis) window.speechSynthesis.cancel();
  });

  root.querySelector('[data-fullscreen]').addEventListener('click', function () {
    if (!document.fullscreenElement) {
      if (game.requestFullscreen) game.requestFullscreen();
    } else if (document.exitFullscreen) {
      document.exitFullscreen();
    }
  });

  root.querySelector('[data-replay]').addEventListener('click', function () {
    state.round = 0;
    state.score = 0;
    complete.hidden = true;
    render();
  });

  render();
})();
