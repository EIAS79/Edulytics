'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
const read=p=>JSON.parse(rd(p));
test('G4 L23 original source six fraction comparisons use exact rational mathematics',()=>{
const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l15-2021.rich-lesson-v2.json').Lessons[0];
assert.equal(v.Title,'Common Denominators to Compare');
assert.equal(v.WorkedExamples.length,3);
const comparisons=[[[4,3],[13,12],1],[[4,3],[7,5],-1],[[5,6],[3,4],1],[[2,3],[5,8],1],[[2,6],[4,10],-1],[[7,4],[17,10],1]];
for(const [[a,b],[c,d],wanted] of comparisons){const sign=Math.sign(a*d-c*b);assert.equal(sign,wanted);}
assert.match(v.WorkedExamples[2].Steps.join(' '),/10\/12 > 9\/12/);
assert.match(v.WorkedExamples[2].Steps.join(' '),/35\/20 > 34\/20/);
});
test('G4 L23 A-C source contract is exact, stage D and academic approval remain pending',()=>{
const contract=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2FifteenthLessonPracticeContracts.cs');
const registrations=rd('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
assert.match(contract,/fractions\.compare\.unlike\.common_denominator/);
assert.match(contract,/fractions\.compare\.unlike\.select_greater/);
assert.match(registrations,/foreach\(var reviewed in UsCcssG4Unit2FifteenthLessonPracticeContracts\.Reviewed\)/);
const cp=read('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-23-v2-stage-a-d-checkpoint.json');
assert.equal(cp.stages.D,'NOT_RUN');
assert.equal(cp.sourceVisualParityProven,false);
assert.equal(cp.academicApproved,false);
});
