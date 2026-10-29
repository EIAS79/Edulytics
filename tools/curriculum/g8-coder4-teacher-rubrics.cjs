'use strict';
// First-party draft teacher-scored assessment tasks. Not an installed/observed teacher workflow.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const matrix=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json'),'utf8'));
const richDir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const docs=fs.readdirSync(richDir).filter(x=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/i.test(x))
 .flatMap(x=>JSON.parse(fs.readFileSync(path.join(richDir,x),'utf8')).Lessons);
const byCode=new Map(docs.map(x=>[x.LessonCode,x]));
const taskRe=/student task|partner|construct|draw|investigat|original|model|sketch|sort|challenge|diagram|compare|learner/i;
const framework={
 1:['Reproducible geometric movement or correspondence','Lengths, angles and direction mathematically correct','Readable labeled figure or construction','Explanation with accurate terms and a valid reason'],
 2:['Accurate dilation/similarity diagram and center','Corresponding side/angle scale justified','Calculations and units correct','Original task explanation clear to a peer'],
 3:['Correct tables, equations or coordinate graph','Slope, intercept and domain accurately identified','Independent substitution or rate check','Contextual explanation supported by evidence'],
 4:['Faithful translation from verbal puzzle or system','Legal algebraic equivalence transformations','Independent check in original equation(s)','Correct solution count and realistic constraints'],
 5:['Correct function mapping, graph or 3D representation','Rule or volume formula valid for given conditions','Numerical examples and dimensional units correct','Model/physical interpretation and limitations explained'],
 6:['Correct paired data display or two-way table','Appropriate trend, rate or conditional share calculation','Unusual points and measurement limitations considered','Interpretation avoids unsupported causal claims'],
 7:['Correct integer-exponent or scientific notation model','Exponent laws applied under correct nonzero conditions','Exact numerical value, scale and units verified','Written equivalence or estimate justified without irrelevant logarithms'],
 8:['Correct root, triangle or decimal representation','Pythagorean/inverse/irrational reasoning justified','Numerical check, domain and units correct','Graph or geometric proof supported by a diagram'],
 9:['Correct tiling or statistical model representation','Vertex angle, scatter trend or fitted prediction correct','Units, original data and assumptions labeled','Limitations or tessellation fit justified']
};
const results=[];
for(const row of matrix.rows){
 const l=byCode.get(row.lessonCode);
 if(!l)throw Error('No first-party task for '+row.lessonCode);
 const text=l.ExplanationParagraphs.slice(1,-1);
 const task=text.find(x=>taskRe.test(x) && x.length>60)??text.find(x=>x.length>60);
 if(!task)throw Error('No task paragraph for '+row.lessonCode);
 const criteria=framework[row.unit];
 if(!criteria||criteria.length!==4)throw Error('No rubric domain');
 results.push({
  lessonCode:row.lessonCode,sourceLessonCode:row.sourceLessonCode,unit:row.unit,title:l.Title,
  curriculumSource:row.sourceUrl,sourceEdition:row.sourceEdition,
  authoredTaskPrompt:task,
  deliverable:'Student-created response, labeled diagram, written justification or source-aligned investigation, as applicable to the stated task.',
  evidenceType:'TEACHER_REVIEW_REQUIRED',
  rubric:{scoring:'0=absent or incorrect; 1=partially supported; 2=correct and fully justified for each criterion',
   maximum:8,criteria:criteria.map((criterion,i)=>({criterionId:i+1,description:criterion,points:[0,1,2]}))},
  machineScored:false,
  teacherWorkflowRuntimeIntegrated:false,
  studentSubmissionPersisted:false,
  teacherEvaluationObserved:false,
  academicCertified:false,
  caution:'Draft independently authored prompt/rubric. Requires lesson-specific human validation and a real authenticated teacher workflow before verification.'
 });
}
if(results.length!==131||new Set(results.map(r=>r.lessonCode)).size!==131)throw Error('Incomplete task corpus');
const report={schemaVersion:1,packCode:'US-CCSS-MATH',course:'US-CCSS-MATH:G8:OUR-IM-2017',
 status:'DRAFT_FIRST_PARTY_TEACHER_ASSESSMENT_SPECIFICATIONS_NOT_RUNTIME_INTEGRATED',
 source:'Edulytiks original lesson task paragraphs; Open Up Resources / IM 2017-2019 CC BY 4.0 credited in lessons',
 lessonCoverage:131,draftRubrics:131,authenticatedTeacherAssessmentImplementationsVerified:0,observedTeacherScores:0,
 disclaimer:'Rubrics are candidate handoffs only; this does not create a teacher-facing UI, saved evidence, student submission endpoint, automatic grading or publisher assessment parity.',lessons:results};
const output='docs/curriculum/us-ccss-g8-coder4-teacher-rubric-candidates-20261029.json';
fs.writeFileSync(path.join(root,output),JSON.stringify(report,null,2)+'\n');
console.log('TEACHER_RUBRICS '+JSON.stringify({file:output,draftRubrics:results.length,observed:0}));
