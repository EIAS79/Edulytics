(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const host = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-mascot');
  if (!host) return;

  /* Final mascot source: derived directly from the user's transparent artwork.
     Render as a normal image only — no canvas cleanup, crop, bounding-box detection or cover scaling. */
  const image = document.createElement('img');
  image.className = 'ed-home-v17-mascot-image';
  image.alt = 'Edulytics mathematics mascot';
  image.decoding = 'async';
  image.loading = 'eager';
  image.src = '/images/brand/edulytics-mascot-full.png?v=18-final';

  host.replaceChildren(image);
  host.dataset.v17Applied = 'true';
})();
