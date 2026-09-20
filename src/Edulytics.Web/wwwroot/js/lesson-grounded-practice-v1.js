(function () {
  'use strict';

  var root = document.querySelector('[data-lesson-grounded-game]');
  if (!root) return;

  var mechanic = root.dataset.mechanic || '';
  var locale = root.dataset.lessonLanguage || 'en';
  var unitTitle = root.dataset.unitTitle || '';
  var lessonTitle = root.dataset.lessonTitle || '';
  var langIndex = locale === 'pl' ? 1 : (locale === 'ar' ? 2 : 0);
  var guide = '/images/public/edulaytiks-character.png?v=43';

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
      var swap = a[i]; a[i] = a[j]; a[j] = swap;
    }
    return a;
  }
  function uniq(values) {
    return values.filter(function (value, index, array) { return array.indexOf(value) === index; });
  }

  var copy = {
    brand: t('EDULYTICS PRACTICE', 'ĆWICZENIA EDULYTICS', 'تدريب EDULYTICS'),
    mission: t('YOUR MISSION', 'TWOJE ZADANIE', 'مهمتك'),
    correct: t('Correct!', 'Dobrze!', 'صحيح!'),
    retry: t('Try again.', 'Spróbuj ponownie.', 'حاول مرة أخرى.'),
    complete: t('Lesson practice complete!', 'Ćwiczenie ukończone!', 'اكتمل تدريب الدرس!'),
    again: t('Practice again', 'Ćwicz ponownie', 'تدرّب مرة أخرى'),
    sound: t('Sound', 'Dźwięk', 'الصوت'),
    fullscreen: t('Fullscreen', 'Pełny ekran', 'ملء الشاشة')
  };

  var state = { round: 0, count: 8, score: 0, locked: false, sound: true };
  var supported = mechanic === 'ADDITIVE_RATIO_RELATIONSHIPS' ||
    mechanic === 'RATIO_RELATIONSHIPS' ||
    mechanic === 'ADDITIVE_RELATIONSHIPS';

  if (!supported) {
    root.innerHTML = '<div class="gw-error">' + esc(t(
      'Practice is blocked because this lesson does not yet have a verified lesson-grounded question model.',
      'Ćwiczenie jest zablokowane, ponieważ ta lekcja nie ma jeszcze zweryfikowanego modelu pytań.',
      'تم حظر التدريب لأن هذا الدرس لا يملك بعد نموذج أسئلة موثوقًا مبنيًا على الدرس.'
    )) + '</div>';
    return;
  }

  root.innerHTML = '<section class="gw-game gw-universal" data-grounded-game>' +
    '<header class="gw-hud"><div class="gw-brand"><span class="gw-brand-mark">◆</span><span><strong>' + esc(copy.brand) + '</strong><small>' + esc(unitTitle) + '</small></span></div>' +
    '<div class="gw-hud-right"><div class="gw-pill">★ <span data-score>0</span></div><div class="gw-pill"><span data-round>1 / 8</span></div><button class="gw-icon" type="button" data-sound aria-label="' + esc(copy.sound) + '">🔊</button><button class="gw-icon" type="button" data-fullscreen aria-label="' + esc(copy.fullscreen) + '">⛶</button></div></header>' +
    '<div class="gw-progress"><span data-progress></span></div>' +
    '<main class="gw-stage"><div class="gw-mission"><span>' + esc(copy.mission) + '</span><strong data-question></strong><small data-subquestion></small></div><div class="gw-runtime-board" data-board></div></main>' +
    '<div class="gw-eddy"><img src="' + guide + '" alt="Edulytics character"><div><span>EDULYTICS</span><p data-eddy></p></div></div>' +
    '<div class="gw-complete" data-complete hidden><div class="gw-complete-card"><img src="' + guide + '" alt="Edulytics character"><p>' + esc(lessonTitle) + '</p><h2>' + esc(copy.complete) + '</h2><p>' + esc(t(
      'All eight questions came from the mathematical relationships developed in this lesson.',
      'Wszystkie osiem pytań wynika z zależności matematycznych rozwijanych w tej lekcji.',
      'جميع الأسئلة الثمانية مبنية على العلاقات الرياضية التي يطورها هذا الدرس.'
    )) + '</p><strong data-final></strong><br><button class="gw-primary" type="button" data-replay>' + esc(copy.again) + '</button></div></div>' +
    '</section>';

  var game = root.querySelector('[data-grounded-game]');
  var board = root.querySelector('[data-board]');
  var question = root.querySelector('[data-question]');
  var sub = root.querySelector('[data-subquestion]');
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
    } catch (ignore) { }
  }

  function hud() {
    roundNode.textContent = (state.round + 1) + ' / ' + state.count;
    scoreNode.textContent = state.score;
    progress.style.width = ((state.round / state.count) * 100) + '%';
  }

  function mission(main, detail, guidance) {
    question.textContent = main;
    sub.textContent = detail || '';
    eddy.textContent = guidance || '';
    speak(main);
  }

  function success(button, message) {
    state.locked = true;
    state.score += 200;
    if (button) button.classList.add('is-correct');
    hud();
    eddy.textContent = copy.correct + ' ' + message;
    speak(eddy.textContent);
    window.setTimeout(next, 800);
  }

  function failure(button, message) {
    if (button) button.classList.add('is-wrong');
    eddy.textContent = copy.retry + ' ' + message;
    speak(eddy.textContent);
    window.setTimeout(function () {
      if (button) button.classList.remove('is-wrong');
    }, 600);
  }

  function next() {
    state.round += 1;
    if (state.round >= state.count) {
      progress.style.width = '100%';
      finalNode.textContent = t('Stars: ', 'Gwiazdki: ', 'النجوم: ') + state.score;
      complete.hidden = false;
      speak(copy.complete);
      return;
    }
    render();
  }

  function equationCard(text) {
    var card = document.createElement('div');
    card.className = 'gw-operation-model';
    card.innerHTML = '<div class="gw-equation">' + esc(text) + '</div>';
    board.appendChild(card);
  }

  function choiceRow(items, correct, message) {
    var row = document.createElement('div');
    row.className = 'gw-choice-row gw-runtime-choices';
    shuffle(uniq(items)).forEach(function (item) {
      var value = typeof item === 'object' ? item.value : item;
      var label = typeof item === 'object' ? item.label : item;
      var button = document.createElement('button');
      button.type = 'button';
      button.className = 'gw-choice';
      button.textContent = label;
      button.dataset.answer = value;
      row.appendChild(button);
    });
    row.addEventListener('click', function (event) {
      if (state.locked) return;
      var button = event.target.closest('[data-answer]');
      if (!button) return;
      if (String(button.dataset.answer) === String(correct)) {
        success(button, message);
      } else {
        failure(button, t(
          'Identify the relationship first, then check that the representation preserves it.',
          'Najpierw rozpoznaj zależność, a potem sprawdź, czy reprezentacja ją zachowuje.',
          'حدد العلاقة أولًا، ثم تحقق أن التمثيل يحافظ عليها.'
        ));
      }
    });
    board.appendChild(row);
  }

  function partWholeQuestion() {
    var whole = rnd(72, 140);
    var known = rnd(24, whole - 28);
    var missing = whole - known;
    mission(
      t('A whole is ' + whole + '. One part is ' + known + '. What is the other part?',
        'Całość to ' + whole + '. Jedna część to ' + known + '. Ile wynosi druga część?',
        'الكل يساوي ' + whole + ' وأحد الجزأين يساوي ' + known + '. ما قيمة الجزء الآخر؟'),
      t('Use a part-whole relationship, not a keyword.', 'Użyj zależności część–całość.', 'استخدم علاقة الجزء بالكل.'),
      t('Represent the relationship as ' + known + ' + ? = ' + whole + '.',
        'Zapisz zależność jako ' + known + ' + ? = ' + whole + '.',
        'مثّل العلاقة هكذا: ' + known + ' + ؟ = ' + whole + '.'));
    equationCard(known + ' + ? = ' + whole);
    choiceRow([missing, missing + 10, Math.max(1, missing - 10)], missing,
      t('The missing part completes the whole.', 'Brakująca część uzupełnia całość.', 'الجزء المفقود يكمل الكل.'));
  }

  function changeQuestion() {
    var start = rnd(35, 79);
    var change = rnd(16, 38);
    var finish = start + change;
    mission(
      t('A quantity starts at ' + start + ' and increases by ' + change + '. What is the new value?',
        'Wartość początkowa to ' + start + ' i wzrasta o ' + change + '. Jaka jest nowa wartość?',
        'تبدأ الكمية عند ' + start + ' وتزداد بمقدار ' + change + '. ما القيمة الجديدة؟'),
      t('This is a change relationship.', 'To zależność zmiany.', 'هذه علاقة تغيّر.'),
      t('Keep the meaning of each quantity visible in the equation.', 'Zachowaj znaczenie każdej wielkości w równaniu.', 'حافظ على معنى كل كمية في المعادلة.'));
    equationCard(start + ' + ' + change + ' = ?');
    choiceRow([finish, finish - change, finish + 10], finish,
      t('The equation matches the increase.', 'Równanie odpowiada wzrostowi.', 'المعادلة تمثل الزيادة بشكل صحيح.'));
  }

  function differenceQuestion() {
    var larger = rnd(82, 145);
    var smaller = rnd(31, larger - 24);
    var difference = larger - smaller;
    mission(
      t('Two quantities are ' + larger + ' and ' + smaller + '. What is the difference between them?',
        'Dwie wielkości to ' + larger + ' i ' + smaller + '. Jaka jest różnica między nimi?',
        'كميتان قيمتاهما ' + larger + ' و' + smaller + '. ما الفرق بينهما؟'),
      t('Compare the quantities; do not add them.', 'Porównaj wielkości; nie dodawaj ich.', 'قارن الكميتين ولا تجمعهما.'),
      t('A difference relationship can be represented by subtraction.', 'Różnicę można przedstawić odejmowaniem.', 'يمكن تمثيل علاقة الفرق بالطرح.'));
    equationCard(larger + ' − ' + smaller + ' = ?');
    choiceRow([difference, larger + smaller, difference + 10], difference,
      t('The subtraction measures the difference.', 'Odejmowanie mierzy różnicę.', 'الطرح يقيس الفرق.'));
  }

  function equationRepresentationQuestion() {
    var first = rnd(42, 78);
    var added = rnd(18, 35);
    var total = first + added;
    var correct = first + ' + ' + added + ' = ' + total;
    mission(
      t('A box contains ' + first + ' counters. ' + added + ' more are added. Which equation represents the situation?',
        'W pudełku jest ' + first + ' liczników. Dodano ' + added + '. Które równanie opisuje sytuację?',
        'في صندوق ' + first + ' قطعة، ثم أضيفت ' + added + ' قطعة. أي معادلة تمثل الموقف؟'),
      t('Choose the representation before calculating.', 'Najpierw wybierz reprezentację.', 'اختر التمثيل قبل الحساب.'),
      t('The relationship determines the operation.', 'To zależność wyznacza działanie.', 'العلاقة هي التي تحدد العملية.'));
    choiceRow([
      correct,
      first + ' − ' + added + ' = ' + (first - added),
      total + ' + ' + added + ' = ' + (total + added)
    ], correct, t('The equation preserves the original relationship.', 'Równanie zachowuje pierwotną zależność.', 'المعادلة تحافظ على العلاقة الأصلية.'));
  }

  function numberLineRepresentationQuestion() {
    var start = rnd(28, 55);
    var jump1 = rnd(12, 24);
    var jump2 = rnd(11, 22);
    var end = start + jump1 + jump2;
    var correct = start + ' → +' + jump1 + ' → +' + jump2 + ' → ' + end;
    mission(
      t('Which number-line journey represents an increase from ' + start + ' to ' + end + '?',
        'Który zapis osi liczbowej pokazuje wzrost od ' + start + ' do ' + end + '?',
        'أي مسار على خط الأعداد يمثل الزيادة من ' + start + ' إلى ' + end + '؟'),
      t('Use two jumps whose total change is ' + (jump1 + jump2) + '.',
        'Użyj dwóch skoków, których łączna zmiana to ' + (jump1 + jump2) + '.',
        'استخدم قفزتين مجموعهما ' + (jump1 + jump2) + '.'),
      t('Different representations must describe the same relationship.', 'Różne reprezentacje muszą opisywać tę samą zależność.', 'يجب أن تصف التمثيلات المختلفة العلاقة نفسها.'));
    choiceRow([
      correct,
      start + ' → −' + jump1 + ' → +' + jump2 + ' → ' + (start - jump1 + jump2),
      end + ' → +' + jump1 + ' → +' + jump2 + ' → ' + (end + jump1 + jump2)
    ], correct, t('The jumps and equation describe the same change.', 'Skoki i równanie opisują tę samą zmianę.', 'القفزات والمعادلة تصف التغيّر نفسه.'));
  }

  function unknownQuantityQuestion() {
    var unknown = rnd(34, 76);
    var added = rnd(18, 39);
    var total = unknown + added;
    mission(
      t('Solve the relationship x + ' + added + ' = ' + total + '. What does x represent?',
        'Rozwiąż zależność x + ' + added + ' = ' + total + '. Ile wynosi x?',
        'حل العلاقة x + ' + added + ' = ' + total + '. ما قيمة x؟'),
      t('Use the inverse operation to find the unknown quantity.', 'Użyj działania odwrotnego.', 'استخدم العملية العكسية لإيجاد المجهول.'),
      t('Check by substituting your value back into the original equation.', 'Sprawdź przez podstawienie wyniku do równania.', 'تحقق بتعويض القيمة في المعادلة الأصلية.'));
    equationCard('x + ' + added + ' = ' + total);
    choiceRow([unknown, unknown + added, Math.max(1, unknown - 10)], unknown,
      t('The unknown restores equality.', 'Niewiadoma przywraca równość.', 'قيمة المجهول تحافظ على المساواة.'));
  }

  function ratioLinkedQuantityQuestion() {
    var left = rnd(2, 5);
    var right = rnd(left + 1, 8);
    var scale = rnd(3, 6);
    var known = left * scale;
    var answer = right * scale;
    mission(
      t('A relationship is ' + left + ':' + right + '. If the first quantity is ' + known + ', what is the linked second quantity?',
        'Stosunek to ' + left + ':' + right + '. Jeśli pierwsza wielkość wynosi ' + known + ', ile wynosi druga?',
        'العلاقة هي ' + left + ':' + right + '. إذا كانت الكمية الأولى ' + known + '، فما الكمية الثانية المرتبطة؟'),
      t('Scale both linked quantities by the same factor.', 'Pomnóż obie powiązane wielkości przez ten sam czynnik.', 'كبّر الكميتين المرتبطتين بالعامل نفسه.'),
      t('The relationship is preserved only when both parts scale together.', 'Zależność zachowuje się tylko wtedy, gdy obie części skalują się razem.', 'تبقى العلاقة صحيحة فقط عندما تتغير الجزآن بالعامل نفسه.'));
    equationCard(left + ':' + right + ' = ' + known + ':?');
    choiceRow([answer, right + scale, answer + right], answer,
      t('Both quantities were scaled by the same factor.', 'Obie wielkości pomnożono przez ten sam czynnik.', 'تم تكبير الكميتين بالعامل نفسه.'));
  }

  function additiveLinkedQuantityQuestion() {
    var first = rnd(48, 86);
    var difference = rnd(17, 34);
    var second = first + difference;
    mission(
      t('One quantity is ' + first + '. A linked quantity is ' + difference + ' greater. Which equation preserves the relationship?',
        'Jedna wielkość to ' + first + '. Druga jest większa o ' + difference + '. Które równanie zachowuje zależność?',
        'إحدى الكميتين ' + first + ' والأخرى أكبر منها بمقدار ' + difference + '. أي معادلة تحافظ على العلاقة؟'),
      t('Represent the relationship before calculating.', 'Przed obliczeniem zapisz zależność.', 'مثّل العلاقة قبل الحساب.'),
      t('The linked quantity must remain exactly ' + difference + ' greater.', 'Druga wielkość musi pozostać większa dokładnie o ' + difference + '.', 'يجب أن تبقى الكمية المرتبطة أكبر بمقدار ' + difference + ' بالضبط.'));
    var correct = first + ' + ' + difference + ' = ' + second;
    choiceRow([correct, first + ' − ' + difference + ' = ' + (first - difference), second + ' + ' + difference + ' = ' + (second + difference)], correct,
      t('The equation preserves the stated difference.', 'Równanie zachowuje podaną różnicę.', 'المعادلة تحافظ على الفرق المحدد.'));
  }

  function ratioErrorAnalysisQuestion() {
    var a = rnd(2, 5);
    var b = rnd(a + 2, 9);
    var factor = rnd(2, 4);
    var wrongSecond = b * factor - 1;
    var claim = (a * factor) + ':' + wrongSecond;
    var correct = 'same-factor';
    mission(
      t('A student says ' + a + ':' + b + ' is equivalent to ' + claim + '. What is the error?',
        'Uczeń twierdzi, że ' + a + ':' + b + ' jest równoważne ' + claim + '. Na czym polega błąd?',
        'يقول طالب إن ' + a + ':' + b + ' تكافئ ' + claim + '. ما الخطأ؟'),
      t('Check whether both parts were multiplied by the same factor.', 'Sprawdź, czy obie części pomnożono przez ten sam czynnik.', 'تحقق هل ضُرب الجزآن بالعامل نفسه.'),
      t('A correct-looking number is not enough; verify the relationship.', 'Sam poprawnie wyglądający wynik nie wystarcza; sprawdź zależność.', 'الرقم الذي يبدو صحيحًا لا يكفي؛ تحقق من العلاقة.'));
    choiceRow([
      { value: 'same-factor', label: t('The two parts were not scaled by the same factor.', 'Obu części nie pomnożono przez ten sam czynnik.', 'لم يتم تكبير الجزأين بالعامل نفسه.') },
      { value: 'add', label: t('Ratios must always be found by addition.', 'Stosunki zawsze oblicza się dodawaniem.', 'يجب دائمًا إيجاد النسب بالجمع.') },
      { value: 'swap', label: t('The two parts must always be swapped.', 'Obie części zawsze trzeba zamienić miejscami.', 'يجب دائمًا تبديل الجزأين.') }
    ], correct, t('You checked the relationship, not just the numbers.', 'Sprawdziłeś zależność, a nie tylko liczby.', 'تحققت من العلاقة وليس من الأرقام فقط.'));
  }

  function inverseCheckQuestion() {
    var total = rnd(96, 154);
    var part = rnd(28, total - 35);
    var result = total - part;
    var correct = result + ' + ' + part + ' = ' + total;
    mission(
      t('A student calculates ' + total + ' − ' + part + ' = ' + result + '. Which second line of reasoning verifies the result?',
        'Uczeń oblicza ' + total + ' − ' + part + ' = ' + result + '. Które sprawdzenie potwierdza wynik?',
        'حسب طالب ' + total + ' − ' + part + ' = ' + result + '. أي تحقق ثانٍ يثبت النتيجة؟'),
      t('Use the inverse operation to verify the original relationship.', 'Użyj działania odwrotnego do sprawdzenia.', 'استخدم العملية العكسية للتحقق من العلاقة الأصلية.'),
      t('A valid check must reconstruct the original quantity.', 'Poprawne sprawdzenie musi odtworzyć wielkość początkową.', 'التحقق الصحيح يجب أن يعيد الكمية الأصلية.'));
    choiceRow([
      correct,
      result + ' − ' + part + ' = ' + (result - part),
      total + ' + ' + part + ' = ' + (total + part)
    ], correct, t('The inverse operation reconstructs the original whole.', 'Działanie odwrotne odtwarza pierwotną całość.', 'العملية العكسية تعيد الكل الأصلي.'));
  }

  function renderCombined() {
    var sequence = [
      partWholeQuestion,
      changeQuestion,
      differenceQuestion,
      equationRepresentationQuestion,
      numberLineRepresentationQuestion,
      unknownQuantityQuestion,
      ratioLinkedQuantityQuestion,
      ratioErrorAnalysisQuestion
    ];
    sequence[state.round]();
  }

  function renderRatioOnly() {
    var sequence = [
      ratioLinkedQuantityQuestion,
      ratioErrorAnalysisQuestion,
      ratioLinkedQuantityQuestion,
      ratioErrorAnalysisQuestion,
      equationRepresentationQuestion,
      unknownQuantityQuestion,
      ratioLinkedQuantityQuestion,
      ratioErrorAnalysisQuestion
    ];
    sequence[state.round]();
  }

  function renderAdditiveOnly() {
    var sequence = [
      partWholeQuestion,
      changeQuestion,
      differenceQuestion,
      equationRepresentationQuestion,
      numberLineRepresentationQuestion,
      unknownQuantityQuestion,
      additiveLinkedQuantityQuestion,
      inverseCheckQuestion
    ];
    sequence[state.round]();
  }

  function render() {
    state.locked = false;
    board.innerHTML = '';
    hud();
    if (mechanic === 'ADDITIVE_RATIO_RELATIONSHIPS') renderCombined();
    else if (mechanic === 'RATIO_RELATIONSHIPS') renderRatioOnly();
    else renderAdditiveOnly();
  }

  root.querySelector('[data-sound]').addEventListener('click', function () {
    state.sound = !state.sound;
    this.textContent = state.sound ? '🔊' : '🔇';
    if (!state.sound && window.speechSynthesis) window.speechSynthesis.cancel();
  });

  root.querySelector('[data-fullscreen]').addEventListener('click', function () {
    if (!document.fullscreenElement && game.requestFullscreen) game.requestFullscreen();
    else if (document.exitFullscreen) document.exitFullscreen();
  });

  root.querySelector('[data-replay]').addEventListener('click', function () {
    state.round = 0;
    state.score = 0;
    complete.hidden = true;
    render();
  });

  render();
}());
