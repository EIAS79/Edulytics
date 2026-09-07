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

  /* Replace the generated v12 family with the exact mascot image supplied by the user. */
  const applyUserMascot = async () => {
    const firstSlide = root.querySelector('.ed-home-v12-slide:first-child');
    const mascotHost = firstSlide?.querySelector('.ed-home-v12-mascot');
    if (!mascotHost || mascotHost.dataset.v13Applied === 'true') return;

    mascotHost.dataset.v13Applied = 'true';
    try {
      const response = await fetch('/images/brand/edulytics-mascot-v13.b64', { cache: 'force-cache' });
      if (!response.ok) throw new Error(`Mascot asset ${response.status}`);
      const base64 = (await response.text()).trim();
      if (!base64) throw new Error('Mascot asset empty');

      const image = document.createElement('img');
      image.className = 'ed-home-v13-mascot-image';
      image.alt = 'Edulytics mathematics mascot';
      image.decoding = 'async';
      image.loading = 'eager';
      image.src = `data:image/webp;base64,${base64}`;

      mascotHost.replaceChildren(image);
    } catch (error) {
      mascotHost.dataset.v13Applied = 'false';
      console.warn('Edulytics mascot asset could not be applied.', error);
    }
  };

  applyUserMascot();

  /* v12 is static after construction, but guard against a later DOM refresh. */
  const hero = root.querySelector('.ed-home-v12-hero');
  if (hero) {
    const observer = new MutationObserver(() => applyUserMascot());
    observer.observe(hero, { childList: true, subtree: true });
    window.addEventListener('pagehide', () => observer.disconnect(), { once: true });
  }
})();
