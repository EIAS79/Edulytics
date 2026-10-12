namespace Edulytics.Services.Mathematics;

/// <summary>
/// Narrow Grade 2 2.NBT.A.1.a and 2.NBT.A.1.b numeric question candidates:
/// ten tens make a hundred, and 100..900 are 1..9 hundreds with no tens/ones.
/// The official standard still requires place-value representations and
/// teacher-reviewed evidence. These exact generators are SHADOW candidates,
/// not standalone full academic child-clause certification.
/// </summary>
internal static partial class SupportingPracticeCompletionEngine
{
    private static Problem UsHundredsAsTens(Random random)
    {
        var hundreds = random.Next(1, 10);
        return P(
            "usccss.g2.hundreds_as_tens",
            $"A base-ten bundle of 10 tens rods makes 1 hundred. There are {hundreds} full hundreds. " +
            "How many tens rods are in all these bundles? Enter a whole number.",
            "There are exactly 10 tens in a hundred; count ten for each hundred, " +
            "then verify by regrouping all tens into bundles of 10.",
            ("hundreds", hundreds));
    }

    private static Problem UsHundredsDigitToNumber(Random random)
    {
        var hundreds = random.Next(1, 10);
        return P(
            "usccss.g2.hundreds_digit_to_number",
            $"Write the whole number represented by {hundreds} hundreds, 0 tens, and 0 ones.",
            "Each hundred contributes 100 units. A number with no tens " +
            "and no ones is the number of hundreds multiplied by 100.",
            ("hundreds", hundreds));
    }
}
