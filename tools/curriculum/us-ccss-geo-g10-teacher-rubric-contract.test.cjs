const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '../..');
const read = name => fs.readFileSync(path.join(root, name), 'utf8');
const prefix = 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:';
const svc = read('src/Edulytics.Web/Services/Grade10GeometryRubricService.cs');
const ctr = read('src/Edulytics.Web/Controllers/Grade10GeometryRubricController.cs');
const schema = read('src/Edulytics.Data/Migrations/20261106023000_Grade10GeometryTeacherRubric.cs');
const learner = read('src/Edulytics.Web/Views/Grade10GeometryRubricStudent/Index.cshtml');
const review = read('src/Edulytics.Web/Views/Grade10GeometryRubricTeacher/Review.cshtml');
const lesson = read('src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml');
const blueprint = JSON.parse(read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json'));

test('G10 rubric exposes only 124 canonical Geometry lesson identities', () => {
  assert.equal(blueprint.Lessons.length,124);
  assert.equal(new Set(blueprint.Lessons.map(x=>x.LessonCode)).size,124);
  for(const row of blueprint.Lessons)assert.ok(row.LessonCode.startsWith(prefix));
  assert.match(svc,/PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:/);
  assert.match(svc,/US-CCSS-MATH:L10:TRADITIONAL:COURSE-GEO/);
  assert.match(svc,/lesson\.Code\.StartsWith\(G10GeoRubricStorage\.Prefix\)/);
});

test('Student submission always derives assigned adoption, class and school from identity',()=>{
  assert.match(ctr,/\[Authorize\(Policy = "StudentPortal"\)\]/);
  assert.match(ctr,/\[HttpPost\("submit"\), ValidateAntiForgeryToken\]/);
  assert.match(svc,/p\.UserId == actor && !p\.IsArchived && adoption\.IsActive/);
  assert.match(svc,/e\.StudentProfileId == row\.StudentProfileId/);
  assert.match(svc,/adoption\.CurriculumLevelKey == G10GeoRubricStorage\.AdoptionLevelKey/);
  assert.match(svc,/db\.StudentEnrollments\.AsNoTracking/);
  assert.match(svc,/ON CONFLICT \("SchoolId","ClassGroupId","StudentProfileId",/);
  assert.match(ctr,/StudentScopeAsync\(actor, lessonId, Guid\.Empty, ct\)/);
  assert.ok(!ctr.includes('FromForm] Guid curriculumAdoptionId'));
});

test('Teacher grading checks assignment, tenant, submission and disallows regrade',()=>{
  assert.match(ctr,/\[Authorize\(Roles = RoleNames\.Teacher\)\]/);
  assert.match(ctr,/MayReviewAsync\(actor, row, ct\)/);
  assert.match(svc,/t\.TeacherUserId == actor && t\.SchoolId == row\.SchoolId/);
  assert.match(svc,/t\.ClassGroupId == row\.ClassGroupId/);
  assert.match(svc,/t\.SubjectId == adoption\.SubjectId/);
  assert.match(svc,/WHERE "Id"=@id AND "SchoolId"=@school/);
  assert.match(svc,/"TeacherUserId" IS NULL/);
  assert.match(svc,/marks\.Length != 4/);
  assert.match(ctr,/teacherFeedback\.Trim\(\)\.Length is < 10 or > 2000/);
});

test('Persisted rubric is unique, explicitly human-scored and bounded 0..4',()=>{
  assert.match(schema,/Grade10GeometryTeacherRubrics/);
  assert.match(schema,/unique: true/);
  assert.match(schema,/"TeacherScore" BETWEEN 0 AND 4/);
  assert.match(schema,/FK_G10GeometryRubric_Student/);
  assert.match(learner,/Teacher review pending/);
  assert.match(review,/Save teacher assessment/);
  assert.match(review,/name="marks"/);
  assert.match(lesson,/asp-controller="Grade10GeometryRubricStudent"/);
  assert.match(lesson,/PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:/);
});
