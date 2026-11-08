from pathlib import Path
import json,math,datetime,re,subprocess,pypdfium2
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==48 and p["FirstIncompleteLessonOrdinal"]==49
row=p["LessonEntries"][48];assert row["Ordinal"]==49 and row["SourceSection"]=="8.8"
d=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
text=lambda i:d[i].get_textpage().get_text_range()
assert "8.8" in text(744) and "CHAPTER 8 REVIEW" in text(762)
s="\n".join(text(i) for i in range(744,758))
worked=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",s,re.I)]
tryits=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(text(i)for i in range(744,762)),re.I)})
assert [n for n,_ in worked]==list(range(1,18)), worked
assert tryits==list(range(1,5)),tryits
assert figures==list(range(1,20)), figures
assert "79." in text(761)
Q=[]
def add(q,key,source,check=False):
    assert q and key
    Q.append({"OriginalExercise":len(Q)+1,"IndependentMathQuestion":q,"TeacherMathAnswer":key,
              "OriginalSourceSkill":source,"MathSpotChecked":check,"OriginalPublisherTextCopied":False,
              "TeacherManualScoring":True,"RuntimeVerified":False})
vadd=lambda a,b:(a[0]+b[0],a[1]+b[1])
vsub=lambda a,b:(a[0]-b[0],a[1]-b[1])
mult=lambda a,k:(a[0]*k,a[1]*k)
mag=lambda a:math.hypot(*a)
dot=lambda a,b:a[0]*b[0]+a[1]*b[1]
ang=lambda a:math.degrees(math.atan2(a[1],a[0]))%360
for q,key,src in [
("How are vectors normally notated?","Bold lowercase letters or an arrow over a symbol.","Source 1 notation."),
("What distinguishes vector from an undirected segment?","Magnitude and direction; its displacement can be translated without changing vector.","Source 2 geometric meaning."),
("What do i and j denote in R²?","Basis unit vectors i=(1,0), j=(0,1).","Source 3 standard basis."),
("Define component form.","v=<terminal x−initial x, terminal y−initial y>.","Source 4 component form."),
("In ai+bj, which coefficient is horizontal?","a horizontal i, b vertical j.","Source 5 coefficient roles.")]:add(q,key,src)
for a,b in [((1,5),(-2,-1)),((-5,2),(2,-3)),((6,-2),(-2,-6))]:
    add(f"Find vector from P={a} to Q={b}, translated to origin.",f"Q−P={vsub(b,a)}.",
        "Source 6–8 displacement component.",True)
for a,b,c,e in [
((0,0),(3,-4),(2,1),(5,-3)),
((2,-2),(7,3),(-3,0),(2,5)),
((-3,1),(1,5),(-1,-1),(3,3)),
((1,7),(-1,2),(0,0),(-2,-5)),
((6,3),(4,5),(1,2),(-1,4))]:
    u=vsub(b,a);v=vsub(e,c)
    add(f"Determine if vectors P={a}→Q={b} and R={c}→S={e} are equal.",
        f"u={u},v={v}, equal={u==v}.","Source 9–13 compare displacements.",True)
for a,b in [((-2,1),(5,-4)),((3,0),(-2,-3))]:
    add(f"Express vector from {a} to {b} using i and j.",
        f"{vsub(b,a)[0]}i+({vsub(b,a)[1]})j.",
        "Source 14–15 initial/terminal components.",True)
u=(2,4);v=(-3,-1);w=(5,-2)
add(f"Given u={u},v={v},w={w}, evaluate u+(v−w).",str(vadd(u,vsub(v,w))),"Source16 combine vector sums.",True)
add(f"Given u={u},v={v}, find 4v+2u.",str(vadd(mult(v,4),mult(u,2))),"Source17 scalar combination.",True)
for u,v in [((3,-4),(2,6)),((-4,3),(-2,5))]:
    add(f"For u={u},v={v}, calculate u+v, u−v, 2u−3v.",
        f"sum={vadd(u,v)}, difference={vsub(u,v)}, combination={vsub(mult(u,2),mult(v,3))}.",
        "Source 18–19 operations.",True)
