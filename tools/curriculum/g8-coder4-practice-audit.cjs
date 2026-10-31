'use strict';
// Read-only Practice crosswalk; never claims actual generator or saved-attempt success.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const matrixPath='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
const outcomes=read('src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-map.v1.json');
const skills=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const matrix=read(matrixPath);
const map=new Map(outcomes.entries.map(x=>[x.outcomeCode,x]));
const bySkill=new Map(skills.mappings.filter(x=>x.lessonCode?.startsWith('PED:US-CCSS-MATH:G8:')).map(x=>[x.lessonCode,x]));
const docs=fs.readdirSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2'))
 .filter(x=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(x)).flatMap(x=>read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/'+x).Lessons);
const byCode=new Map(docs.map(x=>[x.LessonCode,x]));
const rules=read('src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-rules.v1.json');
const semanticRiskIds=new Set(['transformations','shape-properties','linear-equations','exponential-log','scatter-plots','statistics-scatter','pytagorean-distance']);
const rows=[];
for(const r of matrix.rows){
 const targets=r.outcomeCodes.map(code=>({
  code,resolvedStatic:map.has(code),
  targetRuleId:map.get(code)?.targetRuleId??null,
  resolutionRuleId:map.get(code)?.resolutionRuleId??null
 }));
 const explicit=bySkill.get(r.lessonCode);
 const content=byCode.get(r.lessonCode);
 rows.push({
  lessonCode:r.lessonCode,unit:r.unit,sourceLessonCode:r.sourceLessonCode,title:r.title,
  teacherSourceGoal:r.publisherStudentGoal,
  officialOutcomeTargets:targets,
  explicitLessonSkill:explicit?{
   primarySkills:explicit.primarySkills??[],approvedFamilies:explicit.allowedQuestionFamilies??[],
   practiceMechanic:explicit.practiceMechanic??null,practiceReadiness:explicit.practiceReadiness??null
  }:null,
  firstPartyWorkedExampleCount:content?.WorkedExamples?.length??0,
  originalTaskCandidateDescription:content?.ExplanationParagraphs?.find(p=>/Student task:|Partner task:|Original task:|Original Edulytiks task:|Investigate|Draw |Construct |Make |Partner|Teacher/.test(p))??null,
  lessonScopedGeneratorActuallyExecuted:false,
  evaluatorNegativeCasesExecuted:false,
  savedStudentAttemptVerified:false,
  authenticatedTeacherEvidenceVerified:false,
  teacherAssessmentRubricDeliveredToRuntime:false,
  reviewDecision:'PRACTICE_NOT_VERIFIED',
  integrationRisk:(!targets.every(t=>t.resolvedStatic)||new Set(targets.map(t=>t.targetRuleId)).size>1)
   ?'REQUIRES_EXPLICIT_MULTI_TARGET_OR_UNRESOLVED_REVIEW'
   :'MAPPING_PRESENT_BUT_LESSON_OBJECTIVE_UNVERIFIED'
 });
}
const freq={};for(const r of rows)for(const t of r.officialOutcomeTargets)freq[t.targetRuleId??'<unmapped>']=(freq[t.targetRuleId??'<unmapped>']??0)+1;
const mapped=rows.filter(x=>x.officialOutcomeTargets.every(y=>y.resolvedStatic));
const report={schemaVersion:1,scope:'US-CCSS-MATH:G8:OUR-IM-2017',lessonCount:rows.length,
 source:'Embedded official-outcome-practice-map.v1.json plus lesson-skill-mappings.v1.json',
 explicitLessonSkillMappings:rows.filter(x=>x.explicitLessonSkill).length,
 staticOfficialTargetsMappedForAllLessons:mapped.length,
 unmappedLessonCodes:rows.filter(x=>!x.officialOutcomeTargets.every(y=>y.resolvedStatic)).map(x=>x.lessonCode),
 potentialMultipleTargetLessonCodes:rows.filter(x=>new Set(x.officialOutcomeTargets.map(y=>y.targetRuleId)).size>1).map(x=>x.lessonCode),
 targetRuleFrequency:freq,
 authenticatedPracticeLessons:0,teacherAssessmentPersistedLessons:0,verifiedLessonExactPractice:0,
 warning:'A registry entry identifies a candidate Practice target; it does not establish that a question correctly assesses this exact publisher lesson, that a generator renders, that grading is secure, or that attempts are persisted. These remain unverified for ALL 131 lessons.',
 rows};
report.unmappedMathematicalPracticeOutcomes=[...new Set(rows.flatMap(row=>row.officialOutcomeTargets.filter(t=>!t.resolvedStatic&&t.code.startsWith('CCSS:MP.')).map(t=>t.code)))].sort();
report.unmappedFormalOutcomes=[...new Set(rows.flatMap(row=>row.officialOutcomeTargets.filter(t=>!t.resolvedStatic&&!t.code.startsWith('CCSS:MP.')).map(t=>t.code)))].sort();
report.lessonsWithOnlyMathematicalPracticeTargets=rows.filter(row=>row.officialOutcomeTargets.every(t=>t.code.startsWith('CCSS:MP.'))).map(row=>row.lessonCode);
report.lessonsWithMultipleResolvedConcreteFamilies=rows.filter(row=>new Set(row.officialOutcomeTargets.filter(t=>t.resolvedStatic).map(t=>t.targetRuleId)).size>1).map(row=>row.lessonCode);
report.note='Mathematical Practice (MP) outcomes without direct outcome-target mappings are not automatically missing Grade 8 content standards; source and exact task suitability still require independent inspection. All generator/runtime claims remain unverified.';
const p='docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json';
fs.writeFileSync(path.join(root,p),JSON.stringify(report,null,2)+'\n');
console.log('PRACTICE_CROSSWALK '+JSON.stringify({lessonCount:report.lessonCount,explicitMappings:report.explicitLessonSkillMappings,staticMappedAll:report.staticOfficialTargetsMappedForAllLessons,unmapped:report.unmappedLessonCodes,potentialMultipleTarget:report.potentialMultipleTargetLessonCodes,targetRuleFrequency:report.targetRuleFrequency}));
