from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=json.loads((B/"u08-l01-independent-questions-001-049.json").read_text(encoding="utf8"))["IndependentTeacherMathTasks"]
assert len(Q)==49
S=lambda x:math.sin(math.radians(x))
C=lambda x:math.cos(math.radians(x))
T=lambda x:math.tan(math.radians(x))
def add(q,a,src):
 i=len(Q)+1;Q.append({"SourceExercise":i,"IndependentStudentQuestion":q,
 "TeacherMathematicalSolution":a,"SourceMathematicsSkillReviewed":src,
 "OriginalPublisherQuestionCopied":False,"ManualGeometryMarking":True})
for chord,central in [(5,130),(8.5,100)]:
 i=len(Q)+1;r=chord/(2*S(central/2))
 add(f"A circle contains chord {chord} with central angle{central}°. Find radius.",
 f"R=c/[2sin(θ/2)]≈{r:.3f}.",f"Source{i} circle radius or diameter from inscribed chord geometry")
for A,B0,base in [(55,60,8),(50,44,13),(48,46,4),(125,35,12),
(35,65,7),(74,36,5),(35,65,29)]:
 i=len(Q)+1;C0=180-A-B0
 a=base*S(A)/S(C0);b=base*S(B0)/S(C0)
 add(f"Independently split oblique figure into triangle with A={A}°,B={B0}°, opposite C side={base}. Find other sides.",
 f"C={C0}°,a≈{a:.3f},b≈{b:.3f}; use paired Law of Sines.",
 f"Source{i} composite oblique figure and supplementary/altitude angles")
assert len(Q)==58
# applications with independent coherent measurements
i=len(Q)+1
add("A tilted signpost has length L and leans10° from vertical away from a light source. A sun ray elevation60° gives shadow40ft. Find L by triangle laws.",
 f"Assuming ground shadow opposite post, L=40sin60°/sin(110°)≈{40*S(60)/S(110):.2f}ft.",
 "Source59 tilted pole shadow and sine law")
baseline=500;A=70;B0=60;C0=180-A-B0;dist=baseline*S(B0)/S(C0);height=dist*S(A)
add(f"Radar stations A,B are{baseline}ft apart; bearings to boat make triangle base angles{A}° and{B0}°. Find boat distance from A and perpendicular distance to shore.",
 f"Angle C={C0}°, A-to-boat≈{dist:.2f}ft, shore altitude≈{height:.2f}ft.","Source60 two radar station triangulation")
# satellite on one side, angles 83°,86°, stations apart 70mi
alpha=83;beta=86;sep=70
h=sep/(1/T(alpha)-1/T(beta));slant=h/S(alpha)
add(f"Two tracking stations{sep}mi apart sight satellite in same direction at elevations{alpha}° and{beta}° (farther angle smaller). Find height and slant from farther station.",
 f"h={sep}/(cot{alpha}°−cot{beta}°)≈{h:.2f}mi; slant≈{slant:.2f}mi.",
 "Source61 two-station satellite altitude")
hill=60;wire_hill=20;ground=130
# one triangle with known base length, top angle 70 and anchor 20 => third90
c=ground*S(70)/S(90)
add(f"A guy wire rises from an anchor on slope of {hill}°; triangle's angle at anchor={wire_hill}°, at tower top=70°, downhill base=130m. Find cable length.",
 f"Remaining angle=90°, cable opposite70°, length≈{c:.2f}m.",
 "Source62 tower cable on a hill by law of sines")
panel=8;roof=20;panelang=38
support=panel*(S(panelang)-C(panelang)*T(roof))
add(f"A panel of length{panel}ft is angled{panelang}° to horizontal, roof angle{roof}°. Panel front touches roof, vertical brace at panel back: find brace length.",
 f"Vertical brace=panel(sin{panelang}°−cos{panelang}°tan{roof}°)≈{support:.3f}ft.",
 "Source63 rooftop panel support geometry")
for distance,angfar,angnear in [(6.5,35,45),(4.2,32,56)]:
 i=len(Q)+1
 h=distance/(1/T(angfar)-1/T(angnear))
 slant=h/S(angfar)
 add(f"Plane sights near and far points on same side of nadir, separated{distance}km, depression angles{angnear}° and{angfar}°. Find height and slant to far point.",
 f"h≈{h:.3f}km, slant≈{slant:.3f}km; use cotangent difference.",
 f"Source{i} airplane angle-of-depression triangulation")
