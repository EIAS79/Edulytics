"""L01 independent authoring oracle; not production grading."""
import json
from pathlib import Path
R=Path(__file__).resolve().parents[2]
c=json.loads((R/'docs/curriculum/US_CCSS_ALG1_CODER5_U01_L01_AUTHORING_C_20261030.json').read_text(encoding='utf8'))
def grade(x):
 if isinstance(x,str):a=x.split(',')
 elif isinstance(x,list):a=x
 else:return 'MALFORMED'
 if not a:return 'MALFORMED'
 z=[]
 for e in a:
  if not isinstance(e,str):return 'MALFORMED'
  y=e.strip().upper()
  if y not in ('A','B','C','D','E','F') or y in z:return 'MALFORMED'
  z.append(y)
 return 'CORRECT' if set(z)=={'A','C','E'} else 'WRONG'
opts=c['activities'][4]['options']
assert [(x['id'],x['statistical']) for x in opts]==[('A',True),('B',False),('C',True),('D',False),('E',True),('F',False)]
assert c['runtimeEnabled'] is False
assert c['originalPublisherCooldown']=='PUBLISHER_COOLDOWN_UNVERIFIED'
assert c['activities'][1]['genuineCollectionClaimed'] is False
for i,maxpts in [(1,8),(5,6)]:
 item=c['activities'][i]
 assert item['possiblePoints']==maxpts
 assert sum(x['points'] for x in item['rubric'])==maxpts
n=0
for kind in ('correct','wrong','malformed'):
 for fixture in c['oracleFixtures'][kind]:
  actual=grade(fixture)
  assert actual==kind.upper(),(kind,fixture,actual)
  n+=1
print('L01_ORACLE_PASS',n,'cases; source choices classified; human 8/6; runtime NOT_RUN')
