const fs=require('fs'),path=require('path'),root=path.resolve(__dirname,'..');
const specs=[
['usccss.hs.rational_hole_or_pole','UsRationalHoleOrPole','(p["ask"]==p["removed"]?1:2)','supporting.algebra.equations'],
['usccss.hs.rational_branch_limit_sign','UsRationalApproachDirection','(((p["numerator"]>0)==(p["right"]==1))?1:2)','supporting.algebra.equations'],
['usccss.vector.head_tail_endpoint_y','UsVectorHeadToTailEndpoint','(p["startY"]+p["ay"]+p["by"])','supporting.vectors.core'],
['usccss.vector.scalar_direction_class','UsVectorScaleDirection','(p["k"]>0?1:p["k"]<0?2:0)','supporting.vectors.core'],
['usccss.probability.expected_vs_worst_case','UsExpectedVsRiskDecision','(p["objective"]==0?(p["baseCost"]+p["extra"]/p["denom"]<p["fixedCost"]?2:1):(p["fixedCost"]<p["baseCost"]+p["extra"]?1:2))','supporting.probability_statistics.core']
];
const cf=path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs');
let code=fs.readFileSync(cf,'utf8');
const anchors=[
['            "usccss.hs.rational_horizontal_asymptote",',specs.map(x=>'            "'+x[0]+'",').join('\n')],
['            "usccss.hs.rational_horizontal_asymptote" => UsRationalHorizontal(random, scale),',specs.map(x=>'            "'+x[0]+'" => '+x[1]+'(random, scale),').join('\n')],
['            "usccss.hs.rational_horizontal_asymptote" => (p["k"]).ToString(CultureInfo.InvariantCulture),',specs.map(x=>'            "'+x[0]+'" => '+x[2]+'.ToString(CultureInfo.InvariantCulture),').join('\n')]];
for(const [anchor,additional] of anchors){if(code.split(anchor).length!==2)throw Error('Family anchor not unique: '+anchor);code=code.replace(anchor,anchor+'\n'+additional)}
fs.writeFileSync(cf,code);
const f=path.join(root,'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'),registry=JSON.parse(fs.readFileSync(f));
for(const [id,,,skill] of specs){if(registry.families.some(x=>x.id===id))throw Error('duplicate family '+id);registry.families.push({id,version:1,skillId:skill,requiredCapabilities:['lesson.practice.exact.generate','lesson.practice.exact.verify'],answerType:'contract_defined',verificationPolicy:'independent-recomputation-from-original-question-parameters',representations:['symbolic','context'],status:'ShadowVerified',productionRouting:false,lessonPracticeRouting:true})}
fs.writeFileSync(f,JSON.stringify(registry,null,2)+'\n');
const mf=path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'),map=JSON.parse(fs.readFileSync(mf));
const changes=[
['PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17',specs.slice(0,2).map(x=>x[0])],
['PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01',specs.slice(2,4).map(x=>x[0])],
['PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05',[specs[4][0]]]];
for(const [lesson,families] of changes){const m=map.mappings.find(x=>x.lessonCode===lesson);if(!m||m.sourceType!=='ReviewedPublisherLessonExactPractice')throw Error('expected reviewed lesson missing '+lesson);for(const id of families){if(m.allowedQuestionFamilies.includes(id))throw Error('duplicate route '+id);m.allowedQuestionFamilies.push(id)}}
fs.writeFileSync(mf,JSON.stringify(map,null,2)+'\n');
const countfile=path.join(root,'tests/Edulytics.Tests/MathematicsIntelligence/LessonPracticeInteractionRegistryTests.cs');
let c=fs.readFileSync(countfile,'utf8');if(!c.includes('            282,\n'))throw Error('Interaction cardinality changed; review required');fs.writeFileSync(countfile,c.replace('            282,\n','            287,\n'));
console.log('ADDED 5 contextual families; registry',registry.families.length,'lesson routes',changes.length);
