namespace Edulytics.Web.Curriculum.Grade3;

/// <summary>Independent table representation; no publisher image assets.</summary>
public sealed record Grade3MultiplicationTable(
    int Size,
    int RectangleRows = 0,
    int RectangleColumns = 0,
    bool ShowRectangleProduct = false,
    bool SeedTwos = false);
