#!/usr/bin/env python3
import importlib.util
import pathlib
import unittest

p=pathlib.Path(__file__).resolve().parents[1]/"tools/us-ccss-b01-publisher-goal-alignment.py"
spec=importlib.util.spec_from_file_location("b01pub",p)
module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module)

SOURCE="""<main><h1>Lesson 2</h1><p>Explore Expressions and Sums</p>
<h3>Lesson Purpose</h3>
<p>The purpose of this lesson is to write addition expressions within 10 and find sums.</p>
<h3>Lesson Narrative</h3>
<p>Match expressions to dot images before writing addition expressions.</p>
<h3>Learning Goals</h3><p>Teacher Facing</p>
<ul><li>Write addition expressions based on images and find totals.</li></ul>
<h3>CCSS Standards</h3><p>Addressing</p><ul><li>1.OA.C.5</li><li>1.OA.C.6</li></ul>
<h3>Lesson Timeline</h3><p>10 min</p></main>"""

class PublisherAlignmentTests(unittest.TestCase):
    def test_purpose_goals_standards_are_source_extracted(self):
        x=module.parse_publisher(SOURCE)
        self.assertEqual(x["title"],"Explore Expressions and Sums")
        self.assertIn("write addition expressions within 10",x["purpose"])
        self.assertIn("based on images",x["goals"])
        self.assertEqual(set(x["standards"]),{"1.OA.C.5","1.OA.C.6"})
        self.assertNotIn("10 min",x["goals"])

    def test_missing_images_is_a_triage_gap_not_auto_pass(self):
        p=module.parse_publisher(SOURCE)
        r=module.align_row({
            "lessonCode":"PED:US-CCSS-MATH:G1:U01:L02","ordinal":2,
            "title":"Explore Expressions and Sums",
            "sourceUrl":"https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html",
            "officialOutcomeCodes":["CCSS:1.OA.C.5","CCSS:1.OA.C.6"]},
            {"learnerTeachingText":"Generic algebra text with unknown variables.",
             "sourcePreparationUrl":"https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html"},
            p)
        self.assertEqual(r["verdict"],"REVIEW_REQUIRED")
        self.assertIn("SOURCE_GOAL_INSTRUCTIONAL_ALIGNMENT_REVIEW",r["issues"])

    def test_compatibility_requires_allowlisted_source(self):
        self.assertTrue(module.allowed_source_url(
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-4/lesson-20/preparation.html"))
        self.assertFalse(module.allowed_source_url(
            "https://evil.test/k5/teachers/grade-1/unit-1/lesson-1/preparation.html"))
        self.assertFalse(module.allowed_source_url(
            "http://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html"))

    def test_full_batch_requires_exactly_500_same_identities(self):
        with self.assertRaisesRegex(ValueError,"scope"):
            module.run({"batchId":"B01","lessons":[]},{"lessonRows":[]},lambda _:None)

if __name__=="__main__":unittest.main()
