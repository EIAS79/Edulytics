(function () {
  'use strict';

  var root = document.querySelector('[data-lesson-grounded-game][data-stage22-server-authoritative="true"]');
  if (!root) return;

  var antiForgery = document.querySelector('[data-stage22-antiforgery] input[name="__RequestVerificationToken"]');
  if (!antiForgery) {
    root.innerHTML = '<div class="gw-error">Secure game session could not be started.</div>';
    return;
  }

  var locale = root.dataset.lessonLanguage || 'en';
  var lessonTitle = root.dataset.lessonTitle || '';
  var unitTitle = root.dataset.unitTitle || '';
  var adoptionId = root.dataset.curriculumAdoptionId || '';
  var lessonId = root.dataset.lessonId || '';
  var state = { round: 0, count: 8, score: 0, locked: false, current: null };

  function t(en, pl, ar) {
    return locale === 'pl' ? pl : (locale === 'ar' ? ar : en);
  }

  function esc(value) {
    return String(value).replace(/[&<>'"]/g, function (c) {
      return {'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','"':'&quot;'}[c];
    });
  }

  root.innerHTML =
    '<section class="gw-game gw-universal" data-stage22-exact-game>' +
      '<header class="gw-hud">' +
        '<div class="gw-brand"><span class="gw-brand-mark">◆</span><span><strong>' +
          esc(t('SERVER-VERIFIED PRACTICE', 'ĆWICZENIE WERYFIKOWANE NA SERWERZE', 'تدريب متحقق منه على الخادم')) +
        '</strong><small>' + esc(unitTitle) + '</small></span></div>' +
        '<div class="gw-hud-right"><div class="gw-pill">★ <span data-score>0</span></div><div class="gw-pill"><span data-round>1 / 8</span></div></div>' +
      '</header>' +
      '<div class="gw-progress"><span data-progress></span></div>' +
      '<main class="gw-stage">' +
        '<div class="gw-mission"><span>' + esc(t('YOUR MISSION', 'TWOJE ZADANIE', 'مهمتك')) + '</span><strong data-question></strong><small data-family></small></div>' +
        '<div class="gw-runtime-board" data-board></div>' +
      '</main>' +
      '<div class="gw-eddy"><img src="/images/game/v9/eddy-hint.webp" alt="Eddy"><div><span>EDDY</span><p data-eddy></p></div></div>' +
      '<div class="gw-complete" data-complete hidden><div class="gw-complete-card"><img src="/images/game/v9/eddy-hint.webp" alt="Eddy"><p>' +
        esc(lessonTitle) +
        '</p><h2>' + esc(t('Lesson practice complete!', 'Ćwiczenie ukończone!', 'اكتمل تدريب الدرس!')) +
        '</h2><strong data-final></strong><br><button class="gw-primary" type="button" data-replay>' +
        esc(t('Practice again', 'Ćwicz ponownie', 'تدرّب مرة أخرى')) +
        '</button></div></div>' +
    '</section>';

  var board = root.querySelector('[data-board]');
  var question = root.querySelector('[data-question]');
  var family = root.querySelector('[data-family]');
  var eddy = root.querySelector('[data-eddy]');
  var scoreNode = root.querySelector('[data-score]');
  var roundNode = root.querySelector('[data-round]');
  var progress = root.querySelector('[data-progress]');
  var complete = root.querySelector('[data-complete]');
  var finalNode = root.querySelector('[data-final]');

  function hud() {
    scoreNode.textContent = state.score;
    roundNode.textContent = (state.round + 1) + ' / ' + state.count;
    progress.style.width = ((state.round / state.count) * 100) + '%';
  }

  async function postJson(url, payload) {
    var response = await fetch(url, {
      method: 'POST',
      credentials: 'same-origin',
      headers: {
        'Content-Type': 'application/json',
        'RequestVerificationToken': antiForgery.value
      },
      body: JSON.stringify(payload)
    });

    if (!response.ok) {
      throw new Error('game-runtime-' + response.status);
    }

    return response.json();
  }

  function setBusy(message) {
    state.locked = true;
    board.innerHTML = '<div class="gw-operation-model"><strong>' + esc(message) + '</strong></div>';
  }

  function renderChoices(round) {
    board.innerHTML = '';
    var row = document.createElement('div');
    row.className = 'gw-choice-row gw-runtime-choices';

    round.choices.forEach(function (value) {
      var button = document.createElement('button');
      button.type = 'button';
      button.className = 'gw-choice';
      button.textContent = String(value);
      button.dataset.serverAnswer = String(value);
      row.appendChild(button);
    });

    row.addEventListener('click', function (event) {
      if (state.locked) return;
      var button = event.target.closest('[data-server-answer]');
      if (!button) return;
      submitAnswer(button, button.dataset.serverAnswer);
    });

    board.appendChild(row);
  }

  async function loadRound() {
    state.locked = true;
    hud();
    setBusy(t('Loading a verified problem…', 'Ładowanie zweryfikowanego zadania…', 'جارٍ تحميل مسألة متحقق منها…'));

    try {
      var round = await postJson('/student/practice/game/runtime/start', {
        curriculumAdoptionId: adoptionId,
        lessonId: lessonId,
        roundIndex: state.round
      });

      state.current = round;
      question.textContent = round.prompt;
      family.textContent = round.skillId + ' · ' + round.questionFamily;
      eddy.textContent = round.hint;
      state.locked = false;
      renderChoices(round);
    } catch (error) {
      root.innerHTML = '<div class="gw-error">' +
        esc(t(
          'The verified game runtime is unavailable. This exact game will not fall back to browser grading.',
          'Zweryfikowany moduł gry jest niedostępny. Ta gra nie przejdzie na ocenianie w przeglądarce.',
          'وقت تشغيل اللعبة المتحقق منه غير متاح. لن تعود هذه اللعبة إلى التصحيح داخل المتصفح.'
        )) +
        '</div>';
    }
  }

  async function submitAnswer(button, answer) {
    if (!state.current || state.locked) return;
    state.locked = true;
    button.disabled = true;

    try {
      var result = await postJson('/student/practice/game/runtime/answer', {
        curriculumAdoptionId: adoptionId,
        lessonId: lessonId,
        roundToken: state.current.roundToken,
        answer: answer
      });

      if (result.isCorrect) {
        button.classList.add('is-correct');
        state.score += Number(result.points || 0);
        eddy.textContent = result.feedback + (result.solution ? ' ' + result.solution : '');
        hud();
        window.setTimeout(nextRound, 800);
      } else {
        button.classList.add('is-wrong');
        eddy.textContent = result.feedback;
        window.setTimeout(function () {
          button.classList.remove('is-wrong');
          button.disabled = false;
          state.locked = false;
        }, 550);
      }
    } catch (error) {
      eddy.textContent = t(
        'Server verification failed. Your answer was not graded locally.',
        'Weryfikacja serwerowa nie powiodła się. Odpowiedź nie została oceniona lokalnie.',
        'فشل التحقق على الخادم. لم يتم تصحيح إجابتك محليًا.'
      );
      button.disabled = false;
      state.locked = false;
    }
  }

  function nextRound() {
    state.round += 1;
    state.current = null;

    if (state.round >= state.count) {
      progress.style.width = '100%';
      finalNode.textContent = t('Stars: ', 'Gwiazdki: ', 'النجوم: ') + state.score;
      complete.hidden = false;
      return;
    }

    loadRound();
  }

  root.querySelector('[data-replay]').addEventListener('click', function () {
    state.round = 0;
    state.score = 0;
    state.current = null;
    complete.hidden = true;
    loadRound();
  });

  loadRound();
}());
