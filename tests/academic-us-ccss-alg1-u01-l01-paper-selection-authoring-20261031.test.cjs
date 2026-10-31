// ALG1 U01:L01 lesson-local offline learner assessment safeguards; AUTHORED_NOT_RUN.
// This is an independent oracle specification, NOT an integrated service grader.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),at=p=>path.join(root,p),read=p=>JSON.parse(fs.readFileSync(at(p),'utf8'));
const a=read('docs/curriculum/US_CCSS_ALG1_CODER5_U01_L01_AUTHORING_C_20261030.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg1-u01-l01-2021.rich-lesson-v2.json');

test('U01:L01 offline paper links the actual lesson without publishing the P3 key',()=>{
 const x=a.printableTeacherSelectedEvidence20261031;assert.equal(x.lessonCode,a.LessonCode);
 const l=r.Lessons.find(y=>y.LessonCode===a.LessonCode);assert(l);
 assert.equal(r.ContentVersion,a.contentVersion);
 assert(l.ExplanationParagraphs.some(s=>s.includes(x.studentUrl)));
 const sheet=fs.readFileSync(at(x.studentAsset),'utf8');
 for(const id of ['warmup','survey','P3','cool'])assert(sheet.includes(id));
 const labels=[...sheet.matchAll(/<input type="checkbox" name="p3">\s*([A-F])\./g)].map(m=>m[1]);
 assert.deepEqual(labels,['A','B','C','D','E','F']);
 const keyedP3=a.activities.find(x=>x.id==='P3');
 assert.deepEqual(keyedP3.expected,['A','C','E']);
 const learnerVisible=l.ExplanationParagraphs.join('\n')+'\n'+sheet;
 assert(!learnerVisible.includes('{A, C, E}'),'never expose exact multi-select key in learner view');
 assert(!learnerVisible.includes('Answer set ='),'never display the teacher key');
 assert.equal(a.selectedPracticeRouteVerified,false);
 assert.equal(x.machineSelectedPractice,false);
});

test('P3 draft oracle accepts choice arrays and strict CSV while rejecting malformed values',()=>{
 const expected=a.activities.find(x=>x.id==='P3').expected.join('');
 const normalize=value=>{
  if(typeof value==='string'){
   // The historical source-aligned authoring oracle accepts exactly three comma-separated IDs.
   if(!/^\s*[A-Fa-f]\s*,\s*[A-Fa-f]\s*,\s*[A-Fa-f]\s*$/.test(value))return null;
   value=value.split(',');
  }
  if(!Array.isArray(value)||value.length!==3||value.some(x=>typeof x!=='string'))return null;
  const items=value.map(x=>x.trim().toUpperCase());
  if(items.some(x=>!/^[A-F]$/.test(x))||new Set(items).size!==items.length)return null;
  return items.sort().join('');
 };
 for(const sample of a.oracleFixtures.correct)
  assert.equal(normalize(sample),expected,'should accept '+JSON.stringify(sample));
 for(const sample of a.oracleFixtures.wrong)
  assert.notEqual(normalize(sample),expected,'should reject wrong '+JSON.stringify(sample));
 for(const sample of a.oracleFixtures.malformed)
  assert.equal(normalize(sample),null,'should reject malformed '+JSON.stringify(sample));
 assert.equal(normalize(['c',' A ','e']),expected);
 assert.equal(normalize(['A','a','C','E']),null);
 assert.equal(normalize('c, E, a'),expected,'historically accepted strict CSV transcription');
});
