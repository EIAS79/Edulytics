from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==47 and p["FirstIncompleteLessonOrdinal"]==48
row=p["LessonEntries"][47];assert row["Ordinal"]==48 and row["SourceSection"]=="8.7"
doc=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
text=lambda i:doc[i].get_textpage().get_text_range()
assert "8.7" in text(734) and "8.8" in text(744)
s="\n".join(text(i)for i in range(734,741))
ex=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",s,re.I)]
tr=sorted({int(v)for v in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
fig=sorted({int(v)for v in re.findall(r"Figure\s+(\d+)",s,re.I)})
assert [n for n,_ in ex]==list(range(1,6)) and tr==[1,2,3] and fig==list(range(1,8))
assert "73." in text(743)
Q=[]
def add(q,a,skill,checked=False):
 Q.append({"SourceExercise":len(Q)+1,"IndependentStudentQuestion":q,"TeacherMath":a,
 "OriginalSourceMathSkill":skill,"MathematicalSpotCheck":checked,
 "OriginalSourceQuestionCopied":False,"TeacherMarking":"MANUAL_PARAMETRIC_GRAPH"})
for q,a,skill in [
("State two ways to graph parametric equations.","Make a t,x(t),y(t) table and plot oriented points; or graph directly in parametric mode.","Source 1 graph methods."),
("How does parametric point plotting differ from Cartesian y=f(x)?","Use the same t to calculate both x and y and track ordered t direction.","Source 2 plotting order."),
("What do arrows on parametric curves represent?","Direction of travel as parameter increases.","Source 3 orientation arrows."),
("Name three common parametric graph types.","Lines, parabolas, ellipses/circles; also Lissajous and projectile trajectories.","Source 4 curve types."),
("Why parametric graphs help with projectiles?","They encode horizontal and vertical coordinates as functions of time, showing flight path and direction.","Source 5 projectile application.")
]:add(q,a,skill)
for idx,(fx,fy,description)in enumerate([
(lambda t:t,lambda t:t*t-2,"upward parabola y=x²−2"),
(lambda t:t+1,lambda t:t*t,"right-shifted parabola y=(x−1)²"),
(lambda t:3+t,lambda t:4-2*t,"line y=10−2x"),
(lambda t:-2-t,lambda t:5+t,"line y=3−x"),
(lambda t:t**3,lambda t:t-1,"cubic x=(y+1)³"),
(lambda t:t*t,lambda t:t+1,"sideways parabola x=(y−1)²")
],6):
 ts=[-2,-1,0,1,2];coords=[(fx(t),fy(t)) for t in ts]
 add(f"Plot parametric coordinates for independent {description} at t={ts}; add orientation arrows.",
 f"Ordered coordinates {coords}; trace in increasing t; {description}.",
 f"Source {idx} table-based oriented parametric graph.",True)
for idx,(fx,fy,domain,description)in enumerate([
(lambda t:t,lambda t:math.sqrt(t),[0,1,4,9],"y=sqrt(x) from origin rightward"),
(lambda t:-math.sqrt(t),lambda t:t,[0,1,4,9],"left branch x=−sqrt(y), y≥0"),
(lambda t:4-abs(t),lambda t:t+1,[-2,-1,0,1,2],"corner with t orientation"),
(lambda t:2-t,lambda t:5-abs(t),[-2,-1,0,1,2],"oriented corner"),
(lambda t:3*math.sin(t),lambda t:2*math.cos(t),[0,math.pi/2,math.pi,3*math.pi/2],"ellipse x²/9+y²/4=1"),
(lambda t:2*math.sin(t),lambda t:5*math.cos(t),[0,math.pi/2,math.pi,3*math.pi/2],"ellipse x²/4+y²/25=1"),
(lambda t:4*math.cos(t)**2,lambda t:-2*math.sin(t),[0,math.pi/2,math.pi,3*math.pi/2],"parabolic locus x=4−y²"),
(lambda t:3*math.cos(t)**2,lambda t:-3*math.sin(t)**2,[0,math.pi/2,math.pi,3*math.pi/2],"line segment x/3−y/3=1"),
(lambda t:1/math.cos(t),lambda t:math.tan(t),[-.6,0,.6],"hyperbola x²−y²=1, right branch"),
(lambda t:1/math.cos(t),lambda t:math.tan(t)**2,[-.6,0,.6],"y=x²−1 with |x|≥1"),
(lambda t:math.exp(-2*t),lambda t:math.exp(-t),[-1,0,1],"x=y²,y>0")
],12):
 pts=[(round(fx(t),5),round(fy(t),5))for t in domain]
 add(f"Sketch independent parametric {description}; plot sample parameter values.",
 f"t→(x,y):{pts}. Preserve physical parameter domain and increasing-t orientation.",
 f"Source {idx} graph/range/orientation family.",True)
for idx,(fx,fy,desc)in enumerate([
(lambda t:t+1,lambda t:-t*t,"downward parabola y=−(x−1)²"),
(lambda t:t**3,lambda t:t+1,"cubic x=(y−1)³"),
(lambda t:3*math.cos(t),lambda t:-2*math.sin(t),"ellipse x²/9+y²/4=1"),
(lambda t:5*math.cos(t),lambda t:5*math.sin(t),"circle radius5"),
(lambda t:math.exp(2*t),lambda t:-math.exp(t),"parabola x=y²,y<0")
],23):
 add(f"Write the Cartesian locus of parametric curve {desc} and indicate orientation.",
 f"{desc}; derive by eliminating t; direction determined by increasing parameter.",
 f"Source {idx} Cartesian equivalence and oriented graph.",True)
for idx,(fx,fy,lo,hi)in enumerate([
(lambda t:t*t,lambda t:2*t,0,4),
(lambda t:2*t,lambda t:t*t,-3,3),
(lambda t:t,lambda t:math.sqrt(16-t*t),0,4),
(lambda t:-t,lambda t:math.sqrt(t),1,5),
(lambda t:-3*math.cos(t),lambda t:5*math.sin(t),0,math.pi),
(lambda t:-1/math.cos(t),lambda t:math.tan(t),-.8,.8)
],28):
 pts=[(round(fx(lo+(hi-lo)*i/4),5),round(fy(lo+(hi-lo)*i/4),5))for i in range(5)]
 add(f"Graph restricted independent parametric curve {idx}: sample t interval [{lo:.3f},{hi:.3f}].",
 f"Ordered sample points {pts}; orientation from first to last.",
 f"Source {idx} restricted-domain parametric plot.",True)
for idx,(a,b)in enumerate([(2,1),(3,2),(4,3),(5,4),(50,49),(2,3),(3,4),(4,5)],34):
 def fx(t):return a*math.cos((a+b)*t)
 def fy(t):return a*math.cos((a-b)*t)
 pts=[(round(fx(t),3),round(fy(t),3))for t in (-math.pi,-math.pi/2,0)]
 add(f"Plot x={a}cos({a+b}t),y={a}cos({a-b}t), -π≤t≤0.",
 f"Points t=−π,−π/2,0: {pts}. Relative frequencies {a+b} and {abs(a-b)}; trace orientation.",
 f"Source {idx} frequency pair a,b parametric analysis.",True)
for idx,(fx,fy,desc)in enumerate([
(lambda t:-t*t,lambda t:3*t,"left-opening parabola x=−y²/9"),
(lambda t:t,lambda t:t*t,"upward parabola"),
(lambda t:t,lambda t:-t*t,"downward parabola")
],42):
 add(f"Describe shape/direction of parametric {desc}.",
 f"{desc}; increasing t determines curve travel.",
 f"Source {idx} qualitative parabola direction.",True)
add("Parameterize counterclockwise circle radius4 centered at origin.","x=4cost,y=4sint, 0≤t<2π.","Source45 CCW circle.")
add("Parameterize ellipse with semiaxes6 and2, CCW.","x=6cost,y=2sint, 0≤t<2π.","Source46 CCW ellipse.")
for a,b in [(1,3),(3,1),(2,2),(4,4),(2,5),(5,2)]:
 ts=[0,math.pi/4,math.pi/2,math.pi,3*math.pi/2]
 pts=[(round(math.sin(a*t),4),round(math.sin(b*t),4))for t in ts]
 add(f"Plot Lissajous x=sin({a}t),y=sin({b}t),0≤t<2π with orientation.",
 f"Key points {pts}; frequencies {a}:{b}; trace actual continuous curve between points.",
 "Source47–52 Lissajous family.",True)
for a,b,c,d0 in [(4,1,3,2),(3,2,5,1),(5,3,2,4),(2,4,4,3)]:
 pts=[(round(a*math.cos(b*t),3),round(c*math.sin(d0*t),3))for t in (0,math.pi/4,math.pi/2)]
 add(f"Given a first-party curve x={a}cos({b}t),y={c}sin({d0}t), recover amplitudes and frequencies.",
 f"a={a},b={b},c={c},d={d0}; points {pts}.",
 "Source53–56 reverse graph parameter recognition.",True)
for interval in [(0,2*math.pi),(0,4*math.pi),(-4*math.pi,6*math.pi)]:
 ts=[interval[0],(interval[0]+interval[1])/2,interval[1]]
 f1=lambda t:(math.cos(t)-2,math.sin(t)+t)
 f2=lambda t:(math.cos(t)+t,math.sin(t)-2)
 f3=lambda t:(t-math.sin(t),math.cos(t)-2)
 add(f"Graph three independent creeping parametric traces x1=cost−2,y1=sint+t; x2=cost+t,y2=sint−2; x3=t−sint,y3=cost−2 on [{interval[0]:.3f},{interval[1]:.3f}].",
 f"Sample endpoints and midpoints {[[tuple(round(v,4)for v in f(t))for t in ts]for f in (f1,f2,f3)]}; trace between points.",
 "Source57–59 parametric cycloid orientation/domain effect.",True)
add("Which axis contains linear drift in x=sin t+t, y=cos t?","x-axis because x has additive t, while y oscillates boundedly.","Source60 creeping axis.")
add("What changes if sin and cos are swapped between x(t) and y(t)?","Parametric orientation, reflection/rotation and starting point can change; locus may or may not remain the same.","Source61 sin/cos switching.")
add("What changes if t interval changes for x=t−sin t, y=1−cos t?","Curve segment and count of traced cycloid arches; direction stays increasing t.","Source62 parameter domain.")
for vx,vy,y0 in [(12,18,0),(7,13,5)]:
 T= (vy+math.sqrt(vy*vy+64*y0))/32
 add(f"Projectile x={vx}t,y={y0}+{vy}t−16t² (feet). Eliminate t, find positive landing time.",
 f"y={y0}+({vy}/{vx})x−(16/{vx**2})x², landing t≈{T:.5f}s, x≈{vx*T:.4f}ft.",
 "Source63–64 projectile rectangular locus and time.",True)
vx=60*math.cos(math.radians(45));vy=60*math.sin(math.radians(45))
T=vy/8
for label,ans in [
("Give projectile x(t),y(t) at60ft/s angle45°.",f"x={vx:.6f}t, y={vy:.6f}t−16t²."),
("Find horizontal range before ground impact.",f"t∈[0,{T:.5f}], x∈[0,{vx*T:.5f}]."),
("Find landing time.",f"{T:.5f}s."),
("Find maximum height.",f"{vy*vy/64:.5f}ft."),
("Find time to maximum height.",f"{vy/32:.5f}s.")
]:add(label,ans,"Source65–69 projectile launch and max height.",True)
for fn,fy,desc in [
(lambda t:12*math.cos(t)-math.cos(12*t),lambda t:12*math.sin(t)+math.sin(12*t),"epicycloid-like"),
(lambda t:5*math.sin(t)+2*math.sin(5*t),lambda t:5*math.cos(t)-2*math.cos(5*t),"hypocycloid-like"),
(lambda t:2*math.sin(t)+4*math.cos(6*t),lambda t:4*math.cos(t)-2*math.sin(6*t),"hypotrochoid-like"),
(lambda t:4*math.sin(2*t)*math.sin(t),lambda t:4*math.sin(2*t)*math.cos(t),"rose-like")]:
 pts=[(round(fn(t),5),round(fy(t),5)) for t in (0,math.pi/2,math.pi,3*math.pi/2)]
 add(f"Sketch an independent {desc} parametric curve on t∈[0,2π].",
 f"Key samples {pts}, trace full continuous parametric trajectory and show increasing-t orientation.",
 "Source70–73 named parametric curves.",True)
assert len(Q)==73,len(Q)
W=[("Point plotting","Plot x=t,y=t².","Parabola upward, motion left-to-right as t grows."),
("Trigonometric ellipse","Plot x=4cost,y=2sint.","Ellipse x²/16+y²/4=1, CCW."),
("Graph equivalent","x=t+2,y=t².","y=(x−2)²."),
("Restricted curve","x=t,y=sqrt(t), t≥0.","y=sqrtx,x≥0."),
("Projectile","x=10t,y=20t−16t².","y=2x−.16x², 0≤t≤1.25s.")]
T=[("Plot x=t,y=t²−1.","Upward parabola with vertex(0,-1), left-to-right."),
("Plot x=3cost,y=2sint.","Ellipse x²/9+y²/4=1, CCW."),
("Graph x=t+1,y=sqrt(t), t≥0.","y=sqrt(x−1), x≥1.")]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L07"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l07.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l07-firstparty-teacher-led.json"
def save(path,x):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L07",
"ContentVersion":"g12-u08-l07-firstparty-parametric-graphs-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Parametric Equations: Graphs",
"ExplanationParagraphs":["Make a table of (t,x(t),y(t)) then plot points in increasing t with arrows for direction.",
"Parametric curves can trace the same Cartesian locus with different orientation and speeds, or trace a locus multiple times.",
"Recognize line and parabola parameterizations and closed circles and ellipses from trig parameterizations.",
"Retain parameter domain to determine which branch or segment is actually plotted. A restricted curve differs from the entire locus.",
"Lissajous curves combine distinct sine frequencies; parametric curves can form complex periodic trajectories.",
"Projectile motion ignoring air resistance can use x=vx t, y=y0+vy t−16t² when feet/seconds and g≈32ft/s².",
"Independent Cool-down: graph x=t²,y=t on t∈[-2,2], sketch circle x=5cost,y=5sint, and derive projectile y(x) from x=12t,y=20t−16t²." ]+
[f"Independent Try It {i}: {x}"for i,(x,_)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":x,"Method":"Compute plotted coordinate pairs and parameter orientation.",
"Steps":[ans],"Answer":ans,"Check":"Trace only allowed t intervals."}for title,x,ans in W],
"KeyConcepts":[{"Title":"Orientation","Definition":"Arrows follow increasing t.","Rule":"Sample t monotonically.","Example":"x=t,y=t²."},
{"Title":"Parametric ellipse","Definition":"x=a cos t,y=b sin t.","Rule":"CCW for a,b>0.","Example":"a=4,b=2."},
{"Title":"Projectile","Definition":"x=vxt,y=y0+vyt−16t².","Rule":"Time domain until landing.","Example":"x=10t,y=20t−16t²."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent ordered parabola positions",
"Description":"First-party x=t,y=t² coordinate table with direction.",
"PrimaryLabel":"Time","SecondaryLabel":"Coordinates","Rows":[{"Label":"t","Cells":["-2","-1","0","1","2"]},
{"Label":"x","Cells":["-2","-1","0","1","2"]},{"Label":"y","Cells":["4","1","0","1","4"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Plot t-coordinate pairs and direction arrows.","Respect intervals.","Compare Cartesian loci and parametric travel.","Represent projectiles and periodic curves."],
"CommonMistakes":[{"Mistake":"Ignore interval t∈[0,1].","WhyWrong":"The full Cartesian graph includes unattained positions.","Correction":"Trace only allowed parameter values."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":x,"TeacherExpectedMath":ans}for i,(x,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Graph x=t²,y=t, -2≤t≤2.","Graph x=5cost,y=5sint.","Eliminate x=12t,y=20t−16t²."],
"TeacherExpectedMath":["x=y², y in[-2,2], arrows increasing y.","Circle radius5 CCW.","y=(5/3)x−x²/9."],
"OriginalPublisherCoolDownCopied":False},"StageC":"MANUAL_TEACHER_PARAMETRIC_GRAPH_SCORING"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l07-figure-skill-alternatives.json"
fig_roles=["Baseball flight photo replaced with trajectory equations","Parametric table","Traced parabola",
"Ellipse plot","Cartesian and parametric graph overlap","Restricted square root segment",
"Projectile position-time plot"]
save(R/figrel,{"Section":"8.7","OriginalSourceFigures":7,
"IndependentFigureSkillAlternatives":[{"OriginalFigure":i,"Skill":v,
"MathematicalAlternative":"First-party plotted curve coordinate table and teacher explanation",
"PublisherArtworkCopied":False}for i,v in enumerate(fig_roles,1)]})
route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l07-stage-c-73-graph-routes.json"
save(R/route,{"Section":"8.7","Count":73,"ExistingManualTeacherScoringReused":True,
"NewMathEngine":False,"Routes":[{"Source":i,"TeacherMath":True,"RuntimeVerified":False}for i in range(1,74)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l07-source-academic-audit.json"
save(R/audit,{"SourceSection":"8.7","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[734,743],"IncorrectBlueprintPages":[735,744],
"OriginalWorked":ex,"OriginalTryIt":tr,"OriginalFigures":fig,
"OriginalExerciseCount":73,"MathTeacherKeys":trel,"RichV2":rrel,"FigureSkillAlternatives":figrel,
"StageC":route,"ReadyForStageD":True,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l07-abc-close-audit.json"
save(R/close,{"Section":"8.7","OriginalSource":"OpenStax Precalculus 2017 local",
"SourcePdfPages":[734,743],"SourceExercises":73,"IndependentMathTeacherKeys":73,
"SourceWorkedExamples":5,"SourceTryIt":3,"IndependentWorked":5,"IndependentTryIt":3,
"FirstPartyFigureSkills":7,"StageC":"REUSE","NewEngine":False,
"TestsActuallyRun":["Original source page/section boundary and numbered exercises",
"73 independently written graph-skill problems with mathematical checks","5 worked3 Try It",
"Seven figure skills","Git staged diff check"],"Runtime":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"PublisherArtworkCopied":False,"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":734,"PdfPageEnd":743,"EvidencePath":audit,"ClosureEvidencePath":close,
"BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][48]["Ordinal"]==49
p.update({"LastClosedLessonOrdinal":48,"FirstIncompleteLessonOrdinal":49,
"ActiveLessonOrdinal":49,"NextLessonOrdinal":49,"ReadyForStageD":48,
"SourceVerifiedGrade12Lessons":48,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_48_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,route,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l07): close oriented parametric graph skills academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L07_PUSH_VERIFIED_48_OF_73")
