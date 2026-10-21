
# Edulytics — 20-Lesson Curriculum Execution Plan V2

**Plan ID:** `EDULYTICS-US-CCSS-20-V2` (replaces the prior `EDULYTICS-US-CCSS-500-V2` policy ID for future batch planning; preserve legacy artifact IDs as history)  
**Effective:** 2026-10-17 owner-approved amendment; V2 originally adopted on 2026-10-16  
**Status:** **SUPERSEDED / INACTIVE** by owner direction on 2026-10-17. The original V1 grade-first five-lesson plan is restored as the active curriculum execution policy. All other V2 text below is retained only as historical policy and evidence context; V2's 20-lesson release/acceptance procedure must not be started for future work  
**Repository:** `EIAS79/Edulytics`  
**Programme:** `US-CCSS-MATH`  
**Recorded scope:** 1,560 unique lessons, excluding Kindergarten, from `docs/curriculum/us-ccss-g1-g12-structural-audit.json` (2026-10-11 snapshot)  
**Requested work unit:** **20 lessons** per batch, **78 ordered batches** `B001`–`B078` (1,560 ÷ 20 = 78).  
**Compatibility:** This existing file path retains `500_LESSON` in its filename so prior links do not break; the **document body** is the authoritative 20-lesson policy. The prior 500-lesson B001 intake, technical evidence, branches and CI runners remain historical 500-scope records until a separate, verified migration. No previously recorded test, academic acceptance, release or deployment is implied by this policy amendment.

**Scope of the owner's change:** Reduce only the **work/release/acceptance unit** from 500 to 20. Retain the original V2 phases P0–P7, per-lesson academic evidence, reuse-first engineering rules, existing security protections and honest P4/P5 separation. Do not silently lower quality checks, automatically mark a lesson passed, or assume the existing 500-scope CI gate now enforces 20.


## 1. Governing decision

1. Replace the original five-lesson chunks and the interim 500-lesson V2 batches with **20-lesson operational batches**: `B001` through `B078`, each containing exactly 20 real lessons in canonical order.
2. Preserve **curriculum / grade / unit / lesson ordering** across all 1,560 lessons. A batch may cross a grade/course boundary; never reorder, duplicate, omit or invent a lesson. Keep actual boundaries visible for accurate source reconciliation, capability testing, reporting and navigation.
3. Retain each lesson's canonical `LessonCode`. Derive the first 20 and each following set of 20 from the reconciled, ordered inventory, not by guessing lesson IDs or units.
4. **Reuse first:** inspect real student-visible lesson content, outcomes, current Practice routing, shared mathematics capabilities and assessments. Repair demonstrated defects; do not regenerate passing lessons or build one generator per lesson.
5. The **aggregate intake, P3 work and P5 acceptance unit is 20**, while the programme remains all 1,560 lessons. Internal engineering may use grade checkpoints, reviewable PRs and resumable test shards; none can claim an accepted 20-lesson batch without the whole cohort.
6. **Every one of the 20 needs genuine per-lesson academic and technical evidence** before `BATCH_INTERNAL_ACADEMIC_ACCEPTED`; one blocked or unverified lesson blocks that batch. Green CI alone does not establish pedagogical acceptance.
7. A **20-lesson production release** requires full cohort acceptance, the existing GitHub/Render protections and verified live student/Practice workflows including persisted attempts and owner UAT. Working branch commits or documentation-only merges are not a lesson release.
8. Preserve **15 previously shipped Grade 1 lessons** as historical baseline: T001 first ten plus Grade 1 Batch 03 next five (prior release commit `54833b1669fe33f39c0043ac229f7440e1c8da9a`). They occupy the first 15 places of the new B001 and may count only after evidence, exact SHA/content and student-visible drift checks, not as newly completed work.
9. Preserve all previously recorded 500-lesson intake/source scans/PDF audits/P4 technical evidence and open code branches, but **do not transfer a 500-scope PASS or the prior NO_GO into an automatic 20-scope decision**. Re-scope by exact lesson IDs/source and code SHAs; implement or verify the real 20-scope gate separately.
10. Never fabricate academic approval, source licence, independent verification, test PASS, deployment IDs or production evidence.


