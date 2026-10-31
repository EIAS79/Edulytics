#!/usr/bin/env python3
"""Algebra 1 V13 course-local repairs; exact source activity review remains unapproved.
Idempotence guard: never overwrite a newer course version or regenerate accepted work.
"""
from pathlib import Path
import json
import html

ROOT=Path(__file__).resolve().parents[2]
COURSE="PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:"
SIDE=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/RichV2"
DOCS=ROOT/"docs/curriculum"
IMAGE=ROOT/"src/Edulytics.Web/wwwroot/images/curriculum/alg1-v13"
DATE="2026-10-30"
REVIEWS={
"U01:L03":dict(
 assessment=["accurate original dot plots, histogram and box plot","same-scale center/shape comparisons","three evidence-backed gallery comments","honest student data provenance"],
 practice=["read shifted distribution from two dot plots","identify histogram bins and quartile definitions","create and critique three displays; human-scored"],
 cases=["A [2,2,3,3,4,4,5,5] has median 3.5; B shifted by 5 has median 8.5","same shape, range 3, centers shifted; using different scales is not evidence of variance"],
 text=[
 "V13 source-task comparison: IM U1L3 starts with a Notice-and-Wonder comparison of two dot plots with identical shape but shifted centers; its group gallery asks for dot plot, histogram, box plot, three interpretations and peer critique. Retain the already-authored three-display gallery and optional fictional dataset only when real collected data are unavailable; do not report fictional responses as a real class survey.",
 "V13 independent noticing task: first dot frequencies at values 1,2,3,4,5 are [1,2,3,2,1], and the second at 6,7,8,9,10 uses the same counts. Each has nine observations, range4 and median3 versus8. Draw on a shared numerical scale; state what is unchanged and what has shifted. The observed shape alone cannot establish the data-collection mechanism."
 ]),
"U01:L06":dict(
 assessment=["optional prerequisite/teacher assignment","live cell-reference formula validated with three trials","correct mixed-sign and fractional probe","partner rule discovery without fabricated technology evidence"],
 practice=["verify =A2+A4 with integer/fraction/negative inputs","infer which inputs influence B2 from controlled changes","live spreadsheet edit and partner exchange; human-scored"],
 cases=["A2=5,A4=-2 gives B2=3","A2=-1/2,A4=3/4 gives B2=1/4","changing A3 alone must not change =A2+A4"],
 text=[
 "V13 optionality and required medium: publisher U1L6 is part of an OPTIONAL three-lesson spreadsheet introduction. If students already know spreadsheets, an authorized teacher can skip the sequence; no school should record an unassigned optional task as failed. An internet-enabled spreadsheet is the original interaction, not a static arithmetic worksheet. No actual application operation or partner turn is evidenced by this content file.",
 "V13 original Make-24 and cell-probe sequence: combine 1,3,4,6 with 6/(1-3/4)=24. In a live spreadsheet define B2 = A2+A4 and hold A3 constant while testing A2=5,A4=-2 (3); A2=-0.5,A4=0.75 (0.25); then change only A3 and explain why B2 does not move. Move to a new B3 formula and exchange hidden-rule input trials with another learner; explicitly distinguish a spreadsheet-run observation from a written hypothetical."
 ]),
"U02:L03":dict(
 assessment=["four correctly matched table rules and units","precise constant-sum/offset/multiplier distinctions","inverse area and prize model with domain","peer justification and counterexample"],
 practice=["table to y=300x, y=x+300, y=x-300, x+y=300","distinguish constant sum from constant offset","inverse area xy=48, x>0; human explanation"],
 cases=["x=2 -> 600 for y=300x","x=90 ->210 for x+y=300","x=360 ->60 for y=x-300","x=4 -> 12 for xy=48"],
 text=[
 "V13 source reconciliation: source U2L3 asks students first to infer a novel table rule, then MATCH FOUR contextual tables to four distinct equations: multiplying by a fixed rate, adding an offset, subtracting an offset, and a fixed combined total. The original lesson described only three candidate patterns and did not give an explicit four-way equation-matching task.",
 "V13 independent four-card sort (not publisher numbers): A (x,y)=(0,0),(1,300),(2,600) -> y=300x; B (0,300),(20,320),(40,340) -> y=x+300; C (300,0),(360,60),(420,120) -> y=x-300; D (0,300),(90,210),(275,25) -> x+y=300. Shuffle rules separately; students justify unit-consistent contexts, all sample rows, and a value not shown. Additionally retain the earlier constant-area rectangle xy=48 for positive dimensions and explain why it is an inverse rather than linear rule."
 ]),
"U03:L03":dict(
 assessment=["row/column denominators and conditional relative frequencies","credible association-vs-no-association contrast","two labelled segmented bars","causation/sampling limitation and independent-variable design"],
 practice=["correct healthy/exposure column-conditional frequencies","select reference population to interpret a percentage","association does not prove causation; segmented bar human-scored"],
 cases=["16/20=.80, 6/20=.30","difference 50 percentage points","10/20=.50 in both groups is no association in sample","raw 16 vs6 must not be labeled an unconditional causal effect"],
 text=[
 "V13 source-task fidelity: U3L3 requires inspecting a relative-frequency table by the correct conditional population, contrasting one association with nearly matching conditional distributions, and testing associations in other categorical variable pairs. The prior authored club example had only one 15-point contrast and an equal-frequency control, but insufficient cross-context investigation or graph labels.",
 "V13 original fictional aquarium observational sample: 20 tanks have low dissolved-solute levels, with 16 healthy and4 unhealthy; another 20 have high levels, with6 healthy and14 unhealthy. Conditional healthy percentages are 80% and30%. Plot TWO segmented bars with each denominator=20, legend and percentage axes, and explain the 50-percentage-point difference. This suggests a possible association in fictional data only, not chemical causation; other tank variables may confound the relationship.",
 "V13 counterexample and student inquiry: compare an independent 40-response table in which both groups have 10 healthy of20; its conditional rates are equal at50%, indicating no detectable group association in that sample. Then propose two genuinely categorical variables in an ethical, anonymous survey, describe how data would be collected and whether a claim is supported; never invent participants or assert the exercise was administered."
 ]),
"U04:L03":dict(
 assessment=["correct interpretation of named input/output and units","comparison with same named function and different contexts","student-generated constraints-to-graph sketch","context-sensitive domains and peer justification"],
 practice=["interpret P(t) with calendar-year offset","compare W(5)>W(2), W(12)=W(10)","graph from discrete checkpoints; human-scored"],
 cases=["P(10)=53 thousand in baseline year+10","W5=55>W2=26","W12=100=W10","W15=96>W30=70","W0=18<W30=70"],
 text=[
 "V13 source comparison: U4L3 has distinct drone, calendar-year/device-ownership and water-temperature activities. Its teacher goal includes translating function statements into context with correct units and SKETCHING A POSSIBLE GRAPH from given inequalities. The earlier authored drone is sound but does not cover all three modalities. An independent model below avoids inventing real ownership measurements.",
 "V13 fictional installed-device count P(t), measured in THOUSANDS of installations, with t years since 2020: P(0)=10, P(5)=22, P(10)=53, P(15)=79. Thus P(10)=53 means 53 thousand installations in2030, not 53 people or the year10. Students convert three prose statements into notation and draw possible points; no interpolated values are uniquely determined by a finite table.",
 "V13 original temperature graph challenge: W(t) is Celsius water temperature t minutes after heating begins. Constraints and one possible fictional solution are W(0)=18, W(2)=26, W(5)=55, W(10)=100, W(12)=100, W(15)=96, W(30)=70, with heat switched off after minute12. Draw a correctly labelled piecewise connected example; explain W(5)>W(2), W(12)=W(10), W(15)>W(30) and W(0)<W(30). These constraints admit many different graphs; no automated grader should require this single polyline."
 ]),
"U05:L03":dict(
 assessment=["positive base zero-exponent and exponent-rule reasoning","two different growth tables with initial values","two actual labelled discrete graphs","growth factor and contextual meaning of a,b"],
 practice=["2^3 times 2^4 equals 2^7 (not 2^12)","nonzero a^0=1; zero base excluded","table and comparative graph for two growth models"],
 cases=["N(t)=50*2^t: 50,100,200,400,800","P(t)=20*3^t: 20,60,180,540,1620","N2=200>P2=180; N3=400<P3=540","2^7=128; 3^0=1"],
 text=[
 "V13 publisher comparison: U5L3 explicitly features an exponent-law Math Talk, an OPTIONAL zero-exponent reminder, two different microbial exponential contexts and their TWO labelled graphs. The original first-party single-model table was mathematically valid but did not ensure both functions/graphs or the optional status of the review.",
 "V13 independent, fictional equal-step growth models: N(t)=50*2^t and P(t)=20*3^t at integer t=0..4. N values [50,100,200,400,800]; P values [20,60,180,540,1620]. Draw both sets of points on clearly labelled axes and compare: N(2)=200>P(2)=180 but N(3)=400<P(3)=540. Initial values are 50 and20 (y intercepts), growth factors2 and3 (successive ratios), not fixed additions. These numbers do not describe actual microbes.",
 "V13 optional exponent-rule discussion: for any nonzero base a, a^0=1. Check 2^3*2^4=2^7=128 and 2^9/2^2=2^7 when dividing by a nonzero value. Correct the misconception 2^3*2^4=2^12; students explain why adding exponents corresponds to repeated factors. The optional review is not mandatory to mark the core lesson assigned."
 ]),
"U06:L03":dict(
 assessment=["correct square-area expressions and domains","first-party expanding square diagrams and step counts","geometric step rectangles and equivalent algebra","counterexample refuting linearity with differences"],
 practice=["n^2, n^2-4, 2n^2+4 for square figures","n^2+3 pattern and n(n+3) equivalence","draw non-overlapping cells; human-scored"],
 cases=["n=3: A9 B5 C22","n=18: n^2+3=327","n(n+3) n1..4 =4,10,18,28","first differences 6,8,10 (not constant)"],
 text=[
 "V13 publisher activity repair: U6L3 compares areas of three square arrangements, sketches expanding square patterns, and reasons about a growing step diagram. The prior rectangular n(n+4) model correctly illustrated a quadratic but was insufficient to represent the geometric contrasts and student-created growing figures. New figures here are independently selected rather than copied.",
 "V13 original area-card task: for a large n-by-n grid (n integer at least2), Card A is the intact square n^2, Card B cuts out a nonoverlapping 2-by-2 corner for n^2-4, and Card C shows TWO separate n-by-n squares plus a disjoint 2-by-2 tile for 2n^2+4. At n=3 the areas are9,5,22. Draw and label all cards, justify why the plus/minus terms are areas, and reject n^2-2 for the 2-by-2 removal.",
 "V13 independent growing patterns: expanding square pattern S(n)=n^2+3 has counts 4,7,12 at steps1,2,3 and327 at step18. Another growing rectangular-steps pattern T(n)=n(n+3)=n^2+3n uses an n-by-(n+3) grid, giving 4,10,18,28 at steps1..4; first differences6,8,10 are not constant, disproving the claim that a simultaneous one-unit increase in rows and columns automatically makes total area linear. Students produce cell diagrams and explain equivalence by distribution."
 ]),
"U07:L03":dict(
 assessment=["0,1,2 real-root equation classification","both sign branches after squaring","explicit substitution for each candidate","structure-first reasoning before procedures"],
 practice=["(x+2)^2=25 -> x=3 or-7","3x^2=75 -> +/-5","(x-2)^2=-1 has no real solution","factor equation x(x-6)=0 ->0 or6"],
 cases=["(3+2)^2=25 and(-7+2)^2=25","3*(5^2)=75 and3*((-5)^2)=75","(x-2)^2=0 has one distinct real root2","(x-2)^2=-1 has zero real roots"],
 text=[
 "V13 source task detail: U7L3 asks students to reason about equations with zero, one or two distinct real roots, including products and shifted squares, then find BOTH roots of several shifted equations. The prior sample discussion emphasized shifted squares but insufficiently practiced factoring as a separate reasoning path or the one-root/zero-root boundary cases.",
 "V13 original equation sort: (x+2)^2=25 has x=3 or x=-7; 3x^2=75 has x=5 or x=-5; (x-2)^2=0 has only x=2; (x-2)^2=-1 has no real roots; x(x-6)=0 has x=0 or6. Check each proposed root in the ORIGINAL equation, and identify which has no real root using nonnegativity of real squares.",
 "V13 teacher note: the principal radical sqrt(25) denotes positive5, but an equation y^2=25 has BOTH y=5 and y=-5. Teach by reasoning about squares, not a mechanical claim that square-rooting is one-to-one. Optional extension may use degree-three factorizations; never present three or ten distinct real roots as possible for a nonzero quadratic."
 ])
}
ASSETS={"U03:L03":"u03-l03-conditional-association.svg","U04:L03":"u04-l03-temperature-constraints.svg","U05:L03":"u05-l03-two-growth-models.svg"}
def save_json(path,obj):
    path.write_text(json.dumps(obj,indent=2,ensure_ascii=False)+"\n",encoding="utf-8")

