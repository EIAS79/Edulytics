#!/usr/bin/env python3
"""UAE Grade 2 fail-closed Stage D diagnostics. No run, no PASS."""
import argparse
import json
import os
from pathlib import Path
import re
import sys

ROOT=Path(__file__).resolve().parents[2]
ART=ROOT/"artifacts/curriculum-delivery/UAE-MOE-MATH/G2"
OUT=ROOT/"artifacts/uae-g2-stage-d"
PACK=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g2-common-t1-ogl-v1.lesson-content-pack.json"
BP=ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g2-common-t1-ogl-v1.lesson-blueprint.json"
MANIFEST=ROOT/"docs/curriculum/UAE_G2_COMMON_OFFICIAL_MANIFEST.json"
FIELDS=("Explanation","KeyConceptsAndRules","WorkedExamples","StepByStepSolutions","CommonMistakes","QuickSummary")
def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))
def structural():
    problems=[]
    m,p,b=read(MANIFEST),read(PACK),read(BP)
    expected=[(f"PED:UAE-MOE-MATH:L2:COMMON:{u['n']:02}:{n:02}",x["ar"])
              for u in m["Units"] for n,x in enumerate(u["ls"],1)]
    codes=[x.get("LessonCode") for x in p["Lessons"]]
    blue=[x.get("LessonCode") for x in b["Lessons"]]
    if len(m["Units"])!=12 or len(expected)!=99 or m.get("LessonCount")!=99:
        problems.append("MANIFEST_99_12_INCONSISTENT")
    if codes!=[x[0] for x in expected] or len(set(codes))!=99:
        problems.append("PACK_ORDER_OR_DUPLICATE")
    if blue!=[x[0] for x in expected] or len(set(blue))!=99:
        problems.append("BLUEPRINT_ORDER_OR_DUPLICATE")
    if p.get("AcademicLanguage")!="ar":
        problems.append("ACADEMIC_LANGUAGE_NOT_AR")
    if b.get("LogicalLevel")!=2 or b.get("Pathway")!="Common":
        problems.append("BLUEPRINT_LEVEL_OR_PATHWAY_WRONG")
    passed=0
    for i, (code,title) in enumerate(expected,1):
        if i>len(p["Lessons"]): break
        row=p["Lessons"][i-1]
        translations=[t for t in row.get("Translations",[]) if t.get("CultureCode")=="ar"]
        if len(translations)!=1:
            problems.append(f"ARABIC_TRANSLATION_MISSING:{i}")
        else:
            ar=translations[0]
            if ar.get("Title")!=title:
                problems.append(f"ARABIC_TITLE_MISMATCH:{i}")
            if any(len(str(ar.get(field,"")).strip())<20 or
                   not re.search(r"[\u0600-\u06ff]",str(ar.get(field,"")))
                   for field in FIELDS):
                problems.append(f"ARABIC_BODY_MISSING_OR_GENERIC:{i}")
        checkpoint=ART/f"lesson-{i}-stage-a-c-checkpoint.json"
        evidence=ART/f"lesson-{i}-source-evidence.md"
        atomic=ART/f"lesson-{i}-atomic-coverage.json"
        if not all(x.is_file() for x in (checkpoint,evidence,atomic)):
            problems.append(f"STAGE_A_C_ARTIFACTS_MISSING:{i}")
        else:
            cp=read(checkpoint)
            if (cp.get("LessonOrdinal")==i and cp.get("LessonCode")==code and
                cp.get("Status")=="READY_FOR_STAGE_D"):
                passed+=1
            else: problems.append(f"STAGE_A_C_CHECKPOINT_INVALID:{i}")
    return problems,{"candidateLessons":len(codes),"blueprintLessons":len(blue),
                     "readySourceScoped":passed,"denominator":99,"units":len(m["Units"]),
                     "runtimeTestResult":"NOT_ASSESSED"}
def source():
    progress=read(ART/"full-grade-progress.json")
    selection=read(ART/"source-selection.json")
    p=read(PACK)
    inconsistencies=[]
    individuallyCertified=0
    for i,row in enumerate(p["Lessons"],1):
        cp=read(ART/f"lesson-{i}-stage-a-c-checkpoint.json")
        a=read(ART/f"lesson-{i}-atomic-coverage.json")
        lhs=cp.get("SourceSha256","")
        rhs=row.get("SourceSha256","")
        if lhs and rhs and lhs.lower()!=rhs.lower():
            inconsistencies.append(i)
        if (a.get("OneToOneExerciseParity")=="INDEPENDENTLY_CERTIFIED" and
            cp.get("2026_2027ExerciseExactParity")=="CERTIFIED"):
            individuallyCertified+=1
    problems=[]
    if selection.get("latestEdition2026_2027")!="AVAILABLE_VERIFIED":
        problems.append("LATEST_2026_27_SOURCE_NOT_AVAILABLE")
    if progress.get("2026_2027ExerciseExactParity")!="CERTIFIED":
        problems.append("2026_27_1_TO_1_PARITY_UNKNOWN")
    if individuallyCertified!=99:
        problems.append("SOURCE_ELEMENT_DENOMINATOR_UNVERIFIED")
    if inconsistencies:
        problems.append("CONTENT_TO_CHECKPOINT_SOURCE_SHA_MISMATCH")
    if p.get("SourceCurriculumPeriod","").startswith("2026-2027"):
        problems.append("METADATA_CLAIMS_NEWER_SOURCE_THAN_LOCAL_EDITION")
    return problems,{"2026_27Numerator":None,"2026_27Denominator":None,
                     "certifiedIndividualLessons":individuallyCertified,
                     "sourceShaInconsistentOrdinals":inconsistencies,
                     "sourceParity":"UNKNOWN"}
def final():
    keys=("STRUCTURAL_JOB_RESULT","SOURCE_JOB_RESULT","DOTNET_JOB_RESULT",
          "AUTHENTICATED_E2E_JOB_RESULT","ARABIC_BROWSER_JOB_RESULT")
    results={key:os.environ.get(key,"NOT_RUN") for key in keys}
    problems=[f"{k}:{v}" for k,v in results.items() if v!="success"]
    problems.append("NO_RENDER_OR_ACADEMIC_RELEASE_FROM_CI_DIAGNOSTICS")
    return problems,{"jobs":results,"releaseStatus":"NO_GO","deployment":"NOT_RUN"}
def main():
    parser=argparse.ArgumentParser()
    parser.add_argument("--phase",choices=("structural","source","final"),required=True)
    phase=parser.parse_args().phase
    problems,measurements={"structural":structural,"source":source,"final":final}[phase]()
    OUT.mkdir(parents=True,exist_ok=True)
    result={"grade":2,"phase":phase,"candidateSha":os.getenv("GITHUB_SHA"),
            "actionsRunId":os.getenv("GITHUB_RUN_ID"),"result":"FAIL" if problems else "PASS",
            "problems":problems,"measurements":measurements,
            "dVerifiedLessons":0,"renderReleaseVerified":False}
    (OUT/f"{phase}-result.json").write_text(
        json.dumps(result,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
    print(json.dumps(result,ensure_ascii=False,indent=2))
    return 1 if problems else 0
if __name__=="__main__":
    sys.exit(main())