## 2. Exact sequence by planning snapshot

| Batch | Ordered curriculum positions | Planned total | Existing work / obligations |
|---|---|---:|---|
| **B001** | Grade 1 ordinals **1–20**, spanning actual units according to canonical `LessonCode` order | **20** | First 15 historically released (drift review needed); next 5 not previously released (review, reuse/repair, test and acceptance evidence needed) |
| **B002** | Grade 1 ordinals 21–40 | **20** | Preserve existing source-verified candidate work where matching these exact IDs |
| **B003–B007** | Grade 1 ordinals 41–140 in successive ordered 20-lesson groups | **100** | Per-batch evidence, no automatic acceptance |
| **B008** | Grade 1 ordinals 141–146 (6) + Grade 2 ordinals 1–14 (14) | **20** | Explicit cross-grade boundary |
| **B009–B077** | Successive next 20-lesson slices of the authoritative 1,560-lesson order | **1,380** | Determine grade/course/lesson boundaries from the reconciled inventory; no fabricated subsegments |
| **B078** | Last 20 positions in that order | **20** | Final normal-sized batch; no special 60-lesson exception |
| **Total** | US CCSS Grade 1–8, named high-school courses/supplements in their recorded order | **1,560** | **78 batches × 20**, no omissions or repeats |

This is a **batching policy and count-based preview** until all 78 ordered 20-code manifests are generated and reconciled with the actual database/blueprints and lawful source references. Never treat a grade ordinal as a unit-specific lesson number. Keep the 29 named advanced supplementary lessons in their real courses; do not invent a single grade or course label.

The **former 500+500+500+60 B001–B004 table is retired for future releases**, not erased from Git history. The former `US-CCSS-B001-INTAKE.json` remains a **legacy 500-ID checkpoint** and its status/technical test measurements retain their original scope. Reusing its 500 records for a 20-lesson cohort requires filtering by 20 exact lesson IDs and re-verifying source/code SHA and applicable per-lesson evidence.

British/Cambridge, Polish and UAE curricula remain outside this 1,560-lesson US-CCSS programme.


## 3. B001 intake and fixed checkpoint

**Next operational release candidate:** new `US-CCSS-B001`, positions **1–20 of Grade 1**, identified by exact reconciled canonical codes (do not infer unit identifiers from ordinal positions). First 15 were released under prior policies, but need a fresh *drift and evidence review for this 20-cohort*; Grade 1 position **#16** is the first not previously shipped.

**Required intake outputs before claiming cohort completion:**

- Ordered, immutable manifest of **20 real lesson codes**, grade/unit/title, edition/reference metadata, lawful asset provenance and hash/version of effective student-visible content.
- Per-lesson outcomes and goals, source teacher/student activity review, learner-visible explanation/visuals, exact `SkillContract`, allowed/forbidden Practice families, answer types, independent solver/verifier and effective authenticated Practice route.
- Reconcile against actual blueprint/database/content; no duplicates, missing entries, invented approvals or unreviewed licence issues.
- Classify each lesson as `NOT_REVIEWED`, `READY_REUSE`, `CONTENT_REPAIR`, `PRACTICE_REPAIR`, `NEW_SHARED_CAPABILITY`, `BLOCKED`, `TESTED` or `INTERNALLY_ACCEPTED` only when supported by evidence.
- Separate **15 historical positions** (must pass drift/evidence checks) from **five new positions** (must be genuinely reviewed, repaired where needed and independently tested); preserve all source and code SHAs and avoid counting historical releases as newly implemented.
- Track the next cursor and the real 20/20 acceptance decision; preserve reusable older work and avoid repeated green P4 runs except when materially required.

**New 20-scope intake and P5 artifact:** create `docs/curriculum/batches/US-CCSS-B001-20-INTAKE.json` and its exact 20-code evidence/acceptance records in a separate implementation PR. **This path is proposed, not asserted to exist.** Existing `docs/curriculum/batches/US-CCSS-B001-INTAKE.json` is the **500-lesson historical checkpoint**, not the new 20-batch pass ledger. Any old B001 tests or PR #540 lesson repairs must be mapped by **actual lesson codes and exact candidate SHAs** before being attributed to B001 or B002.

