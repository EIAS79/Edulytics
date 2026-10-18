#!/usr/bin/env python3
"""Fail-closed corrections for original 2021 L11/L12 PR code review.
U03 L12 includes the student PDF's 12.2 teen equations, not an invented
second digital Shake & Spill score. L11 must include 18+2=20."""
from pathlib import Path
import json
r=Path(__file__).resolve().parents[2]
def change(path,old,new,n=1):
 p=r/path;s=p.read_text(encoding="utf-8")
 if s.count(old)!=n:raise RuntimeError(f"{path} expected {n}, got {s.count(old)} for {old[:70]!r}")
 p.write_text(s.replace(old,new,n),encoding="utf-8",newline="\n")
q="src/Edulytics.Services/Mathematics/UsCcssG1TeenFiveQuestions.cs"
change(q,'public const string HiddenCounters="usccss.g1.u03.l12.teen.shake_cover";',
  'public const string TeenEquations="usccss.g1.u03.l12.teen.equations_missing";')
change(q,'TeenAddMissing,SubtractCollection,HiddenCounters,ResultStory,ChangeStory',
  'TeenAddMissing,SubtractCollection,TeenEquations,ResultStory,ChangeStory')
change(q,'HiddenCounters=>a is >=11 and <=20 && b is >=1 and <=19 && b<a,',
  'TeenEquations=>a is >=11 and <=19 && b is >=1 and <=9 && a-b>=10,')
change(q,
'''            case HiddenCounters:
                a=random.Next(11,21);b=random.Next(1,a);
                prompt=mode%2==0
                 ?$"Shake and Spill, Cover: there are {a} counters in total; {b} red counters are visible and the yellow counters are hidden under a cup. How many yellow counters are hidden? Write HIDDEN=number;EQUATION=number+number=number."
                 :$"Shake and Spill, Cover: out of {a} counters, you can see {b}. The rest are under the cup. How many are covered? Write HIDDEN=number;EQUATION=number+number=number. Tell a partner your method.";
                break;''',
'''            case TeenEquations:
                // IM 2021 L12 Activity 12.2: solve teen-number addition
                // AND subtraction equations for the unknown in every
                // position, keeping one full ten without regrouping.
                a=random.Next(11,20);b=random.Next(1,a-9);
                prompt=mode switch
                {
                    0=>$"Addition and Subtraction Equations with Teen Numbers: solve {a}-{b}=□. Write MISSING=number;CHECK=number+number=number.",
                    1=>$"Addition and Subtraction Equations with Teen Numbers: solve {a}-□={a-b}. Write MISSING=number;CHECK=number+number=number.",
                    2=>$"Addition and Subtraction Equations with Teen Numbers: solve {a-b}+{b}=□. Write MISSING=number;CHECK=number+number=number.",
                    _=>$"Addition and Subtraction Equations with Teen Numbers: solve □+{b}={a}. Write MISSING=number;CHECK=number+number=number."
                };
                break;''')
change(q,'HiddenCounters=>$"HIDDEN={a-b};EQUATION={b}+{a-b}={a}",',
  'TeenEquations=>$"MISSING={(mode==1?b:mode==2?a:a-b)};CHECK={a-b}+{b}={a}",')
change(q,
'''                 ?$"Kiran has {a} rocks and finds {b} more on a hike. How many rocks does he have? Keep the group of ten and add the ones. Write TOTAL=number;EQUATION=number+number=number."''',
'''                 ?$"Kiran has {a} rocks and finds {b} more on a hike. How many rocks does he have? Combine the ones; if ten ones are made, show a SECOND group of ten. Write TOTAL=number;EQUATION=number+number=number."''')
contracts="src/Edulytics.Core/Mathematics/Practice/UsCcssG1TeenFivePracticeContracts.cs"
change(contracts,"SUBTRACT_FROM_TEEN_AND_SHAKE_SPILL_COVER","SUBTRACT_FROM_TEEN_AND_MISSING_EQUATIONS")
change(contracts,"usccss.g1.u03.l12.teen.shake_cover","usccss.g1.u03.l12.teen.equations_missing")

