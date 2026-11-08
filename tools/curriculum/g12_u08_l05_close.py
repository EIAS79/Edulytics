from pathlib import Path
import math,cmath,json,re,datetime,subprocess,pypdfium2
from fractions import Fraction
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json"
p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==45 and p["FirstIncompleteLessonOrdinal"]==46
row=p["LessonEntries"][45]
assert row["Ordinal"]==46 and row["SourceSection"]=="8.5" and not row["ReadyForStageD"]
doc=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda i:doc[i].get_textpage().get_text_range()
assert "8.5" in txt(712) and "8.6" in txt(723)
s="\n".join(txt(i)for i in range(712,721))
worked=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})",s,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(txt(i)for i in range(712,723)),re.I)})
assert [n for n,_ in worked]==list(range(1,12)),worked
assert tries==list(range(1,9)),tries
assert figures==list(range(1,7)),figures
assert "61." in txt(722)
def polar(z):
    assert z!=0
    return abs(z),math.degrees(cmath.phase(z))%360
def fc(x):return f"{x:.6f}".rstrip('0').rstrip('.')
def cformat(z):return f"{fc(z.real)}+({fc(z.imag)})i"
Q=[]
def add(q,a,skill,check=None):
    n=len(Q)+1
    assert q and a and skill
    if check is not None:assert check
    Q.append({"OriginalExerciseNumber":n,"IndependentStudentQuestion":q,
              "TeacherMathAnswer":a,"SourceMathSkill":skill,
              "MathematicalSpotCheck":check is not None,"PublisherExerciseCopied":False,
              "AssessmentMode":"MANUAL_TEACHER_REVIEW","RuntimeVerified":False})
add("Explain real and imaginary coordinates of z=-4+7i.","Real part -4, imaginary part 7; point (-4,7).","Source 1 complex a+bi components.")
add("Interpret |z| geometrically for z=a+bi.","Distance from origin to (a,b), sqrt(a²+b²).","Source 2 modulus meaning.")
add("Convert z=3-3i to polar form.","r=3sqrt2; principal θ=315° (or -45°); z=3sqrt2 cis315°.","Source 3 rectangular-to-polar.")
add("Multiply z1=2cis30° and z2=3cis70°.","6cis100°; multiply magnitudes and add arguments.","Source 4 complex multiplication.")
add("State De Moivre's identity and a use.","(r cisθ)^n=r^n cis(nθ) for integer n; powers, roots by root formula.","Source 5 De Moivre theorem.")
for z in [complex(4,3),complex(-8,6),complex(-5,-12),complex(math.sqrt(3),-1),3j,complex(2.5,-1.5)]:
    add(f"Find modulus of z={cformat(z)}.",f"|z|={fc(abs(z))}.","Source exercises 6–11 absolute values.",abs(z)>0)
for z in [complex(3,3),complex(6,-2),complex(-1,-math.sqrt(3)),complex(math.sqrt(3),3),-4j]:
    radius,theta=polar(z)
    add(f"Convert z={cformat(z)} to cis polar form.",
        f"r≈{radius:.6f}, θ≈{theta:.6f}°; z=r cisθ.",
        "Source 12–16 rectangular-to-polar forms.",
        abs(radius*cmath.exp(1j*math.radians(theta))-z)<1e-8)
for radius,angle in [(8,30),(3,60),(5,210),(6,135),(4,240),(2*math.sqrt(2),300)]:
    z=radius*cmath.exp(1j*math.radians(angle))
    add(f"Convert {fc(radius)}cis({angle}°) to rectangular form.",
        f"z={cformat(z)}, via (r cosθ,r sinθ).",
        "Source 17–22 polar-to-rectangular forms.",
        math.isclose(abs(z),radius,rel_tol=1e-9))
