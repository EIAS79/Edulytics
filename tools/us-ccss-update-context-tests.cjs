const fs=require("fs"),path=require("path"),r=path.resolve(__dirname,"..");
const f=path.join(r,"tests/Edulytics.Tests/MathematicsIntelligence/UsCommonCoreAcademicContentClosureTests.cs");let s=fs.readFileSync(f,'utf8');
const checks=[
['        "usccss.vector.resultant_angle"\n','        "usccss.vector.resultant_angle",\n        "usccss.vector.head_tail_endpoint_y",\n        "usccss.vector.scalar_direction_class"\n'],
['        "usccss.probability.compare_expected_cost"\n','        "usccss.probability.compare_expected_cost",\n        "usccss.probability.expected_vs_worst_case"\n'],
['Assert.Equal(8 * 3 * 12, checkedCount);','Assert.Equal(11 * 3 * 12, checkedCount);']
];
for(const [before,after] of checks){if(s.split(before).length!==2)throw Error("Unexpected academic test baseline "+before);s=s.replace(before,after)}
fs.writeFileSync(f,s);
const g=path.join(r,"tests/Edulytics.Tests/MathematicsIntelligence/UsCommonCoreRemainingClauseTargetedTests.cs");
s=fs.readFileSync(g,"utf8");const old='"usccss.hs.rational_horizontal_asymptote"]',neu='"usccss.hs.rational_horizontal_asymptote", "usccss.hs.rational_hole_or_pole", "usccss.hs.rational_branch_limit_sign"]';
if(s.split(old).length!==2)throw Error("Unexpected rational routing baseline");s=s.replace(old,neu);fs.writeFileSync(g,s);
console.log("Updated both existing lesson exact-route tests.");
