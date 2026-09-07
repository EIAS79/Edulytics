(() => {
  const root = document.querySelector('.ed-home');
  const hero = root?.querySelector('.ed-home-v6-hero');
  if (!hero) return;

  const next = hero.querySelector('.ed-home-v6-next');
  const dots = [...hero.querySelectorAll('.ed-home-v6-dot')];
  if (!next || dots.length < 2) return;

  let lastIndex = dots.findIndex(dot => dot.classList.contains('is-active'));
  if (lastIndex < 0) lastIndex = 0;
  let lastChangeAt = Date.now();

  const readIndex = () => {
    const index = dots.findIndex(dot => dot.classList.contains('is-active'));
    return index < 0 ? lastIndex : index;
  };

  const noteChange = () => {
    const index = readIndex();
    if (index !== lastIndex) {
      lastIndex = index;
      lastChangeAt = Date.now();
    }
  };

  // Watch only the dot class state. This never rewrites the hero DOM.
  const dotObserver = new MutationObserver(noteChange);
  dots.forEach(dot => dotObserver.observe(dot, { attributes: true, attributeFilter: ['class'] }));

  // Safety watchdog: if the original carousel has not advanced for 9 seconds,
  // advance it through the existing next button. This keeps autoplay working
  // even when the pointer remains over the hero and the older script pauses its timer.
  const watchdog = window.setInterval(() => {
    if (document.hidden) return;
    noteChange();
    if (Date.now() - lastChangeAt < 9000) return;
    next.click();
    lastChangeAt = Date.now();
  }, 1000);

  document.addEventListener('visibilitychange', () => {
    lastChangeAt = Date.now();
    noteChange();
  });

  window.addEventListener('pagehide', () => {
    window.clearInterval(watchdog);
    dotObserver.disconnect();
  }, { once: true });
})();