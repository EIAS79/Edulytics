(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const firstSlideVisual = root.querySelector(
    '.ed-home-v12-slide:first-child .ed-home-v12-visual'
  );

  const renderMascotWithoutWhiteBackground = host => {
    const source = new Image();
    source.alt = 'Edulytics mathematics mascot';
    source.loading = 'eager';
    source.decoding = 'async';

    const showSourceFallback = () => {
      if (host.querySelector('.ed-home-v16-mascot-canvas')) return;
      source.className = 'ed-home-v16-mascot-canvas';
      host.appendChild(source);
    };

    source.addEventListener('load', () => {
      try {
        const naturalWidth = source.naturalWidth;
        const naturalHeight = source.naturalHeight;
        if (!naturalWidth || !naturalHeight) {
          showSourceFallback();
          return;
        }

        const maxDimension = 1800;
        const scale = Math.min(1, maxDimension / Math.max(naturalWidth, naturalHeight));
        const width = Math.max(1, Math.round(naturalWidth * scale));
        const height = Math.max(1, Math.round(naturalHeight * scale));
        const pixelCount = width * height;

        const canvas = document.createElement('canvas');
        canvas.width = width;
        canvas.height = height;
        canvas.className = 'ed-home-v16-mascot-canvas';
        canvas.setAttribute('role', 'img');
        canvas.setAttribute('aria-label', source.alt);
        canvas.dataset.edMascotEdgeBackgroundRemoved = 'true';

        const context = canvas.getContext('2d', { willReadFrequently: true });
        if (!context) {
          showSourceFallback();
          return;
        }

        context.drawImage(source, 0, 0, width, height);
        const imageData = context.getImageData(0, 0, width, height);
        const pixels = imageData.data;
        const visited = new Uint8Array(pixelCount);
        const queue = new Int32Array(pixelCount);
        let head = 0;
        let tail = 0;

        const isEdgeBackground = index => {
          const offset = index * 4;
          const alpha = pixels[offset + 3];
          if (alpha <= 24) return true;

          const red = pixels[offset];
          const green = pixels[offset + 1];
          const blue = pixels[offset + 2];
          const maximum = Math.max(red, green, blue);
          const minimum = Math.min(red, green, blue);
          const brightness = (red + green + blue) / 3;
          const chroma = maximum - minimum;

          // Only light, low-chroma pixels connected to the image perimeter are removed.
          // This preserves enclosed whites such as eyes, teeth and highlights.
          return brightness >= 184 && chroma <= 72;
        };

        const enqueue = index => {
          if (index < 0 || index >= pixelCount || visited[index] || !isEdgeBackground(index)) return;
          visited[index] = 1;
          queue[tail++] = index;
        };

        for (let x = 0; x < width; x += 1) {
          enqueue(x);
          enqueue((height - 1) * width + x);
        }
        for (let y = 0; y < height; y += 1) {
          enqueue(y * width);
          enqueue(y * width + width - 1);
        }

        while (head < tail) {
          const index = queue[head++];
          const x = index % width;
          const y = Math.floor(index / width);

          if (x > 0) enqueue(index - 1);
          if (x + 1 < width) enqueue(index + 1);
          if (y > 0) enqueue(index - width);
          if (y + 1 < height) enqueue(index + width);
        }

        for (let index = 0; index < pixelCount; index += 1) {
          if (visited[index]) pixels[index * 4 + 3] = 0;
        }

        // Remove the very light anti-aliased halo immediately beside the cleared background.
        for (let pass = 0; pass < 2; pass += 1) {
          const newlyCleared = [];
          for (let index = 0; index < pixelCount; index += 1) {
            if (visited[index]) continue;

            const x = index % width;
            const y = Math.floor(index / width);
            const touchesCleared =
              (x > 0 && visited[index - 1]) ||
              (x + 1 < width && visited[index + 1]) ||
              (y > 0 && visited[index - width]) ||
              (y + 1 < height && visited[index + width]);
            if (!touchesCleared) continue;

            const offset = index * 4;
            const red = pixels[offset];
            const green = pixels[offset + 1];
            const blue = pixels[offset + 2];
            const maximum = Math.max(red, green, blue);
            const minimum = Math.min(red, green, blue);
            const brightness = (red + green + blue) / 3;
            const chroma = maximum - minimum;

            if (brightness >= 218 && chroma <= 82) newlyCleared.push(index);
          }

          newlyCleared.forEach(index => {
            visited[index] = 1;
            pixels[index * 4 + 3] = 0;
          });
        }

        context.putImageData(imageData, 0, 0);
        host.appendChild(canvas);
      } catch {
        showSourceFallback();
      }
    }, { once: true });

    source.addEventListener('error', showSourceFallback, { once: true });
    source.src = '/images/public/edulytics-math-mascot.png?v=39';
  };

  if (firstSlideVisual) {
    // Replace only the previous assistant-generated first-slide artwork.
    firstSlideVisual.querySelectorAll('.ed-home-v12-mascot').forEach(node => node.remove());
    firstSlideVisual.querySelectorAll('.ed-home-v16-mascot-canvas').forEach(node => node.remove());
    renderMascotWithoutWhiteBackground(firstSlideVisual);
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
