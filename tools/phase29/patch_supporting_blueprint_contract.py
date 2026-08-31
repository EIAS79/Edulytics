#!/usr/bin/env python3
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
PATH = ROOT / "src/Edulytics.Core/Curriculum/PedagogicalLessonBlueprint.cs"

OLD = '''        var diagnostics =
            document.AcquisitionDiagnostics;

        if (diagnostics.UnitCount !=
                document.Units.Count ||
            diagnostics.LessonCount !=
                document.Lessons.Count ||
            diagnostics.EffectiveOfficialStandardCount <= 0 ||
            diagnostics.AddressingCoverageCount !=
                diagnostics.EffectiveOfficialStandardCount ||
            diagnostics.FormalMappingCount !=
                document.Lessons.Sum(
                    x => x.OutcomeCodes.Count))
        {
            throw new InvalidOperationException(
                $"Blueprint acquisition diagnostics drift: " +
                $"{document.BlueprintCode}.");
        }
'''

NEW = '''        var diagnostics =
            document.AcquisitionDiagnostics;

        var formalMappingCount =
            document.Lessons.Sum(
                x => x.OutcomeCodes.Count);

        var hasFormalMappings =
            formalMappingCount > 0;

        var hasResolvedAlignment =
            document.Lessons
                .SelectMany(x => x.Alignments)
                .Any(
                    x =>
                        x.ResolutionKind is
                            "ExactAcceptedStandard" or
                            "SubpartToAcceptedParent" ||
                        !string.IsNullOrWhiteSpace(
                            x.OutcomeCode));

        var supportingOnly =
            !hasFormalMappings &&
            !hasResolvedAlignment &&
            document.Lessons.All(
                x => x.OutcomeCodes.Count == 0);

        var diagnosticsDrift =
            diagnostics.UnitCount !=
                document.Units.Count ||
            diagnostics.LessonCount !=
                document.Lessons.Count ||
            diagnostics.FormalMappingCount !=
                formalMappingCount;

        if (hasFormalMappings)
        {
            diagnosticsDrift =
                diagnosticsDrift ||
                diagnostics.EffectiveOfficialStandardCount <= 0 ||
                diagnostics.AddressingCoverageCount !=
                    diagnostics.EffectiveOfficialStandardCount;
        }
        else
        {
            diagnosticsDrift =
                diagnosticsDrift ||
                !supportingOnly ||
                diagnostics.EffectiveOfficialStandardCount != 0 ||
                diagnostics.AddressingCoverageCount != 0;
        }

        if (diagnosticsDrift)
        {
            throw new InvalidOperationException(
                $"Blueprint acquisition diagnostics drift: " +
                $"{document.BlueprintCode}.");
        }
'''

text = PATH.read_text(encoding="utf-8")
if NEW in text:
    print("PASS: supporting-only blueprint contract already applied")
elif OLD in text:
    PATH.write_text(text.replace(OLD, NEW), encoding="utf-8")
    print("PASS: supporting-only blueprint contract applied")
else:
    raise SystemExit("FAIL: expected V1 diagnostics contract was not found")
