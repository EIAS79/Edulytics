"""Read-only local source file inventory; reports metadata, never copies publisher HTML."""
from pathlib import Path
from html.parser import HTMLParser
import hashlib,json,re
ROOT=Path(__file__).resolve().parents[2]
BOOKS=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core")
UNIT_COUNTS=(11,26,19,18,11,19,16)
class P(HTMLParser):
 def __init__(self):super().__init__();self.stack=[];self.titles=[];self.h=[];self.images=[];self.external=[];self.skip=0
 def handle_starttag(self,tag,attrs):
  a=dict(attrs)
  if tag in ('script','style','noscript'):self.skip+=1
  if tag in ('h1','h2','h3','h4','title'):self.stack.append([tag,[]])
  if tag in ('img','iframe','video','source') and a.get('src'):self.images.append({'tag':tag,'src':a['src'],'alt':a.get('alt')})
  if tag=='a' and a.get('href','').startswith('http'):self.external.append(a['href'])
 def handle_data(self,data):
  if not self.skip and self.stack:self.stack[-1][1].append(data)
 def handle_endtag(self,tag):
  if self.stack and self.stack[-1][0]==tag:
   t,parts=self.stack.pop();s=re.sub(r'\s+',' ',''.join(parts)).strip()
   if t=='title':self.titles.append(s)
   elif s:self.h.append({'heading':t,'title':s})
  if tag in ('script','style','noscript') and self.skip:self.skip-=1
out={'framework':'US-CCSS-MATH','grade':11,'course':'Algebra 2','units':7,'expectedLessons':120,'edition':'Kendall Hunt archive, 2019-2021 per existing metadata; exact release requires verification','notSource':['AccessIM','v360 public_teacher/unit-8','v360 public_student/unit-8'],'lessons':[]}
ordinal=0
for unit,count in enumerate(UNIT_COUNTS,1):
 for lesson in range(1,count+1):
  ordinal+=1;docs={}
  for kind,folder,names in [('teacher','11_HS_Algebra_2',['preparation.html','index.html','practice.html']),('student','11_HS_Algebra_2_Student',['index.html','practice.html'])]:
   for name in names:
    p=BOOKS/folder/str(unit)/str(lesson)/name
    assert p.is_file(),p
    b=p.read_bytes();parser=P();parser.feed(b.decode('utf8','replace'))
    docs[f'{kind}/{name}']={'path':str(p),'sha256':hashlib.sha256(b).hexdigest(),'bytes':len(b),'headingCount':len(parser.h),'headingLabels':[x['title'][:120] for x in parser.h if x['heading'] in ('h1','h2','h3')][:26],'embeddedSourceCount':len(parser.images),'externalLinkCount':len(parser.external)}
  out['lessons'].append({'ordinal':ordinal,'lessonCode':f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U{unit:02d}:L{lesson:02d}','unit':unit,'lesson':lesson,'documents':docs})
p=ROOT/'artifacts/curriculum-delivery/US-CCSS-MATH/G11/local-archive-source-inventory-20261106.json'
p.parent.mkdir(parents=True,exist_ok=True);p.write_text(json.dumps(out,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
print('G11_READ_ONLY_INVENTORY',len(out['lessons']),'lessons',len(out['lessons'])*5,'pages')
for x in out['lessons'][:11]:
 print('U01',x['lesson'],x['documents']['student/index.html']['headingLabels'][:12])
