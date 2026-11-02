(() => {
  const form = document.querySelector('[data-role-gated-login]');
  const roleInputs = [...document.querySelectorAll('.ed-login-v28-role-input')];
  if (!form || roleInputs.length === 0) return;

  const controls = [...form.querySelectorAll('[data-role-gated-control]')];
  const helper = document.querySelector('[data-login-gated-copy]');

  const frontdoorLanguageKey = 'edulytics.frontdoor.language';
  let language = (document.documentElement.lang || 'en').toLowerCase();
  try {
    const selected = window.localStorage.getItem(frontdoorLanguageKey)
      || window.localStorage.getItem('edulytics.public.siteLanguage');
    if (selected === 'en' || selected === 'pl' || selected === 'ar') language = selected;
  } catch { }
  language = language.startsWith('ar') ? 'ar' : language.startsWith('pl') ? 'pl' : 'en';

  const copy = {
    en: {
      title: 'Sign in', help: 'Need help signing in?', contact: 'Contact us',
      choose: 'Choose your account type', chooseBody: 'School users select the role registered for this account.',
      roles: ['School administrator','Subject supervisor','Teacher','Student'],
      locked: 'School users choose an account type above.', ready: 'Enter the credentials for the selected account type.',
      email: 'Email address', emailPlaceholder: 'Enter your email address',
      password: 'Password', passwordPlaceholder: 'Enter your password', signIn: 'Sign in',
      access: 'School accounts must match the selected role.', change: 'Change language',
      proofTitle: 'Built for connected learning',
      proofBody: 'Curriculum, teaching, assessment and progress stay connected in one mathematics platform.',
      proof: ['Curriculum-aligned learning','Teachers control formal assessment','Practice supports learning','Results guide the next step'],
      footer: 'Contact us'
    },
    pl: {
      title: 'Logowanie', help: 'Potrzebujesz pomocy z logowaniem?', contact: 'Skontaktuj się',
      choose: 'Wybierz typ konta', chooseBody: 'Użytkownicy szkolni wybierają rolę przypisaną do konta.',
      roles: ['Administrator szkoły','Opiekun przedmiotu','Nauczyciel','Uczeń'],
      locked: 'Użytkownicy szkolni wybierają typ konta powyżej.', ready: 'Wpisz dane logowania dla wybranego typu konta.',
      email: 'Adres e-mail', emailPlaceholder: 'Wprowadź adres e-mail',
      password: 'Hasło', passwordPlaceholder: 'Wprowadź hasło', signIn: 'Zaloguj się',
      access: 'Konta szkolne muszą odpowiadać wybranej roli.', change: 'Zmień język',
      proofTitle: 'Stworzone dla spójnego uczenia się',
      proofBody: 'Program, nauczanie, ocenianie i postępy pozostają połączone w jednej platformie matematycznej.',
      proof: ['Nauka zgodna z programem','Nauczyciel kontroluje formalne ocenianie','Ćwiczenia wspierają naukę','Wyniki prowadzą do kolejnego kroku'],
      footer: 'Skontaktuj się'
    },
    ar: {
      title: 'تسجيل الدخول', help: 'هل تحتاج مساعدة في تسجيل الدخول؟', contact: 'تواصل معنا',
      choose: 'اختر نوع حسابك', chooseBody: 'يختار مستخدمو المدرسة الدور المسجل لهذا الحساب.',
      roles: ['مدير المدرسة','مشرف المادة','معلم','طالب'],
      locked: 'اختر نوع الحساب من القائمة أعلاه.', ready: 'أدخل بيانات الدخول لنوع الحساب المحدد.',
      email: 'البريد الإلكتروني', emailPlaceholder: 'أدخل بريدك الإلكتروني',
      password: 'كلمة المرور', passwordPlaceholder: 'أدخل كلمة المرور', signIn: 'تسجيل الدخول',
      access: 'يجب أن يتطابق حساب المدرسة مع الدور المحدد.', change: 'تغيير اللغة',
      proofTitle: 'مصمم لتعلّم مترابط',
      proofBody: 'المنهج والتدريس والتقييم والتقدم مترابطة في منصة رياضيات واحدة.',
      proof: ['تعلّم متوافق مع المنهج','المعلمون يتحكمون في التقييم الرسمي','التدريب يدعم التعلّم','النتائج توجه الخطوة التالية'],
      footer: 'تواصل معنا'
    }
  }[language];

  document.documentElement.lang = language;
  document.documentElement.dir = language === 'ar' ? 'rtl' : 'ltr';
  document.title = `${copy.title} | ${language === 'pl' ? 'Edulityks' : 'Edulytics'}`;

  const languageBadge = document.querySelector('.ed-login-v28-language');
  if (languageBadge) {
    const badgeText = languageBadge.querySelector(':scope > span:last-child');
    if (badgeText) badgeText.textContent = language.toUpperCase();
    const flag = languageBadge.querySelector('.ed-login-v28-flag');
    if (flag) flag.hidden = language !== 'pl';
  }

  const topHelp = document.querySelector('.ed-login-v28-top-help');
  if (topHelp) {
    const helpLink = topHelp.querySelector('a');
    if (topHelp.firstChild) topHelp.firstChild.nodeValue = `${copy.help} `;
    if (helpLink) helpLink.textContent = copy.contact;
  }

  const heading = document.querySelector('#account-type-heading');
  const headingBody = heading?.parentElement?.querySelector('p');
  if (heading) heading.textContent = copy.choose;
  if (headingBody) headingBody.textContent = copy.chooseBody;

  document.querySelectorAll('.ed-login-v28-audience strong').forEach((node, index) => {
    if (copy.roles[index]) node.textContent = copy.roles[index];
  });

  const authTitle = document.querySelector('.auth-heading h1');
  if (authTitle) authTitle.textContent = copy.title;
  if (helper) {
    helper.dataset.lockedText = copy.locked;
    helper.dataset.readyText = copy.ready;
  }

  const fields = form.querySelectorAll('.field-group');
  const emailLabel = fields[0]?.querySelector('label');
  const emailInput = fields[0]?.querySelector('input');
  const passwordLabel = fields[1]?.querySelector('label');
  const passwordInput = fields[1]?.querySelector('input');
  const submit = form.querySelector('[data-role-gated-submit]');
  if (emailLabel) emailLabel.textContent = copy.email;
  if (emailInput) {
    emailInput.placeholder = copy.emailPlaceholder;
    emailInput.setAttribute('aria-label', copy.email);
  }
  if (passwordLabel) passwordLabel.textContent = copy.password;
  if (passwordInput) {
    passwordInput.placeholder = copy.passwordPlaceholder;
    passwordInput.setAttribute('aria-label', copy.password);
  }
  if (submit) {
    submit.textContent = copy.signIn;
    submit.setAttribute('aria-label', copy.signIn);
  }

  const accessNote = document.querySelector('.ed-login-v28-access-note');
  if (accessNote) {
    const change = accessNote.querySelector('a');
    if (accessNote.firstChild) accessNote.firstChild.nodeValue = `${copy.access} `;
    if (change) change.textContent = copy.change;
  }

  const proof = document.querySelector('.ed-login-v28-proof');
  const proofTitle = proof?.querySelector('h2');
  const proofBody = proof?.querySelector('.ed-login-v28-proof-footer');
  if (proofTitle) proofTitle.textContent = copy.proofTitle;
  if (proofBody) proofBody.textContent = copy.proofBody;
  proof?.querySelectorAll('.ed-login-v28-proof-item > span:last-child').forEach((node, index) => {
    if (copy.proof[index]) node.textContent = copy.proof[index];
  });

  const footerLink = document.querySelector('.ed-auth-support-footer a');
  if (footerLink) footerLink.textContent = copy.footer;

  const syncState = ({ focusEmail = false } = {}) => {
    const selected = roleInputs.find(input => input.checked);
    const hasSchoolRole = Boolean(selected);

    document.querySelectorAll('.ed-login-v28-audience').forEach(card => {
      const input = document.getElementById(card.htmlFor);
      const active = Boolean(input?.checked);
      card.classList.toggle('is-selected', active);
      card.setAttribute('aria-pressed', String(active));
    });

    if (helper) {
      helper.textContent = hasSchoolRole
        ? helper.dataset.readyText || helper.textContent
        : helper.dataset.lockedText || helper.textContent;
    }

    if (hasSchoolRole && focusEmail) {
      controls[0]?.focus();
    }
  };

  roleInputs.forEach(input => {
    input.addEventListener('change', () => syncState({ focusEmail: true }));
  });

  // Credentials stay available even when no public school role is selected.
  // The backend permits that path only for the internal platform administrator;
  // every school user is still required to select a matching public role.
  syncState();
})();
