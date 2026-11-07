#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const ROOT = path.resolve(__dirname, '../..');

const blueprintDir = path.join(
  ROOT,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs'
);
const contentDir = path.join(
  ROOT,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs'
);
const registryPath = path.join(
  ROOT,
  'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.curriculum-pack.json'
);
const skillMapPath = path.join(
  ROOT,
  'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'
);

// Official-outcome closure is deliberately scoped to the 17 existing
// CCSS grade/course packs. Owner-adopted Grade12 Advanced Precalculus is
// an additional source-sequenced 73-lesson course; it is NOT one of the
// original 1560 officially aligned lessons. Audit its published owner pack
// independently rather than classifying 72 advanced sections as defects.
const g12BlueprintFile = 'us-ccss-math-hs-advanced-precalculus-openstax-2017.lesson-blueprint.json';
const g12ContentFile = 'us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json';
const g12BlueprintDoc = JSON.parse(fs.readFileSync(path.join(blueprintDir,g12BlueprintFile),'utf8'));
const g12ContentDoc = JSON.parse(fs.readFileSync(path.join(contentDir,g12ContentFile),'utf8'));
const g12BlueprintCodes = (g12BlueprintDoc.Lessons || []).map(x => x.LessonCode);
const g12ContentCodes = (g12ContentDoc.Lessons || []).map(x => x.LessonCode);
const g12AdditionalErrors = [];
if (g12BlueprintCodes.length !== 73 || g12ContentCodes.length !== 73 ||
    new Set(g12BlueprintCodes).size !== 73 ||
    g12BlueprintCodes.some((code,i) => code !== g12ContentCodes[i]) ||
    g12ContentDoc.Status !== 'Published' ||
    !/owner/i.test(g12ContentDoc.ReviewedBy || ''))
  g12AdditionalErrors.push('supplemental G12 published lesson blueprint/content owner alignment not 73/73');

const blueprintFiles = fs.readdirSync(blueprintDir)
  .filter((name) => /^us-ccss-math-.*\.lesson-blueprint\.json$/i.test(name) && name !== g12BlueprintFile)
  .sort();
const contentFiles = fs.readdirSync(contentDir)
  .filter((name) => /^us-ccss-math-.*\.lesson-content-pack\.json$/i.test(name) && name !== g12ContentFile)
  .sort();

const registry = JSON.parse(fs.readFileSync(registryPath, 'utf8'));
const officialCodes = new Set(
  (registry.Nodes ?? [])
    .filter((node) =>
      node.IsOfficial &&
      node.IsActive &&
      (node.Kind === 'Standard' || node.Kind === 'Outcome'))
    .map((node) => node.Code)
);

const blueprintLessons = new Map();
const invalidBlueprintCodes = [];
const duplicateBlueprintCodes = [];
let blueprintOfficial = 0;
let blueprintSupporting = 0;

for (const file of blueprintFiles) {
  const doc = JSON.parse(fs.readFileSync(path.join(blueprintDir, file), 'utf8'));
  for (const lesson of doc.Lessons ?? []) {
    if (blueprintLessons.has(lesson.LessonCode)) {
      duplicateBlueprintCodes.push(lesson.LessonCode);
    }

    const outcomeCodes =
      doc.SchemaVersion === 2
        ? [...new Set((lesson.FormalTargets ?? []).map((x) => x.OutcomeCode).filter(Boolean))]
        : [...new Set((lesson.OutcomeCodes ?? []).filter(Boolean))];

    blueprintLessons.set(lesson.LessonCode, {
      file,
      schemaVersion: doc.SchemaVersion,
      outcomeCodes
    });

    if (outcomeCodes.length === 0) blueprintSupporting++;
    else blueprintOfficial++;

    for (const code of outcomeCodes) {
      if (!officialCodes.has(code)) {
        invalidBlueprintCodes.push({
          lessonCode: lesson.LessonCode,
          outcomeCode: code
        });
      }
    }
  }
}

const contentLessons = new Map();
const invalidContentCodes = [];
const duplicateContentCodes = [];
let contentOfficial = 0;
let contentSupporting = 0;

for (const file of contentFiles) {
  const doc = JSON.parse(fs.readFileSync(path.join(contentDir, file), 'utf8'));
  for (const lesson of doc.lessons ?? []) {
    if (contentLessons.has(lesson.lessonCode)) {
      duplicateContentCodes.push(lesson.lessonCode);
    }

    const outcomeCodes = [...new Set((lesson.outcomeCodes ?? []).filter(Boolean))];
    contentLessons.set(lesson.lessonCode, {
      file,
      outcomeCodes,
      isSupporting: Boolean(lesson.isSupporting)
    });

    if (lesson.isSupporting || outcomeCodes.length === 0) contentSupporting++;
    else contentOfficial++;

    for (const code of outcomeCodes) {
      if (!officialCodes.has(code)) {
        invalidContentCodes.push({
          lessonCode: lesson.lessonCode,
          outcomeCode: code
        });
      }
    }
  }
}

