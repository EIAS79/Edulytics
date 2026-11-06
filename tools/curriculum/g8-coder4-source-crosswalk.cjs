'use strict';
// Public preparation-page crosswalk. Automated source capture is not educational approval.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..'),bpPath='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json';
const bp=JSON.parse(fs.readFileSync(path.join(root,bpPath),'utf8'));
const target=path.join(root,'docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const clean=s=>s.replace(/<[^>]*>/g,' ').replace(/&#(\d+);/g,(_,n)=>String.fromCodePoint(+n)).replace(/&#x([\da-f]+);/gi,(_,n)=>String.fromCodePoint(parseInt(n,16))).replace(/&nbsp;/g,' ').replace(/&amp;/g,'&').replace(/&rsquo;/g,'’').replace(/&quot;/g,'"').replace(/\s+/g,' ').trim();
const roleName=s=>s.replace(/\s/g,'');
function extract(html,l){
 const a=html.indexOf('Learning Goals</h3>'),b=html.indexOf('CCSS Standards</h3>'),c=html.indexOf('Learning Targets</h3>');
 if(a<0||b<0)throw Error('Missing learning/standards headings');
 const segment=html.slice(a,c>a?c:b);
 const match=segment.match(/Student Facing<\/h3>[\s\S]*?<div class="im-c-content">([\s\S]*?)<\/div>/);
 const goal=match?clean(match[1]):null;
 const standards=html.slice(b,Math.min(html.length,b+9000));
 const roles=[];
 for(const m of standards.matchAll(/<p class="im-c-hero__underline">([\s\S]*?)<\/p>\s*<ul\b[^>]*>([\s\S]*?)<\/ul>/g))
 for(const n of m[2].matchAll(/<a\b[^>]*>([\s\S]*?)<\/a>/g))
  roles.push({role:roleName(clean(m[1])),reference:clean(n[1])});
 const src=new Set(roles.map(r=>r.role+':'+r.reference));
 const blue=new Set(l.Alignments.map(r=>r.Role+':'+r.ReferenceCode));
 if(!goal||!roles.length)throw Error('Cannot extract student goal or standards');
 return {goal,roles,blueprintExtra:[...blue].filter(x=>!src.has(x)),blueprintMissing:[...src].filter(x=>!blue.has(x))};
}
const rows=new Array(bp.Lessons.length);let next=0;
async function worker(){
 for(;;){const i=next++;if(i>=rows.length)return;const l=bp.Lessons[i];let last;
 for(let attempt=0;attempt<3;attempt++){try{
  const r=await fetch(l.SourceUrl,{signal:AbortSignal.timeout(15000)});
  if(!r.ok)throw Error('HTTP_'+r.status);
  const html=await r.text(),data=extract(html,l);
  rows[i]={ordinal:i+1,lessonCode:l.LessonCode,sourceLessonCode:l.SourceLessonCode,title:l.Title,unit:l.UnitNumber,url:l.SourceUrl,
   retrievalDate:'2026-10-29',httpStatus:r.status,sourcePageSha256:crypto.createHash('sha256').update(html).digest('hex'),
   sourceStudentGoal:data.goal,sourceRoles:data.roles,blueprintExtraRoles:data.blueprintExtra,blueprintMissingRoles:data.blueprintMissing,
   fullStudentActivityReview:false,authoringAcceptance:'NOT_VERIFIED'};last=null;break;
 }catch(e){last=String(e.message||e);await new Promise(resolve=>setTimeout(resolve,250*(attempt+1)));}}
 if(last)rows[i]={ordinal:i+1,lessonCode:l.LessonCode,url:l.SourceUrl,error:last,authoringAcceptance:'NOT_VERIFIED'};
 if((i+1)%25===0)console.log('CROSSWALK '+(i+1)+'/'+rows.length);
 }}
(async()=>{await Promise.all(Array.from({length:5},()=>worker()));const successes=rows.filter(r=>r.sourceStudentGoal);
const report={schemaVersion:1,curriculum:'US-CCSS-MATH',blueprint:'US-CCSS-MATH:G8:OUR-IM-2017',checkedAt:'2026-10-29',total:rows.length,
retrieved:successes.length,missing:rows.length-successes.length,roleDiscrepancies:successes.filter(r=>r.blueprintExtraRoles.length||r.blueprintMissingRoles.length).length,
verifiedAcademicLessons:0,disclaimer:'Read-only public preparation HTML retrieval is not a lesson-by-lesson activity review, academic acceptance or checked student/Practice runtime.',rows};
fs.writeFileSync(target,JSON.stringify(report,null,2)+'\n');
console.log('CROSSWALK_RESULT '+JSON.stringify({total:report.total,retrieved:report.retrieved,missing:report.missing,roleDiscrepancies:report.roleDiscrepancies,missingCodes:rows.filter(r=>r.error).map(r=>r.lessonCode)}));
if(report.missing)process.exitCode=2;
})().catch(e=>{console.error(e);process.exitCode=1;});
