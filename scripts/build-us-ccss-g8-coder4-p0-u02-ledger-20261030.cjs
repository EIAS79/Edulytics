
'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..'),base='docs/curriculum/';
const read=x=>JSON.parse(fs.readFileSync(path.join(root,base+x),'utf8'));
const write=(x,v)=>fs.writeFileSync(path.join(root,base+x),JSON.stringify(v,null,2)+'\n');
const prev=read('us-ccss-g8-coder4-v13-u01-complete-spot-review-ledger-20261029.json');
const prevAudit=read('us-ccss-g8-coder4-v13-u01-complete-source-practice-teacher-audit-20261029.json');
const evid=read('us-ccss-g8-coder4-p0-u02-source-fixes-20261030.json');
const figMap=new Map(evid.figures.map(x=>[x.lessonCode,x]));
const sourceMap=new Map(evid.reviewedLessons.map(x=>[x.lessonCode,x]));
const contractTemplates=[
{code:'L01',numerical:'25/8 and 71/35 as mixed-number division results; rectangle A:C:E scale factors 1:1/2:1/4',equivalent:['25/8','3.125','3 1/8'],wrong:'B(5.5,8.5) is not an A(8.5,11) scaled copy even though it came from halving; alternating short/long is not uniform scaling',human:'Student physical rectangle sorting and shared-diagonal explanation'},
{code:'L02',numerical:'Radius 2 to radius 5 gives factor 5/2 for every chosen ray',equivalent:['5/2','2.5'],wrong:'Using a direction-dependent factor is invalid when two concentric circles are dilated',human:'Construct four rays, intersections and measured table on a circular grid'},
{code:'L03',numerical:'A(0,0), B(2,0): factors 2, 1/2, 5 map to (4,0),(1,0),(10,0)',equivalent:['2.0','2'],wrong:'A negative factor in a source positive-ray dilation points the wrong way; factor 0 not a one-to-one similarity',human:'Select center and draw mapped figures on an ungridded plane'},
{code:'L04',numerical:'Center P(3,7), k=2: A(2,9) maps to (1,11), B(8,9) to (13,11), C(8,5) to (13,3), D(2,5) to (1,3)',equivalent:['(1,11)','(1.0,11.0)'],wrong:'Raw doubling A to (4,18) ignores the nonorigin center',human:'Image quadrilateral construction and dilation card-sort justification'},
{code:'L05',numerical:'14-unit side scales to 7 with k=1/2, 28 with k=2; area ratio 1/4 or 4',equivalent:['1/2','0.5'],wrong:'Scaling area by k instead of k^2 is incorrect',human:'Applet exploration and two-person information-gap communication'},
{code:'L06',numerical:'10(2+3)-8*3 = 26; similarity requires an overall uniform scale plus rigid transformations',equivalent:['26','2*13','50-24'],wrong:'Unequal side scale factors cannot prove a single dilation',human:'Corresponding transformed polygons and geometric alignment demonstration'},
{code:'L07',numerical:'Congruence always implies similarity; similarity only sometimes implies congruence; dilation does not change angle',equivalent:['all','always'],wrong:'Rectangles 4x2 and 6x4 do not have a common side ratio (6/4 != 4/2)',human:'Classify all/some/none and criticize competing polygon arguments'},
{code:'L08',numerical:'AA similarity with two equal corresponding angles; one common angle alone is insufficient',equivalent:['AA','angle-angle'],wrong:'One 40-degree angle shared by two triangles cannot establish similarity',human:'Pasta triangles, angle measurements and comparing constructed figures'},
{code:'L09',numerical:'4,5,7 scaled by 2 produces 8,10,14; by 3 produces 12,15,21; by 1/2 produces 2,2.5,3.5',equivalent:['7/4','1.75'],wrong:'(2,3,4) and (4,5,6) are not similar: length factors differ',human:'Partner quotient table and explanation of consistent correspondence'},
{code:'L10',numerical:'Rise/run (3/4,6/8,1.5/2) all yield 0.75',equivalent:['3/4','0.75'],wrong:'Vertical lines have undefined slope; 0 is a horizontal slope',human:'Draw similar slope triangles along the source line and justify similarity'},
{code:'L11',numerical:'Line y=3x/4 contains (20,15), (100,75) and rejects (90,68)',equivalent:['3x/4','0.75x'],wrong:'(90,68) is not on the line because expected y=67.5',human:'Find and label E via similar triangles and derive a general coordinate rule'},
{code:'L12',numerical:'y=2x-7 contains (9,11) and (100,193), rejects (9,10); dilation s=4.5 around (0,1) maps C(2,2) to (9,5.5)',equivalent:['y=2x-7','y-3=2(x-5)'],wrong:'Using y=2x+7 or omitting the dilation center contradicts source coordinate pairs',human:'Draw slope triangles, determine dilation center, construct labeled image'},
{code:'L13',numerical:'Using 72/48=1.5 estimates 114-inch lamppost shadow height as about 171 inches; 43/29 differs slightly',equivalent:['≈171 inches','approximately 171'],wrong:'An exact 171-inch measured height cannot be inferred from rounded photos; nearby point-source shadows need not share slope',human:'Explain similar right triangles and assumptions of parallel sunlight, with uncertainty'}
];
const contracts=contractTemplates.map(t=>({
 lessonCode:'PED:US-CCSS-MATH:G8:U02:'+t.code,contractStatus:'COURSE_OWNED_CANDIDATE_NOT_PRACTICE_GENERATOR_VERIFIED',
 sourceLinkedObjective:sourceMap.get('PED:US-CCSS-MATH:G8:U02:'+t.code).sourceActivities,
 requiredNumericalOrLogicalCheck:t.numerical,
 equivalentAcceptExamples:t.equivalent,
 mustRejectOrFlagWrong:t.wrong,
 malformedMustReject:['wrong lessonCode','non-numeric where numeric expected','NaN/Infinity','missing required value','unexpected JSON structure'],
 boundaryAndEquivalenceCases:'Verify exact rational fractions and decimals, zero denominators in quotients, center equals point in dilation, allowed positive scale ranges, near-boundary approximations when observations rounded.',
 requiredHumanReview:t.human,teacherRubricDraft:'EXISTS_FROM_PRIOR_COURSE_MANIFEST_NOT_RUNTIME_PERSISTED',
 selectedProductionItemFamilyObserved:false,authenticatedAttemptSaved:false,wrongSchoolRejectedInRealDb:false
}));
const newLedger={...prev,counts:{...prev.counts,assets:40,sourceFigurePriorityWithOriginalAsset:37,sourceFigurePriorityWithoutOriginalAsset:76,sourceActivitySpotReviewedLessons:30,AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,authenticatedSvgRenderedLessons:0,teacherScoredLessons:0,release:'NO_GO'},
 purpose:'U02 13 student source lesson activity reviews and source-specific mathematical corrections, 4 candidate first-party SVGs (L04 additionally preserving original), all 131 lesson identities and prior 36 SVGs intact',
 limitations:'U02 public student pages checked; only 6 teacher lesson pages individually reviewed; source role decisions still need independent reviewer; new SVGs are static files not authenticated rendered; no real Practice/teacher score/2-school DB acceptance',
 rows:prev.rows.map(row=>{
  const e=sourceMap.get(row.lessonCode),fig=figMap.get(row.lessonCode);if(!e)return row;
  const out={...row,p0U02SourceEvidence:'docs/curriculum/us-ccss-g8-coder4-p0-u02-source-fixes-20261030.json',
   p0U02ReviewLevel:'STUDENT_ACTIVITY_CONTENT_SPOT_REVIEW_WITH_SOURCE_SPECIFIC_CORRECTION_NOT_COMPLETE_PARITY',
   nextChecks:[...new Set(['U02_TEACHER_SOURCE_ACTIVITY_AND_COOLDOWN_ACCEPTANCE_PENDING','LESSON_EXACT_PRACTICE_AND_HUMAN_SCORE_PENDING',...row.nextChecks])]};
  if(fig){
   if(!out.verifiedCourseOwnedSvgFile){out.verifiedCourseOwnedSvgFile=fig.assetPath;out.verifiedCourseOwnedSvgSha256=fig.sha256;}
   else{out.additionalCourseOwnedSvgFiles=[...(out.additionalCourseOwnedSvgFiles||[]),{path:fig.assetPath,sha256:fig.sha256}];}
   out.svgHasIndependentMathUnitChecks=true;
  }
  return out;
 })};
