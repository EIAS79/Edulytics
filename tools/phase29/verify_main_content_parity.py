#!/usr/bin/env python3
"""Check that V2 preserves existing main-branch canonical lesson identities and bodies.

Purely read-only. No curriculum scope additions, DB connections, or updates.
Use a full git checkout with origin/main available.
"""
import json
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
        if old_hash == new_hash or previous not in (old_hash, new_hash):
            errors.append(f"approved baseline hash changed unexpectedly: {key}")
        if current != new_hash or source_old != row["expectedUnchangedSourceSha256"] or source_new != source_old:
            errors.append(f"verified corrected body/source hash mismatch: {key}")
        if previous == old_hash:
            changes_expected.add(key)
        if previous == new_hash and before.get("Translations", before.get("translations")) != after.get("Translations", after.get("translations")):
            errors.append(f"already-approved lesson edited again without a new body version: {key}")
        before_other, after_other = dict(before), dict(after)
        for record in (before_other, after_other):
            for field in ("Translations", "translations", "CanonicalBodySha256", "canonicalBodySha256"):
                record.pop(field, None)
        if before_other != after_other:
            errors.append(f"non-teaching metadata changed in reviewed repair: {key}")

    if set(changed) != changes_expected:
        errors.append(f"unapproved or missing teaching-body edits: changed={changed[:9]}, authorized={sorted(changes_expected)}")
    unexpected_metadata = [key for key in baseline.keys() & candidate.keys()
                           if key not in approved and baseline[key] != candidate[key]]
    if unexpected_metadata:
        errors.append(f"unapproved lesson metadata changes: {unexpected_metadata[:6]}")
    print(f"MAIN_TO_V2_PARITY original={len(baseline)} matched={len(set(baseline)&set(candidate))} missing={len(missing)} changedBodies={len(changed)} newlyApproved={len(changes_expected)}")
    for e in errors: print("ERROR:",e)
    return 1 if errors else 0

if __name__ == "__main__":
    raise SystemExit(run())