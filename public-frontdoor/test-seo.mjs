#!/usr/bin/env node
// Offline marketing-only smoke test. No Render service or database calls.
import {mkdtempSync,mkdirSync,writeFileSync,readFileSync,rmSync} from 'node:fs';
import {tmpdir} from 'node:os';
import {join,dirname,resolve} from 'node:path';
import {spawnSync} from 'node:child_process';
import assert from 'node:assert/strict';
const out=mkdtempSync(join(tmpdir(),'edulytiks-seo-'));
const script=resolve('public-frontdoor/seo-postprocess.mjs');
const html=(title,lang)=>'<html lang="'+lang+'"><head><title>'+title+'</title><meta name="description" content="Edulytics to zgodna z programem platforma do nauki matematyki, oceniania i monitorowania postępów dla szkół, nauczycieli i uczniów."></head><body><h1>'+title+'</h1><p>Edulytics dla szkół.</p></body></html>';
const routes=[['index.html','Edulityks | Matematyka','pl'],['pl/index.html','Edulityks | Matematyka','pl'],['en/index.html','Edulytics | Mathematics','en'],['ar/index.html','Edulytics | &#x627;&#x644;&#x631;&#x64A;&#x627;&#x636;&#x64A;&#x627;&#x62A;','ar'],['product/curricula/index.html','Programy nauczania','pl'],['account/login/index.html','Logowanie','pl']];
try {
  for(const [route,title,lang] of routes){const file=join(out,route);mkdirSync(dirname(file),{recursive:true});writeFileSync(file,html(title,lang));}
  for(const args of [[],['--check'],[],['--check']]){
    const r=spawnSync(process.execPath,[script,...args],{env:{...process.env,EDULYTIKS_FRONTDOOR_DIST:out},encoding:'utf8'});
    if(r.status!==0)throw new Error(r.stdout+'\n'+r.stderr);
  }
  const root=readFileSync(join(out,'index.html'),'utf8');
  const en=readFileSync(join(out,'en/index.html'),'utf8');
  const login=readFileSync(join(out,'account/login/index.html'),'utf8');
  const arabic=readFileSync(join(out,'ar/index.html'),'utf8');
  const sitemap=readFileSync(join(out,'sitemap.xml'),'utf8');
  assert.match(root,/canonical" href="https:\/\/edulytiks.com\/"/);
  assert.match(root,/name="description" content="Edulytics to zgodna z programem/);
  assert.match(arabic,/property="og:title" content="Edulytics \| الرياضيات"/);
  assert.match(en,/hreflang="ar"/);
  assert.match(login,/noindex,follow/);
  assert.doesNotMatch(sitemap,/account\/login|<loc>https:\/\/edulytiks.com\/pl\//);
  assert.match(sitemap,/product\/curricula/);
  assert.equal((root.match(/rel="canonical"/g)||[]).length,1);
  console.log('Offline SEO fixtures passed: canonical, 3 languages, sitemap, noindex, idempotence.');
}finally{rmSync(out,{recursive:true,force:true});}
