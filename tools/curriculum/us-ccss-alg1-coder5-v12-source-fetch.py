#!/usr/bin/env python3
"""Independently fetch first-edition publisher teacher/student lesson pages.
Source metadata and compact terms only: do not copy publisher activities.
Fetch success does not equal educator source-task approval.
"""
from html.parser import HTMLParser
from urllib.request import Request,urlopen
from concurrent.futures import ThreadPoolExecutor,as_completed
from collections import Counter
from pathlib import Path
from datetime import date
import hashlib,json,re,time

ROOT=Path(__file__).resolve().parents[2]
PROGRESS=json.loads((ROOT/'docs/curriculum/US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json').read_text(encoding='utf-8'))
OUT=ROOT/'docs/curriculum/US_CCSS_ALG1_CODER5_V12_SOURCE_FETCH_AUDIT_20261029.json'
EXCLUDE={'U06:L11','U06:L16','U07:L24'}

class Extract(HTMLParser):
    def __init__(self):
        super().__init__(convert_charrefs=True)
        self.hlevel=None; self.tag=[];self.words=[];self.headings=[];self.curr_heading=''
    def handle_starttag(self,tag,attrs):
        if tag in ('h1','h2','h3','h4'):
            self.hlevel=tag;self.words=[]
    def handle_data(self,data):
        if self.hlevel: self.words.append(data)
    def handle_endtag(self,tag):
        if tag==self.hlevel:
            val=' '.join(' '.join(self.words).split())
            if val:self.headings.append([tag,val])
            self.hlevel=None;self.words=[]

def get(url):
    r=Request(url,headers={'User-Agent':'EdulytiksCurriculumAudit/1.0 (source lesson metadata; not bulk asset copying)'})
    err=''
    for attempt in range(2):
        try:
            with urlopen(r,timeout=25) as response:
                raw=response.read(850000)
                if response.status!=200: raise ValueError('HTTP '+str(response.status))
            page=raw.decode('utf-8','replace')
            parser=Extract();parser.feed(page)
            return {'status':'HTTP_200','sha256':hashlib.sha256(raw).hexdigest(),
                    'byteLength':len(raw),
                    'headings':parser.headings,
                    'containsLearningGoals':'Learning Goals' in page,
                    'containsCCSS':'CCSS' in page,
                    'containsActivity':'activity' in page.lower()}
        except Exception as ex:
            err=type(ex).__name__+': '+str(ex)[:130]
            time.sleep(.25*(attempt+1))
    return {'status':'FAILED','error':err}

def one(row):
    code=row['lessonCode'];tail=code.rsplit(':',2);unit=row['unitNumber'];lesson=row['lessonNumber']
    teacher=row['officialTeacherPreparationUrl']
    student=f'https://im.kendallhunt.com/HS/students/1/{unit}/{lesson}/index.html'
    a=get(teacher);b=get(student)
    if a['status']=='HTTP_200':
        teacher_headers=[v for _,v in a['headings']]
        label=next((h for h in teacher_headers if h not in ('Lesson Narrative','Learning Goals',
          'Student Facing','Teacher Facing','Required Materials','Preparation','Lesson','Practice',
          'Learning Targets','Glossary Entries','Required Preparation','CCSS Standards','Lesson Summary')
          and not re.match('^Lesson [0-9]+$',h)), '')
    else:label=''
    student_headers=[v for tag,v in b.get('headings',[])
                     if tag=='h3' and re.match(r'^\\d+\\.\\d+:', v)]
    authored=json.loads((ROOT/row['authoredRichV2Sidecar']).read_text(encoding='utf-8'))['Lessons'][0]
    authored_text=' '.join(authored['ExplanationParagraphs']).casefold()
    keywords=[w for w in re.findall('[a-z]{4,}', ' '.join(student_headers)) if w not in
      ('lesson','activity','practice','summary','number','answer','explain','part','more','about','what','with','from','your','some','which','this','that')]
    unique=list(dict.fromkeys(keywords))[:12]
    hits=[w for w in unique if w in authored_text]
    return {'lessonCode':code,'unitNumber':unit,'lessonNumber':lesson,
      'publisherTeacherUrl':teacher,'publisherStudentUrl':student,
      'teacherFetch':{k:v for k,v in a.items() if k!='headings'},
      'studentFetch':{k:v for k,v in b.items() if k!='headings'},
      'publisherLessonTitleDetected':label,
      'studentActivityHeadingCandidates':student_headers[:9],
      'studentActivityHeadingTermMatch':{'sourceTerms':unique,'representedInAuthoredNarrative':hits,
        'missingFromAuthoredNarrative':[w for w in unique if w not in hits]},
      'auditClass':'AUTOMATED_SOURCE_METADATA_COMPARE__AWAITING_HUMAN_ACTIVITY_REVIEW',
      'teacherGoalsIndependentlyVerified':False,
      'studentActivitiesIndependentlyVerified':False,
      'stageAApproved':False}

def main():
    rows=[r for r in PROGRESS['lessons'] if r['lessonCode'].rsplit(':',2)[-2]+':'+r['lessonCode'].rsplit(':',2)[-1] not in EXCLUDE]
    print('AUDIT_REQUESTS',len(rows),'LESSONS',2*len(rows),'URLS',flush=True)
    outputs=[];done=0
    with ThreadPoolExecutor(max_workers=6) as pool:
        fut=[pool.submit(one,row) for row in rows]
        for f in as_completed(fut):
            result=f.result();outputs.append(result);done+=1
            if done%15==0: print('FETCHED',done,'/',len(rows),flush=True)
    outputs.sort(key=lambda v:(v['unitNumber'],v['lessonNumber']))
    counts=Counter((x['teacherFetch']['status'],x['studentFetch']['status']) for x in outputs)
    doc={'schemaVersion':1,'sourceEdition':'IM Algebra 1 First Edition 2019-2021',
         'asOfDate':'2026-10-29','courseKey':PROGRESS['courseKey'],
         'scope':'V12 AUTOMATED BOTH PAGE RETRIEVAL FOR 129 NOT PRIOR V11 SOURCE SPOT CHECKS',
         'copyrightNote':'Stores only source short activity headings, source hashes and extracted metadata. Does not copy full textbook pages.',
         'warning':'Automated retrieval and heading lexical matches do NOT establish teaching-goal, every-activity, worked-example, diagram, or source acceptance.',
         'lessonCount':len(outputs),'fetchStatusCounts':{str(k):v for k,v in counts.items()},
         'teacherApprovedCount':0,'stageAAcceptedCount':0,'rows':outputs}
    OUT.write_text(json.dumps(doc,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print('COMPLETE',json.dumps({'count':len(outputs),'statuses':{str(k):v for k,v in counts.items()},
                 'file':str(OUT)}),flush=True)
if __name__=='__main__':main()
