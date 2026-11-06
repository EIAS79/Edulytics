"""Targeted and sequential source review for G11 Algebra 2 U01:L05-L11.
One invocation processes exactly ONE specified lesson. Does not run tests/CI.
Writes original curricular explanation, source-anchored rubric, authored cooldown,
lesson-specific RichV2 and checkpoint while preserving the 120-lesson blueprint.
"""
from pathlib import Path
from html.parser import HTMLParser
import json,sys,re,hashlib,datetime
ROOT=Path(__file__).resolve().parents[2]
BASE=Path("artifacts/curriculum-delivery/US-CCSS-MATH/G11")
PACK=Path("src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json")
BP=Path("src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json")
class Elements(HTMLParser):
 def __init__(self):super().__init__();self.stack=[];self.rows=[];self.skip=0
 def handle_starttag(self,tag,attrs):
  if tag in ('script','style','svg','noscript'):self.skip+=1
  if tag in ('h1','h2','h3','h4','p','li','th','td'):self.stack.append((tag,[]))
 def handle_endtag(self,tag):
  if tag in ('script','style','svg','noscript') and self.skip:self.skip-=1
  if self.stack and self.stack[-1][0]==tag:
   t,b=self.stack.pop();s=re.sub(r'\s+',' ',''.join(b)).strip()
   if s:self.rows.append((t,s))
 def handle_data(self,data):
  if not self.skip and self.stack:self.stack[-1][1].append(data)
def put(p,v):
 full=ROOT/p;full.parent.mkdir(parents=True,exist_ok=True)
 full.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding='utf8')
def source_rows(path):
 parser=Elements();parser.feed(Path(path).read_text(encoding='utf8',errors='replace'))
 return [(tag,text) for tag,text in parser.rows if not text.startswith(('Privacy Policy','© 2019','The Illustrative Mathematics name','Teachers with a valid work email','This book includes public domain'))]
def summarize_activity(rows):
 activities=[];section='Introduction';ordinal=0
 for tag,text in rows:
  if tag in ('h2','h3') and (re.match(r'\d+\.\d+:',text) or re.match('Problem [0-9]+',text)):
   section=text;ordinal=0
  elif tag in ('p','li') and len(text)>18 and section!='Introduction':
   if any(s in text.lower() for s in ('licensed under','cookies','register or sign in','privacy policy')):continue
   ordinal+=1
   if text.startswith('Description:'):continue
   # preserve mathematically precise original quantities in evidence only
   activities.append((section,ordinal,text))
 return activities
