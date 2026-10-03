# EDULYTICS DIRECT STUDENT + MOBILE COMMERCE MASTER EXECUTION PLAN

**Status:** Master planning document  
**Product:** Edulytics  
**Scope:** Direct Student Premium subscriptions + secure online payments + Android/iOS delivery  
**Supersedes:** `docs/EDULYTICS_MOBILE_APP_STORE_ROADMAP.md`

---

# 1. Purpose

This document replaces the previous mobile-only roadmap.

The correct implementation order is:

```text
Existing Edulytics Web Platform
        ↓
Direct Student Identity & Access Model
        ↓
Premium Subscription Model
        ↓
Secure Payment Architecture
        ↓
Entitlement Engine
        ↓
Direct Student Web Experience
        ↓
Security Hardening & QA
        ↓
Mobile App Foundation
        ↓
Android / iOS Commerce Integration
        ↓
Store Testing & Release
```

The mobile application must not be implemented as an isolated wrapper that ignores subscription, payment, and entitlement requirements.

The Direct Student commercial model must exist first so that Web, Android, and iOS all rely on the same authoritative access model.

---

# 2. Core Business Model

Edulytics will support two independent customer paths.

```text
Edulytics
│
├── School / B2B
│   ├── School Admin
│   ├── Subject Supervisor
│   ├── Teacher
│   └── School Student
│
└── Direct Student / B2C
    └── Premium Student
```

The existing School/B2B system remains intact.

The Direct Student model is added as a separate access source.

---

# 3. Non-Negotiable Architecture Rule

Do **not** convert Direct Students into fake schools.

Do **not** redesign the entire product around nullable school relationships.

The target model is:

```text
Student Identity
     ↓
Effective Access
     ├── School Enrollment Entitlements
     └── Personal Subscription Entitlements
```

This allows one user to be:
- a school student;
- a Direct Premium student;
- or both at the same time.

The final access available to a student is the union of all valid entitlements.

---

# 4. Direct Student Premium Product

There is only one commercial product family:

## Edulytics Premium

There is:
- no Free plan;
- no freemium tier;
- no trial tier unless a future business decision explicitly adds one.

The user chooses the billing duration.

---

# 5. Pricing Model

## 5.1 Monthly

```text
Premium Monthly
20 AED
Duration: 1 month
```

## 5.2 Annual Plan — 10-Month Subscription Term

Edulytics uses the commercial label **Annual Plan**, but the subscription term is **10 months**.

Base calculation:

```text
20 AED × 10 months = 200 AED
30% discount = 60 AED
Final price = 140 AED
```

Therefore:

```text
Premium Annual
Duration: 10 months
Price before discount: 200 AED
Discount: 30%
Final price: 140 AED
```

No 12-month entitlement must be created by this plan.

No marketing or backend logic should silently convert this product to 12 months.

---

# 6. Currency Strategy

The business reference price is AED.

The system must support students outside the UAE and outside Poland.

Target behavior:

```text
Base commercial price
        ↓
AED reference price
        ↓
Payment provider / pricing layer
        ↓
Local display or payment currency where supported
```

Examples may include:
- AED;
- PLN;
- EUR;
- GBP;
- USD;
- other supported currencies.

The system must never trust a client-supplied converted amount.

The backend or approved payment provider configuration must determine the authoritative amount.

---

# 7. Subscription Selection

During subscription, the Direct Student must choose:

1. Curriculum
2. Grade
3. Subject
4. Billing duration:
   - Monthly
   - Annual / 10-month term

Flow:

```text
Create / Sign in to account
        ↓
Choose Curriculum
        ↓
Choose Grade
        ↓
Choose Subject
        ↓
Choose Premium Monthly or Premium Annual
        ↓
Review price
        ↓
Pay
        ↓
Verified payment confirmation
        ↓
Create personal entitlement
        ↓
Premium access activated
```

---

# 8. Curriculum / Grade / Subject Lock

This is a fixed business rule.

After successful payment and activation:

- the student cannot change Curriculum;
- the student cannot change Grade;
- the student cannot change Subject.