**Current acceptance:** the old 500-cohort P5 decision was `NO_GO`. The new 20-cohort has **not yet passed a verified 20/20 P5 gate** and is **not released**. This policy change is not a test execution, code gate update or Render deployment.

## 4. Track A + Track B for each lesson

**Track A: curriculum fidelity**
1. Check exact `LessonCode`, edition, original pedagogical aim, standards/outcome association and source licence.
2. Inspect **rendered** learner content including introduction, explanation, worked steps, visual representations, accessibility descriptions, common mistakes and summary.
3. Choose exact primary and secondary `SkillContract` mappings with evidence; never map from broad keywords alone.
4. Identify missing, incorrect, over-difficult or unrelated teaching and patch **only** those deficiencies.

**Track B: mathematical correctness and Practice**
1. Reuse existing generator, solver, verifier, answer evaluator, graph/game or interaction when capable.
2. If capabilities are missing, add **shared reusable** families and/or Mathematics Intelligence Kernel primitives; do not write a one-off engine per lesson.
3. Enforce answer types and input bounds, allowed and forbidden families, difficulty profiles and curriculum alignment.
4. Independently verify correct answers and reject incorrect/malformed/forbidden responses; preserve server-side authority, persisted attempts and student isolation.
5. Block unsafe/unverified lessons from auto-generation; never silently substitute a broad unrelated math question.


## 5. Efficient processing without fake speed

1. **Extract source indexes once per grade**, version/hash them and reuse sound prior 500-scope analysis for matching exact lesson IDs rather than discarding it.
2. Partition the overall 1,560 lesson programme into **78 ordered, fixed 20-lesson release cohorts**; use grade-aware internal test shards if appropriate. Smaller internal review slices never constitute a release.
3. Group repeated defects and skills. One shared solver/generator/renderer fix should benefit every affected lesson, with impact-based regression tests on its consumers.
4. Persist checkpoints after each verified 20-lesson work cohort: source version, exact lesson codes, changed files, evidence/run IDs, blockers and next cursor.
5. Preserve passing prior records tied to their original SHAs. Never silently relabel a 500-scope technical run as a 20-lesson academic PASS.
6. Report **20 expected, inventoried, reviewed, reused, modified, tested, academically accepted, blocked and verified live**, all backed by exact lesson identifiers and proof links; also report overall programme progression honestly.
7. Do not promise fixed completion time for any cohort; new math families, asset rights, CI and live UAT can dominate the work.


## 6. Verification gates

For **each** of the 20 lessons in the selected cohort, require:

- Stable canonical code, actual grade/unit/position/title, edition/reference provenance, asset rights and honest outcome mapping.
- Real effective/rendered learner content matching the instructional objective: independent Edulytics-authored explanations, worked examples, expected student activities, relevant representations and correct mathematics.
- Exact `SkillContract` and permitted Practice family routing, without unrelated fallback tasks.
- Appropriate seeded question variation and difficulty, independently solved/verified mathematics, correct/wrong/malformed answer checks and interaction constraints.
- Relevant lesson route, scoring and authenticated persistence tests; UI/keyboard/mobile/accessibility tests for changed interactive components.
- No fabricated sign-off, tenant isolation bypass, corruption of prior grades, historical evidence drift or unauthorized reused external assets.

**Risk-based P4 CI:** preserve existing required repository checks. Rerun tests when source/code changes or evidence invalidation requires it, including broad security/migration regressions when relevant. P4 produces actual test records; P5 only reads and decides from valid per-lesson evidence.

**Batch-level P5 gate:** **`20/20` genuinely accepted in each of B001–B078**. Compute using machine-readable exact 20-code manifests, source/content hashes and the **candidate code SHA**. Any missing or failed per-lesson proof is `BATCH_BLOCKED`, never `BATCH_ACCEPTED`. Historical releases for the first 15 Grade 1 positions count only after their matching content-version, student-visible behavior and SHA drift checks; the new cohort still requires 20 valid acceptance records.

