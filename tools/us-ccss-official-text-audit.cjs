#!/usr/bin/env node
// Compare the retained CCSS official-text corpus with the publisher's public HTML pages.
// Read-only. Does not approve semantic mappings or modify database/curriculum source.
'use strict';
const fs=require('node:fs'), path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const official=read(path.join(root,'docs/curriculum/reference/us-ccss-official-codes.snapshot.json'));
const pack=read(path.join(root,'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.curriculum-pack.json'));
const normCode=s=>s.replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.');
// The official HTML uses <sup> for powers. Dropping its contents can turn
// x^2 into x, and a negative exponent into an apparently positive one.
// Preserve all visible superscripts/subscripts, including footnote markers;
// deciding whether a marker is a footnote is a separate human source review.
const clean=s=>String(s??'')
  .replace(/<(sup|sub)\b[^>]*>([\s\S]*?)<\/\1>/gi,(_,kind,body)=>{
    const value=body.replace(/<[^>]*>/g,'').replace(/\s+/g,' ').trim();
    return (kind.toLowerCase()==='sup'?'^{':'_{')+value+'}';
  })
  .replace(/<[^>]*>/g,' ')
  .replace(/&#x([a-f0-9]+);/gi,(_,x)=>String.fromCodePoint(parseInt(x,16)))
  .replace(/&#([0-9]+);/g,(_,x)=>String.fromCodePoint(Number(x)))
  .replace(/&(amp|lt|gt|quot|apos|nbsp|ndash|mdash|times|divide|ne|le|ge|theta|pi|minus);/gi,
    (_,x)=>({amp:'&',lt:'<',gt:'>',quot:'"',apos:"'",nbsp:' ',ndash:'-',mdash:'-',
      times:'×',divide:'÷',ne:'≠',le:'≤',ge:'≥',theta:'θ',pi:'π',minus:'−'}[x.toLowerCase()]))
  .replace(/\s+/g,' ').trim();
const tokens=s=>clean(s).normalize('NFKC').toLowerCase().replace(/[\u2018\u2019]/g,"'")
  .replace(/[\u2013\u2014\u2212]/g,'-').replace(/[^a-z0-9]+/g,' ').trim().split(' ').filter(Boolean);
// Token similarity is only a triage aid: it deliberately ignores punctuation.
// Never count a comparison as *exact* if its mathematical operators, signs,
// powers or subscripts differ. This is intentionally conservative.
const notationTokens=s=>clean(s).replace(/[\u2013\u2014\u2212]/g,'-')
  .match(/\^\{[^}]*\}|_\{[^}]*\}|[=<>+\-×÷≠≤≥±]/g)||[];
const sameExtractedMathText=(a,b)=>
  tokens(a).join(' ')===tokens(b).join(' ')&&
  notationTokens(a).join('|')===notationTokens(b).join('|');
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
   if(sameExtractedMathText(source,fromPage)){exact.push(code);continue;}
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
 // Side-by-side baseline comparison is deliberately NOT an academic disposition.
 // Keep the checked-in 107/98 historical queues immutable during fresh source
 // verification; the candidate report is stored separately for reviewer triage.
 const trackedOutput=path.join(root,'docs/curriculum/us-ccss-official-text-diff.json');
 if(process.argv.includes('--compare-baseline')){
  const previous=read(trackedOutput);
  const oldCodes=new Set(previous.nonIdenticalResults.map(x=>x.code));
  const newCodes=new Set(notIdentical.map(x=>x.code));
  report.baselineComparison={
   priorCapturedAtUtc:previous.retrievedUtc,
   previouslyNonIdentical:oldCodes.size,
   freshlyNonIdentical:newCodes.size,
   stillNonIdentical:[...newCodes].filter(x=>oldCodes.has(x)).sort(),
   newlyFlagged:[...newCodes].filter(x=>!oldCodes.has(x)).sort(),
   noLongerFlagged:[...oldCodes].filter(x=>!newCodes.has(x)).sort(),
   warning:'Mathematical HTML normalization changed. Differences are source-review candidates only; historical 98 pending human dispositions and nine guarded repairs are not automatically altered.'
  };
 }
 const outputArg=process.argv.find(x=>x.startsWith('--output='));
 const output=outputArg?path.resolve(outputArg.slice('--output='.length)):trackedOutput;
 if(!output)throw Error('Missing output path');
 fs.writeFileSync(output,JSON.stringify(report,null,2)+'\n');
 if(fetchErrors.length||missingPage.length)
  process.exitCode=2; // report remains available, but an incomplete source capture cannot pass

 console.log(JSON.stringify({sourceNodes:report.sourceNodes,officialExtractedCodes:report.officialExtractedCodes,
  matchedExact:exact.length,nonIdentical:notIdentical.length,lowSimilarity:lowSimilarity.length,
  firstLow:lowSimilarity.slice(0,12),missingInOfficialPage:missingPage.slice(0,15),officialChildSubstandards:children.length,
  fetchErrors:fetchErrors.length,baselineComparison:report.baselineComparison&&{
   previous:report.baselineComparison.previouslyNonIdentical,
   fresh:report.baselineComparison.freshlyNonIdentical,
   newlyFlagged:report.baselineComparison.newlyFlagged,
   noLongerFlagged:report.baselineComparison.noLongerFlagged}},null,2));
}
if(require.main===module)main().catch(e=>{console.error(e);process.exitCode=1});
module.exports={clean,tokens,notationTokens,sameExtractedMathText,similarity};