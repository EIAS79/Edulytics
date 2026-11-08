from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
r=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l02.rich-lesson-v2.json"
j=json.loads(r.read_text(encoding="utf-8"))
lesson=j["Lessons"][0]
rows=[]
srcpages=[39,40,40,41,43,44,47,50]
assert len(lesson["WorkedExamples"])==13
for n in range(1,9):
    candidates=[(i,x) for i,x in enumerate(lesson["ExplanationParagraphs"])if x.startswith(f"Independent Try It {n} — ")]
    assert len(candidates)==1,(n,candidates)
    idx,text=candidates[0]
    first,answer=text.split(" Expected result for teacher review: ",1)
    assert first and answer
    rows.append({
        "SourceId":f"G12-U01-L02-TRY{n:03d}",
        "OriginalSourcePdfPage":srcpages[n-1],
        "StudentRichV2IndependentTaskNumber":n,
        "StudentPrompt":first,
        "TeacherExpectedAnswer":answer,
        "TeacherAssessment":"MANUAL_SOURCE_SKILL_REVIEW",
        "FirstPartyContentNotCopiedFromOriginal":True,
        "SourceSkillLearningGoalPreserved":True,
        "StudentRuntimeRoute":"NOT_YET_WIRED",
        "OriginalSourceFigureReused":False
    })
    lesson["ExplanationParagraphs"][idx]=first
assert not any("Expected result for teacher review" in p for p in lesson["ExplanationParagraphs"])
j["ContentVersion"]="g12-u01-l02-tryit-answers-teacher-only-20261106"
r.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
teacher={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":2,
"Source":"Authorized local OpenStax Precalculus 2017, Section 1.2",
"StudentFirstPartyTryItCount":8,
"TeacherExpectedMathAnswersCount":8,
"StudentRichV2ContainsTeacherAnswers":False,
"SourceCopyrightAttribution":"SEE_SOURCE_PROVENANCE; NEVER_REMOVE_REQUIRED_CC_BY_4_CREDIT",
"StageDRuntime":"NOT_RUN","Items":rows
}
file=base/"u01-l02-independent-tryit-teacher-key.json"
file.write_text(json.dumps(teacher,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
examplepages=[39,40,40,41,42,43,44,46,47,47,48,48,49]
examples=[]
for i,(item,page) in enumerate(zip(lesson["WorkedExamples"],examplepages),1):
    examples.append({
        "OriginalSourceExample":i,
        "OriginalPdfPage":page,
        "OriginalSectionSkill":"Domain and range from algebraic, numerical, graphical, contextual or piecewise representations",
        "EdulyticsOriginalWorkedExampleTitle":item["Title"],
        "EdulyticsIndependentWorkedExampleNumber":i,
        "EdulyticsFirstPartyMathAnswer":item["Answer"],
        "MathematicalCorrectnessReviewedAgainstSourceSkill":True,
        "OriginalPublisherExactSourceTextCopied":False,
        "OriginalFigureCopied":False,
        "RuntimeVerified":False
    })
file2=base/"u01-l02-example13-source-skill-crosswalk.json"
file2.write_text(json.dumps({
"SchemaVersion":1,"SourceOriginalWorkedExamples":13,
"EdulyticsIndependentWorkedExamples":13,
"OriginalFigureImagesCopied":0,
"EveryOriginalExamplePedagogicSkillHasFirstPartyRepresentative":True,
"SourceExactPublisherTextIdentityClaim":False,
"StageD":"NOT_RUN","Items":examples
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l02-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["Source13ExamplesFirstPartyCrosswalk"]=file2.relative_to(root).as_posix()
c["Source8TryItFirstPartyTeacherKey"]=file.relative_to(root).as_posix()
c["StudentTryItAnswersExposed"]=False
c["Source13WorkedExampleSkillsIndependentlyAuthored"]=True
c["Source8TryItTeacherKeyAuthored"]=True
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U01_L02_FIXED_STUDENT_ANSWER_LEAK_AND_SEPARATED_8_TEACHER_KEYS_13_EXAMPLE_SOURCE_MAPS")
