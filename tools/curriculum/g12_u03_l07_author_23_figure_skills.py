from pathlib import Path
import json,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson007.json").read_text(encoding="utf-8"))
auditpath=base/"u03-l07-source-inventory-and-gap-audit.json"
audit=json.loads(auditpath.read_text(encoding="utf-8"))
assert source["Section"]=="3.7" and audit["OriginalExerciseAlternativesReviewed"]==88
models=[
(1,"Reciprocal and reciprocal-square families","Compare y=1/(x−1) and y=1/(x−1)², noting the one-sided signs at x=1.","Different pole parity gives opposite versus same-sign divergence.",61),
(2,"Arrow notation at a vertical pole and infinity","For y=1/(x+2), list one-sided limits at −2 and both end limits.","Translate graph behavior into x→a± and x→±∞ notation.",62),
(3,"Vertical asymptote definition","For y=2/(x−3), mark x=3 as excluded and describe one-sided unbounded behavior.","A vertical asymptote is a limiting behavior, not an included graph point.",57),
(4,"End behavior arrow notation","For y=−3/(x+1), describe outputs for very large positive and negative inputs.","Distinguish end behavior from local behavior.",62),
(5,"Horizontal asymptote line","For y=4/(x−2), identify y=0 and draw the level independently.","A horizontal asymptote is an end-limit, not an uncrossable barrier.",61),
(6,"Read one-sided and end limits from an independently plotted curve","For y=(x−1)/((x+2)(x−3)), identify both vertical poles and the zero.","Graph-to-arrow reading requires branch signs and the correct excluded inputs.",57),
(7,"Translated reciprocal graph","For y=2/(x+3)+1, mark x=−3 and y=1.","Read shifts from parent y=1/x with nonzero asymptotes.",62),
(8,"Original-domain zero exclusion with a removable hole","For y=(x−1)(x+2)/((x−1)(x−4)), identify the hole and vertical pole.","A cancelled factor still forbids its input; x=1 is a hole at y=−1 and x=4 remains a vertical pole.",61),
(9,"Two uncancelled denominator roots","For y=5/((x−1)(x+4)), identify vertical poles x=1 and x=−4.","Real uncancelled denominator zeros produce vertical asymptotes.",57),
(10,"Distinguish removable discontinuity and vertical asymptote","For y=(x+2)(x−3)/((x+2)(x−1)), find hole (−2,5/3) and pole x=1.","A domain hole and asymptote have different limiting behavior.",58),
(11,"Hole versus remaining denominator factor","For y=(x−2)(x+5)/((x−2)(x+3)), identify hole (2,7/5) and pole x=−3.","Factor, cancel, and preserve original exclusions.",59),
(12,"Numerator lower degree and horizontal limit zero","For y=(x−2)/((x+1)(x−4)), find horizontal y=0.","A rational curve may cross its horizontal asymptote at a finite input.",59),
(13,"Linear slant end asymptote from polynomial division","For y=(x²+4x+7)/(x+1), divide to y=x+3+4/(x+1).","The quotient line y=x+3 controls end behavior.",61),
(14,"Equal-degree leading coefficient horizontal limit","For y=(5x²+2x+2)/(2x²+3), identify horizontal y=5/2 and the finite crossing x=11/4.","A horizontal level can be crossed where the rational function equals 5/2.",62),
(15,"Several poles and zero end-limit","For y=(x+2)/((x−1)(x+3)(x−4)), identify three vertical poles and horizontal y=0.","Compare three branch intervals and zero degree dominance.",60),
(16,"Intercepts alongside asymptotes","For y=(x−1)(x+3)/((x−2)(x+4)), derive zeros and y-intercept before sketching.","Crossings from numerator roots must not coincide with denominator exclusions.",57),
(17,"Odd-multiplicity pole branches","For y=1/(x−3), verify opposite one-sided infinities.","A simple denominator zero changes local sign.",61),
(18,"Even-multiplicity pole branches","For y=1/(x−3)², verify same positive infinity on both sides.","The square factor does not change sign across the excluded input.",63),
(19,"Repeated root and repeated pole affect touching/crossing","For y=(x−1)²/((x+2)²(x−3)), compare contact at x=1 with second-order pole x=−2.","A repeated numerator zero can touch; even denominator multiplicity gives same-side divergence.",63),
(20,"Use signs to fill intervals between vertical asymptotes","For y=(x−2)²/((x+2)(x−4)), evaluate sample signs in all intervals.","A sign chart fixes branch placement without tracing a publisher graph.",63),
(21,"Complete missing curve branches using critical inputs","For y=(x−1)²/((x+3)(x−4)), use x=−3,1,4 and end y=1.","Infer branch locations from factors, a tangency and end behavior.",64),
(22,"Infer rational factors from visually marked intercepts and poles","Interpret independently drawn exercise 57 as a factored rational function.","Source-equivalent skill is algebraic reconstruction using graph features.",57),
(23,"Infer factor multiplicities from one- versus two-sided shape","Interpret independently drawn exercise 64, noting the double zero at x=1.","Tangency identifies an even numerator multiplicity; poles identify denominator factors.",64)
]
assert [n for n,*_ in models]==list(range(1,24))
# Verify hand-authored removable-hole substitute values.
from fractions import Fraction
assert Fraction(1+2,1-4)==-1

