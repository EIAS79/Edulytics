from pathlib import Path
import json,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l08-source-inventory-and-gap-audit.json"
a=json.loads(ap.read_text(encoding="utf-8"))
assert a["OriginalSourceMathExercisesIndividuallyVerified"]==66
figs=[
(1,"Cone volume and inverse radius","For a cone with h=2r, use V=2πr³/3 to obtain r=∛(3V/(2π)). Require V≥0 and r≥0.","An inverse of a cubic measurement model returns a physical length from volume.",59),
(2,"Physical parabolic trough cross section","Use a symmetric parabolic trough model y=x²/2 and compare widths at nonnegative depths; width w=2√(2y).","Physical dimensions select a nonnegative root even when the mathematical parabola has both signs.",57),
(3,"Parabola coordinate restriction","For y=x²/2, restrict x≥0 to obtain x=√(2y), or x≤0 to obtain x=−√(2y).","A full parabola is not one-to-one; each monotone half yields an inverse.",32),
(4,"Cubic curve and inverse reflected about y=x","For f(x)=x³+1, f⁻¹(x)=∛(x−1); swap ordered pairs (0,1) and (1,0).","Inverse graphs reflect across y=x; every original input-output pair is swapped.",36),
(5,"Both quadratic branches represent different inverse functions","For f(x)=(x−4)², x≥4 gives inverse 4+√x while x≤4 gives inverse 4−√x.","Choosing an inverse branch is dictated by the original restricted domain.",35),
(6,"Quadratic and inverse branch reflection","For f(x)=(x−2)²−1, x≥2, inverse g(x)=2+√(x+1), x≥−1.","Graph original and inverse to display reflection and exchange of domain and range.",39),
(7,"Quadratic translated vertex and inverse","For f(x)=(x+1)²−3 on x≥−1, inverse −1+√(x+3) for x≥−3.","The vertex coordinate (−1,−3) reverses to (−3,−1) on inverse.",34),
(8,"Radical function and inverse reflection","For f(x)=1+√(x−1), x≥1, inverse g(x)=(x−1)²+1 for x≥1.","A square-root curve and its restricted quadratic inverse reflect about y=x.",32),
(9,"Radical of a rational function requires nonnegative radicand","For y=√((x−2)/(x+3)), use sign intervals about −3 (excluded) and 2 (included); domain (−∞,−3)∪[2,∞).","A sign chart, not cancellation or taking real square roots of negative values, determines allowable inputs.",42)
]
assert [f[0] for f in figs]==list(range(1,10))
artifact={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":8,"SourceSection":"3.8","OriginalFigureCount":9,
"IndependentFigureConcepts":[{"OriginalFigureNumber":n,"Title":title,"IndependentMathematicalModel":model,"StudentTeacherExplanation":meaning,"RelatedSourceExerciseNumber":exercise,"PublisherImageCopied":False,"ExactPublisherFigureReproductionClaimed":False}for n,title,model,meaning,exercise in figs],
"OriginalPublisherGraphicBitmapsCopied":False,"StageD":"NOT_RUN"}
rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-nine-figure-firstparty-skill-alternatives.json"
(base/"u03-l08-nine-figure-firstparty-skill-alternatives.json").write_text(json.dumps(artifact,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
cs=['namespace Edulytics.Web.Presentation;','','/// <summary>Independent educational alternatives to nine figure-skill concepts','/// from local Grade 12 Precalculus Section 3.8. No original images are copied.</summary>',
'public static class Grade12PrecalculusSection38FigureAlternatives','{','    public sealed record FigureSkill(int Number, string Title, string Model, string TeacherExplanation, int Exercise);',
'    public static readonly IReadOnlyList<FigureSkill> All =','    [']
for n,title,model,meaning,exercise in figs:
 esc=lambda x:x.replace('\\','\\\\').replace('"','\\"')
 cs.append(f'        new({n}, "{esc(title)}", "{esc(model)}", "{esc(meaning)}", {exercise}),')
cs.extend(['    ];','}',''])
cs_rel="src/Edulytics.Web/Presentation/Grade12PrecalculusSection38FigureAlternatives.cs"
(root/cs_rel).write_text("\n".join(cs),encoding="utf8")
a.update({"SourceFigureConceptsCrosswalked":9,"FirstPartyIndependentFigureSkillAlternativesAuthored":9,
"FirstPartyFigureSkillAlternativeAssetPath":rel,"FirstPartyFigureSkillStudentView":"PENDING_VISUAL_WIRING",
"BlockingReasons":["INVERSE_GRAPH_REFLECTION_MODELS_AND_SOURCE_FIGURE_STUDENT_VIEW_PENDING","STAGE_C_ASSESSMENT_REUSE_CONTRACT_PENDING"],
"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03L08_9_FIGURE_ALTERNATIVES_AUTHORED_NOT_CLOSED")
