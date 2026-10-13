#!/usr/bin/env python3
"""Fail-closed, offline fixtures for 500-lesson original activity audit."""
import unittest
from pathlib import Path
import importlib.util

SCRIPT=Path(__file__).resolve().parents[1]/"tools/us-ccss-b01-original-teacher-activities.py"
spec=importlib.util.spec_from_file_location("source_activities", SCRIPT)
mod=importlib.util.module_from_spec(spec)
spec.loader.exec_module(mod)

SAMPLE = """<html><nav><h2>Ignore unrelated menu</h2></nav>
<main>
<h2>Warm-up: Count to 41</h2>
<div>Warm-up counting strategy.</div>
<h2>Activity 1: Matching Dot Cards and Expressions</h2>
<p>Match the dot images and build addition expressions.</p>
<h2>Activity 2: Complete the Missing Dots</h2>
<p>Use dot cubes to find two addends.</p>
<h2>Lesson Synthesis</h2>
<h2>Cool-down: Explain Your Sum</h2>
</main></html>"""

class ActivityAuditTest(unittest.TestCase):
    def fixture(self):
        entries=[]; runtime=[]; rich=[]
        for i in range(1,501):
            code=f"PED:US-CCSS-MATH:G1:U01:L{i}"
            entries.append({"ordinal":i,"lessonCode":code,
                "sourceUrl":f"https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-{i}/preparation.html",
                "sourceSemanticSha256":"a"*64})
            runtime.append({"ordinal":i,"lessonCode":code,
                "effectiveContentSha256":"b"*64,
                "actualQuestionFamilies":["usccss.b01.g1.match_dot_cubes_expression"]})
            rich.append({"ordinal":i,"lessonCode":code,
                "effectiveBodyFingerprint":"b"*64,"visualKinds":["EquationSet"]})
        return {"batchId":"B01","lessons":entries},{"lessonRows":runtime},{"rows":rich}

    def test_parser_captures_real_main_activity_and_not_navigation(self):
        result=mod.parse_full_activity(SAMPLE)
        self.assertEqual(result["activityCount"],2)
        self.assertTrue(result["hasWarmUp"])
        self.assertTrue(result["hasCoolDown"])
        self.assertTrue(result["hasLessonSynthesis"])
        self.assertIn("DOT_IMAGES", [x["cue"] for x in result["instructionalRepresentationCues"]])
        self.assertEqual(len(result["activityHeadingDigests"]),2)
        self.assertTrue(result["hasReadableLessonBody"])

    def test_500_join_produces_candidates_not_pass(self):
        manifest,runtime,rich=self.fixture()
        out=mod.analyze(manifest,runtime,rich,lambda url:mod.parse_full_activity(SAMPLE))
        self.assertEqual(out["inspected"],500)
        self.assertEqual(out["teacherPagesFetched"],500)
        self.assertEqual(out["academicDecision"],"NOT_CERTIFIED")
        self.assertEqual(out["withReviewCues"],500)
        self.assertEqual(out["rows"][0]["candidateMissingRepresentations"],["DOT_IMAGES"])
        self.assertNotIn("PASS",out["rows"][0]["status"])

    def test_matching_visual_removes_review_signal_but_never_certifies(self):
        m,r,v=self.fixture()
        v["rows"][0]["visualKinds"]=["EquationSet","PlaceValueChart"]
        out=mod.analyze(m,r,v,lambda url:mod.parse_full_activity(SAMPLE))
        self.assertEqual(out["rows"][0]["candidateMissingRepresentations"],[])
        self.assertEqual(out["rows"][0]["status"],"SOURCE_ACTIVITY_SCREENED_NOT_CERTIFIED")

    def test_failed_source_is_blocked_and_keeps_identity(self):
        m,r,v=self.fixture()
        def fetch(url):
            if "lesson-5/" in url:raise ValueError("not accessible")
            return mod.parse_full_activity(SAMPLE)
        out=mod.analyze(m,r,v,fetch)
        self.assertEqual(out["teacherPagesFetched"],499)
        self.assertEqual(len(out["sourceFetchFailures"]),1)
        self.assertEqual(out["rows"][4]["status"],"BLOCKED_SOURCE_ACTIVITY_NOT_FETCHED")

    def test_rejects_wrong_source_duplicate_or_body_fingerprint(self):
        m,r,v=self.fixture()
        m["lessons"][0]["sourceUrl"]="https://example.org/fake.html"
        with self.assertRaisesRegex(ValueError,"Unexpected official"):
            mod.analyze(m,r,v,lambda url:mod.parse_full_activity(SAMPLE))
        m,r,v=self.fixture()
        v["rows"][0]["effectiveBodyFingerprint"]="c"*64
        with self.assertRaisesRegex(ValueError,"fingerprint mismatch"):
            mod.analyze(m,r,v,lambda url:mod.parse_full_activity(SAMPLE))
        m,r,v=self.fixture()
        r["lessonRows"][9]["lessonCode"]=r["lessonRows"][8]["lessonCode"]
        with self.assertRaisesRegex(ValueError,"Duplicate|unexpected"):
            mod.analyze(m,r,v,lambda url:mod.parse_full_activity(SAMPLE))

    def test_url_restrictions(self):
        real="https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-14/preparation.html"
        self.assertEqual(mod.source_lesson_url(real),
          "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-14/lesson.html")
        for candidate in ["http://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html",
                          "https://evil.example/k5/teachers/grade-1/unit-1/lesson-1/preparation.html",
                          real+"?redirect=elsewhere"]:
            with self.assertRaises(ValueError):mod.source_lesson_url(candidate)

if __name__=="__main__":
    unittest.main()
