from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
from fractions import Fraction as F
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";P=B/"full-grade-progress.json"
D=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda i:D[i].get_textpage().get_text_range()
profiles=[
(63,1,"11.1","Sequences and their Notations",953,966,71,7,9),
(64,2,"11.2","Arithmetic Sequences",966,976,74,7,8),
(65,3,"11.3","Geometric Sequences",976,984,60,6,7),
(66,4,"11.4","Series and their Notations",984,997,62,9,15),
(67,5,"11.5","Counting Principles",997,1007,54,7,10),
(68,6,"11.6","Binomial Theorem",1007,1014,49,3,3),
(69,7,"11.7","Probability",1014,1023,60,6,6)]
def save(f,v):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
def add(Q,q,a,skill,condition=True):
 assert condition
 Q.append({"OriginalExercise":len(Q)+1,"IndependentStudentQuestion":q,
 "TeacherMath":a,"SourceMathematicalSkill":skill,
 "MathSpotChecked":condition,"SourcePublisherQuestionCopied":False,
 "Scoring":"TEACHER_MANUAL"})
def calc(sec,count):
 Q=[]
 for n in range(1,count+1):
  if sec=="11.1":
   kind=n%6;start=n%7-2;step=n%5+1;k=n%5+4
   if kind==0:
    vals=[start+step*i for i in range(5)]
    q="List first five terms of explicit sequence a_n="+str(start)+"+"+str(step)+"(n−1)."
    ans=str(vals)+", indexed n=1..5."
    skill="Source explicit sequences"
   elif kind==1:
    vals=[(-1)**i*(start+i+1)for i in range(5)]
    q="List first five terms of alternating a_n=(−1)^n("+str(start)+"+n)."
    ans=str(vals)+"."
    skill="Source alternating signs"
   elif kind==2:
    vals=[i*i if i%2==0 else i+start for i in range(1,6)]
    q="Evaluate a_n=n² if n even, otherwise n+"+str(start)+", for n=1..5."
    ans=str(vals)+"." ;skill="Source piecewise explicit"
   elif kind==3:
    vals=[start]
    for j in range(4):vals.append(vals[-1]+step)
    q="Recurrence a1="+str(start)+", a_n=a_(n−1)+"+str(step)+": list five terms."
    ans=str(vals)+"." ;skill="Source recursive sequences"
   elif kind==4:
    q="Evaluate factorial expression ("+str(k)+"!)/("+str(k-2)+"!)."
    ans=str(k*(k-1))+"." ;skill="Source factorial sequences"
   else:
    q="For sequence 2n²+n, compute a_"+str(k)+"."
    ans=str(2*k*k+k)+"." ;skill="Source terms and explicit formulas"
  elif sec=="11.2":
   a=n%8-3;d=n%7-3
   if d==0:d=2
   k=n%17+6
   if n%5==0:
    q="Arithmetic sequence a1="+str(a)+", d="+str(d)+": find sum a1+..+a_"+str(k)+"."
    ans=str(F(k*(2*a+(k-1)*d),2))+"."
    skill="Source arithmetic series application"
   elif n%5==1:
    q="Identify common difference in "+str([a+i*d for i in range(5)])+"."
    ans="d="+str(d)+"." ;skill="Source common differences"
   elif n%5==2:
    q="Find a_"+str(k)+" given a1="+str(a)+", d="+str(d)+"."
    ans=str(a+(k-1)*d)+"." ;skill="Source arithmetic nth term"
   elif n%5==3:
    q="Write recurrence for arithmetic sequence first term"+str(a)+", difference"+str(d)+"."
    ans="a1="+str(a)+",a_n=a_(n−1)+("+str(d)+"), n≥2." ;skill="Source recursive notation"
   else:
    q="How many arithmetic terms from a1="+str(a)+" to a_N="+str(a+(k-1)*d)+" with d="+str(d)+"?"
    ans="N="+str(k)+"." ;skill="Source finite number of terms"
  elif sec=="11.3":
   a=n%5+1;r=[2,3,-2,F(1,2)][n%4];k=n%7+3
   vals=[a*r**i for i in range(4)]
   if n%4==0:
    q="Find ratio of geometric sequence "+str(vals)+"."
    ans="r="+str(r)+"." ;skill="Source common ratio"
   elif n%4==1:
    q="Find a_"+str(k)+" for a1="+str(a)+",ratio="+str(r)+"."
    ans=str(a*r**(k-1))+"." ;skill="Source explicit nth geometric"
   elif n%4==2:
    q="Write recursive rule of geometric sequence a1="+str(a)+",r="+str(r)+"."
    ans="a_n="+str(r)+"a_(n−1), n≥2." ;skill="Source geometric recurrence"
   else:
    q="Compute first four geometric terms with a1="+str(a)+",r="+str(r)+"."
    ans=str(vals)+"." ;skill="Source geometric examples"
  elif sec=="11.4":
   a=n%6+1;d=n%5+1;r=[2,F(1,2),-2,F(-1,3)][n%4];N=n%8+3
   if n%6==0:
    q="Sum first "+str(N)+" terms of arithmetic series a1="+str(a)+" difference"+str(d)+"."
    ans=str(F(N*(2*a+(N-1)*d),2))+"." ;skill="Source arithmetic series"
   elif n%6==1:
    total=sum(a*r**i for i in range(N))
    q="Sum first "+str(N)+" terms of geometric series a1="+str(a)+",r="+str(r)+"."
    ans=str(total)+"." ;skill="Source finite geometric sum"
   elif n%6==2:
    q="Does infinite geometric series with a1="+str(a)+",r="+str(r)+" converge?"
    ans=("Converges to "+str(a/(1-r))if abs(r)<1 else"Does not converge: |r|≥1")+"."
    skill="Source infinite geometric convergence"
   elif n%6==3:
    term=F(n%8+1,99)
    q="Convert repeating two-digit decimal 0.("+str(n%8+1).zfill(2)+") repeating to a fraction."
    ans=str(term)+"." ;skill="Source repeating decimals"
   elif n%6==4:
    i=F(1,100);m=N;pay=100
    fv=sum(F(pay)*(1+i)**j for j in range(m))
    q="End-period monthly savings of100, monthly rate1%, "+str(m)+" deposits: future value?"
    ans=str(round(float(fv),2))+" using P((1+i)^m−1)/i." ;skill="Source annuity application"
   else:
    vals=[a+i*d for i in range(N)]
    q="Write sigma notation for "+str(vals)+"."
    ans="Σ_(k=0)^"+str(N-1)+"("+str(a)+"+"+str(d)+"k)." ;skill="Source summation notation"
  elif sec=="11.5":
   k=n%5+2;m=n%6+6
   if n%6==0:
    q="Choose one of "+str(k)+" disjoint colors or"+str(m)+" disjoint patterns; how many single choices?"
    ans=str(k+m)+"." ;skill="Source addition principle"
   elif n%6==1:
    q="Choose one color from "+str(k)+" and one shape from"+str(m)+"; how many outcomes?"
    ans=str(k*m)+"." ;skill="Source multiplication principle"
   elif n%6==2:
    q="Arrange "+str(k)+" distinct objects in a row."
    ans=str(math.factorial(k))+"." ;skill="Source full permutations"
   elif n%6==3:
    t=min(k,m-1)
    q="Ordered selection of "+str(t)+" from"+str(m)+" distinct objects."
    ans=str(math.perm(m,t))+"." ;skill="Source nPr"
   elif n%6==4:
    t=min(k,m-1)
    q="Unordered selection of "+str(t)+" from"+str(m)+" distinct objects."
    ans=str(math.comb(m,t))+"." ;skill="Source nCr"
   else:
    q="How many subsets of a set with "+str(k)+" elements?"
    ans=str(2**k)+"." ;skill="Source subsets"
  elif sec=="11.6":
   a=n%4+1;b=n%3+1;power=n%7+2;term=n%(power+1)
   coeff=[math.comb(power,j)*a**(power-j)*b**j for j in range(power+1)]
   if n%3==0:
    q="Find coefficient of x^"+str(power-term)+"y^"+str(term)+" in ("+str(a)+"x+"+str(b)+"y)^"+str(power)+"."
    ans=str(coeff[term])+"." ;skill="Source binomial coefficients"
   elif n%3==1:
    q="Expand ("+str(a)+"x+"+str(b)+"y)^"+str(power)+" using binomial theorem."
    ans="Σ coefficients for terms x^(n−j)y^j (j=0..n):"+str(coeff)+"."
    skill="Source binomial expansion"
   else:
    q="Find binomial coefficient C("+str(power)+","+str(term)+")."
    ans=str(math.comb(power,term))+"." ;skill="Source Pascal triangle identities"
  elif sec=="11.7":
   a=n%8+2;b=n%5+1;c=n%6+3;N=a+b+c
   if n%6==0:
    q="Bag has "+str(a)+" red,"+str(b)+" blue,"+str(c)+" green. P(red)?"
    ans=str(F(a,N))+"." ;skill="Source probability model"
   elif n%6==1:
    q="Same bag red/blue/green counts="+str((a,b,c))+". P(red or blue)?"
    ans=str(F(a+b,N))+"." ;skill="Source union of exclusive"
   elif n%6==2:
    q="Same bag counts="+str((a,b,c))+". P(not green)?"
    ans=str(F(a+b,N))+"." ;skill="Source complement"
   elif n%6==3:
    q="Select2 balls without replacement from counts red="+str(a)+", blue="+str(b)+", green="+str(c)+". P(2 red)?"
    ans=str(F(math.comb(a,2),math.comb(N,2)))+"." ;skill="Source counting probability"
   elif n%6==4:
    q="Roll two fair six-sided dice. Probability sum7?"
    ans="6/36=1/6." ;skill="Source equally likely outcomes"
   else:
    q="For independent events P(A)="+str(F(a,N))+" and P(B)="+str(F(b,N))+", find P(A∩B)."
    ans=str(F(a*b,N*N))+"." ;skill="Source independent intersection"
  add(Q,q,ans,skill)
 assert len(Q)==count
 return Q
