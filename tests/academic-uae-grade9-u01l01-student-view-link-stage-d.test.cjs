'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');

test('Student lesson view preserves UAE MOE Grade 9 General localization alongside explicitly scoped Grade 10 and existing UAE grades',()=>{
 const view=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml'),'utf8');
 assert.match(view,/var isUaeGrade9Arabic\s*=\s*Model\.LessonCode\.StartsWith\("PED:UAE-MOE-MATH:L9:GENERAL:", StringComparison\.Ordinal\)/);
 assert.match(view,/var isUaeGrade10Arabic\s*=\s*Model\.LessonCode\.StartsWith\("PED:UAE-MOE-MATH:L10:GENERAL:", StringComparison\.Ordinal\)/);
 assert.match(view,/var isUaePrimaryArabic\s*=\s*Model\.LessonCode\.StartsWith\("PED:UAE-MOE-MATH:L1:COMMON:", StringComparison\.Ordinal\)\s*\|\|\s*Model\.LessonCode\.StartsWith\("PED:UAE-MOE-MATH:L2:COMMON:", StringComparison\.Ordinal\)\s*\|\|\s*isUaeGrade3Arabic\s*\|\|\s*isUaeGrade4Arabic\s*\|\|\s*isUaeGrade5Arabic\s*\|\|\s*isUaeGrade6Arabic\s*\|\|\s*isUaeGrade7Arabic\s*\|\|\s*isUaeGrade8Arabic\s*\|\|\s*isUaeGrade9Arabic\s*\|\|\s*isUaeGrade10Arabic;/);
 assert.match(view,/var lessonNavTitle = isUaePrimaryArabic/);
 assert.match(view,/ViewData\["UseYouTubeStudioV2"\] = !isUaePrimaryArabic/);
 assert.doesNotMatch(view,/PED:US-CCSS-MATH:L9:GENERAL:/);
});

test('Only UAE Grade 9 General lesson 1-1 contains a scoped student navigation link to original teacher-only tasks',()=>{
 const view=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml'),'utf8');
 const code='PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS';
 assert.ok(view.includes('@if (Model.LessonCode == "'+code+'")'));
 assert.ok(view.includes('href="/lesson-labs/uae-g9-general/u01-l01-source-cooldown-ar.html"'));
 const folder='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/';
 const cooldown=fs.readFileSync(path.join(root,folder,'u01-l01-source-cooldown-ar.html'),'utf8');
 const extra=fs.readFileSync(path.join(root,folder,'u01-l01-original-61-practice-ar.html'),'utf8');
 assert.match(cooldown,/href="u01-l01-original-61-practice-ar\.html"/);
 assert.match(extra,/lang="ar" dir="rtl"/);
 assert.match(view,/lang="ar" dir="rtl"/);
 assert.match(view,/teacher reviewed and separate from automatically graded Practice/);
 assert.match(extra,/graded:false/);
 assert.match(extra,/submitted:false/);
});
