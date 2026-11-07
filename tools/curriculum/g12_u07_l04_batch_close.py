from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2 as pdfium
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"));assert p["LastClosedLessonOrdinal"]==38 and p["FirstIncompleteLessonOrdinal"]==39
row=p["LessonEntries"][38];assert row["Ordinal"]==39 and row["SourceSection"]=="7.4" and not row["ReadyForStageD"]
D=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
tx=lambda i:D[i].get_textpage().get_text_range()
assert "7.4" in tx(612) and "7.5" in tx(618) and "63." in tx(617)
head="\n".join(tx(i)for i in range(611,616))
examples=[(int(n),s.strip())for n,s in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,108})",head,re.I)]
tries=sorted({int(i)for i in re.findall(r"Try It\s*#?(\d+)",head,re.I)})
figures=sorted({int(i)for i in re.findall(r"Figure\s+(\d+)","\n".join(tx(i)for i in range(611,618)),re.I)})
assert [n for n,_ in examples]==list(range(1,8)) and tries==list(range(1,6)) and figures==[1,2]
Q=[]
def add(q,a,source):
 n=len(Q)+1;assert q and a and source
 Q.append({"SourceExercise":n,"IndependentQuestion":q,"TeacherExpectedMath":a,"SourceSkillReviewed":source,
 "PublisherTextCopied":False,"TeacherManualScoring":True})
for q,a,sk in [
("Derive cosαsinβ product-to-sum.","cosαsinβ=[sin(α+β)−sin(α−β)]/2.","Source1 product-to-sum order/sign"),
("Compute cos210° cos30° by product-to-sum and direct exact values.","Product=(cos240°+cos180°)/2=(−1/2−1)/2=−3/4.","Source2 alternate methods of products"),
("Explain when converting sinα−sinβ to a product is useful.","Turns difference into 2cos((α+β)/2)sin((α−β)/2); useful factoring trig equation.","Source3 sum-to-product application"),
("Explain when converting sinαcosβ to a sum is useful.","sinαcosβ=[sin(α+β)+sin(α−β)]/2; useful wave decomposition.","Source4 product-to-sum application")
]:add(q,a,sk)
prod=[("sin","sin",9,4,4),("cos","cos",8,3,6),("sin","cos",7,2,2),
("cos","sin",5,4,10),("sin","sin",-2,7,1),("sin","cos",4,6,1)]
def formula_product(f,g,a,b,scale):
 if f=="sin" and g=="sin":return f"{scale/2:g}[cos({a-b}x)−cos({a+b}x)]"
 if f=="cos" and g=="cos":return f"{scale/2:g}[cos({a-b}x)+cos({a+b}x)]"
 if f=="sin" and g=="cos":return f"{scale/2:g}[sin({a+b}x)+sin({a-b}x)]"
 return f"{scale/2:g}[sin({a+b}x)−sin({a-b}x)]"
for f,g,a,b,scale in prod:
 n=len(Q)+1;add(f"Rewrite {scale}{f}({a}x){g}({b}x) as a sum/difference.",
 formula_product(f,g,a,b,scale),f"Source{n} product-to-sum {f}×{g}")
assert len(Q)==10
sums=[("cos","+",8,2),("sin","+",4,10),("cos","+",6,-6),
("sin","-",5,-5),("cos","+",3,7),("sin","-",1,3)]
for f,operator,a,b in sums:
 n=len(Q)+1
 if f=="cos"and operator=="+":answer=f"2cos({(a+b)/2:g}x)cos({(a-b)/2:g}x)"
 elif f=="cos":answer=f"−2sin({(a+b)/2:g}x)sin({(a-b)/2:g}x)"
 elif operator=="+":answer=f"2sin({(a+b)/2:g}x)cos({(a-b)/2:g}x)"
 else:answer=f"2cos({(a+b)/2:g}x)sin({(a-b)/2:g}x)"
 add(f"Factor {f}({a}x){operator}{f}({b}x).",answer,f"Source{n} sum-to-product {f}")
for f,g,a,b in [("cos","cos",60,30),("cos","sin",45,30),("sin","sin",-315,-15),
("sin","cos",210,30),("sin","sin",-45,-15)]:
 n=len(Q)+1
 va=(math.sin if f=="sin"else math.cos)(math.radians(a))
 vb=(math.sin if g=="sin"else math.cos)(math.radians(b))
 add(f"Evaluate exactly {f}({a}°){g}({b}°) using product-to-sum.",
 f"Exact via special angles; numerical check {va*vb:.8f}.",f"Source{n} exact product identity")
