'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.join(root,p),'utf8');
test('G9 authentic Arabic lesson forces RTL from academic scope even when portal UI cookie remains English',()=>{
 const view=read('src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml');
 const layout=read('src/Edulytics.Web/Views/Shared/_StudentLayout.cshtml');
 const exact='Model.LessonCode.StartsWith("PED:UAE-MOE-MATH:L9:GENERAL:", StringComparison.Ordinal)';
 assert.ok(view.includes(exact));
 assert.ok(view.includes('ViewData["ForceUaeG9AcademicArabicDirection"] = isUaeGrade9Arabic;'));
 assert.ok(layout.includes('ViewData["ForceUaeG9AcademicArabicDirection"] is true'));
 assert.ok(layout.includes('CultureInfo.CurrentUICulture.TwoLetterISOLanguageName'));
 assert.ok(layout.includes('dir="@(pageLanguage == "ar" ? "rtl" : "ltr")"'));
 assert.ok(layout.includes('<html lang="@pageLanguage"'));
 const grade7Path=view.includes('isUaeGrade7Arabic || isUaeGrade8Arabic');
 assert.ok(grade7Path,'existing Grade7/8 academic content handling must remain');
 assert.ok(!view.includes('CultureInfo.CurrentUICulture ='),'student view must not mutate global localization');
});
