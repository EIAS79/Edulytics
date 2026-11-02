using Edulytics.Core.Curriculum;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.DirectStudents;

public sealed class DirectPremiumCatalogService
    : IDirectPremiumCatalogService
{
    public const string MonthlyCode = "DIRECT_PREMIUM_MONTHLY";
    public const string AnnualCode = "DIRECT_PREMIUM_ANNUAL";

    private static readonly DirectPremiumPlan[] Plans =
    [
        new(
            MonthlyCode,
            "Premium Monthly",
            DurationMonths: 1,
            BaseAmountAed: 20m,
            DiscountPercent: 0m,
            FinalAmountAed: 20m),
        new(
            AnnualCode,
            "Premium Annual",
            DurationMonths: 10,
            BaseAmountAed: 200m,
            DiscountPercent: 30m,
            FinalAmountAed: 140m)
    ];

    private static readonly DirectSubjectOption[] Subjects =
    [
        new(
            MathematicsCurriculumPackRegistry.MathematicsSubjectCode,
            "Mathematics")
    ];

    private readonly IDirectCurriculumCatalogRepository _catalog;

    public DirectPremiumCatalogService(
        IDirectCurriculumCatalogRepository catalog)
    {
        _catalog = catalog;
    }

    public async Task<DirectPremiumCatalog> GetAsync(
        CancellationToken cancellationToken = default)
    {
        var frameworkRows =
            await _catalog.ListActiveFrameworksAsync(cancellationToken);

        var curricula = frameworkRows
            .Select(row =>
            {
                var pack = MathematicsCurriculumPackRegistry.All
                    .SingleOrDefault(x =>
                        string.Equals(
                            x.Code,
                            row.FrameworkCode,
                            StringComparison.Ordinal));

                if (pack is null)
                    return null;

                var levels = CurriculumLevelIdentityRegistry
                    .ForPack(pack.Code)
                    .Where(level =>
                        pack.Levels.Any(source =>
                            source.LogicalLevel == level.LogicalLevel &&
                            string.Equals(
                                source.Pathway ?? string.Empty,
                                level.Pathway ?? string.Empty,
                                StringComparison.Ordinal)))
                    .Select(level =>
                        new DirectCurriculumLevelOption(
                            level.Key,
                            level.LogicalLevel,
                            level.Label,
                            level.Stage,
                            level.Pathway))
                    .ToArray();

                return new DirectCurriculumOption(
                    row.FrameworkVersionId,
                    row.FrameworkCode,
                    row.FrameworkName,
                    row.FrameworkVersionName,
                    row.CountryCode,
                    levels);
            })
            .Where(x => x is not null)
            .Cast<DirectCurriculumOption>()
            .ToArray();

        return new DirectPremiumCatalog(
            Plans,
            curricula,
            Subjects);
    }

    public DirectPremiumPlan GetPlan(string code) =>
        Plans.SingleOrDefault(x =>
            string.Equals(x.Code, code, StringComparison.Ordinal))
        ?? throw new KeyNotFoundException(
            $"Unknown Direct Premium plan '{code}'.");
}