**Implementation dependency (not yet done by this document):** existing B001 500-lesson acceptance scripts, manifests, workflow check names, CI artifacts and fail-closed deployment gates must be **safely adapted/tested for exact 20-code cohorts**, or explicitly superseded by proven equivalently protective 20-scope checks. **Do not** change an expected count alone, weaken validation, delete old tests, mark a gate green without proof, or ship before the actual CI/P5/deployment paths are coherent.

**P6 deployment:** enforce existing branch protections/checks/merge/Render verification, actual live SHA/deployment ID, student navigation, authenticated Practice response grading and database persistence, followed by owner production UAT. Merging this Markdown policy is **not** a 20-lesson release.


## 7. Status and artifacts

Proposed lifecycle (descriptive; not asserted to exist as implemented state in current code):

```text
BATCH_PLANNED
  -> INVENTORY_RECONCILED (20 real codes + source metadata)
  -> REVIEW_IN_PROGRESS (per-lesson Track A/Track B)
  -> LESSON_TESTED / LESSON_BLOCKED
  -> 20/20 INTERNALLY_ACCEPTED
  -> BATCH_READY
  -> MERGED
  -> VERIFIED_LIVE
  -> OWNER_UAT
```

Recommended *future* 20-scope artifacts (do not overwrite prior 500-lesson originals):

```text
docs/curriculum/batches/US-CCSS-B001-20-INTAKE.json
artifacts/curriculum-delivery/US-CCSS-MATH/
  source-index.json                  # reuse/hash-check existing source index
  B001-20/ordered-lesson-codes.json
  B001-20/lesson-evidence.jsonl
  B001-20/gap-dashboard.json
  B001-20/checkpoint.json
  B001-20/batch-acceptance.json
```

Preserve old `B001/` 500-lesson artifacts and public Git history as historical input; never store identifying student details, copyrighted PDF bytes or confidential traces in public Git.


## 8. Rollout sequence

| Phase | Deliverable | Completion proof |
|---|---|---|
| P0 | Apply owner-authorized **20-lesson V2 policy**; V1 remains superseded; archive prior 500-batch scope | Authoritative document + follow-on implementation ticket/PR |
| P1 | Resolve exact first **20** lesson codes and match source/DB/effective content | Ordered 20/20 inventory, no missing/duplicate/mismatched identities |
| P2 | Review/classify all **20** (including 15 historical drift checks) | Per-lesson source-goal-content-Practice/math evidence and genuine gaps |
| P3 | Reuse/repair only deficient content and shared Practice/Math capability | Correct source-aligned original student-visible output for 20 |
| P4 | Impact-appropriate real tests and evidence on the actual candidate SHA | Each required lesson independently verified; persisted attempts where required |
| P5 | **20-lesson academic acceptance** and existing security/deployment gates | Genuine exact-cohort **`20/20`**, no additional signatures or fabricated PASS |
| P6 | Merge, verify Render live release and owner production UAT | Live SHA, deployment ID, navigation and authenticated persisted Practice |
| P7 | Repeat B002 through B078 | **1,560/1,560** total with independent cohort and lesson evidence |

**Do not modify production math code, the database, CI/branch protections or Render solely by treating this Markdown amendment as an executable software change.** Implement follow-on 20-scope manifests/acceptance checks in independently reviewable code changes. Preserve T001 historical IDs, SHAs, prior source work and earlier evidence.


## 9. Reporting contract

After any session or checkpoint, report actual and separately distinguish planning from execution:

```text
Programme: US-CCSS-MATH
Batch: B001 of 78 | planned release scope: 20 | overall programme: 1,560
Actual IDs reconciled: X/20
Previously shipped positions in B001: 15 (G1 T001 10 + B03 5; drift proof pending)
Not previously shipped positions in B001: 5 (G1 grade ordinals #16-#20)
Actual learner/source/activities reviewed: X/20 (historical and new split reported)
P3 lessons confirmed source-aligned/reused/repaired: X/20, with exact IDs and evidence
P4 independently tested per-lesson: X/20 on a specified candidate SHA
P5 independently accepted under new 20-policy: X/20; gate PASS/NO_GO only if executed
Blocked: count + exact codes + reasons
Old 500-scope evidence reused: run IDs/commit SHAs/exact overlapping lesson IDs
Git PR/commit and required CI checks: actual state
Render live SHA/deployment + student/Practice persistence + owner UAT: actual state
Next cursor: curriculum/grade/unit/lesson, ordered without omissions
```

