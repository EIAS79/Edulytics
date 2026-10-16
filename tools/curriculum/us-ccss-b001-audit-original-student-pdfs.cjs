'use strict';
// Audit all 500 original 2021 student PDFs. No PDF bytes/text persist to artifacts.
// Structural PDF inspection is NOT pedagogical review or academic approval.
const fs=require('node:fs');
const fsp=require('node:fs/promises');
const path=require('node:path');
const os=require('node:os');
const assert=require('node:assert/strict');
const crypto=require('node:crypto');
const {Readable}=require('node:stream');
const {spawnSync}=require('node:child_process');
const root=path.resolve(__dirname,'../..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
function validateSources(s,c){
 assert.equal(s.lessons.length,500);assert.equal(c.lessons.length,500);
 const codes=new Set(),urls=new Set();
 s.lessons.forEach((x,i)=>{
  assert.equal(x.ordinal,i+1);assert.equal(x.lessonCode,c.lessons[i].lessonCode);
  assert.equal(x.sourceEdition,'1st Edition (2021)');
  assert.equal(x.academicAccepted,false);
  assert.equal(x.studentTaskContentReviewed,false);
  const u=new URL(x.originalStudentTaskStatementPdfUrl);
  assert.equal(u.protocol,'https:');assert.equal(u.hostname,'cms-assets.illustrativemathematics.org');
  assert.ok(!codes.has(x.lessonCode));assert.ok(!urls.has(u.toString()));
  codes.add(x.lessonCode);urls.add(u.toString());
 });
 return s.lessons;
}
function classifyText(text,pages){
 const words=(text.match(/\S+/gu)||[]).length,characters=text.replace(/\s/gu,'').length;
 const flags=[];
 if(characters===0)flags.push('NO_EXTRACTABLE_TEXT_VISUAL_REVIEW');
 else if(words<40)flags.push('LOW_TEXT_DENSITY_VISUAL_REVIEW');
 if(!pages)flags.push('MISSING_PAGE_COUNT');
 return {words,characters,flags};
}
function pdfTools(file){
 const info=spawnSync('pdfinfo',[file],{encoding:'utf8',timeout:20000,maxBuffer:262144});
 if(info.status!==0||info.error)throw Error('PDFINFO_ERROR_'+String(info.stderr||info.error).slice(0,80));
 const m=info.stdout.match(/^Pages:\s*(\d+)/m),pages=m?Number(m[1]):0;
 if(pages<1)throw Error('INVALID_PAGES');
 const text=spawnSync('pdftotext',['-layout','-nopgbrk',file,'-'],{encoding:'utf8',timeout:30000,maxBuffer:16*1024*1024});
 if(text.status!==0||text.error)throw Error('PDFTOTEXT_ERROR_'+String(text.stderr||text.error).slice(0,80));
 return {pages,...classifyText(text.stdout,pages)};
}
async function fetchPdf(url,file,signal){
 const res=await fetch(url,{signal,redirect:'follow'});
 if(!res.ok)throw Error('HTTP_'+res.status);
 const u=new URL(res.url);
 if(u.protocol!=='https:')throw Error('UNSAFE_REDIRECT');
 const ct=res.headers.get('content-type')||'';
 if(!/application\/pdf|application\/octet-stream/i.test(ct))throw Error('NOT_PDF_MIME_'+ct);
 const fd=await fsp.open(file,'w'),hash=crypto.createHash('sha256');let bytes=0;
 try{
  for await(const buf of Readable.fromWeb(res.body)){
   bytes+=buf.length;if(bytes>35*1024*1024)throw Error('SIZE_LIMIT');
   hash.update(buf);await fd.writeFile(buf);
  }
 }finally{await fd.close();}
 if(bytes<100)throw Error('EMPTY');
 const input=await fsp.open(file,'r'),header=Buffer.alloc(5);
 try{await input.read(header,0,5,0);}finally{await input.close();}
 if(header.toString('ascii')!=='%PDF-')throw Error('NOT_PDF_SIGNATURE');
 return {bytes,sha256:hash.digest('hex')};
}
async function audit(x,tmp){
 const dest=path.join(tmp,String(x.ordinal)+'.pdf');
 const fixed={ordinal:x.ordinal,lessonCode:x.lessonCode,pdfSourceUrl:x.originalStudentTaskStatementPdfUrl,
  sourcePdfContentAcademicallyReviewed:false,independentTeachingApproved:false,authenticatedPracticeApproved:false};
 let error='';for(let trial=0;trial<2;trial++){
  const c=new AbortController(),timer=setTimeout(()=>c.abort(),25000);
  try{
   const stat=await fetchPdf(x.originalStudentTaskStatementPdfUrl,dest,c.signal);
   return {...fixed,...stat,...pdfTools(dest),status:'STRUCTURALLY_INSPECTED'};
  }catch(e){error=String(e.message||e).slice(0,160);}
  finally{clearTimeout(timer);await fsp.rm(dest,{force:true}).catch(()=>{});}
 }
 return {...fixed,status:'INSPECTION_FAILED',flags:['TECHNICAL_REVIEW'],error};
}
function summarize(records){
 return {total:records.length,structurallyInspected:records.filter(r=>r&&r.status==='STRUCTURALLY_INSPECTED').length,
  failed:records.filter(r=>r&&r.status==='INSPECTION_FAILED').length,
  visualReview:records.filter(r=>r&&(r.flags||[]).some(f=>f.includes('VISUAL_REVIEW'))).length,
  academicallyApproved:0,authenticatedPracticeApproved:0};
}
async function main(){
 const lessons=validateSources(load('docs/curriculum/batches/US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json'),
  load('docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json'));
 const arg=process.argv.indexOf('--output'),out=arg<0?'b001-500-original-pdf-evidence.json':process.argv[arg+1];
 if(!out)throw Error('Missing output file');
 const tmp=await fsp.mkdtemp(path.join(os.tmpdir(),'b001-pdf-'));
 const records=Array(500).fill(null);let n=0;
 try{
  async function worker(){while(n<500){const i=n++;records[i]=await audit(lessons[i],tmp);console.log((i+1)+'/500 '+records[i].status+' '+lessons[i].lessonCode+' '+(records[i].flags||[]).join(',')+' '+(records[i].error||''));}}
  await Promise.all([worker(),worker(),worker(),worker()]);
  const summary=summarize(records);
  const report={batchId:'US-CCSS-B001',generatedAt:new Date().toISOString(),
   assurance:'Structural extraction only; no educational content certification, visual review, mathematical truth or student UAT. B001 NO_GO.',
   summary,lessons:records};
  await fsp.writeFile(out,JSON.stringify(report,null,2)+'\n','utf8');
  console.log(JSON.stringify(summary));
  if(summary.structurallyInspected!==500)process.exitCode=1;
 }finally{await fsp.rm(tmp,{recursive:true,force:true});}
}
if(require.main===module)main().catch(e=>{console.error(e);process.exitCode=1;});
module.exports={validateSources,classifyText,summarize};