const nextAudit={...prevAudit,scope:'30 lessons (U01 17, U02 13) student activity source spot-review; teacher for six U02 only. Full Grade 8 131-row existing static source/Practice/teacher-gap audit preserved.',
 unitSummary:{...prevAudit.unitSummary,U02:{...prevAudit.unitSummary.U02,sourceStudentLessonPagesChecked:13,sourceTeacherLessonPagesIndividuallyChecked:6}},
 counts:{...prevAudit.counts,sourceActivitySpotReviewedLessons:30,candidateSvgAssets:40,sourceFigurePriorityWithOriginalAsset:37,sourceFigurePriorityWithoutOriginalAsset:76},
 rows:prevAudit.rows.map(row=>{
 const e=sourceMap.get(row.lessonCode),fig=figMap.get(row.lessonCode);if(!e)return row;
 const visuals={...row.visuals};if(fig){if(visuals.candidateSvg)visuals.additionalCandidateSvg=fig.assetPath;else visuals.candidateSvg=fig.assetPath;}
 return {...row,reviewScope:'P0 U02 student activity source page reviewed, teacher detailed scope 6/13, no certified parity or teacher grade',p0SourceReview:'docs/curriculum/us-ccss-g8-coder4-p0-u02-source-fixes-20261030.json',visuals};
 })};
if(newLedger.rows.length!==131||nextAudit.rows.length!==131||new Set(newLedger.rows.map(x=>x.lessonCode)).size!==131)throw Error('Lesson count change');
if(newLedger.rows.some(x=>x.A!=='NOT_ACCEPTED'||x.B!=='NOT_ACCEPTED'||x.C!=='NOT_VERIFIED'||x.D!=='NOT_VERIFIED'))throw Error('False acceptance');
write('us-ccss-g8-coder4-p0-u02-implementation-ledger-20261030.json',newLedger);
write('us-ccss-g8-coder4-p0-u02-source-practice-teacher-audit-20261030.json',nextAudit);
write('us-ccss-g8-coder4-p0-u02-practice-human-contracts-20261030.json',{
 schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',
 status:'COURSE_OWNED_PROPOSALS_SHARED_GENERATOR_AND_DB_NOT_TESTED',contracts,sharedOwner:'Supervisor 2',sourceMismatchCounts:{U02ExistingRoleFlags:3,staticUnmappedLessonsInU02:2,grade8TotalExistingRoleFlags:22},missingSharedProof:['Actual selected question family and lesson binding','Generator correct/wrong/equivalent/malformed and boundary','Authenticated two-school Postgres attempted, scored, readback','Teacher-only rubric persistence and feedback','Accessible user-built graph/dilation/card interaction, not a static SVG'],release:'NO_GO'
});
console.log(JSON.stringify({rows:newLedger.rows.length,counts:newLedger.counts,unit2:nextAudit.unitSummary.U02,contracts:contracts.length},null,2));
