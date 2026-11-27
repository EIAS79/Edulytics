"""Regression for the exact archival boundary; no semantic release waiver."""
import unittest
from unittest.mock import patch
import lesson_semantic_content_audit as audit


class G10SemanticArchiveTests(unittest.TestCase):
    def test_exact_scope_matches_embedded_resource_exclusion(self):
        retired, candidate, errors = audit.g10_archival_scope()
        self.assertEqual([], errors)
        self.assertEqual(52, len(retired))
        self.assertEqual(148, len(candidate))
        self.assertFalse(retired & candidate)
        self.assertTrue(all(c.startswith(audit.G10_GENERAL) for c in candidate))

    def test_unexpected_archive_content_is_not_excluded(self):
        original = audit.read_json
        def tampered(path):
            result = original(path)
            if path == audit.G10_RETIRED_CONTENT:
                result["Lessons"][0]["LessonCode"] = audit.G10_ADVANCED + "UNEXPECTED"
            return result
        with patch.object(audit, "read_json", side_effect=tampered):
            retired, candidate, errors = audit.g10_archival_scope()
        self.assertEqual(set(), retired)
        self.assertEqual(set(), candidate)
        self.assertTrue(errors)


if __name__ == "__main__":
    unittest.main()
