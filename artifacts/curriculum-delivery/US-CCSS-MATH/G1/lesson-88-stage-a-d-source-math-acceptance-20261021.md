# Grade 1 — lesson 88 (U04 L23) independent first-party A–D source/math audit
**Code:** `PED:US-CCSS-MATH:G1:U04:L23`. Optional source lesson; first-party Edulytics quantitative understanding is separable from physical classroom experiments.

## A. Original first-edition lesson independently rechecked 2026-10-21
Read-only Our-CS `01_Grade_1_IM_K5/unit-4/lesson-23/lesson.html`, observed full-source SHA-256 `49a4abdbfc72a974394661f746a84c411fa5478f8babf090e21cd4e2d041e0b6`, matching committed original source register; preparation hash `3fd91382fe53cdc5618e7e8ebfe569b90490618d6bb77cef792acdb61674cabe`. Public page https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-23/lesson.html.

- Original warm-up bean photograph asks for three estimates labelled too low, about right, too high; the physical/image quantity is not invented as a fixed digitally known value.
- Activity 1 two handfuls of physical objects: partners conduct THREE separate trials, recording low/near/high estimates and then exact counts with ones and/or groups of ten. These are real-classroom investigations, not guaranteed saved results from simulated arithmetic Practice.
- Activity 2 quantity-card image sort bands exactly **less than 20**, **20 to 50**, and **more than 50**; choose an image from each band and represent the estimated quantity in different tens/ones forms.
- Section synthesis asks how tens and ones support both estimating and counting. Source photos/picture cards are not copied to Edulytics or falsely assumed to have one universally correct number.

## B. First-party teaching boundaries
Existing correction `src/Edulytics.Data/Seeding/CommonCoreG1FullGradeUnit5ContentCorrections.cs` describes "THREE two-handful" trials and all three quantity bands. Original real manipulatives and card/photo observation remain learner/teacher experiences rather than prerequisites for content publication or machine-scored claims.

## C. Independent numeric Practice
Registered `usccss.g1.u04.l23.estimate_and_count` family provides separately validated number/quantity bands (modes 4–6). `UsCcssG1Lesson88SourceQuantityBandsTests.cs` checks 10/19 in <20, 20/50 in middle, 51/99 in >50, correct tens/ones decomposition, wrong count and extra-key rejection, and disallowed out-of-band inputs. This verifies a meaningful first-party *numerical subskill*, not a child's real bean estimate, two-handful experiment, or sorting a specific original photograph.

## D. Authenticated math evidence
Same-run source-scoped authorized learner HTML, real persisted correct/wrong/malformed practice attempt checks and source text verified in [39600111939](https://github.com/EIAS79/Edulytics/actions/runs/39600111939); artifacts 12345398813 and 12346166910. Tested application SHA `5fb0d3786d52379e60de01e3773297afc34d3721`; original source text fragments checked: 2. Authentic unescaped student HTML SHA-256 `8e30332b213065da72a67d1a95b782a0dc16458ba18f00120bfe8de053797cd7`. Mathematics Foundation run 39622317973 passed 1800 principal-suite .NET tests without failure.

**Decision scope:** First-party learner explanations and numeric estimation/category subskill are evidenced, with the optional authentic hands-on tasks available as instructional extensions but not electronically certified or a human publication approval gate. No publisher photo/image reproduction, academic/teacher signoff or production UAT claimed. Grade remains NO_GO.
