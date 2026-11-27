import unittest

from g10_arabic_semantic_signatures import (
    PACK, arabic_translation, load_g10_rules, match_g10_rule, read,
)


class G10ArabicSemanticSignaturesTests(unittest.TestCase):
    def test_all_exact_148_arabic_fallback_targets_are_classified(self):
        rules = load_g10_rules()
        lessons = read(PACK)["Lessons"]
        self.assertEqual(set(rules), {l["LessonCode"] for l in lessons})
        self.assertEqual(148, len(lessons))
        for lesson in lessons:
            with self.subTest(code=lesson["LessonCode"]):
                ar = arabic_translation(lesson)
                passed, missing = match_g10_rule(
                    rules[lesson["LessonCode"]], ar["Title"], ar["WorkedExamples"])
                self.assertTrue(passed, missing)

    def test_english_translation_is_never_used_for_arabic_scope(self):
        lesson = {"translations": [
            {"cultureCode": "en", "title": "wrong target"},
            {"cultureCode": "ar", "title": "عنوان عربي"}]}
        self.assertEqual("عنوان عربي", arabic_translation(lesson)["title"])
        lesson["translations"].append({"cultureCode": "ar", "title": "duplicate"})
        self.assertEqual({}, arabic_translation(lesson))

    def test_wrong_title_empty_and_generic_worked_examples_fail(self):
        for code, rule in load_g10_rules().items():
            with self.subTest(code=code):
                self.assertFalse(match_g10_rule(rule, rule[0], "")[0])
                self.assertFalse(match_g10_rule(
                    rule, rule[0], "اقرأ المسألة ثم احسب الإجابة وتحقق من الناتج.")[0])
                self.assertFalse(match_g10_rule(rule, "عنوان آخر", "")[0])

    def test_unrelated_real_examples_do_not_classify_as_circle_equation(self):
        rules = load_g10_rules()
        lessons = read(PACK)["Lessons"]
        polygon = next(l for l in lessons if l["LessonCode"].endswith("ADDING-AND-SUBTRACTING-POLYNOMIALS"))
        circle = next(l for l in lessons if l["LessonCode"].endswith("C105"))
        self.assertFalse(match_g10_rule(rules[circle["LessonCode"]],
            arabic_translation(circle)["Title"],
            arabic_translation(polygon)["WorkedExamples"])[0])


if __name__ == "__main__":
    unittest.main()
