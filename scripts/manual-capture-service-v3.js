const express = require('express');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE = (process.env.BASE_URL || 'https://staging.edulytiks.com').replace(/\/$/, '');
const EMAIL = process.env.SUPERVISOR_EMAIL || 'demo.supervisor@edulytiks.com';
const PASSWORD = process.env.DEMO_PASSWORD || '';
const runId = Date.now();

function event(type, data = {}) {
  console.log('CREATE_USER_SMOKE', JSON.stringify({ at: new Date().toISOString(), type, ...data }));
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

async function setCulture(page) {
  const url = new URL(BASE);
  await page.setCookie({
    name: 'Edulytics.Culture',
    value: 'c=en|uic=en',
    domain: url.hostname,
    path: '/',
    secure: true,
    sameSite: 'Strict'
  });
}

async function login(page) {
  await setCulture(page);
  const response = await page.goto(`${BASE}/account/login`, { waitUntil: 'networkidle2', timeout: 60000 });
  event('login-page', { status: response?.status() || null, url: page.url() });
  const email = await page.$('input[type="email"],input[name="Email"],input[name$=".Email"]');
  const password = await page.$('input[type="password"]');
  const submit = await page.$('button[type="submit"],input[type="submit"]');
  if (!email || !password || !submit) throw new Error('login controls missing');
  await email.type(EMAIL);
  await password.type(PASSWORD);
  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null),
    submit.click()
  ]);
  if (page.url().toLowerCase().includes('/account/login')) {
    const msg = await page.evaluate(() => document.querySelector('.validation-summary-errors,.alert-danger')?.innerText?.trim() || '');
    throw new Error(`login failed: ${msg || 'still on login page'}`);
  }
  event('login-pass', { url: page.url() });
}

async function openCreate(page) {
  const response = await page.goto(`${BASE}/School/Users/Create`, { waitUntil: 'networkidle2', timeout: 60000 });
  const status = response?.status() || null;
  const url = page.url();
  event('create-page', { status, url });
  if (!response || status >= 400 || url.toLowerCase().includes('/account/login')) {
    throw new Error(`create page unavailable: status=${status} url=${url}`);
  }
  const key = await page.$eval('input[name="_idempotencyKey"]', el => el.value);
  if (!key) throw new Error('fresh idempotency key missing');
  return key;
}

async function submitCreate(page, role) {
  const key = await openCreate(page);
  const suffix = `${runId}.${role.toLowerCase()}`;
  const email = `smoke.${suffix}@example.invalid`;
  await page.select('#Role', role);
  await page.type('#Email', email);

  let classId = null;
  if (role === 'Student') {
    await page.waitForFunction(() => {
      const section = document.querySelector('#direct-student-setup');
      return section && !section.hidden;
    }, { timeout: 15000 });
    await page.waitForFunction(() => {
      const select = document.querySelector('#ClassGroupId');
      return select && [...select.options].some(o => o.value);
    }, { timeout: 30000 });
    await page.type('#StudentNumber', `SMK-${runId}`);
    await page.type('#FirstName', 'Smoke');
    await page.type('#LastName', 'Student');
    classId = await page.$eval('#ClassGroupId', select => [...select.options].find(o => o.value)?.value || null);
    if (!classId) throw new Error('no active class available for Student smoke');
    await page.select('#ClassGroupId', classId);
  }

  const posts = [];
  const listener = response => {
    const request = response.request();
    try {
      const url = new URL(response.url());
      if (request.method() === 'POST' && url.pathname.toLowerCase() === '/school/users/create') {
        posts.push({ status: response.status(), url: response.url() });
      }
    } catch {}
  };
  page.on('response', listener);
  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null),
    page.click('form.user-form-card button[type="submit"]')
  ]);
  page.off('response', listener);

  const finalUrl = page.url();
  const detailsMatch = /\/School\/Users\/[0-9a-f-]{36}(?:\?|$)/i.test(finalUrl);
  const errors = await page.evaluate(() => [...document.querySelectorAll('.validation-summary-errors,.user-field-error,.alert-danger')]
    .map(x => (x.innerText || '').trim()).filter(Boolean));
  const result = { role, email, key, classId, posts, finalUrl, detailsMatch, errors };
  event(detailsMatch ? 'create-pass' : 'create-fail', result);
  if (!detailsMatch) throw new Error(`${role} create did not reach Details: ${JSON.stringify(result)}`);
  return result;
}

async function run() {
  let browser;
  try {
    event('start', { base: BASE, supervisor: EMAIL });
    if (!PASSWORD) throw new Error('DEMO_PASSWORD is empty');
    browser = await launch();
    const page = await browser.newPage();
    await login(page);
    const student = await submitCreate(page, 'Student');
    const teacher = await submitCreate(page, 'Teacher');
    event('done', { status: 'PASS', studentEmail: student.email, teacherEmail: teacher.email });
    await page.close();
  } catch (error) {
    event('fatal', { status: 'FAIL', error: error.stack || error.message });
    process.exitCode = 1;
  } finally {
    if (browser) await browser.close().catch(() => {});
  }
}

const app = express();
app.get('/', (_req, res) => res.type('text').send('Edulytics create-user staging smoke helper\n'));
app.listen(PORT, '0.0.0.0', () => {
  console.log(`create-user smoke helper listening on ${PORT}`);
  void run();
});