The entitlement is permanently tied to the purchased selection for the active subscription term.

Example:

```text
Premium Entitlement
Curriculum = Cambridge
Grade = Stage 6
Subject = Mathematics
Term = Monthly
```

The student cannot edit those fields.

If the student wants a different:
- Curriculum;
- Grade;
- Subject;

the student must purchase a **new separate subscription**.

No "change plan content" button should exist for an active entitlement.

---

# 9. Multiple Personal Subscriptions

The architecture should support more than one personal entitlement per account.

Example:

```text
Student Account
├── Cambridge / Grade 6 / Mathematics
└── Common Core / Grade 7 / Mathematics
```

Each subscription has its own:
- payment record;
- start date;
- end date;
- selected curriculum;
- selected grade;
- selected subject;
- subscription status.

This avoids forcing the student to create multiple accounts.

---

# 10. Direct Student Academic Experience

A Direct Student does not belong to:
- a school class;
- a teacher allocation;
- a teacher's assessment workflow.

Therefore the Direct Student must not receive school-only academic concepts.

## Excluded for Direct Students

Do not expose:
- Teacher Assigned Assessment
- Teacher Grade
- Teacher Comments
- Teacher Homework
- Class Ranking
- Classroom Report
- School-specific assessment allocation

## Included for Direct Students

Premium should support:

- Lessons
- Practice
- Auto-marking
- Knowledge Checks
- Diagnostic Tests
- Progress Checks
- Mastery tracking
- Weak Areas
- Learning history
- Personal recommendations
- AI Tutor
- automated progress analytics

The distinction is:

```text
School Student
├── Automated Learning Evaluation
└── Teacher / Classroom Evaluation

Direct Premium Student
└── Automated Learning Evaluation only
```

The Direct Student may receive automated scores and mastery indicators, but not a formal teacher grade.

---

# 11. Direct Student Dashboard

The Direct Student dashboard should prioritize personal learning rather than school operations.

Recommended areas:

```text
Dashboard
├── Continue Learning
├── My Premium Subscription(s)
├── Curriculum / Grade / Subject
├── Progress
├── Weak Areas
├── Practice
├── Diagnostic Checks
├── AI Tutor
└── Account
```

Hide school-only modules when no school entitlement exists.

---

# 12. Subscription Data Model

Recommended domain concepts:

```text
PersonalSubscription
PersonalEntitlement
SubscriptionPlan
PaymentTransaction
PaymentProviderReference
EntitlementAudit
```

A conceptual record:

```text
PersonalSubscription
- Id
- StudentUserId
- PlanType
- Status
- StartAt
- EndAt
- CurriculumId
- GradeId
- SubjectId
- BasePrice
- Currency
- PaidAmount
- PaymentProvider
- ExternalCustomerReference
- ExternalPaymentReference
- CreatedAt
- ActivatedAt
- CancelledAt
```

The exact schema should follow current Edulytics conventions after repository inspection.

---

# 13. Subscription Status Model

Required operational states:

```text
Pending
Active
PaymentFailed
PastDue
Cancelled
Expired
```

There is **no Refunded subscription state**.

There is **no Edulytics refund system**.

The product specification does not provide a refund workflow to students.

---

# 14. Expiry Behavior

When a subscription expires:

Do not delete:
- the user account;
- learning history;
- results;
- progress;
- mastery data;
- diagnostic history.

Instead:

```text
Account remains active
Historical learning data remains
Purchased Premium entitlement becomes inactive
Paid content becomes unavailable
```

If a new subscription is later purchased, previous learning history may be reused where relevant.

---

# 15. School + Personal Access Combination

A school student may also purchase a personal subscription.

Example:

```text
School access:
Cambridge / Grade 5 / Mathematics

Personal access:
Cambridge / Grade 6 / Mathematics
```

Effective access becomes:

```text
Effective Entitlements =
School Entitlements
+
Active Personal Entitlements
```

Do not duplicate:
- account;
- identity;
- progress records unnecessarily.

---

# 16. Payment Architecture

Edulytics must **not** collect raw card details.

