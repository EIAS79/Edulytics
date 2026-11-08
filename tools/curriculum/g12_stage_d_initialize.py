from pathlib import Path
import json,datetime,subprocess
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p=json.loads((B/"full-grade-progress.json").read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==73 and len(p["LessonEntries"])==73
sha=subprocess.check_output(["git","rev-parse","HEAD"],cwd=R,text=True).strip()
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
rows=[]
for item in p["LessonEntries"]:
    n=item["Ordinal"]
    reasons=[]
    if n==1:
        if item.get("SourceElementsIndependentlyMatched",0)==0: reasons.append("SOURCE_SEMANTIC_VERIFICATION_NOT_ESTABLISHED")
        if not item.get("RightsVerified",False): reasons.append("SOURCE_RIGHTS_NOT_VERIFIED")
        if item.get("ExerciseRoutingRuntimeBound",0)==0: reasons.append("GRADE12_ASSESSMENT_RUNTIME_NOT_WIRED")
        if not item.get("SemanticEquivalenceVerified",False): reasons.append("SOURCE_SEMANTIC_FLAG_FALSE")
    rows.append({
        "lessonOrdinal":n,"lessonCode":item["LessonCode"],"unit":item["Unit"],
        "section":item["SourceSection"],"aCStatus":item.get("ACStatus"),
        "sourceSnapshotId":p["SourceSnapshotSha256"],"sourceEdition":p["SourceEdition"],
        "contentVersion":"NOT_RUNTIME_RESOLVED","candidateSha":sha,
        "ciRefKind":"BRANCH_HEAD_PENDING_PR_MERGE_RUN",
        "sourceElementCount":None,"verifiedEquivalentElementCount":None,
        "sourceParity":"UNKNOWN","digitalCoverage":"UNKNOWN","rightsStatus":"UNVERIFIED",
        "unitTests":"NOT_RUN","integrationTests":"NOT_RUN","authenticatedE2E":"NOT_RUN",
        "visualAccessibility":"NOT_RUN","teacherReviewStatus":"PENDING_EVIDENCE",
        "technicalStatus":"NOT_RUN","academicStatus":"NOT_APPROVED",
        "openDefects":reasons,"actionsRunIds":[],
        "evidencePaths":[x for x in (item.get("EvidencePath"),item.get("ClosureEvidencePath")) if x],
        "dStatus":"D_BLOCKED" if reasons else "D_PENDING",
        "nextAction":"RECONCILE_A_C_HANDOFF_AND_RUNTIME_EVIDENCE" if reasons else "WAIT_FOR_SEQUENTIAL_TESTED_CANDIDATE",
        "updatedAtUtc":now
    })
ledger={
 "schemaVersion":1,"scope":"US-CCSS-MATH:G12:OPENSTAX_PRECALCULUS_2017",
 "lessonCount":73,"unitCount":12,"branch":"curriculum/us-ccss-g12-precalculus-abc-20261106",
 "pullRequest":626,"sourceSnapshotId":p["SourceSnapshotSha256"],
 "testedCandidateSha":None,"baselineCandidateSha":sha,"documentedAtUtc":now,
 "noProductionChanges":True,"runtimeTestsStarted":False,"lessonResults":rows,
 "firstIncompleteLessonOrdinal":1,"blockedLessons":[1],
 "dVerifiedCount":0,"academicApprovedCount":0,
 "sourceParity":{"numerator":None,"denominator":None,"percent":"UNKNOWN"},
 "digitalCoverage":{"numerator":None,"denominator":None,"percent":"UNKNOWN"},
 "gradeStatus":"NO_GO_TECHNICAL","sharedResourceStatus":"NOT_VERIFIED",
 "handoffConflict":{"progress":"73_OF_73_READY_FOR_STAGE_D","handoffDocument":"INCOMPLETE_A_C_HANDOFF",
 "resolution":"NO_D_VERIFIED_UNTIL_SOURCE_AND_RUNTIME_EVIDENCE_ON_CANDIDATE"},
 "ciRunIds":[],"releaseDecision":"NOT_AUTHORIZED",
 "notes":["Source-alternative questions do not establish independent semantic source parity.",
 "Existing Grade 12 tests written but not run on candidate.",
 "No authenticated student/teacher E2E or visual QA verified."]}
(B/"grade12-stage-d-execution-ledger.json").write_text(json.dumps(ledger,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
lines=[
 "# Grade 12 Stage D — evidence index (initial checkpoint)",
 "",
 "- PR: https://github.com/EIAS79/Edulytics/pull/626",
 "- Baseline candidate, not yet tested: "+sha,
 "- Original source SHA256: "+p["SourceSnapshotSha256"],
 "- Source edition: "+p["SourceEdition"],
 "- Inventory: 73 sections / 12 chapters; A–B–C progress READY 73/73.",
 "- Handoff document still claims incomplete. Conflict unresolved.",
 "- Source parity: UNKNOWN. Digital coverage: UNKNOWN.",
 "- First Stage D lesson U01:L01: D_BLOCKED; 2–73: D_PENDING.",
 "- D_VERIFIED 0/73, academic approval 0/73.",
 "- No Stage D tests run on this candidate yet.",
 "- No deployment, production data changes, approval or merge.",
 "",
 "## First lesson actual blocking evidence",
 "Progress first lesson: SourceElementsIndependentlyMatched=0;",
 "SemanticEquivalenceVerified=false; RightsVerified=false;",
 "ExerciseRoutingRuntimeBound=0 despite READY_FOR_STAGE_D.",
 "",
 "## Next step",
 "Run GitHub Actions independent source/static checks and focused G12 .NET tests.",
 "Reconcile only proven source/runtime deficits and record actual run IDs."
]
(B/"grade12-stage-d-evidence-index.md").write_text("\n".join(lines)+"\n",encoding="utf8")
print("LEDGER_CREATED",sha,len(rows))
