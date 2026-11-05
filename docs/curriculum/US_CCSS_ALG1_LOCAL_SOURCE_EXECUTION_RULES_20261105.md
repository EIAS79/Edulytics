# Algebra 1 ? local-source A?B?C execution rules

These rules govern sequential A?B?C repair on PR #589.

- Authoritative teacher source root: `C:\Users\khali\Downloads\New folder (2)\Common Core\09_HS_Algebra_1`.
- Authoritative student source root: `C:\Users\khali\Downloads\New folder (2)\Common Core\09_HS_Algebra_1_Student`.
- Exclude `AccessIM_*` and v360-derived material from source decisions.
- Work strictly in canonical lesson order. Do not start the next lesson until the current lesson is A?B?C closed and recorded on the PR branch.
- Preserve correct existing lesson content. Repair only concrete gaps or errors. Do not rebuild a lesson from scratch without a demonstrated need.
- Practice implementation order is **Reuse ? Extend ? New Engine**. Create a new engine only for a genuinely different instructional/grading mechanism.
- A locked external Cool-down is not a blocker: author an independent Edulytiks Cool-down from the verified goal, skill and lesson content. Do not copy inaccessible text.
- Keep source provenance and hashes in internal curriculum evidence. Do not put source publisher identity, source URLs, or publisher branding into learner-facing Edulytiks copy.
- After A?B?C is satisfied, record `READY_FOR_STAGE_D`. Do not add `REVIEW complete 2021` for this Algebra 1 run.
- Do not run CI, Stage D, a full build, or repetitive broad tests per lesson. Focused tests may be authored and explicitly recorded as `AUTHORED_NOT_RUN`. Never claim an unrun test passed.
- Use one clear lesson-closure commit when practical. Report progress by unit rather than stopping for a report after every lesson.
