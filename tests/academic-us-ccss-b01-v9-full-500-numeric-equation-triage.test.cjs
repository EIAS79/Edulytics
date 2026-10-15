'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const {evalNumeric,classifyEquation,buildAudit}=require('../tools/us-ccss-b01-v9-full-500-numeric-equation-triage.cjs');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/academic-us-ccss/batches/';
const input=[read(base+'B01-lesson-manifest.json'),
 [1,2,3,4].map(g=>read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json')),
 read(base+'B01-book-source-reference-index-500.json')];
test('Evaluate the entire numeric expression on each side, not just first operands',()=>{
 for(const [x,val] of [
  ['18+5',23],['18+2+3',23],['6+7',13],['10+6',16],
  ['3*(4+2)',18],['4*2+3',11],['(4+2)*3',18],
  ['35-20+1',16],['35-19',16],['75,940-12,786',63154],
  ['8+2+3',13],['2+2+2',6],['3+3',6]
 ]) assert.equal(evalNumeric(x),val,x);
 assert.equal(evalNumeric('1/0'),null);
 assert.equal(evalNumeric('Math.random()'),null);
 assert.equal(evalNumeric('3+x'),null);
 assert.equal(evalNumeric('1+'),null);
});
test('False pedagogical true/false examples must never be automatically rewritten',()=>{
 const good=classifyEquation('18 + 5 = 18 + 2 + 3');
 assert.equal(good.status,'NUMERIC_EQUALITY_VERIFIED_ONLY');
 const intentional=classifyEquation('9 + 5 = 10 + 6','Decide if each statement is true or false.');
 assert.equal(intentional.status,'POSSIBLY_INTENTIONAL_FALSE_EQUATION_IN_EXERCISE');
 const unresolved=classifyEquation('9+5=10+6','Worked example shows the answer');
 assert.equal(unresolved.status,'NUMERIC_INCONSISTENCY_NEEDS_SOURCE_TASK_CONTEXT');
 assert.equal(classifyEquation('x+5=7').status,'UNSUPPORTED_MATH_NOT_EVALUATED');
 assert.equal(classifyEquation('1+2=3=4').status,'UNSUPPORTED_MATH_NOT_EVALUATED');
});
test('All 500 raw canonical B01 lessons enter one fail-closed arithmetic review without any academic PASS',()=>{
 const audit=buildAudit(...input);
 assert.equal(audit.scope.covered,500);
 assert.equal(audit.scope.unique,500);
 assert.equal(audit.rows.length,500);
 assert.equal(audit.academicDecision,'BATCH_NO-GO');
 assert.equal(audit.releaseEligible,false);
 assert.equal(audit.scope.fullLessonAcademicCertificationsGranted,0);
 assert.equal(audit.scope.completeOriginalTaskMathematicsVerified,0);
 assert(audit.scope.equationStatistics.equationsEvaluated>20);
 const rows=new Map(audit.rows.map(x=>[x.lessonCode,x]));
 assert.equal(rows.size,500);
 for(const row of input[0].lessons){
  const x=rows.get(row.lessonCode);
  assert(x);assert.equal(x.ordinal,row.ordinal);
  assert.equal(x.sourceSemanticSha256,row.sourceSemanticSha256);
  assert.equal(x.academicCertifiedByThisTool,false);
 }
 const altered=structuredClone(input);
 altered[0].lessons.pop();
 assert.throws(()=>buildAudit(...altered));
 const duplicated=structuredClone(input);
 duplicated[0].lessons[1].lessonCode=duplicated[0].lessons[0].lessonCode;
 assert.throws(()=>buildAudit(...duplicated));
});