# Coder 8 — Phase04 academic revision/approved-predecessor handoff

**State: PROPOSED / NO academic approval / NO production mutation.** This document is owned course-specific evidence for the designated global academic/version integration owner. It does not update the historical seven-repair Phase04 manifest, official outcomes, publishing state, global materializer, database, or release gates.

- Course: `US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC` (`ADV-ALG-FUNC`, `Fourth Courses`); exactly 2 lessons.
- PR: https://github.com/EIAS79/Edulytics/pull/585
- Protected baseline: `main` Git commit `39c757630d2bd67217a1468fce099653ed0f3f53`, pack blob `69306240d3f240177af7dcc6b25fc420c59a8316`.
- Candidate pack blob at handoff: `e0efdbebf6cb50db5ba91556ede1d28a17b2f413`. Source: `src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-alg-func-phase29-v1.lesson-content-pack.json`.
- **Pack content version:** before `phase30-common-core-rich-content-v1` → proposed `us-ccss-hs-adv-alg-func-2lesson-teaching-v1`. A pack-level string alone is not approved per-lesson revision authorization.
- Historical source review: original Phase04 manifest `docs/curriculum/us-ccss-phase04-hs-content-repair-pilot.json`, blob `8bd21971361d4a33270c669f5ff4dae7623a3f91`; existing `humanAcademicSignoff=PENDING` and `gate=NO-GO` are unchanged.

## Immutable original course evidence

Official standards edition and blueprint identities are unchanged; all source hashes and original publisher references remain from the pre-existing accepted blueprint/content pack. No publisher PDF binary or source-hash recomputation has been performed in this handoff; PDF-level source equivalence is **unverified**.

| Lesson | Formal CCSS standards | Publisher source SHA-256 (immutable) | Previous body SHA-256 | Revised body SHA-256 | Historical Phase04 signoff |
| --- | --- | --- | --- | --- | --- |
| L01 All Systems Go | CCSS:HSA-REI.C.8, CCSS:HSA-REI.C.9 | `87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55` | `fc9f1a49bf236c330112fcf1c02e108893344eab58c2275c8eee80552ec5560c` | `6abd1e7b8c3d2472c4d8ec349e4d816f8607f1cdb1f5e3455d1284159a4c3e50` | PENDING |
| L02 Log Logic | CCSS:HSF-BF.B.5 | `590d11f4e182141dc1ba220a9824243fc7b60c18793ebefa5f628931b3a54db3` | `456292fd1af76eaa845e34a66de892a1cd499646b354baed397376ea89a29e30` | `f26d0b43c2be38cbbc5851564a85ab28fc31c3dbb4c970ca829f03343c1f4ec6` | PENDING |

## Exact learner-text field additions (candidate vs baseline)

The following complete appended spans are first-party explanatory extensions; unchanged original content is **reused**. They are not represented as direct quotations or as publisher-sanctioned replacement tasks.

### PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01 — All Systems Go

- Source lesson reference (recorded): MVP-ALG2:M10:T06H — https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/a2mod10tnh718.pdf
- Source URL (recorded): https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/a2mod10tnh718.pdf
- Original source SHA-256 (unchanged): `87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55`
- Body `fc9f1a49bf236c330112fcf1c02e108893344eab58c2275c8eee80552ec5560c` → `6abd1e7b8c3d2472c4d8ec349e4d816f8607f1cdb1f5e3455d1284159a4c3e50`

**keyConceptsAndRules — full appended text**

> For an invertible 2×2 matrix A=[[a,b],[c,d]], det(A)=ad−bc must be nonzero and A⁻¹=(1/(ad−bc))[[d,−b],[−c,a]]. Check dimensions before multiplying A⁻¹b; a 2×2 inverse cannot multiply a 3-entry constants vector.

**workedExamples — full appended text**

