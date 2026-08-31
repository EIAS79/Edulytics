using System.Net;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29StagingDeploymentProbeTests
{
    [Fact]
    public async Task MergedPhase29StagingDeploymentIsReady()
    {
        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        Exception? lastError = null;
        HttpStatusCode? lastStatus = null;
        string? lastBody = null;

        for (var attempt = 1; attempt <= 20; attempt++)
        {
            try
            {
                using var response = await client.GetAsync(
                    "https://staging.edulytiks.com/health/ready");

                lastStatus = response.StatusCode;
                lastBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Assert.False(string.IsNullOrWhiteSpace(lastBody));
                    return;
                }
            }
            catch (Exception ex)
            {
                lastError = ex;
            }

            if (attempt < 20)
            {
                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        throw new Xunit.Sdk.XunitException(
            $"Staging /health/ready did not become successful. " +
            $"LastStatus={(lastStatus?.ToString() ?? "none")}; " +
            $"LastBody={lastBody ?? "<none>"}; " +
            $"LastError={lastError?.Message ?? "<none>"}");
    }
}
