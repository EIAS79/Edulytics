(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Replace the broken v13 base64 injection with the verified, normal image asset. */
  const mascotHost = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-mascot');
  if (mascotHost) {
    const image = document.createElement('img');
    image.className = 'ed-home-v14-mascot-image';
    image.src = '/images/brand/edulytics-mascot-v14.webp';
    image.alt = 'Edulytics mathematics mascot';
    image.decoding = 'async';
    image.loading = 'eager';
    mascotHost.replaceChildren(image);
    mascotHost.dataset.v14Applied = 'true';
  }

  /* Add hierarchy to mega-menu items without changing any approved copy. */
  const tones = ['blue', 'teal', 'purple', 'coral'];
  const glyphs = ['▰', '✦', '◎', '⚙', '∑', '¤', '✓', '↗'];

  const normalize = value => String(value || '').trim().toLowerCase();
  const iconGroup = title => {
    const value = normalize(title);
    return [
      'overview', 'przegląd', 'نظرة عامة',
      'subjects', 'tematy', 'المواضيع',
      'teaching with edulytics', 'nauczanie z edulytics', 'التدريس باستخدام edulytics',
      'learning with edulytics', 'nauka z edulytics', 'التعلّم مع edulytics',
      'transform education', 'transformacja edukacji', 'تطوير التعليم'
    ].some(token => value.includes(token));
  };

  root.querySelectorAll('.ed-home-v11-mega-group').forEach((group, groupIndex) => {
    const title = group.querySelector('h3')?.textContent || '';
    const useIcons = iconGroup(title);
    const tone = tones[groupIndex % tones.length];

    group.querySelectorAll('a').forEach((link, linkIndex) => {
      if (link.classList.contains('ed-home-v14-menu-link')) return;

      link.classList.add('ed-home-v14-menu-link', `ed-home-v14-tone-${tone}`);
      const marker = document.createElement('span');
      marker.setAttribute('aria-hidden', 'true');

      if (useIcons) {
        marker.className = 'ed-home-v14-menu-icon';
        marker.textContent = glyphs[linkIndex % glyphs.length];
      } else {
        marker.className = 'ed-home-v14-menu-dot';
      }

      link.prepend(marker);
    });
  });
})();