DATA={
5:{
 'goal':'Define sequences as integer-domain functions, specify initial conditions and write recursive function notation.',
 'explanation':'For a triangular dot pattern with one dot at step 1 and a new row of n dots at step n, the count is D(1)=1 and D(n)=D(n-1)+n for whole-number n>=2. The first five counts are 1,3,6,10,15 and the explicit formula is n(n+1)/2. Its differences increase, so it is neither arithmetic nor geometric. A recursive equation without a starting term is incomplete.',
 'concepts':[('Sequence as function','An integer-indexed function has one output for each permitted input n.','State domain and first term.','D: positive integers to nonnegative integers.'),('Recursive notation','A term is defined through an earlier term.','For n>=2, f(n)=f(n-1)+d, with f(1) specified.','f(1)=6, f(n)=f(n-1)+4.'),('Nonlinear triangular numbers','Each new triangle row adds its row number.','T(n)=T(n-1)+n; T(1)=1.','1,3,6,10,15')],
 'worked':[('Triangular pattern','How many dots are there at step 5?','Use the previous count and add the new row.',['D(1)=1.','D(2)=1+2=3; D(3)=3+3=6.','D(4)=6+4=10; D(5)=10+5=15.'],'15 dots','Differences 2,3,4,5 increase, not a constant.'),
 ('Compare recursive sequences','Classify 30,40,50,... and 1,3,7,15,...','Check consecutive differences and ratios.',['The first has constant +10, hence arithmetic.','The second has differences 2,4,8 so not arithmetic.','Ratios 3,7/3,15/7 differ so not geometric.'],'The first is arithmetic; second is neither.','1,3,7,15 follows next=2*previous+1.')],
 'cooldown':('A triangular display starts with one dot. On each next step add a new row containing as many dots as the step number. Write D(1), D(n) for n>=2, list D(1)...D(5), and explain whether it is arithmetic or geometric.','D(1)=1, D(n)=D(n-1)+n; 1,3,6,10,15; neither.'),'review':['dot pattern drawing and incremental row count','compare domain positive integers to continuous inputs','write correct initial condition and recurrence','identify which given patterns are arithmetic, geometric or neither']},
6:{
 'goal':'Translate among a list, table, graph and recursive definition; calculate common differences and growth factors.',
 'explanation':'A sequence can be represented as indexed terms, an input-output table, discrete graph points or a recurrence. For 5,15,25,35,45 the common difference is 10. For g(1)=-5 and g(n)=-2g(n-1), the first four terms are -5,10,-20,40. A geometric list of -factor values alternates signs. For the growing square arrangement with totals 1,5,14,... each new step adds n^2, so S(1)=1 and S(n)=S(n-1)+n^2; S(4)=30. For maximal planar pieces from n straight cuts, R(n)=1+n(n+1)/2.',
 'concepts':[('Four representations','One indexed sequence can appear as numbers, table, graph and recurrence.','Keep index and start conditions consistent.','4,7,10,13 has f(1)=4, f(n)=f(n-1)+3.'),('Sign-changing geometric sequence','Negative ratio reverses the sign every step.','g(n)=-2g(n-1).','-5,10,-20,40.'),('Sum of squares pattern','A new n-by-n block contributes n squared cells.','S(n)=S(n-1)+n^2.','S(1)=1, S(2)=5, S(3)=14, S(4)=30.')],
 'worked':[('Square growth','A pattern totals 1,5,14 squares at steps 1,2,3. What is step 4?','Add a new 4-by-4 block.',['The added counts are 1,4,9 at the first three steps.','Step 4 adds 16.','S(4)=14+16=30.'],'30 squares','The differences 4,9,16 are not constant.'),
 ('Maximum pieces of a circle','Starting with one region, how many at most after four straight cuts?','Each new line can cross all previous lines at distinct points.',['New regions after cuts 1,2,3,4 are 1,2,3,4.','Total is 1+1+2+3+4=11.'],'11 regions','A drawing must actually exhibit the maximal intersections.')],
 'cooldown':('A block pattern has S(1)=1 and adds n squared small squares at step n. Find S(2), S(3) and S(4), write its recursive definition and state if arithmetic, geometric or neither.','S(2)=5,S(3)=14,S(4)=30; S(n)=S(n-1)+n^2 for n>=2; neither.'),'review':['match corresponding data cards and recurrence','graphs must be discrete with correct labels','square pattern drawing and nonconstant change','physical diagram maximal cuts claim']},
7:{
 'goal':'Ask for missing data needed to represent and solve an indexed sequence, and explain the justification without exposing a partner’s card.',
 'explanation':'A recursive rule alone may not identify a unique sequence: f(n)=f(n-1)-5 requires a starting term. A defined function also needs a meaningful index domain. In a two-person information gap, request the missing first term, multiplier or difference and index origin, then justify the derived table/graph. For H(1)=20.25 with common ratio 2/3, the first terms are 20.25,13.5,9,6. A visual pattern with count n^2+3n+3 starting at n=0 has counts 3,7,13,21,...; differences 4,6,8 increase.',
 'concepts':[('Missing initial condition','A recurrence does not specify its first value without an initial term.','Ask for f(1) or f(0) explicitly.','f(n)=f(n-1)-5 admits many sequences.'),('Questioning for information','Mathematical information can be shared and justified through precise questions.','Request the start index, initial term and update relationship.','Given H(1)=20.25 and ratio 2/3, H(3)=9.'),('Quadratic visual count','A pattern with n^2+3n+3 increases by nonconstant amounts.','At n=0 count=3; n=1 count=7.','3,7,13,21')],
 'worked':[('Geometric info gap','Given H(1)=20.25 and factor 2/3, find H(4).','Multiply by the common ratio three times.',['H(2)=13.5.','H(3)=9.','H(4)=6.'],'6','H(1)=(81/4), H(4)=(81/4)*(8/27)=6.'),
 ('Quadratic dots','For D(n)=n^2+3n+3 from step zero, find the first four values.','Evaluate the expression for whole-number inputs.',['D(0)=3.','D(1)=7; D(2)=13.','D(3)=21.'],'3,7,13,21','Differences 4,6,8 are nonconstant.')],
 'cooldown':('You receive the recurrence q(n)=q(n-1)-1.5 for n>=2 but not the first term. State which information is missing; then if q(1)=10, give q(1) through q(5) and classify.','Need q(1). Terms 10,8.5,7,5.5,4; arithmetic d=-1.5.'),'review':['partner-requested specific information only','problem card and data card not exposed before comparison','justify why first term needed','quadratic dot drawing and explanation']},
8:{
 'goal':'Interpret equivalent explicit n-th-term formulas, including exponential factors and indexing offsets.',
 'explanation':'The recursive form f(n)=2f(n-1) with f(1)=3 has explicit form f(n)=3*2^(n-1) for integers n>=1. The same function can be written as (3/2)*2^n on the same domain. In repeated paper slicing, 80 square inches reduced to one fourth of the previous piece per stage yields A(1)=80 and A(n)=80*(1/4)^(n-1). Using exponents avoids iterating through every earlier value, but the stated domain is constrained by the physical model.',
 'concepts':[('Explicit term rule','A formula for term n uses n without earlier terms.','g(n)=g(1)*r^(n-1).','g(1)=3,r=2 yields 3*2^(n-1).'),('Equivalent expressions','Algebraically different formulas can define equal values.','3*2^(n-1)=(3/2)*2^n.','At n=4 both are 24.'),('Domain matters','Sequence indices are positive or nonnegative integers according to the model.','Do not interpolate a fractional number of cuts.','1,2,3,... rather than all real n.')],
 'worked':[('Convert recurrence to nth term','f(1)=3, f(n)=2f(n-1). Find f(6).','Apply the exponent formula.',['f(n)=3*2^(n-1).','f(6)=3*2^5.','3*32=96.'],'96','Term 1 gives 3, not 6.'),
 ('Paper slicing','A(1)=80 and each new piece has one-quarter the area. Find A(4).','Compute 80*(1/4)^3.',['There are three multiplications by 1/4.','A(4)=80/64.'],'5/4 square inches','Physical model requires a feasible step count.')],
 'cooldown':('A sequence begins 5 and triples each term. Write both a recursive and an explicit definition starting at n=1, calculate the fifth term and state why n=2.5 is not a sequence index.','a(1)=5,a(n)=3a(n-1); a(n)=5*3^(n-1); a(5)=405; indices are integers.'),'review':['identify nth-term expressions matched to recurrence','justify equation equivalence','paper slicing and geometric diagram','discrete graph/domain']},
9:{
 'goal':'Use sequences to model contexts, state valid discrete domains, and compare recursive and explicit representations.',
 'explanation':'A model needs a meaningful domain as well as a rule. For a 4-by-4 subdivision where each new piece has a quarter of the previous area, an 80-square-inch region creates A(1)=80, A(n)=80*(1/4)^(n-1); the first four areas are 80,20,5,1.25. In a line of n joined hexagonal tables, one table exposes 6 sides and each additional table adds 4 usable sides, so P(n)=6+4(n-1)=4n+2 for positive integer n. A paper or table count cannot sensibly be evaluated at a fractional n.',
 'concepts':[('Domain from context','The context determines permitted inputs.','Tables and people require whole nonnegative or positive numbers.','P(3.2) joined tables is meaningless.'),('Linear pattern','An equal increase each step yields an arithmetic sequence.','P(n)=6+4(n-1).','Three connected hexagonal tables seat 14 people.'),('Quarter-area decay','Repeated division by 4 produces geometric decrease.','A(n)=80*(1/4)^(n-1).','80,20,5,1.25.')],
 'worked':[('Connected tables','How many places around five joined hexagonal tables?','Start with 6 and add 4 per additional table.',['P(1)=6.','P(n)=6+4(n-1).','P(5)=6+16=22.'],'22 seats','Each joined edge removes one exposed side from each of two tables.'),
 ('Area decline','A(1)=80, next area one-quarter previous. Find A(4).','Apply three factors of one-quarter.',['A(2)=20.','A(3)=5.','A(4)=1.25.'],'1.25 square inches','Domain is positive whole-number piece index.')],
 'cooldown':('An envelope starts at 6.75 grams and each inserted clip adds 0.5 grams. Define w(n) for n nonnegative clips, find w(5), and explain why w(2.5) is not an appropriate count of clips.','w(n)=6.75+0.5n; w(5)=9.25 grams; n is a nonnegative integer.'),'review':['cake-share and numerical domain justification','visual Fibonacci-square data','connected tables and fractional-table invalidity','graph with labelled integer input']},
10:{
 'goal':'Create and compare sequence equations from tables, growth percentages and context patterns.',
 'explanation':'For geometric sequence 16,24,36,54,81, each term is 1.5 times the previous, corresponding to 50 percent growth per step. A comparison of two population models requires calculating from each table and evaluating the same term index. Square-grid patterns can grow as arithmetic for their borders and quadratically for their interiors, so do not force an exponential model onto every growing picture. For f(0)=3 and f(n)=2f(n-1), f(n)=3*2^n.',
 'concepts':[('Percent growth factor','A multiplier r=1+p/100 corresponds to p percent growth.','p=100(r-1).','Ratio 1.5 gives 50 percent growth.'),('Equation from a sequence','Write a rule for any permitted term index.','f(n)=f(0)*r^n for geometric n starting at zero.','f(0)=3 and factor2 gives f(n)=3*2^n.'),('Compare types','Check additive differences and multiplicative ratios.','An arithmetic border may coexist with quadratic interior.','4,8,12,... additive; 1,4,9,... quadratic.')],
 'worked':[('Growth percent','The sequence 16,24,36,54,81 has what growth percent per step?','Find the multiplier from two consecutive terms.',['24/16=3/2.','Growth relative to prior value = (3/2)-1=1/2.','Convert 1/2 to 50%.'],'50 percent','36/24 and 54/36 also equal 1.5.'),
 ('Recursive to explicit','f(0)=3 and f(n)=2f(n-1) for n>=1. Find f(5).','Use f(n)=3*2^n.',['Five multiplications by two after n=0.','f(5)=3*32.'],'96','f(0)=3 matches the start condition.')],
 'cooldown':('A geometric sequence starts 8,12,18. Find its growth factor and percentage increase, write an nth-term rule beginning at n=1 and find term five.','r=1.5, growth 50%; a(n)=8*(1.5)^(n-1); a(5)=40.5.'),'review':['classify two population tables, compare overtaking index','draw black-and-white square pattern','justify recurrence and explicit form','argue why Fibonacci closed form is not always preferable']},
11:{
 'goal':'Calculate short geometric sequence sums from contexts without requiring a formula, using exact arithmetic.',
 'explanation':'A sequence sum adds its terms; it is not another single term. For 1,2,4,8,16,32,64 the seven-term sum is 127, despite last term 64. For half the remainder at each stop, the covered fractions after two and four stops are 3/4 and 15/16; no finite number of stops reaches 1 exactly. For a triangular design whose sides are quadrupled at each step, S(0)=3, S(n)=3*4^n, while newly added triangles can follow a separate geometric sequence. Sum only through the specified number of terms.',
 'concepts':[('Finite sequence sum','Add exactly the specified terms.','S_N = a1+a2+...+aN.','1+2+4+8=15.'),('Geometric fractions','Repeatedly halve a remaining positive quantity.','Covered proportion after n halves: 1-(1/2)^n.','After four stops: 15/16.'),('Fractal geometric growth','Counts and areas must be tracked separately.','Initial 3 edges quadruple at each new subdivision step.','3,12,48 sides at steps 0,1,2.')],
 'worked':[('Seven-step doubling sum','How much is received if daily receipts double from 1 through seven days?','List terms and add.',['Terms are 1,2,4,8,16,32,64.','Sum first four:15; last three:112.','Total 127.'],'127','A sum is not the final 64.'),
 ('Half-remaining walk','After four halfway segments, what fraction of a hallway has been covered?','Subtract remaining fraction from one.',['Remaining = (1/2)^4=1/16.','Covered=1-1/16.'],'15/16','No finite n covers all of it under exact halving.')],
 'cooldown':('A seven-day sequence pays 2,4,8,16,32,64,128 dollars. Find the total amount, show the addition and explain how the result differs from the final daily payment.','Total 254 dollars; last payment 128 dollars.'),'review':['paper fraction drawings over repeated quarters','triangle subdivision count and geometric sum','interpret infinite limiting behaviour as distinct from finite steps','explain conservation versus accumulating sums']}
}
if __name__=='__main__':
 n=int(sys.argv[1]);assert 5<=n<=11
 d=DATA[n];ord=n;code=f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L{n:02d}'
 started=datetime.datetime.now(datetime.timezone.utc).isoformat()
 inv=json.loads((ROOT/BASE/'local-archive-source-inventory-20261106.json').read_text(encoding='utf8'))['lessons'][n-1]
 assert inv['lessonCode']==code
 for doc in inv['documents'].values():assert hashlib.sha256(Path(doc['path']).read_bytes()).hexdigest()==doc['sha256']
 bp=json.loads((ROOT/BP).read_text(encoding='utf8'))['Lessons'][n-1]
 assert bp['LessonCode']==code
 source=inv['documents'];teacher=source_rows(source['teacher/preparation.html']['path'])
 goals=[];in_goals=False
 for tag,t in teacher:
  if t=='Learning Goals':in_goals=True;continue
  if in_goals and tag=='h3':break
  if in_goals and tag=='li':goals.append(t)
 activities=summarize_activity(source_rows(source['student/index.html']['path']))
 problems=summarize_activity(source_rows(source['student/practice.html']['path']))
 assert len(goals)>=1 and len(activities)>=5 and len(problems)>=2,(n,len(goals),len(activities),len(problems))
 pack=json.loads((ROOT/PACK).read_text(encoding='utf8'))
 l=next(l for l in pack['lessons'] if l['lessonCode']==code);t=next(t for t in l['translations'] if t['cultureCode']=='en')
 t['explanation']='Learning goals: '+' '.join(goals)+' '+d['explanation']
 t['keyConceptsAndRules']='\n'.join(f'{a}: {b} Rule: {c} Example: {e}' for a,b,c,e in d['concepts'])
 t['workedExamples']='\n'.join('Example '+str(i+1)+': '+v[1]+' Method: '+v[2]+' Steps: '+' '.join(v[3])+' Answer: '+v[4]+' Check: '+v[5] for i,v in enumerate(d['worked']))
 t['stepByStepSolutions']='1. Name initial index and starting term. 2. Identify changes, ratios, or context operations. 3. Match source activity representation with a mathematical rule. 4. Calculate examples exactly and check. 5. Explain the result and domain; diagrams and open explanation are teacher-reviewed.'
 t['commonMistakes']='\n'.join('Do not substitute a generic arithmetic question for the lesson goal. Check the initial index, the units, consecutive differences or ratios and any domain restrictions. Teacher-graded diagrams, discussion or technology are not automatically scored.')
 t['quickSummary']=d['goal']+' '+d['explanation'][:150]
 put(PACK,pack)
 richpath=Path(f'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l{n:02d}.rich-lesson-v2.json')
 concepts=[{'Title':a,'Definition':b,'Rule':c,'Example':e} for a,b,c,e in d['concepts']]
 worked=[{'Title':a,'Question':b,'Method':c,'Steps':steps,'Answer':ans,'Check':check} for a,b,c,steps,ans,check in d['worked']]
 # Original explanation tied to local source sections rather than reproducing their paragraphs
 by_section={}
 for s,index,text in activities:by_section.setdefault(s,[]).append(text)
 section_descriptions=[]
 for heading,items in by_section.items():
  joined=' '.join(items)
  snippet=joined[:320].rsplit(' ',1)[0] if len(joined)>320 else joined
  section_descriptions.append(f'Activity {heading}: math source context — {snippet}. Work the source task, document quantities and justify the rule. Teacher checks any diagram, model or partner discussion.')
 practice_descriptions=[]
 for heading,items in dict((sec,[]) for sec,_,_ in problems).items():
  vals=[v for sec,idx,v in problems if sec==heading]
  joined=' '.join(vals)
  snippet=joined[:280].rsplit(' ',1)[0] if len(joined)>280 else joined
  practice_descriptions.append(f'Cumulative {heading}: source mathematics — {snippet}. Preserve original index, signs, ratios and units; independent first-party answer and reasoning required.')
 coolq,coola=d['cooldown']
 rich={'LessonCode':code,'CultureCode':'en','Title':bp['Title'],
  'ExplanationParagraphs':[d['goal'],d['explanation']]+section_descriptions+practice_descriptions+[
  'First-party independent cool-down: '+coolq+' Answer key: '+coola,
  'Assessment boundary: digital exact numeric questions may reuse an existing compatible family only if semantics match; diagrams, card discussion and written justifications require teacher verification. Source activities are guidance, not proof an interactive simulator exists.'],
  'KeyConcepts':concepts,'WorkedExamples':worked,
  'CommonMistakes':[{'Mistake':'Treating a sequence rule as context-free.','WhyWrong':'It can alter the index, initial term, units or learning target.','Correction':'State the beginning term, domain and original lesson model.'},
    {'Mistake':'Using a fixed common difference when the sequence is not arithmetic.','WhyWrong':'Differing consecutive differences invalidate the arithmetic formula.','Correction':'Calculate two or more differences or ratios before selecting a rule.'},
    {'Mistake':'Auto-grading graphs, card exchanges or manipulative proof from a number alone.','WhyWrong':'Numeric equality does not establish a correct diagram or explanation.','Correction':'Use a teacher rubric for visual and reasoning evidence.'}],
  'SummaryPoints':[d['goal'],d['explanation'][:320],'Use current existing exact sequence families where genuinely compatible; leave unsupported questions teacher-reviewed.'],'Visuals':[],'Videos':[]}
 put(richpath,{'SchemaVersion':2,'PackCode':'US-CCSS-MATH','ContentVersion':f'g11-u01-l{n:02d}-local-source-20261106','Lessons':[rich]})
 aspath=Path(f'src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l{n:02d}-local-source-20261106.json')
 assess={'LessonCode':code,'LearningGoals':goals,'BlueprintAlignments':bp.get('Alignments',[]),
 'ReusePolicy':'Existing arithmetic/geometric/affine exact sequence engines only when question matches; unverified new mechanics remain teacher-reviewed',
 'ExistingNumericFamilies':['sequences.core.arithmetic_nth_term','sequences.core.geometric_nth_term','sequences.core.find_position_arithmetic','usccss.g11.alg2.u01.l01.affine_recurrence','usccss.g11.alg2.u01.l01.fibonacci_term','usccss.g11.alg2.u01.l02.growth_factor'],
 'QuestionRouting':'UNPROMOTED_NO_RUNTIME_CLAIM',
 'TeacherReview':{'Required':True,'SourceSpecificTasks':d['review'],'Rubric':['correct term index and initial condition','mathematically correct representation or diagram','explanation justified by source constraints','accurate signed/fractional answers']},
 'FirstPartyCoolDown':{'Prompt':coolq,'Expected':coola,'Origin':'EDULYTICS_ORIGINAL','Grading':'TEACHER_REVIEW_REQUIRED'},
 'TestsAuthoredNotRun':[f'tools/curriculum/us-ccss-g11-u01-l{n:02d}-authoring-contract.test.cjs'],'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN','RichV2Path':str(richpath).replace('\\','/')}
 put(aspath,assess)
 evidence=[]
 for kind,rows in [('STUDENT_ACTIVITY',activities),('STUDENT_PRACTICE',problems)]:
  for section,index,text in rows:
   evidence.append({'SourceElementId':f'G11-U01-L{n:02d}-{kind}-{len(evidence)+1:03d}',
    'SourceSection':section,'SourceTaskIndex':index,
    'SourceExcerptForTraceability':text[:260],
    'OriginalMathTaskCoverage':'rich instruction and teacher-reviewed source task, including diagrams where source needs it',
    'Target':str(richpath).replace('\\','/'),
    'Status':'AUTHORED_SOURCE_SCOPED','StageD':'NOT_RUN'})
 ep=BASE/f'u01-l{n:02d}-element-coverage.json'
 put(ep,{'LessonCode':code,'SourceDocuments':inv['documents'],'PublisherMathObjectives':goals,'Count':len(evidence),'Elements':evidence,'StudentSourcePracticesIncluded':len(problems)>0,'TeacherCoolDownAccess':'RESTRICTED_FIRST_PARTY_AUTHORED','TestsExecuted':'NOT_RUN','ReuseSource':'LOCAL_KENDALL_HUNT_ARCHIVE_NOT_V360'})
 cp=BASE/f'u01-l{n:02d}-abc-checkpoint.json'
 put(cp,{'LessonCode':code,'Ordinal':n,'StartedUtc':started,'ClosedUtc':datetime.datetime.now(datetime.timezone.utc).isoformat(),'Status':'READY_FOR_STAGE_D','ReadyForStageD':True,'PhaseA':'SOURCE_READ_COMPARED_AND_MAPPED','PhaseB':'ORIGINAL_LEARNING_CONTENT_AUTHORED','PhaseC':'ASSESSMENT_RUBRIC_AND_COOLDOWN_AUTHORED','QuestionRouting':'UNPROMOTED_NO_RUNTIME_CLAIM','SourceElementCount':len(evidence),'AssessmentPath':str(aspath).replace('\\','/'),'ElementEvidencePath':str(ep).replace('\\','/'),'RichV2Path':str(richpath).replace('\\','/'),'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN'})
 ledgerp=BASE/'abc-independent-course-ledger.json';ledger=json.loads((ROOT/ledgerp).read_text(encoding='utf8'))
 assert len(ledger['LessonEntries'])==n-1 and ledger['LessonEntries'][-1]['Ordinal']==n-1
 ledger['LessonEntries'].append({'Ordinal':n,'LessonCode':code,'Unit':1,'Lesson':n,'ABCReadyForD':True,'Status':'READY_FOR_STAGE_D','ClosedUtc':datetime.datetime.now(datetime.timezone.utc).isoformat(),'SourceCheckpoint':str(cp).replace('\\','/'),'ElementEvidence':str(ep).replace('\\','/'),'AssessmentPath':str(aspath).replace('\\','/'),'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN'})
 ledger['UpdatedUtc']=datetime.datetime.now(datetime.timezone.utc).isoformat();put(ledgerp,ledger)
 pp=BASE/'full-grade-progress.json';progress=json.loads((ROOT/pp).read_text(encoding='utf8'))
 assert progress['LessonsReadyForStageD']==n-1
 progress.update({'LessonsReadyForStageD':n,'LessonsRemaining':120-n,'LastClosedLesson':code,'FirstOpenLesson':f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U{(2 if n==11 else 1):02d}:L{(1 if n==11 else n+1):02d}','UpdatedUtc':datetime.datetime.now(datetime.timezone.utc).isoformat()})
 put(pp,progress)
 print('G11_SOURCE_AUTHORING_READY',code,'teacher_goals',len(goals),'source_elements',len(evidence),'tests_NOT_RUN')
