from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson005.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.5" and len(source["Pages"])==25
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L05"
# Each authored model preserves the mathematical skill and representation of
# its source example without copying original narrative, tables or artwork.
ex=[
("Vertical translation in a context","A rooftop sensor records open area A(t) square metres; its redesigned value is always 3 m² higher. Express the transformation.","A_new(t)=A(t)+3; each plotted y-coordinate increases by 3 while time stays fixed.","At each t, new output−old output=3 m²."),
("Translate a table vertically","For x=[0,2,4,6] and f(x)=[2,5,7,9], construct g(x)=f(x)−2.","g outputs [0,3,5,7], with inputs unchanged.","The original and new rows have the same x-values."),
("Shift a schedule earlier","A shift schedule is represented by S(t). The plan begins one hour earlier without altering the output sequence. Define shifted function.","T(t)=S(t+1), a horizontal shift left one hour.","T(2)=S(3)."),
("Translate a table horizontally","For f(1)=3,f(3)=5,f(5)=7, tabulate g(x)=f(x−2) at the appropriate new inputs.","g(3)=3,g(5)=5,g(7)=7; every x-value shifts right 2.","g(x) reuses the former f-output at x−2."),
("Identify a shifted parabola","An upward parabola has the same shape as y=x² but a vertex at (−3,0). Find its rule.","g(x)=(x+3)²; horizontal shift left 3.","g(−3)=0."),
("Compare input versus output shifts in context","For a distance-to-cost function C(m) dollars for m miles, interpret C(m)+5 and C(m+5).","The first adds a $5 fee; the second prices five extra miles.","The input and output units cannot be exchanged."),
("Combine horizontal and vertical shifts","Transform f(x)=|x| into h(x)=f(x−2)+3; state vertex and range.","h(x)=|x−2|+3 with vertex (2,3), domain ℝ and range [3,∞).","Inside −2 moves right; outside +3 moves up."),
("Recover a shifted root function","A square-root curve begins at (−2,4) and retains the toolkit shape. Give its rule and domain/range.","g(x)=√(x+2)+4; domain [−2,∞), range [4,∞).","The startpoint is included."),
("Reflect a square-root graph","Reflect f(x)=√x in each coordinate axis separately.","Across x-axis: −√x with domain x≥0; across y-axis: √(−x) with domain x≤0.","Output reflection changes sign; input reflection changes input direction."),
("Reflect a finite mapping","f(1)=2,f(3)=5,f(4)=7. Give −f(x) and f(−x) tabulations.","−f: (1,−2),(3,−5),(4,−7); f(−x): (−1,2),(−3,5),(−4,7).","Reflection over x-axis negates outputs; over y-axis negates inputs."),
("Combine reflection and translation","Transform y=2^x into k(x)=1−2^(−x); identify reflection and shift.","Reflect horizontally y=2^(−x), then vertically −2^(−x), then up 1; range (−∞,1).","At x=0 output 0; horizontal asymptote y=1."),
("Test odd and even algebraically","Classify f(x)=x³+4x.","Odd, since f(−x)=−x³−4x=−f(x); origin symmetry.","Odd function domain must be symmetric about zero."),
("Vertical scaling in a measured context","A city's weekly count is P(t). A new city follows twice the output at each corresponding week. Express graph relation.","Q(t)=2P(t), vertical stretch factor 2.","x-coordinates unchanged."),
("Vertical compression of a table","Given f(1)=4,f(2)=10,f(3)=12, form g(x)=(1/2)f(x).","g(1)=2,g(2)=5,g(3)=6.","Outputs halve, domain unchanged."),
("Identify vertical stretch in a cubic","A transformed cubic passes through (1,3), (0,0), (−1,−3) and is g(x)=a·x³. Determine a.","a=3, so g(x)=3x³; vertical stretch factor 3.","At x=1, g(1)=3."),
("Horizontal compression of a time series","An activity model Q(t) follows the original profile P but evolves three times as quickly. Write transformation.","Q(t)=P(3t); each original event at t=a occurs at t=a/3.","Inside input ×3 means horizontal compression by 1/3."),
("Horizontal stretch of a table","For f(2)=3,f(4)=5,f(6)=7, create g(x)=f(x/2) at known inputs.","g(4)=3,g(8)=5,g(12)=7; x doubles for matching outputs.","The new graph stretches horizontally by 2."),
("Identify horizontal compression from coordinate pairs","The original graph contains a feature (6,4) and transformed graph has feature (2,4) with unchanged height. State scaling.","g(x)=f(3x), horizontal compression by factor 1/3.","g(2)=f(6)."),
("Transform a table in three steps","For f(3)=5,f(6)=8,f(9)=10, tabulate g(x)=2f(3x)+1.","g(1)=11,g(2)=17,g(3)=21.","Inputs divide by 3, outputs double then increase by 1."),
("Compose three graph transformations","For f(x)=|x|, give k(x)=f((x+2)/2)−3 and identify each transformation.","k(x)=|(x+2)/2|−3=(1/2)|x+2|−3; vertex (−2,−3), range [−3,∞).","Inside (x+2)/2 creates horizontal stretch 2 and left shift 2; outside −3 lowers graph.")
]
assert len(ex)==20
worked=[{"Title":t,"Question":q,
 "Method":"Use first-party input/output transformation rules, preserve units, and verify domain and marked graph points.",
 "Steps":[a],"Answer":a,"Check":check}for t,q,a,check in ex]
