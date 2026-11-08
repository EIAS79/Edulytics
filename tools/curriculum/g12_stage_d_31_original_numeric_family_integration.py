#!/usr/bin/env python3
"""Register 31 original, bounded Grade12 exact numerical subskills with existing
global generation/independent verification runtime. Never approve Stage D itself."""
from pathlib import Path
import subprocess
root=Path(__file__).resolve().parents[2]
def load(rel):
 p=root/rel
 if not p.exists():
  p.parent.mkdir(parents=True,exist_ok=True)
  p.write_bytes(subprocess.check_output(["git","show","HEAD:"+rel],cwd=root))
 return p
def replace(s,a,b):
 assert s.count(a)==1,(a[:70],s.count(a))
 return s.replace(a,b,1)
p=load("src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs")
s=p.read_text(encoding="utf8")
s=replace(s,
'''        (SupportedFamilies.Contains(family.Trim()) ||
''',
'''        (SupportedFamilies.Contains(family.Trim()) ||
         Grade12PrecalculusAdvancedNumericQuestions.Supports(family.Trim()) ||
''')
s=replace(s,
'''    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string answer)
    {
''',
'''    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string answer)
    {
        if (Grade12PrecalculusAdvancedNumericQuestions.Supports(family))
            return Grade12PrecalculusAdvancedNumericQuestions.Verify(
                family, parameters, answer);
''')
a='''        if (UsCcssAlg1U01L01StatisticalSelectionQuestions.Supports(family))
'''
b='''        if (Grade12PrecalculusAdvancedNumericQuestions.Supports(family))
        {
            var advanced = Grade12PrecalculusAdvancedNumericQuestions.Build(
                family, random, scale);
            return new ExactProblem(
                advanced.Family, advanced.Prompt, advanced.Solution,
                advanced.ItemType, advanced.Parameters);
        }

'''+a
start=s.index("private static ExactProblem BuildOriginalProblem(")
pre,post=s[:start],s[start:]
post=replace(post,a,b)
s=pre+post
s=replace(s,
'''    private static string Solve(ExactProblem problem)
    {
        var p = problem.Parameters;
''',
'''    private static string Solve(ExactProblem problem)
    {
        var p = problem.Parameters;
        if (Grade12PrecalculusAdvancedNumericQuestions.Supports(problem.Family))
            return Grade12PrecalculusAdvancedNumericQuestions.Solve(
                problem.Family, p);
''')
p.write_text(s,encoding="utf8")
p=load("src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs")
s=p.read_text(encoding="utf8")
a='''        return byLesson.Values
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();'''
b='''        // These 31 distinct mathematical question families implement an
        // exact, bounded original skill from the corresponding Grade12
        // section. They are subject to independent runtime certification and
        // never auto-grade source figures, proofs or teacher-led assignments.
        const string grade12Root =
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";
        foreach (var (subskill, section) in
            Grade12PrecalculusAdvancedNumericQuestions.All)
        {
            var code = grade12Root + section;
            if (byLesson.ContainsKey(code))
                throw new InvalidOperationException(
                    $"Grade12 contract collision: {code}");
            var family = Grade12PrecalculusAdvancedNumericQuestions.Prefix + subskill;
            byLesson[code] = new LessonPracticeContract(
                code,
                "g12.precalculus.exact." + subskill,
                "FIRST_PARTY_G12_EXACT_NUMERICAL_SUBSKILL_NOT_FULL_SOURCE_PARITY",
                [family],
                "GRADE12_SECTION_MATCHED_MATHEMATICAL_SKILL_TEACHER_REVIEW_FOR_OPEN_TASKS",
                "READY_VERIFIED",
                Version);
        }

'''+a
s=replace(s,a,b)
p.write_text(s,encoding="utf8")
print("GRADE12_31_ORIGINAL_EXACT_FAMILIES_INTEGRATED_IN_PRIMARY_PRACTICE_GENERATOR_VERIFIER_AND_REGISTRY")
