(function () {
  'use strict';

  var root = document.querySelector('[data-curriculum-workspace-game]');
  if (!root) return;

  var locale = root.dataset.lessonLanguage || 'en';
  var langIndex = locale === 'pl' ? 1 : (locale === 'ar' ? 2 : 0);
  function t(en, pl, ar) { return [en, pl, ar][langIndex]; }

  var hintPrefix = t('Hint: ', 'Wskazówka: ', 'تلميح: ');
  var correctPrefix = t('Correct!', 'Dobrze!', 'صحيح!');
  var retryPrefix = t('Try again.', 'Spróbuj ponownie.', 'حاول مرة أخرى.');
  var genericHint = hintPrefix + t(
    'Use the visual model and the exact skill named in the lesson.',
    'Użyj modelu i dokładnie tej umiejętności, której dotyczy lekcja.',
    'استخدم النموذج البصري والمهارة المحددة في الدرس.'
  );

  var eddy = root.querySelector('[data-eddy]');
  var soundButton = root.querySelector('[data-sound]');
  var synth = window.speechSynthesis;
  var Utterance = window.SpeechSynthesisUtterance;
  var nativeCancel = synth && synth.cancel ? synth.cancel.bind(synth) : null;
  var nativeSpeak = synth && synth.speak ? synth.speak.bind(synth) : null;
  var feedbackProtected = false;
  var feedbackReleaseTimer = null;
  var latestLessonGuidance = eddy ? eddy.textContent.trim() : '';

  function soundEnabled() {
    return !soundButton || soundButton.textContent.indexOf('🔇') === -1;
  }

  function makeUtterance(text) {
    if (!Utterance) return null;
    var utterance = new Utterance(text);
    utterance.lang = locale === 'pl' ? 'pl-PL' : (locale === 'ar' ? 'ar-AE' : 'en-GB');
    utterance.rate = 0.92;
    return utterance;
  }

  function releaseFeedbackProtection(cancelSpeech) {
    feedbackProtected = false;
    if (feedbackReleaseTimer) {
      window.clearTimeout(feedbackReleaseTimer);
      feedbackReleaseTimer = null;
    }
    if (cancelSpeech && nativeCancel) nativeCancel();
  }

  if (synth && nativeCancel) {
    try {
      synth.cancel = function () {
        if (!feedbackProtected) nativeCancel();
      };
    } catch (ignore) { }
  }

  if (soundButton) {
    soundButton.addEventListener('click', function () {
      if (feedbackProtected) releaseFeedbackProtection(true);
    }, true);
  }

  function speakCompleteFeedback(text) {
    if (!synth || !nativeSpeak || !nativeCancel || !soundEnabled()) return;

    var utterance = makeUtterance(text);
    if (!utterance) return;

    feedbackProtected = true;
    nativeCancel();

    var release = function () { releaseFeedbackProtection(false); };
    utterance.onend = release;
    utterance.onerror = release;
    nativeSpeak(utterance);

    feedbackReleaseTimer = window.setTimeout(
      release,
      Math.max(4500, Math.min(9000, text.length * 90))
    );
  }

  function speakLessonHint(text) {
    if (!synth || !nativeSpeak || !nativeCancel || !soundEnabled()) return;
    var utterance = makeUtterance(text);
    if (!utterance) return;
    nativeCancel();
    nativeSpeak(utterance);
  }

  function enhanceClock() {
    var clocks = root.querySelectorAll('.gw-clock:not([data-full-clock-dial])');
    clocks.forEach(function (clock) {
      clock.dataset.fullClockDial = 'true';
      clock.querySelectorAll(':scope > b, :scope > em, :scope > strong, :scope > small').forEach(function (node) {
        node.style.display = 'none';
      });

      for (var number = 1; number <= 12; number += 1) {
        var angle = (number * 30 - 90) * Math.PI / 180;
        var label = document.createElement('span');
        label.className = 'gw-clock-number';
        label.textContent = String(number);
        label.style.position = 'absolute';
        label.style.left = (50 + 40 * Math.cos(angle)) + '%';
        label.style.top = (50 + 40 * Math.sin(angle)) + '%';
        label.style.transform = 'translate(-50%, -50%)';
        label.style.color = '#ecfbff';
        label.style.fontWeight = '900';
        label.style.fontStyle = 'normal';
        label.style.fontSize = 'clamp(14px, 1.4vw, 20px)';
        label.style.lineHeight = '1';
        label.style.pointerEvents = 'none';
        label.style.zIndex = '3';
        clock.appendChild(label);
      }
    });
  }

  function handleEddyText() {
    if (!eddy) return;
    var text = eddy.textContent.trim();
    if (!text) return;

    if (text.indexOf(correctPrefix) === 0) {
      speakCompleteFeedback(text);
      return;
    }

    if (text === genericHint && latestLessonGuidance) {
      var lessonHint = hintPrefix + latestLessonGuidance;
      eddy.textContent = lessonHint;
      speakLessonHint(lessonHint);
      return;
    }

    if (text.indexOf(retryPrefix) === 0 || text.indexOf(hintPrefix) === 0) return;
    latestLessonGuidance = text;
  }

  var observer = new MutationObserver(function () {
    enhanceClock();
    handleEddyText();
  });

  observer.observe(root, { childList: true, subtree: true, characterData: true });
  enhanceClock();
  handleEddyText();
})();
