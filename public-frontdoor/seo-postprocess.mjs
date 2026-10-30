#!/usr/bin/env node
// Marketing-only SEO postprocessor. Never reaches the database or the ASP.NET app.
import { readFileSync, writeFileSync, readdirSync, statSync, existsSync } from 'node:fs';
import { resolve, join, relative, sep } from 'node:path';

const out = resolve(process.env.EDULYTIKS_FRONTDOOR_DIST || 'public-frontdoor/dist');
const base = 'https://edulytiks.com';
const image = base + '/images/brand/edulityks-pl.png';
const check = process.argv.includes('--check');
if (!existsSync(out)) throw new Error('Missing public frontdoor output: ' + out);

const pages = [];
function walk(directory) {
  for (const item of readdirSync(directory)) {
    const full = join(directory, item);
    if (statSync(full).isDirectory()) {
      if (!['lib', 'node_modules', '.git'].includes(item)) walk(full);
    } else if (item === 'index.html') {
      pages.push(full);
    }
  }
}
walk(out);

const homeRoutes = new Set(['/', '/pl/', '/en/', '/ar/']);
const normalized = str => String(str || '').replace(/\s+/g, ' ').trim();
const cleanText = str => normalized(String(str || '')
  .replace(/<script\b[^>]*>[\s\S]*?<\/script>/gi, '')
  .replace(/<style\b[^>]*>[\s\S]*?<\/style>/gi, '')
  .replace(/<[^>]*>/g, ' ')
  .replace(/&#x([0-9a-f]+);/gi, (match, code) => {
    try { return String.fromCodePoint(parseInt(code, 16)); } catch { return match; }
  })
  .replace(/&#([0-9]+);/g, (match, code) => {
    try { return String.fromCodePoint(parseInt(code, 10)); } catch { return match; }
  })
  .replace(/&nbsp;/gi, ' ').replace(/&amp;/gi, '&')
  .replace(/&quot;/gi, '"').replace(/&#39;|&apos;/gi, "'"));
const escapeAttr = value => String(value).replace(/&/g, '&amp;')
  .replace(/"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
const routeFor = file => {
  const fragment = relative(out, file).split(sep).join('/');
  return fragment === 'index.html' ? '/' : '/' + fragment.replace(/\/index\.html$/, '') + '/';
};
const canonicalRoute = route => route === '/pl/' ? '/' : route;
const langFor = route => route === '/en/' ? 'en' : route === '/ar/' ? 'ar' : 'pl';
const localeFor = lang => ({pl:'pl_PL',en:'en_US',ar:'ar_AR'})[lang];
const publicPage = route => route !== '/account/login/' && route !== '/pl/' &&
  !route.startsWith('/__frontdoor-live/');
const getTitle = html => cleanText(html.match(/<title\b[^>]*>([\s\S]*?)<\/title>/i)?.[1] || 'Edulytics');
const getH1 = html => cleanText(html.match(/<h1\b[^>]*>([\s\S]*?)<\/h1>/i)?.[1] || '');
const getDescription = html => {
  const tag = html.match(/<meta\b(?=[^>]*\bname\s*=\s*["']description["'])[^>]*>/i)?.[0] || '';
  return cleanText(tag.match(/\bcontent\s*=\s*["']([^"']*)["']/i)?.[1] || '');
};
const fallbackDescription = (route, html, lang) => {
  const heading = getH1(html);
  if (lang === 'en') return 'Explore Edulytics: curriculum-aligned mathematics lessons, assessments and student practice for schools and teachers.';
  if (lang === 'ar') return 'تعرّف على Edulytics لتعليم الرياضيات والتقييم والتدريب الفردي للطلاب وإدارة العملية التعليمية في المدارس.';
  const topic = heading && heading.length > 7 ? heading : getTitle(html).split('|')[1] || 'nauczanie matematyki';
  return normalized(topic + '. Poznaj możliwości Edulytics w nauczaniu matematyki, ocenianiu i analizie postępów uczniów.').slice(0, 159);
};
function descriptionFor(route, html, lang) {
  const existing = getDescription(html);
  const generic = /Edulytics to zgodna z programem platforma/i.test(existing) ||
    /Edulytics is a curriculum-aligned mathematics learning/i.test(existing);
  if (!existing || (route !== '/' && route !== '/en/' && route !== '/ar/' && generic))
    return fallbackDescription(route, html, lang);
  return existing.slice(0, 159);
}
function jsonLd(route, lang, title, description) {
  if (!homeRoutes.has(route)) return '';
  const website = {
    '@context':'https://schema.org',
    '@graph':[
      {'@type':'Organization','@id':base+'/#organization',name:'OUR-CS Sp. z o.o.',
        url:'https://our-cs.com/',email:'info@our-cs.com'},
      {'@type':'WebSite','@id':base+'/#website',name:'Edulytics',url:base+'/',
        inLanguage:['pl','en','ar'],publisher:{'@id':base+'/#organization'}},
      {'@type':'SoftwareApplication',name:'Edulytics',
        url:base+'/',applicationCategory:'EducationalApplication',
        operatingSystem:'Web',description,
        publisher:{'@id':base+'/#organization'}}
    ]
  };
  return '<script type="application/ld+json">' +
    JSON.stringify(website).replace(/</g, '\\u003c') + '</script>';
}
function tags(route, html) {
  const lang = langFor(route), canonical = base + canonicalRoute(route);
  const title = getTitle(html), description = descriptionFor(route, html, lang);
  const pieces = [
    '<link rel="canonical" href="' + escapeAttr(canonical) + '">',
    '<meta name="robots" content="' + (route === '/account/login/' ? 'noindex,follow' : 'index,follow') + '">',
    '<meta name="description" content="' + escapeAttr(description) + '">',
    '<meta property="og:type" content="website">',
    '<meta property="og:site_name" content="Edulytics">',
    '<meta property="og:locale" content="' + localeFor(lang) + '">',
    '<meta property="og:title" content="' + escapeAttr(title) + '">',
    '<meta property="og:description" content="' + escapeAttr(description) + '">',
    '<meta property="og:url" content="' + escapeAttr(canonical) + '">',
    '<meta property="og:image" content="' + escapeAttr(image) + '">',
    '<meta property="og:image:alt" content="Edulytics education platform">',
    '<meta name="twitter:card" content="summary_large_image">',
    '<meta name="twitter:title" content="' + escapeAttr(title) + '">',
    '<meta name="twitter:description" content="' + escapeAttr(description) + '">',
    '<meta name="twitter:image" content="' + escapeAttr(image) + '">'
  ];
  if (homeRoutes.has(route)) {
    for (const [language, path] of [['pl','/'],['en','/en/'],['ar','/ar/'],['x-default','/']])
      pieces.push('<link rel="alternate" hreflang="' + language + '" href="' + base + path + '">');
  }
  pieces.push(jsonLd(route, lang, title, description));
  return '<!-- edulytiks-static-seo:start -->\n' + pieces.filter(Boolean).join('\n') +
    '\n<!-- edulytiks-static-seo:end -->';
}
const metaTag = name => new RegExp('<meta\\b(?=[^>]*\\bname\\s*=\\s*["\\x27]' + name + '["\\x27])[^>]*>', 'gi');
function prepare(html, route) {
  // Capture the original title/description before removing old SEO tags.
  const seoTags = tags(route, html);
  const dropTag = /<link\b(?=[^>]*\brel\s*=\s*["'](?:canonical|alternate)["'])[^>]*>/gi;
  const dropOg = /<meta\b(?=[^>]*(?:\bproperty\s*=\s*["']og:|\bname\s*=\s*["']twitter:))[^>]*>/gi;
  html = html.replace(/<!-- edulytiks-static-seo:start -->[\s\S]*?<!-- edulytiks-static-seo:end -->/g, '')
    .replace(dropTag, '').replace(dropOg, '').replace(metaTag('description'), '')
    .replace(metaTag('robots'), '');
  if (!/<\/head>/i.test(html)) throw new Error('Missing </head> in ' + route);
  return html.replace(/<\/head>/i, seoTags + '\n</head>');
}
function validate() {
  const failures = [];
  const urls = [];
  for (const file of pages) {
    const route = routeFor(file), html = readFileSync(file, 'utf8');
    const expected = base + canonicalRoute(route);
    if (!html.includes('rel="canonical" href="' + expected + '"')) failures.push(route + ' canonical');
    if (!html.includes('property="og:image"')) failures.push(route + ' open graph image');
    if (!html.includes('name="description"')) failures.push(route + ' description');
    if ((html.match(/rel="canonical"/g)||[]).length !== 1) failures.push(route+' duplicate canonical');
    if ((html.match(/name="description"/g)||[]).length !== 1) failures.push(route+' duplicate description');
    if (route === '/account/login/' && !html.includes('noindex,follow')) failures.push('Login must remain noindex');
    if (homeRoutes.has(route)) for (const code of ['pl','en','ar','x-default'])
      if (!html.includes('hreflang="' + code + '"')) failures.push(route + ' missing hreflang ' + code);
    if (publicPage(route)) urls.push(expected);
  }
  const sitemap = readFileSync(join(out,'sitemap.xml'),'utf8');
  for (const url of urls) if (!sitemap.includes('<loc>' + url + '</loc>')) failures.push('sitemap missing '+url);
  if (sitemap.includes('/account/login/') || sitemap.includes('<loc>'+base+'/pl/</loc>'))
    failures.push('sitemap includes noncanonical/login');
  const robots = readFileSync(join(out,'robots.txt'),'utf8');
  if (!robots.includes('Sitemap: '+base+'/sitemap.xml') ||
      !robots.includes('Disallow: /__frontdoor-live/')) failures.push('robots.txt');
  if (failures.length) throw new Error(failures.join('; '));
  console.log('Edulytiks static SEO validation passed: '+pages.length+' static HTML pages.');
}
if (check) {
  validate();
} else {
  for (const file of pages) {
    const route = routeFor(file), old = readFileSync(file,'utf8');
    writeFileSync(file, prepare(old, route));
  }
  const urls = [...new Set(pages.map(routeFor).filter(publicPage).map(r => base + canonicalRoute(r)))].sort();
  const xml = '<?xml version="1.0" encoding="UTF-8"?>\n' +
    '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n' +
    urls.map(u=> '  <url><loc>'+escapeAttr(u)+'</loc></url>').join('\n') +
    '\n</urlset>\n';
  writeFileSync(join(out,'sitemap.xml'),xml);
  writeFileSync(join(out,'robots.txt'),
    'User-agent: *\nAllow: /\nDisallow: /__frontdoor-live/\nDisallow: /account/\nDisallow: /api/\nSitemap: '+base+'/sitemap.xml\n');
  console.log('Generated Edulytiks marketing metadata + sitemap for '+urls.length+' URLs.');
}
