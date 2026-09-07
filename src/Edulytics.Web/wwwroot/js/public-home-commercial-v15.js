(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Remove the parent free-trial CTA from the teacher slide in every language.
     The second slide is the teacher slide for EN / PL / AR, so this remains copy-independent. */
  const teacherActions = root.querySelector('.ed-home-v12-slide:nth-child(2) .ed-home-v12-actions');
  if (teacherActions) {
    teacherActions.querySelector('.ed-home-v12-primary')?.remove();
    teacherActions.classList.add('is-v15-single');
  }

  /* Convert only the edge-connected near-white field of the supplied mascot image to transparency.
     Internal whites (eyes, teeth, calculator details) are preserved because they are not connected to the image edge. */
  const mascotImage = root.querySelector('.ed-home-v12-slide:first-child .ed-home-v14-mascot-image');
  if (!mascotImage) return;

  const processMascot = () => {
    if (!mascotImage.naturalWidth || !mascotImage.naturalHeight) return;

    const width = mascotImage.naturalWidth;
    const height = mascotImage.naturalHeight;
    const source = document.createElement('canvas');
    source.width = width;
    source.height = height;
    const ctx = source.getContext('2d', { willReadFrequently: true });
    if (!ctx) return;

    try {
      ctx.drawImage(mascotImage, 0, 0, width, height);
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

      let minX = width;
      let minY = height;
      let maxX = -1;
      let maxY = -1;
      for (let index = 0; index < pixelCount; index++) {
        const offset = index * 4;
        if (background[index]) {
          const dr = 255 - pixels[offset];
          const dg = 255 - pixels[offset + 1];
          const db = 255 - pixels[offset + 2];
          const distance = Math.sqrt(dr * dr + dg * dg + db * db);
          pixels[offset + 3] = Math.max(0, Math.min(255, Math.round(((distance - 5) / (threshold - 5)) * 255)));
        }

        if (pixels[offset + 3] > 18) {
          const x = index % width;
          const y = (index / width) | 0;
          if (x < minX) minX = x;
          if (x > maxX) maxX = x;
          if (y < minY) minY = y;
          if (y > maxY) maxY = y;
        }
      }

      if (maxX < minX || maxY < minY) return;

      const pad = 8;
      const sx = Math.max(0, minX - pad);
      const sy = Math.max(0, minY - pad);
      const sw = Math.min(width - sx, (maxX - minX + 1) + pad * 2);
      const sh = Math.min(height - sy, (maxY - minY + 1) + pad * 2);

      ctx.putImageData(imageData, 0, 0);
      const output = document.createElement('canvas');
      output.width = sw;
      output.height = sh;
      output.className = 'ed-home-v15-mascot-canvas';
      output.setAttribute('role', 'img');
      output.setAttribute('aria-label', mascotImage.alt || 'Edulytics mathematics mascot');
      const outCtx = output.getContext('2d');
      if (!outCtx) return;
      outCtx.drawImage(source, sx, sy, sw, sh, 0, 0, sw, sh);
      mascotImage.replaceWith(output);
    } catch (error) {
      console.warn('Edulytics mascot background cleanup could not be applied.', error);
    }
  };

  if (mascotImage.complete) processMascot();
  else mascotImage.addEventListener('load', processMascot, { once: true });
})();
