#!/usr/bin/env node
'use strict';
// Stage D G11: independently audit A-C handoff and fail closed on unverified source and Practice.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const exists=p=>fs.existsSync(path.join(root,p));
const counts=[11,26,19,18,11,19,16];
const prefix='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:';
const base='artifacts/curriculum-delivery/US-CCSS-MATH/G11/';
const bpFile='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json';
const blueprint=read(bpFile),registry=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const inv=read(base+'local-archive-source-inventory-20261106.json');
const ledger=read(base+'abc-independent-course-ledger.json'),progress=read(base+'full-grade-progress.json');
const richDir='src/Edulytics.Core/Curriculum/LessonContent/RichV2/';
const richFiles=fs.readdirSync(path.join(root,richDir)).filter(x=>/^us-ccss-hs-alg2-u\d\d-l\d\d\.rich-lesson-v2\.json$/.test(x));
const rich=richFiles.flatMap(file=>read(richDir+file).Lessons.map(l=>({...l,__file:richDir+file,__schema:read(richDir+file).SchemaVersion})));
const packs=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const packLessons=packs.lessons.filter(x=>typeof x.lessonCode==='string'&&x.lessonCode.startsWith(prefix));
const mapping=registry.mappings.filter(x=>typeof x.lessonCode==='string'&&x.lessonCode.startsWith(prefix));
const issues=[],lessons=[];const check=(ok,code,detail)=>{if(!ok)issues.push({code,detail})};
check(blueprint.Lessons.length===120,'BLUEPRINT_COUNT',blueprint.Lessons.length);
check(ledger.LessonEntries.length===120,'LEDGER_COUNT',ledger.LessonEntries.length);
check(inv.lessons.length===120,'INVENTORY_COUNT',inv.lessons.length);
check(progress.LessonsReadyForStageD===120,'ABC_READY_COUNT',progress.LessonsReadyForStageD);
check(rich.length===120,'RICH_COUNT',rich.length);
check(packLessons.length===120,'PACK_COUNT',packLessons.length);
for(const [name,arr,key] of [['blueprint',blueprint.Lessons,'LessonCode'],['ledger',ledger.LessonEntries,'LessonCode'],['rich',rich,'LessonCode'],['pack',packLessons,'lessonCode']]){
 check(new Set(arr.map(x=>x[key])).size===120,'DUPLICATE_'+name,arr.length);
}
const byBP=new Map(blueprint.Lessons.map(x=>[x.LessonCode,x]));
const byRich=new Map(rich.map(x=>[x.LessonCode,x]));
const byPack=new Map(packLessons.map(x=>[x.lessonCode,x]));
const byMap=new Map(mapping.map(x=>[x.lessonCode,x]));
let ordinal=0,totalKnown=0,totalVerified=0,teacherAuthor=0,verifiedPractice=0;
let unknownDenominator=0, sourceStatusCounts={};
for(let u=1;u<=7;u++)for(let l=1;l<=counts[u-1];l++){
 ordinal++;const suffix=`U${String(u).padStart(2,'0')}:L${String(l).padStart(2,'0')}`;
 const code=prefix+suffix,e=ledger.LessonEntries[ordinal-1],b=byBP.get(code),r=byRich.get(code),p=byPack.get(code),source=inv.lessons[ordinal-1];
 check(!!e&&e.Ordinal===ordinal&&e.LessonCode===code&&e.ABCReadyForD===true,'LEDGER_ORDER',code);
 check(!!b&&b.SortOrder===ordinal&&b.UnitNumber===u&&b.LessonNumber===l,'BLUEPRINT_ORDER',code);
 check(!!r&&r.__schema===1&&r.CultureCode==='en'&&Array.isArray(r.WorkedExamples)&&r.WorkedExamples.length>=3&&r.WorkedExamples.every(x=>Array.isArray(x.Steps)&&x.Steps.length>=3)&&Array.isArray(r.KeyConcepts)&&r.KeyConcepts.length>=3&&Array.isArray(r.CommonMistakes)&&r.CommonMistakes.length>=2&&Array.isArray(r.SummaryPoints)&&r.SummaryPoints.length>=3&&Array.isArray(r.ExplanationParagraphs)&&r.ExplanationParagraphs.length>=2,'RICH_RUNTIME_VALIDATION',code);
 check(!!p&&Array.isArray(p.translations)&&p.translations.some(t=>t.cultureCode==='en'),'PACK_LESSON',code);
 check(!!source&&source.lessonCode===code,'SOURCE_ORDER',code);
 if(source){for(const [kind,d] of Object.entries(source.documents||{}))check(typeof d.sha256==='string'&&/^[a-f0-9]{64}$/.test(d.sha256),'SOURCE_HASH',code+':'+kind)}
 const evpath=e?.ElementEvidence;check(!!evpath&&exists(evpath),'SOURCE_EVIDENCE_PATH',code);
 const apath=e?.AssessmentPath;check(!!apath&&exists(apath),'ASSESSMENT_PATH',code);
 const cppath=e?.SourceCheckpoint;check(!!cppath&&exists(cppath),'CHECKPOINT_PATH',code);
 const ev=evpath&&exists(evpath)?read(evpath):{};
 const assess=apath&&exists(apath)?read(apath):{};
 const cp=cppath&&exists(cppath)?read(cppath):{};
 check(assess.LessonCode===code,'ASSESSMENT_ID',code);
 check(cp.LessonCode===code,'CHECKPOINT_ID',code);
 const elements=ev.Elements??ev.SourceElements??[];
 check(elements.length>0,'EMPTY_SOURCE_MAP',code);
 check(elements.every(x=>typeof x.SourceElementId==='string'&&x.SourceElementId.length>0),'SOURCE_ELEMENT_ID',code);
 check(new Set(elements.map(x=>x.SourceElementId)).size===elements.length,'SOURCE_ELEMENT_DUPLICATE',code);
 const declared=ev.SourceElementCount??ev.Count??ev.ElementCount;
 check(elements.length===declared,'SOURCE_ELEMENT_COUNT_MISMATCH',code);
 const explicit=elements.filter(x=>['MATCHED_DIGITAL','MATCHED_TEACHER_LED'].includes(x.Status));
 const authored=elements.filter(x=>x.Treatment==='SOURCE_SCOPED_AUTHORING_WITH_TEACHER_REVIEW'||x.Status==='AUTHORED_SOURCE_SCOPED');
 const rights=elements.every(x=>x.RightsStatus==='VERIFIED_CLEAR' && x.VerificationMethod==='INDEPENDENT_SOURCE_SEMANTIC_REVIEW');
 const sourceTotalProven=ev.SourceElementDenominatorVerified===true&&ev.PublisherImagesPdfReviewed===true;
 const parityProven=elements.length>0 && explicit.length===elements.length && rights && sourceTotalProven;
 if(!sourceTotalProven)unknownDenominator++;
 if(sourceTotalProven)totalKnown+=elements.length;
 if(parityProven)totalVerified+=explicit.length;
 for(const el of elements){const status=el.Status||el.Treatment||'UNCLASSIFIED';sourceStatusCounts[status]=(sourceStatusCounts[status]||0)+1}
 if(assess.TeacherReview)teacherAuthor++;
 const m=byMap.get(code);
 if(m?.practiceReadiness==='READY_VERIFIED'&&Array.isArray(m.allowedQuestionFamilies)&&m.allowedQuestionFamilies.length>0&&m.StageDVerifiedCandidateSha&&m.StageDActionsRunId)verifiedPractice++;
 lessons.push({ordinal,lessonCode:code,unit:u,lesson:l,sourceElementsRecorded:elements.length,matchedAuthored:explicit.length,authoringOnly:authored.length,sourceParity:parityProven?'VERIFIED':'UNKNOWN',sourceDenominatorVerified:sourceTotalProven,teacherReviewAuthored:!!assess.TeacherReview,teacherReviewExecuted:false,practiceVerified:!!(m?.practiceReadiness==='READY_VERIFIED'&&m.StageDVerifiedCandidateSha&&m.StageDActionsRunId),aCStatus:e?.ABCReadyForD?'READY_FOR_STAGE_D':'D_PENDING_A_C_EVIDENCE',dStatus:'D_BLOCKED'});
}
const sha=crypto.createHash('sha256').update(fs.readFileSync(path.join(root,bpFile))).digest('hex');
const structural={pass:issues.length===0,errors:issues,lessons:lessons.length,units:7,unitCounts:counts,blueprintSha256:sha,richFiles:richFiles.length,packLessons:packLessons.length,sourceFilesDeclared:inv.lessons.reduce((acc,l)=>acc+Object.keys(l.documents||{}).length,0)};
const sourceAudit={status:unknownDenominator?'UNKNOWN':'VERIFIED_IF_ALL_RIGHTS_AND_SEMANTICS',sourceDenominatorKnownLessons:120-unknownDenominator,sourceDenominatorUnknownLessons:unknownDenominator,sourceKnownElements:totalKnown,verifiedElements:totalVerified,sourceStatusCounts,teacherRubricsAuthored:teacherAuthor,teacherReviewsExecuted:0};
// Teacher-reviewed responses count only after HTTPS evidence exists on the exact candidate,
// including 120 authenticated entry pages and a persisted, authorized sample review.
const teacherAt=process.argv.indexOf('--teacher-manifest');
const shaAt=process.argv.indexOf('--candidate-sha');
let teacherRouteVerified=0, teacherEvidenceRunId=null;
if(teacherAt!==-1 && shaAt!==-1){
 try {
  const manifest=JSON.parse(fs.readFileSync(process.argv[teacherAt+1],'utf8'));
  const candidateSha=process.argv[shaAt+1];
  if (manifest.candidateSha===candidateSha && /^[a-f0-9]{40}$/.test(candidateSha) &&
      manifest.status==='TEACHER_REVIEW_RUNTIME_ROUTE_PASS' &&
      manifest.studentRichV2Verified===120 &&
      manifest.perLessonTeacherRubricEntryVerified===120 &&
      manifest.teacherSubmissionAndReviewPersistedOnSampleLesson===true &&
      manifest.teacherSubmissionAndReviewPersistedForAll120===true &&
      manifest.wrongSchoolTeacherAndStudentDenied===true &&
      Number.isInteger(manifest.actionsRunId) && manifest.actionsRunId>0){
     teacherRouteVerified=120;teacherEvidenceRunId=manifest.actionsRunId;
  }
 }catch(err){console.error('G11 Teacher evidence invalid',String(err));}
}
const effectiveRuntimeLessonRoutes=Math.max(teacherRouteVerified,verifiedPractice);
const practiceAudit={verifiedExactDigitalMappings:verifiedPractice,verifiedTeacherRouteLessonEntries:teacherRouteVerified,teacherActionsRunId:teacherEvidenceRunId,requiredLessonRoutes:120,effectiveRuntimeLessonRoutes,discoveredMappings:mapping.length};
const result={structural,sourceAudit,practiceAudit,lessons};
console.log(JSON.stringify(process.argv.includes('--json')?result:{structural,sourceAudit,practiceAudit},null,2));
if(issues.length)process.exitCode=1;
if(process.argv.includes('--strict-source') && (unknownDenominator || totalVerified!==totalKnown)){console.error('FAIL_CLOSED: 1:1 source denominator, independent semantic and asset/license verification are incomplete');process.exitCode=2}
if(process.argv.includes('--strict-practice') && effectiveRuntimeLessonRoutes!==120){console.error('FAIL_CLOSED: verified Grade 11 effective digital or teacher-reviewed runtime lesson paths '+effectiveRuntimeLessonRoutes+'/120; authored rubrics and generic HTML are not evidence of teacher scoring');process.exitCode=4}
