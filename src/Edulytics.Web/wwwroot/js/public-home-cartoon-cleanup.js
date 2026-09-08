(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const firstSlideVisual = root.querySelector(
    '.ed-home-v12-slide:first-child .ed-home-v12-visual'
  );
  if (!firstSlideVisual) return;

  // Replace only the previous assistant-generated first-slide artwork.
  firstSlideVisual.querySelectorAll('.ed-home-v12-mascot').forEach(node => node.remove());

  if (firstSlideVisual.querySelector('.ed-home-v16-mascot-canvas')) return;

  const image = document.createElement('img');
  image.className = 'ed-home-v16-mascot-canvas';
  image.src = '/images/public/edulytics-math-mascot.png';
  image.alt = 'Edulytics mathematics mascot';
  image.loading = 'eager';
  image.decoding = 'async';
  image.style.width = '430px';
  firstSlideVisual.appendChild(image);
})();
