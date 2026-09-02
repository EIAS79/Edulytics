const express = require('express');
const fs = require('fs');
const path = require('path');
const puppeteer = require('puppeteer-core');

const PORT = process.env.PORT || 10000;
const BASE_URL = (process.env.BASE_URL || 'https://staging.edulytiks.com').replace(/\/$/, '');
const PASSWORD = process.env.DEMO_PASSWORD || '';
const MODE = process.env.CAPTURE_MODE || 'prepare';
const SCHOOL_ID = process.env.DEMO_SCHOOL_ID || 'f85dd737-3ca3-4027-ac6e-e5a06c8c7b93';
const SUPERADMIN_EMAIL = process.env.SUPERADMIN_EMAIL || 'info.ourcs@gmail.com';
const OUT = path.join(process.cwd(), 'manual-capture-output');
const SHOTS = path.join(OUT, 'shots');
fs.mkdirSync(SHOTS, { recursive: true });

const USERS = [
  { role: 'school-administrator', email: 'demo.admin@edulytiks.com' },
  { role: 'subject-supervisor', email: 'demo.supervisor@edulytiks.com' },
  { role: 'teacher', email: 'demo.teacher@edulytiks.com' },
  { role: 'student', email: 'demo.student@edulytiks.com' }
];
const manifest = { mode: MODE, baseUrl: BASE_URL, startedAt: new Date().toISOString(), status: 'starting', events: [], screenshots: [] };
function saveManifest(){ fs.writeFileSync(path.join(OUT,'manifest.json'), JSON.stringify(manifest,null,2)); }
function event(type,data={}){ const x={at:new Date().toISOString(),type,...data}; manifest.events.push(x); console.log('MANUAL_CAPTURE',JSON.stringify(x)); saveManifest(); }
function slug(s){ return String(s||'').toLowerCase().replace(/[^a-z0-9]+/g,'-').replace(/^-|-$/g,'').slice(0,80)||'screen'; }

