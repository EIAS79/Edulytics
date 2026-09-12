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
    await new Promise(r => setTimeout(r, 1800));

    const data = await page.evaluate(() => {
      const visual = document.querySelector('.ed-home-v12-slide:first-child .ed-home-v12-visual');
      if (!visual) return { error: 'visual missing' };
      const matches = [];
      const walkRules = (rules, source) => {
        for (const rule of Array.from(rules || [])) {
          if (rule.cssRules) { try { walkRules(rule.cssRules, source); } catch {} continue; }
          if (!rule.selectorText || !rule.style) continue;
          let matched = false;
          try { matched = visual.matches(rule.selectorText); } catch {}
          if (!matched) continue;
          const interesting = ['background','background-color','border','border-color','border-width','box-shadow','overflow','overflow-x'];
          const props = {};
          for (const p of interesting) {
            const value = rule.style.getPropertyValue(p);
            if (value) props[p] = { value, priority: rule.style.getPropertyPriority(p) || '' };
          }
          if (Object.keys(props).length) matches.push({ source, selector: rule.selectorText, props });
        }
      };
      for (const sheet of Array.from(document.styleSheets)) {
        const source = sheet.href || (sheet.ownerNode && sheet.ownerNode.id) || 'inline-style';
        try { walkRules(sheet.cssRules, source); } catch (e) { matches.push({ source, error: String(e) }); }
      }
      const s = getComputedStyle(visual);
      return {
        clientWidth: document.documentElement.clientWidth,
        scrollWidth: document.documentElement.scrollWidth,
        computed: { background: s.background, border: s.border, boxShadow: s.boxShadow, overflow: s.overflow, cssText: visual.getAttribute('style') },
        matches
      };
    });
    result.checks.push(data);
    event('cascade', data);
    result = { ...result, status: 'PASS', completedAt: new Date().toISOString() };
  } catch (error) {
    result = { ...result, status: 'FAIL', error: error.stack || error.message, completedAt: new Date().toISOString() };
    event('fatal', { error: result.error });
  } finally { if (browser) await browser.close().catch(() => {}); }
}

const app = express();
app.get('/', (_req, res) => res.status(result.status === 'FAIL' ? 500 : 200).json(result));
app.listen(PORT, '0.0.0.0', () => { console.log(`Public homepage cascade helper listening on ${PORT}`); void run(); });
