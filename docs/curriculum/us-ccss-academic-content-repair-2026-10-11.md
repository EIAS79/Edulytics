# US Common Core Academic Content Repair — 2026-10-11

## Scope and safeguards

This follow-up repairs two incorrect high-school student-facing lesson bodies, adds eight narrower verified Practice question families, and corrects a case-sensitivity error in the child-clause evidence inventory. It does not certify every Common Core lesson. Authenticated student and teacher UI testing remains deferred as requested.

## Source-backed findings

- The lesson The Arithmetic of Vectors (PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01) included unrelated prose about parabolas, ellipses, and the distance formula rather than vector operations (HSN-VM.B.4/B.5).
- The lesson Expected Value in Games and Decisions (PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05) included site-navigation data dumps and webpage chrome in the instructional prose rather than a complete HSS-MD.B.5 explanation and decision example.
- These two English lessons now contain original mathematical explanations, independent examples, procedural steps, common mistakes, and concise summaries. They retain the old publisher-artifact sourceSha256 for provenance and use new canonicalBodySha256 digests for the corrected text.
- Eight dedicated generated question families cover vector component addition, subtraction, scalar multiplication, resultant magnitude/direction and scalar magnitude, plus expected net game payoff and expected costs across strategies. Routes are restricted to these two lessons and do not change other lessons.
- The earlier evidence count (82 direct / 39 unresolved) incorrectly treated the same publisher clause reference with uppercase and lowercase final letters as different. Rechecking the exact unmodified blueprints case-insensitively yields 106 direct source references and 15 without publisher-exact child-code references. Direct source references still do NOT certify question quality.
- Remaining 15 source-code gaps: 3.MD.C.7.a; 5.MD.C.3.a; 6.SP.B.5.a; HSF-BF.A.1.c; HSF-BF.B.4.b/c/d; HSF-IF.C.7.d; HSN-VM.B.4.a/b/c; HSN-VM.B.5.a/b; HSS-MD.B.5.a/b. The last seven have improved educational content and Practice examples but cannot be marked publisher-exact without source evidence.

## Test and deployment requirements

- A focused regression ensures valid source provenance, corrected body fingerprints, absence of unrelated source chrome, proper Practice routing and correct independently recomputed answers across random seeds and difficulties.
- Require all CI and full unit tests before merging. Do not revise the 1,560 US lesson identities, the 2,882 official outcome links, or student progress.
- Confirm runtime Edulytics:LessonContent:ReadFromJson is enabled before asserting that a JSON-only deployment updates what students see. The maintenance seeder may be deliberately skipped on the running Render service; that does not prove JSON mode either way.
- The release scope is these targeted academic fixes. A final all-clause certification must await independently checked lesson content and question/grading evidence.