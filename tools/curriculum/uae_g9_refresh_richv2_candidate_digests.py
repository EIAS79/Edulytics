#!/usr/bin/env python3
"""Refresh only SHA-256 evidence of 82 independently authored G9 RichV2 files.

The old candidate manifest remains a NO_GO for publisher literal 1:1 parity.
This tool must not authorize source rights, deployment, student certification,
or the separate owner-first-party scoped importer. Dry-run unless --apply.
"""
import argparse
import hashlib
import json
from pathlib import Path

ROOT=Path(__file__).resolve().parents[2]
MANIFEST=ROOT/"artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-82-production-import-candidate-NO_GO.json"

def main(apply):
    old=MANIFEST.read_text(encoding="utf-8")
    doc=json.loads(old)
    assert doc["expectedCanonicalLessons"]==82
    assert len(doc["items"])==82
    assert doc["allowProductionWrites"] is False
    assert doc["productionImportExecuted"] is False
    assert doc["candidateStageDAcademicSourceParityVerified"] is False
    altered=[]
    for ordinal,row in enumerate(doc["items"],1):
        assert row["ordinal"]==ordinal
        p=ROOT/row["richV2RelativePath"]
        raw=p.read_bytes()
        rich=json.loads(raw)
        assert rich["SchemaVersion"]==1 and rich["PackCode"]=="UAE-MOE-MATH"
        assert len(rich["Lessons"])==1
        lesson=rich["Lessons"][0]
        assert lesson["LessonCode"]==row["lessonCode"]
        assert lesson["Title"]==row["runtimeDisplayArabicTitle"]
        sha=hashlib.sha256(raw).hexdigest()
        if row["richV2Sha256"]!=sha:
            altered.append(row["lessonCode"])
            row["richV2Sha256"]=sha
    if apply:
        MANIFEST.write_text(json.dumps(doc,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(json.dumps({"status":"G9_RICHV2_CANDIDATE_DIGESTS_RECONCILED",
        "applied":apply,"changed":len(altered),"total":82,
        "sourceLiteralParity":"UNKNOWN",
        "allowProductionWrites":False},indent=2))
if __name__=="__main__":
    parser=argparse.ArgumentParser()
    parser.add_argument("--apply",action="store_true")
    args=parser.parse_args()
    main(args.apply)
