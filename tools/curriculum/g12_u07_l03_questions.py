from pathlib import Path
import json,math
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,ans,skill):
 Q.append({"SourceExercise":len(Q)+1,"IndependentStudentQuestion":q,
 "TeacherExpectedMath":ans,"OriginalSourceSkillReviewed":skill,
 "OriginalPublisherQuestionCopied":False})
for q,a,src in [
("Derive sin²θ and cos²θ power reduction.","sin²θ=(1−cos2θ)/2;cos²θ=(1+cos2θ)/2.","Source1 power reductions from double angle"),
("Derive tan2θ from sine/cosine doubles.","tan2θ=2tanθ/(1−tan²θ) on valid domain.","Source2 tangent double-angle"),
("Give two radical-free half-angle tangent identities.","tan(θ/2)=sinθ/(1+cosθ)=(1−cosθ)/sinθ when denominators nonzero.","Source3 half-angle tangent rationalization"),
("Explain half-angle denominator restriction.","If 1+cosθ=0, θ=π+2πk and θ/2 has undefined tangent; formulas require domains.","Source4 tangent half-angle domains")
]:add(q,a,src)
cases=[("sin",3/5,"I"),("cos",5/13,"I"),("cos",-4/5,"III"),("tan",-3/4,"IV")]
for kind,val,quad in cases:
 if kind=="sin":s=val;c=math.sqrt(1-s*s)*(-1 if quad in ("II","III") else 1)
 elif kind=="cos":c=val;s=math.sqrt(1-c*c)*(-1 if quad in ("III","IV") else 1)
 else:s=-3/5;c=4/5
 s2=2*s*c;c2=c*c-s*s;t2=s2/c2
 add(f"Given {kind}θ={val:g} in quadrant{quad}, find sin2θ, cos2θ, tan2θ.",
 f"sin2θ={s2:.8f};cos2θ={c2:.8f};tan2θ={t2:.8f}, provided cos2θ≠0.",
 f"Source{len(Q)+1} derive double values without solving θ")
for theta2,quad in [(3/5,"II"),(-1/2,"III")]:
 add(f"If cos2θ={theta2} and θ in Q{quad}, find six trig values of θ.",
 "cos²θ=(1+cos2θ)/2; sin²θ=(1−cos2θ)/2; use Q sign for each root then quotient/reciprocal.",
 f"Source{len(Q)+1} half-angle root with quadrant")
add("Simplify 2sin(π/6)cos(π/6).","sin(π/3)=sqrt3/2.","Source11 double sine")
add("Simplify 2cos(π/8)sin(π/8).","sin(π/4)=sqrt2/2.","Source12 double sine")
for q,a in [
("Find sin(π/8) exact.","sqrt(2−sqrt2)/2"),
("Find cos(π/8) exact.","sqrt(2+sqrt2)/2"),
("Find sin(5π/12) exact.","(sqrt6+sqrt2)/4"),
("Find cos(7π/8) exact.","−sqrt(2+sqrt2)/2"),
("Find tan(π/8) exact.","sqrt2−1"),
("Find tan(5π/12) exact.","2+sqrt3"),
("Find tan(3π/8) exact.","sqrt2+1")]:
 add(q,a,f"Source{len(Q)+1} exact half-angle identity")
for original,ref in [("sinθ=−3/5, QIV","θ/2 in QII"),("sinθ=−5/13,QIII","θ/2 in QII"),
("cosθ=−4/5,QII","θ/2 in QI"),("cosθ=1/3,QI","θ/2 in QI")]:
 add(f"Given {original} with 0≤θ≤2π, find sin(θ/2),cos(θ/2),tan(θ/2).",
 f"sin half=±sqrt((1−cosθ)/2); cos half=±sqrt((1+cosθ)/2), sign from {ref}; tan ratio.",
 f"Source{len(Q)+1} half angle via quadrant and Pythagoras")
for q,a in [
("Right triangle 3-4-5 with acuteθ opposite3, find sin2θ/cos2θ/tan2θ.","sin2θ=24/25,cos2θ=7/25,tan2θ=24/7"),
("Right triangle 3-4-5 with acuteα opposite4, find double-angle.","sin2α=24/25,cos2α=−7/25,tan2α=−24/7"),
("For θ acute with cosθ=4/5 find half-angle expressions.","sinθ/2=sqrt(1/10),cosθ/2=sqrt(9/10),tanθ/2=1/3"),
("For θ acute with cosθ=3/5 find half-angle expressions.","sinθ/2=1/sqrt5,cosθ/2=2/sqrt5,tanθ/2=1/2")]:
 add(q,a,f"Source{len(Q)+1} figure5 double/half angle reference triangle")
