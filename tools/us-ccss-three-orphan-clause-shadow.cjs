#!/usr/bin/env node
'use strict';
// PHASE 05/06/07: three precise shadow-only competency probes for historical
// parent-only / publisher-unresolved subordinate standards. Never assert exact
// publisher child evidence, student routing or human academic certification.
const fs = require('node:fs'), path = require('node:path');
const root = path.resolve(__dirname,'..');
const read = p => JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const check = (v,m) => { if(!v) throw Error(m); };
const specs = [
  {
    "clause": "3.MD.C.7.a",
    "parent": "3.MD.C.7",
    "lesson": "PED:US-CCSS-MATH:G3:U02:L05",
    "title": "Represent Products as Areas PLC Activity",
    "sourceUrl": "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-5/preparation.html",
    "blueprint": "us-ccss-math-grade3-im-k5-2021.lesson-blueprint.json",
    "family": "rectangle_tiling_product",
    "officialUrl": "https://www.thecorestandards.org/Math/Content/3/MD/C/7/a/"
  },
  {
    "clause": "5.MD.C.3.a",
    "parent": "5.MD.C.3",
    "lesson": "PED:US-CCSS-MATH:G5:U01:L01",
    "title": "What Is Volume?",
    "sourceUrl": "https://im.kendallhunt.com/k5/teachers/grade-5/unit-1/lesson-1/preparation.html",
    "blueprint": "us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json",
    "family": "unit_cube_recognition",
    "officialUrl": "https://www.thecorestandards.org/Math/Content/5/MD/C/3/a/"
  },
  {
    "clause": "6.SP.B.5.a",
    "parent": "6.SP.B.5",
    "lesson": "PED:US-CCSS-MATH:G6:U08:L17",
    "title": "Using Box Plots",
    "sourceUrl": "https://im.kendallhunt.com/MS/teachers/1/8/17/preparation.html",
    "blueprint": "us-ccss-math-grade6-our-2017.lesson-blueprint.json",
    "family": "number_of_observations",
    "officialUrl": "https://www.thecorestandards.org/Math/Content/6/SP/B/5/a/"
  }
];
function casesFor(kind) {
 const result=[];
 if(kind==='rectangle_tiling_product') {
  for(let rows=1;rows<=10;rows++)for(let cols=1;cols<=10;cols++)
   result.push({id:`tile-${rows}x${cols}`,rows,columns:cols,
    tileRows:Array(rows).fill('#'.repeat(cols)),
    answerArea:rows*cols,answerUnit:'square units'});
 } else if(kind==='unit_cube_recognition') {
  const dimensionsList=[[1,1,1],[1,1,2],[1,2,1],[2,1,1],[2,2,2],[3,3,3]];
  for(const unit of ['cm','m','in','ft'])
   for(const dimensions of dimensionsList) {
    const isUnitCube=dimensions.every(edge=>edge===1);
    result.push({id:`unit-${unit}-${dimensions.join('x')}`,
     edgeDimensions:dimensions,lengthUnit:unit,isUnitCube,
     answerUnit:isUnitCube?`one cubic ${unit}`:'not a unit cube'});
   }
 } else if(kind==='number_of_observations') {
  for(let size=1;size<=12;size++)for(let pattern=0;pattern<5;pattern++) {
   const samples=Array.from({length:size},(_,i)=>pattern===0?0:
    pattern===1?i%3:pattern===2?(i%2===0?-2:3):
    pattern===3?(i-Math.floor(size/2)):Math.floor(i/2));
   result.push({id:`count-${size}-pattern-${pattern}`,
    measurements:samples,answerCount:size,answerUnit:'observations'});
  }
 } else throw Error('Unsupported shadow mathematical family: '+kind);
 return result;
}
function independentlyVerify(example, family) {
 if(family==='rectangle_tiling_product') {
  check(Number.isInteger(example.rows)&&Number.isInteger(example.columns)&&
   example.rows>=1&&example.rows<=10&&example.columns>=1&&example.columns<=10,
   'Area inputs outside grade-3 domain');
  // Oracle 1: count concrete array cells, not multiplication formula.
  check(example.tileRows.length===example.rows,'Tile row count mismatch');
  let counted=0;
  for(const row of example.tileRows) {
   check(/^#+$/.test(row)&&row.length===example.columns,
    'Tiles must be rectangular with no gaps or overlaps');
   for(const tile of row)if(tile==='#')counted++;
  }
  // Oracle 2: area via the independently stated rectangular product.
  check(counted===example.rows*example.columns &&
   example.answerArea===counted && example.answerUnit==='square units',
   'Rectangular tiling and multiplication do not agree');
 } else if(family==='unit_cube_recognition') {
  const edges=example.edgeDimensions;
  check(edges.length===3&&edges.every(x=>Number.isInteger(x)&&x>=1),
   'Unit cube dimensions must be three positive whole edges');
  // Distinct definitions: three unit edges vs enclosed integer grid-cell count.
  const allUnitEdges=edges.every(x=>x===1);
  const enclosedCount=edges.reduce((v,x)=>v*x,1);
  check(allUnitEdges===(enclosedCount===1) &&
   example.isUnitCube===allUnitEdges &&
   example.answerUnit===(allUnitEdges?
      `one cubic ${example.lengthUnit}`:'not a unit cube'),
   'One cubic unit classification is wrong');
 } else if(family==='number_of_observations') {
  check(Array.isArray(example.measurements)&&
   example.measurements.every(Number.isFinite),
   'Observations must be numeric');
  // Independently count individual items and sum multiset frequencies;
  // duplicates and zero-valued observations remain real observations.
  let direct=0;
  const frequencies=new Map();
  for(const v of example.measurements) {
   direct++;
   frequencies.set(v,(frequencies.get(v)||0)+1);
  }
  let byFrequency=0;for(const count of frequencies.values())byFrequency+=count;
  check(direct===byFrequency && example.answerCount===direct &&
   example.answerUnit==='observations','Reported n omits duplicates or zero');
 } else throw Error('Unknown family');
 return true;
}
function buildEvidence() {
 const registry=read('artifacts/academic-us-ccss/01-authority-registry.json');
 const report=read('artifacts/academic-us-ccss/05-clause-publisher-reference-reconciliation.json');
 check(report.summary.clauses===121&&report.summary.publisherExactReferenced===106&&
  report.summary.publisherUnresolved===15,
  'Pinned publisher 121/106/15 report changed');
 const records=specs.map(s=>{
  const child=registry.subordinateClauses.find(x=>x.clauseCode===s.clause);
  const publisher=report.clauses.find(x=>x.clause===s.clause);
  check(child&&child.parentCode===s.parent&&child.exactPublisherSourceRefs===0&&
   publisher&&publisher.parent===s.parent&&publisher.publisherExactReferences===0,
   'Refusing to misclassify a publisher exact or changed child source: '+s.clause);
  const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/'+s.blueprint);
  const lesson=blueprint.Lessons.find(x=>x.LessonCode===s.lesson);
  check(lesson&&lesson.Title===s.title&&lesson.SourceUrl===s.sourceUrl&&
   lesson.OutcomeCodes.includes('CCSS:'+s.parent)&&
   !lesson.Alignments.some(x=>x.Role==='Addressing'&&
     String(x.ReferenceCode||'').toLowerCase()===s.clause.toLowerCase()),
   'Parent/lesson evidence drift; do not invent publisher child alignment '+s.clause);
  const cases=casesFor(s.family);
  for(const entry of cases)independentlyVerify(entry,s.family);
  return {clause:s.clause,parent:s.parent,officialUrl:s.officialUrl,
   mathematicalFamily:s.family,rawPublisherChildAddressingCount:0,
   candidateLesson:{lessonCode:s.lesson,expectedTitle:s.title,
    sourceUrl:s.sourceUrl,blueprintFile:s.blueprint,
    publisherRelation:s.clause==='3.MD.C.7.a'?
     'PARENT_OUTCOME_AND_SIBLING_B_NOT_EXACT_A':'PARENT_OUTCOME_ONLY_NOT_EXACT_CHILD'},
   releaseStatus:'SHADOW_ONLY_NOT_STUDENT_ROUTED',humanAcademicReviewerId:null,
   verifiedCases:cases.length,questionCases:cases};
 });
 return {schemaVersion:1,scope:'US-CCSS-MATH/CCSSM-2010/G1-G12',
  basis:'PINNED_PUBLISHER_BLUEPRINT_PARENT_RELATION_AND_OFFICIAL_SUBCLAUSE_TEXT',
  sourcePublisherExactCoverageBefore:{total:121,exact:106,unresolved:15},
  sourcePublisherExactCoverageAfter:{total:121,exact:106,unresolved:15},
  reviewState:'MATHEMATICAL_NUMERIC_SHADOW_CASES_ONLY',skillFamilyProductionReady:false,
  studentPracticeRouting:false,independentHumanAcademicReviews:0,
  mathematicalCaseCount:records.reduce((sum,x)=>sum+x.verifiedCases,0),
  newShadowFamilies:records.length,
  limitations:[
   'Only primitive numerical/oracle probes are independently recomputed; written reasoning, diagrams and human marking rubrics remain unreviewed',
   'Related publisher lessons do NOT assert exact child-standard Addressing source',
   'No generated questions, practice routing, assessment, student data, grade mappings or official wording changed'
  ],clauses:records};
}
const output='artifacts/academic-us-ccss/05-three-orphan-child-math-shadow-candidates.json';
if(require.main===module) {
 const data=JSON.stringify(buildEvidence(),null,2)+'\n';
 if(process.argv.includes('--check')) {
  check(fs.readFileSync(path.join(root,output),'utf8')===data,
   'Three orphan clause shadow candidate evidence is missing or stale');
 } else if(process.argv.includes('--write')) {
  fs.writeFileSync(path.join(root,output),data);
 } else throw Error('Explicit --check or --write required');
 console.log('Shadow mathematical cases validated: '+JSON.parse(data).mathematicalCaseCount);
}
module.exports={casesFor,independentlyVerify,buildEvidence};
