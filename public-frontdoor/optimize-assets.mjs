#!/usr/bin/env node
// Optimizes only Edulytiks Public build output. Never alters the ASP.NET application.
import {readFileSync,writeFileSync,copyFileSync,existsSync,mkdirSync,readdirSync,statSync} from 'node:fs';
import {resolve,join,relative,sep,dirname} from 'node:path';
import {fileURLToPath} from 'node:url';

const scriptDir=dirname(fileURLToPath(import.meta.url));
const out=resolve(process.env.EDULYTIKS_FRONTDOOR_DIST || 'public-frontdoor/dist');
const sourceDir=join(scriptDir,'optimized-images');
const dryRun=process.argv.includes('--check');
const replacements=[
  {from:'edulaytiks-character.png',to:'edulaytiks-character.webp'},
  {from:'edulaytiks-character-background-1.png',to:'edulaytiks-character-background-1.webp'}
];
const skipped=new Set(['lib','node_modules','.git']);
const pages=[];
const files=[];
function walk(dir){
  for(const item of readdirSync(dir)){
    const file=join(dir,item);
    if(statSync(file).isDirectory()){
      if(!skipped.has(item))walk(file);
    }else{
      const extension=item.slice(item.lastIndexOf('.')+1);
      if(['html','css','js'].includes(extension)) files.push(file);
      if(item==='index.html')pages.push(file);
    }
  }
}
if(!existsSync(out))throw new Error('Public static build directory missing: '+out);
walk(out);
if(!dryRun){
  for(const asset of replacements){
    const src=join(sourceDir,asset.to);
    const dest=join(out,'images','public',asset.to);
    if(!existsSync(src))throw new Error('Missing optimized marketing asset: '+src);
    mkdirSync(dirname(dest),{recursive:true});
    copyFileSync(src,dest);
  }
  for(const file of files){
    let content=readFileSync(file,'utf8');
    let updated=content;
    for(const asset of replacements){
      updated=updated.split('/images/public/'+asset.from).join('/images/public/'+asset.to);
    }
    const location=relative(out,file).split(sep).join('/');
    if(['index.html','pl/index.html','en/index.html','ar/index.html'].includes(location)){
      const preload='<link rel="preload" as="image" href="/images/public/edulaytiks-character.webp?v=43" fetchpriority="high">';
      if(!updated.includes(preload))updated=updated.replace(/<head>/i,'<head>\n'+preload);
    }
    if(updated!==content)writeFileSync(file,updated);
  }
}
let wrong=[];
for(const asset of replacements){
  if(!existsSync(join(out,'images','public',asset.to)))wrong.push('optimized file missing: '+asset.to);
}
for(const file of files){
  const content=readFileSync(file,'utf8');
  const path=relative(out,file).split(sep).join('/');
  for(const asset of replacements){
    if(content.includes('/images/public/'+asset.from))wrong.push(path+' still references '+asset.from);
  }
  if(['index.html','pl/index.html','en/index.html','ar/index.html'].includes(path)){
    if(!content.includes('rel="preload" as="image" href="/images/public/edulaytiks-character.webp?v=43"'))
      wrong.push(path+' missing mascot preload');
  }
}
if(wrong.length)throw new Error(wrong.slice(0,30).join('\n'));
console.log('Static image delivery validated: '+pages.length+' pages, '+replacements.length+' optimized assets.');
