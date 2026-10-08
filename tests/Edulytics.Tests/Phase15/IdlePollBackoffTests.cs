using Edulytics.Web.Background;

namespace Edulytics.Tests.Phase15;

public sealed class IdlePollBackoffTests
{
    [Fact]
    public void EmptyQueues_BackOffGradually_ButNeverPastConfiguredMaximum()
    {
        var backoff = new IdlePollBackoff(5000, 15000);

        Assert.Equal(TimeSpan.FromSeconds(5), backoff.NextDelay());
        Assert.Equal(TimeSpan.FromSeconds(10), backoff.NextDelay());
        Assert.Equal(TimeSpan.FromSeconds(15), backoff.NextDelay());
        Assert.Equal(TimeSpan.FromSeconds(15), backoff.NextDelay());
    }

    [Fact]
    public void NewWork_ResetsPollingLatency()
    {
        var backoff = new IdlePollBackoff(5000, 15000);
        backoff.NextDelay();
        backoff.NextDelay();

        backoff.Reset();

        Assert.Equal(TimeSpan.FromSeconds(5), backoff.NextDelay());
        Assert.Equal(TimeSpan.FromSeconds(10), backoff.NextDelay());
    }

    [Fact]
    public void StaleSubSecondEnvironmentOverrides_CannotRestoreHotPolling()
    {
        var backoff = new IdlePollBackoff(250, 300);

        Assert.Equal(TimeSpan.FromSeconds(5), backoff.NextDelay());
        Assert.Equal(TimeSpan.FromSeconds(5), backoff.NextDelay());
    }

    [Fact]
    public void VeryLargeIntervals_DoNotOverflowExponentialCalculation()
    {
        var backoff = new IdlePollBackoff(1000000000, int.MaxValue);

        Assert.Equal(
            TimeSpan.FromMilliseconds(1000000000),
            backoff.NextDelay());
        Assert.Equal(
            TimeSpan.FromMilliseconds(2000000000),
            backoff.NextDelay());
        Assert.Equal(
            TimeSpan.FromMilliseconds(int.MaxValue),
            backoff.NextDelay());
        Assert.Equal(
            TimeSpan.FromMilliseconds(int.MaxValue),
            backoff.NextDelay());
    }
}
