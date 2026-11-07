# Grade 12 / US-CCSS-MATH — binding A–B–C execution rules

Scope: OpenStax Precalculus local ©2017 PDF revision PR-2015-002(03/17)-BW, 12 chapters, 73 Sections. Local PDF is READ ONLY. Exact_Sources is supplementary; exclude AccessIM/v360, Grade11 Algebra2, HS Fourth Courses and UAE Grade12 from this baseline.

Discovery status: earlier main lacked this route, but PR #626 now contains the 73-Section Grade12 blueprint and one Section 1.1 content draft. Check branch HEAD before creating any missing lessons.

Sequence: read first not-ready lesson from full-grade-progress.json, inspect all original source elements and chapter resources, align Stage A then author/repair Stage B then Stage C, write but DO NOT RUN tests, save element evidence/checkpoint, commit lesson closure before moving to the next. Prefetch whole chapter page ranges without marking future lessons complete.

Speed: batch local PDF reading and independent artifact writes. One commit per lesson when truly closed, no repeated sources, full solution builds or CI. Target three minutes only for straightforward repairs; correctness and denominators override elapsed time. Reuse existing math engine, extend narrow family only on verified gap, build new engine only for distinct interaction.

Cool-down: author first-party Edulytics cool-down when original requires unavailable teacher access; never claim it is original. Human evaluation and drawings require teacher-review contract, not automatic-grading claims.

Completion: distinguish source inventory, pedagogic-semantic parity, digitally assessed skills and evidence completeness. Never mark MATCHED without actual equivalent. Missing diagram/rights/skill/atomic item blocks READY_FOR_STAGE_D. Do not add REVIEW complete 2021. No Stage D, automated tests, CI dispatch, merge, Render or production changes during A–B–C.

Academic owner: approves first-party instructional direction and future conditional publication, but CC BY 4.0 attribution and third-party rights remain mandatory; keep copyright credits separate from learner explanation prose.

Current state: 73/73 source section locators indexed; zero source-verified lesson closures, first active U01:L01.

## Unit-based continuous execution and reporting (owner instruction, 2026-11-06)

- Work continuously from the first unclosed lesson, strictly in source order. After **each fully eligible lesson** complete A→B→C→evidence→checkpoint→one commit, then move straight to the next lesson without requesting another user turn.
- **Do not stop to send one report per lesson.** Aggregate progress reports after completing an entire unit (U01 then U02, etc.); if a genuinely unresolved blocking source/rights/runtime gap prevents closure, record it in the active lesson and do not report a later lesson as closed.
- Prefetch/collate the necessary local pages and original exercises for the current unit in a single read operation when possible, reuse existing previously verified source notes, and keep all original books read-only. Do not re-download or re-index what is already anchored to the same source SHA.
- A–B–C deliberately writes tests and contracts **without running tests, CI, E2E or Stage D**. No blanket builds, full sync, or repeated checks without a distinct technical cause. Use [skip ci] where possible; verify GitHub trigger rules before updating branch.
- First-party Cool-down derived from a verified learning objective is allowed where an original instructor-only assessment is inaccessible; label it clearly as first-party and assign teacher review for diagrams, explanations and proofs.
- Measure actual per-lesson start/close time if available; three minutes is a performance target for straightforward repairs, not permission to skip source element matching or falsify closure.
- Report each completed unit's closed lessons, source gap counts, reused/extended engines, evidence URLs, commit SHA(s), and next sequential lesson, without calling Stage D ready unless every exit criterion is met.

Draft course now authored: PRECALC/Advanced Precalculus selector at American Grade 12 logical Level 13 only, V2 source identity inventory of 73 lessons. This does not close any lesson or verify authenticated runtime. Continue *U01:L01* from full-grade-progress.json until 100% source, semantic, assessment, rights and evidence completeness are proved. Do not misinterpret 92 classroom score proposals as 92 delivered digital questions.

**Grade placement invariant:** US-CCSS-MATH Grade12 = logical level 13; never reuse UAE Grade12 logical12. PRECALC is offered only at logical13. Grade11 uses logical12.

## Stage A–B–C gate classification (prevent duplicated work)

- **Lesson gate:** Verify only the current Section's original elements, first-party content, skill/assessment contracts, accessible representations and rights. Chapter Review / Review Exercises / Practice Test remain **unit-owned shared resources**; reconcile them at U01 unit closure, and attach relevant cross-references to current lessons. Never demand all 80 review exercises and 35 practice-test questions as a prerequisite for **L01 alone**.
- **Grade-wide gate:** The other 72 Sections' official CCSS mapping is a grade-wide backlog, not an unresolved standard mapping for Section 1.1, whose two formal references were checked already. Do not duplicate checks across every earlier lesson.
- **Stage D boundary:** Do not require **executed** authenticated student/teacher UI or CI tests in A–B–C. Record those as `B_RUNTIME_UNVERIFIED` and `D_PENDING`; however actual missing content, teacher assessment contracts, source parity, and unserved diagrams remain blockers to `READY_FOR_STAGE_D`.
- **Counts:** Distinguish original source examples (15 identified in Section 1.1) from first-party authored examples (now 16, including the new overlapping-ray remediation). Rich V2 currently has 21 first-party table/equation visuals. Student code holds 5 teaching graphs and 20 source-skill exercise graph models, including a separately labelled first-party graph-52 analogue with its own known rule. These are authored teaching models, not an independently certified source figure 1:1 match. Recompute counts from HEAD.
- When updating source/assessment/teacher material, stage **one coherent content/test/evidence/checkpoint change** per actual lesson close. Interim correction commits are permissible only for an independently repaired defect; they never advance the sequential cursor. Stage D tests remain written, not run.


