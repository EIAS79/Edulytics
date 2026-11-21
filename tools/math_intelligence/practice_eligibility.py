#!/usr/bin/env python3
from __future__ import annotations

from dataclasses import dataclass
import json
from pathlib import Path
from typing import Any

from polish_practice_map import load_polish_outcome_mappings


@dataclass(frozen=True)
class PracticeEligibility:
    status: str
    reason_code: str
    evidence: tuple[str, ...]


_POLISH_MAPPINGS: dict[str, dict[str, Any]] | None = None
_POLISH_MAPPING_ERRORS: list[str] | None = None


def _polish_mappings() -> tuple[dict[str, dict[str, Any]], list[str]]:
    global _POLISH_MAPPINGS, _POLISH_MAPPING_ERRORS
    if _POLISH_MAPPINGS is None or _POLISH_MAPPING_ERRORS is None:
        _POLISH_MAPPINGS, _POLISH_MAPPING_ERRORS = load_polish_outcome_mappings()
    return _POLISH_MAPPINGS, _POLISH_MAPPING_ERRORS


def _text(row: dict[str, Any], *names: str) -> str:
    for name in names:
        if name in row:
            return str(row.get(name) or "").strip()
    return ""


def classify_lesson(
    pack: dict[str, Any],
    lesson: dict[str, Any],
) -> PracticeEligibility:
    pack_code = _text(pack, "PackCode", "packCode")
    pedagogical_source_type = _text(
        pack,
        "PedagogicalSourceType",
        "pedagogicalSourceType",
    )
    title_provenance = _text(
        lesson,
        "TitleProvenance",
        "titleProvenance",
    )
    adaptation_status = _text(
        lesson,
        "AdaptationStatus",
        "adaptationStatus",
    )


    # Exactly two source-authored Grade9 General lessons require a teacher to
    # assess diagrams and open coordinate proofs. They must never be mislabeled
    # as automatically generated/graded exercises. The checkpoint is checked
    # independently; mere Grade9 membership does not bypass Practice controls.
    g9_teacher_review = {
        "PED:UAE-MOE-MATH:L9:GENERAL:13:07:CONGRUENCE-TRANSFORMATIONS": 80,
        "PED:UAE-MOE-MATH:L9:GENERAL:13:08:TRIANGLES-AND-COORDINATE-PROOF": 81,
    }
    lesson_code = _text(lesson, "LessonCode", "lessonCode")
    if lesson_code in g9_teacher_review:
        ordinal = g9_teacher_review[lesson_code]
        source = (Path(__file__).resolve().parents[2]
                  / "artifacts/curriculum-delivery/UAE-MOE-MATH/G9"
                  / f"lesson-{ordinal}-stage-a-c-checkpoint.json")
        try:
            checkpoint = json.loads(source.read_text(encoding="utf-8"))
            stage_c = checkpoint.get("stageC") or {}
            stage_b = checkpoint.get("stageB") or {}
            valid = (
                pack_code == "UAE-MOE-MATH"
                and _text(pack, "AcademicLanguage", "academicLanguage") == "ar"
                and _text(pack, "Status", "status") == "Published"
                and checkpoint.get("lessonCode") == lesson_code
                and checkpoint.get("lessonOrdinal") == ordinal
                and checkpoint.get("closed") is True
                and stage_c.get("practiceFamilies") == []
                and "TEACHER_REVIEW_ONLY" in str(stage_c.get("status") or "")
                and bool(stage_c.get("teacherReviewRequiredFor"))
                and stage_b.get("coolDownQuestions", 0) > 0
                and stage_b.get("coolDownGraded") is False
            )
        except (OSError, ValueError, TypeError, KeyError):
            valid = False
        if valid:
            return PracticeEligibility(
                "NON_STANDALONE_WITH_EVIDENCE",
                "UAE_G9_OPEN_RESPONSE_TEACHER_REVIEW_ONLY",
                (
                    f"Exact closed authoring checkpoint lesson-{ordinal} records teacher-review-only route.",
                    "No automated mathematical item generator or automatic grade is claimed.",
                    "The user-facing teacher grading flow requires independent four-role HTTPS E2E.",
                ),
            )
        return PracticeEligibility(
            "BLOCKED_TEMPORARY",
            "UAE_G9_TEACHER_REVIEW_EVIDENCE_MISSING",
            ("Missing or inconsistent exact teacher-only checkpoint; block automatic Practice.",),
        )

    # Polish Phase-29 identities are learner-facing official-outcome lessons.
    # They become Practice-eligible only through the exact reviewed OutcomeCode
    # map. The generated "ćwiczenie NN" title is never authorization evidence.
    if (
        pack_code == "PL-NATIONAL-MATH"
        and pedagogical_source_type == "OfficialFrameworkOnly"
        and title_provenance == "EdulyticsDerivedFromOfficialOutcome"
        and "deterministic one-outcome-per-lesson fallback identity"
        in adaptation_status.lower()
    ):
        mappings, mapping_errors = _polish_mappings()
        outcomes = lesson.get("OutcomeCodes") or lesson.get("outcomeCodes") or []
        codes = [
            str(value).strip()
            for value in outcomes
            if str(value).strip()
        ] if isinstance(outcomes, list) else []
        if (
            not mapping_errors
            and codes
            and all(code in mappings for code in codes)
        ):
            return PracticeEligibility(
                "PRACTICE_ELIGIBLE",
                "POLISH_EXACT_OUTCOME_MAP",
                (
                    "Lesson is learner-facing and attached to an accepted Polish official OutcomeCode.",
                    "Every OutcomeCode resolves through the reviewed exact Polish Practice map.",
                    "Practice authorization is based on OutcomeCode evidence, not the generated fallback title.",
                ),
            )

        return PracticeEligibility(
            "BLOCKED_TEMPORARY",
            "POLISH_EXACT_OUTCOME_MAPPING_MISSING",
            (
                "Learner-facing Polish lesson is fail-closed until every OutcomeCode resolves through the exact reviewed Practice map.",
            ),
        )

    return PracticeEligibility(
        "PRACTICE_ELIGIBLE",
        "STANDALONE_OR_EXPLICIT_PRACTICE_TARGET",
        (),
    )
