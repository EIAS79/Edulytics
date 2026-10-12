#!/usr/bin/env node
'use strict';
// PHASE 04: machine-detectable content risks. Flags are NOT academic decisions.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),cp=require('node:child_process');
const root=path.resolve(__dirname,'..'),write=process.argv.includes('--write');
// Baseline SHA identifies source inputs. Final candidate SHA is separately
// checked at the independent academic/release signoff gate.
const sha=JSON.parse(fs.readFileSync(path.join(root,
 'artifacts/academic-us-ccss/00-source-manifest.json'),'utf8')).sourceCommitSha;
const hash=s=>crypto.createHash('sha256').update(String(s||'')).digest('hex');
const bpDir='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs';
const cpDir='src/Edulytics.Core/Curriculum/LessonContent/Packs';
const bpFiles=fs.readdirSync(path.join(root,bpDir)).filter(n=>/^us-ccss-math-.*\.lesson-blueprint\.json$/.test(n)).sort();
const contentFiles=fs.readdirSync(path.join(root,cpDir)).filter(n=>/^us-ccss-math-.*\.lesson-content-pack\.json$/.test(n)).sort();
if(bpFiles.length!==17||contentFiles.length!==17)
 throw Error('Expected 17 blueprint and 17 content pack source files');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const blueprints=new Map();
for(const f of bpFiles){
 const p=bpDir+'/'+f, b=load(p);
 for(const l of b.Lessons){
  if(blueprints.has(l.LessonCode))throw Error('Duplicate blueprint code '+l.LessonCode);
  blueprints.set(l.LessonCode,{blueprint:b.BlueprintCode,sourceFile:p,
   title:l.Title||'',sourceUrl:l.SourceUrl||'',officialOutcomeCodes:l.OutcomeCodes||[]});
 }
}
if(blueprints.size!==1560)throw Error('Expect 1560 source blueprint lesson identities');
const byCode=new Map(),packSummary=[];
for(const f of contentFiles){
 const p=cpDir+'/'+f, b=load(p);
 if(b.packCode!=='US-CCSS-MATH'||!Array.isArray(b.lessons))throw Error('Invalid content pack '+p);
 packSummary.push({file:p,lessons:b.lessons.length,contentVersion:b.contentVersion||null,
  claimedReviewMethod:b.reviewMethod||null,reviewedBy:b.reviewedBy||null,
  publisherSource:b.pedagogicalSourceUrl||null});
 for(const l of b.lessons){
  if(byCode.has(l.lessonCode))throw Error('Duplicate content lesson '+l.lessonCode);
  byCode.set(l.lessonCode,{file:p,lesson:l});
 }
}
const fields=['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
const repeats=new Map(),records=[];
for(const [code,b] of blueprints){
 const linked=byCode.get(code), flags=[];
 let chars={},data={};
 if(!linked){flags.push('NO_CANONICAL_RICH_CONTENT_IN_17_SOURCE_PACKS');}
 else {
  const translations=linked.lesson.translations||[];
  const en=translations.find(x=>x.cultureCode==='en');
  if(!en)flags.push('MISSING_ENGLISH_TRANSLATION');
  for(const f of fields){
   const val=String(en?.[f]||'').trim();data[f]=val;chars[f]=val.length;
   if(val.length<40)flags.push('INSUFFICIENT_'+f.toUpperCase());
   if(val.length>=40&&['workedExamples','stepByStepSolutions'].includes(f)){
    const key=f+':'+hash(val);
    if(!repeats.has(key))repeats.set(key,[]);
    repeats.get(key).push(code);
   }
  }
  // Specific, conservative inconsistency: task explicitly about multiples,
  // but procedure exclusively discusses classification of geometric figures.
  const focus=b.title.toLowerCase();
  const steps=String(data.stepByStepSolutions||'').toLowerCase();
  if(/\bmultiples?\b/.test(focus) &&
     /\b(classify|figure|angles|corners)\b/.test(steps) &&
     !/\b(multiple|area|product|factor|rectangle)\b/.test(steps))
    flags.push('MATH_TOPIC_PROCEDURE_CONFLICT_MULTIPLES_VS_FIGURE_CLASSIFICATION');
  if((data.workedExamples||'').includes('&nbsp;')||(data.workedExamples||'').includes('&lt;'))
    flags.push('PUBLISHER_HTML_ENTITY_LEAK_IN_WORKED_EXAMPLES');
 }
 records.push({lessonCode:code,lessonTitle:b.title,blueprintCode:b.blueprint,
  publisherSourceUrl:b.sourceUrl,contentSourceFile:linked?.file||null,
  canonicalBodySha256:linked?.lesson.canonicalBodySha256||null,
  translationCharCounts:chars,contentRiskFlags:flags,
  academicReviewStatus:'PENDING_INDEPENDENT_REVIEW'});
}
const repeating=[];
for(const [key,lessons] of repeats){
 if(lessons.length>=4)repeating.push({field:key.split(':')[0],textSha256:key.split(':')[1],
  identicalLessonCount:lessons.length,sampleLessonCodes:lessons.slice(0,12),
  action:'REVIEW_SEMANTIC_SPECIFICITY'});
}
repeating.sort((a,b)=>b.identicalLessonCount-a.identicalLessonCount);
const reusedProcedureCodes=new Set();
for(const [key,codes] of repeats)if(codes.length>=4)for(const code of codes)reusedProcedureCodes.add(code);
const flagCounts={};for(const r of records)for(const flag of r.contentRiskFlags)flagCounts[flag]=(flagCounts[flag]||0)+1;
const missing=records.filter(r=>r.contentSourceFile===null).length;
if(records.length!==1560)throw Error('Lesson audit count mismatch');
const out={schemaVersion:1,scope:'US-CCSS-MATH/CCSSM-2010/G1-G12',
 sourceCommitSha:sha,reviewType:'AUTOMATED_CONTENT_RISK_DETECTION_NOT_ACADEMIC_CERTIFICATION',
 academicApproval:'NO-GO',basis:{blueprintFiles:17,contentPackFiles:17},
 summary:{expectedLessons:1560,lessonsWithCanonicalContent:1560-missing,
  missingCanonicalContent:missing,riskFlagCounts:flagCounts,
  exactRepeatedLongProcedures:repeating.length,
  lessonsSharingOneOfThoseProcedures:reusedProcedureCodes.size,
  largestIdenticalProcedureUsage:Math.max(0,...repeating.map(x=>x.identicalLessonCount)),
  humanReviewed:0},
 repeatedProcedures:repeating,sourcePacks:packSummary,
 lessons:records};
const p=path.join(root,'artifacts/academic-us-ccss/04-automated-content-risk-audit.json');
const text=JSON.stringify(out,null,2)+'\n';
if(write){fs.mkdirSync(path.dirname(p),{recursive:true});fs.writeFileSync(p,text);}
else if(!fs.existsSync(p)||fs.readFileSync(p,'utf8')!==text)throw Error('04 audit not generated or drift');
console.log(JSON.stringify(out.summary,null,2));
