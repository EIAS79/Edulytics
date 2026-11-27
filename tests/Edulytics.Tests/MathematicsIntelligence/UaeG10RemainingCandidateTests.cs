using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10RemainingCandidateTests
{
    [Theory]
    [InlineData(3, "1")]
    [InlineData(4, "2")]
    [InlineData(5, "3")]
    public void TriangleClassificationAndLargestAngleCoverAllCases(int a, string answer)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",4},{"c",5},{"d",6},{"e",2},{"f",7}};
        foreach(var family in new[]{"uae.g10.general.u09.l01.classify_triangle_sides", "uae.g10.general.u10.l03.largest_opposite_angle"})
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
            foreach(var wrong in new[]{"1","2","3"}.Where(x=>x!=answer))
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,wrong));
        }
    }

    [Theory]
    [InlineData("uae.g10.general.u07.l01.line_pair_type", "1")]
    [InlineData("uae.g10.general.u07.l01.plane_pair_type", "2")]
    [InlineData("uae.g10.general.u07.l06.perpendicular_point_line_distance", "6")]
    [InlineData("uae.g10.general.u07.l06.perpendicular_slope", "-4/3")]
    [InlineData("uae.g10.general.u08.l01.reflection_vertical_x", "13")]
    [InlineData("uae.g10.general.u08.l01.reflection_horizontal_y", "11")]
    [InlineData("uae.g10.general.u08.l01.reflection_diagonal_y", "-3")]
    [InlineData("uae.g10.general.u08.l02.translation_x", "2")]
    [InlineData("uae.g10.general.u08.l02.translation_y", "-7")]
    [InlineData("uae.g10.general.u08.l03.rotation_90_x", "1")]
    [InlineData("uae.g10.general.u08.l03.rotation_180_y", "1")]
    [InlineData("uae.g10.general.u08.l03.rotation_270_y", "3")]
    [InlineData("uae.g10.general.u08.l04.reflect_then_translate_x", "8")]
    [InlineData("uae.g10.general.u08.l04.translate_then_reflect_x", "-2")]
    [InlineData("uae.g10.general.u08.l05.regular_polygon_symmetry_axes", "6")]
    [InlineData("uae.g10.general.u08.l05.least_rotation_angle", "60")]
    [InlineData("uae.g10.general.u08.l06.dilation_origin_x", "-15")]
    [InlineData("uae.g10.general.u08.l06.dilation_side_length", "12/5")]
    [InlineData("uae.g10.general.u09.l01.classify_triangle_sides", "1")]
    [InlineData("uae.g10.general.u09.l01.classify_triangle_angles", "2")]
    [InlineData("uae.g10.general.u09.l02.third_triangle_angle", "104")]
    [InlineData("uae.g10.general.u09.l02.exterior_triangle_angle", "72")]
    [InlineData("uae.g10.general.u09.l03.corresponding_side_length", "7")]
    [InlineData("uae.g10.general.u09.l03.corresponding_side_equation", "3")]
    [InlineData("uae.g10.general.u09.l04.sss_match_condition", "1")]
    [InlineData("uae.g10.general.u09.l04.sas_included_angle", "1")]
    [InlineData("uae.g10.general.u09.l05.asa_included_side", "1")]
    [InlineData("uae.g10.general.u09.l05.aas_remaining_angle", "113")]
    [InlineData("uae.g10.general.u09.l06.isosceles_base_angle", "77")]
    [InlineData("uae.g10.general.u09.l06.equilateral_perimeter", "21")]
    [InlineData("uae.g10.general.u09.l07.rigid_image_side_length", "7")]
    [InlineData("uae.g10.general.u09.l07.rigid_image_angle", "43")]
    [InlineData("uae.g10.general.u09.l08.coordinate_side_square", "25")]
    [InlineData("uae.g10.general.u09.l08.coordinate_midpoint_x", "-2")]
    [InlineData("uae.g10.general.u09.l09.parallelogram_area", "35")]
    [InlineData("uae.g10.general.u09.l09.triangle_area", "12")]
    [InlineData("uae.g10.general.u10.l01.perpendicular_bisector_distance", "7")]
    [InlineData("uae.g10.general.u10.l01.angle_bisector_equidistance", "8")]
    [InlineData("uae.g10.general.u10.l02.centroid_vertex_segment", "6")]
    [InlineData("uae.g10.general.u10.l02.centroid_midpoint_segment", "7")]
    [InlineData("uae.g10.general.u10.l03.largest_opposite_angle", "1")]
    [InlineData("uae.g10.general.u10.l03.exterior_angle_bound", "34")]
    [InlineData("uae.g10.general.u10.l04.negated_inequality_membership", "1")]
    [InlineData("uae.g10.general.u10.l04.contradiction_upper_bound", "6")]
    [InlineData("uae.g10.general.u10.l05.triangle_third_side_min_integer", "2")]
    [InlineData("uae.g10.general.u10.l05.triangle_third_side_max_integer", "6")]
    [InlineData("uae.g10.general.u10.l06.hinge_opposite_side_relation", "-1")]
    [InlineData("uae.g10.general.u10.l06.hinge_converse_angle_relation", "-1")]
    [InlineData("uae.g10.general.u11.l01.proportion_missing_term", "9/2")]
    [InlineData("uae.g10.general.u11.l01.partition_by_ratio", "15")]
    [InlineData("uae.g10.general.u11.l02.similar_polygon_side", "20/3")]
    [InlineData("uae.g10.general.u11.l02.similar_polygon_perimeter", "72")]
    [InlineData("uae.g10.general.u11.l03.similar_triangle_aa_angle", "123")]
    [InlineData("uae.g10.general.u11.l03.similar_triangle_corresponding_side", "20")]
    [InlineData("uae.g10.general.u11.l04.parallel_segment_proportion", "20")]
    [InlineData("uae.g10.general.u11.l04.triangle_midsegment_length", "7/2")]
    [InlineData("uae.g10.general.u11.l05.similar_triangle_altitude", "20")]
    [InlineData("uae.g10.general.u11.l05.similar_triangle_median", "20/3")]
    [InlineData("uae.g10.general.u11.l06.similarity_dilation_translation_x", "-4")]
    [InlineData("uae.g10.general.u11.l06.similarity_transformed_length", "35/6")]
    [InlineData("uae.g10.general.u11.l07.map_actual_distance_km", "27")]
    [InlineData("uae.g10.general.u11.l07.model_scale_length_cm", "20/3")]
    [InlineData("uae.g10.general.u12.l01.polygon_interior_sum", "720")]
    [InlineData("uae.g10.general.u12.l01.regular_polygon_exterior_angle", "60")]
    [InlineData("uae.g10.general.u12.l02.parallelogram_adjacent_angle", "131")]
    [InlineData("uae.g10.general.u12.l02.parallelogram_diagonal_half", "7")]
    [InlineData("uae.g10.general.u12.l03.parallelogram_sufficient_condition", "1")]
    [InlineData("uae.g10.general.u12.l03.parallelogram_diagonals_test", "0")]
    [InlineData("uae.g10.general.u12.l04.rectangle_equal_diagonals", "12")]
    [InlineData("uae.g10.general.u12.l04.rectangle_diagonal_length", "15")]
    [InlineData("uae.g10.general.u12.l05.rhombus_side_from_diagonals", "15")]
    [InlineData("uae.g10.general.u12.l05.square_perimeter", "28")]
    [InlineData("uae.g10.general.u12.l06.trapezoid_midsegment", "9")]
    [InlineData("uae.g10.general.u12.l06.kite_perimeter", "24")]
    [InlineData("uae.g10.general.u16.l01.sample_space_product_count", "12")]
    [InlineData("uae.g10.general.u16.l01.sample_space_three_stages", "60")]
    [InlineData("uae.g10.general.u16.l02.permutations_two_positions", "30")]
    [InlineData("uae.g10.general.u16.l02.combinations_two_members", "15")]
    [InlineData("uae.g10.general.u16.l02.combination_probability", "1/7")]
    [InlineData("uae.g10.general.u16.l03.geometric_segment_probability", "3/7")]
    [InlineData("uae.g10.general.u16.l03.geometric_rectangle_probability", "15/77")]
    [InlineData("uae.g10.general.u16.l04.simulation_relative_frequency", "3/7")]
    [InlineData("uae.g10.general.u16.l04.simulation_expected_count", "15")]
    [InlineData("uae.g10.general.u16.l06.independent_two_events", "9/49")]
    [InlineData("uae.g10.general.u16.l06.dependent_without_replacement", "2/7")]
    [InlineData("uae.g10.general.u16.l07.mutually_exclusive_union", "7/12")]
    [InlineData("uae.g10.general.u16.l07.overlapping_union", "2/3")]
    [InlineData("uae.g10.general.u17.l01.sequence_function_term", "19")]
    [InlineData("uae.g10.general.u17.l01.sequence_common_difference", "4")]
    [InlineData("uae.g10.general.u17.l02.arithmetic_nth_term", "19")]
    [InlineData("uae.g10.general.u17.l02.arithmetic_finite_sum", "55")]
    [InlineData("uae.g10.general.u17.l03.geometric_nth_term", "27")]
    [InlineData("uae.g10.general.u17.l03.geometric_finite_sum", "39")]
    [InlineData("uae.g10.general.u17.l04.geometric_infinite_sum", "4")]
    [InlineData("uae.g10.general.u17.l04.geometric_infinite_alternating_sum", "12/5")]
    [InlineData("uae.g10.general.u17.l04.geometric_infinite_convergence", "1")]
    public void GoldenSubskillAndMalformedInput(string family,string expected)
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",4},{"c",5},{"d",6},{"e",2},{"f",7}};
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.NotNull(interaction);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected),family);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,string.Concat(expected.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch))));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"999999"));
        p["a"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected));
    }
}
