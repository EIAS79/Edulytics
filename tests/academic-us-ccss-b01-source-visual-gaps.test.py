#!/usr/bin/env python3
import importlib.util
import pathlib
import unittest

source=pathlib.Path(__file__).resolve().parents[1]/"tools/us-ccss-b01-source-visual-gaps.py"
spec=importlib.util.spec_from_file_location("b01visual",source)
audit=importlib.util.module_from_spec(spec)
spec.loader.exec_module(audit)

def fixture():
    publisher={"rows":[{"ordinal":i+1,"lessonCode":f"B01:{i:03d}",
        "publisherTitle":"Learning numbers","sourcePurposePreview":"Explore numbers.",
        "sourceUrl":"https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html",
        "effectiveContentSha256":"a"*64} for i in range(500)]}
    rich={"rows":[{"ordinal":i+1,"lessonCode":f"B01:{i:03d}",
        "effectiveBodyFingerprint":"a"*64,
        "visualKinds":["EquationSet"]} for i in range(500)]}
    return publisher,rich

class PublisherVisualEvidenceTests(unittest.TestCase):
    def test_exact_500_with_generic_visuals_is_not_certification(self):
        a,b=fixture()
        d=audit.analyze(a,b)
        self.assertEqual(d["observed"],500)
        self.assertEqual(d["lessonsWithOnlyGenericEquationSet"],500)
        self.assertEqual(d["academicDecision"],"NOT_CERTIFIED")

    def test_fraction_goal_requires_a_real_fraction_representation(self):
        a,b=fixture()
        a["rows"][14]["publisherTitle"]="Partition Fractions into Equal Parts"
        d=audit.analyze(a,b)
        self.assertEqual(d["visualReviewTypes"]["FRACTION_PARTS"],1)
        b["rows"][14]["visualKinds"]=["FractionBars","EquationSet"]
        d=audit.analyze(a,b)
        self.assertNotIn("FRACTION_PARTS",d["visualReviewTypes"])

    def test_graph_purpose_is_not_satisfied_by_generic_equation_set(self):
        a,b=fixture()
        a["rows"][299]["sourcePurposePreview"]="Choose an appropriate bar graph scale."
        d=audit.analyze(a,b)
        self.assertEqual(d["visualReviewTypes"]["GRAPH_AND_SCALE"],1)

    def test_clock_requires_analog_face_not_generic_scale(self):
        a,b=fixture()
        a["rows"][20]["publisherTitle"]="Tell Time with an Analog Clock"
        b["rows"][20]["visualKinds"]=["Scale","EquationSet"]
        d=audit.analyze(a,b)
        self.assertIn("CLOCK_DIAGRAM",d["rows"][20]["publisherCueVisualReviewTypes"])

    def test_clock_halves_and_quarters_are_not_misclassified_as_fraction_bars(self):
        a,b=fixture()
        a["rows"][70]["publisherTitle"]="Tell Time with Halves and Quarters"
        a["rows"][70]["sourcePurposePreview"]="Read times from an analog clock."
        b["rows"][70]["visualKinds"]=["EquationSet","ClockFace"]
        d=audit.analyze(a,b)
        self.assertNotIn("FRACTION_PARTS",
                         d["rows"][70]["publisherCueVisualReviewTypes"])
        self.assertNotIn("CLOCK_DIAGRAM",
                         d["rows"][70]["publisherCueVisualReviewTypes"])
        b["rows"][70]["visualKinds"]=["EquationSet"]
        d=audit.analyze(a,b)
        self.assertIn("CLOCK_DIAGRAM",
                      d["rows"][70]["publisherCueVisualReviewTypes"])

    def test_polygon_shape_cue_requires_real_polygons_not_placeholder_tiles(self):
        a,b=fixture()
        a["rows"][15]["publisherTitle"]="Some Triangles, All Triangles"
        a["rows"][15]["sourcePurposePreview"]="Identify defining attributes of triangles."
        b["rows"][15]["visualKinds"]=["EquationSet","PlaceValueChart"]
        d=audit.analyze(a,b)
        self.assertIn("TRIANGLE_SHAPES",d["rows"][15]["publisherCueVisualReviewTypes"])
        b["rows"][15]["visualKinds"]=["EquationSet","PolygonDiagram"]
        d=audit.analyze(a,b)
        self.assertNotIn("TRIANGLE_SHAPES",d["rows"][15]["publisherCueVisualReviewTypes"])
        self.assertEqual(d["academicDecision"],"NOT_CERTIFIED")

    def test_line_plot_requires_x_mark_line_plot_not_only_numeric_axis(self):
        a,b=fixture()
        a["rows"][34]["publisherTitle"]="Interpret a Line Plot of Lengths"
        b["rows"][34]["visualKinds"]=["NumberLine","Scale","EquationSet"]
        d=audit.analyze(a,b)
        self.assertIn("LINE_PLOT",d["rows"][34]["publisherCueVisualReviewTypes"])
        b["rows"][34]["visualKinds"]=["LinePlot","EquationSet"]
        d=audit.analyze(a,b)
        self.assertNotIn("LINE_PLOT",d["rows"][34]["publisherCueVisualReviewTypes"])
        self.assertEqual(d["academicDecision"],"NOT_CERTIFIED")

    def test_number_line_is_not_satisfied_by_x_mark_line_plot(self):
        a,b=fixture()
        a["rows"][78]["sourcePurposePreview"]="Represent values on a number line."
        b["rows"][78]["visualKinds"]=["LinePlot","EquationSet"]
        d=audit.analyze(a,b)
        self.assertIn("NUMBER_LINE",d["rows"][78]["publisherCueVisualReviewTypes"])
        b["rows"][78]["visualKinds"]=["NumberLine","EquationSet"]
        d=audit.analyze(a,b)
        self.assertNotIn("NUMBER_LINE",d["rows"][78]["publisherCueVisualReviewTypes"])

    def test_source_verified_scaled_picture_graph_accepts_real_picture_graph(self):
        a,b=fixture()
        a["rows"][4]["publisherTitle"]="Create Scaled Picture Graphs"
        a["rows"][4]["sourcePurposePreview"]=(
            "Create a scaled picture graph of categorical data with a scale of 2.")
        b["rows"][4]["visualKinds"]=["EquationSet"]
        d=audit.analyze(a,b)
        self.assertIn("GRAPH_AND_SCALE",
                      d["rows"][4]["publisherCueVisualReviewTypes"])
        b["rows"][4]["visualKinds"]=["EquationSet","PictureGraph"]
        d=audit.analyze(a,b)
        self.assertNotIn("GRAPH_AND_SCALE",
                         d["rows"][4]["publisherCueVisualReviewTypes"])
        self.assertEqual(d["academicDecision"],"NOT_CERTIFIED")

    def test_fractional_eighth_measurements_use_line_plot_not_shading_bars(self):
        a,b=fixture()
        a["rows"][13]["publisherTitle"]="Fractional Measurements on Line Plots"
        a["rows"][13]["sourcePurposePreview"]=(
            "Plot one-eighth, quarter and half-inch fractional measurements "
            "and add/subtract fractions from the line plot.")
        b["rows"][13]["visualKinds"]=["EquationSet"]
        d=audit.analyze(a,b)
        self.assertIn("FRACTION_PARTS",
                      d["rows"][13]["publisherCueVisualReviewTypes"])
        self.assertIn("LINE_PLOT",
                      d["rows"][13]["publisherCueVisualReviewTypes"])
        b["rows"][13]["visualKinds"]=["EquationSet","LinePlot"]
        d=audit.analyze(a,b)
        self.assertNotIn("FRACTION_PARTS",
                         d["rows"][13]["publisherCueVisualReviewTypes"])
        self.assertNotIn("LINE_PLOT",
                         d["rows"][13]["publisherCueVisualReviewTypes"])
        self.assertEqual(d["academicDecision"],"NOT_CERTIFIED")
        # No generic exemption: a distinct source about fraction *parts*
        # still requires a fraction diagram even if some LinePlot is present.
        a["rows"][13]["publisherTitle"]="Partition Fractions into Equal Parts"
        d=audit.analyze(a,b)
        self.assertIn("FRACTION_PARTS",
                      d["rows"][13]["publisherCueVisualReviewTypes"])

    def test_one_source_content_sha_mismatch_blocks_exact_batch_integrity(self):
        a,b=fixture()
        b["rows"][301]["effectiveBodyFingerprint"]="b"*64
        d=audit.analyze(a,b)
        self.assertEqual(d["effectiveContentShaMismatches"],1)
        self.assertEqual(d["lessonsNeedingPedagogicalVisualReview"],1)

    def test_ci_preview_lists_only_source_cue_cases_without_accreditation(self):
        a,b=fixture()
        a["rows"][0]["sourcePurposePreview"]="Read a line plot."
        a["rows"][8]["sourcePurposePreview"]="Plot a value on a number line."
        b["rows"][0]["visualKinds"]=["LinePlot"]
        b["rows"][8]["visualKinds"]=["EquationSet"]
        d=audit.analyze(a,b)
        summary=audit.summarize_for_ci(d)
        self.assertEqual(summary["lessonsNeedingPedagogicalVisualReview"],1)
        self.assertEqual(len(summary["reviewCuePreview"]),1)
        self.assertEqual(summary["reviewCuePreview"][0]["lessonCode"],"B01:008")
        self.assertIn("NUMBER_LINE",summary["reviewCuePreview"][0]["missingRepresentationCues"])
        self.assertNotIn("rows",summary)
        self.assertFalse(summary["reviewCuePreviewTruncated"])
        self.assertEqual(summary["academicDecision"],"NOT_CERTIFIED")

    def test_different_source_500_set_is_rejected(self):
        a,b=fixture()
        b["rows"][0]["lessonCode"]="OUT-OF-SCOPE"
        with self.assertRaisesRegex(ValueError,"identity"):
            audit.analyze(a,b)

if __name__=="__main__":
    unittest.main()