def svg_plot(name,title,description,xlabel,ylabel,xmax,ymax,series,ticks):
    w,h=740,440; left,top,ww,hh=92,58,555,306
    out=[f'<svg xmlns="http://www.w3.org/2000/svg" role="img" aria-labelledby="graphTitle graphDesc" viewBox="0 0 {w} {h}">',
         f'<title id="graphTitle">{html.escape(title)}</title><desc id="graphDesc">{html.escape(description)}</desc>',
         '<rect width="740" height="440" fill="#fff"/>',
         f'<path d="M {left} {top} V {top+hh} H {left+ww}" stroke="#242b35" stroke-width="2" fill="none"/>']
    for val in ticks:
        yy=top+hh-hh*val/ymax
        out.append(f'<path d="M {left} {yy:.1f} H {left+ww}" stroke="#d2d8df" stroke-width="1"/>')
        out.append(f'<text x="{left-10}" y="{yy+5:.1f}" text-anchor="end" fill="#242b35" font-size="13">{val:g}</text>')
    for x in range(0, xmax+1, max(1, xmax//6)):
        xx=left+ww*x/xmax
        out.append(f'<text x="{xx:.1f}" y="{top+hh+22}" text-anchor="middle" fill="#242b35" font-size="13">{x}</text>')
    out.append(f'<text x="{left+ww/2:.1f}" y="{h-20}" text-anchor="middle" fill="#242b35" font-size="17">{html.escape(xlabel)}</text>')
    out.append(f'<text x="24" y="{top+hh/2}" transform="rotate(-90 24 {top+hh/2})" text-anchor="middle" fill="#242b35" font-size="17">{html.escape(ylabel)}</text>')
    palette=["#0b7285","#9c3636","#6264ae"]
    for j,(label,pts) in enumerate(series):
        color=palette[j]
        coords=" ".join(f'{left+ww*x/xmax:.2f},{top+hh-hh*y/ymax:.2f}' for x,y in pts)
        out.append(f'<polyline points="{coords}" fill="none" stroke="{color}" stroke-width="2" stroke-dasharray="5 4"/>')
        for x,y in pts:
            out.append(f'<circle cx="{left+ww*x/xmax:.2f}" cy="{top+hh-hh*y/ymax:.2f}" r="5" fill="{color}"/>')
        out.append(f'<rect x="{"500"}" y="{18+j*22}" width="13" height="9" fill="{color}"/>')
        out.append(f'<text x="521" y="{28+j*22}" font-size="13" fill="#242b35">{html.escape(label)}</text>')
    out.append('</svg>')
    (IMAGE/name).write_text("\n".join(out)+"\n",encoding="utf-8")

def save_assets():
    IMAGE.mkdir(parents=True,exist_ok=True)
    svg_plot(ASSETS["U04:L03"],"A possible temperature graph from function constraints",
         "Fictional W(t) in Celsius at minutes 0,2,5,10,12,15,30: 18,26,55,100,100,96,70. Dashed lines are one possible interpolation, not uniquely determined by constraints.",
         "Minutes since heating started","Water temperature (degrees Celsius)",30,120,
         [("W(t), example only",[(0,18),(2,26),(5,55),(10,100),(12,100),(15,96),(30,70)])],[0,30,60,90,120])
    svg_plot(ASSETS["U05:L03"],"Two different fictional exponential-growth models",
         "Discrete N(t)=50 times 2^t yields 50,100,200,400,800. P(t)=20 times 3^t yields 20,60,180,540,1620 for integer t from 0 to 4. Dashed connections are only visual guides.",
         "Time step t (integer)","Fictional count",4,1800,
         [("N(t)=50*2^t",[(t,50*2**t) for t in range(5)]),
          ("P(t)=20*3^t",[(t,20*3**t) for t in range(5)])],[0,450,900,1350,1800])
    # Visual reference only: not student-built bars, authenticated rendering, nor teacher scoring.
    body='''<svg xmlns="http://www.w3.org/2000/svg" role="img" aria-labelledby="plotTitle plotDesc" viewBox="0 0 690 340">
<title id="plotTitle">Conditional relative frequencies: original fictional aquarium sample</title>
<desc id="plotDesc">Low-solute sample: 16 of 20 tanks healthy, 80 percent; high-solute sample: 6 of 20 healthy, 30 percent. Samples show possible association, not causation.</desc>
<rect width="690" height="340" fill="white"/>
<text x="40" y="37" font-size="20" fill="#242b35">Healthy vs unhealthy within each group (20 tanks each)</text>
<g font-size="15" fill="#242b35"><text x="40" y="95">Low solute</text><text x="40" y="185">High solute</text></g>
<rect x="175" y="70" width="360" height="46" fill="#d6dde6"/><rect x="175" y="70" width="288" height="46" fill="#0b7285"/>
<rect x="175" y="160" width="360" height="46" fill="#d6dde6"/><rect x="175" y="160" width="108" height="46" fill="#0b7285"/>
<g fill="#ffffff" font-size="16" font-weight="bold"><text x="270" y="99">80% healthy</text><text x="190" y="189">30%</text></g>
<g fill="#242b35" font-size="14"><text x="468" y="99">20%</text><text x="390" y="189">70% unhealthy</text>
<text x="175" y="245">0%</text><text x="260" y="245">25%</text><text x="350" y="245">50%</text><text x="440" y="245">75%</text><text x="525" y="245">100%</text>
<text x="175" y="284">Teal: healthy   Gray: unhealthy</text>
<text x="175" y="309">Reference chart only; students must construct their own display.</text></g></svg>'''
    (IMAGE/ASSETS["U03:L03"]).write_text(body+"\n",encoding="utf-8")

def main():
    evidence=[]
    for suffix,v in REVIEWS.items():
        path=SIDE/("us-ccss-hs-alg1-"+suffix.lower().replace(":","-")+"-2021.rich-lesson-v2.json")
        doc=json.loads(path.read_text(encoding="utf-8"))
        lesson=doc["Lessons"][0]
        if lesson["LessonCode"]!=COURSE+suffix or "v13" in doc["ContentVersion"].lower():
            raise RuntimeError(f"Identity mismatch or repeat on {suffix}")
        if len(lesson["WorkedExamples"])<3: raise RuntimeError(f"Missing existing examples {suffix}")
        lesson["ExplanationParagraphs"].extend(v["text"])
        doc["ContentVersion"]="alg1-"+suffix.lower().replace(":","-")+"-v13-source-task-math-20261030"
        save_json(path,doc)
        u,n=[int(s[1:]) for s in suffix.split(":")]
        evidence.append(dict(lessonCode=COURSE+suffix,lessonTitle=lesson["Title"],candidatePath=str(path.relative_to(ROOT)).replace("\\","/"),
           teacherUrl=f"https://im.kendallhunt.com/HS/teachers/1/{u}/{n}/preparation.html",
           studentUrl=f"https://im.kendallhunt.com/HS/students/1/{u}/{n}/index.html",
           sourceActivityHeadingsReviewed=True,teacherGoalReviewed=True,
           sourceComparison="ASSISTANT_TARGETED_TEACHER_STUDENT_SOURCE_REVIEW_NOT_AUTHORIZED_EDUCATOR_APPROVAL",
           correction="SOURCE_TASK_COMPLETENESS_REPAIR_WITH_INDEPENDENT_FIRST_PARTY_MODELS",
           newOriginalExplanations=v["text"],
           rubricDraft=dict(criteria=v["assessment"],maxPoints=8,minPoints=0,status="AUTHOR_DRAFT_ONLY"),
           practiceProposal=dict(families=v["practice"],oracleCases=v["cases"],
              acceptanceCasesRequired=["correct","incorrect","equivalent representation","domain boundary","malformed or unsupported","human-scored artifact"],
              integrated=False,attemptPersisted=False,teacherFeedbackPersisted=False),
           svgRelativePath=ASSETS.get(suffix),svgAuthenticatedRendered=False,
           A_ACCEPTED=False,B_ACCEPTED=False,C_VERIFIED=False,D_VERIFIED=False))
    save_assets()
    save_json(DOCS/"US_CCSS_ALG1_CODER5_V13_THIRD_CROSSUNIT_SOURCE_TASK_REPAIRS_20261030.json",
        dict(schemaVersion=1,courseKey="US-CCSS-MATH:HS:TRAD:ALG1:IM-1E",asOfDate=DATE,reviewed=8,
        newlySourceReviewed=8,priorSubstantiveReviews=17,unreviewedRemaining=107,
        stageAAccepted=0,stageBAccepted=0,stageCVerified=0,stageDVerified=0,lessons=evidence))
    ledgerPath=DOCS/"US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json"
    ledger=json.loads(ledgerPath.read_text(encoding="utf-8"))
    for row in ledger["lessons"]:
        suffix=row["lessonCode"].split(":")[-2]+":"+row["lessonCode"].split(":")[-1]
        if suffix not in REVIEWS:continue
        match=next(x for x in evidence if x["lessonCode"]==row["lessonCode"])
        row["contentVersion"]="alg1-"+suffix.lower().replace(":","-")+"-v13-source-task-math-20261030"
        row["v13"]={"sourceComparison":match["sourceComparison"],"firstPartyRepair":True,
                 "draftRubric":True,"staticSvgRelativePath":match["svgRelativePath"],
                 "svgAuthenticatedRendered":False,"academicApproved":False,
                 "practiceAttemptPersisted":False,"authorizedTeacherGradePersisted":False,
                 "stageAAccepted":False,"stageBAccepted":False,"stageCVerified":False,"stageDVerified":False}
    ledger["asOfDate"]=DATE
    ledger["v13ReviewSummary"]={"priorSubstantiveComparisons":17,"newlyReviewed":8,
         "preliminaryReviewedTotal":25,"stillUnreviewed":107,"academicApproval":0}
    save_json(ledgerPath,ledger)
    matrixPath=DOCS/"US_CCSS_ALG1_CODER5_V12_TEACHER_ASSESSMENT_MATRIX_20261029.json"
    matrix=json.loads(matrixPath.read_text(encoding="utf-8"))
    for row in matrix["lessons"]:
        suffix=row["lessonCode"].split(":")[-2]+":"+row["lessonCode"].split(":")[-1]
        if suffix not in REVIEWS: continue
        row["sourceActivityReview"]="V13_TARGETED_SOURCE_REVIEW_AUTHOR_DRAFT_NOT_APPROVED"
        row["status"]="STRUCTURED_AUTHOR_RUBRIC_DRAFT_NOT_TEACHER_SCORED"
        row["structuredCriteria"]=[{"id":"C"+str(i+1),"name":s,"min":0,"max":2} for i,s in enumerate(REVIEWS[suffix]["assessment"])]
        row["possiblePoints"]=8
    matrix["asOfDate"]=DATE
    matrix["structuredDrafts"]=25
    matrix["unstructuredPending"]=107
    save_json(matrixPath,matrix)
    handoffPath=DOCS/"US_CCSS_ALG1_CODER5_V12_SUPERVISOR2_PRACTICE_CONTRACT_HANDOFF_20261029.json"
    handoff=json.loads(handoffPath.read_text(encoding="utf-8"))
    for row in handoff["contracts"]:
        suffix=row["lessonCode"].split(":")[-2]+":"+row["lessonCode"].split(":")[-1]
        if suffix not in REVIEWS:continue
        row["sourceEvidence"]="V13_TARGETED_SOURCE_REVIEW_NOT_ACADEMIC_APPROVAL"
        row["proposedAssessmentFamilies"]=REVIEWS[suffix]["practice"]
        row["familiesEvidenceLevel"]="SOURCE_SPECIFIC_AUTHOR_PROPOSAL_NOT_SHARED_ENGINE"
        row["exampleOracles"]=REVIEWS[suffix]["cases"]
        row["requiredCases"]=["correct","wrong","equivalent","boundary","malformed","teacher-only"]
    handoff["asOfDate"]=DATE
    handoff["sourceComparedCandidates"]=25
    handoff["sourceActivityReviewsOutstanding"]=107
    handoff["v13NewDraftContracts"]=8
    save_json(handoffPath,handoff)
    print("ALG1 V13 repaired",len(evidence),"candidate lessons; source-reviewed 25/132; A-D remain 0; exact Practice pending Supervisor2")

if __name__=="__main__":
    import sys
    if len(sys.argv)>1 and sys.argv[1]=="--assets-only":
        save_assets()
    else:
        main()
