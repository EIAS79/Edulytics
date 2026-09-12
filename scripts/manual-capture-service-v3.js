const express = require('express');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE = 'https://edulytiks.com';

let result = { status: 'RUNNING', base: BASE, checks: [] };

function event(type, data = {}) {
  console.log('PUBLIC_HOME_DIAG', JSON.stringify({ at: new Date().toISOString(), type, ...data }));
}

async function launch() {
  const mod = await import('@sparticuz/chromium');
  const chromium = mod.default || mod;
  return puppeteer.launch({
    executablePath: await chromium.executablePath(),
    args: [...chromium.args, '--disable-dev-shm-usage'],
    headless: 'shell'
  });
}

async function setLanguage(page, language) {
  const url = new URL(BASE);
  await page.setCookie({
    name: 'Edulytics.PublicLanguage',
    value: language,
    domain: url.hostname,
    path: '/',
    secure: true,
    sameSite: 'Lax'
  });
  await page.evaluateOnNewDocument(lang => {
    try { window.localStorage.setItem('edulytics.public.siteLanguage', lang); } catch {}
  }, language);
}

async function inspect(page, language, width) {
  await page.setViewport({ width, height: 1000, deviceScaleFactor: 1, isMobile: true, hasTouch: true });
  await setLanguage(page, language);
  const response = await page.goto(BASE, { waitUntil: 'networkidle2', timeout: 90000 });
  if (!response || response.status() >= 400) throw new Error(`${language}/${width}: HTTP ${response?.status()}`);
  await new Promise(resolve => setTimeout(resolve, 1500));

  const data = await page.evaluate(({ language, width }) => {
    const pick = selector => {
      const el = document.querySelector(selector);
      if (!el) return null;
      const r = el.getBoundingClientRect();
      const s = getComputedStyle(el);
      return {
        selector,
        tag: el.tagName,
        left: Math.round(r.left * 10) / 10,
        right: Math.round(r.right * 10) / 10,
        top: Math.round(r.top * 10) / 10,
        width: Math.round(r.width * 10) / 10,
        height: Math.round(r.height * 10) / 10,
        display: s.display,
        position: s.position,
        overflowX: s.overflowX,
        direction: s.direction,
        transform: s.transform,
        backgroundColor: s.backgroundColor
      };
    };

    const offenders = [];
    for (const el of document.body.querySelectorAll('*')) {
      const r = el.getBoundingClientRect();
      if (r.width < 2 || r.height < 2) continue;
      if (r.right > innerWidth + 2 || r.left < -2) {
        const cls = typeof el.className === 'string' ? el.className.trim().replace(/\s+/g, '.') : '';
        offenders.push({
          tag: el.tagName,
          id: el.id || '',
          cls: cls.slice(0, 180),
          left: Math.round(r.left),
          right: Math.round(r.right),
          width: Math.round(r.width),
          top: Math.round(r.top),
          visible: getComputedStyle(el).display !== 'none' && getComputedStyle(el).visibility !== 'hidden'
        });
      }
    }
    offenders.sort((a, b) => Math.abs(b.right - innerWidth) + Math.abs(Math.min(0, b.left)) - (Math.abs(a.right - innerWidth) + Math.abs(Math.min(0, a.left))));

    const mascot = document.querySelector('canvas[data-ed-mascot-background-cleaned="true"], .ed-home-v12-slide:first-child img.ed-home-v16-mascot-canvas');
    let mascotInfo = null;
    if (mascot) {
      const r = mascot.getBoundingClientRect();
      const s = getComputedStyle(mascot);
      mascotInfo = {
        tag: mascot.tagName,
        cleaned: mascot instanceof HTMLCanvasElement && mascot.dataset.edMascotBackgroundCleaned === 'true',
        left: Math.round(r.left),
        right: Math.round(r.right),
        width: Math.round(r.width),
        height: Math.round(r.height),
        backgroundColor: s.backgroundColor,
        mixBlendMode: s.mixBlendMode,
        parentBackground: mascot.parentElement ? getComputedStyle(mascot.parentElement).backgroundColor : null
      };
      if (mascot instanceof HTMLCanvasElement) {
        try {
          const ctx = mascot.getContext('2d');
          const pts = [
            [0, 0], [mascot.width - 1, 0], [0, mascot.height - 1], [mascot.width - 1, mascot.height - 1],
            [Math.floor(mascot.width / 2), 0], [0, Math.floor(mascot.height / 2)],
            [mascot.width - 1, Math.floor(mascot.height / 2)], [Math.floor(mascot.width / 2), mascot.height - 1]
          ];
          mascotInfo.edgeAlpha = pts.map(([x, y]) => ctx.getImageData(Math.max(0, x), Math.max(0, y), 1, 1).data[3]);
        } catch (e) {
          mascotInfo.edgeAlphaError = String(e);
        }
      }
    }

    return {
      language,
      requestedWidth: width,
      innerWidth,
      clientWidth: document.documentElement.clientWidth,
      documentScrollWidth: document.documentElement.scrollWidth,
      bodyScrollWidth: document.body.scrollWidth,
      dir: document.documentElement.dir,
      href: location.href,
      elements: {
        html: pick('html'),
        body: pick('body'),
        home: pick('.ed-home'),
        header: pick('.ed-home-header'),
        nav: pick('.ed-home-nav-shell'),
        brand: pick('.ed-home-brand'),
        logo: pick('.ed-home-brand img'),
        hero: pick('.ed-home-v12-hero'),
        viewport: pick('.ed-home-v12-viewport'),
        track: pick('.ed-home-v12-track'),
        firstSlide: pick('.ed-home-v12-slide:first-child'),
        grid: pick('.ed-home-v12-slide:first-child .ed-home-v12-slide-grid'),
        copy: pick('.ed-home-v12-slide:first-child .ed-home-v12-copy'),
        visual: pick('.ed-home-v12-slide:first-child .ed-home-v12-visual')
      },
      mascot: mascotInfo,
      offenders: offenders.slice(0, 25),
      css: Array.from(document.querySelectorAll('link[rel="stylesheet"]')).map(x => x.getAttribute('href')),
      js: Array.from(document.querySelectorAll('script[src]')).map(x => x.getAttribute('src'))
    };
  }, { language, width });

  result.checks.push(data);
  event('viewport', data);
}

async function run() {
  let browser;
  try {
    browser = await launch();
    const page = await browser.newPage();
    for (const language of ['en', 'ar']) {
      for (const width of [360, 390, 412, 768]) {
        await inspect(page, language, width);
      }
    }
    result = { ...result, status: 'PASS', completedAt: new Date().toISOString() };
    event('done', { status: result.status, count: result.checks.length });
  } catch (error) {
    result = { ...result, status: 'FAIL', error: error.stack || error.message, completedAt: new Date().toISOString() };
    event('fatal', { error: result.error });
  } finally {
    if (browser) await browser.close().catch(() => {});
  }
}

const app = express();
app.get('/', (_req, res) => res.status(result.status === 'FAIL' ? 500 : 200).json(result));
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Public homepage diagnostic helper listening on ${PORT}`);
  void run();
});
