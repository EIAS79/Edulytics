(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const firstSlideVisual = root.querySelector(
    '.ed-home-v12-slide:first-child .ed-home-v12-visual'
  );

  if (firstSlideVisual) {
    // The supplied PNG already carries transparency. Keep the stage only for layout;
    // do not rasterize, recolor, segment, or otherwise process the artwork.
    firstSlideVisual.classList.add('ed-home-v40-mascot-stage');
    firstSlideVisual.querySelectorAll('.ed-home-v12-mascot, .ed-home-v16-mascot-canvas, .ed-home-v17-mascot-image, .ed-home-v40-mascot-image')
      .forEach(node => node.remove());

    const image = document.createElement('img');
    image.className = 'ed-home-v40-mascot-image';
    image.src = '/images/public/edulytics-math-mascot.png?v=40';
    image.alt = 'Edulytics mathematics mascot';
    image.loading = 'eager';
    image.decoding = 'async';
    firstSlideVisual.appendChild(image);
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