from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
def rep(text,old,new):
    assert text.count(old)==1,(old[:90],text.count(old))
    return text.replace(old,new)
file=root/"src/Edulytics.Services/Mathematics/UsCcssAlg1Unit4FunctionQuestions.cs"
s=file.read_text(encoding="utf-8")
anchor='''    public const string FunctionOneToOneFamily = "functions.relations.one_to_one.classify";'''
s=rep(s,anchor,anchor+'''
    public const string FunctionImplicitClassificationFamily = "functions.relations.implicit.classify";
    public const string FunctionTableLookupFamily = "functions.table.lookup.invert";
    public const string FunctionQuadraticRootsFamily = "functions.quadratic.output_roots.solve";''')
s=rep(s,'family is FunctionRelationFamily or FunctionOneToOneFamily or FunctionNotationFamily',
        'family is FunctionRelationFamily or FunctionOneToOneFamily or FunctionImplicitClassificationFamily or FunctionTableLookupFamily or FunctionQuadraticRootsFamily or FunctionNotationFamily')
build='''        if (family == FunctionImplicitClassificationFamily)
        {
            var variant = random.Next(0, 5);
            var relation = variant switch
            {
                0 => "2x+3y=9",
                1 => "x=y²",
                2 => "x²+y²=9",
                3 => "y=x³",
                _ => "y=√(4−x)"
            };
            return new(family,
                $"Determine whether {relation} defines y as a function of real x in its permitted domain. Enter yes or no; justify domain and branches to the teacher.",
                variant is 0 or 3 or 4
                    ? "The relation assigns at most one real y-value to each permitted x."
                    : "At some permitted x the relation contains two different real y-values.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int> { ["variant"] = variant });
        }

        if (family == FunctionTableLookupFamily)
        {
            var first = random.Next(0, 50);
            var inverse = random.Next(0, 2);
            var prompt = $"Table: x=[1,2,3,4,5]; f(x)=[{first+1},{first+7},{first+2},{first+7},{first+3}].";
            prompt += inverse == 1
                ? $" Find all x-values in the table with f(x)={first+7}. Enter both inputs separated by commas."
                : " Evaluate f(3) using the table. Enter the integer output.";
            return new(family, prompt,
                inverse == 1 ? "Both x=2 and x=4 have the requested output." : $"f(3)={first+2}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int> { ["first"] = first, ["inverse"] = inverse });
        }

        if (family == FunctionQuadraticRootsFamily)
        {
            var rootA = random.Next(-5, 6);
            var rootB = random.Next(-5, 6);
            if (rootB == rootA)
                rootB = rootA == 5 ? -5 : rootA + 1;
            return new(family,
                $"Given f(x)=(x-({rootA}))(x-({rootB})), solve f(x)=0 over the reals. Enter both distinct integer roots separated by commas; show the zero-product reasoning to the teacher.",
                $"The two zero-product roots are {Math.Min(rootA,rootB)} and {Math.Max(rootA,rootB)}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int> { ["rootA"] = rootA, ["rootB"] = rootB });
        }

'''
s=rep(s,'        if (family == FunctionOneToOneFamily)\n        {\n            // Exactly one output per input in every generated problem.',build+'        if (family == FunctionOneToOneFamily)\n        {\n            // Exactly one output per input in every generated problem.')
solve='''        if (family == FunctionImplicitClassificationFamily)
        {
            if (!ValidateImplicit(parameters, out var variant))
                throw new InvalidOperationException("Invalid implicit relation parameters.");
            return variant is 0 or 3 or 4 ? "yes" : "no";
        }
        if (family == FunctionTableLookupFamily)
        {
            if (!ValidateTableLookup(parameters, out var first, out var inverse))
                throw new InvalidOperationException("Invalid tabular lookup parameters.");
            return inverse == 1 ? "2,4" : (first+2).ToString();
        }
        if (family == FunctionQuadraticRootsFamily)
        {
            if (!ValidateQuadraticRoots(parameters, out var a, out var b))
                throw new InvalidOperationException("Invalid quadratic root parameters.");
            return $"{Math.Min(a,b)},{Math.Max(a,b)}";
        }

'''
s=rep(s,'        if (family == FunctionOneToOneFamily)\n        {\n            if (!ValidateOneToOne(parameters, out _, out _, out _, out var injective))',solve+'        if (family == FunctionOneToOneFamily)\n        {\n            if (!ValidateOneToOne(parameters, out _, out _, out _, out var injective))')
verify='''        if (family == FunctionImplicitClassificationFamily)
        {
            if (!ValidateImplicit(parameters, out _)) return false;
            var value = answer.Trim().ToLowerInvariant();
            return (value is "yes" or "no") && value == Solve(family, parameters);
        }
        if (family == FunctionTableLookupFamily)
        {
            if (!ValidateTableLookup(parameters, out var first, out var inverse))
                return false;
            if (inverse == 0)
                return int.TryParse(answer.Trim(), out var output) && output == first + 2;
            return EqualIntegerPair(answer, 2, 4);
        }
        if (family == FunctionQuadraticRootsFamily)
        {
            if (!ValidateQuadraticRoots(parameters, out var rootA, out var rootB))
                return false;
            return EqualIntegerPair(answer, rootA, rootB);
        }

'''
s=rep(s,'        if (family == FunctionOneToOneFamily)\n        {\n            if (!ValidateOneToOne(parameters, out _, out _, out _, out _))',verify+'        if (family == FunctionOneToOneFamily)\n        {\n            if (!ValidateOneToOne(parameters, out _, out _, out _, out _))')
validators='''    private static bool ValidateImplicit(IReadOnlyDictionary<string,int>? p,
        out int variant)
    {
        variant = 0;
        return p is not null && p.TryGetValue("variant", out variant) &&
            variant is >= 0 and <= 4;
    }

    private static bool ValidateTableLookup(IReadOnlyDictionary<string,int>? p,
        out int first, out int inverse)
    {
        first = inverse = 0;
        return p is not null &&
            p.TryGetValue("first", out first) && first is >= 0 and <= 49 &&
            p.TryGetValue("inverse", out inverse) && inverse is >= 0 and <= 1;
    }

    private static bool ValidateQuadraticRoots(IReadOnlyDictionary<string,int>? p,
        out int rootA, out int rootB)
    {
        rootA = rootB = 0;
        return p is not null &&
            p.TryGetValue("rootA", out rootA) && rootA is >= -5 and <= 5 &&
            p.TryGetValue("rootB", out rootB) && rootB is >= -5 and <= 5 &&
            rootA != rootB;
    }

    private static bool EqualIntegerPair(string answer, int expectedA, int expectedB)
    {
        var parts = answer.Split([',', ';'], StringSplitOptions.TrimEntries);
        return parts.Length == 2 &&
            int.TryParse(parts[0], out var a) &&
            int.TryParse(parts[1], out var b) &&
            a != b &&
            ((a == expectedA && b == expectedB) ||
             (a == expectedB && b == expectedA));
    }

'''
s=rep(s,'    private static bool ValidateOneToOne(IReadOnlyDictionary<string, int>? p,',validators+'    private static bool ValidateOneToOne(IReadOnlyDictionary<string, int>? p,')
file.write_text(s,encoding="utf-8",newline="\n")

