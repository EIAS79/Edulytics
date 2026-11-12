import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
p=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=p.read_text(encoding='utf-8-sig')
a='UaeGrade4Unit1RoundingQuestions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a,a+'\n          UaeGrade4Unit1FourStepQuestions.Supports(family.Trim()) ||')
a='if (UaeGrade4Unit1RoundingQuestions.Supports(family))\n            return UaeGrade4Unit1RoundingQuestions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1FourStepQuestions.Supports(family))\n            return UaeGrade4Unit1FourStepQuestions.Verify(family, parameters, answer);')
a='if (UaeGrade4Unit1RoundingQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit1RoundingQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1FourStepQuestions.Supports(family))\n        {\n            var q = UaeGrade4Unit1FourStepQuestions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade4Unit1RoundingQuestions.Supports(problem.Family))\n            return UaeGrade4Unit1RoundingQuestions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1FourStepQuestions.Supports(problem.Family))\n            return UaeGrade4Unit1FourStepQuestions.Solve(problem.Family, p);')
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
ins='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:01:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:01:06",
            "problem_solving.four_steps_with_whole_numbers",
            "FOUR_STEPS_DIFFERENCE_TOTAL_REMAINING_TEACHER_RUBRIC",
            ["uae.g4.u01.l06.find_increase",
             "uae.g4.u01.l06.find_total",
             "uae.g4.u01.l06.find_remaining"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP43_48_TEACHER_REVIEW_OPEN_EXPLANATION_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''
s=s.replace(marker,ins+marker)
p.write_text(s,encoding='utf-8')
p=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=json.loads(p.read_text(encoding='utf-8-sig'))
for kind in ('find_increase','find_total','find_remaining'):
    fid='uae.g4.u01.l06.'+kind
    assert not any(x.get('id')==fid for x in j['families'])
    t=next(x for x in j['families'] if x['id']=='uae.g4.u01.l05.round_nearest_thousand')
    e=dict(t)
    e.update({'id':fid,'skillId':'problem_solving.four_steps_with_whole_numbers','answerType':'exact_integer','verificationPolicy':'g4-u01-l06-student-arabic-teacher-review-explanation','representations':['symbolic','contextual'],'status':'ShadowVerified','productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS: G4 U1 L6 three numeric step-planning families routed')
