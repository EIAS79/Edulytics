from pathlib import Path
import json,math
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
a=json.loads((b/"u07-l01-source-inventory.json").read_text(encoding="utf8"))
assert not a["ReadyForStageD"]
raw=[
("Classify sin²x,cos²x,tan²x by parity","All even on domains","Source1 squared trig parity"),
("Why is secx even?","sec(-x)=1/cos(-x)=secx","Source2 even sec graph"),
("Where does secx not exist?","x=π/2+kπ","Source3 reciprocal denominator"),
("Derive two other Pythagorean identities","Divide sin²+cos²=1 by cos² or sin²: 1+tan²=sec²,1+cot²=csc²","Source4 Pythagorean relations"),
("Simplify sinx cosx secx","sinx; cosx≠0","Source5 reciprocal cancel"),
("Simplify sin(-x)cos(-x)csc(-x)","cosx; sinx≠0","Source6 parity and reciprocal"),
("Simplify tanx sinx+secx cos²x","secx; cosx≠0","Source7 quotient and Pythagoras"),
("Simplify cscx+cosx cot(-x)","sinx; sinx≠0","Source8 parity and Pythagoras"),
("Simplify (cotx+tanx)/sec(-x)","cscx; sinx,cosx≠0","Source9 quotient/reciprocal"),
("Simplify 3sin³x cscx+cos²x+2cos(-x)cosx","3; sinx≠0","Source10 parity and Pythagoras"),
("Simplify -tan(-x)cot(-x)","-1; sinx,cosx≠0","Source11 odd-function product"),
("Simplify -sin(-x)cosx secx cscx tanx/cotx","tan²x; sinx,cosx≠0","Source12 ratio identity"),
("Simplify (1+tan²x)/csc²x+sin²x+1/sec²x","1+sin²x; sinx,cosx≠0","Source13 Pythagorean forms"),
("Simplify tanx/csc²x","sin³x/cosx; sinx,cosx≠0","Source14 complex rational trig identities"),
("Simplify (1-cos²x)/tan²x+2sin²x","1+sin²x; tanx defined nonzero","Source15 reduction"),
("Express (tanx+cotx)/cscx with cosine","secx, sinx,cosx≠0","Source16 ratio to cosine"),
("Express (secx+cscx)/(1+tanx) with sine","cscx, sinx,cosx≠0,1+tanx≠0","Source17 ratio to sine"),
("Simplify cosx/(1+sinx)+tanx","secx, cosx≠0,1+sinx≠0","Source18 rationalization"),
("Simplify 1/(sinx cosx)-cotx","tanx; sinx,cosx≠0","Source19 quotient reduction"),
("Simplify 1/(1-cosx)-cosx/(1+cosx)","(1+cosx)/(sin²x)-cosx/(1+cosx), original both denominators nonzero","Source20 rational manipulation"),
("Simplify (secx+cscx)(sinx+cosx)-2-cotx","tanx; sinx,cosx≠0","Source21 simplify in tangent"),
("Simplify 1/(cscx-sinx)","secx tanx; sinx,cosx≠0","Source22 sec/tan"),
("Rewrite (1-sinx)/(1+sinx)","(secx-tanx)²; sinx≠-1 and cosx≠0 for RHS","Source23 conjugate"),
("For x in quadrantI, express tanx using secx","sqrt(sec²x-1)","Source24 positive root"),
("For x in quadrantI, express secx in cotx","sqrt(1+1/cot²x)","Source25 positivity"),
("For x in quadrantI, express secx in sinx","1/sqrt(1-sin²x)","Source26 Pythagoras"),
("For x in quadrantI, express cotx in sinx","sqrt(1-sin²x)/sinx","Source27 quotient"),
("For x in quadrantI, express cotx using cscx","sqrt(csc²x-1)","Source28 alternate identity"),
("Verify cosx-cos³x=cosx sin²x","Factor cosx(1-cos²x)","Source29 identity proof"),
("Verify cosx(tanx-sec(-x))=sinx-1","cos*tan=sin, cos*sec=1","Source30 identity proof"),
("Verify (1+sin²x)/cos²x=1+2tan²x","Replace 1=sin²+cos² in numerator","Source31 verify identity"),
("Verify (sinx+cosx)²=1+2sinx cosx","Expand square and Pythagoras","Source32 algebra"),
("Verify cos²x-tan²x=2-sin²x-sec²x","Substitute tan²=sec²-1, cos²=1-sin²","Source33 Pythagoras"),
("Verify 1/(1+cosx)-1/(1-cosx)=-2cotx cscx","LHS=-2cosx/(1-cos²x)=-2cosx/sin²x","Source34 prove identity"),
("Disprove csc²x(1+sin²x)=cot²x","LHS=csc²x+1; RHS=csc²x-1; false","Source35 disprove"),
("Prove sec²x-tan²x=1","From sin²+cos²=1 divided by cos²","Source36 Pythagorean nested identity"),
("Disprove (tanx/secx)sin(-x)=cos²x","LHS=-sin²x, generally not cos²x","Source37 counterexample"),
("Verify sec(-x)/(tanx+cotx)=-sin(-x)","LHS=sinx=RHS, on common domain","Source38 parity proof"),
("Verify (1+sinx)/cosx=cosx/(1+sin(-x))","Both=(1+sinx)/cosx; rationalize right side","Source39 prove"),
("Assess (cos²x-sin²x)/(1-tan²x)=sin²x","False; quotient=cos²x where defined","Source40 correct false claim"),
("Verify 3sin²x+4cos²x=3+cos²x","LHS=3(sin²+cos²)+cos²","Source41 verify"),
("Assess (secx+tanx)/(cotx+cosx)=sec²x","False; x=π/4 gives sqrt2 vs2","Source42 disprove")
]
assert len(raw)==42
for n,(q,ans,src) in enumerate(raw,1):
 assert q and ans and src
raw_path=b/"u07-l01-independent-math-questions.json"
data={"SchemaVersion":1,"Section":"7.1","OriginalSourceExerciseCount":42,
"IndependentMathQuestionTeacherKeys":[{"SourceExercise":i,"IndependentQuestion":q,"TeacherAnswer":ans,
"SourceMathematicalSkill":src,"OriginalTextCopied":False}for i,(q,ans,src)in enumerate(raw,1)],
"Runtime":"NOT_RUN"}
raw_path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L01_42_INDEPENDENT_SKILL_TASKS_PREPARED_NOT_CLOSED")
