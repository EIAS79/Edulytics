# B001 — G1 U02 L11–L15 source-aligned learner teaching repairs (candidate only)

**Batch:** US-CCSS-B001, canonical ordinals **26–30/500**, Grade 1 Unit 2.  
**Authority:** Illustrative Mathematics / Kendall Hunt K–5 Mathematics, first edition (2021), teacher preparation pages and original teacher/student-facing activity extracts already recorded in B001's source-pedagogy packets. Original Student Task Statement PDF links are preserved by canonical lesson code. No independent full-page/diagram validation of these five student PDFs is claimed here.  
**Release status:** **NOT internally accepted**, no new B001 approval, no production authorization. No human teacher signature is required for *Edulytics internal* source-and-test-based acceptance under V2.

| Lesson | Source teaching objective | Learner body defect corrected | Candidate-specific teaching |
|---|---|---|---|
| [L11 — Make Them the Same](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-11/preparation.html) | Compare unequal cube towers, determine unknown difference | Previous examples defaulted to combining groups of 3 and 7 rather than visually aligning 9 and 5 | Align two towers, identify four unmatched cubes, verify both 5 + 4 = 9 and 9 - 5 = 4 |
| [L12 — School Supplies](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-12/preparation.html) | Difference-unknown comparison in school-supply stories | Old steps were generic rather than solving the actual compare situations | Match 10 students with 6 pencils, count four missing; also 9 markers with 4 caps |
| [L13 — Compare Favorite Art Supply Data](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-13/preparation.html) | Compare categorical art-supply data and write matching equations | Generic unrelated addition examples did not teach the difference in a data chart | Read category labels, pair chart marks, find 7 - 4 = 3 and label **illustrative** example counts rather than passing them off as real survey data |
| [L14 — Compare with Addition and Subtraction PLC Activity](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-14/preparation.html) | Match addition **and** subtraction equations to one comparison | Old learner steps addressed only one subtraction answer | Explain why 8 - 3 = 5 and 3 + 5 = 8 both model the art-station difference, while 8 + 3 does not answer “how many fewer?” |
| [L15 — Different Types of Story Problems](https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-15/preparation.html) | Distinguish total, part, change and difference unknowns, write matching equations | Prior body defaulted to ordinary result-unknown addition | Represent eight people/six students/two teachers, a join/unknown story and a comparison; box the variable asked about |

## Engineering implementation

- One **source URL + canonical content SHA-256 + exact outcome-order guarded** correction module, scoped only to five canonical lesson IDs.
- Revised explanation, concept, three worked examples, five clear learner steps, common mistakes and summary for each lesson. Prior generic text is not silently overwritten on unexpected drift.
- Wire into the existing canonical content materializer, expected version calculation, metadata-only source correction whitelist, safe persisted-version upgrade validation, and fixed inventory count.
- Add independently focused tests for the exact five IDs, sourced worked examples, arithmetic, runtime Rich V2 composition, idempotency, source-hash drift and unexpected persisted versions.
- Add **five source-specific Practice families**, with exact contract routing and independent answer oracles across three difficulties, correct/incorrect/invalid-input checks and alternate story modes. These verify the generated numerical/choice responses, **not** every visual or student-authored activity in the original lessons.
- **No five-lesson release**: these remain candidates inside the one 500-lesson B001 batch; original 2021 student PDF diagrams and visual tasks, interactive constructed responses and full source-activity parity remain unverified for P4/P5.

## Next acceptance work under existing V2

Evaluate the actual student task PDF pages and representation requirements; confirm the newly routed comparison/difference, data-chart and mixed-story Practice families cover each required source activity, and implement missing learner-generated drawings/equations or equivalent interactions. Run existing exact-commit targeted and whole-batch regression gates after the candidate changes. Continue subsequent original-source lessons in curriculum order while retaining valid historical 15-lesson evidence. Do **not** write fake PASS records, require outside teacher signatures, or publish unaccepted B001.
