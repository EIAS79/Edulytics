from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,a,s):
 i=len(Q)+1;assert q and a and s
 Q.append({"SourceExercise":i,"IndependentPolarQuestion":q,"TeacherMathematicalAnswer":a,
 "SourceMathSkillReviewed":s,"PublisherTextCopied":False,"TeacherScoring":"MANUAL_POLAR_COORDINATES"})
for q,a,s in [
("Compare rectangular and polar representation of a point.","Cartesian uses signed(x,y); polar uses signed radius and directed angle(r,θ).","Source1 polar vs Cartesian"),
("Explain polar axis and pole.","Pole is origin, polar axis is reference ray θ=0 along positive x-axis.","Source2 polar axes"),
("Describe how to plot (r,θ).","Rotate θ from polar axis, move radius |r| along ray if r≥0 or opposite ray if r<0.","Source3 polar plotting"),
("Compare (3,π/2) and (−3,π/2).","Opposite points: (0,3) and(0,−3).","Source4 radius negative"),
("Why are (−3,π/2) and (3,−π/2) identical?","Both rectangular(0,−3), since (−r,θ)=(r,θ+π).","Source5 equivalent polar points")
]:add(q,a,s)
# 6-10 polar→Cartesian
polar=[(7,7*math.pi/6),(5,math.pi),(6,-math.pi/4),(-3,math.pi/6),(4,7*math.pi/4)]
for r,theta in polar:
 i=len(Q)+1
 x=r*math.cos(theta);y=r*math.sin(theta)
 add(f"Convert polar(r={r},θ={theta/math.pi:.5f}π) to Cartesian(x,y).",
 f"x=r cosθ≈{x:.6f},y=r sinθ≈{y:.6f}.",f"Source{i} polar to Cartesian with signed radius")
# 11–15 Cartesian→polar
for x,y in [(4,2),(-4,6),(3,-5),(-10,-13),(8,8)]:
 i=len(Q)+1
 r=math.hypot(x,y);theta=math.atan2(y,x)%(2*math.pi)
 add(f"Convert Cartesian({x},{y}) to polar with r>0,0≤θ<2π.",
 f"r=sqrt({x*x+y*y})≈{r:.6f};θ≈{theta:.6f}rad.",
 f"Source{i} quadrant-aware rectangular to polar")
assert len(Q)==15
# 16–27 rectangular equation to polar; keep undivided r for origin
conversions=[
("x=3","r cosθ=3"),("y=4","r sinθ=4"),
("y=4x²","r sinθ=4r²cos²θ"),("y=2x⁴","r sinθ=2r⁴cos⁴θ"),
("x²+y²=4y","r²=4r sinθ"),("x²+y²=3x","r²=3r cosθ"),
("x²−y²=x","r²(cos²θ−sin²θ)=r cosθ"),
("x²−y²=3y","r²cos2θ=3r sinθ"),
("x²+y²=9","r²=9"),("x²=9y","r²cos²θ=9r sinθ"),
("y²=9x","r²sin²θ=9r cosθ"),
("9xy=1","9r²sinθcosθ=1")]
for original,polar_form in conversions:
 i=len(Q)+1
 add(f"Convert Cartesian equation {original} to polar without losing the origin.",
 polar_form+".",f"Source{i} algebraic x=rcosθ,y=rsinθ,r²=x²+y² substitution")
# 28-39 polar equation to rectangular; identify conic
reverse=[
("r=3sinθ","x²+y²=3y; circle center(0,1.5),radius1.5."),
("r=4cosθ","x²+y²=4x; circle center(2,0),radius2."),
("r=4/(sinθ+7cosθ)","y+7x=4; line (source domain excludes denominator zero)."),
("r=6/(cosθ+3sinθ)","x+3y=6; line."),
("r=2secθ","x=2, straight vertical line."),
("r=3cscθ","y=3, straight horizontal line."),
("r²=r cosθ+2","x²+y²=x+2; circle center(.5,0),radius1.5."),
("r²=4secθcscθ","xy=4, rectangular hyperbola, original sinθcosθ nonzero."),
("r=4","x²+y²=16; circle radius4."),
("r²=4","x²+y²=4; circle radius2."),
("r=1/(4cosθ−3sinθ)","4x−3y=1; line."),
("r=3/(cosθ−5sinθ)","x−5y=3; line.")]
for pol,rect in reverse:
 i=len(Q)+1
 add(f"Convert polar {pol} to Cartesian; state geometric form.",
 rect,f"Source{i} polar equation conversion and conic classification")
assert len(Q)==39
# 40–44 visual point read
for r,theta in [(2,math.pi/3),(3,5*math.pi/4),(-2,math.pi/6),(1.5,3*math.pi/2),(4,-math.pi/4)]:
 i=len(Q)+1;x=r*math.cos(theta);y=r*math.sin(theta)
 add(f"Independent polar grid shows radius {r} and angle{theta/math.pi:.5f}π. Give rectangular location and one equivalent polar pair.",
 f"Cartesian≈({x:.5f},{y:.5f}); equivalent polar(−{r},θ+π).",
 f"Source{i} read polar grid point")
assert len(Q)==44
path=B/"u08-l03-independent-questions-001-044.json"
path.write_text(json.dumps({"SchemaVersion":1,"Section":"8.3","Count":44,
"IndependentPolarStudentMath":Q,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L03_SOURCE_SKILL_1_44_PREPARED")
