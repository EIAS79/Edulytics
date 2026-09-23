# Edulytics UI/Workflow Production Execution Plan

## Purpose

Execute the Admin, Supervisor, Teacher and Student UI/workflow improvement pass in dependency order without hiding correctness, authorization, data-integrity or scalability defects behind cosmetic UI work.

This plan is intentionally incremental. Existing role/permission boundaries, curriculum hierarchy, academic-year structure, accounts, assessment workflows, mastery history, historical generated items and production routes must remain stable unless a phase explicitly requires a compatible change.

## Execution order

```text
Correctness
→ Authorization / data boundaries
→ Data/query foundations
→ Shared UX architecture
→ Role workflows
→ Student dashboard/reporting
→ Responsive/performance hardening
→ End-to-end regression
→ Production rollout
```

Later phases must not compensate for unresolved defects in earlier phases.

---

## Phase 0 — Baseline, discovery and change map

### Objectives

- Capture current main SHA and CI state.
- Inventory affected routes/controllers/views/components/services/repositories/view models.
- Reproduce the Practice multi-tab correctness bug.
- Reproduce curriculum Core/Extended duplication.
- Reproduce Curriculum layout overlap.
- Record role authorization behavior.
- Identify existing reporting data sources.

### Deliverable

`docs/ui-workflow-improvement-baseline.md` with:

```text
Issue
Current route/component/service
Root-cause layer
Proposed implementation location
Tests
Risk
Status
```

### Exit gate

Major issues are mapped to actual source layers before broad redesign begins.

---

## Phase 1 — Correctness and data-integrity blockers

### 1A. Practice multi-tab correctness

Every active Practice attempt must be isolated by stable identifiers.

Authoritative submission context:

```text
StudentId
PracticeAttemptId
QuestionId
LessonId / SkillId
Expected-answer identity
```

The server must evaluate a submitted answer against the authoritative problem belonging to that attempt/question. It must not depend on another browser tab's current-question state, cached expected answer, shared globals or ambiguous browser storage.

Required regression coverage:

- Practice A + Practice B open simultaneously.
- Different lessons and different questions.
- Alternating submissions.
- Correct answers remain correct.
- Incorrect answers remain incorrect.
- Same lesson with two separate Practice sessions.
- Refreshing one tab does not alter another.
- Completing one attempt does not invalidate another.

### 1B. Curriculum duplicate Core/Extended values

Trace duplication through database/query/join/projection/variant/filter-option/frontend layers. If records represent the same entity, deduplicate at source. If tracks are legitimately distinct, expose the distinction explicitly. Never hide the issue with CSS.

### 1C. Authorization regression protection

Server-side tests must prove:

```text
Teacher → assigned classes/students only
Supervisor → permitted academic scope only
School Administrator → current school only
```

Client filters are never authorization.

### Exit gate

Correctness/data-integrity blockers and their regression tests pass before broad UI migration.

---

## Phase 2 — Shared academic query foundation

Create reusable authorized, server-aware query contracts for:

- academic year;
- curriculum;
- grade/level;
- class;
- subject;
- search term;
- assignment/enrollment/active status;
- sorting;
- page/page size.

Target model:

```text
Academic Year
→ Curriculum
→ Grade / Level
→ Class
→ Teacher(s)
→ Students
```

Do not load the entire school population into the browser. Prevent N+1 query regressions.

### Exit gate

Reusable, authorized, paginated query services support the remaining role UIs.

---

## Phase 3 — Shared UI components and information architecture

Build/reuse consistent components for:

- search;
- filter bars;
- academic-year/curriculum/grade/class/subject selectors;
- paged user tables;
- multi-select;
- selected-count indicator;
- status badges;
- confirmation dialogs;
- empty/loading/error states;
- class summary cards.

Class summaries should consistently show class name, academic year, curriculum, grade/level, teacher(s), student count and status.

### Exit gate

Shared components are stable before role-specific migrations.

---

## Phase 4 — Admin Manage Users

### Students

Primary navigation:

```text
Academic Year
→ Curriculum
→ Grade / Level
→ Class
→ Students
```

Also retain direct global student search with contextual result information and direct navigation to student/class.

Support server-side search, filters, sort, pagination and status.

### Staff

Separate Teachers, Subject Supervisors and School Administrators. Show current assignment context inline and provide role-appropriate filters.

### Exit gate

Large school datasets are usable without uncontrolled flat lists.

---

## Phase 5 — Student enrollment and class movement

Keep enrollment and movement responsibilities distinct.

Required move workflow:

```text
Select academic context
→ From Class
→ Find students
→ Select student(s)
→ To Class
→ Review
→ Confirm
→ Execute
→ Refresh both classes
→ Preserve history/audit where supported
```

Block same-source/destination and incompatible academic contexts unless explicitly supported.

