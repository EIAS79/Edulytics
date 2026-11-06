#!/usr/bin/env python3
"""Negative controls for the exact-run Geometry teacher-route verifier."""
import json
from pathlib import Path
from tempfile import TemporaryDirectory
import unittest
from g10_stage_d_runtime_route_verify import verify, PREFIX, ROOT

class EvidenceTests(unittest.TestCase):
    def setUp(self):
        self.temp=TemporaryDirectory()
        self.addCleanup(self.temp.cleanup)
        bp=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json").read_text(encoding="utf8"))
        self.inv={"candidateSha":"0"*40,"checked":124,"passed":124,"rubricEntryPagesRendered":124,
          "crossSchoolDenial":"PASS","technicalStatus":"PASS","failures":[],
          "lessons":[{"lessonCode":x["LessonCode"],"status":"AUTHENTICATED_RENDERED","actualBodyLength":100} for x in bp["Lessons"]]}
        self.rub={"course":"G10 Geometry","lessonCode":PREFIX+"U01:L01","studentSubmitted":True,
          "teacherScorePersisted":True,"studentReadBack":True,"wrongSchoolTeacherDenied":True,
          "teacherAwardedScore":3,"academicCorrectnessVerified":False,"all124Verified":False}
        self.a=Path(self.temp.name)/"a.json"
        self.b=Path(self.temp.name)/"b.json"

    def check(self):
        self.a.write_text(json.dumps(self.inv),encoding="utf8")
        self.b.write_text(json.dumps(self.rub),encoding="utf8")
        return verify(self.a,self.b,"0"*40)

    def test_valid_proof_keeps_one_sample_not_124_grades(self):
        result=self.check()
        self.assertEqual(result["lessonEntryPagesVerified"],124)
        self.assertEqual(result["persistedManualTeacherGradeSamples"],1)
        self.assertFalse(result["all124ScoresPersisted"])

    def test_rejects_missing_lesson(self):
        self.inv["lessons"].pop()
        with self.assertRaises(AssertionError):self.check()

    def test_rejects_fake_persisted_grade(self):
        self.rub["teacherScorePersisted"]=False
        with self.assertRaises(AssertionError):self.check()

    def test_rejects_wrong_school_access(self):
        self.rub["wrongSchoolTeacherDenied"]=False
        with self.assertRaises(AssertionError):self.check()

    def test_rejects_missing_rubric_entry_pages(self):
        self.inv["rubricEntryPagesRendered"]=123
        with self.assertRaises(AssertionError):self.check()

if __name__=="__main__":unittest.main()