async function makeBrowser(){
  const mod = await import('@sparticuz/chromium');
  const chromium = mod.default || mod;
  const executablePath = await chromium.executablePath();
  const args = [...chromium.args, '--disable-dev-shm-usage'];
  return puppeteer.launch({ executablePath, args, headless: 'shell', defaultViewport:{width:1440,height:900} });
}
async function setCulture(page,culture){ const u=new URL(BASE_URL); await page.setCookie({name:'Edulytics.Culture',value:culture,domain:u.hostname,path:'/',secure:true,sameSite:'Strict'}); }
async function login(page,email,culture='en'){
  await setCulture(page,culture);
  await page.goto(`${BASE_URL}/account/login`,{waitUntil:'networkidle2',timeout:60000});
  const emailInput=await page.$('input[type="email"], input[name="Email"], input[name$=".Email"]');
  const passwordInput=await page.$('input[type="password"]');
  if(!emailInput||!passwordInput) throw new Error(`Login inputs not found at ${page.url()}`);
  await emailInput.click({clickCount:3}); await emailInput.type(email); await passwordInput.type(PASSWORD);
  const submit=await page.$('button[type="submit"], input[type="submit"]'); if(!submit) throw new Error('Login submit not found');
  await Promise.all([page.waitForNavigation({waitUntil:'networkidle2',timeout:60000}).catch(()=>null),submit.click()]);
  const ok=!page.url().includes('/account/login'); event('login',{email,culture,ok,url:page.url()}); return ok;
}
async function requestPasswordLinks(browser){
  const page=await browser.newPage();
  if(!(await login(page,SUPERADMIN_EMAIL,'en'))) throw new Error('SuperAdmin login failed');
  for(const user of USERS){
    try{
      await page.goto(`${BASE_URL}/School/Users?schoolId=${encodeURIComponent(SCHOOL_ID)}`,{waitUntil:'networkidle2',timeout:60000});
      const href=await page.evaluate(email=>{ const links=[...document.querySelectorAll('a[href]')]; const a=links.find(x=>(x.textContent||'').toLowerCase().includes(email.toLowerCase())||(x.closest('tr')?.textContent||'').toLowerCase().includes(email.toLowerCase())); return a?a.href:null; },user.email);
      if(!href){event('password-link-user-not-found',{email:user.email});continue;}
      await page.goto(href,{waitUntil:'networkidle2',timeout:60000});
      const submitted=await page.evaluate(()=>{ const form=[...document.querySelectorAll('form')].find(f=>(f.getAttribute('action')||'').includes('Password-Link')); if(!form)return false; form.requestSubmit?form.requestSubmit():form.submit(); return true; });
      if(!submitted){event('password-link-form-not-found',{email:user.email,url:page.url()});continue;}
      await page.waitForNavigation({waitUntil:'networkidle2',timeout:60000}).catch(()=>null); event('password-link-requested',{email:user.email,url:page.url()});
    }catch(e){event('password-link-error',{email:user.email,error:e.message});}
  }
  await page.close();
}
async function applyResetLinks(browser){
  let links={}; try{links=JSON.parse(process.env.RESET_LINKS_JSON||'{}');}catch{throw new Error('RESET_LINKS_JSON is invalid JSON');}
  for(const user of USERS){ const url=links[user.email]; if(!url){event('reset-link-missing',{email:user.email});continue;} const page=await browser.newPage(); try{
    await page.goto(url,{waitUntil:'networkidle2',timeout:60000}); const inputs=await page.$$('input[type="password"]'); if(!inputs.length)throw new Error(`No password inputs at ${page.url()}`); for(const input of inputs)await input.type(PASSWORD); const submit=await page.$('button[type="submit"], input[type="submit"]'); if(!submit)throw new Error('Password submit not found'); await Promise.all([page.waitForNavigation({waitUntil:'networkidle2',timeout:60000}).catch(()=>null),submit.click()]); event('password-reset-applied',{email:user.email,url:page.url()});
  }catch(e){event('password-reset-error',{email:user.email,error:e.message});}finally{await page.close();} }
}
async function shot(page,locale,role,index,label){ const filename=`${locale}_${role}_${String(index).padStart(2,'0')}_${slug(label)}.jpg`; await page.screenshot({path:path.join(SHOTS,filename),type:'jpeg',quality:72,fullPage:false}); const info=await page.evaluate(()=>({title:document.title,h1:document.querySelector('h1')?.innerText?.trim()||'',h2:document.querySelector('h2')?.innerText?.trim()||''})); manifest.screenshots.push({locale,role,filename,url:page.url(),label,...info}); saveManifest(); event('screenshot',{locale,role,filename,url:page.url(),title:info.title,h1:info.h1}); }
function goodHref(href){ try{const u=new URL(href,BASE_URL),b=new URL(BASE_URL); if(u.origin!==b.origin)return false; if(u.pathname.startsWith('/account/logout')||u.pathname.startsWith('/account/login')||u.pathname.startsWith('/set-culture'))return false; if(/\.(css|js|png|jpg|jpeg|svg|ico|woff2?)$/i.test(u.pathname))return false; return true;}catch{return false;} }
async function captureRole(browser,email,role,locale){ const page=await browser.newPage(); try{ if(!(await login(page,email,locale))){event('capture-login-failed',{email,role,locale});return;} let idx=1; await shot(page,locale,role,idx++,'dashboard'); let links=await page.evaluate(()=>[...document.querySelectorAll('a[href]')].map(a=>({href:a.href,text:(a.textContent||'').trim()})).filter(x=>x.href)); const priority=/dashboard|school|user|academic|program|curriculum|class|assessment|report|analytic|student|lesson|subject|supervisor|teacher|result|progress|mastery|platform|subscription|billing/i; links=links.filter(x=>goodHref(x.href)).sort((a,b)=>Number(priority.test(b.text))-Number(priority.test(a.text))); const seen=new Set([new URL(page.url()).pathname]); for(const link of links){ if(idx>12)break; let u;try{u=new URL(link.href);}catch{continue;} const key=u.pathname+u.search;if(seen.has(key))continue;seen.add(key);try{const resp=await page.goto(link.href,{waitUntil:'networkidle2',timeout:45000});if(resp&&resp.status()>=400)continue;if(page.url().includes('/account/login'))break;if(!(await page.$('body')))continue;await shot(page,locale,role,idx++,link.text||u.pathname);}catch(e){event('crawl-skip',{role,locale,href:link.href,error:e.message});} } }finally{await page.close();} }
async function run(){ saveManifest(); let browser; try{browser=await makeBrowser(); if(MODE==='prepare')await requestPasswordLinks(browser); else{if(process.env.RESET_LINKS_JSON)await applyResetLinks(browser); const accounts=[{role:'superadmin',email:SUPERADMIN_EMAIL},...USERS]; for(const locale of ['en','pl'])for(const a of accounts)await captureRole(browser,a.email,a.role,locale);} manifest.status='done';manifest.finishedAt=new Date().toISOString();saveManifest();event('done',{count:manifest.screenshots.length});}catch(e){manifest.status='error';manifest.error=e.stack||e.message;saveManifest();event('fatal',{error:e.message});}finally{if(browser)await browser.close().catch(()=>{});} }
const app=express();app.use('/shots',express.static(SHOTS));app.get('/manifest.json',(_req,res)=>res.sendFile(path.join(OUT,'manifest.json')));app.get('/',(_req,res)=>res.type('text').send(`Edulytics manual capture helper: ${manifest.status}\n`));app.listen(PORT,'0.0.0.0',()=>{console.log(`capture helper listening on ${PORT}`);run();});
