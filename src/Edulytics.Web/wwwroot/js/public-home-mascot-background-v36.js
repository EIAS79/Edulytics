(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const mascot = root.querySelector(
    '.ed-home-v12-slide:first-child img.ed-home-v16-mascot-canvas'
  );
  if (!(mascot instanceof HTMLImageElement)) return;

  const revealFallback = () => {
    mascot.style.visibility = '';
    mascot.style.mixBlendMode = 'multiply';
  };

  const removeConnectedLightBackground = () => {
    try {
      const naturalWidth = mascot.naturalWidth;
      const naturalHeight = mascot.naturalHeight;
      if (!naturalWidth || !naturalHeight) {
        revealFallback();
        return;
      }

      const maxDimension = 1600;
      const scale = Math.min(1, maxDimension / Math.max(naturalWidth, naturalHeight));
      const width = Math.max(1, Math.round(naturalWidth * scale));
      const height = Math.max(1, Math.round(naturalHeight * scale));

      const canvas = document.createElement('canvas');
      canvas.width = width;
      canvas.height = height;
      canvas.className = mascot.className;
      canvas.setAttribute('role', 'img');
      canvas.setAttribute('aria-label', mascot.alt || 'Edulytics mathematics mascot');
      canvas.dataset.edMascotBackgroundCleaned = 'true';

      const context = canvas.getContext('2d', { willReadFrequently: true });
      if (!context) {
        revealFallback();
        return;
      }

      context.drawImage(mascot, 0, 0, width, height);
      const imageData = context.getImageData(0, 0, width, height);
      const pixels = imageData.data;
      const pixelCount = width * height;
      const visited = new Uint8Array(pixelCount);
      const queue = new Int32Array(pixelCount);
      let head = 0;
      let tail = 0;

      const cornerIndexes = [
        0,
        width - 1,
        (height - 1) * width,
        pixelCount - 1
      ];

      let refR = 0;
      let refG = 0;
      let refB = 0;
      let refCount = 0;
      for (const index of cornerIndexes) {
        const offset = index * 4;
        if (pixels[offset + 3] < 32) continue;
        refR += pixels[offset];
        refG += pixels[offset + 1];
        refB += pixels[offset + 2];
        refCount += 1;
      }

      if (refCount === 0) {
        mascot.style.visibility = '';
        return;
      }

      refR /= refCount;
      refG /= refCount;
      refB /= refCount;

      const isBackground = index => {
        const offset = index * 4;
        const alpha = pixels[offset + 3];
        if (alpha < 32) return true;

        const red = pixels[offset];
        const green = pixels[offset + 1];
        const blue = pixels[offset + 2];
        const maximum = Math.max(red, green, blue);
        const minimum = Math.min(red, green, blue);
        const brightness = (red + green + blue) / 3;
        const chroma = maximum - minimum;

        if (brightness < 205 || chroma > 70) return false;

        const dr = red - refR;
        const dg = green - refG;
        const db = blue - refB;
        const distanceSquared = dr * dr + dg * dg + db * db;

        return brightness >= 242 || distanceSquared <= 12000;
      };

      const enqueue = index => {
        if (visited[index] || !isBackground(index)) return;
        visited[index] = 1;
        queue[tail++] = index;
      };

      for (let x = 0; x < width; x += 1) {
        enqueue(x);
        enqueue((height - 1) * width + x);
      }
      for (let y = 1; y < height - 1; y += 1) {
        enqueue(y * width);
        enqueue(y * width + width - 1);
      }

      while (head < tail) {
        const index = queue[head++];
        const x = index % width;
        const y = Math.floor(index / width);
        pixels[index * 4 + 3] = 0;

        if (x > 0) enqueue(index - 1);
        if (x + 1 < width) enqueue(index + 1);
        if (y > 0) enqueue(index - width);
        if (y + 1 < height) enqueue(index + width);
      }

      context.putImageData(imageData, 0, 0);
      mascot.replaceWith(canvas);
    } catch {
      revealFallback();
    }
  };

  mascot.style.visibility = 'hidden';
  if (mascot.complete) {
    removeConnectedLightBackground();
  } else {
    mascot.addEventListener('load', removeConnectedLightBackground, { once: true });
    mascot.addEventListener('error', revealFallback, { once: true });
  }
})();
