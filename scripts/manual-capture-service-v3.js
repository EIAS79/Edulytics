const express = require('express');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE = (process.env.BASE_URL || 'https://staging.edulytiks.com').replace(/\/$/, '');
const EMAIL = process.env.TEACHER_EMAIL || '';
const PASSWORD = process.env.TEACHER_PASSWORD || '';
const ASSESSMENT_ID = 'c66f49e6-a1cf-483d-8443-eeffb45e6618';
const TITLE = 'PDF Acceptance 20260905-X9Q2';

let result = { status: 'RUNNING', assessmentId: ASSESSMENT_ID, checks: [] };

function event(type, data = {}) {
  console.log('PDF_CLEANUP', JSON.stringify({ at: new Date().toISOString(), type, ...data }));
}
function pass(kind, data = {}) {
  result.checks.push({ kind, status: 'PASS', ...data });
  event(kind, { status: 'PASS', ...data });
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
    name: 'Edulytics.Culture', value: 'c=en|uic=en', domain: url.hostname,
    path: '/', secure: true, sameSite: 'Strict'
  });
  const response = await page.goto(`${BASE}/account/login`, { waitUntil: 'networkidle2', timeout: 60000 });
  if (!response || response.status() >= 400) throw new Error(`login page HTTP ${response?.status()}`);
  const email = await page.$('input[type="email"],input[name="Email"],input[name$=".Email"]');
  const password = await page.$('input[type="password"],input[name="Password"],input[name$=".Password"]');
  const submit = await page.$('button[type="submit"],input[type="submit"]');
  if (!email || !password || !submit) throw new Error('login controls missing');
  await email.type(EMAIL);
  await password.type(PASSWORD);
  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null),
    submit.click()
  ]);
  if (page.url().toLowerCase().includes('/account/login')) throw new Error('teacher login failed');
  pass('teacher-login', { url: page.url() });
}

async function cleanupAssessment(page) {
  const detailsUrl = `${BASE}/school/assessments/${ASSESSMENT_ID}`;
  const response = await page.goto(detailsUrl, { waitUntil: 'networkidle2', timeout: 60000 });
  if (!response) throw new Error('assessment details returned no response');
  if (response.status() === 404) {
    pass('assessment-already-absent');
    return;
  }
  if (response.status() >= 400) throw new Error(`assessment details HTTP ${response.status()}`);

  const deleteForm = `form[action$="/${ASSESSMENT_ID}/delete"]`;
  if (!await page.$(deleteForm)) throw new Error('assessment delete form missing');

  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null),
    page.evaluate(selector => {
      const form = document.querySelector(selector);
      if (!form) throw new Error('delete form missing');
      if (typeof form.requestSubmit === 'function') form.requestSubmit();
      else form.submit();
    }, deleteForm)
  ]);

  await page.goto(`${BASE}/school/assessments`, { waitUntil: 'networkidle2', timeout: 60000 });
  const stillVisible = await page.evaluate(title => (document.body.innerText || '').includes(title), TITLE);
  if (stillVisible) throw new Error('assessment fixture still visible after delete');
  pass('assessment-deleted-via-ui');
}

async function run() {
  let browser;
  try {
    if (!EMAIL || !PASSWORD) throw new Error('temporary teacher credentials missing');
    browser = await launch();
    const page = await browser.newPage();
    await login(page);
    await cleanupAssessment(page);
    result = { ...result, status: 'PASS', completedAt: new Date().toISOString() };
    event('done', result);
  } catch (error) {
    result = { ...result, status: 'FAIL', error: error.stack || error.message, completedAt: new Date().toISOString() };
    event('fatal', result);
  } finally {
    if (browser) await browser.close().catch(() => {});
  }
}

const app = express();
app.get('/', (_req, res) => res.status(result.status === 'FAIL' ? 500 : 200).json(result));
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Teacher PDF cleanup helper listening on ${PORT}`);
  void run();
});
