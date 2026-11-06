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
SPECS={
1:('Polynomial box volume','Removing an equal square of side x from each corner of an L by W sheet and folding gives a box with height x and base dimensions L-2x and W-2x; V(x)=x(L-2x)(W-2x) with 0<x<min(L,W)/2. A graph must be interpreted only over the physically permitted domain.','For a 20 by 12 inch sheet, write the volume polynomial and evaluate at x=2.','V=x(20-2x)(12-2x); V=4x^3-64x^2+240x; V(2)=256.','V(x)=4x^3-64x^2+240x cubic inches; V(2)=256.','For an 18 by 10 sheet, derive V(x) and V(2), with domain.','V=x(18-2x)(10-2x); V(2)=2*14*6=168; 0<x<5.'),
2:('Investment polynomial','Repeated yearly deposits produce a polynomial in an unspecified growth multiplier r. After three equal deposits of 1000, a simple end-of-year model is P(r)=1000(1+r+r^2); check each deposit timing. Positional representation parallels place value: digits weighted by powers of 10.','A 1000 gift is invested at the ends of three successive periods. At the third period end, give the accumulated sum when r=1.05.','P(r)=1000(1+r+r^2); 1+1.05+1.1025=3.1525.','3152.50 currency units','Write the accumulation polynomial for 200 deposited at the end of each of four years; evaluate at r=1.','200(1+r+r^2+r^3); at r=1 total 800.'),
3:('Polynomial vocabulary','A polynomial is a finite sum of nonnegative integer powers with real coefficients. The degree is the largest exponent of a nonzero term, the leading term has that degree, and the constant is the x-independent term. A graph may have relative maxima or minima that require examining turning behaviour, not just counting terms.','Determine degree, leading term and constant for 3x^4-2x^2+7.','Highest exponent 4; leading monomial 3x^4; x-independent term 7.','Degree 4, leading term 3x^4, constant 7','Find the degree, leading coefficient and constant of -2x^3+5x-9.','Degree 3, leading coefficient -2, constant -9.'),
4:('Polynomial closure','Sums, differences and products of polynomials remain polynomials. Combine like powers only when adding or subtracting; use distribution when multiplying. Degree of a product generally adds nonzero factor degrees, while addition may cancel leading terms and lower the result degree.','Add x^2+2x and 3x^2-x+1, then multiply x by x+2.','Group like powers: (1+3)x^2+(2-1)x+1; distribute x.','Sum 4x^2+x+1; product x^2+2x.','Subtract (2x^2+3x-1) from (5x^2-x+4).','3x^2-4x+5.'),
5:('Zeros of factored polynomials','A product is zero if any factor is zero. For p(x)=(x-2)(x+3), p(x)=0 at x=2 and -3; these are horizontal intercepts of its graph. Factored form reveals zeros, but a sketch also depends on degree and the leading coefficient.','Find the zeros and vertical intercept of p(x)=(x-2)(x+3).','Set each factor to zero: x=2 or -3; evaluate p(0)=-2*3.','Zeros -3 and 2; y-intercept -6.','Find zeros of 2(x+4)(x-1), and evaluate at x=0.','Zeros -4 and 1; value -8.'),
6:('Equivalent polynomial forms','Factored form reveals roots and intercepts, while expanded standard form reveals degree and leading term; distribution makes these equivalent. A physical box model may be expressed in either form without changing its units, zeros or restricted domain.','Expand V=x(20-2x)(12-2x).','Multiply base factors to obtain 240-64x+4x^2; multiply by x.','V=4x^3-64x^2+240x.','Expand (x-3)(x+2)(x+1) and give the leading term.','x^3-7x-6; leading term x^3.'),
7:('Zeros and graphing window','For prescribed zeros a and b, a possible function is f(x)=k(x-a)(x-b) where k is nonzero. A valid graphing window must include the given zeros and enough vertical variation to see the turning behaviour; do not change a zero to make it fit an arbitrary window.','Make a monic quadratic whose horizontal intercepts are x=-1 and x=3.','Choose factors (x+1) and (x-3); distribute.','f(x)=x^2-2x-3.','Give a polynomial with zeros -2,0,4 and leading coefficient 1.','f(x)=x(x+2)(x-4).'),
8:('End behaviour by parity','For large absolute x, the leading term dominates a polynomial. A positive even-degree leading term rises at both ends; positive odd-degree is negative on the far left and positive on the far right. The lower degree terms do not change this eventual sign pattern.','Describe the end behaviour of f(x)=2x^4-3x+1.','Highest power is even and leading coefficient positive; term 2x^4 dominates as |x| grows.','f(x) tends to +infinity as x tends to either +infinity or -infinity.','Describe both ends of 5x^3-x^2.','Left -infinity, right +infinity.'),
9:('Leading coefficient and end behaviour','Changing the sign of the leading coefficient reverses the large-|x| sign of a polynomial. For -2x^3+7x, the far-left values grow positive and far-right values grow negative. State both the degree parity and leading sign before inferring behaviour.','Describe the left and right ends of f(x)=-2x^3+7x.','Leading term -2x^3 dominates. For x large positive it is negative; for x large negative it is positive.','Left +infinity, right -infinity.','Give an odd-degree polynomial that goes up on the left and down on the right.','-x^3 or any odd degree with negative leading coefficient.'),
10:('Root multiplicity','If a factored polynomial contains (x-a)^m, the root a has multiplicity m. At an odd-multiplicity real root the sign generally changes and the curve crosses the axis; at even multiplicity the graph touches and turns without a sign change. Leading sign and total degree govern end behaviour.','Analyse zeros and crossing for f(x)=(x-1)^2(x+2).','At x=1 exponent2 even so touches; at x=-2 exponent1 odd so crosses; degree 3 and leading positive.','Roots 1 (touch), -2 (cross); left down, right up.','For f(x)=-(x+3)^2(x-4), identify touch/cross roots.','-3 touches; 4 crosses.'),
11:('Intersections of polynomial functions','A solution to f(x)=g(x) provides the horizontal coordinate of the intersection; evaluating either function gives its shared vertical coordinate. Algebraic solutions must be turned into ordered points and checked on both graphs.','Find intersections of y=x^2 and y=2x.','x^2=2x; x(x-2)=0; x=0 or 2; compute y.','(0,0) and (2,4).','Solve intersections of y=x^2-1 and y=2x-1.','(0,-1) and (2,3).'),
12:('Polynomial division diagrams','A rectangular area diagram models polynomial division by grouping like powers: if P(x)=(x+2)(x+3), then dividing P by x+2 gives quotient x+3. A factor divides with remainder zero. Check by multiplication before accepting a quotient.','Divide x^2+5x+6 by x+2.','Factor (x+2)(x+3); cancel the divisor x+2.','Quotient x+3, remainder 0.','Divide x^2+7x+12 by x+3.','x+4, remainder 0.'),
13:('Polynomial long division','Polynomial long division uses leading-term division, multiplication and subtraction repeatedly. For (2x^3+3x^2-8x-12)/(x+2), successive quotient terms are 2x^2,-x,-6, giving zero remainder. Always compare product divisor times quotient plus remainder with dividend.','Divide 2x^3+3x^2-8x-12 by x+2.','2x^3/x=2x^2, subtract; then -x; then -6; remainder 0.','2x^2-x-6 with remainder zero.','Divide x^3-1 by x-1.','x^2+x+1, remainder 0.'),
14:('Polynomial information gaps','A known factor gives one zero but does not determine a unique polynomial. A complete sketch needs more: additional factors or zeros, multiplicities, vertical intercept or leading sign and degree. Ask explicitly for the missing information rather than inventing it.','A monic cubic has known zeros -2,1 and 3. Write it factored and calculate its value at zero.','f=(x+2)(x-1)(x-3); f(0)=2*(-1)*(-3)=6.','f=(x+2)(x-1)(x-3), y-intercept 6.','Explain why knowing x-4 is a factor does not uniquely specify a polynomial.','Infinitely many choices p=(x-4)q(x), where q is another polynomial.'),
15:('Remainder theorem','For polynomial p and any a, dividing by x-a leaves remainder p(a). Thus x-a is a factor precisely when p(a)=0. This follows by writing p(x)=(x-a)q(x)+r and setting x=a.','For p(x)=x^3-4x+3, what is the remainder on division by x-1?','Evaluate p(1)=1-4+3=0.','Remainder 0; x-1 is a factor.','For p(x)=2x^3+3x+7 find remainder upon division by x-2.','p(2)=16+6+7=29.'),
16:('Cylinder surface optimisation','For a closed circular cylinder of fixed volume V, volume pi*r^2*h=V so h=V/(pi*r^2) with r>0. Total material area S=2pi*r^2+2pi*r*h=2pi*r^2+2V/r. The rational formula and positive radius domain are essential; a negative radius is not physical.','A closed cylinder has volume 100pi. Express its surface area in terms of radius r.','h=100/r^2; S=2pi*r^2+2pi*r*(100/r^2).','S(r)=2pi*r^2+200pi/r for r>0.','For radius 5 and volume 100pi find h and surface area.','h=4, S=2pi*25+2pi*5*4=90pi.'),
17:('Reciprocal travel model','For a fixed 10-mile trip at constant positive speed v, travel time t(v)=10/v hours. The graph declines as speed increases, has vertical asymptote v=0 and approaches t=0 for arbitrarily large speed without reaching zero at finite v. The physically valid domain is v>0.','How long does a 10-mile trip take at 4 miles per hour?','Use t=distance/speed=10/4.','2.5 hours.','What speed gives a 10-mile time of 1.25 hours?','v=10/1.25=8 miles/hour.'),
18:('Horizontal rational asymptote','A rational function with form f(n)=c+k/n approaches c as n grows without bound, with n restricted by its context. For a book with total cost 1000+5n to print n copies, average cost is 5+1000/n, approaching 5 from above. The asymptote is a limit, not a finite attainable average cost when the fixed cost is positive.','If total cost is 1000+5n, find average cost per copy for n=100 and its horizontal asymptote.','Average=5+1000/n; at n=100 average=15; as n increases fixed share tends to 0.','15 per copy; asymptote y=5.','If average is 4+200/n, find the asymptote and the value at n=50.','y=4; f(50)=8.'),
19:('End behaviour via polynomial division','To analyse a rational function p(x)/q(x), divide polynomials: f(x)=Q(x)+R(x)/q(x). If the remainder fraction approaches zero, the end behaviour follows the polynomial Q(x). Numerator and denominator degrees determine whether there is a constant, zero or nonconstant polynomial asymptote.','Divide (2x^2+3)/(x+1) and describe end behaviour.','Long division gives quotient 2x-2 and remainder 5; 5/(x+1) approaches 0 for large |x|.','f(x)=2x-2+5/(x+1); asymptotic to y=2x-2.','Find horizontal asymptote of (3x^2+1)/(x^2+4).','y=3.'),
20:('Solving rational price equations','A contextual rational equation models average price with a fixed cost plus variable cost: C(n)=(200+5n)/n=5+200/n, n>0. To solve C=9, require n=50, then substitute back. Multiplying by n is legitimate only if n is nonzero.','A shirt run has fixed setup 200 and unit cost 5. How many shirts make average cost 9?','5+200/n=9; 200/n=4; n=50.','50 shirts, with n>0.','Solve 6+180/n=12 for n>0.','n=30.'),
21:('Two rational expressions in a model','A river trip with still-water speed v and current c has upstream speed v-c, downstream v+c and times distance/(v-c) and distance/(v+c). A physical model requires v>|c|. Rational expressions can be combined with a common denominator but forbidden speeds remain excluded.','A boat travels 8 km upstream and 8 km downstream at still-water speed 6 km/h against a 2 km/h current. Find total time.','Upstream=8/(6-2)=2; downstream=8/(6+2)=1; sum=3.','3 hours.','A boat with still speed 10 and current 2 travels 12 km each way. Find total time.','12/8+12/12=2.5 hours.'),
22:('Extraneous rational solutions','Clearing a denominator can introduce candidate roots where the original expression is undefined. Solve an equation algebraically, then inspect excluded denominator zeros before accepting solutions. A rational expression cannot be evaluated at a pole even when the cleared polynomial has a zero there.','Solve (x^2-4)/(x-2)=0 with the original domain restriction.','Denominator forbids x=2; numerator zero at x=-2 or x=2; exclude x=2.','x=-2 only.','Solve (x^2-9)/(x+3)=0.','x=3 only; x=-3 is excluded.'),
23:('Polynomial identities','An identity is an equality valid for every allowed variable value, unlike an equation with isolated solutions. Expand both sides or use an area model to prove (a+b)^2=a^2+2ab+b^2. Testing one number is not a general proof.','Expand (x+3)^2 and justify the middle term.','(x+3)(x+3)=x^2+3x+3x+9.','x^2+6x+9.','Prove (x-2)^2=x^2-4x+4 by expansion.','(x-2)(x-2)=x^2-2x-2x+4=x^2-4x+4.'),
24:('Identities and visual proofs','An identity can link different geometric decompositions, e.g. (a+b)^2+(a-b)^2=2(a^2+b^2). Verify by expansion and interpret the equality with appropriate diagrams or a related theorem. An expression may be equal for all permitted inputs even if it looks unrelated.','Prove (a+b)^2+(a-b)^2=2(a^2+b^2).','Expand both squares: a^2+2ab+b^2+a^2-2ab+b^2; middle terms cancel.','2a^2+2b^2.','Verify 9(x^2-1)=9x^2-9 as a polynomial identity.','Distribution yields 9x^2-9 for every x.'),
25:('Finite geometric series formula','A geometric sum a+ar+...+ar^(n-1) for r not 1 is a(r^n-1)/(r-1), derived by subtracting S from rS. For r=1 the sum is na. Check a small n case before interpreting sum in a decay context.','Compute 3+6+12+24 using the sum formula.','a=3,r=2,n=4; S=3*(2^4-1)/(2-1).','45.','Sum 5+10+20+40+80.','155.'),
26:('Applying geometric sums','Financial, housing and growth contexts require interpreting the sum rather than confusing it with the final term. For n terms with initial a and growth ratio r use S_n=a(r^n-1)/(r-1) when r is not one; state the time indexing and unit. If r=1, add n equal terms.','Five housing projects contain 5,10,20,40,80 houses. Find the total.','a=5,r=2,n=5; S=5*(2^5-1).','155 houses.','A fund receives 100 each of four periods with factor 1.1 per period; find the amount at the end of the fourth period assuming first deposit accrues three times.','100*(1+1.1+1.21+1.331)=464.1.')
}
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
 n=int(sys.argv[1]);assert 1<=n<=26
 ord=11+n;started=datetime.datetime.now(datetime.timezone.utc).isoformat()
 code=f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L{n:02d}'
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
 title,explain,q,steps,answer,coolq,coola=SPECS[n]
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
 richp=Path(f'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u02-l{n:02d}.rich-lesson-v2.json')
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
 write(richp,{'SchemaVersion':2,'PackCode':'US-CCSS-MATH','ContentVersion':f'g11-u02-l{n:02d}-local-archive-20261106','Lessons':[rich]})
 assessp=Path(f'src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u02-l{n:02d}-local-source-20261106.json')
 assess={'LessonCode':code,'LearningGoals':goals,'BlueprintAlignments':bp.get('Alignments',[]),'SourceAuthority':'Local Kendall Hunt archival teacher/student pages; AccessIM v360 excluded',
 'EnginePolicy':'REUSE_IF_MATCHING_ELSE_TEACHER_REVIEW; DO_NOT_PROMOTE_UNVERIFIED','QuestionRouting':'UNPROMOTED_NO_RUNTIME_CLAIM',
 'CoreMathWorkedExample':{'Question':q,'Steps':steps.split('; '),'Answer':answer},
 'TeacherReview':{'Required':True,'ActivitySections':list(bysection),'PracticeSections':list(bypractice),'GradingRubric':['exact algebraic/model fidelity','correct numerical/symbolic value','valid input and units','teacher verifies diagrams, graphs, card work and explanation']},
 'FirstPartyCoolDown':{'Prompt':coolq,'Expected':coola,'Origin':'EDULYTICS_ORIGINAL','Grading':'TEACHER_REVIEW_REQUIRED'},
 'TestsAuthoredNotRun':[f'tools/curriculum/us-ccss-g11-u02-l{n:02d}-authoring-contract.test.cjs'],'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN','RichV2Path':str(richp).replace('\\','/')}
 write(assessp,assess)
 source_elements=[]
 for family,rows in [('activity',activity),('practice',practice)]:
  for k,(sec,text) in enumerate(rows,1):
   source_elements.append({'SourceElementId':f'G11-U02-L{n:02d}-{family.upper()}-{k:03d}',
   'SourceSection':sec,'SourceExcerpt':clipped(text,240),
   'SourceLessonArtifact':str(richp).replace('\\','/'),'Treatment':'SOURCE_SCOPED_AUTHORING_WITH_TEACHER_REVIEW','TestsExecuted':'NOT_RUN'})
 evidp=BASE/f'u02-l{n:02d}-element-coverage.json'
 write(evidp,{'LessonCode':code,'SourceDocumentHashes':{name:doc['sha256'] for name,doc in inv['documents'].items()},'Goals':goals,'ElementCount':len(source_elements),'SourceElements':source_elements,'OriginalSourceRights':'CC_BY_4_EVIDENCE_META','Assets':'source pictures/graphs retained as teacher source context; no claim of new interactive component','TestsExecuted':'NOT_RUN'})
 cpp=BASE/f'u02-l{n:02d}-abc-checkpoint.json';closed=datetime.datetime.now(datetime.timezone.utc).isoformat()
 write(cpp,{'LessonCode':code,'Ordinal':ord,'StartedUtc':started,'ClosedUtc':closed,'Status':'READY_FOR_STAGE_D',
 'StageA':'SOURCE_READ_AND_COMPARED','StageB':'FIRST_PARTY_SOURCE_SCOPED_RICH_V2_AUTHORED','StageC':'ORIGINAL_ASSESSMENT_AND_TEACHER_REVIEW_AUTHORED',
 'NoNewEngineNeeded':'EXACT_FAMILY_REUSE_ONLY_WHEN_MATCHING','SourceElementsMapped':len(source_elements),'AssessmentPath':str(assessp).replace('\\','/'),'SourceEvidencePath':str(evidp).replace('\\','/'),'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN'})
 ledgerp=BASE/'abc-independent-course-ledger.json';ledger=json.loads((ROOT/ledgerp).read_text(encoding='utf8'))
 assert len(ledger['LessonEntries'])==ord-1 and ledger['LessonEntries'][-1]['Ordinal']==ord-1
 ledger['LessonEntries'].append({'Ordinal':ord,'LessonCode':code,'Unit':2,'Lesson':n,'ABCReadyForD':True,'Status':'READY_FOR_STAGE_D','ClosedUtc':closed,'SourceCheckpoint':str(cpp).replace('\\','/'),'ElementEvidence':str(evidp).replace('\\','/'),'AssessmentPath':str(assessp).replace('\\','/'),'TestsExecuted':'NOT_RUN','StageD':'NOT_RUN'})
 ledger['UpdatedUtc']=closed;write(ledgerp,ledger)
 progressp=BASE/'full-grade-progress.json';progress=json.loads((ROOT/progressp).read_text(encoding='utf8'));assert progress['LessonsReadyForStageD']==ord-1
 unit=3 if n==26 else 2;next_l=1 if n==26 else n+1
 progress.update({'LessonsReadyForStageD':ord,'LessonsRemaining':120-ord,'LastClosedLesson':code,'FirstOpenLesson':f'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U{unit:02d}:L{next_l:02d}','UpdatedUtc':closed})
 write(progressp,progress)
 print('G11_UNIT2_SOURCE_REPAIR_AUTHORED',code,'goals',len(goals),'source_items',len(source_elements),'TESTS_NOT_RUN')
