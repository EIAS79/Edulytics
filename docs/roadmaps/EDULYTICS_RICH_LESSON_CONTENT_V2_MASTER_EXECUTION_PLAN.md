# Edulytics Rich Lesson Content V2 — Master Execution Plan

**Status:** COMPLETE — CATALOGUE-WIDE PRODUCTION ROLLOUT CLOSED  
**Repository:** `EIAS79/Edulytics`  
**Programme scope:** Every learner-visible mathematics pedagogical lesson in every supported curriculum, official and supporting.  
**Primary purpose:** Replace thin/generic lesson bodies with complete, accurate, source-backed, student- and teacher-usable teaching content without changing curriculum objectives, official outcomes, lesson identities, assessment history, mastery history, or existing academic mappings.

---

# 1. Governing intent

The current issue is not curriculum structure. It is lesson-content depth.

Many lessons technically contain all six required sections:

1. Explanation
2. Key Concepts and Rules
3. Worked Examples
4. Step-by-Step Solutions
5. Common Mistakes
6. Quick Summary

but the actual content can still be too short, generic, repetitive, or insufficient to teach the lesson independently.

This programme upgrades the **content inside the existing lessons**.

It does **not** redefine what the curriculum says should be learned.

---

# 2. Non-negotiable boundaries

The following remain unchanged unless a separate approved programme explicitly changes them:

```text
Curriculum hierarchy
Framework identities
Academic years
Grades / levels
Units
Topics
Lesson identities
Official OutcomeCodes
Existing official curriculum mappings
Student accounts
Teacher accounts
Assessment identities
Assessment approval workflow
Existing assessment results
Existing practice history
Mastery history
Analytics history
Historical generated items
```

The programme changes **pedagogical lesson content and presentation quality**, not official curriculum truth.

---

# 3. Core model

Edulytics will keep three distinct layers:

```text
LEVEL 1 — CURRICULUM AUTHORITY
What must be learned?

        ↓

LEVEL 2 — PEDAGOGICAL / RESEARCH SOURCES
How can this mathematics be taught accurately?

        ↓

LEVEL 3 — EDULYTICS-AUTHORED LESSON CONTENT
How Edulytics explains, demonstrates and teaches it.

        ↓

LEVEL 4 — OPTIONAL SUPPLEMENTARY HELP
Curated YouTube / external learning resources.
```

Official curriculum identity and pedagogical explanation must never be confused.

A lesson may belong to Cambridge, UAE, Polish, American, British or another curriculum while its learner-facing explanation is Edulytics-authored using lawful pedagogical sources.

---

# 4. Source policy

## 4.1 Curriculum authority

For an official lesson, the official curriculum remains the source of truth for:

- lesson identity;
- official objective/outcome;
- grade/level;
- curriculum scope;
- official wording where lawfully used;
- mapping/provenance.

The curriculum authority determines **what** the lesson is about.

## 4.2 Pedagogical sources

Pedagogical research may use:

- official government mathematics guidance;
- ministry/department teaching resources;
- open educational resources;
- current official textbooks where rights permit;
- school-adopted textbooks where rights permit;
- widely used publisher material where rights permit;
- other reputable mathematical references for independent verification.

## 4.3 Commercial-rights requirement

Any material used as an adaptation/source-driven content basis must pass the existing Edulytics licence policy.

Currently approved examples include:

- Public Domain
- CC0 1.0
- CC BY 4.0
- Open Government Licence v3.0

Restrictive or unclear rights must fail closed.

Examples that are not acceptable as adaptation sources without separate permission include:

- CC BY-NC
- CC BY-NC-SA
- CC BY-ND
- CC BY-NC-ND
- All Rights Reserved
- unknown licence
- "free for educational use" with no clear commercial/adaptation rights

## 4.4 Copyrighted reference material

Copyrighted material may be used only as a lawful research/reference input where appropriate.

Do not:

```text
copy textbook prose
lightly paraphrase copyrighted teaching pages
copy publisher illustrations
copy proprietary worked-example sequences
copy proprietary page structure
```