Do not build a card form that sends:
- card number;
- CVV;
- expiry date;

through the Edulytics backend.

The required architecture is:

```text
Edulytics
   ↓
Backend creates secure checkout session
   ↓
Hosted Payment Page
   ↓
Payment Provider
   ↓
Card authorization
   ↓
Provider confirms payment
   ↓
Signed Webhook
   ↓
Edulytics verifies event
   ↓
Subscription activated
```

The payment page should be hosted by the payment provider.

---

# 17. Payment Provider Requirements

The selected provider must support:

- international cards;
- students in multiple countries;
- hosted checkout;
- server-side session creation;
- secure webhooks;
- payment status verification;
- recurring or repeat billing if required by the final product behavior;
- payout to the company bank account;
- multiple currencies where practical;
- strong account security;
- test / sandbox mode.

The provider must be selected separately before implementation.

The architecture must avoid hard-coding Edulytics to one provider where unnecessary.

---

# 18. Payment Security & Anti-Redirect Controls

Because the web application itself must not become the source of payment-account risk, the payment path requires specific controls.

## 18.1 Server-Created Checkout

The browser must never choose:
- merchant account;
- destination bank account;
- authoritative price;
- subscription entitlement.

The backend creates checkout sessions.

## 18.2 Server-Side Product Mapping

Example:

```text
Product Code:
DIRECT_PREMIUM_MONTHLY

Backend mapping:
Price = 20 AED
Duration = 1 month
```

and:

```text
Product Code:
DIRECT_PREMIUM_ANNUAL

Backend mapping:
Reference term = 10 months
Base = 200 AED
Discount = 30%
Final = 140 AED
Duration = 10 months
```

Never accept the final price directly from the client.

## 18.3 Webhook Verification

Entitlement activation must require:
- valid provider signature;
- expected event type;
- matching internal checkout record;
- correct amount;
- correct currency;
- successful payment status;
- unused event / idempotency check.

## 18.4 Success Page Is Not Proof of Payment

Do not activate Premium because the browser reached:

```text
/payment/success
```

The success screen is display-only.

The authoritative source is the verified server-side payment event.

## 18.5 Secret Management

Payment secrets must live in:
- protected environment variables;
- approved secrets management.

Never commit them to GitHub.

## 18.6 Payment Account Protection

Require:
- strong unique credentials;
- multi-factor authentication where supported;
- restricted administrative access;
- audit trail for configuration changes;
- payout-change alerts where supported;
- least-privilege access.

## 18.7 Redirect Integrity

Where supported:
- allow only approved return URLs;
- use server-generated sessions;
- prevent arbitrary return/destination parameters;
- validate internal state after return.

---

# 19. Payment Reconciliation

Edulytics must be able to reconcile its internal subscription data with provider records.

Recommended admin fields:

```text
Internal Subscription ID
Provider
Provider Payment ID
Student ID
Amount
Currency
Payment status
Entitlement status
Created
Activated
Expires
```

A mismatch must be visible to administrators.

---

# 20. Direct Student Administration

Add a dedicated admin area.

Recommended modules:

```text
Direct Students
Personal Subscriptions
Active Entitlements
Expired Entitlements
Payments
Failed Payments
Past-Due Accounts
Plans
Revenue Summary
Payment Reconciliation
Audit Log
```

There is no refund-management module in this specification.

---

# 21. Security Audit Before Payments

Payment capability must not be activated in production before a focused security review.

Review at minimum:

- authentication;
- authorization;
- admin permissions;
- session handling;
- CSRF;
- secure cookies;
- HTTPS enforcement;
- secret storage;
- rate limiting;
- checkout creation endpoints;
- webhook endpoint;
- entitlement creation;
- privilege escalation;
- IDOR/BOLA risks;
- logging;
- error leakage.

High-risk findings affecting payment or entitlement integrity must be fixed before launch.

---

# 22. Phase Order

The implementation sequence is mandatory because the mobile application depends on the subscription and entitlement architecture.

---

# PHASE 0 — Current-System Audit

## Goal