const missingContentLessons = [...blueprintLessons.keys()]
  .filter((code) => !contentLessons.has(code))
  .sort();
const staleContentLessons = [...contentLessons.keys()]
  .filter((code) => !blueprintLessons.has(code))
  .sort();

const skillMapDocument = JSON.parse(fs.readFileSync(skillMapPath, 'utf8'));
const allSkillMappings =
  skillMapDocument.mappings ??
  skillMapDocument.Mappings ??
  [];
const commonCoreSkillMappings = allSkillMappings
  .filter((row) => String(row.lessonCode ?? '').startsWith('PED:US-CCSS-MATH:'));
const supportingSkillMappings = commonCoreSkillMappings
  .filter((row) => row.sourceType === 'SupportingLesson');
const officialFlagMismatches = commonCoreSkillMappings
  .filter((row) =>
    row.sourceType === 'OfficialMappedPedagogicalLesson' &&
    row.officialOutcomeMapped !== true);

const errors = [...g12AdditionalErrors];
if (blueprintFiles.length !== 17) errors.push(`expected 17 blueprints, found ${blueprintFiles.length}`);
if (contentFiles.length !== 17) errors.push(`expected 17 content packs, found ${contentFiles.length}`);
if (blueprintLessons.size !== 1560) errors.push(`expected 1560 blueprint lessons, found ${blueprintLessons.size}`);
if (contentLessons.size !== 1560) errors.push(`expected 1560 content lessons, found ${contentLessons.size}`);
if (blueprintSupporting !== 0) errors.push(`blueprint Supporting lessons: ${blueprintSupporting}`);
if (contentSupporting !== 0) errors.push(`content Supporting lessons: ${contentSupporting}`);
if (invalidBlueprintCodes.length) errors.push(`invalid blueprint outcome codes: ${invalidBlueprintCodes.length}`);
if (invalidContentCodes.length) errors.push(`invalid content outcome codes: ${invalidContentCodes.length}`);
if (duplicateBlueprintCodes.length) errors.push(`duplicate blueprint lesson codes: ${duplicateBlueprintCodes.length}`);
if (duplicateContentCodes.length) errors.push(`duplicate content lesson codes: ${duplicateContentCodes.length}`);
if (missingContentLessons.length) errors.push(`blueprint lessons missing from content: ${missingContentLessons.length}`);
if (staleContentLessons.length) errors.push(`stale content lessons: ${staleContentLessons.length}`);
if (supportingSkillMappings.length) errors.push(`SupportingLesson skill mappings: ${supportingSkillMappings.length}`);
if (officialFlagMismatches.length) errors.push(`official skill mappings missing officialOutcomeMapped=true: ${officialFlagMismatches.length}`);

const report = {
  scope: '1560_CC_SSM_OFFICIAL_17_BASELINE_PACKS_PLUS_INDEPENDENT_G12_OWNER_AUTHORIZED_ADVANCED_COURSE',
  supplementalG12BlueprintLessons: g12BlueprintCodes.length,
  supplementalG12PublishedContentLessons: g12ContentCodes.length,
  blueprintFiles: blueprintFiles.length,
  contentFiles: contentFiles.length,
  blueprintLessons: blueprintLessons.size,
  blueprintOfficial,
  blueprintSupporting,
  contentLessons: contentLessons.size,
  contentOfficial,
  contentSupporting,
  invalidBlueprintCodes: invalidBlueprintCodes.length,
  invalidContentCodes: invalidContentCodes.length,
  duplicateBlueprintLessonCodes: duplicateBlueprintCodes.length,
  duplicateContentLessonCodes: duplicateContentCodes.length,
  missingContentLessons: missingContentLessons.length,
  staleContentLessons: staleContentLessons.length,
  commonCoreSkillMappings: commonCoreSkillMappings.length,
  supportingSkillMappings: supportingSkillMappings.length,
  officialSkillFlagMismatches: officialFlagMismatches.length,
  fullOfficialCoverage:
    blueprintOfficial === 1560 &&
    contentOfficial === 1560 &&
    blueprintSupporting === 0 &&
    contentSupporting === 0,
  errors
};

console.log(JSON.stringify(report, null, 2));

if (errors.length) {
  process.exit(1);
}