unless Edulytics has permission or an appropriate licence.

The final lesson body should be independently authored by Edulytics.

## 4.5 Missing source

A missing adaptable textbook/source must **not** cause the lesson to remain empty.

Where the mathematical target is clear:

```text
exact lesson target
+
reliable mathematical research
+
independent verification
=
original Edulytics pedagogical content
```

If the target itself is ambiguous, the lesson must be routed to review rather than guessed.

---

# 5. Target lesson quality standard

Every learner-visible mathematics lesson must be able to teach the exact target independently.

The six existing sections remain conceptually valid, but their quality standard changes.

## 5.1 Explanation

Must include, as applicable:

- clear statement of what the student is learning;
- definitions and vocabulary;
- conceptual meaning;
- why the method/rule works;
- prerequisite ideas when relevant;
- how to recognize the problem type;
- all principal cases implied by the lesson target;
- meaningful mathematical representations;
- connections to prior knowledge where useful.

A one- or two-sentence generic definition is not sufficient.

## 5.2 Key Concepts and Rules

Must include:

- all necessary rules for the exact target;
- definition of each important term;
- explanation of what each rule means;
- conditions or limitations;
- notation;
- short examples attached to rules where useful.

A repeated copy of the Explanation section is not acceptable.

## 5.3 Worked Examples

Must include multiple genuine examples where the topic supports multiple forms/cases.

Expected principle:

```text
simple concept introduction
+
standard question
+
different representation or variation
+
reasoning/application case
+
harder or boundary case where appropriate
```

The number of examples is determined by the concept, not a fixed word-count rule.

For normal lessons, one generic example is not sufficient when materially different question forms exist.

## 5.4 Step-by-Step Solutions

Solutions must be executable and connected to real questions.

Avoid generic instructions such as:

> Identify the relationship, apply the rule, then verify.

Prefer:

```text
Question
Step 1 — ...
Step 2 — ...
Step 3 — ...
Answer — ...
Check — ...
```

Where a topic has different methods or cases, provide distinct worked solutions.

## 5.5 Common Mistakes

Must include realistic lesson-specific misconceptions.

For each important mistake, explain:

- what the learner may do;
- why it is wrong;
- how to correct it;
- a counterexample or check where useful.

## 5.6 Quick Summary

Must act as a revision aid.

It should summarize:

- the central idea;
- core rules;
- method checklist;
- critical warning(s).

It must not be generic QA language.

---

# 6. Rich Lesson Content V2 target model

The current six text fields remain supported during migration, but the target architecture should permit structured lesson content.

Target structure:

```text
RichLessonContentV2
 ├─ Explanation
 │   ├─ Introduction
 │   ├─ Definitions[]
 │   ├─ ConceptSections[]
 │   └─ Cases[]
 │
 ├─ KeyConcepts[]
 │   ├─ Name
 │   ├─ Definition
 │   ├─ Rule
 │   ├─ Conditions
 │   └─ Example
 │
 ├─ WorkedExamples[]
 │   ├─ Question
 │   ├─ Difficulty
 │   ├─ Method
 │   ├─ Steps[]
 │   ├─ Answer
 │   └─ Check
 │
 ├─ CommonMistakes[]
 │   ├─ Mistake
 │   ├─ WhyWrong
 │   └─ Correction
 │
 ├─ SummaryPoints[]
 │
 ├─ Visuals[]
 │
 └─ Videos[]
```

Migration must preserve compatibility with existing lesson routes until the V2 renderer is production-ready.

---

# 7. Visual-content policy

Mathematics visuals must be instructional, not decorative.

Preferred approach: Edulytics-owned structured renderers.

Examples:

```text
fractions      → fraction bars / number lines
ratio          → double number lines / ratio tables
place value    → place-value charts
coordinates    → coordinate planes
geometry       → labelled figures
statistics     → charts/plots
scale reading  → labelled measurement scales
area           → decomposition diagrams
arrays         → grids/arrays
```

Generic fallback visuals such as concept-flow placeholders must not count as sufficient visual pedagogy.

External images may be used only when rights and attribution are clear.

