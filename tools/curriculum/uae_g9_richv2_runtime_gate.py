#!/usr/bin/env python3
"""Independent Grade9 RichV2 runtime compatibility diagnosis.

A prepared authoring file is not a successfully embeddable RichV2 artifact.
This audit never writes production content or modifies original lessons.
"""
import argparse
import json
from pathlib import Path
import sys

ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/RichV2"
OUT=ROOT/"artifacts/uae-g9-stage-d/grade9-richv2-runtime-readiness.json"
MINIMUM={"ExplanationParagraphs":2,"KeyConcepts":3,"WorkedExamples":3,
         "CommonMistakes":2,"SummaryPoints":3}

def run():
    files=sorted(BASE.glob("uae-g9-general-*.rich-lesson-v2.json"))
    gaps=[]
    for file in files:
        doc=json.loads(file.read_text(encoding="utf-8"))
        issue=[]
        if doc.get("SchemaVersion")!=1:
            issue.append("SCHEMA_VERSION_NOT_RUNTIME_ONE")
        if doc.get("PackCode")!="UAE-MOE-MATH":
            issue.append("PACK_CODE_NOT_CANONICAL_UAE")
        if len(doc.get("Lessons",[]))!=1:
            issue.append("ONE_LESSON_PER_DOCUMENT_REQUIRED")
        for lesson in doc.get("Lessons",[]):
            if lesson.get("CultureCode")!="ar" or not lesson.get("LessonCode","").startswith("PED:UAE-MOE-MATH:L9:GENERAL:"):
                issue.append("GRADE_PATHWAY_OR_LANGUAGE_WRONG")
            for field,mincount in MINIMUM.items():
                if len(lesson.get(field,[]))<mincount:
                    issue.append(field+"_TOO_SHORT")
            for n,example in enumerate(lesson.get("WorkedExamples",[]),1):
                if len(example.get("Steps",[]))<3:
                    issue.append(f"WORKED_EXAMPLE_{n}_BELOW_THREE_EXPLICIT_STEPS")
                for required in ("Title","Question","Method","Answer"):
                    if not example.get(required):
                        issue.append(f"WORKED_EXAMPLE_{n}_{required}_MISSING")
        if issue:
            gaps.append({"file":file.name,"reasons":issue})
    report={
        "grade":9,"pathway":"General","academicLanguage":"ar",
        "preparedAuthoredFiles":len(files),"expectedCanonicalFiles":82,
        "runtimeReadyRichV2Files":len(files)-len(gaps),
        "runtimeBlockedRichV2Files":len(gaps),
        "sourcePublisherLiteralParity":"UNKNOWN",
        "missingFilesMayNotBeReplacedByGenericEnglish":True,
        "status":"PASS" if len(files)==82 and not gaps else "D_BLOCKED",
        "productionWritten":False,"gaps":gaps
    }
    OUT.parent.mkdir(parents=True,exist_ok=True)
    OUT.write_text(json.dumps(report,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    return report

if __name__=="__main__":
    parser=argparse.ArgumentParser()
    parser.add_argument("--require-ready",action="store_true")
    args=parser.parse_args()
    try:
        result=run()
    except Exception as exc:
        result={"status":"D_BLOCKED","error":f"{type(exc).__name__}: {exc}"}
    print(json.dumps(result,ensure_ascii=True,indent=2))
    sys.exit(1 if args.require_ready and result["status"]!="PASS" else 0)
