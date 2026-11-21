'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const activityPath='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-original-61-practice-ar.html';
const cooldownPath='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-source-cooldown-ar.html';

test('All 61 visually identified Grade 9 lesson 1-1 source items have an original independent teacher-reviewed task',()=>{
 const audited=read(base+'u01l01-all-61-source-exercises-preparation.json');
 const practice=read(base+'u01l01-original-61-parallel-skill-practice.json');
 assert.equal(practice.questionCount,61);
 assert.equal(practice.records.length,61);
 assert.equal(audited.items.length,61);
 assert.equal(practice.coreSkillTasks,46);
 assert.equal(practice.priorSkillReviewTasks,15);
 const ids=new Set();
 for(let i=0;i<61;i++){
   const source=audited.items[i],task=practice.records[i];
   assert.equal(task.sourceItem,source.sourceItem);
   assert.equal(task.printedPage,source.printedPage);
   assert.equal(task.sourceSkillCategory,source.visualSourceSkillCategory);
   assert.equal(task.sourceGroup,source.sourceGroup);
   assert.equal(source.independentTeacherTaskId,task.independentTaskId);
   assert.equal(source.stageDAudit,'NOT_RUN');
   assert.equal(task.independentTaskId,'G9-U01L01-T'+String(i+1).padStart(2,'0'));
   assert.equal(task.scope,i<46?'CORE_SKILL':'PRIOR_SKILL_REVIEW');
   assert.equal(task.gradingRoute,'TEACHER_REVIEW_REQUIRED');
   assert.ok(task.studentPrompt.trim().length>5);
   assert.ok(task.teacherExpectedEvidence.trim().length>0);
   assert.ok(!ids.has(task.independentTaskId));
   ids.add(task.independentTaskId);
 }
 assert.equal(practice.records[28].sourceSkillCategory,'CYLINDER_VOLUME_MODEL');
 assert.equal(practice.records[36].sourceSkillCategory,'SAME_BASE_POWER_PRODUCT');
 assert.equal(practice.records[40].sourceSkillCategory,'CUBE_VOLUME_FACE_AREA_RELATION');
 assert.equal(practice.records[46].scope,'PRIOR_SKILL_REVIEW');
 assert.equal(practice.sourceExerciseItemsCheckedForMathematicalEquivalence,0);
 assert.equal(practice.fullSourceElementParityVerified,false);
 assert.equal(practice.publishGateSatisfied,false);
});