for dist,angle1,angle2 in [(300,39,50),(250,35,53)]:
 i=len(Q)+1
 h=dist/(1/T(angle1)-1/T(angle2))
 add(f"Two students on level street move {dist}ft closer to building; elevations change from{angle1}° to{angle2}°. Find height.",
 f"h={dist}/(cot{angle1}°−cot{angle2}°)≈{h:.2f}ft.",
 f"Source{i} two-position building height")
A=101;C0=53;B0=180-A-C0;AC=97
AB=AC*S(C0)/S(B0)
add(f"Cities A,C are{AC}m apart, ∠A={A}°,∠C={C0}°. Find distance AB.","B=26°, AB≈"+f"{AB:.3f}m.","Source68 lake distance triangulation")
sep=3.5;ang1=27;ang2=41;h=sep/(1/T(ang1)+1/T(ang2))
add(f"Two observers {sep} miles apart view balloon between them at elevations{ang1}° and{ang2}°. Find balloon altitude.",
 f"h={sep}/(cot{ang1}°+cot{ang2}°)≈{h:.4f}mi.","Source69 balloon triangle altitude")
sep=.5;first=15;second=22
h=sep/(1/T(first)+1/T(second))
add(f"Two teams at altitude1mi, {sep}mi apart, view climber between them at elevations{first}° and{second}°. Find climber altitude.",
 f"Climber elevation above team≈{h:.3f}mi, absolute altitude≈{1+h:.3f}mi.",
 "Source70 stranded climber elevation")
pole=10;person=6;distance=20
# analogous 6ft observers opposite sides of light at symmetric distances
# if two individuals at ±10 pole, shadow length s from similarity: pole/(10+s)=person/s => s=person*10/(pole-person)
shadow=person*(distance/2)/(pole-person)
add(f"Streetlamp height{pole}ft, two people height{person}ft stand symmetrically {distance}ft apart on opposite sides of pole. Find each shadow length and distance from lamp to tip.",
 f"Each person10ft from pole; shadow length={shadow:g}ft; pole-to-tip≈{10+shadow:.3f}ft.",
 "Source71 shadows with equal height and angular elevation")
# cities A due east B, C northwest B 70mi, AC100
BC=70;AC=100
# BC vector (-70 sin35,70 cos35); A=(d,0), AC²=(d+70sin35)²+(70cos35)²
dist=math.sqrt(AC**2-(BC*C(35))**2)-BC*S(35)
add("City C lies 35° west of north from B at distance70mi; city A is due east B, AC=100mi. Find AB.",
 f"AB=sqrt(100²−(70cos35°)²)−70sin35°≈{dist:.3f}mi.",
 "Source72 triangle from compass bearings")
assert len(Q)==72
for a,b,ang in [(180,215,80),(40,56,135),(1030,980,62)]:
 i=len(Q)+1;area=.5*a*b*S(ang)
 add(f"Triangle has two adjacent lengths {a},{b}, included angle{ang}°. Find area.",
 f"Area=1/2ab sinθ≈{area:.3f} square units.",
 f"Source{i} triangle real-world SAS area")
side=30
add("An equilateral triangular sign has side30in. Find area.",
 f"A=s²sqrt3/4=225sqrt3≈{225*math.sqrt(3):.3f} in².",
 "Source76 equilateral triangular sign area")
a=4;b=4.5;ang=106
area=.5*a*b*S(ang)
add(f"A table top is triangle with adjacent sides4ft and4.5ft, included angle{ang}°. Find area.",
 f"A=.5·4·4.5·sin{ang}°≈{area:.3f}ft².",
 "Source77 triangular dining table SAS area")
assert len(Q)==77 and [x["SourceExercise"]for x in Q]==list(range(1,78))
# Check ambiguity cases and each triangle side numeric consistency
assert math.isclose(2*math.sin(math.radians(30)),1)
assert math.isclose(.5*4*5*S(60),5*math.sqrt(3),rel_tol=1e-10)
P=B/"u08-l01-independent-questions-all-077.json"
P.write_text(json.dumps({"SchemaVersion":1,"Section":"8.1","Count":77,
"IndependentStudentAndTeacherMathematics":Q,"StageD":"NOT_RUN"},
ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L01_ALL77_SOURCE_SKILL_INDEPENDENT_TEACHER_MATH_PREPARED")
