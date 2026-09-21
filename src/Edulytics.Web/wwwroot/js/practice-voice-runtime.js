(function () {
  'use strict';

  var key = 'edulytics.practice.sound';

  function isSupported() {
    return !!(window.speechSynthesis && window.SpeechSynthesisUtterance);
  }

  function isEnabled() {
    try {
      return window.localStorage.getItem(key) !== 'off';
    } catch (_) {
      return true;
    }
  }

  function setEnabled(value) {
    try {
      window.localStorage.setItem(key, value ? 'on' : 'off');
    } catch (_) { }
    if (!value && isSupported()) {
      window.speechSynthesis.cancel();
    }
    return value;
  }

  function language(locale) {
    if (locale === 'pl') return 'pl-PL';
    if (locale === 'ar') return 'ar-AE';
    return 'en-GB';
  }

  function utter(text, locale) {
    var value = String(text || '').trim();
    if (!value || !isSupported()) return null;
    var item = new SpeechSynthesisUtterance(value);
    item.lang = language(locale || document.documentElement.lang || 'en');
    item.rate = 0.92;
    return item;
  }

  function speak(text, locale) {
    if (!isEnabled() || !isSupported()) return;
    var item = utter(text, locale);
    if (!item) return;
    try {
      window.speechSynthesis.cancel();
      window.speechSynthesis.speak(item);
    } catch (_) { }
  }

  function speakMany(parts, locale) {
    if (!isEnabled() || !isSupported()) return;
    var items = (parts || [])
      .map(function (part) { return utter(part, locale); })
      .filter(Boolean);
    if (!items.length) return;
    try {
      window.speechSynthesis.cancel();
      items.forEach(function (item) {
        window.speechSynthesis.speak(item);
      });
    } catch (_) { }
  }

  function toggle() {
    return setEnabled(!isEnabled());
  }

  window.EdulyticsPracticeVoice = {
    isSupported: isSupported,
    isEnabled: isEnabled,
    setEnabled: setEnabled,
    toggle: toggle,
    speak: speak,
    speakMany: speakMany,
    stop: function () {
      if (isSupported()) window.speechSynthesis.cancel();
    }
  };
}());
