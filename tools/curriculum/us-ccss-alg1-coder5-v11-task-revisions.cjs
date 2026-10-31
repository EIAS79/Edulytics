#!/usr/bin/env node
'use strict';
// Course-local source-task refinements and proposed teacher scoring rubrics.
// NEVER create scores/teacher submissions or convert this manifest into approval.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const dir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const key='PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:';
const edits=[
  {id:'U06:L11',filename:'us-ccss-hs-alg1-u06-l11-2021.rich-lesson-v2.json',
    originalPrefix:'Student task:',
    instruction:'Student task (original first-party source-equivalent comparison): without reading the prepared graph first, use g(x)=-2(x-1)(x-5) and h(x)=2(x-1)(x-5) to predict BOTH x-intercepts, each vertex, and each y-intercept. Complete an original table at x=0,1,2,3,4,5,6 for both functions. Plot the two parabolas on the SAME labeled coordinate axes, with distinct colors or point markers. Justify why their x-intercepts agree but their opening, extrema and y-intercepts differ; verify predictions using graphing technology if available. Submit your own plotted graph plus reasoning to an authorized teacher. A pre-drawn SVG is only an explanatory example, not an auto-graded student submission. Human-reviewed draft rubric: 0–2 each for correct table/zero products, both graphs with scale and labels, vertex/y-intercept reasoning, and actual independent verification and explanation.',
    newParagraph:'For the comparison, h(x)=+2(x-1)(x-5) has the same zero-factor inputs x=1 and x=5 as g, but vertex (3,-8) and y-intercept (0,10). Unlike g, it opens upward. This second independently constructed function is an original analogous task rather than a copy of the publisher activity.',
    standardNote:'Teacher goal and student task reviewed against U06:L11 teacher preparation and student graph-comparison activities. Graphing-technology use must be documented if claimed.',
    criteria:[
      ['Table and zeros','Correct outputs for both functions, all shared x-intercepts, with verified substitutions.'],
      ['Two self-created graphs','Both graph curves plotted on one labeled scale with distinguishable markings; includes the vertices.'],
      ['Function-form explanations','Identifies both extrema and y-intercepts and explains factor-product root relationships.'],
      ['Verification and communication','Gives reasoned comparison, checking graphing technology only if actually used; independent learner work.']
    ], teacher:'https://im.kendallhunt.com/HS/teachers/1/6/11/preparation.html',student:'https://im.kendallhunt.com/HS/students/1/6/11/index.html'},
  {id:'U06:L16',filename:'us-ccss-hs-alg1-u06-l16-2021.rich-lesson-v2.json',
    originalPrefix:'Student task:',
    instruction:'Student task (original source-equivalent shared-vertex comparison): match the standard, factored and vertex descriptions of a sample function to their relevant graph features. On the SAME coordinate axes graph g(x)=-0.5(x+2)^2+6 and the original comparison q(x)=0.5(x+2)^2+6. Both have vertex (-2,6). Plot reflected pairs x=-4,0, label both y-intercepts g(0)=4 and q(0)=8, and identify g maximum and q minimum. Discuss with a partner how the two parabolas differ, and submit your own labeled graph. Teacher human-review draft rubric 0–2 each for graph points/symmetry, vertex and y-intercepts, correct min/max interpretation, and form-choice/explanation. In line with publisher teacher guidance, do NOT formally assess a proof for why a positive/negative square coefficient creates a minimum/maximum.',
    newParagraph:'Second comparison function q(x)=0.5(x+2)^2+6 has the SAME vertex (-2,6) as g(x)=-0.5(x+2)^2+6, but opens upward with minimum 6, not maximum. At x=0 and x=-4, q=8 and g=4. These independently chosen parallel expressions support the official activity goal of graphing two functions that share a vertex.',
    standardNote:'U06:L16 source explicitly says informal structural sign reasoning should not be assessed. Grade the labeled graph, extrema and intercept identification, not a formal coefficient-sign proof.',
    criteria:[
      ['Labeled original graph','Both distinct curves, correct symmetric points and sensible coordinate-axis scale.'],
      ['Vertex and vertical intercepts','Correct shared (-2,6) vertex and two y-intercepts (0,4),(0,8).'],
      ['Maximum/minimum','Correctly identifies g maximum=6 and q minimum=6 without demanding a formal proof.'],
      ['Form choice and explanation','Explains how the chosen representations help read graph features and preserves learner work.']
    ],teacher:'https://im.kendallhunt.com/HS/teachers/1/6/16/preparation.html',student:'https://im.kendallhunt.com/HS/students/1/6/16/index.html'},
  {id:'U07:L24',filename:'us-ccss-hs-alg1-u07-l24-2021.rich-lesson-v2.json',
    originalPrefix:'Student culminating task:',
    instruction:'Student culminating task — choose and independently solve two substantial ORIGINAL contexts without prescribed algebraic method. Task A: analyze the hypothetical H(t)=-t^2+6t+2 flight: initial height, maximum height/time, first nonnegative time reaching ground, and the two nonnegative times at height 8m; justify the useful equation form. Task B: analyze the line P(t)=t+6 and parabola H(t) on the same coordinate plane, derive all equal-height intersections algebraically and graphically, and explain why equal heights cannot establish a collision without horizontal-position data. Learners may complete both tasks or, if teacher authorizes, select one and prepare a visual display with a group/gallery-walk discussion. No graphing tool is required, but record genuine use if chosen. Original-first-party numbers are NOT publisher task/figure copies. Human-review draft rubric: 0–2 each for independently selected equivalent quadratic forms, exact verified solutions and units, complete labeled graph and contextual interpretation, and critique of model assumptions and clear group presentation. This task is human scored; generated examples, static SVGs and a rubric JSON never constitute a submitted assignment or a persisted grade.',
    newParagraph:'The publisher culminating source has two substantial open tasks (a quadratic height model and a parabola–line intersection) and offers a choice of presentation/gallery walk. This first-party adaptation deliberately uses other equations H(t)=-t^2+6t+2 and P(t)=t+6 while retaining independent problem selection, two-task structure, physical domain, and exact intersection reasoning; no source screenshot or publisher graph is copied.',
    standardNote:'U07:L24 official teacher preparation lists open problems, choice of representation, and optional gallery walk; rubric candidates are not an approved teacher-scoring workflow.',
    criteria:[
      ['Independent model and representation','Selects and justifies equation forms for both height and line-parabola problems, without step-by-step scaffolding.'],
      ['Solutions and verification','Exact H ground and maximum and equal-height (1,7),(4,10) checked in original functions, with correct units.'],
      ['Visual graph and context','Learner-created labeled parabola/line with valid physical time domain and interpreted maximum/roots.'],
      ['Critical reasoning and communication','Explains limitations of equal-height collision inference, records real tool use, and communicates findings clearly.']
    ],teacher:'https://im.kendallhunt.com/HS/teachers/1/7/24/preparation.html',student:'https://im.kendallhunt.com/HS/students/1/7/24/index.html'}
];
const rubrics=[];
for(const spec of edits){
  const filename=path.join(dir,spec.filename);
  const doc=JSON.parse(fs.readFileSync(filename,'utf8'));
  const lesson=doc.Lessons?.[0];
  if(doc.Lessons.length!==1||lesson.LessonCode!==key+spec.id)throw Error('Identity drift '+spec.id);
  const index=lesson.ExplanationParagraphs.findIndex(p=>p.startsWith(spec.originalPrefix));
  if(index<0)throw Error('Student task prefix missing '+spec.id);
  lesson.ExplanationParagraphs[index]=spec.instruction;
  if(!lesson.ExplanationParagraphs.some(p=>p===spec.newParagraph))
    lesson.ExplanationParagraphs.splice(index,0,spec.newParagraph);
  doc.ContentVersion='alg1-'+spec.id.toLowerCase().replace(':','-')+'-v11-source-task-graph-review-20261029';
  fs.writeFileSync(filename,JSON.stringify(doc,null,2)+'\n','utf8');
  rubrics.push({
    lessonCode:key+spec.id,title:lesson.Title,
    teacherPreparationUrl:spec.teacher,studentActivityUrl:spec.student,
    status:'DRAFT_TEACHER_RUBRIC_HUMAN_REVIEW_PENDING',
    sourceComparisonNote:spec.standardNote,
    studentTask:spec.instruction,
    criteria:spec.criteria.map(([name,evidence],i)=>({id:'C'+(i+1),name,expectedEvidence:evidence,min:0,max:2})),
    totalPoints:8,
    scoreAnchors:{'0':'Missing or mathematically incorrect evidence','1':'Partially supported evidence with material omissions','2':'Specific, correctly justified and independently verifiable evidence'},
    requiredTeacherWorkflow:['authenticated submission by enrolled student in exact ALG1 lesson','authorized assigned teacher reviews actual learner artifact','teacher records 0-2 for each criterion, total 0-8 and narrative feedback','server checks school/class/course/lesson/teacher assignment','grade+feedback persisted with attempt ownership and audit trail'],
    teacherWorkflowVerified:false,submissionVerified:false,gradingVerified:false,feedbackPersistenceVerified:false,
    noFakeGrading:true
  });
}
const report={
  schemaVersion:1,courseKey:'US-CCSS-MATH:HS:TRAD:ALG1:IM-1E',
  checkedAtDate:'2026-10-29',
  status:'THREE_SOURCE_SPECIFIC_DRAFT_TEACHER_RUBRICS_NOT_ACCEPTED',
  approvedTeacherRubricCount:0,verifiedTeacherWorkflowCount:0,
  draftCount:rubrics.length,teacherRubricCandidates:rubrics
};
const file=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json');
fs.writeFileSync(file,JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({updated:rubrics.map(r=>r.lessonCode),rubricDrafts:rubrics.length,accepted:0,output:path.relative(root,file)}));
