(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const host = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-mascot');
  if (!host) return;

  const image = document.createElement('img');
  image.className = 'ed-home-v17-mascot-image';
  image.alt = 'Edulytics mathematics mascot';
  image.decoding = 'async';
  image.loading = 'eager';
  image.src = '/images/brand/edulytics-mascot-final.png?v=17';

  host.replaceChildren(image);
  host.dataset.v17Applied = 'true';
})();
