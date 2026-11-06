#!/usr/bin/env python3
"""Conservative source activity-to-checkpoint index. NO semantic equivalence asserted."""
from pathlib import Path
import json
import re
import hashlib
import datetime

ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G10"
INVENTORY=json.loads((BASE/"g10-local-source-element-inventory-20261106.json").read_text(encoding="utf-8"))
LEDGER=json.loads((ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/GEO/GEO_ABC_INDEPENDENT_COURSE_LEDGER_20261030.json").read_text(encoding="utf-8"))
BLUEPRINT=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json").read_text(encoding="utf-8"))
out=[]
for source,entry,lesson in zip(INVENTORY["Lessons"],LEDGER["LessonEntries"],BLUEPRINT["Lessons"]):
    assert source["lessonCode"]==entry["LessonCode"]==lesson["LessonCode"]
    assert source["ordinal"]==entry["Ordinal"]==lesson["SortOrder"]
    cp=json.loads((ROOT/entry["SourceCheckpoint"]).read_text(encoding="utf-8"))
    assessment=json.loads((ROOT/entry["AssessmentPath"]).read_text(encoding="utf-8"))
    headings=source["documents"]["TeacherLesson"]["headings"]
    activities=[]
    for h in headings:
        if h["tag"]=="h2":
            match=re.match(r"^(\d+\.\d+):",h["label"])
            if match and match.group(1) not in activities:
                activities.append(match.group(1))
    practice_headings=source["documents"]["StudentPractice"]["headings"]
    practice=[h["label"] for h in practice_headings if re.match(r"^Problem\s+\d+",h["label"],re.I)]
    keys=("SourceTaskCrosswalk","SourceCrosswalk","SourceToRichV2","SourceComparisonA")
    mapped=[]
    for origin,doc in (("checkpoint",cp),("assessment",assessment)):
        for key in keys:
            for row in doc.get(key,[]) if isinstance(doc.get(key),list) else []:
                if not isinstance(row,dict):continue
                for field in ("Activity","Source","SourceActivity","SourceTask","ElementId"):
                    raw=str(row.get(field,""))
                    match=re.search(r"(?<!\d)(\d+\.\d+)(?!\d)",raw)
                    if match:
                        mapped.append({"source":origin,"field":key,"activityId":match.group(1)})
                        break
    seen={m["activityId"] for m in mapped}
    missing=[x for x in activities if x not in seen]
    out.append({
        "lessonCode":entry["LessonCode"],
        "ordinal":entry["Ordinal"],
        "unitNumber":lesson["UnitNumber"],
        "sourceHtmlSnapshotId":INVENTORY["SourceSnapshotId"],
        "sourceActivityIds":activities,
        "teacherActivityCount":len(activities),
        "documentedSourceActivityIds":sorted(seen),
        "mappedByIdentifierCount":len(set(activities)&seen),
        "unmappedSourceActivityIds":missing,
        "studentPracticeProblemCount":len(practice),
        "teacherSourceVisualReferenceCount":source["documents"]["TeacherLesson"]["visualAssetCount"],
        "studentSourceVisualReferenceCount":source["documents"]["StudentLesson"]["visualAssetCount"],
        "practiceSourceVisualReferenceCount":source["documents"]["StudentPractice"]["visualAssetCount"],
        "checkpointEvidence":entry["SourceCheckpoint"],
        "assessmentEvidence":entry["AssessmentPath"],
        "semanticMatchStatus":"NOT_PROVEN",
        "assetRightsStatus":"NOT_PROVEN",
        "runtimeElementStatus":"NOT_PROVEN",
        "method":"Heading identifier comparison only; duplicate mappings cannot substitute for semantic fidelity",
    })
summary={
    "schemaVersion":1,"sourceSnapshotId":INVENTORY["SourceSnapshotId"],
    "course":"US-CCSS-MATH Geometry Grade 10",
    "method":"Machine-extracted headings plus existing A-C map identifiers; NOT semantic parity",
    "lessonCount":len(out),
    "lessonsWithNoMissingActivityIds":sum(not x["unmappedSourceActivityIds"] for x in out),
    "lessonsWithMissingActivityIds":sum(bool(x["unmappedSourceActivityIds"]) for x in out),
    "sourceTeacherActivityIdCount":sum(x["teacherActivityCount"] for x in out),
    "sourceTeacherActivityIdsDocumentedInCrosswalk":sum(x["mappedByIdentifierCount"] for x in out),
    "practiceProblemsSourceHeadingCount":sum(x["studentPracticeProblemCount"] for x in out),
    "sourceParity":"UNKNOWN",
    "rightsStatus":"NOT_VERIFIED",
    "lessons":out
}
outfile=BASE/"g10-source-activity-crosswalk-diagnostics-20261106.json"
outfile.write_text(json.dumps(summary,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print(json.dumps({k:v for k,v in summary.items() if k!="lessons"},indent=2))
print("FIRST_MISSING",[(x["ordinal"],x["unmappedSourceActivityIds"]) for x in out if x["unmappedSourceActivityIds"]][:16])