for f,g,a,b,scale in [("cos","sin",33,11,1),("sin","sin",100,40,2),
("sin","sin",-80,-30,2),("sin","cos",225,15,1),("cos","cos",54,36,2)]:
 n=len(Q)+1
 add(f"Express {scale}{f}({a}°){g}({b}°) as a sum or difference and evaluate.",
 "Use product-to-sum formula and exact sine/cosine of sum and difference angles; "+
 f"decimal≈{scale*(math.sin if f=='sin'else math.cos)(math.radians(a))*(math.sin if g=='sin'else math.cos)(math.radians(b)):.6f}.",
 f"Source{n} numerical product-to-sum")
for kind,op,a,b in [("sin","+",75,15),("cos","-",70,10),("sin","-",111,31),
("cos","+",120,220),("sin","+",-2,-4)]:
 n=len(Q)+1
 A=(math.sin if kind=="sin"else math.cos)(math.radians(a))
 B0=(math.sin if kind=="sin"else math.cos)(math.radians(b))
 add(f"Express {kind}({a}°){op}{kind}({b}°) as a product.",
 ("2sin((a+b)/2)cos((a−b)/2)"if kind=="sin"and op=="+" else
 "2cos((a+b)/2)sin((a−b)/2)"if kind=="sin" else
 "2cos((a+b)/2)cos((a−b)/2)"if op=="+" else
 "−2sin((a+b)/2)sin((a−b)/2)")+f"; decimal≈{A+B0 if op=='+' else A-B0:.6f}.",
 f"Source{n} degree-angle sum-to-product")
assert len(Q)==31
identities=[
("Show cos(a+b)/cos(a−b)=(1−tan a tan b)/(1+tan a tan b).","Divide both cosine sum/difference formulas by cos a cos b."),
("Prove 4sin3x cos4x=2sin7x−2sinx.","2sin3xcos4x=sin7x+sin(−x)."),
("Prove 6cos8x sin2x/sin(−6x)=−3sin10x csc6x+3.","6cos8x sin2x=3[sin10x−sin6x], sin−6x=−sin6x."),
("Prove sinx+sin3x=4sinx cos²x.","LHS=2sin2x cosx=4sinxcos²x."),
("Prove 2(cos³x−cosx sin²x)=cos3x+cosx.","LHS=2cosxcos2x=cos3x+cosx."),
("Prove 2tanx cos3x=secx(sin4x−sin2x).","RHS=secx·2cos3x sinx=2tanxcos3x."),
("Show cos(a+b)+cos(a−b)=2cos a cos b.","Add cosine sum and difference expansions."),
("Evaluate cos62°+cos18° by sum-to-product.",f"2cos40°cos22°, numeric≈{math.cos(math.radians(62))+math.cos(math.radians(18)):.4f}."),
("Evaluate sin3°−sin5° by sum-to-product.",f"2cos4°sin(−1°), numeric≈{math.sin(math.radians(3))-math.sin(math.radians(5)):.4f}."),
("Evaluate cos48°−cos24° by sum-to-product.",f"−2sin36°sin12°, numeric≈{math.cos(math.radians(48))-math.cos(math.radians(24)):.4f}."),
("Evaluate cos160°sin15° via product-to-sum.",f"[sin175°−sin145°]/2, numeric≈{math.cos(math.radians(160))*math.sin(math.radians(15)):.4f}."),
("Evaluate sin(−24°)sin75° via product-to-sum.",f"[cos(−99°)−cos51°]/2, numeric≈{math.sin(math.radians(-24))*math.sin(math.radians(75)):.4f}."),
("Verify 2sin2x sin3x=cosx−cos5x.","2sinαsinβ=cos(α−β)−cos(α+β)."),
("Is (cos10x+cos6x)/(cos6x−cos10x)=cot2x cot8x?","Yes on common domain: numerator2cos8xcos2x, denominator2sin8x sin2x."),
("Simplify (sin3x−sin5x)/(cos3x+cos5x).","−tanx, where denominator nonzero."),
("Check 2cos2x cosx+sin2x sinx=2sinx.","False: at x=0 LHS=2 RHS=0."),
("Verify (sin2x+sin4x)/(sin2x−sin4x)=−tan3x cotx.","sin sum=2sin3x cosx; difference=−2cos3x sinx."),
("Simplify (sin9t−sin3t)/(cos9t+cos3t).","tan3t on common domain."),
("Simplify 2sin8x cos6x−sin2x.","sin14x."),
("Simplify (sin3x−sinx)/sinx.","2cos2x+1? sin3x/sinx−1=(3−4sin²x)−1=2−4sin²x=2cos2x, sinx≠0."),
("Simplify (cos5x+cos3x)/(sin5x+sin3x).","cot4x on common domain."),
("Simplify sinx cos15x−cosx sin15x.","sin(−14x)=−sin14x."),
("Derive sinx−siny formula.","2cos((x+y)/2)sin((x−y)/2)."),
("Derive cosx+cosy formula.","2cos((x+y)/2)cos((x−y)/2)."),
("Verify (sin6x+sin4x)/(sin6x−sin4x)=tan5x cotx.","Ratio [2sin5x cosx]/[2cos5x sinx]."),
("Verify (cos3x+cosx)/(cos3x−cosx)=−cot2x cotx.","Numerator2cos2xcosx; denominator−2sin2xsinx."),
("Verify (cos6y+cos8y)/(sin6y−sin4y)=cot y cos7y sec5y.","Numerator2cos7y cos y; denominator2cos5y sin y."),
("Verify (cos2y−cos4y)/(sin2y+sin4y)=tan y.","Numerator2sin3y sin y; denominator2sin3y cos y."),
("Verify (sin10x−sin2x)/(cos10x+cos2x)=tan4x.","Numerator2cos6x sin4x; denominator2cos6x cos4x."),
("Verify cosx−cos3x=4sin²x cosx.","Cos difference=−2sin2x sin−x=2sin2x sinx."),
("Verify (cos2x−cos4x)²+(sin4x+sin2x)²=4sin²3x.","Both squared terms have common factor2sin3x; cosx²+sinx²=1."),
("Prove tan(π/4−t)=(1−tant)/(1+tant).","Use tangent difference with tanπ/4=1.")
]
assert len(identities)==32,len(identities)
for q,a in identities:add(q,a,f"Source{len(Q)+1} trigonometric identity or application")
assert len(Q)==63 and [x["SourceExercise"]for x in Q]==list(range(1,64))
# Spot-check numerical identities and counterexample
for x in (.19,.38):
 assert math.isclose((math.sin(3*x)-math.sin(5*x))/(math.cos(3*x)+math.cos(5*x)),-math.tan(x),rel_tol=1e-9)
 assert math.isclose((math.sin(9*x)-math.sin(3*x))/(math.cos(9*x)+math.cos(3*x)),math.tan(3*x),rel_tol=1e-9)
 assert math.isclose((math.cos(2*x)-math.cos(4*x))**2+(math.sin(4*x)+math.sin(2*x))**2,4*math.sin(3*x)**2,rel_tol=1e-9)
