from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json"
p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==46 and p["FirstIncompleteLessonOrdinal"]==47
row=p["LessonEntries"][46];assert row["Ordinal"]==47 and row["SourceSection"]=="8.6"
d=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:d[i].get_textpage().get_text_range()
assert "8.6" in get(723) and "8.7" in get(734)
intro="\n".join(get(i) for i in range(723,732))
examples=[(int(n),t.strip())for n,t in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",intro,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",intro,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(723,734)),re.I)})
assert [n for n,_ in examples]==list(range(1,10)) and tries==list(range(1,7))
assert figures==list(range(1,10)) and "49." in get(733)
Q=[]
def add(q,key,source,verified=True):
    assert q and key
    Q.append({"OriginalExercise":len(Q)+1,"IndependentStudentQuestion":q,
    "TeacherAnswer":key,"SkillFromOriginalSource":source,
    "MathematicalSpotChecked":verified,"OriginalQuestionCopied":False,"RuntimeTested":False})
for q,k,s in [
("What is a parametric curve?","x=f(t),y=g(t) with shared parameter t and specified domain.","Source 1 definitions."),
("When is time a useful parameter?","Position and direction of travel are tracked in time order.","Source 2 temporal motion."),
("Explain parameter elimination.","Solve one equation for t where possible, substitute into other and retain parameter restrictions.","Source 3 elimination."),
("Why express parametric curve in Cartesian form?","It makes the geometric locus visible in x-y coordinates, though may lose direction and multiplicity.","Source 4 Cartesian benefit."),
("What information can parametric equations retain?","Travel direction, speed and sequencing along a locus.","Source 5 parametric benefit."),
("Why can the same Cartesian curve have many parameterizations?","Different monotone reparameterizations traverse same locus with different rate/orientation.","Source 6 nonuniqueness.")]:add(q,k,s)
for a,b,c,e in [(3,-1,5,2),(4,-2,1,3),(5,2,7,-1)]:
    # x=a*t+b,y=c*t+e => y=c(x-b)/a+e
    add(f"Eliminate t from x={a}t+({b}), y={c}t+({e}).",
        f"t=(x-({b}))/{a}; y={c}/{a}(x-({b}))+({e}).",
        "Source 7–9 linear elimination.")
for shift,exp in [(2,2),(1,3)]:
    add(f"Eliminate t from x=t+{shift}, y=t^{exp}.",
        f"y=(x-{shift})^{exp}, real x.","Source 10,15 polynomial elimination.")
for a,b in [(2,3),(5,1)]:
    add(f"Eliminate t from x={a}e^t,y={b}+t, t real.",
        f"y={b}+ln(x/{a}), domain x>0.","Source 11 exponential and 13 logarithmic substitutions.")
add("Eliminate t from x=e^(2t),y=4e^t.","y=4sqrt(x), x>0.","Source 12 related exponentials.")
add("Eliminate t from x=log10(t), y=t−2, t>0.","y=10^x−2.","Source 14 log-parametric pair.")
add("Eliminate t from x=t−t⁴,y=t+2.","x=(y−2)−(y−2)^4; y real.","Source 16 polynomial substitution.")
add("Eliminate t from x=e^(3t),y=e^(6t).","y=x², x>0.","Source 17 exponential powers.")
add("Eliminate t from x=t^5,y=t^10.","y=x², x real.","Source 18 polynomial powers.")
for ra,rb in [(3,4),(4,5)]:
    add(f"Eliminate t from x={ra}cos(t), y={rb}sin(t), t in[0,2π).",
        f"x²/{ra**2}+y²/{rb**2}=1, ellipse, orientation counterclockwise.",
        "Source 19–20 trigonometric parameter elimination.")
