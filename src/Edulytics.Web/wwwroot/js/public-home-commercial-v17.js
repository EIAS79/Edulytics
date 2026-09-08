(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const host = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-mascot');
  if (!host) return;

  /* v17 is the single owner of the public-home mascot.
     The approved artwork is stored once in the existing SVG wrapper as an embedded PNG.
     Read that PNG payload and render it as a normal <img>; no canvas, crop, white-removal,
     bounding-box processing, Docker reconstruction or legacy mascot race. */
  const fallbackMarkup = host.innerHTML;
  const image = document.createElement('img');
  image.className = 'ed-home-v17-mascot-image';
  image.alt = 'Edulytics mathematics mascot';
  image.decoding = 'async';
  image.loading = 'eager';

  fetch('/images/brand/edulytics-mascot-clean.svg', { cache: 'force-cache' })
    .then(response => {
      if (!response.ok) throw new Error(`Mascot asset request failed: ${response.status}`);
      return response.text();
    })
    .then(svgText => {
      const match = svgText.match(/href=["']data:image\/png;base64,([^"']+)["']/i);
      if (!match) throw new Error('Embedded mascot PNG was not found.');
      image.src = `data:image/png;base64,${match[1].replace(/\s+/g, '')}`;
      host.replaceChildren(image);
      host.dataset.v17Applied = 'true';
    })
    .catch(error => {
      console.error('Edulytics mascot could not be rendered.', error);
      host.innerHTML = fallbackMarkup;
    });
})();
