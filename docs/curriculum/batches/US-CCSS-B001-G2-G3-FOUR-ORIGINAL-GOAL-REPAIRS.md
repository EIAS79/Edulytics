# B001 source-faithful learning-goal mismatch repairs — four lessons (IN REVIEW)

These four corrections were authored by comparing the **original Kendall Hunt / Illustrative Mathematics 2021 teacher preparation and full teaching lesson HTML on Our-CS** against Edulytics' original English learner content pack. The original teacher `Student Facing` activity prompts were included in the original full lesson HTML; a separate same-edition student book PDF and external academic reviewer signoff remain unverified. No source images, publisher word-for-word activities or brand marks are copied.

| B001 lesson | Original 2021 goal and learning sequence | Defect found in the old learner body | Edulytics-authored proposed repair |
|---|---|---|---|
| G2 U02 L01 — Add and Subtract to Compare | Add/subtract within 100 **without composing/decomposing a ten**, compare bar-graph values, show tens/ones methods | The earlier worked examples `32-18` and `27+15` require exchanging tens/ones, directly contradicting the lesson's core constraint | Consistent non-regrouping graph comparisons, tens/ones explanation: `46-22=24`, `34+23=57`, cross-check methods |
| G2 U03 L12 — Saree Silk Stories: Friendship Bracelets | Solve **two-step length** problems with ribbon strips or labelled tape diagrams, link first result to second step | Prior StepByStepSolutions was a generic five-line instruction; examples overemphasized unrelated number sentences | Unique 2-step labelled ribbon story `86-24=62`, `62-18=44` cm; back-check and unit-aware tape diagrams |
| G3 U01 L12 — Represent and Solve Multiplication Problems | Pose relevant questions about equal groups, represent with drawings, arrays and labelled tape diagrams, then multiply | Original pack example reasoning was an empty generic "identify equal groups" answer even for an actual contextual question | Ask the missing question, model 4 trays ×3 pencils, compare representations, and explicitly explain factors |
| G3 U01 L15 — More Factors, More Problems | Solve equal-group multiplication and **unknown-factor equations**, use diagrams; avoid defining multiplication only as repeated addition | Prior step sequence taught writing an **addition** relationship, with unrelated measurement rules and addend mistakes | `4×[ ]=20`, `[ ]×6=18`, unknown position identification, arrays and skip-counting; misconception correction |

## Exact original source file identity (read-only Our-CS)

| Code | Local teacher preparation SHA-256 | Full original 2021 lesson SHA-256 |
|---|---|---|
| `PED:US-CCSS-MATH:G2:U02:L01` | `9c3303b2e57e1861fcefc7f5a99ea611d5b3207d4c24d49f6149d051e7e936dc` | `6726250e4c4a918c021debc92a2ca9914a9892e6f86d07a284cb85534a1c633e` |
| `PED:US-CCSS-MATH:G2:U03:L12` | `8eef8b59a9b83fcf69232d0c92b2c7d503829db29550bbf6df756905087f4599` | `8be1542cf7adf6c8794f5aa75ac87cbeb0ff4b60cb35204fe69ffebc7b3b2108` |
| `PED:US-CCSS-MATH:G3:U01:L12` | `2a79df3c205c8417ac0aef21d33ecb75f580a6f498e885f84f96bf4f6fd26032` | `afdd7c443c1f2db8f2195bbc6e96310aa78d2b5942796b9dd135c24676a22675` |
| `PED:US-CCSS-MATH:G3:U01:L15` | `2727d60eae1fc8abdd5c3b8176d77e3f80b5c32627e57d02e6f19b92b1b1a699` | `9cc785aa8de895a3e5bd66a7dd0abdab281d8be83d9429c08217782e9b05bd28` |

The source preparation and the complete lesson include activity and learning-goal references. The original source JSON `CanonicalBodySha256`, exact teacher preparation URL and original Common Core outcomes are guarded by code, along with the existing persisted content version and old body signatures. Any mismatched source identity or edited pre-existing text **fails closed**. Edulytics' 2021 original source pack remains unchanged.

## Release and acceptance state

- Code: `CommonCoreB001SourceGoalMismatchCorrections.cs`; tests: `CommonCoreB001SourceGoalMismatchCorrectionsTests.cs`. The new examples are original Edulytics text.
- Per-lesson math arithmetic, effective Rich V2 learner rendering, old-version drift and idempotence have automated regression coverage, pending GitHub CI results on the exact branch SHA.
- Matching **Practice question families** for each primary source learning goal, authentic student attempts/response persistence for these exact four, lesson-specific teacher signoff, visual activity parity and final source licence verification are **not complete**.
- No B001 academic approval, no full 500-lesson release, and no owner production UAT is asserted. **B001 NO_GO** remains in force.
