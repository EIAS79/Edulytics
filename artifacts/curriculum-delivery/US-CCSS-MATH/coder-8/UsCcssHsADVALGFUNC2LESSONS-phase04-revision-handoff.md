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
