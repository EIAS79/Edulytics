from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
f=base/"u01-l05-source-81-firstparty-question-contracts.json"
r=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l05-firstparty-teacher-led.json"
j=json.loads(f.read_text(encoding="utf-8"))
a=json.loads(r.read_text(encoding="utf-8"))
graph={
33:("An upward parabola has vertex (2,1) and passes through (3,2).","y=(x−2)²+1"),
34:("A downward-opening parabola has vertex (−1,0) and passes through (0,−1).","y=−(x+1)²"),
35:("An upward V-shape has vertex (−2,−3) and passes through (−1,−2).","y=|x+2|−3"),
36:("A root-shaped curve starts at included (−1,−2) and passes through (0,−1), rising rightward.","y=√(x+1)−2"),
37:("A decreasing-to-the-left negative-root curve has included right endpoint (4,0), passes (3,−1), and extends left/down.","y=−√(4−x)"),
38:("An increasing cubic has inflection point (−1,−2) and passes (0,−1).","y=(x+1)³−2"),
39:("An upward V-shape has vertex (1,1) and passes (2,3).","y=2|x−1|+1"),
40:("A downward V-shape has vertex (−3,2) and passes (−2,1).","y=−|x+3|+2"),
41:("A rightward rising square-root curve begins at included (2,3) and passes (3,4).","y=√(x−2)+3"),
42:("A rightward decreasing negative-root curve starts at included (−4,0), passes (−3,−1), and extends right/down.","y=−√(x+4)"),
43:("A narrow upward parabola has vertex (1,−4) and passes (2,−1).","y=3(x−1)²−4"),
44:("A decreasing cubic has inflection point (−1,0) and passes (0,−2).","y=−2(x+1)³"),
45:("An upward V-shape has vertex (2,0) and passes (4,1).","y=(1/2)|x−2|"),
46:("A reciprocal branch graph has asymptotes x=−2,y=1 and passes (−1,2).","y=1/(x+2)+1")
}
assert len(graph)==14
for n,(shape,formula)in graph.items():
 item=j["Items"][n-1]
 assert item["Number"]==n and item["SourceSkillGroup"]=="GRAPH_TO_TOOLKIT_EXPRESSION"
 item["FirstPartyStudentPrompt"]=f"An independent source-skill graph is described by labelled coordinate information: {shape} Reconstruct the graph from the description and determine its exact equation and real domain. Do not assume it is an image of the publisher's original graph."
 item["TeacherExpectedMathematics"]=f"One valid equation is {formula}, with the domain determined by the described toolkit family; teacher checks anchor coordinates and endpoint/asymptote conditions."
 a["FirstPartyStudentQuestions"][n-1]["StudentPrompt"]=item["FirstPartyStudentPrompt"]
 a["TeacherOnlyMathAnswers"][n-1]["TeacherExpectedMath"]=item["TeacherExpectedMathematics"]
f.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
r.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
# No source boilerplate changes needed: this is an evidence-based correction
# after the first-party graph exercises were drafted.
print("G12_L05_14_GRAPH_EXERCISES_NOW_REQUIRE_STUDENT_INFER_FORMULA_FROM_COORDINATES_NO_ANSWER_LEAK")
