from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=json.loads((B/"u08-l02-independent-questions-001-051.json").read_text(encoding="utf8"))["IndependentMathProblemsTeacherKeys"]
assert len(Q)==51
def rad(x):return math.radians(x)
def S(x):return math.sin(rad(x))
def C(x):return math.cos(rad(x))
def cside(a,b,theta):return math.sqrt(a*a+b*b-2*a*b*C(theta))
def heron(a,b,c):
 if min(a,b,c)<=0 or a+b<=c or a+c<=b or b+c<=a:return None
 s=(a+b+c)/2
 return math.sqrt(s*(s-a)*(s-b)*(s-c))
def add(q,a,src):
 i=len(Q)+1
 Q.append({"SourceExercise":i,"FirstPartyIndependentQuestion":q,"TeacherMathAnswer":a,
 "OriginalSourceMathematicsSkill":src,"PublisherOriginalTextCopied":False,
 "ScoringMode":"TEACHER_MANUAL_OBLIQUE_GEOMETRY"})
for a,b,known,which in [(16,10,12,"long"),(11,17,22,"short")]:
 i=len(Q)+1;d=math.sqrt(2*(a*a+b*b)-known*known)
 add(f"A parallelogram sides{a},{b} with one diagonal{known}. Find other diagonal using parallelogram law.",
 f"d²+{known}²=2({a}²+{b}²), other≈{d:.4f}.",
 f"Source{i} parallelogram cosine law")
a=28;b=40;theta=100
small=math.sqrt(a*a+b*b+2*a*b*C(theta))
add(f"Parallelogram sides{a}cm,{b}cm included angle{theta}°; find shorter diagonal.",
 f"Shorter d≈{small:.4f}cm (cos{theta}°<0).",
 "Source54 parallelogram included obtuse angle")
for sides,radius in [(8,8),(5,12)]:
 i=len(Q)+1
 peri=2*sides*radius*math.sin(math.pi/sides)
 add(f"Regular {sides}-gon inscribed in circle radius{radius}. Determine perimeter by chord lengths.",
 f"P=2nRsin(π/n)≈{peri:.4f}.",
 f"Source{i} regular polygon circumradius/perimeter")
a,b,theta=5,6,52
for i in (57,58):
 add(f"An oblique triangle has a={a},b={b}, included angle{theta}°. Sketch and calculate c.",
 f"c=sqrt({a*a}+{b*b}−2·{a}·{b}cos{theta}°)≈{cside(a,b,theta):.4f}.",
 f"Source{i} derive and calculate third side from cosine relationship")
for a,b,theta in [(5.3,3.4,22),(8,6,80),(12.8,18.8,18)]:
 i=len(Q)+1
 add(f"Find triangular area, adjacent sides{a},{b}, included{theta}°.",
 f"A=ab sinθ/2≈{a*b*S(theta)/2:.4f}.",
 f"Source{i} SAS oblique triangle area")
for a,b,theta in [(800,900,70),(370,350,2.1)]:
 i=len(Q)+1
 add(f"Two sightline lengths {a},{b} meet at angle{theta}°. Find straight distance between endpoints.",
 f"d=sqrt(a²+b²−2ab cosθ)≈{cside(a,b,theta):.4f}.",
 f"Source{i} surveyor/satellite distance")
# multi-leg bearing vector
def vector(legs):
 e=sum(l*S(heading)for l,heading in legs)
 n=sum(l*C(heading)for l,heading in legs)
 return e,n,math.hypot(e,n),math.degrees(math.atan2(e,n))%360
legs=[(200,40),(180,170)]
e,n,d,heading=vector(legs)
add(f"Flight legs distance,bearing(clockwise from north){legs}; find net distance and heading.",
 f"east={e:.3f},north={n:.3f},distance={d:.3f},bearing={heading:.3f}°.",
 "Source64 aircraft course correction resultant")
tower=113;hill=98;included=56
add(f"A tower{tower}ft vertical on hill and anchor{hill}ft uphill at34° to horizontal. Find cable from top to anchor.",
 f"Cable≈sqrt(113²+98²−2(113)(98)cos56°)={cside(tower,hill,included):.3f}ft.",
 "Source65 tower support wire cosine-law geometry")
