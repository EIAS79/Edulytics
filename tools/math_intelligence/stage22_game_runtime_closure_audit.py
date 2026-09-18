#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
STAGE18 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage18-practice-migration-manifest.v1.json"
STAGE22 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage22-game-runtime-migration-manifest.v1.json"
RUNTIME = ROOT / "src/Edulytics.Web/GameRouting/Stage22ExactGameRuntime.cs"
CONTROLLER = ROOT / "src/Edulytics.Web/Controllers/StudentPracticeController.cs"
VIEW = ROOT / "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"
CLIENT = ROOT / "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-stage22.js"
PROGRAM = ROOT / "src/Edulytics.Web/Program.cs"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage22GameRuntimeMigrationTests.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage22-game-runtime-closure-audit.json"

EXPECTED_RUNTIME = "stage22-server-authoritative-game-v1"
EXPECTED_MECHANICS = {
    "TWO_UNKNOWNS",
    "SCALE_READING",
    "FRACTION_COMPARE_UNLIKE",
    "FRACTION_EQUIVALENT",
    "UNIT_RATE",
}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    stage18 = read_json(STAGE18)
    stage22 = read_json(STAGE22)
    blockers: list[str] = []

    entries = stage18.get("entries") or []
    lesson_codes = [str(x.get("lessonCode") or "") for x in entries if isinstance(x, dict)]
    mechanics = {str(x.get("mechanic") or "") for x in entries if isinstance(x, dict)}

    if stage22.get("status") != "COMPLETE":
        blockers.append("Stage 22 manifest is not marked COMPLETE.")

    if stage22.get("runtimeVersion") != EXPECTED_RUNTIME:
        blockers.append("Stage 22 runtime version does not match the accepted server-authoritative runtime.")

    if int(stage22.get("exactLessonCount") or 0) != len(entries):
        blockers.append("Stage 22 exact lesson count does not match the Stage 18 exact lesson registry.")

    if mechanics != EXPECTED_MECHANICS:
        blockers.append(f"Stage 18 exact mechanics changed unexpectedly: {sorted(mechanics)}")

    manifest_mechanics = set(str(x) for x in stage22.get("exactMechanics") or [])
    if manifest_mechanics != EXPECTED_MECHANICS:
        blockers.append("Stage 22 exact mechanic scope does not match the accepted five-mechanic registry.")

    authority = stage22.get("authoritativeComponents") or {}
    if authority.get("browserAuthority") is not False:
        blockers.append("Stage 22 manifest still allows browser authority over answer correctness.")

    client_contract = stage22.get("clientContract") or {}
    if client_contract.get("decidesCorrectness") is not False:
        blockers.append("Stage 22 client contract still permits local correctness decisions.")

    never_receives = {str(x).lower() for x in client_contract.get("neverReceives") or []}
    if "correctanswer" not in never_receives:
        blockers.append("Stage 22 client contract does not explicitly prohibit CorrectAnswer disclosure.")

    runtime = RUNTIME.read_text(encoding="utf-8")
    controller = CONTROLLER.read_text(encoding="utf-8")
    view = VIEW.read_text(encoding="utf-8")
    client = CLIENT.read_text(encoding="utf-8")
    program = PROGRAM.read_text(encoding="utf-8")
    tests = TESTS.read_text(encoding="utf-8")

    for token in [
        'RuntimeVersion = "stage22-server-authoritative-game-v1"',
        "IDataProtector",
        "CreateProtector",
        "ExactSkillContractQuestionEngine().Generate",
        "ExactSkillContractQuestionEngine.Verify",
        "Stage18PracticeSkillContracts.TryResolve",
        "RoundLifetime",
        "ActorUserId",
        "CurriculumAdoptionId",
        "LessonId",
        "AllowedQuestionFamilies.Contains",
    ]:
        if token not in runtime:
            blockers.append(f"Stage 22 runtime is missing required server-authority token: {token}")

    if "CorrectAnswer" in runtime.split("public sealed record Stage22GameRound(", 1)[1].split(");", 1)[0]:
        blockers.append("Stage 22 public round contract exposes CorrectAnswer.")

    for token in [
        '[HttpPost("game/runtime/start"), ValidateAntiForgeryToken]',
        '[HttpPost("game/runtime/answer"), ValidateAntiForgeryToken]',
        "MathematicsV2ProductMigrationPolicy.RendererKey",
        "Stage18PracticeSkillContracts.TryResolve",
        "gameRuntime.CreateRound",
        "gameRuntime.EvaluateAnswer",
    ]:
        if token not in controller:
            blockers.append(f"Stage 22 controller wiring is missing token: {token}")

    if "AddSingleton<Edulytics.Web.GameRouting.Stage22ExactGameRuntime>()" not in program:
        blockers.append("Stage 22 server-authoritative runtime is not registered in DI.")

    stage22_branch = view.find("isStage22ServerAuthoritative")
    stage22_script = view.find("lesson-grounded-practice-stage22.js")
    legacy_script = view.find("lesson-grounded-practice-v2.js")
    if min(stage22_branch, stage22_script, legacy_script) < 0:
        blockers.append("Stage 22 game view does not contain the accepted server/legacy routing branches.")
    elif stage22_script > legacy_script:
        blockers.append("Stage 22 server-authoritative exact script must be selected before the legacy exact script.")

    for token in [
        'data-stage22-server-authoritative="@(isStage22ServerAuthoritative ? "true" : "false")"',
        "data-curriculum-adoption-id",
        "data-lesson-id",
        "Html.AntiForgeryToken",
    ]:
        if token not in view:
            blockers.append(f"Stage 22 game view is missing token: {token}")

    required_client_tokens = [
        "/student/practice/game/runtime/start",
        "/student/practice/game/runtime/answer",
        "RequestVerificationToken",
        "roundToken",
        "result.isCorrect",
        "result.points",
    ]
    for token in required_client_tokens:
        if token not in client:
            blockers.append(f"Stage 22 interaction client is missing server-runtime token: {token}")

    forbidden_client_tokens = [
        "Math.random(",
        "correctAnswer",
        "CorrectAnswer",
        "ExactSkillContractQuestionEngine",
        "dataset.answer",
        "data-answer",
    ]
    for token in forbidden_client_tokens:
        if token in client:
            blockers.append(f"Stage 22 interaction client contains local mathematics authority token: {token}")

    if "Your answer was not graded locally" not in client:
        blockers.append("Stage 22 client does not explicitly fail closed when server verification is unavailable.")

    required_tests = [
        "EveryStage18ExactLessonUsesServerGeneratedVerifiedRound",
        "BrowserCannotForgeRoundScopeAcrossStudent",
        "BrowserCannotForgeCurriculumOrLessonScope",
        "TamperedRoundTokenFailsClosed",
        "InvalidMechanicCannotEnterServerAuthoritativeExactRuntime",
        "ArbitraryWrongAnswerIsRejectedByServer",
    ]
    for token in required_tests:
        if token not in tests:
            blockers.append(f"Stage 22 acceptance coverage is missing test: {token}")

    if len(set(lesson_codes)) != len(lesson_codes):
        blockers.append("Stage 18 exact lesson registry contains duplicate lesson codes.")

    return {
        "schemaVersion": 1,
        "audit": "Stage 22 server-authoritative game runtime closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "exactLessonCount": len(entries),
            "exactMechanicCount": len(mechanics),
            "browserAuthority": False,
            "missingExactLessons": 0,
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
        "exactLessonCodes": sorted(lesson_codes),
        "exactMechanics": sorted(mechanics),
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    report = audit()
    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(
            json.dumps(report, ensure_ascii=False, indent=2) + "\n",
            encoding="utf-8",
        )

    print(json.dumps(report["summary"], ensure_ascii=False, indent=2))
    if args.strict and report["blockers"]:
        for blocker in report["blockers"]:
            print(f"BLOCKER: {blocker}")
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
