"""Grade 11 Algebra 2 Unit 2 source-scoped A-B-C, one lesson per invocation.
Read-only original local archives; per-lesson source alignment; no tests or CI.
"""
from pathlib import Path
from html.parser import HTMLParser
import datetime,hashlib,json,re,sys
ROOT=Path(__file__).resolve().parents[2]
BASE=Path('artifacts/curriculum-delivery/US-CCSS-MATH/G11')
PACK=Path('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json')
BP=Path('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json')
# Each item is first-party original mathematics, aligned to the exact local lesson goal.
# title, explanation, example question, steps (semicolon delimited), numeric or algebraic answer, cooldown and key.
UNIT_COUNTS=[11,26,19,18,11,19,16]
SPECS_FILE=Path(__file__).with_name('g11_u03_u07_specifications.json')
SPECS_ALL=json.loads(SPECS_FILE.read_text(encoding='utf8'))

class Extract(HTMLParser):
 def __init__(self):super().__init__();self.stack=[];self.rows=[];self.skip=0
 def handle_starttag(self,t,a):
  if t in ('script','style','svg','noscript'):self.skip+=1
  if t in ('h2','h3','h4','p','li','th','td'):self.stack.append((t,[]))
 def handle_endtag(self,t):
  if t in ('script','style','svg','noscript') and self.skip:self.skip-=1
  if self.stack and self.stack[-1][0]==t:
   tag,items=self.stack.pop();x=re.sub(r'\s+',' ',''.join(items)).strip()
   if x:self.rows.append((tag,x))
 def handle_data(self,x):
  if not self.skip and self.stack:self.stack[-1][1].append(x)
def parsed(path):
 p=Extract();p.feed(Path(path).read_text(encoding='utf8',errors='replace'))
 return [(tag,text) for tag,text in p.rows if not text.startswith(('Privacy Policy','© 2019','The Illustrative Mathematics name','Teachers with a valid work email','This book includes public domain'))]
def tasks(rows):
 section='Introduction';out=[]
 for tag,text in rows:
  if tag in ('h2','h3') and (re.match(r'\d+\.\d+:',text) or re.match(r'Problem \d+',text)):section=text
  if section!='Introduction' and tag in ('p','li') and len(text)>18 and not text.startswith('Description:'):
   if any(key in text.lower() for key in ('privacy policy','licensed under','sign in for free access')):continue
   out.append((section,text))
 return out
def write(p,obj):
 file=ROOT/p;file.parent.mkdir(parents=True,exist_ok=True);file.write_text(json.dumps(obj,indent=2,ensure_ascii=False)+'\n',encoding='utf8')
def clipped(s,length=300):
 return s if len(s)<length else s[:length].rsplit(' ',1)[0]+'…'
