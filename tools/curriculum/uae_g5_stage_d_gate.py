#!/usr/bin/env python3
"""Fail-closed UAE Grade 5 Stage D CI diagnostics; no academic/release approval."""
import argparse
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
EVID = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G5"
OUT = ROOT / "artifacts/uae-g5-stage-d"
BLUE = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g5-general-t1-ogl-v1.lesson-blueprint.json"
RICH = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
COUNTS = [9, 10, 13, 6, 14, 11, 8, 13, 12, 7, 11, 13]
TOTAL = sum(COUNTS)


def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def report(phase, status, errors, **extra):
    OUT.mkdir(parents=True, exist_ok=True)
    data = {
        "phase": phase, "status": status, "errors": sorted(set(errors)),
        "candidateSha": os.getenv("GITHUB_SHA"), "testedRef": os.getenv("GITHUB_REF"),
        "actionsRunId": os.getenv("GITHUB_RUN_ID"), "actionsRunAttempt": os.getenv("GITHUB_RUN_ATTEMPT"),
        "grade": 5, "framework": "UAE-MOE-MATH", "provisionalLessons": TOTAL,
        "releaseApproved": False, "renderDeployed": False, **extra,
    }
    file = OUT / f"grade5-{phase}-gate.json"
    file.write_text(json.dumps(data, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print(json.dumps({"status": status, "phase": phase, "errors": errors,
                      "evidence": str(file)}, ensure_ascii=False))
    return 0 if status == "PASS" else 1


def audit_structural():
    errors = []
    expected = []
    checked = 0
    mapped = 0
    offset = 0
    for unit, count in enumerate(COUNTS, 1):
        for lesson in range(1, count + 1):
            ordinal = offset + lesson
            code = f"PED:UAE-MOE-MATH:L5:GENERAL:{unit:02d}:{lesson:02d}"
            expected.append(code)
            for suffix in ("stage-a-c-checkpoint", "atomic-source-coverage"):
                file = EVID / f"lesson-{ordinal:03d}-{suffix}.json"
                if not file.exists():
                    errors.append(f"MISSING:{file.name}")
                    continue
                item = read(file)
                if item.get("LessonOrdinal") != ordinal or item.get("LessonCode") != code:
                    errors.append(f"IDENTITY:{file.name}")
                if suffix.startswith("stage"):
                    checked += 1
                else:
                    mapped += 1
        offset += count
    blueprint = read(BLUE)
    official_codes = [x.get("LessonCode") for x in blueprint.get("Lessons", [])]
    if len(official_codes) != TOTAL or set(official_codes) != set(expected):
        errors.append("BLUEPRINT_IDENTITY_OR_COUNT_MISMATCH")
    if len(set(official_codes)) != TOTAL:
        errors.append("BLUEPRINT_DUPLICATES")
    rich_codes = []
    for file in RICH.glob("uae-g5-*.rich-lesson-v2.json"):
        for content in read(file).get("Lessons", []):
            rich_codes.append(content.get("LessonCode"))
            if not content.get("ExplanationParagraphs") or not content.get("WorkedExamples"):
                errors.append(f"EMPTY_ARABIC_CONTENT:{content.get('LessonCode')}")
    if len(rich_codes) != TOTAL or set(rich_codes) != set(expected):
        errors.append("RICH_V2_127_IDENTITY_OR_COUNT_MISMATCH")
    if len(set(rich_codes)) != TOTAL:
        errors.append("RICH_V2_DUPLICATES")
    families = read(ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json")
    family_ids = [x["id"] for x in families.get("families", [])]
    if len(family_ids) != len(set(family_ids)):
        errors.append("QUESTION_FAMILY_DUPLICATES")
    return report("structural", "FAIL" if errors else "PASS", errors,
                  actualCheckpoints=checked, actualAtomicRecords=mapped,
                  actualArabicRichLessons=len(rich_codes), actualBlueprintLessons=len(official_codes),
                  note="GENERAL identities are existing source-contract identifiers, not proof of an official COMMON alias.")


def audit_source():
    """Owner-approved G1-G3 style local textbook scope; not 2026-27 Ministry parity."""
    errors = []
    selection = read(EVID / "source-selection.json")
    index = read(EVID / "source-index.json")
    approval = read(EVID / "grade5-owner-academic-approval.json")
    blueprint = read(BLUE)
    pack = read(ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs" /
                "uae-g5-general-t1-ogl-v1.lesson-content-pack.json")
    manifest_path = ROOT / "docs/curriculum/UAE_G5_GENERAL_OFFICIAL_MANIFEST.json"
    manifest = read(manifest_path) if manifest_path.exists() else {}
    progress = read(EVID / "full-grade-progress.json")
    owner_scope = (
        approval.get("ownerAcademicApproval") == "APPROVED" and
        approval.get("ownerReleaseAuthorization") ==
            "APPROVED_SUBJECT_TO_TESTS_AND_RELEASE_SAFETY" and
        approval.get("OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease") is True and
        approval.get("Independent2026_2027ElementParity") == "UNKNOWN" and
        approval.get("SourceParityIsNotReleaseClaim") is True and
        approval.get("NoFalseCIClaim") is True
    )
    if not owner_scope:
        errors.append("OWNER_FIRST_PARTY_LIMITED_SCOPE_RELEASE_NOT_AUTHORIZED")
    hashes = approval.get("SelectedLocalSourceSha256") or {}
    names = {"01.pdf", "02-1.pdf", "02.pdf", "03.pdf"}
    if set(hashes) != names or any(len(str(hashes[n])) != 64 for n in names):
        errors.append("SELECTED_LOCAL_PDF_HASHES_MISSING")
    if hashes.get("01.pdf", "").lower() != selection.get("StudentPdfSha256", "").lower():
        errors.append("SOURCE_SELECTION_01_PDF_SHA_MISMATCH")
    if (manifest.get("Grade"), manifest.get("Pathway"), manifest.get("LessonCount"),
        len(manifest.get("Units", []))) != (5, "General", TOTAL, 12):
        errors.append("EXISTING_G5_GENERAL_MANIFEST_NOT_127_12")
    if (blueprint.get("Pathway"), blueprint.get("LogicalLevel"),
        len(blueprint.get("Lessons", []))) != ("General", 5, TOTAL):
        errors.append("EXISTING_G5_BLUEPRINT_NOT_127_GENERAL")
    if (pack.get("AcademicLanguage"), len(pack.get("Lessons", [])),
        progress.get("A_B_C_Ready"), len(index.get("lessons", []))) != (
            "ar", TOTAL, TOTAL, TOTAL):
        errors.append("LOCAL_SOURCE_ARABIC_ABC_OR_INDEX_NOT_127")
    handoff = EVID / "grade5-a-c-handoff.md"
    if not handoff.exists() or "UNKNOWN" not in handoff.read_text(encoding="utf-8"):
        errors.append("SOURCE_LIMITATIONS_NOT_DISCLOSED_IN_HANDOFF")
    unit_pdf = {**{i: "01.pdf" for i in range(1, 6)},
                **{i: "02-1.pdf" for i in range(6, 8)},
                **{i: "02.pdf" for i in range(8, 10)},
                **{i: "03.pdf" for i in range(10, 13)}}
    selected, elements, missing_optional_sha, printed_only_locators = 0, 0, [], []
    ordinal = 0
    for unit, total_in_unit in enumerate(COUNTS, 1):
        for lesson in range(1, total_in_unit + 1):
            ordinal += 1
            code = f"PED:UAE-MOE-MATH:L5:GENERAL:{unit:02d}:{lesson:02d}"
            cp_path = EVID / f"lesson-{ordinal:03d}-stage-a-c-checkpoint.json"
            src_path = EVID / f"lesson-{ordinal:03d}-atomic-source-coverage.json"
            if not cp_path.exists() or not src_path.exists():
                errors.append(f"LOCAL_BOOK_SOURCE_EVIDENCE_MISSING:{ordinal}")
                continue
            cp, src = read(cp_path), read(src_path)
            expected_pdf = unit_pdf[unit]
            expected_hash = str(hashes.get(expected_pdf, "")).lower()
            actual_file = str(src.get("SourcePdfPath") or src.get("SourceBook") or "").replace("\\", "/")
            actual_hash = str(src.get("SourcePdfSha256") or "").lower()
            pdf_pages = (src.get("SourcePdfPages") or src.get("LessonPdfPages") or
                         src.get("PdfPages"))
            printed_pages = (src.get("SourcePrintedPages") or src.get("LessonPrintedPages") or
                             src.get("PrintedPages"))
            pages = pdf_pages or printed_pages
            parts = (src.get("SourceElements") or src.get("SourceCoverage") or
                     src.get("Elements") or src.get("Coverage"))
            if (cp.get("LessonOrdinal") != ordinal or
                src.get("LessonOrdinal") != ordinal or
                cp.get("LessonCode") != code or src.get("LessonCode") != code or
                cp.get("Status") != "READY_FOR_STAGE_D" or
                src.get("Status") != "READY_FOR_STAGE_D" or
                not actual_file.endswith("/" + expected_pdf) or
                actual_hash != expected_hash or
                not isinstance(pages, list) or not pages or
                not isinstance(parts, list) or not parts):
                errors.append(f"LOCAL_BOOK_SHA_PAGE_OR_ABC_IDENTITY:{ordinal}")
                continue
            cp_hash = cp.get("SourceSha256") or cp.get("SourcePdfSha256")
            if cp_hash and str(cp_hash).lower() != expected_hash:
                errors.append(f"CHECKPOINT_BOOK_HASH_DRIFT:{ordinal}")
                continue
            if not cp_hash:
                missing_optional_sha.append(ordinal)
            if not pdf_pages:
                printed_only_locators.append(ordinal)
            if ordinal > len(pack.get("Lessons", [])):
                errors.append(f"CANONICAL_LESSON_MISSING:{ordinal}")
                continue
            row = pack["Lessons"][ordinal - 1]
            translations = row.get("Translations", [])
            if (row.get("LessonCode") != code or not row.get("SourceLocator") or
                not any(t.get("CultureCode") == "ar" and
                        all(t.get(k) for k in ("Title", "Explanation", "WorkedExamples"))
                        for t in translations)):
                errors.append(f"ORIGINAL_ARABIC_LESSON_MISMATCH:{ordinal}")
                continue
            selected += 1
            elements += len(parts)
    if selected != TOTAL:
        errors.append(f"OWNER_SELECTED_SOURCE_LESSONS_{selected}_OF_{TOTAL}")
    # Static digital-only scan: no image tags, data URLs or third-party media
    # embedded in the Arabic learner content. This is not a copyright opinion.
    rich_codes, externally_embedded = [], []
    for file in sorted(RICH.glob("uae-g5-*.rich-lesson-v2.json")):
        for lesson in read(file).get("Lessons", []):
            code = lesson.get("LessonCode")
            rich_codes.append(code)
            prose = json.dumps(lesson, ensure_ascii=False).lower()
            if any(s in prose for s in ("<img", "<iframe", "<object", "<embed",
                                         "data:image", "http://", "https://")):
                externally_embedded.append(code)
    if len(rich_codes) != TOTAL or len(set(rich_codes)) != TOTAL:
        errors.append("G5_FIRST_PARTY_RICH_SCOPE_NOT_127")
    if externally_embedded:
        errors.append("G5_RICHV2_EXTERNAL_EMBEDDED_ASSET_REVIEW_REQUIRED")
    # Owner acceptance permits first-party publication; UNKNOWN remains UNKNOWN.
    return report(
        "source", "PASS" if not errors else "FAIL", errors,
        sourceReleaseBasis="OWNER_APPROVED_FIRST_PARTY_LOCAL_BOOK_LIMITED_SCOPE",
        selectedLocalBookLessons=selected,
        selectedBookSha256=hashes,
        sourceElementRowsRecorded=elements,
        sourceElementFullDenominator=None,
        equivalentElementsVerified=None,
        semanticParityPercent="UNKNOWN",
        official2026_2027TextbookParity="UNKNOWN_NOT_CERTIFIED",
        ministrySourceCertificationClaim=False,
        ownerFirstPartyApproval=owner_scope,
        embeddedExternalRichMediaCount=len(externally_embedded),
        originalArabicRichLessonCount=len(rich_codes),
        independentThirdPartyRightsCertified=False,
        humanRtlReviewCertified=False,
        optionalCheckpointHashesNotRepeated=missing_optional_sha,
        printPageOnlySourceLocators=printed_only_locators,
        releaseScopeAllowed=not errors,
        note=("Same first-party owner-release scope as UAE G1, G2, G3. "
              "2026-27 textbook order and exercise-level parity are NOT certified.")
    )


def audit_final():
    """Require same-SHA current-run proof; owner-approved source limits are separate."""
    import xml.etree.ElementTree as ET
    results = {name: os.getenv(env, "NOT_RUN") for name, env in [
        ("source", "SOURCE_RESULT"), ("structural", "STRUCTURAL_RESULT"),
        ("math", "MATH_RESULT"), ("authenticatedE2E", "E2E_RESULT"),
    ]}
    errors = [f"{name}:{status}" for name, status in results.items()
              if status != "success"]
    sha, run_id = os.getenv("GITHUB_SHA"), os.getenv("GITHUB_RUN_ID")
    artifacts = {}
    for phase in ("structural", "source", "authenticated-e2e", "arabic-visual"):
        path = OUT / f"grade5-{phase}-gate.json"
        try:
            item = read(path)
        except (OSError, ValueError, TypeError):
            errors.append("CURRENT_RUN_EVIDENCE_MISSING:" + phase)
            continue
        artifacts[phase] = item
        if item.get("status") != "PASS" or item.get("candidateSha") != sha:
            errors.append("SHA_OR_STATUS_NOT_VERIFIED:" + phase)
        if phase in ("structural", "source") and str(item.get("actionsRunId")) != str(run_id):
            errors.append("CURRENT_RUN_SOURCE_OR_STRUCTURAL_ID_MISMATCH:" + phase)
    structural = artifacts.get("structural", {})
    if (structural.get("actualCheckpoints") != TOTAL or
        structural.get("actualArabicRichLessons") != TOTAL):
        errors.append("GRADE5_STRUCTURAL_127_NOT_PROVEN")
    source = artifacts.get("source", {})
    if (source.get("selectedLocalBookLessons") != TOTAL or
        source.get("ownerFirstPartyApproval") is not True or
        source.get("official2026_2027TextbookParity") != "UNKNOWN_NOT_CERTIFIED" or
        source.get("embeddedExternalRichMediaCount") != 0 or
        source.get("originalArabicRichLessonCount") != TOTAL):
        errors.append("OWNER_LIMITED_SOURCE_OR_DIGITAL_ASSET_EVIDENCE_NOT_PROVEN")
    auth = artifacts.get("authenticated-e2e", {}).get("checks", {})
    if (auth.get("learnerPages") != TOTAL or
        auth.get("persistedAnswerAttempts") != 3 * TOTAL or
        auth.get("fiveRoleLogins") is not True or
        auth.get("crossTenantAccessDenied") is not True):
        errors.append("GRADE5_AUTHENTICATED_127_381_NOT_PROVEN")
    browser = artifacts.get("arabic-visual", {}).get("checks", {})
    if (browser.get("chromiumDesktopPages") != TOTAL or
        browser.get("chromiumMobilePages") != TOTAL or
        browser.get("screenshotSamples", 0) < 8):
        errors.append("GRADE5_CHROMIUM_127_DESKTOP_MOBILE_NOT_PROVEN")
    for name in ("g5.trx", "g5-impact.trx"):
        file = OUT / "trx" / name
        try:
            root = ET.parse(file).getroot()
            counts = next(node for node in root.iter() if node.tag.endswith("Counters"))
            if (int(counts.get("passed", "0")) < 1 or
                int(counts.get("failed", "-1")) != 0 or
                int(counts.get("error", "-1")) != 0):
                errors.append("NON_GREEN_MATH_TRX:" + name)
        except (OSError, ValueError, TypeError, StopIteration, ET.ParseError):
            errors.append("MATH_TRX_MISSING_OR_INVALID:" + name)
    approval = read(EVID / "grade5-owner-academic-approval.json")
    owner_approved = (
        approval.get("ownerAcademicApproval") == "APPROVED" and
        approval.get("OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease") is True and
        approval.get("ownerReleaseAuthorization") ==
            "APPROVED_SUBJECT_TO_TESTS_AND_RELEASE_SAFETY"
    )
    if not owner_approved:
        errors.append("GRADE5_OWNER_FIRST_PARTY_RELEASE_NOT_APPROVED")
    # Technical and local-book acceptance, not independent 2026-27 source parity,
    # third-party asset certification, human classroom UAT, or Render deployment.
    success = not errors
    return report(
        "final", "PASS" if success else "BLOCKED", errors,
        dependencyResults=results,
        finalCandidateAccepted=success,
        sourceParityPercent="UNKNOWN",
        official2026_2027SourceParityCertified=False,
        ownerFirstPartyApprovalRecorded=owner_approved,
        authenticatedE2ETested=results["authenticatedE2E"] == "success",
        humanRtlScreenshotReview="NOT_INDEPENDENTLY_CERTIFIED",
        independentThirdPartyAssetRights="NOT_CERTIFIED",
        releaseDecision="READY_FOR_RELEASE_DECISION" if success else "NO_GO_TECHNICAL",
        mergeAllowed=success,
        renderAllowed=False,
        note=("All technical acceptance requires current-run SHA-linked artifacts; "
              "source latest-edition parity remains UNKNOWN as in Grades 1-3.")
    )


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", choices=["structural", "source", "final"], required=True)
    args = parser.parse_args()
    raise SystemExit({"structural": audit_structural, "source": audit_source,
                      "final": audit_final}[args.phase]())
