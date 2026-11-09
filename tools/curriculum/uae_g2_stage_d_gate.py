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
PDF_SHA_BY_UNIT={
    **{n:"867b35b6cfa6f2d01b71af47ccda7c8e8a916e964d54face15d4e17ac600335" for n in range(1,5)},
    **{n:"1b04e49007d899041189d2750f70893b1649bf5933f3afeed2e078e897fb5ecb" for n in range(5,9)},
    **{n:"0b888c3918421a411118cfe2b5436b155d4695b6582742c87babd8fe94173784" for n in range(9,13)}
}
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
    titles_by_unit={u["n"]:u["ar"] for u in m["Units"]}
    if len(b.get("Units",[]))!=12 or any(u.get("Title")!=titles_by_unit.get(u.get("Number")) for u in b["Units"]):
        problems.append("BLUEPRINT_UNIT_TITLE_DRIFT_FROM_ARABIC_MANIFEST")
    if any(row.get("UnitTitle")!=titles_by_unit.get(row.get("UnitNumber")) for row in b["Lessons"]):
        problems.append("BLUEPRINT_LESSON_UNIT_TITLE_DRIFT")
    if p.get("AcademicLanguage")!="ar":
        problems.append("ACADEMIC_LANGUAGE_NOT_AR")
    view=(ROOT/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf-8-sig")
    if "uae-g2-original-arabic-lesson" not in view or "isUaePrimaryArabic" not in view:
        problems.append("G2_ARABIC_RTL_LEARNER_VIEW_NOT_GUARDED")
    if b.get("LogicalLevel")!=2 or b.get("Pathway")!="Common":
        problems.append("BLUEPRINT_LEVEL_OR_PATHWAY_WRONG")
    passed=0
    for i, (code,title) in enumerate(expected,1):
        if i>len(p["Lessons"]): break
        row=p["Lessons"][i-1]
        translations=[t for t in row.get("Translations",[]) if t.get("CultureCode")=="ar"]
        if i<=len(b['Lessons']) and b['Lessons'][i-1].get('Title')!=title:
            problems.append(f'BLUEPRINT_ARABIC_TITLE_MISMATCH:{i}')
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
            if (cp.get("LessonOrdinal", cp.get("Ordinal", cp.get("GlobalOrdinal")))==i and cp.get("LessonCode")==code and
                cp.get("Status")=="READY_FOR_STAGE_D"):
                passed+=1
            else: problems.append(f"STAGE_A_C_CHECKPOINT_INVALID:{i}")
    return problems,{"candidateLessons":len(codes),"blueprintLessons":len(blue),
                     "readySourceScoped":passed,"denominator":99,"units":len(m["Units"]),
                     "runtimeTestResult":"NOT_ASSESSED"}
def source():
    """Source-scoped release acceptance, never a 2026-27 item-level parity certificate."""
    progress=read(ART/"full-grade-progress.json")
    selected=read(ART/"source-selection.json")
    approved=read(ART/"grade2-owner-academic-publication-authorization-20261109.json")
    pack=read(PACK)
    problems=[]
    unknown=[]
    mismatched=[]
    partialEvidence=[]
    verified=0
    expected=approved.get("SelectedLocalSourceSha256",{})
    if (approved.get("OwnerDecision")!="ACADEMICALLY_APPROVED_BY_OWNER"
        or not approved.get("OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease")
        or approved.get("Independent2026_2027ElementParity")!="UNKNOWN"
        or approved.get("ThirdPartyAssetClearance")!="NOT_INDEPENDENTLY_CERTIFIED"
        or approved.get("SourceParityIsNotReleaseClaim")!=True):
        problems.append("ACADEMIC_OWNER_LIMITED_RELEASE_AUTHORIZATION_INVALID")
    if selected.get("latestEdition2026_2027")!="UNAVAILABLE_LOCALLY":
        problems.append("SELECTED_SOURCE_SCOPE_CHANGED_REVERIFY")
    if progress.get("A_B_C_Ready")!=99:
        problems.append("ABC_COMPLETENESS_NOT_99")
    if len(pack.get("Lessons",[]))!=99:
        problems.append("G2_PACK_NOT_99")
    pdfs={
        **{i:("01.pdf",expected.get("01.pdf")) for i in range(1,5)},
        **{i:("02-1.pdf",expected.get("02-1.pdf")) for i in range(5,9)},
        **{i:("03.pdf",expected.get("03.pdf")) for i in range(9,13)},
    }
    for i,row in enumerate(pack.get("Lessons",[]),1):
        lesson_code=row.get("LessonCode","")
        try:
            unit=int(lesson_code.split(":")[-2])
            pdf_name, verified_sha=pdfs[unit]
        except (KeyError, ValueError, IndexError, TypeError):
            problems.append(f"SOURCE_FILE_SCOPE_NOT_MAPPED:{i}")
            continue
        actual_sha=row.get("SourceSha256","")
        if not verified_sha or actual_sha.lower()!=verified_sha.lower():
            mismatched.append(i)
            continue
        cp=read(ART/f"lesson-{i}-stage-a-c-checkpoint.json")
        atomic=read(ART/f"lesson-{i}-atomic-coverage.json")
        explicit=[h for h in (cp.get("SourceSha256"),atomic.get("SourceSha256"))
                  if h and isinstance(h,str)]
        if any(h.lower()!=verified_sha.lower() for h in explicit):
            mismatched.append(i)
            continue
        if len(explicit)<2:
            partialEvidence.append(i)
        verified+=1
    if mismatched:
        problems.append("LOCAL_SOURCE_SHA_MISMATCH")
    if verified!=99:
        problems.append("SELECTED_LOCAL_PDF_SHA_COVERAGE_NOT_99")
    if progress.get("2026_2027ExerciseExactParity")!="CERTIFIED":
        unknown.append("2026_27_EXERCISE_1_TO_1_PARITY_UNKNOWN")
    if approved.get("IndependentExerciseDenominator") is None:
        unknown.append("INDEPENDENT_SOURCE_ELEMENT_DENOMINATOR_UNKNOWN")
    if partialEvidence:
        unknown.append("LEGACY_ATOMIC_OR_CHECKPOINT_SOURCE_SHA_NOT_REPEATED_EVERYWHERE")
    return problems,{
        "selectedLocalEditionDigestCoverage":verified,
        "sourceLessonCount":99,
        "ownerApprovedLimitedSourceScope":True,
        "sourceReleaseGate":"PASS_WITH_EXPLICIT_UNKNOWN_LATEST_PARITY" if not problems else "FAIL",
        "latest2026_27IndependentParity":"UNKNOWN",
        "latest2026_27ExerciseElementNumerator":None,
        "latest2026_27ExerciseElementDenominator":None,
        "localSourcePdfShaMismatches":mismatched,
        "legacyMetadataNotRepeatingSha":partialEvidence,
        "limitationsNotClaimedAsPassed":unknown
    }


def final():
    """Evidence-bound technical decision; a CI job result alone is insufficient."""
    import xml.etree.ElementTree as ET
    keys=("STRUCTURAL_JOB_RESULT","SOURCE_JOB_RESULT","DOTNET_JOB_RESULT",
          "AUTHENTICATED_E2E_JOB_RESULT","ARABIC_BROWSER_JOB_RESULT")
    results={key:os.environ.get(key,"NOT_RUN") for key in keys}
    problems=[f"{key}:{value}" for key,value in results.items() if value!="success"]
    sha=os.getenv("GITHUB_SHA")
    run=os.getenv("GITHUB_RUN_ID")
    if not sha or not run:
        problems.append("EXACT_CI_SHA_OR_RUN_ID_MISSING")
    def artifact_json(relative):
        path=OUT/relative
        if not path.is_file():
            problems.append("EXACT_RUN_ARTIFACT_MISSING:"+relative)
            return {}
        try:
            obj=read(path)
        except (OSError,ValueError):
            problems.append("INVALID_EXACT_RUN_JSON:"+relative)
            return {}
        if obj.get("candidateSha")!=sha:
            problems.append("ARTIFACT_COMMIT_NOT_CURRENT_SHA:"+relative)
        return obj
    for name in ("structural","source"):
        a=artifact_json(f"gates/{name}-result.json")
        if a.get("phase")!=name or a.get("result")!="PASS" or a.get("actionsRunId")!=run:
            problems.append("INVALID_CURRENT_RUN_SOURCE_OR_STRUCTURAL:"+name)
    h=artifact_json("runtime/uae-g2-stage-d-authenticated-99-lessons.json")
    records=h.get("lessons",[])
    if (h.get("lessonPageCount")!=99 or
        h.get("correctAttemptPersistenceCount")!=99 or
        h.get("wrongAttemptPersistenceCount")!=99 or
        h.get("malformedAttemptRejectionCount")!=99 or
        not h.get("crossSchoolAccessDenied") or
        h.get("academicLanguage")!="ar" or
        len(records)!=99 or
        len({row.get("lessonCode") for row in records})!=99):
        problems.append("G2_RUNTIME_99_297_AUTHORIZATION_OR_PERSISTENCE_NOT_VERIFIED")
    for i,row in enumerate(records,1):
        if (row.get("lessonOrdinalInCanonicalSortedScope")!=i
            or row.get("correct",{}).get("status")!="SELF_CONSISTENT_GRADE_PERSISTED"
            or row.get("wellFormedWrong",{}).get("status")!="WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED"
            or row.get("malformed",{}).get("status")!="MALFORMED_ANSWER_REJECTED_AND_PERSISTED"):
            problems.append(f"G2_LESSON_PRACTICE_DID_NOT_PASS:{i}")
    browser=artifact_json("browser/report.json")
    pages=browser.get("pages",[])
    if (browser.get("rtlDesktopLessons")!=99 or
        browser.get("rtlMobileLessons")!=99 or
        browser.get("studentAuthenticated") is not True or
        browser.get("screenshotsWritten",0)<8 or
        len(pages)!=198 or len({(p.get("lessonCode"),p.get("viewportWidth"))
                               for p in pages})!=198):
        problems.append("G2_CHROMIUM_99_DESKTOP_MOBILE_RUN_NOT_VERIFIED")
    trx=OUT/"trx"
    for filename in ("grade2-math.trx","grade1-us-regression.trx"):
        file=trx/filename
        if not file.is_file():
            problems.append("DOTNET_TRX_FROM_EXACT_RUN_MISSING:"+filename)
            continue
        try:
            root=ET.parse(file).getroot()
            counters=next((node for node in root.iter() if node.tag.endswith("Counters")),None)
            if counters is None or int(counters.get("total","0"))<1 or int(counters.get("failed","0"))!=0:
                problems.append("DOTNET_TRX_TESTS_NOT_GREEN:"+filename)
        except (ET.ParseError,OSError,ValueError):
            problems.append("DOTNET_TRX_INVALID:"+filename)
    owner=read(ART/"grade2-owner-academic-publication-authorization-20261109.json")
    if (owner.get("OwnerDecision")!="ACADEMICALLY_APPROVED_BY_OWNER"
        or not owner.get("OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease")):
        problems.append("GRADE2_OWNER_AUTHORIZATION_MISSING")
    return problems,{
        "jobs":results,
        "candidateSha":sha,"runId":run,
        "technicalStageD":"PASS" if not problems else "NO_GO",
        "academicOwnerReleaseDecision":"APPROVED_FIRST_PARTY_WITH_SOURCE_LIMITATIONS",
        "independentSourceExerciseParity":"UNKNOWN",
        "thirdPartyAssetRights":"NOT_INDEPENDENTLY_CERTIFIED",
        "mergeAllowed":not problems,
        "renderPublished":"NOT_VERIFIED_IN_CI"
    }


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