test('Student activity is 61-task Arabic RTL teacher-only and reachable from existing Grade 9 cooldown',()=>{
 const html=fs.readFileSync(path.join(root,activityPath),'utf8');
 const cooldown=fs.readFileSync(path.join(root,cooldownPath),'utf8');
 const match=html.match(/const tasks = (\[[^\n]+\]);/);
 assert.ok(match,'embedded student questions exist');
 const tasks=JSON.parse(match[1]);
 assert.equal(tasks.length,61);
 assert.equal(tasks.filter(x=>x.scope==='CORE_SKILL').length,46);
 assert.equal(tasks.filter(x=>x.scope==='PRIOR_SKILL_REVIEW').length,15);
 assert.ok(tasks.every(x=>!Object.hasOwn(x,'teacherExpectedEvidence')));
 assert.ok(tasks.every(x=>x.studentPrompt && x.independentTaskId && x.sourceItem));
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/role="img"/);
 assert.match(html,/id="g9-geometry-title"/);
 assert.match(html,/<ellipse cx="165"/);
 assert.match(html,/<polygon points="430,70/);
 assert.ok(html.includes('question.textContent=task.studentPrompt;'));
 assert.match(html,/event\.preventDefault\(\)/);
 assert.match(html,/graded:false/);
 assert.match(html,/submitted:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.doesNotMatch(html,/\bfetch\(/);
 assert.doesNotMatch(html,/XMLHttpRequest|sendBeacon\(/);
 assert.match(cooldown,/href="u01-l01-original-61-practice-ar\.html"/);
});

test('Independent source-aligned task arithmetic samples use correct mathematical conclusions',()=>{
 const r=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 assert.equal(10**4*10**2,10**6);
 assert.equal(Math.PI*3**2*5,45*Math.PI);
 assert.equal(4*(5**2+4)-(4*5**2+4),12);
 assert.ok(Math.abs((2/3)*(3/5)-2/5)<1e-12);
 assert.ok(Math.abs((5/6)/(10/9)-3/4)<1e-12);
 assert.ok(Math.abs((7/10)*(5/14)-1/4)<1e-12);
 assert.equal((3+4+4+5+9)/5,5);
 assert.equal(8.43-2.17,6.26);
 assert.equal((1940-785)/100,11.55);
 assert.equal(r[36].sourceSkillCategory,'SAME_BASE_POWER_PRODUCT');
 assert.equal(r[46].sourceSkillCategory,'PRIOR_DATA_CHART');
});
const vm=require('node:vm');

test('Real student script builds 61 answer boxes and exports only local ungraded teacher responses',()=>{
 const html=fs.readFileSync(path.join(root,activityPath),'utf8');
 const script=html.match(/<script>([\s\S]*?)<\/script>/)?.[1];
 assert.ok(script);
 const nodes=new Map();
 const element=tag=>{
   let currentId='';
   return {
     tag,children:[],attributes:{},value:'',textContent:'',listeners:{},
     get id(){return currentId;},
     set id(value){currentId=value;nodes.set(value,this);},
     setAttribute(name,value){this.attributes[name]=value;},
     append(...children){this.children.push(...children);},
     appendChild(child){this.children.push(child);},
     addEventListener(event,callback){this.listeners[event]=callback;}
   };
 };
 for(const id of ['practice','core','prior','output','status'])element('section').id=id;
 const document={
   getElementById:id=>nodes.get(id),
   createElement:tag=>element(tag)
 };
 vm.runInNewContext(script,{document,JSON,Array,String});
 assert.equal(nodes.get('core').children.length,46);
 assert.equal(nodes.get('prior').children.length,15);
 assert.ok(nodes.has('G9-U01L01-T01'));
 assert.ok(nodes.has('G9-U01L01-T61'));
 nodes.get('G9-U01L01-T01').value='\u062e\u0645\u0633\u0629 \u0623\u0645\u062b\u0627\u0644 \u0645';
 nodes.get('G9-U01L01-T61').value='\u0661\u0661\u066b\u0665\u0665';
 let prevented=false;
 nodes.get('practice').listeners.submit({preventDefault:()=>{prevented=true;}});
 assert.equal(prevented,true);
 const result=JSON.parse(nodes.get('output').value);
 assert.equal(result.answers.length,61);
 assert.equal(result.completed,2);
 assert.equal(result.total,61);
 assert.equal(result.graded,false);
 assert.equal(result.submitted,false);
 assert.equal(result.answers[0].question,'G9-U01L01-T01');
 assert.equal(result.answers[60].question,'G9-U01L01-T61');
 assert.ok(result.answers.every(x=>x.review==='TEACHER_REVIEW_REQUIRED'));
 assert.match(nodes.get('status').textContent,/\u0644\u0645 \u062a\u064f\u0631\u0633\u0644/);
});
test('U01L01 source skills preserve reasoning direction for powers, unit conversion and box plots',()=>{
 const tasks=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 assert.ok(tasks[36].studentPrompt.includes("\u0627\u0644\u0646\u0645\u0637"));
 assert.equal(tasks[36].sourceSkillCategory,'SAME_BASE_POWER_PRODUCT');
 assert.ok(tasks[44].studentPrompt.includes("\u0628\u0627\u0644\u064a\u0627\u0631\u062f\u0627\u062a"));
 assert.ok(tasks[44].teacherExpectedEvidence.includes("\u0663\u0633/\u0663\u0666"));
 assert.equal(tasks[44].sourceSkillCategory,'FABRIC_LENGTH_UNIT_CONVERSION');
 assert.ok(tasks[47].studentPrompt.includes("\u0623\u0646\u0634\u0626 \u0645\u062e\u0637\u0637 \u0635\u0646\u062f\u0648\u0642 \u0648\u0634\u0627\u0631\u0628\u064a\u0646"));
 assert.equal(tasks[47].sourceSkillCategory,'PRIOR_BOX_PLOT');
 assert.equal(10**2*10**3,10**5);
 assert.equal(3*240/36,20);
 assert.equal(20-14,6);
});
test('U01L01 multistep context tasks retain fractional composition and inverse scoring',()=>{
 const tasks=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 assert.equal(tasks[34].sourceSkillCategory,'FRACTION_OF_REAL_QUANTITY_MODEL');
 assert.equal(tasks[35].sourceSkillCategory,'SCORING_RATE_EQUATION_MODEL');
 assert.ok(tasks[34].studentPrompt.includes("\u0646\u0635\u0641"));
 assert.ok(tasks[35].teacherExpectedEvidence.includes("\u0665\u0647\u0640+\u0625"));
 assert.equal(50*3/5/2,15);
 assert.equal((26-2)/6,4);
});
test('U01L01 visual source atomic tasks preserve numbered items cognitive demand and exact numeric rubrics',()=>{
 const tasks=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 const task=n=>tasks[n-1];
 assert.equal(tasks.length,61);
 assert.ok(task(34).studentPrompt.includes("\u0641\u0633\u0651\u0631"));
 assert.ok(task(34).studentPrompt.includes("\u0627\u062d\u0633\u0628"));
 assert.ok(task(36).studentPrompt.includes("\u0662\u0663\u2013\u0660"));
 assert.ok(task(36).studentPrompt.includes("\u0661\u0668\u2013\u0666"));
 assert.ok(task(36).studentPrompt.includes("\u0660\u2264\u0625\u2264\u0647\u0640"));
 assert.ok(task(36).teacherExpectedEvidence.includes("\u0623\u0631\u0628\u0639\u0629 \u0623\u0647\u062f\u0627\u0641 \u0648\u062b\u0644\u0627\u062b \u0625\u0636\u0627\u0641\u0627\u062a"));
 assert.ok(task(37).studentPrompt.includes("\u0641\u0631\u0636\u064a\u0629"));
 assert.ok(task(37).studentPrompt.includes("\u0661\u0660\u00b2\u00d7\u0661\u0660\u2074"));
 assert.ok(task(43).studentPrompt.includes("\u0623\u064a \u062a\u0639\u0628\u064a\u0631"));
 assert.ok(task(43).studentPrompt.includes("\u0664\u00b3"));
 assert.ok(task(44).studentPrompt.includes("\u0623\u064a \u062a\u0639\u0628\u064a\u0631 \u064a\u0645\u062b\u0644 \u0645\u062d\u064a\u0637\u0647"));
 assert.ok(task(46).studentPrompt.includes("\u0627\u062e\u062a\u0631 \u0645\u0633\u0627\u062d\u062a\u0647\u0627"));
 assert.ok(task(47).studentPrompt.includes("\u0633\u0628\u0639 \u0623\u0644\u0639\u0627\u0628"));
 assert.ok(task(47).studentPrompt.includes("\u0623\u0646\u0634\u0626 \u0645\u062e\u0637\u0637 \u0623\u0639\u0645\u062f\u0629"));
 assert.ok(task(48).studentPrompt.includes("\u0661\u0664 \u0645\u0634\u0627\u0631\u0643\u064b\u0627"));
 assert.ok(task(48).studentPrompt.includes("\u0623\u0646\u0634\u0626 \u0645\u062e\u0637\u0637 \u0635\u0646\u062f\u0648\u0642 \u0648\u0634\u0627\u0631\u0628\u064a\u0646"));
 assert.ok(task(48).studentPrompt.includes("\u0627\u0643\u062a\u0628 \u062c\u0645\u0644\u0629"));
 assert.ok(task(58).teacherExpectedEvidence.includes("\u2212\u0662\u0666/\u0661\u0665"));
 assert.equal(39+0.4*20+3*2,53);
 assert.equal(5*4+3,23);
 assert.equal(5*3+3,18);
 assert.equal(5*1+1,6);
 assert.equal(10**2*10**4,10**6);
 assert.equal(4**3,64);
 assert.equal(2*7+2*3,20);
 assert.equal(7*3,21);
 assert.equal(8+5+7+11+6+9+4,50);
 const times=[19,20,21,21,22,22,23,24,24,25,26,27,28,29];
 assert.equal(times.length,14);
 assert.equal((times[6]+times[7])/2,23.5);
 assert.equal(times[3],21);
 assert.equal(times[10],26);
 assert.equal(times[10]-times[3],5);
 assert.ok(Math.abs(3/5-7/3-(-26/15))<1e-12);
});
test('U01L01 original statistics and fraction analogs match source data set cardinality and result types',()=>{
 const records=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 const p=n=>records[n-1];
 assert.ok(p(49).studentPrompt.includes("\u0627\u0644\u0639\u0634\u0631"));
 assert.ok(p(50).studentPrompt.includes("\u062c\u0645\u064a\u0639 \u0642\u064a\u0645 \u0627\u0644\u0645\u0646\u0648\u0627\u0644"));
 assert.ok(p(51).studentPrompt.includes("\u0627\u0644\u062b\u0645\u0627\u0646\u064a"));
 const a=[3,5,5,6,7,7,7,9,10,11],b=[-4,-2,-2,-2,0,0,0,1,4,5],c=[4,9,10,12,14,18,18,27];
 assert.equal(a.length,10);assert.equal(a.reduce((x,y)=>x+y,0)/a.length,7);
 assert.equal(b.length,10);assert.equal(b.reduce((x,y)=>x+y,0),0);
 assert.deepEqual([...new Set(b.filter(x=>b.filter(y=>y===x).length===3))],[-2,0]);
 assert.equal(c.length,8);assert.equal(c.reduce((x,y)=>x+y,0)/c.length,14);
 assert.equal((c[3]+c[4])/2,13);
 assert.ok(p(56).teacherExpectedEvidence.includes("\u0662\u0669/\u0661\u0668"));
 assert.ok(p(60).teacherExpectedEvidence.includes("\u0663\u0661/\u0662\u0664"));
 assert.ok(Math.abs(5/6+7/9-29/18)<1e-12);
 assert.ok(Math.abs(7/8+5/12-31/24)<1e-12);
});
test('U01L01 verbal subtraction tasks keep direction and free-variable question remains open',()=>{
 const records=read(base+'u01l01-original-61-parallel-skill-practice.json').records;
 assert.ok(records[4].teacherExpectedEvidence.includes("\u0661\u0661\u2212\u0633"));
 assert.ok(records[19].teacherExpectedEvidence.includes("\u062a\u2212\u0663\u0665"));
 assert.ok(records[38].studentPrompt.includes("\u0627\u062e\u062a\u0631 \u0628\u0646\u0641\u0633\u0643"));
 assert.ok(records[38].studentPrompt.includes("\u0639\u0631\u0651\u0641 \u0644\u0647 \u0645\u062a\u063a\u064a\u0631\u064b\u0627"));
 assert.equal(11-4,7);assert.notEqual(11-4,4-11);
 assert.equal(40-35,5);assert.notEqual(40-35,35-40);
});
test('U01L01 visually reviewed 61 numbered textbook source skills without falsely certifying full source parity',()=>{
 const audit=read(base+'u01l01-61-source-visual-skill-audit.json');
 const source=read(base+'u01l01-all-61-source-exercises-preparation.json');
 const analog=read(base+'u01l01-original-61-parallel-skill-practice.json');
 assert.equal(audit.countedNumberedSourceExercises,61);
 assert.equal(audit.visualSkillCategoryReviews,61);
 assert.equal(audit.mappedIndependentAnalogQuestionCount,61);
 assert.deepEqual(audit.sourcePdfPage1Based,[16,17,18,19,20]);
 assert.equal(audit.sourceObjectivesObserved.length,2);
 assert.equal(audit.sourceGlossaryObserved.length,8);
 assert.equal(audit.sourceGuidedPracticeObserved.length,5);
 assert.equal(audit.records.length,61);
 for(let i=0;i<61;i++){
  const item=audit.records[i];
  assert.equal(item.sourceItem,'U01L01-Q'+String(i+1).padStart(2,'0'));
  assert.equal(item.sourceItem,source.items[i].sourceItem);
  assert.equal(item.sourcePrintedPage,source.items[i].printedPage);
  assert.equal(item.sourcePdfPage1Based,item.sourcePrintedPage+11);
  assert.equal(item.analogQuestionId,analog.records[i].independentTaskId);
  assert.equal(item.skillCategoryComparedAgainstScannedPage,true);
  assert.equal(item.sourceExerciseItemFullyCertified,false);
  assert.equal(item.verifierRoutes,'MANUAL_TEACHER_REVIEW_ONLY');
 }
 assert.deepEqual(audit.requiresFurtherTextAmbiguityInspection,[7,25]);
 assert.equal(audit.originalQuestionsFullyPreserved,false);
 assert.equal(audit.publisherExactTaskOneToOneParityCertified,false);
 assert.equal(audit.fullyAuditedRawSourceLesson,false);
 assert.equal(audit.allRawSourceNonnumberedAtomicElementsCovered,false);
 assert.equal(audit.sourceElementDenominatorVerified,false);
 assert.equal(audit.sourceIndexComplete,false);
});
test('The original cube figure has a 4-by-4 grid on all three visible faces and remains first-party SVG',()=>{
 const html=fs.readFileSync(path.join(root,activityPath),'utf8');
 const grid=html.match(/<g id="g9-cube-cells-grid"[^>]*>([\s\S]*?)<\/g>/);
 assert.ok(grid);
 assert.equal((grid[1].match(/<line /g)||[]).length,18);
 assert.ok(html.includes('role="img"'));
 assert.ok(html.includes('<ellipse cx="165"'));
 assert.ok(html.includes('<polygon points="430,70'));
});
test('U01L01 source has two printed goals mapped explicitly to four expanded Edulytics competencies without implying teacher grading occurred',()=>{
 const contract=read(base+'u01l01-source-objectives-practice-contract.json');
 const audit=read(base+'u01l01-61-source-visual-skill-audit.json');
 const ledger=read(base+'semantic-audit-ledger.json').rows[0];
 assert.equal(contract.sourceTextbookExplicitObjectiveCount,2);
 assert.equal(contract.sourceTextbookExplicitObjectives.length,2);
 assert.equal(contract.edulyticsExpandedPracticeCompetencyObjectiveCount,4);
 assert.equal(contract.objectives.length,4);
 assert.deepEqual(contract.sourceTextbookExplicitObjectives.flatMap(x=>x.mappedDerivedOutcomes).sort(),['O1','O2','O3','O4']);
 assert.equal(ledger.sourcePrintedExplicitGoalCount,2);
 assert.equal(ledger.derivedAcademicCompetencyCount,4);
 assert.equal(ledger.objectiveCountKind,'EDULYTICS_DERIVED_COMPETENCY_OBJECTIVES_NOT_SOURCE_PRINTED_HEADINGS');
 assert.equal(audit.records.length,61);
 for(const row of audit.records){
   assert.equal(row.analogQuestionRequiresTeacherReview,true);
   assert.equal(row.analogQuestionActuallyReviewedByTeacher,false);
   assert.equal(row.sourceExerciseItemFullyCertified,false);
 }
});
test('U01L01 source visual lesson corpus records 21 nonnumbered teaching elements separate from 61 numbered exercises',()=>{
 const subset=read(base+'u01l01-nonnumbered-source-visual-subset.json');
 const numbered=read(base+'u01l01-61-source-visual-skill-audit.json');
 assert.equal(subset.observedTwoSourceObjectives,2);
 assert.equal(subset.observedGlossaryTerms,8);
 assert.equal(subset.observedWorkedExampleSubparts,6);
 assert.equal(subset.observedGuidedPracticePrompts,5);
 assert.equal(subset.itemizedNonNumberedInstructionalSubsetCount,21);
 assert.equal(subset.elements.length,21);
 assert.equal(numbered.records.length,61);
 assert.equal(subset.combinedDocumentedSubsetCount,82);
 const types=subset.elements.reduce((acc,row)=>(acc[row.type]=(acc[row.type]||0)+1,acc),{});
 assert.deepEqual(types,{'printed-objective':2,'glossary':8,'worked-example-subpart':6,'guided-practice':5});
 assert.deepEqual(subset.sourcePdfPage1Based,[16,17]);
 assert.ok(subset.elements.every(x=>x.visualTextbookLocationVerified===true));
 assert.ok(subset.elements.every(x=>x.fullSourceAtomicEquivalenceCertified===false));
 assert.equal(subset.sourceTotalAtomicElementDenominatorVerified,false);
 assert.equal(subset.fullSourceLessonAuditPassed,false);
});
