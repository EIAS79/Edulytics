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

async function academicPage(page) {
  const response = await page.goto(`${BASE}/school/academic-structure`, { waitUntil: 'networkidle2', timeout: 60000 });
  if (!response || response.status() >= 400) throw new Error(`academic structure unavailable: ${response?.status()}`);
  const error = await page.$eval('.academic-alert-error', el => el.innerText.trim()).catch(() => '');
  if (error) throw new Error(`academic structure error: ${error}`);
}

async function submitForm(page, actionSuffix, configure) {
  const nav = page.waitForNavigation({ waitUntil: 'networkidle2', timeout: 60000 }).catch(() => null);
  const snapshot = await page.evaluate(({ actionSuffix, configure }) => {
    const forms = [...document.forms];
    const form = forms.find(f => {
      try { return new URL(f.action).pathname.toLowerCase().endsWith(actionSuffix.toLowerCase()); }
      catch { return false; }
    });
    if (!form) {
      return { ok: false, reason: 'form-not-found', actions: forms.map(f => f.action) };
    }

    const setValue = (name, value) => {
      const el = form.querySelector(`[name="${name}"]`);
      if (!el) throw new Error(`missing field ${name}`);
      el.disabled = false;
      el.value = value;
      el.dispatchEvent(new Event('input', { bubbles: true }));
      el.dispatchEvent(new Event('change', { bubbles: true }));
    };

    const firstValue = name => {
      const el = form.querySelector(`[name="${name}"]`);
      if (!el) throw new Error(`missing field ${name}`);
      if (el.tagName === 'SELECT') {
        const option = [...el.options].find(o => o.value);
        if (!option) throw new Error(`no option for ${name}`);
        option.disabled = false;
        option.hidden = false;
        el.disabled = false;
        el.value = option.value;
        return option.value;
      }
      return el.value;
    };

    try {
      if (configure.kind === 'year') {
        setValue('name', configure.name);
        setValue('startsOn', '2026-09-01');
        setValue('endsOn', '2027-06-30');
        firstValue('status');
      } else if (configure.kind === 'program') {
        setValue('academicYearId', configure.academicYearId);
        setValue('programChoice', 'american');
      } else if (configure.kind === 'level') {
        setValue('academicYearId', configure.academicYearId);
        const program = form.querySelector('[name="academicProgramId"]');
        if (!program) throw new Error('missing academicProgramId');
        const programOption = [...program.options].find(o => o.value && (o.dataset.offeredYears || '').split('|').includes(configure.academicYearId)) || [...program.options].find(o => o.value);
        if (!programOption) throw new Error('no program option');
        programOption.disabled = false;
        programOption.hidden = false;
        program.disabled = false;
        program.value = programOption.value;
        program.dispatchEvent(new Event('change', { bubbles: true }));

        const level = form.querySelector('[name="curriculumLevelKey"]');
        if (!level) throw new Error('missing curriculumLevelKey');
        const levelOption = [...level.options].find(o => o.value && o.dataset.programId === programOption.value) || [...level.options].find(o => o.value);
        if (!levelOption) throw new Error('no curriculum level option');
        levelOption.disabled = false;
        levelOption.hidden = false;
        level.disabled = false;
        level.value = levelOption.value;
      } else if (configure.kind === 'class') {
        setValue('academicYearId', configure.academicYearId);
        firstValue('curriculumAdoptionId');
        setValue('name', configure.name);
        firstValue('status');
      }
    } catch (error) {
      return {
        ok: false,
        reason: error.message,
        action: form.action,
        controls: [...form.elements].map(el => ({ name: el.name, value: el.value, disabled: el.disabled, tag: el.tagName }))
      };
    }

    const controls = [...form.elements].map(el => ({ name: el.name, value: el.value, disabled: el.disabled, tag: el.tagName }));
    form.requestSubmit();
    return { ok: true, action: form.action, controls };
  }, { actionSuffix, configure });

  if (!snapshot.ok) {
    throw new Error(`academic submit ${actionSuffix} failed before POST: ${JSON.stringify(snapshot)}`);
  }
  await nav;
  const error = await page.$eval('.academic-alert-error', el => el.innerText.trim()).catch(() => '');
  event('academic-submit', { actionSuffix, snapshot, finalUrl: page.url(), error });
  if (error) throw new Error(`academic submit ${actionSuffix} returned error: ${error}`);
}

async function ensureActiveClass(page) {
  await academicPage(page);

  let existingClassCount = await page.evaluate(() => {
    const form = [...document.forms].find(f => {
      try { return new URL(f.action).pathname.toLowerCase().endsWith('/curriculum-classes'); }
      catch { return false; }
    });
    if (!form) return -1;
    const adoption = form.querySelector('[name="curriculumAdoptionId"]');
    return adoption && adoption.tagName === 'SELECT' ? [...adoption.options].filter(o => o.value).length : 0;
  });

  const existingYearId = await page.evaluate(() => {
    const select = document.querySelector('#level-year');
    return select ? [...select.options].find(o => o.value)?.value || null : null;
  });

  let academicYearId = existingYearId;
  if (!academicYearId) {
    await submitForm(page, '/academic-years', { kind: 'year', name: `Smoke AY ${runId}` });
    academicYearId = await page.evaluate(() => {
      const select = document.querySelector('#level-year');
      return select ? [...select.options].find(o => o.value)?.value || null : null;
    });
    if (!academicYearId) throw new Error('academic year not visible after creation');
  }

  const offered = await page.evaluate(yearId => {
    const select = document.querySelector('#program-choice');
    if (!select) return false;
    return [...select.options].some(o => o.value && (o.dataset.offeredYears || '').split('|').includes(yearId));
  }, academicYearId);
  if (!offered) {
    await submitForm(page, '/academic-programs', { kind: 'program', academicYearId });
  }

  const hasAdoption = await page.evaluate(yearId => {
    const form = [...document.forms].find(f => {
      try { return new URL(f.action).pathname.toLowerCase().endsWith('/curriculum-classes'); }
      catch { return false; }
    });
    if (!form) return false;
    const adoption = form.querySelector('[name="curriculumAdoptionId"]');
    return !!adoption && [...adoption.options].some(o => o.value);
  }, academicYearId);
  if (!hasAdoption) {
    await submitForm(page, '/curriculum-levels', { kind: 'level', academicYearId });
  }

  await submitForm(page, '/curriculum-classes', { kind: 'class', academicYearId, name: `Smoke Class ${runId}` });

  const active = await page.goto(`${BASE}/School/Users/Student-Classes`, { waitUntil: 'networkidle2', timeout: 60000 });
  const body = await page.evaluate(() => document.body.innerText);
  event('academic-class-ready', { status: active?.status() || null, body });
  if (!active || active.status() !== 200) throw new Error(`Student-Classes status ${active?.status()}`);
  let parsed;
  try { parsed = JSON.parse(body); } catch { throw new Error(`Student-Classes not JSON: ${body}`); }
  if (!Array.isArray(parsed) || parsed.length === 0) throw new Error('Student-Classes still empty after academic setup');
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
  const errors = await page.evaluate(() => [...document.querySelectorAll('.validation-summary-errors,.user-field-error,.alert-danger,.academic-alert-error')]
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
    await ensureActiveClass(page);
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
