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
    # A separate exact-blob first-party Grade 6 candidate exception; it does
    # not confer source parity, human academic acceptance, or release rights.
    try:
        from verify_g6_owner_content_delta import validate as validate_g6
        g6_first_party = validate_g6()
    except (ValueError, KeyError, TypeError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"Grade 6 pinned first-party delta invalid: {exc}")
        g6_first_party = set()
    # Scope-pinned, grade-wide owner approval for the 102 Edulytics
    # first-party UAE G1 Arabic lesson bodies. It is *not* a source-parity
    # certificate and does not authorize a single unrelated lesson change.
    try:
        from verify_g1_owner_academic_content_delta import validate as validate_g1
        g1_owner_content = validate_g1()
    except (ValueError, KeyError, TypeError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"Grade1 pinned owner academic delta invalid: {exc}")
        g1_owner_content = set()
    # Scope-pinned 99-lesson owner release; no 2026-27 exercise parity claim.
    try:
        from verify_g2_owner_academic_content_delta import validate as validate_g2
        g2_owner_content = validate_g2()
    except (ValueError, KeyError, TypeError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"Grade2 pinned owner academic delta invalid: {exc}")
        g2_owner_content = set()
    # Exact SHA-pinned owner-accepted Grade 3 Common Arabic 109-lesson body
    # revision. This is first-party authoring, NOT official 2026-27 parity.
    try:
        from verify_g3_owner_academic_content_delta import validate as validate_g3
        g3_owner_content = validate_g3()
    except (ValueError, KeyError, TypeError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"Grade3 pinned owner academic delta invalid: {exc}")
        g3_owner_content = set()
    # Owner-approved UAE Grade5 first-party 127 Arabic translations and
    # exact blueprint revision; 2026-27 official source parity stays UNKNOWN.
    try:
        from verify_uae_g5_owner_academic_content_delta import validate as validate_uae_g5
        uae_g5_owner_content = validate_uae_g5()
    except (ValueError, KeyError, TypeError, OSError, subprocess.CalledProcessError) as exc:
        errors.append(f"UAE Grade5 exact-blob owner academic delta invalid: {exc}")
        uae_g5_owner_content = set()
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
            uae_g4_blueprint = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json"
            # The UAE G4 academic replacement is not a generic storage-only edit.
            # Accept ONLY the audited source and candidate Git blobs. Any other
            # alteration remains an error, including a future unreviewed rewrite.
            uae_g4_exact = False
            if path == uae_g4_blueprint:
                old_blob = subprocess.check_output(
                    ["git", "rev-parse", f"origin/main:{path}"],
                    cwd=ROOT, text=True).strip()
                new_blob = subprocess.check_output(
                    ["git", "hash-object", str(current)],
                    cwd=ROOT, text=True).strip()
                approval = json.loads((ROOT /
                    "artifacts/curriculum-delivery/UAE-MOE-MATH/G4/grade4-owner-approved-93-to-132-replacement.json"
                ).read_text(encoding="utf8"))
                blueprint_doc = json.loads(current.read_text(encoding="utf8"))
                uae_g4_exact = (
                    old_blob == "9886574a71cdc0ffd0bda58f6331ef9eb1d67eb2"
                    and new_blob == "d3aef0a9ca39e28cf91cdedee3c0b58d45baa036"
                    and approval.get("ownerRequestedReplacement") is True
                    and approval.get("scope") == "UAE Grade 4 Common only"
                    and blueprint_doc.get("PackCode") == "UAE-MOE-MATH"
                    and blueprint_doc.get("LogicalLevel") == 4
                    and blueprint_doc.get("Pathway") == "Common"
                    and blueprint_doc.get("AcquisitionDiagnostics", {}).get("UnitCount") == 16
                    and blueprint_doc.get("AcquisitionDiagnostics", {}).get("LessonCount") == 132
                )
            g1_blueprint = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g1-common-t1-ogl-v1.lesson-blueprint.json"
            g2_blueprint = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g2-common-t1-ogl-v1.lesson-blueprint.json"
            uae_g5_blueprint = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g5-general-t1-ogl-v1.lesson-blueprint.json"
            if not (
                uae_g4_exact or
                (path == g4_blueprint and len(g4_owner_authored) == 149) or
                (path == g1_blueprint and len(g1_owner_content) == 102) or
                (path == g2_blueprint and len(g2_owner_content) == 99) or
                (path == uae_g5_blueprint and len(uae_g5_owner_content) == 127)
            ):
                errors.append(f"changed original lesson blueprint: {path}")
        verified_blueprints += 1
    print(f"MAIN_TO_V2_BLUEPRINT_PARITY compared={verified_blueprints}")
    missing = sorted(set(baseline) - set(candidate))
    changed = sorted(key for key in baseline.keys() & candidate.keys()
                     if baseline[key].get("Translations", baseline[key].get("translations"))
                     != candidate[key].get("Translations", candidate[key].get("translations")))
    # Owner-authorized replacement is confined to the 93 original UAE G4 Common
    # identities. Each old identity must be audited, even when it disappears.
    # This only changes parity classification; the independent migration,
    # historical-record and source-authority release gates still apply.
    g4_legacy = {
        key for key in baseline
        if key[0] == "UAE-MOE-MATH" and
        key[1].startswith("PED:UAE-MOE-MATH:L4:COMMON:")
    }
    g4_new = {
        key for key in candidate
        if key[0] == "UAE-MOE-MATH" and
        key[1].startswith("PED:UAE-MOE-MATH:L4:COMMON:")
    }
    g4_missing = set(missing) & g4_legacy
    g4_changed = set(changed) & g4_legacy
    g4_replacement_authorized = False
    try:
        g4_dir = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G4"
        approval = json.loads((g4_dir / "grade4-owner-approved-93-to-132-replacement.json").read_text(encoding="utf8"))
        migration = json.loads((g4_dir / "grade4-migration-validation.json").read_text(encoding="utf8"))
        # Once the exact 132-lesson owner-approved candidate is merged into
        # main, the old 93 IDs are no longer in the Git baseline. A follow-up
        # production hotfix must instead prove it has retained the precise
        # approved released blueprint, without accepting an arbitrary rewrite.
        released_blueprint = (
            "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/"
            "uae-g4-common-t1-ogl-v1.lesson-blueprint.json"
        )
        approved_blob = "d3aef0a9ca39e28cf91cdedee3c0b58d45baa036"
        already_released = (
            approval.get("ownerRequestedReplacement") is True
            and approval.get("scope") == "UAE Grade 4 Common only"
            and approval.get("legacyPedagogicalLessonCount") == 93
            and approval.get("newPedagogicalLessonCount") == 132
            and migration.get("oldLessonCount") == 93
            and migration.get("newLessonCount") == 132
            and len(g4_legacy) == 132
            and g4_legacy == g4_new
            and not g4_missing
            and not g4_changed
            and subprocess.check_output(
                ["git", "rev-parse", f"origin/main:{released_blueprint}"],
                cwd=ROOT, text=True).strip() == approved_blob
            and subprocess.check_output(
                ["git", "hash-object", str(ROOT / released_blueprint)],
                cwd=ROOT, text=True).strip() == approved_blob
        )
        if already_released:
            g4_replacement_authorized = True
        else:
            rows = migration["mapping"]
            audited = {}
            for row in rows:
                code = row["oldLessonCode"]
                key = ("UAE-MOE-MATH", code)
                if key in audited:
                    raise ValueError(f"duplicate legacy mapping: {code}")
                audited[key] = row
                if key not in baseline:
                    raise ValueError(f"unrecognized legacy lesson: {code}")
                if row.get("classification") == "LEGACY_ID_NOT_PRESENT":
                    if key in candidate or row.get("currentLessonCode") is not None:
                        raise ValueError(f"absent legacy ID unexpectedly reused: {code}")
                elif row.get("classification") == "SAME_ID_DIFFERENT_TITLE_COLLISION":
                    if key not in candidate or row.get("currentLessonCode") != code:
                        raise ValueError(f"changed legacy ID not accounted for: {code}")
                else:
                    raise ValueError(f"unreviewed legacy classification: {code}")
            if not (
                approval.get("ownerRequestedReplacement") is True
                and approval.get("scope") == "UAE Grade 4 Common only"
                and approval.get("legacyPedagogicalLessonCount") == 93
                and approval.get("newPedagogicalLessonCount") == 132
                and approval.get("targetFrameworkVersionCode") == "MOE-2026-2027-T1"
                and migration.get("oldLessonCount") == 93
                and migration.get("newLessonCount") == 132
                and migration.get("identityCollisionCount") == 83
                and migration.get("absentOldIdCount") == 10
                and len(g4_legacy) == 93
                and len(g4_new) == 132
                and len(rows) == 93
                and set(audited) == g4_legacy
                and len(g4_missing) == 10
                and len(g4_changed) == 83
                and all(k in g4_changed for k in g4_legacy - g4_missing)
                and all(audited[k]["classification"] == "LEGACY_ID_NOT_PRESENT" for k in g4_missing)
            ):
                raise ValueError("93-to-132 replacement audit does not match exact candidate delta")
            g4_replacement_authorized = True
    except (KeyError, ValueError, TypeError, OSError, json.JSONDecodeError) as exc:
        errors.append(f"UAE Grade4 narrowly-scoped owner replacement audit failed: {exc}")
    unexpected_missing = set(missing) - (g4_missing if g4_replacement_authorized else set())
    if unexpected_missing:
        errors.append(f"missing original lesson identities outside accepted Grade4 replacement: {sorted(unexpected_missing)[:5]}")


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
                entry.get("contentPackFile", "").startswith(BASE.as_posix() + "/") is False):
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
    expected_changes = changes_expected | g4_new_changes | g5_owner_content | g6_first_party | g1_owner_content | g2_owner_content | g3_owner_content | uae_g5_owner_content | (g4_changed if g4_replacement_authorized else set())
    if set(changed) != expected_changes:
        errors.append(f"unapproved or missing teaching-body edits: changed={changed[:9]}, authorizedCount={len(expected_changes)}")
    unexpected_metadata = [key for key in baseline.keys() & candidate.keys()
                           if key not in approved and key not in g4_owner_authored
                           and (not g4_replacement_authorized or key not in g4_changed)
                           and key not in g5_owner_content and key not in g6_first_party
                           and key not in g1_owner_content and key not in g2_owner_content and key not in g3_owner_content and key not in uae_g5_owner_content and baseline[key] != candidate[key]]
    if unexpected_metadata:
        errors.append(f"unapproved lesson metadata changes: {unexpected_metadata[:6]}")
    print(f"MAIN_TO_V2_PARITY original={len(baseline)} matched={len(set(baseline)&set(candidate))} missing={len(missing)} changedBodies={len(changed)} newlyApproved={len(changes_expected)} g4OwnerAuthoredTechnicalOnly={len(g4_owner_authored)} g5OwnerContentOnly={len(g5_owner_content)} g6FirstPartyTechnicalOnly={len(g6_first_party)} g1OwnerAcademicApprovedUnknownSourceParity={len(g1_owner_content)} g2OwnerAcademicApprovedUnknownSourceParity={len(g2_owner_content)} g3OwnerAcademicApprovedUnknownSourceParity={len(g3_owner_content)} uaeG5OwnerAcademicApprovedUnknownSourceParity={len(uae_g5_owner_content)} release=NO_GO_SOURCE_PARITY_NOT_CERTIFIED")
    for e in errors: print("ERROR:",e)
    return 1 if errors else 0

if __name__ == "__main__":
    raise SystemExit(run())