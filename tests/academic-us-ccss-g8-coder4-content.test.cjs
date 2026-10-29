'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const bpPath='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json';
const packPath='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g8-phase29-v1.lesson-content-pack.json';
const srcPath='docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json';
const richDir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const bp=read(bpPath),pack=read(packPath),source=read(srcPath),matrix=read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const richFiles=fs.readdirSync(path.join(root,richDir)).filter(x=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/i.test(x));
const richDocuments=richFiles.map(f=>({file:f,data:read(richDir+'/'+f)}));
const rich=richDocuments.flatMap(x=>x.data.Lessons),by=new Map(rich.map(x=>[x.LessonCode,x]));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const plus=(p,v)=>[p[0]+v[0],p[1]+v[1]],ccw=([x,y])=>[-y,x],cw=([x,y])=>[y,-x];
const mx=([x,y])=>[x,-y],my=([x,y])=>[-x,y],half=([x,y])=>[-x,-y];
const dist2=(p,q)=>(p[0]-q[0])**2+(p[1]-q[1])**2;
test('Grade 8 ordered blueprint, content, public source and manifest reconcile 131 identities in 9 units',()=>{
 assert.equal(bp.BlueprintCode,'US-CCSS-MATH:G8:OUR-IM-2017');
 assert.equal(bp.LogicalLevel,9);assert.equal(bp.NativeLevel,'Grade 8');
 assert.deepEqual(bp.Units.map(x=>x.LessonCount),[17,13,14,16,22,11,16,16,6]);
 for(const arr of [bp.Lessons,pack.lessons,source.rows,matrix.rows])assert.equal(arr.length,131);
 assert.equal(new Set(bp.Lessons.map(x=>x.LessonCode)).size,131);
 assert.equal(new Set(bp.Lessons.map(x=>x.SourceLessonCode)).size,131);
 assert.deepEqual(matrix.rows.map(x=>x.lessonCode),bp.Lessons.map(x=>x.LessonCode));
 assert.deepEqual(matrix.rows.map(x=>x.sourceUrl),bp.Lessons.map(x=>x.SourceUrl));
 assert.equal(matrix.rows[0].lessonCode,'PED:US-CCSS-MATH:G8:U01:L01');
 assert.equal(matrix.rows[130].lessonCode,'PED:US-CCSS-MATH:G8:U09:L06');
 assert.equal(matrix.hashes.blueprint,sha(bpPath));
 assert.equal(matrix.hashes.canonicalPack,sha(packPath));
 assert.equal(matrix.hashes.publisherCrosswalk,sha(srcPath));
 for(const [file,expected] of Object.entries(matrix.hashes.richCandidates))assert.equal(sha(richDir+'/'+file),expected);
});
test('public Grade 8 source pages were retrieved without treating page scan as academic proof',()=>{
 assert.equal(source.retrieved,131);assert.equal(source.missing,0);assert.equal(source.roleDiscrepancies,22);
 for(const x of source.rows){
  assert.ok(x.sourceStudentGoal?.length>10,x.lessonCode);
  assert.match(x.url,/^https:\/\/im\.kendallhunt\.com\//);
  assert.equal(x.fullStudentActivityReview,false);
 }
 assert.ok(source.rows[0].blueprintExtraRoles.includes('Addressing:8.G.A.1'));
 assert.ok(source.rows[0].sourceRoles.some(x=>x.role==='BuildingTowards'&&x.reference==='8.G.A.1'));
 assert.equal(matrix.counts.academicAccepted,0);
});
test('all 131 source-specific Rich V2 candidate lessons load as distinct valid embedded-resource documents',()=>{
 assert.equal(richFiles.length,11);assert.equal(rich.length,131);assert.equal(by.size,131);
 assert.deepEqual([...by.keys()].sort(),bp.Lessons.map(x=>x.LessonCode).sort());
 const csproj=fs.readFileSync(path.join(root,'src/Edulytics.Core/Edulytics.Core.csproj'),'utf8');
 assert.match(csproj,/RichV2\/\*\.rich-lesson-v2\.json/);
 for(const {file,data} of richDocuments){
  assert.equal(data.SchemaVersion,1,file);assert.equal(data.PackCode,'US-CCSS-MATH',file);
  assert.ok(data.ContentVersion.includes('g8'),file);
 }
 let examples=0;const questions=[];
 for(const l of rich){
  assert.equal(l.CultureCode,'en',l.LessonCode);
  assert.ok(l.ExplanationParagraphs.length>=2,l.LessonCode);
  assert.ok(l.KeyConcepts.length>=3,l.LessonCode);
  assert.ok(l.WorkedExamples.length>=3,l.LessonCode);
  assert.ok(l.CommonMistakes.length>=2,l.LessonCode);
  assert.ok(l.SummaryPoints.length>=3,l.LessonCode);
  assert.deepEqual(l.Videos,[],l.LessonCode);
  assert.ok(l.ExplanationParagraphs.some(p=>p.includes('CC BY 4.0')),l.LessonCode);
  assert.ok(l.ExplanationParagraphs.some(p=>/teacher/i.test(p)),l.LessonCode);
  assert.ok(!l.ExplanationParagraphs.some(p=>/^Learning goal: You will /i.test(p)),l.LessonCode);
  assert.equal(new Set(l.WorkedExamples.map(x=>x.Question)).size,l.WorkedExamples.length,l.LessonCode);
  for(const e of l.WorkedExamples){
   assert.ok(e.Title&&e.Question&&e.Method&&e.Answer&&e.Check,l.LessonCode);
   assert.ok(e.Steps.length>=3,l.LessonCode+': '+e.Title);questions.push(e.Question);examples++;
  }
  for(const c of l.KeyConcepts)assert.ok(c.Title&&c.Definition&&c.Rule&&c.Example,l.LessonCode);
  for(const c of l.CommonMistakes)assert.ok(c.Mistake&&c.WhyWrong&&c.Correction,l.LessonCode);
 }
 assert.equal(examples,393);
 assert.ok(new Set(questions).size>=370,'Excess duplicate worked examples');
});
test('independent 4096-case rigid transformation metamorphic and invalid-direction checks',()=>{
 let seed=20261029;const rand=()=>((seed=(Math.imul(1664525,seed)+1013904223)>>>0)%1025)-512;
 for(let i=0;i<4096;i++){
  const p=[rand(),rand()],q=[rand(),rand()],v=[rand(),rand()];
  assert.equal(dist2(plus(p,v),plus(q,v)),dist2(p,q));
  for(const fn of [ccw,cw,mx,my,half])assert.equal(dist2(fn(p),fn(q)),dist2(p,q));
  assert.deepEqual(cw(ccw(p)),p);assert.deepEqual(mx(mx(p)),p);assert.deepEqual(my(my(p)),p);
  assert.deepEqual(half(half(p)),p);assert.deepEqual(plus(plus(p,v),[-v[0],-v[1]]),p);
 }
 assert.notDeepEqual(ccw([2,1]),cw([2,1]));
 assert.notDeepEqual(mx([5,-3]),my([5,-3]));
 assert.deepEqual([[2,1],[3,1],[2,2]].map(ccw),[[-1,2],[-1,3],[-2,2]]);
 assert.deepEqual(my([3,-2]),[-3,-2]);
});
test('full grade math oracle: slopes, systems, quadratic/sphere volumes and similarity',()=>{
 const L=(u,n)=>by.get('PED:US-CCSS-MATH:G8:U'+String(u).padStart(2,'0')+':L'+String(n).padStart(2,'0'));
 assert.match(L(2,11).WorkedExamples[0].Answer,/y=2x\+1/);
 assert.match(L(4,15).WorkedExamples[1].Answer,/5 adults and 5 children/);
 assert.equal((8-2)/(4-1),2);
 assert.equal((1-5)/(2-0),-2);
 assert.deepEqual([3,7],[(10-1)/3,2*(10-1)/3+1]);
 assert.equal(5*8+5*5,65);
 assert.equal(2*5+3*3,19);
 assert.equal(3**2+4**2,5**2);
 assert.equal(Math.sqrt(169-25),12);
 assert.equal(Math.sqrt((4-(-2))**2+(9-1)**2),10);
 assert.equal(2*3*4,24);
 assert.equal(Math.PI*3**2*5,45*Math.PI);
 assert.equal(Math.PI*3**2*6/3,18*Math.PI);
 assert.ok(Math.abs((4/3)*Math.PI*3**3-36*Math.PI)<1e-10);
 assert.equal(3*12/8,4.5);
 assert.equal(57600/337<192,true);
 assert.match(L(8,16).WorkedExamples[1].Answer,/57600\/337/);
});
test('integer exponent and scientific notation oracles include negative and rational powers; no logarithm substitution',()=>{
 const L=(u,n)=>by.get('PED:US-CCSS-MATH:G8:U'+String(u).padStart(2,'0')+':L'+String(n).padStart(2,'0'));
 for(const k of [-9,-5,-1,0,1,6])for(const j of [-5,-2,0,3]){
  assert.ok(Math.abs(10**k*10**j-10**(k+j))<=Math.abs(10**(k+j))*1e-12+1e-18);
  assert.ok(Math.abs((2**k)/(2**j)-2**(k-j))<=Math.abs(2**(k-j))*1e-12+1e-18);
 }
 assert.equal(2**3*2**-5,1/4);
 assert.ok(Math.abs((2/3)**-2-9/4)<1e-12);
 assert.equal(10**-3,0.001);
 assert.equal((3*10**4)*(2*10**3),6*10**7);
 assert.equal(2.4e6+3e5,2.7e6);
 assert.match(L(7,6).WorkedExamples[0].Answer,/1\/4/);
 assert.match(L(7,13).WorkedExamples[0].Answer,/6.2×10⁴/);
 assert.ok(!L(7,6).WorkedExamples.some(e=>/logarithm|log\(/i.test(e.Answer)));
});
test('probabilistic measurement, decimal and fitted-model error oracles',()=>{
 const L=(u,n)=>by.get('PED:US-CCSS-MATH:G8:U'+String(u).padStart(2,'0')+':L'+String(n).padStart(2,'0'));
 assert.equal((4+4)/2,4);
 assert.equal(12/20,0.6);assert.equal(5/20,0.25);
 assert.equal((31-(40-0.5*20)),1);
 assert.equal((14-(40-0.5*50)),-1);
 assert.equal((40-0.5*45),17.5);
 assert.equal((5.4**2<30)&&(5.5**2>30),true);
 assert.equal(1.4142**2<2,true);
 assert.equal(4/33,0.12121212121212122);
 assert.match(L(9,6).WorkedExamples[0].Answer,/17.5/);
 assert.match(L(8,7).WorkedExamples[0].Answer,/a²\+b²=c²/);
});
test('exact Practice, teacher observation and academic approvals are never manufactured',()=>{
 assert.equal(matrix.counts.curriculumLessons,131);
 assert.equal(matrix.counts.richV2AuthoredCandidates,131);
 assert.equal(matrix.counts.exactPracticeVerified,0);
 assert.equal(matrix.counts.humanAssessmentVerified,0);
 assert.equal(matrix.counts.academicAccepted,0);
 assert.equal(matrix.counts.fullGradeVerified,0);
 for(const row of matrix.rows){
  assert.ok(row.authoredRichV2Asset,row.lessonCode);
  assert.equal(row.exactPracticeChecked,false);
  assert.equal(row.authStudentRenderingChecked,false);
  assert.equal(row.academicAccepted,false);
  assert.equal(row.stageD,'NOT_RUN');
  assert.ok(row.unresolved.length>=5);
 }
});

test('V7 per-lesson triage preserves source, Practice and teacher evidence without promotion',()=>{
 const gate=read('docs/curriculum/us-ccss-g8-coder4-v7-lesson-gates-20261029.json');
 const audit=read('docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json');
 assert.equal(gate.totalLessons,131);
 assert.equal(new Set(gate.rows.map(x=>x.lessonCode)).size,131);
 assert.deepEqual(gate.rows.map(x=>x.lessonCode),bp.Lessons.map(x=>x.LessonCode));
 assert.equal(gate.coverage.richCandidates,131);
 assert.equal(gate.coverage.acceptedA,0);
 assert.equal(gate.coverage.acceptedB,0);
 assert.equal(gate.coverage.verifiedPracticeC,0);
 assert.equal(gate.coverage.verifiedRuntimeD,0);
 assert.equal(gate.coverage.sourceRoleDiscrepancy,22);
 assert.equal(gate.coverage.mathematicalPracticeOnlyOutcomes,12);
 assert.equal(gate.coverage.unmappedFormalOutcomes,0);
 assert.equal(gate.coverage.multiPracticeFamilyTargets,18);
 assert.equal(gate.coverage.explicitSkillSemanticMismatchCandidates,1);
 assert.equal(gate.rows.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G8:U03:L12').evidence.semanticPracticeMismatchCandidate,true);
 assert.equal(audit.unmappedFormalOutcomes.length,0);
 assert.deepEqual(audit.unmappedMathematicalPracticeOutcomes,['CCSS:MP.1','CCSS:MP.2','CCSS:MP.3','CCSS:MP.4','CCSS:MP.5','CCSS:MP.6','CCSS:MP.7','CCSS:MP.8']);
 for(const row of gate.rows){
  assert.ok(row.classification.includes('SOURCE_REVIEW_BLOCKED'));
  assert.ok(row.classification.includes('TEACHER_ASSESSMENT'));
  assert.equal(row.evidence.academicA,false);
  assert.equal(row.evidence.academicB,false);
  assert.equal(row.evidence.academicC,false);
  assert.equal(row.evidence.academicD,false);
  assert.equal(row.evidence.actualPracticeGenerated,false);
  assert.equal(row.evidence.actualAuthLearnerRendering,false);
  assert.equal(row.evidence.integratedTeacherScoringRubric,false);
 }
});

test('V8 fetched authentic student and teacher lesson activity structures for all canonical Grade 8 identities without certifying source parity',()=>{
 const fetched=read('docs/curriculum/us-ccss-g8-coder4-v8-publisher-activity-structure-20261029.json');
 const ledger=read('docs/curriculum/us-ccss-g8-coder4-v8-acceptance-ledger-20261029.json');
 assert.equal(fetched.student200,131);assert.equal(fetched.teacher200,131);
 assert.equal(fetched.studentActivityHeadingsFetched,131);
 assert.equal(fetched.sourceFigureInspectionPriorities,118);
 assert.equal(ledger.counts.sourceActivityHeadings,411);
 assert.equal(ledger.rows.length,131);
 assert.deepEqual(ledger.rows.map(r=>r.lessonCode),bp.Lessons.map(x=>x.LessonCode));
 assert.equal(ledger.counts.standardsRoleDifferences,22);
 assert.equal(ledger.counts.explicitPracticeSemanticMismatch,1);
 assert.equal(ledger.counts.aAccepted,0);assert.equal(ledger.counts.bAccepted,0);
 assert.equal(ledger.counts.cVerified,0);assert.equal(ledger.counts.dVerified,0);
 for(const x of ledger.rows){
  assert.ok(x.sourceEvidence.teacherPreparationUrl.includes('/teachers/3/'));
  assert.ok(x.sourceEvidence.studentLessonUrl.includes('/students/3/'));
  assert.ok(x.sourceEvidence.teacherLessonHttp200);
  assert.ok(x.sourceEvidence.studentLessonHttp200);
  assert.match(x.sourceEvidence.studentHtmlSha256,/^[a-f0-9]{64}$/);
  assert.match(x.sourceEvidence.teacherHtmlSha256,/^[a-f0-9]{64}$/);
  assert.ok(x.sourceEvidence.studentActivityCount>0);
  assert.equal(x.verdict,'BLOCKED_WITH_CAUSE');
  assert.equal(x.stageA,'NOT_ACCEPTED');assert.equal(x.stageB,'NOT_ACCEPTED');
  assert.equal(x.stageC,'NOT_VERIFIED');assert.equal(x.stageD,'NOT_VERIFIED');
  assert.equal(x.sourceEvidence.fullSourceStudentActivityReviewedByHuman,false);
  assert.equal(x.practice.attemptPersistenceVerified,false);
  assert.equal(x.practice.generatedLessonSpecificPracticeVerified,false);
 }
 const twoVariable=ledger.rows.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G8:U03:L12');
 assert.deepEqual(twoVariable.sourceEvidence.studentActivityTitles,['12.1: Estimate Area','12.2: Apples and Oranges','12.3: Solutions and Everything Else']);
 assert.equal(twoVariable.practice.knownSemanticMappingMismatch,true);
 const proof=ledger.rows.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G8:U08:L07');
 assert.equal(proof.sourceEvidence.visualSourceInspectionPriority,true);
 assert.equal(proof.candidate.richV2VisualCount,0);
});

test('V9 activity-scoped publisher figure, cool-down and role evidence has 131 intact lesson identities with no false A-D promotions',()=>{
 const activity=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
 const ledger=read('docs/curriculum/us-ccss-g8-coder4-v9-acceptance-ledger-20261029.json');
 assert.equal(activity.counts.lessons,131);
 assert.equal(activity.counts.studentActivities,411);
 assert.equal(activity.counts.lessonsWithUniqueSourceFigures,113);
 assert.equal(activity.counts.teacherOnlyActivityHeadings,117);
 assert.equal(activity.counts.studentOnlyActivityHeadings,0);
 assert.equal(activity.counts.totalActivityFigureOccurrences,339);
 assert.equal(ledger.counts.sourceRoleDiscrepancyDecisions,22);
 assert.equal(ledger.counts.lessonsWithFirstPartyRichV2Visuals,1);
 assert.equal(ledger.counts.sourceTeacherOnlyActivities,117);
 assert.deepEqual(ledger.rows.map(r=>r.lessonCode),bp.Lessons.map(l=>l.LessonCode));
 const seen=new Set();
 for(const row of ledger.rows){
  assert.ok(!seen.has(row.lessonCode));seen.add(row.lessonCode);
  assert.equal(row.status,'BLOCKED_WITH_CAUSE');
  assert.equal(row.A,'NOT_ACCEPTED');assert.equal(row.B,'NOT_ACCEPTED');
  assert.equal(row.C,'NOT_VERIFIED');assert.equal(row.D,'NOT_VERIFIED');
  assert.ok(row.source.activityCount>0);
  assert.ok(row.source.activityEvidence.every(a=>a.title&&a.bodySha256.length===64));
  assert.equal(row.practice.securePersistedStudentAttemptVerified,false);
  assert.equal(row.figures.effectiveBrowserFigureRenderingVerified,false);
  assert.equal(row.figures.independentlyAccessibleSourceParityVerified,false);
  if(row.standardsRoles.hasDiscrepancy)assert.equal(row.standardsRoles.ownerApproved,false);
 }
 const lesson=by.get('PED:US-CCSS-MATH:G8:U03:L12');
 assert.equal(lesson.Visuals.length,2);
 const table=lesson.Visuals.find(v=>v.Kind==='RatioTable');
 assert.ok(table);assert.match(table.Description,/two-variable linear equation/i);
 const xs=table.Rows[1].Cells.map(Number),ys=table.Rows[2].Cells.map(Number),
  left=table.Rows[3].Cells.map(Number),yes=table.Rows[4].Cells;
 for(let j=0;j<4;j++){
  assert.equal(left[j],2*xs[j]+ys[j]);
  assert.equal(yes[j],left[j]===7?'Yes':'No');
 }
 assert.equal(ledger.rows.find(x=>x.lessonCode.endsWith('U03:L12')).practice.explicitKnownMismatch,true);
});
