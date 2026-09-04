const express = require('express');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE = (process.env.BASE_URL || 'https://staging.edulytiks.com').replace(/\/$/, '');
const EMAIL = process.env.SUPERVISOR_EMAIL || 'demo.supervisor@edulytiks.com';
const PASSWORD = process.env.DEMO_PASSWORD || '';

let result = { status: 'RUNNING', checks: [] };

function event(type, data = {}) {
  console.log('PHASE43_REPORTS_ACCEPTANCE', JSON.stringify({
    at: new Date().toISOString(),
    type,
    ...data
  }));
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

  const response = await page.goto(
    `${BASE}/account/login`,
    { waitUntil: 'networkidle2', timeout: 60000 });

  event('login-page', {
    status: response?.status() || null,
    url: page.url()
  });

  const email = await page.$(
    'input[type="email"],input[name="Email"],input[name$=".Email"]');
  const password = await page.$('input[type="password"]');
  const submit = await page.$('button[type="submit"],input[type="submit"]');

  if (!email || !password || !submit) {
    throw new Error('login controls missing');
  }

  await email.type(EMAIL);
  await password.type(PASSWORD);

  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 })
      .catch(() => null),
    submit.click()
  ]);

  if (page.url().toLowerCase().includes('/account/login')) {
    throw new Error('login failed: still on login page');
  }

  event('login-pass', { url: page.url() });
}

async function inspectReports(page, kind, expected, forbidden) {
  const response = await page.goto(
    `${BASE}/school/reports?kind=${encodeURIComponent(kind)}`,
    { waitUntil: 'networkidle2', timeout: 60000 });

  const status = response?.status() || null;
  const url = page.url();

  if (!response || status >= 400 ||
      url.toLowerCase().includes('/account/login')) {
    throw new Error(
      `reports ${kind} unavailable: status=${status} url=${url}`);
  }

  const snapshot = await page.evaluate(() => ({
    kind: document.querySelector('#kind')?.value || null,
    names: [...document.querySelectorAll(
      'form[data-report-filter-form] select[name]')]
      .map(x => x.getAttribute('name')),
    hasGenerate: !!document.querySelector(
      'form[data-report-filter-form] button[type="submit"]')
  }));

  for (const name of expected) {
    if (!snapshot.names.includes(name)) {
      throw new Error(`${kind}: required filter ${name} not rendered`);
    }
  }

  for (const name of forbidden) {
    if (snapshot.names.includes(name)) {
      throw new Error(`${kind}: forbidden filter ${name} rendered`);
    }
  }

  if (!snapshot.hasGenerate) {
    throw new Error(`${kind}: Generate Report button missing`);
  }

  const check = {
    kind,
    status,
    filters: snapshot.names,
    selectedKind: snapshot.kind
  };

  result.checks.push(check);
  event('kind-pass', check);

  return snapshot;
}

async function verifyDynamicKindChange(page) {
  await page.goto(
    `${BASE}/school/reports?kind=Class`,
    { waitUntil: 'networkidle2', timeout: 60000 });

  const select = await page.$('#kind[data-report-kind-filter]');
  if (!select) {
    throw new Error('dynamic report-kind control missing');
  }

  const options = await page.$$eval(
    '#kind option',
    items => items.map(x => x.value));

  if (!options.includes('Student')) {
    throw new Error('Student report kind is not available to acceptance account');
  }

  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }),
    page.select('#kind', 'Student')
  ]);

  const snapshot = await page.evaluate(() => ({
    url: location.href,
    kind: document.querySelector('#kind')?.value || null,
    names: [...document.querySelectorAll(
      'form[data-report-filter-form] select[name]')]
      .map(x => x.getAttribute('name'))
  }));

  const expected = [
    'kind',
    'academicYearId',
    'classGroupId',
    'studentProfileId'
  ];

  if (snapshot.kind !== 'Student') {
    throw new Error(
      `dynamic kind change did not select Student: ${JSON.stringify(snapshot)}`);
  }

  for (const name of expected) {
    if (!snapshot.names.includes(name)) {
      throw new Error(
        `dynamic Student filter ${name} missing after kind change`);
    }
  }

  for (const name of ['subjectId', 'learningOutcomeId']) {
    if (snapshot.names.includes(name)) {
      throw new Error(
        `dynamic Student page retained stale filter ${name}`);
    }
  }

  result.checks.push({
    kind: 'DynamicClassToStudent',
    filters: snapshot.names
  });

  event('dynamic-kind-pass', snapshot);
}

async function run() {
  let browser;

  try {
    event('start', { base: BASE, account: EMAIL });

    if (!PASSWORD) {
      throw new Error('DEMO_PASSWORD is empty');
    }

    browser = await launch();
    const page = await browser.newPage();

    await login(page);

    await inspectReports(
      page,
      'Class',
      ['kind', 'academicYearId', 'classGroupId'],
      ['subjectId', 'studentProfileId', 'learningOutcomeId']);

    await inspectReports(
      page,
      'Student',
      ['kind', 'academicYearId', 'classGroupId', 'studentProfileId'],
      ['subjectId', 'learningOutcomeId']);

    await inspectReports(
      page,
      'LearningOutcome',
      ['kind', 'academicYearId', 'classGroupId', 'learningOutcomeId'],
      ['subjectId', 'studentProfileId']);

    await inspectReports(
      page,
      'Subject',
      ['kind', 'academicYearId', 'classGroupId', 'subjectId'],
      ['studentProfileId', 'learningOutcomeId']);

    await verifyDynamicKindChange(page);

    result = {
      ...result,
      status: 'PASS',
      completedAt: new Date().toISOString()
    };

    event('done', result);
    await page.close();
  } catch (error) {
    result = {
      ...result,
      status: 'FAIL',
      error: error.stack || error.message,
      completedAt: new Date().toISOString()
    };

    event('fatal', result);
  } finally {
    if (browser) {
      await browser.close().catch(() => {});
    }
  }
}

const app = express();

app.get('/', (_req, res) => {
  res.status(result.status === 'FAIL' ? 500 : 200).json(result);
});

app.listen(PORT, '0.0.0.0', () => {
  console.log(`Phase43 reports acceptance helper listening on ${PORT}`);
  void run();
});
