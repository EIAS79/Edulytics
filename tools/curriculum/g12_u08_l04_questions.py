from pathlib import Path
import json,math
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,a,s):
 i=len(Q)+1;Q.append({"SourceExercise":i,"FirstPartyStudentPolarGraphQuestion":q,
 "TeacherMathematicalAnswer":a,"OriginalSourceGraphSkill":s,
 "PublisherFigureCopied":False,"TeacherManualGraphMarking":True})
for q,a,sk in [
("Explain three common polar graph symmetries.","Reflect across polar axis (x-axis), lineθ=π/2 (y-axis), and pole (origin).","Source1 polar symmetries"),
("Relate polar-axis and θ=π/2 symmetries to Cartesian axes.","Polar axis corresponds x-axis; lineπ/2 corresponds y-axis; pole corresponds origin.","Source2 symmetry axes"),
("Outline a systematic method to graph r=f(θ).","Test symmetry, zeros, extrema, sign of r; sample θ and plot signed radius; connect continuously where valid.","Source3 polar graph procedure"),
("Distinguish cardioid, limaçon and lemniscate.","Cardioid has cusp a=b, limaçon a±b sin/cos may have loops, lemniscate is figure-eight r²=a²sin2θ/cos2θ.","Source4 polar curve families"),
("How does the ratio a/b classify r=a+b cosθ?","|a|=|b| cardioid, |a|<|b| inner-loop limaçon, |a|>|b| no inner loop (dimple/convex depends ratio).","Source5 coefficients determine shape")
]:add(q,a,sk)
# 6–15 symmetry cases. Use sufficient substitution tests; not overstate necessary cases.
eqs=[
("r=5cos3θ","r(−θ)=r(θ), so polar-axis reflection symmetry."),
("r=3−3cosθ","r(−θ)=r(θ), polar-axis symmetry."),
("r=3+2sinθ","r(π−θ)=r(θ), vertical θ=π/2 symmetry."),
("r=3sin2θ","r(θ+π)=r(θ), and appropriate angular reflections yield four-petal symmetry."),
("r=4","Circular radius4, symmetry in all axes and about pole."),
("r=2θ","Archimedean spiral on θ≥0, no global reflection symmetry in fixed domain."),
("r=4cos(θ/2)","Even in θ on symmetric domain; test negative radius representation for complete curve."),
("r=2/θ","Defined θ≠0; compare r(−θ)=−r(θ), domain-specific pole rotation relation."),
("r=3sqrt(1−cos²θ)","r=3|sinθ|, symmetric about both coordinate axes."),
("r²=5sin2θ","Lemniscate along diagonal; test θ→θ+π and sign/domain constraints.")
]
for eq,ans in eqs:
 i=len(Q)+1;add(f"Test symmetry and valid θ-domain for polar equation {eq}.",
 ans,f"Source{i} polar symmetry substitution")
assert len(Q)==15
# 16–43 recognize and reason shapes and extrema
plots=[
("r=3cosθ","circle center(1.5,0), radius1.5"),
("r=4sinθ","circle center(0,2), radius2"),
("r=2+2cosθ","right-facing cardioid cusp atθ=π"),
("r=2−2cosθ","left-facing cardioid cusp atθ=0"),
("r=5−5sinθ","down-facing cardioid"),
("r=3+3sinθ","up-facing cardioid"),
("r=3+2sinθ","limaçon without inner loop, 1≤r≤5"),
("r=7+4sinθ","limaçon without inner loop,3≤r≤11"),
("r=4+3cosθ","limaçon without inner loop,1≤r≤7"),
("r=5+4cosθ","limaçon without inner loop,1≤r≤9"),
("r=10+9cosθ","limaçon without inner loop,1≤r≤19"),
("r=1+3sinθ","inner-loop limaçon, r can be negative"),
("r=2+5sinθ","inner-loop limaçon, signed radius range[-3,7]"),
("r=5+7sinθ","inner-loop limaçon, signed radius range[-2,12]"),
("r=2+4cosθ","inner-loop limaçon, radius range[-2,6]"),
("r=5+6cosθ","inner-loop limaçon, signed radius range[-1,11]"),
("r²=36cos2θ","horizontal lemniscate; |r|≤6 where cos2θ≥0"),
("r²=10cos2θ","horizontal lemniscate; |r|≤sqrt10"),
("r²=4sin2θ","diagonal lemniscate; |r|≤2"),
("r²=10sin2θ","diagonal lemniscate; |r|≤sqrt10"),
("r=3sin2θ","four-petal rose, petal length3"),
("r=3cos2θ","four-petal rose, petal length3"),
("r=5sin3θ","three-petal rose, petal length5"),
("r=4sin4θ","eight-petal rose, petal length4"),
("r=4sin5θ","five-petal rose, petal length4"),
("r=−θ","Archimedean spiral with negative signed radius"),
("r=2θ","Archimedean spiral with increasing positive radius"),
("r=−3θ","Archimedean spiral with negative radius scaling3")
]
assert len(plots)==28
for eq,desc in plots:
 i=len(Q)+1;add(f"Graph the independently authored polar curve {eq} and identify type, characteristic axes/loops.",
 desc+".",f"Source{i} graph family circle/cardioid/limaçon/lemniscate/rose/spiral")
