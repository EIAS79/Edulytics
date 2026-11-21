'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
const read=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
const sha=rel=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,rel))).digest('hex');

test('U01L01 actual scanned 2025-26 book pages visually map eight structural source tasks without authorizing full parity',()=>{
 const audit=read(base+'u01l01-source-visual-structure-closure-NO_GO.json');
 const source=read(base+'u01l01-61-source-visual-skill-audit.json');
 const index=read(base+'grade9-82-target-index.json');
 assert.equal(audit.sourceSha256,index.records[0].SourcePdfSha256);
 assert.deepEqual(audit.sourcePhysicalPdfPages,[16,17,18,19,20]);
 assert.deepEqual(audit.sourcePrintedPages,[5,6,7,8,9]);
 assert.equal(audit.sourcePagesVisuallyInspectedDirectly,true);
 assert.equal(audit.originalNumberedQuestionsVisuallyClassified,61);
 assert.equal(audit.independentlyAuthoredSkillAnalogs,61);
 assert.deepEqual(audit.visualStructureMapping.map(x=>x.sourceItem),[29,37,41,43,44,46,47,48].map(n=>'U01L01-Q'+String(n).padStart(2,'0')));
 for(const r of audit.visualStructureMapping){
  const ordinal=Number(r.sourceItem.slice(-2));
  assert.equal(source.records[ordinal-1].sourcePrintedPage,r.sourcePrintedPage);
  assert.equal(source.records[ordinal-1].sourcePdfPage1Based,r.sourcePdfPage1Based);
  assert.equal(source.records[ordinal-1].additionalTaskVisualizationReviewed,true);
  assert.equal(r.studentAnswerAutomaticallyGraded,false);
  assert.equal(r.fullSourceAtomicParityGranted,false);
  assert.equal(r.actualHumanTeacherScoringCompleted,false);
  assert.equal(r.sourceArtReproduced,false);
 }
 assert.equal(audit.fullSourceLessonAuditPassed,false);
 assert.equal(audit.targetLessonStageDClosed,false);
 assert.equal(audit.sourceIndexComplete,false);
 assert.equal(audit.productionImportAllowed,false);
 assert.equal(audit.releaseDecision,'NO_GO');
 assert.equal(source.sourceElementDenominatorVerified,false);
 assert.equal(source.fullyAuditedRawSourceLesson,false);
 assert.equal(source.independentEndToEndStudentTestingCompleted,false);
});

test('Visual task repair evidence is anchored to the exact current student activity and independent source-chart JS SHA256',()=>{
 const audit=read(base+'u01l01-source-visual-structure-closure-NO_GO.json');
 assert.equal(sha(audit.studentPlotBuilderModule),audit.studentPlotBuilderSha256);
 assert.equal(sha(audit.originalStudentActivity),audit.originalStudentActivitySha256);
 const html=fs.readFileSync(path.join(root,audit.originalStudentActivity),'utf8');
 const js=fs.readFileSync(path.join(root,audit.studentPlotBuilderModule),'utf8');
 assert.ok(html.includes('id="g9-power-pattern-template"'));
 assert.ok(html.includes('geometry-rectangle-gallery'));
 assert.ok(html.includes('g9-cube-cells-grid'));
 assert.ok(html.includes('u01-l01-source-plot-builder.js'));
 assert.ok(js.includes('TEACHER_REVIEW_REQUIRED'));
 assert.ok(js.includes('dataset.plotDraft'));
 assert.ok(!js.includes('fetch('));
 assert.ok(!js.includes('XMLHttpRequest'));
 assert.ok(!js.includes('navigator.sendBeacon'));
 const browser=audit.realEdgeBrowserEvidence;
 assert.equal(browser.totalQuestions,browser.coreQuestionsLoaded+browser.priorReviewQuestionsLoaded);
 assert.equal(browser.studentBarChartGenerated,true);
 assert.equal(browser.studentBoxChartGenerated,true);
 assert.equal(browser.exportedDiagramDrafts,2);
 assert.equal(browser.studentResponsesSubmittedToServer,false);
 assert.equal(browser.hostedCIOrAuthenticatedStudentAcceptanceEquivalent,false);
});