---

# 8. Video Help policy

Add an optional seventh learner-facing section:

## Video Help / Watch & Learn

Purpose:

- offer an alternative explanation;
- reinforce the same lesson target;
- help students and teachers who benefit from audiovisual instruction.

Video content is supplementary. It is not the authoritative written lesson.

Target data:

```text
VideoResource
- LessonId
- Provider
- VideoId / URL
- Title
- Channel / creator
- Language
- Duration
- WhyRecommended
- CheckedAt
- ReviewStatus
```

Suggested review states:

```text
Candidate
Reviewed
Approved
Unavailable
Rejected
```

## YouTube-specific rules

- use official YouTube embeds/links;
- do not download or rehost videos;
- do not remove attribution;
- do not represent videos as Edulytics-owned;
- do not gate payment specifically for access to the video;
- prefer privacy-enhanced embedding for the school/student context;
- do not expose unrestricted YouTube search directly to children as the default lesson experience;
- search may be used in the content-acquisition pipeline to discover candidates;
- candidate videos must be reviewed before publication.

---

# 9. Student and Teacher presentation

There should be one rich canonical academic lesson body.

```text
               Rich canonical lesson
                        │
              ┌─────────┴─────────┐
              │                   │
        Student view         Teacher view
```

Student view:

- rich Explanation;
- Key Concepts;
- Worked Examples;
- Step-by-Step Solutions;
- Common Mistakes;
- Quick Summary;
- instructional visuals;
- optional Video Help;
- Practice entry point.

Teacher view:

- the same rich teaching body;
- plus curriculum identity;
- official standards/outcomes;
- source/provenance;
- lesson code and staff metadata.

Do not maintain a weak teacher explanation and a separate rich student explanation.

---

# 10. Catalogue-wide coverage target

The programme applies to **100% of learner-visible mathematics pedagogical lessons** across all supported curricula.

Do not rely on old approximate counts.

The first execution phase must calculate the exact current catalogue count and break it down by:

- curriculum;
- framework;
- level/grade;
- unit/topic;
- official vs supporting;
- content version;
- source type;
- current quality status.

If the current catalogue contains more than 5,000 lessons, all 5,000+ remain in scope.

---

# 11. Programme Phase R1 — Exact catalogue and content-quality audit

## Objective

Establish the exact workload and identify weak lesson bodies without changing production behavior.

For every learner-visible lesson, record:

```text
LessonId
LessonCode
Curriculum
Framework
Grade / Level
Unit
Topic
Title
OfficialOutcomeCount
IsSupporting
ContentVersion
PedagogicalSourceType

ExplanationQuality
KeyConceptQuality
WorkedExampleQuality
StepByStepQuality
CommonMistakeQuality
SummaryQuality
VisualQuality
VideoStatus
OverallQuality
```

Suggested content defect/status values:

```text
GOOD
NEEDS_EXPANSION
GENERIC
OFF_TARGET
INSUFFICIENT_EXAMPLES
INSUFFICIENT_SOLUTIONS
INSUFFICIENT_MISCONCEPTIONS
VISUAL_WEAK
SOURCE_RESEARCH_REQUIRED
REQUIRES_ACADEMIC_REVIEW
```

## Deliverables

```text
artifacts/lesson-content-v2/catalogue-inventory.json
artifacts/lesson-content-v2/content-quality-audit.json
docs/lesson-content-v2/catalogue-summary.md
```

## Exit criteria

- exact current lesson count known;
- 100% learner-visible mathematics lessons classified;
- zero lessons silently excluded;
- no production content changed.

---

# 12. Programme Phase R2 — Rich Lesson Content Quality Contract

## Objective

Turn "six non-empty strings" into a meaningful pedagogical quality gate.

Implement deterministic checks for:

- empty/truncated sections;
- duplicate/repeated sections;
- generic authoring language;
- generic solution templates;
- lack of concrete examples;
- insufficient distinct examples where the topic requires variants;
- step-by-step content with no real steps;
- no lesson-specific misconceptions;
- summary not matching target;
- weak/generic visuals;
- source/provenance gaps.

