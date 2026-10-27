# Grade 4 2024 migration gate — fail-closed decision at ordinal 101

## Verified scope and source
- User-selected newer IM K–5 v.360 local snapshot has 151 Grade 4 lessons across nine units: 8/17/20/23/18/26/16/11/12.
- Legacy Edulytics 2021 inventory contains 149 lessons. The unit 6 and unit 8 count differ by one each.
- The complete read-only navigation inventory and title-based candidates are in `edition-2024-navigation-crosswalk-151.json`. Of 151 entries, 126 originally had normalized title candidates, 25 required manual or deeper verification. Title-only equivalence does not prove the same curriculum task.
- The 2024 U06:L15 title and five division-number exercises correspond to the legacy 2021 U06:L16 (legacy ordinal **102**), not legacy ordinal 101. Existing G4 U06:L15 remains the 2021 *Situations Involving Area* content, a different mathematics focus.
- Teacher, student, and preparation source hashes and locations are recorded in `grade4-source-edition-2024-decision.json`. All original textbooks/files were read only.

## Actual runtime identity collision
`src/Edulytics.Core/Curriculum/RichLessonContentV2.cs` exposes `RichLessonContentV2Registry.Find(lessonCode,cultureCode)` and builds one dictionary index keyed only by `LessonCode` and normalized `CultureCode`. There is **no source-edition selector** in that index; a 2024 replacement with the same lesson code would collide with, or silently replace, the 2021 content. This is a verified architecture incompatibility, not just a title-label issue.

## Stage A–C state of the first open 2024 lesson
- Stage A: 2024 first-party number and teacher source inventory partially recorded; original publisher Cool-down protected, some artwork and PDF/teacher assets not certified. `DENOMINATOR_INCOMPLETE`.
- Stage B: an independent Edulytics draft covers all five known numerical examples 488/4, 104/8, 96/4, 86/2, 108/9, and an original 132/6 Cool-down with a block-exchange rubric. It is **not** in the live Rich V2 registry.
- Stage C: existing whole-number division mechanic is a reuse candidate, but an exact base-ten exchange family, independent verifier, and teacher-reviewed diagram flow are unverified. `lesson-101-2024-skill-contract.json` records `allowedQuestionFamilies: []` and the proposed extension. A focused test is written but **not run**.
- Saved 2024 publisher HTML declares CC BY-NC 4.0; using protected original diagrams, media or text in a commercial product requires appropriate rights. Original first-party mathematics/examples are staged separately.
- No Stage D, no QA green claim, no production migration, no historic student record updates, no newer-edition academic closures.

## Version-safe work necessary before closing L101
1. Introduce an explicit `SourceEditionKey` / versioned lesson identity and resolve the complete 151-row crosswalk, especially 25 unresolved title matches, before assigning 2024 sequences to students. Avoid changing prior 2021 students' content references or attempt history.
2. Add an edition-aware Rich V2 lookup and edition-scoped lesson/pedagogical blueprint packaging that cannot select 2024 content for a 2021 enrolment. Use test *specifications* written but not run until Stage D per the active A–B–C plan.
3. Finish the 2024 L101 source element denominator, distinguish publisher-protected content from independent Edulytics text/models, and map physical blocks/group presentations to genuinely delivered teacher guidance.
4. Extend the *existing* division engine with a place-value exchange trace and exact question family, rather than adding a new engine for an already-supported division operation.
5. Only then set `READY_FOR_STAGE_D` for 2024 L101, record its candidate commit and advance to 2024 L102. Until then hold at L101.

**Sequencing:** The user's rule forbids progressing to the next lesson before closing the current. Therefore no source or content changes for L102–L151 are presented as newly closed; the grade-wide navigation inventory was prefetched only to settle the edition/version issue.
