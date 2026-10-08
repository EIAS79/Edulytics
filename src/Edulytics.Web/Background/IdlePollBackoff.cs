namespace Edulytics.Web.Background;

/// <summary>
/// Reduces database polling when no queue work is available.
/// The minimum guards against accidentally reverting to sub-second polling
/// through an environment override; a successful dequeue resets the delay.
/// </summary>
public sealed class IdlePollBackoff
{
    public const int MinimumDelayMilliseconds = 5000;

    private readonly int _initialMilliseconds;
    private readonly int _maximumMilliseconds;
    private int _nextMilliseconds;

    public IdlePollBackoff(
        int configuredInitialMilliseconds,
        int configuredMaximumMilliseconds)
    {
        _initialMilliseconds = Math.Max(
            MinimumDelayMilliseconds,
            configuredInitialMilliseconds);
        _maximumMilliseconds = Math.Max(
            _initialMilliseconds,
            configuredMaximumMilliseconds);
        _nextMilliseconds = _initialMilliseconds;
    }

    public TimeSpan NextDelay()
    {
        var delayMilliseconds = _nextMilliseconds;
        _nextMilliseconds = (int)Math.Min(
            (long)_maximumMilliseconds,
            (long)_nextMilliseconds * 2L);
        return TimeSpan.FromMilliseconds(delayMilliseconds);
    }

    public void Reset() =>
        _nextMilliseconds = _initialMilliseconds;
}
