'use strict';
// Structural Stage D preflight: passing this diagnostic is NOT production approval.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const file=p=>path.join(root,p);
const exists=p=>typeof p==='string'&&fs.existsSync(file(p));
const read=p=>JSON.parse(fs.readFileSync(file(p),'utf8'));
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const prefix='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const failures=[],releaseBlockers=[],warnings=[];
const fail=(kind,n)=>failures.push({kind,ordinal:n});
const block=(kind,n)=>releaseBlockers.push({kind,ordinal:n});
const counters={checked:0,arabic:0,rich:0,sourceBodyVerified:0,indexOnly:0,teacherScored:0,stageDCertified:0,testsDeclared:0};
if(bp.Units.length!==18)fail('EXPECTED_18_UNITS',0);
if(bp.Lessons.length!==148)fail('EXPECTED_148_BLUEPRINT_LESSONS',0);
if(pack.Lessons.length!==148)fail('EXPECTED_148_PACK_LESSONS',0);
if(pack.AcademicLanguage!=='ar')fail('ARABIC_ACADEMIC_LANGUAGE_REQUIRED',0);
const ordered=[...bp.Lessons].sort((a,b)=>a.SortOrder-b.SortOrder);
const content=new Map(),skills=new Map();
for(const p of pack.Lessons){if(content.has(p.LessonCode))fail('DUPLICATE_PACK_CODE',0);content.set(p.LessonCode,p);}
for(const m of mappings.mappings){if(!skills.has(m.lessonCode))skills.set(m.lessonCode,m);}
const unitCounts={};
for(let n=1;n<=148;n++){
 const b=ordered[n-1];if(!b||b.SortOrder!==n){fail('BLUEPRINT_ORDER',n);continue;}
 unitCounts[b.UnitNumber]=(unitCounts[b.UnitNumber]||0)+1;
 const p=content.get(b.LessonCode);
 if(!p){fail('MISSING_PACK_LESSON',n);continue;}
 const ars=(p.Translations||[]).filter(t=>t.CultureCode==='ar');
 if(ars.length!==1||!ars[0].Title||!ars[0].Explanation||ars[0].Explanation.length<50)fail('ARABIC_TRANSLATION_MISSING',n);
 else {counters.arabic++;if(ars[0].Title!==b.Title)warnings.push({kind:'ARABIC_TITLE_VARIANCE',ordinal:n});}
 const cpPath=prefix+'lesson-'+String(n).padStart(3,'0')+'-stage-a-c-checkpoint.json';
 if(!exists(cpPath)){fail('MISSING_CHECKPOINT',n);continue;}
 const c=read(cpPath);counters.checked++;
 if((c.canonicalOrdinal??c.lessonOrdinal)!==n)fail('ORDINAL_MISMATCH',n);
 if(c.lessonCode!==b.LessonCode)fail('LESSON_CODE_MISMATCH',n);
 if(!/^READY_FOR_STAGE_D(?:_|$)/.test(c.status||''))fail('A_B_C_INCOMPLETE',n);
 if(c.productionRouting===true)fail('PREMATURE_PRODUCTION_ROUTING',n);
 if(c.sourceBodyVerified===true)counters.sourceBodyVerified++;
 if(c.sourceBodyVerified===false){
  counters.indexOnly++;
  if(c.fullSourceBookEquivalenceClaimed===true)fail('FALSE_SOURCE_PARITY',n);
  block('BODY_ABSENT_FIRST_PARTY_REVIEW_NEEDED',n);
 }
 if(c.fullSourceBookEquivalenceClaimed===true&&c.sourceElementDenominatorForThisLesson==null)fail('UNSUPPORTED_100_PERCENT_CLAIM',n);
 if(c.richV2Path&&!exists(c.richV2Path))fail('MISSING_RICH_LESSON',n);
 else if(c.richV2Path){
  const x=read(c.richV2Path).Lessons?.find(v=>v.LessonCode===b.LessonCode);
  if(!x||x.CultureCode!=='ar'||!x.WorkedExamples?.length||!x.Visuals?.length)fail('RICH_LESSON_INCOMPLETE',n);
  else counters.rich++;
 }else fail('RICH_PATH_MISSING',n);
 const m=skills.get(b.LessonCode);
 if(!m||!m.primarySkills?.length)fail('MISSING_MATH_SKILL_MAPPING',n);
 else if(/TEACHER|PENDING|PARTIAL|NOT_READY/.test(m.practiceReadiness||''))counters.teacherScored++;
 for(const t of c.testsWritten||[]){counters.testsDeclared++;if(!exists(t))fail('MISSING_DECLARED_TEST',n);}
 if(['PASS','PASSED'].includes(c.stageD))counters.stageDCertified++;
 else block('STAGE_D_NOT_CERTIFIED',n);
}
if(Object.keys(unitCounts).length!==18)fail('UNIT_DISTRIBUTION',0);
for(let u=1;u<=18;u++){
 const p=prefix+'unit-'+String(u).padStart(2,'0')+'-stage-a-c-progress.json';
 if(!exists(p))fail('UNIT_PROGRESS_MISSING',u);
}
const report={gate:'UAE_G10_148_STAGE_D_PREFLIGHT_V1',candidateSha:process.env.GITHUB_SHA||'LOCAL',
 structuralStatus:failures.length?'FAIL':'PASS',releaseDecision:failures.length||releaseBlockers.length?'NO_GO':'PASS',
 disclaimer:'Structural passing does not certify authored example correctness, textbook exercise parity, student E2E or release.',
 counters,unitCounts,failures,releaseBlockers,warnings};
const output=file('artifacts/uae-g10-stage-d/g10-preflight.json');
fs.mkdirSync(path.dirname(output),{recursive:true});
fs.writeFileSync(output,JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({structuralStatus:report.structuralStatus,releaseDecision:report.releaseDecision,counters,failureCount:failures.length,releaseBlockerCount:releaseBlockers.length,firstFailures:failures.slice(0,15),output:'artifacts/uae-g10-stage-d/g10-preflight.json'}));
if(failures.length||(process.argv.includes('--release')&&releaseBlockers.length))process.exitCode=1;
