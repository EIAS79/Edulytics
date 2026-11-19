"""Stage D ONLY regression specifications for UAE Grade 9 semantic admission.

Do not execute in A-B-C. CI Stage D must run this as well as the source gate.
"""
import copy
import importlib.util
import pathlib
import unittest


FILE = pathlib.Path(__file__).with_name("uae_g9_stage_d_semantic_gate.py")
SPEC = importlib.util.spec_from_file_location("uae_g9_gate", FILE)
GATE = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(GATE)
SHA = "a" * 40


class Grade9SemanticEvidenceGateTests(unittest.TestCase):
    def setUp(self):
        self.source = {
            "scope": {
                "packCode": "UAE-MOE-MATH", "grade": 9,
                "pathway": "General", "academicLanguage": "ar",
            },
            "publicationStatus": "LOCKED_COMPLETE_GRADE9_GENERAL",
            "sourceLock": {
                "status": "LOCKED", "verifiedSchoolYearLessonCount": 1,
                "editionConflictResolved": True,
                "selectedSourceFingerprints": ["sha256:fetched-from-textbook"],
            },
            "lessonCount": 1,
            "units": [{"unit": 1, "lessons": [{
                "code": "1-1", "title": "المتغيرات والتعابير",
            }]}],
        }
        self.evidence = {
            "branchCommitSha": SHA, "testsRunInStage": "D",
            "lessons": [{
                "indexCode": "1-1", "author": "author-a",
                "semanticReviewer": "reviewer-b",
                "sourceSha256": "sha256:fetched-from-textbook",
                "unresolvedDefects": 0,
                "objectives": [{
                    "objectiveId": "O1", "sourcePage": 5,
                    "reviewVerdict": "PASS",
                    "realGeneratedSamples": [{
                        "generatorFamily": "uae.g9.u01.l01.term_coefficient",
                        "actualQuestion": "ما معامل س في التعبير ٣س + ٥؟",
                        "displayCulture": "ar", "rtl": True,
                        "correctAccepted": True, "wrongRejected": True,
                        "invalidRejected": True,
                        "independentVerifierPassed": True,
                        "skillAlignmentReviewed": True,
                        "seed": 44,
                    }],
                }],
            }],
        }

    def check(self):
        return GATE.check(self.source, self.evidence, SHA)

    def test_valid_structural_audit_metadata_can_pass(self):
        self.assertEqual([], self.check())

    def test_partial_year_index_must_fail_even_if_questions_pass(self):
        self.source["publicationStatus"] = "SOURCE_INDEX_ONLY_NOT_PUBLISHED"
        self.assertIn("FULL_SCHOOL_YEAR_SOURCE_LOCK_NOT_PROVEN", self.check())

    def test_no_independent_reviewer_blocks(self):
        self.evidence["lessons"][0]["semanticReviewer"] = "author-a"
        self.assertTrue(any("NO_INDEPENDENT_REVIEWER" in d for d in self.check()))

    def test_other_skill_correct_answer_is_not_academic_alignment(self):
        item = self.evidence["lessons"][0]["objectives"][0]["realGeneratedSamples"][0]
        item["skillAlignmentReviewed"] = False
        self.assertTrue(any("skillAlignmentReviewed" in d for d in self.check()))

    def test_english_question_is_rejected(self):
        item = self.evidence["lessons"][0]["objectives"][0]["realGeneratedSamples"][0]
        item["actualQuestion"] = "Find coefficient of x"
        self.assertTrue(any("ENGLISH_LEARNER_QUESTION" in d for d in self.check()))

    def test_missing_objective_or_real_questions_block(self):
        self.evidence["lessons"][0]["objectives"][0]["realGeneratedSamples"] = []
        self.assertTrue(any("NO_REAL_QUESTION_SAMPLES" in d for d in self.check()))

    def test_missing_one_lesson_blocks_full_grade_acceptance(self):
        self.source["lessonCount"] = 2
        self.source["sourceLock"]["verifiedSchoolYearLessonCount"] = 2
        self.source["units"][0]["lessons"].append({"code": "1-2", "title": "ترتيب العمليات"})
        self.assertTrue(any("MISSING_LESSON_EVIDENCE:1-2" in d for d in self.check()))

    def test_invalid_answer_not_rejected_blocks(self):
        item = self.evidence["lessons"][0]["objectives"][0]["realGeneratedSamples"][0]
        item["invalidRejected"] = False
        self.assertTrue(any("invalidRejected" in d for d in self.check()))

    def test_a_c_does_not_substitute_for_stage_d(self):
        self.evidence["testsRunInStage"] = "A-C"
        self.assertIn("TESTS_NOT_RUN_IN_STAGE_D", self.check())


if __name__ == "__main__":
    unittest.main()