Planned, previously shipped, technically checked, independently academically accepted and deployed **must never be reported as interchangeable**.


## 10. Precedence and historical compatibility

This owner's **2026-10-17 20-lesson V2 amendment overrides every earlier V2 500-lesson scope/acceptance statement for *future* batching and releases**. The historical five-lesson V1 remains superseded. No interim 500-lesson release gate or old B001 cohort is silently treated as the new B001.

The existing file path `EDULYTICS_500_LESSON_BATCH_EXECUTION_PLAN_V2.md`, old Plan ID in saved JSON, 500-lesson intake `US-CCSS-B001-INTAKE.json`, old 500-scope workflows/evidence, and previously opened source/Practice PRs are **preserved for links, auditability and reuse**. They must be explicitly re-keyed/re-scoped in future implementation so automated gates enforce **the right exact 20 lessons** and every 20-batch can be released safely. This policy alone does not alter the executable 500-scope gate or assert new CI checks have passed.

Useful V1 reuse-first and grade intake rules, historic T001/G1-B03 deliveries and the older academic-certification plan remain historical. Publisher reference use, independent Edulytics authorship, existing repository safety protections, student privacy and rigorous source/math acceptance requirements are unchanged.

## 11. Owner-approved execution commitments (2026-10-17)

**Authority:** These are the owner's explicit operating and accountability instructions for future Edulytics V2 execution. They preserve V2 phases P0–P7, per-lesson quality evidence and deployment protections. **The owner subsequently changed only the release/acceptance unit from 500 to 20 on 2026-10-17**; that explicit later instruction supersedes the old 500-count references in these commitments without weakening the remaining requirements.

1. **عدم استخدام TinyFish؛ استخدام Desktop Commander.** ممنوع استخدام TinyFish نهائيًا في تنفيذ Edulytics، بما في ذلك كبديل عند التعطل. يُستخدم Remote Desktop Commander لفحص الملفات وتنفيذ الأوامر والاختبارات على الأجهزة **المصرح بالاتصال بها فقط**. يمكن أيضًا استخدام أدوات GitHub وRender وNeon المتصلة مباشرة عند الحاجة. إذا كان Desktop Commander غير متصل، يجب توضيح العائق وعدم الادعاء بالوصول أو التحول إلى TinyFish.

2. **كتب الرياضيات الخارجية مراجع فقط.** تُستخدم كتب Illustrative Mathematics وغيرها، بما فيها Teacher Guide وStudent Book، لمراجعة أهداف كل درس ومهاراته وصحة الرياضيات والشرح والأنشطة المطلوبة للاعتماد وفق V2؛ لا تُعامل هذه المراجع كمحتوى مطلوب نقله إلى Edulytics.

3. **استقلال محتوى Edulytics.** الشرح والأمثلة والأسئلة والتمارين والأنشطة والتفاعلات والرسومات داخل المنتج تُؤلَّف وتُطوَّر بصورة مستقلة، ولا تُنسخ نصوص الكتب أو صورها أو رسوماتها أو أنشطتها حرفيًا. المطلوب تحقيق الهدف التعليمي بدقة بمحتوى أصلي، لا استنساخ صفحة الناشر.

4. **عدم فرض اسم الناشر داخل البرنامج.** لا يُضاف اسم Illustrative Mathematics أو شعاره أو نسبة دروس Edulytics إليه في واجهة المستخدم لمجرد الاستعانة بكتبه في المراجعة، ولا يُفترض وجوب نسب المحتوى المستقل إليه. إن أُريد إدخال أصل خارجي بعينه فعلًا، تُراجع حقوقه وشروطه أولًا ولا يُدرج تلقائيًا. استخدام الكتاب كمرجع لا يُعدّ نسخًا لمحتواه ولا نتيجة اعتماد.

