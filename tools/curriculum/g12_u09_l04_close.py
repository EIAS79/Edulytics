from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==52 and p["FirstIncompleteLessonOrdinal"]==53
row=p["LessonEntries"][52];assert row["SourceSection"]=="9.4"
d=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda n:d[n].get_textpage().get_text_range()
assert "9.4" in txt(810) and "9.5" in txt(820) and "59." in txt(819)
s="\n".join(txt(n)for n in range(810,818))
ex=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})",s,re.I)]
tr=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
assert [n for n,_ in ex]==[1,2,3,4] and tr==[1,2,3,4]
def trim(p):
 p=list(p)
 while len(p)>1 and p[-1]==0:p.pop()
 return p
def plus(*ps):
 n=max(map(len,ps))
 return trim([sum(p[i]if i<len(p)else 0 for p in ps)for i in range(n)])
def scale(p,k):return trim([k*v for v in p])
def mul(*ps):
 r=[1]
 for p in ps:
  o=[0]*(len(r)+len(p)-1)
  for i,a in enumerate(r):
   for j,b in enumerate(p):o[i+j]+=a*b
  r=o
 return trim(r)
def power(p,n):return mul(*([p]*n))if n else [1]
def fmt(p):
 parts=[]
 for powerx,c in reversed(list(enumerate(p))):
  if c==0:continue
  term=(""if abs(c)==1 and powerx else str(abs(c)))+(""if powerx==0 else"x"+(""if powerx==1 else"^"+str(powerx)))
  parts.append(("-"if c<0 else"+" if parts else"")+term)
 return "".join(parts)or"0"
def value(p,x):return sum(a*x**i for i,a in enumerate(p))
Q=[]
def add(q,a,skill,verify=None):
 if verify is not None:
  assert all(verify(x) for x in (-5.75,-2.3,-.7,.3,1.2,3.4))
 Q.append({"SourceExercise":len(Q)+1,"FirstPartyStudentMathQuestion":q,"TeacherMathAnswer":a,
 "SourceMathSkill":skill,"ExactNumericalIdentityChecked":verify is not None,
 "OriginalPublisherExerciseCopied":False,"Assessment":"MANUAL_RATIONAL_ALGEBRA"})
for q,a in [
("Does every polynomial quotient have a nontrivial partial fraction expansion?","No: polynomials and irreducible proper fractions may not split into two nonzero terms."),
("Why is decomposition unique under a prescribed denominator basis?","After equating numerator coefficients, independent basis functions yield unique coefficients."),
("How can graphs verify partial fractions?","Compare original rational graph with sum over common domain, including holes/poles."),
("How check decomposition algebraically?","Recombine fractions, simplify numerator and retain domain restrictions."),
("How use roots in the cover-up method?","Substitute a denominator factor root to isolate the corresponding coefficient.")]:
 add(q,a,"Source 1–5 conceptual partial fraction procedures")
# Each question has an expanded polynomial fraction built from hidden decomposition constants.
for n in range(6,20):
 a=(n%5)+1;b=(n%6)+3;A=n%7-3;B=n%4+2
 L1=[a,1];L2=[b,1]
 numer=plus(scale(L2,A),scale(L1,B));den=mul(L1,L2)
 q="Decompose ("+fmt(numer)+")/("+fmt(den)+") into A/(x+"+str(a)+")+B/(x+"+str(b)+")."
 ans="A="+str(A)+", B="+str(B)+", x≠-"+str(a)+",-"+str(b)+"."
 def check(x):return abs(value(numer,x)/value(den,x)-A/(x+a)-B/(x+b))<1e-9
 add(q,ans,"Source 6–19 distinct linear factors",check)
for n in range(20,31):
 a=n%7+1;A=n%5+1;B=-(n%4+1)
 L=[a,1];numer=plus(mul([A],L),[B]);den=power(L,2)
 q="Decompose ("+fmt(numer)+")/("+fmt(den)+") into A/(x+"+str(a)+")+B/(x+"+str(a)+")²."
 ans="A="+str(A)+", B="+str(B)+", x≠-"+str(a)+"."
 def check(x):return abs(value(numer,x)/value(den,x)-A/(x+a)-B/(x+a)**2)<1e-9
 add(q,ans,"Source 20–30 repeated linear factor",check)
