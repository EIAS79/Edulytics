from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=json.loads((B/"u08-l03-independent-questions-001-044.json").read_text(encoding="utf8"))["IndependentPolarStudentMath"]
assert len(Q)==44
def add(q,a,src):
 n=len(Q)+1
 Q.append({"SourceExercise":n,"IndependentPolarQuestion":q,"TeacherMathematicalAnswer":a,
 "SourceMathSkillReviewed":src,"PublisherTextCopied":False,"TeacherScoring":"MANUAL_POLAR_COORDINATES"})
for r,rad in [(-2,math.pi/3),(-1,-math.pi/2),(3.5,7*math.pi/4),(-4,math.pi/3),
(5,math.pi/2),(4,-5*math.pi/4),(3,5*math.pi/6),(-1.5,7*math.pi/6),
(-2,math.pi/4),(1,3*math.pi/2)]:
 i=len(Q)+1
 x=r*math.cos(rad);y=r*math.sin(rad)
 add(f"Plot polar point(r={r},θ={rad/math.pi:.4f}π) and give its Cartesian position.",
 f"Cartesian≈({x:.5f},{y:.5f}); negative r reverses direction.",
 f"Source{i} polar plotting with signed radius")
assert len(Q)==54
for original,polar in [
("5x−y=6","r(5cosθ−sinθ)=6"),
("2x+7y=−3","r(2cosθ+7sinθ)=−3"),
("x²+(y−1)²=1","r²−2r sinθ=0"),
("(x+2)²+(y+3)²=13","r²+4r cosθ+6r sinθ=0"),
("x=2","r cosθ=2"),
("x²+y²=5y","r²=5r sinθ"),
("x²+y²=3x","r²=3r cosθ")]:
 i=len(Q)+1
 add(f"Convert {original} from Cartesian to polar without deleting origin.",
 polar+".",f"Source{i} convert rectangular equation and graph polar")
assert len(Q)==61
for polar,cart in [
("r=6","x²+y²=36; circle radius6"),
("r=−4","x²+y²=16; circle radius4"),
("θ=−2π/3","line through origin y=√3 x (when signed r allowed); if r≥0, one ray"),
("θ=π/4","line y=x (if r any real), otherwise northeast ray"),
("r=secθ","x=1"),
("r=−10sinθ","x²+y²=−10y, center(0,−5), radius5"),
("r=3cosθ","x²+y²=3x, center(1.5,0), radius1.5")]:
 i=len(Q)+1
 add(f"Convert {polar} to rectangular curve and identify shape, with signed-radius convention.",
 cart+".",f"Source{i} polar-to-Cartesian graphical equivalence")
assert len(Q)==68
for kind,x,y in [("polar",2,-math.pi/5),("polar",-3,3*math.pi/7),
("rect",-7,8),("rect",3,-4),("rect",-2,0)]:
 i=len(Q)+1
 if kind=="polar":
  vx=x*math.cos(y);vy=x*math.sin(y)
  ans=f"(x,y)≈({vx:.3f},{vy:.3f})"
  question=f"Use calculator to convert polar(r={x},θ={y/math.pi:.6f}π) to Cartesian."
 else:
  rr=math.hypot(x,y);theta=math.atan2(y,x)%(2*math.pi)
  ans=f"r≈{rr:.5f}, θ≈{theta:.5f} radians ({math.degrees(theta):.3f}°)."
  question=f"Convert Cartesian({x},{y}) to polar by calculator."
 add(question,ans,f"Source{i} numerical coordinate conversion")
# 74-78 line descriptions and oblique line polar equation
for a,fn in [(2,"sec"),(-3,"sec"),(4,"csc"),(-5,"csc")]:
 i=len(Q)+1
 add(f"Describe the locus r={a}{fn}θ in Cartesian coordinates.",
 ("x="+str(a)if fn=="sec"else"y="+str(a))+"; entire straight line with allowable signed r and defined denominator.",
 f"Source{i} polar secant/cosecant straight-line locus")
add("Find a polar equation for oblique line 3x−2y=5.",
 "r=5/(3cosθ−2sinθ), wherever denominator nonzero.",
 "Source78 oblique non-axis polar line")
# 79–84 inequalities
for q,a in [
("Plot region 0≤r<4, 0≤θ<2π.","Open disk of radius4 centered at pole, including origin and excluding outer circle."),
("Describe all points with 0≤θ≤π/4 and r≥0.","Closed wedge bounded by positive x-axis and 45° ray, unbounded radial direction."),
("Plot θ=π/4, r≥2.","Ray along45° starting at radius2, inclusive, and extending outward."),
("Plot θ=π/4,r≥−3, allowing signed radius.","Forward 45° ray all r≥0 plus backward225° segment length3 (endpoint included)."),
("Plot 0≤θ≤π/3 with0≤r<2.","First-quadrant sector with radius<2; radial sides included, circle boundary excluded."),
("Plot −π/6<θ≤π/3 with −3<r<2.","Forward sector with r∈[0,2) and opposite angular sector with negative r∈(−3,0); angular boundary inclusion follows θ interval.")
]:add(q,a,f"Source{len(Q)+1} signed polar region and boundary")
assert len(Q)==84 and [x["SourceExercise"]for x in Q]==list(range(1,85))
assert math.isclose(math.hypot(3,4),5)
assert math.isclose(math.atan2(0,-2)%(2*math.pi),math.pi)
out=B/"u08-l03-independent-questions-all-084.json"
out.write_text(json.dumps({"SchemaVersion":1,"Section":"8.3","Count":84,
"IndependentStudentMathTeacherKeys":Q,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L03_ALL84_INDEPENDENT_POLAR_SKILL_MATH_KEYS_PREPARED")