Semantic/AI-assisted review may propose classifications, but promotion to production must use explicit evidence and deterministic gates where possible.

## Exit criteria

Every lesson can be evaluated against one published Lesson Content V2 quality standard.

---

# 13. Programme Phase R3 — Source and research acquisition pipeline

## Objective

Create a reproducible source dossier for every lesson that needs enrichment.

For each lesson:

```text
Existing lesson identity
        ↓
Exact target evidence
        ↓
Official curriculum source
        ↓
Pedagogical source search
        ↓
Licence / rights validation
        ↓
Independent mathematical verification
        ↓
Research dossier
```

Research dossier must record:

```text
target
curriculum context
source title
publisher
edition/version
URL
checked date
licence
specific locator/page/section where available
selection reason
source rights
key mathematical concepts
required cases
recommended examples
known misconceptions
recommended visual forms
```

No unlicensed adaptation source may enter the authored-content pipeline.

## Exit criteria

Every lesson selected for enrichment has either:

- an approved source dossier; or
- an explicit review/block reason.

---

# 14. Programme Phase R4 — Rich Content V2 model and renderer

## Objective

Support deep structured lessons without breaking current routes.

Implement:

- structured content contracts;
- persistence/versioning strategy;
- compatibility adapter for existing six-field content;
- student renderer;
- teacher renderer;
- accessibility;
- responsive presentation;
- content-version auditing.

No existing lesson URL should break.

## Exit criteria

A representative V2 lesson can render in Student My Learning and Teacher Lesson Content from the same canonical academic body.

---

# 15. Programme Phase R5 — Instructional visual system

## Objective

Replace generic concept placeholders with mathematically meaningful visuals where needed.

Implement/extend reusable renderers and visual specifications.

Each visual must be:

- mathematically consistent;
- tied to lesson content;
- accessible;
- deterministic where possible;
- responsive;
- independently testable.

## Exit criteria

Pilot lessons that require a visual have a target-specific instructional representation rather than a generic fallback.

---

# 16. Programme Phase R6 — Curated Video Help

## Objective

Add safe supplementary audiovisual help.

Implement:

- VideoResource model;
- candidate discovery;
- review/approval workflow;
- availability checks;
- student/teacher display;
- privacy-enhanced YouTube embedding where applicable;
- attribution.

Do not make unrestricted YouTube search the default student experience.

## Exit criteria

Approved pilot lessons can show reviewed video help without changing the lesson's authoritative written content.

---

# 17. Programme Phase R7 — Pilot enrichment

## Objective

Prove the complete pipeline before catalogue-wide migration.

Initial pilot:

- approximately 20–30 representative lessons;
- multiple domains;
- official and supporting lessons;
- different grades;
- lessons with and without required visuals;
- at least one lesson requiring substantial source research.

Cambridge Primary Stage 6 is a strong candidate for the first pilot because known thin/generic lessons already exist there.

Pilot must verify:

- mathematical correctness;
- exact-topic alignment;
- section depth;
- distinct examples;
- solution quality;
- misconception quality;
- visual usefulness;
- video usefulness where present;
- Student UI;
- Teacher UI;
- mobile/responsive behavior;
- Practice alignment;
- performance.

## Exit criteria

Rich Lesson Content V2 is proven on representative production-like lessons before scaling.

---

# 18. Programme Phase R8 — Catalogue-wide rollout and permanent quality gate

## Objective

Enrich every weak learner-visible mathematics lesson.

Roll out in controlled waves determined by the Phase R1 inventory, for example:

```text
Wave A — pilot curriculum/grade
Wave B — remaining primary curricula
Wave C — secondary curricula
Wave D — advanced curricula
Wave E — remaining supporting/legacy lessons
```

The actual wave sequence must come from the inventory, not assumption.

For every upgraded lesson:

```text
source dossier
→ authored content
→ math verification
→ quality validation
→ visual validation
→ optional video review
→ student/teacher render test
→ versioned publication
```

## Permanent CI gate

No future lesson may be considered Rich Content Ready unless it passes the active Lesson Content V2 quality contract.

