#!/usr/bin/env node
import {mkdtempSync,mkdirSync,writeFileSync,readFileSync,existsSync,rmSync} from 'node:fs';
import {tmpdir} from 'node:os';
import {join,resolve,dirname} from 'node:path';
import {spawnSync} from 'node:child_process';
import assert from 'node:assert/strict';
const out=mkdtempSync(join(tmpdir(),'edulytiks-image-fixtures-'));
const script=resolve('public-frontdoor/optimize-assets.mjs');
try{
  const samples={
    'index.html':'<html><head></head><body><img src="/images/public/edulaytiks-character.png?v=43"></body></html>',
    'en/index.html':'<html><head></head><body><img src="/images/public/edulaytiks-character.png?v=43"></body></html>',
    'css/styles.css':'body{background:url("/images/public/edulaytiks-character-background-1.png")}',
    'js/app.js':'image.src="/images/public/edulaytiks-character.png?v=43";'
  };
  for(const [name,content] of Object.entries(samples)){
    const path=join(out,name);mkdirSync(dirname(path),{recursive:true});writeFileSync(path,content);
  }
  for(const flags of [[],['--check'],[],['--check']]){
    const r=spawnSync(process.execPath,[script,...flags],{env:{...process.env,EDULYTIKS_FRONTDOOR_DIST:out},encoding:'utf8'});
    if(r.status)throw new Error(r.stdout+'\n'+r.stderr);
  }
  const html=readFileSync(join(out,'index.html'),'utf8');
  assert.match(html,/fetchpriority="high"/);
  assert.match(html,/edulaytiks-character\.webp\?v=43/);
  assert.equal((html.match(/rel="preload"/g)||[]).length,1);
  assert.match(readFileSync(join(out,'css/styles.css'),'utf8'),/background-1\.webp/);
  assert.match(readFileSync(join(out,'js/app.js'),'utf8'),/character\.webp/);
  for(const name of ['edulaytiks-character.webp','edulaytiks-character-background-1.webp'])
    assert.ok(existsSync(join(out,'images/public',name)));
  console.log('Offline image optimization fixtures passed: JS, CSS, preload, idempotence.');
}finally{rmSync(out,{recursive:true,force:true});}
