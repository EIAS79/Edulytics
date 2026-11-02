const fs=require('fs'),path=require('path');const root='C:/Users/khali/Edulytiks-US-CCSS-20261010/src/Edulytics.Core/Mathematics/';
const familyFile=root+'Generation/question-family-registry.v1.json',ruleFile=root+'Curriculum/supporting-practice-target-rules.v1.json',mapFile=root+'Curriculum/lesson-skill-mappings.v1.json';
const defs=[
  {id:'usccss.number.repeating_decimal_digit',skill:'supporting.fractions.multiply_divide'},
  {id:'usccss.number.signed_rate_displacement',skill:'supporting.fractions.multiply_divide'},
  {id:'usccss.algebra.exponent_product_value',skill:'supporting.algebra.expressions'}
];
function save(file,key,add){const obj=JSON.parse(fs.readFileSync(file,'utf8'));for(const row of add){if(obj[key].some(x=>x.id===row.id||x.lessonCode&&x.lessonCode===row.lessonCode))throw Error('Duplicate '+(row.id||row.lessonCode));obj[key].push(row);}fs.writeFileSync(file,JSON.stringify(obj,null,2)+'\n');return obj;}
save(familyFile,'families',defs.map(x=>({id:x.id,version:1,skillId:x.skill,requiredCapabilities:['lesson.practice.exact.generate','lesson.practice.exact.verify'],answerType:'contract_defined',verificationPolicy:'supporting-completion-independent-recompute-from-original-parameters',representations:['symbolic'],status:'ShadowVerified',productionRouting:false,lessonPracticeRouting:true})));
const rules=JSON.parse(fs.readFileSync(ruleFile,'utf8'));for(const [code,add] of [['us-ccss-g7-rational-operations',['usccss.number.repeating_decimal_digit','usccss.number.signed_rate_displacement']],['us-ccss-g6-expression-structure',['usccss.algebra.exponent_product_value']]]){const rule=rules.rules.find(r=>r.id===code);if(!rule)throw Error('Rule missing '+code);rule.families.push(...add);}
fs.writeFileSync(ruleFile,JSON.stringify(rules,null,2)+'\n');
const mappings=JSON.parse(fs.readFileSync(mapFile,'utf8'));
const decimal=mappings.mappings.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G7:U04:L05');if(!decimal)throw Error('No decimal lesson');decimal.allowedQuestionFamilies.push('usccss.number.repeating_decimal_digit');
const base='PED:US-CCSS-MATH:';
const lessonDefs=[
  ['G6:U06:L10','supporting.algebra.expressions','ALGEBRA_DISTRIBUTIVE_PROPERTY',['supporting.algebra.expand','supporting.algebra.simplify']],
  ['G6:U06:L11','supporting.algebra.expressions','ALGEBRA_DISTRIBUTIVE_PROPERTY',['supporting.algebra.expand','supporting.algebra.simplify']],
  ['G6:U06:L15','supporting.algebra.expressions','GRADE6_EXPONENT_PRODUCT',['usccss.algebra.exponent_product_value']],
  ['G6:U06:L19','supporting.functions.core','TABLES_EXPRESSIONS',['supporting.functions.evaluate']],
  ['G7:U05:L08','supporting.fractions.multiply_divide','SIGNED_RATE_DISPLACEMENT',['usccss.number.signed_rate_displacement']]
];
const familyById=new Map(JSON.parse(fs.readFileSync(familyFile)).families.map(f=>[f.id,f]));const inputDir='C:/Users/khali/Edulytiks-US-CCSS-20261010/src/Edulytics.Core/Curriculum/LessonBlueprints/Packs';const lessonByCode=new Map(fs.readdirSync(inputDir).filter(x=>x.startsWith('us-ccss-')&&x.endsWith('.lesson-blueprint.json')).flatMap(f=>JSON.parse(fs.readFileSync(path.join(inputDir,f))).Lessons.map(l=>[l.LessonCode,l])));
for(const [short,skill,mechanic,families] of lessonDefs){
 const code=base+short;if(mappings.mappings.some(x=>x.lessonCode===code))throw Error('Existing exact mapping '+code);const l=lessonByCode.get(code);if(!l)throw Error('Unknown lesson '+code);
 for(const f of families){const fr=familyById.get(f);if(!fr||fr.skillId!==skill)throw Error('Skill mismatch '+f+' '+skill+' actual '+JSON.stringify(fr));}
 const codes=[...(l.OutcomeCodes||[]),...(l.FormalTargets||[]).map(x=>x.OutcomeCode)].filter(Boolean);
 mappings.mappings.push({lessonCode:code,primarySkills:[skill],sourceType:'ReviewedPublisherLessonExactPractice',officialOutcomeMapped:true,mappingConfidence:'High',evidence:['Verified source title: '+l.Title,'Exact publisher teacher preparation: '+l.SourceUrl,'The selected family exercises the central named skill without replacing or fabricating official alignments.'],practiceReadiness:'READY_VERIFIED',practiceMechanic:mechanic,allowedQuestionFamilies:families,officialOutcomeCodes:[...new Set(codes)]});
}
fs.writeFileSync(mapFile,JSON.stringify(mappings,null,2)+'\n');
console.log('Added families',defs.map(x=>x.id));console.log('Added grade-specific lessons',lessonDefs.map(x=>x[0]));