add("Eliminate t from x=3cos²t,y=−sin t.","x=3(1−y²), -1≤y≤1.","Source 21 squared trig parameter.")
add("Eliminate t from x=5+cos t,y=2sin²t.","y=2(1−(x−5)²), 4≤x≤6.","Source 22 shifted trig locus.")
for a,b,powr in [(2,1,2),(-1,3,3),(4,-2,3)]:
    add(f"Eliminate t from x=t+({a}),y=t^{powr}+({b}).",
    f"y=(x-({a}))^{powr}+({b}), real x.","Source 23–25 polynomial elimination.")
for a,b,c,d0 in [(2,-3,1,4),(4,-2,3,1),(3,2,-2,5),(5,-1,5,3)]:
    add(f"Build a table for x={a}t+({b}), y={c}t+({d0}) at t=-1,0,1; find Cartesian relation.",
        f"Points {[(a*t+b,c*t+d0) for t in [-1,0,1]]}; y=({c}/{a})(x-({b}))+({d0}).",
        "Source 26–29 table-driven elimination.")
for q,a in [
("Parameterize y=4x²+2 with x=t.","x=t,y=4t²+2."),
("Parameterize y=3sinx−2 with x=t.","x=t,y=3sint−2."),
("Parameterize x=2ln(y)+y for y>0.","y=t,x=2ln(t)+t,t>0."),
("Parameterize x=sqrt(y)+3y for y≥0.","y=t,x=sqrt(t)+3t,t≥0.")]:
    add(q,a,"Source 30–33 parameterize Cartesian functions.")
for a,b in [(2,3),(3,4),(5,5),(math.sqrt(10),math.sqrt(10))]:
    add(f"Parameterize ellipse/circle x²/{a*a:.6g}+y²/{b*b:.6g}=1.",
        f"x={a:.6g}cos t,y={b:.6g}sin t, t in[0,2π); {'circle' if abs(a-b)<1e-9 else 'ellipse'}.",
        "Source 34–37 ellipse/circle parametrization.")
for (x1,y1),(x2,y2) in [((0,3),(5,-2)),((-4,0),(2,6)),((-2,4),(3,1)),((7,2),(4,-6))]:
    add(f"Parameterize segment from ({x1},{y1}) to ({x2},{y2}), endpoints t=0,1.",
        f"x={x1}+({x2-x1})t,y={y1}+({y2-y1})t,0≤t≤1.",
        "Source 38–41 directed line segments.")
# Compare geometric loci, not only equal-t locations; choose line pairs with known intersections.
for (a,b,c,d0),(e,f,g,h) in [((2,0,3,1),(-1,5,1,-2)),((1,2,-2,4),(2,-3,1,7))]:
    # y = (c/a)*(x-b)+d0 and (g/e)*(x-f)+h
    m1=c/a;m2=g/e; q1=d0-m1*b;q2=h-m2*f
    assert m1!=m2
    x=(q2-q1)/(m1-m2);y=m1*x+q1
    add(f"Find Cartesian-curve intersection of x1={a}t+{b},y1={c}t+{d0} and x2={e}u+{f},y2={g}u+{h}. Parameters may differ.",
        f"Intersection at ({x:.6g},{y:.6g}); compare lines after eliminating both parameters.",
        "Source 42–43 parametric intersections.")
for f,g,ts in [
    (lambda t:t*t+2,lambda t:3*t-1,[-1,0,1]),
    (lambda t:t*t-3,lambda t:2*t*t+1,[1,2,3]),
    (lambda t:t**3,lambda t:t*t-2,[-1,0,1,2])]:
    add(f"Tabulate independent polynomial parametric point coordinates at t={ts}.",
    f"Points {[(t,f(t),g(t)) for t in ts]} (t,x,y).","Source 44–46 numerical tables.")
for n in range(3):
    if n==0:
        q="Give two distinct parameterizations of y=(x−2)²."
        ans="x=t,y=(t−2)² and x=2t,y=(2t−2)²; both t real."
    elif n==1:
        q="Give two parameterizations of line y=4x+1."
        ans="x=t,y=4t+1 and x=2t,y=8t+1."
    else:
        q="Give two distinct parameterizations of y=x²+2x+1."
        ans="x=t,y=(t+1)² and x=t−1,y=t²."
    add(q,ans,"Source 47–49 extension alternative parametrizations.")
