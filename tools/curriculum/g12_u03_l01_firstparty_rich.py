from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson001.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.1" and source["Pages"][0]["PdfPage"]==213 and source["Pages"][-1]["PdfPage"]==222
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L01"
worked=[
("Simplify a negative square root","Write √(−25) as a complex number a+bi.","5i, because i²=−1."),
("Plot complex coordinate","Represent −2+3i in the Argand plane.","Point (−2,3), with real axis horizontal and imaginary axis vertical."),
("Add two complex numbers","Compute (4−3i)+(2+5i).","6+2i."),
("Scale a complex number","Compute −3(2+4i).","−6−12i."),
("Multiply complex binomials","Multiply (2+3i)(4−i).","11+10i, because i²=−1."),
("Take complex conjugates","Find conjugates of 5−2i and −3i.","5+2i and 3i."),
("Divide using conjugate","Compute (2+3i)/(1−i) in standard complex form.","(−1/2)+(5/2)i by multiplying numerator/denominator by (1+i)."),
("Substitute complex input in polynomial","For f(z)=z²+2z+1, calculate f(2+i).","(2+i)²+2(2+i)+1=8+6i."),
("Substitute complex input in rational function","For g(z)=(z+2)/(z+1), evaluate g(i).","(2+i)/(1+i)=(3−i)/2."),
("Reduce high power of i","Evaluate i^37 using the four-step cycle.","i because 37≡1 mod4.")
]
tries=[
("Negative-root conversion","Simplify √(−48).","4√3 i."),
("Complex coordinate plotting","Plot 4−5i on complex axes.","Point (4,−5)."),
("Complex subtraction","Find (5+2i)−(3−4i).","2+6i."),
("Multiply real and complex","Find −2(3+5i).","−6−10i."),
("Multiply two complex numbers","Find (3−2i)(1+4i).","11+10i."),
("Polynomial substitution","Evaluate f(z)=z²−z at z=3−i.","(3−i)²−(3−i)=5−5i."),
("Rational substitution","Evaluate (z+1)/(z−2) at z=i.","(1+i)/(−2+i)=(−1−3i)/5.")
]
concept=[
("Imaginary unit","i is defined by i²=−1.","For real a>0, √(−a)=i√a.","√−9=3i."),
("Complex standard form","z=a+bi with a,b real.","Re(z)=a, Im(z)=b.","2−3i has Re2, Im−3."),
("Complex plane","Plot (a,b) for complex a+bi.","Horizontal axis real; vertical axis imaginary.","−2+3i→(−2,3)."),
("Addition/subtraction","Combine real parts and imaginary parts separately.","(a+bi)+(c+di)=(a+c)+(b+d)i.","(1+i)+(2−3i)=3−2i."),
("Complex multiplication","Distribute and replace i² by −1.","(a+bi)(c+di)=(ac−bd)+(ad+bc)i.","(1+i)(1−i)=2."),
("Conjugate","Conjugate of a+bi is a−bi.","z·conj(z)=a²+b².","Conjugate 2−i is 2+i."),
("Complex quotient","Multiply numerator and denominator by denominator conjugate.","A nonzero complex denominator gives a positive real a²+b².","(1+i)/(1−i)=i."),
("Powers of i","Powers repeat every four.","i^n is 1,i,−1,−i for n mod4=0,1,2,3.","i^37=i."),
("Complex substitution","Polynomial/rational functions may accept complex inputs if denominators are nonzero.","Use complex multiplication and conjugates to simplify.","(2+i)²=3+4i.")
]
visual=[
{"Kind":"RatioTable","Title":"Complex coordinate pairs","Description":"Independent mapping from standard form to the Argand plane.","PrimaryLabel":"Complex number","SecondaryLabel":"Coordinate","Rows":[{"Label":"z","Cells":["2+3i","−2+3i","2−3i","−2−3i"]},{"Label":"(Re,Im)","Cells":["(2,3)","(−2,3)","(2,−3)","(−2,−3)"]}],"Items":[],"SecondaryItems":[]},
{"Kind":"RatioTable","Title":"Cyclic imaginary powers","Description":"The values repeat every four exponent steps.","PrimaryLabel":"n mod4","SecondaryLabel":"i^n","Rows":[{"Label":"Exponent remainder","Cells":["0","1","2","3"]},{"Label":"Value","Cells":["1","i","−1","−i"]}],"Items":[],"SecondaryItems":[]}
]
cool={"Prompt":"(a) Plot z=−3+2i. (b) Compute (2−i)(3+2i). (c) Simplify (3+i)/(1−i). (d) Find i^26.","TeacherExpectedMath":"(a) (−3,2); (b) 8+i; (c) 1+2i; (d) −1.","OriginalPublisherRestrictedCoolDownCopied":False}
lesson={"LessonCode":code,"CultureCode":"en","Title":"Complex Numbers",
"ExplanationParagraphs":[
 "Learning goals: write square roots of negative reals in terms of i, plot complex coordinates, add/subtract and multiply/divide complex numbers.",
 "The imaginary unit i satisfies i²=−1. A complex number is a+bi with independently real and imaginary coefficients.",
 "Complex numbers form ordered-coordinate points (real,imaginary) in the plane. Addition is component-wise and subtraction reverses each component of the subtrahend.",
 "Multiply using distribution and i²=−1. For a quotient, multiply numerator and denominator by the conjugate of a nonzero complex denominator.",
 "The powers of i cycle every four. To evaluate complex-valued functions, substitute and simplify algebraically, checking that original rational denominators are not zero.",
 "Independent Edulytics cool-down: "+cool["Prompt"]
 ]+[f"Independent Try It {n} — {title}. {prompt}"for n,(title,prompt,answer)in enumerate(tries,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Use standard complex algebra and i²=−1; show exact real/imaginary components.","Steps":[answer],"Answer":answer,"Check":"Verify with pairwise multiplication/conjugate and required denominators."}for title,question,answer in worked],
"KeyConcepts":[{"Title":title,"Definition":definition,"Rule":rule,"Example":example}for title,definition,rule,example in concept],
"Visuals":visual,"Videos":[],
"SummaryPoints":["i²=−1; √(−a)=i√a for a>0.","Plot a+bi at (a,b).","Complex arithmetic combines real and imaginary components.","Conjugates rationalize a complex denominator.","i^n has period four."],
"CommonMistakes":[
 {"Mistake":"Treating i² as +1.","WhyWrong":"Defining identity is i²=−1.","Correction":"Reduce i² before collecting terms."},
 {"Mistake":"Plotting a+bi as (b,a).","WhyWrong":"Real axis is horizontal and imaginary vertical.","Correction":"Plot (Re,Im)=(a,b)."},
 {"Mistake":"Dividing real and imaginary parts term by term.","WhyWrong":"Complex denominator must be handled as a whole.","Correction":"Use its complex conjugate."}
]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l01.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L01","ContentVersion":"g12-u03-l01-firstparty-source-math-skill-authoring-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u03-l01-firstparty-seven-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalTryItSkillCount":7,"IndependentFirstPartyTryItCount":7,
"TeacherExpectedAnswers":[{"Number":i,"Answer":answer}for i,(_,_,answer)in enumerate(tries,1)],
"IndependentCoolDown":cool,"PublisherOriginalImagesCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":1,"SourceSection":"3.1",
"OriginalSourceSHA256":source["SourceSha256"],"SourcePdfPages":[213,222],
"OriginalWorkedExampleSkills":10,"OriginalTryItSkills":7,"OriginalSourceNumberedExercises":58,
"FirstPartyWorkedExamples":10,"FirstPartyTryItAlternatives":7,"FirstPartyKeyConcepts":9,
"FirstPartyVisualTables":2,"FirstPartyCoolDown":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"SourceOriginalNumberedAssessmentReview":"PENDING","OriginalFigureCrosswalk":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u03-l01-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==11 and p["FirstIncompleteLessonOrdinal"]==12
entry=p["LessonEntries"][11]
entry["ACStatus"]="A_C_IN_PROGRESS";entry["FirstPartyRichV2Path"]=cp["FirstPartyRichV2Path"]
entry["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l01-abc-checkpoint.json"
entry["ReadyForStageD"]=False
entry["BlockingReasons"]=["Author 58 independent source-skill exercise questions with correct complex answer keys","Figure concept crosswalk pending"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L01_10_WORKED_7_TRYIT_9_COMPLEX_CONCEPTS_2_TABLES_COOLDOWN_AUTHORED")
