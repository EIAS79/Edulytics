import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
path=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=path.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit1ComparisonQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit1RoundingQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit1ComparisonQuestions.Supports(family))\n            return UaeGrade4Unit1ComparisonQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1RoundingQuestions.Supports(family))\n            return UaeGrade4Unit1RoundingQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit1ComparisonQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit1ComparisonQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1RoundingQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit1RoundingQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit1ComparisonQuestions.Supports(problem.Family))\n            return UaeGrade4Unit1ComparisonQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1RoundingQuestions.Supports(problem.Family))\n            return UaeGrade4Unit1RoundingQuestions.Solve(problem.Family, p);')
path.write_text(s,encoding='utf-8')
path=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=path.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
names=['ten','hundred','thousand','ten_thousand','hundred_thousand','million']
a=',\n             '.join('"uae.g4.u01.l05.round_nearest_'+x+'"' for x in names)
insert='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:01:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:01:05",
            "number.rounding.to_millions",
            "WHOLE_NUMBER_ROUND_TO_PLACE_UP_TO_MILLIONS",
            ['''+a+'''],
            "UAE_G4_STUDENT_04_01_PRINTED_PP37_42_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
s=s.replace(marker,insert+marker)
path.write_text(s,encoding='utf-8')
path=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=json.loads(path.read_text(encoding='utf-8-sig'))
for name in names:
    ident='uae.g4.u01.l05.round_nearest_'+name
    assert not any(x.get('id')==ident for x in j['families'])
    t=next(x for x in j['families'] if x['id']=='uae.g4.u01.l04.middle_of_three')
    e=dict(t)
    e.update({'id':ident,'skillId':'number.rounding.to_millions','answerType':'exact_integer','verificationPolicy':'g4-u01-l05-student-arabic-stage-d-not-run','representations':['number_line','place_value_chart','symbolic'],'status':'ShadowVerified','productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
path.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS: six nearest place-value rounding families wired')
