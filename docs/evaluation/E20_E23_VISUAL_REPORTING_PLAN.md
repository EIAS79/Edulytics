# Evaluation E20-E23 — Visual Analytics, Full Student Reporting and UX Production Plan

## Objective

The Evaluation Engine is already production-qualified through E19. E20-E23 do not change the source-of-truth evidence model. They improve how the same evaluation is understood, printed and acted on.

The release goal is to make teacher/admin analytics visually readable at a glance, make student progress motivating rather than table-heavy, and provide a professional full-record student report in both web/print and PDF form.

## Product invariants

1. Charts visualize existing evaluation values; they do not recalculate mastery in JavaScript.
2. Missing evidence remains unavailable, never 0%.
3. Assessment and Practice remain separate dimensions.
4. Student private Practice is visible only to the owning student.
5. Staff reports use the staff-safe official evaluation stream.
6. Teacher, Subject Supervisor and School Administrator report access remains server-authorized through Analytics scope.
7. A student can export only their own report.
8. Motion respects `prefers-reduced-motion`.
9. Every chart has text/table fallback so accessibility and printing do not depend on animation.
10. A selected term filters the report history/term focus without pretending that year-wide current mastery is a term-only metric.

---

# E20 — Visual Analytics System

## Scope

Create a dependency-free Evaluation chart layer using server-rendered data, CSS and small progressive-enhancement JavaScript.

### Chart primitives

- animated donut;
- grouped horizontal bars;
- stacked distribution bar;
- lollipop skill/topic chart;
- line/sparkline assessment progression;
- waterfall assessment-change chart;
- visual progress bands.

### Teacher/Admin Analytics

**Overview**
- overall mastery donut;
- evidence / at-risk / critical / weak-topic compact bars;
- visual outcome distribution where data exists.

**All Students**
- mastery-state donut;
- stacked class distribution;
- improving/stable/declining bars;
- retain full student table beneath charts.

**Student 360**
- animated current-mastery donut;
- Assessment vs Practice grouped bars;
- topic mastery lollipop;
- assessment progression line chart;
- term/assessment waterfall;
- visual skill bars alongside detailed evidence table.

**Topics & Skills**
- topic current/Assessment/Practice comparison bars;
- affected-student lollipops;
- existing drill-down tables remain canonical.

**Subject Overview**
- class comparison bars;
- priority-gap lollipop chart;
- retain class cards and exact skill table.

### Student My Progress

- animated mastery donut;
- Assessment vs private Practice bars;
- assessment progression line chart;
- term change waterfall;
- colorful skill progress bars;
- motivating but factual status language.

## Acceptance

- chart values come directly from server model;
- no external chart CDN/library;
- no chart hides underlying text values;
- no animation under reduced-motion preference;
- responsive at 360px width;
- charts render even when some values are unavailable.

---

# E21 — Full Student Report System

## Staff report

Add a dedicated report page separate from Student 360:

`/school/analytics/student/{studentId}/report`

The report supports:
- academic year;
- class;
- subject;
- optional term focus.

### Sections

1. branded report header;
2. student identity/context;
3. executive evaluation;
4. visual mastery overview;
5. current / Assessment / Practice comparison;
6. development over time;
7. term progression;
8. topic and exact-skill breakdown;
9. strengths;
10. skills requiring focus;
11. prerequisite/retention concerns;
12. recommended next steps;
13. assessment history;
14. evidence summary;
15. teacher comments placeholder;
16. parent/student review placeholder;
17. generated timestamp and report scope.

### Actions

- Print report;
- Download PDF;
- return to Student 360.

## Student self-report

Add:

`/student/progress/report`
`/student/progress/report.pdf`

The student report contains:
- official current mastery;
- Assessment mastery;
- private Practice mastery;
- Practice -> Assessment gap;
- curriculum coverage/confidence;
- development and term progression;
- private Practice summary;
- skill progress;
- next steps;
- assessment history.

Private Practice is explicitly labelled as self-only practice evidence and is never merged silently into official mastery.

## PDF

Upgrade the PDF from a table export into a presentation-quality report:
- Edulytics branding/logo when available;
- report title/context block;
- shaded KPI cards;
- visual mastery bars;
- Assessment/Practice comparison;
- development tables with delta indicators;
- strengths/focus sections;
- recommendations;
- evidence appendix;
- footer/page numbering;
- comments/signature placeholders.

---

# E22 — Permissions and Report Access

## Teacher

Within assigned class/subject:
- view class analytics;
- view every authorized student;
- open Student 360;
- open full student report;
- print/download student report;
- print/download class report;
- create targeted intervention check when exact generation contract exists.

## Subject Supervisor

Within supervised subject:
- cross-class analytics;
- authorized student drill-down;
- full student report/PDF;
- class PDF.

## School Administrator

Within school:
- class/student analytics;
- Student 360;
- full student report/PDF;
- class PDF.

School Administrator does not receive teacher-only intervention creation unless separately authorized by existing role policy.

## Student

- own My Progress only;
- own report only;
- own PDF only;
- no route parameter can select another student.

## Acceptance

- all access enforced server-side;
- report links are only rendered where the route can authorize;
- IDOR tests cover staff student report and student self-report;
- student PDF route contains no studentProfileId parameter.

---

# E23 — UX / Report Polish and Production Closure

## Web polish

- consistent chart palette and status language;
- motion only for progressive reveal;
- printable web report stylesheet;
- chart legends;
- empty/sparse-evidence states;
- long-name wrapping;
- compact mobile layouts;
- keyboard/ARIA labels for charts.

## PDF polish

- logo;
- branded header;
- visually separated sections;
- colored mastery/status bands;
- chart-like horizontal bars;
- page footer and generated date;
- no raw data-entry appearance;
- long tables paginate correctly.

## Regression / release verification

E20-E23 production closure verifies:
- chart system without external dependency;
- reduced-motion handling;
- teacher/admin/supervisor report routes;
- student self-report IDOR boundary;
- student private Practice separation;
- PDF rendering API compatibility;
- term filter semantics;
- print stylesheet;
- EN/PL localization parity;
- source review for missing-evidence behavior and role boundaries.

## Done definition

E20-E23 are complete when:

- the implementation is merged to `main`;
- the PDF renderer uses supported MigraDoc APIs;
- charts never convert missing evidence into 0%;
- staff and student report routes preserve their authorization boundaries;
- print/PDF/report layouts are present for staff and students;
- E20-E23 do not modify repository workflow files.
