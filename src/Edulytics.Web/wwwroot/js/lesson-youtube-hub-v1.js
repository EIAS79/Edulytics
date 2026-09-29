(() => {
  const hubs = document.querySelectorAll('[data-yt-learning-hub]');
  const fmt = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 1 });

  const escapeHtml = value => String(value ?? '')
    .replaceAll('&', '&amp;').replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;').replaceAll('"', '&quot;')
    .replaceAll("'", '&#039;');

  async function load(hub, query) {
    const state = hub.querySelector('[data-yt-state]');
    const body = hub.querySelector('[data-yt-body]');
    const fallback = hub.querySelector('[data-yt-fallback]');
    state.hidden = false;
    state.textContent = 'Searching YouTube for this lesson…';
    body.hidden = true;
    fallback.hidden = true;

    const params = new URLSearchParams({
      lessonCode: hub.dataset.lessonCode,
      title: hub.dataset.lessonTitle,
      culture: hub.dataset.culture || 'en'
    });
    if (query) params.set('q', query);

    try {
      const response = await fetch('/api/lesson-youtube/search?' + params.toString(), {
        credentials: 'same-origin',
        headers: { 'Accept': 'application/json' }
      });
      if (!response.ok) throw new Error('search failed');
      const data = await response.json();

      hub.querySelector('[data-yt-band]').textContent =
        data.band === 'primary-1-6' ? 'Grades 1–6' :
        data.band === 'middle-7-9' ? 'Grades 7–9' : 'Grade 10+';

      hub.querySelector('[data-yt-channels]').innerHTML =
        (data.preferredChannels || []).map(x =>
          '<span class="yt-learning-hub__channel">' + escapeHtml(x) + '</span>').join('');

      if (!data.featured) {
        state.textContent = data.message ||
          'No sufficiently relevant YouTube result was found for this lesson.';
        return;
      }

      const featured = data.featured;
      hub.querySelector('[data-yt-frame]').innerHTML =
        '<iframe src="https://www.youtube-nocookie.com/embed/' +
        encodeURIComponent(featured.videoId) +
        '" title="' + escapeHtml(featured.title) +
        '" loading="lazy" referrerpolicy="strict-origin-when-cross-origin" ' +
        'allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>';
      hub.querySelector('[data-yt-feature-title]').textContent = featured.title;
      hub.querySelector('[data-yt-feature-meta]').textContent =
        featured.channelTitle + ' · ' + fmt.format(featured.viewCount) + ' views';

      hub.querySelector('[data-yt-results]').innerHTML =
        (data.related || []).map(video =>
          '<a class="yt-learning-result" target="_blank" rel="noopener noreferrer" href="https://www.youtube.com/watch?v=' +
          encodeURIComponent(video.videoId) + '">' +
          '<img loading="lazy" src="' + escapeHtml(video.thumbnailUrl) + '" alt="">' +
          '<div><strong>' + escapeHtml(video.title) + '</strong><span>' +
          escapeHtml(video.channelTitle) + ' · ' + fmt.format(video.viewCount) +
          ' views</span></div></a>').join('');

      if (data.usedFallback) {
        fallback.hidden = false;
        fallback.textContent =
          'No strong match was found in the preferred education channels, so Edulytics used the highest-ranked relevant YouTube result.';
      }

      state.hidden = true;
      body.hidden = false;
    } catch {
      state.textContent = 'YouTube results are temporarily unavailable. The lesson itself is unaffected.';
    }
  }

  hubs.forEach(hub => {
    const form = hub.querySelector('[data-yt-search-form]');
    form.addEventListener('submit', event => {
      event.preventDefault();
      const input = form.querySelector('input');
      load(hub, input.value.trim());
    });
    load(hub, '');
  });
})();