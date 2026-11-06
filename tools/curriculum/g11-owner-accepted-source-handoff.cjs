#!/usr/bin/env node
'use strict';
// Stage D checks source handoff integrity + formal academic authorization.
// This does NOT manufacture an independent 1:1 source parity certification.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const text=p=>fs.readFileSync(path.join(root,p),'utf8');
const prefix='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:';
const base='artifacts/curriculum-delivery/US-CCSS-MATH/G11/';
const owner=read(base+'grade11-owner-academic-publication-authorization-20261106.json');
const truth=read(base+'source-of-truth.json');
const inventory=read(base+'local-archive-source-inventory-20261106.json');
const ledger=read(base+'abc-independent-course-ledger.json');
const licencePage=text('src/Edulytics.Web/wwwroot/content-sources-licenses.html');
const studentLayout=text('src/Edulytics.Web/Views/Shared/_StudentLayout.cshtml');
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json');
const errors=[];const ck=(p,code)=>{if(!p)errors.push(code)};
ck(owner.OwnerDecision==='ACADEMICALLY_APPROVED_BY_OWNER','OWNER_NOT_APPROVED');
ck(owner.PublicationAuthorization==='OWNER_APPROVES_PUBLICATION_AFTER_REQUIRED_TECHNICAL_AND_LEGAL_GATES','PUBLICATION_NOT_AUTHORIZED');
ck(owner.NoFalseCIOrReviewerApproval===true,'OWNER_TECHNICAL_BOUNDARY');
ck(truth.TeacherStudentFilesRead===600,'SOURCE_ARCHIVE_COUNT');
ck(JSON.stringify(truth.Exclude).includes('AccessIM_Algebra2')&&JSON.stringify(truth.Exclude).includes('v360'),'WRONG_SOURCE_EXCLUSION');
ck(ledger.TotalLessons===120&&ledger.LessonEntries.length===120,'ABC_TOTAL');
ck(inventory.lessons.length===120,'SOURCE_LESSONS');
ck(licencePage.includes('Grade 11 Algebra 2 (2019)')&&licencePage.includes('Illustrative Mathematics')&&licencePage.includes('creativecommons.org/licenses/by/4.0/')&&licencePage.includes('im.kendallhunt.com/HS/teachers/3/index.html'),'LEGAL_ATTRIBUTION_OUTSIDE_LESSON');
ck(studentLayout.includes('/content-sources-licenses.html'),'LEGAL_NOTICE_NOT_AVAILABLE_TO_STUDENT');
ck(blueprint.Sources.some(x=>x.License==='CC BY 4.0' && x.RootUrl==='https://im.kendallhunt.com/HS/teachers/3/index.html'),'G11_SOURCE_LICENSE_METADATA_MISSING');
let recordedSourceElements=0,sourcePages=0,richVisuals=0,externalMedia=0,studentBrandMentions=0;
for(const e of ledger.LessonEntries){
  ck(e.ABCReadyForD===true && e.LessonCode.startsWith(prefix),'ABC_LESSON_NOT_READY:'+e.LessonCode);
  const v=read(e.ElementEvidence);
  const elements=v.Elements||v.SourceElements||[];
  const total=v.SourceElementCount??v.ElementCount??v.Count;
  ck(elements.length===total&&total>0,'SOURCE_ELEMENT_LIST_COUNT:'+e.LessonCode);
  ck(elements.every(x=>x.SourceElementId),'SOURCE_ELEMENT_MISSING_ID:'+e.LessonCode);
  ck(new Set(elements.map(x=>x.SourceElementId)).size===total,'SOURCE_ELEMENT_DUPLICATES:'+e.LessonCode);
  recordedSourceElements+=elements.length;
  const doc=inventory.lessons[e.Ordinal-1];
  ck(doc?.lessonCode===e.LessonCode,'SOURCE_INVENTORY_ORDER:'+e.LessonCode);
  for(const page of Object.values(doc?.documents||{})){
    ck(/^[0-9a-f]{64}$/.test(page.sha256),'SOURCE_DOCUMENT_HASH:'+e.LessonCode);
    sourcePages++;
  }
  const u=String(e.Unit).padStart(2,'0'),l=String(e.Lesson).padStart(2,'0');
  const f='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u'+u+'-l'+l+'.rich-lesson-v2.json';
  const rich=read(f);
  ck(rich.SchemaVersion===1&&rich.Lessons.length===1&&rich.Lessons[0].LessonCode===e.LessonCode,'RICH_RUNTIME_RESOURCE:'+e.LessonCode);
  const lesson=rich.Lessons[0];
  richVisuals+=(lesson.Visuals||[]).length;
  externalMedia+=(lesson.Videos||[]).length;
  const learnerProse=JSON.stringify([lesson.ExplanationParagraphs,lesson.SummaryPoints,lesson.KeyConcepts,lesson.WorkedExamples]);
  if(/(?:Illustrative Mathematics|Kendall Hunt|Open Up Resources|https?:\/\/)/i.test(learnerProse)) studentBrandMentions++;
}
ck(sourcePages===600,'SOURCE_PAGE_HASH_COUNT');
ck(richVisuals===0&&externalMedia===0,'UNREVIEWED_RUNTIME_MEDIA_ASSET_PRESENT');
ck(studentBrandMentions===0,'PUBLISHER_BRANDING_IN_LEARNER_PROSE');
const report={status:errors.length?'FAIL':'OWNER_ACADEMIC_HANDOFF_ACCEPTED',
 academicApproval:'APPROVED_BY_PROGRAMME_OWNER',independentPublisherElementParity:'NOT_INDEPENDENTLY_VERIFIED',
 sourceLessonsChecked:120,sourcePagesHashed:sourcePages,sourceElementsRecorded:recordedSourceElements,
 sourceElementMeaningIndependentlyVerified:false,richVisuals,externalMedia,studentBrandMentions,
 centralContentLicenceNoticePresent:licencePage.includes('Grade 11 Algebra 2 (2019)'),
 protectedStatus:'ACADEMIC_OWNER_APPROVAL_IS_NOT_PROOF_OF_RIGHTS_FOR_FUTURE_IMAGES',errors};
console.log(JSON.stringify(report,null,2));
if(errors.length)process.exitCode=2;
