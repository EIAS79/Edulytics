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

  const removeLargestLightComponent = () => {
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
      const pixelCount = width * height;

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
      const labels = new Int32Array(pixelCount);
      const queue = new Int32Array(pixelCount);
      let componentId = 0;
      let largestComponentId = 0;
      let largestComponentSize = 0;

      const isLightBackground = index => {
        const offset = index * 4;
        const alpha = pixels[offset + 3];
        if (alpha < 24) return false;

        const red = pixels[offset];
        const green = pixels[offset + 1];
        const blue = pixels[offset + 2];
        const maximum = Math.max(red, green, blue);
        const minimum = Math.min(red, green, blue);
        const brightness = (red + green + blue) / 3;
        const chroma = maximum - minimum;

        return (brightness >= 178 && chroma <= 48) ||
               (brightness >= 215 && chroma <= 92);
      };

      for (let start = 0; start < pixelCount; start += 1) {
        if (labels[start] !== 0 || !isLightBackground(start)) continue;

        componentId += 1;
        let head = 0;
        let tail = 0;
        let size = 0;
        labels[start] = componentId;
        queue[tail++] = start;

        while (head < tail) {
          const index = queue[head++];
          size += 1;
          const x = index % width;
          const y = Math.floor(index / width);

          const tryAdd = next => {
            if (labels[next] !== 0 || !isLightBackground(next)) return;
            labels[next] = componentId;
            queue[tail++] = next;
          };

          if (x > 0) tryAdd(index - 1);
          if (x + 1 < width) tryAdd(index + 1);
          if (y > 0) tryAdd(index - width);
          if (y + 1 < height) tryAdd(index + width);
        }

        if (size > largestComponentSize) {
          largestComponentSize = size;
          largestComponentId = componentId;
        }
      }

      // The unwanted rounded white/light card occupies a large connected region.
      // Small light components such as eyes, teeth and highlights must remain intact.
      const minimumBackgroundSize = Math.max(1500, Math.round(pixelCount * 0.08));
      if (!largestComponentId || largestComponentSize < minimumBackgroundSize) {
        revealFallback();
        return;
      }

      for (let index = 0; index < pixelCount; index += 1) {
        if (labels[index] === largestComponentId) {
          pixels[index * 4 + 3] = 0;
        }
      }

      context.putImageData(imageData, 0, 0);
      mascot.replaceWith(canvas);
    } catch {
      revealFallback();
    }
  };

  mascot.style.visibility = 'hidden';
  if (mascot.complete) {
    removeLargestLightComponent();
  } else {
    mascot.addEventListener('load', removeLargestLightComponent, { once: true });
    mascot.addEventListener('error', revealFallback, { once: true });
  }
})();
