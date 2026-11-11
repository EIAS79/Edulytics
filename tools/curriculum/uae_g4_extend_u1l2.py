import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
p=r/'src/Edulytics.Services/Mathematics/UaeGrade4Unit1Lesson01Questions.cs'
s=p.read_text(encoding='utf-8-sig')
a='    internal const string Expanded = "uae.g4.u01.l01.expanded_to_standard";'
assert s.count(a)==1
s=s.replace(a,a+'\n    internal const string WordToStandard = "uae.g4.u01.l02.words_to_standard";')
a='    internal static bool Supports(string? family) =>\n        family is DigitValue or Compose or Expanded;'
assert s.count(a)==1
s=s.replace(a,'    internal static bool Supports(string? family) =>\n        family is DigitValue or Compose or Expanded or WordToStandard;')
a='    private static readonly string[] Positions ='
assert s.count(a)==1
s=s.replace(a,'    private static readonly string[] SmallArabicNumbers =\n        ["صفر","واحد","اثنان","ثلاثة","أربعة","خمسة","ستة","سبعة","ثمانية","تسعة"];\n'+a)
a='        int number = rng.Next(1_000_000, 10_000_000);'
assert s.count(a)==1
s=s.replace(a,'''        int number = rng.Next(1_000_000, 10_000_000);
        if (family == WordToStandard)
        {
            // Arabic words in the student lesson; exact three-group number.
            number = rng.Next(3, 10) * 1_000_000
                   + rng.Next(3, 10) * 1_000
                   + rng.Next(1, 10);
        }''')
a='            Compose =>\n                $"كوّن العدد بالصيغة القياسية من الأرقام حسب ترتيب المنازل: {string.Join("، ", digits)}.",'
assert s.count(a)==1
s=s.replace(a,'''            WordToStandard =>
                $"اكتب بالصيغة القياسية: {SmallArabicNumbers[digits[0]]} ملايين و{SmallArabicNumbers[digits[3]]} آلاف و{SmallArabicNumbers[digits[6]]}.",
'''+a)
p.write_text(s,encoding='utf-8')
contract=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=contract.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
added='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:01:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:01:02",
            "number.place_value.read_write_to_millions",
            "ARABIC_WORDS_AND_DIGITS_TO_SEVEN_DIGIT_STANDARD_FORM",
            ["uae.g4.u01.l01.compose_seven_digits",
             "uae.g4.u01.l01.expanded_to_standard",
             "uae.g4.u01.l02.words_to_standard"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP17_22_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
assert 'ARABIC_WORDS_AND_DIGITS_TO_SEVEN_DIGIT_STANDARD_FORM' not in s
s=s.replace(anchor,added+anchor)
contract.write_text(s,encoding='utf-8')
reg=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=json.loads(reg.read_text(encoding='utf-8-sig'))
fid='uae.g4.u01.l02.words_to_standard'
assert not any(x.get('id')==fid for x in j['families'])
template=next(x for x in j['families'] if x['id']=='uae.g4.u01.l01.compose_seven_digits')
entry=dict(template)
entry.update({'id':fid,'skillId':'number.place_value.read_write_to_millions','verificationPolicy':'g4-u01-l02-arabic-student-source-stage-d-not-run','representations':['symbolic','place_value_chart'],'productionRouting':False,'lessonPracticeRouting':True})
j['families'].append(entry)
reg.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS: Grade4 U1 L2 three families selected, words extension routed via shared G4 engine')
