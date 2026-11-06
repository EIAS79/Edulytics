"""Author a scoped Grade 11 Algebra 2 practice extension. Deliberately do not execute tests."""
from pathlib import Path
import json
ROOT=Path(__file__).resolve().parents[2]
engine=ROOT/"src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"
s=engine.read_text(encoding="utf8")
families=["usccss.g11.alg2.u01.l01.affine_recurrence","usccss.g11.alg2.u01.l01.hanoi_moves","usccss.g11.alg2.u01.l01.fibonacci_term"]
anchor='            "sequences.core.find_position_arithmetic",'
assert s.count(anchor)>=1
s=s.replace(anchor,anchor+'\n'+''.join('            "'+f+'",\n' for f in families).rstrip('\n'),1)
verify_anchor='''            "sequences.core.arithmetic_nth_term" =>
                parameters["n"] > 0 &&'''
assert verify_anchor in s
verification='''            "usccss.g11.alg2.u01.l01.affine_recurrence" =>
                parameters["n"] >= 1 && parameters["n"] <= 8 &&
                parameters["multiplier"] is 2 or -2 &&
                parameters["offset"] is 1 or -1 or 3 &&
                value == G11AffineRecurrenceTerm(parameters["first"], parameters["multiplier"], parameters["offset"], parameters["n"]),

            "usccss.g11.alg2.u01.l01.hanoi_moves" =>
                parameters["n"] is >= 1 and <= 12 &&
                value == G11HanoiMinimalMoves(parameters["n"]),

            "usccss.g11.alg2.u01.l01.fibonacci_term" =>
                parameters["n"] is >= 1 and <= 20 &&
                value == G11FibonacciTerm(parameters["n"]),

'''
s=s.replace(verify_anchor,verification+verify_anchor,1)
build_anchor='''            "sequences.core.arithmetic_nth_term" =>
                BuildArithmeticSequenceNthTerm(random, scale),'''
assert build_anchor in s
s=s.replace(build_anchor,'''            "usccss.g11.alg2.u01.l01.affine_recurrence" =>
                BuildG11AffineSequence(random),
            "usccss.g11.alg2.u01.l01.hanoi_moves" =>
                BuildG11HanoiMoves(random),
            "usccss.g11.alg2.u01.l01.fibonacci_term" =>
                BuildG11FibonacciTerm(random),
'''+build_anchor,1)
answer_anchor='''            "sequences.core.arithmetic_nth_term" =>
                (p["first"] + (p["n"] - 1) * p["difference"]).ToString(CultureInfo.InvariantCulture),'''
assert answer_anchor in s
s=s.replace(answer_anchor,'''            "usccss.g11.alg2.u01.l01.affine_recurrence" =>
                G11AffineRecurrenceTerm(p["first"],p["multiplier"],p["offset"],p["n"])
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l01.hanoi_moves" =>
                G11HanoiMinimalMoves(p["n"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l01.fibonacci_term" =>
                G11FibonacciTerm(p["n"]).ToString(CultureInfo.InvariantCulture),

'''+answer_anchor,1)
methods_anchor="    private static ExactProblem BuildArithmeticSequenceNthTerm(Random random, int scale)"
assert methods_anchor in s
methods='''    // G11 Algebra 2 U01:L01 first-party recurrence question families.
    // Explicitly separate the numerical response from teacher-scored
    // explanations and legal-move demonstrations.
    private static int G11AffineRecurrenceTerm(int first, int multiplier, int offset, int n)
    {
        var term=first;
        for(var position=1;position<n;position++)
            term=checked(multiplier*term+offset);
        return term;
    }

    private static int G11HanoiMinimalMoves(int n) => checked((1 << n)-1);

    private static int G11FibonacciTerm(int n)
    {
        if(n<=2)return n-1;
        var a=0;var b=1;
        for(var i=3;i<=n;i++)
        {
            var next=checked(a+b);
            a=b;
            b=next;
        }
        return b;
    }

    private static ExactProblem BuildG11AffineSequence(Random random)
    {
        var first=random.Next(-3,5);
        var multiplier=random.Next(0,2)==0?2:-2;
        var offset=multiplier==2?-1:3;
        var n=random.Next(3,7);
        return Problem(
            "usccss.g11.alg2.u01.l01.affine_recurrence",
            $"A sequence starts with {first}. Each following term is {multiplier} times the previous term plus ({offset}). What is term {n}?",
            "Start with the first term and apply the stated operation exactly once per step. Do not reverse subtraction or change the initial value.",
            AssessmentItemType.Numeric,
            ("first",first),("multiplier",multiplier),("offset",offset),("n",n));
    }

    private static ExactProblem BuildG11HanoiMoves(Random random)
    {
        var n=random.Next(3,9);
        return Problem(
            "usccss.g11.alg2.u01.l01.hanoi_moves",
            $"Under the legal three-peg, one-disc transfer rules, what is the smallest number of moves for {n} discs?",
            "Move n-1 smaller discs aside, move the largest once, then transfer n-1 back. Explain why both subproblems are necessary.",
            AssessmentItemType.Numeric,("n",n));
    }

    private static ExactProblem BuildG11FibonacciTerm(Random random)
    {
        var n=random.Next(5,11);
        return Problem(
            "usccss.g11.alg2.u01.l01.fibonacci_term",
            $"A sequence begins 0,1 and each later term is the sum of its previous two terms. Counting 0 as term 1, find term {n}.",
            "Keep both previous terms, add them to form the next, then slide the two-term window forward.",
            AssessmentItemType.Numeric,("n",n));
    }

'''
s=s.replace(methods_anchor,methods+methods_anchor,1)
engine.write_text(s,encoding="utf8")
reg=ROOT/"src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
r=json.loads(reg.read_text(encoding="utf8"))
for f in families:
 assert not any(x.get("id")==f for x in r["families"])
 r["families"].append({"id":f,"version":1,"skillId":"sequences.core",
 "requiredCapabilities":["sequences.core.exact","sequences.verify.core"],
 "answerType":"exact_integer","verificationPolicy":"g11-u01l01-exact-move-count-or-recursive-value-independent-oracle-stage-d-written-not-run",
 "representations":["symbolic","sequence","contextual"],"status":"Shadow","productionRouting":False,"lessonPracticeRouting":True})
reg.write_text(json.dumps(r,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
maps=ROOT/"src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
m=json.loads(maps.read_text(encoding="utf8"))
code="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01"
assert not any(x.get("lessonCode")==code for x in m["mappings"])
m["mappings"].append({"lessonCode":code,"primarySkills":["sequences.core"],
 "sourceType":"FirstPartySourceReviewedCandidate","officialOutcomeMapped":True,
 "mappingConfidence":"HighForRecurrenceNotTeacherEvaluation",
 "evidence":["Kendall Hunt local teacher/student U01 L01; mathematical tasks involve affine and second-order recurrences, legal Tower of Hanoi moves, and open teacher-scored explanation.","Stage A-B-C code authored; runtime grading not yet executed, so this is not READY_VERIFIED."],
 "practiceReadiness":"WRITTEN_NOT_RUN","practiceMechanic":"RECURSIVE_SEQUENCES",
 "allowedQuestionFamilies":families,
 "officialOutcomeCodes":["CCSS:HSF-BF.A.1","CCSS:HSF-BF.A.2"]})
maps.write_text(json.dumps(m,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("G11_SCOPED_ENGINE_EXTENSION_AUTHORED_NOT_EXECUTED",families)
