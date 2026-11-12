import xml.etree.ElementTree as ET
#!/usr/bin/env python3
"""Fail-closed, evidence-based Stage D preflight for UAE Grade 4 Common.

This script audits repository evidence, never claims authenticated E2E or
production status from static files. Run with --phase structural/source/final.
"""
import argparse
import hashlib
import json
import pathlib
import re
import subprocess
from datetime import datetime, timezone

ROOT = pathlib.Path(__file__).resolve().parents[2]
G = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G4"
MP = ROOT / "docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json"
BP = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json"
CP = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json"
REG = ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
SKILLS = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
CON = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
OUTPUT = ROOT / "artifacts/uae-g4-stage-d"

def load(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))

def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()

def candidate_sha():
    p = subprocess.run(["git", "rev-parse", "HEAD"], cwd=ROOT,
                       capture_output=True, text=True, check=False)
    return p.stdout.strip() if p.returncode == 0 else None

def report(phase, errors, checks, details):
    OUTPUT.mkdir(parents=True, exist_ok=True)
    data = {
        "phase": phase, "grade": 4, "pathway": "Common",
        "candidateSha": candidate_sha(),
        "timestampUtc": datetime.now(timezone.utc).isoformat(),
        "status": "PASS" if not errors else "BLOCKED",
        "errors": errors, "checks": checks, "details": details,
        "notAuthenticatedE2eEvidence": True,
        "notPublicationEvidence": True,
    }
    path = OUTPUT / f"grade4-{phase}-gate.json"
    path.write_text(json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps({"phase": phase, "status": data["status"], "errors": errors,
                      "checks": checks, "candidateSha": data["candidateSha"]},
                     ensure_ascii=False, indent=2))
    print("EVIDENCE", path)
    return not errors

