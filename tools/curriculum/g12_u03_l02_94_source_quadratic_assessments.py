from pathlib import Path
import json,math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson002.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.2"
items={}
def put(n,skill,prompt,answer,parts=None):
 assert n not in items,n
 items[n]={"Number":n,"OriginalSourcePDFPage":236 if n<=52 else 237 if n<=73 else 238,
 "OriginalSourceAcademicSkill":skill,
 "FirstPartyIndependentStudentPrompt":prompt,
 "TeacherExactMathematicsAndScoring":answer,
 "SourceCognitiveActions":parts or ["show algebraic reasoning","state exact mathematics with domain if applicable"],
 "OriginalPublisherQuestionVerbatimCopied":False,
 "OriginalPublisherGraphBitmapCopied":False,
 "StudentTeacherAppRuntime":"NOT_RUN"}
def vert(a,b,c):
 h=-F(b)/(2*a);k=a*h*h+b*h+c
 return h,k
def roots(a,b,c):
 delta=b*b-4*a*c
 if delta>=0:
  return f"Δ={delta} ≥0, roots x=(−({b})±√({delta}))/(2·{a}), simplify if possible."
 return f"Δ={delta}<0, complex roots x=(−({b})±i√({-delta}))/(2·{a})."
def quadstr(a,b,c):return f"({a})x²+({b})x+({c})"
for n,prompt,ans in[
(1,"Explain why vertex form a(x−h)²+k is useful for quadratic graphs.","It exposes vertex (h,k), symmetry axis x=h and extremum k directly."),
(2,"How does the vertex help optimize revenue or projectile height?","Choose vertex input h if it is in physical domain, then evaluate k and compare boundaries."),
(3,"Why is a≠0 required in f(x)=ax²+bx+c?","If a=0, it becomes a linear/constant expression rather than degree-2 function."),
(4,"Give the alternate name for vertex form.","Vertex form is also called standard form in this source; be explicit because terminology differs across textbooks."),
(5,"Name two algebraic methods to find x-intercepts of a quadratic.","Factoring when possible, and quadratic formula (also completing the square).")
]:put(n,"QUADRATIC_VERBAL_DEFINITIONS",prompt,ans)
coeff={
6:(1,-12,32),7:(1,2,-3),8:(1,-1,0),9:(1,5,-2),
10:(2,8,-10),11:(3,-6,-9),12:(2,-6,0),13:(3,-5,-1)
}
for n,(a,b,c)in coeff.items():
 h,k=vert(a,b,c)
 put(n,"CONVERT_QUADRATIC_GENERAL_TO_VERTEX",
 f"Complete the square for quadratic f(x)={quadstr(a,b,c)}. Write vertex form and vertex coordinates.",
 f"f(x)=({a})(x−({h}))²+({k}); vertex ({h},{k}).",
 ["complete square","state vertex form","state exact vertex"])
extr={
14:(2,10,12),15:(2,-10,4),16:(-1,4,3),17:(4,1,-1),
18:(-4,6,-1),19:(F(1,2),3,1),20:(F(-1,3),-2,3)
}
for n,(a,b,c)in extr.items():
 h,k=vert(a,b,c);kind="minimum"if a>0 else "maximum"
 put(n,"QUADRATIC_OPTIMIZATION_VERTEX_AXIS",
 f"For f(x)={quadstr(a,b,c)}, determine whether the parabola has a global minimum or maximum, its exact value and axis of symmetry.",
 f"{kind} {k} at x={h}; symmetry axis x={h}.",
 ["determine opening","find axis x=−b/2a","calculate optimum"])
dr={
21:("1(x−3)²+2",1,3,2),
22:("−2(x+3)²−6",-2,-3,-6),
23:("x²+6x+4",1,-3,-5),
24:("2x²−4x+2",2,1,0),
25:("3x²−6x−9",3,1,-12)
}
for n,(f,a,h,k)in dr.items():
 direction=f"[{k},∞)"if a>0 else f"(−∞,{k}]"
 put(n,"QUADRATIC_DOMAIN_RANGE",
 f"Find domain, range and vertex for quadratic f(x)={f}, with no additional input restriction.",
 f"Domain all real numbers, vertex ({h},{k}), range {direction}.",
 ["find vertex","determine direction","state domain and range"])
