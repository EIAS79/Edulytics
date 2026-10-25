"use strict";
const fs = require("node:fs");
const path = require("node:path");
const assert = require("node:assert/strict");
const batch = process.argv[2];
assert.ok(["D1", "D2", "D3"].includes(batch), "Specify D1, D2 or D3.");
const root = "src/Edulytics.Core/Curriculum/LessonContent/RichV2";
const grade = JSON.parse(fs.readFileSync("artifacts/curriculum-delivery/US-CCSS-MATH/G3/grade-manifest.json", "utf8"));
const ordinal = new Map();
let n = 0;
for (const unit of grade.units) {
  for (let j = 1; j <= unit.lessonCount; j++) {
    n++;
    ordinal.set(String(unit.unitNumber).padStart(2, "0") + "-" + String(j).padStart(2, "0"), n);
  }
}
assert.equal(n, 143);
const bad = [];
let documents = 0, examples = 0;
for (const file of fs.readdirSync(root)) {
  const match = /^us-ccss-g3-u(\d\d)-l(\d\d)-2021\.rich-lesson-v2\.json$/.exec(file);
  if (!match) continue;
  const ordinalNumber = ordinal.get(match[1] + "-" + match[2]);
  if (!ordinalNumber) { bad.push("Unknown Grade 3 source document: " + file); continue; }
  const group = ordinalNumber <= 48 ? "D1" : ordinalNumber <= 96 ? "D2" : "D3";
  if (group !== batch) continue;
  documents++;
  const source = JSON.parse(fs.readFileSync(path.join(root, file), "utf8"));
  for (const lesson of source.Lessons || []) {
    if ((lesson.WorkedExamples || []).length < 3)
      bad.push(file + " — " + (lesson.LessonCode || "unknown lesson") + ": fewer than three first-party worked examples");
    if ((lesson.KeyConcepts || []).length < 3)
      bad.push(file + " — " + (lesson.LessonCode || "unknown lesson") + ": fewer than three source-specific key concepts");
    for (const work of lesson.WorkedExamples || []) {
      examples++;
      if (!Array.isArray(work.Steps) || work.Steps.length < 3 ||
          work.Steps.some(s => typeof s !== "string" || !s.trim()))
        bad.push(file + " — " + (work.Title || "(untitled)") + ": fewer than three nonempty explanatory steps");
    }
  }
}
console.log(batch + " Grade 3 Rich V2 worked-example check: " + documents + " files; " + examples + " examples; " + bad.length + " blockers.");
if (bad.length) { for (const e of bad) console.error(e); process.exitCode = 1; }