def structural():
    errors, checks, details = [], {}, {}
    for p in (MP, BP, CP, REG, CON):
        if not p.is_file():
            errors.append(f"Missing {p.relative_to(ROOT)}")
    if errors:
        return report("structural", errors, checks, details)
    m, b, c, reg = load(MP), load(BP), load(CP), load(REG)
    codes = [x["LessonCode"] for x in b["Lessons"]]
    content_codes = [x["LessonCode"] for x in c["Lessons"]]
    unit_counts = [len(x["ls"]) for x in m["Units"]]
    checks["manifestUnitCount"] = len(m["Units"])
    checks["manifestLessonCount"] = m["LessonCount"]
    checks["unitLessonCountTotal"] = sum(unit_counts)
    checks["blueprintLessonCount"] = len(codes)
    checks["contentPackLessonCount"] = len(content_codes)
    checks["uniqueBlueprintCodes"] = len(set(codes))
    checks["uniqueContentCodes"] = len(set(content_codes))
    checks["orderedBlueprint"] = [x["SortOrder"] for x in b["Lessons"]] == list(range(1, 133))
    checks["unitCounts"] = unit_counts
    checks["curriculumUnit12"] = m["Units"][11]["title"]
    checks["arabicAcademicLanguage"] = c.get("AcademicLanguage") == "ar"
    checks["arabicTranslationsPresent"] = sum(
        1 for lesson in c["Lessons"]
        if any(t.get("CultureCode") == "ar" for t in lesson.get("Translations", [])))
    checks["contentReleaseStatus"] = c.get("Status")
    checks["blueprintMetadataUnitCount"] = b.get("AcquisitionDiagnostics", {}).get("UnitCount")
    checks["blueprintMetadataLessonCount"] = b.get("AcquisitionDiagnostics", {}).get("LessonCount")
    if not checks["arabicAcademicLanguage"] or checks["arabicTranslationsPresent"] != 132:
        errors.append("Arabic-first Grade 4 content metadata or translations are incomplete")
    # Published is the learner runtime visibility status inside a candidate.
    # It is NOT evidence of a production deployment, which final CI verifies.
    checks["contentReadyForLearnerRuntime"] = checks["contentReleaseStatus"] == "Published"
    if not checks["contentReadyForLearnerRuntime"]:
        errors.append("The Grade4 candidate cannot be exercised by learners while canonical content remains Draft")
    if checks["blueprintMetadataUnitCount"] != 16 or checks["blueprintMetadataLessonCount"] != 132:
        errors.append("Blueprint acquisition diagnostics retain obsolete 14-unit/93-lesson scope")
    expected = [6,9,8,11,6,11,9,10,9,11,10,7,3,9,5,8]
    if unit_counts != expected: errors.append("Unit-wise lesson counts do not match reconciled 16-unit index")
    if not (m["LessonCount"] == len(codes) == len(content_codes) == 132):
        errors.append("Manifest / blueprint / content pack count discrepancy")
    if len(set(codes)) != 132 or set(codes) != set(content_codes):
        errors.append("Missing, duplicate, or extra LessonCodes")
    if not checks["orderedBlueprint"]:
        errors.append("Blueprint order differs from 1..132")
    if m["Units"][11]["title"] != "الهندسة (1)":
        errors.append("Unit 12 Geometry (1) missing")
    contracts = CON.read_text(encoding="utf-8-sig")
    contract_missing = [code for code in codes if f'byLesson["{code}"]' not in contracts]
    checks["presentPracticeContracts"] = 132 - len(contract_missing)
    if contract_missing:
        errors.append(f"{len(contract_missing)} lesson Practice contracts missing")
        details["practiceContractMissing"] = contract_missing
    known_skill_ids = {x["id"] for x in load(SKILLS)["skills"]}
    support_file = G / "grade4-reviewed-supporting-skill-reuse.json"
    support_doc = load(support_file) if support_file.is_file() else {"lessons": []}
    reviewed_by_code = {x["lessonCode"]: x for x in support_doc["lessons"]}
    family_owners = {x["id"]: x["skillId"] for x in reg["families"]}
    pattern = re.compile(
        r'byLesson\["(PED:UAE-MOE-MATH:L4:COMMON:\d{2}:\d{2})"\]\s*'
        r'=\s*new LessonPracticeContract\(\s*"[^"]+",\s*'
        r'"([^"]+)",\s*"([^"]+)",\s*\[([^\]]+)\]',
        re.S)
    contract_rows = pattern.findall(contracts)
    unknown_contract_skills, cross_skill_family_uses = [], []
    reviewed_supporting_reuse = []
    for lesson_code, skill, mechanic, listed in contract_rows:
        family_list = re.findall(r'"([^"]+)"', listed)
        if skill not in known_skill_ids:
            unknown_contract_skills.append(
                {"lessonCode":lesson_code,"skillId":skill,"strategy":mechanic})
        start = contracts.find('byLesson["'+lesson_code+'"]')
        stop = contracts.find(';', start)
        declaration = contracts[start:stop+1]
        extra = re.search(r'SkillIds\s*=\s*\[([^\]]+)\]',declaration)
        declared = {skill}
        if extra:
            declared.update(re.findall(r'"([^"]+)"',extra.group(1)))
        review = reviewed_by_code.get(lesson_code)
        for f in family_list:
            family_skill = family_owners.get(f)
            if family_skill != skill:
                is_documented = bool(
                    review and review["primarySkillId"] == skill and
                    f in review["supportedFamilyIds"] and
                    family_skill in review["declaredSkills"] and
                    family_skill in declared and
                    all(k in known_skill_ids for k in declared) and
                    review.get("sourceLocator") and
                    review.get("mathematicalRationale"))
                if is_documented:
                    reviewed_supporting_reuse.append({
                        "lessonCode":lesson_code,"questionFamily":f,
                        "familySkillId":family_skill})
                else:
                    cross_skill_family_uses.append({
                        "lessonCode":lesson_code,"contractSkillId":skill,
                        "questionFamily":f,"registeredFamilySkillId":family_skill})
    checks["parsedPracticeContracts"] = len(contract_rows)
    checks["unknownContractSkillIdCount"] = len(unknown_contract_skills)
    checks["unreviewedCrossSkillFamilyCount"] = len(cross_skill_family_uses)
    checks["explicitReviewedSupportingFamilyReuseCount"] = len(reviewed_supporting_reuse)
    details["reviewedSupportingPracticeOnly"] = reviewed_supporting_reuse
    details["unknownContractSkillIds"] = unknown_contract_skills
    details["crossSkillFamilyReuseRequiringEvidence"] = cross_skill_family_uses
    if len(contract_rows)!=132:
        errors.append("Exact Grade4 contract parser does not resolve all 132 contracts")
    if unknown_contract_skills:
        errors.append(
            f"{len(unknown_contract_skills)} Practice contracts reference unknown SkillIds")
    if cross_skill_family_uses:
        errors.append(
            f"{len(cross_skill_family_uses)} cross-skill question-family usages lack reviewed equivalence evidence")
    family_ids = {x["id"] for x in reg["families"]}
    details["questionFamilyRegistrySize"] = len(family_ids)
    checkpoint_ok, absent, mismatched = 0, [], []
    for i, code in enumerate(codes, 1):
        f = G / f"lesson-{i:03}-stage-a-c-checkpoint.json"
        if not f.exists():
            absent.append(i)
            continue
        x = load(f)
        if x.get("LessonOrdinal") == i and x.get("LessonCode") == code and x.get("Status") == "READY_FOR_STAGE_D":
            checkpoint_ok += 1
        else:
            mismatched.append(i)
    checks["readyAbcCheckpoints"] = checkpoint_ok
    details["absentCheckpoints"] = absent
    details["mismatchedCheckpoints"] = mismatched
    if checkpoint_ok != 132:
        errors.append(f"Only {checkpoint_ok}/132 A-B-C checkpoints match curriculum index")
    details["sourceHashManifest"] = sha(MP)
    details["blueprintHash"] = sha(BP)
    details["contentHash"] = sha(CP)
    return report("structural", errors, checks, details)

