const express = require('express');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE = (process.env.BASE_URL || 'https://staging.edulytiks.com').replace(/\/$/, '');
const EMAIL = process.env.TEACHER_EMAIL || '';
const PASSWORD = process.env.TEACHER_PASSWORD || '';

const CLASS_ID = 'e11f4a89-ef25-496d-9271-1a7b36400b01';
const SUBJECT_ID = '87acef8a-cfbc-4053-859f-92e98d323e47';
const TERM_ID = '4a055ec2-a739-4db7-bee9-a0513f140e15';
const OUTCOME_ID = 'df429c95-6a67-43c7-9aa1-dcd6db8febc2';
const RUN_TAG = '20260905-X9Q2';
const TITLE = `PDF Acceptance ${RUN_TAG}`;
const PROMPT = `PDF_PROMPT_${RUN_TAG}`;
const ANSWER = `PDF_CORRECT_ANSWER_${RUN_TAG}`;
const SOLUTION = `PDF_SOLUTION_${RUN_TAG}`;

let studentPdf = null;
let answerKeyPdf = null;
let result = {
  status: 'RUNNING',
  startedAt: new Date().toISOString(),
  checks: [],
  fixture: { title: TITLE, prompt: PROMPT, answerSentinel: ANSWER, solutionSentinel: SOLUTION }
};

function event(type, data = {}) {
  console.log('PDF_ACCEPTANCE', JSON.stringify({ at: new Date().toISOString(), type, ...data }));
}

function addCheck(kind, data = {}) {
  result.checks.push({ kind, ...data });
  event(kind, data);
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

  const response = await page.goto(`${BASE}/account/login`, { waitUntil: 'networkidle2', timeout: 60000 });
  if (!response) throw new Error('login page returned no response');
  const retryAfter = response.headers()['retry-after'] || null;
  if (response.status() === 429) throw new Error(`login rate limited (retry-after=${retryAfter || 'unknown'})`);
  if (response.status() >= 400) throw new Error(`login page failed with HTTP ${response.status()}`);

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

  if (page.url().toLowerCase().includes('/account/login')) {
    const message = await page.$eval('.validation-summary, [role="alert"]', x => (x.innerText || '').trim()).catch(() => '');
    throw new Error(`login failed${message ? `: ${message}` : ''}`);
  }

  addCheck('teacher-login', { status: 'PASS', url: page.url() });
}

async function setNamedControl(page, formSelector, name, value) {
  const ok = await page.evaluate(({ formSelector, name, value }) => {
    const form = document.querySelector(formSelector);
    if (!form) return false;
    const control = [...form.elements].find(el => (el.name || '').toLowerCase() === name.toLowerCase());
    if (!control) return false;
    if (control.tagName === 'SELECT') {
      const option = [...control.options].find(o => o.value === value);
      if (!option) return false;
      control.value = value;
      control.dispatchEvent(new Event('change', { bubbles: true }));
      return true;
    }
    control.value = value;
    control.dispatchEvent(new Event('input', { bubbles: true }));
    control.dispatchEvent(new Event('change', { bubbles: true }));
    return true;
  }, { formSelector, name, value });
  if (!ok) throw new Error(`control ${name}=${value} not found in ${formSelector}`);
}

async function submitForm(page, formSelector) {
  await Promise.all([
    page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null),
    page.evaluate(selector => {
      const form = document.querySelector(selector);
      if (!form) throw new Error(`form missing: ${selector}`);
      if (typeof form.requestSubmit === 'function') form.requestSubmit();
      else form.submit();
    }, formSelector)
  ]);
}