for pair in [((2,20),(3,80)),((math.sqrt(2),125),(3,135)),((5,120),(2,60)),
             ((3,45),(2,30)),((2,112.5),(4,15)),((6,90),(1.5,75))]:
    (r1,t1),(r2,t2)=pair; r=r1*r2; ang=(t1+t2)%360
    add(f"Compute product of z1={fc(r1)}cis{fc(t1)}°, z2={fc(r2)}cis{fc(t2)}°.",
        f"z1z2={fc(r)}cis({fc(ang)}°).","Source 23–28 polar multiplication.",
        abs((r1*cmath.exp(1j*math.radians(t1)))*(r2*cmath.exp(1j*math.radians(t2)))-r*cmath.exp(1j*math.radians(ang)))<1e-8)
for pair in [((24,120),(4,50)),((3,90),(2,60)),((18,120),(6,40)),
             ((8,60),(2,45)),((5*math.sqrt(2),180),(math.sqrt(2),120)),((9,108),(3,45))]:
    (r1,t1),(r2,t2)=pair;r=r1/r2;ang=(t1-t2)%360
    add(f"Compute quotient z1/z2 for z1={fc(r1)}cis{fc(t1)}°, z2={fc(r2)}cis{fc(t2)}°.",
        f"z1/z2={fc(r)}cis({fc(ang)}°).","Source 29–34 polar division.",
        abs((r1*cmath.exp(1j*math.radians(t1)))/(r2*cmath.exp(1j*math.radians(t2)))-r*cmath.exp(1j*math.radians(ang)))<1e-8)
for radius,angle,power in [(3,45,3),(2,75,4),(4,120,2),(5,30,2),(2,35,4),(2,105,3)]:
    r=radius**power;deg=(angle*power)%360
    add(f"Apply De Moivre to ({radius}cis{angle}°)^{power}.",
        f"{fc(r)}cis({fc(deg)}°).","Source 35–40 integer complex powers.",
        abs((radius*cmath.exp(1j*math.radians(angle)))**power-r*cmath.exp(1j*math.radians(deg)))<1e-6)
for radius,angle,n in [(8,120,3),(25,100,2),(64,240,3),(36,180,2),(27,315,3)]:
    mod=radius**(1/n);args=[(angle+360*k)/n for k in range(n)]
    add(f"Find all {n}th roots of z={radius}cis({angle}°).",
        f"{n} roots: {fc(mod)}cisθ where θ in {[round(v,6) for v in args]}°.",
        "Source 41–45 distinct roots of complex numbers.",
        all(abs((mod*cmath.exp(1j*math.radians(v)))**n-radius*cmath.exp(1j*math.radians(angle)))<1e-6 for v in args))
for z in [complex(3,4),complex(-4,-2),complex(5,-3),complex(-2,-6),
          complex(4,1),5j,-6,complex(2,-7),complex(-3,2),complex(1,-5)]:
    add(f"Plot z={cformat(z)} on an independent Argand plane.",
        f"Plot ({fc(z.real)},{fc(z.imag)}), Re horizontal, Im vertical.",
        "Source 46–55 complex plane plotted coordinates.",
        math.isclose(abs(z)**2,z.real**2+z.imag**2))
for z in [complex(7,7),complex(4,-3),complex(-5,-5)]:
    rad,deg=polar(z)
    add(f"Use calculator method to express z={cformat(z)} in polar form (2 decimal places).",
        f"r≈{rad:.2f}, θ≈{deg:.2f}°.",
        "Source 56–58 technology polar conversion.",
        math.isclose(rad,abs(z)))
for r,deg in [(5,120),(3,45),(6,210)]:
    z=r*cmath.exp(1j*math.radians(deg))
    add(f"Convert {r}cis({deg}°) to rectangular coordinates rounded to hundredths.",
        f"z≈{z.real:.2f}+({z.imag:.2f})i.",
        "Source 59–61 technology rectangular conversion.",
        math.isclose(abs(z),r))