def source():
    errors, checks, details = [], {}, {}
    handoff = G / "grade4-a-c-handoff.md"
    checks["sourceHandoffPresent"] = handoff.is_file()
    evidence = G / "grade4-a-c-source-handoff.json"
    checks["sourceSnapshotHandoffPresent"] = evidence.is_file()
    if not handoff.exists() or not evidence.exists():
        errors.append("Missing source handoff and hashed evidence manifest")
    else:
        handoff_data = load(evidence)
        checks["ownerAcademicApprovalRecorded"] = handoff_data.get("OwnerAcademicApproval", {}).get("approved") is True
        checks["all132SourceLocatorsPresent"] = len(handoff_data.get("lessonMappings", [])) == 132
        checks["annualPlanSha256Present"] = bool(handoff_data.get("annualPlan", {}).get("sha256"))
        checks["sourceSnapshotIdRecorded"] = bool(handoff_data.get("SourceSnapshotId"))
        # A third-party annual-plan file hash authenticates the downloaded
        # bytes, not the ministry's issuance or the 2026-27 prescribed scope.
        # Never turn owner academic signoff into fabricated source authority.
        annual_plan = handoff_data.get("annualPlan", {})
        checks["annualPlanAuthorityVerified"] = annual_plan.get("publisherVerification") in (
            "OFFICIAL_MOE_ORIGIN_VERIFIED",
            "SIGNED_MOE",
            "OFFICIAL_MOE_ARCHIVE_VERIFIED",
        )
        lesson_mappings = handoff_data.get("lessonMappings", [])
        missing_rows = [
            x.get("ordinal") for x in lesson_mappings
            if not isinstance(x.get("annualPlanExactRowId"), str)
            or not x["annualPlanExactRowId"].strip()
            or x["annualPlanExactRowId"].startswith("NOT_")
        ]
        checks["all132AnnualPlanRowsReconciled"] = (
            len(lesson_mappings) == 132 and not missing_rows)
        details["unreconciledAnnualPlanOrdinals"] = missing_rows
        details["annualPlanPublisherVerification"] = annual_plan.get("publisherVerification")
        if not checks["annualPlanAuthorityVerified"]:
            errors.append("Annual 2026-27 plan has no verified ministry provenance; recorded hash alone is not authority")
        if not checks["all132AnnualPlanRowsReconciled"]:
            errors.append(f"{len(missing_rows)} lessons lack a verified exact 2026-27 annual-plan row mapping")
        if not all(checks.get(x, False) for x in [
          "ownerAcademicApprovalRecorded","all132SourceLocatorsPresent",
          "annualPlanSha256Present","sourceSnapshotIdRecorded"]):
            errors.append("Incomplete academic owner approval or source provenance snapshot")
    audit = G / "full-grade-stage-a-c-132-lesson-audit.json"
    if audit.exists():
        source_audit = load(audit)
        checks["auditCount"] = source_audit.get("LessonCount")
        checks["auditEntries"] = len(source_audit.get("Lessons", []))
        details["annualPlanOrigin"] = source_audit.get("AnnualPlanEvidence")
        if "not independently verified" in str(source_audit.get("AnnualPlanEvidence", "")).lower():
            checks["ministrySignatureVerified"] = False
            details["annualPlanCaveat"] = "Third-party-hosted annual plan, no ministry signature claim"
        lacking_pages = [x["Ordinal"] for x in source_audit.get("Lessons", [])
                         if not x.get("Source")]
        details["missingSourceLocatorOrdinals"] = lacking_pages
        if lacking_pages:
            errors.append(f"{len(lacking_pages)} lesson source locators are empty in handoff audit")
    else:
        errors.append("Missing full-grade-stage-a-c-132-lesson-audit.json")
    rights = G / "grade4-rights-release-clearance.json"
    checks["publicationRightsReviewPresent"] = rights.exists()
    checks["publicationRightsCleared"] = rights.exists() and load(rights).get("allowsDeployment") is True
    if not checks["publicationRightsCleared"]:
        errors.append("Learner-visible third-party image/text rights not cleared for production")
    checks["sourceSnapshotIdPresent"] = bool(
        handoff.exists() and evidence.exists() and
        load(evidence).get("SourceSnapshotId") in handoff.read_text(encoding="utf-8"))
    if not checks["sourceSnapshotIdPresent"]:
        errors.append("SourceSnapshotId not consistently present in source snapshot and handoff")
    return report("source", errors, checks, details)