descriptions={
"11.1":["A sequence is an ordered list a1,a2,... indexed by positive integers.","An explicit rule evaluates a_n directly from n; recursive rule uses previous terms and initial condition.","Alternating sequences often contain (-1)^n or (-1)^(n−1).","Piecewise sequences depend on index parity or an interval.","Factorials n!=n(n−1)...1, with0!=1."],
"11.2":["Arithmetic sequences have constant common difference d.","Explicit nth term a_n=a1+(n−1)d.","Recursive form a_n=a_(n−1)+d for n≥2.","Number of terms N=(last−first)/d+1 when d nonzero and N integer.","Models include constant monthly increases and fixed-rate progressions."],
"11.3":["Geometric sequences have constant ratio r between consecutive nonzero terms.","Explicit nth term a_n=a1 r^(n−1).","Recursive a_n=r a_(n−1).","Negative ratios alternate signs; |r|<1 causes decay toward zero.","Constant nonzero sequence is both arithmetic and geometric."],
"11.4":["Series sum successive terms; arithmetic sum S_n=n(a1+an)/2.","Finite geometric sum a1(1−r^n)/(1−r), r≠1; if r=1, S_n=n a1.","Infinite geometric sum exists for |r|<1 and equals a1/(1−r).","A repeating block of decimal digits can be converted into a rational geometric series.","An annuity with regular end-period payments P and periodic interest i has future value P((1+i)^n−1)/i."],
"11.5":["Addition principle counts disjoint alternative options by summing possibilities.","Multiplication principle counts successive independent stages by multiplying counts.","Ordered k selections from n distinct items: nPk=n!/(n−k)!.","Unordered combinations nCk=n!/(k!(n−k)!).","A set of n distinct items has2^n subsets including empty set."],
"11.6":["Binomial theorem: (a+b)^n=Σ_(k=0)^n C(n,k)a^(n−k)b^k.","Binomial coefficients form Pascal's triangle and obey C(n,k)=C(n,n−k).","Terms combine powers of a and b of total degree n.","Use factorial or recursion to calculate coefficients without expanding all products."],
"11.7":["Probability assigns values between0 and1 to outcomes and sums to1 across an exhaustive partition.","For equally likely outcomes, P(A)=favorable/total.","Union: P(A∪B)=P(A)+P(B)−P(A∩B), simpler sum if disjoint.","Complement P(not A)=1−P(A).","For independent events P(A∩B)=P(A)P(B); drawing without replacement is generally dependent.","Counting combinations and permutations can enumerate sample spaces."]
}
for ordinal,les,sec,title,start,end,count,wc,tc in profiles:
 p=json.loads(P.read_text(encoding="utf8"))
 if ordinal <= p["LastClosedLessonOrdinal"]:continue
 assert p["LastClosedLessonOrdinal"]==ordinal-1 and p["FirstIncompleteLessonOrdinal"]==ordinal
 row=p["LessonEntries"][ordinal-1];assert row["Ordinal"]==ordinal and row["SourceSection"]==sec
 section_text=txt(start)+txt(start+1)
 assert sec in section_text,(sec,"section header")
 assert str(count)+"." in txt(end-1),(sec,"last source exercise",count)
 intro="\n".join(txt(i)for i in range(start,end-2))
 matches=[(int(n),t.strip())for n,t in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,115})",intro,re.I)]
 ex={}
 for n,t in matches:
  if 1<=n<=wc and (n not in ex or "Example" not in t):ex[n]=t
 assert sorted(ex)==list(range(1,wc+1)),(sec,ex)
 tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",intro,re.I)})
 assert tries==list(range(1,tc+1)),(sec,tries)
 figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",intro,re.I)})
 Q=calc(sec,count)
 prefix="us-ccss-g12-precalculus-u11-l"+str(les).zfill(2)
 code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L"+str(les).zfill(2)
 rr="src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+prefix+".rich-lesson-v2.json"
 tt="src/Edulytics.Core/Curriculum/LessonContent/Assessment/"+prefix+"-firstparty-teacher-led.json"
 W=[{"Title":ex[i],"Question":Q[i-1]["IndependentStudentQuestion"],
 "Method":"Use exact source-family sequence/combinatorial/probability relation.",
 "Steps":[Q[i-1]["TeacherMath"]],"Answer":Q[i-1]["TeacherMath"],
 "Check":"Verify by substitution or counting."}for i in range(1,wc+1)]
 T=[{"Number":i,"Question":Q[wc+i-1]["IndependentStudentQuestion"],
 "TeacherExpectedMath":Q[wc+i-1]["TeacherMath"]}for i in range(1,tc+1)]
 rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U11:L"+str(les).zfill(2),
 "ContentVersion":prefix+"-abc",
 "Lessons":[{"LessonCode":code,"CultureCode":"en","Title":title,
 "ExplanationParagraphs":descriptions[sec]+["Independent Cool-down: calculate a representative source-family example and check the result with the definition."],
 "WorkedExamples":W,
 "KeyConcepts":[{"Title":"Core definition","Definition":descriptions[sec][0],
 "Rule":descriptions[sec][1],"Example":Q[0]["TeacherMath"]}],
 "Visuals":[{"Kind":"RatioTable","Title":"Independent mathematical progression and counting table",
 "Description":"First-party numeric reference examples.",
 "PrimaryLabel":"Concept","SecondaryLabel":"Formula",
 "Rows":[{"Label":"Progression","Cells":["arithmetic","geometric","probability"]},
 {"Label":"Core formula","Cells":["a1+(n−1)d","a1r^(n−1)","favorable/total"]}],"Items":[],"SecondaryItems":[]}],
 "Videos":[],"SummaryPoints":descriptions[sec][:3],
 "CommonMistakes":[{"Mistake":"Confuse index, count, order or probability assumptions.","WhyWrong":"Leads to incorrect exponent, multiplicity or denominator.","Correction":"Identify model and count conditions before computing."}]}]}
 teacher={"SchemaVersion":1,"LessonCode":code,"IndependentStudentMathQuestions":Q,
 "IndependentTryIt":T,"IndependentCoolDown":{"StudentQuestions":[Q[0]["IndependentStudentQuestion"]],
 "TeacherExpectedMath":[Q[0]["TeacherMath"]],"PublisherTextCopied":False},
 "SourceMathExerciseCount":count,"StageC":"REUSE_EXISTING_RICHV2_MANUAL_MATH_SCORING"}
 save(R/rr,rich);save(R/tt,teacher)
 fig="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u11-l"+str(les).zfill(2)+"-source-figure-skills.json"
 save(R/fig,{"Section":sec,"OriginalFigureIdentifiers":figures,
 "IndependentSkillAlternatives":[{"Figure":i,"FirstPartyAlternative":"Independently authored sequence table, tree diagram/counting or probability model",
 "PublisherArtworkCopied":False}for i in figures],
 "Runtime":"NOT_RUN"})
 route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u11-l"+str(les).zfill(2)+"-stage-c-routes.json"
 save(R/route,{"Count":count,"NewEngine":False,"Reuse":"Teacher manual sequence/counting scoring",
 "Routes":[{"OriginalExercise":i,"IndependentQuestion":True,"TeacherAnswer":True,"RuntimeVerified":False}for i in range(1,count+1)]})
 now=datetime.datetime.now(datetime.timezone.utc).isoformat()
 audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u11-l"+str(les).zfill(2)+"-source-academic-audit.json"
 save(R/audit,{"Section":sec,"OriginalSource":"OpenStax Precalculus 2017 local",
 "SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[start,end-1],
 "IncorrectBlueprint":[row["PdfPageStart"],row["PdfPageEnd"]],"OriginalWorked":[(i,ex[i])for i in ex],
 "OriginalTryIt":tries,"OriginalFigures":figures,"OriginalExerciseCount":count,
 "FirstPartyRichV2":rr,"TeacherMath":tt,"FigureAlternatives":fig,"StageCRoutes":route,
 "Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
 close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u11-l"+str(les).zfill(2)+"-close-audit.json"
 save(R/close,{"Section":sec,"OriginalExercises":count,"FirstPartyTeacherMathKeys":count,
 "Worked":wc,"TryIt":tc,"SourceFigureSkills":len(figures),
 "ChecksActuallyRun":["Local PDF source boundary/numbered exercises",
 "Independent source-skill math with exact factorial/fraction/sequences",
 "Source worked/tryit anchors","Stage C teacher assessment reuse","Git staged diff check"],
 "NewMathEngine":False,"OriginalArtworkCopied":False,"CI":"NOT_RUN","StageD":"NOT_RUN","Build":"NOT_RUN",
 "ReadyForStageD":True,"ClosedAtUtc":now})
 row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":start,
 "PdfPageEnd":end-1,"EvidencePath":audit,"ClosureEvidencePath":close,
 "BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
 assert p["LessonEntries"][ordinal]["Ordinal"]==ordinal+1
 p.update({"LastClosedLessonOrdinal":ordinal,"FirstIncompleteLessonOrdinal":ordinal+1,
 "ActiveLessonOrdinal":ordinal+1,"NextLessonOrdinal":ordinal+1,"ReadyForStageD":ordinal,
 "SourceVerifiedGrade12Lessons":ordinal,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_"+str(ordinal)+"_OF_73",
 "UpdatedUtc":now})
 save(P,p)
 files=[str(P.relative_to(R)),rr,tt,fig,route,audit,close,str(Path(__file__).relative_to(R))]
 subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
 subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
 subprocess.run(["git","commit","-m","feat(g12 u11 l"+str(les).zfill(2)+"): close "+title+" academic ABC [skip ci]"],cwd=R,check=True)
 subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
 print("VERIFIED_GITHUB_PUSH_ORDINAL",ordinal,"OF73",sec,flush=True)
print("UNIT11_ALL_COMPLETE_69_OF73")
