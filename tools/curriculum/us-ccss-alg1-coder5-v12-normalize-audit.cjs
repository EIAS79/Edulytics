#!/usr/bin/env node
'use strict';
// Normalize observed lesson activity headings from publisher fetch results.
// Heuristic overlap is triage only, never pedagogical or academic acceptance.
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..','..');
const fp=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V12_SOURCE_FETCH_AUDIT_20261029.json');
const progress=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json')));
const report=JSON.parse(fs.readFileSync(fp));
const byCode=new Map(progress.lessons.map(x=>[x.lessonCode,x]));
const stop=new Set('lesson summary glossary entries which doesnt belong what notice wonder activity step with each same data number numbers table tables using about from that these those this there when then some into values find your pair part and for one how can are more does list their time talk get back'.split(' '));
let sourceHeadingCount=0;let missingTerms=0;
for(const r of report.rows){
 const meta=byCode.get(r.lessonCode);
 if(!meta)throw Error('Unexpected lesson '+r.lessonCode);
 r.publisherLessonTitleDetected=null;
 r.publisherLessonTitleDetection='NOT_RELIABLY_EXTRACTABLE_FROM_HTML_HEADERS_USE_VERIFIED_CANONICAL_BLUEPRINT';
 const headings=(r.studentActivityHeadingCandidates||[]).filter(h=>/^\d+\.\d+:/u.test(h));
 sourceHeadingCount+=headings.length;
 r.studentActivityHeadingCandidates=headings;
 const lesson=JSON.parse(fs.readFileSync(path.join(root,meta.authoredRichV2Sidecar))).Lessons[0];
 const authored=[...lesson.ExplanationParagraphs,...lesson.KeyConcepts.map(x=>x.Title+' '+x.Definition),
 ...lesson.WorkedExamples.map(x=>x.Title+' '+x.Question)].join(' ').toLowerCase();
 const words=[...new Set(headings.flatMap(h=>h.toLowerCase().normalize('NFKD')
   .replace(/[\u0300-\u036f]/g,'').replace(/^\d+\.\d+:\s*/, '')
   .match(/[a-z]{4,}/g)||[]).filter(x=>!stop.has(x)))].slice(0,14);
 const missing=words.filter(w=>!authored.includes(w));
 missingTerms+=missing.length;
 r.studentActivityHeadingTermMatch={
   sourceTerms:words,representedInAuthoredNarrative:words.filter(w=>authored.includes(w)),
   missingFromAuthoredNarrative:missing,
   evidenceStatus:'HEURISTIC_LEXICAL_TRIAGE_ONLY_NOT_SOURCE_ACTIVITY_FIDELITY'
 };
 r.auditClass='TEACHER_AND_STUDENT_PAGES_FETCHED_200__SOURCE_ACTIVITY_REVIEW_NOT_COMPLETE';
 r.teacherGoalsIndependentlyVerified=false;
 r.studentActivitiesIndependentlyVerified=false;
 r.stageAApproved=false;
}
report.qualityCorrection={
 date:'2026-10-29',
 note:'Raw HTML headings include navigational sections; filter to n.n activity headings and lowercase before calculating lexical terms. Original unreliable title detection removed.',
 totalRealStudentActivityHeadings:sourceHeadingCount,
 missingLexicalTermsAcross129:missingTerms,
 lexicalMatchCanNotCertifySourceAlignment:true
};
fs.writeFileSync(fp,JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({fetchedPairs:report.lessonCount,activityHeadings:sourceHeadingCount,triageMissingTerms:missingTerms,academicAccepted:0}));
