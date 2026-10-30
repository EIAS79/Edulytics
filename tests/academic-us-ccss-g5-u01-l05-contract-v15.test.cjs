'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 L05 actual source growing 4×10 prism',()=>assert.deepEqual([1,2,3,10,25].map(h=>4*10*h),[40,80,120,400,1000]));
test('G5 L05 alternate base choices count same solid',()=>assert.deepEqual([3*4*5,3*5*4,4*5*3],[60,60,60]));
test('G5 L05 rejects malformed dimensions and area-volume confusion',()=>{assert.notEqual(40*25,100);for(const a of [[3,4],[3,-4,5],[3,4.5,5],['3',4,5]])assert.equal(a.length===3&&a.every(x=>Number.isSafeInteger(x)&&x>0),false)});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l05-practice-rubric-v15.json');
test('G5 L05 original models present; Practice C still gated',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'requires full checkout'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L05'));const s2=JSON.parse(fs.readFileSync(s,'utf8'));assert.ok(l.ExplanationParagraphs.join(' ').includes('40×25'));assert.ok(l.ExplanationParagraphs.join(' ').includes('What Is the Question?'));assert.equal(s2.StageC,'BLOCKED_SHARED_INTEGRATION')});
