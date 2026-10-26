namespace Edulytics.Web.Curriculum.Grade3;

/// <summary>Independent equal-unit grid, optionally omitting an upper-right corner.</summary>
public sealed record Grade3AreaGrid(
    int Rows, int Columns, string Unit,
    int TopRows = 0, int TopColumns = 0, int ShadedColumns = 0);