assert Fraction(-2-3,-2-1)==Fraction(5,3) # Fig 10 model hole at -2
assert Fraction(2+5,2+3)==Fraction(7,5) # Fig 11 model hole at 2
asset=[]
for n,concept,model,explanation,linked in models:
 asset.append({"OriginalSourceFigureNumber":n,"SourceFigureConceptFromLocalPDF":concept,"IndependentFirstPartyMathematics":model,"IndependentStudentExplanation":explanation,"RelatedIndependentGraphExercise":linked,"OriginalPublisherImageCopied":False,"ExactOriginalFigureParityClaimed":False,"StageDVisualRender":"NOT_RUN"})
asset_path=base/"u03-l07-23-original-figure-skill-firstparty-alternatives.json"
asset_path.write_text(json.dumps({"SchemaVersion":1,"SourceSection":"3.7","SourceSHA256":source["SourceSha256"],"OriginalFigureCount":23,"FirstPartyIndependentFigureSkillAlternatives":asset,"PublisherImageCopied":False,"ExactPublisherGraphicIdentityCertified":False},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
def cs_escape(x):return x.replace('\\','\\\\').replace('"','\\"').replace('\n',' ')
cs="""namespace Edulytics.Web.Presentation;

/// <summary>Independently authored teaching explanations for the 23 mathematical
/// concepts shown by local source Section 3.7 figures. Not image reproductions.
/// Stage D runtime not executed.</summary>
public static class Grade12PrecalculusSection37FigureAlternatives
{
    public sealed record FigureSkill(int OriginalFigureNumber,
        string Topic, string IndependentModel, string StudentExplanation,
        int RelatedIndependentGraphExercise);
    public static readonly IReadOnlyList<FigureSkill> All =
    [
"""
for n,concept,model,explanation,linked in models:
 cs+=f'        new({n}, "{cs_escape(concept)}", "{cs_escape(model)}", "{cs_escape(explanation)}", {linked}),\n'
cs+="    ];\n}\n"
cspath=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection37FigureAlternatives.cs"
cspath.write_text(cs,encoding="utf-8")
view_path=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=view_path.read_text(encoding="utf-8")
needle='                @foreach (var task in Grade12PrecalculusSection37GraphModels.All)'
assert view.count(needle)==1 and 'Grade12PrecalculusSection37FigureAlternatives.All' not in view
insertion="""                <details class="student-card">
                    <summary>Twenty-three figure-based mathematical skills</summary>
                    <p>These are original Edulytics alternatives for the mathematical
                       ideas referenced by the source chapter. They do not reproduce
                       textbook illustrations. Use the eight independent graphs below
                       to practice reading features and explaining their algebra.</p>
                    <ol>
                        @foreach (var figure in Grade12PrecalculusSection37FigureAlternatives.All)
                        {
                            <li>
                                <strong>Source skill figure @figure.OriginalFigureNumber:
                                    @figure.Topic</strong>
                                <p>@figure.IndependentModel</p>
                                <p>@figure.StudentExplanation</p>
                                <p>Related independent graph exercise:
                                    @figure.RelatedIndependentGraphExercise</p>
                            </li>
                        }
                    </ol>
                </details>
"""
view_path.write_text(view.replace(needle,insertion+needle,1),encoding="utf-8")
audit["SourceFigureConceptsCrosswalked"]=23
audit["FirstPartyIndependentFigureSkillAlternativesAuthored"]=23
audit["FirstPartyFigureAlternativesEvidencePath"]=asset_path.relative_to(root).as_posix()
audit["FigureAlternativeStudentViewWiring"]="AUTHORED_NOT_RUNTIME_VERIFIED"
audit["BlockingReasons"]=["STAGE_C_ASSESSMENT_REUSE_EXTEND_CONTRACT_PENDING","STAGE_D_RUNTIME_AND_INTEGRATION_TESTS_NOT_RUN_BY_DESIGN"]
audit["ReadyForStageD"]=False
audit["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditpath.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_23_SOURCE_FIGURE_SKILL_ALTERNATIVES_AUTHORED_NOT_CLOSED")
