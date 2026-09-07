(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const mascotHost = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-mascot');
  if (!mascotHost) return;

  /* Rebuild from the original supplied asset after v15, then remove only the edge-connected
     white background. The output canvas always keeps the original width/height: no crop,
     no bounding box, no zoom-based cutting of symbols around the mascot. */
  const image = new Image();
  image.alt = 'Edulytics mathematics mascot';
  image.decoding = 'async';
  image.loading = 'eager';

  const renderFullCanvas = () => {
    if (!image.naturalWidth || !image.naturalHeight) return;

    const width = image.naturalWidth;
    const height = image.naturalHeight;
    const canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;
    canvas.className = 'ed-home-v16-mascot-canvas';
    canvas.setAttribute('role', 'img');
    canvas.setAttribute('aria-label', image.alt);

    const ctx = canvas.getContext('2d', { willReadFrequently: true });
    if (!ctx) return;

    try {
      ctx.drawImage(image, 0, 0, width, height);
      const imageData = ctx.getImageData(0, 0, width, height);
      const pixels = imageData.data;
      const pixelCount = width * height;
      const background = new Uint8Array(pixelCount);
      const queue = new Int32Array(pixelCount);
      let head = 0;
      let tail = 0;
      const threshold = 110;
      const thresholdSq = threshold * threshold;

      const nearWhite = index => {
        const offset = index * 4;
        const dr = 255 - pixels[offset];
        const dg = 255 - pixels[offset + 1];
        const db = 255 - pixels[offset + 2];
        return (dr * dr + dg * dg + db * db) < thresholdSq;
      };

      const enqueue = index => {
        if (background[index] || !nearWhite(index)) return;
        background[index] = 1;
        queue[tail++] = index;
      };

      for (let x = 0; x < width; x++) {
        enqueue(x);
        enqueue((height - 1) * width + x);
      }
      for (let y = 1; y < height - 1; y++) {
        enqueue(y * width);
        enqueue(y * width + width - 1);
      }

      while (head < tail) {
        const index = queue[head++];
        const x = index % width;
        const y = (index / width) | 0;
        if (x > 0) enqueue(index - 1);
        if (x + 1 < width) enqueue(index + 1);
        if (y > 0) enqueue(index - width);
        if (y + 1 < height) enqueue(index + width);
      }

      for (let index = 0; index < pixelCount; index++) {
        if (!background[index]) continue;
        const offset = index * 4;
        const dr = 255 - pixels[offset];
        const dg = 255 - pixels[offset + 1];
        const db = 255 - pixels[offset + 2];
        const distance = Math.sqrt(dr * dr + dg * dg + db * db);
        pixels[offset + 3] = Math.max(0, Math.min(255, Math.round(((distance - 5) / (threshold - 5)) * 255)));
      }

      ctx.putImageData(imageData, 0, 0);
      mascotHost.replaceChildren(canvas);
      mascotHost.dataset.v16Applied = 'true';
    } catch (error) {
      console.warn('Edulytics mascot transparency cleanup could not be applied.', error);
      image.className = 'ed-home-v16-mascot-canvas';
      mascotHost.replaceChildren(image);
    }
  };

  image.addEventListener('load', renderFullCanvas, { once: true });
  image.src = '/images/brand/edulytics-mascot-v14.webp';
  if (image.complete) renderFullCanvas();
})();
