'use strict';
// A-B-C authoring test specs, execute in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:10:03:CONDITIONAL-STATEMENTS';
const family='uae.g9.general.u10.l03.conditional_truth_tables';
test('Source Arabic conditional lesson covers original converse inverse contrapositive and biconditional',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u10-l03-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(pack&&rich);assert.match(pack.SourceLocator,/02-1\.pdf printed 577/);
 for(const title of ['الشرطية الأصلية','العكس والمعكوس','المقابل المعاكس','الثنائية الشرط'])
  assert.ok(rich.KeyConcepts.some(x=>x.Title===title),title);
 assert.match(rich.WorkedExamples.find(x=>x.Title==='تكوين العبارات الأربع').Answer,/المقابل المعاكس صحيحتان/);
});
test('Shared conditional family is wired and has independent bounded truth verifier',()=>{
 const entry=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(entry);assert.deepEqual(entry.allowedQuestionFamilies,[family]);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9ConditionalTruthTables(random)'));
 assert.ok(engine.includes('private static Problem G9ConditionalTruthTables(Random r)'));
 assert.match(engine,/mode is <0 or >4/);
 assert.match(engine,/given==expected/);
 for(let p=0;p<=1;p++)for(let q=0;q<=1;q++){
  const original=p===1&&q===0?0:1;
  const contrapositive=q===0&&p===1?0:1;
  assert.equal(original,contrapositive);
 }
});
test('Conditional Cool-down offers six open teacher reviewed source tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u10-l03-conditionals-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/المقابل المعاكس/);assert.match(html,/العدد ٦/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل إجابات ولم تُحتسب درجة/);
});
test('Conditional lesson 62 is A-B-C authored but Stage D tests remain unrun',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-62-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,62);assert.equal(cp.closed,true);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(cp.stageD,'NOT_RUN');
});
