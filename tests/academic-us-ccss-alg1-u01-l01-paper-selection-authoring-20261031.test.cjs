// ALG1 U01:L01 lesson-local paper selection; authored only, NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('docs/curriculum/US_CCSS_ALG1_CODER5_U01_L01_AUTHORING_C_20261030.json'),r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg1-u01-l01-2021.rich-lesson-v2.json');
test('exact student paper route with all P1-P3 and protected machine readiness',()=>{
 const x=a.printableTeacherSelectedEvidence20261031;assert.equal(x.lessonCode,a.LessonCode);
 const l=r.Lessons.find(y=>y.LessonCode===a.LessonCode);assert(l.ExplanationParagraphs.some(s=>s.includes(x.studentUrl)));
 const sheet=fs.readFileSync(path.join(root,x.studentAsset),'utf8');
 for(const id of ['warmup','survey','P3','cool'])assert(sheet.includes(id));
 assert.equal((sheet.match(/type="checkbox"/g)||[]).length,6);
 assert.equal(a.selectedPracticeRouteVerified,false);
});
test('P3 exact set independent normalization, reject duplicated/extra/missing',()=>{
 const norm=v=>{if(!Array.isArray(v)||!v.length||v.some(x=>typeof x!=='string'))return null;const s=v.map(x=>x.trim().toUpperCase());return s.some(x=>!/^[A-F]$/.test(x))||new Set(s).size!==s.length?null:s.sort().join('')};
 assert.equal(norm(['c',' A ','E']),'ACE');
 assert.notEqual(norm(['A','C']),'ACE');assert.equal(norm(['A','a','C','E']),null);assert.equal(norm(['A','C','E','Z']),null);
});
