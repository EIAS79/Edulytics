#!/usr/bin/env python3
"""Manual source-only verification, OpenStax 2017 Section 1.1 exercises 1–39.
No source answer-key copying, no unsupported claim of Edulytics parity.
Original local PDF page 34 visually inspected for each prompt.
"""
import json,datetime
from pathlib import Path
base=Path(__file__).resolve().parents[2]/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p=base/"u01-l01-element-coverage.json"
j=json.loads(p.read_text(encoding="utf8"))
math={
 1:"A relation is a set of ordered pairs; a function assigns every domain input exactly one output.",
 2:"An input is the independent argument; an output is its uniquely assigned dependent value.",
 3:"Vertical-line test: more than one intersection gives two outputs for the same x and is not a function.",
 4:"To test one-to-one, compare whether two distinct permitted inputs share an output.",
 5:"Horizontal-line test: more than one intersection means the function is not one-to-one.",
 6:"Not a function: input a has distinct outputs b and c.",
 7:"Function: distinct inputs a,b,c have one output each (repeating the output c is allowed).",
 8:"Function: 5x+2y=10 uniquely defines y=5−(5/2)x for every real x.",
 9:"Function: y=x² assigns one output to each real x; not one-to-one over all reals.",
 10:"Not a function of x: x=y² admits both y=√x and −√x when x>0.",
 11:"Function: 3x²+y=14 gives unique y=14−3x².",
 12:"Not a function of x: 2x+y²=6 has y=±√(6−2x) at many x≤3.",
 13:"Function: y=−2x²+40x is a polynomial, hence one output per x.",
 14:"Function: y=1/x for x≠0; x=0 is excluded from domain.",
 15:"Function: x=(3y+5)/(7y−1) rearranges to y=(x+5)/(7x−3) for x≠3/7; check source denominator y≠1/7.",
 16:"Not a y-function of x: x=√(1−y²) is the right semicircle; at 0≤x<1 there are two y values.",
 17:"Function: y=(3x+5)/(7x−1), uniquely defined for x≠1/7.",
 18:"Not a y-function: x²+y²=9 has two y values at x=0.",
 19:"Function: 2xy=1 gives y=1/(2x) for x≠0.",
 20:"Function: x=y³ gives unique real y=cuberoot(x).",
 21:"Function: y=x³ has one output per real x.",
 22:"Function: y=√(1−x²), domain −1≤x≤1; principal nonnegative root is unique.",
 23:"Function: x=±√(1−y) is equivalent to y=1−x²; every real x has one y.",
 24:"Not a y-function: y=±√(1−x), for x<1 there are two distinct y values.",
 25:"Not a y-function: y²=x² gives y=±|x| at every nonzero x.",
 26:"Function: y³=x² gives unique real y=cuberoot(x²)≥0.",
 27:"For f(x)=2x−5: f(−3)=−11; f(2)=−1; f(−a)=−2a−5; −f(a)=−2a+5; f(a+h)=2a+2h−5.",
 28:"For f(x)=−5x²+2x−1: f(−3)=−52; f(2)=−17; f(−a)=−5a²−2a−1; −f(a)=5a²−2a+1; f(a+h)=−5(a+h)²+2(a+h)−1.",
 29:"For f(x)=√(2−x)+5: domain x≤2; f(−3)=√5+5; f(2)=5; f(−a)=√(2+a)+5 for a≥−2; −f(a)=−√(2−a)−5; f(a+h)=√(2−a−h)+5.",
 30:"For f(x)=(6x−1)/(5x+2): domain x≠−2/5; f(−3)=19/13; f(2)=11/12; f(−a)=(−6a−1)/(2−5a); −f(a)=−(6a−1)/(5a+2); f(a+h)=(6a+6h−1)/(5a+5h+2).",
 31:"For f(x)=|x−1|−|x+1|: f(−3)=2; f(2)=−2; f(−a)=|a+1|−|a−1|; −f(a)=|a+1|−|a−1|; f(a+h)=|a+h−1|−|a+h+1|.",
 32:"For g(x)=5−x², (g(x+h)−g(x))/h=−2x−h for h≠0.",
 33:"For g(x)=x²+2x, (g(x)−g(a))/(x−a)=x+a+2 for x≠a.",
 34:"k(t)=2t−1: k(2)=3; solve k(t)=7 giving t=4.",
 35:"f(x)=8−3x: f(−2)=14; solve f(x)=−1 giving x=3.",
 36:"p(c)=c²+c: p(−3)=6; solve p(c)=2 giving c=−2 or c=1.",
 37:"f(x)=x²−3x: f(5)=10; solve f(x)=4 giving x=4 or −1.",
 38:"f(x)=√(x+2): f(7)=3; solve f(x)=4 giving x=14; real domain x≥−2.",
 39:"3r+2t=18 gives r=f(t)=6−2t/3; f(−3)=8; f(t)=2 gives t=6."
}
assert set(math)==set(range(1,40))
for element in j["Elements"]:
 if element["Category"]!="EXERCISE":continue
 n=int(element["SourceElementId"].rsplit("EXERCISE",1)[-1])
 if n not in math:continue
 assert element["Status"]=="SOURCE_IDENTIFIED",(n,element["Status"])
 element["Status"]="SOURCE_VERIFIED"
 element["SourceMathCheck"]=math[n]
 element["VerificationMethod"]="ORIGINAL_OPENSTAX_PDF_PAGE34_VISUALLY_INSPECTED_AND_MATH_RECONCILED"
 element["DeliveryStatus"]="NOT_YET_IMPLEMENTED"
 element["ImplementationType"]="PENDING_GRADE12_DIGITAL_OR_TEACHER_ASSESSMENT"
 # Graph/Math source semantics are checked here. No implementation equality is asserted.
 element["SemanticEdulyticsEquivalenceStatus"]="NOT_ASSESSED"
j["SourceMathVisuallyInspectedExerciseCount"]=89
j["SourceMathUninspectedOriginalExerciseNumbers"]=[52,53,54]
j["OriginalMathSourceReviewedDoesNotEqualEdulyticsParity"]=True
j["MatchedDigitally"]=j["MatchedTeacherLed"]=0
j["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
p.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
prog=base/"full-grade-progress.json"
k=json.loads(prog.read_text(encoding="utf8"))
row=k["LessonEntries"][0]
row["SourceExercisesIndividuallyInterpreted"]=89
row["SourceExercisesMathPendingNumbers"]=[52,53,54]
row["SourceElementsIndependentlyMatched"]=0
row["UpdatedUtc"]=j["UpdatedUtc"]
prog.write_text(json.dumps(k,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
print("SOURCE_ONLY_89_OF_92_EXERCISES_INTERPRETED;_EDULYTICS_SEMANTIC_MATCHES_0;_NEXT_EXERCISES_52_53_54")
