"""Extend existing exact sequence engine with U01:L02 first-party geometric source families. No execution."""
from pathlib import Path
import json
p=Path(__file__).resolve().parents[2]
engine=p/"src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"
s=engine.read_text(encoding="utf8")
familyIds=["usccss.g11.alg2.u01.l02.paper_piece_count","usccss.g11.alg2.u01.l02.paper_piece_area","usccss.g11.alg2.u01.l02.rational_geometric_term","usccss.g11.alg2.u01.l02.growth_factor"]
anchor='            "usccss.g11.alg2.u01.l01.halving_fraction",'
assert anchor in s
s=s.replace(anchor,anchor+"\n"+"\n".join('            "'+x+'",' for x in familyIds),1)
anchor='''        if (family == "usccss.g11.alg2.u01.l01.halving_fraction")'''
assert anchor in s
s=s.replace(anchor,'''        if (family == "usccss.g11.alg2.u01.l02.paper_piece_area")
        {
            if (!TryParseFraction(answer, out var n, out var d)) return false;
            return parameters["cutCount"] is >= 0 and <= 8 &&
                (long)n * (1 << parameters["cutCount"]) == (long)80 * d;
        }

        if (family == "usccss.g11.alg2.u01.l02.rational_geometric_term")
        {
            if (!TryParseFraction(answer, out var n, out var d)) return false;
            var first=parameters["first"];var rn=parameters["ratioNumerator"];
            var rd=parameters["ratioDenominator"];var term=parameters["term"];
            return rd > 0 && term is >= 1 and <= 8 &&
                (long)n * G11IntPow(rd,term-1)==
                (long)d * first * G11IntPow(rn,term-1);
        }

        if (family == "usccss.g11.alg2.u01.l02.growth_factor")
        {
            if (!TryParseFraction(answer, out var n, out var d)) return false;
            return parameters["previous"] != 0 &&
                (long)n*parameters["previous"]==(long)d*parameters["next"];
        }

'''+anchor,1)
anchor='''            "sequences.core.arithmetic_nth_term" =>
                parameters["n"] > 0 &&'''
assert anchor in s
s=s.replace(anchor,'''            "usccss.g11.alg2.u01.l02.paper_piece_count" =>
                parameters["cutCount"] is >= 0 and <= 9 &&
                value == (1 << parameters["cutCount"]),

'''+anchor,1)
anchor='''            "usccss.g11.alg2.u01.l01.halving_fraction" =>
                BuildG11HalvingFraction(random),'''
assert anchor in s
s=s.replace(anchor,anchor+'''
            "usccss.g11.alg2.u01.l02.paper_piece_count" =>
                BuildG11PaperPieceCount(random),
            "usccss.g11.alg2.u01.l02.paper_piece_area" =>
                BuildG11PaperPieceArea(random),
            "usccss.g11.alg2.u01.l02.rational_geometric_term" =>
                BuildG11RationalGeometricTerm(random),
            "usccss.g11.alg2.u01.l02.growth_factor" =>
                BuildG11GeometricGrowthFactor(random),''',1)
