"use strict";
// Course-owned static Rich V2 sidecar verification. Nothing here is a claim
// of academic acceptance, a successful Practice grading route, or release.
const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const root = path.resolve(__dirname,'..','..');
const pack = path.join(root,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra1-im1e.lesson-blueprint.json');
const richDir = path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const blueprint = JSON.parse(fs.readFileSync(pack,'utf8'));
const official = blueprint.Lessons || blueprint.lessons;
assert.ok(Array.isArray(official),'Blueprint lessons missing');
assert.equal(official.length,132);
const byCode = new Map();
const ids = new Set();
for (const entry of official) {
 const code = entry.LessonCode || entry.lessonCode;
 assert.ok(code && !ids.has(code),'Duplicate or absent canonical ID '+code);
 ids.add(code);byCode.set(code,entry);
}
const files = fs.readdirSync(richDir).filter(name => /^us-ccss-hs-alg1-u\d\d-l\d\d-2021\.rich-lesson-v2\.json$/.test(name)).sort();
const perUnit={};
const seen = new Set();
const problems=[];
const report=[];
for (const filename of files){
 try{
  const doc=JSON.parse(fs.readFileSync(path.join(richDir,filename),'utf8'));
  assert.equal(doc.SchemaVersion,1);
  assert.equal(doc.PackCode,'US-CCSS-MATH');
  assert.ok(Array.isArray(doc.Lessons)&&doc.Lessons.length===1);
  const x=doc.Lessons[0],ref=byCode.get(x.LessonCode);
  assert.ok(ref,'Not in blueprint: '+x.LessonCode);
  assert.equal(x.CultureCode,'en');
  assert.equal(x.Title,ref.Title || ref.title);
  assert.ok(!seen.has(x.LessonCode),'Duplicate: '+x.LessonCode);
  seen.add(x.LessonCode);
  const m=filename.match(/u(\d\d)-l(\d\d)/);
  const expectedId=':U'+m[1]+':L'+m[2];
  assert.ok(x.LessonCode.endsWith(expectedId), 'File name mismatches '+x.LessonCode);
  const url=ref.SourceUrl || ref.sourceUrl;
  assert.ok(url && x.ExplanationParagraphs.some(v=>v.includes(url)), 'Source link mismatch '+x.LessonCode);
  assert.ok(x.ExplanationParagraphs.length>=3,'Missing multiple explanation paragraphs');
  assert.ok(x.KeyConcepts.length>=3 && x.WorkedExamples.length>=3);
  for (const worked of x.WorkedExamples) assert.ok(worked.Steps.length>=3 && worked.Answer && worked.Check,'Missing worked explanation');
  assert.ok(x.CommonMistakes.length>=2 && x.SummaryPoints.length>=3);
  assert.ok(x.Visuals.length>=1 && x.Visuals.every(v=>v.Description),'No accessible visual description');
  assert.ok(x.Videos.length===0, 'Claimed videos not verified');
  perUnit[m[1]]=(perUnit[m[1]]||0)+1;
  report.push({lessonCode:x.LessonCode,title:x.Title,source:url,worked:x.WorkedExamples.length});
 }catch(e){problems.push({file:filename,error:e.message})}
}
const result={blueprint:official.length,candidateFiles:files.length,uniqueValid:seen.size,perUnit,problems, unAuthored:official.map(x=>x.LessonCode||x.lessonCode).filter(c=>!seen.has(c)).length, nextMissing:official.map(x=>x.LessonCode||x.lessonCode).find(c=>!seen.has(c))};
console.log(JSON.stringify(result,null,2));
if (problems.length)process.exitCode=1;
