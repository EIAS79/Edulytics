(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const firstSlideVisual = root.querySelector(
    '.ed-home-v12-slide:first-child .ed-home-v12-visual'
  );

  if (firstSlideVisual) {
    // Replace only the previous assistant-generated first-slide artwork.
    firstSlideVisual.querySelectorAll('.ed-home-v12-mascot').forEach(node => node.remove());

    if (!firstSlideVisual.querySelector('.ed-home-v16-mascot-canvas')) {
      const image = document.createElement('img');
      image.className = 'ed-home-v16-mascot-canvas';
      image.src = '/images/public/edulytics-math-mascot.png';
      image.alt = 'Edulytics mathematics mascot';
      image.loading = 'eager';
      image.decoding = 'async';
      image.style.width = '430px';
      firstSlideVisual.appendChild(image);
    }
  }

  // Load the editorial curriculum pathway experience after the existing homepage layers.
  if (!document.querySelector('script[data-ed-home-v20]')) {
    const script = document.createElement('script');
    script.src = '/js/public-home-commercial-v20.js';
    script.async = false;
    script.dataset.edHomeV20 = 'true';
    document.body.appendChild(script);
  }
})();
