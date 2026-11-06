namespace Edulytics.Web.Presentation;

/// <summary>
/// Instructor-only checklist of original numbered Section 1.1 exercise atoms.
/// 92 parent exercises => 56 singleton actions and 106 explicit parts =>
/// 162 atomic review actions. This DOES NOT assert authenticated grading,
/// complete worked-example atoms or original source image equivalence.
/// </summary>
public static class Grade12PrecalculusSection11SourcePartChecklist
{
    public sealed record Part(int ParentExercise, string Label, string RequestedSkill,
        bool OriginalSourceNumericalResultUnresolved, string SourceElementId);

    private static readonly IReadOnlyDictionary<int, IReadOnlyList<Part>> Parts =
        new Dictionary<int, IReadOnlyList<Part>>
        {
            [1] = new Part[]
            {
                new(1, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE001"),
            },
            [2] = new Part[]
            {
                new(2, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE002"),
            },
            [3] = new Part[]
            {
                new(3, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE003"),
            },
            [4] = new Part[]
            {
                new(4, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE004"),
            },
            [5] = new Part[]
            {
                new(5, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE005"),
            },
            [6] = new Part[]
            {
                new(6, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE006"),
            },
            [7] = new Part[]
            {
                new(7, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE007"),
            },
            [8] = new Part[]
            {
                new(8, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE008"),
            },
            [9] = new Part[]
            {
                new(9, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE009"),
            },
            [10] = new Part[]
            {
                new(10, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE010"),
            },
            [11] = new Part[]
            {
                new(11, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE011"),
            },
            [12] = new Part[]
            {
                new(12, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE012"),
            },
            [13] = new Part[]
            {
                new(13, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE013"),
            },
            [14] = new Part[]
            {
                new(14, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE014"),
            },
            [15] = new Part[]
            {
                new(15, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE015"),
            },
            [16] = new Part[]
            {
                new(16, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE016"),
            },
            [17] = new Part[]
            {
                new(17, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE017"),
            },
            [18] = new Part[]
            {
                new(18, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE018"),
            },
            [19] = new Part[]
            {
                new(19, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE019"),
            },
            [20] = new Part[]
            {
                new(20, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE020"),
            },
            [21] = new Part[]
            {
                new(21, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE021"),
            },
            [22] = new Part[]
            {
                new(22, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE022"),
            },
            [23] = new Part[]
            {
                new(23, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE023"),
            },
            [24] = new Part[]
            {
                new(24, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE024"),
            },
            [25] = new Part[]
            {
                new(25, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE025"),
            },
            [26] = new Part[]
            {
                new(26, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE026"),
            },
            [27] = new Part[]
            {
                new(27, "f(−3)", "Evaluate f(−3) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE027-PART01"),
                new(27, "f(2)", "Evaluate f(2) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE027-PART02"),
                new(27, "f(−a)", "Evaluate f(−a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE027-PART03"),
                new(27, "−f(a)", "Evaluate −f(a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE027-PART04"),
                new(27, "f(a+h)", "Evaluate f(a+h) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE027-PART05"),
            },
            [28] = new Part[]
            {
                new(28, "f(−3)", "Evaluate f(−3) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE028-PART01"),
                new(28, "f(2)", "Evaluate f(2) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE028-PART02"),
                new(28, "f(−a)", "Evaluate f(−a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE028-PART03"),
                new(28, "−f(a)", "Evaluate −f(a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE028-PART04"),
                new(28, "f(a+h)", "Evaluate f(a+h) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE028-PART05"),
            },
            [29] = new Part[]
            {
                new(29, "f(−3)", "Evaluate f(−3) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE029-PART01"),
                new(29, "f(2)", "Evaluate f(2) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE029-PART02"),
                new(29, "f(−a)", "Evaluate f(−a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE029-PART03"),
                new(29, "−f(a)", "Evaluate −f(a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE029-PART04"),
                new(29, "f(a+h)", "Evaluate f(a+h) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE029-PART05"),
            },
            [30] = new Part[]
            {
                new(30, "f(−3)", "Evaluate f(−3) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE030-PART01"),
                new(30, "f(2)", "Evaluate f(2) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE030-PART02"),
                new(30, "f(−a)", "Evaluate f(−a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE030-PART03"),
                new(30, "−f(a)", "Evaluate −f(a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE030-PART04"),
                new(30, "f(a+h)", "Evaluate f(a+h) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE030-PART05"),
            },
            [31] = new Part[]
            {
                new(31, "f(−3)", "Evaluate f(−3) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE031-PART01"),
                new(31, "f(2)", "Evaluate f(2) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE031-PART02"),
                new(31, "f(−a)", "Evaluate f(−a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE031-PART03"),
                new(31, "−f(a)", "Evaluate −f(a) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE031-PART04"),
                new(31, "f(a+h)", "Evaluate f(a+h) with the exercise's function rule and necessary domain restrictions", false, "G12-U01-L01-EXERCISE031-PART05"),
            },
            [32] = new Part[]
            {
                new(32, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE032"),
            },
            [33] = new Part[]
            {
                new(33, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE033"),
            },
            [34] = new Part[]
            {
                new(34, "a", "Evaluate the requested function value", false, "G12-U01-L01-EXERCISE034-PART01"),
                new(34, "b", "Solve the stated function-output equation for every permitted input", false, "G12-U01-L01-EXERCISE034-PART02"),
            },
            [35] = new Part[]
            {
                new(35, "a", "Evaluate the requested function value", false, "G12-U01-L01-EXERCISE035-PART01"),
                new(35, "b", "Solve the stated function-output equation for every permitted input", false, "G12-U01-L01-EXERCISE035-PART02"),
            },
            [36] = new Part[]
            {
                new(36, "a", "Evaluate the requested function value", false, "G12-U01-L01-EXERCISE036-PART01"),
                new(36, "b", "Solve the stated function-output equation for every permitted input", false, "G12-U01-L01-EXERCISE036-PART02"),
            },
            [37] = new Part[]
            {
                new(37, "a", "Evaluate the requested function value", false, "G12-U01-L01-EXERCISE037-PART01"),
                new(37, "b", "Solve the stated function-output equation for every permitted input", false, "G12-U01-L01-EXERCISE037-PART02"),
            },
            [38] = new Part[]
            {
                new(38, "a", "Evaluate the requested function value", false, "G12-U01-L01-EXERCISE038-PART01"),
                new(38, "b", "Solve the stated function-output equation for every permitted input", false, "G12-U01-L01-EXERCISE038-PART02"),
            },
            [39] = new Part[]
            {
                new(39, "a", "Solve relation for r=f(t)", false, "G12-U01-L01-EXERCISE039-PART01"),
                new(39, "b", "Evaluate f(−3)", false, "G12-U01-L01-EXERCISE039-PART02"),
                new(39, "c", "Solve f(t)=2", false, "G12-U01-L01-EXERCISE039-PART03"),
            },
            [40] = new Part[]
            {
                new(40, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE040"),
            },
            [41] = new Part[]
            {
                new(41, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE041"),
            },
            [42] = new Part[]
            {
                new(42, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE042"),
            },
            [43] = new Part[]
            {
                new(43, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE043"),
            },
            [44] = new Part[]
            {
                new(44, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE044"),
            },
            [45] = new Part[]
            {
                new(45, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE045"),
            },
            [46] = new Part[]
            {
                new(46, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE046"),
            },
            [47] = new Part[]
            {
                new(47, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE047"),
            },
            [48] = new Part[]
            {
                new(48, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE048"),
            },
            [49] = new Part[]
            {
                new(49, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE049"),
            },
            [50] = new Part[]
            {
                new(50, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE050"),
            },
            [51] = new Part[]
            {
                new(51, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE051"),
            },
            [52] = new Part[]
            {
                new(52, "a", "Read f(−1) from the actual source graph", false, "G12-U01-L01-EXERCISE052-PART01"),
                new(52, "b", "Graphically solve f(x)=3 with all indicated roots and graphical precision", true, "G12-U01-L01-EXERCISE052-PART02"),
            },
            [53] = new Part[]
            {
                new(53, "a", "Read f(0) from the actual source graph", false, "G12-U01-L01-EXERCISE053-PART01"),
                new(53, "b", "Graphically solve f(x)=−3 with all indicated roots and graphical precision", false, "G12-U01-L01-EXERCISE053-PART02"),
            },
            [54] = new Part[]
            {
                new(54, "a", "Read f(4) from the actual source graph", false, "G12-U01-L01-EXERCISE054-PART01"),
                new(54, "b", "Graphically solve f(x)=1 with all indicated roots and graphical precision", false, "G12-U01-L01-EXERCISE054-PART02"),
            },
            [55] = new Part[]
            {
                new(55, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE055"),
            },
            [56] = new Part[]
            {
                new(56, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE056"),
            },
            [57] = new Part[]
            {
                new(57, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE057"),
            },
            [58] = new Part[]
            {
                new(58, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE058"),
            },
            [59] = new Part[]
            {
                new(59, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE059"),
            },
            [60] = new Part[]
            {
                new(60, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE060"),
            },
            [61] = new Part[]
            {
                new(61, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE061"),
            },
            [62] = new Part[]
            {
                new(62, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE062"),
            },
            [63] = new Part[]
            {
                new(63, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE063"),
            },
            [64] = new Part[]
            {
                new(64, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE064"),
            },
            [65] = new Part[]
            {
                new(65, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE065"),
            },
            [66] = new Part[]
            {
                new(66, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE066"),
            },
            [67] = new Part[]
            {
                new(67, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE067"),
            },
            [68] = new Part[]
            {
                new(68, "x=-2", "Evaluate f(-2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE068-PART01"),
                new(68, "x=-1", "Evaluate f(-1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE068-PART02"),
                new(68, "x=0", "Evaluate f(0) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE068-PART03"),
                new(68, "x=1", "Evaluate f(1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE068-PART04"),
                new(68, "x=2", "Evaluate f(2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE068-PART05"),
            },
            [69] = new Part[]
            {
                new(69, "x=-2", "Evaluate f(-2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE069-PART01"),
                new(69, "x=-1", "Evaluate f(-1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE069-PART02"),
                new(69, "x=0", "Evaluate f(0) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE069-PART03"),
                new(69, "x=1", "Evaluate f(1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE069-PART04"),
                new(69, "x=2", "Evaluate f(2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE069-PART05"),
            },
            [70] = new Part[]
            {
                new(70, "x=-2", "Evaluate f(-2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE070-PART01"),
                new(70, "x=-1", "Evaluate f(-1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE070-PART02"),
                new(70, "x=0", "Evaluate f(0) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE070-PART03"),
                new(70, "x=1", "Evaluate f(1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE070-PART04"),
                new(70, "x=2", "Evaluate f(2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE070-PART05"),
            },
            [71] = new Part[]
            {
                new(71, "x=-2", "Evaluate f(-2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE071-PART01"),
                new(71, "x=-1", "Evaluate f(-1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE071-PART02"),
                new(71, "x=0", "Evaluate f(0) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE071-PART03"),
                new(71, "x=1", "Evaluate f(1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE071-PART04"),
                new(71, "x=2", "Evaluate f(2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE071-PART05"),
            },
            [72] = new Part[]
            {
                new(72, "x=-2", "Evaluate f(-2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE072-PART01"),
                new(72, "x=-1", "Evaluate f(-1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE072-PART02"),
                new(72, "x=0", "Evaluate f(0) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE072-PART03"),
                new(72, "x=1", "Evaluate f(1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE072-PART04"),
                new(72, "x=2", "Evaluate f(2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE072-PART05"),
            },
            [73] = new Part[]
            {
                new(73, "x=-2", "Evaluate f(-2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE073-PART01"),
                new(73, "x=-1", "Evaluate f(-1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE073-PART02"),
                new(73, "x=0", "Evaluate f(0) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE073-PART03"),
                new(73, "x=1", "Evaluate f(1) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE073-PART04"),
                new(73, "x=2", "Evaluate f(2) and preserve applicable function domain", false, "G12-U01-L01-EXERCISE073-PART05"),
            },
            [74] = new Part[]
            {
                new(74, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE074"),
            },
            [75] = new Part[]
            {
                new(75, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE075"),
            },
            [76] = new Part[]
            {
                new(76, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE076-PART01"),
                new(76, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE076-PART02"),
            },
            [77] = new Part[]
            {
                new(77, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE077-PART01"),
                new(77, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE077-PART02"),
            },
            [78] = new Part[]
            {
                new(78, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE078-PART01"),
                new(78, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE078-PART02"),
            },
            [79] = new Part[]
            {
                new(79, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE079-PART01"),
                new(79, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE079-PART02"),
            },
            [80] = new Part[]
            {
                new(80, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE080-PART01"),
                new(80, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE080-PART02"),
            },
            [81] = new Part[]
            {
                new(81, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE081-PART01"),
                new(81, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE081-PART02"),
            },
            [82] = new Part[]
            {
                new(82, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE082-PART01"),
                new(82, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE082-PART02"),
            },
            [83] = new Part[]
            {
                new(83, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE083-PART01"),
                new(83, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE083-PART02"),
            },
            [84] = new Part[]
            {
                new(84, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE084-PART01"),
                new(84, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE084-PART02"),
            },
            [85] = new Part[]
            {
                new(85, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE085-PART01"),
                new(85, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE085-PART02"),
            },
            [86] = new Part[]
            {
                new(86, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE086-PART01"),
                new(86, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE086-PART02"),
            },
            [87] = new Part[]
            {
                new(87, "graph", "Graph the specified function on the exact given viewing window with labelled units and axes", false, "G12-U01-L01-EXERCISE087-PART01"),
                new(87, "range", "Determine the corresponding range for that same window, with correct endpoint precision", false, "G12-U01-L01-EXERCISE087-PART02"),
            },
            [88] = new Part[]
            {
                new(88, "a", "Interpret and/or express the first original contextual statement with units", false, "G12-U01-L01-EXERCISE088-PART01"),
                new(88, "b", "Interpret the second original contextual statement with units", false, "G12-U01-L01-EXERCISE088-PART02"),
            },
            [89] = new Part[]
            {
                new(89, "a", "Interpret and/or express the first original contextual statement with units", false, "G12-U01-L01-EXERCISE089-PART01"),
                new(89, "b", "Interpret the second original contextual statement with units", false, "G12-U01-L01-EXERCISE089-PART02"),
            },
            [90] = new Part[]
            {
                new(90, "a", "Interpret and/or express the first original contextual statement with units", false, "G12-U01-L01-EXERCISE090-PART01"),
                new(90, "b", "Interpret the second original contextual statement with units", false, "G12-U01-L01-EXERCISE090-PART02"),
            },
            [91] = new Part[]
            {
                new(91, "a", "Interpret and/or express the first original contextual statement with units", false, "G12-U01-L01-EXERCISE091-PART01"),
                new(91, "b", "Interpret the second original contextual statement with units", false, "G12-U01-L01-EXERCISE091-PART02"),
            },
            [92] = new Part[]
            {
                new(92, "whole exercise", "Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested.", false, "G12-U01-L01-EXERCISE092"),
            },
        };

    public static IReadOnlyList<Part> ForExercise(int number) =>
        Parts.TryGetValue(number, out var items) ? items : [];

    public static int OriginalSourceExerciseCount => Parts.Count;
    public static int OriginalAtomicReviewActions => Parts.Values.Sum(parts => parts.Count);
    public static int ExplicitSourceSubparts =>
        Parts.Values.SelectMany(p => p).Count(x => x.Label != "whole exercise");
}
