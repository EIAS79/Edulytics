
'use strict';
const fs=require('fs'),path=require('path');const root=path.resolve(__dirname,'..'),doc='docs/curriculum/';
const get=p=>JSON.parse(fs.readFileSync(path.join(root,doc+p),'utf8'));
const put=(p,v)=>fs.writeFileSync(path.join(root,doc+p),JSON.stringify(v,null,2)+'\n');
const old=get('us-ccss-g8-coder4-v13-implementation-ledger-20261029.json');
const oldAudit=get('us-ccss-g8-coder4-v13-course-source-practice-teacher-audit-20261029.json');
const recent=get('us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json');
const figures=new Map(recent.figures.map(f=>[f.lessonCode,f]));
const reviews=new Map(recent.lessons.map(l=>[l.lessonCode,l]));
const ledger=JSON.parse(JSON.stringify(old));
ledger.purpose='Grade8 V13 extended U01: 10 new source-specific lessons L08-L17, 5 new first-party SVG assets; retained all 131, stages zero';
ledger.limitations='Old U01 L02 V12 and U01 L08-L17 V13 are partial source spot checks not complete academic parity. SVG assets are not authenticated-rendered. No teacher scores, real DB, exact Practice grading or tenant fixture claimed.';
ledger.rows=ledger.rows.map(l=>{
 const row={...l},f=figures.get(l.lessonCode),review=reviews.get(l.lessonCode);
 if(f){
  if(row.verifiedCourseOwnedSvgFile)throw Error('Overwrite prevention '+l.lessonCode);
  row.verifiedCourseOwnedSvgFile=f.assetPath;
  row.verifiedCourseOwnedSvgSha256=f.sha256;
  row.svgHasIndependentMathUnitChecks=true;
  row.nextChecks=row.nextChecks.filter(x=>x!=='SOURCE_FIGURE_ORIGINAL_REPRESENTATION_STILL_REQUIRED_OR_CLASSIFY_REUSE');
  row.nextChecks.unshift('NEW_STATIC_SVG_NOT_AUTHENTICATED_RENDERED','CONSTRUCTION_TEACHER_REVIEW_PENDING');
 }
 if(review)row.v13SourceSpotCheck='docs/curriculum/us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json';
 return row;
});
ledger.counts={...old.counts,assets:36,sourceFigurePriorityWithOriginalAsset:34,sourceFigurePriorityWithoutOriginalAsset:79,sourceActivitySpotReviewedLessons:11,AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,authenticatedSvgRenderedLessons:0,teacherScoredLessons:0,release:'NO_GO'};
const audit=JSON.parse(JSON.stringify(oldAudit));
audit.scope='V13 U01 10 source student/teacher activity spot checks (L08-L17), plus existing V12 L02; all 131 rows from existing source/Practice inventories; NOT complete A-D parity';
audit.rows=audit.rows.map(row=>{
 const f=figures.get(row.lessonCode),rv=reviews.get(row.lessonCode);
 if(f)row.visuals.candidateSvg=f.assetPath;
 if(rv){row.reviewScope='V13 U01 student and teacher source activity spot review with first-party teaching additions; full activity fidelity and human rubric still unverified';row.v13SourceReviewEvidence='docs/curriculum/us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json';}
 return row;
});
audit.unitSummary.U01.candidateSvgLessons=13;
audit.unitSummary.U01.activitySpotCheckedInV13=10;
audit.counts={...audit.counts,sourceActivitySpotReviewedLessons:11,candidateSvgAssets:36,sourceFigurePriorityWithOriginalAsset:34,sourceFigurePriorityWithoutOriginalAsset:79};
for(const row of ledger.rows){
 if(row.A!=='NOT_ACCEPTED'||row.B!=='NOT_ACCEPTED'||row.C!=='NOT_VERIFIED'||row.D!=='NOT_VERIFIED')throw Error('Approval cannot be faked');
}
if(ledger.rows.length!==131||audit.rows.length!==131)throw Error('Missing lessons');
put('us-ccss-g8-coder4-v13-u01-extended-implementation-ledger-20261029.json',ledger);
put('us-ccss-g8-coder4-v13-u01-extended-source-practice-teacher-audit-20261029.json',audit);
console.log('U01 extended current ledger',ledger.counts,'row count',ledger.rows.length);