for v1,h1,b1,v2,h2,b2 in [(18,10,320,22,10,194),(18,2,327,4,2,60)]:
 i=len(Q)+1
 a=v1*h1;b=v2*h2
 sep=abs(b1-b2)%360;sep=min(sep,360-sep)
 dist=cside(a,b,sep)
 add(f"Ships depart same dock speed{v1}mph bearing{b1}° for{h1}h and speed{v2}mph bearing{b2}° for{h2}h. Find separation.",
 f"Distances{a},{b}mi, angular separation{sep}°, separation≈{dist:.3f}mi.",
 f"Source{i} two ship bearings cosine law")
a,b,theta=40,65,50
add(f"Triangular pool sides{a}ft,{b}ft enclosing{theta}°. Find third side.",
 f"Third side≈{cside(a,b,theta):.4f}ft.",
 "Source68 triangular pool length")
a=1.5*680;b=2*680;theta=10
result=math.sqrt(a*a+b*b+2*a*b*C(theta))
add("Pilot travels1.5h at680mph, turns10° then travels2h same speed. Find net displacement.",
 f"Add successive vectors: d=sqrt({a:g}²+{b:g}²+2{a:g}{b:g}cos10°)≈{result:.4f}mi.",
 "Source69 two successive flight legs")
def angles(a,b,c):
 def one(side,x,y):
  return math.degrees(math.acos(min(1,max(-1,(x*x+y*y-side*side)/(2*x*y)))))
 return one(a,b,c),one(b,a,c),one(c,a,b)
for sides in [(1744,714,2451),(140,442,315)]:
 i=len(Q)+1
 a,b,c=sides;A,Bang,Cang=angles(a,b,c)
 add(f"Three cities form triangle whose sides measure{a},{b},{c} miles. Determine all angles.",
 f"Opposite side angles≈{A:.3f}°, {Bang:.3f}°, {Cang:.3f}°.",
 f"Source{i} city triangle SSS")
for a,b,bear1,bear2 in [(500*2,600*2,20,150),(300*1.5,420*1.5,270,295)]:
 i=len(Q)+1
 theta=abs(bear1-bear2);theta=min(theta,360-theta)
 dist=cside(a,b,theta)
 add(f"Two aircraft leave same point and travel{a}mi at bearing{bear1}°, {b}mi at bearing{bear2}°. Find separation.",
 f"Angle between directions{theta}°, separation≈{dist:.4f}mi.",
 f"Source{i} divergent aircraft")
a=15.4;b=9.8;area=72.9
sinv=area/(a*b)
cosmag=math.sqrt(1-sinv*sinv)
long=math.sqrt(a*a+b*b+2*a*b*cosmag)
add(f"Parallelogram sides{a},{b}, area{area}; find longer diagonal.",
 f"sinθ={sinv:.5f}, |cosθ|={cosmag:.5f}, longer diagonal≈{long:.4f}.",
 "Source74 parallelogram area and diagonal")
for a,b,c,d,theta in [(4.5,7.9,9.4,12.9,117),(5.7,7.2,9.4,12.8,106)]:
 i=len(Q)+1
 diag=cside(a,b,theta)
 area1=.5*a*b*S(theta)
 area2=heron(c,d,diag)
 add(f"Quadrilateral consecutive sides{a},{b},{c},{d} and included angle{theta}° between first two. Split at opposite diagonal, find area.",
 f"Diagonal≈{diag:.4f}, triangle area1≈{area1:.4f},area2≈{area2:.4f}, total≈{area1+area2:.4f}.",
 f"Source{i} quadrilateral decomposition into SAS and Heron")
for a,b,theta in [(30,42,132),(110,250,85)]:
 i=len(Q)+1
 add(f"Land parcel triangular sides{a},{b} with included{theta}°. Find area.",
 f"A=ab sinθ/2≈{a*b*S(theta)/2:.4f} square units.",
 f"Source{i} land area")
assert len(Q)==78 and [q["SourceExercise"]for q in Q]==list(range(1,79))
# Numerical law of cosines and parallelogram checks
assert math.isclose(cside(3,4,90),5)
assert math.isclose(heron(3,4,5),6)
assert math.isclose(sum(angles(5,6,7)),180)
out=B/"u08-l02-independent-questions-all-078.json"
out.write_text(json.dumps({"SchemaVersion":1,"Section":"8.2","Count":78,"IndependentMathProblemsTeacherKeys":Q,
"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L02_ALL_78_SOURCE_MATH_SKILLS_PREPARED")
