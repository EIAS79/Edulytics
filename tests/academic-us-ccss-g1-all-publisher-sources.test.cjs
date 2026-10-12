'use strict';
const test = require('node:test'), assert = require('node:assert/strict'), fs = require('node:fs'), path = require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const manifest=read('artifacts/academic-us-ccss/grade-reports/G01-146-local-teacher-source-evidence.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json');
const g1Class=fs.readFileSync(path.join(root,'src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs'),'utf8');
const protectedCodes=new Set([...g1Class.matchAll(/public const string \w+LessonCode = "(PED:US-CCSS-MATH:G1:[^"]+)"/g)].map(x=>x[1]));
test('all 146 Grade 1 publisher preparation file identities are pinned without false approval',()=>{
 assert.equal(pack.lessons.length,146);
 assert.equal(manifest.lessonRecords,146);
 assert.equal(manifest.sourceFilesMatched,146);
 assert.equal(manifest.uniqueLessonCount,146);
 assert.equal(manifest.missingSourceFiles,0);
 assert.equal(manifest.independentAcademicApprovals,0);
 assert.equal(manifest.notFinalAcademicApproval,true);
 assert.equal(manifest.materializedStudentBodyVerification,'NOT_TESTED');
 assert.equal(manifest.copyrightLicensingReview,'PENDING');
 assert.deepEqual(manifest.records.map(r=>r.lessonCode),pack.lessons.map(l=>l.lessonCode));
 assert.equal(new Set(manifest.records.map(x=>x.lessonCode)).size,146);
 for(let i=0;i<pack.lessons.length;i++){
  const l=pack.lessons[i],r=manifest.records[i];
  const m=l.lessonCode.match(/:G1:U(\d+):L(\d+)/);
  assert.ok(m,l.lessonCode);
  assert.equal(r.title,l.translations.find(t=>t.cultureCode==='en').title);
  assert.equal(r.recordedCanonicalSourceSha256,l.canonicalBodySha256);
  assert.equal(r.teacherPreparationFile,'unit-'+Number(m[1])+'/lesson-'+Number(m[2]));
  assert.match(r.teacherPreparationSha256,/^[a-f0-9]{64}$/);
  assert.ok(r.teacherGoalExcerpt.length>=8,l.lessonCode);
  assert.equal(r.verificationScope,'ORIGINAL_SOURCE_IDENTITY_AND_GOAL_ONLY');
  assert.equal(r.allEightReview,'PENDING');
  assert.equal(r.independentHumanSignoff,'PENDING');
  assert.equal(r.hasSourceGuardedRepair,protectedCodes.has(r.lessonCode));
 }
});
test('teacher goals are tracked without treating G1 scope as academically certified',()=>{
 const groups={};
 for(const r of manifest.records){const u=r.lessonCode.match(/:U(\d+):/)[1];groups[u]=(groups[u]||0)+1;}
 assert.equal(Object.values(groups).reduce((a,b)=>a+b,0),146);
 assert.equal(Object.keys(groups).length,8);
 assert.equal(manifest.records.filter(x=>x.hasSourceGuardedRepair).length,19);
 assert.ok(manifest.records.some(x=>x.teacherGoalExcerpt.toLowerCase().includes('length')));
 assert.ok(manifest.records.some(x=>x.teacherGoalExcerpt.toLowerCase().includes('shape')));
 assert.ok(manifest.records.some(x=>x.teacherGoalExcerpt.toLowerCase().includes('data')));
});