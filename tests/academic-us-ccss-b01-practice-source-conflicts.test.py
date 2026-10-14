#!/usr/bin/env python3
import importlib.util, pathlib, unittest
p=pathlib.Path(__file__).resolve().parents[1]/"tools/us-ccss-b01-practice-source-conflicts.py"
spec=importlib.util.spec_from_file_location("b01semantic",p)
module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module)
def fixtures():
    names=[f"PED:US-CCSS-MATH:G1:U01:L{i:03d}" for i in range(500)]
    runtime={"lessonRows":[{"ordinal":i+1,"lessonCode":c,
        "practiceSkillId":"number.whole.add",
        "actualQuestionFamilies":["number.whole.add.direct"],
        "effectiveContentSha256":"a"*64} for i,c in enumerate(names)]}
    publisher={"rows":[{"ordinal":i+1,"lessonCode":c,
        "publisherTitle":"Addition with counters",
        "sourcePurposePreview":"Solve addition stories within ten.",
        "sourceUrl":"https://im.kendallhunt.com/"} for i,c in enumerate(names)]}
    return runtime,publisher
class WholeBatchSemanticTests(unittest.TestCase):
    def test_exact_500_join_with_one_collective_verdict(self):
        a,b=fixtures()
        v=module.analyze(a,b)
        self.assertEqual(v["checked"],500)
        self.assertEqual(v["conflictingLessons"],0)
        self.assertEqual(v["academicDecision"],"NOT_CERTIFIED")
    def test_correct_math_is_not_correct_practice_scope(self):
        a,b=fixtures()
        a["lessonRows"][123]["actualQuestionFamilies"]=[
            "supporting.geometry.circle_area_pi_coefficient"]
        b["rows"][123]["publisherTitle"]="Some Triangles, All Triangles"
        b["rows"][123]["sourcePurposePreview"]="Identify defining attributes of triangles."
        v=module.analyze(a,b)
        self.assertEqual(v["conflictingLessons"],1)
        self.assertEqual(v["rows"][123]["conflicts"][0]["rule"],
            "CIRCLE_PI_NOT_SOURCE_TARGET")
    def test_surds_with_source_bar_graph_is_detected(self):
        a,b=fixtures()
        a["lessonRows"][200]["actualQuestionFamilies"]=["supporting.surds.coefficient"]
        b["rows"][200]["publisherTitle"]="Choose a Scale"
        b["rows"][200]["sourcePurposePreview"]="Pick an appropriate scale for a bar graph."
        self.assertEqual(module.analyze(a,b)["conflictingLessons"],1)
    def test_exact_b01_lesson_family_is_locked_and_mismatches_fail_closed(self):
        a,b=fixtures()
        code="PED:US-CCSS-MATH:G3:U01:L06"
        a["lessonRows"][42]["lessonCode"]=code
        b["rows"][42]["lessonCode"]=code
        b["rows"][42]["publisherTitle"]="Choose a Scale"
        b["rows"][42]["sourcePurposePreview"]="Choose the scale for a bar graph."
        a["lessonRows"][42]["actualQuestionFamilies"]=["supporting.surds.coefficient"]
        r=module.analyze(a,b)
        self.assertEqual(r["conflictingLessons"],1)
        self.assertIn("SOURCE_TASK_EXACT_LESSON_FAMILY_DRIFT",
            [x["rule"] for x in r["rows"][42]["conflicts"]])

    def test_source_scoped_task_cannot_be_leaked_to_another_lesson(self):
        a,b=fixtures()
        a["lessonRows"][17]["actualQuestionFamilies"]=[
            "usccss.b01.triangle.attributes"]
        result=module.analyze(a,b)
        self.assertEqual(result["conflictingLessons"],1)
        self.assertEqual(result["rows"][17]["conflicts"][0]["rule"],
                         "SOURCE_TASK_LEAKED_OUTSIDE_EXACT_LESSON_SCOPE")


    def test_g2_u04_l10_cannot_fall_back_to_generic_place_value(self):
        a,b=fixtures()
        code="PED:US-CCSS-MATH:G2:U04:L10"
        a["lessonRows"][50]["lessonCode"]=code
        b["rows"][50]["lessonCode"]=code
        b["rows"][50]["publisherTitle"]="Place Value and the Number Line"
        b["rows"][50]["sourcePurposePreview"]=(
            "Compare addition and subtraction within 100 using number line methods.")
        a["lessonRows"][50]["actualQuestionFamilies"]=[
            "supporting.number.place_value"]
        findings=module.analyze(a,b)["rows"][50]["conflicts"]
        self.assertIn("SOURCE_TASK_EXACT_LESSON_FAMILY_DRIFT",
            [item["rule"] for item in findings])
        a["lessonRows"][50]["actualQuestionFamilies"]=[
            "number.whole.add_subtract.within_100.apply",
            "number.whole.add_subtract.within_100.build"]
        self.assertEqual(module.analyze(a,b)["conflictingLessons"],0)

    def test_wrong_join_fails_closed(self):
        a,b=fixtures();b["rows"].pop()
        with self.assertRaisesRegex(ValueError,"500"):
            module.analyze(a,b)
if __name__=="__main__":unittest.main()
