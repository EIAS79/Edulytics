(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Mascot rendering is intentionally owned only by v17. */

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