> Example 3 — compute an inverse, then solve: 2x+y=5 and x+y=3 give A=[[2,1],[1,1]], b=[5,3]ᵀ and det(A)=2·1−1·1=1. Thus A⁻¹=[[1,−1],[−1,2]] and A⁻¹b=[5−3,−5+6]ᵀ=[2,1]ᵀ. Check: 2(2)+1=5 and 2+1=3. Example 4 — technology for 3×3: return to Example 1 and enter the ordered coefficient matrix A and column b into a matrix-capable tool. Check det(A)=7, so A is invertible. The verified inverse is A⁻¹=(1/7)[[−1,3,2],[3,−2,1],[5,−1,−3]]. Compute A⁻¹b=(1/7)[−6+9+4,18−6+2,30−3−6]ᵀ=[1,2,3]ᵀ. Confirm AA⁻¹=I and Ax=b; if the tool produces decimals, test the original equations instead of treating rounded entries as exact. Example 5 — singular does not always mean inconsistent: x+y=2, 2x+2y=4 has infinitely many solutions (x=2−t,y=t); replacing 4 with 5 gives no solutions. Both coefficient matrices have determinant zero and no inverse.

**stepByStepSolutions — full appended text**

> 8. To use the inverse method on a 2×2 system, calculate ad−bc first. If it is nonzero, form A⁻¹ with the correct signs and 1/determinant factor, multiply A⁻¹b in that order, and check Ax=b. 9. For dimension 3×3 or larger, use appropriate matrix technology to find a candidate inverse, inspect determinant/rank, calculate A⁻¹b and independently check both AA⁻¹=I and every original equation. If A is singular, do not attempt A⁻¹; row-reduce [A|b] to distinguish dependent from contradictory equations. 10. Compare substitution/elimination, row reduction, and inverse methods: they should agree on an invertible system, but only row reduction can classify all singular cases.

**commonMistakes — full appended text**

> A zero determinant means no inverse; it does not by itself mean that the equations have no solutions. For a computed inverse, verify multiplication gives the identity before using it. Matrix software output may be rounded, so numerical agreement alone is not proof of exact equality.


### PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L02 — Log Logic

- Source lesson reference (recorded): MVP-MATH3:M02:T01 — https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/sec3mod2tn718.pdf
- Source URL (recorded): https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/sec3mod2tn718.pdf
- Original source SHA-256 (unchanged): `590d11f4e182141dc1ba220a9824243fc7b60c18793ebefa5f628931b3a54db3`
- Body `456292fd1af76eaa845e34a66de892a1cd499646b354baed397376ea89a29e30` → `f26d0b43c2be38cbbc5851564a85ab28fc31c3dbb4c970ca829f03343c1f4ec6`

**keyConceptsAndRules — full appended text**

> For b>1, logarithms preserve order: 0<x<y implies log_b x<log_b y. For 0<b<1, they reverse order: 0<x<y implies log_b x>log_b y. The quotient rule log_b(x/y)=log_b x−log_b y requires x>0 and y>0. The power law log_b(x^r)=r log_b x is valid for x>0 and real r. These are not licenses to take logs of zero or negative real numbers.

**workedExamples — full appended text**

> Example 5 — reason about logs without a calculator: because 2<3<4 and b=2>1, 1=log₂2<log₂3<log₂4=2. For a base between zero and one, ordering reverses: 3<5 implies log_(1/2)(3)>log_(1/2)(5), and both values are negative because (1/2)⁰=1 and the arguments exceed 1. Example 6 — compare logs in different bases: log₄3=ln3/ln4=(1/2)log₂3, so 0<log₄3<log₂3. Example 7 — justify the product rule: let u=log_b x and v=log_b y for positive x,y. Then b^(u+v)=b^u·b^v=xy, so log_b(xy)=u+v. The same reasoning does not give log_b(x+y)=u+v: log₂(6)≠log₂(2)+log₂(4)=3. Example 8 — apply the domain before solving: log₂(x−3)=1 requires x>3, and exponentiating gives x−3=2, hence x=5, which satisfies the domain and the original equation.

