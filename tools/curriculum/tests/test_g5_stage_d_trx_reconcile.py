#!/usr/bin/env python3
"""Synthetic TRX negative controls for Grade5 source-test attribution."""
import json
import tempfile
import unittest
from pathlib import Path
import importlib.util

script = Path(__file__).resolve().parents[1] / "g5-stage-d-trx-reconcile.py"
spec = importlib.util.spec_from_file_location("g5_reconcile", script)
mod = importlib.util.module_from_spec(spec)
spec.loader.exec_module(mod)

class ReconcileTests(unittest.TestCase):
    def test_legacy_and_new_classes_map_only_grade_five(self):
        self.assertEqual(mod.match_lesson("Edulytics.Tests.UsCcssG5Unit1Lesson7QuestionsTests"),
                         "PED:US-CCSS-MATH:G5:U01:L07")
        self.assertEqual(mod.match_lesson("Edulytics.Tests.UsCcssG5U03L03UnitFractionCandidateTests"),
                         "PED:US-CCSS-MATH:G5:U03:L03")
        self.assertIsNone(mod.match_lesson("Edulytics.Tests.UsCcssG4U03L03Tests"))
    def test_trx_is_parsed_by_test_id_and_class(self):
        xml = """<?xml version="1.0"?>
<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
<TestDefinitions>
  <UnitTest id="111" name="SomeTest"><TestMethod className="School.UsCcssG5U03L03SourceTests" name="Case" /></UnitTest>
  <UnitTest id="222" name="OtherTest"><TestMethod className="School.UnrelatedTests" name="Case" /></UnitTest>
</TestDefinitions>
<Results><UnitTestResult testId="111" testName="SomeTest" outcome="Passed" />
<UnitTestResult testId="222" testName="OtherTest" outcome="Failed" /></Results>
</TestRun>"""
        with tempfile.TemporaryDirectory() as tmp:
            path=Path(tmp)/"t.trx"
            path.write_text(xml, encoding="utf-8")
            results=mod.parse_test_results(path)
        self.assertEqual(len(results), 2)
        self.assertEqual(results[0]["lessonCode"], "PED:US-CCSS-MATH:G5:U03:L03")
        self.assertIsNone(results[1]["lessonCode"])
    def test_per_lesson_coverage_cannot_be_inferred_from_aggregate_success(self):
        lessons=[]
        unit_lengths=(12,17,20,21,26,21,13,18)
        for u,n in enumerate(unit_lengths,1):
            for k in range(1,n+1):
                lessons.append({"LessonCode":f"PED:US-CCSS-MATH:G5:U{u:02d}:L{k:02d}"})
        tests=[{"lessonCode":lessons[0]["LessonCode"],"outcome":"Passed","testName":"One",
                "className":"UsCcssG5Unit1Lesson1Tests","testId":"1"}]
        report=mod.reconcile({"lessons":lessons},tests,"f"*40)
        self.assertEqual(report["totalXunitResultCount"],1)
        self.assertEqual(report["lessonDirectTestCount"],1)
        self.assertEqual(report["lessonWithoutDirectTestCount"],147)
        self.assertEqual(report["dVerified"],0)

    def test_node_linked_lessons_are_not_falsely_failed_or_promoted_by_dotnet(self):
        lessons=[]
        for unit,n in enumerate((12,17,20,21,26,21,13,18),1):
            for lesson in range(1,n+1):
                code=f"PED:US-CCSS-MATH:G5:U{unit:02d}:L{lesson:02d}"
                linked=(f"tests/academic-us-ccss-g5-u{unit:02d}-l{lesson:02d}.test.cjs"
                        if unit <= 2 else f"tests/UsCcssG5U{unit:02d}L{lesson:02d}Tests.cs")
                lessons.append({"LessonCode":code,"testPath":linked})
        xunit=[{"lessonCode":row["LessonCode"],"outcome":"Passed","testName":"Real",
                "className":"G5Test","testId":str(i)}
               for i,row in enumerate(lessons) if i >= 29]
        report=mod.reconcile({"lessons":lessons},xunit,"a"*40)
        self.assertEqual(report["requiredXunitMissingCount"],0)
        self.assertEqual(report["nodeShardEvidenceRequiredCount"],29)
        self.assertEqual(report["lessonDirectTestCount"],119)
        self.assertEqual(report["lessons"][0]["technicalStatus"],"NODE_SHARD_EVIDENCE_REQUIRED")
        self.assertEqual(report["dVerified"],0)

    def test_missing_csharp_class_still_fails_closed(self):
        lessons=[]
        for unit,n in enumerate((12,17,20,21,26,21,13,18),1):
            for lesson in range(1,n+1):
                lessons.append({"LessonCode":f"PED:US-CCSS-MATH:G5:U{unit:02d}:L{lesson:02d}",
                                "testPath":f"tests/UsCcssG5U{unit:02d}L{lesson:02d}Tests.cs"})
        report=mod.reconcile({"lessons":lessons},[],"b"*40)
        self.assertEqual(report["requiredXunitMissingCount"],148)
        self.assertEqual(report["nodeShardEvidenceRequiredCount"],0)
        self.assertEqual(report["dVerified"],0)

if __name__=="__main__":
    unittest.main()
