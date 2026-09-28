#!/usr/bin/env python3
"""
Advanced Mathematics M0 capability inventory.

This audit is intentionally conservative. It distinguishes mathematical-engine
evidence from learner-facing readiness and never upgrades a family merely
because a solver exists.

The report is deterministic and suitable for CI artifacts.
"""

from __future__ import annotations

import argparse
import json
import re
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
REGISTRY = ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
CAPABILITIES = ROOT / "src/Edulytics.Core/Mathematics/Skills/capability-registry.v1.json"
INTERACTIONS = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeInteractionRegistry.cs"
VISUAL_RENDERER = ROOT / "src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.cs"
PRACTICE_CONTRACTS = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
ASSESSMENT_CONTRACTS = ROOT / "src/Edulytics.Core/Mathematics/Assessment/Stage19AssessmentSkillContracts.cs"
CURRICULUM_ROOT = ROOT / "src/Edulytics.Core/Mathematics/Curriculum"
GENERATION_ROOT = ROOT / "src/Edulytics.Services/Mathematics/Generation"
SOLVING_ROOT = ROOT / "src/Edulytics.Services/Mathematics/Solving"
VERIFY_ROOT = ROOT / "src/Edulytics.Services/Mathematics/Verification"
OUTPUT = ROOT / "artifacts/math-intelligence/advanced-math-capability-matrix.json"
MARKDOWN = ROOT / "artifacts/math-intelligence/advanced-math-capability-matrix.md"

ADVANCED_PREFIXES = (
    "functions.",
    "calculus.",
    "trigonometry.",
    "geometry.coordinate.",
    "geometry.analytic.",
    "geometry.circle.",
    "geometry.conic.",
    "vectors.",
    "matrices.",
    "complex.",
    "sequences.",
    "series.",
    "limits.",
    "probability.",
    "statistics.",
    "numerical.",
    "mechanics.",
    "supporting.functions.",
    "supporting.calculus.",
    "supporting.trigonometry.",
    "supporting.geometry.analytic.",
    "supporting.vectors.",
    "supporting.matrices.",
    "supporting.complex.",
    "supporting.sequences.",
    "supporting.statistics.",
    "supporting.probability.",
    "supporting.numerical.",
    "supporting.mechanics.",
)

NON_VISUAL_REPRESENTATIONS = {
    "symbolic",
    "contextual",
    "textual_constraint",
}

SUPPORTED_PRIMARY_ANSWER_TYPES = {
    "side_label",
    "boolean",
    "ordered_labels",
    "relation",
    "exact_integer",
    "exact_scalar",
    "exact_rational",
    "exact_vector",
    "exact_interval_or_union",
    "quotient_and_remainder",
    "enum_text",
    "contract_defined",
}

EXAM_MANIFESTS = (
    "stage23-igcse-extended-gate-manifest.v1.json",
    "stage24-as-a-level-9709-gate-manifest.v1.json",
    "stage25-ib-aa-hl-style-gate-manifest.v1.json",
)


def load_json(path: Path) -> dict[str, Any]:
    return json.loads(path.read_text(encoding="utf-8"))


def all_text(root: Path, pattern: str = "*.cs") -> str:
    if not root.exists():
        return ""
    return "\n".join(
        p.read_text(encoding="utf-8")
        for p in sorted(root.rglob(pattern))
    )


def literal_occurrences(text: str, literal: str) -> int:
    return text.count(literal)


def family_domain(family_id: str) -> str:
    parts = family_id.split(".")
    if parts and parts[0] == "supporting" and len(parts) > 1:
        return parts[1]
    return parts[0] if parts else "unknown"


def is_advanced_family(family_id: str) -> bool:
    return family_id.startswith(ADVANCED_PREFIXES)


def visual_required(representations: list[str]) -> bool:
    return any(r not in NON_VISUAL_REPRESENTATIONS for r in representations)


def load_capability_ids() -> set[str]:
    document = load_json(CAPABILITIES)
    rows = document.get("capabilities", [])
    return {
        str(row.get("id", "")).strip()
        for row in rows
        if str(row.get("id", "")).strip()
    }


def curriculum_texts() -> dict[str, str]:
    result: dict[str, str] = {}
    for path in sorted(CURRICULUM_ROOT.glob("*")):
        if path.suffix.lower() not in {".json", ".cs"}:
            continue
        result[path.name] = path.read_text(encoding="utf-8")
    return result


