(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  // Some later routing/localization layers update anchor text with textContent,
  // which removes the v14 marker span. Restore only a missing marker after all
  // public-shell mutations have completed.
  const glyphs = ['▰', '✦', '◎', '⚙', '∑', '¤', '✓', '↗'];
  const iconHeadings = [
    'overview', 'przegląd', 'نظرة عامة',
    'learning scope', 'zakres nauki', 'نطاق التعلّم',
    'teaching with edulytics', 'nauczanie z edulytics', 'التدريس باستخدام edulytics',
    'learning with edulytics', 'nauka z edulytics', 'التعلّم مع edulytics',
    'transform education', 'transformacja edukacji', 'تطوير التعليم'
  ];

  const normalize = value => String(value || '').trim().toLowerCase();

  root.querySelectorAll('.ed-home-v11-mega-group').forEach(group => {
    const heading = normalize(group.querySelector('h3')?.textContent);
    const useIcon = iconHeadings.some(token => heading.includes(token));

    group.querySelectorAll('a.ed-home-v14-menu-link').forEach((link, linkIndex) => {
      if (link.querySelector('.ed-home-v14-menu-icon, .ed-home-v14-menu-dot')) return;

      const marker = document.createElement('span');
      marker.setAttribute('aria-hidden', 'true');
      marker.className = useIcon ? 'ed-home-v14-menu-icon' : 'ed-home-v14-menu-dot';
      if (useIcon) marker.textContent = glyphs[linkIndex % glyphs.length];
      link.prepend(marker);
    });
  });
})();
