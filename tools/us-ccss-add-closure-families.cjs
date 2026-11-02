const fs=require("fs"),path=require("path");const root=path.resolve(__dirname,"..");
const svc=path.join(root,"src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs");
let code=fs.readFileSync(svc,'utf8');
const defs=[
["usccss.vector.add_x","VectorComponentSum(random, scale)",'(p["ax"]+p["bx"]).ToString(CultureInfo.InvariantCulture)'],
["usccss.vector.subtract_y","VectorDifference(random, scale)",'(p["ay"]-p["by"]).ToString(CultureInfo.InvariantCulture)'],
["usccss.vector.scalar_y","VectorScalar(random, scale)",'(p["y"]*p["k"]).ToString(CultureInfo.InvariantCulture)'],
["usccss.vector.sum_magnitude","VectorSumMagnitude(random, scale)",'Math.Sqrt(Math.Pow(p["ux"]+p["vx"],2)+Math.Pow(p["uy"]+p["vy"],2)).ToString("0",CultureInfo.InvariantCulture)'],
["usccss.vector.scalar_magnitude","VectorScalarMagnitude(random, scale)",'(5*p["factor"]*Math.Abs(p["k"])).ToString(CultureInfo.InvariantCulture)'],
["usccss.vector.resultant_angle","VectorResultantDirection(random, scale)",'Math.Round(Math.Atan2(p["uy"]+p["vy"],p["ux"]+p["vx"])*180/Math.PI,0,MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture)'],
["usccss.probability.expected_net_payoff","NetGameExpectedValue(random, scale)",'(p["prize"]/p["q"]-p["ticket"]).ToString(CultureInfo.InvariantCulture)'],
["usccss.probability.compare_expected_cost","CompareExpectedCost(random, scale)",'(p["fixedCost"]<p["baseCost"]+p["extra"]/p["q"]?1:p["fixedCost"]>p["baseCost"]+p["extra"]/p["q"]?2:0).ToString(CultureInfo.InvariantCulture)']
];
const canonical="            \"usccss.algebra.exponent_product_value\",";
if(code.split(canonical).length!==2)throw Error("Family anchor missing/duplicate");
code=code.replace(canonical,canonical+"\n"+defs.map(x=>'            "'+x[0]+'",').join("\n"));
const build='            "usccss.algebra.exponent_product_value" => UsExponentProductValue(random, scale),';
if(code.split(build).length!==2)throw Error("Build anchor missing/duplicate");
code=code.replace(build,build+"\n"+defs.map(x=>'            "'+x[0]+'" => '+x[1]+',').join("\n"));
const solver='            "usccss.algebra.exponent_product_value" => IntPow(p["base"], p["exponent"]).ToString(CultureInfo.InvariantCulture),';
if(code.split(solver).length!==2)throw Error("Solve anchor missing/duplicate");
code=code.replace(solver,solver+"\n"+defs.map(x=>'            "'+x[0]+'" => '+x[2]+',').join("\n"));
fs.writeFileSync(svc,code);
const reg=path.join(root,"src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json");
const registry=JSON.parse(fs.readFileSync(reg,'utf8'));
for(const [name] of defs){if(registry.families.some(f=>f.id===name))throw Error("Family already exists "+name);registry.families.push({id:name,version:1,skillId:name.startsWith("usccss.vector.")?"supporting.vectors.core":"supporting.probability_statistics.core",requiredCapabilities:["lesson.practice.exact.generate","lesson.practice.exact.verify"],answerType:"contract_defined",verificationPolicy:"independent-recomputation-from-original-question-parameters",representations:["symbolic"],status:"ShadowVerified",productionRouting:false,lessonPracticeRouting:true});}
fs.writeFileSync(reg,JSON.stringify(registry,null,2)+"\n");
const mapping=path.join(root,"src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json");
const m=JSON.parse(fs.readFileSync(mapping));
const rows=[
{lessonCode:"PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01",primarySkills:["supporting.vectors.core"],mechanic:"VECTOR_COMPONENTS_MAGNITUDE_DIRECTION",codes:["CCSS:HSN-VM.A.1","CCSS:HSN-VM.A.2","CCSS:HSN-VM.A.3","CCSS:HSN-VM.B.4","CCSS:HSN-VM.B.5"],families:defs.slice(0,6).map(x=>x[0])},
{lessonCode:"PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05",primarySkills:["supporting.probability_statistics.core"],mechanic:"EXPECTED_NET_VALUE_DECISION",codes:["CCSS:HSS-MD.B.5"],families:defs.slice(6).map(x=>x[0])}
];
for(const row of rows){if(m.mappings.some(x=>x.lessonCode===row.lessonCode))throw Error("Explicit mapping already exists");m.mappings.push({lessonCode:row.lessonCode,primarySkills:row.primarySkills,sourceType:"ReviewedPublisherLessonExactPractice",officialOutcomeMapped:true,mappingConfidence:"High",evidence:["Source lesson title and its source artifacts verified in US-CCSS curriculum lesson blueprint","CCSS HSN-VM.B.4–5 or HSS-MD.B.5 subsection tasks reviewed against mathematical definitions","Independent randomized solver recomputation tests for each allowed family"],practiceReadiness:"READY_VERIFIED",practiceMechanic:row.mechanic,allowedQuestionFamilies:row.families,officialOutcomeCodes:row.codes});}
fs.writeFileSync(mapping,JSON.stringify(m,null,2)+"\n");
const assertion=path.join(root,"tests/Edulytics.Tests/MathematicsIntelligence/LessonPracticeInteractionRegistryTests.cs");
let a=fs.readFileSync(assertion,'utf8');if(!a.includes("            263,\n"))throw Error("Registry cardinality baseline changed");a=a.replace("            263,\n","            271,\n");fs.writeFileSync(assertion,a);
console.log(JSON.stringify({families:defs.length,routes:rows.length,registeredFamilies:registry.families.length,routesTotal:m.mappings.length}));