def exact_mapping_evidence(
    family_id: str,
    curriculum_files: dict[str, str],
    practice_contract_text: str,
) -> tuple[int, list[str]]:
    files: list[str] = []
    count = literal_occurrences(practice_contract_text, family_id)
    if count:
        files.append(PRACTICE_CONTRACTS.name)

    for name, text in curriculum_files.items():
        occurrences = literal_occurrences(text, family_id)
        if occurrences:
            count += occurrences
            files.append(name)

    return count, sorted(set(files))


def source_evidence(
    family_id: str,
    skill_id: str,
    generation_text: str,
    solving_text: str,
    verification_text: str,
) -> dict[str, bool]:
    # Family literal evidence is strongest. Skill-token evidence is a bounded
    # fallback because some exact engines route a SkillContract rather than
    # repeating every family id in each solver/verifier class.
    skill_tokens = [
        token
        for token in re.split(r"[^A-Za-z0-9]+", skill_id)
        if len(token) >= 5
    ]
    family_tokens = [
        token
        for token in re.split(r"[^A-Za-z0-9]+", family_id)
        if len(token) >= 5
    ]
    tokens = sorted(set(skill_tokens + family_tokens))

    def has(text: str) -> bool:
        if family_id in text:
            return True
        lowered = text.lower()
        meaningful = [t.lower() for t in tokens]
        return bool(meaningful) and sum(t in lowered for t in meaningful) >= min(2, len(meaningful))

    return {
        "generatorSourceEvidence": has(generation_text),
        "solverSourceEvidence": has(solving_text),
        "verifierSourceEvidence": has(verification_text),
    }


def exam_certification_evidence(
    family_id: str,
    curriculum_files: dict[str, str],
) -> list[str]:
    return [
        name
        for name in EXAM_MANIFESTS
        if family_id in curriculum_files.get(name, "")
    ]


def readiness(
    *,
    declared_status: str,
    required_capabilities_complete: bool,
    generator_evidence: bool,
    solver_evidence: bool,
    verifier_evidence: bool,
    lesson_practice_routing: bool,
    interaction_supported: bool,
    visual_needed: bool,
    visual_covered: bool,
    mapping_count: int,
    assessment_evidence: bool,
    exam_evidence: bool,
) -> tuple[str, list[str]]:
    blockers: list[str] = []

    if not required_capabilities_complete:
        blockers.append("MISSING_REQUIRED_CAPABILITY")
    if not generator_evidence:
        blockers.append("GENERATOR_SOURCE_NOT_PROVEN")
    if not solver_evidence:
        blockers.append("SOLVER_SOURCE_NOT_PROVEN")
    if not verifier_evidence:
        blockers.append("VERIFIER_SOURCE_NOT_PROVEN")

    engine_closed = not any(
        b in blockers
        for b in (
            "MISSING_REQUIRED_CAPABILITY",
            "GENERATOR_SOURCE_NOT_PROVEN",
            "SOLVER_SOURCE_NOT_PROVEN",
            "VERIFIER_SOURCE_NOT_PROVEN",
        )
    )

    if not engine_closed:
        return "ENGINE_INCOMPLETE", blockers

    if not lesson_practice_routing:
        blockers.append("LESSON_PRACTICE_ROUTING_DISABLED")
        return "GENERATION_VERIFIED", blockers

    if not interaction_supported:
        blockers.append("LEARNER_INTERACTION_INCOMPLETE")
    if visual_needed and not visual_covered:
        blockers.append("VISUAL_RENDERER_INCOMPLETE")
    if mapping_count <= 0:
        blockers.append("EXACT_CURRICULUM_MAPPING_NOT_PROVEN")

    if blockers:
        return "PRACTICE_SURFACE_INCOMPLETE", blockers

    if not assessment_evidence:
        return "PRACTICE_READY_VERIFIED", ["ASSESSMENT_CERTIFICATION_NOT_PROVEN"]

    if not exam_evidence:
        return "ASSESSMENT_READY_VERIFIED", ["EXAM_CERTIFICATION_NOT_PROVEN"]

    return "EXAM_READY_VERIFIED", []


