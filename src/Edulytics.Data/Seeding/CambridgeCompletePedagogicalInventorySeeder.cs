using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Completes the Edulytics pedagogical lesson inventory for Cambridge scopes
/// that do not yet have a reviewed source-driven lesson blueprint.
///
/// The accepted Cambridge curriculum pack remains reference-only and is never
/// mutated. Existing reviewed blueprint lessons (currently Primary Stage 1)
/// always win. Every still-unmapped official Outcome reference becomes one
/// Edulytics-owned pedagogical lesson, mapped exactly to that reference. The
/// generated lesson is not represented as an official Cambridge lesson title.
/// Canonical lesson bodies remain a separate reviewed/published concern.
/// </summary>
public sealed class CambridgeCompletePedagogicalInventorySeeder
{
    private const string DerivedPrefix = "PED:CAM:DERIVED:";

    private readonly EdulyticsDbContext _db;

    public CambridgeCompletePedagogicalInventorySeeder(EdulyticsDbContext db) =>
        _db = db;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var state = await _db.CurriculumPackImportStates
            .AsNoTracking()
            .SingleAsync(
                x =>
                    x.FrameworkCode == MathematicsCurriculumPackRegistry.CambridgeCode &&
                    x.IsComplete,
                ct);

        var nodes = await _db.CurriculumPackContentNodes
            .AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == state.FrameworkVersionId &&
                x.FrameworkCode == MathematicsCurriculumPackRegistry.CambridgeCode &&
                x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Code)
            .ToArrayAsync(ct);

        var officialOutcomes = nodes
            .Where(x => x.IsOfficial && x.NodeKind == "Outcome")
            .ToArray();

        if (officialOutcomes.Length != state.OfficialNodeCount)
        {
            throw new InvalidOperationException(
                $"Cambridge official-reference count drift. Expected {state.OfficialNodeCount}, got {officialOutcomes.Length}.");
        }

        var existingMappings = await _db.CurriculumPedagogicalLessonOutcomes
            .AsNoTracking()
            .Where(x => x.FrameworkVersionId == state.FrameworkVersionId)
            .ToArrayAsync(ct);

        var alreadyCovered = existingMappings
            .Select(x => x.OutcomeNodeId)
            .ToHashSet();

        var existingLessons = await _db.CurriculumPedagogicalLessons
            .Where(x => x.FrameworkVersionId == state.FrameworkVersionId)
            .ToArrayAsync(ct);

        var lessonById = existingLessons.ToDictionary(x => x.Id);
        var lessonByCode = existingLessons.ToDictionary(x => x.Code, StringComparer.Ordinal);
        var nodeById = nodes.ToDictionary(x => x.Id);
        var now = DateTime.UtcNow;
        var addedMappings = new HashSet<(Guid LessonId, Guid OutcomeId)>();

        foreach (var outcome in officialOutcomes)
        {
            if (alreadyCovered.Contains(outcome.Id))
                continue;

            var id = G($"cambridge-derived-lesson|{state.FrameworkVersionId}|{outcome.Code}");
            var code = DerivedPrefix + outcome.Code;

            if (lessonByCode.TryGetValue(code, out var codeCollision) && codeCollision.Id != id)
            {
                throw new InvalidOperationException(
                    $"Cambridge derived lesson code collision: {code}.");
            }

            CurriculumPedagogicalLesson lesson;
            if (!lessonById.TryGetValue(id, out lesson!))
            {
                var parent = outcome.ParentId.HasValue &&
                             nodeById.TryGetValue(outcome.ParentId.Value, out var parentNode)
                    ? parentNode
                    : null;

                lesson = new CurriculumPedagogicalLesson
                {
                    Id = id,
                    FrameworkVersionId = state.FrameworkVersionId,
                    OfficialLessonNodeId = null,
                    Code = code,
                    UnitKey = parent?.Code ?? $"CAM:SCOPE:{outcome.LogicalLevelFrom}",
                    UnitTitle = parent?.Title ?? outcome.NativeLevel,
                    Title = $"Mathematics reference {outcome.SourceLocator}",
                    LogicalLevelFrom = outcome.LogicalLevelFrom,
                    LogicalLevelTo = outcome.LogicalLevelTo,
                    NativeLevel = outcome.NativeLevel,
                    Pathway = outcome.Pathway,
                    SortOrder = 100_000 + outcome.SortOrder,
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now,
                    RowVersion = []
                };

                _db.CurriculumPedagogicalLessons.Add(lesson);
                lessonById[id] = lesson;
                lessonByCode[code] = lesson;
            }

            if (existingMappings.Any(x =>
                    x.PedagogicalLessonId == id &&
                    x.OutcomeNodeId == outcome.Id) ||
                !addedMappings.Add((id, outcome.Id)))
            {
                continue;
            }

            _db.CurriculumPedagogicalLessonOutcomes.Add(
                new CurriculumPedagogicalLessonOutcome
                {
                    PedagogicalLessonId = id,
                    FrameworkVersionId = state.FrameworkVersionId,
                    OutcomeNodeId = outcome.Id,
                    SortOrder = 1
                });
        }

        await _db.SaveChangesAsync(ct);
    }

    private static Guid G(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        Span<byte> guidBytes = stackalloc byte[16];
        bytes.AsSpan(0, 16).CopyTo(guidBytes);
        return new Guid(guidBytes);
    }
}