Understand the existing Edulytics identity, student, school, curriculum, lesson, practice, assessment, and authorization models.

Audit:
- User
- Student profile
- School membership
- Class membership
- curriculum assignment
- grade assignment
- subject assignment
- published lessons
- practice
- assessments
- AI Tutor
- progress
- authorization policies

## Deliverable

A technical mapping document showing exactly where personal entitlement support must be introduced without breaking school access.

---

# PHASE 1 — Entitlement Foundation

## Goal

Create a common access layer.

Target concept:

```text
Student requests learning content
        ↓
Entitlement Service
        ↓
School access?
Personal access?
        ↓
Combined effective access
        ↓
Authorized content
```

Do not spread subscription checks manually across every controller/view.

Centralize them.

---

# PHASE 2 — Direct Student Identity

Add support for a student account that has no school enrollment.

Required behavior:
- register/sign in;
- maintain student profile;
- no fake school;
- no fake class;
- no teacher requirement;
- route to Direct Student experience when appropriate.

Existing School Student behavior must remain unchanged.

---

# PHASE 3 — Premium Product Catalog

Create product definitions for:

```text
DIRECT_PREMIUM_MONTHLY
Price: 20 AED
Duration: 1 month
```

```text
DIRECT_PREMIUM_ANNUAL
Reference term: 10 months
Base price: 200 AED
Discount: 30%
Final price: 140 AED
Duration: 10 months
```

Add versioning so historical payments keep their original commercial terms if pricing changes later.

---

# PHASE 4 — Subscription Selection Flow

Build:

```text
Curriculum
→ Grade
→ Subject
→ Duration
→ Review
→ Checkout
```

Validation:
- selected curriculum exists;
- grade belongs to available curriculum path;
- subject is valid;
- content exists;
- product is active;
- selection is saved before external checkout.

The student cannot change these dimensions after activation.

---

# PHASE 5 — Payment Provider Integration

Implement:
- provider abstraction;
- secure hosted checkout creation;
- callback/return handling;
- signed webhook;
- payment status persistence;
- idempotency;
- reconciliation references.

No raw card storage.

No browser-controlled pricing.

---

# PHASE 6 — Subscription Activation

After verified successful payment:

```text
Payment verified
    ↓
PersonalSubscription = Active
    ↓
PersonalEntitlement created
    ↓
Curriculum locked
Grade locked
Subject locked
    ↓
End date calculated
    ↓
Premium access available
```

Activation must be transactional where possible.

Avoid:
- payment success with missing entitlement;
- duplicate entitlement due to duplicate webhook.

---

# PHASE 7 — Direct Student Learning Experience

Adapt the portal based on effective access.

Direct Student should receive:
- My Learning
- Practice
- automated diagnostics
- progress
- weak areas
- mastery
- AI Tutor

Do not show school-only modules that require:
- teacher;
- class;
- school assessment.

---

# PHASE 8 — Expiry & Cancellation

Implement:
- expiry;
- cancellation state;
- failed payment handling;
- past-due handling.

Expired or cancelled Premium access must not delete academic history.

No refund workflow is part of the system.

---

# PHASE 9 — B2C Administration

Create operational tools for:
- user lookup;
- subscription lookup;
- entitlement status;
- payment reference;
- expiry;
- plan;
- curriculum/grade/subject;
- reconciliation;
- audit history.

Administration actions must be permission-protected.

---

# PHASE 10 — Security Hardening

Before production billing:
- threat-model checkout;
- verify payment secrets;
- test webhook forgery attempts;
- test duplicate events;
- test price manipulation;
- test entitlement escalation;
- test unauthorized subscription lookup;
- test admin authorization;
- confirm secure logging.

Production payment must remain disabled until the security gate passes.

---

# PHASE 11 — Web Launch

Recommended release sequence:

```text
Development
↓
Staging / test payment mode
↓
Internal B2C QA
↓
Controlled production launch
↓
Monitoring
```

Validate international payment behavior with supported test scenarios before broad launch.

---

# 23. Mobile Architecture

Only after the Direct Student subscription and entitlement foundation is stable should the mobile work be promoted to store release.

