namespace Edulytics.Services.DirectStudents;

public interface IDirectPremiumCatalogService
{
    Task<DirectPremiumCatalog> GetAsync(
        CancellationToken cancellationToken = default);

    DirectPremiumPlan GetPlan(string code);
}
