#!/usr/bin/env python3
"""Grade 7 release model: first-party scope and literal parity are separate.

No production mutation. The final gate requires actual same-run reports.
Historical A-C labels are never changed into test results.
"""
import datetime
import json
import os
import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
G10 = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G10"
OUT = ROOT / "artifacts/uae-g10-stage-d"
PREFIX = "PED:UAE-MOE-MATH:L10:GENERAL:"
N = 148
SHA = os.environ.get("GITHUB_SHA", "")
FAILURES = []


def require(condition, label):
    if not condition:
        FAILURES.append(label)


def read(path):
    try:
        return json.loads(path.read_text(encoding="utf-8-sig"))
    except (OSError, ValueError):
        FAILURES.append("MISSING_OR_INVALID_REPORT:" + str(path.name))
        return {}


def same_sha(report, phase):
    require(bool(re.fullmatch(r"[a-f0-9]{40}", SHA)), "MISSING_EXACT_SHA")
    require(report.get("candidateSha") == SHA, phase + "_SHA_MISMATCH")


def write(path, report):
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n",
                    encoding="utf-8")
    print(json.dumps({k: v for k, v in report.items()
                      if k not in ("lessons",)}, ensure_ascii=False, indent=2))


def catalogue():
    doc = read(ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json")
    rows = sorted(doc.get("Lessons", []), key=lambda x: x["SortOrder"])
    codes = [r["LessonCode"] for r in rows]
    require(len(doc.get("Units", [])) == 18, "UNITS_NOT_18")
    require(len(codes) == len(set(codes)) == N and
            all(x.startswith(PREFIX) for x in codes) and
            [r["SortOrder"] for r in rows] == list(range(1, N + 1)),
            "EXACT_ORDERED_GENERAL_CATALOGUE_REQUIRED")
    return codes


def source():
    codes = catalogue()
    owner = read(G10 / "grade10-owner-first-party-release-scope.json")
    require(owner.get("precedentPr") == 638 and owner.get("pr") == 655 and
            owner.get("grade") == 10 and owner.get("pathway") == "General" and
            owner.get("academicLanguage") == "ar" and
            owner.get("indexedLessons") == N and owner.get("indexedUnits") == 18 and
            owner.get("ownerPermissionToProceedToReleaseGates") is True and
            owner.get("ownerApprovalOfIndex") is True and
            owner.get("releaseScope") == "FIRST_PARTY_ORIGINAL_EDUCATIONAL_EQUIVALENCE_WITH_DISCLOSED_SOURCE_LIMITATIONS" and
            owner.get("deploymentAuthorizedOnlyAfterCiAndScopedDbVerification") is True,
            "OWNER_SCOPE_NOT_RECORDED")
    require(owner.get("originalPublisherElementParity") == "UNKNOWN_NOT_CERTIFIED" and
            owner.get("fullSourceElementDenominator") is None and
            owner.get("publisherArtworkOrVerbatimTextReuseLicensed") is False and
            owner.get("publisherBookReferenceOnly") is True and
            owner.get("internalSourceProvenanceRetained") is True and
            owner.get("independentHumanTeacherReview") == "NOT_RECORDED",
            "SOURCE_OR_RIGHTS_SCOPE_MISREPRESENTED")
    progress = read(G10 / "full-grade-progress.json")
    repair = progress.get("repairValidation", {})
    require(progress.get("reportedAuthoredLessonCount") == N and
            progress.get("ownerApprovedLessonCount") == N and
            progress.get("ownerApprovedUnitCount") == 18 and
            repair.get("gradeClosedCount") == N and
            repair.get("unresolvedRepairCount") == 0 and
            repair.get("missingMatchingSourceBodyCount") == 0 and
            repair.get("unresolvedSourceGapCount") == 0 and
            progress.get("sourceParity") == "UNKNOWN",
            "SELECTED_SOURCE_HANDOFF_INCOMPLETE")
    inventory = read(G10 / "source-inventory.json")
    edition = read(G10 / "edition-diff.json")
    physical = inventory.get("stageDIntakePhysicalInventory", {})
    require(inventory.get("sourceLock") is False and
            inventory.get("knownSourceElementDenominator") is None and
            inventory.get("sourceCoveragePercent") is None and
            physical.get("originalFilesMatched") == 7 and
            physical.get("supplementalFilesMatched") == 5 and
            physical.get("totalPhysicalPages") == 2403 and
            edition.get("allRequiredFileHashesMatched") is True and
            edition.get("sourceSemanticCoverage") == "UNKNOWN",
            "SOURCE_INVENTORY_OR_LIMITATIONS_DRIFT")
    hashes = {x.get("sha256VerifiedLocal"): x.get("pagesFromPriorDocument")
              for x in inventory.get("files", [])}
    hashes.update({x.get("sha256"): x.get("pages") for x in
                   inventory.get("supplementalSourceSnapshot", {}).get("files", [])})
    require(len(hashes) == 12 and all(isinstance(k, str) and
            re.fullmatch(r"[a-f0-9]{64}", k) and isinstance(v, int) and v > 0
            for k, v in hashes.items()) and sum(hashes.values()) == 2403,
            "PHYSICAL_HASH_PAGE_INVENTORY_DRIFT")

    preflight = read(OUT / "g10-preflight.json")
    same_sha(preflight, "STRUCTURE")
    counters = preflight.get("counters", {})
    require(preflight.get("structuralStatus") == "PASS" and
            not preflight.get("failures") and
            all(counters.get(k) == N for k in ("checked", "arabic", "rich")) and
            counters.get("indexOnly") == 0,
            "STRUCTURAL_PREFLIGHT_INCOMPLETE")
    allowed_literal_only = {"SOURCE_ATOMIC_PARITY_UNPROVEN", "STAGE_D_NOT_CERTIFIED"}
    require(all(x.get("kind") in allowed_literal_only
                for x in preflight.get("releaseBlockers", [])),
            "NON_LITERAL_SOURCE_BLOCKER_PRESENT")
    lessons = []
    for ordinal, code in enumerate(codes, 1):
        start = len(FAILURES)
        cp = read(G10 / f"lesson-{ordinal:03d}-stage-a-c-checkpoint.json")
        evidence = G10 / f"lesson-{ordinal:03d}-source-evidence.md"
        require(cp.get("lessonCode") == code and
                str(cp.get("status", "")).startswith("READY_FOR_STAGE_D") and
                cp.get("sourceBodyVerified") is not False and
                cp.get("productionRouting") is not True,
                f"LESSON_{ordinal}_CHECKPOINT")
        require(evidence.is_file() and evidence.stat().st_size > 0,
                f"LESSON_{ordinal}_SOURCE_EVIDENCE")
        # Selected source locators, not an exhaustive source denominator.
        ev_text = evidence.read_text(encoding="utf-8-sig") if evidence.is_file() else ""
        cp_text = json.dumps(cp, ensure_ascii=False)
        found_hashes = [h for h in hashes if isinstance(h, str) and
                        (h in cp_text or h in ev_text)]
        require(bool(found_hashes), f"LESSON_{ordinal}_SOURCE_SHA")
        require(not cp.get("fullSourceBookEquivalenceClaimed") or
                isinstance(cp.get("sourceElementDenominatorForThisLesson"), int),
                f"LESSON_{ordinal}_UNSUPPORTED_LITERAL_CLAIM")
        rich_paths = [cp.get(k) for k in ("richV2Path", "arabicRichV2Path", "studentRichV2")
                      if isinstance(cp.get(k), str)]
        if rich_paths:
            rich_doc = read(ROOT / rich_paths[0])
            rows = [x for x in rich_doc.get("Lessons", []) if x.get("LessonCode") == code]
            require(len(rows) == 1 and rows[0].get("CultureCode") == "ar",
                    f"LESSON_{ordinal}_ARABIC_RICH_SOURCE")
            # No third-party image/media URL is a grant of rights. Rendered
            # original semantic models are separately checked by browser tests.
            for row in rows:
                for visual in row.get("Visuals", []):
                    for key, value in visual.items():
                        if isinstance(value, str) and re.search(r"(?:image|asset|media|svg)(?:url|path|src)$", key, re.I):
                            require(not re.search(r"https?://|\.pdf(?:$|[?#])", value, re.I),
                                    f"LESSON_{ordinal}_UNAPPROVED_EXTERNAL_ART")
        else:
            require(False, f"LESSON_{ordinal}_MISSING_RICH_PATH")
        lessons.append({"ordinal": ordinal, "lessonCode": code,
                        "selectedProvenanceStatus": "PASS" if len(FAILURES) == start else "BLOCKED",
                        "sourceSha256": found_hashes,
                        "literalSourceParity": "UNKNOWN",
                        "teacherObservation": "NOT_CLAIMED"})
    report = {"phase": "source", "candidateSha": SHA,
              "status": "NO_GO" if FAILURES else "PASS",
              "sourceApprovalBasis": "OWNER_DIRECTED_FIRST_PARTY_WITH_DISCLOSED_SOURCE_LIMITATIONS",
              "sourcePublisherItemParity": "UNKNOWN_NOT_CERTIFIED",
              "independentTeacherReview": "NOT_RECORDED",
              "sourceFileIdentityCount": 12, "physicalPages": 2403,
              "selectedProvenancePassed": sum(x["selectedProvenanceStatus"] == "PASS" for x in lessons),
              "productionWritten": False, "failures": FAILURES, "lessons": lessons}
    write(OUT / "grade10-first-party-source.json", report)
    return 1 if FAILURES else 0


def final():
    codes = catalogue()
    for name in ("STRUCTURE_RESULT", "SOURCE_RESULT", "COMPILE_RESULT",
                 "AUTH_RESULT", "CATALOGUE_RESULT", "TENANT_RESULT"):
        require(os.environ.get(name) == "success", name + "_NOT_SUCCESS")
    artifacts = OUT / "current-run"
    source_doc = read(artifacts / "source/grade10-first-party-source.json")
    same_sha(source_doc, "SOURCE")
    require(source_doc.get("status") == "PASS" and
            source_doc.get("selectedProvenancePassed") == N and
            source_doc.get("sourcePublisherItemParity") == "UNKNOWN_NOT_CERTIFIED" and
            [x.get("lessonCode") for x in source_doc.get("lessons", [])] == codes and
            all(x.get("selectedProvenanceStatus") == "PASS" for x in source_doc.get("lessons", [])),
            "FIRST_PARTY_SOURCE_GATE_NOT_PASS")

    auth = read(artifacts / "auth/uae-g10-stage-d-authenticated-148-lessons.json")
    rtl_files = list((artifacts / "auth").rglob("report.json"))
    require(len(rtl_files) == 1, "MISSING_OR_AMBIGUOUS_RTL_REPORT")
    rtl = read(rtl_files[0]) if len(rtl_files) == 1 else {}
    same_sha(auth, "AUTH")
    same_sha(rtl, "RTL")
    for key in ("checkedLessons", "lessonPageCount", "negativeEnglishFallbackChecks",
                "declaredDigitalCandidateLessons", "correctAttemptPersistenceCount",
                "wrongAttemptPersistenceCount", "malformedAttemptRejectionCount"):
        require(auth.get(key) == N, "AUTH_COUNT:" + key)
    require(auth.get("releaseTokenMode") is True and
            auth.get("crossSchoolAccessDenied") is True and
            auth.get("productionWritten") is False and
            auth.get("sourceLiteralParity") == "UNKNOWN" and
            all(auth.get("roleLogins", {}).get(k) is True for k in
                ("student", "teacher", "supervisor", "admin", "foreign")),
            "AUTH_SCOPE_OR_RELEASE_MODE")
    auth_rows = auth.get("lessons", [])
    require([x.get("lessonCode") for x in auth_rows] == codes and
            all(x.get("pageStatus") == "PASS" and x.get("englishRequestStatus") == "PASS" and
                x.get("practiceStatus") == "PASS" and
                all(x.get(k + "Passed") is True for k in ("correct", "wrong", "malformed"))
                for x in auth_rows), "AUTH_EXACT_LESSON_PROOF")
    require(rtl.get("rtlDesktopLessons") == rtl.get("rtlMobileLessons") == N and
            rtl.get("studentAuthenticated") is True and rtl.get("productionWritten") is False and
            len(rtl.get("pages", [])) == N * 2, "RTL_COUNT_OR_SCOPE")
    for width in (1280, 390):
        pages = [x for x in rtl.get("pages", []) if x.get("width") == width]
        require([x.get("lessonCode") for x in pages] == codes and
                all(x.get("status") == "PASS" and x.get("rtlComputed") is True and
                    x.get("noHorizontalOverflow") is True for x in pages),
                f"RTL_EXACT_LESSON_PROOF:{width}")

    before = read(artifacts / "catalogue/g10-catalogue-before.json")
    after = read(artifacts / "catalogue/g10-catalogue-after-1.json")
    replay = read(artifacts / "catalogue/g10-catalogue-after-2.json")
    for report, phase in ((before, "BEFORE"), (after, "AFTER"), (replay, "REPLAY")):
        same_sha(report, phase)
        require(report.get("productionWritten") is False, phase + "_PRODUCTION_WRITE")
    require(len(before.get("target", [])) == 100 and
            sum(x.get("pathway") == "General" for x in before.get("target", [])) == 48 and
            sum(x.get("pathway") == "Advanced" for x in before.get("target", [])) == 52 and
            len(after.get("target", [])) == len(after.get("metadata", [])) == N and
            set(x.get("code") for x in after.get("target", [])) == set(codes) and
            set(x.get("code") for x in after.get("metadata", [])) == set(codes) and
            before.get("unrelated") == after.get("unrelated") and after == replay,
            "CATALOGUE_REHEARSAL_NOT_EXACT")
    tenant = read(artifacts / "tenant/grade10-tenant-transition.json")
    same_sha(tenant, "TENANT")
    require(tenant.get("status") == "PASS" and
            tenant.get("scope") == "UAE-G10-GENERAL" and
            tenant.get("productionWritten") is False and
            tenant.get("advancedAdoptionsBefore") == 2 and
            tenant.get("advancedAdoptionsAfter") == 0 and
            tenant.get("classesRehomed") == 2 and
            tenant.get("enrollmentsPreserved") == 50 and
            tenant.get("teacherAssignmentsPreserved") == 2 and
            tenant.get("historicalOfficialNodesPreserved") is True and
            tenant.get("unrelatedRowsPreserved") is True and
            tenant.get("rollbackNegativeControlPassed") is True and
            tenant.get("idempotentReplayPassed") is True and
            tenant.get("atomicCatalogueAndTenantCutover") is True and
            tenant.get("legacyLessonsBefore") == 100 and
            tenant.get("generalLessonsAfter") == N and
            tenant.get("publishedMetadataAfter") == N,
            "TENANT_MIGRATION_NOT_VERIFIED")
    trx = list((artifacts / "compiled").rglob("g10-contracts.trx"))
    require(len(trx) == 1, "MISSING_COMPILED_TEST_PROOF")
    if len(trx) == 1:
        try:
            root = ET.parse(trx[0]).getroot()
            results = root.findall(".//{*}UnitTestResult")
            pairs = [x for x in results if "ExactFamilyDispatchRendererAlignmentAndSeedReplay" in x.get("testName", "")]
            require(len(pairs) == 691 and all(x.get("outcome") == "Passed" for x in pairs),
                    "DECLARED_FAMILY_PAIR_TESTS_INCOMPLETE")
        except (ET.ParseError, OSError):
            require(False, "INVALID_COMPILED_TEST_PROOF")
    # Final CI additionally tests all unrelated curricula at the exact same
    # merge ref. These local grade gates do not themselves authorize deployment.
    success = not FAILURES
    report = {"phase": "final", "candidateSha": SHA,
              "recordedAtUtc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
              "status": "PASS" if success else "NO_GO",
              "sourcePublisherItemParity": "UNKNOWN_NOT_CERTIFIED",
              "academicReleaseBasis": "OWNER_DIRECTED_FIRST_PARTY_SCOPE",
              "technicalVerifiedLessonCount": N if success else 0,
              "productionWritten": False, "mergeDone": False, "renderDeploymentDone": False,
              "failures": FAILURES,
              "lessons": [{"ordinal": i, "lessonCode": c,
                           "status": "D_TECHNICALLY_VERIFIED" if success else "D_BLOCKED",
                           "literalSourceParity": "UNKNOWN"}
                          for i, c in enumerate(codes, 1)]}
    write(OUT / "grade10-first-party-final.json", report)
    return 0 if success else 1


if __name__ == "__main__":
    if len(sys.argv) != 2 or sys.argv[1] not in ("source", "final"):
        raise SystemExit("usage: uae_g10_first_party_release.py source|final")
    raise SystemExit(source() if sys.argv[1] == "source" else final())
