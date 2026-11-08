from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,a,s):
 n=len(Q)+1;assert q and a and s
 Q.append({"SourceExercise":n,"FirstPartyIndependentQuestion":q,"TeacherMathAnswer":a,
 "OriginalSourceMathematicsSkill":s,"PublisherOriginalTextCopied":False,
 "ScoringMode":"TEACHER_MANUAL_OBLIQUE_GEOMETRY"})
D=lambda x:math.radians(x)
def cside(a,b,C):return math.sqrt(max(0,a*a+b*b-2*a*b*math.cos(D(C))))
def angle(a,b,c): # angle opposite a
 v=(b*b+c*c-a*a)/(2*b*c)
 return math.degrees(math.acos(min(1,max(-1,v))))
def heron(a,b,c):
 if min(a,b,c)<=0 or a+b<=c or a+c<=b or b+c<=a:return None
 s=(a+b+c)/2;return math.sqrt(s*(s-a)*(s-b)*(s-c))
def all_angles(a,b,c):
 return angle(a,b,c),angle(b,a,c),angle(c,a,b)
def ssa(A,a,b):
 discr=a*a-b*b*math.sin(D(A))**2
 if discr <0:return []
 res=[]
 for sign in (-1,1):
  c=b*math.cos(D(A))+sign*math.sqrt(max(0,discr))
  if c>1e-8 and c+a>b and c+b>a and a+b>c and not any(abs(c-k)<1e-6 for k in res):
   res.append(c)
 return sorted(res)
for q,a,sk in [
("Which measurements allow Law of Cosines to find the missing side?","SAS: two side lengths and their included angle; a²=b²+c²−2bc cosA.","Source1 SAS missing side"),
("Which measurements allow Law of Cosines to find a missing angle?","SSS: all three side lengths, with triangle inequality.","Source2 SSS missing angle"),
("Define semiperimeter s in Heron's area formula.","s=(a+b+c)/2; area=sqrt(s(s−a)(s−b)(s−c)).","Source3 Heron's semiperimeter"),
("How does Law of Cosines reduce to Pythagorean theorem?","At included angle90° cos90°=0 so c²=a²+b².","Source4 cosine and Pythagoras"),
("When does a non-right triangle require cosine law rather than right-angle Pythagoras?","For non-90° included angle in SAS/SSS oblique triangles.","Source5 nonright geometry")
]:add(q,a,sk)
# 6-9 SAS sides
for a,b,C in [(2.49,3.13,41.2),(6,7,120),(10.6,15.7,58.7),(18,23,115)]:
 i=len(Q)+1;c=cside(a,b,C)
 add(f"Find side c of triangle with adjacent sides a={a},b={b} and included C={C}°.",
 f"c=sqrt(a²+b²−2ab cosC)≈{c:.4f}.",
 f"Source{i} oblique triangle missing side")
# 10-15 SSA compute both possible valid third-side lengths
for A,a,b in [(119,26,14),(113,32,10),(67,38,49),
(43.1,184.2,242.8),(36.6,186.2,242.2),(50,45,105)]:
 i=len(Q)+1
 cvals=ssa(A,a,b)
 ans=("No feasible triangle."if not cvals else "; ".join(f"c≈{v:.3f}"for v in cvals)+f" ({len(cvals)} triangle(s)); derived via quadratic cosine law.")
 add(f"Given A={A}°, opposite a={a}, second side b={b}, solve all positive c using cosine law.",
 ans,f"Source{i} Law of Cosines SSA solving and feasibility")
assert len(Q)==15
# 16-20 angles SSS
for a,b,c,which in [(42,19,30,"A"),(14,13,20,"C"),(16,31,20,"B"),
(13,22,28,"A"),(108,132,160,"C")]:
 i=len(Q)+1
 angles={"A":angle(a,b,c),"B":angle(b,a,c),"C":angle(c,a,b)}
 add(f"Triangle a={a}, b={b}, c={c}. Find angle{which} by cosine law.",
 f"{which}=arccos((other sides squared−{ {'A':a,'B':b,'C':c}[which]}²)/(2·other sides))≈{angles[which]:.3f}°.",
 f"Source{i} SSS angle")
# 21-23 SAS complete
for A,b,c in [(35,8,11),(88,4.4,5.2),(121,21,37)]:
 i=len(Q)+1;a=cside(b,c,A);A0,Bangle,Cangle=all_angles(a,b,c)
 add(f"Solve full triangle A={A}°, b={b},c={c}.",
 f"a≈{a:.4f},B≈{Bangle:.3f}°,C≈{Cangle:.3f}°; A+B+C=180.",
 f"Source{i} complete SAS solution")
# 24-26 SSS complete
for a,b,c in [(13,11,15),(3.1,3.5,5),(51,25,29)]:
 i=len(Q)+1;angs=all_angles(a,b,c)
 add(f"Solve SSS triangle with a={a},b={b},c={c} angles.",
 f"A≈{angs[0]:.3f}°,B≈{angs[1]:.3f}°,C≈{angs[2]:.3f}°.",
 f"Source{i} SSS full triangle")
# 27-31 Heron
for a,b,c in [(18,21,32),(20,26,37),(.5,1/3,.25),(12.4,13.7,20.2),(1.6,2.6,4.1)]:
 i=len(Q)+1;area=heron(a,b,c)
 add(f"Find area by Heron's formula: side lengths{a},{b},{c}.",
 f"s={(a+b+c)/2:.6f}, area≈{area:.5f} square units.",
 f"Source{i} Heron area")
assert len(Q)==31
# 32-37 diagram missing opposite side
for a,b,ang in [(6.5,5,72),(4.5,3.4,42),(12,15,40),(30,23,65),(225,305,50),(15,13,123)]:
 i=len(Q)+1
 add(f"Independent oblique diagram sides{a},{b}, included{ang}°. Find third side.",
 f"c≈{cside(a,b,ang):.4f}.",f"Source{i} figure SAS unknown side")
# 38-42 diagram SSS angle
for a,b,c in [(1.5,2.5,2.3),(115,125,100),(4.3,6.8,8.2),
(38.7,40.6,23.3),(12,10,7)]:
 i=len(Q)+1
 add(f"Given independently labeled SSS triangle with a={a},b={b},c={c}, find angleA.",
 f"A≈{angle(a,b,c):.4f}°.",f"Source{i} diagram inferred unknown angle")
# 43-46 unknown side from SAS
for a,b,ang in [(28,20,60),(16,10,30),(13,20,22),(9,5,88)]:
 i=len(Q)+1
 add(f"Find third side of oblique triangle with sides{a},{b}, included{ang}°.",
 f"c≈{cside(a,b,ang):.4f}.",f"Source{i} diagram and cosine law")
# 47-51 Heron area from diagrams
for a,b,c in [(12,8,17),(50,36,22),(1.9,2.6,4.3),(12.5,8.9,16.2),(6,7,8)]:
 i=len(Q)+1;area=heron(a,b,c)
 add(f"Find area of graphical triangle side lengths{a},{b},{c} by Heron.",
 "No valid triangle."if area is None else f"Area≈{area:.4f} square units.",
 f"Source{i} diagram Heron's formula")
assert len(Q)==51
path=B/"u08-l02-independent-questions-001-051.json"
path.write_text(json.dumps({"SchemaVersion":1,"Section":"8.2","Count":51,
"IndependentMathProblemsTeacherKeys":Q,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L02_FIRST51_MATH_SKILLS_PREPARED")
