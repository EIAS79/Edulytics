#!/usr/bin/env python3
"""Adds small, explicitly scoped verified exact-skills to six G12 lessons.
No claim that open-ended source book exercises are machine graded."""
from pathlib import Path
import subprocess
root=Path(__file__).resolve().parents[2]
relative="src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
p=root/relative
s=p.read_text(encoding="utf8") if p.exists() else subprocess.check_output(["git","show","HEAD:"+relative],cwd=root).decode("utf8")
marker='''        return byLesson.Values
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();'''
assert s.count(marker)==1
prefix="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:"
rows=[
("U01:L04","functions.inverse.composition","COMPOSITION_INVERSE_EXACT_PARTIAL",["usccss.hs.inverse_composition"],"FIRSTPARTY_SECTION_1_4_COMPOSITION_EXACT_SUBSET_TEACHER_PROSE"),
("U01:L05","functions.absolute_value.translate","TRANSFORMATION_TRANSLATION_EXACT_PARTIAL",["functions.absolute_value.translate_evaluate"],"FIRSTPARTY_SECTION_1_5_FUNCTION_TRANSLATION_SUBSET_TEACHER_GRAPH"),
("U01:L06","functions.absolute_value.translate","ABSOLUTE_VALUE_TRANSLATION_EXACT_PARTIAL",["functions.absolute_value.translate_evaluate","functions.absolute_error.from_target"],"FIRSTPARTY_SECTION_1_6_ABSOLUTE_VALUE_SUBSET_TEACHER_GRAPH"),
("U02:L01","functions.average_rate_of_change","LINEAR_SLOPE_EXACT_PARTIAL",["geometry.coordinate.gradient_between_points"],"FIRSTPARTY_SECTION_2_1_SLOPE_SUBSET_TEACHER_LINEAR_MODELS"),
("U02:L02","functions.graph.key_features","LINEAR_INTERCEPT_EXACT_PARTIAL",["geometry.coordinate.y_intercept_from_rule"],"FIRSTPARTY_SECTION_2_2_Y_INTERCEPT_SUBSET_TEACHER_GRAPH"),
("U02:L04","functions.model.linear_data","LINEAR_REGRESSION_PREDICTION_EXACT_PARTIAL",["statistics.linear_model.predict","statistics.linear_model.residual"],"FIRSTPARTY_SECTION_2_4_LINEAR_MODEL_SUBSET_TEACHER_FIT")]
for suffix,*_ in rows:
    if 'byLesson["'+prefix+suffix+'"]' in s:
        raise ValueError(f"Existing explicit contract already present: {suffix}")
lines=['''        // Stage D lesson-specific exact subset. These contracts only certify the
        // tested numerical generator family. Graphs, explanations, source
        // exercise mapping and teacher rubric remain separately assessed.''']
for suffix,skill,mechanic,families,origin in rows:
    fullname=prefix+suffix
    fs=", ".join('"'+f+'"' for f in families)
    lines.extend([
        f'        byLesson["{fullname}"] = new LessonPracticeContract(',
        f'            "{fullname}",',
        f'            "{skill}",',
        f'            "{mechanic}",',
        f'            [{fs}],',
        f'            "{origin}",',
        '            "READY_VERIFIED",',
        '            Version);'])
s=s.replace(marker,"\n".join(lines)+"\n\n"+marker)
p.parent.mkdir(parents=True,exist_ok=True)
p.write_text(s,encoding="utf8")
print("G12_EXACT_SCOPED_6_CONTRACTS_ADDED_NOT_SOURCE_EXERCISE_FULL_DIGITAL_PARITY")
