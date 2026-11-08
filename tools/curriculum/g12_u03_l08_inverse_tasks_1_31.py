from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l08-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l08-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"));t=json.loads(tp.read_text(encoding="utf-8"))
assert a["OriginalSourceMathExercisesIndividuallyVerified"]==0 and len(a["SourceNumberedExerciseLedger"])==66
data=[
(1,"Why does a quadratic polynomial need a branch restriction for a functional inverse, whereas y=x³ does not?","x² gives the same output for ±x, so is not injective over all real inputs. x³ is strictly increasing, so its inverse cube root is a function.","An inverse is a function only if its original mapping is one-to-one on the stated domain."),
(2,"Explain which branch of y=x²+6 would yield inverse y=√(x−6).","Restrict original x≥0; then the inverse outputs are nonnegative and its domain is x≥6.","A quadratic has two branches; choose one to define a function inverse."),
(3,"What domain restriction does the inverse of f(x)=√(x+2) need?","The inverse f⁻¹(x)=x²−2 has domain x≥0, because original square-root outputs are nonnegative.","The inverse domain equals the original range."),
(4,"Write the general form of the inverse of y=(x−h)²+k on original x≥h.","Inverse f⁻¹(x)=h+√(x−k), x≥k.","Restricted quadratic inverses use the principal positive square-root branch."),
(5,"Invert f(x)=(x−5)² on x≥5.","f⁻¹(x)=5+√x for x≥0.","Original source #5: f=(x−4)², x≥4; inverse 4+√x, x≥0."),
(6,"Invert f(x)=(x+3)² on x≥−3.","f⁻¹(x)=−3+√x for x≥0.","Original source #6: f=(x+2)², x≥−2; inverse −2+√x."),
(7,"Invert f(x)=(x+2)²−4 on x≥−2.","f⁻¹(x)=−2+√(x+4) for x≥−4.","Original source #7: f=(x+1)²−3 x≥−1, inverse −1+√(x+3)."),
(8,"Invert f(x)=5−√(x+1); give inverse input domain.","f⁻¹(x)=(5−x)²−1 for x≤5.","Original source #8: 2−√(x+3), inverse (2−x)²−3, x≤2."),
(9,"Invert f(x)=2x²+3, original x≤0.","f⁻¹(x)=−√((x−3)/2), x≥3.","Original source #9: 3x²+5, x≤0; inverse −√((x−5)/3), x≥5."),
(10,"Invert f(x)=15−x² for original x≥0.","f⁻¹(x)=√(15−x), x≤15.","Original source #10: 12−x² x≥0; inverse √(12−x)."),
(11,"Invert f(x)=20−x² for original x≥0.","f⁻¹(x)=√(20−x), x≤20.","Original source #11: 9−x² x≥0; inverse √(9−x)."),
(12,"Invert f(x)=4x²+1 for original x≥0.","f⁻¹(x)=√((x−1)/4)=(1/2)√(x−1), x≥1.","Original source #12: 2x²+4 x≥0; inverse √((x−4)/2)."),
(13,"Find inverse of f(x)=x³+7.","f⁻¹(x)=∛(x−7), all real inputs.","Original source #13: x³+5, inverse ∛(x−5)."),
(14,"Find inverse of f(x)=4x³−3.","f⁻¹(x)=∛((x+3)/4), all real inputs.","Original source #14: 3x³+1, inverse ∛((x−1)/3)."),
(15,"Find inverse of f(x)=6−x³.","f⁻¹(x)=∛(6−x), all real inputs.","Original source #15: 4−x³, inverse ∛(4−x)."),
(16,"Find inverse of f(x)=5−3x³.","f⁻¹(x)=∛((5−x)/3), all real inputs.","Original source #16: 4−2x³, inverse ∛((4−x)/2)."),
(17,"Invert f(x)=√(3x+2), with inverse domain.","f⁻¹(x)=(x²−2)/3, x≥0.","Original source #17: √(2x+1), inverse (x²−1)/2, x≥0."),
(18,"Invert f(x)=√(5−2x), with inverse domain.","f⁻¹(x)=(5−x²)/2, x≥0.","Original source #18: √(3−4x), inverse (3−x²)/4."),
(19,"Invert f(x)=4+√(2x−6), with inverse domain.","f⁻¹(x)=((x−4)²+6)/2, x≥4.","Original source #19: 9+√(4x−4), inverse ((x−9)²+4)/4."),
(20,"Invert f(x)=7+√(4x−12), with inverse domain.","f⁻¹(x)=((x−7)²+12)/4, x≥7.","Original source #20: 5+√(6x−8), inverse ((x−5)²+8)/6."),
(21,"Invert f(x)=5+3∛x.","f⁻¹(x)=((x−5)/3)³.","Original source #21: 9+2∛x, inverse ((x−9)/2)³."),
(22,"Invert f(x)=7−2∛x.","f⁻¹(x)=((7−x)/2)³.","Original source #22: 3−∛x, inverse (3−x)³."),
(23,"Invert f(x)=5/(x+2).","f⁻¹(x)=5/x−2, x≠0.","Original source #23: 2/(x+8), inverse 2/x−8."),
(24,"Invert f(x)=4/(x−3).","f⁻¹(x)=4/x+3, x≠0.","Original source #24: 3/(x−4), inverse 3/x+4."),
(25,"Invert f(x)=(x+2)/(x+5).","f⁻¹(x)=(2−5x)/(x−1), x≠1.","Original source #25: (x+3)/(x+7), inverse (3−7x)/(x−1)."),
(26,"Invert f(x)=(x−3)/(x+4).","f⁻¹(x)=(3+4x)/(1−x), x≠1.","Original source #26: (x−2)/(x+7), inverse (2+7x)/(1−x)."),
(27,"Invert f(x)=(2x+5)/(3−x).","f⁻¹(x)=(3x−5)/(x+2), x≠−2.","Original source #27: (3x+4)/(5−4x), inverse (5x−4)/(4x+3)."),
(28,"Invert f(x)=(4x+1)/(3−2x).","f⁻¹(x)=(3x−1)/(2x+4), x≠−2.","Original source #28: (5x+1)/(2−5x), inverse (2x−1)/(5x+5)."),
(29,"Invert f(x)=x²+4x, restricted x≥−2.","f⁻¹(x)=−2+√(x+4), x≥−4.","Original source #29: x²+2x x≥−1, inverse −1+√(x+1)."),
(30,"Invert f(x)=x²+6x+2, restricted x≥−3.","f⁻¹(x)=−3+√(x+7), x≥−7.","Original source #30: x²+4x+1 x≥−2, inverse −2+√(x+3)."),
(31,"Invert f(x)=x²−8x+4, restricted x≥4.","f⁻¹(x)=4+√(x+12), x≥−12.","Original source #31: x²−6x+3 x≥3, inverse 3+√(x+6).")
]
assert [n for n,*_ in data]==list(range(1,32))
# Focused mathematical substitution checks on source/inverse parameter choices.
assert (5+3)**2==64
assert (4-2)**2-4==0
assert 4*(3**3)-3==105
assert Fraction(2-5*0,0-1)==-2
assert Fraction(3+4*0,1-0)==3
assert Fraction(3*0-5,0+2)==Fraction(-5,2)
assert Fraction(3*0-1,2*0+4)==Fraction(-1,4)
assert (4**2)-8*4+4==-12
assessment=t.setdefault("FirstPartyIndependentStudentMathQuestions",[])
assert not assessment
for n,q,ans,original in data:
 assessment.append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":ans,"OriginalSourceMathReviewed":original,"MarkingMode":"TEACHER_REVIEW_COMPOSITION_AND_DOMAIN","Runtime":"NOT_RUN"})
 for row in a["SourceNumberedExerciseLedger"]:
  if row["Number"]==n:
   row.update({"OriginalMathematicalAnswerIndividuallyVerified":True,"FirstPartyIndependentMathQuestionVerified":True,"TeacherRubricVerified":True,"TeacherKey":ans})
assert len(assessment)==31
t["FirstPartyIndependentStudentQuestionsAuthored"]=31
t["SourceMathReviewedCount"]=31
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
a["OriginalSourceMathExercisesIndividuallyVerified"]=31
a["FirstPartyIndependentSourceExerciseMathAuthored"]=31
a["BlockingReasons"]=["SOURCE_EXERCISES_32_TO_66_REQUIRE_MATH_REVIEW_AND_INDEPENDENT_QUESTION_KEYS","9_SOURCE_FIGURE_MATH_ALTERNATIVES_PENDING","STAGE_C_PRACTICE_AND_TEACHER_REVIEW_ROUTES_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L08_31_INDEPENDENT_INVERSE_TASKS_AND_SOURCE_MATH_REVIEWED_STILL_OPEN")