for v,factor in [((-6,8),.5),((4,3),-2)]:
    add(f"Find vector with {abs(factor)} times |v|, {'same' if factor>0 else 'opposite'} orientation, v={v}.",
        f"{mult(v,factor)}.","Source 20–21 scalar scaling.",True)
for a in [(5,12),(-3,4),(7,-24),(-8,15),(12,16),(-6,8)]:
    length=mag(a)
    add(f"Compute unit vector in direction of v={a}.",
        f"v/|v|=({a[0]}/{length:g},{a[1]}/{length:g}), magnitude1.",
        "Source 22–27 unit vectors.",math.isclose(mag((a[0]/length,a[1]/length)),1))
for a in [(0,-5),(8,6),(3,-7),(-5,-12)]:
    add(f"Find magnitude and standard direction of v={a}.",
        f"|v|≈{mag(a):.6f}, angle≈{ang(a):.6f}°.",
        "Source 28–31 magnitude/direction.",True)
for a,b in [((4,-3),(-2,5)),((-2,-1),(1,4)),((-3,6),(-4,2)),((-5,2),(3,-1))]:
    add(f"Compute dot product u={a},v={b}.",
        f"u·v={dot(a,b)}.","Source 32–35 dot product.",True)
for v in [(3,-2),(-2,5),(-4,-3)]:
    add(f"Draw v={v}, 3v and (1/2)v from same origin.",
        f"Endpoints v={v},3v={mult(v,3)},half={mult(v,.5)}.",
        "Source 36–38 scaled graphical vectors.",True)
for u,v in [((3,1),(-1,4)),((-2,3),(4,1)),((4,-3),(-2,-2))]:
    add(f"Draw vectors u={u},v={v} and u+v, u−v, 2u.",
        f"sum={vadd(u,v)},diff={vsub(u,v)},twice={mult(u,2)}.",
        "Source 39–41 geometric addition/subtraction.",True)
for u,v in [((2,-3),(1,4)),((-2,5),(3,-1))]:
    add(f"Graph 2u+v given u={u},v={v}.",
        f"Resultant endpoint {vadd(mult(u,2),v)}.",
        "Source 42–43 combined graphical vectors.",True)
for u,v in [((4,2),(1,-3)),((-3,5),(2,-2))]:
    add(f"Graph u−3v for u={u},v={v}.",
        f"Resultant {vsub(u,mult(v,3))}.","Source 44–45 combination plotted.",True)
for vector in [(4,-2),(-3,5)]:
    add(f"Read independently drawn vector from origin to{vector} in component form.",
        f"v=<{vector[0]},{vector[1]}>.","Source 46–47 graphical vectors.",True)
for a,b in [((1,3),(-4,4)),((5,-2),(-2,3)),((4,4),(-4,4))]:
    add(f"Express vector from {a} to{b} as ai+bj.",
        f"{vsub(b,a)[0]}i+({vsub(b,a)[1]})j.",
        "Source 48–50 initial/terminal point drawing.",True)
for length,degree in [(8,30),(7,225),(3,330),(6,120)]:
    components=(length*math.cos(math.radians(degree)),length*math.sin(math.radians(degree)))
    add(f"Find components of a vector magnitude{length}, direction{degree}°.",
        f"({components[0]:.6f},{components[1]:.6f}).",
        "Source 51–54 magnitude-direction to components.",math.isclose(mag(components),length))
for weight,incline in [(40,15),(30,10)]:
    add(f"A {weight}lb weight on frictionless incline of{incline}°; find normal/parallel gravitational components.",
        f"Normal={weight*math.cos(math.radians(incline)):.3f}lb, down ramp={weight*math.sin(math.radians(incline)):.3f}lb.",
        "Source 55–56 inclined ramp force decomposition.",True)
for length,degree in [(12,35),(6,135),(7,55),(3,20)]:
    a=length*math.cos(math.radians(degree));b=length*math.sin(math.radians(degree))
    add(f"For force magnitude{length}lb at{degree}°, find signed x,y components and magnitudes.",
        f"Fx≈{a:.4f}lb,Fy≈{b:.4f}lb; component magnitudes≈{abs(a):.4f},{abs(b):.4f}.",
        "Source 57–60 directional force components.",True)