for q,a in [
("Reduce cos²(31°)−sin²(31°).","cos62°"),
("Reduce 2cos²(27°)−1.","cos54°"),
("Reduce 1−2sin²(19°).","cos38°"),
("Reduce cos²(6x)−sin²(6x).","cos12x"),
("Reduce 4sin(3x)cos(3x).","2sin6x"),
("Reduce 6sin(4x)cos(4x).","3sin8x")]:
 add(q,a,f"Source{len(Q)+1} double-angle simplification")
for q,a in [
("Prove (sinθ−cosθ)²=1−sin2θ.","Expand sin²+cos²−2sin cos"),
("Prove sin2x=−2sin(−x)cos(−x).","sin(-x)=−sinx, cos(-x)=cosx"),
("Prove cotx−tanx=2cot2x.","(c²−s²)/(sc)=2cos2x/sin2x on common domain"),
("Verify sin2θ/(1+cos2θ)·tan²θ=tan³θ.","sin2θ/(1+cos2θ)=tanθ on common domain")]:
 add(q,a,f"Source{len(Q)+1} verify trigonometric identity")
for q,a in [
("Reduce cos²(4x) to first power trig.","(1+cos8x)/2"),
("Reduce cos²(7x) to first power trig.","(1+cos14x)/2"),
("Reduce sin⁴(3x).","(3−4cos6x+cos12x)/8"),
("Reduce sin⁴(5x).","(3−4cos10x+cos20x)/8"),
("Reduce cos²x sin⁴x.","Use sin²x=(1−cos2x)/2, cos²x=(1+cos2x)/2 and expand"),
("Reduce cos⁴x sin²x.","Use cos²x=(1+cos2x)/2 and sin²x=(1−cos2x)/2"),
("Reduce tan²x sin²x.","(sin⁴x)/(cos²x), defined cosx≠0")]:
 add(q,a,f"Source{len(Q)+1} power reduction")
for q,a in [
("Rewrite tan⁴x using first powers of cos2x.","[(1−cos2x)/(1+cos2x)]²; further power reduction possible"),
("Reduce sin²(3x).","(1−cos6x)/2"),
("Reduce sin²x cos²x.","(1−cos4x)/8"),
("Rewrite tan²x sinx.","sin³x/cos²x (retain cosx≠0)"),
("Rewrite tan⁴x cos²x.","sin⁴x/cos²x (retain cosx≠0)"),
("Reduce cos²x sin2x.","(1+cos2x)sin2x/2"),
("Reduce cos²(3x) sinx.","(1+cos6x)sinx/2"),
("Rewrite tan²(x/2)sinx.","[(1−cosx)/(1+cosx)]sinx, original defined where cos(x/2)≠0")]:
 add(q,a,f"Source{len(Q)+1} high-power reduction and graph reasoning")
add("Express sin4x using only sinx,cosx.","4sinx cosx(cos²x−sin²x).","Source53 sin4x expansion")
add("Express cos4x in sinx,cosx.","cos⁴x−6sin²xcos²x+sin⁴x.","Source54 cos4x expansion")
for q,a in [
("Verify sin2x=2tanx/(1+tan²x).","Replace tan=sin/cos, simplify to2sin cos, cosx≠0"),
("Verify cos2x=(1−tan²x)/(1+tan²x).","Ratio of cos²−sin² and cos²+sin²"),
("Verify tan2x=2sinxcosx/(2cos²x−1).","sin2x/cos2x"),
("Verify (sin²x−1)²=cos2x+sin⁴x.","LHS cos⁴x; RHS (1−2sin²x)+sin⁴x=(1−sin²x)²"),
("Prove sin3x=3sinxcos²x−sin³x.","Use sin(2x+x) and sum formula"),
("Prove cos3x=cos³x−3sin²xcosx.","Use cos(2x+x)"),
("Simplify (1+cos2t)/(sin2t−cost).","2cos²t/[cos t(2sin t−1)]=2cost/(2sin t−1), with original domain"),
("Prove sin16x=16sinxcosxcos2xcos4xcos8x.","Apply sin2u=2sinu cosu four times"),
("Prove cos16x=cos²8x−sin²8x.","Standard double-angle; factor further if needed")]:
 add(q,a,f"Source{len(Q)+1} extended identity")
assert len(Q)==63,len(Q)
path=b/"u07-l03-independent-source-skills.json"
path.write_text(json.dumps({"Section":"7.3","Count":63,"IndependentTeacherMath":Q,
"CI":"NOT_RUN","StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L03_63_INDEPENDENT_MATH_QUESTIONS_PREPARED_NOT_CLOSED")