regpath=root/"src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
reg=regpath.read_text(encoding="utf-8")
reg=rep(reg,'''"PRECALC_FUNCTION_RELATIONS_INJECTIVITY_NOTATION_DOMAIN_TOOLKIT_PARTIAL",
                [
                    "functions.relations.unique_output.classify",
                    "functions.relations.one_to_one.classify",
                    "functions.notation.evaluate_context",''',
    '''"PRECALC_FUNCTION_RELATIONS_INJECTIVITY_IMPLICIT_TABLE_ROOTS_PARTIAL",
                [
                    "functions.relations.unique_output.classify",
                    "functions.relations.one_to_one.classify",
                    "functions.relations.implicit.classify",
                    "functions.table.lookup.invert",
                    "functions.quadratic.output_roots.solve",
                    "functions.notation.evaluate_context",''')
reg=rep(reg,'"G12_SECTION_1_1_FOUR_REUSED_ONE_EXTENDED_FAMILY_NOT_FULL_SOURCE_PARITY"',
    '"G12_SECTION_1_1_FOUR_REUSED_FOUR_EXTENDED_FAMILIES_NOT_92_SOURCE_PARITY"')
regpath.write_text(reg,encoding="utf-8",newline="\n")

testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=testpath.read_text(encoding="utf-8")
anchor='''            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionOneToOneFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily,'''
new='''            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionOneToOneFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionImplicitClassificationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionTableLookupFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionQuadraticRootsFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily,'''
test=rep(test,anchor,new)
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
tests='''    [Fact]
    public void ExtendedPrecalculusSourceFamiliesRejectInvalidAnswerAndParameters()
    {
        var implicitParameters = new Dictionary<string, int> { ["variant"] = 1 };
        const string implicitFamily = UsCcssAlg1Unit4FunctionQuestions.FunctionImplicitClassificationFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(implicitFamily));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(implicitFamily, implicitParameters, "no"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(implicitFamily, implicitParameters, "yes"));
        implicitParameters["variant"] = 99;
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(implicitFamily, implicitParameters, "no"));

        var table = new Dictionary<string,int> { ["first"] = 10, ["inverse"] = 1 };
        const string tableFamily = UsCcssAlg1Unit4FunctionQuestions.FunctionTableLookupFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "2,4"));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "4; 2"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "2,2"));
        table["inverse"] = 0;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "12"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "17"));

        var roots = new Dictionary<string,int> { ["rootA"]=-3, ["rootB"]=2 };
        const string rootFamily = UsCcssAlg1Unit4FunctionQuestions.FunctionQuadraticRootsFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "-3,2"));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "2,-3"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "-3,-3"));
        roots["rootB"] = -3;
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "-3"));
    }

'''
test=rep(test,anchor,tests+anchor)
testpath.write_text(test,encoding="utf-8",newline="\n")

