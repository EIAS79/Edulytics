// Algebra 1 only. Reconcile the review inventory with authored Rich V2 files,
// never convert a file's existence into academic, Practice or runtime approval.
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..', '..');
const progress = path.join(root, 'docs/curriculum/US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json');
const current = JSON.parse(fs.readFileSync(progress, 'utf8'));
const reviewedSourceLessons = new Set([
  'U01:L01','U01:L02','U01:L03','U01:L04',
  'U01:L05','U01:L06','U01:L07','U01:L08',
  'U01:L09','U01:L10','U01:L11','U01:L12','U01:L13',
  'U01:L14','U01:L15','U01:L16',
  'U02:L01','U02:L02','U02:L03','U02:L04','U02:L05',
  'U02:L06','U02:L07','U02:L08','U02:L09','U02:L10',
  'U02:L11','U02:L12','U02:L13','U02:L14',
  'U02:L15','U02:L16','U02:L17','U02:L18',
  'U02:L19','U02:L20','U02:L21','U02:L22',
  'U02:L23','U02:L24','U02:L25','U02:L26',
  'U03:L01','U03:L02','U03:L03','U03:L04','U03:L05',
  'U03:L06','U03:L07','U03:L08','U03:L09','U03:L10',
  'U04:L01','U04:L02','U04:L03','U04:L04','U04:L05',
  'U04:L06','U04:L07','U04:L08',
  'U04:L09','U04:L10','U04:L11','U04:L12','U04:L13',
  'U04:L14','U04:L15','U04:L16','U04:L17','U04:L18',
  'U05:L01','U05:L02','U05:L03','U05:L04','U05:L05',
  'U05:L06','U05:L07','U05:L08','U05:L09','U05:L10',
  'U05:L11','U05:L12','U05:L13','U05:L14','U05:L15',
  'U05:L16','U05:L17','U05:L18','U05:L19','U05:L20','U05:L21',
  'U06:L01','U06:L02','U06:L03','U06:L04','U06:L05',
  'U06:L06','U06:L07','U06:L08','U06:L09','U06:L10',
  'U06:L11','U06:L12','U06:L13','U06:L14','U06:L15',
  'U06:L16','U06:L17',
  'U07:L01','U07:L02','U07:L03','U07:L04','U07:L05','U07:L06',
  'U07:L07','U07:L08','U07:L09','U07:L10','U07:L11','U07:L12',
  'U07:L13','U07:L14','U07:L15','U07:L16','U07:L17','U07:L18',
  'U07:L19','U07:L20','U07:L21','U07:L22','U07:L23','U07:L24'
]);
let authoringCandidates = 0;
let sourceReviewed = 0;
for (const row of current.lessons) {
  const key = 'U' + String(row.unitNumber).padStart(2, '0') + ':L'
    + String(row.lessonNumber).padStart(2, '0');
  const filename = 'us-ccss-hs-alg1-u'
    + String(row.unitNumber).padStart(2, '0') + '-l'
    + String(row.lessonNumber).padStart(2, '0') + '-2021.rich-lesson-v2.json';
  const relative = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/' + filename;
  const file = path.join(root, ...relative.split('/'));
  if (reviewedSourceLessons.has(key)) {
    if (!fs.existsSync(file)) throw new Error('Missing reviewed sidecar ' + relative);
    const json = JSON.parse(fs.readFileSync(file, 'utf8'));
    if (json.Lessons.length !== 1 ||
        json.Lessons[0].LessonCode !== row.lessonCode ||
        json.Lessons[0].Title !== row.title) {
      throw new Error('Identity/title mismatch ' + relative);
    }
    if (!json.Lessons[0].ExplanationParagraphs.some(p => p.includes(row.officialTeacherPreparationUrl))) {
      throw new Error('Official source URL absent ' + relative);
    }
    row.sourceStudentAndTeacherPagesDirectlyReviewed = true;
    row.authoredRichV2Sidecar = relative;
    row.stage = 'SOURCE_REVIEWED_RICH_V2_AUTHORING_CANDIDATE_D_PENDING';
    row.blocker = 'RICH_V2_CANDIDATE_REQUIRES_EXACT_PRACTICE_AND_AUTHENTICATED_EFFECTIVE_RUNTIME_AND_GRADING_EVIDENCE';
    sourceReviewed++;
    authoringCandidates++;
  }
  row.evidenceClassification = 'BLOCKED_WITH_EXACT_CAUSE';
}
if (current.lessons.length !== 132 || new Set(current.lessons.map(x => x.lessonCode)).size !== 132) {
  throw new Error('Course identity drift');
}
current.counts.directSourceSampleReviewed = sourceReviewed;
current.counts.richV2AuthoringCandidates = authoringCandidates;
current.counts.evidenceVerifiedLessons = 0;
current.counts.practiceVerifiedLessons = 0;
current.counts.authenticatedRuntimeVerified = 0;
current.recoveryNote = 'Review and authored Rich V2 candidates only; no per-lesson exact Practice/persistence/teacher assessment or authenticated deployment verification. Do not infer success from files.';
current.ciDiagnosis = {
  baseSha: '39c757630d2bd67217a1468fce099653ed0f3f53',
  failingCandidateHead: '343a25f856818b80eb09ea6359ebf7611f3ceac8',
  detectedRegression: 'Shared Rich V2 tests globally assert 26+143+149=318 before extra Algebra 1 files were introduced. First three extra files raised result to 321. These tests must scope original legacy baselines and independently check each new course.',
  integrationOwner: 'American Supervisor 2 single-writer assignment required',
  affectedSharedTestFiles: [
    'tests/Edulytics.Tests/MathematicsIntelligence/RichLessonContentV2PilotTests.cs',
    'tests/Edulytics.Tests/MathematicsIntelligence/RichLessonContentV2RolloutTests.cs'
  ]
};
fs.writeFileSync(progress, JSON.stringify(current, null, 2) + '\n', 'utf8');
console.log(JSON.stringify({
  total: current.lessons.length,
  authoredCandidates: authoringCandidates,
  directSourceReviewed: sourceReviewed,
  independentlyVerified: current.counts.evidenceVerifiedLessons
}));
