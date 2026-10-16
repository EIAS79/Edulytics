#!/usr/bin/env python3
"""Structural evidence for 500 lesson-scoped Practice starts, not grading or approval."""
import json
import re
from urllib.parse import urlparse

FORM = re.compile(r"""<form\b[^>]*\baction\s*=\s*(["'])(.*?)\1""", re.I | re.S)

def has_lesson_start_form(page_html):
    """Check a real form action, not a matching link, script or page text."""
    for match in FORM.finditer(page_html):
        address = match.group(2).replace("&amp;", "&")
        if urlparse(address).path.lower().rstrip("/") == "/student/practice/lesson/start":
            return True
    return False

def coverage_report(rows):
    if len(rows) != 500 or len({r["lessonCode"] for r in rows}) != 500:
        raise ValueError("B001 coverage must consist of precisely 500 unique canonical lesson codes")
    present = sum(1 for row in rows if row["hasLessonStartForm"])
    return {
        "batchId": "US-CCSS-B001",
        "checked": 500,
        "lessonStartFormsPresent": present,
        "missingLessonStartForms": 500 - present,
        "practiceGeneratedCount": 0,
        "gradedAttemptsVerifiedCount": 0,
        "academicallyAcceptedCount": 0,
        "releaseDecision": "NO_GO",
        "assurance": "HTML form presence is not Practice generation, grading, source fidelity or independent academic review",
        "lessons": rows
    }

if __name__ == "__main__":
    import unittest
    class CoverageTests(unittest.TestCase):
        def test_only_matching_form_action_counts(self):
            self.assertTrue(has_lesson_start_form('<form action="/student/practice/lesson/start" method="post"></form>'))
            self.assertTrue(has_lesson_start_form("<form method='post' action='https://example.com/student/practice/lesson/start?x=1'>"))
            self.assertFalse(has_lesson_start_form('<a href="/student/practice/lesson/start">Open</a>'))
            self.assertFalse(has_lesson_start_form('<script>"/student/practice/lesson/start"</script>'))
            self.assertFalse(has_lesson_start_form('<form action="/student/practice/generate"></form>'))
        def test_coverage_never_confers_acceptance(self):
            rows=[{"ordinal":i+1,"lessonCode":f"PED:US-CCSS-MATH:G1:U01:L{i+1}","hasLessonStartForm":True} for i in range(500)]
            report=coverage_report(rows)
            self.assertEqual(report["lessonStartFormsPresent"],500)
            self.assertEqual(report["academicallyAcceptedCount"],0)
            self.assertEqual(report["releaseDecision"],"NO_GO")
            self.assertRaises(ValueError,coverage_report,rows[:499])
    unittest.main()
