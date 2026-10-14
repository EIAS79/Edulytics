'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const {buildReview}=require('../tools/us-ccss-b01-v9-effective-figure-rich-v2-structure.cjs');
const file=x=>JSON.parse(fs.readFileSync(path.join(root,x),'utf8'));
const base='artifacts/academic-us-ccss/batches/';
const manifest=file(base+'B01-lesson-manifest.json');
const source=file(base+'B01-v9-original-teacher-figure-dom-asset-map-40.json');
const effective=file('b01-v9-effective-root-cause-500.json');
test('Join all 500 actual Rich V2 lessons with 40 source figure lessons, detect equations-only structural gaps and never certify math',()=>{
 const d=buildReview(manifest,source,effective);
 assert.equal(d.rows.length,40);
 assert.equal(d.coverage.actualMaterializedLessons,500);
 assert.equal(d.coverage.sourceFigureFrames,397);
 assert.equal(d.coverage.imageFileFrames,45);
 assert.equal(d.coverage.tikzFrames,352);
 assert.equal(d.coverage.studentLessonsOnlyEquations,0);
 assert.equal(d.coverage.studentLessonsWithStructuralDiagram,40);
 assert.equal(d.coverage.studentLessonsWithNoVisual,0);
 assert.equal(d.highPriorityFigureLessons.length,0);
 const byCode=new Map(d.rows.map(x=>[x.lessonCode,x]));
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G2:U06:L02').actualStructuralDiagramKinds,Array(5).fill('PolygonDiagram'));
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G3:U06:L09').actualStructuralDiagramKinds,['ClockFace']);
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G2:U08:L02').actualStructuralDiagramKinds,['CounterPairs','CounterPairs']);
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G2:U08:L03').actualStructuralDiagramKinds,['CounterPairs','CounterPairs']);
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G3:U07:L04').actualStructuralDiagramKinds,['PolygonDiagram','PolygonDiagram','PolygonDiagram']);
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G3:U08:L01').actualStructuralDiagramKinds,['FractionBars']);
 assert.deepEqual(byCode.get('PED:US-CCSS-MATH:G4:U04:L20').actualStructuralDiagramKinds,['PlaceValueChart']);
 assert.equal(d.coverage.exactTaskMathDiagramComparisonsApproved,0);
 assert.equal(d.coverage.lessonL6BrowserPassesApproved,0);
 assert.equal(d.academicDecision,'BATCH_NO-GO');
 assert.equal(d.releaseEligible,false);
 assert.equal(d.mathEquivalentIsIndependentOfIllustrationAppearance,true);
 for(const row of d.rows){
   assert.equal(row.originalSourceFigureFrames,row.originalSourceFrameAssetURLs.length);
   assert(row.effectiveLearnerFingerprint.length===64);
   assert.equal(row.originalFigureMathSemanticsIndependentlyVerified,false);
   assert.equal(row.sourceTaskVsRenderedMathValuesVerified,false);
   assert.equal(row.studentAccessibilityL6Verified,false);
   assert.equal(row.lessonAcademicallyCertifiedByThisAudit,false);
 }
});
test('Negative controls reject missing materialized lesson, fake visuals, manifest/source drift and certificate fraud',()=>{
 const mutations=[
  ([m,s,e])=>e.rows.pop(),
  ([m,s,e])=>e.coverage.materialized=499,
  ([m,s,e])=>s.rows.pop(),
  ([m,s,e])=>s.rows[0].sourceSemanticSha256='0'.repeat(64),
  ([m,s,e])=>m.lessons[0].ordinal=55,
  ([m,s,e])=>e.rows.find(x=>x.lessonCode===s.rows[0].lessonCode).richV2.composed=false,
  ([m,s,e])=>e.rows.find(x=>x.lessonCode===s.rows[0].lessonCode).richV2.visualCount=99,
  ([m,s,e])=>s.rows[0].frames[0].rawHtmlFrameRecordLinked=false,
  ([m,s,e])=>e.academicDecision='BATCH_CERTIFIED'
 ];
 for(const mutate of mutations){
  const copies=[structuredClone(manifest),structuredClone(source),structuredClone(effective)];
  mutate(copies);assert.throws(()=>buildReview(...copies));
 }
});