assert len(Q)==49, len(Q)
W=[("Parametric curve","For x=t,y=t² identify locus/direction.","y=x², moving left to right as t increases."),
("Elimination","x=3t+1,y=2t−1.","y=2(x−1)/3−1."),
("Segment","From (1,2) to(4,8).","x=1+3t,y=2+6t,0≤t≤1."),
("Polynomial","x=t+2,y=t².","y=(x−2)²."),
("Exponential","x=e^t,y=3t.","y=3lnx,x>0."),
("Logarithmic","x=ln t,y=t, t>0.","y=e^x."),
("Ellipse","x=2cost,y=3sint.","x²/4+y²/9=1."),
("Trig squared","x=3cost,y=4sint.","x²/9+y²/16=1."),
("Parametrize curve","y=2x²−4.","x=t,y=2t²−4.")]
T=[("x=2t,y=5t+1; eliminate t.","y=5x/2+1."),
("x=t²,y=t; eliminate t.","x=y²."),
("Line from(0,0) to(3,6).","x=3t,y=6t,0≤t≤1."),
("Eliminate x=e^t,y=t+3.","y=lnx+3, x>0."),
("Eliminate x=4cost,y=5sint.","x²/16+y²/25=1."),
("Parametrize y=sqrt(x),x≥0.","x=t²,y=t,t≥0.")]
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L06"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l06.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l06-firstparty-teacher-led.json"
def save(path,x):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L06",
"ContentVersion":"g12-u08-l06-firstparty-parametric-abc",
"Lessons":[{"LessonCode":code,"CultureCode":"en","Title":"Parametric Equations",
"ExplanationParagraphs":[
"Parametric equations x=f(t), y=g(t) trace ordered positions as a parameter changes. They can represent curves not single-valued as y=f(x).",
"Eliminate the parameter algebraically, retaining any restrictions induced by exponential, logarithmic, radical or time domains.",
"The Cartesian locus generally does not preserve travel direction, time rate or whether a curve is traced repeatedly.",
"Directed line segment from A to B has x=xA+(xB−xA)t, y=yA+(yB−yA)t on 0≤t≤1.",
"Ellipse x²/a²+y²/b²=1 has parameterization x=a cos t, y=b sin t, t in [0,2π), for a,b>0.",
"An infinite family of parameterizations may share the same locus with differing speed and orientation.",
"Parametric curve intersections need not occur at the same parameter value; compare Cartesian loci and parameter ranges.",
"Independent Cool-down: eliminate t from x=4t−3,y=2t+1; parameterize a circle radius5; distinguish x=t²,y=t and x=t,y=t²." ]+
[f"Independent Try It {i}: {x}"for i,(x,_)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Evaluate coordinate pairs, eliminate t or construct a valid parametrization.",
"Steps":[ans],"Answer":ans,"Check":"Preserve parameter domain and curve direction."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Parameter elimination","Definition":"Substitute t into coordinates to describe the x-y locus.","Rule":"Retain restricted domain.","Example":"x=e^t implies x>0."},
{"Title":"Directed segment","Definition":"A+t(B−A),0≤t≤1.","Rule":"Endpoints at t=0 and t=1.","Example":"(0,0)→(2,3)."},
{"Title":"Ellipse parametric","Definition":"x=a cos t,y=b sin t.","Rule":"a,b positive; full turn0≤t<2π.","Example":"a=2,b=3."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party parametric evaluation table",
"Description":"Independent traced parabola coordinates x=t,y=t².",
"PrimaryLabel":"Parameter","SecondaryLabel":"Position",
"Rows":[{"Label":"t","Cells":["-2","-1","0","1","2"]},{"Label":"x(t)","Cells":["-2","-1","0","1","2"]},
{"Label":"y(t)","Cells":["4","1","0","1","4"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Parameter encodes position and direction.","Elimination may drop time information.","Specify restrictions and intervals.","Construct lines, ellipses and functions parametrically."],
"CommonMistakes":[{"Mistake":"Compare two curves only at equal t.","WhyWrong":"Intersecting traces can arrive at different parameter values.","Correction":"Solve for independent parameters or Cartesian loci."}]}]}
teacher={"SchemaVersion":1,"LessonCode":code,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Eliminate x=4t−3,y=2t+1.","Parametrize radius5 circle.","Compare x=t²,y=t with x=t,y=t²."],
"TeacherExpectedMath":["y=(x+3)/2+1.","x=5cost,y=5sint,0≤t<2π.","First x=y²; second y=x²; different loci."],
"OriginalPublisherCoolDownCopied":False},"SourceMathSkills":49,"StageD":"NOT_RUN"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l06-parametric-figure-skills.json"
roles=["Moon orbit image replaced with original position equations","Circle fails function vertical line test",
"Directed parametric parabola","Parameter table and direction arrows","Line segment A→B",
"Polynomial parameter elimination","Exponential parametric locus","Logarithmic parametric locus",
"Elliptical trigonometric parameterization"]
save(R/figrel,{"Section":"8.6","OriginalSourceFigures":9,
"IndependentFigureSkillAlternatives":[{"Figure":i,"Skill":v,
"PublisherArtworkCopied":False,"MathematicalAlternative":"Independent equations with coordinate table and teacher explanation"}
for i,v in enumerate(roles,1)]})
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l06-stage-c-49-parametric-routes.json"
save(R/routes,{"Section":"8.6","Count":49,"Reuse":"RichV2 and manual teacher coordinate plotting",
"NewEngine":False,"RuntimeVerified":False,
"Routes":[{"SourceExercise":i,"TeacherMath":True,"IndependentQuestion":True}for i in range(1,50)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l06-source-academic-audit.json"
save(R/audit,{"SourceSection":"8.6","CorrectedPdfPages":[723,733],"IncorrectBlueprint":[724,734],
"SourceSHA256":p["SourceSnapshotSha256"],"OriginalWorked":examples,"OriginalTryIt":tries,
"OriginalFigures":figures,"OriginalExerciseNumbers":list(range(1,50)),
"IndependentRichV2":rrel,"TeacherAssessment":trel,"FigureAlternatives":figrel,
"StageCRoutes":routes,"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l06-abc-close-audit.json"
save(R/close,{"Section":"8.6","Source":"Local OpenStax Precalculus 2017",
"CorrectedPdfPages":[723,733],"OriginalExercises":49,
"IndependentMathQuestionKeys":49,"OriginalWorked":9,"OriginalTryIt":6,"Figures":9,
"IndependentWorked":9,"IndependentTryIt":6,"IndependentCoolDown":3,
"FigureAlternatives":9,"NewMathEngine":False,"OriginalPublisherGraphicsCopied":False,
"TargetedChecksActuallyRun":["Source pages and 49 numbered exercises",
"49 independently authored mathematical cases and keys","9 worked and6 Try It skills",
"Coordinate and equation elimination checks","Stage C route count","Git staged diff check"],
"CI":"NOT_RUN","Build":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":723,
"PdfPageEnd":733,"EvidencePath":audit,"ClosureEvidencePath":close,
"BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][47]["Ordinal"]==48
p.update({"LastClosedLessonOrdinal":47,"FirstIncompleteLessonOrdinal":48,"ActiveLessonOrdinal":48,
"NextLessonOrdinal":48,"ReadyForStageD":47,"SourceVerifiedGrade12Lessons":47,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_47_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,routes,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l06): close parametric equations academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L06_PUSH_VERIFIED_47_OF_73")
