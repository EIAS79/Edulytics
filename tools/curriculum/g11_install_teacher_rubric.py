"""Install actual authorized Algebra2 teacher-marked route with school-scoped persistence.
Adapt the previously reviewed Grade10 course-scoped implementation. Distinct table
and adoption key; retain anti-forgery protection, RBAC and review-once semantics.
"""
from pathlib import Path
root=Path(__file__).resolve().parents[2]
web=root/"src/Edulytics.Web"
data=root/"src/Edulytics.Data"
def rewrite(source,target):
    s=source.read_text(encoding="utf8")
    names=[
       ("Grade10GeometryTeacherRubrics","Grade11Algebra2TeacherRubrics"),
       ("Grade10GeometryTeacherRubric","Grade11Algebra2TeacherRubric"),
       ("Grade10GeometryRubric","Grade11Algebra2Rubric"),
       ("G10GeoRubric","G11Alg2Rubric"),
       ("G10GeometryRubric","G11Algebra2Rubric"),
       ("G10Geometry","G11Algebra2"),
       ("Grade 10 Geometry","Grade 11 Algebra 2"),
       ("Geometry lesson","Algebra 2 lesson"),
       ("Geometry - Explain","Algebra 2 - Explain"),
       ("student/g10/geometry/rubric","student/g11/algebra2/rubric"),
       ("school/g10/geometry/rubric","school/g11/algebra2/rubric"),
       ('PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:', 'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:'),
       ('US-CCSS-MATH:L10:TRADITIONAL:COURSE-GEO', 'US-CCSS-MATH:L11:TRADITIONAL:COURSE-ALG2')
    ]
    for old,new in names:s=s.replace(old,new)
    target.parent.mkdir(parents=True,exist_ok=True)
    if target.exists():raise RuntimeError("Would overwrite existing file "+str(target))
    target.write_text(s,encoding="utf8")
    print('CREATED',str(target.relative_to(root)))
    return s
service=rewrite(web/"Services/Grade10GeometryRubricService.cs",
                web/"Services/Grade11Algebra2RubricService.cs")
controller=rewrite(web/"Controllers/Grade10GeometryRubricController.cs",
                   web/"Controllers/Grade11Algebra2RubricController.cs")
migration=rewrite(data/"Migrations/20261106023000_Grade10GeometryTeacherRubric.cs",
                  data/"Migrations/20261106050000_Grade11Algebra2TeacherRubric.cs")
migration_path=data/"Migrations/20261106050000_Grade11Algebra2TeacherRubric.cs"
migration=migration.replace('20261106023000_Grade11Algebra2TeacherRubric',
                            '20261106050000_Grade11Algebra2TeacherRubric')
migration_path.write_text(migration,encoding="utf8")
for name,source in [
 ("Grade11Algebra2RubricStudent/Index.cshtml","Grade10GeometryRubricStudent/Index.cshtml"),
 ("Grade11Algebra2RubricTeacher/Index.cshtml","Grade10GeometryRubricTeacher/Index.cshtml"),
 ("Grade11Algebra2RubricTeacher/Review.cshtml","Grade10GeometryRubricTeacher/Review.cshtml")
]:
    rewrite(web/"Views"/source,web/"Views"/name)
# Four criteria and statements must describe Algebra 2 rather than constructions.
p=web/"Services/Grade11Algebra2RubricService.cs"
s=p.read_text(encoding="utf8")
before='''        "The lesson-specific mathematical model, construction or event space is represented accurately",
        "Calculation, construction steps or mathematical notation are correct",
        "The learner justifies the result, proof or probability assumptions",
        "Labels, explanation and conclusions meet the lesson's communication goals"'''
after='''        "Mathematical functions, variables, equations, data or graphical models are represented accurately",
        "Calculations, symbolic transformations and stated constraints are mathematically correct",
        "A complete explanation justifies the reasoning, domain assumptions, and conclusions",
        "Written labels, units, interpretation and any teacher-inspected evidence meet the lesson goals"'''
assert before in s;s=s.replace(before,after)
s=s.replace("Course-owned G10 Geometry teacher-scored rubrics", "Course-owned G11 Algebra 2 teacher-scored rubrics")
s=s.replace("Six teacher-awarded binary points", "Four teacher-awarded binary points")
p.write_text(s,encoding="utf8")
prog=web/"Program.cs"
s=prog.read_text(encoding="utf8")
a="builder.Services.AddScoped<Edulytics.Web.Controllers.Grade10GeometryRubricService>();"
assert a in s and "Grade11Algebra2RubricService" not in s
s=s.replace(a,a+"\nbuilder.Services.AddScoped<Edulytics.Web.Controllers.Grade11Algebra2RubricService>();")
prog.write_text(s,encoding="utf8")
print('DI_REGISTERED_FOR_G11_ALGEBRA2_TEACHER_RUBRIC')