anchor='''    // G11 Algebra 2 U01:L01 first-party recurrence question families.'''
assert anchor in s
methods='''    private static int G11IntPow(int value, int exponent)
    {
        var output=1;
        for(var i=0;i<exponent;i++) output=checked(output*value);
        return output;
    }

    private static ExactProblem BuildG11PaperPieceCount(Random random)
    {
        var cuts=random.Next(1,8);
        return Problem("usccss.g11.alg2.u01.l02.paper_piece_count",
            $"An 8-by-10-inch sheet is split into equal halves at each step. How many pieces exist after {cuts} cuts?",
            "The count doubles after every cut. Start at one piece with zero cuts.",
            AssessmentItemType.Numeric,("cutCount",cuts));
    }

    private static ExactProblem BuildG11PaperPieceArea(Random random)
    {
        var cuts=random.Next(1,7);
        return Problem("usccss.g11.alg2.u01.l02.paper_piece_area",
            $"A sheet has area 80 square inches. Every cut halves every existing equal piece. What is the exact area of each piece after {cuts} cuts? Give a reduced fraction if needed.",
            "Conserve the total sheet area: 80 divided by 2^n pieces.",
            AssessmentItemType.ShortAnswer,("cutCount",cuts));
    }

    private static ExactProblem BuildG11RationalGeometricTerm(Random random)
    {
        var variants=new (int N,int D)[]{(1,2),(1,10),(3,4),(2,1),(3,1),(4,1)};
        var ratio=variants[random.Next(variants.Length)];
        var first=random.Next(2,9);
        var term=random.Next(3,6);
        return Problem("usccss.g11.alg2.u01.l02.rational_geometric_term",
            $"A geometric sequence has first term {first} and constant multiplier {ratio.N}/{ratio.D}. What is term {term}? Give an exact fraction or integer.",
            "Multiply by the common ratio once for each move to a new term, preserving exact rational values.",
            AssessmentItemType.ShortAnswer,("first",first),
            ("ratioNumerator",ratio.N),("ratioDenominator",ratio.D),("term",term));
    }

    private static ExactProblem BuildG11GeometricGrowthFactor(Random random)
    {
        var choices=new (int Previous,int Next)[]{(256,192),(40,120),(1000,500),(2,8),(200,20),(24,12)};
        var row=choices[random.Next(choices.Length)];
        return Problem("usccss.g11.alg2.u01.l02.growth_factor",
            $"Two successive terms in a sequence are {row.Previous} and {row.Next}. Assuming a geometric rule, what is the exact growth factor?",
            "Divide the later term by the earlier nonzero term and simplify.",
            AssessmentItemType.ShortAnswer,("previous",row.Previous),("next",row.Next));
    }

'''
s=s.replace(anchor,methods+anchor,1)
anchor='''            "usccss.g11.alg2.u01.l01.halving_fraction" =>
                SimplifyFraction(1, 1 << p["n"]),'''
assert anchor in s
s=s.replace(anchor,anchor+'''
            "usccss.g11.alg2.u01.l02.paper_piece_count" =>
                (1 << p["cutCount"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l02.paper_piece_area" =>
                SimplifyFraction(80,1 << p["cutCount"]),
            "usccss.g11.alg2.u01.l02.rational_geometric_term" =>
                SimplifyFraction(checked(p["first"]*G11IntPow(p["ratioNumerator"],p["term"]-1)),
                    G11IntPow(p["ratioDenominator"],p["term"]-1)),
            "usccss.g11.alg2.u01.l02.growth_factor" =>
                SimplifyFraction(p["next"],p["previous"]),''',1)
engine.write_text(s,encoding="utf8")
reg=p/"src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json";j=json.loads(reg.read_text(encoding="utf8"))
for family in familyIds:
 assert not any(f.get("id")==family for f in j["families"])
 j["families"].append({"id":family,"version":1,"skillId":"sequences.core",
 "requiredCapabilities":["sequences.core.exact","sequences.verify.core"],
 "answerType":"exact_integer" if family.endswith("count") else "exact_fraction",
 "verificationPolicy":"g11-alg2-u01-l02-source-math-independent-stage-d-written-not-run",
 "representations":["sequence","table","fraction","symbolic"],
 "status":"Shadow","productionRouting":False,"lessonPracticeRouting":True})
reg.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
maps=p/"src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json";j=json.loads(maps.read_text(encoding="utf8"))
code="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L02"
assert not any(x.get("lessonCode")==code for x in j["mappings"])
j["mappings"].append({"lessonCode":code,"primarySkills":["sequences.core"],"sourceType":"FirstPartyLocalSourceReviewedCandidate",
"officialOutcomeMapped":True,"mappingConfidence":"SourceSpecificAuthoring",
"evidence":["Kendall Hunt local Algebra 2 teacher/student U01 L02, paper-cut models and geometric sequences; four exact question families coded but not run. Not a full interactive graph substitute."],
"practiceReadiness":"WRITTEN_NOT_RUN","practiceMechanic":"GEOMETRIC_SEQUENCES",
"allowedQuestionFamilies":["sequences.core.geometric_nth_term"]+familyIds,
"officialOutcomeCodes":["CCSS:HSF-BF.A.1","CCSS:HSF-BF.A.2"]})
maps.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("G11_U01_L02_SCOPED_FOUR_GEOMETRIC_EXACT_FAMILIES_AUTHORED_NOT_TESTED")