**stepByStepSolutions — full appended text**

> 7. When comparing two logarithms with the same valid base, first ensure both arguments are positive and then use increasing order for b>1 or reversed order for 0<b<1; bound unknown values between powers of the base. 8. To compare different bases, use a valid change-of-base expression or equivalent powers. 9. To justify log laws, translate them to products or quotients of positive exponential expressions; never distribute logarithms across addition. 10. For an open-ended claim, state the domain and write an explicit inverse-exponent or monotonicity argument rather than presenting a decimal alone.

**commonMistakes — full appended text**

> Comparing logarithms without checking whether 0<b<1 reverses order can invert the correct inequality. Do not use log_b(x/y) for invalid nonpositive inputs, and do not mistake a justified ordering task for a single exact-number response.

## Proposed global academic/version-owner action (NOT EXECUTED)

1. Independently source-review each new mathematical example and original MVP goal; distinguish original Phase04 repairs from additional first-party extensions, confirm legally required attribution, and sign with authentic reviewer evidence. Do not promote prior pending human evidence retroactively.
2. Create a **new append-only course revision authorization** for both exact lesson codes: prior pack version, previous body SHA, new body SHA, immutable source SHA, revised lesson fields/teaching tasks, version/reviewer/provenance evidence, tested code SHA and human decision. Retain the historical Phase04 seven-repair manifest and its old body hashes unchanged; never rewrite original approvals in place as if already tested.
3. Decide per-lesson revision identity and pack-level edition transition. Safely add only reviewed predecessor versions to the global materializer/seeder (owned by integration owner; coordinate other curricula's shared-file PRs); preserve existing learner persistence/attempts. Reject unknown predecessors, unexpected body hashes, and unresolved source/auth status; no production DB mutation from Coder 8.
4. Reconcile existing Phase04 hash guards and V2 JSON parity against both the immutable previous revision and a genuinely separately approved candidate revision, rather than disabling checks or substituting unchecked local SHA.
5. Run exact candidate SHA CI, disposable persistent content upgrade from known versions and negative-predecessor cases. Verify actual effective rendered content before accepting. Preserve school adoption/Practice issue #453 and matrix Practice issue #584 as distinct gates.

## Failing gate evidence and handoff pointers

- Academic evidence safety: https://github.com/EIAS79/Edulytics/actions/runs/40775690308
- Original curriculum / JSON parity: https://github.com/EIAS79/Edulytics/actions/runs/40775690395
- Shared implementation request: https://github.com/EIAS79/Edulytics/issues/584
- HS adoption blocker: https://github.com/EIAS79/Edulytics/issues/453
- Full changed-file/line diff: https://github.com/EIAS79/Edulytics/pull/585/files

**Gate verdict: NO-GO until independently approved predecessor/source transition and original academic evidence review exist.**

---

## P0 V2 amendment — source-task closure (2026-10-30; supersedes the earlier unapproved v1 candidate, not the historical evidence)

**Exact course-owned change:** U01:L01 All Systems Go added the MVP painting-supplies context verified against indexed original student + teacher source excerpts. Source review record: `artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/AllSystemsGo-original-source-task-review-v2.md`. The student PDF and teacher PDF could not be fully downloaded/rendered in this session; original publisher SHA remains recorded, not newly verified.

**Proposed version history (none of these candidate revisions is an academic approval):**
- Prior protected content pack: `phase30-common-core-rich-content-v1`; historical Phase04 corrected body L01 `fc9f1a49bf236c330112fcf1c02e108893344eab58c2275c8eee80552ec5560c` remains immutable.
- Superseded *unapproved* Coder 8 v1: `us-ccss-hs-adv-alg-func-2lesson-teaching-v1`, file blob `e0efdbebf6cb50db5ba91556ede1d28a17b2f413`, L01 `6abd1e7b8c3d2472c4d8ec349e4d816f8607f1cdb1f5e3455d1284159a4c3e50`; no approved v1 persisted predecessor is claimed.
- Current *unapproved* Coder 8 v2: `us-ccss-hs-adv-alg-func-2lesson-teaching-v2`, file blob `e19755bf768b384a55518accefecd3f47e23304c`, L01 `9c30b68f87f48b668c32875483260a44b6600ee69ca46b9ec5b8a0706ed6b6f7`.
- L02 Log Logic remains semantically **identical** to v1 with body SHA `f26d0b43c2be38cbbc5851564a85ab28fc31c3dbb4c970ca829f03343c1f4ec6` and source SHA `590d11f4e182141dc1ba220a9824243fc7b60c18793ebefa5f628931b3a54db3`.
- L01 publisher source SHA remains recorded `87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55`, unchanged (not recomputed from source PDF bytes). No change to lesson code or outcome mapping.
- **Field-specific diff:** L01 only `explanation`, `workedExamples`, `stepByStepSolutions`, `commonMistakes` plus its `canonicalBodySha256` and pack-level `contentVersion`; existing earlier lesson material was retained. Full exact appended strings below.

**explanation — appended first-party instruction:**

```text
In the original All Systems Go painting-supplies context, three purchase totals provide three equations for the unknown unit prices of paint, brushes, and masking tape. A correct 3×3 coefficient matrix records zero wherever an item was not bought, and the inverse solution must be interpreted as prices rather than just an ordered triple.
```

**workedExamples — appended first-party instruction:**

```text
Example 6 — painting-supplies context, adapted from the MVP student task: let p be dollars per gallon of paint, q dollars per brush, and r dollars per roll of masking tape. Three shopping totals give 2p+r=30, p+4q=20, and 2q+r=10. In variable order [p,q,r]ᵀ, A=[[2,0,1],[1,4,0],[0,2,1]], c=[30,20,10]ᵀ, so A[p,q,r]ᵀ=c. The zeros correspond to supplies absent from each purchase. det(A)=10, so A is invertible. A⁻¹=(1/10)[[4,2,−4],[−1,2,1],[2,−4,8]], which yields [p,q,r]ᵀ=[12,2,6]ᵀ. Therefore paint costs $12 per gallon, a brush costs $2, and masking tape costs $6 per roll. Check all original totals: 2(12)+6=30, 12+4(2)=20, and 2(2)+6=10. Counterexample to careless contextual acceptance: if only the third total were $50, the same invertible matrix would produce [−4,6,38]ᵀ. That triple satisfies the altered equations but a negative paint price is not plausible; discuss the data instead of calling it an inconsistent linear system.
```

**stepByStepSolutions — appended first-party instruction:**

```text
11. For the painting-supplies task, name the price variables and lock the order [paint, brush, tape]. Translate the three receipts into equations, then A=[[2,0,1],[1,4,0],[0,2,1]] and c=[30,20,10]ᵀ. Evaluate det(A)=10, use a verified 3×3 inverse or a trusted matrix tool to calculate A⁻¹c, and check all three receipts before interpreting the unit prices. 12. Treat missing items as zero coefficients; do not swap the brush and tape columns without changing the variable vector too. For a different set of totals, a negative unit price can be algebraically valid but contextually impossible; report that modeling limitation separately from matrix singularity.
```

**commonMistakes — appended first-party instruction:**

```text
In the source painting-supplies model, the first receipt has zero brushes, the second has zero tape, and the third has zero gallons of paint; inserting a nonzero coefficient for an unpurchased item changes the data. An invertible system can also give negative prices for altered receipts: this is an invalid real-world interpretation, not proof of an algebraic contradiction.
```


**Independent mathematical oracle:** A=[[2,0,1],[1,4,0],[0,2,1]], b=[30,20,10]ᵀ, det=10, A⁻¹=(1/10)[[4,2,−4],[−1,2,1],[2,−4,8]], prices [12,2,6]ᵀ. Three receipt substitutions verified; wrong first-row brush/tape ordering rejected; hypothetical third receipt $50 returns [−4,6,38]ᵀ, algebraically valid but contextually nonphysical. The added course-specific C# test asserts A·(10A⁻¹)=10I₃ and boundaries, but test execution is separate evidence.

**Shared academic/version-owner handoff:** On main the protected Phase04/JSON parity guards still reject changed learner bodies without an independently reviewed append-only authorization. Do NOT replace previous seven-row hashes, treat prior pending signoffs as approval, allow arbitrary predecessors, or claim a v1 candidate was persisted. Independently review the source task + new field spans and approve or reject the v2 successor; then route transition through the single shared materializer/seeder owner and run original/negative predecessor tests on an isolated integrated SHA. CI failures are expected until authorized. `#584` and `#453` remain separate gates; `#600` alone does not certify production Practice or school adoption.


---

## 2026-10-30 P0 follow-up: Log Logic source-goal repairs, 26-case input boundaries, six human rubrics

**Latest candidate remains unapproved.** Earlier historical Phase04 rows and original publisher SHA values are unchanged, including the original seven-row pending-signoff record. Initial prior protected content version was `phase30-common-core-rich-content-v1`; subsequent course-only draft candidates teaching-v1 and teaching-v2 were not independently signed, materialized, or deployed. **Do not authorize teaching-v1 or v2 as a persisted predecessor simply because these Draft commits exist.**

| Item | Prior unapproved candidate | Current unapproved candidate |
| --- | --- | --- |
| Pack content version | `us-ccss-hs-adv-alg-func-2lesson-teaching-v2` | `us-ccss-hs-adv-alg-func-2lesson-teaching-v3` |
| Pack Git blob | `e19755bf768b384a55518accefecd3f47e23304c` | `3828be4b5babd5cfa09dd1b226ef62185790c2b8` |
| U01:L01 All Systems Go body | `9c30b68f87f48b668c32875483260a44b6600ee69ca46b9ec5b8a0706ed6b6f7` | `9c30b68f87f48b668c32875483260a44b6600ee69ca46b9ec5b8a0706ed6b6f7` (no change after V2) |
| U01:L02 Log Logic body | `f26d0b43c2be38cbbc5851564a85ab28fc31c3dbb4c970ca829f03343c1f4ec6` | `d4edd48f5880526c169b3e4207d3b77cf75129c1e36011eb27e780240b940f2c` |
| U01:L01 publisher source SHA | `87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55` | unchanged, PDF binary not rehashed |
| U01:L02 publisher source SHA | `590d11f4e182141dc1ba220a9824243fc7b60c18793ebefa5f628931b3a54db3` | unchanged, PDF binary not rehashed |

### Proven source-task gap and narrowly scoped correction

Recorded source: MVP Secondary Mathematics III, Module 2, `2.1 Log Logic` teacher notes `https://www.mathematicsvisionproject.org/uploads/1/1/6/3/11636986/sec3mod2tn718.pdf`. Indexed student and teacher task text places logarithmic values on a number line, estimates unknowns using neighboring inverse powers, requires explanations, and asks students to test global log-comparison claims. Existing L02 did teach inverse powers, monotonicity, domain and change-of-base, but did **not explicitly implement** source-style number-line placement or demonstrate a counterexample to a universal cross-base inequality. This v3 extension does so through original first-party teaching examples, not verbatim task copying or publisher artwork. Original large PDF files were not downloaded/rendered and the full publisher PDF task review is NOT certified.

Exact source-aligned new mathematics: base-3 number-line anchors `log₃(1/9)=−2, log₃(1/3)=−1, log₃1=0, log₃3=1, log₃9=2`; bound `1<log₃5<2`; explain that a proposed universal `log₂x<log₃x` for all `x>0` is **false**, since at `x=1` both zero, at `x>1` relation reverses to `log₂x>log₃x`, and at `0<x<1` it holds. Independent exact-power witnesses: `log₂9>3>log₃9=2` and `log₂(1/9)<−3<log₃(1/9)=−2`.

**Field-specific differences**: only `explanation`, `workedExamples`, `stepByStepSolutions`, `commonMistakes` within L02 plus its `canonicalBodySha256` and pack-level `contentVersion`. No L01, lesson code, source SHA or official outcome changes.

**L02.explanation exact appended span:**

```text
In the source Log Logic activity, put known and estimated logarithms on a number line and justify each position from exponential powers. Not every comparison between logarithms with different bases has one fixed direction for every positive argument.
```

**L02.workedExamples exact appended span:**

```text
Example 9 — number-line reasoning without a logarithm button: for base 3, log₃(1/9)=−2, log₃(1/3)=−1, log₃1=0, log₃3=1, and log₃9=2, in that left-to-right order. The exponents give the exact locations; to locate log₃5, use 3<5<9 so 1<log₃5<2. Example 10 — test a claimed universal comparison of bases: log₂(x)<log₃(x) is NOT true for every x>0. At x=1 both are 0; for x>1, ln(x)>0 and ln(2)<ln(3), so log₂(x)>log₃(x); for 0<x<1, ln(x)<0 and the inequality reverses to log₂(x)<log₃(x). The argument must be positive. Check using exact powers: log₂9>3>log₃9=2, but log₂(1/9)<−3<log₃(1/9)=−2. A single observed example cannot prove a statement about every positive x.
```

**L02.stepByStepSolutions exact appended span:**

```text
11. To position logarithms on a number line, first evaluate inverse powers exactly (including zero and negative exponents), then bound unknown entries between nearby known powers of their positive valid base; explain placements without relying solely on decimals. 12. Before accepting a conjecture comparing log₂x and log₃x for all x>0, split the domain into 0<x<1, x=1, and x>1; the sign of ln(x) changes, so a claimed universal strict inequality fails.
```

**L02.commonMistakes exact appended span:**

```text
A logarithm comparison observed at x>1 cannot be generalized unchanged to 0<x<1; both expressions equal zero at x=1. Placing logarithms on a number line without explaining inverse-power bounds omits the source task's reasoning goal.
```

### Shared integration and teacher evidence, no fake acceptance

- All original **26** course fixture identities preserved (11 matrix, 15 logarithm), now each separately records canonical, equivalent, wrong, malformed and boundary exemplars. These are proposed test vectors; **no 26 actual student submissions or selected production questions verified**.
- L04–L06 now explicitly require a relation **and explanation**, and their comparison teacher rubrics join existing M05, M11, L13 rubrics (six total). These human review criteria are course-owned candidate evidence; real authenticated teacher score persistence has not been exercised.
- Exact source-backed matrix/Practice blocker: `PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01` maps via outcomes `CCSS:HSA-REI.C.8/.9` to an old `one-step-formula-equation` fallback on main. Expected are matrix-system and inverse capabilities with exact 2×2/3×3, singular dependent/inconsistent, dimensions, matrix order and malformed-response rejection. Do not relax `MatrixStandardsMustNotSilentlyUseOneVariablePractice` or `LessonPracticeRegistryMustExposeActualMatrixAndLogCompetencies`.
- Shared owner on PR #600 must integrate course fixtures, route actual student Practice, test exact-equivalent/invalid cases, row-justification and log-comparison teacher grading, attempt persistence, school adoption #453 and two-school isolation. PR #602 addresses candidate leak safeguards separately.
- Shared materializer/version owner must independently approve an append-only v3 successor from **actually persisted** version/verified prior body SHA, not assume candidate-v2 is persisted. Protected Phase04 parity/academic checks remain red until honest lineage evidence and reviewer approval.
- Course-side C# tests now include 26 new `[InlineData]` schema cases and log source-goal independent oracle; connector-side JS mathematical fixtures 26/26 passed **without runtime or .NET implications**. CI exact tested SHA must be recorded independently.

