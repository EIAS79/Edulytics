(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Keep dropdowns open long enough for normal pointer travel from the tab into the mega panel. */
  const navItems = [...root.querySelectorAll('.ed-home-v11-nav-item')];
  const closeTimers = new WeakMap();

  const clearClose = item => {
    const timer = closeTimers.get(item);
    if (timer) window.clearTimeout(timer);
    closeTimers.delete(item);
  };

  const openHover = item => {
    clearClose(item);
    navItems.forEach(other => {
      if (other === item) return;
      other.classList.remove('is-hover-open');
      if (other.classList.contains('is-open')) {
        other.classList.remove('is-open');
        other.querySelector('.ed-home-v11-nav-trigger')?.setAttribute('aria-expanded', 'false');
      }
    });
    item.classList.add('is-hover-open');
    item.querySelector('.ed-home-v11-nav-trigger')?.setAttribute('aria-expanded', 'true');
  };

  const scheduleClose = item => {
    clearClose(item);
    const timer = window.setTimeout(() => {
      if (item.matches(':hover') || item.contains(document.activeElement) || item.classList.contains('is-open')) return;
      item.classList.remove('is-hover-open');
      item.querySelector('.ed-home-v11-nav-trigger')?.setAttribute('aria-expanded', 'false');
      closeTimers.delete(item);
    }, 320);
    closeTimers.set(item, timer);
  };

  navItems.forEach(item => {
    const trigger = item.querySelector('.ed-home-v11-nav-trigger');
    const mega = item.querySelector('.ed-home-v11-mega');

    item.addEventListener('mouseenter', () => openHover(item));
    item.addEventListener('mouseleave', () => scheduleClose(item));
    mega?.addEventListener('mouseenter', () => openHover(item));
    mega?.addEventListener('mouseleave', () => scheduleClose(item));

    trigger?.addEventListener('focus', () => openHover(item));
  });

  document.addEventListener('pointerdown', event => {
    if (event.target.closest('.ed-home-v11-nav-item')) return;
    navItems.forEach(item => {
      clearClose(item);
      item.classList.remove('is-hover-open');
    });
  });

  /* Mascot replacement intentionally removed in v14.
     v14 owns the verified, normal image asset and must not race an async legacy fetch. */
})();
