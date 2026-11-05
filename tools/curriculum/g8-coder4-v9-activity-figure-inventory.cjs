'use strict';
// Source-attributed, lesson/activity-scoped structural evidence; no publisher assets copied.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const data=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const list=data('docs/curriculum/us-ccss-g8-coder4-v8-publisher-activity-structure-20261029.json').rows;
const sha=text=>crypto.createHash('sha256').update(text).digest('hex');
const unescape=text=>String(text).replace(/&nbsp;|&#160;/gi,' ').replace(/&amp;/gi,'&').replace(/&quot;/gi,'"').replace(/&apos;|&#39;/gi,"'").replace(/&lt;/gi,'<').replace(/&gt;/gi,'>').replace(/&rsquo;|&#8217;/gi,'’').replace(/&ldquo;|&#8220;/gi,'“').replace(/&rdquo;|&#8221;/gi,'”').replace(/&#(\d+);/g,(_,n)=>String.fromCodePoint(+n));
const clean=html=>unescape(html.replace(/<script[\s\S]*?<\/script>/gi,' ').replace(/<style[\s\S]*?<\/style>/gi,' ').replace(/<[^>]*>/g,' ').replace(/\s+/g,' ').trim());
const attr=(tag,key)=>{const m=tag.match(new RegExp('\\b'+key+'\\s*=\\s*["\\x27]([^"\\x27]*)["\\x27]','i'));return m?unescape(m[1]).trim():null};
function segment(html){
 const headings=[...html.matchAll(/<h[23]\b[^>]*class=["'][^"']*(?:im-c-card__heading|im-c-hero__heading)[^"']*["'][^>]*>([\s\S]*?)<\/h[23]>/gi)]
 .map(m=>({start:m.index,title:clean(m[1]).replace(/\s*\(\d+\s+minutes?\)\s*$/i,''),end:m.index+m[0].length}));
 return headings.map((x,i)=>({title:x.title,body:html.slice(x.end,i+1<headings.length?headings[i+1].start:html.length)}))
  .filter(x=>/^\d+\.\d+:/.test(x.title));
}
function classify(body){
 const imgs=[...body.matchAll(/<img\b[^>]*>/gi)].map(x=>x[0])
 .map(tag=>({src:attr(tag,'src'),alt:attr(tag,'alt')||''}))
 .filter(x=>x.src?.includes('cms-assets.illustrativemathematics.org/'));
 const uniques=[...new Map(imgs.map(x=>[x.src,x])).values()];
 const words=clean(body).split(/\s+/).filter(Boolean);
 const tables=(body.match(/<table\b/gi)||[]).length;
 const figures=(body.match(/<figure\b/gi)||[]).length;
 const inlineMath=(body.match(/class=["'][^"']*\bmath\b[^"']*["']/gi)||[]).length;
 return {bodySha256:sha(clean(body)),approxWords:words.length,uniqueMathFigures:uniques.map(x=>({alt:x.alt,sourceAssetUrl:x.src})),
 mathFigureCount:uniques.length,figureHtmlTags:figures,tables,inlineMathTags:inlineMath,
 publisherInteractiveElementMarkers:(body.match(/data-record-model=["'](?:Desmos|GeoGebra|Applet|Video)/gi)||[]).length};
}
async function load(url){
 const r=await fetch(url,{signal:AbortSignal.timeout(25000),headers:{'User-Agent':'Edulytiks-G8-Source-Activity-Verification/1.0'}});
 if(!r.ok)throw Error(url+' returned '+r.status);
 return await r.text();
}
const rows=new Array(list.length);let index=0;
async function worker(){
 for(;;){const i=index++;if(i>=list.length)return;const x=list[i];try{
  const [s,t]=await Promise.all([load(x.studentUrl),load(x.teacherUrl)]);
  const st=segment(s),tt=segment(t);
  const teacherTitles=new Set(tt.map(z=>z.title));
  const activities=st.map(a=>({title:a.title,teacherSameTitlePresent:teacherTitles.has(a.title),...classify(a.body)}));
  const pageFigures=new Map(activities.flatMap(a=>a.uniqueMathFigures).map(f=>[f.sourceAssetUrl,f]));
  const studentPageAssets=[...s.matchAll(/<img\b[^>]*>/gi)].map(m=>attr(m[0],'src')).filter(v=>v?.includes('cms-assets.illustrativemathematics.org/'));
  rows[i]={lessonCode:x.lessonCode,sourceLessonCode:x.sourceLessonCode,sourceStudentUrl:x.studentUrl,sourceTeacherUrl:x.teacherUrl,
   studentPageSha256:sha(s),teacherPageSha256:sha(t),studentActivityCount:activities.length,teacherActivityCount:tt.length,
   studentTeacherActivityTitlesMatch:st.length===tt.length&&st.every((z,j)=>z.title===tt[j]?.title),
   teacherActivityTitles:tt.map(z=>z.title),
   teacherOnlyActivityTitles:tt.filter(z=>!st.some(a=>a.title===z.title)).map(z=>z.title),
   studentOnlyActivityTitles:st.filter(z=>!teacherTitles.has(z.title)).map(z=>z.title),
   uniqueMathFiguresInActivities:pageFigures.size,uniqueMathFiguresInWholeStudentPage:new Set(studentPageAssets).size,
   activities};
 }catch(e){rows[i]={lessonCode:x.lessonCode,error:String(e.message||e)}}
 if((i+1)%25===0)console.log('V9_ACTIVITY_FETCH '+(i+1)+'/'+list.length);
 }}
(async()=>{
 await Promise.all([worker(),worker(),worker(),worker()]);
 const counts={lessons:rows.length,studentActivities:rows.reduce((n,x)=>n+(x.studentActivityCount||0),0),
 lessonsWithActivities:rows.filter(x=>x.studentActivityCount>0).length,
 studentAndTeacherActivityHeadingMatch:rows.filter(x=>x.studentTeacherActivityTitlesMatch).length,
 teacherOnlyActivityHeadings:rows.reduce((n,x)=>n+(x.teacherOnlyActivityTitles||[]).length,0),
 studentOnlyActivityHeadings:rows.reduce((n,x)=>n+(x.studentOnlyActivityTitles||[]).length,0),
 lessonsWithTeacherOnlyActivities:rows.filter(x=>(x.teacherOnlyActivityTitles||[]).length>0).length,
 lessonsWithUniqueSourceFigures:rows.filter(x=>x.uniqueMathFiguresInActivities>0).length,
 totalActivityFigureOccurrences:rows.reduce((n,x)=>n+(x.activities||[]).reduce((m,a)=>m+a.mathFigureCount,0),0),
 lessonsWithoutUniqueSourceFigures:rows.filter(x=>x.uniqueMathFiguresInActivities===0).map(x=>x.lessonCode),
 totalActivityTables:rows.reduce((n,x)=>n+(x.activities||[]).reduce((m,a)=>m+a.tables,0),0),
 failures:rows.filter(x=>x.error||x.studentActivityCount===0).map(x=>({lessonCode:x.lessonCode,error:x.error||'no-activity'}))};
 const out='docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json';
 fs.writeFileSync(path.join(root,out),JSON.stringify({schemaVersion:1,method:'Live publisher HTML, each student activity card and its teacher lesson heading; image sources filtered by CMS domain, de-duplicated by original asset URL. No asset downloaded or republished. Math-fidelity/diagram accessibility and authentic UI not independently certified.',sourceEdition:'IM/Open Up Resources 2017–2019',counts,rows},null,2)+'\n');
 console.log('V9_ACTIVITY_SUMMARY '+JSON.stringify({output:out,...counts}));
 if(counts.failures.length||counts.lessons!==131)process.exitCode=1;
})().catch(e=>{console.error(e);process.exitCode=1});
