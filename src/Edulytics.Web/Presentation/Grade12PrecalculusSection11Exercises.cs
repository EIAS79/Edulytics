namespace Edulytics.Web.Presentation;

/// <summary>
/// Instructor-reviewable first-party practice tasks addressing the 92 original
/// Section 1.1 exercise learning demands. This bank is not automatically graded,
/// not the original figures, and NOT a claim of full 1:1 source parity.
/// </summary>
public static class Grade12PrecalculusSection11Exercises
{
    public sealed record Task(int Number, string Skill, string Prompt,
        string ReviewMode, string AnswerKind);
    public static readonly IReadOnlyList<Task> All =
    [
        new(1, "Definition and conceptual reasoning", "Explain how a relation differs from a function. Illustrate each with ordered pairs.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(2, "Definition and conceptual reasoning", "Distinguish an independent input from its dependent output in a function. Give an example with meaningful units.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(3, "Definition and conceptual reasoning", "Explain why the vertical-line test detects a relation that has multiple outputs for the same input.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(4, "Definition and conceptual reasoning", "State and justify a method for deciding whether a function is one-to-one on its specified domain.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(5, "Definition and conceptual reasoning", "Explain why a horizontal line crossing a function graph more than once proves it is not one-to-one.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(6, "Relations and functions", "Decide whether {(a,b),(c,d),(a,c)} is a function of its first entry. Justify a repeated-input case.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(7, "Relations and functions", "Decide whether {(a,b),(b,c),(c,c)} is a function of its first entry. Explain any repeated outputs.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(8, "Implicit relations and domains", "Does the equation 5x + 2y = 10 define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(9, "Implicit relations and domains", "Does the equation y = x² define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(10, "Implicit relations and domains", "Does the equation x = y² define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(11, "Implicit relations and domains", "Does the equation 3x² + y = 14 define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(12, "Implicit relations and domains", "Does the equation 2x + y² = 6 define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(13, "Implicit relations and domains", "Does the equation y = −2x² + 40x define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(14, "Implicit relations and domains", "Does the equation y = 1/x define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(15, "Implicit relations and domains", "Does the equation x = (3y + 5)/(7y − 1) define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(16, "Implicit relations and domains", "Does the equation x = √(1 − y²) define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(17, "Implicit relations and domains", "Does the equation y = (3x + 5)/(7x − 1) define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(18, "Implicit relations and domains", "Does the equation x² + y² = 9 define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(19, "Implicit relations and domains", "Does the equation 2xy = 1 define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(20, "Implicit relations and domains", "Does the equation x = y³ define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(21, "Implicit relations and domains", "Does the equation y = x³ define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(22, "Implicit relations and domains", "Does the equation y = √(1 − x²) define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(23, "Implicit relations and domains", "Does the equation x = ±√(1 − y) define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(24, "Implicit relations and domains", "Does the equation y = ±√(1 − x) define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(25, "Implicit relations and domains", "Does the equation y² = x² define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(26, "Implicit relations and domains", "Does the equation y³ = x² define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED", "SYMBOLIC_PROOF"),
        new(27, "Function notation", "Given f(x)=2x−5, calculate f(−3), f(2), f(−a), −f(a), and f(a+h). Show substitutions and state any domain restrictions.", "TEACHER_REVIEW_REQUIRED", "MULTIPART_SYMBOLIC"),
        new(28, "Function notation", "Given f(x)=−5x²+2x−1, calculate f(−3), f(2), f(−a), −f(a), and f(a+h). Show substitutions and state any domain restrictions.", "TEACHER_REVIEW_REQUIRED", "MULTIPART_SYMBOLIC"),
        new(29, "Function notation", "Given f(x)=√(2−x)+5, calculate f(−3), f(2), f(−a), −f(a), and f(a+h). Show substitutions and state any domain restrictions.", "TEACHER_REVIEW_REQUIRED", "MULTIPART_SYMBOLIC"),
        new(30, "Function notation", "Given f(x)=(6x−1)/(5x+2), calculate f(−3), f(2), f(−a), −f(a), and f(a+h). Show substitutions and state any domain restrictions.", "TEACHER_REVIEW_REQUIRED", "MULTIPART_SYMBOLIC"),
        new(31, "Function notation", "Given f(x)=|x−1|−|x+1|, calculate f(−3), f(2), f(−a), −f(a), and f(a+h). Show substitutions and state any domain restrictions.", "TEACHER_REVIEW_REQUIRED", "MULTIPART_SYMBOLIC"),
        new(32, "Difference quotients", "For g(x)=5−x², simplify [g(x+h)−g(x)]/h with h≠0. Show the cancellation steps.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(33, "Difference quotients", "For g(x)=x²+2x, simplify [g(x)−g(a)]/(x−a) with x≠a. State why the restriction is needed.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(34, "Evaluate and solve", "Given k(t)=2t−1, evaluate k(2) and solve k(t)=7 for all real permitted inputs, checking every solution.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(35, "Evaluate and solve", "Given f(x)=8−3x, evaluate f(−2) and solve f(x)=−1 for all real permitted inputs, checking every solution.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(36, "Evaluate and solve", "Given p(c)=c²+c, evaluate p(−3) and solve p(c)=2 for all real permitted inputs, checking every solution.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(37, "Evaluate and solve", "Given f(x)=x²−3x, evaluate f(5) and solve f(x)=4 for all real permitted inputs, checking every solution.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(38, "Evaluate and solve", "Given f(x)=√(x+2), evaluate f(7) and solve f(x)=4 for all real permitted inputs, checking every solution.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(39, "Isolating function output", "From the relation 3r+2t=18, (a) write r=f(t), (b) calculate f(−3), and (c) solve f(t)=2.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(40, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a cubic-like curve that rises and falls while still presenting one output at each x. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(41, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a sideways parabola with upper and lower branches. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(42, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a reciprocal-type two-branch hyperbola with a vertical asymptote. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(43, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a connected, nonoverlapping piecewise horizontal–sloped–horizontal segments. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(44, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a closed oval or ellipse. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(45, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a smooth rising S-curve. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(46, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a two horizontal rays whose x-domains overlap: y=3 for x>0 and y=−3 for x<2. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(47, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a nonvertical straight line with a negative slope. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(48, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a two diagonal lines crossing in an X. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(49, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a nonvertical straight line with a positive slope. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(50, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a closed circle. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(51, "Vertical-line test on diagrams", "Draw a labelled coordinate sketch of a sine-wave-shaped continuous curve. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.", "TEACHER_DIAGRAM_REQUIRED", "DIAGRAM_AND_EXPLANATION"),
        new(52, "Reading values from a graph", "Use the independent first-party increasing concave-down graph displayed with this task to read f(−1) and locate all displayed crossings of y=3. The first-party teaching model has a known rule, but the original textbook graph does not state one: explain why its precise root requires separate source verification.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_INTERPRETATION"),
        new(53, "Reading values from a graph", "For the W-shaped curve with a central value 1 and two valleys at (−2,−3) and (2,−3), (a) read f(0), (b) identify all inputs with f(x)=−3. Sketch to scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_INTERPRETATION"),
        new(54, "Reading values from a graph", "For the inverted V with vertex (−3,4) and linear arms through (−6,1) and (0,1), (a) read f(4) from the extended arm, (b) solve f(x)=1 from both sides. Sketch to scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_INTERPRETATION"),
        new(55, "Function and one-to-one tests", "On a clearly labelled graph of a closed oval with upper and lower points for some x, apply the vertical-line test first. If it defines a function, determine whether it is one-to-one by the horizontal-line test, and justify.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_CLASSIFICATION"),
        new(56, "Function and one-to-one tests", "On a clearly labelled graph of a upward-opening V with vertex (1,1), apply the vertical-line test first. If it defines a function, determine whether it is one-to-one by the horizontal-line test, and justify.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_CLASSIFICATION"),
        new(57, "Function and one-to-one tests", "On a clearly labelled graph of a strictly increasing S-shaped curve, apply the vertical-line test first. If it defines a function, determine whether it is one-to-one by the horizontal-line test, and justify.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_CLASSIFICATION"),
        new(58, "Function and one-to-one tests", "On a clearly labelled graph of a strictly decreasing continuous curve, apply the vertical-line test first. If it defines a function, determine whether it is one-to-one by the horizontal-line test, and justify.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_CLASSIFICATION"),
        new(59, "Function and one-to-one tests", "On a clearly labelled graph of a curve that is flat, rises to a peak, then falls, apply the vertical-line test first. If it defines a function, determine whether it is one-to-one by the horizontal-line test, and justify. The printed source example uses horizontal-angle labels in multiples of π. When inspecting the original, preserve those tick labels rather than treating the horizontal axis as ordinary integer units.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_CLASSIFICATION"),
        new(60, "Relations with ordered pairs", "Is the first-coordinate relation {(−1,−1),(−2,−2),(−3,−3)} a function? Explain how repeated inputs and outputs affect classification.", "EXACT_FAMILY_CANDIDATE", "YES_NO_WITH_REASON"),
        new(61, "Relations with ordered pairs", "Is the first-coordinate relation {(3,4),(4,5),(5,6)} a function? Explain how repeated inputs and outputs affect classification.", "EXACT_FAMILY_CANDIDATE", "YES_NO_WITH_REASON"),
        new(62, "Relations with ordered pairs", "Is the first-coordinate relation {(2,5),(7,11),(15,8),(7,9)} a function? Explain how repeated inputs and outputs affect classification.", "EXACT_FAMILY_CANDIDATE", "YES_NO_WITH_REASON"),
        new(63, "Function tables", "Interpret the table (x: 5, 10, 15; y: 3, 8, 14). Is y a function of x? If so, is it one-to-one? Justify both answers.", "EXACT_FAMILY_CANDIDATE", "FUNCTION_CLASSIFICATION"),
        new(64, "Function tables", "Interpret the table (x: 5, 10, 15; y: 3, 8, 8). Is y a function of x? If so, is it one-to-one? Justify both answers.", "EXACT_FAMILY_CANDIDATE", "FUNCTION_CLASSIFICATION"),
        new(65, "Function tables", "Interpret the table (x: 5, 10, 10; y: 3, 8, 14). Is y a function of x? If so, is it one-to-one? Justify both answers.", "EXACT_FAMILY_CANDIDATE", "FUNCTION_CLASSIFICATION"),
        new(66, "Table lookup", "Using the finite table x = [0,1,2,3,4,5,6,7,8,9]; f(x) = [74,28,1,53,56,3,36,45,14,47], evaluate f(3).", "EXACT_FAMILY_CANDIDATE", "INTEGER"),
        new(67, "Inverse lookup", "Using the finite table x = [0,1,2,3,4,5,6,7,8,9]; f(x) = [74,28,1,53,56,3,36,45,14,47], find all tabulated x values for which f(x)=1.", "EXACT_FAMILY_CANDIDATE", "INTEGER_SET"),
        new(68, "Function evaluation", "For f(x)=4−2x, evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.", "EXACT_FAMILY_CANDIDATE", "MULTIPART_EXACT"),
        new(69, "Function evaluation", "For f(x)=8−3x, evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.", "EXACT_FAMILY_CANDIDATE", "MULTIPART_EXACT"),
        new(70, "Function evaluation", "For f(x)=8x²−7x+3, evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.", "EXACT_FAMILY_CANDIDATE", "MULTIPART_EXACT"),
        new(71, "Function evaluation", "For f(x)=3+√(x+3), evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.", "EXACT_FAMILY_CANDIDATE", "MULTIPART_EXACT"),
        new(72, "Function evaluation", "For f(x)=(x−2)/(x+3), evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.", "EXACT_FAMILY_CANDIDATE", "MULTIPART_EXACT"),
        new(73, "Function evaluation", "For f(x)=3^x, evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.", "EXACT_FAMILY_CANDIDATE", "MULTIPART_EXACT"),
        new(74, "Composite function-value arithmetic", "Given f(x)=3x−2 and g(x)=5−x², evaluate 3f(1)−4g(−2) with substitution.", "EXACT_FAMILY_CANDIDATE", "INTEGER"),
        new(75, "Composite function-value arithmetic", "Given f(x)=3x−2 and h(x)=−2x²+3x−1, evaluate f(7/3)−h(−2) with substitution.", "EXACT_FAMILY_CANDIDATE", "INTEGER"),
        new(76, "Graph in specified viewing window", "Graph y=x² on the exact x-window [−0.1,0.1], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(77, "Graph in specified viewing window", "Graph y=x² on the exact x-window [−10,10], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(78, "Graph in specified viewing window", "Graph y=x² on the exact x-window [−100,100], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(79, "Graph in specified viewing window", "Graph y=x³ on the exact x-window [−0.1,0.1], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(80, "Graph in specified viewing window", "Graph y=x³ on the exact x-window [−10,10], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(81, "Graph in specified viewing window", "Graph y=x³ on the exact x-window [−100,100], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(82, "Graph in specified viewing window", "Graph y=√x on the exact x-window [0,0.01], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(83, "Graph in specified viewing window", "Graph y=√x on the exact x-window [0,100], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(84, "Graph in specified viewing window", "Graph y=√x on the exact x-window [0,10000], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(85, "Graph in specified viewing window", "Graph y=∛x on the exact x-window [−0.001,0.001], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(86, "Graph in specified viewing window", "Graph y=∛x on the exact x-window [−1000,1000], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(87, "Graph in specified viewing window", "Graph y=∛x on the exact x-window [−1000000,1000000], with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.", "TEACHER_DIAGRAM_REQUIRED", "GRAPH_AND_RANGE"),
        new(88, "Units in contextual notation", "A city's population p is in thousands and its weekly garbage amount G=f(p) is in tons/week. (a) Express 40,000 people producing 13 tons/week as function notation. (b) Interpret f(5)=2 with the correct units.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(89, "Area and volume units", "A garden area a in ft² maps to dirt volume D=g(a) in yd³. (a) Write the function observation for a 5,000 ft² garden requiring 50 yd³. (b) Interpret g(100)=1 including the units.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(90, "Time offsets in notation", "If f(t) counts ducks t years after 1990, interpret f(5)=30 and f(10)=40 with actual calendar years.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(91, "Function interpretation", "Let h(t) be a rocket's height in feet t seconds after launch. Interpret h(1)=200 and h(2)=350 with units.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
        new(92, "Counterexample to one-to-one", "Show that f(x)=3(x−5)²+7 is not one-to-one on all real inputs. Give two distinct inputs with equal output, and explain the horizontal-line consequence.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_REASONING"),
    ];

    // Original Section 1.1 has exactly 92 numbered exercises.
    // Item 93 is an explicitly separate Edulytics first-party Cool-down.
    public const int CoolDownNumber = 93;

    public static readonly Task CoolDown = new(
        CoolDownNumber,
        "Edulytics original Cool-down",
        "(a) Decide whether R={(1,2),(1,4),(2,4)} is a function and explain. " +
        "(b) For f(x)=x²−2x+1, compute f(−1) with substitutions. " +
        "(c) Is h(x)=x² one-to-one when its domain is x≥0? Prove or explain.",
        "TEACHER_REVIEW_REQUIRED", "MULTIPART_WRITTEN_PROOF");

    // Original Section 1.1 also contains twelve Try It learning actions.
    // Numbers 94–105 are Edulytics internal submission identifiers, not
    // publisher exercise numbers or additions to the 92 exercise denominator.
    public static readonly IReadOnlyList<Task> TryIts =
    [
        new(94, "Try It — independent practice", "In a small library, book IDs A, B, C identify titles Alpha, Beta, Gamma without duplicates. Is title a function of book ID? Is book ID a function of title?", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(95, "Try It — independent practice", "A plant's measured height h in centimetres depends on the day d since planting. Express this as a named function and state what h(8)=17 means.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(96, "Try It — independent practice", "A table contains input/output pairs (1,10), (2,100), (3,1000). Decide whether this is a function and justify.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(97, "Try It — independent practice", "For f(t)=sqrt(t-9), calculate f(13) and state the real domain.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(98, "Try It — independent practice", "For f(t)=sqrt(t-9), find the input with output four.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(99, "Try It — independent practice", "Rewrite x=27y^3 as y=f(x) for real x.", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(100, "Try It — independent practice", "The table shows (0,6), (1,8), (2,10). Find g(1).", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(101, "Try It — independent practice", "On coordinate paper, graph f(x)=|x-1|, then identify each x at which f(x)=1.", "TEACHER_REVIEW_REQUIRED", "DIAGRAM_AND_JUSTIFICATION"),
        new(102, "Try It — independent practice", "Each employee is assigned exactly one department, and at least two employees are assigned to the same department. Is department a function of employee? Is employee a function of department? Is the first function one-to-one?", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(103, "Try It — independent practice", "A score from 0 to 100 determines exactly one letter grade, and several scores share each letter. Is letter grade a function of numeric score? Is it one-to-one?", "TEACHER_REVIEW_REQUIRED", "WRITTEN_EXPLANATION"),
        new(104, "Try It — independent practice", "Sketch y=|x| and explain whether each vertical line meets its graph in at most one point.", "TEACHER_REVIEW_REQUIRED", "DIAGRAM_AND_JUSTIFICATION"),
        new(105, "Try It — independent practice", "Consider the independently drawn circle x²+y²=4 on real coordinates. Is this a one-to-one FUNCTION of x? Check the vertical-line test at x=0 BEFORE applying any horizontal-line argument.", "TEACHER_REVIEW_REQUIRED", "DIAGRAM_AND_JUSTIFICATION"),
    ];

    public static string DisplayLabel(int number) =>
        number == CoolDownNumber ? "Edulytics original Cool-down" :
        number is >= 94 and <= 105 ? $"Try It {number - 93}" :
        number is >= 1 and <= 92 ? $"Exercise {number}" : "Unknown activity";

    public static Task? Find(int number) =>
        number == CoolDownNumber ? CoolDown :
        number is >= 94 and <= 105
            ? TryIts.FirstOrDefault(task => task.Number == number)
            : All.FirstOrDefault(task => task.Number == number);

    // Reading/plotting a graph can be assessed using a labelled coordinate
    // representation supplied as text. This is an accessible teacher-review
    // alternative; it does not silently create or assert a rendered drawing.
    public static bool SupportsGraphEvidence(int number) =>
        number is (>= 40 and <= 59) or (>= 76 and <= 87) or 104 or 105;

    // Section source exercises 76–87 explicitly require graphing the function.
    public static bool RequiresGraphEvidence(int number) =>
        number is >= 76 and <= 87;

    public static string? ComposeTeacherResponse(
        int number, string? explanation, string? graphEvidence)
    {
        if (Find(number) is null || string.IsNullOrWhiteSpace(explanation))
            return null;
        var reasoning = explanation.Trim();
        if (reasoning.Length is < 20 or > 8000)
            return null;
        var evidence = graphEvidence?.Trim() ?? string.Empty;
        if (evidence.Length > 3000 ||
            (RequiresGraphEvidence(number) && evidence.Length < 20) ||
            (!SupportsGraphEvidence(number) && evidence.Length > 0))
            return null;
        if (evidence.Length == 0)
            return reasoning;
        var combined = "Written reasoning:\n" + reasoning +
            "\n\nLabelled graph / coordinate representation:\n" + evidence;
        return combined.Length <= 8000 ? combined : null;
    }
}