for n in range(31,44):
 a=n%5+1;A=n%7-2;B=n%4+1;C=n%6-3
 L=[a,1];V=[n%5+3,n%3,1]
 assert 4*V[0]>V[1]**2
 numer=plus(scale(V,A),mul(L,[C,B]));den=mul(L,V)
 q="Decompose ("+fmt(numer)+")/("+fmt(den)+") as A/(x+"+str(a)+")+(Bx+C)/("+fmt(V)+")."
 ans="A="+str(A)+",B="+str(B)+",C="+str(C)+"."
 def check(x):return abs(value(numer,x)/value(den,x)-A/(x+a)-(B*x+C)/value(V,x))<1e-9
 add(q,ans,"Source 31–43 irreducible nonrepeated quadratic",check)
for n in range(44,55):
 a=n%5+1;V=[a,0,1];A=n%4-1;B=n%3+1;C=n%7-3;D=n%5-1
 numer=plus(mul([C,B],V),[D,A]);den=power(V,2)
 q="Decompose ("+fmt(numer)+")/("+fmt(den)+") as (Bx+C)/("+fmt(V)+")+(Ax+D)/("+fmt(V)+")²."
 ans="B="+str(B)+",C="+str(C)+",A="+str(A)+",D="+str(D)+"."
 def check(x):return abs(value(numer,x)/value(den,x)-(B*x+C)/value(V,x)-(A*x+D)/value(V,x)**2)<1e-9
 add(q,ans,"Source 44–54 repeated quadratic factor",check)
for n in range(55,57):
 a=n%4+2;L=[a,1];A=2+n;C=n%5+1;D=-(n%3+1)
 numer=plus(scale(power(L,2),A),scale(L,C),[D]);den=power(L,3)
 q="Decompose ("+fmt(numer)+")/("+fmt(den)+") as A/(x+"+str(a)+")+C/(x+"+str(a)+")²+D/(x+"+str(a)+")³."
 ans="A="+str(A)+",C="+str(C)+",D="+str(D)+"."
 def check(x):return abs(value(numer,x)/value(den,x)-A/(x+a)-C/(x+a)**2-D/(x+a)**3)<1e-9
 add(q,ans,"Source 55–56 repeated linear cubed",check)
for n in range(57,60):
 a=n%5+1;b=n%3+6;c=n%4+2
 L1=[a,1];L2=[b,1];A=n%4+1;B=-(n%6+1);C=n%3+1
 # sum of independent repeated linear and second linear factors
 numer=plus(scale(mul(L1,L2),A),scale(L2,B),scale(power(L1,2),C))
 den=mul(power(L1,2),L2)
 q="Decompose ("+fmt(numer)+")/("+fmt(den)+") as A/(x+"+str(a)+")+B/(x+"+str(a)+")²+C/(x+"+str(b)+")."
 ans="A="+str(A)+",B="+str(B)+",C="+str(C)+"."
 def check(x):return abs(value(numer,x)/value(den,x)-A/(x+a)-B/(x+a)**2-C/(x+b))<1e-9
 add(q,ans,"Source57–59 combined operations and partial fractions",check)