5. **الحفاظ على مراحل V2.** لا تُضاف مراحل أو بوابات أو شروط اعتماد أو توقيعات جديدة من دون قرار صريح من صاحب المشروع. **قرار المالك اللاحق في 2026-10-17 غيّر حجم الدفعة والاعتماد فقط إلى 20 درسًا**، مع بقاء P0–P7 وشروط التحقق الأخرى كما هي.

6. **العمل على كل دروس الدفعة الـ20.** B001 الجديدة تشمل أول **20 درسًا من الصف الأول بترتيبها الرسمي**، منها 15 درسًا منشورًا سابقًا تتطلب مراجعة الأدلة والانجراف وخمسة دروس لم تُنشر سابقًا. إصلاح عدد أقل أو فتح PR جزئي لا يعني اكتمال الدفعة. أما الـ1,560 درسًا فتُنفّذ بالتتابع عبر 78 دفعة، مع الحفاظ على كل العمل الموثّق سابقًا للدفعة القديمة ذات الـ500.

7. **التحقق التعليمي لكل درس.** تُراجع الأهداف والشرح والأمثلة والخطوات والأنشطة والرسوم والإجابات ومطابقة Practice للمهارة، بالاستعانة بالمراجع وبفحص ما يراه الطالب فعليًا؛ لا يُكتفى بالعنوان أو نجاح تشغيل الصفحة.

8. **إصلاح الأخطاء الحقيقية فقط.** لا يُعاد إنشاء أو تغيير درس صحيح بسبب التشابه أو تكرار النص الخام وحده. يجب إثبات الخلل الفعلي ومجاله قبل التعديل، مع الاحتفاظ بالأجزاء الصحيحة.

9. **عدم تكرار الاختبارات الناجحة بلا سبب تقني.** تُحفظ نتائج P4 وأدلتها ومعرّفات الاختبارات وروابط تشغيلها وSHA الصحيح، ولا تُعاد إلا عند تغيير مؤثر أو فساد دليل أو حاجة فنية لاختبار الانحدار. لا تُخلط نتائج نسخ مختلفة.

10. **الفصل بين بوابة الاختبار والاعتماد.** P4 تنفّذ الاختبارات الفعلية وتسجّل PASS أو FAIL بأدلتها؛ P5 تقرأ الأدلة المسجّلة وتطبّق الشروط الموجودة في V2، دون إعادة تنفيذ الاختبارات داخلها أو اختلاق نتائج.

11. **لا توقيع معلم ولا موافقة ناشر كشرط اعتماد داخلي إضافي.** يتم الاعتماد الداخلي وفق اختبارات Edulytics وأدلة V2 القائمة؛ لا تُفرض توقيعات أو موافقات خارجية غير متفق عليها. هذا لا يعفي أي أصل خارجي يُستخدم فعلًا من حقوقه الواجبة.

12. **عدم اختلاق نتائج نجاح.** لا يُسجّل PASS أو TESTED أو INTERNALLY_ACCEPTED إلا بدليل حقيقي صالح للدرس ونسخة الكود المعنية. نجاح CI العام أو قراءة PDF أو نجاح التوليد التقني وحده لا يثبت جودة التدريس.

13. **الحفاظ على الكود والأعمال السابقة.** تُراجع ملفات المستودع والفروع وPRs والاختبارات الناجحة والقرارات قبل أي تعديل؛ لا تُحذف أو تُصفّر أو تُستبدل تغييرات قائمة بغير مبرر موثّق وإذن مناسب.

14. **إكمال المسار حتى النشر وفق V2.** إعداد/إصلاح المحتوى المستقل وPractice ← اختبارات P4 وأدلتها ← اعتماد **20/20 في P5 لكل دفعة** ← دمج GitHub بالشروط القائمة ← نشر Render ← التحقق من SHA الفعلي وعمل الطالب وتخزين إجاباته وإجراء owner UAT. لا تجاوز لمراحل الخطة.

15. **عدم إعلان نشر الدفعة الـ20 قبل اكتمالها.** نشر ملفات متابعة أو تحديث CI أو بعض الدروس لا يُسمّى إطلاق B001. إعلان الإصدار يتطلب قبول الدروس الـ20 كلها ونشرها واختبارها فعليًا.

