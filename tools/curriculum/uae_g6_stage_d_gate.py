#!/usr/bin/env python3
"""UAE Grade 6 Arabic Stage D diagnostic gates.
Checks are separate: technical consistency != source parity != authenticated live release.
No network, production DB writes, or owner approval fabrication.
"""
import argparse
import hashlib
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
ART = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G6"
OUT = ROOT / "artifacts/uae-g6-stage-d"
CONTENT = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g6-general-ar128-student-local-v1.lesson-content-pack.json"
BLUEPRINT = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g6-general-ar128-student-local-v1.lesson-blueprint.json"
VIEW = ROOT / "src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
UNIT_COUNTS = [11, 11, 10, 11, 12, 12, 10, 10, 11, 14, 7, 9]
TOTAL = sum(UNIT_COUNTS)


def load(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def report(phase, findings, *, notes=None, metrics=None):
    OUT.mkdir(parents=True, exist_ok=True)
    status = "PASS" if not findings else "FAIL"
    payload = {
        "phase": phase,
        "status": status,
        "errors": sorted(set(findings)),
        "candidateSha": os.environ.get("GITHUB_SHA"),
        "testedRef": os.environ.get("GITHUB_REF"),
        "actionsRunId": os.environ.get("GITHUB_RUN_ID"),
        "academicSourceParity": "UNKNOWN",
        "releaseGo": False,
        "renderLiveVerified": False,
        "sourceLock": "PENDING",
        "grade": 6,
        "curriculumCode": "UAE-MOE-MATH",
        "technicalIndexedLessons": TOTAL,
        "notes": notes or [],
        "metrics": metrics or {},
    }
    dest = OUT / f"grade6-{phase}-gate.json"
    dest.write_text(json.dumps(payload, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print(json.dumps({"phase": phase, "status": status, "errors": findings,
                      "output": str(dest)}, ensure_ascii=False))
    return int(bool(findings))


def structural():
    errors, notes = [], []
    if TOTAL != 128:
        errors.append("G6_UNIT_COUNTS_DO_NOT_SUM_TO_128")
    try:
        blue = load(BLUEPRINT)
        pack = load(CONTENT)
        progress = load(ART / "full-grade-progress.json")
        index = load(ART / "source-index.json")
    except (FileNotFoundError, ValueError) as e:
        return report("structural", ["MISSING_OR_INVALID_REQUIRED_JSON:" + str(e)])
    b = blue.get("Lessons", [])
    c = pack.get("Lessons", [])
    x = index.get("sourceTocLessons", [])
    codes = [z.get("LessonCode") for z in b]
    content_codes = [z.get("LessonCode") for z in c]
    if len(b) != TOTAL or len(c) != TOTAL or len(x) != TOTAL:
        errors.append(f"REGISTERED_SCOPE_DIFFERS:{len(b)}:{len(c)}:{len(x)}")
    if len(set(codes)) != TOTAL or len(set(content_codes)) != TOTAL or codes != content_codes:
        errors.append("G6_BLUEPRINT_CONTENT_IDENTITY_OR_ORDER_MISMATCH")
    if any(not str(code).startswith("PED:UAE-MOE-MATH:L6:GENERAL:") for code in codes):
        errors.append("G6_LESSON_CODE_PATHWAY_MISMATCH")
    if pack.get("AcademicLanguage") != "ar":
        errors.append("G6_ACADEMIC_LANGUAGE_NOT_AR")
    if pack.get("Status") != "Published" or not pack.get("ReviewedBy") or not pack.get("ReviewEvidence"):
        errors.append("G6_OWNER_APPROVED_ARABIC_PACK_NOT_PUBLISHED")
    # Accepted VersionCode is the existing database persistence key, NOT
    # a claim of original 2026-2027 textbook edition parity.
    accepted = load(ROOT / "src/Edulytics.Core/Curriculum/Packs/uae-moe-math.curriculum-pack.json").get("VersionCode")
    if blue.get("VersionCode") != accepted or pack.get("VersionCode") != accepted:
        errors.append("G6_CURRICULUM_PERSISTENCE_VERSION_DRIFT")
    # Only one Grade 6 General blueprint/content resource is allowed.
    # Reject duplicates or accidental reintroductions in either registry.
    for folder, expected in [
        (BLUEPRINT.parent, BLUEPRINT),
        (CONTENT.parent, CONTENT),
    ]:
        candidates = list(folder.glob("uae-g6-general-*.lesson-*.json"))
        unexpected = [candidate.name for candidate in candidates if candidate != expected]
        if unexpected or len(candidates) != 1:
            errors.append("G6_COMPETING_GENERAL_PACK_RESOURCES:" +
                          ",".join(sorted(unexpected)))
    if blue.get("Pathway", "").lower() not in ("general", "common"):
        errors.append("G6_BLUEPRINT_PATHWAY_WRONG")
    if any(not any(t.get("CultureCode") == "ar" and t.get("Title") and
                       t.get("Explanation") for t in lesson.get("Translations", []))
           for lesson in c):
        errors.append("G6_CANONICAL_ARABIC_LESSON_MISSING")
    if progress.get("closedLessons") != TOTAL or progress.get("firstIncompleteLessonOrdinal") != 129:
        errors.append("G6_A_C_HANDOFF_COUNT_MISMATCH")
    if len(progress.get("unitStates", [])) != len(UNIT_COUNTS) or [
        u.get("closed") for u in progress.get("unitStates", [])
    ] != UNIT_COUNTS:
        errors.append("G6_UNIT_STATE_COUNT_MISMATCH")
    if len(list(ART.glob("lesson-*-stage-a-c-checkpoint.json"))) != TOTAL:
        errors.append("G6_128_A_C_CHECKPOINTS_INCOMPLETE")
    rich_dir = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
    rich_files = list(rich_dir.glob("uae-g6-general-*-ar.rich-lesson-v2.json"))
    rich_codes = []
    for f in rich_files:
        try:
            rich_codes += [item["LessonCode"] for item in load(f)["Lessons"]]
        except (KeyError, ValueError, FileNotFoundError):
            errors.append("G6_INVALID_RICH_V2_JSON:" + f.name)
    if len(rich_codes) != TOTAL or set(rich_codes) != set(codes):
        errors.append(f"G6_RICH_V2_SCOPE_MISMATCH:{len(rich_codes)}")
    extra = load(ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-practice-target-rules.v1.json")
    for rule in extra.get("rules", []):
        for pattern in rule.get("codePatterns", []):
            if pattern.startswith("^PED:UAE-MOE-MATH:L6:GENERAL:"):
                literal = pattern.removeprefix("^").removesuffix("$")
                if literal not in set(codes):
                    errors.append("G6_NONCANONICAL_SUPPLEMENTAL_TARGET:" + str(rule.get("id")))
    markup = VIEW.read_text(encoding="utf-8")
    for marker in ("isUaeGrade6Arabic", "uae-g6-original-arabic-lesson",
                   'Model.RichContent?.Visuals.Count > 0'):
        if marker not in markup:
            errors.append("G6_STUDENT_ARABIC_ROUTE_MISSING:" + marker)
    g5 = load(ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g5-general-t1-ogl-v1.lesson-content-pack.json")
    if g5.get("AcademicLanguage") != "ar":
        errors.append("G5_ARABIC_PACK_LANGUAGE_REGRESSION")
    notes.append("Source/edition parity is checked separately and does not inherit a structural PASS.")
    return report("structural", errors, notes=notes, metrics={
        "blueprintLessons": len(b), "contentLessons": len(c), "richLessons": len(rich_codes),
        "indexedSourceItems": len(x), "aCCheckpoints": len(list(ART.glob("lesson-*-stage-a-c-checkpoint.json"))),
        "arabicPackLanguage": pack.get("AcademicLanguage"),
    })



def source():
    # Owner-approved educational equivalence does not certify verbatim source parity.
    errors=[]
    ap=ART / "grade6-owner-academic-approval.json"
    handoff=ART / "grade6-a-c-handoff.md"
    if not ap.is_file() or not handoff.is_file():
        return report("source", ["G6_OWNER_APPROVAL_OR_HANDOFF_MISSING"])
    approved=load(ap)
    progress=load(ART / "full-grade-progress.json")
    index=load(ART / "source-index.json")
    bp=load(BLUEPRINT)
    pack=load(CONTENT)
    required={
        "curriculumCode":"UAE-MOE-MATH","grade":6,"pathway":"General",
        "language":"ar","scopeIndexedLessons":128,"scopeUnits":12,
        "academicApprovalAuthority":"PLATFORM_OWNER",
        "ownerDecision":"APPROVED_FOR_PUBLICATION_AS_EDULYTICS_ORIGINAL_EDUCATIONAL_EQUIVALENCE",
        "productionApproval":True,"sourceEditionIdentityClaim":"NONE",
        "approvedPublicationContentStatus":"Published",
        "originalTextbookExerciseNumeratorDenominator":"NOT_INDEPENDENTLY_VERIFIED",
    }
    for k,v in required.items():
        if approved.get(k)!=v:errors.append("G6_OWNER_APPROVAL_INVALID:"+k)
    if len(pack.get("Lessons",[]))!=128 or len(bp.get("Lessons",[]))!=128:
        errors.append("G6_OWNER_APPROVAL_SCOPE_DRIFT")
    if len(index.get("sourceTocLessons",[]))!=128 or progress.get("closedLessons")!=128:
        errors.append("G6_INDEX_OR_A_C_SCOPE_DRIFT")
    def canonical_json_sha256(path):
        obj = json.loads(path.read_text(encoding="utf-8-sig"))
        payload = json.dumps(obj, ensure_ascii=False, sort_keys=True,
                             separators=(",", ":")).encode("utf-8")
        return hashlib.sha256(payload).hexdigest()
    for name,path in [
        ("sourceIndexSha256",ART / "source-index.json"),
        ("contentPackSha256",CONTENT),
        ("blueprintSha256",BLUEPRINT)]:
        if approved.get(name)!=canonical_json_sha256(path):
            errors.append("G6_SOURCE_FINGERPRINT_DRIFT:"+name)
    rich_dir=ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
    rich=list(rich_dir.glob("uae-g6-general-*-ar.rich-lesson-v2.json"))
    if len(rich)!=128:errors.append("G6_RICH_VISUAL_SCOPE_DRIFT")
    invalid=[
        p.name for p in rich if
        "http://" in p.read_text(encoding="utf-8") or
        "https://" in p.read_text(encoding="utf-8") or
        any(ext in p.read_text(encoding="utf-8").lower()
            for ext in (".png",".jpg",".jpeg",".webp"))
    ]
    if invalid or approved.get("noExternalRichVisualImageRefsVerified") is not True:
        errors.append("G6_EXTERNAL_VISUAL_MEDIA_REFERENCE_FOUND")
    if not (ART / "lesson-088-atomic-source-coverage.json").is_file():
        errors.append("G6_DISCLOSED_SOURCE_GAP_MISSING")
    checkpoints=list(ART.glob("lesson-*-stage-a-c-checkpoint.json"))
    if len(checkpoints)!=128:
        errors.append("G6_A_C_CHECKPOINTS_INCOMPLETE")
    unverified=sum(
        1 for p in checkpoints
        if not isinstance(load(p).get("sourceElementCount"),int)
        or not isinstance(load(p).get("verifiedEquivalentCount"),int))
    return report("source",errors,notes=[
        "Owner has explicitly approved Edulytics first-party educational equivalence and publication.",
        "PASS is not a claim of exact source question/image or 2026-2027 edition parity.",
        "The source gap in Unit 9 remains disclosed, not silently claimed inspected.",
        "No third-party image/media references in 128 Rich V2 visual model resources.",
        "Independent .NET, authenticated HTTPS/DB, school roles and browser RTL CI are required.",
    ],metrics={
        "approvalMode":"OWNER_APPROVED_EDUCATIONAL_EQUIVALENCE",
        "exactTextbookParity":"UNKNOWN",
        "unverifiedSourceQuestionDenominators":unverified,
        "firstPartyVisualResources":len(rich),
        "externalRichVisualMediaReferences":len(invalid)
    })

def final():
    results = {job: os.getenv(job.upper() + "_RESULT", "MISSING")
               for job in ("structural", "source", "math", "e2e")}
    errors = [f"{job.upper()}_NOT_SUCCESS:{value}" for job, value in results.items()
              if value != "success"]
    expected_sha = os.environ.get("GITHUB_SHA")
    for name in ["structural", "source", "authenticated-e2e", "arabic-visual"]:
        filename = OUT / f"grade6-{name}-gate.json"
        if not filename.exists():
            errors.append(f"CURRENT_RUN_GATE_MISSING:{name}")
            continue
        data = load(filename)
        if data.get("candidateSha") != expected_sha or data.get("status") != "PASS":
            errors.append(f"CURRENT_SHA_GATE_NOT_VERIFIED:{name}")
    if not list((OUT / "trx").glob("*.trx")):
        errors.append("CURRENT_RUN_DOTNET_TRX_MISSING")
    return report("final", errors, notes=[
        "This is only a fail-closed diagnostics workflow. No merge, Render production writes or real student data.",
        "Owner-approved equivalence is not exact original text parity; third-party media rights remain separate.",
    ], metrics={"jobResults":results})


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", choices=("structural", "source", "final"), required=True)
    a = parser.parse_args()
    raise SystemExit({"structural":structural,"source":source,"final":final}[a.phase]())
