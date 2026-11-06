"""Refine only runtime-contract additions using independently authored grade-11 cooldowns.
The lesson's existing paragraphs and 2 original worked examples stay unchanged.
Does NOT claim external source parity or automatic grading of written work.
"""
from pathlib import Path
import json
root=Path(__file__).resolve().parents[2]
base=Path("artifacts/curriculum-delivery/US-CCSS-MATH/G11/")
rows=json.loads((root/base/"abc-independent-course-ledger.json").read_text(encoding="utf8"))["LessonEntries"]
changed=0; examples=0; concepts=0
for row in rows:
    assessment=json.loads((root/row["AssessmentPath"]).read_text(encoding="utf8"))
    cooldown=assessment.get("FirstPartyCoolDown") or assessment.get("IndependentCoolDown")
    path=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2"/(
        f'us-ccss-hs-alg2-u{row["Unit"]:02d}-l{row["Lesson"]:02d}.rich-lesson-v2.json')
    doc=json.loads(path.read_text(encoding="utf8"))
    lesson=doc["Lessons"][0]
    if not isinstance(cooldown,dict) or not cooldown.get("Prompt") or not cooldown.get("Expected"):
        continue
    # Replace the generic extra error-check with an independent authored actual math prompt.
    if lesson["WorkedExamples"][-1]["Title"].startswith("Independent error-check of "):
        first=lesson["WorkedExamples"][0]
        lesson["WorkedExamples"][-1]={
            "Title":"Independent source-aligned Edulytics cool-down walkthrough",
            "Question":cooldown["Prompt"],
            "Method":"Apply the definition and procedure from this lesson, then test the answer against the original constraints. Written interpretation remains teacher-reviewed.",
            "Steps":[
                "Identify the given quantities, domain and requested output in the new problem: "+cooldown["Prompt"],
                "Select and apply the relevant mathematical rule demonstrated earlier: "+first["Method"]+" "+first["Steps"][0],
                "Verify the result with a second substitution or interpretation; the authored solution is: "+cooldown["Expected"]
            ],
            "Answer":cooldown["Expected"],
            "Check":"Independent first-party solution; a teacher verifies the reasoning and any diagrams or explanations."
        }
        examples+=1;changed+=1
    # A third key concept should address an actual goal rather than duplicate the first example.
    if lesson["KeyConcepts"][-1]["Title"].startswith("Validate ") and assessment.get("LearningGoals"):
        goal=assessment["LearningGoals"][-1]
        lesson["KeyConcepts"][-1]={
            "Title":"Apply the learning goal to independent practice",
            "Definition":goal,
            "Rule":"For a new set of inputs, use the lesson's model, show the intermediate steps and check the allowed domain, units and interpretation.",
            "Example":cooldown["Prompt"]+" Answer: "+cooldown["Expected"]
        }
        concepts+=1;changed+=1
    path.write_text(json.dumps(doc,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
print("G11_INDEPENDENT_COOLDOWN_RUNTIME_CONTENT_REFINEMENT",{"substitutedDistinctExamples":examples,"goalDerivedConcepts":concepts,"filesEdited":changed})
