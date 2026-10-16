'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {validateSources,classifyText,summarize}=require('../tools/curriculum/us-ccss-b001-audit-original-student-pdfs.cjs');
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
