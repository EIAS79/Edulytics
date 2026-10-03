# Direct Student / Mobile Commerce — Phase 0 Architecture Audit

Date: 2026-10-03  
Branch: feat/direct-student-mobile-execution

## Executive finding

The existing Edulytics student model is school-first and must not be made nullable globally to support B2C.

### Current coupling

- `ApplicationUser.SchoolId` is nullable, so Identity can represent a user without a school.
- `StudentProfile.SchoolId` is required and the entity implements `ISchoolScoped`.
- `StudentEnrollment` requires School, ClassGroup and AcademicYear.
- `GradeLevel`, `Subject`, `CurriculumTopic`, and `LearningLesson` are school-scoped.
- `StudentPortalService` explicitly denies Student users without `SchoolId`.
- `StudentPortalRepository` builds the learning workspace from school enrollments and school curriculum adoptions.
- Curriculum frameworks and framework versions are global-capable; `CurriculumFramework.OwnerSchoolId` can be null.
- `CurriculumPackContentNode` is global by framework version and carries logical/native level identity and pathway data.
- Existing school billing is intentionally separate and includes school invoices/transfers/refunds. It must not be reused as the Direct Student commercial model.

## Architecture decision

Do not:
- create a fake school for B2C users;
- make `StudentProfile.SchoolId` nullable;
- make existing school GradeLevel/Subject rows global;
- route personal subscriptions through SchoolSubscription.

Add a parallel B2C domain:
- DirectStudentProfile
- PersonalSubscription
- PersonalEntitlement
- PersonalPaymentTransaction
- Direct Learning Catalog
- Entitlement Resolver

## Direct learning identity

Because current GradeLevel and Subject entities are school-scoped, a personal entitlement will use global curriculum identity:
- FrameworkVersionId
- CurriculumLevelKey / LogicalLevel / Label / Pathway
- SubjectCode / SubjectName

The initial production curriculum is mathematics-focused, so Mathematics is the first Direct catalog subject. The catalog remains extensible to additional subjects later.

## Portal migration strategy

The existing School Student portal remains unchanged.

A Direct Student workspace will be resolved separately and then presented through the same student-facing layout where practical.

Combined users will resolve effective access as:
School Entitlements + Active Personal Entitlements.

## Security constraints

- Raw card data never enters Edulytics.
- Checkout price and merchant destination are server-controlled.
- Payment success pages never grant access.
- Only verified provider events may activate personal entitlements.
- Payment configuration secrets stay outside source control.
- New Direct Student features are feature-flagged until the payment security gate passes.

## Phase 0 result

GO to Phase 1 with a parallel Personal Entitlement model and no destructive changes to the school domain.