## Section 1.1 Edulytics original Cool-down handoff

- OpenStax Section 1.1 contains exactly 92 numbered exercises. An Edulytics original Cool-down is a **separate item 93**; it is not an original source problem and does not belong in the 92-item denominator.
- Its question is stored in u01-l01-assessment.json and Grade12PrecalculusSection11Exercises.CoolDown; the student view links to the G12 scoped teacher-review route.
- The authored database migration accepts identifiers 1–105: source exercises 1–92; original Edulytics Cool-down 93; twelve first-party Try Its 94–105. The migration is WRITTEN_NOT_APPLIED; authenticated student/teacher persistence remains Stage D.
- Do not claim source Cool-down equivalence or completed E2E until verified.


## Section 1.1 independent Try It delivery

- Original source contains 12 Try It skill checks; independent Edulytics first-party replacements are saved under u01-l01-tryit-first-party-teacher-review.json.
- All twelve prompts are now authored in the Section 1.1 student lesson view and routed by unique teacher-submission identifiers 94 through 105. These identifiers are NOT original numbered Section exercises.
- Student/teacher authorization, PostgreSQL persistence, rubric feedback and accessible rendering remain WRITTEN_NOT_RUN or STAGE_D_NOT_VERIFIED, not a claim of production success.


## Efficient local source prefetch (no external search or sync)

- Use tools/curriculum/prepare_g12_openstax_unit_source_cache.py --unit 1 on the authorized Windows device. It extracts local PDF pages for the seven sequential Unit 1 lessons together, validates the exact adopted 2017 source SHA-256, and caches them outside the repository under the local temp directory.
- Re-run the same command to reuse the hash-stable cache without opening the entire PDF again. Later use --unit 2 through --unit 12, as and when work reaches those chapters. The cache is not checked into GitHub.
- Cached original text is an inspection aid; equations, embedded diagrams and page-window semantics still require visual checks where a source ambiguity exists.
- Strictly preserve first-incomplete ordering: Unit 1 remains U01:L01 until the academic A–B–C parity and assessment gate is actually met; never advance source review based on cache existence alone.
- Targeted isolated C# compilation for the renderer and exact question family passed; the full student/teacher runtime, migrations, CI, Stage D and authenticated tests remain outside A–B–C.

## Continuous execution mode — same workflow as previously completed grades

**Mode:** CONTINUOUS_UNIT_CHAIN, not ONE_REPLY_PER_STEP and not ONE_REPLY_PER_LESSON.

1. Resume at the first not-closed lesson recorded in `full-grade-progress.json`. Inspect the currently authored lesson before creating anything new. Use the local OpenStax 2017 source cache and already-verified evidence. Avoid re-reading identical source pages and avoid exploratory redevelopment.
2. For the active lesson, complete Stage A → Stage B → Stage C as a single coherent correction pass. Reuse existing engines, add only verified missing capabilities, and write targeted tests without executing Stage D/CI.
3. Close the lesson only when all A–B–C source/content/assessment/evidence gates are actually met. Commit the complete checkpoint once, then immediately move to the next ordinal without requesting user approval or writing a per-lesson final report.
4. Complete all lessons of the current unit before writing a consolidated unit report. After the unit report, continue automatically with the next unit. The requested end state is all 73 sections processed sequentially.
5. Intermediate GitHub commits, tool completions, source discoveries, and successful partial corrections are **not** session-completion events. Avoid premature final answers and repeated verbose progress narration; continue tool execution while the request is actionable in the current turn.
6. Genuine unresolved source, rights, or content gaps stay explicitly `A_C_EXPLICIT_GAP`; they must not be cosmetically marked `READY_FOR_STAGE_D`, and the sequential close cursor must not jump over them. Record an actionable blocker with precise artifact and continue independent repairs within the same lesson.
7. Batch independent reads and coherent writes, prefer one lesson-close commit, and measure start/close timestamps. Three minutes per straightforward repair is an optimization target, not permission to fabricate source parity.
8. Do not claim automated or background continuation after the assistant response, and never claim unit/grade completion without recorded closure evidence. If execution genuinely cannot continue within this turn, report the specific verified obstacle and last completed operation only, not a routine step-by-step report.

**Authority:** This changes execution and reporting cadence, **not** academic standards or acceptance thresholds. Stage D still owns runtime testing, CI, authenticated E2E, deployment, and publishing; A–B–C owns source alignment, teaching content, practice contracts, and written tests.