'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const manifest=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json'),'utf8'));
const selection=JSON.parse(fs.readFileSync(path.join(root,base,'source-selection.json'),'utf8'));
const coverage=JSON.parse(fs.readFileSync(path.join(root,base,'source-toc-and-body-coverage.json'),'utf8'));
test('General 2025-2026 volume1 retains 48 book-source headings without replacing the approved 148-lesson unified index',()=>{
 const general=selection.pdfs.find(x=>x.name==='01.pdf');
 assert.equal(general.classification,'PRIMARY_GENERAL_VOLUME1_TERM1');
 assert.equal(selection.generalVolume1LessonCount,48);
 assert.equal(coverage.mainBookFamilies.find(x=>x.pathway==='General').sourceTocLessonCount,48);
 assert.equal(manifest.LessonCount,148);
 assert.equal(manifest.UnitCount,18);
 assert.equal(selection.finalCanonicalLessonCount,148);
 assert.equal(coverage.actualCanonicalLessonCount,148);
 assert.equal(selection.generalAnnualSourceComplete,false);
});
test('Source limitations remain visible even when owner approved canonical numbering',()=>{
 assert.equal(manifest.FullLessonLevelSemanticAuditComplete,false);
 assert.ok(coverage.sourceGaps.some(x=>x.case==='ADVANCED_NEW_VOLUME2_MISSING'));
 assert.ok(coverage.sourceGaps.some(x=>x.case==='APPLIED_VOLUME1_MISSING'));
 assert.equal(coverage.sourceBodyAcademicCoveragePercent,null);
});
