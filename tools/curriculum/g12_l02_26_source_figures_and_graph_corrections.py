from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l02.rich-lesson-v2.json"
j=json.loads(rich.read_text(encoding="utf-8"))
lesson=j["Lessons"][0]
assert len(lesson["WorkedExamples"])==13
ex6=lesson["WorkedExamples"][5]
assert ex6["Title"]=="Domain and range from plotted V"
ex6.update({
"Title":"Finite graph domain and range with open and closed endpoints",
"Question":"An independent parabola segment follows f(x)=(x+1)²−4 only for −3<x≤1. Plot it with correct open/filled boundary points and find its domain and range.",
"Method":"Use the stated function restriction for domain and the vertex and attained endpoints for range.",
"Steps":[
"The x-window is −3<x≤1, so the domain is (−3,1].",
"The left endpoint would be (−3,0), but it is OPEN because x=−3 is excluded.",
"The vertex (−1,−4) is included and gives the smallest output.",
"The right endpoint (1,0) is included, so 0 is in the range despite the excluded left endpoint."
],
"Answer":"Domain (−3,1]; range [−4,0].",
"Check":"An excluded endpoint may still have an attained y-value elsewhere; range inclusion requires checking ALL graphed points."
})
ex7=lesson["WorkedExamples"][6]
assert ex7["Title"]=="Observed production graph"
ex7.update({
"Title":"Observed continuous interpolation has a bounded domain and range",
"Question":"A first-party environmental metric is recorded in tons/day for 2018–2023: 120,150,180,130,220,200. Model a CHART by connecting consecutive annual points with straight line segments. Read the domain and range of the displayed interpolation, and distinguish them from data outside the window.",
"Method":"The sampled annual values are given; a continuous interpolation is explicitly defined only between sampled years.",
"Steps":[
"Label x as year (2018–2023) and y as metric (tons/day); connect consecutive points only inside that closed window.",
"Under the explicitly specified piecewise-linear interpolation, every time t from 2018 to 2023 has one output.",
"The observed and interpolated minimum is 120 and maximum is 220 tons/day.",
"Do not infer that the actual process has those bounds before 2018 or after 2023."
],
"Answer":"Displayed interpolation: domain [2018,2023] years; range [120,220] tons/day.",
"Check":"Units and the measurement window are essential; unobserved years are not automatically in the domain."
})
for i,text in enumerate(lesson["ExplanationParagraphs"]):
    if "Independent Try It 6" in text and "observed" in text:
        lesson["ExplanationParagraphs"][i]+=" For a continuous chart, explicitly declare an interpolation rather than claiming unmeasured dates are known."
        break
