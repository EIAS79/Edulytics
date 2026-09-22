# Rich Lesson Content V2 — R3–R7 Pilot Implementation

**Branch:** `rich-lesson-content-v2-r3-r7-pilot`  
**Status:** IMPLEMENTED — CI / review validation pending  
**Date:** 2026-09-22

## Scope

This implementation advances the active Rich Lesson Content V2 programme from the completed R1/R2 audit into a production-capable rich-lesson architecture and controlled pilot.

It does **not** change curriculum objectives, official OutcomeCodes, lesson identities, existing mappings, assessment history, Practice history, mastery history or analytics history.

## R3 — Source and research acquisition pipeline

Implemented `RichLessonSourceDossierFactory` as a fail-closed source decision layer for all 4,453 effective mathematics lessons.

The dossier records:

- lesson and curriculum identity;
- curriculum authority/source URL;
- pedagogical source title/publisher/edition/URL;
- lesson source locator;
- rights evidence;
- adaptation decision;
- decision reason;
- deterministic research query.

Decision states:

- `ApprovedForAdaptation`
- `IndependentAuthoringReferenceOnly`
- `ResearchRequired`

Current catalogue policy:

- openly licensed material with accepted commercial/adaptation rights can enter source-driven authoring;
- official/reference-only material is reference evidence only and requires independently authored learner copy;
- framework-only material remains `ResearchRequired` until a lawful pedagogical source is resolved.

The pipeline emits `artifacts/lesson-content-v2/source-dossiers.json`.

The 1,569 Polish framework-only lessons remain explicitly `ResearchRequired`; they are not pretended resolved.

## R4 — Rich Content V2 model and shared renderer

Implemented a versioned sidecar model:

`RichLessonContentV2Document`
→ `RichLessonContentV2Lesson`
→ structured explanation paragraphs, concepts/rules, worked examples, explicit solution steps, misconceptions, summaries, visuals and optional videos.

The V2 registry is embedded in `Edulytics.Core`, validates content at load time, and resolves by existing `LessonCode` + academic culture.

Compatibility behavior:

- if a V2 lesson exists, Student My Learning and Staff/Teacher Lesson Content render it;
- otherwise the existing canonical six-field content remains the fallback;
- no existing lesson route changes.

Student and teacher use the **same canonical Rich V2 lesson body**.

## R5 — Instructional visual system

Implemented structured visual types:

- CalculationChain
- NumberLine
- DoubleNumberLine
- FractionBars
- RatioTable
- PlaceValueChart
- Scale
- EquationSet
- ShapeDecomposition

The renderer consumes mathematical data stored in the Rich V2 lesson and does not fabricate external images.

All current pilot lessons contain at least one explicit instructional visual.

## R6 — Curated Video Help

Implemented optional `07 — Video Help`.

Rules enforced:

- only `Approved` video resources render;
- YouTube IDs are validated;
- videos use `youtube-nocookie.com` privacy-enhanced embeds;
- external links preserve YouTube attribution;
- video resources are optional and supplemental;
- no unrestricted YouTube search is exposed to students;
- no downloading/rehosting is implemented.

The pilot contains reviewed videos only where the exact external resource was verified. Unverified candidates are not published.

## R7 — Pilot enrichment

### Cambridge Primary Stage 6

Rich V2 coverage: **24/24 source-backed Stage 6 pedagogical lessons**.

Each pilot lesson has:

- 4 explanation paragraphs;
- at least 4 key concepts/rules;
- 4 worked examples;
- explicit multi-step solutions;
- 3 common mistakes;
- 4 revision summary points;
- at least one structured instructional visual.

The source dossier reuses the existing UK Department for Education Year 6 mathematics guidance / Open Government Licence v3.0 provenance already recorded by the canonical Stage 6 pack.

No Cambridge OutcomeCode is invented or added.

### UAE Grade 9 Advanced official pilot

Added two existing official lessons:

- `PED:UAE:G9:ADV:T1:L2-1` — writing equations;
- `PED:UAE:G9:ADV:T1:L2-3` — solving multi-step equations.

Their existing official mappings remain in the base canonical pack.

The Rich V2 sidecar contains no OutcomeCodes or Framework IDs.

Because UAE official textbook material is recorded as reference-only, the source dossier classifies these lessons as `IndependentAuthoringReferenceOnly`; the learner-facing rich explanations/examples are independently authored by Edulytics rather than adapted from official textbook prose.

### Pilot total

**26 lessons**:

- 24 Cambridge Stage 6 source-backed pedagogical lessons;
- 2 UAE Grade 9 Advanced officially mapped lessons.

This satisfies the planned representative 20–30 lesson pilot while covering both source-backed supporting/no-formal-outcome content and existing official mapped content.

## Validation

Automated tests cover:

- exact 24-lesson Cambridge Stage 6 coverage;
- whole-catalogue source dossier generation;
- 1,569 Polish `ResearchRequired` status;
- OGL adaptation permission for the Cambridge pilot;
- reference-only/no-adaptation policy for the UAE official pilot;
- shared Student/Teacher renderer;
- privacy-enhanced YouTube embedding;
- sidecars cannot redefine OutcomeCodes or Framework IDs;
- curated approved-video-only behavior;
- unique Rich V2 lesson identities;
- minimum structural teaching depth.

## Next phase

After CI/review acceptance and production deployment, the programme moves to:

**R8 — Catalogue-wide rollout and permanent quality gate**

R8 remains incomplete until the full 4,453-lesson catalogue is either:

- Rich Content Ready; or
- explicitly blocked/research/review status with evidence.

The controlled production pilot must not be represented as full-catalogue completion.
