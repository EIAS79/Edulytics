#!/usr/bin/env python3
"""Third batch: seven independent, exact, lesson-scoped precalculus numerical subsets.
High-school source task parity and human assessment remain separate gates."""
from pathlib import Path
import subprocess
root=Path(__file__).resolve().parents[2]
rel="src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
p=root/rel;s=p.read_text(encoding="utf8") if p.exists() else subprocess.check_output(["git","show","HEAD:"+rel],cwd=root).decode("utf8")
marker='''        return byLesson.Values
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();'''
assert s.count(marker)==1
prefix="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:"
rows=[
("U03:L03","functions.quadratic.equations","POLYNOMIAL_QUADRATIC_SPECIAL_CASE_EXACT_PARTIAL",["functions.quadratic.output_roots.solve"],"G12_SECTION_3_3_QUADRATIC_POLYNOMIAL_SUBSET"),
("U03:L06","functions.quadratic.equations","POLYNOMIAL_ZERO_QUADRATIC_SPECIAL_CASE_EXACT_PARTIAL",["functions.quadratic.output_roots.solve"],"G12_SECTION_3_6_POLYNOMIAL_QUADRATIC_ROOT_SUBSET"),
("U05:L04","trigonometry.right_triangle.ratio","RIGHT_TRIANGLE_TRIGONOMETRY_EXACT_PARTIAL",["trigonometry.right_triangle.ratio_exact","trigonometry.right_triangle.find_side_exact"],"G12_SECTION_5_4_RIGHT_TRIANGLE_MEASUREMENT_SUBSET"),
("U08:L08","vectors.operations","VECTOR_EXACT_ARITHMETIC_PARTIAL",["vectors.add.exact_rational","vectors.subtract.exact_rational","vectors.magnitude.exact","vectors.dot.exact_rational"],"G12_SECTION_8_8_VECTOR_ARITHMETIC_SUBSET"),
("U11:L01","sequences.core","EXPLICIT_SEQUENCE_NTH_TERM_EXACT_PARTIAL",["sequences.core.arithmetic_nth_term","sequences.core.geometric_nth_term"],"G12_SECTION_11_1_SEQUENCE_NOTATION_SUBSET"),
("U11:L02","sequences.arithmetic","ARITHMETIC_NTH_TERM_EXACT_PARTIAL",["sequences.core.arithmetic_nth_term","sequences.core.find_position_arithmetic"],"G12_SECTION_11_2_ARITHMETIC_SEQUENCE_SUBSET"),
("U11:L03","sequences.geometric","GEOMETRIC_NTH_TERM_EXACT_PARTIAL",["sequences.core.geometric_nth_term"],"G12_SECTION_11_3_GEOMETRIC_SEQUENCE_SUBSET")]
blocks=[]
for suffix,skill,mechanic,families,source in rows:
 code=prefix+suffix
 if f'byLesson["{code}"]' in s:raise ValueError(f"Duplicate {code}")
 blocks.extend([
  f'        byLesson["{code}"] = new LessonPracticeContract(',
  f'            "{code}", "{skill}",',
  f'            "{mechanic}",',
  '            ['+', '.join('"'+family+'"' for family in families)+'],',
  f'            "{source}", "READY_VERIFIED", Version);'])
s=s.replace(marker,'''        // Stage D: arithmetic/sequence/vector/trigonometry exact subsets.
        // No source-atom, diagram, proof, human rubric or full digital parity
        // certification can be inferred from these numerical mechanics.
'''+('\n'.join(blocks))+'\n\n'+marker)
p.write_text(s,encoding="utf8")
print("G12_7_ADDITIONAL_APPROVED_NUMERICAL_SKILL_SUBSETS_WRITTEN_NOT_YET_CI_TESTED")