## Exit criteria

- 100% catalogue classified;
- every weak lesson either enriched or explicitly blocked/reviewed;
- no learner-visible lesson passes simply because six fields are non-empty;
- all published enriched examples/solutions are mathematically verified to the appropriate level;
- student and teacher consume the same canonical rich content;
- production rollout completed without breaking curriculum identities, assessment history or mastery history.

---

# 19. Execution order

```text
R1 Catalogue Audit
    ↓
R2 Quality Contract
    ↓
R3 Source/Research Pipeline
    ↓
R4 Rich Content Model + Renderer
    ↓
R5 Instructional Visuals
    ↓
R6 Curated Video Help
    ↓
R7 Pilot Enrichment
    ↓
R8 Catalogue-wide Rollout + CI Gate
```

Research evidence may be collected during R1/R2, but no mass enrichment should begin before R2 and R3 are defined.

---

# 20. First implementation PR

The first implementation PR after this plan should be:

## PR 1 — Lesson Content V2 Catalogue Audit + Quality Contract Foundation

It must:

1. calculate the exact current learner-visible mathematics lesson count;
2. inventory every lesson;
3. classify official/supporting/source type;
4. inspect all six current lesson sections;
5. detect generic/repeated/insufficient content;
6. classify visual quality;
7. produce machine-readable audit artifacts;
8. produce curriculum/grade/unit breakdowns;
9. make **no production lesson-content changes**.

This PR gives the programme a real measured baseline before any large-scale rewrite.

---

# 21. Required safeguards

Never:

```text
change official outcomes to make explanations easier
invent official curriculum mappings
bulk-rewrite thousands of lessons without lesson-level QA
copy copyrighted textbook prose or images without permission
treat one generic example as full lesson coverage
treat non-empty fields as proof of quality
treat a decorative visual as instructional evidence
publish arbitrary YouTube search results directly to students
use an LLM as the sole mathematics verifier
overwrite strong existing lesson content simply to make everything uniform
```

---

# 22. Definition of Done for one rich lesson

A lesson is Rich Content Ready only when:

```text
[ ] Existing lesson identity preserved
[ ] Existing curriculum/outcome relationship preserved
[ ] Exact teaching target is clear
[ ] Source/provenance recorded
[ ] Source rights are acceptable
[ ] Explanation fully teaches the target
[ ] Key concepts/rules are defined and explained
[ ] Worked examples cover the meaningful cases
[ ] Step-by-step solutions are executable
[ ] Common mistakes are lesson-specific
[ ] Quick summary is useful for revision
[ ] Required visual representations exist
[ ] Mathematics/examples/solutions are verified
[ ] Student presentation passes
[ ] Teacher presentation passes
[ ] Practice remains aligned
[ ] Optional Video Help is reviewed/approved if present
[ ] Content version is traceable
```

---

# 22.1 R8 closure record

R8 is closed when repository CI confirms the final catalogue state:

```text
Total learner-visible mathematics lessons: 4,453
Curated Rich V2: 26
Runtime verified Rich V2: 4,427
Total Rich-ready: 4,453 / 4,453
Polish localized runtime Rich V2: 1,569
Localized-authoring blockers: 0
Practice-contract blockers: 0
Generation blockers: 0
```

The Polish closure reuses the accepted official OutcomeCode mapping and the existing
`READY_VERIFIED` Polish Practice contracts. Generated worked examples continue to
come from the exact Mathematics engine and independent verifier; learner-facing
Polish prompt/solution prose is produced by a deterministic, reviewable localization
layer rather than a runtime machine-translation service.

# 23. Definition of programme success

Success is not:

```text
"Every lesson has text."
```

Success is:

```text
Every learner-visible mathematics lesson
has enough accurate, source-backed, pedagogically structured content
for a student to learn the exact topic
and for a teacher to use the same lesson as a reliable teaching/reference aid.
```

The official curriculum still determines **what** is taught.

Edulytics becomes responsible for making **how it is taught** substantially clearer, deeper, more useful and more consistent.
