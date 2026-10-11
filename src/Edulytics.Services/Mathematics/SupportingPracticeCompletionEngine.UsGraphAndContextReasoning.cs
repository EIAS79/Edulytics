namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact-answer contextual/diagram interpretation; not arbitrary sketch grading.
/// </summary>
internal static partial class SupportingPracticeCompletionEngine
{
    private static Problem UsRationalHoleOrPole(Random r,int scale)
    {
        int removed=r.Next(-12,13);
        int pole=removed+NonZero(r,-10,11);
        int root=removed+NonZero(r,-9,10);
        if(root==pole)root=pole+1;
        int ask=r.Next(2)==0?removed:pole;
        return P("usccss.hs.rational_hole_or_pole",
            $"Consider f(x)=((x−(${removed}))(x−(${root})))/((x−(${removed}))(x−(${pole}))). The original function excludes x=${removed} and x=${pole}. At x=${ask}, enter 1 for a REMOVABLE HOLE or 2 for a VERTICAL ASYMPTOTE.",
            "A canceled factor gives a hole; an uncanceled denominator root produces an asymptote. Original domain exclusions remain.",
            ("removed",removed),("pole",pole),("root",root),("ask",ask));
    }

    private static Problem UsRationalApproachDirection(Random r,int scale)
    {
        int pole=r.Next(-9,10);
        int numerator=NonZero(r,-9-scale,10+scale);
        int right=r.Next(2);
        string side=right==1?"RIGHT (x>a)":"LEFT (x<a)";
        return P("usccss.hs.rational_branch_limit_sign",
            $"The graph of f(x)=(${numerator})/(x−(${pole})) has a vertical asymptote at x=${pole}. As x approaches ${pole} from the ${side}, choose 1 for f(x)→+∞ or 2 for f(x)→−∞.",
            "Denominator sign depends on side of approach; numerator sign decides whether the branch rises or falls.",
            ("pole",pole),("numerator",numerator),("right",right));
    }

    private static Problem UsVectorHeadToTailEndpoint(Random r,int scale)
    {
        int startY=r.Next(-10,11),startX=r.Next(-10,11);
        int ax=r.Next(-10-scale,11+scale),ay=r.Next(-10-scale,11+scale);
        int bx=r.Next(-10-scale,11+scale),by=r.Next(-10-scale,11+scale);
        return P("usccss.vector.head_tail_endpoint_y",
            $"Start at (${startX},${startY}). Draw a=(${ax},${ay}) with its tail at the start, then b=(${bx},${by}) with its tail at the HEAD of a. What is the final endpoint's y-coordinate?",
            "Endpoint = start + a + b; the diagonal of the parallelogram gives the same resultant.",
            ("startX",startX),("startY",startY),("ax",ax),("ay",ay),("bx",bx),("by",by));
    }

    private static Problem UsVectorScaleDirection(Random r,int scale)
    {
        int x=NonZero(r,-7-scale,8+scale),y=r.Next(-8,9);
        int k=r.Next(-5-scale,6+scale);
        return P("usccss.vector.scalar_direction_class",
            $"For NONZERO v=(${x},${y}), let w=${k}v. Enter 1 if w has the SAME direction as v, 2 if OPPOSITE, or 0 if w is the ZERO vector with NO direction.",
            "Positive scalar preserves direction; negative reverses direction; zero produces a zero vector with no defined direction.",
            ("x",x),("y",y),("k",k));
    }

    private static Problem UsExpectedVsRiskDecision(Random r,int scale)
    {
        int denom=r.Next(3,7+scale);
        int step=r.Next(2,9+scale);
        int baseCost=r.Next(2,16+scale);
        int extra=denom*step;
        int fixedCost=baseCost+step+1;
        int objective=r.Next(2);
        string criterion=objective==0?"MINIMUM EXPECTED COST":"MINIMUM WORST-CASE COST";
        return P("usccss.probability.expected_vs_worst_case",
            $"Plan A always costs $${fixedCost}. Plan B costs $${baseCost}, plus $${extra} in an incident with probability 1/${denom}. Choose the plan with ${criterion} (enter 1 for A or 2 for B).",
            "B's expected cost is base+extra/denominator; worst-case B is base+extra. Compare by the stated criterion. Numeric selection alone does not grade written justification.",
            ("denom",denom),("step",step),("baseCost",baseCost),("extra",extra),("fixedCost",fixedCost),("objective",objective));
    }
}
