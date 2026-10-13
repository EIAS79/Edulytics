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
    def test_wrong_join_fails_closed(self):
        a,b=fixtures();b["rows"].pop()
        with self.assertRaisesRegex(ValueError,"500"):
            module.analyze(a,b)
if __name__=="__main__":unittest.main()
