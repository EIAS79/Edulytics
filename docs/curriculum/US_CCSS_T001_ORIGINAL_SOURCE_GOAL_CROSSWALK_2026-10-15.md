# US CCSS T001 — original-source goal crosswalk and runtime evidence

**Scope:** Grade 1 Unit 1, Lessons 1–10 ONLY. **Status:** source-purpose comparison completed; **not a substitute for lesson-specific automated test acceptance**. **Source reading:** 2026-10-15; first-party source files on authorized Our-CS, read-only.

## Original 2021 publisher teaching goals versus actual lesson practice

The original preparation and teacher lesson pages in `C:\\Users\\khali\\Downloads\\New folder (2)\\Common Core\\01_Grade_1_IM_K5\\unit-1\\lesson-N` were read without modifying, copying or pushing publisher files. Each T001 record in the frozen first-ten manifest matches the corresponding Grade 1 Unit 1 lesson title and publisher-source URL. The descriptions below are **paraphrases of the original lesson purposes**, not copied lesson content.

| Lesson | Original instructional purpose | Edulytics exact family (prefix `usccss.t001.g1.` for L02–L10) | Findings / boundaries |
|---|---|---|---|
| [L01](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html) — Count and Add | Count objects and link totals to addition | `usccss.g1.u01.l01.count_collection_to20; usccss.g1.u01.l01.add_dot_cubes_to10` | Previously certified pilot; retain unchanged |
| [L02](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html) — Explore Expressions and Sums | Write sums represented by dot images, within 10 | `dot_pair_sum` | Check source image-to-expression and numeric sum |
| [L03](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-3/preparation.html) — Add 1 or 2 | Add one or two within 10 by counting on | `add_one_or_two` | One- and two-step addition only |
| [L04](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-4/preparation.html) — More Work with 1 and 2 | Subtract one or two within 10 by counting back | `subtract_one_or_two` | Prior original-pack addition-only worked examples are replaced in effective Rich V2 canonical body |
| [L05](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-5/preparation.html) — Explore Addition and Subtraction PLC Activity | Practice both addition and subtraction within 10 | `mixed_add_subtract` | Source-guarded correction restores addition and subtraction teaching |
| [L06](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-6/preparation.html) — Center Day 1 | Fluency games: find partner making 10, plus add/subtract centers | `make_ten_missing_part; mixed_add_subtract` | Two exact practice types and source-guarded make-ten worked examples |
| [L07](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-7/preparation.html) — Sort Math Tools | Sort math tools into named categories and report counts | `sort_math_tools` | Repaired learner explanation and mixed unsorted input; oral explanation not auto-marked |
| [L08](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-8/preparation.html) — Sort and Count Shape Cards PLC Activity | Sort shape cards into three categories and represent counts | `sort_shape_cards` | Repaired examples plus category chart; written table drawing not independently auto-marked |
| [L09](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-9/preparation.html) — What is Your Favorite ______________ ? | Collect a class survey, organize, represent and interpret category data | `class_survey_categories` | Repaired tally/graph teaching; generated practice simulates votes rather than polling a live class |
| [L10](https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-10/preparation.html) — Center Day 2 | Add and subtract within 10 through center games, including missing part | `make_ten_missing_part; mixed_add_subtract` | Repaired source-focused center examples and both assessed operations |

**Exact lesson outcomes:** those in `artifacts/academic-us-ccss/batches/T001-lesson-manifest.json`. Lesson IDs and URLs are frozen in that manifest. Family names above are shorthand unless already fully qualified.

## Independently captured local original HTML file hashes