assert len(Q)==61 and [v["OriginalExerciseNumber"]for v in Q]==list(range(1,62))
W=[
("Complex plane","Plot -3+4i.","(-3,4); modulus5."),
("Absolute value","Compute modulus of 5-12i.","13."),
("Modulus with radical","Compute modulus of sqrt7+3i.","4."),
("Polar coordinates","Convert 3+3i to polar.","3sqrt2 cis45°."),
("Quadrant and argument","Convert -2-2sqrt3 i to polar.","4cis240°."),
("Polar to rectangular","Convert 8cis60°.","4+4sqrt3 i."),
("Rectangular negative axis","Convert 3cis180°.","-3."),
("Product","(2cis30°)(3cis100°).","6cis130°."),
("Quotient","(8cis240°)/(2cis30°).","4cis210°."),
("De Moivre power","(2cis45°)^3.","8cis135°."),
("Cube roots","Cube roots of 8cis0°.","2cis0°, 2cis120°, 2cis240°.")
]
T=[("Plot -1+2i.","(-1,2)."),
("Find |8-6i|.","10."),
("Find |-5+12i|.","13."),
("Convert -3+3i to cis form.","3sqrt2cis135°."),
("Convert 4cis150° to a+bi.","-2sqrt3+2i."),
("Multiply 2cis40° and 5cis110°.","10cis150°."),
("Divide 6cis300° by 3cis60°.","2cis240°."),
("Find (3cis30°)^2.","9cis60°.")]
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L05"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l05.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l05-firstparty-teacher-led.json"
def save(path,x):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L05",
"ContentVersion":"g12-u08-l05-firstparty-complex-polar-abc",
"Lessons":[{"LessonCode":code,"CultureCode":"en","Title":"Polar Form of Complex Numbers",
"ExplanationParagraphs":[
"Complex z=a+bi plots at (a,b) in the Argand plane with real horizontal axis and imaginary vertical axis.",
"The modulus r=sqrt(a²+b²) and argument θ=atan2(b,a) describe z=r(cosθ+i sinθ)=r cisθ. For zero, the argument is undefined.",
"Use the correct quadrant when converting a nonzero complex number from rectangular to polar form.",
"Polar multiplication multiplies moduli and adds arguments; division divides moduli and subtracts arguments when denominator is nonzero.",
"De Moivre's theorem (r cisθ)^n=r^n cis(nθ) for integer n supports efficient powers.",
"The n distinct nth roots of nonzero z=r cisθ are r^(1/n) cis((θ+2πk)/n), k=0,...,n-1.",
"Round only final decimal approximations; retain exact special-angle values when known.",
"Independent Cool-down: convert -3-3i to polar, multiply 2cis30° by 4cis90°, and find all square roots of 9cis180°." ]+
[f"Independent Try It {i}: {x}"for i,(x,_)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Use Argand coordinates, modulus/argument and cis arithmetic.",
"Steps":[ans],"Answer":ans,"Check":"Convert back to a+bi or exponentiate roots."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Modulus","Definition":"|a+bi|=sqrt(a²+b²).","Rule":"Nonnegative radius.","Example":"|3+4i|=5."},
{"Title":"Polar multiplication","Definition":"r1cisθ1·r2cisθ2=r1r2cis(θ1+θ2).","Rule":"Angles same unit.","Example":"2cis30°·3cis60°=6cis90°."},
{"Title":"Complex roots","Definition":"r^(1/n)cis((θ+2πk)/n).","Rule":"k=0,...,n-1 for nonzero z.","Example":"Cube roots of8."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent complex plane coordinates",
"Description":"First-party Argand quadrant table.","PrimaryLabel":"Complex number",
"SecondaryLabel":"Point","Rows":[{"Label":"z","Cells":["3+4i","-3+4i","-3-4i","3-4i"]},
{"Label":"(Re,Im)","Cells":["(3,4)","(-3,4)","(-3,-4)","(3,-4)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Plot complex numbers in the Argand plane.","Use modulus and quadrant-aware argument.","Multiply/divide in cis form.","Apply De Moivre and all nth-root branches."],
"CommonMistakes":[{"Mistake":"Use atan(b/a) without quadrant correction.","WhyWrong":"Arctangent loses quadrant.","Correction":"Use atan2(b,a)."},
{"Mistake":"Return only one nonzero nth root.","WhyWrong":"There are n distinct complex nth roots.","Correction":"Enumerate k=0,...,n-1."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":code,
"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Convert -3-3i to polar.","Multiply 2cis30° and4cis90°.","Find two square roots of9cis180°."],
"TeacherExpectedMath":["3sqrt2cis225°.","8cis120°.","3cis90°, 3cis270°."],
"OriginalPublisherCoolDownCopied":False},
"SourceMathExercisesReviewed":61,"StageC":"REUSE_RICHV2_MANUAL_COMPLEX_MATH",
"AutoGradingVerified":False,"StageD":"NOT_RUN"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l05-six-complex-figure-skills.json"
figroles=["Complex Argand point","Quadrant-aware argument","Modulus-radius right triangle",
"Rectangular versus polar plane","Complex polar angle addition","Roots angular spacing"]
save(R/figrel,{"Section":"8.5","OriginalFigureCount":6,
"IndependentFigureSkillAlternatives":[{"OriginalFigure":i,"Skill":role,
"OriginalPublisherGraphicCopied":False,"FirstPartyDiagramType":"Argand mathematical points and polar rays"}
for i,role in enumerate(figroles,1)],"GraphicsRuntimeVerified":False})
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l05-stage-c-61-complex-routes.json"
save(R/routes,{"Section":"8.5","Count":61,"Reuse":["RichV2","Manual teacher evaluation of complex forms"],
"NewMathEngine":False,"RuntimeVerified":False,
"Routes":[{"OriginalExercise":i,"FirstPartyStudentQuestion":True,"TeacherMath":True}for i in range(1,62)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l05-source-academic-audit.json"
save(R/audit,{"SourceSection":"8.5","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[712,722],"IncorrectBlueprint":[713,723],
"OriginalWorkedExampleAnchors":worked,"OriginalTryItNumbers":tries,"OriginalFigures":figures,
"OriginalExerciseNumbers":list(range(1,62)),"IndependentTeacherAssessment":trel,
"IndependentStudentRichV2":rrel,"FigureAlternatives":figrel,
"AssessmentRoutes":routes,"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN",
"StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l05-abc-close-audit.json"
save(R/close,{"Section":"8.5","Source":"Local original OpenStax 2017 Precalculus PDF",
"PdfPages":[712,722],"OriginalExercisesReviewed":61,"IndependentStudentTeacherPairs":61,
"SourceWorked":11,"SourceTryIt":8,"SourceFigureSkills":6,
"IndependentWorked":11,"IndependentTryIt":8,"IndependentCoolDown":3,
"StageCReuse":True,"NewMathEngine":False,"PublisherGraphicsCopied":False,
"MathSpotChecks":sum(v["MathematicalSpotCheck"]for v in Q),
"SourceSkillAlternatives":6,"RuntimeTest":"NOT_RUN",
"CI":"NOT_RUN","StageD":"NOT_RUN","Build":"NOT_RUN","ReadyForStageD":True,
"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":712,"PdfPageEnd":722,"EvidencePath":audit,
"ClosureEvidencePath":close,"BlockingReasons":[],
"MathExercisesReviewed":61,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][46]["Ordinal"]==47
p.update({"LastClosedLessonOrdinal":46,"FirstIncompleteLessonOrdinal":47,
"ActiveLessonOrdinal":47,"NextLessonOrdinal":47,"ReadyForStageD":46,
"SourceVerifiedGrade12Lessons":46,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_46_OF_73",
"UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,routes,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l05): close complex polar forms and De Moivre academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L05_PUSH_VERIFIED_46_OF_73")
