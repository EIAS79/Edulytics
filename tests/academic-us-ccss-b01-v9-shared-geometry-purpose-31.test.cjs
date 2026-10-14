'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const base=path.join(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(base,p),'utf8'));
function inputs(){return {manifest:read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
 index:read('artifacts/academic-us-ccss/batches/B01-book-source-reference-index-500.json'),
 cohort:read('artifacts/academic-us-ccss/batches/B01-v9-geometry-template-31-source-purpose-reviewed.json'),
 packs:[2,3,4].map(g=>read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json'))};}
function verify(o){
 const {manifest,index,cohort,packs}=o;
 assert.equal(cohort.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(cohort.batchId,'B01');
 assert.equal(cohort.academicDecision,'BATCH_NO-GO');
 assert.equal(cohort.academicCertificationGranted,0);
 assert.equal(manifest.lessons.length,500);
 assert.equal(index.rows.length,500);
 assert.equal(cohort.rows.length,31);
 assert.equal(cohort.scope.sourceScope,31);
 assert.equal(cohort.scope.newUniqueSourcePurposes,31);
 assert.equal(cohort.scope.sourceProvenWrongGenericProcedureNeedsCorrection,28);
 assert.equal(cohort.scope.alreadyOverridden,2);
 assert.equal(cohort.scope.sourceAppropriateGenericTemplate,1);
 const sourceCodes=new Set(),templates=new Set(),f=new Set();
 const all=new Map(packs.flatMap(p=>p.lessons.map(l=>[l.lessonCode,l])));
 for(const row of cohort.rows){
  assert.ok(!sourceCodes.has(row.lessonCode),'Duplicate original lesson purpose');
  sourceCodes.add(row.lessonCode);
  const idx=row.ordinal-1,m=manifest.lessons[idx],s=index.rows[idx],lesson=all.get(row.lessonCode);
  assert.ok(m&&s&&lesson);
  assert.equal(m.ordinal,row.ordinal);
  assert.equal(s.lessonCode,row.lessonCode);
  assert.equal(m.lessonCode,row.lessonCode);
  assert.equal(m.sourceUrl,row.sourceOriginalPreparationUrl);
  assert.equal(m.sourceSemanticSha256,row.sourceSemanticSha256);
  assert.equal(s.localFileByteSha256,row.sourceFileInventorySha256);
  assert.equal(s.localRelativePath,row.sourceLocalRelativePath);
  assert.equal(row.officialTeacherActivityUrl,m.sourceUrl.replace('/preparation.html','/lesson.html'));
  assert.equal(row.sourceActivityPageHTTP,'READ_SUCCESS_200');
  assert.equal(row.lessonAcademicL1toL6CertifiedByThisEvidence,false);
  const en=lesson.translations.find(t=>t.cultureCode==='en');
  assert.ok(en);
  assert.ok(en.stepByStepSolutions.startsWith('Step 1: Identify the figure and the attributes named in the task.'));
  if(row.needsSharedCauseCorrection) f.add(row.sourceTaskMathematicalFamily);
  templates.add(row.lessonCode);
 }
 assert.equal(sourceCodes.size,31);
 assert.equal(f.size,22);
 assert.equal(cohort.rows.filter(r=>r.needsSharedCauseCorrection).length,28);
 assert.equal(cohort.rows.filter(r=>!r.needsSharedCauseCorrection).length,3);
 assert.equal(cohort.rows.filter(r=>!r.currentV9EffectiveGenericTemplate).length,2);
 assert.equal(cohort.rows.filter(r=>r.sourceTaskMathematicalFamily==='GEOMETRIC_ATTRIBUTES_CLASSIFICATION').length,1);
}
test('Read-only original 31 teacher goals/activity-purpose reviews stay a whole-cohort source-bound decision',()=>verify(inputs()));
test('Fake academic passes, source identities and source inventory digests cannot be smuggled into a whole cohort',()=>{
 const tamper=[
  o=>o.cohort.rows.pop(),
  o=>o.cohort.rows[0].sourceSemanticSha256='0'.repeat(64),
  o=>o.cohort.rows[1].sourceFileInventorySha256='0'.repeat(64),
  o=>o.cohort.rows[2].needsSharedCauseCorrection=false,
  o=>o.cohort.academicCertificationGranted=28,
  o=>o.manifest.lessons.pop()
 ];
 for(const f of tamper){const v=inputs();f(v);assert.throws(()=>verify(v));}
});
