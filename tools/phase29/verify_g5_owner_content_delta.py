#!/usr/bin/env python3
"""Exact Grade 5 owner-consented English-translation delta, not academic source parity.

Fail closed for any changed unapproved content pack. The shared storage parity
gate still protects other grades, official standards, source metadata and other
lesson translations. This only permits the 107 changes explicitly owner-consented
in Issue #619 and pinned to one Git blob.
"""
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
RELATIVE = ("src/Edulytics.Core/Curriculum/LessonContent/Packs/"
            "us-ccss-math-g5-phase29-v1.lesson-content-pack.json")
CONSENT = ("artifacts/curriculum-delivery/US-CCSS-MATH/G5/"
           "grade5-owner-academic-consent-20261101.json")
MAIN_BLOB = "0bb012cdd41166b76915a7cd18ced1d9ab3208cc"
CANDIDATE_BLOB = "0769be253207c06d144f6bb9f745a57b86207178"
ALLOWED = frozenset(("title", "explanation", "keyConceptsAndRules",
                     "workedExamples", "stepByStepSolutions",
                     "commonMistakes", "quickSummary"))


def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT, text=True).strip()


def validate_pair(original, candidate):
    before = dict(original)
    after = dict(candidate)
    old = before.pop("lessons", None)
    new = after.pop("lessons", None)
    old_version = before.pop("contentVersion", None)
    new_version = after.pop("contentVersion", None)
    # The fixed owner-approved Grade5 Git blob also updates exactly this pack
    # contentVersion. It is not permission to edit any other pack metadata.
    if old_version != new_version and (
        old_version, new_version) != (
            "phase30-common-core-rich-content-v1",
            "phase30-common-core-rich-content-v1-g5-u01-l01-source-20261030"):
        raise ValueError("G5 unexpected contentVersion transition")
    if (before != after or not isinstance(old, list) or
            not isinstance(new, list) or len(old) != 148 or len(new) != 148):
        raise ValueError("G5 top-level metadata or 148-lesson denominator changed")
    changed = set()
    seen = set()
    for o, n in zip(old, new):
        code = o.get("lessonCode")
        if (not isinstance(code, str) or
                not code.startswith("PED:US-CCSS-MATH:G5:") or
                code != n.get("lessonCode") or code in seen):
            raise ValueError("G5 lesson order/identity duplicated or changed")
        seen.add(code)
        o_meta, n_meta = dict(o), dict(n)
        ot = o_meta.pop("translations", None)
        nt = n_meta.pop("translations", None)
        if o_meta != n_meta:
            raise ValueError("G5 official or publisher/source metadata changed: " + code)
        if (not isinstance(ot, list) or not isinstance(nt, list) or
                len(ot) != len(nt) or
                sorted(t.get("cultureCode") for t in ot) !=
                sorted(t.get("cultureCode") for t in nt)):
            raise ValueError("G5 translation cultures changed: " + code)
        originals = {t["cultureCode"]: t for t in ot}
        for t in nt:
            culture = t["cultureCode"]
            prev = originals[culture]
            if culture != "en":
                if prev != t:
                    raise ValueError("G5 non-English translation changed: " + code)
                continue
            fields = {k for k in set(prev) | set(t) if prev.get(k) != t.get(k)}
            if not fields <= ALLOWED or set(prev) != set(t):
                raise ValueError("G5 non-teaching English field changed: " + code)
            if fields:
                changed.add(("US-CCSS-MATH", code))
    if len(changed) != 107:
        raise ValueError("G5 owner-consented teaching edit count must equal 107, actual " +
                         str(len(changed)))
    return changed


def validate():
    actual_main = git("rev-parse", "origin/main:" + RELATIVE)
    actual_candidate = git("hash-object", RELATIVE)
    if actual_candidate == actual_main:
        # No exception is needed when this branch preserves the published pack.
        return set()
    consent_path = ROOT / CONSENT
    if not consent_path.exists():
        raise ValueError("Grade 5 owner consent is missing for changed lesson pack")
    consent = json.loads(consent_path.read_text(encoding="utf-8"))
    if (actual_main != MAIN_BLOB or actual_candidate != CANDIDATE_BLOB or
            consent.get("schemaVersion") != 1 or
            consent.get("type") !=
            "GRADE5_OWNER_PROJECT_ACADEMIC_CONSENT_NOT_PER_LESSON_SOURCE_CERTIFICATION" or
            consent.get("ownerDecision") != "OWNER_ACADEMIC_CONSENT_GRANTED" or
            consent.get("scope") != {
                "repository": "EIAS79/Edulytics", "prNumber": 596,
                "curriculum": "US-CCSS-MATH", "grade": "G5", "stage": "D",
                "lessonCount": 148, "requiredEntryState": "READY_FOR_STAGE_D"} or
            consent.get("approvedContentPackPath") != RELATIVE or
            consent.get("approvedContentPackGitBlobSha") != CANDIDATE_BLOB or
            consent.get("publicDecisionEvidence") !=
            "https://github.com/EIAS79/Edulytics/issues/619#issuecomment-6564702967" or
            consent.get("authorizationClaims") != {
                "ownerConsentsToReviewedGrade5Content": True,
                "ownerConsentsTo107IdentifiedTextChanges": True} or
            consent.get("ownerSignedCryptographicDocument") is not False or
            consent.get("certificationExclusions") != {
                "individualSourceElementsIndependentlyCompared": False,
                "thirdPartyAssetRightsConfirmed": False,
                "firstEditionStudentPageParityProven": False,
                "authenticatedStudentTeacherE2EPassed": False,
                "grade6WithinScope": False,
                "releaseApproved": False}):
        raise ValueError("G5 immutable owner consent or Git content blob drift")
    original = json.loads(subprocess.check_output(
        ["git", "show", "origin/main:" + RELATIVE], cwd=ROOT, text=True))
    candidate = json.loads((ROOT / RELATIVE).read_text(encoding="utf-8"))
    return validate_pair(original, candidate)


if __name__ == "__main__":
    approved = validate()
    print(f"G5_OWNER_CONTENT_DELTA_PINNED 148 lessons; "
          f"{len(approved)} source-preserving English edits; "
          "source-element parity and rights NOT certified")