### Exit gate

Single and bulk movement are explicit, safe and immediately reflected.

---

## Phase 6 — Supervisor teacher assignment

Use shared queries/components and filter candidates by academic year, curriculum, subject, grade/level, class, teacher and assignment status. Show subject/current classes/curriculum/assignment state inline. Enforce supervisor scope on the server.

### Exit gate

The correct teacher can be assigned without browsing an uncontrolled school-wide list.

---

## Phase 7 — Curriculum layout and responsiveness

After fixing source data duplication, fix overlap through correct grid/flex sizing, wrapping, overflow handling and responsive breakpoints. Test long names and desktop/laptop/tablet/smaller supported widths. Do not patch with arbitrary fixed margins.

### Exit gate

No overlap/clipping across supported sizes.

---

## Phase 8 — Student dashboard information architecture

Dashboard must answer:

1. What do I need to do?
2. What is active?
3. What have I completed?
4. What is awaiting result/review?
5. How am I performing?
6. What should I practice next?

Separate assessment states:

```text
To Do
Active
Submitted / In Review
Awaiting Offline Result
Completed
```

Clearly distinguish online/offline assessment behavior. Use existing states where possible instead of inventing presentation-only state.

### Exit gate

Student next actions and statuses are immediately understandable.

---

## Phase 9 — Reporting

Inventory metrics as:

```text
AVAILABLE_NOW
DERIVABLE_RELIABLY
NOT_CURRENTLY_AVAILABLE
```

Display only supported/derivable metrics.

Recommended structure:

```text
Student Context
Summary KPIs
Mastery by Domain / Topic / Outcome
Progress Over Time
Assessments
Practice Activity
Strengths / Weak Areas
Detailed Evidence
```

Every displayed metric must be traceable to real stored data or a deterministic calculation.

---

## Phase 10 — Performance and responsive hardening

Test representative schools with hundreds/thousands of students, many classes/teachers, multiple curricula and academic years.

Verify:

- server-side pagination/filtering;
- indexed query paths where required;
- no unnecessary full-school payloads;
- no major N+1 queries;
- stable filter/pagination behavior;
- loading/error/no-results states;
- responsive behavior without destroying desktop density.

---

## Phase 11 — End-to-end role regression

### Administrator
User management, search, class navigation, staff management, school boundaries.

### Supervisor
Academic structure, class management, teacher assignment, student visibility, scope restrictions.

### Teacher
Assigned classes/students only, assessment workflows, Practice-related workflows.

### Student
Dashboard, assessment states, Practice, multi-tab Practice, results and mastery.

Also regression-test existing curriculum hierarchy, academic years, accounts, assessment creation/approval, mastery history and production routes.

---

## Phase 12 — Production gate

Do not call the work complete because the UI looks improved.

Required:

- [ ] Practice multi-tab root cause fixed.
- [ ] Practice regression tests.
- [ ] Duplicate curriculum levels fixed at source.
- [ ] Curriculum overlap fixed responsively.
- [ ] Server-side authorization preserved.
- [ ] Shared academic context queries.
- [ ] Class-first student navigation.
- [ ] Global student search.
- [ ] Scalable staff management.
- [ ] Scalable teacher assignment.
- [ ] Safe student movement.
- [ ] Shared components across roles.
- [ ] Clear student dashboard states.
- [ ] Reports use real data only.
- [ ] Server-aware filtering/pagination.
- [ ] N+1 checks.
- [ ] Responsive checks.
- [ ] Full regression and CI green.
- [ ] Production deployment healthy.

---

## Commit / PR strategy

Keep changes reviewable and revertible:

1. Baseline + regression tests.
2. Practice attempt isolation/correctness.
3. Curriculum duplicate-level root-cause fix.
4. Shared authorized query foundation.
5. Shared academic navigation/UI components.
6. Admin Manage Users.
7. Enrollment/student movement.
8. Supervisor teacher assignment.
9. Curriculum responsive layout.
10. Student Dashboard.
11. Reporting.
12. Performance/responsive hardening/final regression.

Do not mix Practice correctness with unrelated visual redesign. Do not mix curriculum data correction with cosmetic layout unless inseparable.

Every commit/PR should state:

```text
Problem
Root cause
Files changed
Behavior changed
Behavior intentionally unchanged
Tests added/changed
Known remaining work
```

## Execution rules

For every phase:

1. Inspect existing implementation first.
2. Reuse sound architecture.
3. Identify root cause before editing.
4. Implement the smallest coherent architectural change.
5. Add/update tests.
6. Run targeted tests.
7. Run broader regression.
8. Record changes.
9. Commit logically.
10. Continue only after the phase exit gate passes.

Unsupported or unresolved correctness/authorization/data-integrity failures block production promotion.
