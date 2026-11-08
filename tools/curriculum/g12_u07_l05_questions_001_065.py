from pathlib import Path
import math,json
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,a,src,mode="TEACHER_MANUAL_MATH"):
 i=len(Q)+1
 assert q and a and src
 Q.append({"SourceExercise":i,"IndependentStudentQuestion":q,"TeacherExpectedMath":a,
 "SourceSkillReviewed":src,"SourceOriginalQuestionCopied":False,"Marking":mode})
pi=math.pi
def roots(kind,v,k=1):
 if kind in ("sin","cos") and not -1-1e-10<=v<=1+1e-10:return []
 base=(math.asin(max(-1,min(1,v))) if kind=="sin" else math.acos(max(-1,min(1,v))) if kind=="cos" else math.atan(v))
 if kind=="sin":loc=[base,pi-base]
 elif kind=="cos":loc=[base,-base]
 else:loc=[base]
 step=pi if kind=="tan"else 2*pi
 vals=set()
 for phase in loc:
  for n in range(-3*k-2,3*k+3):
   t=(phase+n*step)/k
   if 0<=t<2*pi-1e-10:vals.add(round(t,9))
 return sorted(vals)
def fmt(vals):
 return "No solutions."if not vals else "x≈"+", ".join(f"{z:.4f}" for z in vals)+" radians on [0,2π)."
def solve(kind,k,v,src):
 i=len(Q)+1
 add(f"Find all x in [0,2π) satisfying {kind}({k}x)={v:.6f}; give values to4 decimals.",
 fmt(roots(kind,v,k)),f"Source #{i} {src}")
for q,a,s in [
("Must sinθ=1.7 have a real solution?","No; sine range[-1,1].","Source1 solvability"),
("Why might factoring a product of two trig functions be preferable to rewriting everything in sin?","Factoring exposes each solution family without introducing spurious divisions.","Source2 solve mixed trig"),
("Give criterion for sinθ=c or cosθ=c to have real solutions.","Exactly when -1≤c≤1.","Source3 real trig ranges")
]:add(q,a,s)
# 4–22 source linear and multiple-angle equation families
spec=[
("sin",1,-.5),("sin",1,math.sqrt(3)/2),("cos",1,.5),
("cos",1,-math.sqrt(2)/2),("tan",1,-1),("tan",1,1),
("tan",1,-1),("sin",1,math.sqrt(2)/2),("sin",1,-.5),
("cos",1,math.sqrt(2)/2),("cos",1,-.5),("sin",1,-.5),
("sin",1,-math.sqrt(3)/2),("sin",3,.5),("sin",2,math.sqrt(3)/2),
("cos",3,-math.sqrt(2)/2),("cos",2,-math.sqrt(3)/2),
("sin",1,.25),("cos",1,.9)]
assert len(spec)==19
for kind,k,val in spec:
 i=len(Q)+1;solve(kind,k,val,f"linear {kind} or multiple angle solution family")
assert len(Q)==22
# 23-32 factoring, quotient domain restrictions
for kind,u,v,src in [
("sin",0,.5,"Source23 factor sine against bracket"),
("sin",0,.25,"Source24 factor tan with sine, retain cos domain"),
("cos",-.5,1,"Source25 quadratic cosine"),
("tan",-2,1,"Source26 quadratic in tangent with secant condition"),
("cos",0,.5,"Source27 grouped sin/cos factors"),
("cos",-.5,.5,"Source28 squared cosine"),
("cos",-1,1,"Source29 squared secant"),
("tan",-2,-1,"Source30 tan parity polynomial"),
("sin",-.5,-.25,"Source31 quadratic sine"),
("tan",-.5,.5,"Source32 higher tangent powers factor")]:
 i=len(Q)+1
 ans=sorted(set(roots(kind,u)+roots(kind,v)))
 add(f"Solve ({kind}x−({u:.3f}))({kind}x−({v:.3f}))=0 on [0,2π); respect original trig domains.",
 fmt(ans),src)
assert len(Q)==32
# 33-40 sin/cos addition and double angle
for k,kind,v in [(3,"sin",.6),(5,"sin",-.2),(1,"cos",1),
 (2,"sin",0),(2,"cos",-.25),(2,"sin",.5),(2,"cos",0),(3,"cos",.5)]:
 i=len(Q)+1
 add(f"Use a trig identity to solve {kind}({k}x)={v:.5f} on [0,2π).",
 fmt(roots(kind,v,k)),f"Source{i} sum/difference, double angle, multiple angle identity")
# 41-49 quadratic trig formula and real-domain root filtering
quad=[
("tan",1,-1,0),("sin",1,1,-2),("sin",1,-2,-4),
("cos",5,3,-1),("cos",3,-2,-2),("sin",5,2,-1),
("tan",1,5,-1),("tan",1,1,0),("tan",1,1,2)]
def quadratic(a,b,c):
 delta=b*b-4*a*c
 if delta<0:return []
 return [(-b+math.sqrt(delta))/(2*a),(-b-math.sqrt(delta))/(2*a)]
for kind,a,b,c in quad:
 i=len(Q)+1
 z=quadratic(a,b,c)
 ans=sorted(set(v for u in z for v in roots(kind,u)))
 add(f"Solve {a}({kind}x)²+({b}){kind}x+({c})=0 on [0,2π), to4 decimals.",
 fmt(ans),f"Source{i} quadratic trig, discard outside[-1,1] for sin/cos")
assert len(Q)==49
# 50-65 identities and Pythagoras; independent equations of known forms
for kind,k,v,src in [
("cos",2,0,"Source50 sin²−cos² identity"),
("sin",1,0,"Source51 sin²+cos² zero/no roots"),
("sin",2,.5,"Source52 sin2x−sinx identity"),
("cos",2,.5,"Source53 cos2x−cosx identity"),
("tan",1,0,"Source54 reciprocal identity domain"),
("cos",2,0,"Source55 cosine double-angle"),
("cos",1,1/3,"Source56 sec²=9"),
("sin",1,3/5,"Source57 factoring cosine"),
("tan",1,-1/5,"Source58 products with tangent"),
("cos",1,-.25,"Source59 quadratic cosine relation"),
("sin",1,-.5,"Source60 sine quadratic"),
("cos",1,.5,"Source61 cosine quadratic"),
("sin",1,.25,"Source62 sine/cos mixed Pythagoras"),
("cos",1,.25,"Source63 cosine mixed identity"),
("tan",1,2,"Source64 tan=sin quotient"),
("cos",1,-1,"Source65 cos3x=cosx")]:
 i=len(Q)+1
 if i==51:
  add("Can sin²x+cos²x=0 have real solutions?","No, the expression equals1 for every real x.",src)
 else:solve(kind,k,v,src)
assert len(Q)==65
path=B/"u07-l05-independent-questions-001-065.json"
path.write_text(json.dumps({"SourceSection":"7.5","Count":65,"IndependentMathTasks":Q,
"CI":"NOT_RUN","StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L05_FIRST_65_SOURCE_SKILL_MATH_QUESTIONS_PREPARED")
