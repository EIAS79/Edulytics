(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const normalize = value => String(value || '').replace(/\s+/g, ' ').trim().replace(/\s*→\s*$/, '');

  const directRoutes = new Map([
    ['Request a demo', '/contact/request-demo'],
    ['Poproś o demo', '/contact/request-demo'],
    ['اطلب عرضًا توضيحيًا', '/contact/request-demo'],
    ['Request the demo', '/contact/request-demo'],
    ['Poproś o prezentację', '/contact/request-demo'],
    ['اطلب العرض', '/contact/request-demo'],
    ['Try Edulytics', '/contact/request-demo'],
    ['Wypróbuj Edulytics', '/contact/request-demo'],
    ['جرّب Edulytics', '/contact/request-demo'],
    ['Talk to us', '/contact/sales-enquiry'],
    ['Porozmawiaj z nami', '/contact/sales-enquiry'],
    ['تحدث معنا', '/contact/sales-enquiry'],
    ['Ask now — for teachers', '/contact/sales-enquiry?audience=teacher'],
    ['Zapytaj teraz — dla nauczycieli', '/contact/sales-enquiry?audience=teacher'],
    ['اسأل الآن — للمعلمين', '/contact/sales-enquiry?audience=teacher'],
    ['Sales enquiry', '/contact/sales-enquiry'],
    ['Zapytanie sprzedażowe', '/contact/sales-enquiry'],
    ['استفسار المبيعات', '/contact/sales-enquiry'],
    ['Help center', '/help'],
    ['Centrum pomocy', '/help'],
    ['مركز المساعدة', '/help'],
    ['Contact', '/contact'],
    ['Kontakt', '/contact'],
    ['تواصل معنا', '/contact']
  ]);

  root.querySelectorAll('a').forEach(anchor => {
    const label = normalize(anchor.textContent);
    const route = directRoutes.get(label);
    if (route) anchor.setAttribute('href', route);

    const href = anchor.getAttribute('href') || '';
    if (/^mailto:.*(?:demo|prezent|عرض)/i.test(href)) {
      anchor.setAttribute('href', '/contact/request-demo');
    }
    if (href === '#contact') anchor.setAttribute('href', '/contact');

    if (label === 'Global Partnerships') {
      anchor.textContent = 'Partnerships';
      anchor.setAttribute('href', '/company/partnerships');
    } else if (label === 'Partnerstwa globalne') {
      anchor.textContent = 'Partnerstwa';
      anchor.setAttribute('href', '/company/partnerships');
    } else if (label === 'الشراكات العالمية') {
      anchor.textContent = 'الشراكات';
      anchor.setAttribute('href', '/company/partnerships');
    }
  });

  root.querySelectorAll('.audience-card').forEach(card => {
    const heading = normalize(card.querySelector('h3')?.textContent).toLowerCase();
    const link = card.querySelector('a');
    if (!link) return;

    if (/school|szko|مدرس/.test(heading)) link.setAttribute('href', '/schools/overview');
    else if (/teacher|nauczyc|معلم/.test(heading)) link.setAttribute('href', '/teachers/overview');
    else if (/parent|rodzic|ولي|أولياء/.test(heading)) link.setAttribute('href', '/parents/overview');
    else if (/student|uczni|طالب|طلاب/.test(heading)) link.setAttribute('href', '/students/overview');
    else if (/leader|lider|قياد/.test(heading)) link.setAttribute('href', '/schools/overview');
  });
})();
