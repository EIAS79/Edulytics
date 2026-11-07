#!/usr/bin/env python3
"""Second batch: eight bounded exact family reuses for existing G12 skills.
Separate release blockers remain for manual reasoning and student submissions."""
from pathlib import Path
import subprocess
root=Path(__file__).resolve().parents[2]
rel="src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
p=root/rel
s=p.read_text(encoding="utf8") if p.exists() else subprocess.check_output(["git","show","HEAD:"+rel],cwd=root).decode("utf8")
marker='''        return byLesson.Values
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();'''
assert s.count(marker)==1
prefix="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:"
rows=[
("U03:L08","functions.inverse.composition","RESTRICTED_QUADRATIC_INVERSE_EXACT_PARTIAL",["usccss.hs.restricted_inverse"],"G12_SECTION_3_8_RESTRICTED_INVERSE_SUBSET_TEACHER_RADICAL_GRAPHS"),
("U04:L01","functions.exponential.growth","EXPONENTIAL_GROWTH_EVALUATION_EXACT_PARTIAL",["functions.exponential.growth.evaluate"],"G12_SECTION_4_1_EXPONENTIAL_EVALUATION_SUBSET"),
("U04:L02","functions.exponential.graph.interpret","EXPONENTIAL_GROWTH_EVALUATION_EXACT_PARTIAL",["functions.exponential.growth.evaluate"],"G12_SECTION_4_2_EXPONENTIAL_GRAPH_NUMERICAL_SUBSET"),
("U04:L07","functions.exponential.model.choose","EXPONENTIAL_GROWTH_MODEL_EVALUATION_EXACT_PARTIAL",["functions.exponential.growth.evaluate"],"G12_SECTION_4_7_EXPONENTIAL_MODELS_EXACT_SUBSET"),
("U09:L01","algebra.linear.systems.two_by_two.solve","TWO_BY_TWO_LINEAR_SYSTEM_EXACT_PARTIAL",["algebra.linear.systems.two_by_two.integer_coefficients"],"G12_SECTION_9_1_TWO_VARIABLE_LINEAR_SYSTEM_SUBSET"),
("U09:L06","algebra.linear.systems.two_by_two.solve","GAUSSIAN_TWO_BY_TWO_ELIMINATION_EXACT_PARTIAL",["algebra.linear.systems.two_by_two.integer_coefficients"],"G12_SECTION_9_6_ELIMINATION_TWO_VARIABLE_SUBSET"),
("U09:L08","algebra.linear.systems.two_by_two.solve","CRAMER_TWO_BY_TWO_NUMERICAL_SUBSET",["algebra.linear.systems.two_by_two.integer_coefficients"],"G12_SECTION_9_8_CRAMER_TWO_VARIABLE_SYSTEM_SUBSET_TEACHER_DETERMINANT"),
("U11:L07","probability.theoretical.core","PROBABILITY_COMPLEMENT_EXACT_PARTIAL",["probability.theoretical.complement"],"G12_SECTION_11_7_PROBABILITY_COMPLEMENT_SUBSET")]
blocks=[]
for suffix,skill,mechanic,families,evidence in rows:
    key=prefix+suffix
    if f'byLesson["{key}"]' in s:raise ValueError(f"Duplicate {key}")
    blocks.extend([
        f'        byLesson["{key}"] = new LessonPracticeContract(',
        f'            "{key}", "{skill}",',
        f'            "{mechanic}",',
        '            ['+', '.join('"'+family+'"' for family in families)+'],',
        f'            "{evidence}", "READY_VERIFIED", Version);'])
intro='''        // Additional Stage D exact numeric subsets selected for matching
        // independently authored Grade12 lesson skills. These are not a
        // blanket adaptation of a textbook exercise bank and do not grade
        // proofs, source illustrations, submissions, or teacher rubrics.
'''
s=s.replace(marker,intro+'\n'.join(blocks)+'\n\n'+marker)
p.write_text(s,encoding='utf8')
print('ADDED_8_G12_SCOPE_BOUND_NUMERIC_PRACTICE_CONTRACTS_NO_WHOLE_LESSON_CLAIM')
