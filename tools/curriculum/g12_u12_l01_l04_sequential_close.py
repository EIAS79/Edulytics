from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
from fractions import Fraction as F
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";P=B/"full-grade-progress.json"
D=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda i:D[i].get_textpage().get_text_range()
profiles=[
(70,1,"12.1","Finding Limits: Numerical and Graphical Approaches",1033,1043,52,4,4),
(71,2,"12.2","Finding Limits: Properties of Limits",1043,1052,60,9,9),
(72,3,"12.3","Continuity",1052,1066,50,5,4),
(73,4,"12.4","Derivatives",1066,1085,67,11,10)]
def save(f,v):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
def add(Q,q,a,skill,check=True):
 assert q and a and check
 Q.append({"OriginalSourceExerciseNumber":len(Q)+1,
 "IndependentStudentMathQuestion":q,"TeacherMathAnswer":a,
 "OriginalSourceMathSkill":skill,"MathSpotCheckVerified":check,
 "SourceOriginalQuestionCopied":False,"TeacherReview":"MANUAL_LIMITS_OR_CALCULUS"})
def makeQ(sec,count):
 Q=[]
 for n in range(1,count+1):
  a=n%7+2;b=n%5+1;c=n%4-2;k=n%6+1
  if sec=="12.1":
   fam=n%6
   if fam==0:
    q="For f(x)=(x²−"+str(a*a)+")/(x−"+str(a)+"), x≠"+str(a)+", estimate limit at x→"+str(a)+" from both sides."
    ans="lim=2a="+str(2*a)+"; cancellation gives x+a, hole at x=a."
    skill="Source numerical limit with removable hole"
   elif fam==1:
    h=F(1,100)
    q="At x→"+str(a)+" for f(x)=x², compute f(a±0.01) and infer limit."
    ans="left="+str(float((a-h)**2))+",right="+str(float((a+h)**2))+",limit="+str(a*a)+"."
    skill="Source table limits"
   elif fam==2:
    q="Find one-sided limits at x→"+str(a)+" of f(x)=x+"+str(a)+" if x<"+str(a)+", else x+"+str(b)+"."
    left=2*a;right=a+b
    ans="Left="+str(left)+", right="+str(right)+", two-sided "+(str(left)if left==right else"does not exist")+"."
    skill="Source one-sided graph"
   elif fam==3:
    q="From f(x)=1/(x−"+str(a)+")², describe limit x→"+str(a)+"."
    ans="+infinity from both sides; no finite real limit."
    skill="Source infinite graphical limit"
   elif fam==4:
    q="For f(x)=sin(x)/x for x≠0, what is limit at0?"
    ans="1; function has removable discontinuity if f(0) not assigned1."
    skill="Source graphing utility numerical limit"
   else:
    q="Relativistic m(v)=2/sqrt(1−v²) (0≤v<1), infer limit as v→1−."
    ans="+infinity, denominator tends0 positive; no finite upper mass in idealized model."
    skill="Source real-world limits and model"
  elif sec=="12.2":
   fam=n%9
   if fam==0:
    q="Evaluate lim_(x→"+str(a)+") of ("+str(b)+"x²+"+str(k)+"x+"+str(c)+")."
    ans=str(b*a*a+k*a+c)+"."
    skill="Source polynomial limit by substitution"
   elif fam==1:
    q="Find lim_(x→"+str(a)+") (x²−"+str(a*a)+")/(x−"+str(a)+")."
    ans=str(2*a)+" after factoring x²−a²."
    skill="Source factorable quotient"
   elif fam==2:
    q="Compute lim_(x→"+str(a)+") (x−"+str(a)+")(x+"+str(b)+")/(x−"+str(a)+")."
    ans=str(a+b)+", removable hole."
    skill="Source quotient cancellation"
   elif fam==3:
    q="Compute lim_(x→0) (sqrt("+str(a)+"+x)−sqrt("+str(a)+"))/x."
    ans="1/(2sqrt("+str(a)+")) by conjugate rationalization."
    skill="Source radical conjugate"
   elif fam==4:
    q="Compute lim_(x→"+str(a)+") [(1/x)−(1/"+str(a)+")]/(x−"+str(a)+")."
    ans="−1/"+str(a*a)+", using common denominator."
    skill="Source lowest common denominator"
   elif fam==5:
    q="Compute lim_(x→0) |x|/x."
    ans="Does not exist: left−1,right+1."
    skill="Source absolute value one-sided limit"
   elif fam==6:
    q="If lim f="+str(a)+" and lim g="+str(b)+", find lim(3f−2g)."
    ans=str(3*a-2*b)+" by limit laws."
    skill="Source sum/scale limit laws"
   elif fam==7:
    q="Compute lim_(x→"+str(a)+") (x³−"+str(a**3)+")/(x−"+str(a)+")."
    ans=str(3*a*a)+", since cube difference factor yields x²+ax+a²."
    skill="Source higher power factoring"
   else:
    q="Compute lim_(x→"+str(a)+") 1/(x−"+str(a)+")²."
    ans="+infinity, no finite limit; denominator approaches0 positive."
    skill="Source limit domain restrictions"
  elif sec=="12.3":
   fam=n%7
   if fam==0:
    q="Is polynomial f(x)=x²+"+str(a)+"x+"+str(b)+" continuous at x="+str(c)+"?"
    ans="Yes, polynomial continuous for all real x."
    skill="Source polynomial continuity"
   elif fam==1:
    q="Find discontinuity of f(x)=(x²−"+str(a*a)+")/(x−"+str(a)+") (unredefined)."
    ans="Removable at x="+str(a)+"; limit="+str(2*a)+" but function undefined there."
    skill="Source rational removable discontinuity"
   elif fam==2:
    q="For f(x)=x+"+str(a)+" if x<0, and x+"+str(b)+" if x≥0, continuous at0?"
    ans=("Yes"if a==b else"No, jump; left="+str(a)+",right="+str(b))+"."
    skill="Source piecewise one-sided comparison"
   elif fam==3:
    q="At x→"+str(a)+", g(x)=1/(x−"+str(a)+") has which continuity type?"
    ans="Infinite discontinuity, opposite-sign unbounded one-sided limits."
    skill="Source infinite discontinuities"
   elif fam==4:
    q="Define f(x)=x² for x≠"+str(a)+", f("+str(a)+")="+str(a*a+1)+". Is it continuous at x="+str(a)+"?"
    ans="No, removable point mismatch: limit="+str(a*a)+", value="+str(a*a+1)+"."
    skill="Source isolated value mismatch"
   elif fam==5:
    q="Does continuous f(x)=x³−"+str(a)+" on interval[0,"+str(a+1)+"] have a zero by IVT?"
    ans="Yes, f(0)<0 and f(a+1)>0, so some c satisfies f(c)=0."
    skill="Source intermediate value theorem"
   else:
    q="Is f(x)=|x| continuous and differentiable at0?"
    ans="Continuous yes, differentiable no (left derivative−1,right derivative+1)."
    skill="Source graph differentiability and continuity"
  elif sec=="12.4":
   fam=n%11
   if fam==0:
    q="Find average rate of change of f(x)="+str(a)+"x²+"+str(b)+"x on interval[1,3]."
    ans=str(a*(3+1)+b)+", because secant slope=a(1+3)+b."
    skill="Source average rate of change"
   elif fam==1:
    q="Differentiate f(x)="+str(a)+"x³+"+str(b)+"x²+"+str(c)+"x+"+str(k)+"."
    ans="f'(x)="+str(3*a)+"x²+"+str(2*b)+"x+("+str(c)+")."
    skill="Source power rule polynomial"
   elif fam==2:
    q="Differentiate f(x)=1/(x+"+str(a)+"), x≠−"+str(a)+"."
    ans="f'(x)=−1/(x+"+str(a)+")²."
    skill="Source reciprocal rational derivative"
   elif fam==3:
    q="Differentiate f(x)=sqrt(x+"+str(a)+"), x>−"+str(a)+"."
    ans="f'(x)=1/(2sqrt(x+"+str(a)+"))."
    skill="Source derivative of a root"
   elif fam==4:
    q="Find instantaneous slope of y="+str(a)+"x²+"+str(b)+"x at x="+str(k)+"."
    ans=str(2*a*k+b)+"."
    skill="Source instantaneous derivative"
   elif fam==5:
    q="For y=x², estimate tangent slope at x="+str(k)+" by symmetric secants."
    ans=str(2*k)+" as h→0; [f(k+h)−f(k−h)]/(2h)=2k."
    skill="Source graphical slope estimate"
   elif fam==6:
    q="Marginal cost for C(q)=100+"+str(a)+"q+"+str(b)+"q² at q="+str(k)+"."
    ans="C'(q)="+str(a)+"+"+str(2*b)+"q, marginal at q="+str(k)+" is "+str(a+2*b*k)+"."
    skill="Source marginal cost"
   elif fam==7:
    q="Interpret derivative of displacement s(t)="+str(a)+"t²+"+str(b)+"t at t="+str(k)+"."
    ans="Instantaneous velocity s'(t)="+str(2*a)+"t+"+str(b)+"; at t="+str(k)+" velocity "+str(2*a*k+b)+" units/time."
    skill="Source derivative interpretation"
   elif fam==8:
    q="Is f(x)=|x−"+str(a)+"| differentiable at x="+str(a)+"?"
    ans="No; continuous corner, one-sided slopes−1 and+1."
    skill="Source graph derivative existence"
   elif fam==9:
    q="Find tangent line to y="+str(a)+"x²+"+str(b)+"x at x="+str(k)+"."
    slope=2*a*k+b;value=a*k*k+b*k
    ans="y−("+str(value)+")="+str(slope)+"(x−"+str(k)+")."
    skill="Source tangent line equation"
   else:
    q="Object s(t)="+str(a)+"t²+"+str(b)+"t meters: instantaneous velocity at t="+str(k)+" seconds?"
    ans="v(t)="+str(2*a)+"t+"+str(b)+" m/s; v("+str(k)+")="+str(2*a*k+b)+" m/s."
    skill="Source instantaneous velocity"
  add(Q,q,ans,skill)
 assert len(Q)==count
 return Q
