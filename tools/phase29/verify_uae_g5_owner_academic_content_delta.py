#!/usr/bin/env python3
"""Exact-blob owner-authorized UAE G5 first-party Arabic delta.

This is a narrowed Git publication exception, not 2026-27 Ministry textbook
parity, third-party artwork permission, or a human visual UAT certificate.
Every original lesson ID stays intact and all other grades stay protected.
"""
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
PACK = ("src/Edulytics.Core/Curriculum/LessonContent/Packs/"
        "uae-g5-general-t1-ogl-v1.lesson-content-pack.json")
BLUEPRINT = ("src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/"
             "uae-g5-general-t1-ogl-v1.lesson-blueprint.json")
AUTH = ("artifacts/curriculum-delivery/UAE-MOE-MATH/G5/"
        "grade5-owner-academic-approval.json")
LOCKED = {
    PACK: ("8920f408d9c602f8bcb517d1f6cdbb6378fdea23",
           "4696a505ba112cdc746b53f3f1dd504220d8da1d"),
    BLUEPRINT: ("31575def9076b25deaf14a7639226b8460805fe1",
                "3faf62015240abbcecaa294e270f3efc369aa025"),
}
FIELDS = ("Title", "Explanation", "KeyConceptsAndRules", "WorkedExamples",
          "StepByStepSolutions", "CommonMistakes", "QuickSummary")
TOTAL = 127
UNIT_COUNTS = (9, 10, 13, 6, 14, 11, 8, 13, 12, 7, 11, 13)


def git(*args):
    return subprocess.check_output(
        ["git", *args], cwd=ROOT, text=True, encoding="utf8").strip()


def document(path):
    return json.loads((ROOT / path).read_text(encoding="utf-8-sig"))


def validate():
    for path, (old, approved) in LOCKED.items():
        baseline = git("rev-parse", "origin/main:" + path)
        committed = git("rev-parse", "HEAD:" + path)
        worktree = git("hash-object", path)
        if (baseline not in (old, approved) or
                committed != approved or worktree != approved):
            raise ValueError("UAE Grade5 exact owner-reviewed Git blob drift: " + path)

    owner = document(AUTH)
    if any((
        owner.get("programme") != "UAE-MOE-MATH",
        owner.get("grade") != 5,
        owner.get("pathway") != "General/Common",
        owner.get("ownerAcademicApproval") != "APPROVED",
        owner.get("ownerReleaseAuthorization") !=
            "APPROVED_SUBJECT_TO_TESTS_AND_RELEASE_SAFETY",
        owner.get("OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease")
            is not True,
        owner.get("Independent2026_2027ElementParity") != "UNKNOWN",
        owner.get("SourceParityIsNotReleaseClaim") is not True,
        owner.get("NoFalseCIClaim") is not True,
        owner.get("ThirdPartyAssetClearance") != "NOT_INDEPENDENTLY_CERTIFIED",
    )):
        raise ValueError("UAE Grade5 owner first-party approval or limitations invalid")

    pack, blueprint = document(PACK), document(BLUEPRINT)
    lessons = pack.get("Lessons", [])
    codes = [row.get("LessonCode") for row in lessons]
    expected = [
        f"PED:UAE-MOE-MATH:L5:GENERAL:{unit:02d}:{lesson:02d}"
        for unit, count in enumerate(UNIT_COUNTS, 1)
        for lesson in range(1, count + 1)
    ]
    if not (
        pack.get("AcademicLanguage") == "ar" and
        pack.get("SourceResolution") == "PreviousOfficialFallback" and
        pack.get("FallbackReason") and
        blueprint.get("LogicalLevel") == 5 and
        blueprint.get("Pathway") == "General" and
        len(blueprint.get("Units", [])) == len(UNIT_COUNTS) and
        codes == expected == [x.get("LessonCode") for x in blueprint.get("Lessons", [])] and
        len(set(codes)) == TOTAL
    ):
        raise ValueError("UAE Grade5 General identity/Arabic/source fallback drift")

    original = json.loads(subprocess.check_output(
        ["git", "show", "origin/main:" + PACK], cwd=ROOT,
        text=True, encoding="utf8"))
    previous = original.get("Lessons", [])
    if len(previous) != TOTAL or [x.get("LessonCode") for x in previous] != codes:
        raise ValueError("Original main Grade5 identity, order or 127 denominator changed")

    changes = set()
    old_is_unpublished = git("rev-parse", "origin/main:" + PACK) == LOCKED[PACK][0]
    for before, after in zip(previous, lessons):
        code = after["LessonCode"]
        translations = after.get("Translations", [])
        english = [t for t in translations if t.get("CultureCode") == "en"]
        arabic = [t for t in translations if t.get("CultureCode") == "ar"]
        if len(english) != 1 or len(arabic) != 1 or len(translations) != 2:
            raise ValueError("UAE Grade5 expected exactly one English and Arabic body: " + code)
        for field in FIELDS:
            value = str(arabic[0].get(field, "")).strip()
            if len(value) < 5:
                raise ValueError("UAE Grade5 original Arabic field missing: " + code + ":" + field)
            if any(marker in value.casefold() for marker in (
                "<img", "<video", "<iframe", "<svg", "data:image",
                "http://", "https://", ".png", ".jpg", ".jpeg", ".mp4", "!["
            )):
                raise ValueError("UAE Grade5 unreviewed embedded media: " + code)
        if old_is_unpublished:
            originals = before.get("Translations", [])
            if len(originals) != 1 or originals[0].get("CultureCode") != "en":
                raise ValueError("UAE Grade5 main original non-English content drift: " + code)
            old_en = originals[0]
            new_en = english[0]
            if code == expected[0]:
                diff = {k for k in set(old_en) | set(new_en)
                        if old_en.get(k) != new_en.get(k)}
                if (diff != {"Title"} or
                    old_en.get("Title") != "Place Value to Millions" or
                    new_en.get("Title") != "Place Value to Billions"):
                    raise ValueError("UAE Grade5 U01L01 exact English title correction drift")
            elif old_en != new_en:
                raise ValueError("UAE Grade5 prior English body unexpectedly changed: " + code)
        elif before != after:
            raise ValueError("UAE Grade5 post-merge immutable body changed: " + code)
        if before.get("Translations") != after.get("Translations"):
            changes.add(("UAE-MOE-MATH", code))
    if old_is_unpublished and len(changes) != TOTAL:
        raise ValueError("UAE Grade5 expected exactly 127 first-party Arabic additions")
    if not old_is_unpublished and changes:
        raise ValueError("UAE Grade5 already released to main; no further changes allowed")
    print("UAE_G5_OWNER_EXACT_BLOB_ARABIC",
          f"verified={TOTAL}", f"changedBodies={len(changes)}",
          "latest2026_27Parity=UNKNOWN", "firstPartyOnly=True")
    return changes


if __name__ == "__main__":
    print("UAE_G5_PINNED_OWNER_ACCEPTED_DELTA_KEYS", len(validate()))
