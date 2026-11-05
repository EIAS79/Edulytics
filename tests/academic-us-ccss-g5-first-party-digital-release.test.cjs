'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const cp=require('node:child_process');
const root=path.resolve(__dirname,'..');
const reportFile=path.join(root,'stage-d-results/grade5-owner-first-party-digital-release.json');

test('Grade 5 owner first-party release scope requires honest source report but not publisher-identical certification',()=>{
 const run=cp.spawnSync(process.execPath,
  ['tools/curriculum/g5-first-party-digital-release.cjs'],
  {cwd:root,encoding:'utf8',timeout:30000});
 assert.equal(run.status,0,run.stderr+'\n'+run.stdout);
 const r=JSON.parse(fs.readFileSync(reportFile,'utf8'));
 assert.equal(r.scopeLessons,148);
 assert.equal(r.authentic2021ReviewCompleteLessons,148);
 assert.equal(r.richV2LessonCount,148);
 assert.equal(r.richV2FirstPartyVisualDescriptions,286);
 assert.equal(r.copiedOrExternallyLinkedVisualsDetected,0);
 assert.equal(r.externalVideoEmbedsDetected,0);
 assert.ok(r.sourceReferencesPreservedInternally>=95);
 assert.equal(r.ownerAcademicDecision,'GRANTED_FOR_FIRST_PARTY_CONTENT');
 assert.equal(r.ownerProductionPublicationPermission,'GRANTED');
 assert.equal(r.independentPublisherSourceElementCertification,'NOT_CLAIMED');
 assert.equal(r.thirdPartyMediaRightsCertificate,'NOT_CLAIMED');
 assert.equal(r.teacherLedSourceTaskCertification,'NOT_CLAIMED');
 assert.equal(r.firstPartyDigitalScopePreflightPassed,true);
 assert.deepEqual(r.errors,[]);
 const source=JSON.parse(fs.readFileSync(path.join(root,'stage-d-results/grade5-source-evidence.json'),'utf8'));
 assert.equal(r.sourceDiagnosticBlockedLessons,source.blockedLessons);
 assert.equal(r.sourceDiagnosticUnknownDenominators,source.unknownSourceDenominatorLessons);
 assert.equal(source.independentlySourceApprovedLessons,0,
  'not entitled to a fabricated publisher parity certificate');
});

test('first-party digital proof must not erase original 2021 evidence and source-license limitations',()=>{
 const owner=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G5/grade5-owner-academic-release-authorization-20261103.json'),'utf8'));
 for(const value of Object.values(owner.nonClaims))assert.equal(value,false);
 assert.match(owner.rightsAndAttributionPolicy,/attribution must be met/);
 const code=fs.readFileSync(path.join(root,'tools/curriculum/g5-stage-d-diagnostic.cjs'),'utf8');
 assert.match(code,/if\(q\.errors\.length\|\|result\.blockedLessons\)process\.exitCode=2/);
});
