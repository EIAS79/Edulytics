(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const understandingRoute = '/product/learning-built-for-understanding';
  const understandingLabels = new Set([
    'Learning Built for Understanding',
    'Nauka oparta na zrozumieniu',
    'تعلّم قائم على الفهم'
  ]);

  const normalize = value => String(value || '').replace(/\s+/g, ' ').trim();
  const currentPath = window.location.pathname.replace(/\/+$/, '') || '/';

  root.querySelectorAll('a').forEach(anchor => {
    const label = normalize(anchor.textContent);
    if (understandingLabels.has(label)) {
      anchor.setAttribute('href', understandingRoute);
      return;
    }

    if (currentPath !== '/') {
      const href = anchor.getAttribute('href');
      if (href && href.startsWith('#')) {
        anchor.setAttribute('href', `/${href}`);
      }
    }
  });
})();