def math():
    errors, checks, details = [], {}, {}
    folder = OUTPUT / "trx"
    candidates = sorted(folder.glob("*.trx")) if folder.exists() else []
    checks["trxFiles"] = len(candidates)
    if len(candidates) != 1:
        return report("math", ["One Grade4 TRX result is required from same candidate build"], checks, details)
    try:
        root = ET.parse(candidates[0]).getroot()
        summary = next((node for node in root.iter() if node.tag.rsplit("}",1)[-1] == "Counters"), None)
        if summary is None:
            errors.append("TRX file has no Counters element")
        else:
            counts = {k:int(v) for k,v in summary.attrib.items() if v.isdigit()}
            details["trxCounters"] = counts
            checks["passed"] = counts.get("passed",0)
            checks["failed"] = counts.get("failed",-1)
            checks["total"] = counts.get("total",0)
            checks["skipped"] = counts.get("notExecuted",0)
            if checks["passed"] < 645 or checks["total"] != checks["passed"] or checks["failed"] or checks["skipped"]:
                errors.append("Grade4 filtered CI tests did not satisfy minimum 645/645 passing and zero failing/skipped")
    except (OSError, ET.ParseError, ValueError) as ex:
        errors.append("Failed to parse Grade4 TRX: " + str(ex))
    return report("math", errors, checks, details)

def migration():
    errors, checks, details = [], {}, {}
    p = G / "grade4-production-migration-risk.json"
    if not p.is_file():
        return report("migration", ["Missing Grade 4 isolated database migration risk record"], checks, details)
    data = load(p)
    blueprint = load(BP)
    checks["productionLegacyLessonCount"] = data.get("legacyGrade4PedagogicalLessons")
    checks["preservedCanonicalContentsExpected"] = data.get("legacyGrade4CanonicalContents")
    checks["crossGradeFrameworkSchoolAdoptions"] = data.get("frameworkVersionSchoolAdoptionsAcrossAllGrades")
    checks["targetVersion"] = blueprint.get("VersionCode")
    checks["legacyVersion"] = data.get("legacyFrameworkVersionCode")
    checks["differentFrameworkVersion"] = blueprint.get("VersionCode") != data.get("legacyFrameworkVersionCode")
    checks["isolatedMigrationAndRollbackPassed"] = data.get("isolatedDryRunCompleted") is True
    details["isolatedNeonBranchRequest"] = data.get("isolatedNeonBranchRequest")
    details["identityCollisions"] = data.get("sameCodeDifferentTitleOldLessons")
    details["oldLessonIdsAbsent"] = data.get("oldLessonIdsAbsentFromNewBlueprint")
    replacement = G / "grade4-owner-approved-93-to-132-replacement.json"
    checks["ownerApprovedSameVersionReplacementPlanPresent"] = replacement.exists()
    if checks["ownerApprovedSameVersionReplacementPlanPresent"]:
        plan = load(replacement)
        checks["ownerApprovedSameVersionReplacementBackupRecorded"] = (
            plan.get("ownerRequestedReplacement") is True and
            plan.get("legacyPedagogicalLessonCount") == 93 and
            plan.get("newPedagogicalLessonCount") == 132 and
            len(plan.get("localBackupSha256", {})) == 2 and
            plan.get("scope") == "UAE Grade 4 Common only")
    else:
        checks["ownerApprovedSameVersionReplacementBackupRecorded"] = False
    if not checks["differentFrameworkVersion"] and not checks["ownerApprovedSameVersionReplacementBackupRecorded"]:
        errors.append("Same-framework Grade 4 replacement lacks owner approval and exact local legacy backup proof")
    if not checks["isolatedMigrationAndRollbackPassed"]:
        errors.append("Owner-approved 93-to-132 replacement has not been rehearsed and rolled back safely; production replacement remains forbidden")
    return report("migration", errors, checks, details)

def final():
    errors, checks, details = [], {}, {}
    required = ["structural", "source", "math", "migration",
                "authenticated-e2e", "arabic-visual", "rights", "ci"]
    for name in required:
        p = OUTPUT / f"grade4-{name}-gate.json"
        valid = p.is_file() and load(p).get("status") == "PASS"
        checks[name] = valid
        if not valid:
            errors.append(f"Final same-SHA gate missing or blocked: {name}")
    if len({load(OUTPUT / f"grade4-{k}-gate.json").get("candidateSha")
            for k in required if (OUTPUT / f"grade4-{k}-gate.json").exists()}) > 1:
        errors.append("Gate artifacts reference different CandidateSha values")
    return report("final", errors, checks, details)

if __name__ == "__main__":
    p = argparse.ArgumentParser()
    p.add_argument("--phase", choices=["structural", "source", "migration", "math", "final"], required=True)
    a = p.parse_args()
    good = {"structural": structural, "source": source, "migration": migration, "math": math, "final": final}[a.phase]()
    raise SystemExit(0 if good else 1)
