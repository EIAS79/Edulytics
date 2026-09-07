(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const mascotHost = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-mascot');
  if (!mascotHost) return;

  /* Use the supplied transparent mascot as a real PNG asset.
     No canvas processing, white-background removal, crop or runtime recompression. */
  const image = document.createElement('img');
  image.className = 'ed-home-v16-mascot-canvas';
  image.alt = 'Edulytics mathematics mascot';
  image.decoding = 'async';
  image.loading = 'eager';
  image.src = '/images/brand/edulytics-mascot-clean-v2.png?v=2';

  mascotHost.replaceChildren(image);
  mascotHost.dataset.v16Applied = 'true';
})();
