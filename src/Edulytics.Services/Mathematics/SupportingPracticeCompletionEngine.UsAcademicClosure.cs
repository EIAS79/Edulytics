namespace Edulytics.Services.Mathematics;

/// <summary>
/// CCSS HSN-VM.B.4–5 / HSS-MD.B.5 narrow assessed skills.
/// All answers are recomputed solely from original sampled parameters.
/// These families do not certify unrelated content standards.
/// </summary>
internal static partial class SupportingPracticeCompletionEngine
{
    private static Problem VectorComponentSum(Random r, int scale)
    {
        var ax = r.Next(-15 - scale, 16 + scale);
        var ay = r.Next(-15 - scale, 16 + scale);
        var bx = r.Next(-15 - scale, 16 + scale);
        var by = r.Next(-15 - scale, 16 + scale);
        return P("usccss.vector.add_x",
            $"Two vectors a=(${ax},${ay}) and b=(${bx},${by}) are arranged head-to-tail. What is the x-component of their resultant a+b?",
            "Add x-components; the diagonal of the parallelogram has the same components as the head-to-tail sum.",
            ("ax",ax),("ay",ay),("bx",bx),("by",by));
    }

    private static Problem VectorDifference(Random r, int scale)
    {
        var ax=r.Next(-15-scale,16+scale);
        var bx=r.Next(-15-scale,16+scale);
        var ay=r.Next(-15-scale,16+scale);
        var by=r.Next(-15-scale,16+scale);
        return P("usccss.vector.subtract_y",
            $"Given a=(${ax},${ay}) and b=(${bx},${by}), find the y-component of a−b=a+(−b).",
            "Negate both components of b and add to a, or subtract b's y-component directly.",
            ("ax",ax),("ay",ay),("bx",bx),("by",by));
    }

    private static Problem VectorScalar(Random r,int scale)
    {
        var x=r.Next(-12-scale,13+scale);
        var y=r.Next(-12-scale,13+scale);
        var k=NonZero(r,-6-scale,7+scale);
        return P("usccss.vector.scalar_y",
            $"Let v=(${x},${y}). The scalar is k=${k}. Find the y-component of kv. A negative k reverses the vector's direction.",
            "Scalar multiplication multiplies both components by k; the sign can reverse direction.",
            ("x",x),("y",y),("k",k));
    }

    private static Problem VectorSumMagnitude(Random r,int scale)
    {
        var factor=r.Next(1,6+scale);
        var ux=r.Next(-10,11);
        var uy=r.Next(-10,11);
        var vx=3*factor-ux;
        var vy=4*factor-uy;
        return P("usccss.vector.sum_magnitude",
            $"Two displacement vectors are a=(${ux},${uy}) and b=(${vx},${vy}). Find the magnitude ||a+b||.",
            "Add components first and use √(x²+y²); here the resultant is a 3–4–5 scaled triangle.",
            ("ux",ux),("uy",uy),("vx",vx),("vy",vy));
    }

    private static Problem VectorScalarMagnitude(Random r,int scale)
    {
        var f=r.Next(1,6+scale);
        var k=NonZero(r,-6-scale,7+scale);
        return P("usccss.vector.scalar_magnitude",
            $"v=(${3*f},${4*f}) and k=${k}. Find ||kv||. Remember that the length factor is |k|, not k.",
            "First find ||v|| using the Pythagorean theorem, then multiply by |k|.",
            ("factor",f),("k",k));
    }

    private static Problem VectorResultantDirection(Random r,int scale)
    {
        var ux=r.Next(-8,9);
        var uy=r.Next(-8,9);
        var bx=r.Next(2,18+scale);
        var by=r.Next(2,18+scale);
        return P("usccss.vector.resultant_angle",
            $"a=(${ux},${uy}) and b=(${bx-ux},${by-uy}). Find the direction of a+b in degrees anticlockwise from the positive x-axis, rounded to the nearest whole degree.",
            "Add components, identify the quadrant, and apply atan2(y,x), converted to degrees. The resultant lies in the first quadrant here.",
            ("ux",ux),("uy",uy),("vx",bx-ux),("vy",by-uy));
    }

    private static Problem NetGameExpectedValue(Random r,int scale)
    {
        var q=r.Next(2,8+scale);
        var prize=q*r.Next(4,19+scale);
        var ticket=r.Next(1,17+scale);
        return P("usccss.probability.expected_net_payoff",
            $"A ticket costs $${ticket}. The gross prize is $${prize} with probability 1/${q}, otherwise $0. What is the expected NET payoff per ticket, in dollars?",
            "Expected gross winnings = prize/q. Subtract the ticket cost; report a negative expected value if appropriate.",
            ("q",q),("prize",prize),("ticket",ticket));
    }

    private static Problem CompareExpectedCost(Random r,int scale)
    {
        var q=r.Next(2,7+scale);
        var baseCost=r.Next(2,14+scale);
        var extra=q*r.Next(1,11+scale);
        var diff=r.Next(-9-scale,10+scale);
        var fixedCost=baseCost+extra/q+diff;
        if(fixedCost<1)fixedCost=1;
        return P("usccss.probability.compare_expected_cost",
            $"Plan A always costs $${fixedCost}. Plan B costs $${baseCost} normally plus $${extra} with probability 1/${q}. Ignoring risk preferences, which has LOWER expected cost? Enter 1 for Plan A, 2 for Plan B, or 0 for a tie.",
            "Plan A expected cost is fixed. Plan B expected cost is base cost + probability × extra. Compare the two values.",
            ("q",q),("baseCost",baseCost),("extra",extra),("fixedCost",fixedCost));
    }
}