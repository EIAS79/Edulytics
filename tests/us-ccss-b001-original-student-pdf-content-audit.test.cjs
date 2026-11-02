'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {validateSources,classifyText,summarize,informativeTokens,compareStudentTasks}=require('../tools/curriculum/us-ccss-b001-audit-original-student-pdfs.cjs');
const load=p=>JSON.parse(fs.readFileSync(path.join(__dirname,'..',p),'utf8'));
test('Exactly 500 original 2021 student PDF source links match the B001 canonical IDs',()=>{
 const source=load('docs/curriculum/batches/US-CCSS-B001-IM2021-STUDENT-TASKS-PDF-SOURCES-500.json');
 const catalogue=load('docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');
 assert.equal(validateSources(source,catalogue).length,500);
 const fakeApproval=structuredClone(source);fakeApproval.lessons[0].academicAccepted=true;
 assert.throws(()=>validateSources(fakeApproval,catalogue));
 const unsafe=structuredClone(source);unsafe.lessons[0].originalStudentTaskStatementPdfUrl='https://example.com/no.pdf';
 assert.throws(()=>validateSources(unsafe,catalogue));
});
test('PDF extraction does not imply source fidelity or academic approval',()=>{
 assert.deepEqual(classifyText('',2).flags,['NO_EXTRACTABLE_TEXT_VISUAL_REVIEW']);
 assert.ok(classifyText('Count 1 2',1).flags.includes('LOW_TEXT_DENSITY_VISUAL_REVIEW'));
 const summary=summarize([{status:'STRUCTURALLY_INSPECTED',flags:[]},{status:'STRUCTURALLY_INSPECTED',flags:['NO_EXTRACTABLE_TEXT_VISUAL_REVIEW']},{status:'INSPECTION_FAILED',flags:['TECHNICAL_REVIEW']}]);
 assert.equal(summary.structurallyInspected,2);
 assert.equal(summary.failed,1);
 assert.equal(summary.visualReview,1);
 assert.equal(summary.academicallyApproved,0);
 assert.equal(summary.authenticatedPracticeApproved,0);
});


test('Lexical task correspondence is evidence-only and cannot create academic acceptance',()=>{
 const excerpt='Mai has 5 graphic novels. She checks out 4 more graphic novels. How many graphic novels does Mai have in all? Show a drawing and write an equation.';
 const source=[{hasStudentFacing:true,studentFacingExcerpt:excerpt}];
 const richPdf='Student Task Statements. Mai has 5 graphic novels. She checks out 4 more graphic novels. How many graphic novels does Mai have in all? Show a drawing and write an equation. Explain the story using an expression.';
 const close=compareStudentTasks(richPdf,source);
 assert.equal(close.status,'LEXICAL_SUPPORT_ONLY');
 assert.equal(close.matchingPrompts,1);
 assert.equal(close.bestRecall,1);
 assert.equal('academicallyApproved' in close,false);
 const offTopic='Random triangles rectangles perimeters inches measurements conversions geometric shapes rulers protractors tangent coordinates dimensions radius circle polygon length angle scale grid diagonal prism fractions denominators.';
 const unrelated=compareStudentTasks(offTopic,source);
 assert.equal(unrelated.status,'LOW_LEXICAL_SUPPORT_REVIEW');
 assert.equal(unrelated.matchingPrompts,0);
 assert.ok(unrelated.bestRecall<0.25);
 assert.equal(compareStudentTasks('arbitrary PDF text',source).status,'PDF_TEXT_TOO_SPARSE');
 assert.equal(compareStudentTasks(richPdf,[]).status,'NO_SOURCE_STUDENT_PROMPT_EXCERPT');
 assert.equal(compareStudentTasks(richPdf,[{hasStudentFacing:true,studentFacingExcerpt:'Do it'}]).status,'SOURCE_EXCERPT_TOO_GENERIC');
 assert.ok(informativeTokens('Students will show the number').length<=1);
});

test('Source-mapped lexical classifications cannot accidentally modify approval counts',()=>{
 const report=summarize([
  {status:'STRUCTURALLY_INSPECTED',flags:[],studentTaskSourceComparison:{status:'LEXICAL_SUPPORT_ONLY'}},
  {status:'STRUCTURALLY_INSPECTED',flags:[],studentTaskSourceComparison:{status:'LOW_LEXICAL_SUPPORT_REVIEW'}},
  {status:'STRUCTURALLY_INSPECTED',flags:[],studentTaskSourceComparison:{status:'NO_SOURCE_STUDENT_PROMPT_EXCERPT'}}
 ]);
 assert.equal(report.sourceTaskTextSupported,1);
 assert.equal(report.sourceTaskTextLowSupport,1);
 assert.equal(report.sourceTaskNoPromptExcerpt,1);
 assert.equal(report.academicallyApproved,0);
 assert.equal(report.authenticatedPracticeApproved,0);
});