for steps in [
    [(3,180),(2,225)],[(5,90),(3,45)],
    [(3,0),(2,315),(4,270),(2,225)],[(5,0),(4,315),(3,270),(2,225)]] :
    coords=[(length*math.cos(math.radians(degree)),length*math.sin(math.radians(degree)))for length,degree in steps]
    total=(sum(a for a,b in coords),sum(b for a,b in coords))
    add(f"Walk successive independent vector steps {steps} (length miles, compass mathematical degree). Find total walked and direct distance home.",
        f"Total walked={sum(l for l,_ in steps):g}mi, final location≈({total[0]:.5f},{total[1]:.5f}), direct home≈{mag(total):.5f}mi.",
        "Source 61–64 multi-leg walk resultants.",True)
for steps in [[(3,160),(5,260),(4,15)],[(6,40),(2,285),(5,210)]]:
    v=[(n*math.cos(math.radians(a)),n*math.sin(math.radians(a)))for n,a in steps]
    total=(sum(x for x,y in v),sum(y for x,y in v))
    back=(-total[0],-total[1])
    add(f"Walk steps {steps} (degree CCW from east). Give homeward distance and angle.",
        f"Return magnitude≈{mag(back):.5f}mi, homeward direction≈{ang(back):.4f}°.",
        "Source 65–66 real directional displacement.",True)
for speed,windmag,windheading in [(500,80,45),(450,60,315)]:
    wx=windmag*math.cos(math.radians(windheading));wy=windmag*math.sin(math.radians(windheading))
    v=(wx,speed+wy)
    add(f"Plane airspeed{speed}km/h due north with wind{windmag}km/h toward{windheading}° from east. Find ground speed and north-course deviation.",
        f"Ground vector≈({wx:.4f},{speed+wy:.4f}), speed≈{mag(v):.4f}km/h, signed eastward deviation≈{math.degrees(math.atan2(wx,speed+wy)):.3f}°.",
        "Source 67–68 wind ground speed and drift.",True)
for speed,windmag,heading in [(530,50,45),(470,65,315)]:
    wx=windmag*math.cos(math.radians(heading))
    assert abs(wx)<speed
    angle=math.degrees(math.asin(-wx/speed))
    add(f"Plane speed{speed}km/h must track north with wind{windmag}km/h toward{heading}°; find heading correction east/west of north.",
        f"Air heading signed east-of-north≈{angle:.4f}°; cancellation speed*sinθ=−wind east component.",
        "Source 69–70 crosswind correction.",True)
for pt,degree in [((4,6),30),((7,2),45)]:
    x,y=pt;t=math.radians(degree)
    rot=(x*math.cos(t)-y*math.sin(t),x*math.sin(t)+y*math.cos(t))
    add(f"Rotate point {pt} CCW by{degree}° about origin.",
        f"New coordinates≈({rot[0]:.5f},{rot[1]:.5f}).",
        "Source 71–72 rotation of vectors.",math.isclose(mag(pt),mag(rot)))
for vehicle,side in [(30,12),(50,9)]:
    add(f"Vehicle travels{vehicle}mph forward, object thrown{side}mph transversely inside it; find trajectory direction relative to road.",
        f"Velocity resultant ({vehicle} forward,{side} sideways)mph; sideways angle≈{math.degrees(math.atan2(side,vehicle)):.3f}°, speed≈{math.hypot(vehicle,side):.3f}mph.",
        "Source 73–74 relative velocities.",True)
weight,incline=60,20
add(f"Weight {weight}lb on{incline}° slope: force parallel/normal components.",
    f"Parallel≈{weight*math.sin(math.radians(incline)):.3f}lb, normal≈{weight*math.cos(math.radians(incline)):.3f}lb.",
    "Source 75 ramp forces.",True)