16. **تقارير دقيقة بالأرقام والروابط.** يجب التفريق بين المحصور والمنفّذ والمراجع والمختبَر والمعتمد والمدمج والمنشور والمتحقّق منه، مع GitHub PR/run وروابط الأدلة وcommit SHA وRender deployment وأسباب التعطل الدقيقة. لا تُعرض الأعداد المخططة كمنجزة.

17. **عدم تكرار القرارات المحسومة.** يُستكمل العمل من آخر حالة صحيحة دون إعادة البحث أو الجدل في الترخيص أو استقلال المحتوى أو توقيع المعلم ما لم تظهر مسألة جديدة مؤثرة فعلًا.

18. **عدم نسبة اتفاق إلى صاحب المشروع لم يصدر منه.** يُفرَّق بين قراره الصريح والمعلومة العامة والاستنتاج والاقتراح. يُقدّم أحدث قرار صريح منه عند أي تعارض.

19. **عدم الادعاء بالعمل في الخلفية.** لا يُقال إن العمل سيستمر ذاتيًا بعد انتهاء المحادثة العادية. تُذكر فقط الأعمال المنفّذة بالفعل والتحققات الحقيقية والمهام الجارية في أدوات مرتبطة والعوائق المعروفة؛ ولا يُدّعى تشغيل مؤجل إلا إذا وُجد تفويض وتشغيل فعلي.

### 11.1. معيار المحاسبة

**النجاح النهائي ليس بعدد الـPRs أو نجاح CI وحده**، بل استيفاء شروط V2 التعليمية والرياضية والأمنية لكل **20 درسًا في الدفعة المعنية**، واعتماد 20/20 داخليًا بأدلة صادقة، ثم الدمج والنشر والتحقق من التشغيل؛ ويتكرر ذلك عبر 78 دفعة.

- **تم تنفيذه:** محتوى/كود درس مستقل ومطابق أو وظيفة موجودة فعلًا وجرى التحقق منها، وليس مجرد إدراج الدرس في القائمة.
- **تم اختباره:** دليل درس فعلي من P4 مرتبط بمعرّف الاختبار ونسخة المصدر وcommit SHA.
- **تم اعتماده:** قرار P5 الصحيح على نطاق الدفعة كاملة وفق شروط V2 دون إضافة بوابات.
- **تم نشره والتحقق منه:** SHA حيّ فعلي على Render مع تحقق الطالب وPractice وحفظ الإجابات وowner UAT.

**أي حالة غير مؤكدة تُسجّل "غير متحقق منها" ولا تُعرض كنجاح.** هذه الالتزامات تعليمات تشغيل ومحاسبة، وليست بوابة نشر إضافية.

## 12. Explicit 500 → 20 migration safeguards (owner amendment)

- **Policy now = 20 per batch, 78 batches.** Scope/release authority comes from this V2 document, not an old file name or a 500-lesson JSON checkpoint.
- **Software gates still need implementation verification.** The existing 500-lesson fail-closed acceptance/test suite must not be represented as an operational 20/20 gate until changed and genuinely executed on an exact 20-code candidate. Migrate without bypassing security/deployment checks.
- **Preserve rather than overwrite.** Keep the old 500 intake, source/teacher/student/PDF audits, CI proof runs, 15 shipped-lesson baseline, open PRs and prior immutable SHAs. Historical B001 (500) and new B001 (20) are different *cohorts*, even if the batch label is reused; metadata must carry plan ID, scope size and exact code manifest.
- **No automatic pass.** Old technical 500/500 evidence may substantiate compatible technical tests for overlapping 20 lessons at the correct SHA; it does not prove 20/20 independent educational parity or give a P5 PASS. Existing B001 remains non-released until new 20-scope evidence, a real P5 decision and live owner UAT exist.
- **Cross-boundary candidate changes:** if a previously opened PR repairs lessons spanning multiple new 20-lesson batches, split/verify by exact ID and avoid accidentally deploying out-of-cohort learner changes.

---

## 13. Binding execution correction — prevent B001 P2/P3/P4 rework (owner instruction, 2026-10-17)

