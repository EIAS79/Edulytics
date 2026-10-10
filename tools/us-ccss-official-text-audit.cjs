#!/usr/bin/env node
// Compare the retained CCSS official-text corpus with the publisher's public HTML pages.
// Read-only. Does not approve semantic mappings or modify database/curriculum source.
'use strict';
const fs=require('node:fs'), path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const official=read(path.join(root,'docs/curriculum/reference/us-ccss-official-codes.snapshot.json'));
const pack=read(path.join(root,'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.curriculum-pack.json'));
const normCode=s=>s.replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.');
const clean=s=>s.replace(/<sup\b[^>]*>[\s\S]*?<\/sup>/gi,'').replace(/<[^>]*>/g,' ')
  .replace(/&#x([a-f0-9]+);/gi,(_,x)=>String.fromCodePoint(parseInt(x,16)))
  .replace(/&#([0-9]+);/g,(_,x)=>String.fromCodePoint(Number(x)))
  .replace(/&(amp|lt|gt|quot|apos|nbsp|ndash|mdash);/gi,(_,x)=>({amp:'&',lt:'<',gt:'>',quot:'"',apos:"'",nbsp:' ',ndash:'-',mdash:'-'}[x.toLowerCase()]))
  .replace(/\s+/g,' ').trim();
const tokens=s=>clean(s).normalize('NFKC').toLowerCase().replace(/[\u2018\u2019]/g,"'")
  .replace(/[\u2013\u2014\u2212]/g,'-').replace(/[^a-z0-9]+/g,' ').trim().split(' ').filter(Boolean);
const similarity=(a,b)=>{
  const aa=tokens(a),bb=tokens(b),freq=new Map();
  for(const t of aa)freq.set(t,(freq.get(t)||0)+1);
  let common=0;for(const t of bb){const n=freq.get(t)||0;if(n>0){freq.set(t,n-1);common++;}}
  return Math.round(1000*2*common/Math.max(1,aa.length+bb.length))/1000;
};
async function main(){
 const lookup=new Map(),fetchErrors=[];let next=0;
 async function worker(){
  while(next<official.pages.length){
   const entry=official.pages[next++];
   try{
    const r=await fetch(entry.url,{signal:AbortSignal.timeout(25000)});
    if(!r.ok)throw Error('HTTP '+r.status);
    const html=await r.text();
    const re=/<div class="(?:standard|substandard)">\s*<a\b[^>]*name="CCSS\.Math\.Content\.([^"]+)"[^>]*>[\s\S]*?<\/a>\s*<br\s*\/?>\s*([\s\S]*?)<\/div>/g;
    for(const m of html.matchAll(re)){
      const code=normCode(m[1]);
      if(lookup.has(code))continue;
      lookup.set(code,clean(m[2]));
    }
   }catch(e){fetchErrors.push({url:entry.url,error:String(e.message)});}
  }
 }
 await Promise.all(Array.from({length:6},()=>worker()));
 const sourceRows=pack.Nodes.filter(n=>n.Kind==='Standard' && n.Code.startsWith('CCSS:') && /^(?:[1-8]\.|HS[NAFGS][-\.])/.test(n.Code.slice(5)));
 const missingPage=[],notIdentical=[],exact=[],lowSimilarity=[];
 for(const row of sourceRows){
   const code=normCode(row.Code.slice(5)),children=[...lookup.entries()].filter(([k])=>k.startsWith(code+'.')&&/\.[a-z]$/.test(k)).sort((a,b)=>a[0].localeCompare(b[0]));
   const fromPage=lookup.has(code)?[lookup.get(code),...children.map(([k,v])=>k.slice(-1)+'. '+v)].join(' '):null;
   if(!fromPage){missingPage.push(code);continue;}
   const source=row.OfficialText||'',sim=similarity(source,fromPage);
   if(tokens(source).join(' ')===tokens(fromPage).join(' ')){exact.push(code);continue;}
   const sample={code,similarity:sim,sourceLength:source.length,officialLength:fromPage.length,
    sourceTail:source.slice(-110),officialTail:fromPage.slice(-110)};
   notIdentical.push(sample);
   if(sim<.90)lowSimilarity.push(sample);
 }
 const children=[...lookup.keys()].filter(k=>/\.\d+\.[a-z]$/.test(k));
 const report={version:1,retrievedUtc:new Date().toISOString(),source:'thecorestandards.org official HTML',
  sourceNodes:sourceRows.length,officialExtractedCodes:lookup.size,matchedExact:exact.length,
  nonIdentical:notIdentical.length,lowSimilarityCount:lowSimilarity.length,
  officialChildSubstandards:children,missingInOfficialPage:missingPage,
  fetchErrors,lowSimilarity,nonIdenticalResults:notIdentical,
  warning:'Differences are flagged for manual review; mathematical symbols and footnote markup may cause false positives. Do not blindly overwrite official text.'};
 const output=path.join(root,'docs/curriculum/us-ccss-official-text-diff.json');
 fs.writeFileSync(output,JSON.stringify(report,null,2)+'\n');
 console.log(JSON.stringify({sourceNodes:report.sourceNodes,officialExtractedCodes:report.officialExtractedCodes,
  matchedExact:exact.length,nonIdentical:notIdentical.length,lowSimilarity:lowSimilarity.length,
  firstLow:lowSimilarity.slice(0,12),missingInOfficialPage:missingPage.slice(0,15),officialChildSubstandards:children.length,
  fetchErrors:fetchErrors.length},null,2));
}
main().catch(e=>{console.error(e);process.exitCode=1});