quadratics={
26:(1,0,25),27:(1,0,8),28:(1,0,36),29:(1,0,27),
30:(1,2,5),31:(1,-4,5),32:(1,8,25),33:(1,-4,13),
34:(1,6,25),35:(1,-10,26),36:(1,-6,10),
37:(1,-4,-20),38:(1,-2,-10),39:(2,2,5),
40:(5,-8,5),41:(5,6,2),42:(2,-6,5),
43:(1,1,2),44:(1,-2,4)}
for n,(a,b,c)in quadratics.items():
 put(n,"SOLVE_QUADRATIC_OVER_COMPLEX_NUMBERS",
 f"Solve the quadratic equation {quadstr(a,b,c)}=0 over C, keeping exact radical or imaginary roots.",
 roots(a,b,c),["calculate discriminant","apply quadratic formula","state roots over complex numbers"])
vertices={
45:((2,0),(4,4)),46:((-2,-1),(-4,3)),47:((0,1),(2,5)),
48:((2,3),(5,12)),49:((-5,3),(2,9)),50:((3,2),(10,1)),
51:((0,1),(1,0)),52:((1,0),(0,1))
}
for n,((h,k),(x,y))in vertices.items():
 a=F(y-k,(x-h)**2)
 b=-2*a*h;c=a*h*h+k
 put(n,"FIND_QUADRATIC_FROM_VERTEX_AND_POINT",
 f"Given first-party parabola vertex ({h},{k}) and point ({x},{y}), derive f(x)=ax²+bx+c and vertex form.",
 f"a={a}; f(x)=({a})(x−({h}))²+({k})=({a})x²+({b})x+({c}).",
 ["substitute point in vertex form","find a","expand to general form"])
draw={
53:(1,-2,0),54:(1,-6,-1),55:(1,-5,-6),
56:(1,-7,3),57:(-2,5,-8),58:(4,-12,-3)}
for n,(a,b,c)in draw.items():
 h,k=vert(a,b,c)
 put(n,"SKETCH_PARABOLA_VERTEX_AXIS_INTERCEPTS",
 f"Sketch f(x)={quadstr(a,b,c)} with labelled vertex, symmetry axis and both kinds of intercepts. Show roots exactly.",
 f"Vertex ({h},{k}), axis x={h}, y-intercept (0,{c}), opens {'up'if a>0 else 'down'}; "+roots(a,b,c),
 ["vertex and opening","draw symmetric plotted points","derive y-intercept and roots"])
# Source 59–64 have graphic-only publisher exercises; create independently
# specified vertex/point plots, not copied original bitmap.
graphs={
59:((0,-3),(1,-1)),60:((-2,2),(-1,5)),61:((3,0),(5,8)),
62:((-3,-2),(-1,2)),63:((1,3),(3,-5)),64:((2,-4),(4,-2))
}
for n,((h,k),(x,y))in graphs.items():
 a=F(y-k,(x-h)**2);b=-2*a*h;c=a*h*h+k
 put(n,"INFER_QUADRATIC_EQUATION_FROM_INDEPENDENT_GRAPH",
 f"An original Edulytics graph marks vertex ({h},{k}), axis x={h} and plotted point ({x},{y}). Derive its exact quadratic in general and vertex form.",
 f"a={a}, y=({a})(x−({h}))²+({k})=({a})x²+({b})x+({c}).",
 ["read vertex and plotted point","derive a","expand"])
tables={
65:([-2,-1,0,1,2],[5,2,1,2,5]),
66:([-2,-1,0,1,2],[1,0,1,4,9]),
67:([-2,-1,0,1,2],[-2,1,2,1,-2]),
68:([-2,-1,0,1,2],[-8,-3,0,1,0]),
69:([-2,-1,0,1,2],[8,2,0,2,8])
}
for n,(xs,ys)in tables.items():
 c=F(ys[2]);a=F(ys[1]+ys[3]-2*ys[2],2);b=F(ys[3]-ys[1],2)
 assert all(a*x*x+b*x+c==y for x,y in zip(xs,ys))
 h,k=vert(a,b,c)
 put(n,"FIT_QUADRATIC_TO_EXACT_TABLE",
 f"The first-party table lists x={xs}, f(x)={ys}. Determine the exact quadratic, vertex and symmetry axis.",
 f"f(x)=({a})x²+({b})x+({c}), vertex ({h},{k}), axis x={h}.",
 ["recognize symmetric second differences","solve three coefficients","derive vertex"])