def build_report() -> dict[str, Any]:
    question_registry = load_json(REGISTRY)
    capability_ids = load_capability_ids()
    interaction_text = INTERACTIONS.read_text(encoding="utf-8")
    visual_text = VISUAL_RENDERER.read_text(encoding="utf-8")
    practice_contract_text = PRACTICE_CONTRACTS.read_text(encoding="utf-8")
    assessment_text = (
        ASSESSMENT_CONTRACTS.read_text(encoding="utf-8")
        if ASSESSMENT_CONTRACTS.exists()
        else ""
    )
    curricula = curriculum_texts()

    generation_text = all_text(GENERATION_ROOT)
    solving_text = all_text(SOLVING_ROOT)
    verification_text = all_text(VERIFY_ROOT)

    rows: list[dict[str, Any]] = []
    registry_ids: set[str] = set()
    duplicate_ids: list[str] = []

    for family in question_registry.get("families", []):
        family_id = str(family.get("id", "")).strip()
        if not family_id:
            continue

        if family_id in registry_ids:
            duplicate_ids.append(family_id)
        registry_ids.add(family_id)

        if not is_advanced_family(family_id):
            continue

        skill_id = str(family.get("skillId", "")).strip()
        required = [
            str(value).strip()
            for value in family.get("requiredCapabilities", [])
            if str(value).strip()
        ]
        missing = sorted(set(required) - capability_ids)
        representations = [
            str(value).strip()
            for value in family.get("representations", [])
            if str(value).strip()
        ]
        answer_type = str(family.get("answerType", "")).strip()
        lesson_routing = bool(family.get("lessonPracticeRouting", False))
        production_routing = bool(family.get("productionRouting", False))
        declared_status = str(family.get("status", "")).strip()

        mapping_count, mapping_files = exact_mapping_evidence(
            family_id,
            curricula,
            practice_contract_text,
        )
        sources = source_evidence(
            family_id,
            skill_id,
            generation_text,
            solving_text,
            verification_text,
        )

        interaction_supported = (
            answer_type in SUPPORTED_PRIMARY_ANSWER_TYPES
            and (
                not lesson_routing
                or family_id in interaction_text
                or answer_type in interaction_text
            )
        )
        needs_visual = visual_required(representations)
        visual_covered = (
            not needs_visual
            or family_id in visual_text
        )
        assessment_evidence = family_id in assessment_text
        exam_files = exam_certification_evidence(
            family_id,
            curricula,
        )

        state, blockers = readiness(
            declared_status=declared_status,
            required_capabilities_complete=not missing,
            generator_evidence=sources["generatorSourceEvidence"],
            solver_evidence=sources["solverSourceEvidence"],
            verifier_evidence=sources["verifierSourceEvidence"],
            lesson_practice_routing=lesson_routing,
            interaction_supported=interaction_supported,
            visual_needed=needs_visual,
            visual_covered=visual_covered,
            mapping_count=mapping_count,
            assessment_evidence=assessment_evidence,
            exam_evidence=bool(exam_files),
        )

        rows.append(
            {
                "familyId": family_id,
                "domain": family_domain(family_id),
                "skillId": skill_id,
                "declaredStatus": declared_status,
                "productionRouting": production_routing,
                "lessonPracticeRouting": lesson_routing,
                "answerType": answer_type,
                "representations": representations,
                "requiredCapabilities": required,
                "missingCapabilities": missing,
                **sources,
                "interactionSupportedByCurrentPractice": interaction_supported,
                "visualRequired": needs_visual,
                "visualRendererEvidence": visual_covered,
                "curriculumMappingOccurrenceCount": mapping_count,
                "curriculumMappingEvidenceFiles": mapping_files,
                "assessmentContractEvidence": assessment_evidence,
                "examCertificationEvidenceFiles": exam_files,
                "readiness": state,
                "blockers": blockers,
            }
        )

    rows.sort(key=lambda row: (row["domain"], row["familyId"]))

    readiness_counts = Counter(row["readiness"] for row in rows)
    domain_counts: dict[str, Counter[str]] = defaultdict(Counter)
    for row in rows:
        domain_counts[row["domain"]][row["readiness"]] += 1

    strict_blockers: list[str] = []
    if duplicate_ids:
        strict_blockers.append(
            "DUPLICATE_QUESTION_FAMILY_IDS:" + ",".join(sorted(set(duplicate_ids)))
        )

    for row in rows:
        if row["missingCapabilities"]:
            strict_blockers.append(
                f"{row['familyId']}:MISSING_REQUIRED_CAPABILITY"
            )
        if row["productionRouting"] and row["readiness"] != "EXAM_READY_VERIFIED":
            # Production routing is a stronger declaration than ShadowVerified;
            # this M0 matrix refuses to let it bypass missing surface evidence.
            strict_blockers.append(
                f"{row['familyId']}:PRODUCTION_ROUTING_WITHOUT_FULL_CERTIFICATION"
            )
        if row["lessonPracticeRouting"] and not row["interactionSupportedByCurrentPractice"]:
            strict_blockers.append(
                f"{row['familyId']}:ROUTED_WITHOUT_INTERACTION"
            )

    return {
        "schemaVersion": 1,
        "programme": "advanced-mathematics-v3",
        "phase": "M0",
        "sourceRegistryVersion": question_registry.get("registryVersion"),
        "summary": {
            "advancedFamilyCount": len(rows),
            "readinessCounts": dict(sorted(readiness_counts.items())),
            "domainCounts": {
                domain: dict(sorted(counts.items()))
                for domain, counts in sorted(domain_counts.items())
            },
            "lessonPracticeRoutedCount": sum(
                1 for row in rows if row["lessonPracticeRouting"]
            ),
            "visualRequiredCount": sum(
                1 for row in rows if row["visualRequired"]
            ),
            "visualRendererCoveredCount": sum(
                1
                for row in rows
                if row["visualRequired"] and row["visualRendererEvidence"]
            ),
            "assessmentEvidenceCount": sum(
                1 for row in rows if row["assessmentContractEvidence"]
            ),
            "examCertificationEvidenceCount": sum(
                1 for row in rows if row["examCertificationEvidenceFiles"]
            ),
            "strictBlockerCount": len(strict_blockers),
        },
        "strictBlockers": strict_blockers,
        "families": rows,
    }


