#!/usr/bin/env node
'use strict';
// Unified V9 B01 source-figure dependency audit, full 500/500 cross-joined.
// A Rich V2 visual is not the source task's diagram and is never accepted as
// proof merely because the component exists. Do not republish publisher art.
const fs=require('node:fs');
const assert=require('node:assert/strict');
const ORIGINAL_CUE_TO_RENDER_FAMILY={
 DATA_GRAPH:['BarGraph','PictureGraph','LinePlot'],
 PLACE_VALUE:['PlaceValueChart','BaseTenBlocks'],
 NUMBER_LINE:['NumberLine'],
 FRACTION_PARTS:['FractionBars','FractionArea','FractionCircle'],
 MEASUREMENT_SCALE:['Scale','Ruler','MeasurementScale'],
 RECTANGULAR_ARRAY:['RectangleDiagram','ShapeDecomposition','ArrayModel','DotArray'],
 DOT_IMAGES:['DotArray','TenFrame','Counters'],
 CLOCK_FACE:['Clock','ClockFace']
};
function buildFigureDependencies(graph,effective){
 assert.equal(graph?.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(graph?.batchId,'B01');
 assert.equal(effective?.planId,graph.planId);
 assert.equal(effective?.batchId,'B01');
 assert.equal(graph.coverage.observed,500);
 assert.equal(graph.coverage.unique,500);
 assert.equal(graph.nodes.length,500);
 assert.equal(effective.coverage.joined,500);
 assert.equal(effective.coverage.unique,500);
 assert.equal(effective.rows.length,500);
 assert.equal(effective.academicDecision,'BATCH_NO-GO');
 const roots=effective.sharedPatternRoots.SOURCE_FIGURE_CAPTION_PLACEHOLDER;
 assert.ok(roots&&roots.rawCandidates===44&&
  roots.effectiveMarkerStillPresent===40&&roots.effectiveMarkerAbsent===4,
  'V9 source figure dependency group cardinality drift');
 const raw=new Map(graph.nodes.map(x=>[x.lessonCode,x]));
 const current=new Map(effective.rows.map(x=>[x.lessonCode,x]));
 assert.equal(raw.size,500); assert.equal(current.size,500);
 for(let i=0;i<500;i++){
  assert.equal(graph.nodes[i].lessonCode,effective.rows[i].lessonCode,
   '500-wide academic lineage mismatch');
  assert.equal(graph.nodes[i].ordinal,i+1);
  assert.equal(effective.rows[i].ordinal,i+1);
 }
 const signals=[],visualCategoryCounts={},gapByCause={};
 const noSourceActivityFigureImages=[];
 for(const code of roots.effectiveMarkerLessonCodes){
  const g=raw.get(code),r=current.get(code);
  assert.ok(g&&r,'Unlisted figure reference '+code);
  assert.equal(r.effectivePatternSignals.figureCaptionPlaceholder,true);
  assert.match(g.sourceSemanticSha256,/^[a-f0-9]{64}$/i);
  assert.match(r.effectiveLearnerFingerprint,/^[a-f0-9]{64}$/i);
  const cues=g.originalTeacherActivitySignals||[];
  const visuals=r.richV2VisualKinds||[];
  const compatible=cues.filter(cue=>(ORIGINAL_CUE_TO_RENDER_FAMILY[cue]||[])
   .some(kind=>visuals.includes(kind)));
  const unmatched=cues.filter(cue=>!compatible.includes(cue));
  const risk=unmatched.length>0?'SOURCE_CUE_VISUAL_KIND_FAMILY_NOT_MATCHED':
   cues.length===0?'ORIGINAL_ACTIVITY_FIGURE_SEMANTICS_UNINDEXED':
   'RENDER_KIND_FAMILY_COMPATIBLE_SEMANTICS_UNVERIFIED';
  gapByCause[risk]=(gapByCause[risk]||0)+1;
  for(const kind of visuals)visualCategoryCounts[kind]=(visualCategoryCounts[kind]||0)+1;
  // The indexed source is a preparation.html file, NOT a verified archive
  // of its separate student-facing activity figure assets.
  noSourceActivityFigureImages.push(code);
  signals.push({
   ordinal:g.ordinal,lessonCode:code,
   canonicalSourceSemanticSha256:g.sourceSemanticSha256,
   effectiveLessonFingerprint:r.effectiveLearnerFingerprint,
   originalTeacherSourceCues:cues,
   effectiveRichV2VisualKinds:visuals,
   effectiveRichV2Source:r.richV2Source,
   structurallyCompatibleSourceCueFamilies:compatible,
   uncorrelatedSourceCueFamilies:unmatched,
   issueType:risk,
   exactSourceActivityDiagramRetrieved:false,
   sourceActivityImageAssetVerified:false,
   comparableSemanticGeometryOrDataVerified:false,
   accessibleRenderedBrowserSnapshotVerified:false,
   sourceActivityAssetAction:'FETCH_ORIGINAL_ACTIVITY_FIGURE_METADATA_AND_DRAW_INDEPENDENT_RIGHTS_SAFE_EQUIVALENT_IF_NEEDED',
   licenseStatus:'NOT_DETERMINED_FROM_PREPARATION_INDEX',
   releaseDisposition:'BLOCKED_AWAITING_SOURCE_ACTIVITY_FIGURE_AND_RENDERED_COMPARISON',
   automaticFigureReplacementAllowed:false
  });
 }
 assert.equal(new Set(signals.map(s=>s.lessonCode)).size,40);
 assert.equal(signals.length,40);
 assert.equal(roots.effectiveMarkerLessonCodes.length,40);
 return {
  schemaVersion:1,planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
  kind:'B01_V9_SOURCE_FIGURE_REFERENCE_EFFECTIVE_DEPENDENCIES',
  referencePopulation:{batchLessons:500,rawFigureLabelCandidates:44,
   stillEffectiveUnboundFigureLabelCandidates:40,previouslyTransformed:4},
  structuralCorrelationTotals:gapByCause,
  renderedVisualKinds:visualCategoryCounts,
  originalActivityFigureAssetsMissingFromVerifiedSourceEvidence:{
   count:noSourceActivityFigureImages.length,lessonCodes:noSourceActivityFigureImages,
   note:'Source preparation HTML alone does not prove presence, absence or exact data of separate activity figures.'},
  cohorts:signals,confirmedFigureMismatchCount:'NOT_ACADEMICALLY_ESTABLISHED',
  figureReplacementAuthority:false,academicCertificationAwarded:0,
  needsSource:'READ_ONCE_ORIGINAL_ACTIVITY_VISUAL_ELEMENT_CARDS_AND_INDEPENDENT_RIGHTS_SAFE_DRAWING_CONTRACT',
  needsRuntime:'ACTUAL_AUTHENTICATED_DESKTOP_MOBILE_ACCESSIBILITY_VISUAL_L6_COMPARISON',
  academicDecision:'BATCH_NO-GO'
 };
}
if(require.main===module){
 const arg=k=>process.argv.find(x=>x.startsWith('--'+k+'='))?.split('=').slice(1).join('=');
 const a=JSON.parse(fs.readFileSync(arg('graph')||'b01-v9-global-root-cause-500.json','utf8'));
 const b=JSON.parse(fs.readFileSync(arg('effective')||'b01-v9-global-effective-root-causes-500.json','utf8'));
 const x=buildFigureDependencies(a,b),txt=JSON.stringify(x,null,2)+'\n';
 if(arg('out'))fs.writeFileSync(arg('out'),txt);else process.stdout.write(txt);
 process.stderr.write('V9 B01 figures: 40/40 effective figure-reference blockers; labels are not diagrams, BATCH_NO-GO\n');
}
module.exports={buildFigureDependencies};
