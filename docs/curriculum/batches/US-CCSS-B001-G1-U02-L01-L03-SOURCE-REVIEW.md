# B001 original-source pedagogy review — positions 16–18 (IM 2021)

**Status:** source-page comparative content **repair candidate**, **not** 3 internally accepted lessons; not a 500-lesson release. Reviewed against public full 2021 teacher lesson pages and preparation on 2026-10-16. Original source files, images and publisher trademarks are not committed.

| Ordinal | LessonCode | Publisher preparation / complete teacher+student tasks | Source goal | Found in current embedded learner body | Proposed targeted repair |
|---:|---|---|---|---|---|
| 16 | `PED:US-CCSS-MATH:G1:U02:L01` | [prepare](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-1/preparation.html), [complete lesson](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-1/lesson.html) | Add To / Take From, result unknown; **write an expression describing the action** using drawings, 10-frame, objects; connect story and numbers. | Old explanation reproduced teacher-facing phrasing and old steps taught just 9−2=7 with no explicit construction of story expression. | Original Edulytics learner prose compares join vs leave, **expression vs answer/equation**, objects→expression→count→check; paired addition/subtraction stories. |
| 17 | `PED:US-CCSS-MATH:G1:U02:L02` | [prepare](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-2/preparation.html), [complete lesson](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-2/lesson.html) | From story **write and explain full equations**, not just calculate; draw box round answer to actual question; addition and subtraction. | Old examples: 5+1, 5+2, 6+2 mental-sum drills; weak story-to-equation connection and no meaningful take-from representation. | Story-specific examples, equals sign, meaning of each numeral, boxing answer when story asks one, contrast of join vs leave. |
| 18 | `PED:US-CCSS-MATH:G1:U02:L03` | [prepare](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-3/preparation.html), [complete lesson](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-3/lesson.html) | **Add To, Change Unknown**: count on to determine added amount, e.g. start 6, end 8; answer can be middle addend, not RHS. | Old worked example 3+6=9 gave the entire second addend rather than training an unknown change; did not explain how to locate the answer. | Original Edulytics examples 6+[2]=8, 3+[6]=9, demonstrate count on and wrong boxing of total, with checks. |

## Source integrity, coverage and deficiencies

1. Canonical immutable source lesson metadata is in `src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json`. Guard exact `SourceUrl`, `CanonicalBodySha256`, standards and original learner step/example signatures; reject unexpected source/production versions. **Do not mutate the original third-party pack**.
2. The new corrected effective learner prose is in `CommonCoreG1Unit2StorySourceCorrections.cs`, applied by canonical content materializer and checked against stored metadata. A lesson's source link or a Math engine pass alone is not academic certification.
3. The original edition explicitly states CC BY 4.0 for its text, while separately reserving IM name/logo and identifying separately licensed images. All new teaching prose and examples are independently written, no publisher pictures are copied.
4. Online original *preparation and complete public lesson activities* for these three are read. Protected Teacher Guide downloadable file, restricted cool-down materials, independent external pedagogue review and actual rendered production learner evidence remain **unverified**; record that limitation rather than claim full book parity.
5. **Practice still requires a verified exact source-goal family**: L01 expression construction, L02 equation-from-story selection, L03 missing-change selection and correct answer/negative controls. Test the authenticated Grade 1 learner experience in disposable schools and check production SHA separately.
6. All 500 B001 lesson-level approval records remain **NOT_ACCEPTED**. These three are first repair candidates, not an accepted 3/500 release. Do not merge/ship before source and Practice/UAT gates.

### Current evidence
- Source edition: Illustrative Mathematics Grade 1 Unit 2 (2021); publisher URLs listed above.
- Target: `G1:U02:L01`, `G1:U02:L02`, `G1:U02:L03`; original ordinal 16–18.
- Baseline production content version prior to patch: `common-core-rich-content-v1` for all three (read-only Neon 2026-10-16).
- Code and test evidence: PR CI must pass on candidate SHA. `IN_REVIEW` until same-SHA CI, Practice, student-visible UAT, and academic signoff.
- Next cursor: Grade 1 Unit 2 Lesson 4, once source full lesson has been compared.
