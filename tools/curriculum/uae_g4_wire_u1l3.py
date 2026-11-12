import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit1Lesson01Questions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit1ComparisonQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit1Lesson01Questions.Supports(family))\n            return UaeGrade4Unit1Lesson01Questions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1ComparisonQuestions.Supports(family))\n            return UaeGrade4Unit1ComparisonQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit1Lesson01Questions.Supports(family))\n        {\n            var q = UaeGrade4Unit1Lesson01Questions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1ComparisonQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit1ComparisonQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit1Lesson01Questions.Supports(problem.Family))\n            return UaeGrade4Unit1Lesson01Questions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1ComparisonQuestions.Supports(problem.Family))\n            return UaeGrade4Unit1ComparisonQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
insert='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:01:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:01:03",
            "number.place_value.compare_to_millions",
            "SEVEN_DIGIT_NUMBER_LINE_COMPARE",
            ["uae.g4.u01.l03.compare_whole_numbers",
             "uae.g4.u01.l03.select_greater",
             "uae.g4.u01.l03.select_smaller"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP23_28_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
s=s.replace(marker,insert+marker)
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=json.loads(p.read_text(encoding='utf-8-sig'))
for name,template in [('compare_whole_numbers','uae.g3.u01.l02.compare_4digit_relation'),('select_greater','uae.g3.u01.l02.find_greater'),('select_smaller','uae.g3.u01.l02.find_smaller')]:
    ident='uae.g4.u01.l03.'+name
    assert not any(x.get('id')==ident for x in j['families'])
    e=dict(next(x for x in j['families'] if x['id']==template))
    e.update({'id':ident,'skillId':'number.place_value.compare_to_millions','verificationPolicy':'g4-u01-l03-student-arabic-stage-d-not-run','representations':['number_line','symbolic','place_value_chart'],'status':'ShadowVerified','productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS: comparison engine integrated with three selected families')