test('Latest independently observed Arabic exercise visual corrections do not change unfinished Stage D first lesson gate or fake teacher review',()=>{
 const audit=read(base+'u01l01-source-visual-structure-closure-NO_GO.json');
 const ledger=read(base+'grade9-stage-d-execution-ledger.json');
 const source=read(base+'u01l01-61-source-visual-skill-audit.json');
 assert.equal(ledger.firstIncompleteLessonOrdinal,1);
 assert.equal(ledger.sourceSemanticLock.rawLessonAtomicAuditPassed,0);
 assert.equal(ledger.sourceSemanticLock.targetLessonElementParityPassed,0);
 assert.deepEqual(source.requiresFurtherTextAmbiguityInspection,[7,25]);
 assert.equal(source.sourcePublisherDefinitiveIntendedExpressionKnownForExceptions,false);
 assert.ok(audit.remainingAcademicGates.length>=4);
 assert.ok(audit.visualStructureMapping.every(x=>x.fullSourceAtomicParityGranted===false));
 assert.ok(source.records.every(x=>x.sourceExerciseItemFullyCertified===false));
});
test('Direct scanned-source nonnumbered instructional structures are checked without pretending entire lesson or grade is approved',()=>{
 const extra=read(base+'u01l01-nonnumbered-source-structure-reinspection-NO_GO.json');
 const old=read(base+'u01l01-nonnumbered-source-visual-subset.json');
 const main=read(base+'u01l01-source-visual-structure-closure-NO_GO.json');
 const html=fs.readFileSync(path.join(root,extra.activityFile),'utf8');
 assert.equal(extra.sourceSha256,main.sourceSha256);
 assert.deepEqual(extra.sourcePrintedPages,[5,6,7,8,9]);
 assert.equal(extra.previouslyRecordedNonNumberedItems,old.elements.length);
 assert.equal(extra.newSupplementarySourceStructuralGroupsVisuallyInspected,7);
 assert.equal(extra.sourceStructuralChecks.length,7);
 assert.equal(new Set(extra.sourceStructuralChecks.map(e=>e.id)).size,7);
 assert.equal(extra.sourceStructuralChecks.find(e=>e.id==='EXTERNAL_SKILLS_CAREER_MEDIA').correspondingIndependentContentLocallyVerified,false);
 assert.ok(extra.sourceStructuralChecks.every(e=>e.sourcePageDirectlyVisuallyInspected));
 assert.ok(extra.sourceStructuralChecks.every(e=>e.fullSourceAtomicEquivalenceCertified===false));
 assert.equal(extra.activitySha256,sha(extra.activityFile));
 assert.ok(html.includes('id="g9-source-notation-original"'));
 assert.ok(html.includes('class="g9-operation-words"'));
 assert.ok(html.includes('aria-label="'));
 assert.equal(extra.realEdgeLocalVerification.operationTableRows,4);
 assert.equal(extra.realEdgeLocalVerification.arabicIndicDigitsPreservedInChartTextFields,true);
 assert.equal(extra.realEdgeLocalVerification.noAutomaticGradingOrServerSubmission,true);
 assert.equal(extra.fullRawSourceLessonAtomicAuditPassed,false);
 assert.equal(extra.canonicalGrade9LessonStageDClosed,false);
 assert.equal(extra.productionImportAllowed,false);
 assert.equal(extra.releaseDecision,'NO_GO');
 assert.equal(main.targetLessonStageDClosed,false);
 assert.equal(old.sourceTotalAtomicElementDenominatorVerified,false);
});


test('Direct U01L01 five-page source review records exact physical pages and independent numerical findings but no source lock',()=>{
 const source=read(base+'u01l01-stage-d-selective-source-visual-audit.json');
 const r=source.directFivePageSourceReinspection20261121;
 assert.deepEqual(r.printedPages,[5,6,7,8,9]);
 assert.deepEqual(r.physicalPdfPages1Based,[16,17,18,19,20]);
 assert.deepEqual(r.numberedExerciseContainersByPrintedPage,{'7':34,'8':8,'9':19});
 assert.equal(r.observedTotalNumberedExerciseContainers,61);
 assert.equal(r.knownLetteredChildrenQ34ToQ37,11);
 assert.deepEqual(r.independentTeacherOracleMathSamples.q48FiveNumberSummarySeconds,[860,945,1035,1150,1290]);
 assert.equal(r.independentTeacherOracleMathSamples.q48IqrSeconds,205);
 assert.equal(r.independentTeacherOracleMathSamples.q48RangeSeconds,430);
 assert.deepEqual(r.sourceTextAmbiguitiesStillRequireSpecificReview,[7,25]);
 assert.equal(r.fullSourceAtomicDenominatorEstablished,false);
 assert.equal(r.publisherSourceExerciseOneToOneParityGranted,false);
 assert.equal(source.all61ExercisesIndividuallyCompared,false);
 assert.equal(source.fullSourceAtomicAuditPassed,false);
});