c="src/Edulytics.Data/Seeding/CommonCoreG1TenAndTeenSourceFiveCorrections.cs"
pairs=[
("Add a single-digit number to a teen number without composing a new ten; keep the group of ten and combine the ones.",
 "Add one digit to a teen number with sums at most 20. Keep the first ten, combine the ones, and when the ones total ten recognize a SECOND full ten, as in 18+2=20."),
("Goal: add one-digit numbers to teen numbers with sums at most 20 and recognize the ten stays unchanged.",
 "Goal: add one-digit numbers to teen numbers within 20, recognizing that the ones may complete another group of ten in a sum of 20."),
("Example 4: \\u25a1=15+4 asks for nineteen, while 14+\\u25a1=17 asks for three.",
 "Example 4: \\u25a1=15+4 asks for nineteen, while 14+\\u25a1=17 asks for three.\\nExample 5: 18+2=20: eight ones plus two ones make a second complete ten. Two tens are twenty."),
("Step 3: Add the one-digit group to the ones, keeping the ten unchanged.",
 "Step 3: Add the one-digit group to the ones. For sums below twenty, one ten remains full; at twenty, ten additional ones complete a second full ten."),
("This activity does not teach carrying a new ten.",
 "A result of twenty has TWO full tens, not one ten and ten loose singles."),
("I can add to a teen number by keeping ten fixed, combining the ones, and checking matching equations.",
 "I can add a one-digit number to a teen number, combine the ones, and explain how a sum of twenty makes two tens."),
("Practice addition and subtraction equations with teen numbers, and introduce Shake and Spill, Cover with 11 to 20 counters: use the visible counters and the total to find those hidden under a cup.",
 "Next solve the original student Activity 12.2, Addition and Subtraction Equations with Teen Numbers: find missing values in equations where the unknown can occur in several positions. Shake and Spill, Cover is a separate teacher-facilitated center; its physical partner work is not automatically graded as this lesson's equation activity."),
("The warm-up counts by ten starting at two (2,12,22,32,42,52); Shake and Spill, Cover is partner gameplay and the digital version only models its mathematical missing quantity.",
 "The warm-up counts by ten starting at two (2,12,22,32,42,52). The physical Shake and Spill, Cover center is teacher-facilitated, not a replacement for the source student equation activity."),
("Example 4: Fifteen counters include five visible red and ten hidden yellow: 5+10=15.",
 "Example 4: 16-\\u25a1=12 means the unknown is four. Its matching addition fact is 12+4=16."),
("Step 5: For Shake and Spill, Cover, use visible+hidden=total, then explain the count to a partner.",
 "Step 5: For each addition or subtraction equation, identify the blank's position, solve, and check the result with a related fact. Explain to a partner."),
("Hidden-counters questions must not count a covered object as visibly shown.",
 "A missing addend or subtrahend is not automatically the result; locate the blank in the ORIGINAL equation."),
("I can subtract a one-digit part from a teen number and explain the hidden part of a two-color counter collection.",
 "I can subtract within a teen number, solve missing-number addition and subtraction equations, and justify my solution.")
]
for old,new in pairs:change(c,old,new)
reg="src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
change(reg,'"id": "usccss.g1.u03.l12.teen.shake_cover"','"id": "usccss.g1.u03.l12.teen.equations_missing"')
accept="tools/phase29/acceptance_two_school_https.py"
change(accept,'"03:12": "usccss.g1.u03.l12.teen.shake_cover"', '"03:12": "usccss.g1.u03.l12.teen.equations_missing"')
manifest="artifacts/curriculum-delivery/US-CCSS-MATH/G1/batches/US-CCSS-G1-010-LOCAL-SOURCE-CROSSWALK.json"
change(manifest,'"practiceFamily": "usccss.g1.u03.l12.teen.shake_cover"','"practiceFamily": "usccss.g1.u03.l12.teen.equations_missing"')
change(manifest,
"Use visible and hidden parts in Shake and Spill, Cover with 11–20 counters; partner gameplay requires a teacher",
"Find missing values in the original student Activity 12.2, Addition and Subtraction Equations with Teen Numbers; teacher-led Shake and Spill is a separate center")

script="tools/phase29/audit_g1_010_goal_crosswalk.py"
change(script,
"Use visible and hidden parts in Shake and Spill, Cover with 11–20 counters; partner gameplay requires a teacher",
"Find missing values in the original student Activity 12.2, Addition and Subtraction Equations with Teen Numbers; teacher-led Shake and Spill is a separate center")

t="tests/Edulytics.Tests/MathematicsIntelligence/UsCcssG1TeenFiveTests.cs"
change(t,
'''    [Fact]
    public void OriginalG1010VisualsDoNotShowAnswersOrInventHiddenCounters()''',
'''    [Theory]
    [InlineData(0,"MISSING=12;CHECK=12+3=15")]
    [InlineData(1,"MISSING=3;CHECK=12+3=15")]
    [InlineData(2,"MISSING=15;CHECK=12+3=15")]
    [InlineData(3,"MISSING=12;CHECK=12+3=15")]
    public void OriginalL12TeenEquationsAssessUnknownPositionNotCoveredCounters(
        int mode,string expected)
    {
        var p=new Dictionary<string,int>{{"a",15},{"b",3},{"c",0},{"mode",mode}};
        var family=UsCcssG1TeenFiveQuestions.TeenEquations;
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(family,p,expected));
        Assert.True(UsCcssG1TeenFiveQuestions.GradePersistedAnswer(family,
            UsCcssG1TeenFiveQuestions.Solve(family,p),expected));
        var wrong=mode==1?"MISSING=12;CHECK=12+3=15":
                              "MISSING=3;CHECK=12+3=15";
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(family,p,wrong));
    }
    [Fact]
    public void OriginalL12SecondFamilyIsNotUnrelatedShakeCover()
    {
        var lesson=LessonPracticeContractRegistry.All.Single(
            x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L12");
        Assert.Contains(UsCcssG1TeenFiveQuestions.TeenEquations,
            lesson.AllowedQuestionFamilies);
        Assert.DoesNotContain("usccss.g1.u03.l12.teen.shake_cover",
            lesson.AllowedQuestionFamilies);
    }
    [Fact]
    public void OriginalG1010VisualsDoNotShowAnswersOrInventHiddenCounters()''')
t2="tests/Edulytics.Tests/MathematicsIntelligence/CommonCoreG1TenAndTeenFiveTests.cs"
change(t2,
'''        Assert.Contains("Shake and Spill",rows[3].En.Explanation);''',
'''        Assert.Contains("Addition and Subtraction Equations with Teen Numbers",rows[3].En.Explanation);
        Assert.Contains("18+2=20",rows[2].En.Explanation);
        Assert.Contains("second complete ten",rows[2].En.WorkedExamples);
        Assert.Contains("Shake and Spill",rows[3].En.Explanation);''')
print("Source-review fixes applied: L11 20-boundary; L12 true student equation activity and negative controls")