visual={
"Kind":"RatioTable",
"Title":"First-party continuous measurement graph control points",
"Description":"Six independent annual observations, connected by a specified linear interpolation. This creates a function on [2018,2023] but makes no claim outside that data window.",
"PrimaryLabel":"Year",
"SecondaryLabel":"Measurement in tons/day",
"Rows":[
 {"Label":"Year","Cells":["2018","2019","2020","2021","2022","2023"]},
 {"Label":"Metric","Cells":["120","150","180","130","220","200"]}
],
"Items":[],"SecondaryItems":[]
}
assert not any(x["Title"]==visual["Title"] for x in lesson["Visuals"])
lesson["Visuals"].append(visual)
j["ContentVersion"]="g12-u01-l02-source-finite-graph-range-and-interpolated-context-20261106"
rich.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
crosswalk={
1:(38,"Original financial/annual values motivate bounded observations","Independent time-series measurement example 7 with bounded interpolation; original third-party financial chart not copied"),
2:(38,"Domain-to-range mapping as a function machine","Key concept Domain and range; finite input-output table visual"),
3:(39,"Six real interval-notation comparisons","Independent interval notation and endpoint inclusion RatioTable"),
4:(40,"Excluded denominator input x=2 on real number line","Worked example 3 forbids x=3 and uses open interval endpoints"),
5:(41,"Inequality, set-builder and interval notation comparison","Worked example 5 and source-skill interval comparison"),
6:(42,"Union of a closed interval and an open ray","Worked example 5 [−1,2]∪(4,∞) and explicit endpoint explanation"),
7:(43,"Open/filled interval parts translated among three notations","Independent Try It 5 mixed strict/inclusive endpoints"),
8:(43,"Graph horizontal extent is domain and vertical extent is range","Key concept Graph-derived domain and range"),
9:(43,"Source bounded graph with domain (−3,1] and range [−4,0]","Independent bounded parabola example 6 uses exact source-skill intervals but new mathematical curve"),
10:(44,"Highlights horizontal/vertical projections of a bounded curve","Independent example 6 boundary and vertex reasoning"),
11:(44,"Original Alaska production trend, approximate finite time/output window","Independently authored metric chart of observed series, interpolated only on declared years"),
12:(44,"Original historical population graph; domain and range from axis labels","Independent Try It 6 and metric chart with explicit finite observation window"),
13:(45,"Constant toolkit function domain all reals, range a single value","Independent toolkit domain-range nine-family table"),
14:(45,"Identity function domain and range all reals","Independent toolkit domain-range nine-family table"),
15:(45,"Absolute-value function range nonnegative","Independent toolkit domain-range nine-family table"),
16:(45,"Quadratic domain all reals, range nonnegative","Independent toolkit domain-range nine-family table"),
17:(45,"Cubic domain and range all reals","Independent toolkit domain-range nine-family table"),
18:(46,"Reciprocal excludes zero in input and output","Independent toolkit domain-range nine-family table"),
19:(46,"Reciprocal square excludes x=0 and has strictly positive range","Independent toolkit domain-range nine-family table"),
20:(46,"Principal square root domain and range nonnegative","Independent toolkit domain-range nine-family table"),
21:(46,"Real cube root domain and range all real","Independent toolkit domain-range nine-family table"),
22:(47,"Translated radical shifts domain boundary and nonnegative range","Independent worked example 10, radical f(x)=3√(x+1)"),
23:(48,"Group cost piecewise continuous/flat segments and contextual input","Independent worked example 11, positive whole-number group size"),
24:(49,"Consumption cost changes branch at threshold, open/closed endpoints","Independent worked example 12, P(2) and P(5)"),
25:(49,"Each of three piecewise components graphed on its own allowed subdomain","Independent worked example 13, quadratic/constant/line branches"),
26:(50,"Combined piecewise graph with correct selected values at branch boundaries","Independent worked example 13 plus branch membership RatioTable")
}
assert len(crosswalk)==26
rows=[]
for num,(page,skill,alternative) in crosswalk.items():
    rows.append({
     "SourceId":f"G12-U01-L02-FIG{num:03d}",
     "OriginalSourceFigureNumber":num,
     "OriginalPdfPage":page,
     "SourceConcept":skill,
     "EdulyticsIndependentPedagogicAlternative":alternative,
     "EdulyticsRichLessonPath":rich.relative_to(root).as_posix(),
     "OriginalPublisherOrThirdPartyArtworkCopied":False,
     "FirstPartyExactSourcePixelParityCertified":False,
     "OriginalFigureScopeReadableFromLocalPdf":True,
     "OriginalFigurePixelCoordinatesReproduced":False,
     "CognitiveSkillRepresentationAuthored":True,
     "GradeDStudentVisualRenderVerified":False
    })
document={
"SchemaVersion":1,
"Source":"Original authorized local OpenStax Precalculus 2017 Section 1.2 PDF pp.38–50",
"SourceSha256":json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson002.json").read_text(encoding="utf-8"))["SourceSha256"],
"OriginalSectionFigureCount":26,
"OriginalSectionFiguresMappedToFirstPartyConcepts":26,
"OriginalPublisherThirdPartyAssetsCopied":0,
"Figure1OriginalExternalChartNotCopied":True,
"ExactSourceFigureVisualPixelsParalleled":False,
"FullTeacherReviewedGraphStudentRoutes":"NOT_RUN",
"SourceFigurePedagogicAlternativesAuthored":True,
"StageD":"NOT_RUN",
"Rows":rows
}
(base/"u01-l02-figure26-first-party-skill-crosswalk.json").write_text(json.dumps(document,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l02-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["SourceOriginalFigureSkillsReviewed"]=26
c["SourceOriginalFiguresFirstPartyAlternativeMapped"]=26
c["FigureAlternativeCrosswalkPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l02-figure26-first-party-skill-crosswalk.json"
c["OriginalPublisherThirdPartyFigureImagesCopied"]=0
c["FirstPartyRichV2VisualTablesAuthored"]=8
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
row=p["LessonEntries"][1]
row["SourceOriginalFigureSkillsReviewed"]=26
row["SourceOriginalFiguresFirstPartyAlternativeMapped"]=26
row["FirstPartyRichV2VisualTablesAuthored"]=8
row["OriginalSourceFiguresCopied"]=0
row["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_L02_SOURCE_FIGURES_26_SOURCE_SKILLS_CROSSWALKED; EXAMPLES6_7_DOMAIN_RANGE_MISMATCHES_CORRECTED")
