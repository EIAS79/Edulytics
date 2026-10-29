'use strict';
const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..');
const j=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const source=j('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const cross=j('docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const practice=j('docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json');
const led=j('docs/curriculum/us-ccss-g8-coder4-v12-implementation-ledger-20261029.json');
const richDir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const rich=fs.readdirSync(richDir).filter(f=>/^us-ccss-g8-.*rich-lesson-v2.json$/.test(f)).flatMap(f=>j('src/Edulytics.Core/Curriculum/LessonContent/RichV2/'+f).Lessons);
const by=(a,key='lessonCode')=>new Map(a.map(x=>[x[key],x]));
const rows=rich.filter(l=>l.LessonCode.includes(':U01:')).map(l=>{
 const s=by(source.rows).get(l.LessonCode), c=by(cross.rows).get(l.LessonCode),p=by(practice.rows).get(l.LessonCode), le=by(led.rows).get(l.LessonCode);
 return {code:l.LessonCode.split(':').slice(-2).join(':'),title:l.Title,goal:c.sourceStudentGoal,activities:s.activities.map(a=>({title:a.title,alt:a.uniqueMathFigures.map(x=>x.alt),tables:a.tables,interactions:a.publisherInteractiveElementMarkers})),existingFigure:le.verifiedCourseOwnedSvgFile,richVisuals:l.Visuals.map(x=>x.Title),worked:l.WorkedExamples.map(x=>x.Question),sourceRoles:c.sourceRoles,extraRoles:c.blueprintExtraRoles,practiceTargets:p.officialOutcomeTargets, teacherRubricCandidate:p.originalTaskCandidateDescription||null};
});
console.log(JSON.stringify(rows,null,2));