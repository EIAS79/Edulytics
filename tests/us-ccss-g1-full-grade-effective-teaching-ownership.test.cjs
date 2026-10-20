'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');

const ROOT=path.resolve(__dirname,'..');
const evidence='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const seeder='src/Edulytics.Data/Seeding/';
const owners=[
  'CommonCoreG1FullGradeStartingContentCorrections.cs',
  'CommonCoreG1FullGradeFinalUnit4Corrections.cs',
  'CommonCoreG1FullGradeUnit5ContentCorrections.cs',
  'CommonCoreG1FullGradeUnit6To8ContentCorrections.cs',
  'CommonCoreG1SourceGroundedInstructionalCorrections.cs'
];
const read=relative=>JSON.parse(fs.readFileSync(path.join(ROOT,relative),'utf8'));
const sources=owners.map(name=>({
  path:seeder+name, text:fs.readFileSync(path.join(ROOT,seeder,name),'utf8')
}));
const progress=read(evidence+'full-grade-progress.json');
const manifest=read(evidence+'grade-manifest.json');
const map=read(evidence+'full-grade-practice-candidate-map.json');
const evidenceMatrix=read(evidence+'full-grade-academic-evidence-matrix.json');

test('entire 96-lesson original teaching cohort retains exactly one existing effective source correction owner per canonical LessonCode',()=>{
  assert.equal(progress.newLessons.length,96);
  assert.equal(map.records.length,96);
  assert.equal(evidenceMatrix.records.length,96);
  for(let i=0;i<96;i++){
    const row=progress.newLessons[i],canonical=manifest.lessons[i+50],
      mapped=map.records[i],audit=evidenceMatrix.records[i];
    assert.equal(row.ordinal,i+51);
    assert.equal(row.lessonCode,canonical.lessonCode);
    assert.equal(mapped.lessonCode,row.lessonCode);
    assert.equal(audit.lessonCode,row.lessonCode);
    const matches=sources.filter(owner=>owner.text.includes(row.lessonCode));
    assert.equal(matches.length,1,
      'Missing/duplicate source-bound Rich V2 correction owner for '+row.lessonCode+
      ': '+matches.map(x=>x.path).join(', '));
  }
});

test('the 29 source-verified grade-wide resumption checkpoints reference their real existing correction owner without claiming V1 acceptance',()=>{
  const original=[108,109,110,111,112,114,115,117,118,119,
    120,121,122,123,124,127,128,129,130,131,133,135,136,137,
    138,139,143,145,146];
  assert.equal(original.length,29);
  for(const ordinal of original){
    const name='lesson-'+ordinal+'-v2-stage-a-d-checkpoint.json',
      checkpoint=read(evidence+name),
      row=progress.newLessons[ordinal-51];
    const real=sources.filter(owner=>owner.text.includes(row.lessonCode));
    assert.equal(real.length,1);
    assert.equal(checkpoint.lessonCode,row.lessonCode);
    assert.equal(checkpoint.stageB.candidateSourceRepairPath,real[0].path,
      'A resumption checkpoint must identify the original guarded correction, not an invented duplicate: '+ordinal);
    assert.equal(checkpoint.stageA.allOriginalFigureAndStudentTasksFullyAudited,false);
    assert.equal(checkpoint.stageD.originalV1PerLessonAcademicAcceptance,'NOT_CLAIMED');
    assert.equal(checkpoint.ledgerStatus,'IN_PROGRESS');
  }
});
