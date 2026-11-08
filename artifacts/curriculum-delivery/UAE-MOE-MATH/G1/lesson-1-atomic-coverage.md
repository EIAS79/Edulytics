# UAE Grade 1 — Unit A, Lesson 1 — page-by-page Stage A coverage audit

LessonCode `PED:UAE-MOE-MATH:L1:COMMON:01:01` — `قراءة وكتابة الأعداد من ١ إلى ٥`.
Verified source: Our-CS, `Grade_01\01_Student_Books\01.pdf`, Arabic Grade 1 student book 2025–2026, SHA256 `BF2C47A9F2F5457AB4ABF48EFE5603BD6D946CD1A8A09B4BA1BC57120EFEAE44`.
Physical printed pages **A17–A24** = PDF **34–41**. All eight page images were inspected directly on the device on 2026-11-08.

| Printed page / PDF page | Identified teaching/assessment element | Original Edulytics equivalent | Mode |
|---|---|---|---|
| A17 / 34 | Recognize quantities 1–5; connect object sets, digit outlines and spoken/printed number words; exploratory picture and teacher prompt | Original 1–5 dot collections, count → digit → number word, teacher cards | Digital + teacher |
| A18 / 35 | Vocabulary «عدد», «واحد», «اثنان», «ثلاثة», «أربعة», «خمسة»; examples 1–3 with traced digits and object sets | Arabic concepts and steps for matching quantity/digit/word; original traced digit exercise | Digital + teacher handwriting |
| A19 / 36 | Practice items 4–6, visual sets of four/five, writing traced/independent digits | Original 4/5 representation and two-phase tracing/writing; range-scoped count engine | Digital + teacher handwriting |
| A20 / 37 | Independent practice 1–3: count three different small collections and write numbers independently | New dot/physical collection trials 1–5, learner writes and explains | Digital + teacher |
| A21 / 38 | Independent practice 4–6: count five/four/five objects with different layouts | Reordered and differently grouped 4/5 original dots; prevent miscount | Digital + teacher |
| A22 / 39 | Home practice 1–3: 1/2/3 object sets; numeral writing/repetition | Original count-and-write cards for 1–3, repeated writing with teacher/guardian observation | Digital + teacher |
| A23 / 40 | Home practice item 4 and number vocabulary matching/writing 1–3 | Number word and symbol recognition; original match and handwriting activity | Teacher-led for word/penmanship |
| A24 / 41 | Home practice 9–10 objects 4/5, vocabulary items 11–12; grouping/drawing tasks for 4/5 | Original grouped dots and drawing 5/4 circles; handwriting and word activities | Digital + teacher |

**Copyright:** source pictures/examples are described by skill and activity only. None of their images or expressive wording is reused.

## Stage B delta
The existing correctly aligned Arabic numbers/skills were retained. Replaced generic language with explicit original count–read–trace–write progression, examples for 3/4/5, grouped-cardinality invariant, vocabulary, common mistakes, two original practice tasks and an Edulytics-authored exit ticket. Preserved LessonCode and all other lesson translations/records.

## Stage C status
The exact numeric family `uae.g1.u01.l01.read_write_1_5` is in the engine registry, with numeric exact grading and a written `UaeGrade1ArabicCountingFamiliesTests.cs` test. Handwriting, spoken reading and drawings are **teacher-reviewed**, never claimed as automatically graded. The `UniversalExactSkillContractAdapter` depends on a usable profile `HasExactSkillContract`; a definitive operational mapping for this specific curriculum lesson has **not** been verified. **Remain A_C_EXPLICIT_GAP (RUNTIME_FAMILY_ROUTING).** UI RTL runtime testing belongs to Stage D. Tests `WRITTEN_NOT_RUN`; no build or CI executed.

Source-to-authoring equivalence is by educational objective and representation, **not** copyright-sensitive reproduction of the individual source questions.
