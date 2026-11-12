#!/usr/bin/env python3
"""Stage D audit: map only Grade 4 Stage C exact skills with matching family ownership.
No automatic mapping for unknown or cross-skill contract IDs.
"""
import collections
import importlib
import json
import pathlib
import re
import sys
ROOT=pathlib.Path(__file__).resolve().parents[2]
sys.path.insert(0,str(ROOT/'tools/math_intelligence'))
RES=importlib.import_module('lesson_skill_resolution_audit')
MAP=ROOT/'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'
SKILLS=ROOT/'src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json'
REG=ROOT/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
CON=ROOT/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
G=ROOT/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def main():
 before=RES.audit()
 marker='Supporting Practice target rule missing for '
 targets=[x[len(marker):].split(': ',1)[0] for x in before['blockers'] if x.startswith(marker)]
 assert len(targets)==59,len(targets)
 assert all(x.startswith('PED:UAE-MOE-MATH:L4:COMMON:') for x in targets)
 maps=load(MAP)
 known_skills={x['id'] for x in load(SKILLS)['skills']}
 owner={x['id']:x['skillId'] for x in load(REG)['families']}
 text=CON.read_text(encoding='utf-8-sig')
 existing={x['lessonCode'] for x in maps['mappings']}
 added,blocked=[],[]
 for code in targets:
  assert code not in existing
  match=re.search(
   r'byLesson\["'+re.escape(code)+r'"\]\s*=\s*new LessonPracticeContract\(\s*'
   r'"'+re.escape(code)+r'",\s*"([^"]+)",\s*"([^"]+)",\s*\[([^\]]+)\]',text,re.S)
  if not match:raise RuntimeError('No contract '+code)
  skill,strategy,part=match.groups()
  families=re.findall(r'"([^"]+)"',part)
  if not families or skill not in known_skills or any(owner.get(f)!=skill for f in families):
   blocked.append({"lessonCode":code,"skillId":skill,"familyOwnership":[owner.get(f) for f in families]})
   continue
  checkpoints=sorted(G.glob('lesson-*-stage-a-c-checkpoint.json'))
  cp=next((load(p) for p in checkpoints if load(p).get('LessonCode')==code),None)
  assert cp and cp['Status']=='READY_FOR_STAGE_D'
  maps['mappings'].append({
   'lessonCode':code,'primarySkills':[skill],
   'sourceType':'OwnerSelectedUaeGrade4StudentBook',
   'officialOutcomeMapped':False,'mappingConfidence':'High',
   'evidence':[
    'Source-scoped Grade4 A-B-C READY checkpoint for '+code,
    'Existing exact-skill family ownership in question-family registry',
    'Stage C authored Practice contract, not Stage D authenticated attempt proof'],
   'practiceReadiness':'READY_VERIFIED','practiceMechanic':strategy,
   'allowedQuestionFamilies':families,'officialOutcomeCodes':[],
   'stageDVerification':'PENDING'
  })
  added.append(code)
 previous=MAP.read_bytes()
 try:
  MAP.write_text(json.dumps(maps,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
  after=RES.audit()
  remaining=[x for x in after['blockers'] if x.startswith(marker)]
  print('TARGETED ORIGINAL',len(targets),'MAPPED VALID',len(added),
        'LEFT FOR SOURCE REVIEW',len(remaining),flush=True)
  assert len(remaining)==len(targets)-len(added),(len(remaining),len(added))
  assert set(x[len(marker):].split(': ',1)[0] for x in remaining)==set(x['lessonCode'] for x in blocked)
  (G/'grade4-partial-exact-skill-mapping-audit.json').write_text(
   json.dumps({'status':'D_PARTIAL_STATIC_SKILL_MAPPING',
    'verifiedStageCLinksAdded':len(added),'stillNeedsSourceReview':blocked,
    'stageDAuthenticatedVerification':'NOT_RUN'},
    ensure_ascii=False,indent=2)+'\n',encoding='utf8')
 except Exception:
  MAP.write_bytes(previous)
  raise
if __name__=='__main__':main()