assert len(Q)==43
# 44–53 calculator and unusual polar curves
for eq,desc in [
("r=1/θ, θ>0","hyperbolic spiral; r→∞ as θ→0+, r→0 as θ→∞"),
("r=1/sqrtθ, θ>0","Lituus-like spiral, radius decays as inverse sqrtθ"),
("r=2sinθtanθ","cissoid-like polar curve, poles where cosθ=0 unless removable"),
("r=2sqrt(1−sin²θ)","r=2|cosθ|; investigate polar reflected circle-like loci"),
("r=5+cos4θ","fourfold ripple, radius range[4,6]"),
("r=2−sin2θ","bounded radial ripple, r∈[1,3]"),
("r=θ²,θ≥0","rapidly expanding power spiral"),
("r=θ+1,θ≥0","linearly growing Archimedean-type spiral"),
("r=θsinθ","oscillatory signed-radius spiral with zeros kπ"),
("r=θcosθ","oscillatory signed-radius spiral zeros π/2+kπ")]:
 i=len(Q)+1;add(f"Plot {eq} on a suitable domain; describe zeros, poles and radial envelope.",
 desc+".",f"Source{i} technology graph and qualitative features")
assert len(Q)==53
# 54–63 compare two or more polar curve models
for models,desc in [
(["r=θ","r=−θ"],"Opposite signed radii shift plotted direction byπ; compare same θ domain."),
(["r=θ","r=θ+sinθ"],"Second spiral has bounded radial modulation ±1."),
(["r=sinθ+θ","r=sinθ−θ"],"Opposite radial growth direction plus sine modulation."),
(["r=2sin(θ/2)","r=θsin(θ/2)"],"Same sine angular zeros; second grows radial envelope with θ."),
(["r=sin(cos3θ)","r=sin3θ"],"Different nested angular oscillation symmetries and zero positions."),
(["r=sin(3θ/2) over θ∈[0,4π] vs [0,8π]"],"Increasing domain reveals repeated tracing and/or new lobes; determine parameter tracing period."),
(["r=sinθ+(sin(5θ/2))³"],"Mixed angular frequencies generate multi-lobed radial perturbations."),
(["r=3sin3θ","r=2sin3θ","r=sin3θ"],"Same3-petal orientations, petal sizes scale3,2,1."),
(["r=3+3cosθ","r=2+2cosθ","r=1+cosθ"],"Same cardioid shape and orientation, scale3:2:1."),
(["r=3θ","r=2θ","r=θ"],"Same spiral family with radial scale3,2,1.")
]:
 i=len(Q)+1;add(f"Compare first-party polar plots: {', '.join(models)}.",
 desc,f"Source{i} paired polar graph differences")
assert len(Q)==63
# 64–72 curve intersections; include pole where appropriate
for q,ans in [
("Find intersections r=3+2sinθ and r=2 for0≤θ<2π.","3+2sinθ=2 -> sinθ=−1/2, θ=7π/6,11π/6, r=2."),
("Find intersections r=6−4cosθ and r=4.","cosθ=1/2 -> θ=π/3,5π/3, r=4."),
("Find intersections r=1+sinθ and r=3sinθ.","sinθ=1/2 -> θ=π/6,5π/6,r=3/2."),
("Find intersections r=1+cosθ and r=3cosθ.","cosθ=1/2 -> θ=π/3,5π/3,r=3/2."),
("Find same-parameter intersections r=cos2θ and r=sin2θ.","tan2θ=1 -> θ=π/8+kπ/2, k=0,1,2,3; pole is also shared geometrically."),
("Find intersections r=sin²2θ and r=1−cos4θ.","1−cos4θ=2sin²2θ: equality only when sin2θ=0, giving shared pole."),
("Find intersections r=sqrt3 and r=2sinθ.","θ=π/3,2π/3,r=sqrt3."),
("Find intersections r²=sinθ and r²=cosθ with real signed r.","θ=π/4 with r=±sqrt(sqrt2/2); other branches subject to nonnegative r²."),
("Find intersections r=1+cosθ and r=1−sinθ.","cosθ=−sinθ -> θ=3π/4,7π/4 with corresponding positive r.")
]:add(q,ans,f"Source{len(Q)+1} polar curve intersection")
assert len(Q)==72 and [q["SourceExercise"]for q in Q]==list(range(1,73))
out=B/"u08-l04-independent-source-math-072.json"
out.write_text(json.dumps({"SchemaVersion":1,"Section":"8.4","Count":72,
"IndependentMathQuestionsAndTeacherKeys":Q,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U08L04_ALL72_POLAR_GRAPH_SKILLS_PREPARED_NOT_CLOSED")
