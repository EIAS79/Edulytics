from pathlib import Path
import json,math
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,a,skill):
 n=len(Q)+1;Q.append({"SourceExercise":n,"IndependentQuestion":q,"TeacherMath":a,"SourceSkill":skill,
 "PublisherTextCopied":False})
for q,a,s in [
("State cosine-sine cofunction identity for complementary acute angles.","sinθ=cos(π/2−θ); cosθ=sin(π/2−θ)","Source1 cofunctions"),
("Compute cos225° in two different ways.","cos(180+45)=−sqrt2/2; cos(270−45)=−sqrt2/2","Source2 evaluate 5π/4 by sums/differences"),
("Use sum identities to establish parity of sin and cos.","sin(0−θ)=−sinθ and cos(0−θ)=cosθ","Source3 parity from sums")
]:add(q,a,s)
for fn,deg in [("cos",105),("cos",15),("sin",75),("sin",165),("tan",-15),("tan",285)]:
 val=(math.cos if fn=="cos" else math.sin if fn=="sin" else math.tan)(math.radians(deg))
 add(f"Evaluate {fn}({deg}°) exactly with 30°,45°,60° sum/difference identity.",
 f"Exact form via formula; decimal check {val:.8f}.",
 f"Source{len(Q)+1} exact sin/cos/tan using sums of special angles")
for fn,deg in [("sin",330),("sin",-135),("cos",-150),("cos",120)]:
 s=math.sin(math.radians(deg));c=math.cos(math.radians(deg))
 expr=(f"sinx·{c:.7f}+cosx·{s:.7f}"if fn=="sin" else f"cosx·{c:.7f}−sinx·{s:.7f}")
 add(f"Expand {fn}(x+({deg}°)) into sinx and cosx.",expr,
 f"Source{len(Q)+1} angle sum identity symbolic")
for fn,ans,skill in [
("csc(π/2−x)","secx","Source14 cofunction"),
("sec(π/2−x)","cscx","Source15 cofunction"),
("cot(π/2−x)","tanx","Source16 cofunction"),
("tan(π/2−x)","cotx","Source17 cofunction"),
("sin(3x)cos(5x)−sin(5x)cos(3x)","sin(−2x)=−sin2x","Source18 sine difference"),
("(tan(2x)−tan(x))/(1+tan(2x)tan(x))","tanx on common domain","Source19 tangent difference")
]:add("Simplify "+fn+".",ans,skill)
add("If sina=3/5 in QII and cosb=−5/13 in QII find sin(a+b).",
 "cos a=−4/5,sinb=12/13: 3/5·(−5/13)+(−4/5)·12/13=−63/65.",
 "Source20 signed angle sum from trig ratios")
add("If sina=4/5 and cosb=12/13, both QI, find cos(a+b).",
 "cosa=3/5,sinb=5/13; cos(a+b)=36/65−20/65=16/65.",
 "Source21 signed angle addition in QI")
for fn,q,ans in [
("sin","arccos(0)−arccos(1/2)","sqrt3/2"),
("cos","arccos(sqrt2/2)+arcsin(1/2)","cos(5π/12)=(sqrt6−sqrt2)/4"),
("tan","arcsin(1/2)−arccos(1/2)","−sqrt3/3")]:
 add(f"Find {fn}({q}) exactly.",ans,f"Source{len(Q)+1} inverse trig composed with angle difference")
for fn,x,ans in [
("cos","π/2−x","sinx"),
("sin","π−x","sinx"),
("tan","π/4+x","(1+tanx)/(1−tanx)"),
("sin","π/3+x","sqrt3/2 cosx+1/2 sinx"),
("tan","π/4−x","(1−tanx)/(1+tanx)"),
("cos","7π/6+x","−sqrt3/2 cosx+1/2 sinx"),
("sin","π/4+x","sqrt2/2(sinx+cosx)"),
("cos","5π/4+x","−sqrt2/2 cosx+sqrt2/2 sinx")]:
 add(f"Expand {fn}({x}) with angle identities.",ans,f"Source{len(Q)+1} graphical compare expanded expression")
for q,a in [
("Verify sin4x−sin3x cosx=sinx cos3x.","sin(3x+x)=sin3x cosx+cos3x sinx"),
("Verify cos4x+sinx sin3x=cosx cos3x.","cos(3x+x)=cos3x cosx−sin3x sinx"),
("Check sin3x cos6x and its negative are identical.","Not identical generally: x=π/12 gives opposite signs"),
("Verify sin4x=sin5x cosx−cos5x sinx.","sin(5x−x)=sin4x"),
("Verify sin2x=2sinx cosx.","Angle sum sin(x+x)"),
("Verify cos2x=cos²x−sin²x.","Angle sum cos(x+x)"),
("Check tan2x=tanx/(1+tan²x).","False; tan2x=2tanx/(1−tan²x)"),
("Check sin3x sinx =sin²2x cos²x−cos²2x sin²x.","RHS=sin(2x+x)sin(2x−x)=sin3x sinx, true"),
("Verify tan(−x)=(tanx−tan2x)/(1+tanx tan2x).","tan(x−2x)=tan(−x), common domain")]:
 add(q,a,f"Source{len(Q)+1} graphical equivalence or correction")
for fn,deg in [("sin",75),("sin",195),("cos",165),("cos",345),("tan",-15)]:
 v=(math.sin if fn=="sin" else math.cos if fn=="cos" else math.tan)(math.radians(deg))
 add(f"Find exact {fn}({deg}°) by sum/difference and verify calculator value.",f"Exact using special-angle sum; decimal≈{v:.4f}.",
 f"Source{len(Q)+1} exact angle value")
for q,a in [
("Prove tan(x+π/4)=(tanx+1)/(1−tanx).","Tangent sum formula with tanπ/4=1"),
("State tan(a+b)/tan(a−b) and derive it.","Apply tangent sum/difference: (ta+tb)(1+ta tb)/((ta−tb)(1−ta tb))"),
("Prove cos(a+b)/(cos a cos b)=1−tan a tan b.","Expand cos(a+b)=cos a cos b−sin a sin b"),
("Prove cos(x+y)cos(x−y)=cos²x−sin²y.","Product expansion or product-to-sum"),
("Derive [cos(x+h)−cosx]/h.","cosx(cosh−1)/h−sinx sinh/h"),
("Explain tangent sum identity and restricted denominators.","tan(u+v)=(tanu+tanv)/(1−tanu tanv), all terms defined"),
("Explain tangent difference identity and restricted denominators.","tan(u−v)=(tanu−tanv)/(1+tanu tanv), all terms defined"),
("Check tan(x+y)/(1+tanx tany)=(tanx+tany)/(1−tan²x tan²y).","Not an identity as written in general; require domain checks and correction using tangent sum"),
("If α+β+γ=π, show sin(α+β)=sinγ.","sin(π−γ)=sinγ"),
("If α+β+γ=π and tangents finite, derive tanα+tanβ+tanγ=tanαtanβtanγ.","tan(α+β)=−tanγ; rearrange tangent sum")]:
 add(q,a,f"Source{len(Q)+1} prove/disprove sum-difference identity")
assert len(Q)==56,len(Q)
path=b/"u07-l02-independent-questions.json"
path.write_text(json.dumps({"SchemaVersion":1,"Section":"7.2","Count":56,"IndependentTasks":Q,
"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L02_56_SOURCE_SKILL_MATH_TASKS_PREPARED_NOT_CLOSED")
