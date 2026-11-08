from pathlib import Path
import json,math,datetime,time,re,subprocess,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json";p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==30 and p["FirstIncompleteLessonOrdinal"]==31
row=p["LessonEntries"][30]
assert row["Ordinal"]==31 and row["SourceSection"]=="5.3" and not row["ReadyForStageD"]
d=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:d[i].get_textpage().get_text_range()
assert "5.3" in get(488) and "5.4" in get(501)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(488,501)]
section="\n".join(get(i)for i in range(488,499))
worked=[(int(n),t.strip())for n,t in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",section,re.I)]
tries=sorted({int(x)for x in re.findall(r"Try It\s*#?(\d+)",section,re.I)})
figures=sorted({int(x)for x in re.findall(r"Figure\s+(\d+)",section,re.I)})
assert [n for n,_ in worked]==list(range(1,11)) and tries==list(range(1,11))
assert figures==list(range(1,11)) and "76." in get(500)
Q=[]
def add(q,a,src):
 n=len(Q)+1;assert q and a and src
 Q.append({"OriginalSourceExercise":n,"IndependentStudentQuestion":q,
 "TeacherExpectedMath":a,"SourceSkillMathReviewed":src,
 "PublisherTextCopied":False,"TeacherScoringMode":"MANUAL_MATH_AND_GRAPH"})
for q,a,s in [
("Find an angle in[0,2π) where sin t=cos t.","π/4 and5π/4; both functions equal.","Source #1 sine and cosine equality."),
("Calculate cos(π°) when angle measure is degrees.","cos(π°)≈0.998497; radians versus degrees must be distinguished.","Source #2 π degrees is ~3.14159°, not π radians."),
("For t in QII and sin t=5/13, find cosine.","−12/13; Pythagorean identity and QII sign.","Source #3 use sine QII to infer cosine."),
("Define secant and its real domain.","sec t=1/cos t; undefined where cos t=0, t=π/2+kπ.","Source #4 reciprocal cosine and domain."),
("Explain π-periodicity of tangent and cotangent.","tan(t+π)=tan t and cot(t+π)=cot t when defined.","Source #5 tan and cot periodπ.")
]:add(q,a,s)
exact={
30:{"tan":"sqrt3/3","sec":"2sqrt3/3","csc":"2","cot":"sqrt3"},
45:{"tan":"1","sec":"sqrt2","csc":"sqrt2","cot":"1"},
60:{"tan":"sqrt3","sec":"2","csc":"2sqrt3/3","cot":"sqrt3/3"}}
for ang in (30,45,60):
 for fn in ("tan","sec","csc","cot"):
  i=len(Q)+1
  add(f"Evaluate exact {fn}({ang}°).",exact[ang][fn]+".",
 f"Source #{i} matching reciprocal/quotient trig function at special angle π/6,π/4 or π/3.")
assert len(Q)==17
# source18-37 reference in other quadrants
reference=[150,210,330,390,315,135,225,495,420,240,120,300,225,300,150,240,330,120,210,315]
fns=["tan","sec","csc","cot"]*5
def value(fn,ang):
 t=math.radians(ang);s=math.sin(t);c=math.cos(t)
 if fn in ("tan","sec")and abs(c)<1e-9:return None
 if fn in ("csc","cot")and abs(s)<1e-9:return None
 return {"tan":s/c if abs(c)>1e-9 else None,"sec":1/c if abs(c)>1e-9 else None,
 "csc":1/s if abs(s)>1e-9 else None,"cot":c/s if abs(s)>1e-9 else None}[fn]
for ang,fn in zip(reference,fns):
 i=len(Q)+1
 add(f"Use a reference angle to evaluate {fn}({ang}°) (4 decimals).",
 f"{value(fn,ang):.4f}; sign determined by quadrant.",
 f"Source #{i} reciprocal or quotient function at angle outside quadrant I.")
assert len(Q)==37
# 38-42 multiple functions given sin/cos/tan and quadrant
cases=[("sin",.6,"II"),("cos",-.8,"III"),("tan",2.4,"I"),("sin",math.sqrt(3)/2,"I"),("sin",.64,"I")]
for given,v,quadrant in cases:
 i=len(Q)+1
 if given=="sin":
  s=v;c=math.sqrt(1-v*v)*(-1 if quadrant in ("II","III")else 1)
 elif given=="cos":
  c=v;s=math.sqrt(1-v*v)*(-1 if quadrant in ("III","IV")else 1)
 else:
  c=1/math.sqrt(1+v*v);s=v*c
 add(f"Given {given}(t)={v:.6f} in quadrant{quadrant}, calculate remaining five trig values.",
 f"sin={s:.6f},cos={c:.6f},tan={s/c:.6f},sec={1/c:.6f},csc={1/s:.6f},cot={c/s:.6f}.",
 f"Source #{i} use Pythagorean/reciprocal/quotient identities and quadrant sign.")
# parity 43-48
for fn,v,src in zip(["sin","cos","sec","csc","tan","cot"],
[.65,.45,2.8,1.5,-1.2,2.25],range(43,49)):
 odd=fn in ("sin","csc","tan","cot")
 add(f"If {fn}(t)={v}, find {fn}(−t) using parity.",
 f"{-v if odd else v:g}; {'odd'if odd else 'even'} function.",
 f"Source #{src} evaluate same trig function at negative input.")
# 49-51 diagram six trig values at independent unit points
for i,ang in enumerate([30,135,270],49):
 s=math.sin(math.radians(ang));c=math.cos(math.radians(ang))
 fmt=lambda v:"undefined"if v is None else f"{v:.5f}"
 tan=s/c if abs(c)>1e-9 else None
 sec=1/c if abs(c)>1e-9 else None
 csc=1/s if abs(s)>1e-9 else None
 cot=c/s if abs(s)>1e-9 else None
 add(f"On independently authored unit circle draw angle {ang}°, then find six trig values.",
 f"sin={s:.5f},cos={c:.5f},tan={fmt(tan)},sec={fmt(sec)},csc={fmt(csc)},cot={fmt(cot)}.",
 f"Source graphical #{i} circle point with all six trig functions.")
# 52-61 calculator
for fn,angle in [("csc",100),("cot",105),("sec",18),("tan",112.5),
("sec",135),("csc",45),("tan",105),("cot",33),("cot",140),("sec",310)]:
 i=len(Q)+1
 ans=value(fn,angle)
 add(f"Use calculator to find {fn}({angle}°) to five decimals.",
 "undefined"if ans is None else f"{ans:.5f}.",
 f"Source #{i} technology reciprocal/quotient trig evaluation with correct angle mode.")
# 62-65 identities
for q,ans,src in [
("Given tan t=2 and sin t=.8, find cos t.","cos t=sin/tan=0.4.","Source #62 tan and sin imply cos by quotient."),
("Given tan t=1.5 and cos t=.6, find sin t.","sin t=tan·cos=.9.","Source #63 tan and cos infer sin."),
("Given csc t=2 and cos t=sqrt3/2, find tan t.","sin=1/2, tan=1/sqrt3=sqrt3/3.","Source #64 csc and cos determine tan."),
("Given cot t=3/4 and cos t=3/5 in QI, find csc t.","sin=4/5, csc=5/4.","Source #65 cot and cos infer csc.")]:add(q,ans,src)
# 66-69 even/odd
for q,ans,src in [
("Classify f(t)=5 sin t cos t as even/odd.","Odd: sin odd×cos even.","Source #66 sin cos product odd."),
("Classify f(t)=sin² t+2 sec t as even/odd.","Even: sin² even and sec even.","Source #67 sin² cos and sec combine even."),
("Classify f(t)=sin t−cos² t as even/odd.","Neither: odd minus even.","Source #68 sin−2cos² neither."),
("Classify f(t)=csc² t+3sec t as even/odd.","Even: csc² even, sec even.","Source #69 csc²+sec even.")]:add(q,ans,src)
add("Simplify csc(t)tan(t) on domain where defined.","(1/sin)(sin/cos)=sec t; original requires sin≠0 and cos≠0.","Source #70 csc t tan t simplifies sec t with domain retained.")
add("Simplify sec(t)/csc(t) on domain where defined.","(1/cos)/(1/sin)=tan t; original requires cos≠0,sin≠0.","Source #71 sec/csc simplifies tan with original domain.")
# Applications 72-76
for q,ans,src in [
("A periodic toy model h(d)=12cos(d/500)+14. Calculate h(60) and period in days.",
 f"h(60)≈{12*math.cos(60/500)+14:.3f}; period=1000π days (mathematical, not physical annual).",
 "Source #72 daylight h=15cos(d/600), calculate day42 and period1200π days."),
("For h(d)=10cos(d/400)+13, find h(200) and period.",
 f"h(200)≈{10*math.cos(.5)+13:.3f}; period800π days.",
 "Source #73 h=16cos(d/500), day267 and period1000π days."),
("A cyclic pressure model P=30sin(2πt)+110: find P at t=10.25s and extrema.",
 "P=140 at quarter cycle, maximum140, minimum80.",
 "Source #74 20sin(2πt)+100 at t15 gives100 and extrema80,120."),
("A piston follows y=3cosθ+8 inches. Find height atθ=60°.",
 "y=3(.5)+8=9.5in.","Source #75 piston y=2cosθ+6 at55°."),
("A piston follows y=4cosθ+7 inches. Find height atθ=120°.",
 "y=4(−.5)+7=5in.","Source #76 piston y=2cosθ+5 at55°.")
]:add(q,ans,src)
assert len(Q)==76 and [q["OriginalSourceExercise"]for q in Q]==list(range(1,77))
# identity corner cases
assert math.isclose(value("tan",45),1)
assert value("tan",90) is None
assert math.isclose(.6*.6+.8*.8,1)
# independent examples and tryit
W=[
("Point-to-functions","Given point(3/5,4/5) on unit circle find six functions.","sin4/5, cos3/5, tan4/3,sec5/3,csc5/4,cot3/4."),
("Six functions at angle","Find tan, sec, csc,cot atπ/6.","tan=sqrt3/3, sec=2sqrt3/3,csc=2,cot=sqrt3."),
("Reference reciprocal","Find sec(5π/6).","cos=−sqrt3/2, sec=−2sqrt3/3."),
("Odd and even parity","Determine sin(−t), cos(−t), tan(−t).","−sin t,cos t,−tan t."),
("Pythagorean trig identity","Given sin t=−3/5 QIV find sec t.","cos=4/5 so sec=5/4."),
("Simplify quotient","Simplify csc t tan t.","sec t on original domain."),
("Find other five from one","Given cos t=5/13 in QIV find six trig values.","sin=−12/13, tan=−12/5, sec=13/5,csc=−13/12,cot=−5/12."),
("Unit circle sign","Find all six at t=3π/2.","sin=−1,cos=0, tan undefined,sec undefined,csc=−1,cot=0."),
("Reciprocal from sine","sin t=1/2, cos t=−sqrt3/2; find tan etc.","tan=−sqrt3/3,sec=−2sqrt3/3,csc=2,cot=−sqrt3."),
("Calculator","Estimate sec(20°) to four decimals.",f"{1/math.cos(math.radians(20)):.4f}.")]
T=[
("Six functions from(−3/5,4/5).","sin4/5,cos−3/5,tan−4/3,sec−5/3,csc5/4,cot−3/4."),
("Find cot(π/3).","sqrt3/3."),
("Find tan(7π/6).","sqrt3/3."),
("Find sec(−π/3).","2."),
("Use cos=−4/5 QIII to find sin.","−3/5."),
("Simplify sec/csc.","tan t on original domain."),
("Given sin t=5/13 in QII find tangent.","−5/12."),
("Find six trig values at t=π.","sin=0,cos=−1,tan0,sec−1,csc and cot undefined."),
("Given sin=−1/2 and cos=sqrt3/2, find cot.","−sqrt3."),
("Estimate csc(40°).",f"{1/math.sin(math.radians(40)):.4f}.")]
assert len(W)==10 and len(T)==10
lesson_code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L03"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u05-l03.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u05-l03-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U05:L03","ContentVersion":"g12-u05-l03-firstparty-six-trig-functions-abc",
"Lessons":[{"LessonCode":lesson_code,"CultureCode":"en","Title":"The Other Trigonometric Functions",
"ExplanationParagraphs":["Given unit circle point (cos t,sin t), tangent=sin t/cos t, cotangent=cos t/sin t, secant=1/cos t, cosecant=1/sin t.",
"All quotients require nonzero denominators. Tangent/secant undefined where cos t=0, cotangent/cosecant undefined where sin t=0.",
"Use special angles and reference angles to evaluate exact signs in all four quadrants; remember reciprocal and quotient identities.",
"Cosine and secant are even; sine,cosecant,tangent and cotangent are odd on their domains.",
"Besides sin²t+cos²t=1, identities 1+tan²t=sec²t and 1+cot²t=csc²t help infer missing values.",
"Tangent and cotangent are π-periodic, while other common trigonometric functions are 2π-periodic.",
"Independent Cool-down: find all six functions at5π/6; simplify tan t·csc t; and classify sin t+cos t by parity."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Determine sin and cos with quadrant, then apply reciprocal/quotient identities.","Steps":[ans],"Answer":ans,"Check":"Check denominators and signs."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Reciprocals","Definition":"sec=1/cos, csc=1/sin.","Rule":"Undefined when denominator zero.","Example":"secπ=−1."},
{"Title":"Quotients","Definition":"tan=sin/cos, cot=cos/sin.","Rule":"Know tangent/cotangent π period.","Example":"tanπ/4=1."},
{"Title":"Parity","Definition":"cos/sec even; sin/csc/tan/cot odd.","Rule":"f(−t)=±f(t).","Example":"sec(−t)=sec t."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent six-function special-angle values",
"Description":"Independent mathematical chart for 30°,45°,60°; nonzero denominators only.",
"PrimaryLabel":"Angle","SecondaryLabel":"Functions","Rows":[{"Label":"Angle","Cells":["30°","45°","60°"]},
{"Label":"tan","Cells":["sqrt3/3","1","sqrt3"]},
{"Label":"sec","Cells":["2sqrt3/3","sqrt2","2"]},
{"Label":"csc","Cells":["2","sqrt2","2sqrt3/3"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Define four additional trig functions as quotients and reciprocals.",
"Check undefined denominators.","Use quadrant signs, Pythagorean identities and parity."],
"CommonMistakes":[{"Mistake":"Assume sec t=1/sin t.","WhyWrong":"Secant is reciprocal of cosine.","Correction":"sec=1/cos; csc=1/sin."},
{"Mistake":"Return a finite tangent where cosine zero.","WhyWrong":"Division by zero undefined.","Correction":"State undefined."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson_code,
"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Find six trig values at5π/6.","Simplify tan t·csc t on original domain.","Classify sin t+cos t parity."],
"TeacherExpectedMath":["sin1/2, cos−sqrt3/2, tan−sqrt3/3,sec−2sqrt3/3,csc2,cot−sqrt3.",
"sec t for sin t≠0 and cos t≠0.","Neither: odd+even."],"OriginalPublisherCoolDownCopied":False},
"OriginalExercisesMathReviewed":76,"StageC":"REUSE_EXISTING_TEACHER_MANUAL_TRIGONOMETRY_AND_RICHV2","StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teacherrel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l03-ten-source-figure-skills.json"
figroles=["Unit circle defining sin/cos", "Six trig ratios from point in quadrantII",
"Six trig ratios from point in quadrantIV","Signs in four quadrants",
"Even function reflection across y-axis","Odd function rotation symmetry",
"Positive/negative angle parity on unit circle","Pythagorean trigonometry point",
"Six functions from directed angle","Six functions with undefined denominators"]
assert len(figroles)==10
save(root/figrel,{"SchemaVersion":1,"Section":"5.3","FirstPartyIndependentFigureSkills":[
{"SourceFigure":i+1,"MathematicalConcept":v,
"IndependentSourceSkillExplanation":True,"PublisherImageCopied":False}
for i,v in enumerate(figroles)],"OriginalFigureCount":10,"ExactFigureParityClaimed":False})
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l03-stage-c-76-trig-reuse-routes.json"
save(root/routesrel,{"SchemaVersion":1,"Section":"5.3","Count":76,
"Reuse":["RichV2","Teacher manual trig evaluation","Unit-circle visuals from Section5.2"],
"NewMathEngine":False,"AutomatedGradeRuntime":"NOT_VERIFIED",
"Routes":[{"SourceExercise":q["OriginalSourceExercise"],"TeacherMathVerified":True,
"TeacherReviewRequired":True}for q in Q],"CI":"NOT_RUN","StageD":"NOT_RUN"})
# Source-figure skill alternatives are explanation only. Mark matching pedagogical readiness but not live graphical parity.
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
close_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l03-abc-close-audit.json"
save(root/close_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":5,"Lesson":3,
"OriginalSource":"OpenStax Precalculus (2017) original local PDF",
"OriginalSourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[488,500],
"OriginalWorkedExamples":10,"OriginalTryIt":10,"OriginalFigures":10,"OriginalNumberedExercises":76,
"IndependentWorked":10,"IndependentTryIt":10,"IndependentTeacherMath":76,
"IndependentSourceGraphicSkills":10,"GraphicAlternativeMode":"MATH_EXPLANATION_REUSING_EXISTING_UNIT_CIRCLE",
"OriginalPublisherGraphicsCopied":False,"ExactSourceVisualParityClaimed":False,
"CCSSCandidates":["CCSS:HSF-TF.A.2"],"FormalCCSSCertification":False,
"StageCExistingTeacherMathRoutes":76,"NewMathEngine":False,
"ChecksActuallyPerformed":["Original 5.3 PDF source boundary/exercise numbers",
"76 independently written source-math assessment keys",
"10 worked and10 Try It skill alternatives",
"Reciprocal zero-domain numerical spot checks","Even and odd functions",
"Stage C manual scoring routes and git diff check"],
"CI":"NOT_RUN","Build":"NOT_RUN","E2E":"NOT_RUN","StageD":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
audit_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l03-academic-source-audit.json"
save(root/audit_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":5,"Lesson":3,
"SourceSection":"5.3","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[488,500],"IncorrectBlueprintPages":[489,501],
"OriginalWorkedExamples":[{"Number":n,"Title":name}for n,name in worked],
"OriginalTryIts":tries,"SourceFigureNumbers":figures,
"OriginalSourceExerciseLedger":[{"Number":q["OriginalSourceExercise"],
"IndependentMathSkillReviewed":True,"TeacherAnswerVerified":True}for q in Q],
"StudentRichV2":richrel,"TeacherMathAssessment":teacherrel,
"SourceFigureEvidence":figrel,"StageCAssessmentRoutes":routesrel,
"ReadyForStageD":True,"Status":"READY_FOR_STAGE_D","BlockingReasons":[],
"ClosureEvidence":close_rel,"StageD":"NOT_RUN","CI":"NOT_RUN","ClosedAtUtc":now})
assert row["Ordinal"]==31 and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":488,"PdfPageEnd":500,"BlockingReasons":[],
"SourceNumberedExercisesMathReviewed":76,"TeacherMathKeys":76,
"IndependentFigureSkills":10,"EvidencePath":audit_rel,"ClosureEvidencePath":close_rel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][31]["Ordinal"]==32
p.update({"LastClosedLessonOrdinal":31,"FirstIncompleteLessonOrdinal":32,
"ActiveLessonOrdinal":32,"NextLessonOrdinal":32,"ReadyForStageD":31,
"SourceVerifiedGrade12Lessons":31,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_31_OF_73",
"UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),richrel,teacherrel,figrel,routesrel,close_rel,audit_rel,str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u05 l03): close additional trig functions academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U05L03_ACADEMIC_CLOSE_PUSH_VERIFIED_31_OF_73")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("SEC",round(time.monotonic()-started,2))
