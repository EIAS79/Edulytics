#!/usr/bin/env python3
"""Deterministic Grade 8 Arabic 76-lesson semantic fingerprint reconciliation.
Fingerprints represent Edulytics first-party selected curriculum artifacts,
not third-party textbook file hashes, legal clearance or source-item parity.
"""
import argparse
import copy
import datetime
import hashlib
import json
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BLUE = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g8-general-t1-ogl-v1.lesson-blueprint.json"
PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json"
RICH = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
OUT = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G8/grade8-semantic-hash-reconciliation.json"

def sha(obj,domain):
    encoded=json.dumps(obj,ensure_ascii=False,sort_keys=True,separators=(",",":")).encode("utf-8")
    return hashlib.sha256(domain.encode("ascii")+b"\n"+encoded).hexdigest()

def file_json(f):
    return json.loads(f.read_text(encoding="utf-8"))

def calculate(blue,pack):
    if not(len(blue["Units"])==12 and len(blue["Lessons"])==len(pack["Lessons"])==76 and
           blue["LogicalLevel"]==8 and blue["Pathway"]=="General" and pack["AcademicLanguage"]=="ar"):
        raise ValueError("Expected G8 General 12-unit 76-lesson Arabic candidate")
    by_code={x["LessonCode"]:x for x in pack["Lessons"]}
    old_lessons={x["LessonCode"]:x["SemanticSha256"] for x in blue["Lessons"]}
    old_units={x["Number"]:x["SemanticSha256"] for x in blue["Units"]}
    old_graph=blue["SemanticGraphSha256"]
    unique=set()
    lesson_shas={}
    for row in blue["Lessons"]:
        code=row["LessonCode"]
        u,n=row["UnitNumber"],row["LessonNumber"]
        path=RICH / f"uae-g8-general-u{u:02d}-l{n:02d}-ar.rich-lesson-v2.json"
        if code not in by_code or code in unique or not path.is_file():
            raise ValueError(f"Missing/duplicate canonical or rich lesson {code}")
        rich=file_json(path)
        if len(rich.get("Lessons",[]))!=1 or rich["Lessons"][0]["LessonCode"]!=code:
            raise ValueError(f"Rich lesson mismatch: {code}")
        if any(t["CultureCode"]!="ar" for t in by_code[code]["Translations"]):
            raise ValueError(f"English fallback detected in {code}")
        payload={"blueprint":{k:v for k,v in row.items() if k!="SemanticSha256"},
                 "canonical":by_code[code],"richv2":rich}
        digest=sha(payload,"edulytics-uae-g8-general-lesson-v1")
        lesson_shas[code]=digest
        row["SemanticSha256"]=digest
        unique.add(code)
    if len(unique)!=76 or any(":12:05:" in code for code in unique):
        raise ValueError("Wrong or retired lesson code in 76 candidate")
    unit_shas={}
    for row in blue["Units"]:
        u=row["Number"]
        codes=[x["LessonCode"] for x in blue["Lessons"] if x["UnitNumber"]==u]
        if len(codes)!=row["LessonCount"]:
            raise ValueError(f"Unit {u} has wrong count")
        unit_digest=sha({"unit":{k:v for k,v in row.items() if k!="SemanticSha256"},
                         "lessonHashes":[lesson_shas[code] for code in codes]},
                        "edulytics-uae-g8-general-unit-v1")
        row["SemanticSha256"]=unit_digest
        unit_shas[u]=unit_digest
    graph=sha({"packCode":blue["PackCode"],"versionCode":blue["VersionCode"],
               "blueprintCode":blue["BlueprintCode"],"academicLanguage":"ar",
               "logicalLevel":8,"pathway":"General","unitDigests":[unit_shas[i] for i in range(1,13)],
               "lessonCodes":[x["LessonCode"] for x in blue["Lessons"]]},
              "edulytics-uae-g8-general-graph-v1")
    blue["SemanticGraphSha256"]=graph
    return {"oldGraphSha256":old_graph,"newGraphSha256":graph,
            "oldUnitSha256":old_units,"newUnitSha256":unit_shas,
            "changedLessonHashes":sum(old_lessons[code]!=new for code,new in lesson_shas.items()),
            "lessonCount":len(lesson_shas),"unitCount":len(unit_shas),
            "hashAlgorithm":"SHA256(domain + LF + sorted-key compact UTF8 JSON), v1",
            "meaning":"EDULYTICS_FIRST_PARTY_SEMANTIC_SNAPSHOT_NOT_PUBLISHER_PDF_HASH",
            "sourceElementParityCertified":False,"publisherArtworkLicenseCertified":False}

def main(apply,check):
    current=file_json(BLUE)
    revised=copy.deepcopy(current)
    diag=calculate(revised,file_json(PACK))
    identical=revised==current
    if check:
        artifact=file_json(OUT) if OUT.is_file() else {}
        ok=(identical and artifact.get("newGraphSha256")==diag["newGraphSha256"]
            and artifact.get("unitCount")==12 and artifact.get("lessonCount")==76)
        print(json.dumps({"check":"grade8_semantic_hash_reconciliation",
                          "status":"PASS" if ok else "FAIL",
                          "sameComputedHashes":identical,
                          "ledgerMatches":artifact.get("newGraphSha256")==diag["newGraphSha256"],
                          "graph":diag["newGraphSha256"]},ensure_ascii=False))
        return 0 if ok else 1
    if apply:
        BLUE.write_text(json.dumps(revised,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
        OUT.write_text(json.dumps({"schemaVersion":1,"grade":8,"pathway":"General",
                                   "selectedUnitCount":12,"selectedLessonCount":76,
                                   "status":"CALCULATED_FROM_76_CURRENT_EDULYTICS_LESSONS",
                                   **diag},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(json.dumps({"status":"WRITTEN" if apply else "DRY_RUN","sameBefore":identical,
                      "newGraphSha256":diag["newGraphSha256"],
                      "changedLessonHashes":diag["changedLessonHashes"]}))
    return 0

if __name__=="__main__":
    p=argparse.ArgumentParser()
    g=p.add_mutually_exclusive_group()
    g.add_argument("--apply",action="store_true")
    g.add_argument("--check",action="store_true")
    args=p.parse_args()
    sys.exit(main(args.apply,args.check))
