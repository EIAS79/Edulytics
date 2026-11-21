'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.join(root,p),'utf8');
const mappings=JSON.parse(read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json')).mappings;
const skills=new Set(JSON.parse(read('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json')).skills.map(s=>s.id));
test('only exact Grade9 General A-C descriptive skills may be classified as nonregistered authoring proposals',()=>{
 const affected=mappings.filter(x=>(x.primarySkills||[]).concat(x.secondarySkills||[]).some(s=>!skills.has(s)));
 assert.ok(affected.length>0);
 assert.ok(affected.every(x=>x.lessonCode.startsWith('PED:UAE-MOE-MATH:L9:GENERAL:')));
 assert.ok(affected.every(x=>x.officialOutcomeMapped===false));
 assert.ok(affected.every(x=>x.sourceType.startsWith('UAE_G9_GENERAL_')));
 assert.ok(affected.every(x=>x.practiceReadiness.startsWith('PREPARED_')||x.practiceReadiness.startsWith('TEACHER_REVIEW_ONLY_')));
 const gate=read('tools/math_intelligence/catalogue_skill_baseline.py');
 for(const strict of [
  'lesson_code.startswith("PED:UAE-MOE-MATH:L9:GENERAL:")',
  'row.get("officialOutcomeMapped") is False',
  'row.get("sourceType") or ""',
  'row.get("practiceReadiness")',
  'AUTHORING_PROPOSAL_NOT_REGISTERED_NOT_RUNTIME_VERIFIED',
  'unregisteredDistinctCandidateSkillCount',
  'unregisteredSkillProposals',
  'mapping_errors.append',
 ]) assert.ok(gate.includes(strict),strict);
 assert.ok(gate.includes('errors.append('),'non-G9 unknown skills must remain blocking');
});
