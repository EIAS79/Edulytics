'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/US-CCSS-MATH/G4/';
const progress=read(base+'full-grade-progress.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;
const families=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json').families;
const byCode=new Map(mappings.map(m=>[m.lessonCode,m]));
const familyIds=new Set(families.map(f=>f.id));
const issues=[],missingMaps=[],pending=[];
let readyABC=0,stageDPassed=0,academicApproved=0,metadataReady=0,metadataPending=0;
const fileExists=p=>fs.existsSync(path.join(root,p));
for(let n=1;n<=149;n++){
 const cp=base+'lesson-'+n+'-v2-stage-a-d-checkpoint.json';
 if(!fileExists(cp)){issues.push('L'+n+': MISSING_CHECKPOINT');continue;}
 let c;try{c=read(cp)}catch(e){issues.push('L'+n+': BAD_CHECKPOINT_JSON');continue;}
 if(c.ordinal!==n || !/^PED:US-CCSS-MATH:G4:U\d{2}:L\d{2}$/.test(c.lessonCode||''))issues.push('L'+n+': IDENTITY_CONFLICT');
 if(c.status==='READY_FOR_STAGE_D' && c.readyForStageD===true && progress.lessonStates?.[n]==='READY_FOR_STAGE_D')readyABC++;
 else issues.push('L'+n+': A_C_CHECKPOINT_MISMATCH');
 if(c.stages?.D==='PASSED')stageDPassed++;
 else if(c.stages?.D!=='NOT_RUN')issues.push('L'+n+': UNKNOWN_D_STATUS');
 if(c.academicApproved===true)academicApproved++;
 if(n>=101){
   const m=byCode.get(c.lessonCode);
   if(!m){missingMaps.push(n);continue;}
   if(!Array.isArray(m.allowedQuestionFamilies)||!m.allowedQuestionFamilies.length)issues.push('L'+n+': NO_ALLOWED_FAMILY');
   for(const f of m.allowedQuestionFamilies||[])if(!familyIds.has(f))issues.push('L'+n+': MISSING_FAMILY:'+f);
   if(m.practiceReadiness==='READY_VERIFIED')metadataReady++;
   else if(m.practiceReadiness==='STAGE_C_AUTHORED_PENDING_STAGE_D'){metadataPending++;pending.push(n);}
   else issues.push('L'+n+': UNKNOWN_PRACTICE_READINESS');
   const test='tests/academic-us-ccss-g4-ordinal-'+n+'-local-source-guard.test.cjs';
   if(!fileExists(test))issues.push('L'+n+': MISSING_STAGE_C_SOURCE_TEST');
   else if(!/readyForStageD\s*,\s*true\b/.test(fs.readFileSync(path.join(root,test),'utf8')))issues.push('L'+n+': STALE_READY_ASSERTION');
 }
}
if(progress.grade!==4||progress.gradeLessonCount!==149||progress.lastClosedOrdinal!==149||progress.readyForStageD!==149)issues.push('MASTER_PROGRESS_MISMATCH');
if(missingMaps.length)issues.push('MISSING_MAPPINGS:'+missingMaps.join(','));
const mode=process.argv[2]||'preflight';
if(mode==='release'){
 if(stageDPassed!==149)issues.push('STAGE_D_NOT_VERIFIED_149_OF_149');
 if(academicApproved!==149)issues.push('HUMAN_ACADEMIC_APPROVAL_NOT_VERIFIED_149_OF_149');
 if(metadataPending)issues.push('PRACTICE_MAPPING_PENDING:'+metadataPending);
}else if(mode!=='preflight')issues.push('INVALID_MODE');
console.log(JSON.stringify({grade:4,edition:2021,scope:149,readyABC,stageDPassed,academicApproved,mappingCount101to149:49-missingMaps.length,mappingReadyVerifiedMetadata:metadataReady,mappingPendingDMetadata:metadataPending,pendingOrdinals:pending,issues,releaseApproved:false},null,2));
if(issues.length)process.exitCode=1;