def markdown(report: dict[str, Any]) -> str:
    s = report["summary"]
    lines = [
        "# Advanced Mathematics Capability Matrix — M0",
        "",
        "This inventory distinguishes engine capability from learner-facing and formal certification.",
        "",
        f"- Advanced families: **{s['advancedFamilyCount']}**",
        f"- Lesson-Practice routed: **{s['lessonPracticeRoutedCount']}**",
        f"- Visual-required: **{s['visualRequiredCount']}**",
        f"- Visual renderer evidence: **{s['visualRendererCoveredCount']}**",
        f"- Assessment contract evidence: **{s['assessmentEvidenceCount']}**",
        f"- Exam certification evidence: **{s['examCertificationEvidenceCount']}**",
        f"- Strict integrity blockers: **{s['strictBlockerCount']}**",
        "",
        "## Readiness",
        "",
        "| State | Families |",
        "| --- | ---: |",
    ]
    for key, value in s["readinessCounts"].items():
        lines.append(f"| {key} | {value} |")

    lines.extend(
        [
            "",
            "## Family matrix",
            "",
            "| Family | Domain | Practice route | Visual | Mapping | Readiness | Primary blockers |",
            "| --- | --- | --- | --- | ---: | --- | --- |",
        ]
    )
    for row in report["families"]:
        blockers = ", ".join(row["blockers"][:3])
        lines.append(
            "| {family} | {domain} | {route} | {visual} | {mapping} | {ready} | {blockers} |".format(
                family=row["familyId"],
                domain=row["domain"],
                route="yes" if row["lessonPracticeRouting"] else "no",
                visual=(
                    "covered"
                    if row["visualRequired"] and row["visualRendererEvidence"]
                    else "missing"
                    if row["visualRequired"]
                    else "n/a"
                ),
                mapping=row["curriculumMappingOccurrenceCount"],
                ready=row["readiness"],
                blockers=blockers or "—",
            )
        )
    return "\n".join(lines) + "\n"


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--strict", action="store_true")
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--write-markdown", action="store_true")
    args = parser.parse_args()

    report = build_report()

    if args.write_report:
        OUTPUT.parent.mkdir(parents=True, exist_ok=True)
        OUTPUT.write_text(
            json.dumps(report, indent=2, ensure_ascii=False) + "\n",
            encoding="utf-8",
        )

    if args.write_markdown:
        MARKDOWN.parent.mkdir(parents=True, exist_ok=True)
        MARKDOWN.write_text(markdown(report), encoding="utf-8")

    summary = report["summary"]
    print(
        "ADVANCED_MATH_M0 "
        f"families={summary['advancedFamilyCount']} "
        f"practice_routed={summary['lessonPracticeRoutedCount']} "
        f"visual_required={summary['visualRequiredCount']} "
        f"strict_blockers={summary['strictBlockerCount']}"
    )

    if args.strict and report["strictBlockers"]:
        for blocker in report["strictBlockers"]:
            print(f"BLOCKER {blocker}")
        return 1

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