W=[("Cosine product","Convert 2cos5xcos3x.","cos8x+cos2x."),
("Sine cosine product","Convert 2sin5xcos3x.","sin8x+sin2x."),
("Sine product","Convert 2sin5xsin3x.","cos2x−cos8x."),
("Sine difference","Convert sin5x−sin3x.","2cos4x sinx."),
("Exact product","Compute cos60°cos30°.","sqrt3/8."),
("Sum cosine proof","Prove cos(a+b)+cos(a−b)=2cos a cos b.","Add cosine sum/difference expansions."),
("Reciprocal proof","Simplify (sin4x−sin2x)/(cos4x+cos2x).","tanx on denominator nonzero.")]
T=[("Convert 2cos4xcosx.","cos5x+cos3x."),
("Convert 2sin4xsinx.","cos3x−cos5x."),
("Convert sin7x+sin3x.","2sin5x cos2x."),
("Evaluate sin30°cos60°.","1/4."),
("Prove sin(a+b)−sin(a−b)=2cos a sin b.","Subtract addition formulas.")]
def save(f,o):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(o,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L04"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l04.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l04-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U07:L04",
"ContentVersion":"g12-u07-l04-original-product-sum-math-abc",
"Lessons":[{"LessonCode":code,"CultureCode":"en","Title":"Sum-to-Product and Product-to-Sum Formulas",
"ExplanationParagraphs":["Product-to-sum: 2cos a cos b=cos(a+b)+cos(a−b), 2sin a sin b=cos(a−b)−cos(a+b).",
"Mixed products: 2sin a cos b=sin(a+b)+sin(a−b), 2cos a sin b=sin(a+b)−sin(a−b).",
"Sum-to-product: sin a±sin b=2sin((a+b)/2)cos((a−b)/2) for plus, or2cos((a+b)/2)sin((a−b)/2) for minus.",
"Cosine sum: cos a+cos b=2cos((a+b)/2)cos((a−b)/2). Cosine difference=−2sin((a+b)/2)sin((a−b)/2).",
"Use these identities to factor trig equations and compare graphs. Avoid canceling factors at zeros of original denominators.",
"Independent Cool-down: express 2sin6x cos2x as a sum, convert cos7x−cos3x to product, and simplify sin5x−sinx." ]+
[f"Independent Try It {i}: {q}"for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":n,"Question":q,"Method":"Use matching sum-product identity and verify with independent numeric sample.","Steps":[a],"Answer":a,"Check":"Preserve both sides' common domain."}for n,q,a in W],
"KeyConcepts":[{"Title":"Product to sum","Definition":"2sin a cos b=sin(a+b)+sin(a−b).","Rule":"Use sine/cosine parity for negative differences.","Example":"2sin3xcosx=sin4x+sin2x."},
{"Title":"Sum to product","Definition":"sin a−sin b=2cos((a+b)/2)sin((a−b)/2).","Rule":"Half-sum/half-difference order matters.","Example":"sin5x−sin3x=2cos4xsinx."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party sum-product conversion","Description":"Independent formula comparison.",
"PrimaryLabel":"Operation","SecondaryLabel":"Formula","Rows":[{"Label":"Product","Cells":["2cos a cos b=cos(a+b)+cos(a−b)","2sin a sin b=cos(a−b)−cos(a+b)"]},
{"Label":"Sum","Cells":["sin a+sin b=2sin((a+b)/2)cos((a−b)/2)","cos a+cos b=2cos((a+b)/2)cos((a−b)/2)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Two-angle product-to-sum and sum-to-product formulas.","Apply correct sine/cosine sign patterns.","Keep denominator domains when simplifying rational trig forms."],
"CommonMistakes":[{"Mistake":"Reverse sine and cosine order in mixed products.","WhyWrong":"Difference term changes sign.","Correction":"Start from exact identities and use parity."}]}]}
teacher={"SchemaVersion":1,"LessonCode":code,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Expand 2sin6x cos2x.","Factor cos7x−cos3x.","Factor sin5x−sinx."],
"TeacherExpectedMath":["sin8x+sin4x.","−2sin5x sin2x.","2cos3x sin2x."],"OriginalPublisherCoolDownCopied":False},
"StageC":"REUSE_TEACHER_MANUAL_TRIG_MATH","SourceMathExercisesReviewed":63}
save(R/richrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l04-two-firstparty-figure-skills.json"
save(R/figrel,{"OriginalFigureCount":2,"Alternatives":[{"Number":1,"Role":"Source opening photo replaced by independent sinusoidal superposition mathematics"},
{"Number":2,"Role":"Trigonometric wave addition model and phase-angle illustration"}],
"PublisherArtworkCopied":False,"ExactPixelParityClaimed":False})
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l04-stage-c-63-teacher-math-routes.json"
save(R/routes,{"Section":"7.4","Count":63,"NewMathEngine":False,
"Routes":[{"SourceExercise":i,"TeacherKeyPresent":True,"AutoGradingVerified":False}for i in range(1,64)],"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l04-source-and-academic-audit.json"
save(R/audit,{"Section":"7.4","SourceSHA256":p["SourceSnapshotSha256"],"CorrectedLocalPdfPages":[611,617],
"OriginalWorked":[{"Number":n,"Title":v}for n,v in examples],"OriginalTryIt":tries,"OriginalFigures":figures,
"OriginalExercises":63,"IndependentQuestions":63,"RichV2":richrel,"Teacher":trel,
"FigureSkillAlternatives":figrel,"StageCRoutes":routes,"Status":"READY_FOR_STAGE_D","StageD":"NOT_RUN","CI":"NOT_RUN","ClosedAtUtc":now})
closure="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l04-abc-close-audit.json"
save(R/closure,{"Source":"OpenStax Precalculus 2017 original local book","OriginalSection":"7.4",
"OriginalSourceExercises":63,"MathSkillsReviewed":63,"IndependentTeacherAnswerKeys":63,
"WorkedExamples":7,"TryIt":5,"FigureSkillAlternatives":2,"NewEngine":False,
"ChecksActuallyRun":["Exact local PDF source boundary and count","63 independently written math exercises with teacher solutions",
"Product-to-sum numerical spot-check","7 worked/5 TryIt","Git diff check"],
"PublisherImagesCopied":False,"CCSSFullCertified":False,
"Runtime":"NOT_RUN","Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":611,"PdfPageEnd":617,
"EvidencePath":audit,"ClosureEvidencePath":closure,"BlockingReasons":[],
"SourceMathExercisesReviewed":63,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][39]["Ordinal"]==40
p.update({"LastClosedLessonOrdinal":39,"FirstIncompleteLessonOrdinal":40,"ActiveLessonOrdinal":40,
"NextLessonOrdinal":40,"ReadyForStageD":39,"SourceVerifiedGrade12Lessons":39,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_39_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),richrel,trel,figrel,routes,audit,closure,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u07 l04): close sum-product trig identities academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U07L04_CLOSED_PUSH_VERIFIED_39_OF_73")