Recommended mobile technology:

**Capacitor**

Target architecture:

```text
Edulytics Backend
        ↑
        │
Unified Identity / Entitlement / Subscription Services
        ↑
   ┌────┴────┐
   │         │
Android      iOS
```

The mobile application must use the same user identity and entitlement source of truth.

---

# PHASE 12 — Mobile Workspace

Create:

```text
/mobile
  /android
  /ios
  /resources
  /scripts
  capacitor.config.ts
  package.json
  README.md
```

The mobile workspace must be isolated from production web code where possible.

---

# PHASE 13 — Mobile Authentication

Validate:
- login;
- logout;
- session persistence;
- expired session;
- password reset;
- Direct Student account;
- School Student account;
- combined school + personal access.

Do not store raw passwords in the app.

---

# PHASE 14 — Mobile Responsive Audit

Test:
- small Android phone;
- large Android phone;
- iPhone;
- larger iPhone;
- Android tablet;
- iPad.

Important:
- hover-only interactions need touch equivalents;
- lesson cards must be tappable;
- payment and account screens must fit small displays;
- Arabic RTL must work;
- Polish and English text expansion must work.

---

# PHASE 15 — Mobile Subscription UX

The mobile UI must understand the same Premium product model:

```text
Curriculum
Grade
Subject
Monthly / Annual
```

However, the actual purchase mechanism inside Android/iOS must follow the current applicable platform/store rules at the time of implementation and submission.

Do not assume the Web Hosted Checkout flow can automatically be reused unchanged inside store-distributed apps.

The shared rule is:

```text
Whatever approved purchase channel is used
        ↓
Verified purchase
        ↓
Edulytics backend
        ↓
PersonalSubscription
        ↓
PersonalEntitlement
```

---

# 24. Commerce Abstraction

The backend should be designed to accept verified entitlement sources such as:

```text
Web Payment Provider
Apple Purchase Source
Google Play Purchase Source
```

All sources must result in the same Edulytics entitlement model.

Concept:

```text
Purchase Source
     ↓
Verification Adapter
     ↓
Normalized Subscription Event
     ↓
PersonalSubscription
     ↓
PersonalEntitlement
```

This prevents mobile commerce from creating a parallel access system.

---

# PHASE 16 — Android Build

Deliver:
- Android project;
- development APK;
- production Android App Bundle;
- release signing configuration;
- application identity;
- icons;
- splash screen;
- deep-link configuration;
- network configuration;
- file/camera permissions if required.

Secrets and signing credentials must not be committed.

---

# PHASE 17 — iOS Build

Deliver:
- iOS project;
- Xcode configuration;
- bundle identifier;
- signing;
- icons;
- launch assets;
- entitlements;
- device testing;
- TestFlight candidate.

---

# PHASE 18 — Native Integrations

Initial integrations:
- status bar;
- splash screen;
- secure external link handling;
- file picker;
- camera/photo access where required;
- downloads/open-in behavior;
- deep links.

Potential later integrations:
- push notifications;
- share sheet;
- biometric re-entry;
- limited offline capabilities.

---

# PHASE 19 — Mobile Security

Test:
- malicious deep links;
- intercepted navigation;
- external URL handling;
- WebView origin restrictions;
- authentication leakage;
- token/cookie persistence;
- screenshot/log leakage where applicable;
- file permissions;
- compromised return URLs;
- entitlement manipulation.

The mobile app must never contain:
- payment secrets;
- backend administrator credentials;
- database credentials.

---

# PHASE 20 — Android / iOS Store Preparation

Prepare:
- app name;
- production icons;
- screenshots;
- app description;
- privacy policy;
- support URL;
- age rating;
- data/privacy disclosures;
- review credentials;
- test account.

The exact store requirements must be revalidated against current Google Play and Apple App Store rules at submission time.

---

# PHASE 21 — Mobile QA

Test both account models.

## School Student

- login;
- school learning;
- teacher-assigned content;
- assessments;
- results;
- progress.

## Direct Student