base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
path=base/"u01-l01-assessment.json"
a=json.loads(path.read_text(encoding="utf-8"))
for family,reason in [
 ("functions.relations.implicit.classify","Implicit equation/branch classification with real domain"),
 ("functions.table.lookup.invert","Evaluate and invert a finite numeric function table"),
 ("functions.quadratic.output_roots.solve","Solve a factored quadratic output equation for both distinct real integer roots")]:
    if not any(x.get("Family")==family for x in a["Practice"]["ReuseCandidates"]):
        a["Practice"]["ReuseCandidates"].append({"Family":family,
            "ReuseReason":reason,
            "Status":"EXTENDED_EXISTING_FUNCTION_ENGINE_WRITTEN_NOT_RUN"})
a["Practice"]["ExtendedFamiliesCount"]=4
a["Practice"]["ExactFamilyCountForGrade12"]=8
a["Practice"]["PracticeReady"]="PARTIAL_8_FAMILY_CONTRACT_NOT_FULL_SOURCE_PARITY"
path.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(cp_path.read_text(encoding="utf-8"))
cp["PartialPracticeCodeContract"]="FOUR_REUSED_PLUS_FOUR_EXTENDED_EXACT_FAMILIES_WRITTEN_NOT_RUN; GRAPH_TEACHER_LED"
cp["AllowedPracticeFamilyCount"]=8
cp["ReadyForStageD"]=False
cp_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progresspath=base/"full-grade-progress.json"
pg=json.loads(progresspath.read_text(encoding="utf-8"))
entry=pg["LessonEntries"][0]
entry["SelectedLessonPracticeStatus"]="FOUR_REUSED_FOUR_EXTENDED_EXACT_FAMILIES_WRITTEN_NOT_RUN"
entry["AllowedPracticeFamilyCount"]=8
entry["ReadyForStageD"]=False
progresspath.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("EXTENDED_THREE_MORE_EXISTING_FUNCTION_FAMILIES_TO_EIGHT_TOTAL")
