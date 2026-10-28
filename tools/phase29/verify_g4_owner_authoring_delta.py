#!/usr/bin/env python3
"""Verify the exact owner-asserted Grade 4 FIRST-PARTY revision; no academic signoff.

Fail closed: original curriculum, all source/provenance metadata, lesson identities,
non-English translations and all unrelated lesson packs remain protected by the
separate V2 main-parity gate. Requires an origin/main checkout.
"""
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
LOCK = ROOT / "docs/curriculum/g4-2021-owner-authored-revision-lock.json"


def _git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT).decode("utf-8").strip()


def _read_main(path):
    return json.loads(subprocess.check_output(
        ["git", "show", "origin/main:" + path], cwd=ROOT
    ).decode("utf-8"))


def _read_candidate(path):
    return json.loads((ROOT / path).read_text(encoding="utf-8"))


def _check_blob_pair(entry, old_key, new_key):
    path = entry["path"]
    old = _git("rev-parse", "origin/main:" + path)
    new = _git("hash-object", path)
    if old != entry[old_key] or new != entry[new_key] or old == new:
        raise ValueError("Pinned Grade 4 Git blob fingerprint drift: " + path)
    return path


def validate():
    lock = json.loads(LOCK.read_text(encoding="utf-8"))
    if (lock.get("schemaVersion") != 1 or
        lock.get("policy") != "PINNED_OWNER_ASSERTED_FIRST_PARTY_TEACHING_REVISION" or
        lock.get("scope") != "US-CCSS-MATH/IM-K5-2021/G4" or
        lock.get("academicCertification") != "NOT_CLAIMED" or
        lock.get("releaseStatus") != "NO_GO" or
        lock.get("sourceElementParity") != "NOT_VERIFIED" or
        lock.get("originalVisualAndThirdPartyAssetRights") != "REVIEW_REQUIRED" or
        lock.get("authenticatedStudentTeacherRuntimeAcceptance") != "NOT_VERIFIED" or
        _git("rev-parse", "origin/main") != lock.get("originalMainCommit")):
        raise ValueError("Grade 4 owner revision authorization/release lock drift")

    pack_lock = lock["contentPack"]
    blueprint_lock = lock["blueprint"]
    if (pack_lock.get("lessonCount") != 149 or
        pack_lock.get("allowedChanges") != "ENGLISH_TRANSLATION_ONLY" or
        pack_lock.get("identityOutcomeProvenanceSourceHashAndAllOtherMetadata") != "MUST_MATCH_MAIN" or
        blueprint_lock.get("lessonCount") != 149 or
        blueprint_lock.get("expectedTitleOnlyCorrections") != 18 or
        blueprint_lock.get("titleCorrectionRule") != "REMOVE_EXACT_TRAILING_PLC_ACTIVITY_ONLY" or
        blueprint_lock.get("allOtherBlueprintFields") != "MUST_MATCH_MAIN"):
        raise ValueError("Grade 4 change scope widened unexpectedly")

    content_path = _check_blob_pair(pack_lock, "originalMainBlobSha", "ownerAuthoredCandidateBlobSha")
    bp_path = _check_blob_pair(blueprint_lock, "originalMainBlobSha", "titleCorrectedCandidateBlobSha")
    if (content_path != "src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g4-phase29-v1.lesson-content-pack.json"
        or bp_path != "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade4-im-k5-2021.lesson-blueprint.json"):
        raise ValueError("Grade 4 revision paths changed")

    original = _read_main(content_path)
    revised = _read_candidate(content_path)
    before = dict(original)
    after = dict(revised)
    old_rows = before.pop("lessons")
    new_rows = after.pop("lessons")
    if before != after or len(old_rows) != 149 or len(new_rows) != 149:
        raise ValueError("Grade 4 pack-level metadata or lesson count changed")
    changed = set()
    for old, new in zip(old_rows, new_rows):
        code = old.get("lessonCode")
        if (not isinstance(code, str) or
            not code.startswith("PED:US-CCSS-MATH:G4:") or
            code != new.get("lessonCode") or
            code in changed):
            raise ValueError("Grade 4 lesson identity/order/uniqueness changed")
        old_meta, new_meta = dict(old), dict(new)
        old_translations = old_meta.pop("translations", None)
        new_translations = new_meta.pop("translations", None)
        if old_meta != new_meta:
            raise ValueError("Grade 4 original source/official mapping metadata changed: " + code)
        if (not isinstance(old_translations, list) or
            not isinstance(new_translations, list) or
            len(old_translations) != 1 or len(new_translations) != 1 or
            old_translations[0].get("cultureCode") != "en" or
            new_translations[0].get("cultureCode") != "en" or
            old_translations == new_translations):
            raise ValueError("Grade 4 translation scope or missing authored change: " + code)
        if set(old_translations[0]) != set(new_translations[0]):
            raise ValueError("Grade 4 English translation structure changed: " + code)
        changed.add(code)
    if len(changed) != 149:
        raise ValueError("Grade 4 owner-authored lesson denominator changed")

    original_bp = _read_main(bp_path)
    revised_bp = _read_candidate(bp_path)
    old_bp, new_bp = dict(original_bp), dict(revised_bp)
    old_lessons, new_lessons = old_bp.pop("Lessons"), new_bp.pop("Lessons")
    if old_bp != new_bp or len(old_lessons) != 149 or len(new_lessons) != 149:
        raise ValueError("Grade 4 blueprint global metadata/count changed")
    title_only = 0
    for old, new in zip(old_lessons, new_lessons):
        if (old.get("LessonCode") not in changed or old.get("LessonCode") != new.get("LessonCode")):
            raise ValueError("Grade 4 blueprint identity/order changed")
        old_title, new_title = old.get("Title"), new.get("Title")
        old_copy, new_copy = dict(old), dict(new)
        old_copy.pop("Title", None)
        new_copy.pop("Title", None)
        if old_copy != new_copy:
            raise ValueError("Grade 4 blueprint non-title content changed: " + old["LessonCode"])
        if old_title != new_title:
            if not (isinstance(old_title, str) and
                    old_title.endswith(" PLC Activity") and
                    new_title == old_title[:-len(" PLC Activity")]):
                raise ValueError("Grade 4 blueprint title correction is not the exact suffix alias")
            title_only += 1
    if title_only != 18:
        raise ValueError(f"Grade 4 expected 18 title corrections, found {title_only}")
    return {("US-CCSS-MATH", code) for code in changed}


if __name__ == "__main__":
    rows = validate()
    print(f"G4_OWNER_FIRST_PARTY_REVISION_PINNED lessons={len(rows)} blueprintTitles=18 "
          "academicCertification=NOT_CLAIMED release=NO_GO")
