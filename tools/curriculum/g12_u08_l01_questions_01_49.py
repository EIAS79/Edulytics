from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(question,key,skill):
 n=len(Q)+1
 assert question and key and skill
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":question,
 "TeacherMathematicalSolution":key,"SourceMathematicsSkillReviewed":skill,
 "OriginalPublisherQuestionCopied":False,"ManualGeometryMarking":True})
S=lambda x:math.sin(math.radians(x))
C=lambda x:math.cos(math.radians(x))
def solve_ssa(A,a,b):
 z=b*S(A)/a
 if z>1+1e-9 or z<0:return []
 beta=math.degrees(math.asin(min(z,1)))
 ans=[]
 for B in [beta,180-beta]:
  G=180-A-B
  if G>1e-8 and not any(abs(B-prev[0])<1e-7 for prev in ans):
   c=a*S(G)/S(A)
   ans.append((B,G,c))
 return sorted(ans)
def fmt_ssa(ans):
 if not ans:return "No triangle; sine-rule candidate outside feasible angle range."
 return f"{len(ans)} triangle(s): "+ "; ".join(
  f"B={B:.2f}°, C={C0:.2f}°, c={c:.3f}" for B,C0,c in ans)
for q,a,skill in [
("Define the altitude of a triangle.","The perpendicular distance from vertex to the line containing opposite side.","Source1 altitude"),
("Compare right and oblique triangles.","Right triangle has90° angle; oblique triangle has no right angle, may be acute or obtuse.","Source2 right vs oblique"),
("When can Law of Sines determine an unknown angle?","When known opposite side-angle pair and another side yield sin(new angle)=side·sin(known angle)/known side.","Source3 known side-angle pairing"),
("State Law of Sines with correctly paired sides.","a/sinA=b/sinB=c/sinC; each angle opposite its paired side.","Source4 numerator and opposite side"),
("Which given-data case may produce zero, one, or two triangles?","SSA, the non-included known angle opposite a known side.","Source5 ambiguous case")
]:add(q,a,skill)
# 6-10 AAS and ASA
for A,C,known,val in [(45,65,"a",12),(35,75,"c",18),(60,60,"a",10),(25,95,"a",6),(40,30,"b",8)]:
 i=len(Q)+1;B0=180-A-C
 scal=(val/(S(A)if known=="a"else S(B0)if known=="b"else S(C)))
 a=scal*S(A);b=scal*S(B0);c=scal*S(C)
 add(f"Solve triangle A={A}°, C={C}°, and {known}={val}.",
 f"B={B0}°, a≈{a:.3f},b≈{b:.3f},c≈{c:.3f}.",
 f"Source{i} AAS or ASA law of sines")
assert len(Q)==10
# 11-13 missing side with two known angles and opposite side
for A,B0,known,val,target in [(38,52,"c",6,"b"),(135,22,"b",11,"a"),(36,22,"b",15,"c")]:
 i=len(Q)+1;C0=180-A-B0
 sides={x:val*S(y)/S({"a":A,"b":B0,"c":C0}[known]) for x,y in [("a",A),("b",B0),("c",C0)]}
 add(f"A={A}°,B={B0}°, {known}={val}. Find side{target} by Law of Sines.",
 f"{target}≈{sides[target]:.3f}; C={C0}°.",
 f"Source{i} oblique missing side")
# 14-26 SSA count and all solutions; A known, a opp, b another
ssa=[(119,14,26),(113,32,10),(80,5.3,3.5),(35,12,17),
(25,35,20.5),(43,7,9),(24,3,7),(10,5,13),
(28,2.3,1.8),(119,8.2,11.3),(22,5,24),
(20,6,13),(12,2,9)]
for A,a,b in ssa:
 i=len(Q)+1
 add(f"SSA: A={A}°, opposite a={a}, other side b={b}. Count and solve all feasible triangles.",
 fmt_ssa(solve_ssa(A,a,b)),f"Source{i} ambiguous SSA no/one/two triangles")
assert len(Q)==26
# 27-30 area with two sides included angle
for a,b,angle in [(5,8,35),(11,7,28),(30,24,75),(7,4.5,43)]:
 i=len(Q)+1;area=.5*a*b*S(angle)
 add(f"Find oblique triangle area with adjacent sides{a},{b} and included angle{angle}°.",
 f"Area=1/2·{a}·{b}·sin{angle}°≈{area:.3f} square units.",
 f"Source{i} SAS sine area formula")
# 31-36 diagram side from angle pair
for A,B0,known,val,target in [
(70,50,"a",10,"b"),(25,120,"c",6,"a"),
(75,45,"b",15,"c"),(110,40,"b",18,"a"),
(55,45,"c",11,"b"),(111,22,"a",8.6,"c")]:
 i=len(Q)+1;C0=180-A-B0
 ang={"a":A,"b":B0,"c":C0}
 found=val*S(ang[target])/S(ang[known])
 add(f"Independently drawn triangle A={A}°,B={B0}°, side {known}={val}. Find side{target}.",
 f"C={C0}°, {target}={val}sin({ang[target]}°)/sin({ang[known]}°)≈{found:.2f}.",
 f"Source{i} interpret triangle sketch and opposite sides")
assert len(Q)==36
# 37-43 graphical missing angle, including obtuse
for A,a,b in [(98,10,5),(37,8,11),(22,13,5),(59,5.7,5.3),(55,24,21),(65,12,10),(40,10,8)]:
 i=len(Q)+1;ans=solve_ssa(A,a,b)
 add(f"In an independent triangle diagram, given A={A}°,a={a},b={b}, find valid B angles and solve geometry.",
 fmt_ssa(ans),f"Source{i} infer missing angle from geometry, obtuse selection and Law of Sines")
# 44-49 graphical triangle areas
for a,b,angle in [(16,10,30),(18,15,25),(4.5,3.5,51),
 (9,11,58),(25,18,40),(30,14,50)]:
 i=len(Q)+1;area=.5*a*b*S(angle)
 add(f"Find triangle area from diagram where sides {a},{b} meet at angle{angle}°.",
 f"Area=1/2ab sinC≈{area:.2f}.",f"Source{i} graphical area from included angle")
assert len(Q)==49
path=B/"u08-l01-independent-questions-001-049.json"
path.write_text(json.dumps({"Section":"8.1","Count":49,"IndependentTeacherMathTasks":Q,
"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L01_SOURCE_SKILLS_1_49_PREPARED")