for n,text,answer in[
(70,"Compare parabolas y=x², y=2x² and y=x²/3 on the same independent axes. What does positive scale change?","Larger a narrows, 0<a<1 widens; all have vertex (0,0), open up."),
(71,"Compare y=x², y=x²+2, y=x²+5, y=x²−3. Explain impact of constant.","Vertical shifts +2,+5,−3; shape and axis unchanged."),
(72,"Compare y=x², y=(x−2)², y=(x−3)², y=(x+4)². Explain h in vertex form.","Horizontal shifts right2, right3, left4 from original."),
(73,"A projectile model is h(x)=−x²/200+x in feet for horizontal distance x feet. Evaluate h(100).","h(100)=−10000/200+100=50 ft."),
(74,"An independent suspension bridge model is h(x)=x²/10000 feet for −2000≤x≤2000 feet. Find distance from center for h=100.","|x|=1000 ft, two symmetric points ±1000.")
]:put(n,"QUADRATIC_TRANSFORM_AND_GRAPH_APPLICATION",text,answer)
for n,h,k,up in[
(75,1,-2,True),(76,-1,2,False),(77,-5,11,False),(78,-100,100,True)]:
 put(n,"QUADRATIC_DOMAIN_RANGE_FROM_VERTEX_AND_OPENING",
 f"A parabola has vertex ({h},{k}) and opens {'up'if up else 'down'}. Find domain and range.",
 f"Domain R; range {'['+str(k)+',∞)'if up else '(−∞,'+str(k)+']'}.")
for n,x,y,a,h in[
(79,1,1,2,0),(80,-1,4,2,0),(81,2,3,3,0),
(82,1,-3,-1,0),(83,4,3,5,0),(84,1,-6,3,-1)]:
 k=F(y)-a*(x-h)**2
 b=-2*a*h;c=a*h*h+k
 put(n,"MATCH_QUADRATIC_SHAPE_POINT_AND_VERTEX_AXIS",
 f"Find a quadratic with width/opening coefficient a={a}, vertex x-coordinate h={h} and containing point ({x},{y}). Give vertex and general form.",
 f"k={k}; f(x)=({a})(x−({h}))²+({k})=({a})x²+({b})x+({c}).",
 ["use given coefficient and vertex x","solve vertical shift","expand"])
# Enclosure models: a rectangle with n pens adjacent and n-1 dividers
# perpendicular to length. Total fence = 2L + (n+1)W.
for n,perimeter,pens in [(85,200,1),(86,300,2),(87,500,3)]:
 L=F(perimeter,4);W=F(perimeter,2*(pens+1))
 area=L*W
 put(n,"MAXIMIZE_FENCED_RECTANGULAR_PENS",
 f"Use a total of {perimeter} feet of fence to enclose a rectangle split into {pens} equal adjacent {'pens'if pens>1 else 'pen'}; {max(0,pens-1)} internal dividers perpendicular to length. Determine optimal length, width and maximal area.",
 f"Fence 2L+({pens+1})W={perimeter}; maximize A=LW: L={L}ft,W={W}ft, area={area}ft².",
 ["construct fencing constraint with divider count","optimize quadratic","report dimensions and units"])
put(88,"MAX_PRODUCT_GIVEN_SUM",
    "Two real numbers have sum 6. Determine the pair with maximum product and its value.",
    "a=b=3, maximum product9.")
put(89,"MIN_PRODUCT_GIVEN_DIFFERENCE",
    "Two real numbers have difference12. Determine the pair with minimum product.",
    "Set a=b+12; product b²+12b min at b=−6,a=6; min product −36.")
