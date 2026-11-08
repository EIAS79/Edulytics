from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson002.json").read_text(encoding="utf-8"))
assert src["Section"]=="3.2" and (src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"])==(223,238)
worked=[
("Identify parabola characteristics","For f(x)=(x−2)^2−9 identify vertex, symmetry axis, intercepts, and opening.","Vertex (2,−9), axis x=2, opens up, zeros x=−1 and x=5, y-intercept (0,−5)."),
("Write a quadratic from a transformed parabola","A parabola has vertex (3,−4) and passes through (4,−2). Give vertex and general form.","f(x)=2(x−3)^2−4=2x²−12x+14."),
("Convert general to vertex form","Complete square in f(x)=2x²−8x+5 and find its vertex.","f(x)=2(x−2)^2−3; vertex (2,−3)."),
("Find domain and range","Find range of f(x)=−3x²+12x−7 over real inputs.","f(x)=−3(x−2)^2+5: domain R, range (−∞,5]."),
("Maximize an enclosure","With 80 feet of fencing for three sides and an existing fourth wall, maximize rectangular area.","2w+l=80, A=80w−2w²; optimum w=20 feet, l=40 feet, area800 ft²."),
("Maximize linear-demand revenue","A seller can sell q=400−5p items at unit price p dollars. Find revenue-maximizing price and quantity.","R(p)=400p−5p²; max p=40 dollars, q=200 items, revenue $8000."),
("Find quadratic intercepts","For f(x)=3x²−x−2 find both x-intercepts and y-intercept.","Factors (3x+2)(x−1); x=−2/3 and1, y-intercept −2."),
("Solve a nonfactorable quadratic","Find exact x-intercepts of f(x)=2x²+4x−1.","x=(−4±√24)/4=−1±√6/2."),
("Solve a quadratic over complex numbers","Solve x²+2x+5=0.","x=−1±2i; discriminant=−16."),
("Model a projectile","h(t)=−16t²+64t+20 for t≥0 seconds. Find peak time/height and positive time reaching h=0.","Peak t=2s, h=84ft; impact t=2+√21/2≈4.291s; negative root physically excluded.")
]
tries=[
("Vertex from graph","A parabola has vertex (1,−3) and passes through (2,−1). Find its equation.","f(x)=2(x−1)²−3."),
("Complete square","Rewrite x²−4x+7 in vertex form.","(x−2)²+3."),
("Domain and range","Determine range of −2(x+3)²+5.","All real inputs, y≤5."),
("Solve real and complex roots","Find roots of 2x²−4x+5.","x=1±(√6/2)i."),
("Projectile optimization","h(t)=−16t²+96t+112. Find peak time and height; positive impact time.","t=3s, h=256ft, impact t=7s.")
]
concept=[
("Quadratic general form","f(x)=ax²+bx+c, a≠0.","a>0 opens upward, a<0 downward.","x²−4x+1."),
("Vertex form","f(x)=a(x−h)²+k has vertex (h,k).","Axis x=h; y extremum k.","2(x−3)²−1."),
("Vertex coordinate","For ax²+bx+c, h=−b/(2a).","Evaluate k=f(h) without rounding.","x²−6x+2 vertex x=3."),
("Quadratic domain/range","Unrestricted quadratic has domain R and vertex-controlled range.","Open up: [k,∞), open down: (−∞,k].","−(x−1)²+4 has range y≤4."),
("Zeros and discriminant","Roots solve ax²+bx+c=0 and Δ=b²−4ac.","Positive Δ two distinct real, zero double real, negative two nonreal conjugates.","x²+4 has roots ±2i."),
("Quadratic formula","x=(−b±√(b²−4ac))/(2a).","Preserve exact radicals and complex solutions.","x²+2x+5 roots −1±2i."),
("Graph intercepts","y-intercept is (0,c), x-intercepts solve f(x)=0.","A parabola may have 0,1,2 real horizontal intersections.","x²+1 none."),
("Optimization by vertex","A quadratic model with negative leading coefficient peaks at its vertex.","Verify physical input domain before optimizing.","Revenue 400p−5p² maximum at p40."),
("Transformation","a scales/reflections; h shifts horizontal; k shifts vertical.","Use vertex form to read transformed y=x².","−2(x+3)²+5."),
("Contextual domains","Time/distance/production may be nonnegative and bounded.","Reject nonphysical negative-time projectile root.","Height h(t) for t≥0.")
]
cool={"StudentPrompt":"(a) Rewrite 2x²−12x+11 in vertex form. (b) Find real/complex roots of x²+4=0. (c) Maximize R(p)=−3p²+90p under p≥0.","TeacherExpectedMath":"(a) 2(x−3)²−7, vertex (3,−7). (b) ±2i. (c) p=15, maximum revenue675.","OriginalRestrictedPublisherTeacherCoolDownCopied":False}
lesson={
"CultureCode":"en",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L02",
"Title":"Quadratic Functions",
"ExplanationParagraphs":[
 "Original source objectives: identify a parabola’s axis, vertex and intercepts; convert general and vertex forms; determine domain/range and maxima/minima; find complex and real roots; model constrained quadratic optimization.",
 "A quadratic f(x)=ax²+bx+c (a≠0) has axis x=−b/(2a). Writing f(x)=a(x−h)²+k exposes vertex (h,k) and its maximum or minimum.",
 "For intercepts set x=0 for y and set f(x)=0 for x. The discriminant Δ=b²−4ac determines real intersections versus complex conjugate solutions.",
 "The vertex controls range and optimizes a maximum area, revenue or height when its x-value lies in the physical model’s admissible domain.",
 "For projectile formulas, report both algebraic roots but reject negative physical time and state units.",
 "Independent Edulytics Cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {question}"for i,(title,question,answer)in enumerate(tries,1)],
"WorkedExamples":[{"Title":t,"Question":q,"Method":"Apply complete square, vertex/discriminant, exact roots and model domain.","Steps":[a],"Answer":a,"Check":"Substitute into original quadratic and verify physical constraints."}for t,q,a in worked],
"KeyConcepts":[{"Title":t,"Definition":de,"Rule":ru,"Example":ex}for t,de,ru,ex in concept],
"Visuals":[
 {"Kind":"RatioTable","Title":"First-party parabola vertex symmetry table","Description":"The symmetric quadratic y=(x−2)²−9.","PrimaryLabel":"x","SecondaryLabel":"f(x)","Rows":[{"Label":"x","Cells":["−1","0","1","2","3","4","5"]},{"Label":"f(x)","Cells":["0","−5","−8","−9","−8","−5","0"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Revenue maximum from price","Description":"Revenue from demand q=400−5p.","PrimaryLabel":"Price","SecondaryLabel":"Revenue","Rows":[{"Label":"p dollars","Cells":["0","20","40","60","80"]},{"Label":"R(p) dollars","Cells":["0","6000","8000","6000","0"]}],"Items":[],"SecondaryItems":[]}
 ],"Videos":[],
"SummaryPoints":["The quadratic vertex form is a(x−h)²+k.","h=−b/(2a), axis x=h.","Sign of a determines opening; vertex value determines range.","Discriminant determines real versus complex zeros.","Contextual optimization must observe admissible inputs."],
"CommonMistakes":[
 {"Mistake":"Using x=b/(2a) instead of −b/(2a).","WhyWrong":"The sign of the linear coefficient must be reversed.","Correction":"Use h=−b/(2a)."},
 {"Mistake":"Claiming negative discriminant means no solutions.","WhyWrong":"It means no real roots, but there are complex solutions.","Correction":"Write exact complex conjugate roots."},
 {"Mistake":"Optimizing outside the physical range.","WhyWrong":"The mathematical vertex can be outside permissible time or quantity.","Correction":"Check domain endpoints and units."}
]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l02.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L02","ContentVersion":"g12-u03-l02-original-math-skills-firstparty-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u03-l02-five-tryit-teacher-key-and-independent-cooldown.json").write_text(json.dumps({
"OriginalSourceTryItCount":5,"IndependentFirstPartyTryIts":5,
"TeacherOnlyExpectedMath":[{"Number":i,"Expected":a}for i,(t,q,a)in enumerate(tries,1)],
"IndependentCoolDown":cool,"PublisherGraphicsCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":2,
"OriginalSourceSection":"3.2","OriginalSourceSHA256":src["SourceSha256"],
"OriginalSourcePdfPages":[223,238],
"OriginalSourceWorkedExamples":10,"OriginalSourceTryItCount":5,"OriginalSourceNumberedExercises":94,
"FirstPartyWorkedExamples":10,"FirstPartyTryItAlternatives":5,
"FirstPartyKeyConcepts":10,"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"RichV2Path":rp.relative_to(root).as_posix(),
"Original94SourceExerciseSkillAssessment":"PENDING",
"OriginalFigureSkills":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u03-l02-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==12 and p["FirstIncompleteLessonOrdinal"]==13
row=p["LessonEntries"][12]
row["ACStatus"]="A_C_IN_PROGRESS";row["RichV2Path"]=cp["RichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l02-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["Original94 numbered quadratic exercise cognitive/teacher rubric mapping incomplete"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L02_10_WORKED_5_TRYITS_10_CONCEPTS_2_VISUAL_TABLES_COOLDOWN_AUTHORED")
