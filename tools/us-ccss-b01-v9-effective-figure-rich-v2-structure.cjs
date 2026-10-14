#!/usr/bin/env node
'use strict';
// B01 V9: cross-join all 500 corrected/materialized Rich V2 results to
// original 40 teacher-figure lessons. A lesson's EquationSet is not proof
// its source tasks' concrete illustrations were semantically represented.
// Identical/different styling or source image SHA is not a mathematical failure.
const fs=require('node:fs'),assert=require('node:assert/strict');
const re=/^[a-f0-9]{64}$/;
function buildReview(manifest,source,effective){
 assert.equal(manifest?.lessons?.length,500,'B01 manifest must have exactly 500 lessons');
 assert.equal(new Set(manifest.lessons.map(r=>r.lessonCode)).size,500,'Duplicate B01 lesson');
 assert.equal(source?.rows?.length,40,'40 exact original source-figure lesson identities');
 assert.equal(source.coverage?.sourceFigureFramesFound,397,'397 original publisher figure frames');
 assert.equal(source.coverage?.distinctOriginalCMSSources,383,'383 source asset pointers');
 assert.equal(effective?.rows?.length,500,'Materialized 500 effective Rich V2 rows required');
 assert.equal(effective?.coverage?.unique,500);
 assert.equal(effective?.coverage?.materialized,500);
 assert.equal(effective?.academicDecision,'BATCH_NO-GO');
 for(let index=0;index<500;index++){
  assert.equal(manifest.lessons[index].ordinal,index+1,'Manifest ordinal drift');
  assert.equal(effective.rows[index].ordinal,index+1,'Effective ordinal drift');
  assert.equal(effective.rows[index].lessonCode,manifest.lessons[index].lessonCode,
   'A full 500 lesson source/effective identity mismatch');
 }
 const selected=new Map(manifest.lessons.map(r=>[r.lessonCode,r]));
 const learner=new Map(effective.rows.map(r=>[r.lessonCode,r]));
 assert.equal(learner.size,500,'Duplicate materialized lesson');
 const seen=new Set(),out=[];
 let frames=0,images=0,tikz=0,withStructured=0,equationOnly=0,zero=0;
 for(const row of source.rows){
  assert(!seen.has(row.lessonCode),'Source figure code repeated');
  seen.add(row.lessonCode);
  const m=selected.get(row.lessonCode),l=learner.get(row.lessonCode);
  assert(m&&l,'Source figure lesson missing from exact manifest/runtime');
  assert.equal(m.ordinal,row.ordinal);
  assert.equal(m.sourceSemanticSha256,row.sourceSemanticSha256);
  assert.equal(l.ordinal,m.ordinal);
  assert(re.test(l.effectiveBodyFingerprint));
  assert(l.richV2?.composed===true,'Must derive actual composed student visuals');
  assert(l.richV2.visualCount===l.richV2.visualKinds.length);
  const kinds=l.richV2.visualKinds;
  const mathDiagramKinds=kinds.filter(x=>x!=='EquationSet'&&x!=='CalculationChain');
  const issueType=mathDiagramKinds.length===0
   ? (kinds.length?'EQUATIONS_ONLY_SOURCE_FIGURE_REPRESENTATION_REVIEW':'NO_STUDENT_VISUAL_REVIEW')
   :'HAS_STUDENT_DIAGRAM_KIND_MATH_SEMANTICS_UNVERIFIED';
  if(issueType.startsWith('EQUATIONS_ONLY'))equationOnly++;
  if(issueType.startsWith('NO_STUDENT'))zero++;
  if(mathDiagramKinds.length)withStructured++;
  let imageCount=0,tikzCount=0;
  for(const f of row.frames){
   assert(f.rawHtmlFrameRecordLinked===true);
   assert(typeof f.mediaSrc==='string'&&f.mediaSrc.startsWith('https://cms-assets.illustrativemathematics.org/'));
   if(f.publisherAssetKind==='ImageFile')imageCount++;
   else if(f.publisherAssetKind==='TikzFile')tikzCount++;
   else assert.fail('Unexpected source publisher image type');
  }
  frames+=row.frames.length;images+=imageCount;tikz+=tikzCount;
  out.push({
   lessonCode:row.lessonCode,ordinal:m.ordinal,
   exactOriginalTeacherPreparationUrl:m.sourceUrl,
   exactOriginalTeacherActivityUrl:row.publisherTeacherActivityUrl,
   sourceSemanticSha256:m.sourceSemanticSha256,
   effectiveLearnerFingerprint:l.effectiveBodyFingerprint,
   actualRichV2Source:l.richV2.source,
   actualRichV2VisualKinds:kinds,
   actualStructuralDiagramKinds:mathDiagramKinds,
   originalSourceFigureFrames:row.frames.length,
   originalTikzSourceFrames:tikzCount,originalImageFileFrames:imageCount,
   originalSourceFrameAssetURLs:row.frames.map(f=>f.mediaSrc),
   purposeOfReview:issueType,
   originalFigureMathSemanticsIndependentlyVerified:false,
   sourceTaskVsRenderedMathValuesVerified:false,
   originalActivityRenderingBrowserVerified:false,
   firstEditionAttributionRequired:true,
   appearanceOrAssetHashMatchNeededForMathPass:false,
   studentAccessibilityL6Verified:false,
   lessonAcademicallyCertifiedByThisAudit:false
  });
 }
 assert.equal(seen.size,40);assert.equal(frames,397);assert.equal(images,45);
 assert.equal(tikz,352);assert.equal(withStructured+equationOnly+zero,40);
 return {
  schemaVersion:1,planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
  kind:'B01_V9_EXACT_ORIGINAL_FIGURES_VS_REAL_RICH_V2_STRUCTURAL_VISUAL_REVIEW',
  sources:{
   manifest:'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json',
   originalFigures:'artifacts/academic-us-ccss/batches/B01-v9-original-teacher-figure-dom-asset-map-40.json',
   actualRuntime:'500-lesson corrected materializer + RichLessonContentV2RuntimeComposer/sidecars',
   releaseGate:'BATCH_NO-GO'
  },
  methodology:'Cross-join 40 original teacher figure lessons with a real 500-lesson corrected student Rich V2 runtime inventory. Non-equation kinds indicate only the EXISTENCE of a structural visualization, not that mathematical content is the same as the publisher activity. EquationSet alone cannot be counted as task-specific geometry/fraction/data artwork. Image appearance and SHA similarity are never academic disqualifiers. Compare verified original math quantities, geometry, units and structure before marking semantic PASS.',
  coverage:{
   manifestLessons:500,actualMaterializedLessons:500,
   sourceFigureLessons:40,sourceFigureFrames:frames,
   imageFileFrames:images,tikzFrames:tikz,
   studentLessonsWithStructuralDiagram:withStructured,
   studentLessonsOnlyEquations:equationOnly,
   studentLessonsWithNoVisual:zero,
   missingSourceLessonRuntimeJoin:0,
   exactTaskMathDiagramComparisonsApproved:0,
   lessonL6BrowserPassesApproved:0,
   assetRightsApprovedByThisTool:0,
   newAcademicCertifications:0
  },
  highPriorityFigureLessons:out.filter(x=>x.purposeOfReview!=='HAS_STUDENT_DIAGRAM_KIND_MATH_SEMANTICS_UNVERIFIED').map(x=>x.lessonCode),
  rows:out,mathEquivalentIsIndependentOfIllustrationAppearance:true,
  academicDecision:'BATCH_NO-GO',releaseEligible:false
 };
}
if(require.main===module){
 const arg=k=>process.argv.find(x=>x.startsWith('--'+k+'='))?.split('=').slice(1).join('=');
 const get=(file)=>JSON.parse(fs.readFileSync(file,'utf8'));
 const m=get(arg('manifest')||'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json');
 const s=get(arg('source')||'artifacts/academic-us-ccss/batches/B01-v9-original-teacher-figure-dom-asset-map-40.json');
 const e=get(arg('effective')||'b01-v9-effective-root-cause-500.json');
 const out=buildReview(m,s,e),payload=JSON.stringify(out,null,2)+'\n';
 if(arg('out'))fs.writeFileSync(arg('out'),payload);else process.stdout.write(payload);
 console.error(JSON.stringify({sourceFrames:out.coverage.sourceFigureFrames,richV2StructuralKindsPresent:out.coverage.studentLessonsWithStructuralDiagram,equationsOnly:out.coverage.studentLessonsOnlyEquations,mathPasses:out.coverage.exactTaskMathDiagramComparisonsApproved,academicDecision:out.academicDecision}));
}
module.exports={buildReview};