
'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {execFileSync}=require('node:child_process');
const root=path.resolve(__dirname,'..');
const proof='e702b36328ff90cbb01e7ac81a4a90c06e165989';
const expected=new Map([
 ['src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs','4ab3d76929f98384a8cbde990e9298cfa0d7d3f6'],
 ['src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs','1621291d58735d314953c908ac2d02dc04282e8d'],
 ['src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.cs','7fed805bc6cc4cb195cebbc33f03b56b4d68ba7b'],
]);

const git=(...a)=>execFileSync('git',a,{cwd:root,encoding:'utf8'}).trim();
const g1Lines=s=>s.split(/\r?\n/).filter(line=>
  line.includes('usccss.g1.') || line.includes('PED:US-CCSS-MATH:G1:')).map(line=>line.trim());
test('current Grade7 integration preserves exact old Grade1 dispatch text and pins three shared blobs',()=>{
 assert.equal(git('merge-base','--is-ancestor',proof,'HEAD'),'');
 for(const [file,pin] of expected){
   const original=git('show',proof+':'+file);
   const current=fs.readFileSync(path.join(root,file),'utf8');
   assert.ok(g1Lines(original).length>0,file);
   assert.deepEqual(g1Lines(current),g1Lines(original),
     'A Grade 1 family/route changed in '+file);
   assert.equal(git('hash-object',file),pin,'Review exact integration changes before new release');
 }
});
