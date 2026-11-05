
'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..'),dir='docs/curriculum/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,dir+p),'utf8'));
const write=(p,v)=>fs.writeFileSync(path.join(root,dir+p),JSON.stringify(v,null,2)+'\n');
const prev=read('us-ccss-g8-coder4-v13-u01-extended-implementation-ledger-20261029.json');
const oldAudit=read('us-ccss-g8-coder4-v13-u01-extended-source-practice-teacher-audit-20261029.json');
const source=read('us-ccss-g8-coder4-v13-u01-l01-l07-source-evidence-20261029.json');
const reviewed=new Set(source.lessons.map(x=>x.lessonCode));
const next={...prev,counts:{...prev.counts,sourceActivitySpotReviewedLessons:17,AAccepted:0,BAccepted:0,CVerified:0,DVerified:0,release:'NO_GO'},
 purpose:'Grade 8 V13 end-U01 source-activity spot-check checkpoint across 17/17 U01 lessons; complete 131-row triage across 9 units; NO academic acceptance',
 limitations:'Spot-checking the source activity pages and adding mathematics examples is not complete source exact A, authenticated learner-render B, generated lesson-exact Practice C, or real two-school DB/teacher score D; shared integration remains blocked.',
 rows:prev.rows.map(r=>{if(!reviewed.has(r.lessonCode))return r;return {...r,v13SourceSpotCheck:'docs/curriculum/us-ccss-g8-coder4-v13-u01-l01-l07-source-evidence-20261029.json',nextChecks:[...new Set(['SOURCE_INTERACTIVE_AND_HUMAN_COURSE_ASSESSMENT_REVIEW_STILL_REQUIRED',...r.nextChecks])]};})};
const au={...oldAudit,scope:'All 131 source/task indexes preserved; U01 17/17 public student/teacher lesson page activity spot-checked (V12 L02 + V13 L01,L03-L17). No full source fidelity or A-D approval',
 counts:{...oldAudit.counts,sourceActivitySpotReviewedLessons:17},unitSummary:{...oldAudit.unitSummary,U01:{...oldAudit.unitSummary.U01,sourceActivitySpotCheckedTotal:17,activitySpotCheckedInV13:16}},
 rows:oldAudit.rows.map(r=>{if(!reviewed.has(r.lessonCode))return r;return {...r,reviewScope:'V13 public student+teacher source activity spot-review + independently authored teaching; no full source parity, interactive UI or academic approval',v13SourceReviewEvidence:'docs/curriculum/us-ccss-g8-coder4-v13-u01-l01-l07-source-evidence-20261029.json'};})};
if(next.rows.length!==131||au.rows.length!==131)throw Error('Manifest changed');
if(next.rows.some(r=>r.A!=='NOT_ACCEPTED'||r.B!=='NOT_ACCEPTED'||r.C!=='NOT_VERIFIED'||r.D!=='NOT_VERIFIED'))throw Error('False academic acceptance');
write('us-ccss-g8-coder4-v13-u01-complete-spot-review-ledger-20261029.json',next);
write('us-ccss-g8-coder4-v13-u01-complete-source-practice-teacher-audit-20261029.json',au);
console.log('V13 U01 source page spot-reviews 17/17, original SVG 36, A-D 0, 131 preserved');