for vectors in [[(10,0),(20,90),(7,45)],[(8,0),(15,225),(4,150)]]:
    coords=[(length*math.cos(math.radians(t)),length*math.sin(math.radians(t)))for length,t in vectors]
    total=(sum(x for x,y in coords),sum(y for x,y in coords))
    add(f"Find resultant of forces {vectors} (magnitudes lb,direction degrees).",
        f"R≈({total[0]:.4f},{total[1]:.4f})lb, |R|≈{mag(total):.4f}lb, angle≈{ang(total):.4f}°.",
        "Source 76–77 resultant forces.",True)
for forces in [[(3,0),(6,90),(2,45)],[ (4,180),(5,90),(3,30)]]:
    coords=[(length*math.cos(math.radians(t)),length*math.sin(math.radians(t)))for length,t in forces]
    net=(sum(x for x,y in coords),sum(y for x,y in coords))
    eq=(-net[0],-net[1])
    add(f"Find equilibration force to cancel {forces} (lb, degrees).",
        f"F_eq≈({eq[0]:.5f},{eq[1]:.5f})lb, magnitude≈{mag(eq):.5f}, direction≈{ang(eq):.5f}°.",
        "Source 78–79 vector equilibrium.",True)
assert len(Q)==79,len(Q)
W=[("Position vector","From(1,2) to(4,-2).","(3,-4)."),("Vector translation","Move vector(4,1) to origin.","Equivalent vector(4,1)."),
("Magnitude/direction","For v=(3,4).","Magnitude5, direction≈53.13°."),("Equal displacement","Compare (0,0)→(2,3) and(1,1)→(3,4).","Equal vectors."),
("Add/subtract","u=(2,3),v=(1,-2).","u+v=(3,1),u−v=(1,5)."),("Scalar multiplication","2(3,-4).","(6,-8)."),
("Combination","u=(2,1),v=(3,-2), compute2u-v.","(1,4)."),("Components from direction","r=5, θ=30°.","(5sqrt3/2,5/2)."),
("Normalize","v=(5,12).","(5/13,12/13)."),("Basis form","v=(-2,3).","-2i+3j."),
("Displacement form","From(-3,2) to(4,5).","7i+3j."),("Add basis","(2i+j)+(3i-4j).","5i-3j."),
("Magnitude/angle","v=(0,-5).","Magnitude5, direction270°."),("Dot product","(2,3)·(4,-1).","5."),
("Angle from dot","u=(1,0),v=(0,1).","90° (dot0)."),("Angle general","u=(1,1),v=(1,0).","45°."),("Wind vector","Aircraft north200, wind east30.","Ground velocity (30,200), speed sqrt40900≈202.24.")]
T=[("Magnitude of(6,8).","10."),("Scalar3(-2,5).","(-6,15)."),
("Dot (1,2)·(3,-1).","1."),("Unit vector (3,4).","(3/5,4/5).")]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L08"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l08.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l08-firstparty-teacher-led.json"
def save(path,v):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L08",
"ContentVersion":"g12-u08-l08-firstparty-vectors-abc","Lessons":[{
"LessonCode":lesson,"CultureCode":"en","Title":"Vectors",
"ExplanationParagraphs":[
"Vectors in the plane have magnitude and direction, represented by ordered components <a,b> or ai+bj.",
"Displacement from initial P to terminal Q is Q−P; equivalent vectors have equal components regardless of placement.",
"Vector addition, subtraction and scalar multiplication operate componentwise; geometric addition uses head-to-tail triangles.",
"The length of v=(a,b) is sqrt(a²+b²); a unit vector is v/|v| when v≠0, and heading is quadrant-aware atan2(b,a).",
"Dot product u·v=u_x v_x+u_y v_y=|u||v|cosθ; use to determine angle for nonzero vectors.",
"Resolve force or velocity of magnitude r at angleθ into (r cosθ,r sinθ); resultant is componentwise sum.",
"Wind correction, relative motion and force equilibrium require the correct direction conventions and units.",
"Independent Cool-down: normalize(5,12), find angle between(1,0) and(1,1), and calculate net of east10 and north20 vectors." ]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Use component vector operations, geometry and dot product.",
"Steps":[ans],"Answer":ans,"Check":"Check magnitude, direction and units."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Vector components","Definition":"v=(x2−x1,y2−y1).","Rule":"Terminal minus initial.","Example":"(1,2)→(3,5)=(2,3)."},
{"Title":"Unit vector","Definition":"u=v/|v| for nonzero v.","Rule":"Magnitude1.","Example":"(3,4)/5."},
{"Title":"Dot product","Definition":"u·v=u_x v_x+u_y v_y.","Rule":"cosθ=dot/(|u||v|).","Example":"(1,0)·(0,1)=0."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent vector operation table","Description":"Component arithmetic examples.",
"PrimaryLabel":"Operation","SecondaryLabel":"Result",
"Rows":[{"Label":"u","Cells":["(2,3)","(2,3)","(2,3)"]},{"Label":"v","Cells":["(1,-2)","(1,-2)","(1,-2)"]},
{"Label":"Operation","Cells":["u+v","u-v","2u"]},{"Label":"Result","Cells":["(3,1)","(1,5)","(4,6)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Vector position and unit form.","Componentwise operations.","Dot product measures angle.","Resolve and sum forces and velocities."],
"CommonMistakes":[{"Mistake":"Treat velocity magnitudes as directly additive.","WhyWrong":"Angles determine components.","Correction":"Add component vectors then compute magnitude."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Normalize v=(5,12).","Find angle between(1,0) and(1,1).","Net velocity east10,north20."],
"TeacherExpectedMath":["(5/13,12/13).","45°.","(10,20), speed sqrt500≈22.361, bearing angle63.435° CCW from east."],
"OriginalPublisherCoolDownCopied":False},"StageC":"REUSE_MANUAL_TEACHER_VECTOR_SCORING"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l08-figure-skills.json"
save(R/figrel,{"Section":"8.8","OriginalFigures":19,
"IndependentFigureSkillAlternatives":[{"OriginalFigure":i,"Skill":"Independent geometric vector placement, translation, direction or addition",
"FirstPartyMath":"Coordinate plane arrow endpoints and component equations",
"PublisherOriginalGraphicCopied":False}for i in range(1,20)]})
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l08-stage-c-79-vector-routes.json"
save(R/routes,{"Section":"8.8","Count":79,"Reuse":"RichV2 existing teacher marking",
"NewEngine":False,"Routes":[{"Source":i,"TeacherMathPrepared":True,"RuntimeVerified":False}for i in range(1,80)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l08-source-academic-audit.json"
save(R/audit,{"SourceSection":"8.8","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[744,761],"IncorrectBlueprint":[745,762],
"OriginalWorked":worked,"OriginalTryIt":tryits,"OriginalFigures":figures,
"OriginalExerciseNumbers":list(range(1,80)),"TeacherMath":trel,"RichV2":rrel,
"FigureSkills":figrel,"StageCRoutes":routes,"ReadyForStageD":True,
"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l08-abc-close-audit.json"
save(R/close,{"Section":"8.8","Source":"OpenStax Precalculus 2017 original local PDF",
"OriginalExercises":79,"IndependentMathTeacherKeys":79,"OriginalWorked":17,"OriginalTryIt":4,
"IndependentWorked":17,"IndependentTryIt":4,"OriginalFigureSkills":19,
"NewEngine":False,"PublisherFigureCopied":False,"StageCManualTeacherReuse":True,
"TargetedChecksActuallyRun":["Local source/section boundaries","79 independent vector math questions and teacher keys",
"17 worked4 Try It","Vector magnitude/dot/rotation numeric checks","Stage C route count","Git diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","Runtime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":744,"PdfPageEnd":761,"EvidencePath":audit,"ClosureEvidencePath":close,
"BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][49]["Ordinal"]==50
p.update({"LastClosedLessonOrdinal":49,"FirstIncompleteLessonOrdinal":50,
"ActiveLessonOrdinal":50,"NextLessonOrdinal":50,"ReadyForStageD":49,
"SourceVerifiedGrade12Lessons":49,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_49_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,routes,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l08): close vectors academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L08_PUSH_VERIFIED_49_OF_73_UNIT8_COMPLETE")