async function createAssessment(page) {
  const response = await page.goto(`${BASE}/school/assessments`, { waitUntil: 'networkidle2', timeout: 60000 });
  if (!response || response.status() >= 400) throw new Error(`assessments page failed: ${response?.status()}`);

  const createForm = 'form[action="/school/assessments"],form[action$="/school/assessments"]';
  if (!await page.$(createForm)) throw new Error('teacher create-assessment form missing');

  await setNamedControl(page, createForm, 'classGroupId', CLASS_ID);
  await new Promise(resolve => setTimeout(resolve, 300));
  await setNamedControl(page, createForm, 'subjectId', SUBJECT_ID);
  await setNamedControl(page, createForm, 'termId', TERM_ID);
  await setNamedControl(page, createForm, 'title', TITLE);
  await setNamedControl(page, createForm, 'assessmentDate', '2026-09-05');
  await setNamedControl(page, createForm, 'maxScore', '5');
  await submitForm(page, createForm);

  let assessmentId = null;
  const match = page.url().match(/\/school\/assessments\/([0-9a-f-]{36})(?:$|[/?#])/i);
  if (match) assessmentId = match[1];

  if (!assessmentId) {
    await page.goto(`${BASE}/school/assessments`, { waitUntil: 'networkidle2', timeout: 60000 });
    assessmentId = await page.evaluate(title => {
      for (const a of document.querySelectorAll('a[href]')) {
        const row = a.closest('tr,article,.card,li,div');
        if (row && (row.innerText || '').includes(title)) {
          const m = (a.getAttribute('href') || '').match(/\/school\/assessments\/([0-9a-f-]{36})/i);
          if (m) return m[1];
        }
      }
      return null;
    }, TITLE);
  }

  if (!assessmentId) throw new Error(`assessment creation did not expose an id; current=${page.url()}`);
  result.assessmentId = assessmentId;
  addCheck('create-assessment-ui', { status: 'PASS', assessmentId });
  return assessmentId;
}

async function configureOffline(page, assessmentId) {
  const builderUrl = `${BASE}/school/assessments/${assessmentId}/builder`;
  const response = await page.goto(builderUrl, { waitUntil: 'networkidle2', timeout: 60000 });
  if (!response || response.status() >= 400) throw new Error(`builder failed: ${response?.status()}`);

  const settingsForm = `form[action$="/${assessmentId}/builder/settings"]`;
  if (!await page.$(settingsForm)) throw new Error('delivery settings form missing');
  await setNamedControl(page, settingsForm, 'targetType', '1');
  await setNamedControl(page, settingsForm, 'deliveryMode', '1');
  await setNamedControl(page, settingsForm, 'difficultyBand', '1');
  await submitForm(page, settingsForm);

  const state = await page.evaluate(() => ({
    deliveryValue: [...document.querySelectorAll('select')].find(x => (x.name || '').toLowerCase() === 'deliverymode')?.value || null,
    studentHref: [...document.querySelectorAll('a[href]')].map(a => a.getAttribute('href')).find(h => h && h.endsWith('/student-paper.pdf')) || null,
    keyHref: [...document.querySelectorAll('a[href]')].map(a => a.getAttribute('href')).find(h => h && h.endsWith('/answer-key.pdf')) || null
  }));

  if (state.deliveryValue !== '1') throw new Error(`offline setting not persisted: ${state.deliveryValue}`);
  if (!state.studentHref || !state.keyHref) throw new Error('offline PDF download links are not visible to Teacher');
  addCheck('offline-settings-and-buttons', { status: 'PASS', studentHref: state.studentHref, answerKeyHref: state.keyHref });
}

async function addManualQuestion(page, assessmentId) {
  const builderUrl = `${BASE}/school/assessments/${assessmentId}/builder`;
  await page.goto(builderUrl, { waitUntil: 'networkidle2', timeout: 60000 });
  const manualForm = `form[action$="/${assessmentId}/builder/manual"]`;
  if (!await page.$(manualForm)) throw new Error('manual question form missing');

  await setNamedControl(page, manualForm, 'prompt', PROMPT);
  await setNamedControl(page, manualForm, 'correctAnswer', ANSWER);
  await setNamedControl(page, manualForm, 'solution', SOLUTION);
  await setNamedControl(page, manualForm, 'maxScore', '5');
  await setNamedControl(page, manualForm, 'order', '1');

  const difficultySet = await page.evaluate(selector => {
    const form = document.querySelector(selector);
    if (!form) return false;
    const control = [...form.elements].find(el => (el.name || '').toLowerCase() === 'difficulty');
    if (!control || control.tagName !== 'SELECT') return false;
    const option = [...control.options].find(o => o.value && o.value !== '0');
    if (!option) return false;
    control.value = option.value;
    control.dispatchEvent(new Event('change', { bubbles: true }));
    return true;
  }, manualForm);
  if (!difficultySet) throw new Error('manual difficulty select could not be set');

  const outcomeSet = await page.evaluate(({ selector, outcomeId }) => {
    const form = document.querySelector(selector);
    if (!form) return false;
    const input = [...form.querySelectorAll('input')].find(el =>
      (el.name || '').toLowerCase() === 'outcomeids' && el.value.toLowerCase() === outcomeId.toLowerCase());
    if (!input) return false;
    input.checked = true;
    input.dispatchEvent(new Event('change', { bubbles: true }));
    return true;
  }, { selector: manualForm, outcomeId: OUTCOME_ID });
  if (!outcomeSet) throw new Error('mapped outcome checkbox missing from manual question form');

  await submitForm(page, manualForm);
  const body = await page.evaluate(() => document.body.innerText || '');
  if (!body.includes(PROMPT)) throw new Error('manual question prompt not visible after save');
  addCheck('manual-question-ui', { status: 'PASS', outcomeId: OUTCOME_ID });
}

function cookieHeader(cookies) {
  return cookies.map(c => `${c.name}=${c.value}`).join('; ');
}

async function fetchPdfAuthenticated(page, url, label) {
  const cookies = await page.cookies(BASE);
  const response = await fetch(url, {
    headers: { Cookie: cookieHeader(cookies), Accept: 'application/pdf' },
    redirect: 'manual'
  });
  const contentType = response.headers.get('content-type') || '';
  const bytes = Buffer.from(await response.arrayBuffer());
  if (response.status !== 200) throw new Error(`${label} HTTP ${response.status}`);
  if (!contentType.toLowerCase().includes('application/pdf')) throw new Error(`${label} content-type=${contentType}`);
  if (bytes.length < 500 || bytes.subarray(0, 4).toString('ascii') !== '%PDF') throw new Error(`${label} is not a valid PDF envelope`);
  addCheck(label, { status: 'PASS', httpStatus: response.status, contentType, bytes: bytes.length });
  return bytes;
}

async function unauthenticatedGuard(assessmentId) {
  const url = `${BASE}/school/assessments/${assessmentId}/builder/student-paper.pdf`;
  const response = await fetch(url, { redirect: 'manual', headers: { Accept: 'application/pdf' } });
  const contentType = response.headers.get('content-type') || '';
  if (response.status === 200 && contentType.toLowerCase().includes('application/pdf')) {
    throw new Error('unauthenticated request received Student PDF');
  }
  if (![302, 401, 403].includes(response.status)) throw new Error(`unexpected unauthenticated status ${response.status}`);
  addCheck('unauthenticated-pdf-guard', { status: 'PASS', httpStatus: response.status, location: response.headers.get('location') || null });
}

async function run() {
  let browser;
  try {
    if (!EMAIL) throw new Error('TEACHER_EMAIL is empty');
    if (!PASSWORD) throw new Error('TEACHER_PASSWORD is empty');

    browser = await launch();
    const page = await browser.newPage();
    await login(page);
    const assessmentId = await createAssessment(page);
    await configureOffline(page, assessmentId);
    await addManualQuestion(page, assessmentId);

    const studentUrl = `${BASE}/school/assessments/${assessmentId}/builder/student-paper.pdf`;
    const keyUrl = `${BASE}/school/assessments/${assessmentId}/builder/answer-key.pdf`;
    studentPdf = await fetchPdfAuthenticated(page, studentUrl, 'student-paper-pdf');
    answerKeyPdf = await fetchPdfAuthenticated(page, keyUrl, 'teacher-answer-key-pdf');
    await unauthenticatedGuard(assessmentId);

    result = { ...result, status: 'PASS', completedAt: new Date().toISOString() };
    event('done', { status: result.status, assessmentId, checks: result.checks });
  } catch (error) {
    result = {
      ...result,
      status: 'FAIL',
      error: error.stack || error.message,
      completedAt: new Date().toISOString()
    };
    event('fatal', { status: result.status, assessmentId: result.assessmentId || null, error: result.error });
  } finally {
    if (browser) await browser.close().catch(() => {});
  }
}

const app = express();
app.get('/', (_req, res) => res.status(result.status === 'FAIL' ? 500 : 200).json(result));
app.get('/student.pdf', (_req, res) => {
  if (!studentPdf) return res.status(404).send('not ready');
  res.type('application/pdf').send(studentPdf);
});
app.get('/answer-key.pdf', (_req, res) => {
  if (!answerKeyPdf) return res.status(404).send('not ready');
  res.type('application/pdf').send(answerKeyPdf);
});
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Teacher PDF acceptance helper listening on ${PORT}`);
  void run();
});