put(90,"MAXIMIZE_REVENUE_FROM_LINEAR_DEMAND",
    "An independent linear demand model has unit price p=45−0.0125x dollars, x in thousands of phones, total revenue in thousands dollars R=xp. Find the price-maximizing production level and revenue.",
    "R(x)=45x−0.0125x², vertex x=1800 thousand phones, price $22.50, revenue40500 thousand dollars.")
a,b,c=-4.9,229,234
h=-b/(2*a);k=a*h*h+b*h+c
put(91,"MAX_PROJECTILE_HEIGHT",
    "For rocket altitude h(t)=−4.9t²+229t+234 metres (t seconds), determine maximum height and time.",
    f"Peak time t={h:.6f}s, max altitude≈{k:.6f}m.")
put(92,"MAX_PROJECTILE_HEIGHT_TIME",
    "For h(t)=−4.9t²+24t+8 meters, determine time to maximum height.","t=24/9.8≈2.44898 seconds.")
put(93,"MAX_TICKET_REVENUE_FROM_TWO_DEMAND_POINTS",
    "A 62000-seat venue sold 26000 tickets at $11 each and 31000 at $9 each. Assuming linear attendance-demand relation, find revenue-maximizing ticket price (do not exceed capacity).",
    "Demand q(p)=53500−2500p; revenue p(53500−2500p) maximized at p=$10.70, attendance26750≤62000.")
put(94,"MAX_ORCHARD_YIELD_FROM_TREE_DENSITY",
    "With 75 trees/acre each yields20 bushels. For each extra tree yield per tree drops3 bushels. Under the algebraic rule, determine integer tree count maximizing total bushels with nonnegative per-tree yield.",
    "Set n=75+x, per-tree yield20−3x; total Y=(75+x)(20−3x)=1500−205x−3x². Continuous optimum x=−205/6≈−34.17, so n≈40.83; best integer n=41, total 41×122=5002 bushels. However original wording only specifies added trees; extrapolating yield to fewer trees requires explicit assumption.")
assert sorted(items)==list(range(1,95)),(len(items),sorted(set(range(1,95))-set(items)))
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l02-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L02",
"OriginalNumberedQuadraticExercises":94,
"FirstPartyIndependentStudentQuestions":[{"Number":n,"SourceSkill":v["OriginalSourceAcademicSkill"],"Question":v["FirstPartyIndependentStudentPrompt"],"GradingMode":"MANUAL_TEACHER_REVIEW"}for n,v in sorted(items.items())],
"TeacherOnlyMathematicalExpectedResults":[{"Number":n,"Expected":v["TeacherExactMathematicsAndScoring"],"OriginalCognitiveActions":v["SourceCognitiveActions"]}for n,v in sorted(items.items())],
"SourcePublisherDiagramImageCopied":False,
"SourceOriginalVerbatimAndNumericInstanceGraphPixelParityClaim":False,
"LiveTeacherGradebookStudentRuntime":"NOT_RUN","StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u03-l02-original94-firstparty-quadratic-exercise-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceBookSHA256":source["SourceSha256"],
"SourceOriginalNumberedExerciseParents":94,
"FirstPartyIndependentQuadraticMathSkillStudentQuestions":94,
"ExactPublisherOriginalExerciseNumericalAndGraphicParityCertified":False,
"SourcePublisherGraphicsCopied":0,"StageD":"NOT_RUN",
"Items":[items[n]for n in sorted(items)]
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u03-l02-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Original94SourceExerciseSkillAssessment"]="94_QUADRATIC_SOURCE_SKILL_FIRSTPARTY_STUDENT_TASKS_WITH_TEACHER_ALGEBRA_RUBRICS_AUTHORED"
c["FirstPartyTeacherMathRubricPath"]=native.relative_to(root).as_posix()
c["OriginalNumberedExerciseSourceSkillContractPath"]=manifest.relative_to(root).as_posix()
c["OriginalFigureSkills"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L02_94_QUADRATIC_SOURCE_SKILL_FIRSTPARTY_MATH_TEACHER_ASSESSMENT_CONTRACTS_AUTHORING_COMPLETE")
