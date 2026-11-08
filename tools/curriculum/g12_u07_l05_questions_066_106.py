from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=B/"u07-l05-independent-questions-001-065.json";Q=json.loads(src.read_text(encoding="utf8"))["IndependentMathTasks"]
assert len(Q)==65
def add(q,a,src):
 n=len(Q)+1;Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,
 "TeacherExpectedMath":a,"SourceSkillReviewed":src,"SourceOriginalQuestionCopied":False,
 "TeacherManualScoring":True})
def roots(fn,v):
 if fn in ("sin","cos") and not -1<=v<=1:return []
 if fn=="sin":t=math.asin(v);basics=[t,math.pi-t];period=2*math.pi
 elif fn=="cos":t=math.acos(v);basics=[t,-t];period=2*math.pi
 else:t=math.atan(v);basics=[t];period=math.pi
 vals=set()
 for t in basics:
  for k in range(-3,5):
   x=t+k*period
   if -1e-9<=x<2*math.pi-1e-9:vals.add(round(max(0,x),10))
 return sorted(vals)
def answer(vals):
 vals=sorted(set(round(x,10)for x in vals))
 return ("No real roots on [0,2π)."if not vals else
 "x≈"+", ".join(f"{v:.4f}"for v in vals)+" rad in[0,2π).")
def quadratic(a,b,c):
 disc=b*b-4*a*c
 return [] if disc<0 else sorted(set([(-b+math.sqrt(disc))/(2*a),(-b-math.sqrt(disc))/(2*a)]))
for fn,a,b,c in [
("sin",6,-5,1),("cos",8,-2,-1),("tan",20,5,-3),("cos",2,-1,15),
("sin",20,-27,7),("tan",2,7,6),("tan",10,7,-10)]:
 i=len(Q)+1
 z=sorted(set(t for v in quadratic(a,b,c) for t in roots(fn,v)))
 add(f"Use algebra and graph to solve {a}({fn}x)²+({b}){fn}x+({c})=0 on[0,2π), 4 decimals.",
 answer(z),f"Source #{i} graphical verification quadratic trigonometric equation")
assert len(Q)==72
for fn,v in [("sin",.35),("sin",-.65),("tan",-.4),("cos",.8)]:
 i=len(Q)+1
 add(f"Use calculator to solve {fn}x={v} for all x in[0,2π), 4 decimals.",
 answer(roots(fn,v)),f"Source #{i} numeric inverse trig solution")
for fn,a,b,c in [
("tan",1,3,-3),("tan",6,13,6),("cos",2,-1,-1),("sin",3,0,-1),
("tan",2,9,-6),("sin",4,1,-3)]:
 i=len(Q)+1
 z=sorted(set(t for v in quadratic(a,b,c) for t in roots(fn,v)))
 add(f"Factor or use quadratic formula on {a}({fn}x)²+({b}){fn}x+({c})=0; list x in[0,2π).",
 answer(z),f"Source #{i} technology with algebraic quadratic and trig domains")
