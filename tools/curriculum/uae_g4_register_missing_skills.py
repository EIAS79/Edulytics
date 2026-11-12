#!/usr/bin/env python3
"""Register source-linked UAE Grade 4 skill IDs referenced by authored question families.
Does not mark D verification or enable production routing.
"""
import collections
import importlib.util
import json
import pathlib
import re

ROOT=pathlib.Path(__file__).resolve().parents[2]
SKILLS=ROOT/'src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json'
FAMILIES=ROOT/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
BLUEPRINT=ROOT/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
AUDIT=ROOT/'tools/math_intelligence/math_registry_integrity.py'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def main():
 skills=load(SKILLS)
 families=load(FAMILIES)
 blue=load(BLUEPRINT)
 present={v['id'] for v in skills['skills']}
 definitions=collections.defaultdict(list)
 for f in families['families']:
  if f['skillId'] not in present:
   if not f['id'].startswith('uae.g4.'):
    raise ValueError('Unrecognized unknown SkillId in non-G4 family: '+f['id'])
   definitions[f['skillId']].append(f)
 assert len(definitions)==72,(len(definitions),list(definitions))
 lessons={v['LessonCode']:v['Title'] for v in blue['Lessons']}
 new=[]
 for skill_id in sorted(definitions):
  rows=definitions[skill_id]
  title_candidates=set()
  for f in rows:
   m=re.match(r'uae\.g4\.u(\d+)\.l(\d+)\.',f['id'])
   if not m:raise ValueError('Unrecognized Grade 4 family code '+f['id'])
   code=f"PED:UAE-MOE-MATH:L4:COMMON:{int(m.group(1)):02}:{int(m.group(2)):02}"
   title=lessons.get(code)
   if not title:raise ValueError('Family missing canonical lesson '+code)
   title_candidates.add(title)
  capabilities=sorted({cap for f in rows for cap in f.get('requiredCapabilities',[])})
  if not capabilities:raise ValueError('Missing verified capability '+skill_id)
  title=' / '.join(sorted(title_candidates))
  new.append({
   'id':skill_id,'domain':skill_id.split('.')[0],
   'canonicalName':title,
   'v2Status':'StageCAuthoredPendingD',
   'v2Capabilities':capabilities,
   'v2QuestionFamilies':sorted(f['id'] for f in rows)
  })
 skills['skills'].extend(new)
 original=SKILLS.read_bytes()
 try:
  SKILLS.write_text(json.dumps(skills,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
  s=importlib.util.spec_from_file_location('uae_registry_audit',AUDIT)
  module=importlib.util.module_from_spec(s)
  s.loader.exec_module(module)
  result=module.audit()
  print('G4 missing skills registered:',len(new),flush=True)
  print('Remaining registry errors:',result['summary']['blockerCount'],flush=True)
  if result['blockers']:
   print('First errors:',result['blockers'][:6],flush=True)
   raise RuntimeError('Registry integrity still fails')
 except Exception:
  SKILLS.write_bytes(original)
  raise
 print('SUCCESS UAE G4 skill-registry linked to families and original lesson titles',flush=True)
if __name__=='__main__':main()
