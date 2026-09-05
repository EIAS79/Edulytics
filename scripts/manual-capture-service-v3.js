const express = require('express');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE = (process.env.BASE_URL || 'https://staging.edulytiks.com').replace(/\/$/, '');
const EMAIL = process.env.TEACHER_EMAIL || process.env.SUPERVISOR_EMAIL || '';
const PASSWORD = process.env.TEACHER_PASSWORD || process.env.DEMO_PASSWORD || '';

let result = { status: 'RUNNING', checks: [] };

function event(type, data = {}) {
  console.log('PDF_ACCEPTANCE', JSON.stringify({ at: new Date().toISOString(), type, ...data }));
}

async function launch() {
  const mod = await import('@sparticuz/chromium');
  const chromium = mod.default || mod;
  return puppeteer.launch({
    executablePath: await chromium.executablePath(),
    args: [...chromium.args, '--disable-dev-shm-usage'],
    headless: 'shell',
    defaultViewport: { width: 1440, height: 900 }
  });
}

async function login(page) {
  const url = new URL(BASE);
  await page.setCookie({
    name: 'Edulytics.Culture',
    value: 'c=en|uic=en',
    domain: url.hostname,
    path: '/',
    secure: true,
    sameSite: 'Strict'
  });

  const response = await page.goto(`${BASE}/account/login`, {
    waitUntil: 'networkidle2', timeout: 60000
  });

  const email = await page.$('input[type="email"],input[name="Email"],input[name$=".Email"]');
  const password = await page.$('input[type="password"],input[name="Password"],input[name$=".Password"]');
  const submit = await page.$('button[type="submit"],input[type="submit"]');
  if (!email || !password || !submit) {
    const diagnostic = await page.evaluate(() => ({
      url: location.href,
      title: document.title,
      h1: document.querySelector('h1')?.innerText?.trim() || null,
      inputs: [...document.querySelectorAll('input')].map(x => ({
        type: x.getAttribute('type'), name: x.getAttribute('name'), id: x.id || null
      })),
      buttons: [...document.querySelectorAll('button')].map(x => ({
        type: x.getAttribute('type'), text: (x.innerText || '').trim()
      }))
    }));
    event('login-controls-missing', { status: response?.status() || null, diagnostic });
    throw new Error('login controls missing');
  }

  await email.type(EMAIL);
  await password.type(PASSWORD);
  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null),
    submit.click()
  ]);

  if (page.url().toLowerCase().includes('/account/login')) {
    const message = await page.$eval('.validation-summary, [role="alert"]', x => (x.innerText || '').trim()).catch(() => '');
    throw new Error(`login failed: still on login page${message ? `: ${message}` : ''}`);
  }

  result.checks.push({ kind: 'login', status: response?.status() || null, url: page.url() });
  event('login-pass', { url: page.url(), account: EMAIL });
}

async function inspectAssessments(page) {
  const response = await page.goto(`${BASE}/school/assessments`, {
    waitUntil: 'networkidle2', timeout: 60000
  });

  const snapshot = await page.evaluate(() => ({
    url: location.href,
    title: document.title,
    h1: document.querySelector('h1')?.innerText?.trim() || null,
    links: [...document.querySelectorAll('a[href]')]
      .map(a => ({ text: (a.innerText || '').trim(), href: a.getAttribute('href') }))
      .filter(x => x.href && /assessment/i.test(x.href))
      .slice(0, 100),
    forms: [...document.querySelectorAll('form')].map(f => ({
      action: f.getAttribute('action'),
      method: f.getAttribute('method'),
      controls: [...f.querySelectorAll('input,select,textarea,button')].map(el => ({
        tag: el.tagName,
        type: el.getAttribute('type'),
        name: el.getAttribute('name'),
        id: el.id || null,
        text: (el.innerText || '').trim().slice(0, 120)
      }))
    })).slice(0, 20)
  }));

  if (!response || response.status() >= 400 || snapshot.url.toLowerCase().includes('/account/login')) {
    throw new Error(`assessment list unavailable: status=${response?.status()} url=${snapshot.url}`);
  }

  result.checks.push({ kind: 'assessments-page', status: response.status(), ...snapshot });
  event('assessments-page-pass', { status: response.status(), url: snapshot.url, links: snapshot.links.length, forms: snapshot.forms.length });
}

async function run() {
  let browser;
  try {
    if (!EMAIL) throw new Error('TEACHER_EMAIL/SUPERVISOR_EMAIL is empty');
    if (!PASSWORD) throw new Error('TEACHER_PASSWORD/DEMO_PASSWORD is empty');

    browser = await launch();
    const page = await browser.newPage();
    await login(page);
    await inspectAssessments(page);

    result = { ...result, status: 'PASS', completedAt: new Date().toISOString() };
    event('done', result);
  } catch (error) {
    result = {
      ...result,
      status: 'FAIL',
      error: error.stack || error.message,
      completedAt: new Date().toISOString()
    };
    event('fatal', result);
  } finally {
    if (browser) await browser.close().catch(() => {});
  }
}

const app = express();
app.get('/', (_req, res) => res.status(result.status === 'FAIL' ? 500 : 200).json(result));
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Teacher PDF acceptance helper listening on ${PORT}`);
  void run();
});