if __name__=='__main__':
 unit=int(sys.argv[1]);n=int(sys.argv[2]);assert 3<=unit<=7 and 1<=n<=UNIT_COUNTS[unit-1]
 ord=sum(UNIT_COUNTS[:unit-1])+n;started=datetime.datetime.now(datetime.timezone.utc).isoformat()
 code=f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U{unit:02d}:L{n:02d}'
 inv=json.loads((ROOT/BASE/'local-archive-source-inventory-20261106.json').read_text(encoding='utf8'))['lessons'][ord-1]
 assert inv['lessonCode']==code
 for d in inv['documents'].values():assert Path(d['path']).is_file() and hashlib.sha256(Path(d['path']).read_bytes()).hexdigest()==d['sha256']
 bp=json.loads((ROOT/BP).read_text(encoding='utf8'))['Lessons'][ord-1];assert bp['LessonCode']==code
 teacher=parsed(inv['documents']['teacher/preparation.html']['path'])
 goals=[];in_goals=False
 for tag,text in teacher:
  if text=='Learning Goals':in_goals=True;continue
  if in_goals and tag=='h3':break
  if in_goals and tag=='li':goals.append(text)
 activity=tasks(parsed(inv['documents']['student/index.html']['path']))
 practice=tasks(parsed(inv['documents']['student/practice.html']['path']))
 assert goals and len(activity)>=4 and len(practice)>=2,(n,goals,len(activity),len(practice))
 title,explain,q,steps,answer,coolq,coola=SPECS_ALL[str(unit)][str(n)]
 pack=json.loads((ROOT/PACK).read_text(encoding='utf8'))
 src=next(x for x in pack['lessons'] if x['lessonCode']==code)
 trans=next(x for x in src['translations'] if x['cultureCode']=='en')
 # Maintain reliable source-aligned explanation, replace malformed generic template.
 trans['explanation']='Learning targets: '+' '.join(goals)+' '+explain
 trans['keyConceptsAndRules']=explain
 trans['workedExamples']=q+' Steps: '+steps+' Answer: '+answer
 trans['stepByStepSolutions']='First identify the model, its given quantities, and domain. Carry out the demonstrated algebraic reasoning in ordered steps. Compare against a second source context, verify by substitution and explain any visual or contextual constraints.'
 trans['commonMistakes']='Confusing a modeled domain with all real numbers; ignoring factors or denominators and forbidden values; relying only on a plausible graph when algebraic or teacher-reviewed justification is required.'
 trans['quickSummary']=clipped(' '.join(goals)+' '+explain,500)
 write(PACK,pack)
 richp=Path(f'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u{unit:02d}-l{n:02d}.rich-lesson-v2.json')
 bysection={}
 for sec,x in activity:bysection.setdefault(sec,[]).append(x)
 bypractice={}
 for sec,x in practice:bypractice.setdefault(sec,[]).append(x)
 activity_context=[f'Source activity {sec}: '+clipped(' '.join(rows),380)+' Explain the mathematical model, verify the relevant result and have a teacher inspect original diagrams or discussion evidence.' for sec,rows in bysection.items()]
 practice_context=[f'Practice {sec}: '+clipped(' '.join(rows),320)+' Preserve original numbers, constraints and representations in a student practice task.' for sec,rows in bypractice.items()]
 rich={
 'LessonCode':code,'CultureCode':'en','Title':bp['Title'],
 'ExplanationParagraphs':['Source-aligned goals: '+' '.join(goals),explain]+activity_context+practice_context+['Edulytics independent cool-down: '+coolq+' Expected reasoning or answer: '+coola,
 'Stage A-B-C authoring boundary: existing mathematics engines are reused where a proved family supports the actual operation and answer type. Any unmatched explanation, diagram, graph or advanced symbolic manipulation is teacher-reviewed; interactive capabilities are not presumed.'],
 'KeyConcepts':[{'Title':title,'Definition':explain,'Rule':'Apply the exact task conditions, mathematical operation, and permitted domain.','Example':q+' Answer: '+answer},
 {'Title':'Source-context verification','Definition':'A model must be checked against the given quantities and representation.','Rule':'Keep source index, signs, units, factor structure and forbidden inputs.','Example':steps}],
 'WorkedExamples':[{'Title':title,'Question':q,'Method':'Identify relationships and execute the algebraic model with explicit checks.','Steps':steps.split('; '),'Answer':answer,'Check':'Substitute into the original expression and review any constraints.'},
 {'Title':'Independent authored assessment','Question':coolq,'Method':'Use the relevant mathematical rule, then justify why it applies.','Steps':['Identify the source-inspired quantities and domain.','Compute using the correct algebraic rule.','Check the result against original conditions.'],'Answer':coola,'Check':'Teacher checks written justification, diagrams or graph when applicable.'}],
 'CommonMistakes':[{'Mistake':'Applying an algebraic solution at an excluded input.','WhyWrong':'A zero denominator or impossible physical quantity makes the result invalid.','Correction':'State and check the domain before finalising.'},
 {'Mistake':'Guessing a graph shape without source-specific reasoning.','WhyWrong':'Multiplicity, degree, leading sign and context can alter the curve.','Correction':'Explain each relevant feature using algebra, a table or verified source activity.'}],
 'SummaryPoints':[title,explain[:360],'Original source activity and practice task evidence are inventoried; authored explanation and source-specific teacher rubric remain distinct from unrun runtime verification.'],
 'Visuals':[],'Videos':[]}
 write(richp,{'SchemaVersion':2,'PackCode':'US-CCSS-MATH','ContentVersion':f'g11-u{unit:02d}-l{n:02d}-local-archive-20261106','Lessons':[rich]})
 assessp=Path(f'src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u{unit:02d}-l{n:02d}-local-source-20261106.json')
 assess={'LessonCode':code,'LearningGoals':goals,'BlueprintAlignments':bp.get('Alignments',[]),'SourceAuthority':'Local Kendall Hunt archival teacher/student pages; AccessIM v360 excluded',
 'EnginePolicy':'REUSE_IF_MATCHING_ELSE_TEACHER_REVIEW; DO_NOT_PROMOTE_UNVERIFIED','QuestionRouting':'UNPROMOTED_NO_RUNTIME_CLAIM',
 'CoreMathWorkedExample':{'Question':q,'Steps':steps.split('; '),'Answer':answer},
 'TeacherReview':{'Required':True,'ActivitySections':list(bysection),'PracticeSections':list(bypractice),'GradingRubric':['exact algebraic/model fidelity','correct numerical/symbolic value','valid input and units','teacher verifies diagrams, graphs, card work and explanation']},
 'FirstPartyCoolDown':{'Prompt':coolq,'Expected':coola,'Origin':'EDULYTICS_ORIGINAL','Grading':'TEACHER_REVIEW_REQUIRED'},
 'TestsAuthoredNotRun':[f'tools/curriculum/us-ccss-g11-u{unit:02d}-l{n:02d}-authoring-contract.test.cjs'],'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN','RichV2Path':str(richp).replace('\\','/')}
 write(assessp,assess)
 source_elements=[]
 for family,rows in [('activity',activity),('practice',practice)]:
  for k,(sec,text) in enumerate(rows,1):
   source_elements.append({'SourceElementId':f'G11-U{unit:02d}-L{n:02d}-{family.upper()}-{k:03d}',
   'SourceSection':sec,'SourceExcerpt':clipped(text,240),
   'SourceLessonArtifact':str(richp).replace('\\','/'),'Treatment':'SOURCE_SCOPED_AUTHORING_WITH_TEACHER_REVIEW','TestsExecuted':'NOT_RUN'})
 evidp=BASE/f'u{unit:02d}-l{n:02d}-element-coverage.json'
 write(evidp,{'LessonCode':code,'SourceDocumentHashes':{name:doc['sha256'] for name,doc in inv['documents'].items()},'Goals':goals,'ElementCount':len(source_elements),'SourceElements':source_elements,'OriginalSourceRights':'CC_BY_4_EVIDENCE_META','Assets':'source pictures/graphs retained as teacher source context; no claim of new interactive component','TestsExecuted':'NOT_RUN'})
 cpp=BASE/f'u{unit:02d}-l{n:02d}-abc-checkpoint.json';closed=datetime.datetime.now(datetime.timezone.utc).isoformat()
 write(cpp,{'LessonCode':code,'Ordinal':ord,'StartedUtc':started,'ClosedUtc':closed,'Status':'READY_FOR_STAGE_D',
 'StageA':'SOURCE_READ_AND_COMPARED','StageB':'FIRST_PARTY_SOURCE_SCOPED_RICH_V2_AUTHORED','StageC':'ORIGINAL_ASSESSMENT_AND_TEACHER_REVIEW_AUTHORED',
 'NoNewEngineNeeded':'EXACT_FAMILY_REUSE_ONLY_WHEN_MATCHING','SourceElementsMapped':len(source_elements),'AssessmentPath':str(assessp).replace('\\','/'),'SourceEvidencePath':str(evidp).replace('\\','/'),'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN'})
 ledgerp=BASE/'abc-independent-course-ledger.json';ledger=json.loads((ROOT/ledgerp).read_text(encoding='utf8'))
 assert len(ledger['LessonEntries'])==ord-1 and ledger['LessonEntries'][-1]['Ordinal']==ord-1
 ledger['LessonEntries'].append({'Ordinal':ord,'LessonCode':code,'Unit':unit,'Lesson':n,'ABCReadyForD':True,'Status':'READY_FOR_STAGE_D','ClosedUtc':closed,'SourceCheckpoint':str(cpp).replace('\\','/'),'ElementEvidence':str(evidp).replace('\\','/'),'AssessmentPath':str(assessp).replace('\\','/'),'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN'})
 ledger['UpdatedUtc']=closed;write(ledgerp,ledger)
 progressp=BASE/'full-grade-progress.json';progress=json.loads((ROOT/progressp).read_text(encoding='utf8'));assert progress['LessonsReadyForStageD']==ord-1
 next_unit=unit+1 if n==UNIT_COUNTS[unit-1] else unit;next_l=1 if next_unit>unit else n+1
 progress.update({'LessonsReadyForStageD':ord,'LessonsRemaining':120-ord,'LastClosedLesson':code,'FirstOpenLesson':None if unit==7 and n==16 else f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U{next_unit:02d}:L{next_l:02d}','UpdatedUtc':closed})
 write(progressp,progress)
 print('G11_SOURCE_REPAIR_AUTHORED',code,'goals',len(goals),'source_items',len(source_elements),'TESTS_NOT_RUN')
