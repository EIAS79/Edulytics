#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import os
import re
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

import full_practice_completion_matrix

ROOT = Path(__file__).resolve().parents[2]
ARTIFACT_DIR = ROOT / "artifacts/math-intelligence"
FAMILY_REGISTRY = ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
VISUAL_RENDERER = ROOT / "src/Edulytics.Web/Presentation/PracticeMathVisualRenderer.cs"
VISUAL_REPRESENTATIONS = {
    "diagram_metadata",
    "graph",
    "vector",
    "right_triangle",
    "angle",
    "coordinate_pair",
    "solid_dimensions",
}

ADVANCED_DOMAINS = {
    "calculus",
    "complex",
    "functions",
    "indices",
    "matrices",
    "mechanics",
    "numerical_methods",
    "trigonometry",
    "vectors",
}

OUTPUTS = {
    "content": ARTIFACT_DIR / "content-repair-audit.json",
    "mapping": ARTIFACT_DIR / "lesson-skill-mapping-audit.json",
    "family": ARTIFACT_DIR / "question-family-coverage-audit.json",
    "visual": ARTIFACT_DIR / "visual-representation-coverage-audit.json",
    "solver": ARTIFACT_DIR / "solver-verifier-coverage-audit.json",
    "difficulty": ARTIFACT_DIR / "difficulty-grade-coverage-audit.json",
    "curriculum": ARTIFACT_DIR / "practice-readiness-by-curriculum.json",
    "domain": ARTIFACT_DIR / "practice-readiness-by-domain.json",
    "level": ARTIFACT_DIR / "practice-readiness-by-level.json",
    "closure": ARTIFACT_DIR / "practice-programme-final-closure-audit.json",
}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def commit_sha() -> str:
    return os.environ.get("GITHUB_SHA", "").strip() or "local-uncommitted"


def classify_level(lesson_code: str) -> tuple[str, str]:
    upper = lesson_code.upper()
    if ":HS:" in upper:
        return "HS", "UpperSecondary"

    match = re.search(r":(?:G|L|S)(\d{1,2}):", upper)
    if not match:
        return "UNCLASSIFIED", "UNCLASSIFIED"

    level = int(match.group(1))
    stage = (
        "EarlyPrimary" if level <= 2 else
        "Primary" if level <= 6 else
        "LowerSecondary" if level <= 9 else
        "UpperSecondary"
    )
    return str(level), stage


def status_counter(rows: list[dict[str, Any]]) -> dict[str, int]:
    counter = Counter(str(row.get("terminalPracticeStatus") or "UNKNOWN") for row in rows)
    return dict(sorted(counter.items()))


