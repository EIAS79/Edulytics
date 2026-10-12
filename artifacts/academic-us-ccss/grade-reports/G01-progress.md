# Grade 1 (G1) — whole-grade review: work-in-progress evidence

> **NOT A FINAL ACADEMIC CERTIFICATION REPORT.** This is the first execution output for the standalone whole-grade plan. All 146 source lesson codes are enumerated **without subdividing Grade 1 into batches**. Original embedded source is not necessarily the effective learner-rendered body.

- Baseline Git SHA: `2f28a83d9f747030c219cb38ecdc3076dca286e6`. Source pack blob: `25cc1603a15fb5ceb862100c44ed1368f957b235`.
- Source inventory: **146 unique Grade 1 lessons**; automated original-source text screen: **146/146**.
- Full eight-dimension review with sufficient evidence: **0/146 confirmed**; independent academic L7 approvals: **0/146**; **G1 remains NO-GO**.
- Previously recorded **3 Grade 1 correction candidates** are linked, NOT counted as confirmed effective runtime or independent signoff.
- Source-level rework candidates with direct observable issues: **3**; additional **143** blocked for missing full review evidence. A screening flag is **not** proof that the active lesson is incorrect.

## Source screening signals (may overlap)

| Flag in original pack | Source lessons | Meaning |
|---|---:|---|
| Quoted teacher prompt as worked example | 28 | Inspect whether it is an actual solved learner example; quoted prompt alone is not conclusive |
| Generic first step in solution | 39 | Verify the lesson's specific method and worked mathematical steps |
| Literal HTML/SVG fragment in worked example | 1 | Check if unsafe scraped markup appears in learner body |
| Subtract 1/2 lesson includes addition-only examples | 1 | Definite mismatch in embedded G1:U01:L04 source text |
| Negative arithmetic in compare story | 1 | Definite inappropriate G1:U06:L17 example in original pack |
| Earlier corrected negative example still in original pack | 2 | Do not re-fix until effective materialized body checked |
| Earlier corrected data mismatch still in original pack | 1 | Do not re-fix until effective materialized body checked |

## Guarded correction implementation after the initial source screen

Three direct original-source mismatches now have **Edulytics-original, source- and stored-body-SHA-guarded learner-content corrections** in the canonical materializer:

- `G1:U01:L04` — subtract 1 or 2 with fully worked count-back and ten-frame explanations (instead of addition-only examples).
- `G1:U01:L10` — teach add/subtract within 10 using a ten-frame and related facts (instead of quoted teacher reflection and raw SVG `<path>` markup).
- `G1:U06:L17` — use puppy/tulip comparison problems with valid non-negative whole-number answers.

**Evidence:** [G01-first-three-content-corrections.json](G01-first-three-content-corrections.json) and `tests/academic-us-ccss-g1-source-guarded-corrections.test.cjs`. This is an **engineering correction candidate**, NOT an independent mathematics approval, not authenticated Rich V2 browser verification, and not a production update. The raw source signals in the table below deliberately remain unchanged because the original publisher-derived pack is immutable and materialization supplies corrected student-facing text.

## Seven additional verified source-topic corrections (engineering stage)

The Grade 1 **same whole-grade review** has produced seven more targeted Edulytics-original corrections, as recorded in [G01-seven-topic-mismatch-corrections.json](G01-seven-topic-mismatch-corrections.json):

- `G1:U04:L21`: compare 2-digit base-ten representations, including regrouped tens and ones, instead of only solving introductory addition.
- `G1:U06:L02`: compare object lengths indirectly via the same third object, not the arithmetic warm-up.
- `G1:U06:L05`: measure object length with connecting cubes, not cube and prism face geometry.
- `G1:U07:L02`: construct composite 3D solids from smaller blocks, not merely name solid properties.
- `G1:U07:L14`: clock hands at half past the hour, not number-talk sums.
- `G1:U07:L15`: translate hour and half-hour clock faces into digital notation, not sums.
- `G1:U07:L16`: draw/read challenging hour/half-hour clocks in a daily schedule, not addition within 100.

Publisher purposes and learning goals were checked separately against the corresponding Illustrative Mathematics / Kendall Hunt preparation/lesson references. These are **bounded source and persisted-body SHA-guarded candidate learner corrections**, and they preserve original source packs and official outcome mappings. Their engineering regression, staging and licensing/human L1-L7 gates remain required; neither the number of corrections nor a green PR test counts as an academic certificate.

