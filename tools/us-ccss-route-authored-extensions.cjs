const fs=require("fs"),path=require("path"),root=path.resolve(__dirname,"..");
const file=path.join(root,"src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json");
const obj=JSON.parse(fs.readFileSync(file));
const entries=[
{code:"PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15",family:"usccss.hs.restricted_inverse",clause:"HSF-BF.B.4.d",explain:"Original Edulytics lesson-body extension derives and verifies a quadratic inverse after restricting x>=vertex; publisher did not provide an exact child-code alignment."},
{code:"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10",family:"usccss.hs.function_composition",clause:"HSF-BF.A.1.c",explain:"Original Edulytics supplemental worked examples teach function composition and units beyond publisher lesson's arithmetic-on-functions focus; do not ascribe this subsection to publisher."}
];
const reg=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json")));
for(const e of entries){const m=obj.mappings.find(x=>x.lessonCode===e.code);const f=reg.families.find(x=>x.id===e.family);if(!m||!f||!f.lessonPracticeRouting||m.allowedQuestionFamilies.includes(e.family)||!m.primarySkills.includes(f.skillId))throw Error("Mismatch "+e.code);
m.allowedQuestionFamilies.push(e.family);
m.evidence.push("Edulytics-authored supplemental academic extension for "+e.clause+": "+e.explain);
m.practiceMechanic=m.practiceMechanic+"_PLUS_ORIGINAL_STANDARD_EXTENSION";
console.log("SOURCE-GROUNDED ENRICHMENT "+e.code+" "+m.allowedQuestionFamilies.join(","));}
fs.writeFileSync(file,JSON.stringify(obj,null,2)+"\n");