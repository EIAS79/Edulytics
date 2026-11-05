#!/usr/bin/env node
'use strict';
// V12 first integrated cross-unit source-task fixes: original first-party examples,
// preserving 132 existing lessons, independent figures, and honest review gates.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const sourceDir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const P='PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:';
const V=(title,description,rows)=>({
 Kind:'RatioTable',Title:title,Description:description,
 PrimaryLabel:'Original input or category',SecondaryLabel:'Independent calculation',
 Rows:rows.map(([Label,value])=>({Label,Cells:[value]}))
});
const changes=[
{id:'U01:L04',goal:'Recognize distribution shape from dot plots and frequency tables; match multiple representations; invent and defend contextual explanations.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/1/4/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/1/4/index.html',
paragraphs:[
'V12 original source-aligned Which One Does Not Belong and card sort: make five first-party data sets at equally spaced values 1,2,3,4,5: A frequencies [1,2,4,2,1], B [2,2,2,2,2], C [5,3,1,1,1], D [4,1,1,1,4], E [1,1,1,3,5]. Without reading labels first, draw five DOT PLOTS and five matching FREQUENCY TABLES, shuffle these ten cards, and take turns explaining your five matches. A is symmetric bell-like, B uniform, C right-skewed, D bimodal and E left-skewed. Every displayed count is fictional, not a class survey.',
'V12 activity fidelity: the publisher explicitly includes a Which One Does Not Belong discussion and partner matching of ten representations, followed by suggesting a plausible real-world context for a displayed distribution. Students must invent at least two plausible contexts for one of the fictional graphs and explain why the source data cannot be inferred uniquely from a shape. The plotted dot positions and frequency cards must be student-created, not auto-inferred from rubric text.',
'Teacher draft assessment: observe actual partner turns and explanations, check 10 independently prepared cards, and score 0–2 for (1) frequencies and graph consistency, (2) correct five matches and shapes, (3) graph-based contextual explanations with uncertainty, and (4) peer questioning and mathematical language. A written draft is not authenticated teacher evidence.'
],
visual:V('Five original distributions: frequency card reference','At values 1 to 5, each row provides five frequencies; students must draw and match actual dot plots separately.',[
 ['A symmetric bell','1,2,4,2,1'],['B uniform','2,2,2,2,2'],['C right-skew','5,3,1,1,1'],['D bimodal','4,1,1,1,4'],['E left-skew','1,1,1,3,5']
]),
criteria:['Correct five original dot plots and five frequency tables','Ten-card partner matching and precise shape names','Defensible imagined contexts without claims of observation','Actual questioning/explanation and independent student evidence'],
practice:['dot-plot sketch and distribution shape classification','frequency-to-graph and graph-to-frequency matching','verbal context critique; HUMAN_TEACHER_ONLY'],
risk:'ORIGINAL_DOT_PLOT_GRAPH_WORKFLOW_UNVERIFIED'},
{id:'U02:L01',goal:'Model a group cost with expressions, identify changing/fixed quantities, set realistic budget and quantity constraints, and defend different solutions.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/2/1/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/2/1/index.html',
paragraphs:[
'V12 source-aligned three-group planning task (all data fictional): given 20 guests, $14 per pizza with 3 servings per pizza, and a fixed $8 for drinks/plates, GROUP A proposes 6 pizzas ($92, only 18 servings, invalid), GROUP B proposes 7 pizzas ($106, 21 servings, valid), GROUP C proposes 8 pizzas ($120, 24 servings, valid). The class budget is $120. Each group states variables n,p, expression C=14p+8, capacity 3p≥n, budget 14p+8≤120 and integer restrictions; a peer must question an assumption before selecting a plan.',
'V12 warm-up modeling analogue (independently chosen numbers): let m=$9 be a main dish, q be an integer number of $2 side dishes, and T=9+2q. At q=3, T=$15. Explain what each symbol represents and why an equation is a model of one possible meal, not a universal price. Change the day-of-party quantities into variables and identify possible integer/inequality constraints.',
'The publisher expects collaborative planning with more than one defensible approach. Teacher scores actual student proposals and rationale, not only the numerical cost: 0–2 each for assumptions/variables, expressions and units, valid constraints, and reasoned presentation/critique. Planning discussion remains unverified without learner evidence.'
],
visual:V('Compare three fictional plans','The least dollar cost is not necessarily feasible. The guest capacity and budget must both hold.',[['A: six pizzas','18 servings, $92: capacity fails'],['B: seven pizzas','21 servings, $106: valid'],['C: eight pizzas','24 servings, $120: valid']]),
criteria:['Identifies independent and changing quantities and units','Derives each proposed cost expression correctly','Checks capacity, integer and budget constraints','Presents multiple realistic plans and critiques assumptions'],
practice:['contextual expression matching and missing variables','constraint inequality checking with domain','open modeling and peer argument; HUMAN_TEACHER_ONLY'],
risk:'GROUP_PLANNING_AND_TEACHER_FEEDBACK_UNVERIFIED'},
{id:'U03:L01',goal:'Build and interpret two-way tables, recover missing cells using marginal totals, and conduct an authentic information-gap exchange.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/3/1/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/3/1/index.html',
paragraphs:[
'V12 source-equivalent information-gap task using completely invented club-preference observations. Keep the full solution hidden from learners: Group A Sports 12 Arts 8 Total 20; Group B Sports 9 Arts 11 Total 20; columns Sports 21 Arts 19 Grand 40. Learner card A sees A-Sports=12, B-Arts=11, group totals20 and grand total40; learner card B sees column totals Sports21,Arts19, and the location of missing cells. Learners request the information needed to reconstruct the four cells, switch questioner/information roles, and check row/column/grand totals.',
'V12 partner task must have real two separate visible information cards (not a single displayed answer table) before claiming an information gap. Teacher assesses precisely framed questions, categorical-variable distinction, filled table, and interpretation such as "nine Group B participants chose Sports"; count entries are frequencies, not continuous measurements. Source student activities also include real-world category table reconstruction and a separate animal-trait table; the Edulytiks numbers are independent.',
'Teacher draft assessment 0–2 each: categorical row/column labels; correct four interior counts and marginals; genuinely differentiated partner information/questions; contextual interpretation and explained checks. An answer-key table on its own is not proof of partner interaction.'
],
visual:V('Correct full two-way table — teacher-only check','The full solution must not be shown to an active learner before the actual information-gap task; requires separate learner A/B cards.',[['Group A','Sports 12 | Arts 8 | Total 20'],['Group B','Sports 9 | Arts 11 | Total 20'],['Column totals','Sports 21 | Arts 19 | Grand 40']]),
criteria:['Correct two categorical variables and table headings','Correct four interior and all marginal counts','Independent actual information-gap questions and role change','Clear count interpretation and summation verification'],
practice:['two-way missing-cell inference with row and column marginals','cell frequency interpretation with categorical units','partner role/cards and teacher observation; HUMAN_TEACHER_ONLY'],
risk:'ACTUAL_TWO_ROLE_INFORMATION_GAP_NOT_IMPLEMENTED'},
{id:'U04:L01',goal:'Distinguish functions and nonfunctions from contextual situations, select inputs precisely, interpret graph coordinates and multiple defensible pricing models.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/4/1/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/4/1/index.html',
paragraphs:[
'V12 source-equivalent first-party bundle-price discussion: a bakery sells individual rolls for $1.60, a six-pack for $7.20 and a twelve-pack for $12.80. For exactly 13 rolls, different purchase plans yield different totals: 13 singles cost $20.80; two six-packs and one single cost $16.00; a twelve-pack plus one single costs $14.40; one six-pack plus seven singles costs $18.40. Each plan is feasible. Students defend one interpretation and critique why input "number of rolls" alone does NOT select a unique total without a purchase-policy input; under cheapest-price policy there is exactly one price for each integer quantity.',
'V12 function and graph task: label an actual input-output table for bike hours h=0,1,2,3 and cost P(h)=4+3h, then draw its points. In a second plotted relation, include (2,7) and (2,9) at the same input to show why the vertical line at x=2 intersects twice. Explain the difference between one output per fully specified input and a single y-value repeated for different inputs. Interpret x and y coordinates and state relevant whole-hour domain.',
'Publisher student lesson uses alternative bagel purchase plans and graph narratives including motion of a leashed animal; independently invented examples preserve the key ambiguity/function-property comparison without source pixels. Draft teacher rubric0–2 each for graph and axes, uniqueness argument, contextual model comparison and independent written explanation.'
],
visual:V('Four different 13-roll purchase plans','The purchase strategy, not just quantity, is needed to identify the total in this fictional price list.',[['13 singles','$20.80'],['2 six-packs + 1 single','$16.00'],['12-pack + 1 single','$14.40'],['6-pack + 7 singles','$18.40']]),
criteria:['Functions vs nonfunctions with justified input definition','Accurately graphed points and coordinate interpretations','Four checked purchase plans and pricing-policy caveat','Explains real-world domain and counterexample'],
practice:['function table input uniqueness','function/nonfunction graph classification','contextual price-plan interpretation; HUMAN_TEACHER_ONLY'],
risk:'GRAPH_DRAWING_AND_AMBIGUOUS_PRICE_DISCUSSION_UNVERIFIED'},
{id:'U05:L01',goal:'Use calculations, graphs and optional technology to compare repeated addition and multiplication before formal linear/exponential vocabulary.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/5/1/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/5/1/index.html',
paragraphs:[
'V12 source-aligned original tripling sequence for the activity paralleling bacteria reproduction: start with ONE fictional microscopic object and triple each step. Counts at steps0,1,2,3,4 are1,3,9,27,81. Students draw the objects for steps0 through3, write repeated multiplication 3×3×3=27, and explain why "add three" cannot produce the same sequence. This is not a verified physical observation.',
'V12 original comparison activity: construct a table for the existing fictional reward schemes A(t)=100+30t and B(t)=10×2^t at whole weeks0..6. On the same labeled axes plot both curves, identify week5 as the first integer week when B>A, and explain why constant additive differences and constant multiplicative factors lead to different predictions. Students may independently use a scientific calculator or spreadsheet but must record actual tool use; no claim of a performed spreadsheet interaction appears in this lesson.',
'Draft teacher criteria0–2 each for correct diagrams/3^t counts, both exact schemes through week6, genuine two-curve plotted graph with correct intersection ordering, and justified comparisons plus conditional technology report. Graph reading and learner-generated plotting remain unverified.'
],
visual:V('Tripling sequence and competing original offers','Counts are independent hypothetical values; original scheme A increases by 30 while B doubles.',[['tripling steps 0–4','1, 3, 9, 27, 81'],['A at weeks 4,5,6','220, 250, 280'],['B at weeks 4,5,6','160, 320, 640']]),
criteria:['Correct original repeated-multiplication diagrams','Valid linear/exponential tables and crossover week','Student-plotted comparison graph with scales','Comparison reasoning and truthful technology log'],
practice:['repeated-factor tables and exponent representation','additive vs multiplicative comparison','hand-drawn graph/tool workflow; HUMAN_TEACHER_ONLY'],
risk:'ACTUAL_TWO_PATTERN_GRAPH_AND_TECHNOLOGY_EVIDENCE_UNVERIFIED'},
{id:'U06:L01',goal:'Describe increasing-then-decreasing area patterns with drawings, tables and graphs before mandatory symbolic quadratic formalism.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/6/1/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/6/1/index.html',
paragraphs:[
'V12 source-equivalent Notice and Wonder THREE original tables, at integer n=0,1,2,3,4: L(n)=7n+2 has outputs2,9,16,23,30 (constant +7); E(n)=2×3^n has2,6,18,54,162 (constant factor3); Q(n)=12−(n−2)^2 has8,11,12,11,8 (rises then falls). Learners discuss in words and visuals before being required to use function notation or the term quadratic; those expressions are provided as optional teacher checks, not compulsory early student solutions.',
'V12 Measurement and Plotting Garden: using the independently chosen 24-metre perimeter, draw rectangles with widths 1,2,3,4,5,6,7,8,9,10,11 metres, each length12−w metres. Record area w(12−w) and graph width vs area on labeled axes (area in m²). From 11,20,27,32,35,36,35,32,27,20,11 identify the maximum 36 m² at 6×6m. Explain the rise-and-fall verbally, including why equal widths/length swaps give equal area.',
'The publisher student work explicitly has a three-table Notice and Wonder, diagrams of an enclosed garden and plotting garden area. The task is only complete after students prepare the actual diagrams and graphs; a table or provided SVG does not count as authentic student production. Teacher rubric0–2 for three table observations, measured rectangle drawings, area graph and qualitative maximum/comparison.'
],
visual:V('Three independent patterns for Notice and Wonder','Students should graph or discuss the three patterns before using formal algebraic names.',[['linear L(n), n=0..4','2,9,16,23,30'],['multiplicative E(n), n=0..4','2,6,18,54,162'],['rise/fall Q(n), n=0..4','8,11,12,11,8']]),
criteria:['Three-table notice/wonder comparisons','Original garden sketches with side lengths and fixed perimeter','Correctly labeled area-vs-width graph and maximum','Qualitative rise/fall compared to linear/exponential'],
practice:['geometric width/length and maximum-area reasoning','classification from equal-input numerical patterns','student garden sketches and actual graphs; HUMAN_TEACHER_ONLY'],
risk:'ACTUAL_GARDEN_DRAWINGS_AND_INCREASING_DECREASING_GRAPH_UNVERIFIED'},
{id:'U07:L01',goal:'Write equations from geometric constraints and interpret unknown inputs in a contextual quadratic, without expecting a fully mastered solving algorithm at this stage.',
teacher:'https://im.kendallhunt.com/HS/teachers/1/7/1/preparation.html',
student:'https://im.kendallhunt.com/HS/students/1/7/1/index.html',
paragraphs:[
'V12 source-aligned new independently authored GEOMETRIC CONSTRAINT task: a rectangular picture measures8 by5 inches, and a uniform nonoverlapping frame of border thickness x inches surrounds all four edges. The frame material covers 30 square inches. Outer dimensions are (8+2x) by (5+2x), so the necessary area equation is (8+2x)(5+2x)−8×5=30, equivalently4x²+26x=30. A border thickness of x=1 gives outer rectangle10×7 area70, picture area40, and frame area30. Negative x is impossible. Let students propose and explain this equation before completing formal quadratic solving techniques.',
'V12 geometric diagram task: draw an 8×5 inner rectangle centered in a 10×7 outer rectangle for x=1, label four strips of thickness1 and the corner regions; explain why the frame area is outer area minus picture area. Compare with the existing first-party flight example h(t)=16−(t−3)^2, which yields height7 at t0 and6, to explain that the same target output may have more than one input. The publisher uses a launched-object context and a physical framing-cut activity; independent values avoid copying the paper cut-out dimensions.',
'Draft teacher assessment0–2 each for labelled original picture/frame diagram, exact geometric area equation, check at x1 with units/domain, and interpretation of context roots/unknown inputs. This is evidence of an authored task, not of actual cutting, authenticated teacher grading, or a student solution.'
],
visual:V('Picture-frame area model: independent dimensions','Border strips thickness x are included on both sides of each dimension; x=1 produces 30 square inches of material.',[['picture','8×5 = 40 in²'],['outer at x=1','(8+2)×(5+2)=70 in²'],['frame at x=1','70−40=30 in²'],['general frame area','(8+2x)(5+2x)−40=4x²+26x']]),
criteria:['Labelled interior/outer rectangle and uniform border','Correct outer minus inner quadratic area equation','Verified x=1 and valid contextual thickness domain','Interprets unknown inputs and algebraic versus physical roots'],
practice:['frame-area geometric constraints and unknown input equation','quadratic graphical target intersection','cutting/modeling and teacher-reviewed explanation; HUMAN_TEACHER_ONLY'],
risk:'ACTUAL_FRAMING_MANIPULATIVES_AND_TEACHER_GRADING_UNVERIFIED'}
];
const handoffs=[];
for(const c of changes){
 const file=path.join(sourceDir,'us-ccss-hs-alg1-'+c.id.toLowerCase().replace(':','-')+'-2021.rich-lesson-v2.json');
 const doc=JSON.parse(fs.readFileSync(file,'utf8'));
 const lesson=doc.Lessons[0];
 if(doc.Lessons.length!==1||lesson.LessonCode!==P+c.id)throw Error('wrong lesson identity '+c.id);
 if(doc.ContentVersion.includes('v12'))throw Error('V12 applied twice '+c.id);
 lesson.ExplanationParagraphs.push(...c.paragraphs);
 lesson.Visuals.push(c.visual);
 doc.ContentVersion='alg1-'+c.id.toLowerCase().replace(':','-')+'-v12-source-activities-reviewed-20261029';
 fs.writeFileSync(file,JSON.stringify(doc,null,2)+'\n','utf8');
 handoffs.push({
  lessonCode:P+c.id,teacherSourceUrl:c.teacher,studentSourceUrl:c.student,
  teachingGoal:c.goal,independentlyAuthoredStudentTask:c.paragraphs,
  proposedTeacherRubric:{status:'DRAFT_HUMAN_GRADE_NOT_IMPLEMENTED',criteria:c.criteria.map((title,i)=>({
   id:'C'+(i+1),name:title,min:0,max:2,
   anchor0:'No adequate evidence',anchor1:'Partly supported but substantial missing evidence',
   anchor2:'Correct task-specific student work independently verified by assigned teacher'})),
   totalPossible:8,scorePersisted:false,teacherRoleVerified:false,feedbackPersisted:false
  },
  requiredPracticeFamilies:c.practice,
  exactApprovedPracticeContract:false,integratedPracticeEngineModified:false,
  safetyGate:'V10 ALG1 PRACTICE_FAIL_CLOSED',
  integrationBlocker:c.risk,
  academicAccepted:false,authenticatedStudentTaskVerified:false
 });
}
const out=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V12_FIRST_SEVEN_SOURCE_TASK_CORRECTIONS_20261029.json');
fs.writeFileSync(out,JSON.stringify({
 schemaVersion:1,asOfDate:'2026-10-29',courseKey:'US-CCSS-MATH:HS:TRAD:ALG1:IM-1E',
 classification:'SOURCE_ALIGNED_ORIGINAL_ACTIVITY_CANDIDATES_NOT_ACADEMIC_APPROVAL',
 fixes:handoffs.length,
 lessonCodes:handoffs.map(x=>x.lessonCode),approvedAcademicTasks:0,
 teacherScoresPersisted:0,practiceContractsVerified:0,
 lessons:handoffs
},null,2)+'\n','utf8');
console.log(JSON.stringify({fixed:handoffs.length,codes:handoffs.map(x=>x.lessonCode),artifact:path.relative(root,out)}));
