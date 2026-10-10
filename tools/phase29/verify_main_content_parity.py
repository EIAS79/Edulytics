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

    # Never disable immutable content parity for routine migrations. Two
    # independently reviewed US-CCSS pedagogical corrections are the only
    # exceptions, pinned to both old and new body hashes and original source.
    manifest_path = ROOT / "docs/curriculum/us-ccss-verified-body-repairs.v1.json"
    repair_doc = json.loads(manifest_path.read_text(encoding="utf8"))
    approved = {(row["packCode"], row["lessonCode"]): row
                for row in repair_doc["repairs"]}
    if repair_doc.get("schemaVersion") != 1 or repair_doc.get("allowUnlistedContentChanges") is not False or len(approved) != 2:
        errors.append("Reviewed Common Core body repair allowlist is malformed")
    if changed and set(changed) != set(approved):
        errors.append(f"unapproved or missing body repair: actual={changed[:6]}, approved={sorted(approved)}")
    for key in changed:
        row = approved.get(key)
        if row is None:
            continue
        before, after = baseline[key], candidate[key]
        before_body = before.get("Translations", before.get("translations"))
        after_body = after.get("Translations", after.get("translations"))
        before_hash = before.get("CanonicalBodySha256", before.get("canonicalBodySha256"))
        after_hash = after.get("CanonicalBodySha256", after.get("canonicalBodySha256"))
        source_hash = before.get("SourceSha256", before.get("sourceSha256"))
        if (before_hash != row["expectedOriginalCanonicalBodySha256"] or
            after_hash != row["expectedCorrectedCanonicalBodySha256"] or
            source_hash != row["expectedUnchangedSourceSha256"]):
            errors.append(f"verified old/new body hash or original source mismatch: {key}")
        before_other, after_other = dict(before), dict(after)
        for record in (before_other, after_other):
            for field in ("Translations", "translations", "CanonicalBodySha256", "canonicalBodySha256"):
                record.pop(field, None)
        if before_other != after_other:
            errors.append(f"non-teaching metadata changed in reviewed repair: {key}")
        if not before_body or not after_body or before_body == after_body:
            errors.append(f"teaching body was not meaningfully changed: {key}")
    print(f"MAIN_TO_V2_PARITY original={len(baseline)} matched={len(set(baseline)&set(candidate))} missing={len(missing)} changedBodies={len(changed)} reviewedExceptions={len(changed) if not errors else 0}")
    for e in errors: print("ERROR:",e)
    return 1 if errors else 0

if __name__ == "__main__":
    raise SystemExit(run())