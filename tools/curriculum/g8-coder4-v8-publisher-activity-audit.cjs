'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const read=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
const source=read('docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const richRoot='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const rich=fs.readdirSync(path.join(root,richRoot)).filter(f=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(f)).flatMap(f=>read(richRoot+'/'+f).Lessons);
const byCode=new Map(rich.map(l=>[l.LessonCode,l]));
function clean(s){return s.replace(/<script[\s\S]*?<\/script>/gi,' ').replace(/<style[\s\S]*?<\/style>/gi,' ').replace(/<[^>]+>/g,' ').replace(/&nbsp;|&#160;/gi,' ').replace(/&amp;/gi,'&').replace(/&quot;/gi,'"').replace(/&rsquo;|&#8217;/gi,'’').replace(/&lt;/gi,'<').replace(/&gt;/gi,'>').replace(/&\#\d+;/g,' ').replace(/\s+/g,' ').trim()}
function headings(html){return [...html.matchAll(/<h3\b[^>]*>([\s\S]*?)<\/h3>/gi)].map(x=>clean(x[1])).filter(Boolean)}
function sourceImages(html){return [...html.matchAll(/<img\b[^>]*>/gi)].map(x=>x[0]).filter(tag=>!/Kendall|logo|favicon|avatar|profile|icon/i.test(tag)).length}
async function fetchPage(url){for(let n=0;n<2;n++){try{const r=await fetch(url,{signal:AbortSignal.timeout(20000),headers:{'User-Agent':'Edulytiks-Curriculum-Verification/1.0 (Public CC BY 4.0 Source Audit)'}});const t=await r.text();return {status:r.status,sha256:crypto.createHash('sha256').update(t).digest('hex'),html:t}}catch(e){if(n===1)return {status:null,error:String(e.message||e),html:''}}}}
function digest(page){const h=headings(page.html);return {httpStatus:page.status,sha256:page.sha256??null,error:page.error??null,headings:h,activities:h.filter(t=>/^\d+\.\d+:/.test(t)),sourceImageTags:sourceImages(page.html),descriptionMarkers:(page.html.match(/Description:/g)||[]).length,bytes:Buffer.byteLength(page.html)}}
const result=Array(source.rows.length);
let cursor=0;
async function worker(){for(;;){const i=cursor++;if(i>=source.rows.length)break;const x=source.rows[i],studentUrl=x.url.replace('/teachers/','/students/').replace('/preparation.html','/index.html'),teacherUrl=x.url.replace('/preparation.html','/index.html');
 const [s,t]=await Promise.all([fetchPage(studentUrl),fetchPage(teacherUrl)]);const l=byCode.get(x.lessonCode);if(!l)throw Error('Missing '+x.lessonCode);
 const student=digest(s),teacher=digest(t);
 result[i]={lessonCode:x.lessonCode,sourceLessonCode:x.sourceLessonCode,unit:x.unit,lessonTitle:x.title,studentUrl,teacherUrl,student,teacher,richV2CandidateTitle:l.Title,richV2VisualCount:l.Visuals.length,richV2WorkedExamples:l.WorkedExamples.length,
   activityHeadingsFetched:student.httpStatus===200&&student.activities.length>0,
   publisherFigureInspectionNeeded:student.sourceImageTags>0&&l.Visuals.length===0,
   sourceActivitiesIndependentlyCompared:false,sourceGraphicsFaithfullyReproducedVerified:false,studentEffectiveRenderingVerified:false,academicA:false,academicB:false};
 if((i+1)%25===0)console.log('FETCHED_PROGRESS '+(i+1)+'/'+source.rows.length);
}}
(async()=>{await Promise.all([worker(),worker(),worker()]);const failures=result.filter(x=>x.student.httpStatus!==200||x.teacher.httpStatus!==200);
 const report={schemaVersion:1,curriculum:'US-CCSS-MATH:G8:OUR-IM-2017',method:'Fetch full teacher lesson and student lesson HTML; collect SHA256, non-text article structure and activity headings only. Source download is NOT independently judged semantic fidelity.',
 sourceEdition:'2017–2019 Open Up Resources / Illustrative Mathematics, CC BY 4.0',lessons:result.length,student200:result.filter(x=>x.student.httpStatus===200).length,teacher200:result.filter(x=>x.teacher.httpStatus===200).length,
 studentActivityHeadingsFetched:result.filter(x=>x.activityHeadingsFetched).length,
 sourceFigureInspectionPriorities:result.filter(x=>x.publisherFigureInspectionNeeded).length,
 studentMissing:failures.map(x=>({lessonCode:x.lessonCode,studentStatus:x.student.httpStatus,teacherStatus:x.teacher.httpStatus})),
 acadAccepted:0,uncertainty:'No publisher activity contents or figures have been copied into this audit. These source-derived metadata are triage input. Actual source task parity, accessible original visual implementations, authentic UI rendering and A-D acceptance remain unverified.',rows:result};
 const out='docs/curriculum/us-ccss-g8-coder4-v8-publisher-activity-structure-20261029.json';
 fs.writeFileSync(path.join(root,out),JSON.stringify(report,null,2)+'\n');
 console.log('FINAL_SOURCE_AUDIT '+JSON.stringify({file:out,student200:report.student200,teacher200:report.teacher200,activityHeadings:report.studentActivityHeadingsFetched,figuresNeedingReview:report.sourceFigureInspectionPriorities,missing:report.studentMissing.length}))
})().catch(e=>{console.error(e);process.exitCode=1});