def audit() -> dict[str, Any]:
    matrix = full_practice_completion_matrix.audit()
    rows = list(matrix.get("lessons") or [])
    eligible = [
        row for row in rows
        if row.get("practiceEligibility") == "PRACTICE_ELIGIBLE"
    ]
    non_standalone = [
        row for row in rows
        if row.get("practiceEligibility") == "NON_STANDALONE_WITH_EVIDENCE"
    ]

    family_doc = read_json(FAMILY_REGISTRY)
    family_registry = {
        str(row.get("id") or ""): row
        for row in family_doc.get("families", [])
        if isinstance(row, dict) and str(row.get("id") or "").strip()
    }

    content_blockers: list[str] = []
    mapping_blockers: list[str] = []
    for row in eligible:
        code = str(row.get("lessonCode") or "")
        if row.get("semanticContentStatus") in {
            "CONTENT_WEAK",
            "REVIEW_REQUIRED",
            "BLOCKED",
            "MAPPING_CONFLICT",
        }:
            content_blockers.append(
                f"{code}: semanticContentStatus={row.get('semanticContentStatus')}"
            )
        if not bool(row.get("approvedMapping")):
            mapping_blockers.append(f"{code}: approved mapping missing")
        if not list(row.get("primarySkills") or []):
            mapping_blockers.append(f"{code}: primary skill mapping missing")

    family_blockers: list[str] = []
    used_families: set[str] = set()
    family_count_distribution: Counter[int] = Counter()
    family_by_domain: defaultdict[str, set[str]] = defaultdict(set)
    for row in eligible:
        code = str(row.get("lessonCode") or "")
        families = [str(x) for x in row.get("questionFamilies") or [] if str(x).strip()]
        family_count_distribution[len(families)] += 1
        if not families:
            family_blockers.append(f"{code}: no allowed question family")
            continue
        for family_id in families:
            used_families.add(family_id)
            family = family_registry.get(family_id)
            if family is None:
                family_blockers.append(f"{code}: unknown family {family_id}")
                continue
            if not str(family.get("verificationPolicy") or "").strip():
                family_blockers.append(
                    f"{code}: family {family_id} has no verification policy"
                )
            if not list(family.get("requiredCapabilities") or []):
                family_blockers.append(
                    f"{code}: family {family_id} has no required capabilities"
                )
            for domain in row.get("domains") or []:
                family_by_domain[str(domain)].add(family_id)

    visual_blockers: list[str] = []
    visual_required = [row for row in eligible if bool(row.get("visualRequired"))]
    renderer_source = VISUAL_RENDERER.read_text(encoding="utf-8")
    switch_match = re.search(
        r"family\s+switch\s*\{(?P<body>.*?)_\s*=>\s*null",
        renderer_source,
        flags=re.DOTALL,
    )
    rendered_families = set(
        re.findall(r'"([a-z0-9_.-]+)"', switch_match.group("body"))
        if switch_match
        else []
    )
    visual_family_ids: set[str] = set()
    for row in visual_required:
        code = str(row.get("lessonCode") or "")
        if row.get("visualStatus") != "READY_METADATA":
            visual_blockers.append(
                f"{code}: visualStatus={row.get('visualStatus')}"
            )
        for family_id in row.get("questionFamilies") or []:
            family = family_registry.get(str(family_id))
            if family is None:
                continue
            representations = {str(x) for x in family.get("representations") or []}
            if representations & VISUAL_REPRESENTATIONS:
                visual_family_ids.add(str(family_id))

    missing_renderers = sorted(visual_family_ids - rendered_families)
    visual_blockers.extend(
        f"visual Practice family has no deterministic renderer: {family_id}"
        for family_id in missing_renderers
    )

    solver_blockers: list[str] = []
    for row in eligible:
        code = str(row.get("lessonCode") or "")
        if not bool(row.get("solverReady")):
            solver_blockers.append(f"{code}: solver not ready")
        if not bool(row.get("verifierReady")):
            solver_blockers.append(f"{code}: verifier not ready")

    level_blockers: list[str] = []
    level_rows: defaultdict[str, list[dict[str, Any]]] = defaultdict(list)
    stage_rows: defaultdict[str, list[dict[str, Any]]] = defaultdict(list)
    for row in eligible:
        level, stage = classify_level(str(row.get("lessonCode") or ""))
        if stage == "UNCLASSIFIED":
            level_blockers.append(
                f"{row.get('lessonCode')}: curriculum level could not be classified"
            )
        level_rows[level].append(row)
        stage_rows[stage].append(row)

    readiness_blockers: list[str] = []
    for row in eligible:
        if row.get("terminalPracticeStatus") != "READY_VERIFIED":
            readiness_blockers.append(
                f"{row.get('lessonCode')}: {row.get('terminalPracticeStatus')}"
            )

    geometry_trig_vector = [
        row for row in eligible
        if {"geometry", "trigonometry", "vectors"} & set(row.get("domains") or [])
    ]
    advanced = [
        row for row in eligible
        if ADVANCED_DOMAINS & set(row.get("domains") or [])
    ]
    domain_closure_blockers = [
        f"{row.get('lessonCode')}: advanced/domain Practice not ready"
        for row in geometry_trig_vector + advanced
        if row.get("terminalPracticeStatus") != "READY_VERIFIED"
    ]

    by_curriculum: dict[str, Any] = {}
    for pack in sorted({str(row.get("packCode") or "UNKNOWN") for row in rows}):
        pack_rows = [row for row in rows if str(row.get("packCode") or "UNKNOWN") == pack]
        pack_eligible = [row for row in pack_rows if row.get("practiceEligibility") == "PRACTICE_ELIGIBLE"]
        by_curriculum[pack] = {
            "catalogueLessonCount": len(pack_rows),
            "practiceEligibleLessonCount": len(pack_eligible),
            "readyVerifiedCount": sum(
                row.get("terminalPracticeStatus") == "READY_VERIFIED"
                for row in pack_eligible
            ),
            "nonStandaloneWithEvidenceCount": sum(
                row.get("practiceEligibility") == "NON_STANDALONE_WITH_EVIDENCE"
                for row in pack_rows
            ),
            "statuses": status_counter(pack_eligible),
        }

    by_domain: dict[str, Any] = {}
    domain_names = sorted({
        str(domain)
        for row in eligible
        for domain in row.get("domains") or []
    })
    for domain in domain_names:
        domain_rows = [row for row in eligible if domain in (row.get("domains") or [])]
        by_domain[domain] = {
            "lessonCount": len(domain_rows),
            "readyVerifiedCount": sum(
                row.get("terminalPracticeStatus") == "READY_VERIFIED"
                for row in domain_rows
            ),
            "visualRequiredCount": sum(bool(row.get("visualRequired")) for row in domain_rows),
            "solverReadyCount": sum(bool(row.get("solverReady")) for row in domain_rows),
            "verifierReadyCount": sum(bool(row.get("verifierReady")) for row in domain_rows),
            "uniqueQuestionFamilyCount": len(family_by_domain.get(domain, set())),
        }

    by_level: dict[str, Any] = {}
    for level, level_group in sorted(level_rows.items(), key=lambda pair: pair[0]):
        _, stage = classify_level(str(level_group[0].get("lessonCode") or ""))
        by_level[level] = {
            "difficultyStage": stage,
            "lessonCount": len(level_group),
            "readyVerifiedCount": sum(
                row.get("terminalPracticeStatus") == "READY_VERIFIED"
                for row in level_group
            ),
            "visualRequiredCount": sum(bool(row.get("visualRequired")) for row in level_group),
        }

    content_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "practiceEligibleLessonCount": len(eligible),
        "contentPassCount": len(eligible) - len({
            blocker.split(":", 1)[0] for blocker in content_blockers
        }),
        "blockers": content_blockers,
    }
    mapping_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "practiceEligibleLessonCount": len(eligible),
        "approvedMappingCount": sum(bool(row.get("approvedMapping")) for row in eligible),
        "primarySkillMappedCount": sum(bool(row.get("primarySkills")) for row in eligible),
        "blockers": mapping_blockers,
    }
    family_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "practiceEligibleLessonCount": len(eligible),
        "registryFamilyCount": len(family_registry),
        "usedFamilyCount": len(used_families),
        "familyCountDistribution": dict(sorted(family_count_distribution.items())),
        "blockers": family_blockers,
    }
    visual_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "visualRequiredLessonCount": len(visual_required),
        "readyMetadataCount": sum(
            row.get("visualStatus") == "READY_METADATA"
            for row in visual_required
        ),
        "visualFamilyCount": len(visual_family_ids),
        "rendererBackedFamilyCount": len(visual_family_ids & rendered_families),
        "missingRendererFamilies": missing_renderers,
        "blockers": visual_blockers,
    }
    solver_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "practiceEligibleLessonCount": len(eligible),
        "solverReadyCount": sum(bool(row.get("solverReady")) for row in eligible),
        "verifierReadyCount": sum(bool(row.get("verifierReady")) for row in eligible),
        "blockers": solver_blockers,
    }
    difficulty_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "runtimePolicy": "GradeAwareExactDifficultyPolicy",
        "practiceEligibleLessonCount": len(eligible),
        "classifiedLessonCount": len(eligible) - len(level_blockers),
        "byStage": {
            stage: len(stage_group)
            for stage, stage_group in sorted(stage_rows.items())
        },
        "blockers": level_blockers,
    }
    curriculum_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "curricula": by_curriculum,
    }
    domain_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "domains": by_domain,
    }
    level_report = {
        "schemaVersion": 1,
        "commitSha": commit_sha(),
        "levels": by_level,
    }

    blockers = sorted(set(
        content_blockers
        + mapping_blockers
        + family_blockers
        + visual_blockers
        + solver_blockers
        + level_blockers
        + readiness_blockers
        + domain_closure_blockers
    ))
    closure = {
        "schemaVersion": 1,
        "audit": "Mathematics Practice programme final closure",
        "commitSha": commit_sha(),
        "catalogueLessonCount": len(rows),
        "practiceEligibleLessonCount": len(eligible),
        "readyVerifiedCount": sum(
            row.get("terminalPracticeStatus") == "READY_VERIFIED"
            for row in eligible
        ),
        "nonStandaloneWithEvidenceCount": len(non_standalone),
        "geometryTrigVectorLessonCount": len({
            str(row.get("lessonCode"))
            for row in geometry_trig_vector
        }),
        "advancedDomainLessonCount": len({
            str(row.get("lessonCode"))
            for row in advanced
        }),
        "visualRequiredLessonCount": len(visual_required),
        "usedQuestionFamilyCount": len(used_families),
        "difficultyClassifiedLessonCount": len(eligible) - len(level_blockers),
        "blockerCount": len(blockers),
        "blockers": blockers,
    }

    return {
        "content": content_report,
        "mapping": mapping_report,
        "family": family_report,
        "visual": visual_report,
        "solver": solver_report,
        "difficulty": difficulty_report,
        "curriculum": curriculum_report,
        "domain": domain_report,
        "level": level_report,
        "closure": closure,
    }


def write_reports(reports: dict[str, Any]) -> None:
    ARTIFACT_DIR.mkdir(parents=True, exist_ok=True)
    for key, path in OUTPUTS.items():
        path.write_text(
            json.dumps(reports[key], indent=2, sort_keys=True) + "\n",
            encoding="utf-8",
        )


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--strict", action="store_true")
    parser.add_argument("--write-report", action="store_true")
    args = parser.parse_args()

    reports = audit()
    if args.write_report:
        write_reports(reports)

    closure = reports["closure"]
    print(json.dumps(closure, indent=2, sort_keys=True))
    if args.strict and closure["blockerCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
