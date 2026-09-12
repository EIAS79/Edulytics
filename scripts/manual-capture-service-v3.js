const express = require('express');
const puppeteer = require('puppeteer-core');
const PORT = process.env.PORT || 10000;
const BASE = 'https://edulytiks.com';
let result = { status: 'RUNNING', base: BASE, checks: [] };
const event = (type, data = {}) => console.log('PUBLIC_HOME_DIAG', JSON.stringify({ at: new Date().toISOString(), type, ...data }));

async function launch() {
  const mod = await import('@sparticuz/chromium');
  const chromium = mod.default || mod;
  return puppeteer.launch({ executablePath: await chromium.executablePath(), args: [...chromium.args, '--disable-dev-shm-usage'], headless: 'shell' });
}

async function run() {
  let browser;
  try {
    browser = await launch();
    const page = await browser.newPage();
    await page.setViewport({ width: 360, height: 1000, deviceScaleFactor: 1, isMobile: true, hasTouch: true });
    const response = await page.goto(BASE, { waitUntil: 'networkidle2', timeout: 90000 });
    if (!response || response.status() >= 400) throw new Error(`home HTTP ${response?.status()}`);
    await new Promise(r => setTimeout(r, 1600));

    const data = await page.evaluate(async () => {
      const live = document.querySelector('.ed-home-v12-slide:first-child img.ed-home-v16-mascot-canvas, canvas[data-ed-mascot-background-cleaned="true"]');
      const visual = document.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-visual');
      const cssText = await fetch('/css/public-site-v35.css', { cache: 'no-store' }).then(r => r.text());

      async function analyzeAsset(src) {
        const img = new Image();
        img.src = src + (src.includes('?') ? '&' : '?') + 'diag=' + Date.now();
        await new Promise((resolve, reject) => { img.onload = resolve; img.onerror = reject; });
        const max = 700;
        const scale = Math.min(1, max / Math.max(img.naturalWidth, img.naturalHeight));
        const w = Math.max(1, Math.round(img.naturalWidth * scale));
        const h = Math.max(1, Math.round(img.naturalHeight * scale));
        const c = document.createElement('canvas'); c.width = w; c.height = h;
        const ctx = c.getContext('2d', { willReadFrequently: true });
        ctx.drawImage(img, 0, 0, w, h);
        const px = ctx.getImageData(0, 0, w, h).data;
        let transparent = 0, opaqueLight = 0, opaque = 0;
        for (let i = 0; i < px.length; i += 4) {
          const a = px[i+3];
          if (a < 24) transparent++;
          if (a > 230) opaque++;
          const maxc = Math.max(px[i], px[i+1], px[i+2]);
          const minc = Math.min(px[i], px[i+1], px[i+2]);
          const b = (px[i] + px[i+1] + px[i+2]) / 3;
          if (a > 230 && b > 215 && (maxc-minc) < 80) opaqueLight++;
        }
        const total = w*h;
        const sample = (x,y) => Array.from(ctx.getImageData(x,y,1,1).data);
        return {
          src, naturalWidth: img.naturalWidth, naturalHeight: img.naturalHeight,
          transparentRatio: transparent/total, opaqueRatio: opaque/total, opaqueLightRatio: opaqueLight/total,
          corners: [sample(0,0), sample(w-1,0), sample(0,h-1), sample(w-1,h-1)]
        };
      }

      const assets = [];
      for (const src of ['/images/public/edulytics-math-mascot.png?v=33','/images/public/edulytics-math-mascot-1.png']) {
        try { assets.push(await analyzeAsset(src)); } catch (e) { assets.push({ src, error: String(e) }); }
      }

      return {
        clientWidth: document.documentElement.clientWidth,
        scrollWidth: document.documentElement.scrollWidth,
        bodyDisplay: getComputedStyle(document.body).display,
        bodyOverflowX: getComputedStyle(document.body).overflowX,
        htmlOverflowX: getComputedStyle(document.documentElement).overflowX,
        visualBackground: visual ? getComputedStyle(visual).backgroundColor : null,
        visualBorder: visual ? getComputedStyle(visual).border : null,
        visualBoxShadow: visual ? getComputedStyle(visual).boxShadow : null,
        liveMascot: live ? { tag: live.tagName, src: live.src || null, className: live.className, cleaned: live instanceof HTMLCanvasElement } : null,
        cssHasTransparencyFile: cssText.includes('public-home-mascot-transparency-v35.css'),
        cssHasStrongTransparentRule: cssText.includes('background:transparent!important'),
        cssLength: cssText.length,
        assets
      };
    });

    result.checks.push(data);
    event('asset-diagnostic', data);
    result = { ...result, status: 'PASS', completedAt: new Date().toISOString() };
  } catch (error) {
    result = { ...result, status: 'FAIL', error: error.stack || error.message, completedAt: new Date().toISOString() };
    event('fatal', { error: result.error });
  } finally { if (browser) await browser.close().catch(() => {}); }
}

const app = express();
app.get('/', (_req, res) => res.status(result.status === 'FAIL' ? 500 : 200).json(result));
app.listen(PORT, '0.0.0.0', () => { console.log(`Public homepage diagnostic helper listening on ${PORT}`); void run(); });
