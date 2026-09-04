# Edulytics — Phase 38 Final Cleanup Manifest

**Status:** PREPARED — NOT EXECUTED  
**Repository baseline:** `main` @ `60c69918477070210a4c455331c71ea9b8247ffc`  
**Staging Neon branch:** `br-spring-morning-axweyoti`  
**Rollback branch:** `br-dawn-king-axvrh6jx` (`pre-final-cleanup-prep-20260904-phase43`)  
**Rollback branch parent LSN:** `0/1AACBF10`  

This manifest is deliberately non-destructive. It records the verified cleanup scope and dependency order. No `DELETE`, `TRUNCATE`, schema change, or production-data mutation is authorized by this document.

## 1. Preservation invariants

The final cleanup must preserve all of the following exactly unless a separately reviewed migration explicitly changes them:

- the single active **global SuperAdmin** (`SchoolId IS NULL`);
- all **5 system roles**;
- all **5 global Curriculum Frameworks** (`OwnerSchoolId IS NULL`):
  - `CAMBRIDGE-INTL-MATH`
  - `PL-NATIONAL-MATH`
  - `UAE-MOE-MATH`
  - `UK-NC-ENG-MATH`
  - `US-CCSS-MATH`
- all Curriculum Framework Versions;
- the official curriculum corpus:
  - CurriculumPackContentNodes
  - CurriculumPackNodeLinks
  - CurriculumPedagogicalLessons
  - CurriculumPedagogicalLessonOutcomes
  - CurriculumLessonContents
  - CurriculumLessonContentTranslations
- `__EFMigrationsHistory`;
- `DataProtectionKeys` and other required global/system configuration.

### Verified preservation baseline

| Item | Current rows |
|---|---:|
| CurriculumFrameworks | 5 |
| CurriculumFrameworkVersions | 5 |
| CurriculumPackContentNodes | 2,313 |
| CurriculumPackNodeLinks | 48 |
| CurriculumPedagogicalLessons | 5,094 |
| CurriculumPedagogicalLessonOutcomes | 5,037 |
| CurriculumLessonContents | 4,453 |
| CurriculumLessonContentTranslations | 4,454 |
| AspNetRoles | 5 |
| __EFMigrationsHistory | 21 |

## 2. Current operational cleanup scope

Staging currently contains exactly one school tenant:

- `Phase 3 Acceptance School`
- School ID: `fcb660e6-10bc-4dcb-88a6-85ab69aca38c`

Current tenant users:

- 4 active school users: SchoolAdmin, SubjectSupervisor, Teacher, Student;
- 1 disabled temporary Phase43 acceptance user;
- 5 school user-role links total.

The global SuperAdmin is outside this tenant and must not be touched.

### Tenant academic/commercial data currently present

| Item | Rows |
|---|---:|
| AcademicYears | 1 |
| AcademicPrograms | 2 |
| AcademicYearProgramOfferings | 2 |
| SchoolCurriculumAdoptions | 2 |
| GradeLevels | 2 |
| Subjects | 1 |
| Terms | 1 |
| ClassGroups | 3 |
| CurriculumTopics | 1 |
| LearningOutcomes | 1 |
| StudentProfiles | 1 |
| StudentEnrollments | 1 |
| TeacherAssignments | 2 |
| SubjectSupervisorAssignments | 0 |
| Assessments | 1 |
| SchoolSubscriptions | 1 |
| SchoolTrials | 1 |
| SchoolBillingProfiles | 1 |
| SubscriptionSeatChanges | 1 |

### Tenant transient/operational rows currently present

| Item | Rows |
|---|---:|
| AuditLogs | 34 |
| IdempotencyRecords | 5 |
| UserNotifications | 4 |
| NotificationDeliveryJobs | 4 |
| OutboxMessages | 4 |
| ReportExportJobs | 0 |
| ImportBatches | 0 |
| ImportValidationErrors | 0 |
| AnalyticsRefreshStates | 0 |
| SchoolAnalyticsSnapshots | 0 |
| DemoAccesses | 0 |
| DemoRequests referencing tenant | 0 |

### Learning/assessment activity currently zero

The following are currently empty, which materially lowers final-cleanup risk:

- AssessmentItems
- AssessmentQuestions
- AssessmentItemOutcomes
- QuestionLearningOutcomes
- AssessmentResults
- StudentAnswers
- PracticeAttempts
- PracticeAttemptItems
- PracticeResponses
- LearningEvidence
- StudentItemExposures
- StudentOutcomeMasteries
- ClassOutcomeSummaries
- ClassTopicSummaries
- ClassAssessmentTrends
- ReportExportJobs
- ImportBatches
- ImportValidationErrors

## 3. Why direct school deletion is unsafe

The database schema was audited directly. Most foreign keys pointing at `Schools` use **RESTRICT**, not CASCADE. Only selected relationships such as `SchoolTrials` / `AnalyticsRefreshStates` use CASCADE.

Therefore this is **not** an acceptable cleanup strategy:

```text
Delete School
→ hope everything cascades
```

The final cleanup must use one dependency-ordered transaction, explicit row-count verification, preservation guards, and rollback on any mismatch.

## 4. Candidate dependency order

This is the reviewed logical order for the future cleanup transaction. It is a manifest only; it is not executable deletion SQL.

### A. Delivery / transient state

1. NotificationDeliveryJobs
2. UserNotifications
3. OutboxMessages
4. ReportExportJobs
5. ImportValidationErrors
6. ImportBatches
7. IdempotencyRecords
8. AnalyticsRefreshStates
9. SchoolAnalyticsSnapshots
10. AuditLogs
11. DemoAccesses / DemoRequests references if any appear before execution

### B. Learning evidence / generated activity children

1. PracticeResponses
2. PracticeAttemptItems
3. LearningEvidence
4. StudentItemExposures
5. StudentAnswers
6. AssessmentItemOutcomes
7. QuestionLearningOutcomes
8. ClassAssessmentTrends
9. ClassOutcomeSummaries
10. ClassTopicSummaries
11. StudentOutcomeMasteries
12. AssessmentResults

### C. Assessments / practice parents

1. PracticeAttempts
2. AssessmentItems
3. AssessmentQuestions
4. Assessments

### D. Enrollment / assignments / student profile

1. StudentEnrollments
2. TeacherAssignments
3. SubjectSupervisorAssignments
4. StudentProfiles

### E. Billing / commercial tenant state

1. BillingRefunds
2. BankTransferPayments
3. BillingInvoiceLines
4. BillingInvoices
5. SubscriptionSeatChanges
6. SchoolBillingProfiles
7. SchoolSubscriptions
8. SchoolTrials

### F. School-scoped academic/curriculum state

1. LearningLessonOutcomes
2. LearningLessonTranslations
3. LearningLessons
4. LearningOutcomes
5. CurriculumTopics
6. ClassGroups
7. SchoolCurriculumAdoptions
8. AcademicYearProgramOfferings
9. Terms
10. GradeLevels
11. Subjects
12. AcademicPrograms
13. AcademicYears

### G. School identity records

1. AspNetUserClaims for tenant users
2. AspNetUserLogins for tenant users
3. AspNetUserTokens for tenant users
4. AspNetUserRoles for tenant users
5. AspNetUsers where `SchoolId = target school`

### H. Tenant root

1. Verify every remaining table with `SchoolId = target school` is zero.
2. Verify there are no DemoRequests referencing the school through `DemoSchoolId` or `ProvisionedSchoolId`.
3. Delete the `Schools` row last.

## 5. Mandatory transaction guards for the future executable cleanup

Before any future COMMIT, the transaction must prove all of the following:

- target school count is exactly the approved tenant count;
- no global user is included in the deletion set;
- active global SuperAdmin count remains exactly 1;
- system-role count remains 5;
- global framework count remains 5;
- every CurriculumFramework has `OwnerSchoolId IS NULL` for the current five official frameworks;
- official curriculum corpus counts are unchanged from the pre-cleanup snapshot unless explicitly reviewed;
- migration history is unchanged;
- no school-scoped rows remain for the deleted tenant;
- no orphan FK rows remain;
- the final school count matches the approved target state.

Any mismatch must cause `ROLLBACK`, never partial cleanup.

## 6. Execution gate

Actual cleanup remains blocked until both conditions are satisfied:

1. final authenticated application acceptance is explicitly resolved/accepted;
2. destructive cleanup is explicitly approved.

Until then, only audit, backup, verification, and cleanup-script preparation are allowed.