## Six more locally cross-checked source-grounded instructional corrections

The authorized local Grade 1 Illustrative Mathematics/Kendall Hunt teacher preparation HTML files were read in **read-only mode** at `C:\Users\khali\Downloads\New folder (2)\Common Core\01_Grade_1_IM_K5\Exact_Sources`. Six relevant files were matched against lesson identities, learning goals and source SHA-256 hashes; exact local file fingerprints and original content guards are recorded in [G01-six-local-source-verified-corrections.json](G01-six-local-source-verified-corrections.json).

- `G1:U01:L07` — categorize pattern blocks, counters and inch tiles and report actual frequencies.
- `G1:U01:L11` — interpret class-pet tallies, verify true/false data statements.
- `G1:U06:L03` — **choose** a common third object and compare lengths indirectly, rather than merely count identical length units.
- `G1:U07:L13` — read/write analog clock times to whole hours, rather than linear measurement steps.
- `G1:U07:L04` — draw flat polygons using dot paper and identify defining properties.
- `G1:U07:L07` — **compose** larger 2D shapes from smaller pattern blocks.

These authored repairs are guarded by exact lesson codes, canonical SHA, original English body SHA, outcome codes and source URLs. **No local files were modified.** The linked HTML pages are teacher-preparation goals rather than complete independent proof of student visual delivery, question correctness, licensing rights or L7 signoff. All six remain **not academically certified** until the L1–L7 grade plan is fulfilled.

## Required next actions, still **within Grade 1**

1. Compare original raw source with the actual content materialized into Rich V2, especially prior repairs U01:L13, U01:L15 and U04:L03; never label the old raw defect as a live defect without verification.
2. Validate the now-authored guarded corrections for U01:L04, U01:L10 and U06:L17 with scoped CI, persisted-content version checks, and authenticated Rich V2 staging when authorized. Preserve the original source pack and do not count these corrections as academic signoffs.
3. For every one of the 146 lessons, complete a source-grounded human/qualified mathematics review of the eight dimensions; establish actual question-family/assessment/grading, drawings and Rich V2 behavior; resolve L1–L6 evidence gates.
4. Obtain real independent academic L7 approval for each lesson. Then generate **one final Grade 1 report covering all 146 lessons**. Do **not** move to Grade 2 or issue a false GO.

## All 146 lessons — one whole-grade work register

Status labels: **REWORK** = direct source defect before checking active materialization; **BLOCKED** = screening only / missing proof of full content, questions, visuals, delivery or human review. Each row has the eight dimension observations in the associated machine-readable evidence file. Neither label constitutes academic certification.

