#!/usr/bin/env python3
"""Check that V2 preserves existing main-branch canonical lesson identities and bodies.

Purely read-only. No curriculum scope additions, DB connections, or updates.
Use a full git checkout with origin/main available.
"""
import json
import hashlib
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BASE = Path("src/Edulytics.Core/Curriculum/LessonContent/Packs")

def main_files():
    raw = subprocess.check_output(["git", "ls-tree", "-r", "--name-only", "origin/main", "--", str(BASE)], cwd=ROOT, text=True)
    return [p for p in raw.splitlines() if p.endswith(".lesson-content-pack.json")]

def load_main(path):
    return json.loads(subprocess.check_output(["git", "show", f"origin/main:{path}"], cwd=ROOT, text=True))

def identities(doc):
    return [(str(doc.get("PackCode") or doc.get("packCode") or ""), str(x.get("LessonCode") or x.get("lessonCode") or ""), x)
            for x in (doc.get("Lessons") or doc.get("lessons") or [])]

def run():
    baseline, candidate, errors = {}, {}, []
    # One strictly pinned owner-asserted FIRST-PARTY Grade 4 authoring revision.
    # This is not publisher-source parity, academic signoff or release approval.
    # The standalone validator compares both Git blobs, every lesson identity,
    # all source/official metadata, and the exact 18 title-only corrections.
    try:
        from verify_g4_owner_authoring_delta import validate as validate_g4
        g4_owner_authored = validate_g4()
    except (ValueError, KeyError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"Grade 4 pinned owner-authored delta invalid: {exc}")
        g4_owner_authored = set()
    # The Grade 5 owner consent is an immutable, content-only technical
    # exception: no permission to rewrite source, official metadata or other grades.
    # Source/academic rights approval remains in the separate Stage D gate.
    try:
        from verify_g5_owner_content_delta import validate as validate_g5
        g5_owner_content = validate_g5()
    except (ValueError, KeyError, TypeError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"Grade 5 pinned owner content delta invalid: {exc}")
        g5_owner_content = set()
    for path in main_files():
        current = ROOT / path
        if not current.is_file():
            errors.append(f"missing original content pack: {path}")
            continue
        for bucket, doc in [(baseline, load_main(path)), (candidate, json.loads(current.read_text(encoding="utf8")))]:
            for pack, code, lesson in identities(doc):
                key = (pack, code)
                if key in bucket: errors.append(f"duplicate identity {key} in {'main' if bucket is baseline else 'V2'}")
                bucket[key] = lesson
    # Original official curriculum nodes, outcome codes and their mapping links
    # must survive the storage migration byte-for-byte. This is a source parity
    # check, not a new academic certification policy.
    curriculum_base = "src/Edulytics.Core/Curriculum/Packs"
    original_packs = subprocess.check_output(
        ["git", "ls-tree", "-r", "--name-only", "origin/main", "--", curriculum_base],
        cwd=ROOT, text=True,
    ).splitlines()
    compared_packs = 0
    for path in original_packs:
        if not path.endswith(".curriculum-pack.json"):
            continue
        current = ROOT / path
        if not current.is_file():
            errors.append(f"missing original curriculum pack: {path}")
            continue
        original_bytes = subprocess.check_output(
            ["git", "show", f"origin/main:{path}"], cwd=ROOT,
        )
        if original_bytes != current.read_bytes():
            errors.append(f"modified original curriculum standards/outcomes/mapping: {path}")
        compared_packs += 1
    print(f"MAIN_TO_V2_OFFICIAL_PACK_PARITY compared={compared_packs} errors={len(errors)}")
    # All original published academic blueprints are immutable in a storage-only migration.
    blueprint_dir = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs"
    blueprint_files = subprocess.check_output(
        ["git", "ls-tree", "-r", "--name-only", "origin/main", "--", blueprint_dir],
        cwd=ROOT, text=True,
    ).splitlines()
    verified_blueprints = 0
    for path in blueprint_files:
        if not path.endswith(".lesson-blueprint.json"):
            continue
        current = ROOT / path
        if not current.is_file():
            errors.append(f"missing original lesson blueprint: {path}")
            continue
        if subprocess.check_output(["git", "show", f"origin/main:{path}"], cwd=ROOT) != current.read_bytes():
            g4_blueprint = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade4-im-k5-2021.lesson-blueprint.json"
            if not (path == g4_blueprint and len(g4_owner_authored) == 149):
                errors.append(f"changed original lesson blueprint: {path}")
        verified_blueprints += 1
    print(f"MAIN_TO_V2_BLUEPRINT_PARITY compared={verified_blueprints}")
    missing = sorted(set(baseline) - set(candidate))
    changed = sorted(key for key in baseline.keys() & candidate.keys()
                     if baseline[key].get("Translations", baseline[key].get("translations"))
                     != candidate[key].get("Translations", candidate[key].get("translations")))
    if missing: errors.append(f"missing original lesson identities: {len(missing)}; sample {missing[:5]}")

    # Hash-pinned, source-preserving US-CCSS pedagogical corrections only.
    # Main already contains the first two approved repairs. For those entries,
    # enforce the reviewed new fingerprint without demanding an additional edit.
    # For newer entries, authorize only old->new body hash transitions.
    manifest_path = ROOT / "docs/curriculum/us-ccss-verified-body-repairs.v1.json"
    repair_doc = json.loads(manifest_path.read_text(encoding="utf8"))
    rows = repair_doc.get("repairs") or []
    approved = {(row["packCode"], row["lessonCode"]): row for row in rows}
    if (repair_doc.get("schemaVersion") != 1 or
        repair_doc.get("allowUnlistedContentChanges") is not False or
        len(rows) != 8 or len(approved) != 8):
        errors.append("Reviewed Common Core body repair allowlist is malformed")

    # Phase 04: additional *candidate* body repairs (NOT academic signoffs).
    # Accept only exact 7 old->new content digests, publisher SHA, identity
    # and metadata equality. Do not weaken the original storage-only parity.
    candidate_path = ROOT / "docs/curriculum/us-ccss-phase04-hs-content-repair-pilot.json"
    if candidate_path.exists():
        candidate_doc = json.loads(candidate_path.read_text(encoding="utf8"))
        candidates = candidate_doc.get("repairs") or []
        candidate_keys = set()
        if (candidate_doc.get("gate") != "NO-GO" or
            candidate_doc.get("scope") != "US-CCSS-MATH/CCSSM-2010/G1-G12" or
            len(candidates) != 7):
            errors.append("Phase04 candidate repair manifest malformed or signed")
        for entry in candidates:
            key = ("US-CCSS-MATH", entry["lessonCode"])
            if (key in approved or key in candidate_keys):
                errors.append(f"duplicate/overlapping Phase04 repair: {key}")
                continue
            candidate_keys.add(key)
            if (entry.get("humanAcademicSignoff") != "PENDING" or
                entry.get("contentPackFile", "").startswith(str(BASE) + "/") is False):
                errors.append(f"Phase04 repair approval/path metadata invalid: {key}")
            approved[key] = {
                "packCode": "US-CCSS-MATH",
                "lessonCode": entry["lessonCode"],
                "expectedOriginalCanonicalBodySha256": entry["expectedOldCanonicalBodySha256"],
                "expectedCorrectedCanonicalBodySha256": entry["correctedCanonicalBodySha256"],
                "expectedUnchangedSourceSha256": entry["unchangedPublisherSourceSha256"],
                "_candidateSourceFile": entry["contentPackFile"],
            }
    else:
        errors.append("Phase04 candidate repair manifest missing")

    # The new candidates must use the exact body fingerprint recomputed from
    # the seven English learner fields; the older 8 source allowlist entries
    # remain immutable and independently validated by their original rules.
    candidate_fields = ("title","explanation","keyConceptsAndRules",
                        "workedExamples","stepByStepSolutions","commonMistakes",
                        "quickSummary")
    for key, entry in approved.items():
        if "_candidateSourceFile" not in entry: continue
        if key not in baseline or key not in candidate: continue
        b, a = baseline[key], candidate[key]
        if len(a.get("translations") or []) != len(b.get("translations") or []):
            errors.append(f"Phase04 changed translation count: {key}")
            continue
        source_file = entry["_candidateSourceFile"]
        base_document = load_main(source_file)
        if not any(x.get("lessonCode")==key[1] for x in base_document.get("lessons",[])):
            errors.append(f"Phase04 wrong source file path: {key}")
        tr = next((x for x in a.get("translations",[]) if x.get("cultureCode")=="en"),None)
        before_tr = next((x for x in b.get("translations",[]) if x.get("cultureCode")=="en"),None)
        if tr is None or before_tr is None:
            errors.append(f"Phase04 English translation absent: {key}")
            continue
        # Before initial merge the original must actually differ; after merge,
        # an already-reviewed candidate SHA may legitimately match main.
        if tr == before_tr and b.get("canonicalBodySha256") != entry["expectedCorrectedCanonicalBodySha256"]:
            errors.append(f"Phase04 content changed without its expected baseline: {key}")
            continue
        if any(x.get("cultureCode")!="en" and x != y for x,y in
               zip(a.get("translations",[]),b.get("translations",[]))):
            errors.append(f"Phase04 non-English translation changed: {key}")
        actual = hashlib.sha256("\n".join(
            f"{field}:{tr[field]}" for field in candidate_fields).encode("utf8")).hexdigest()
        if actual != entry["expectedCorrectedCanonicalBodySha256"]:
            errors.append(f"Phase04 canonical SHA fails recomputation: {key}")

    changes_expected = set()
    for key, row in approved.items():
        if key not in baseline or key not in candidate:
            errors.append(f"approved repair lesson is missing: {key}")
            continue
        before, after = baseline[key], candidate[key]
        previous = before.get("CanonicalBodySha256", before.get("canonicalBodySha256"))
        current = after.get("CanonicalBodySha256", after.get("canonicalBodySha256"))
        source_old = before.get("SourceSha256", before.get("sourceSha256"))
        source_new = after.get("SourceSha256", after.get("sourceSha256"))
        old_hash = row["expectedOriginalCanonicalBodySha256"]
        new_hash = row["expectedCorrectedCanonicalBodySha256"]
        intermediate = row.get("previousCorrectedCanonicalBodySha256")
        if intermediate is not None and (intermediate == old_hash or intermediate == new_hash):
            errors.append(f"invalid intermediate body fingerprint: {key}")
        accepted_baselines = (old_hash, new_hash) + ((intermediate,) if intermediate else ())
        if old_hash == new_hash or previous not in accepted_baselines:
            errors.append(f"approved baseline hash changed unexpectedly: {key}")
        if current != new_hash or source_old != row["expectedUnchangedSourceSha256"] or source_new != source_old:
            errors.append(f"verified corrected body/source hash mismatch: {key}")
        if previous == old_hash or (intermediate is not None and previous == intermediate):
            changes_expected.add(key)
        if previous == new_hash and before.get("Translations", before.get("translations")) != after.get("Translations", after.get("translations")):
            errors.append(f"already-approved lesson edited again without a new body version: {key}")
        before_other, after_other = dict(before), dict(after)
        for record in (before_other, after_other):
            for field in ("Translations", "translations", "CanonicalBodySha256", "canonicalBodySha256"):
                record.pop(field, None)
        if before_other != after_other:
            errors.append(f"non-teaching metadata changed in reviewed repair: {key}")

    # The 149 pinned first-party G4 edits were expected *once*, when the
    # original main had the pre-authoring pack. After #578 promoted the
    # authorized blobs to main, exact equality is the required state instead.
    # validate_g4 already fails closed unless BOTH source/blueprint Git blobs
    # are the approved old->new pair or approved published->published pair.
    g4_lock = json.loads((
        ROOT / "docs/curriculum/g4-2021-owner-authored-revision-lock.json"
    ).read_text(encoding="utf8"))
    original_main = subprocess.check_output(
        ["git", "rev-parse", "origin/main"], cwd=ROOT, text=True
    ).strip() == g4_lock["originalMainCommit"]
    g4_new_changes = g4_owner_authored if original_main else set()
    expected_changes = changes_expected | g4_new_changes | g5_owner_content
    if set(changed) != expected_changes:
        errors.append(f"unapproved or missing teaching-body edits: changed={changed[:9]}, authorizedCount={len(expected_changes)}")
    unexpected_metadata = [key for key in baseline.keys() & candidate.keys()
                           if key not in approved and key not in g4_owner_authored
                           and key not in g5_owner_content and baseline[key] != candidate[key]]
    if unexpected_metadata:
        errors.append(f"unapproved lesson metadata changes: {unexpected_metadata[:6]}")
    print(f"MAIN_TO_V2_PARITY original={len(baseline)} matched={len(set(baseline)&set(candidate))} missing={len(missing)} changedBodies={len(changed)} newlyApproved={len(changes_expected)} g4OwnerAuthoredTechnicalOnly={len(g4_owner_authored)} g5OwnerContentOnly={len(g5_owner_content)} release=NO_GO")
    for e in errors: print("ERROR:",e)
    return 1 if errors else 0

if __name__ == "__main__":
    raise SystemExit(run())