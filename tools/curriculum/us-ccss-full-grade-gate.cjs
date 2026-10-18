'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const ROOT = path.resolve(__dirname,'../..');
const PLAN = 'EDULYTICS-GRADE-FIRST-FULL-GRADE-V1';
const HISTORIC_SHA = '2c7c43a8f3af7151d422acc6e8e9e8fa5348833c';
const MANIFEST = 'artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json';
const LEDGER = 'artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json';
const isSha = s => typeof s === 'string' && /^[0-9a-f]{40}$/i.test(s);
const isHash = s => typeof s === 'string' && /^[0-9a-f]{64}$/i.test(s);
const load = p => JSON.parse(fs.readFileSync(path.resolve(ROOT,p),'utf8'));

function initial(manifest) {
  if (!Array.isArray(manifest?.lessons) || manifest.lessons.length !== 146 ||
    manifest.lessons.some((x,i)=>x.gradeOrdinal!==i+1) ||
    new Set(manifest.lessons.map(x=>x.lessonCode)).size!==146)
    throw new Error('Grade 1 canonical inventory not 146 unique ordered lessons');
  return {
    schemaVersion:1, planId:PLAN, programme:'US-CCSS-MATH', grade:'G1',
    gradeCount:146,
    historical:{first:1,last:50,count:50,status:'HISTORICALLY_RELEASED_NO_RETEST',
      lastBatch:'US-CCSS-G1-010',mainSha:HISTORIC_SHA,
      sourceEvidence:'https://github.com/EIAS79/Edulytics/pull/566#issuecomment-6253349510',
      note:'Evidence is historical; do not redo academic approval of ordinals 1-50.'},
    newScope:{first:51,last:146,count:96},
    nextOrdinal:51,
    release:'NO_GO',
    ownerUat:'PENDING',
    newLessons:manifest.lessons.slice(50).map(x=>({
      ordinal:x.gradeOrdinal,lessonCode:x.lessonCode,
      unit:x.unitNumber,lessonNumber:x.lessonNumber,
      title:x.title,sourceTeacherUrl:x.sourceTeacherUrl,
      outcomeCodes:x.officialOutcomeCodes,status:'NOT_STARTED',evidence:null
    }))
  };
}
function validEvidenceFile(rel, hash) {
  if (typeof rel!=='string'||!rel||path.isAbsolute(rel)||!isHash(hash)) return false;
  const abs=path.resolve(ROOT,rel);
  if (!abs.startsWith(ROOT+path.sep)||!fs.existsSync(abs)||!fs.statSync(abs).isFile()) return false;
  const actual=crypto.createHash('sha256').update(fs.readFileSync(abs)).digest('hex');
  return actual.toLowerCase()===hash.toLowerCase();
}
function evaluate(manifest,ledger,candidateSha) {
  const issues=[],missing=[];
  if(!Array.isArray(manifest?.lessons)) return {outcome:'NO_GO',issues:['No valid grade manifest']};
  const source=manifest.lessons,rows=ledger?.newLessons||[];
  if(source.length!==146 || source.some((x,i)=>x.gradeOrdinal!==i+1) ||
     new Set(source.map(x=>x.lessonCode)).size!==146) issues.push('Canonical inventory is not unique/ordered 146');
  if(ledger?.schemaVersion!==1||ledger?.planId!==PLAN||ledger?.programme!=='US-CCSS-MATH'||ledger?.grade!=='G1'||ledger?.gradeCount!==146)
    issues.push('Incorrect plan, grade or grade count');
  const h=ledger?.historical;
  if(h?.first!==1||h?.last!==50||h?.count!==50||h?.status!=='HISTORICALLY_RELEASED_NO_RETEST'||
     h?.lastBatch!=='US-CCSS-G1-010'||h?.mainSha!==HISTORIC_SHA||
     h?.sourceEvidence!=='https://github.com/EIAS79/Edulytics/pull/566#issuecomment-6253349510')
    issues.push('Historical 50-lesson snapshot tampered/missing');
  if(ledger?.newScope?.first!==51||ledger?.newScope?.last!==146||ledger?.newScope?.count!==96||rows.length!==96)
    issues.push('New lesson scope must be exactly 51-146');
  if(candidateSha!==undefined && !isSha(candidateSha)) issues.push('Candidate SHA must be exact 40 hex');
  let accepted=0, firstUnfinished=147;
  for(let i=0;i<96;i++){
    const ord=i+51,original=source[i+50],r=rows[i];
    if(!r||!original){missing.push({ordinal:ord,reason:'Missing canonical/new lesson'});firstUnfinished=Math.min(ord,firstUnfinished);continue;}
    if(r.ordinal!==ord||r.lessonCode!==original.lessonCode||
       r.unit!==original.unitNumber||r.lessonNumber!==original.lessonNumber||
       r.title!==original.title||r.sourceTeacherUrl!==original.sourceTeacherUrl||
       JSON.stringify(r.outcomeCodes)!==JSON.stringify(original.officialOutcomeCodes)) {
       issues.push('Lesson identity mismatch at '+ord);firstUnfinished=Math.min(ord,firstUnfinished);continue;
    }
    if(r.status!=='INTERNALLY_ACCEPTED'){missing.push({ordinal:ord,reason:'Not accepted: '+r.status});firstUnfinished=Math.min(ord,firstUnfinished);continue;}
    const e=r.evidence||{};
    const valid = (e.reviewedOriginalTeacherSource===true && e.reviewedOriginalStudentTask===true &&
      e.licenseVerified===true && e.actualLearnerRenderingVerified===true &&
      e.realPracticeGradingPersisted===true && e.independentCorrectAndWrongAnswersVerified===true &&
      Array.isArray(e.allowedFamilies)&&e.allowedFamilies.length>0 &&
      Array.isArray(e.forbiddenFamilies)&&
      Array.isArray(e.sourceObjectives)&&e.sourceObjectives.length>0 &&
      isHash(e.sourceSha256)&&isHash(e.effectiveStudentBodySha256)&&
      isSha(e.candidateSha) &&
      (candidateSha===undefined || candidateSha===e.candidateSha) &&
      validEvidenceFile(e.sourceReviewPath,e.sourceReviewFileSha256) &&
      validEvidenceFile(e.independentGradingProofPath,e.independentGradingProofSha256) &&
      validEvidenceFile(e.runtimePracticeProofPath,e.runtimePracticeProofSha256) &&
      Array.isArray(e.tests)&&e.tests.length>0 && e.tests.every(x=>validEvidenceFile(x.path,x.sha256)));
    if(!valid){missing.push({ordinal:ord,reason:'Invalid/missing verifiable source, grading, runtime, tests or exact-candidate evidence'});firstUnfinished=Math.min(ord,firstUnfinished);continue;}
    accepted++;
  }
  if(ledger?.nextOrdinal!==firstUnfinished) issues.push('Progress nextOrdinal disagrees with verified acceptance; expected '+firstUnfinished);
  const ready=issues.length===0 && missing.length===0 && accepted===96;
  return {planId:PLAN,grade:'G1',historicReleasedCount:50,historicRetestedCount:0,
    newRequired:96,newAccepted:accepted,newNotAccepted:96-accepted,
    gradeCoverageOnSuccess:ready?146:null,
    outcome:ready?'GRADE_READY_FOR_REQUIRED_CI_AND_RELEASE':'NO_GO',
    productionLive:false,ownerUat:'PENDING',
    issues,missing};
}
function main(args){
  const manifest=load(MANIFEST),ledger=load(LEDGER);
  const shaArg=args.find(x=>x.startsWith('--sha='));
  const sha=shaArg?.slice(6);
  if(args.includes('--release')&&!sha) throw new Error('Use --release --sha=<exact-commit-sha>');
  const report=evaluate(manifest,ledger,args.includes('--release')?sha:undefined);
  if(args.includes('--inspect')) report.missing=report.missing.slice(0,5);
  console.log(JSON.stringify(report,null,2));
  if(report.issues.length || (args.includes('--release') && report.outcome==='NO_GO'))process.exitCode=1;
}
if(require.main===module){try{main(process.argv.slice(2));}catch(e){console.error(e.stack);process.exitCode=2;}}
module.exports={initial,evaluate,validEvidenceFile};