| # | LessonCode | Title | Source outcome(s) | Source signal(s) | Editorial triage |
|---:|---|---|---|---|---|
| 1 | `G1:U01:L01` | Count and Add | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 2 | `G1:U01:L02` | Explore Expressions and Sums | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 3 | `G1:U01:L03` | Add 1 or 2 | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 4 | `G1:U01:L04` | More Work with 1 and 2 | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SUBTRACTION_LESSON_USES_ADDITION_ONLY_EXAMPLES | REWORK (raw source) |
| 5 | `G1:U01:L05` | Explore Addition and Subtraction PLC Activity | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 6 | `G1:U01:L06` | Center Day 1 | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 7 | `G1:U01:L07` | Sort Math Tools | CCSS:1.MD.C.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 8 | `G1:U01:L08` | Sort and Count Shape Cards PLC Activity | CCSS:1.MD.C.4 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 9 | `G1:U01:L09` | What is Your Favorite ______________ ? | CCSS:1.MD.C.4, CCSS:1.OA.C.5 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 10 | `G1:U01:L10` | Center Day 2 | CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE; RAW_HTML_SVG_FRAGMENT | REWORK (raw source) |
| 11 | `G1:U01:L11` | Class Pet Surveys | CCSS:1.MD.C.4, CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 12 | `G1:U01:L12` | How Many? PLC Activity | CCSS:1.MD.C.4 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 13 | `G1:U01:L13` | Questions About Data | CCSS:1.MD.C.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | DATA_LESSON_GENERIC_ARITHMETIC_PRIOR_REPAIR_RECORDED; PRIOR_GUARDED_REPAIR_TO_VERIFY | BLOCKED |
| 14 | `G1:U01:L14` | Center Day 3 | CCSS:1.MD.C.4, CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 15 | `G1:U01:L15` | Animals in the Jungle (optional) | CCSS:1.MD.C.4 | OUT_OF_GRADE_NEGATIVE_RESULT_IN_SOURCE_PRIOR_REPAIR_RECORDED; PRIOR_GUARDED_REPAIR_TO_VERIFY | BLOCKED |
| 16 | `G1:U02:L01` | Story Problems and Expressions | CCSS:1.OA.A.1 | No machine flag; semantic proof still missing | BLOCKED |
| 17 | `G1:U02:L02` | Story Problems and Equations | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 18 | `G1:U02:L03` | A Change is Coming PLC Activity | CCSS:1.MD.C.4, CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 19 | `G1:U02:L04` | Result or Change Unknown | CCSS:1.OA.A.1, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 20 | `G1:U02:L05` | Center Day 1 | CCSS:1.MD.C.4, CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 21 | `G1:U02:L06` | Problems about Pets | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 22 | `G1:U02:L07` | Shake and Spill | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 23 | `G1:U02:L08` | Shake, Spill, and Cover | CCSS:1.NBT.A.1, CCSS:1.OA.A.1, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 24 | `G1:U02:L09` | Compare Story Problems PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.B.3, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 25 | `G1:U02:L10` | Center Day 2 | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 26 | `G1:U02:L11` | Make Them the Same | CCSS:1.OA.A.1 | No machine flag; semantic proof still missing | BLOCKED |
| 27 | `G1:U02:L12` | School Supplies | CCSS:1.OA.A.1, CCSS:1.OA.C.5 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 28 | `G1:U02:L13` | Compare Favorite Art Supply Data | CCSS:1.MD.C.4, CCSS:1.OA.A.1 | No machine flag; semantic proof still missing | BLOCKED |
| 29 | `G1:U02:L14` | Compare with Addition and Subtraction PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 30 | `G1:U02:L15` | Different Types of Story Problems | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 31 | `G1:U02:L16` | Center Day 3 | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 32 | `G1:U02:L17` | How Do the Stories Compare? | CCSS:1.OA.A.1, CCSS:1.OA.C.6, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 33 | `G1:U02:L18` | Equations with Unknowns | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 34 | `G1:U02:L19` | Story Problems and Equations PLC Activity | CCSS:1.NBT.A.1, CCSS:1.OA.A.1, CCSS:1.OA.B.4 | No machine flag; semantic proof still missing | BLOCKED |
| 35 | `G1:U02:L20` | What’s the Story? | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 36 | `G1:U02:L21` | Center Day 4 | CCSS:1.OA.A.1, CCSS:1.OA.A.2, CCSS:1.OA.B.4, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 37 | `G1:U02:L22` | Story Problems and Equations (optional) | CCSS:1.OA.B.4 | No machine flag; semantic proof still missing | BLOCKED |
| 38 | `G1:U03:L01` | Sums I Know | CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 39 | `G1:U03:L02` | Relate Counting to Addition | CCSS:1.OA.A.1, CCSS:1.OA.B.3, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 40 | `G1:U03:L03` | Are the Expressions Equal? | CCSS:1.OA.B.3, CCSS:1.OA.C.6, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 41 | `G1:U03:L04` | Sums of 10 | CCSS:1.OA.B.3, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.7, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 42 | `G1:U03:L05` | Find the Difference | CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 43 | `G1:U03:L06` | Story Problems within 10 PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 44 | `G1:U03:L07` | Center Day 1 | CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 45 | `G1:U03:L08` | Ten as a Unit | CCSS:1.NBT.B.2 | No machine flag; semantic proof still missing | BLOCKED |
| 46 | `G1:U03:L09` | Addition With a Ten | CCSS:1.NBT.B.2, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 47 | `G1:U03:L10` | Addition and Subtraction with a Ten | CCSS:1.NBT.B.2, CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 48 | `G1:U03:L11` | Add to a Teen Number | CCSS:1.OA.A.1, CCSS:1.OA.C.6, CCSS:1.OA.D.7, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 49 | `G1:U03:L12` | Subtract From a Teen Number | CCSS:1.NBT.A.1, CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 50 | `G1:U03:L13` | More Story Problems with Teen Numbers PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 51 | `G1:U03:L14` | Center Day 2 | CCSS:1.OA.C.6, CCSS:1.OA.D.7, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 52 | `G1:U03:L15` | Solve Story Problems with Three Numbers | CCSS:1.OA.A.2, CCSS:1.OA.B.3, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 53 | `G1:U03:L16` | Add Three Numbers | CCSS:1.OA.B.3, CCSS:1.OA.C.6, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 54 | `G1:U03:L17` | Make 10 to Add PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.B.3, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 55 | `G1:U03:L18` | Patterns in Addition | CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 56 | `G1:U03:L19` | Methods for Addition Within 20 | CCSS:1.OA.B.3, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 57 | `G1:U03:L20` | A Trip to the Zoo | CCSS:1.OA.A.2, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 58 | `G1:U03:L21` | Center Day 3 | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 59 | `G1:U03:L22` | Subtract from Teen Numbers | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 60 | `G1:U03:L23` | Use a Ten to Subtract | CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 61 | `G1:U03:L24` | Relate Counting to Addition and Subtraction PLC Activity | CCSS:1.NBT.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 62 | `G1:U03:L25` | How Do You Want to Subtract? | CCSS:1.NBT.A.1, CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 63 | `G1:U03:L26` | What’s the Story? | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 64 | `G1:U03:L27` | Center Day 4 | CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 65 | `G1:U03:L28` | Around the Room (optional) | CCSS:1.OA.A.1, CCSS:1.OA.A.2, CCSS:1.OA.B.3, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 66 | `G1:U04:L01` | Count Large Collections | CCSS:1.NBT.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 67 | `G1:U04:L02` | Match Representations of Tens | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 68 | `G1:U04:L03` | Addition and Subtraction with Tens | CCSS:1.NBT.B.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.NBT.C.6, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | OUT_OF_GRADE_NEGATIVE_RESULT_IN_SOURCE_PRIOR_REPAIR_RECORDED; PRIOR_GUARDED_REPAIR_TO_VERIFY | BLOCKED |
| 69 | `G1:U04:L04` | More Addition and Subtraction with Tens PLC Activity | CCSS:1.NBT.B.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.NBT.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 70 | `G1:U04:L05` | Center Day 1 | CCSS:1.NBT.B.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.6, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 71 | `G1:U04:L06` | Count Larger Collections | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.NBT.C.6, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 72 | `G1:U04:L07` | Numbers With Tens and Ones | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.6, CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 73 | `G1:U04:L08` | Different Representations of Tens and Ones PLC Activity | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2 | No machine flag; semantic proof still missing | BLOCKED |
| 74 | `G1:U04:L09` | Show Me Your Number | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.6, CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 75 | `G1:U04:L10` | Write Two-digit Numbers | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2 | No machine flag; semantic proof still missing | BLOCKED |
| 76 | `G1:U04:L11` | Add Tens to Two-digit Numbers | CCSS:1.NBT.B.2, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 77 | `G1:U04:L12` | Mentally Add and Subtract Tens (optional) | CCSS:1.NBT.A.1, CCSS:1.NBT.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 78 | `G1:U04:L13` | Center Day 2 | CCSS:1.NBT.A.1, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.NBT.C.6, CCSS:1.OA.A.1, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 79 | `G1:U04:L14` | Let’s Compare | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.B.3, CCSS:1.NBT.C.5, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 80 | `G1:U04:L15` | Greater Than, Less Than | CCSS:1.NBT.B.3, CCSS:1.NBT.C.5, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 81 | `G1:U04:L16` | Write Comparisons with Symbols PLC Activity | CCSS:1.NBT.B.2, CCSS:1.NBT.B.3 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 82 | `G1:U04:L17` | Compare and Order Numbers | CCSS:1.NBT.B.2, CCSS:1.NBT.B.3 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 83 | `G1:U04:L18` | Center Day 3 | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.B.3, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.NBT.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 84 | `G1:U04:L19` | Make Two-digit Numbers | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.B.3 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 85 | `G1:U04:L20` | Make Two-Digit Numbers in Different Ways PLC Activity | CCSS:1.NBT.B.2 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 86 | `G1:U04:L21` | Compare Two-Digit Numbers Shown in Different Ways | CCSS:1.NBT.B.2, CCSS:1.NBT.B.3, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 87 | `G1:U04:L22` | Center Day 4 | CCSS:1.NBT.B.2, CCSS:1.NBT.B.3 | No machine flag; semantic proof still missing | BLOCKED |
| 88 | `G1:U04:L23` | Two-Digit Numbers in Our World (optional) | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.B.3 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 89 | `G1:U05:L01` | Add Tens or Ones | CCSS:1.NBT.A.1, CCSS:1.NBT.B.2, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 90 | `G1:U05:L02` | How Did You Add? PLC Activity | CCSS:1.NBT.A.1, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 91 | `G1:U05:L03` | Add It, Explain It | CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 92 | `G1:U05:L04` | Center Day 1 | CCSS:1.NBT.A.1, CCSS:1.NBT.C.4, CCSS:1.OA.A.1, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 93 | `G1:U05:L05` | Make a Ten | CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 94 | `G1:U05:L06` | Make a Ten and Make Sense of Equations PLC Activity | CCSS:1.NBT.C.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 95 | `G1:U05:L07` | Does it Make a New Ten? | CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 96 | `G1:U05:L08` | Center Day 2 | CCSS:1.NBT.C.4, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 97 | `G1:U05:L09` | Add 2 Two-digit Numbers | CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 98 | `G1:U05:L10` | Tens and Tens, Ones and Ones | CCSS:1.NBT.C.4, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 99 | `G1:U05:L11` | How Did You Do That? | CCSS:1.NBT.C.4, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 100 | `G1:U05:L12` | Add it Up PLC Activity | CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 101 | `G1:U05:L13` | Center Day 3 | CCSS:1.NBT.A.1, CCSS:1.NBT.B.3, CCSS:1.NBT.C.4, CCSS:1.OA.C.5, CCSS:1.OA.C.6, CCSS:1.OA.D.8 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 102 | `G1:U05:L14` | Food Drive (optional) | CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.NBT.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 103 | `G1:U06:L01` | Compare Lengths | CCSS:1.MD.A.1, CCSS:1.NBT.B.3, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 104 | `G1:U06:L02` | Compare the Length of Objects Indirectly PLC Activity | CCSS:1.MD.A.1, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 105 | `G1:U06:L03` | Choose Objects to Compare Length Indirectly | CCSS:1.MD.A.1 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE | BLOCKED |
| 106 | `G1:U06:L04` | Center Day 1 | CCSS:1.NBT.B.3, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 107 | `G1:U06:L05` | Measure with Connecting Cubes | CCSS:1.MD.A.1, CCSS:1.MD.A.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 108 | `G1:U06:L06` | Measure with Paper Clips | CCSS:1.MD.A.2 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE | BLOCKED |
| 109 | `G1:U06:L07` | Measure Length with Different Length Units | CCSS:1.MD.A.2 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE | BLOCKED |
| 110 | `G1:U06:L08` | Groups Up to 110 PLC Activity | CCSS:1.MD.A.2, CCSS:1.NBT.A.1 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE | BLOCKED |
| 111 | `G1:U06:L09` | Write Numbers to 120 | CCSS:1.MD.A.2, CCSS:1.NBT.A.1 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE | BLOCKED |
| 112 | `G1:U06:L10` | Center Day 2 | CCSS:1.MD.A.2, CCSS:1.NBT.C.4, CCSS:1.NBT.C.5, CCSS:1.OA.C.5, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 113 | `G1:U06:L11` | How Long Are Our Shoes? | CCSS:1.MD.A.2, CCSS:1.OA.A.1, CCSS:1.OA.A.2, CCSS:1.OA.B.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 114 | `G1:U06:L12` | Compare Measurements | CCSS:1.NBT.A.1, CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 115 | `G1:U06:L13` | Solve Take From Story Problems | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 116 | `G1:U06:L14` | Which Equation Matches? PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.B.4, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 117 | `G1:U06:L15` | Write Equations for Story Problems | CCSS:1.NBT.A.1, CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 118 | `G1:U06:L16` | Center Day 3 | CCSS:1.MD.A.2, CCSS:1.NBT.A.1, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 119 | `G1:U06:L17` | Puppies and Tulips (optional) | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | OUT_OF_GRADE_NEGATIVE_RESULT_IN_STORY_EXAMPLE | REWORK (raw source) |
| 120 | `G1:U07:L01` | Shapes That Are Solid | CCSS:MP.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 121 | `G1:U07:L02` | Build Shapes from Other Shapes | CCSS:1.G.A.2 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 122 | `G1:U07:L03` | Shapes That Are Flat | CCSS:MP.3, CCSS:MP.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 123 | `G1:U07:L04` | Draw Flat Shapes PLC Activity | CCSS:1.G.A.1, CCSS:1.NBT.C.4, CCSS:1.OA.C.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 124 | `G1:U07:L05` | Some Triangles, All Triangles | CCSS:1.G.A.1, CCSS:1.NBT.C.4, CCSS:1.OA.C.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 125 | `G1:U07:L06` | Rectangles and Squares | CCSS:1.G.A.1, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 126 | `G1:U07:L07` | Put Together Flat Shapes | CCSS:1.G.A.2 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 127 | `G1:U07:L08` | Center Day 1 | CCSS:1.G.A.1, CCSS:1.G.A.2, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 128 | `G1:U07:L09` | Equal Pieces | CCSS:1.G.A.3 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 129 | `G1:U07:L10` | One of the Pieces, All of the Pieces | CCSS:1.G.A.3 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 130 | `G1:U07:L11` | A Bigger Piece PLC Activity | CCSS:1.G.A.3, CCSS:1.NBT.C.5 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 131 | `G1:U07:L12` | Center Day 2 | CCSS:1.G.A.1, CCSS:1.G.A.2, CCSS:1.NBT.C.4, CCSS:1.OA.C.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 132 | `G1:U07:L13` | It’s Time to Learn About Clocks | CCSS:1.MD.B.3 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE | BLOCKED |
| 133 | `G1:U07:L14` | Half of the Clock PLC Activity | CCSS:1.MD.B.3, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 134 | `G1:U07:L15` | Write Times | CCSS:1.MD.B.3, CCSS:1.NBT.A.1, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 135 | `G1:U07:L16` | Hard Times | CCSS:1.MD.B.3, CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |
| 136 | `G1:U07:L17` | Center Day 3 | CCSS:1.G.A.2, CCSS:1.MD.B.3, CCSS:1.NBT.C.4, CCSS:1.OA.C.6 | GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 137 | `G1:U08:L01` | Addition Fluency Within 10 | CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 138 | `G1:U08:L02` | Relate Addition and Subtraction | CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 139 | `G1:U08:L03` | Add and Subtract Within 20 PLC Activity | CCSS:1.OA.C.6, CCSS:1.OA.D.8 | No machine flag; semantic proof still missing | BLOCKED |
| 140 | `G1:U08:L04` | Change Unknown Story Problems PLC Activity | CCSS:1.OA.A.1, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 141 | `G1:U08:L05` | Put Together and Take Apart Story Problems | CCSS:1.NBT.C.4, CCSS:1.OA.A.1, CCSS:1.OA.C.6, CCSS:1.OA.D.7 | No machine flag; semantic proof still missing | BLOCKED |
| 142 | `G1:U08:L06` | Compare Story Problems | CCSS:1.OA.A.1, CCSS:1.OA.A.2, CCSS:1.OA.C.6 | No machine flag; semantic proof still missing | BLOCKED |
| 143 | `G1:U08:L07` | Count Large Collections | CCSS:1.NBT.A.1 | No machine flag; semantic proof still missing | BLOCKED |
| 144 | `G1:U08:L08` | Show Me All the Ways PLC Activity | CCSS:1.NBT.A.1, CCSS:1.OA.C.6 | SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE; GENERIC_SOLUTION_TEMPLATE | BLOCKED |
| 145 | `G1:U08:L09` | Solve Number Riddles | CCSS:1.NBT.B.3 | No machine flag; semantic proof still missing | BLOCKED |
| 146 | `G1:U08:L10` | Write Number Riddles | CCSS:1.NBT.C.4 | No machine flag; semantic proof still missing | BLOCKED |

## Evidence and constraints

- The per-lesson eight-axis source screen is in [G01-source-screening.json](G01-source-screening.json). It stores canonical lesson identifiers, source hashes, original excerpts, flags, prior repair links and clearly unverified visual/question axes.
- The official L1–L7 certification ledger remains `artifacts/academic-us-ccss/04-lesson-certification-records.json`; this screening **does not** write fabricated statuses or signatures there.
- Review dimension states mean **source text observed**, NOT academic PASS. A learner-rendered visual, verified solver, teacher rubric, live staging interaction, license signoff or reviewer signature has **not** been inferred.
- This report is an execution checkpoint only. The **final grade report** is withheld until all 146 lessons have L1–L7 independent evidence.
