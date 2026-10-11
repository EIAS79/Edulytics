// Exact, source-conscious US CCSS academic release gate. DO NOT equate a
// successful numeric solver with complete child-clause graph/visual mastery.
const fs=require('fs'),path=require('path'),root=path.resolve(__dirname,'..');
const mappings=require(path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json')).mappings;
const clauses=require(path.join(root,'docs/curriculum/us-ccss-121-clause-evidence-corrected.json'));
const required=[
'PED:US-CCSS-MATH:G3:U02:L08',
'PED:US-CCSS-MATH:G5:U01:L01',
'PED:US-CCSS-MATH:G6:U08:L17',
'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10',
'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15',
'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16',
'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17',
'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17'
];
const explicit=mappings.filter(x=>x.lessonCode.startsWith('PED:US-CCSS-MATH:')&&x.sourceType==='ReviewedPublisherLessonExactPractice');
if(explicit.length!==35||required.some(c=>!explicit.some(e=>e.lessonCode===c)))throw Error('Required 35 verified source routes unavailable');
if(clauses.summary.allClauses!==121||clauses.summary.withPublisherExactDirectCode!==106||clauses.summary.withoutExactDirectCode!==15)throw Error('121-child clause source evidence drift');
const report={schemaVersion:3,productionReady:false,sourceReferencedLessons:35,targetedNewLessonRoutes:8,sourceEvidence:{childClauses:121,exactPublisherCodeReferences:106,withoutExactPublisherChildCodeReference:15},numericPracticeFamiliesAdded:11,remainingReleaseBlockers:['Exact source child references missing for 15 clauses; 8 parent-standard lesson contexts received scoped numeric Practice fixes','HSF-BF.A.1.c function composition and HSF-BF.B.4.d restricting inverse domains need source-linked learner content, not just an unused correct numeric family','HSF-IF.C.7.d graph shape, asymptotes and end behavior require visual answer assessment beyond numeric parameters','Diagram-based vector standards and explanatory probability decision-making need their own semantic and graphical graders','No authenticated student/teacher end-to-end sessions yet; explicitly deferred by user'],notice:'Targeted numeric code fixes may be released after CI, but full academic 121-clause certification is not yet proven.'};
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-practice-semantic-blockers.json'),JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({productionReady:false,sourceReferencedLessons:35,childClauses:121,unmatched:15}));