tries=[
("Model vertical shift","A ball's height is h(t)=−5t²+30t metres. Write new height b(t) if launched 8 m above ground.","b(t)=−5t²+30t+8."),
("Root horizontal shift","For f(x)=√x, graph g(x)=f(x+2). Give new startpoint and domain.","Left 2; startpoint (−2,0), domain [−2,∞)."),
("Absolute-value translation","For f(x)=|x|, graph h(x)=f(x−3)+2. Identify its vertex.","h(x)=|x−3|+2; vertex (3,2)."),
("Reciprocal combined shift","Transform y=1/x one unit right and one up.","g(x)=1/(x−1)+1; x≠1."),
("Reflect an absolute value graph","Reflect f(x)=|x−1| about x-axis and about y-axis separately.","−|x−1| and |x+1|."),
("Reflect finite table","If f(−1)=5,f(0)=8,f(2)=3, create tables for −f(x) and f(−x).","−f values −5,−8,−3 on x=−1,0,2; f(−x) pairs (1,5),(0,8),(−2,3)."),
("Reflect an even function","Given f(x)=x², find −f(x) and f(−x). Explain difference.","−x² (downward parabola) versus x² (unchanged)."),
("Even or odd polynomial","Classify f(x)=x⁴+2x²+5.","Even; f(−x)=f(x)."),
("Compress vertical table","Given f(1)=8, f(2)=12, f(3)=16, find g(x)=(3/4)f(x).","g values 6,9,12."),
("Stretch and translate identity","Vertically stretch f(x)=x by 3 and then shift down 2.","g(x)=3x−2."),
("Stretch square root horizontally","Construct √x stretched horizontally by 3.","g(x)=√(x/3), x≥0.")
]
assert len(tries)==11
concepts=[
("Vertical shifts","g(x)=f(x)+k moves a graph vertically by k output units.","Domain unchanged; range shifts by k.","x²+3 has vertex (0,3)."),
("Horizontal shifts","g(x)=f(x−h) shifts right h; f(x+h) shifts left h.","Input changes have opposite sign to visual displacement.","|x−2| vertex (2,0)."),
("Reflections","−f(x) reflects across x-axis; f(−x) reflects across y-axis.","Negating output differs from negating input.","−√x has nonpositive range, √(−x) has nonpositive domain."),
("Even/odd parity","Even means f(−x)=f(x); odd means f(−x)=−f(x).","Verify domain symmetry and algebra; many functions are neither.","x² is even, x³ is odd, (x−1)² neither."),
("Vertical scale","g(x)=a·f(x) stretches for |a|>1 and compresses for 0<|a|<1.","Negative a also reflects vertically.","2x² doubles outputs."),
("Horizontal scale","g(x)=f(bx) compresses horizontally if |b|>1 and stretches if 0<|b|<1.","Compare input coordinates using x_new=x_old/b.","f(3x) shifts old features x=6 to x=2."),
("Combined transformations","Apply inside input changes, then output changes; factor inner affine term to locate horizontal shift.","Transform endpoints and preserve allowed-domain preimages.","f((x+2)/2)−3 shifts stretched graph left 2 and down 3.")
]
visuals=[
 {"Kind":"RatioTable","Title":"Vertical shift and reflection in a first-party table","Description":"Output transformations preserve x coordinates.","PrimaryLabel":"Input","SecondaryLabel":"Output","Rows":[{"Label":"x","Cells":["1","2","3"]},{"Label":"f(x)","Cells":["4","10","12"]},{"Label":"f(x)−2","Cells":["2","8","10"]},{"Label":"−f(x)","Cells":["−4","−10","−12"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Horizontal scale in a first-party table","Description":"To evaluate f(x/2) at known original inputs, double each original input.","PrimaryLabel":"Input","SecondaryLabel":"Output","Rows":[{"Label":"original x","Cells":["2","4","6"]},{"Label":"f(x)","Cells":["3","5","7"]},{"Label":"stretched x","Cells":["4","8","12"]},{"Label":"g(x)=f(x/2)","Cells":["3","5","7"]}],"Items":[],"SecondaryItems":[]}
]
cool={
"StudentPrompt":"(a) For f(x)=|x| give the vertex and range of g(x)=2f(x−3)−4. (b) For h(x)=√x explain the domain of h(−x). (c) Determine if p(x)=x³−5x is odd, even, or neither.",
"TeacherExpectedMathematics":"(a) vertex (3,−4), range [−4,∞); (b) x≤0; (c) odd, since p(−x)=−p(x).",
"TeacherReview":True,"OriginalPublisherRestrictedCooldownCopied":False}
lesson={
"CultureCode":"en","LessonCode":code,"Title":"Transformation of Functions",
"ExplanationParagraphs":[
 "Learning goals: shift graphs vertically and horizontally, reflect about axes, identify even/odd symmetry, stretch/compress inputs and outputs, and combine several transformations while tracking original domain restrictions.",
 "For g(x)=f(x)+k output values shift up or down by k. For g(x)=f(x−h), graph moves right h despite the subtraction inside.",
 "For −f(x), negate the output, reflecting across horizontal x-axis. For f(−x), negate each input to reflect across the vertical y-axis. An even function is unchanged under f(−x); an odd function changes to −f(x).",
 "Multiplying outputs g(x)=a f(x) gives vertical scaling; multiplying inputs g(x)=f(bx) changes horizontal scaling inversely. Negative factors may add reflection.",
 "A contextual formula's units reveal whether the change is to an input or to an output. Never add miles directly to dollars or years directly to population.",
 "For combined transformations apply the input substitution and output scaling carefully, then check vertex/endpoints, asymptotes and domain.",
 "Independent Edulytics Cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {prompt}"for i,(title,prompt,_)in enumerate(tries,1)],
"KeyConcepts":[{"Title":t,"Definition":d,"Rule":r,"Example":e}for t,d,r,e in concepts],
"WorkedExamples":worked,"Visuals":visuals,"Videos":[],
"SummaryPoints":[
 "Output shifts f(x)+k move the graph vertically by k.",
 "Input shifts f(x−h) move graph horizontally right h.",
 "−f(x) reflects outputs; f(−x) reflects input coordinates.",
 "Even functions satisfy f(−x)=f(x), odd functions satisfy f(−x)=−f(x).",
 "Outside multiplication scales vertically; inside multiplication scales inversely horizontally.",
 "Preserve valid original domain restrictions under every transformation."
 ],
"CommonMistakes":[
 {"Mistake":"Moving a graph left for f(x−3).","WhyWrong":"Input−3 is matched at x three units larger.","Correction":"Shift f(x−3) right three."},
 {"Mistake":"Treating f(−x) as −f(x).","WhyWrong":"Horizontal and vertical reflections affect different coordinates.","Correction":"Use f(−x) for y-axis reflection, −f(x) for x-axis reflection."},
 {"Mistake":"Treating f(3x) as a threefold horizontal stretch.","WhyWrong":"The input attains prior values at one-third its previous x.","Correction":"The horizontal length scale becomes 1/3."},
 {"Mistake":"Classifying a function as even/odd without symmetric domain.","WhyWrong":"f(−x) must be defined whenever f(x) is.","Correction":"Verify both domain symmetry and parity identity."}]}
path=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l05.rich-lesson-v2.json"
assert not path.exists()
path.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U01:L05","ContentVersion":"g12-u01-l05-independent-source-skill-content-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l05-teacher-tryit-11-and-cooldown.json").write_text(json.dumps({"SourceOriginalTryIts":11,"IndependentFirstPartyTryIts":11,
 "StudentRichDoesNotExposeTeacherKey":True,"OriginalSourceFiguresCopied":False,
 "TeacherTryItAnswers":[{"Number":i,"Expected":ans}for i,(_,_,ans)in enumerate(tries,1)],
 "TeacherCoolDown":cool},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint={
"SchemaVersion":1,"Lesson":5,"Unit":1,"SourceSection":"1.5",
"SourceLocalPDFPages":[80,104],"SourceSHA256":source["SourceSha256"],
"SourceOriginalWorkedExampleSkills":20,"SourceOriginalTryItSkills":11,
"SourceOriginalNumberedExercises":81,
"FirstPartyWorkedExampleSkillAlternatives":20,"FirstPartyTryItSkillAlternatives":11,
"FirstPartyCoreConcepts":7,"FirstPartyTableVisuals":2,"IndependentCoolDown":True,
"StudentFirstPartyRichV2Path":path.relative_to(root).as_posix(),
"OriginalSourceExerciseAssessmentAuthoring":"PENDING",
"SourceFigureVisualSkillReview":"PENDING",
"FullPublisherDiagramAndProseParityCertified":False,
"ReadyForStageD":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"UpdatedUtc":datetime.now(timezone.utc).isoformat()}
(base/"u01-l05-abc-checkpoint.json").write_text(json.dumps(checkpoint,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==4 and p["FirstIncompleteLessonOrdinal"]==5
row=p["LessonEntries"][4]
row["ACStatus"]="A_C_IN_PROGRESS"
row["FirstPartyRichV2Path"]=checkpoint["StudentFirstPartyRichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l05-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["Source 81 exercise actions not individually mapped","Original source graph/figure skill comparisons need completion","Teacher review assessment contracts not fully authored or runtime bound"]
p["UpdatedUtc"]=checkpoint["UpdatedUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_L05_FIRSTPARTY_20_WORKED_11_TRYIT_7_CORE_CONCEPTS_AND_INDEPENDENT_COOLDOWN_AUTHORED")
