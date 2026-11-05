
'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..'),dir='docs/curriculum/';
const read=x=>JSON.parse(fs.readFileSync(path.join(root,dir+x),'utf8'));
const write=(x,v)=>fs.writeFileSync(path.join(root,dir+x),JSON.stringify(v,null,2)+'\n');
const prev=read('us-ccss-g8-coder4-p0-u02-implementation-ledger-20261030.json');
const priorAudit=read('us-ccss-g8-coder4-p0-u02-source-practice-teacher-audit-20261030.json');
const u03=read('us-ccss-g8-coder4-p0-u03-l12-source-graph-contract-20261030.json');
const same=prev.rows.find(r=>r.lessonCode===u03.lessonCode);
if(!same)throw Error('Missing U03:L12');
if(same.verifiedCourseOwnedSvgFile!== '/curriculum/us-ccss-g8/figures/u03-l12.svg')throw Error('Prior original SVG unexpected');
const rowUpdate=r=>{
 if(r.lessonCode!==u03.lessonCode)return r;
 return {...r,
  additionalCourseOwnedSvgFiles:[...(r.additionalCourseOwnedSvgFiles||[]),{path:u03.courseSvg.assetPath,sha256:u03.courseSvg.sha256}],
  p0U03SourceEvidence:'docs/curriculum/us-ccss-g8-coder4-p0-u03-l12-source-graph-contract-20261030.json',
  p0U03ReviewLevel:'PUBLIC_STUDENT_AND_TEACHER_ACTIVITY_SPOT_REVIEW_NOT_FULL_PARITY',
  svgHasIndependentMathUnitChecks:true,
  nextChecks:[...new Set(['U03L12_REAL_GRAPH_CONSTRUCTION_NOT_PROVED_BY_ORDERED_PAIR_FAMILY','U03L12_HUMAN_TEACHER_SCORE_AND_DOMAIN_RESTRICTION_UNVERIFIED',...r.nextChecks])]};
};
const ledger={...prev,purpose:'P0 combined U02 13 lessons and U03 L12 one source-specific lesson, 5 genuinely new course SVG candidates, 131 original lessons and all prior 36 SVG retained',
 limitations:'These are 14 additional student page reviews and seven teacher pages inspected; prior U01 17 have spot reviews. Full activity source parity, teacher cooldown assessments, authenticated rendered diagrams, actual Practice and production/fixture Stage D are NOT proven.',
 counts:{...prev.counts,assets:41,sourceActivitySpotReviewedLessons:31,authenticatedSvgRenderedLessons:0,AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,release:'NO_GO'},
 rows:prev.rows.map(rowUpdate)};
const audit={...priorAudit,scope:'P0 U01 17 + U02 13 student-page reviews + U03 L12 one student-and-teacher source page review = 31 partial source lesson spot-checks; 131 source registry/Practice/teacher gap rows unchanged',
 counts:{...priorAudit.counts,sourceActivitySpotReviewedLessons:31,candidateSvgAssets:41},
 unitSummary:{...priorAudit.unitSummary,U03:{...priorAudit.unitSummary.U03,sourceStudentLessonPagesCheckedInP0:1,sourceTeacherLessonPagesCheckedInP0:1}},
 rows:priorAudit.rows.map(x=>x.lessonCode!==u03.lessonCode?x:{
  ...x,reviewScope:'U03 L12 first-party graph and contextual math source/teacher spot-review, not full acceptance',
  p0SourceReview:'docs/curriculum/us-ccss-g8-coder4-p0-u03-l12-source-graph-contract-20261030.json',
  visuals:{...x.visuals,additionalCandidateSvg:u03.courseSvg.assetPath}
 })};
if(ledger.rows.length!==131||audit.rows.length!==131)throw Error('Lost Grade 8 lesson');
if(ledger.rows.some(r=>r.A!=='NOT_ACCEPTED'||r.B!=='NOT_ACCEPTED'||r.C!=='NOT_VERIFIED'||r.D!=='NOT_VERIFIED'))throw Error('Premature approval');
write('us-ccss-g8-coder4-p0-cumulative-execution-ledger-20261030.json',ledger);
write('us-ccss-g8-coder4-p0-cumulative-source-practice-teacher-audit-20261030.json',audit);
console.log('P0 Grade8 current: lessons 131, assets 41, partial source spot-check 31, priority 37 / 113, A/B/C/D zero, NO_GO');