- login;
- subscription visibility;
- selected curriculum;
- selected grade;
- selected subject;
- Premium access;
- practice;
- diagnostics;
- AI Tutor;
- progress;
- expiry behavior.

## Combined Student

Verify union of:
- School entitlement;
- Personal entitlement.

---

# PHASE 22 — Store Release

Recommended order:

```text
Android internal testing
↓
Android controlled release

iOS TestFlight
↓
App review
↓
iOS controlled release
```

Do not publish a mobile application before Direct Student purchase/entitlement behavior is stable.

---

# 25. Data & Privacy

Because Edulytics serves students and may be used by minors, minimize data collection.

Maintain an inventory of:
- user identity;
- school association where applicable;
- personal subscription;
- curriculum choice;
- grade choice;
- subject choice;
- payment reference;
- learning activity;
- assessment/practice results;
- progress;
- device identifiers where used;
- push tokens where used.

Do not store raw card details.

Payment-provider data should be limited to the references needed for:
- verification;
- reconciliation;
- support;
- entitlement status.

---

# 26. No-Refund Product Rule

This master specification contains **no refund subsystem**.

There is:
- no `Refunded` subscription state;
- no student refund workflow;
- no refund administration module.

If future legal or operational requirements require separate handling, that requires a new approved specification and must not be silently introduced into this implementation.

---

# 27. Audit Logging

Audit events should include:

- Direct Student account creation;
- checkout initiated;
- checkout session created;
- payment verified;
- payment failed;
- subscription activated;
- subscription expired;
- subscription cancelled;
- entitlement created;
- entitlement disabled;
- admin modification;
- payment configuration change where observable.

Avoid logging:
- raw payment card data;
- secrets;
- sensitive authentication tokens.

---

# 28. Feature Flags

Recommended:

```text
DirectStudentRegistrationEnabled
DirectStudentCheckoutEnabled
DirectStudentPremiumEnabled
DirectStudentDiagnosticsEnabled
DirectStudentAITutorEnabled
MobileAppEnabled
MobileCommerceEnabled
```

These permit controlled rollout without disabling school functionality.

---

# 29. Rollback Strategy

## Web / B2C

If Direct Student checkout has a critical issue:
- disable `DirectStudentCheckoutEnabled`;
- keep school product operational;
- preserve existing active entitlements;
- investigate without changing school enrollments.

## Mobile

If a mobile release has a critical issue:
- stop store rollout where available;
- issue a patched build;
- disable high-risk mobile features via feature flags where possible.

Backend changes must maintain compatibility with published mobile versions for an appropriate support window.

---

# 30. CI/CD

## Web/B2C

Every change should pass:
- build;
- automated tests;
- authorization tests;
- payment-domain tests;
- regression checks.

## Mobile

Build Android and iOS separately.

Store publication should require explicit approval.

Do not make automatic production-store release the first implementation.

---

# 31. Testing Matrix

At minimum:

| Area | School Student | Direct Student | Combined Student |
|---|---:|---:|---:|
| Login | ✓ | ✓ | ✓ |
| Learning | ✓ | ✓ | ✓ |
| School Assessment | ✓ | N/A | ✓ via school |
| Practice | ✓ | ✓ | ✓ |
| Automated Progress | ✓ | ✓ | ✓ |
| AI Tutor | based on entitlement | ✓ | based on entitlement |
| Personal Subscription | optional | ✓ | ✓ |
| Curriculum Lock | N/A | ✓ | ✓ |
| Grade Lock | N/A | ✓ | ✓ |
| Subject Lock | N/A | ✓ | ✓ |
| Expiry | school rules | ✓ | personal entitlement only |

---

# 32. Direct Student Definition of Done

Direct Student B2C is ready only when:

- [ ] student can exist without school;
- [ ] no fake school is created;
- [ ] Premium Monthly is configured at 20 AED;
- [ ] Premium Annual uses a 10-month term;
- [ ] Annual base is 200 AED;
- [ ] Annual discount is 30%;
- [ ] Annual final price is 140 AED;
- [ ] student selects Curriculum;
- [ ] student selects Grade;
- [ ] student selects Subject;
- [ ] selections are immutable after activation;
- [ ] new selection requires new subscription;
- [ ] hosted checkout works;
- [ ] Edulytics never receives raw card data;
- [ ] payment is verified server-side;
- [ ] forged success URL cannot activate access;
- [ ] altered browser price cannot activate access;
- [ ] duplicate webhook cannot duplicate entitlement;
- [ ] expired access locks Premium content;
- [ ] learning history remains;
- [ ] school access remains unaffected;
- [ ] no refund subsystem exists.

---

# 33. Mobile Definition of Done

Mobile delivery is ready only when:

- [ ] Direct Student entitlement system is stable;
- [ ] Android build succeeds;
- [ ] iOS build succeeds;
- [ ] school students work;
- [ ] Direct Students work;
- [ ] combined students work;
- [ ] Curriculum/Grade/Subject entitlements are enforced;
- [ ] mobile commerce uses an approved purchase flow;
- [ ] purchase verification reaches Edulytics backend;
- [ ] no payment secret exists in client package;
- [ ] authentication is secure;
- [ ] Arabic/English/Polish are validated;
- [ ] phone layouts pass QA;
- [ ] tablet layouts pass QA where supported;
- [ ] store assets are complete;
- [ ] privacy declarations are complete;
- [ ] controlled testing passes.

---

# 34. Master Milestones

## M1 — Current Architecture Audit
Map existing student, school, curriculum, grade, subject, and authorization behavior.

## M2 — Entitlement Engine
Create unified School + Personal effective access.

## M3 — Direct Student Accounts
Allow student accounts without school enrollment.

## M4 — Premium Catalog
Create Monthly and 10-month Annual Premium products.

## M5 — Subscription Selection
Curriculum + Grade + Subject + duration.

## M6 — Secure Payments
Hosted checkout + verified webhook + reconciliation.

## M7 — Premium Activation
Create immutable personal entitlement.

## M8 — Direct Student Portal
Learning, practice, diagnostics, progress, weak areas, AI Tutor.

## M9 — B2C Admin
Subscriptions, entitlements, payments, reconciliation, audit.

## M10 — Security Gate
Complete payment and entitlement security review.

## M11 — Web B2C Launch
Controlled Direct Student launch.

## M12 — Mobile Foundation
Capacitor + Android + iOS.

## M13 — Mobile Identity & Entitlements
School, Direct, and combined account support.

## M14 — Mobile Commerce
Integrate approved Android/iOS purchase path with the same backend entitlements.

## M15 — Store QA
Device, privacy, security, review-account, and release testing.

## M16 — Android & iOS Launch
Controlled production releases.

---

# 35. Final Target Architecture

```text
                         ┌─────────────────────┐
                         │   Edulytics Users   │
                         └──────────┬──────────┘
                                    │
                 ┌──────────────────┴──────────────────┐
                 │                                     │
          School Student                       Direct Student
                 │                                     │
         School Enrollment                     Premium Purchase
                 │                                     │
                 │                            Curriculum + Grade
                 │                                + Subject
                 │                                     │
                 └──────────────┬──────────────────────┘
                                │
                        Entitlement Engine
                                │
               ┌────────────────┼────────────────┐
               │                │                │
              Web            Android            iOS
               │                │                │
               └────────────────┼────────────────┘
                                │
                       Edulytics Backend
                                │
                     Progress / Learning Data
```

Payments remain outside the raw-card-data boundary of Edulytics.

```text
Edulytics Backend
     ↓
Secure Hosted / Approved Purchase Flow
     ↓
Payment Platform
     ↓
Verified Server Event
     ↓
Subscription + Entitlement
```

---

# 36. First Implementation Action

Do **not** start with APK generation.

Do **not** start with a payment button.

Do **not** change School Student behavior first.

The correct first execution task is:

> **Audit the current Student/School/Curriculum authorization architecture and design the unified Entitlement Engine that can safely add Direct Student Premium access without changing existing school enrollment behavior.**

Only after that foundation is approved should payment implementation and mobile packaging proceed.
