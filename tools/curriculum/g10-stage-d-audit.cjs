#!/usr/bin/env node
'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const exists=p=>fs.existsSync(path.join(root,p));
const walk=(dir)=>exists(dir)?fs.readdirSync(path.join(root,dir),{withFileTypes:true}).flatMap(x=>x.isDirectory()?walk(path.join(dir,x.name)):[path.join(dir,x.name).replaceAll('\\','/')]):[];
const prefix='PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json');
const sourceInventory=read('artifacts/curriculum-delivery/US-CCSS-MATH/G10/g10-local-source-element-inventory-20261106.json');
const sourceCrosswalk=read('artifacts/curriculum-delivery/US-CCSS-MATH/G10/g10-source-activity-crosswalk-diagnostics-20261106.json');
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/GEO_ABC_INDEPENDENT_COURSE_LEDGER_20261030.json');
const progress=read('artifacts/curriculum-delivery/US-CCSS-MATH/G10/full-grade-progress.json');
const files=walk('src/Edulytics.Core/Curriculum/LessonContent/RichV2').filter(f=>/us-ccss-hs-geo-u\d\d-.*\.json$/.test(f));
const rich=files.flatMap(f=>read(f).Lessons.map(x=>({...x,__file:f})));
const visualKinds={};let externalRuntimeVisuals=0;let runtimeVisualCount=0;let studentCopyPublisherMentions=0;
for(const lesson of rich){
  for(const visual of lesson.Visuals||[]){
    runtimeVisualCount++;visualKinds[visual.Kind]=(visualKinds[visual.Kind]||0)+1;
    if(/https?:\/\//i.test(JSON.stringify(visual)))externalRuntimeVisuals++;
  }
  for(const section of [...(lesson.ExplanationParagraphs||[]),...(lesson.SummaryPoints||[])]){
    if(typeof section==='string' && /Illustrative Mathematics|publisher/i.test(section))studentCopyPublisherMentions++;
  }
}
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;
const mapped=mappings.filter(x=>typeof x.lessonCode==='string'&&x.lessonCode.startsWith(prefix));
const verifiedMapped=mapped.filter(x=>x.practiceReadiness==='READY_VERIFIED'&&Array.isArray(x.allowedQuestionFamilies)&&x.allowedQuestionFamilies.length>0);
const counts=[22,15,16,11,18,17,14,11];
const issues=[];const check=(condition,code,detail)=>{if(!condition)issues.push({code,detail})};
check(blueprint.Lessons.length===124,'BLUEPRINT_COUNT',String(blueprint.Lessons.length));
check(rich.length===124,'RICH_COUNT',String(rich.length));
check(externalRuntimeVisuals===0,'RICH_EXTERNAL_VISUAL_SOURCE_URLS',String(externalRuntimeVisuals));
check(ledger.LessonEntries.length===124,'ABC_LEDGER_COUNT',String(ledger.LessonEntries.length));
check(progress.ReadyForStageD===124,'ABC_READY_COUNT',String(progress.ReadyForStageD));
check(sourceInventory.Lessons.length===124,'LOCAL_INVENTORY_LESSON_COUNT',String(sourceInventory.Lessons.length));
check(sourceCrosswalk.lessonCount===124 && sourceCrosswalk.sourceSnapshotId===sourceInventory.SourceSnapshotId,'SOURCE_ACTIVITY_CROSSWALK_STALE',String(sourceCrosswalk.lessonCount));
check(sourceInventory.availableHtmlFiles===496 && sourceInventory.expectedHtmlFiles===496,'LOCAL_SOURCE_HTML_MISSING',String(sourceInventory.availableHtmlFiles));
check(sourceInventory.SourceSnapshotId?.startsWith('g10-local-html-sha256:'),'LOCAL_SOURCE_SNAPSHOT_MISSING','No pinned local HTML digest');
const bc=new Map(blueprint.Lessons.map(l=>[l.LessonCode,l]));
const rc=new Map(rich.map(l=>[l.LessonCode,l]));
const ec=new Map(ledger.LessonEntries.map(l=>[l.LessonCode,l]));
for(const [name,arr] of [['blueprint',blueprint.Lessons],['rich',rich],['ledger',ledger.LessonEntries]]){
  check(new Set(arr.map(x=>x.LessonCode)).size===124,'DUPLICATE_'+name,'duplicate or missing');
}
const lessons=[];let ordinal=0;let mappedSourceLessonCount=0;let mappedSourceRows=0;const sourceMapKinds={};let assessedLessons=0;let teacherAuthoredRoutes=0;let runtimeVerifiedRoutes=0;
for(let u=1;u<=8;u++) for(let l=1;l<=counts[u-1];l++){
  ordinal++;
  const code=prefix+`U${String(u).padStart(2,'0')}:L${String(l).padStart(2,'0')}`;
  const b=bc.get(code),r=rc.get(code),e=ec.get(code);
  check(!!b,'BLUEPRINT_LESSON_MISSING',code);check(!!r,'RICH_LESSON_MISSING',code);check(!!e,'LEDGER_LESSON_MISSING',code);
  const sourceRow=sourceInventory.Lessons[ordinal-1];
  check(sourceRow?.lessonCode===code && sourceRow?.ordinal===ordinal,'LOCAL_SOURCE_INVENTORY_ORDER',code);
  if(sourceRow) for(const [kind,page] of Object.entries(sourceRow.documents||{}))check(page.available===true && typeof page.sha256==='string' && page.sha256.length===64,'LOCAL_SOURCE_PAGE_MISSING_OR_HASH',code+' '+kind);
  if(b) check(b.SortOrder===ordinal && b.UnitNumber===u && b.LessonNumber===l,'BLUEPRINT_ORDER',code);
  if(e) check(e.Ordinal===ordinal && e.ABCReadyForD===true,'ABC_LESSON_NOT_READY',code);
  if(r) check(typeof r.Title==='string' && r.Title.length>0 && Array.isArray(r.WorkedExamples) && Array.isArray(r.ExplanationParagraphs),'RICH_SHAPE',code);
  const assessmentPath=e?.AssessmentPath;
  check(!!assessmentPath && exists(assessmentPath),'ASSESSMENT_ARTIFACT_MISSING',code);
  const assessment=assessmentPath&&exists(assessmentPath)?read(assessmentPath):null;
  if(assessment){
    check(assessment.LessonCode===code,'ASSESSMENT_CODE_MISMATCH',code);
    assessedLessons++;
    const humanKey=['HumanRubric','TeacherHandoff','PaperHandoff','PaperDelivery','TeacherPaperHandoff','TeacherAssessmentRequired','Rubric','OriginalFirstPartyTaskRubric','TeacherReview','TeacherPaperReview','Handoff','Tasks','Modality'].some(k=>assessment[k]!==undefined);
    if(humanKey)teacherAuthoredRoutes++;
    // Do not infer runtime success from authored rubrics, approved owner, or framework-level acceptance.
    // A future verified runtime evidence contract must contain a candidate SHA and an Actions run ID.
    const evidence=assessment.StageDRuntimeEvidence;
    if(evidence?.CandidateSha && evidence?.ActionsRunId && evidence?.TeacherScorePersisted===true && evidence?.TenantIsolated===true)runtimeVerifiedRoutes++;
  }
  const cp=e?.SourceCheckpoint;
  check(!!cp && exists(cp),'SOURCE_CHECKPOINT_MISSING',code);
  const evidence=cp&&exists(cp)?read(cp):null;
  const mapKeys=['SourceTaskCrosswalk','SourceCrosswalk','SourceToRichV2','SourceComparisonA'];
  const documentedMaps=mapKeys.flatMap(k=>Array.isArray(evidence?.[k])?evidence[k].map(x=>({kind:k,row:x})):[]);
  if(documentedMaps.length) mappedSourceLessonCount++;
  mappedSourceRows+=documentedMaps.length;
  for(const item of documentedMaps)sourceMapKinds[item.kind]=(sourceMapKinds[item.kind]||0)+1;
  // Source inventory hashes or a list of activity headings do not establish full 1:1 element parity.
  const hasElementMap=Array.isArray(evidence?.SourceElements) && evidence.SourceElements.length>0 &&
    evidence.SourceElements.every(x=>x.SourceElementId && ['MATCHED_DIGITAL','MATCHED_TEACHER_LED'].includes(x.Status));
  const sourceParity=hasElementMap ? {verified:evidence.SourceElements.length,total:evidence.SourceElements.length} : {verified:null,total:null};
  lessons.push({ordinal,lessonCode:code,unit:u,checkpoint:cp||null,contentFile:r?.__file||null,
     aCStatus:e?.ABCReadyForD?'READY_FOR_STAGE_D':'D_PENDING_A_C_EVIDENCE',
     sourceParity, sourceEvidenceComplete:hasElementMap,
     technicalStatus:'D_PENDING', academicStatus:'ACADEMIC_APPROVAL_PENDING',
     dStatus:hasElementMap?'D_PENDING':'D_PENDING_A_C_EVIDENCE'});
}
const structural={pass:issues.length===0,errors:issues,count:lessons.length,units:8,
  blueprintHash:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json'))).digest('hex'),
  sourceEvidenceCompleteCount:lessons.filter(x=>x.sourceEvidenceComplete).length,
  sourceParity:'UNKNOWN',digitalCoverage:'UNKNOWN',localSourceSnapshotId:sourceInventory.SourceSnapshotId,localSourceHtmlCount:sourceInventory.availableHtmlFiles,localSourceHeadingCount:sourceInventory.totalHeadingElements,runtimeVisualCount,visualKinds,externalRuntimeVisuals,studentCopyPublisherMentions,localSourceVisualRefs:sourceInventory.totalDistinctWithinPageVisualAssets,sourceTeacherActivityIds:sourceCrosswalk.sourceTeacherActivityIdCount,sourceTeacherActivityIdsCrosswalked:sourceCrosswalk.sourceTeacherActivityIdsDocumentedInCrosswalk,sourcePracticeProblemHeadings:sourceCrosswalk.practiceProblemsSourceHeadingCount,sourceActivityCrosswalkMeaningVerified:false,documentedSourceLessonCount:mappedSourceLessonCount,documentedSourceRows:mappedSourceRows,documentedMapKinds:sourceMapKinds,assessmentArtifactCount:assessedLessons,teacherAuthoredCandidateCount:teacherAuthoredRoutes,runtimeVerifiedTeacherRouteCount:runtimeVerifiedRoutes,practiceMappingCount:mapped.length,verifiedPracticeMappingCount:verifiedMapped.length};
const result={structural,lessons};
if(process.argv.includes('--json')) console.log(JSON.stringify(result,null,2));
else console.log(JSON.stringify(structural,null,2));
if(issues.length)process.exitCode=1;
if(process.argv.includes('--strict-practice') && (verifiedMapped.length+runtimeVerifiedRoutes)<124){console.error('FAIL_CLOSED: Grade10 lesson runtime routes require verified digital and/or independently tested teacher-scored route: digital '+verifiedMapped.length+'/124, teacher persisted/tenant-isolated '+runtimeVerifiedRoutes+'/124. Teacher-authored rubrics are not effective runtime proof.');process.exitCode=4}
if(process.argv.includes('--strict-source')&&lessons.some(x=>!x.sourceEvidenceComplete)){console.error('FAIL_CLOSED: grade10 source-element 1:1 coverage not demonstrated');process.exitCode=2}
