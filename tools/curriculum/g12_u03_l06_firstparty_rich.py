from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson006.json").read_text(encoding="utf-8"))
assert src["Section"]=="3.6" and(src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"])==(281,292)
worked=[
("Use remainder theorem","For p(x)=x⁴−3x²+2x−1, find remainder on division by x−2.","p(2)=16−12+4−1=7."),
("Use factor theorem and find roots","For p(x)=x³−4x²+x+6, show x−2 factor, then find all real zeros.","p(2)=0, p(x)=(x−2)(x−3)(x+1); roots −1,2,3."),
("Generate rational zero candidates","List all possible rational roots of p(x)=2x³−5x²−4x+3.","Candidates ±1,±3,±1/2,±3/2; theorem gives candidates not guaranteed roots."),
("Test rational candidates","Find all roots of p(x)=x³−6x²+11x−6 using rational candidate checks.","p(x)=(x−1)(x−2)(x−3); roots1,2,3."),
("Repeated real zeros","Find real zero multiplicities of p(x)=x³−3x+2.","p(x)=(x−1)²(x+2), x=1 double, −2 simple."),
("Conjugate complex roots","Solve p(x)=x³−x²+4x−4 over complex numbers.","p=(x−1)(x²+4); roots1,±2i."),
("Reconstruct polynomial from nonreal zeros","Construct monic polynomial with real coefficients and zeros 3,1+2i,1−2i.","p(x)=(x−3)(x²−2x+5)=x³−5x²+11x−15."),
("Descartes sign changes","For p(x)=x⁴−3x³+2x−4, list possible numbers of positive and negative real roots.","p(x) coefficient sign changes3→possible positive3 or1; p(−x)=x⁴+3x³−2x−4 sign changes1→negative1."),
("Model a cubic box volume","A cake has length4 inches more than width w and height w/3. Its volume is 351 cubic inches. Find positive dimensions.","w²(w+4)=1053, positive root w=9 inches; length13 inches, height3 inches.")
]
tries=[
("Remainder at negative input","Find remainder of p(x)=x³−4x²+x+7 divided by x+2.","p(−2)=−8−16−2+7=−19."),
("Factor theorem with known root","Find zeros of p(x)=x³−5x²+2x+8 given x−2 is a factor.","At x2:8−20+4+8=0; division yields x²−3x−4=(x−4)(x+1). Zeros −1,2,4."),
("Rational zero possibilities","List possible rational zeros of 3x³−4x²−7x+2.","±1,±2,±1/3,±2/3."),
("Find real and complex zeros","Solve p(x)=x³−x²+9x−9.","(x−1)(x²+9), roots1,±3i."),
("Real coefficients conjugate pair","Build monic cubic with zeros −2,3i,−3i.","(x+2)(x²+9)=x³+2x²+9x+18."),
("Descartes sign possibilities","For p(x)=x⁴−2x³−x²+4x−4, give allowed positive and negative root counts.","p(x) signs +−−+− gives3 or1 positive, p(−x)=x⁴+2x³−x²−4x−4 signs++−−− gives1 negative."),
("Box model polynomial dimensions","A box height h, width h+1, length 2h+1 has volume30 cubic units. Find positive h and dimensions.","h=2 yields2·3·5=30, width3,length5.")
]
concepts=[
("Remainder Theorem","Remainder on division by x−k equals p(k).","Evaluate directly rather than perform full division.","p(2)=7."),
("Factor Theorem","x−k is a factor iff p(k)=0.","Zero remainder certifies root/factor.","p(2)=0 ⇒ x−2."),
("Rational Zero Theorem","For integer coefficients, any reduced rational root a/b has a dividing constant and b dividing leading coefficient.","Candidates need testing; some roots irrational or complex.","p=2x³−5x²−4x+3 candidates ±1,±3,±1/2,±3/2."),
("Synthetic root testing","Substitute candidate and divide to lower polynomial degree.","Repeat a root to detect multiplicity.","(x−1)² repeated root."),
("Linear factorization","Polynomial over complex numbers factors into degree1 factors counting multiplicity.","Real-coefficient polynomials have nonreal conjugate pairs.","x²+4=(x−2i)(x+2i)."),
("Complex conjugate zeros","If coefficients are real and a+bi is a root with b≠0, a−bi also is.","Multiply conjugate factors into real quadratic.","(x−1)²+4."),
("Descartes' Rule of Signs","Positive real zero count equals sign changes of p(x), or less by an even integer.","Negative real zero count uses sign changes of p(−x).","3 sign changes allows3 or1."),
("Polynomial from zeros","Multiply (x−r) factors and solve scale from a known point.","Repeated roots use powers and nonreal roots paired for real coefficients.","p=a(x−2)²(x+1)."),
("Polynomial volume model","Combine dimensional expressions with volume=length×width×height.","Use only positive physical roots and correct units.","w(w+4)(w/3)=351.")
]
cool={"StudentPrompt":"(a) Find remainder of x³−2x²+5x−4 on division by x−1. (b) Factor x³−6x²+11x−6. (c) Construct monic polynomial with roots 2,±i.","TeacherExpectedMath":"(a)0; x−1 a factor. (b)(x−1)(x−2)(x−3). (c)(x−2)(x²+1)=x³−2x²+x−2.","OriginalPublisherRestrictedCoolDownCopied":False}
lesson={"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L06",
"CultureCode":"en","Title":"Zeros of Polynomial Functions",
"ExplanationParagraphs":[
 "Original source learning goals: evaluate a polynomial via remainder theorem, apply factor theorem and Rational Zero Theorem, find real/complex roots, construct factors, analyze Descartes sign changes and solve polynomial volume models.",
 "The Remainder Theorem says division by x−k leaves p(k); the Factor Theorem specializes this to zero remainder. Rational roots must be tested, not assumed.",
 "The Rational Zero Theorem gives possible fractions ±(factor of constant)/(factor of leading coefficient), reduced to lowest terms.",
 "Nonreal zeros of real-coefficient polynomials occur in conjugate pairs. All n complex roots of degree n are counted with multiplicity.",
 "Descartes' Rule of Signs restricts possible positive/negative real zero counts but does not identify roots; explicitly test candidates.",
 "Contextual models such as cubic volume require rejecting nonpositive lengths or other impossible roots.",
 "Independent Edulytics cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {q}"for i,(title,q,a)in enumerate(tries,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Use exact remainder evaluation, factorization, rational candidates, sign changes and physical validity.","Steps":[answer],"Answer":answer,"Check":"Substitute each alleged zero and compare full factor product."}for title,question,answer in worked],
"KeyConcepts":[{"Title":title,"Definition":definition,"Rule":rule,"Example":ex}for title,definition,rule,ex in concepts],
"Visuals":[{"Kind":"RatioTable","Title":"First-party rational candidate trial","Description":"Possible candidates from constant3 and leading2.","PrimaryLabel":"Numerator","SecondaryLabel":"Candidate","Rows":[{"Label":"Possible |a|","Cells":["1","3"]},{"Label":"Possible |b|","Cells":["1","2"]},{"Label":"Reduced candidates","Cells":["±1,±3","±1/2,±3/2"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Polynomial sign-change diagnostic","Description":"Descartes sign-change possibilities are bounds, not actual root counts.","PrimaryLabel":"Expression","SecondaryLabel":"Changes","Rows":[{"Label":"p(x)","Cells":["+","−","+", "−"]},{"Label":"Sign changes across terms","Cells":["","1","2","3"]},{"Label":"Possible positive real zeros","Cells":["3","1","",""]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],
"SummaryPoints":["Remainder at k is p(k), factor iff remainder0.","Rational candidates are possibilities, not certified zeros.","Nonreal roots occur in conjugate pairs for real polynomials.","Descartes gives sign-change bounds, not exact counts.","Volume model roots must satisfy positive dimensions."],
"CommonMistakes":[{"Mistake":"Treating each Rational Zero Theorem candidate as an actual root.","WhyWrong":"It is only a finite list to test.","Correction":"Evaluate p(candidate) or synthetic divide."},
{"Mistake":"Forgetting complex conjugates in real-coefficient polynomial.","WhyWrong":"Nonreal zeros must come in conjugate pairs.","Correction":"Include the other conjugate factor."},
{"Mistake":"Treating Descartes possible count as actual count.","WhyWrong":"Counts decrease by even integers and complex roots may exist.","Correction":"Factor or inspect zeros separately."}]}
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l06.rich-lesson-v2.json"
assert not rich.exists()
rich.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L06",
"ContentVersion":"g12-u03-l06-firstparty-polynomial-zeros-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u03-l06-seven-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalSourceTryIts":7,"FirstPartyIndependentTryItCount":7,
"TeacherMathExpected":[{"Number":i,"Answer":answer}for i,(_,_,answer)in enumerate(tries,1)],
"IndependentCoolDown":cool,"OriginalPublisherImagesCopied":False,"StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":6,
"OriginalSourceSection":"3.6","OriginalSHA256":src["SourceSha256"],"OriginalPdfPages":[281,292],
"SourceWorkedExamples":9,"SourceTryItActivities":7,"SourceNumberedExercises":80,
"FirstPartyWorkedExamples":9,"FirstPartyTryItAlternatives":7,
"FirstPartyKeyConcepts":9,"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"FirstPartyRichV2Path":rich.relative_to(root).as_posix(),
"Original80SourceNumberedQuestionTeacherMathRubrics":"PENDING",
"SourceFigureSkillCrosswalk":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u03-l06-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==16 and p["FirstIncompleteLessonOrdinal"]==17
row=p["LessonEntries"][16]
row["ACStatus"]="A_C_IN_PROGRESS";row["FirstPartyRichV2Path"]=cp["FirstPartyRichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l06-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["80 numbered original polynomial roots cognitive actions and teacher mathematics pending","Original source figures academic skill mapping pending"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L06_9_WORKED_7_TRYIT_9_CONCEPTS_2_VISUALS_COOLDOWN_AUTHORED")
