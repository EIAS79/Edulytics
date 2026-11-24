'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const canonical=fs.readFileSync(path.join(root,base,'G10_SEVEN_BOOKS_UNIFIED_DEDUP_TOC_2026-11-24.md'),'utf8');
const manifest=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json'),'utf8'));
const matches=[...canonical.matchAll(/^\| (\d+) \| `(G10-U\d\d-L\d\d)` \| ([^|]+) \| ([^|]+) \|$/gm)];
test('The only authorized Grade10 TOC contains 148 unique ordered lessons in 18 units',()=>{
 assert.equal(manifest.Grade,10);assert.equal(manifest.Pathway,'General');
 assert.equal(manifest.LessonCount,148);assert.equal(manifest.UnitCount,18);
 assert.equal(manifest.CanonicalUnifiedIndexLocked,true);
 assert.equal(matches.length,148);
 assert.deepEqual(matches.map(m=>Number(m[1])),Array.from({length:148},(_,i)=>i+1));
 assert.equal(new Set(matches.map(m=>m[2])).size,148,'canonical lesson code cannot repeat');
 assert.equal(new Set(matches.map(m=>m[3].trim().normalize('NFKC').replace(/\s+/g,' '))).size,148,'canonical title cannot repeat');
 assert.equal(manifest.Units.length,18);
 assert.equal(manifest.Units.reduce((n,u)=>n+u.ls.length,0),148);
 for(const u of manifest.Units) for(let i=0;i<u.ls.length;i++) {
   const code=u.lessonCodes[i];
   const row=matches.find(m=>m[2]===code);
   assert.ok(row,code);assert.equal(row[3].trim(),u.ls[i]);
 }
});
test('Obsolete competing indexes and Grade10 Advanced official manifest are not present in working tree',()=>{
 for(const name of ['source-index-preliminary.json','source-193-main-toc-raw-entry-ledger.json','general-2025-volume1-48-verified-book-toc.json'])
   assert.equal(fs.existsSync(path.join(root,base,name)),false,name);
 assert.equal(fs.existsSync(path.join(root,'docs/curriculum/UAE_G10_ADVANCED_OFFICIAL_MANIFEST.json')),false);
});
test('Approval of 148 headings does not falsely certify lesson content or Stage D',()=>{
 assert.equal(manifest.FullLessonLevelSemanticAuditComplete,false);
 assert.equal(manifest.ExistingRuntimeImplementationLessonCount,48);
 assert.match(manifest.RuntimeMigrationStatus,/NOT_APPLIED/);
});