The following **raw-file SHA-256** digests were calculated read-only from the original source library (not the manifest's separate *semantic* SHA-256). They identify the precise original `preparation.html` and `lesson.html` editions used for the goal review, without checking in the protected publisher HTML or figures.

| Lesson | `preparation.html` raw SHA-256 | `lesson.html` raw SHA-256 |
|---|---|---|
| L01 | `c2e9ac8dc1f5c5ecccb0c4207edfe14885cbfdca1e7d1004dd6cc6bcf9a53aad` | `d8964d1647d363fb56283c96c141cb616a388a54d2281d2a93cf500a75b6a214` |
| L02 | `e1f8cffb9a84048ac36077c72c54c2aec5d142257600263877a1e124ddf589aa` | `74008e82fc3b153b2efb624efcec5a253e323537e5d213d356fc9f74546ad74d` |
| L03 | `3dc044029307286bedb7e984f71d6f34fc677ad51c013718ea84fce978c033ce` | `f3ece3ae90f9f4d261bc21bd4da5da1fef66393b7ab62ff96eed051248608e53` |
| L04 | `4bfc2ebbabc8039814e8a63991eed3bbaba81fe93567783bb817630a49caf46f` | `b497a6b9a60d2e51e644936a6abddde2675c3b9e29a0d8c23f33cae7795e2bbb` |
| L05 | `f896b11834224b9cddf37bd711ad1978e46825f66934c883b5aefe4246843565` | `f854ae45ef242c78a01b882f24b7e4df9a2d2e7755381fb3fa5b0ecb21f74728` |
| L06 | `41eb08f0c8e1307a588fb7eddc469d505871a184abc5d02f52c564d29e1e78a7` | `b99a6d7cd3896cef240305687fa005f7cfd91048ea3711733666069e71473256` |
| L07 | `d19aef5b2a3f98ae99cbb42a3c5f2d6f4ab2745202e5ad1c87b7e97a1c1caa8b` | `35cb5f461f47c7d55e8dfe89e5033937c7363eb850f1e1f8207ffc7d8900602d` |
| L08 | `e807abf86a7653c3dbb08cb5f0f58a2a2a5ecfdf1d54238b25b5d6da6e2b0cf1` | `2df9e1013a9384852add035d5b9780fc1f2a2da0f0c968e082cca4c6ad1d40a8` |
| L09 | `7c58bd6fcae60d7918be9d5474d072a2f136e9e3f0e01f7afae7b24dce63aaa7` | `e4b94554f24e672feff988754a86a16164491567e7cc67a380ca0620d865625e` |
| L10 | `b572daf3314eb25cf7836b37b4aecd8175f29b3f2e5c3492da09a089de8c069f` | `5a7f3aca8530a93ac85f5f77f84ca9746842394c38783508300188bbcc689347` |

## Verified engineering evidence (disposable CI, not production)

- Commit `db6787008cbcf98d988a70a9652b8c52c25a4691`, workflow [authenticated academic acceptance](https://github.com/EIAS79/Edulytics/actions/runs/38789535387): **L01–L10** lesson-scoped HTTPS GET, exact practice generation, student response submission, correct-answer marking, and persisted Postgres scores. Two synthetic schools; cross-tenant denial verified. L01 separately captured desktop and mobile Chromium rendering.
- At that same commit, [T001 math checks](https://github.com/EIAS79/Edulytics/actions/runs/38789535559) passed actual .NET question engine tests, including generation and independent grading. On later commits the L06 mixed-family expansion and 20-screenshot authenticated Chromium smoke must **rerun and pass**.
- [Academic T001 gate](https://github.com/EIAS79/Edulytics/actions/runs/38789535559): `BATCH_NO-GO`, `evidenceReady: 1`, `remainingEvidence: 9`. This is an **honest pending-certification state** and must not be bypassed.

## Tests-only software acceptance (no mandatory human reviewer)

The [T001 automated acceptance policy](T001_AUTOMATED_LESSON_ACCEPTANCE.md) replaces a human academic review/sign-off requirement with **the three reproducible lesson/system CI checks on the same full Git SHA**. Fail-closed outcomes come from `tools/us-ccss-t001-test-acceptance.cjs`; no manual PASS entries are added to the historical academic ledger.

1. Verify source-goal mapping and concrete lesson student interactions, including L02 writing an expression, L07 sorting each math-tool card, L08 building a shape chart, and L09 recording anonymous responses and building a matching graph. The original publisher goals remain unchanged.
2. Require independent mathematics/verifier tests, authenticated student Practice submission and persisted scoring, desktop/mobile Chromium interactions, cross-school authorization, regression, and source/commit integrity. An untested or failed category blocks **test acceptance** until a new tested commit passes.
3. `BATCH_TESTS_ACCEPTED` means **technical lesson readiness**. No external or internal academic-specialist signature is required. The existing `BATCH_NO-GO` or `BATCH_CERTIFIED` **academic** ledger represents different historical evidence; never silently convert successful CI into a claim that children demonstrated mastery or a real-world L09 survey was performed. Publication/deployment, if authorized separately, must still preserve attribution, security and code integrity.

**Licensing:** Original publisher preparation pages show a CC BY 4.0 notice, while the publisher name/logo and separately attributed third-party imagery can carry separate restrictions. Edulytics uses first-party authored task text/visuals; keep required attribution visible, and do not upload the raw source HTML, logos or third-party diagrams from Our-CS to GitHub.