assert len(Q)==82
for q,a,src in [
("Solve csc²x−2cscx−3=0 on[0,2π).","cscx=3 or−1; hence sinx=1/3 or−1; include both solution families and sinx≠0.","Source83 reciprocal quadratic"),
("Solve sin²x−cos²x−1=0.","cos2x=−1 => x=π/2,3π/2.","Source84 Pythagorean"),
("Solve sin²x(1−sin²x)+cos²x(1−sin²x)=0.","1−sin²x=cos²x=0 => x=π/2,3π/2.","Source85 factor Pythagorean"),
("Can 2sec²x+3−tan²x+sin²x+cos²x=0 hold?","No: sec²x−tan²x=1, remaining sec²x+5>0.","Source86 identity no real solutions"),
("Solve sin²x−1+2cos2x−cos²x=1.","LHS=−cos²x+2cos2x−cos²x=2cos2x−2cos²x=−2sin²x, so no solutions to −2sin²x=1.","Source87 algebraic identity test"),
("Solve tan²x−1−sec³x cosx=0.","sec³x cosx=sec²x, so tan²−1−sec²=−2, no roots on original domain.","Source88 tangent/secant identity"),
("Solve sin2x/sec²x=0.","sin2x=0, cosx≠0 => x=0,π. (x=π/2,3π/2 excluded).","Source89 rational trig numerator and domain"),
("Solve sin2x/(2csc²x)=0.","sin2x=0, sinx≠0 => x=π/2,3π/2.","Source90 reciprocal domain"),
("Can 2cos²x−sin²x−cosx−5=0?","No: max 2−0−(−1)−5=−2, no roots.","Source91 detect no root by range"),
("Solve 1/sec²x+2+sin²x+4cos²x=4.","Original requires cosx≠0; LHS=cos²+2+1−cos²+4cos²=3+4cos² => cos²=1/4 => x=π/3,2π/3,4π/3,5π/3.","Source92 reciprocal and Pythagorean")
]:add(q,a,src)
assert len(Q)==92
app=[
("An airplane's route is150 miles with 30 miles north displacement. Find heading north of east.",f"asin(30/150)≈{math.degrees(math.asin(.2)):.2f}°.","Source93 heading north-of-east from displacement"),
("Ramp rises3ft over length12ft. Find angle to ground.",f"asin(3/12)≈{math.degrees(math.asin(.25)):.2f}°.","Source94 incline ramp"),
("Ramp rises2.5ft over length18ft. Find angle.",f"asin(2.5/18)≈{math.degrees(math.asin(2.5/18)):.2f}°.","Source95 alternate ramp"),
("Rocket altitude9mi observed6mi horizontally away. Find elevation angle.",f"atan(9/6)≈{math.degrees(math.atan(1.5)):.2f}°.","Source96 rocket elevation"),
("Astronaut altitude12mi, ground observer4mi horizontal. Find depression angle.",f"atan(12/4)≈{math.degrees(math.atan(3)):.2f}°.","Source97 depression"),
("Observer7m from building12m tall: top elevation angle.",f"atan(12/7)≈{math.degrees(math.atan(12/7)):.2f}°.","Source98 building elevation"),
("Observer8m from building5m tall, rooftop person's depression angle.",f"atan(5/8)≈{math.degrees(math.atan(5/8)):.2f}°.","Source99 building depression"),
("Building height30ft shadow60ft: sun elevation angle.",f"atan(30/60)≈{math.degrees(math.atan(.5)):.2f}°.","Source100 shadow angle"),
("Building height80ft shadow3ft: sun elevation angle.",f"atan(80/3)≈{math.degrees(math.atan(80/3)):.2f}°.","Source101 steep sun elevation"),
("Spotlight at ground6m from wall projects ray through person3m tall at horizontal distance2m. What elevation angle?",f"atan(3/2)≈{math.degrees(math.atan(1.5)):.2f}°; linear ray height at wall9m.","Source102 similar triangles projection"),
("Spotlight at ground4ft from person6ft tall with vertical wall8ft farther. Find elevation angle.",f"atan(6/4)≈{math.degrees(math.atan(1.5)):.2f}°; at wall18ft.","Source103 spotlight projection"),
("A 6ft person balances in handstand with hands1.2ft from wall and feet on wall. Find angle their body makes with wall.",f"asin(1.2/6)≈{math.degrees(math.asin(.2)):.1f}°.","Source104 handstand vertical angle"),
("A 5ft person handstand has hands2ft from wall and feet on wall. Find angle body makes with wall.",f"asin(2/5)≈{math.degrees(math.asin(.4)):.1f}°.","Source105 handstand alternate"),
("Ladder length20ft, foot6ft from house. Find angle with ground.",f"acos(6/20)≈{math.degrees(math.acos(.3)):.1f}°.","Source106 ladder slipping threshold")
]
assert len(app)==14
for q,a,src in app:add(q,a,src)
assert len(Q)==106 and [x["SourceExercise"]for x in Q]==list(range(1,107))
# Structural and trigonometric spot checks
for v in [.25,.5,.8,-.4]:
 for fn in ["sin","cos","tan"]:
  for t in roots(fn,v):
   y=(math.sin if fn=="sin"else math.cos if fn=="cos"else math.tan)(t)
   assert math.isclose(y,v,rel_tol=1e-8,abs_tol=1e-8)
path=B/"u07-l05-independent-questions-all-106.json"
path.write_text(json.dumps({"SchemaVersion":1,"Section":"7.5","SourceExercises":106,
"IndependentStudentTeacherMath":Q,"AutoGradingClaimed":False,"StageD":"NOT_RUN"},
ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L05_ALL_106_INDEPENDENT_STUDENT_AND_TEACHER_MATH_TASKS_PREPARED")
