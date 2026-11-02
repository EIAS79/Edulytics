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

const blueprintFiles = fs.readdirSync(blueprintDir)
  .filter((name) => /^us-ccss-math-.*\.lesson-blueprint\.json$/i.test(name))
  .sort();
const contentFiles = fs.readdirSync(contentDir)
  .filter((name) => /^us-ccss-math-.*\.lesson-content-pack\.json$/i.test(name))
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

const errors = [];
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