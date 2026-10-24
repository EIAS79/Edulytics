namespace Edulytics.Web.Curriculum.Grade3;

/// <summary>A top rectangle joined to a lower leg; dimensions are whole unit lengths.</summary>
public sealed record Grade3CompositeAreaFigure(
    int TopWidth, int TopHeight, int LegWidth, int LegHeight, int LegOffset,
    bool ShowTicks = false, bool ShowDimensions = false,
    int PartialGridRows = 0, int PartialGridStartColumn = 0,
    string Unit = "units");
