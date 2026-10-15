#!/usr/bin/env node
'use strict';
// Edulytics Unified V9 B01: parse COMPLETE isolated numeric equalities, not
// truncated adjacent operands; never "correct" true/false pedagogical prompts.
// This is all-500 learner-arithmetic evidence ONLY, not original task fidelity
// or a math certificate. No publisher prose or artwork is reproduced.
const fs=require('node:fs'),path=require('node:path'),assert=require('node:assert/strict');
const root=path.resolve(__dirname,'..');
const base='artifacts/academic-us-ccss/batches/';
const packfile=g=>'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json';
const get=f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const fields=['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
const sha=/^[a-f0-9]{64}$/;
function evalNumeric(input) {
 if(typeof input!=='string')return null;
 let expr=input.replace(/\\(?:left|right)\b/g,'').replace(/\\(?:times|cdot)\b|×/g,'*')
  .replace(/\\div\b|÷/g,'/').replace(/−/g,'-').replace(/\\,/g,'')
  .replace(/(\d),(?=\d{3}(?:\D|$))/g,'$1').replace(/\s+/g,'');
 if(!expr||!/^[-+*/().\d]+$/.test(expr))return null;
 const tok=expr.match(/(?:\d+(?:\.\d+)?|\.\d+|[()+*/-])/g)||[];
 if(tok.join('')!==expr)return null;
 let i=0;
 const factor=()=>{
  if(tok[i]==='+'||tok[i]==='-'){const sign=tok[i++]==='-'?-1:1;return sign*factor();}
  if(tok[i]==='('){i++;const v=expression();if(tok[i++]!==')')throw Error('paren');return v;}
  if(i>=tok.length||!/^(?:\d+(?:\.\d+)?|\.\d+)$/.test(tok[i]))throw Error('numeric');
  return Number(tok[i++]);
 };
 const term=()=>{
  let v=factor();
  while(tok[i]==='*'||tok[i]==='/'){
   const op=tok[i++],right=factor();
   if(op==='/'&&right===0)throw Error('divide zero');
   v=op==='*'?v*right:v/right;
  }
  return v;
 };
 const expression=()=>{
  let v=term();
  while(tok[i]==='+'||tok[i]==='-'){
   const op=tok[i++],right=term();v=op==='+'?v+right:v-right;
  }
  return v;
 };
 try{
  const result=expression();
  return i===tok.length&&Number.isFinite(result)&&Math.abs(result)<1e13?result:null;
 }catch{return null;}
}
function classifyEquation(equation,context=''){
 if(typeof equation!=='string'||equation.split('=').length!==2)
  return {status:'UNSUPPORTED_MATH_NOT_EVALUATED'};
 const [left,right]=equation.split('=');
 const lhs=evalNumeric(left),rhs=evalNumeric(right);
 if(lhs===null||rhs===null)
  return {status:'UNSUPPORTED_MATH_NOT_EVALUATED'};
 if(Math.abs(lhs-rhs)<=1e-9)
  return {status:'NUMERIC_EQUALITY_VERIFIED_ONLY',lhs,rhs};
 if(/\b(?:true or false|true\/false|decide whether each statement|decide if each statement)\b/i.test(context))
  return {status:'POSSIBLY_INTENTIONAL_FALSE_EQUATION_IN_EXERCISE',lhs,rhs};
 return {status:'NUMERIC_INCONSISTENCY_NEEDS_SOURCE_TASK_CONTEXT',lhs,rhs};
}
function buildAudit(manifest,packs,sourceIndex){
 assert.equal(manifest?.batchId,'B01');
 assert.equal(manifest?.lessons?.length,500);
 assert.equal(sourceIndex?.rows?.length,500);
 const byCode=new Map(packs.flatMap(p=>p.lessons).map(x=>[x.lessonCode,x]));
 const teacher=new Map(sourceIndex.rows.map(x=>[x.lessonCode,x]));
 const seen=new Set(),rows=[],stats={equationsEvaluated:0,valid:0,exercisePotentialFalse:0,unresolved:0,unsupported:0};
 for(const m of manifest.lessons){
  assert(!seen.has(m.lessonCode),'B01 manifest duplicates');seen.add(m.lessonCode);
  const lesson=byCode.get(m.lessonCode),ref=teacher.get(m.lessonCode);
  assert(lesson&&ref,'Source, mathematics or lesson identity missing '+m.lessonCode);
  assert(sha.test(lesson.canonicalBodySha256)&&sha.test(m.sourceSemanticSha256));
  const en=lesson.translations.find(x=>x.cultureCode==='en');
  assert(en,'No original English student lesson '+m.lessonCode);
  const equations=[];
  for(const field of fields){
   const str=en[field]||'';
   const tex=String(str).matchAll(/\\\(([\s\S]*?)\\\)/g);
   for(const item of tex){
    const mathematical=item[1].trim();
    if(!mathematical.includes('='))continue;
    let prev=str.slice(Math.max(0,item.index-700),item.index);
    const index=prev.lastIndexOf('Example ');
    if(index!==-1)prev=prev.slice(index);
    const verdict=classifyEquation(mathematical,prev);
    if(verdict.status==='UNSUPPORTED_MATH_NOT_EVALUATED'){
     stats.unsupported++;
     continue;
    }
    stats.equationsEvaluated++;
    if(verdict.status==='NUMERIC_EQUALITY_VERIFIED_ONLY')stats.valid++;
    if(verdict.status==='POSSIBLY_INTENTIONAL_FALSE_EQUATION_IN_EXERCISE')stats.exercisePotentialFalse++;
    if(verdict.status==='NUMERIC_INCONSISTENCY_NEEDS_SOURCE_TASK_CONTEXT')stats.unresolved++;
    equations.push({field,operatorSyntax:'ISOLATED_COMPLETE_LATEX_NUMERIC_EQUALITY',
      verdict:verdict.status,computedLeft:verdict.lhs,computedRight:verdict.rhs,
      sourceSnippetTextReproduced:false});
   }
  }
  rows.push({lessonCode:m.lessonCode,ordinal:m.ordinal,
   sourceSemanticSha256:m.sourceSemanticSha256,
   originalStudentPackSha256:lesson.canonicalBodySha256,
   equations,academicCertifiedByThisTool:false});
 }
 assert.equal(rows.length,500);assert.equal(seen.size,500);
 assert.equal(stats.valid+stats.exercisePotentialFalse+stats.unresolved,stats.equationsEvaluated);
 return {
  schemaVersion:1,planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
  kind:'B01_500_COMPLETE_NUMERIC_EQUATIONS_SOURCE_CONTEXT_TRIAGE_NO_CERTIFICATE',
  scope:{expected:500,covered:500,unique:500,equationStatistics:stats,
   completeOriginalTaskMathematicsVerified:0,
   fullLessonAcademicCertificationsGranted:0},
  caveats:'Checks only isolated numeric LaTex equalities on 500 canonical RAW learner lessons; results do NOT establish teacher-source problem meaning, equation context, full source comparison, effective Rich V2 corrections, math diagrams, rights, L1–L6, accessibility or academic certification. Incorrect equations inside true/false exercises MUST be retained and interpreted, not blindly repaired. Unsupported equations are not treated as correct.',
  copyright:'Only numeric computed left/right values, field names, source SHA and counts are stored; no teacher prose, artwork or original LaTex sentences reproduced.',
  rows,academicDecision:'BATCH_NO-GO',releaseEligible:false
 };
}
if(require.main===module){
 const data=buildAudit(get(base+'B01-lesson-manifest.json'),[1,2,3,4].map(g=>get(packfile(g))),get(base+'B01-book-source-reference-index-500.json'));
 const arg=process.argv.find(x=>x.startsWith('--out='));
 if(arg)fs.writeFileSync(arg.slice('--out='.length),JSON.stringify(data,null,2)+'\n');
 console.log(JSON.stringify({coverage:data.scope.covered,equations:data.scope.equationStatistics,
  academicDecision:data.academicDecision}));
}
module.exports={evalNumeric,classifyEquation,buildAudit};