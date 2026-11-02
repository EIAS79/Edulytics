// Re-runable, read-only official content and subordinate clause coverage audit.
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..');
const snap=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/reference/us-ccss-official-codes.snapshot.json'),'utf8'));
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.curriculum-pack.json'),'utf8'));
const nodeMap=new Map(pack.Nodes.map(n=>[n.Code,n]));
function text(s){return s.replace(/<sup\b[^>]*>[\s\S]*?<\/sup>/gi,'').replace(/<[^>]*>/g,' ')
 .replace(/&#x([a-f0-9]+);/gi,(_,x)=>String.fromCodePoint(parseInt(x,16)))
 .replace(/&#([0-9]+);/g,(_,x)=>String.fromCodePoint(Number(x)))
 .replace(/&(amp|lt|gt|quot|apos|nbsp|ndash|mdash);/gi,(_,x)=>({amp:'&',lt:'<',gt:'>',quot:'"',apos:"'",nbsp:' ',ndash:'-',mdash:'-'}[x.toLowerCase()]))
 .replace(/\s+/g,' ').trim();}
function normalize(s){return text(s).normalize('NFKC').toLowerCase().replace(/[\u2018\u2019]/g,"'").replace(/[\u2013\u2014\u2212]/g,'-').replace(/[^a-z0-9]+/g,' ').replace(/\s+/g,' ').trim();}
const normcode=s=>s.replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.');
const pages=snap.pages;const entries=new Map(),fail=[];let i=0;
async function worker(){while(i<pages.length){const page=pages[i++];try{const r=await fetch(page.url,{signal:AbortSignal.timeout(16000)});if(!r.ok)throw new Error('HTTP '+r.status);const h=await r.text();const re=/<div class="(standard|substandard)">\s*<a\b[^>]*name="CCSS\.Math\.Content\.([^"]+)"[^>]*>[\s\S]*?<\/a>\s*<br\s*\/?>\s*([\s\S]*?)<\/div>/g;for(const m of h.matchAll(re)){const code=normcode(m[2]);if(!entries.has(code))entries.set(code,{kind:m[1],code,body:text(m[3]),url:page.url});}}catch(e){fail.push({url:page.url,error:e.message})}}}
(async()=>{
 await Promise.all(Array.from({length:6},worker));
 const rows=[], clauses=[];
 for(const [code,item] of entries){
   if(item.kind!=='standard')continue;
   const n=nodeMap.get('CCSS:'+code)||nodeMap.get('CCSS:'+code.replace(/^(HS[NAFGS])\.([A-Z]{1,3})\./,'$1-$2.'));if(!n)continue;
   const children=[...entries.values()].filter(x=>x.kind==='substandard'&&x.code.startsWith(code+'.')).sort((a,b)=>a.code.localeCompare(b.code));
   const local=normalize(n.OfficialText||''),official=normalize(item.body);
   const childRows=children.map(x=>{const signature=normalize(x.body).split(' ').slice(0,12).join(' ');const present=local.includes(signature);const row={parent:code,code:x.code,presentInStoredText:present,signature,sourceUrl:x.url,officialChildText:x.body};clauses.push(row);return {code:x.code,presentInStoredText:present};});
   const officialCombined=normalize([item.body,...children.map(x=>x.code.slice(-1)+'. '+x.body)].join(' '));
   const potentialTruncation=local.length<official.length*.75&&!local.includes(official.slice(-60));
   let classification=local===officialCombined?'MATCH':officialCombined.startsWith(local)&&local.length<officialCombined.length?'POSSIBLY_TRUNCATED_OR_CHILDREN_ABSENT':officialCombined.includes(local)?'LOCAL_SUBSTRING':local.startsWith(officialCombined)?'POSSIBLE_TRAILING_FOOTNOTES':'NEEDS_REVIEW';
   if(potentialTruncation)classification='POTENTIAL_TRUNCATION';
   rows.push({code,classification,storedCharacters:n.OfficialText?.length||0,officialMainCharacters:item.body.length,
     childCount:children.length,childSignaturesPresent:childRows.filter(x=>x.presentInStoredText).length,
     childSignaturesMissing:childRows.filter(x=>!x.presentInStoredText).map(x=>x.code),
     sourceUrl:item.url,storedText:n.OfficialText,officialCombinedText:[item.body,...children.map(x=>x.code.slice(-1)+'. '+x.body)].join(' ')});
 }
 const problems=rows.filter(x=>x.classification!=='MATCH');
 const report={version:1,authority:'Common Core official standards web pages',date:new Date().toISOString(),
  pages:pages.length,errors:fail,numberedStandards:rows.length,childClauses:clauses.length,
  missingChildTextClauses:clauses.filter(x=>!x.presentInStoredText).length,missingChildClauseCodes:clauses.filter(x=>!x.presentInStoredText).map(x=>x.code),
  classes:Object.fromEntries([...new Set(rows.map(x=>x.classification))].map(k=>[k,rows.filter(x=>x.classification===k).length])),rows,clauses,
  disclaimer:'This compares storage text only; it does not certify lesson or question alignment to a clause.'};
 fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-authority-repair-candidates.json'),JSON.stringify(report,null,2)+'\n');
 console.log(JSON.stringify({pages:report.pages,errors:fail.length,standards:rows.length,clauses:clauses.length,missingChildTextClauses:report.missingChildTextClauses,classes:report.classes,
   truncated:rows.filter(x=>x.classification==='POTENTIAL_TRUNCATION').map(x=>x.code),firstMissingClauses:report.missingChildClauseCodes.slice(0,25)},null,2));
 if(fail.length)process.exitCode=1;
})().catch(e=>{console.error(e);process.exitCode=1});