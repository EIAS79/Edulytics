"""Regression tests for the offline clean Neon curriculum inventory."""
import json
import sys
import tempfile
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / "tools" / "phase29"))

from clean_neon_content_inventory import EXPECTED_PACKS, inventory  # noqa: E402


class CleanNeonInventoryTests(unittest.TestCase):
    def setUp(self):
        self.tmp = tempfile.TemporaryDirectory()
        self.root = Path(self.tmp.name)
        self.base = self.root / "src/Edulytics.Core/Curriculum"
        self.content = self.base / "LessonContent/Packs"
        self.core = self.base / "Packs"
        self.content.mkdir(parents=True)
        self.core.mkdir(parents=True)
        for index, pack in enumerate(sorted(EXPECTED_PACKS)):
            self._json(
                self.core / f"core-{index}.curriculum-pack.json",
                {"PackCode": pack, "VersionCode": "v1"},
            )
            self._json(
                self.content / f"lesson-{index}.lesson-content-pack.json",
                {
                    "packCode": pack,
                    "versionCode": "v1",
                    "status": "Published",
                    "lessons": [
                        {
                            "lessonCode": f"PED:{pack}:L1",
                            "translations": [
                                {"cultureCode": "en", "title": "Example"}
                            ],
                        }
                    ],
                },
            )

    def tearDown(self):
        self.tmp.cleanup()

    @staticmethod
    def _json(path, doc):
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(json.dumps(doc), encoding="utf-8")

    def test_counts_four_packs_and_no_user_data(self):
        report = inventory(self.root)
        self.assertEqual(4, report["totals"]["curriculumPacks"])
        self.assertEqual(4, report["totals"]["lessonContentPacks"])
        self.assertEqual(4, report["totals"]["uniqueLessonCodes"])
        self.assertEqual(0, report["totals"]["errorCount"])
        self.assertEqual(
            sorted(EXPECTED_PACKS), sorted(report["byPack"].keys())
        )
        self.assertEqual(64, len(report["sourceDigestSha256"]))

    def test_duplicate_code_fails_even_in_different_files(self):
        file = self.content / "duplicate.lesson-content-pack.json"
        first_pack = sorted(EXPECTED_PACKS)[0]
        self._json(
            file,
            {
                "PackCode": first_pack,
                "VersionCode": "v1",
                "Lessons": [
                    {
                        "LessonCode": f"PED:{first_pack}:L1",
                        "Translations": [{"CultureCode": "en"}],
                    }
                ],
            },
        )
        report = inventory(self.root)
        self.assertEqual(1, report["totals"]["errorCount"])
        self.assertEqual(5, report["totals"]["lessonEntries"])
        self.assertEqual(4, report["totals"]["uniqueLessonCodes"])
        self.assertIn("duplicate lesson identity", report["errors"][0])

    def test_missing_translation_only_warns(self):
        file = self.content / "lesson-0.lesson-content-pack.json"
        doc = json.loads(file.read_text(encoding="utf-8"))
        doc["lessons"][0]["translations"] = []
        self._json(file, doc)
        report = inventory(self.root)
        self.assertEqual(0, report["totals"]["errorCount"])
        self.assertEqual(1, report["totals"]["warningCount"])

    def test_missing_required_core_pack_fails(self):
        (self.core / "core-0.curriculum-pack.json").unlink()
        report = inventory(self.root)
        self.assertTrue(
            any("Missing core curriculum pack" in e for e in report["errors"])
        )

    def test_missing_lesson_code_fails(self):
        file = self.content / "lesson-0.lesson-content-pack.json"
        doc = json.loads(file.read_text(encoding="utf-8"))
        del doc["lessons"][0]["lessonCode"]
        self._json(file, doc)
        report = inventory(self.root)
        self.assertGreater(report["totals"]["errorCount"], 0)
        self.assertEqual(3, report["totals"]["uniqueLessonCodes"])


if __name__ == "__main__":
    unittest.main()
