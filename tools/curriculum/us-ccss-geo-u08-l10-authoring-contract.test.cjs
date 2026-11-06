const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const rich=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u08-l07-l11-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u08-l10-source-human-paper-20261105.json"),'utf8'));
const lesson=rich.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U08:L10");
test('source-aligned authoring contract',()=>{assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U08:L10");assert.ok(a.SourceTaskCrosswalk.length>=3);assert.equal(a.TestsExecuted,'NOT_RUN');});
test('human-review boundary',()=>{assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);assert.equal(a.SelectedPracticeRoute.SavedGrade,'NOT_VERIFIED_STAGE_D');});
test('independent cooldown',()=>{assert.ok(a.FirstPartyCooldown.Prompt.length>40);assert.ok(lesson.ExplanationParagraphs.some(x=>x.startsWith('Edulytics independent cool-down:')));});