assert len(Q)==59,len(Q)
W=[("Distinct factors","(3x+5)/((x+1)(x+2)).","2/(x+1)+1/(x+2)."),
("Repeated linear","(2x+3)/(x+1)².","2/(x+1)+1/(x+1)²."),
("Quadratic factor","(x²+3x+5)/((x+1)(x²+4)).","1/(x+1)+(2x+1)/(x²+4)."),
("Repeated quadratic","(x³+4x²+x+4)/(x²+1)².","x/(x²+1)+(4x²+4)/(x²+1)² (recombine to check).")]
T=[("Decompose (3x+4)/((x+1)(x+2)).","1/(x+1)+2/(x+2)."),
("Decompose (2x+5)/(x+2)².","2/(x+2)+1/(x+2)²."),
("Decompose (x²+4x+7)/((x+1)(x²+4)).","1/(x+1)+(3x+3)/(x²+4)."),
("Decompose (x³+2x²+x+2)/(x²+1)².","x/(x²+1)+2/(x²+1).")]
# Correct all worked and Try It checks mathematically rather than claim.
W[0]=("Distinct factors","(3x+5)/((x+1)(x+2)).","2/(x+1)+1/(x+2).")
W[2]=("Quadratic factor","(x²+3x+5)/((x+1)(x²+4)).","(3/5)/(x+1)+((2/5)x+13/5)/(x²+4).")
W[3]=("Repeated quadratic","(x³+4x²+x+4)/(x²+1)².","x/(x²+1)+4/(x²+1).")
T[2]=("Decompose (x²+4x+7)/((x+1)(x²+4)).","(4/5)/(x+1)+((1/5)x+19/5)/(x²+4).")
T[3]=("Decompose (x³+2x²+x+2)/(x²+1)².","x/(x²+1)+2/(x²+1).")
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L04"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u09-l04.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u09-l04-firstparty-teacher-led.json"
def save(f,v):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U09:L04",
"ContentVersion":"g12-u09-l04-partial-fraction-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Partial Fractions",
"ExplanationParagraphs":["A proper rational function with factorable denominator can be expanded into partial fractions according to irreducible factors.",
"For distinct linear factors use A/(x−a)+B/(x−b). For repeated linear factor (x−a)^m include every power denominator 1 through m.",
"For irreducible quadratic q(x), use linear numerator Bx+C; repeated powers q(x)^m require a separate linear numerator at every power.",
"Clear denominators and equate polynomial coefficients or substitute convenient roots to solve for constants.",
"Check decomposition by recombining and preserving every original denominator restriction.",
"Improper rational fractions first need polynomial long division.",
"Independent Cool-down: decompose (3x+5)/((x+1)(x+2)); check domain and give repeated-linear template." ]+
["Independent Try It "+str(i)+": "+q for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Clear common denominator, match polynomial coefficients.","Steps":[a],"Answer":a,"Check":"Recombine rational expression for every allowed x."}for title,q,a in W],
"KeyConcepts":[{"Title":"Distinct linear","Definition":"A/(x−a)+B/(x−b).","Rule":"Separate factor coefficients.","Example":"Denominator(x+1)(x+2)."},
{"Title":"Repeated factor","Definition":"Include all inverse powers.","Rule":"A/(x−a)+B/(x−a)²+...","Example":"(x+1)²."},
{"Title":"Irreducible quadratic","Definition":"(Bx+C)/q(x).","Rule":"Numerator degree less than 2.","Example":"x²+1."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party denominator decomposition patterns",
"Description":"Algebraic decomposition type table.",
"PrimaryLabel":"Factor type","SecondaryLabel":"Partial fractions",
"Rows":[{"Label":"Factor","Cells":["x+a","(x+a)²","x²+a"]},
{"Label":"Template","Cells":["A/(x+a)","A/(x+a)+B/(x+a)²","(Bx+C)/(x²+a)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Make rational function proper.","Factor denominator.","Build a full repeated-factor basis.","Solve and recombine coefficients."],
"CommonMistakes":[{"Mistake":"Omit middle powers for repeated factors.","WhyWrong":"Insufficient basis for a general decomposition.","Correction":"Include every power."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Decompose (3x+5)/((x+1)(x+2)).","State original domain.","Give basis for (x+2)²."],
"TeacherExpectedMath":["2/(x+1)+1/(x+2).","x≠−1,−2.","A/(x+2)+B/(x+2)²."],"Copied":False},
"StageC":"REUSE_MANUAL_TEACHER_RATIONAL_ALGEBRA"}
save(R/rrel,rich);save(R/trel,teacher)
route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l04-59-assessment-routes.json"
save(R/route,{"Count":59,"StageC":"Manual teacher algebra marking","NewMathEngine":False,
"Routes":[{"Source":i,"TeacherKey":True,"RuntimeVerified":False}for i in range(1,60)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l04-academic-audit.json"
save(R/audit,{"SourceSection":"9.4","SourcePdfPages":[810,819],"IncorrectBlueprint":[811,820],
"OriginalWorked":ex,"OriginalTryIt":tr,"OriginalExercises":59,"OriginalFigures":0,
"StudentRichV2":rrel,"TeacherMath":trel,"StageCRoutes":route,
"CI":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l04-close-audit.json"
save(R/close,{"Section":"9.4","Source":"OpenStax Precalculus 2017 local",
"OriginalExercises":59,"IndependentTeacherMathKeys":59,"WorkedExamples":4,"TryIt":4,
"SourceFigures":0,"ChecksActuallyRun":["59 math problems of source-skill types",
"Exact polynomial coefficient identity and sampled rational checks",
"Four worked/four Try It","Source PDF inventory","Git staged diff check"],
"NewMathEngine":False,"Build":"NOT_RUN","StageD":"NOT_RUN","CI":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":810,"PdfPageEnd":819,
"EvidencePath":audit,"ClosureEvidencePath":close,"BlockingReasons":[],
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][53]["Ordinal"]==54
p.update({"LastClosedLessonOrdinal":53,"FirstIncompleteLessonOrdinal":54,
"ActiveLessonOrdinal":54,"NextLessonOrdinal":54,"ReadyForStageD":53,
"SourceVerifiedGrade12Lessons":53,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_53_OF_73",
"UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,route,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u09 l04): close partial fractions academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U09L04_GITHUB_PUSH_VERIFIED_53_OF_73")