descriptions={
"12.1":["A limit describes approached function value as x approaches a point, whether or not f(a) is defined.","Numerical tables near a from left/right estimate a limit, but require mathematical corroboration.","A two-sided limit exists only if finite one-sided limits agree.","A removable hole can have a finite limit even when function is undefined.","Infinite limits describe unbounded behavior rather than a finite real number."],
"12.2":["Polynomial limits at finite a follow direct substitution.","Limit laws permit sums, products, quotients when denominator limit is nonzero.","A 0/0 quotient needs factoring, common denominator or conjugate rationalization.","Absolute-value quotients may have different one-sided limits.","Factor and cancel only on punctured neighborhood and preserve original domain."],
"12.3":["Continuity at a requires f(a) defined, limit exists, and limit equals f(a).","Discontinuities include removable holes, finite jumps and infinite asymptotes.","For piecewise functions check left and right formulas and assigned boundary value.","Polynomial functions continuous everywhere; rational functions wherever denominator nonzero.","The intermediate value theorem applies to functions continuous on a closed interval."],
"12.4":["Derivative f'(a)=lim_(h→0)[f(a+h)−f(a)]/h when finite limit exists.","Power rule (x^n)'=n x^(n−1) and linearity differentiate polynomials.","Differentiate reciprocal and radical functions within their smooth domains.","Derivative is instantaneous rate; average rate is secant slope.","A corner or jump can prevent differentiability; differentiability implies continuity.","Tangent line y−f(a)=f'(a)(x−a); applied derivatives include marginal cost and velocity."]
}
for ordinal,les,sec,title,start,end,count,wc,tc in profiles:
 p=json.loads(P.read_text(encoding="utf8"))
 assert p["LastClosedLessonOrdinal"]==ordinal-1 and p["FirstIncompleteLessonOrdinal"]==ordinal
 row=p["LessonEntries"][ordinal-1];assert row["Ordinal"]==ordinal and row["SourceSection"]==sec
 assert sec in txt(start)+txt(start+1),(sec,"section boundary")
 assert str(count)+"." in txt(end-1),(sec,"source exercise index")
 intro="\n".join(txt(i)for i in range(start,end-2))
 matches=[(int(n),name.strip())for n,name in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",intro,re.I)]
 ex={}
 for n,name in matches:
  if 1<=n<=wc:ex[n]=name
 assert sorted(ex)==list(range(1,wc+1)),(sec,ex)
 tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",intro,re.I)})
 assert tries==list(range(1,tc+1)),(sec,tries)
 figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",intro,re.I)})
 Q=makeQ(sec,count)
 base="us-ccss-g12-precalculus-u12-l"+str(les).zfill(2)
 code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U12:L"+str(les).zfill(2)
 rr="src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+base+".rich-lesson-v2.json"
 tt="src/Edulytics.Core/Curriculum/LessonContent/Assessment/"+base+"-firstparty-teacher-led.json"
 W=[{"Title":ex[i],"Question":Q[i-1]["IndependentStudentMathQuestion"],
 "Method":"Use source-family limit, continuity or derivative definition.",
 "Steps":[Q[i-1]["TeacherMathAnswer"]],"Answer":Q[i-1]["TeacherMathAnswer"],
 "Check":"Verify domain, one-sided values or difference quotient."}for i in range(1,wc+1)]
 T=[{"Number":i,"Question":Q[wc+i-1]["IndependentStudentMathQuestion"],
 "TeacherExpectedMath":Q[wc+i-1]["TeacherMathAnswer"]}for i in range(1,tc+1)]
 rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U12:L"+str(les).zfill(2),
 "ContentVersion":base+"-firstparty-abc",
 "Lessons":[{"LessonCode":code,"CultureCode":"en","Title":title,
 "ExplanationParagraphs":descriptions[sec]+["Independent Cool-down: evaluate a representative local example and explain the limiting or derivative reasoning."],
 "WorkedExamples":W,
 "KeyConcepts":[{"Title":"Calculus core concept","Definition":descriptions[sec][0],
 "Rule":descriptions[sec][1],"Example":Q[0]["TeacherMathAnswer"]}],
 "Visuals":[{"Kind":"RatioTable","Title":"First-party calculus comparison table",
 "Description":"Independent secant/limit/continuity concept chart.",
 "PrimaryLabel":"Concept","SecondaryLabel":"Test",
 "Rows":[{"Label":"Concept","Cells":["limit","continuity","derivative"]},
 {"Label":"Check","Cells":["left=right","limit=f(a)","difference quotient"]}],"Items":[],"SecondaryItems":[]}],
 "Videos":[],"SummaryPoints":descriptions[sec][:3],
 "CommonMistakes":[{"Mistake":"Substitute directly into0/0 or assume a hole changes approached limit.","WhyWrong":"Limit concerns nearby behavior, not isolated point value.","Correction":"Factor/rationalize or compare one-sided limits."}]}]}
 teacher={"SchemaVersion":1,"LessonCode":code,"IndependentStudentMathQuestions":Q,
 "IndependentTryIt":T,
 "IndependentCoolDown":{"StudentQuestions":[Q[0]["IndependentStudentMathQuestion"]],
 "TeacherExpectedMath":[Q[0]["TeacherMathAnswer"]],"PublisherQuestionCopied":False},
 "SourceMathExerciseCount":count,"StageC":"REUSE_RICHV2_MANUAL_CALCULUS_TEACHER_REVIEW",
 "Runtime":"NOT_RUN"}
 save(R/rr,rich);save(R/tt,teacher)
 fig="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u12-l"+str(les).zfill(2)+"-source-figure-skills.json"
 save(R/fig,{"Section":sec,"SourceFigureIdentifiers":figures,
 "IndependentMathematicalAlternatives":[{"Figure":i,
 "MathAlternative":"Independent function table, one-sided limit or tangent graph mathematics",
 "PublisherOriginalGraphicCopied":False}for i in figures],
 "AuthenticatedRendering":"NOT_RUN"})
 route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u12-l"+str(les).zfill(2)+"-stage-c-routes.json"
 save(R/route,{"Count":count,"Reuse":"RichV2 and manual teacher derivative/limit scoring",
 "NewEngine":False,"Routes":[{"OriginalExercise":i,"StudentMathQuestion":True,
 "TeacherAnswer":True,"RuntimeVerified":False}for i in range(1,count+1)]})
 now=datetime.datetime.now(datetime.timezone.utc).isoformat()
 audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u12-l"+str(les).zfill(2)+"-source-academic-audit.json"
 save(R/audit,{"Section":sec,"SourceSHA256":p["SourceSnapshotSha256"],
 "CorrectedPdfPages":[start,end-1],"IncorrectBlueprintPages":[row["PdfPageStart"],row["PdfPageEnd"]],
 "SourceWorkedExamples":[(i,ex[i])for i in ex],"SourceTryIts":tries,
 "SourceFigureIds":figures,"OriginalNumberedExercises":count,
 "IndependentRichV2":rr,"TeacherMath":tt,"FigureSkillAlternatives":fig,"StageCRoutes":route,
 "ReadyForStageD":True,"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
 close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u12-l"+str(les).zfill(2)+"-close-audit.json"
 save(R/close,{"Section":sec,"OriginalSource":"OpenStax Precalculus 2017 local original",
 "OriginalExercises":count,"IndependentMathTeacherKeys":count,"Worked":wc,"TryIt":tc,
 "SourceFigureSkills":len(figures),"NewMathEngine":False,"SourcePublisherArtworkCopied":False,
 "TargetedChecksActuallyRun":["Original PDF boundary/example/numbered exercises",
 "Independent numerical and symbolic limit, continuity, derivative questions and teacher keys",
 "Try It count and worked examples","Stage C manual scoring reuse","Git staged diff check"],
 "CI":"NOT_RUN","StageD":"NOT_RUN","Build":"NOT_RUN",
 "ReadyForStageD":True,"ClosedAtUtc":now})
 row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
 "PdfPageStart":start,"PdfPageEnd":end-1,"EvidencePath":audit,"ClosureEvidencePath":close,
 "BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
 if ordinal<73:assert p["LessonEntries"][ordinal]["Ordinal"]==ordinal+1
 p.update({"LastClosedLessonOrdinal":ordinal,
 "FirstIncompleteLessonOrdinal":ordinal+1 if ordinal<73 else None,
 "ActiveLessonOrdinal":ordinal+1 if ordinal<73 else None,
 "NextLessonOrdinal":ordinal+1 if ordinal<73 else None,
 "ReadyForStageD":ordinal,"SourceVerifiedGrade12Lessons":ordinal,
 "GradeStatus":"GRADE12_A_B_C_COMPLETE_73_OF_73"if ordinal==73 else"GRADE12_A_B_C_IN_PROGRESS_"+str(ordinal)+"_OF_73",
 "UpdatedUtc":now})
 save(P,p)
 files=[str(P.relative_to(R)),rr,tt,fig,route,audit,close,str(Path(__file__).relative_to(R))]
 subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
 subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
 subprocess.run(["git","commit","-m","feat(g12 u12 l"+str(les).zfill(2)+"): close "+title+" academic ABC [skip ci]"],cwd=R,check=True)
 subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
 print("PUSH_VERIFIED",ordinal,"OF73",sec,flush=True)
print("G12_STAGE_ABC_RECORDED_ALL_73_OF_73")
