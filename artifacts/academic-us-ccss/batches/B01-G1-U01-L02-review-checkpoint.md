# B01 — Lesson 002 of 500: source-to-learner review in progress

**LessonCode:** `PED:US-CCSS-MATH:G1:U01:L02`
**Title:** Explore Expressions and Sums
**Batch:** B01, ordinal 2/500 (G1 U01 L02)
**Academic status:** `REWORK_REQUIRED` — not certified. L1–L6 are **not** marked PASS here.
**Scope:** This record does not revise the 1,560-row certification ledger or create an unsupported signature.

## Authority and source identity

- Primary source: Illustrative Mathematics / Kendall Hunt, IM K–5 Grade 1, Unit 1, Lesson 2, 2021 first edition (CC BY 4.0).
- [Teacher lesson](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/lesson.html)
- [Teacher preparation](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html)
- Target CCSS standards: `1.OA.C.5`, `1.OA.C.6`; `K.CC.A.1` is prior knowledge, not a full lesson mastery claim.
- Source-locked blueprint semantic SHA-256: `fdcaa650396daf2509379d57c4695bd2fefcf04ed67ed28ad2030c887a56396e`.
- Original pack candidate body SHA-256 (not yet a verified effective runtime SHA): `7d8c9aa59717d264f80522bcf07c5385b5f332b2808b66c5c75fb19672c70e33`.
- Learner source file: `src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json`.

## Source-to-content comparison (first-pass findings)

The 2021 teacher lesson requires two complementary experiences, not merely computational addition:

1. **Match two dot-cube pictures with their addition expressions and find totals;** then **draw missing dot representations or write missing expressions**. Examples given by the publisher include 4+2, 5+3, 6+4, 3+2 and 4+3.
2. **Check it Off — Add within 10:** use two number cards or dot cubes, find each sum with fingers, objects or drawings, and write different addition expressions for the same total. Students learn the term *sum*.

The current English learner pack contains mathematically correct numerical examples `3+2=5`, `4+2=6`, `5+3=8` and a definition of sum. The pack's worked examples and steps, however, **do not actually demonstrate the source's matching/drawing two-dot-cube activity**, and do not show a worked example of writing an expression from a two-dot-cube picture. The pack is text-only at this entry; the **rendered Rich V2 is not yet inspected**, so do not assert that the deployed view lacks all visuals.

**Verified arithmetic (independent calculation):** 3+2=5; 4+2=6; 5+3=8; publisher 6+4=10; publisher 4+3=7. These checks alone are not L2 PASS for the entire lesson/runtime.

## Repair requirements before certification

- Teach the visual-to-symbol direction explicitly: two authored dot-cube faces → match/write `a+b` → calculate and check the sum.
- Teach the symbol-to-visual direction: given `4+2`, produce 4 dots and 2 dots and show why the sum is 6, with first-party drawings/markup; do **not** copy publisher images/logos.
- Include `0–10` number-card/ten-frame choices and two different ways to compose a total where meaningful, preserving Grade 1 language.
- Verify actual lesson-scoped Practice generator(s), exact answers, wrong answers, edge cases, randomized samples and persistence; prevent unrelated question types.
- Inspect `Rich V2` student rendering/accessible dot counts and source credit on desktop/mobile in disposable authenticated CI, plus school/student authorization checks.
- Record source SHA, **effective** learner body SHA, regression test IDs and true L1–L6 / eight-dimension evidence **only after** execution. Avoid inventing PASS.

## Gate checkpoint

| Gate | Current decision | Evidence gap |
|---|---|---|
| L1 | SOURCE_REFERENCES_IDENTIFIED, not PASS | Source attribution/license, official outcomes and effective-version evidence must be anchored in final record |
| L2 | PARTIAL_NUMERIC_CHECK, not PASS | Independently test actual numerical solver and answer validator |
| L3 | REWORK_REQUIRED | Missing taught matching/drawing activity in authored learner examples |
| L4 | NOT_VERIFIED | Check/add source-aligned Rich V2 representations |
| L5 | NOT_VERIFIED | Exact lesson Practice runtime, feedback and stored attempts |
| L6 | NOT_VERIFIED | Authenticated desktop/mobile rendered lesson and role isolation |

**Next exact operation:** Implement the source-scoped G1 U01 L02 representation and lesson-family improvements on the B01 branch, then run targeted tests and secure disposable student acceptance. Do not promote the lesson to `INTERNALLY_CERTIFIED` before its source-specific evidence clears all six gates. Do not merge/publish B01 before 500/500.