**Purpose:** Correct the assistant/operator's *implementation mistakes* that caused the first B001 20-lesson candidate to spend a 2h54m wall-clock interval in repeated amendments. This is a mandatory **execution-order instruction within existing V2 P0–P7**, **not** a new plan, new approval gate, lower quality standard, or invented deadline. Keep all earlier owner commitments, required repository CI/security, exact-SHA evidence, and fail-closed 20/20 P5 intact.

1. **Start from the last verified candidate and the existing work.** Before changing anything, record current main/PR/Our-CS SHAs, uncommitted work, the exact 20 lesson codes, source hashes, published baselines and available reusable P4 evidence. Do not reset, clean, overwrite another workspace, re-open settled decisions, or repeat source scans that are already valid. PR #553 and the earlier #550/#551 are historical candidate/reusable work, **not** an accepted batch.
2. **Complete P2 before starting additional P3 learner-facing changes.** For **each of the 20**, inspect the original teacher goal **and original student task**, the *actual effective student-visible* lesson, diagrams, examples, activities and response formats, its permitted/forbidden Practice families, generator/normalizer/verifier, and previously published student histories. Explicitly record `READY_REUSE`, `CONTENT_REPAIR`, `PRACTICE_REPAIR`, `NEW_SHARED_CAPABILITY`, or `BLOCKED_SOURCE` with the exact independently verified deficiency and relevant source/content SHAs. A PDF text skim or green technical smoke test does not establish full student-activity parity. **If P2 is incomplete, stop adding speculative lesson changes and finish the missing comparison.**
3. **One scoped P3 correction pass per verified defect class.** Reuse existing content, Rich V2 UI and shared math/Practice engines; preserve correct lessons. Where the source calls for constructing an expression, data chart, sort or written explanation, do not substitute only an answer-choice task. Organize the approved defects by common cause, map all impacted lesson contracts/tests and verify mathematical models independently **before** implementation. Do not create unrelated generators, visualizations or new scope simply to keep changing code.
4. **Preflight locally before expensive CI.** For each coherent change, run syntax/compile, source/contract alignment, generated correct **and plausible wrong** answers, existing tests affected by changed contract family counts, and narrowly scoped lesson regression tests. Fix actual failures **before** pushing. After the coherent candidate is stable, run all **required** full, security, migration, database, authentic HTTPS/browser and historical-drift CI where source or code changes make them applicable. Never skip required checks just to save time.
5. **Avoid redundant reruns without compromising SHA binding.** Index evidence by lesson code, source/content hash, candidate SHA, CI run and grade/version; reuse earlier evidence **only if the existing V2 reuse rules establish that it remains valid for the exact unchanged behavior and code**. A changed candidate SHA or teaching/Practice implementation can invalidate evidence; never claim old PASS on a new candidate by convenience. Do not use a continuously changing draft PR as substitute for a final tested candidate.
6. **P4 produces proof; P5 decides; P6 publishes only after P5.** Complete the independent lesson-by-lesson mathematical, instructional, original-activity and persisted-student proof for all 20; P5 must return authentic `20/20` on the exact candidate. `CI success`, `NO_GO gate completed successfully`, `source PDFs read`, `published historical status`, and `ready contract` are not academic certification. If any lesson or historic drift remains unproven, retain `NO_GO`, **do not merge or deploy learner-facing B001**. Only after legitimate P5 acceptance complete required GitHub merge and Render live-SHA/Practice persistence verification plus owner UAT.
7. **Track actual elapsed time and explain rework.** For each stage/defect class, record available measured start/stop, actual test duration, reuse vs rerun decision and cause of retries. Never present a PR's calendar lifetime as programmer time, or call a speculative 45–75 minute target an observed result. Keep internal traceability without generating unsolicited progress reports when the owner has requested execution without reports.

**Operator restart checkpoint (record, not exemption):** B001 current integrated candidate is PR #553 at commit `32433c1f471a47bc58605395e9477024b5b83a93` as inspected on 2026-10-17, with four failing broad CI workflows and no genuine `20/20` P5. Recheck live refs before acting because the branch may move. First reconcile unfinished P2 and impacted historical/family-count tests, **without falsely accepting or deploying** the batch.
