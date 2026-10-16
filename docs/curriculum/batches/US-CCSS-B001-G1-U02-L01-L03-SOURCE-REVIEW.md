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

## 2021 original publisher HTML verified on connected Our-CS (read-only)

Source archive: `C:\\Users\\khali\\Downloads\\New folder (2)\\Common Core`. This review inspected the original 2021 Kendall Hunt **teacher** `preparation.html` and `lesson.html`, including the student-facing tasks reproduced in the teacher lesson. We did not copy the third-party HTML or licensed figures into the repository. The separate `Student_Public_Mirror` includes newer IM v.360 pages and some 404s and **must not** be substituted for the 2021 edition.

| Source lesson | teacher preparation raw SHA-256 | full teacher lesson raw SHA-256 |
|---|---|---|
| G1 U02 L01 | `5963e65f6af0bec30b6cdc51306692a52e4dd982f9aa28efcb38c3b2f8cf7b11` | `6cabbae49552d47181695dec77907bda36af4a4914604bf8c57689530583b89a` |
| G1 U02 L02 | `a447c6b3ecd69d3ae0c1f2f8f27c65d91391de6fd49117df2e5dc9b6a35bdc01` | `ae35a3e4a83534342393f55c1969d3a1d62df95643fcf43b17e4a011275f9399` |
| G1 U02 L03 | `d9e3229faa6f5805627e273fe754e11652073cb5285c43fc33ea32ee98ee4bfe` | `28c0eb6c8edc820313f3f188433f5164913a71900b4d91632885dffb51a99094` |

The local teacher text verifies source goals: L01's **The Library** activity explicitly emphasizes expression construction (equations introduced later); L02's **Write Equations** activity emphasizes story-to-equation sensemaking and meaning of each number; L03's **Kiran's Books** activity emphasizes **Add To, Change Unknown** and identifying the answer as the added amount. Preserve these exact differences in student teaching and Practice.

### On-device directory completeness (not academic approval)

The original local 2021 teacher archive has complete `preparation.html` + `lesson.html` file pairs for Grade 1: **146/146**, Grade 2: **146/146**, Grade 3: **143/143**, Grade 4: **149/149**. B001 uses only the first 65 grade-ordinal positions of G4, so the required 500 have the corresponding source **directory pairs available**. This confirms archive file presence, **not** that all 500 source pages have been individually read, checked for edition/accuracy/rights, compared with the learner content, tested or approved.

Review and quality status remains **BATCH_NO_GO**; source-file presence is an intake gate only.