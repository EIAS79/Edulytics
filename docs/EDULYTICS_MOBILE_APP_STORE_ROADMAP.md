# Edulytics Mobile App & App Store Delivery Roadmap

**Document status:** Execution plan  
**Product:** Edulytics  
**Objective:** Deliver Edulytics as installable Android and iOS applications without destabilizing the existing production web application.  
**Primary principle:** The existing web application, backend, database, authentication, and production deployment remain the source system. Mobile support is added as an isolated client layer.

---

## 1. Executive Summary

Edulytics is already a production web application. The recommended path is **not** to rewrite the product in Flutter, React Native, or native Android/iOS code.

The preferred architecture is:

```text
Edulytics Web Application
        |
        | HTTPS / authenticated web session / APIs
        v
Capacitor Mobile Shell
   |                 |
Android             iOS
AAB / APK           Xcode / IPA
   |                 |
Google Play         App Store / TestFlight
```

The mobile applications should wrap and integrate with the existing Edulytics experience while adding only the native capabilities that are required.

This minimizes:
- regression risk;
- duplicate frontend development;
- long-term maintenance cost;
- divergence between web and mobile;
- risk to the current production system.

The mobile work must be isolated from the existing application and introduced incrementally.

---

# 2. Non-Negotiable Safety Rules

The mobile implementation must **not** destabilize the production web application.

## 2.1 Existing systems that remain unchanged by default

The following remain the source of truth:

- Edulytics production backend;
- Neon/PostgreSQL database;
- existing domain and HTTPS infrastructure;
- current user roles;
- current curriculum and lesson content;
- current assessment engine;
- current authentication and authorization policies;
- existing teacher, supervisor, school administrator, and student portals;
- current Render production deployment pipeline.

No mobile requirement should automatically justify changing these systems.

## 2.2 Isolation rules

Mobile work must be isolated in dedicated directories.

Recommended structure:

```text
/mobile
  /app
  /android
  /ios
  /resources
  /scripts
  capacitor.config.ts
  package.json
  README.md
```

Existing server code remains under the current application structure.

Any shared changes required by mobile must:
1. be implemented in a separate branch;
2. have explicit regression tests;
3. be validated on web before merge;
4. preserve desktop and browser behavior;
5. avoid mobile-only assumptions in shared application code.

## 2.3 Production protection

No mobile build may be allowed to:
- modify production database schemas implicitly;
- bypass role authorization;
- embed production secrets;
- store user passwords;
- weaken cookie/session security;
- expose privileged URLs;
- hard-code administrator credentials;
- disable CSRF/security protections merely to make the wrapper work.

---

# 3. Target Architecture

## 3.1 Recommended technology

Use **Capacitor** as the native bridge.

Capacitor provides:
- Android project generation;
- iOS project generation;
- native application lifecycle;
- access to device capabilities;
- browser/WebView integration;
- deep links;
- native plugins;
- push notification support;
- app icons and splash screens;
- platform-specific configuration.

## 3.2 Why Capacitor is preferred for Edulytics

Edulytics already has:
- a working responsive frontend;
- a working production backend;
- authenticated portals;
- multilingual UI;
- complex learning content;
- server-generated functionality;
- continuous web development.

A full native rewrite would duplicate a substantial portion of the product and create two or three independent frontends.

Capacitor allows us to preserve the existing product while adding mobile distribution.

---

# 4. Delivery Strategy

Implementation is divided into controlled phases.

```text
Phase 0  Mobile readiness audit
Phase 1  Mobile workspace foundation
Phase 2  Authentication/session integration
Phase 3  Mobile UI compatibility
Phase 4  Native integrations
Phase 5  Android build pipeline
Phase 6  iOS build pipeline
Phase 7  Security/privacy compliance
Phase 8  Testing and QA
Phase 9  Store assets and metadata
Phase 10 Google Play release
Phase 11 Apple App Store release
Phase 12 Monitoring and post-release maintenance
```

Each phase has a go/no-go gate.

---

# 5. Phase 0 — Mobile Readiness Audit

## Goal

Determine whether every critical Edulytics workflow behaves correctly inside a mobile WebView before packaging the app.

## Audit areas

### Student

Test:
- sign in;
- dashboard;
- My Learning;
- lesson opening;
- lesson navigation;
- practice;
- assessments;
- results;
- progress;
- notifications;
- language switching;
- logout.

### Teacher

Test:
- dashboard;
- class navigation;
- assessment creation;
- homework/allocation workflows;
- student result views;
- reporting;
- marking workflows;
- lesson access.

### Subject Supervisor

Test:
- operational dashboards;
- curriculum views;
- teacher oversight;
- reports;
- required administrative actions.

### School Administrator

Test:
- school dashboard;
- users;
- classes;
- curriculum configuration;
- reports;
- role management;
- administrative actions.

## Responsive audit

Validate at minimum:
- small Android phone;
- large Android phone;
- iPhone compact width;
- standard iPhone;
- large iPhone;
- Android tablet;
- iPad portrait;
- iPad landscape.

## Gate

Do not continue to native packaging until all critical user journeys are usable at mobile widths.

---

# 6. Phase 1 — Mobile Workspace Foundation

## Create dedicated mobile project

Create:

```text
/mobile
```

with its own:
- package manifest;
- Capacitor configuration;
- native Android project;
- native iOS project;
- mobile-specific assets;
- environment handling;
- build scripts.

## Application identity

Proposed initial identifiers:

```text
App name: Edulytics
Android applicationId: com.edulytics.app
iOS bundle identifier: com.edulytics.app
```

These identifiers must be confirmed before first store registration because changing them later is disruptive.

## Environments

Support at minimum:

```text
development
staging
production
```

Example:

```text
EDULYTICS_MOBILE_ENV=staging
EDULYTICS_BASE_URL=https://staging.edulytics...
```

Production URLs must never be embedded into development builds unintentionally.

---

# 7. Phase 2 — Authentication & Session Integration

This is one of the highest-risk areas.

## Required behavior

The mobile app must support:
- secure login;
- persistent authenticated session;
- logout;
- expired session recovery;
- password reset;
- role authorization;
- navigation after login;
- safe handling of external authentication redirects if introduced later.

## Rules

Do not:
- store passwords locally;
- put credentials in JavaScript source;
- place secrets in the repository;
- bypass HTTPS certificate validation;
- disable normal authentication checks.

## Session validation

Test:
- fresh login;
- application restart while logged in;
- expired session;
- logout;
- multiple accounts;
- account role changes;
- server-side revocation.

## Mobile cookie considerations

If Edulytics continues using server cookies, verify:
- SameSite configuration;
- Secure flag;
- WebView cookie persistence;
- HTTPS-only behavior;
- CSRF protections;
- cross-domain redirects.

If a token-based API layer becomes necessary, implement it as a deliberate backend feature rather than as a workaround.

---

# 8. Phase 3 — Mobile UI Compatibility

## Objective

The mobile application should feel intentional, not like a desktop site squeezed into a phone.

## Required checks

### Navigation

Ensure:
- sidebar does not consume excessive screen width;
- navigation can collapse cleanly;
- touch targets are large enough;
- no hover-only feature is required to complete a task.

### Hover-dependent interactions

The current web application may use hover interactions on desktop.

On mobile:
- hover must never be required;
- focus/selected states need touch equivalents;
- lesson cards must remain directly tappable;
- animations must not block navigation.

### Typography

Validate:
- title wrapping;
- RTL Arabic;
- Polish text expansion;
- English labels;
- button width;
- long lesson titles.

### Scrolling

Prevent:
- nested scroll traps;
- accidental horizontal overflow;
- fixed elements hiding content;
- WebView rubber-banding issues where applicable.

---

# 9. Phase 4 — Native Integrations

Native features are added only when they improve the product.

## Initial native features

### Required

- app lifecycle handling;
- status bar configuration;
- splash screen;
- icons;
- external link handling;
- file picker;
- camera/photo permission if file upload requires it;
- downloads/open-in behavior;
- deep-link support;
- secure storage where appropriate.

### Recommended after base release

- push notifications;
- native share sheet;
- biometric re-entry;
- offline cache for selected content;
- download manager;
- document preview;
- app update messaging.

## Push notifications

Recommended future use cases:
- assigned homework;
- assessment deadlines;
- teacher notifications;
- new results;
- school announcements.

Push notifications must respect role, tenant, consent, and privacy rules.

---

# 10. Phase 5 — Android Build Pipeline

## Deliverables

Create Android project under:

```text
/mobile/android
```

## Build outputs

### Development

```text
APK
```

Use APK for:
- direct device installation;
- QA;
- local testing.

### Google Play

```text
AAB — Android App Bundle
```

AAB is the release artifact intended for Google Play distribution.

## Android configuration

Configure:
- application ID;
- minimum Android version;
- target SDK required by Google Play at release time;
- versionCode;
- versionName;
- adaptive icon;
- splash screen;
- network security;
- HTTPS domain allowlist;
- orientation support;
- file permissions;
- camera permissions if needed;
- notification permissions if enabled.

## Signing

Create secure Android release signing process.

Never commit:
- keystore files;
- keystore passwords;
- signing passwords.

Store credentials securely in CI/CD or an approved secrets manager.

---

# 11. Phase 6 — iOS Build Pipeline

## Deliverables

Create iOS project under:

```text
/mobile/ios
```

## Requirements

iOS release work requires:
- macOS;
- current compatible Xcode;
- Apple Developer account;
- certificates;
- provisioning profiles;
- App Store Connect application record.

## Configure

- bundle identifier;
- display name;
- version;
- build number;
- icons;
- launch screen;
- supported orientations;
- iPhone/iPad capabilities;
- URL schemes/deep links;
- privacy usage descriptions;
- push entitlement if notifications are added.

## Testing

Use:
- iOS Simulator;
- physical iPhone;
- physical iPad if possible;
- TestFlight.

---

# 12. Phase 7 — Privacy, Security & Education Compliance

Edulytics processes educational data and may be used by minors.

This requires stricter handling than a generic consumer application.

## Data inventory

Document every mobile-accessible category of data, including:
- account identity;
- school;
- class;
- student profile;
- lesson activity;
- assessment responses;
- scores;
- progress;
- teacher activity;
- notifications;
- device identifiers if collected;
- crash telemetry;
- push tokens.

## Privacy policy

The privacy policy must explicitly explain mobile processing.

Include:
- controller/operator identity;
- categories of data;
- processing purpose;
- retention;
- third-party processors;
- account deletion/contact mechanism;
- child/student data handling;
- analytics;
- notifications;
- permissions.

## Store declarations

Complete accurately:
- Google Play Data Safety;
- Apple App Privacy questionnaire;
- age rating;
- child-directed/education declarations where applicable.

Do not make declarations until the implemented app behavior is verified.

---

# 13. Phase 8 — Testing Strategy

## 13.1 Functional QA

Test all critical workflows by role.

### Student
- login;
- learning;
- lesson;
- assessment;
- practice;
- progress;
- results;
- notifications.

### Teacher
- class management;
- assignments;
- assessments;
- marks;
- reports.

### Supervisor
- oversight;
- reports;
- curriculum operations.

### School Admin
- administration;
- users;
- classes;
- configuration.

## 13.2 Platform QA

Test:
- Android;
- iOS;
- phone;
- tablet;
- portrait;
- landscape where supported.

## 13.3 Network QA

Test:
- Wi-Fi;
- mobile data;
- slow network;
- intermittent network;
- offline;
- server error;
- expired authentication.

## 13.4 Regression QA

After every mobile-related shared-code change, retest:
- normal desktop browser;
- mobile browser;
- Android app;
- iOS app.

---

# 14. Phase 9 — Store Assets

Prepare a dedicated asset pack.

## Required assets

- production app icon;
- Android adaptive icon;
- iOS icon set;
- splash screen;
- phone screenshots;
- tablet screenshots where required;
- feature/marketing graphics where required;
- app description;
- short description;
- keywords;
- support URL;
- privacy policy URL;
- contact information.

## Screenshot plan

Capture real production/staging screens for:
- Student Dashboard;
- My Learning;
- lesson content;
- Private Practice;
- Assessments;
- Progress;
- Results.

Do not use mock screens that differ materially from the released app.

---

# 15. Phase 10 — Google Play Release

## Recommended release sequence

```text
Local APK
   ↓
Internal testing
   ↓
Closed testing
   ↓
Production candidate
   ↓
Google Play production
```

## Google Play checklist

Before submission:
- package ID final;
- AAB signed;
- version correct;
- privacy policy live;
- Data Safety completed;
- screenshots uploaded;
- age rating completed;
- target API compliant with current Play requirements;
- application tested under release signing;
- crash-free smoke test passed.

---

# 16. Phase 11 — Apple App Store Release

## Recommended sequence

```text
Local Xcode build
   ↓
Physical device testing
   ↓
Archive
   ↓
App Store Connect
   ↓
TestFlight
   ↓
Internal testing
   ↓
External testing if required
   ↓
App Review
   ↓
Production release
```

## Apple checklist

- Bundle ID final;
- App Store Connect record;
- signing valid;
- privacy questionnaire completed;
- screenshots uploaded;
- privacy policy live;
- support URL live;
- age rating completed;
- required permission descriptions present;
- account/login review instructions prepared;
- review account available if Apple requires authenticated access.

---

# 17. App Review Account Strategy

Because most Edulytics screens require authentication, store reviewers may need working credentials.

Create dedicated review accounts.

Recommended set:
- Student review account;
- Teacher review account if reviewing teacher functionality is necessary.

Review accounts must:
- contain safe demo data;
- contain no real student personal data;
- remain stable during review;
- have sufficient permissions to evaluate submitted functionality.

Do not use personal administrator credentials.

---

# 18. Versioning Strategy

Use semantic product versioning.

Example:

```text
Web release:    continuous
Mobile version: 1.0.0
Android code:   1
iOS build:      1
```

Future example:

```text
1.0.1 — patch
1.1.0 — feature update
2.0.0 — major mobile change
```

Every store release must map to a Git commit/tag.

Example:

```text
mobile-v1.0.0
```

---

# 19. CI/CD Strategy

Do not immediately automate store publication.

Start with controlled builds.

## Stage A

Manual:
- Android build;
- Android signing;
- iOS archive;
- store upload.

## Stage B

CI builds:
- validate mobile project;
- build Android;
- test;
- produce signed candidate artifact using protected secrets.

## Stage C

Optional controlled store automation:
- upload to internal Android track;
- upload to TestFlight.

Production store release should require explicit approval.

---

# 20. Rollback Strategy

Mobile releases cannot be treated exactly like web deployments because installed versions may remain on user devices.

## If a mobile shell release has a problem

Possible actions:
- stop rollout in store console;
- release patched version;
- disable problematic native feature via remote feature flag;
- keep the web backend compatible with the previous mobile version.

## Backend compatibility rule

Never deploy a backend change that immediately makes the currently published mobile version unusable.

Maintain backward compatibility for a defined support window.

---

# 21. Feature Flags

Use feature flags for mobile-specific behavior when appropriate.

Potential flags:

```text
MobileAppEnabled
MobilePushNotifications
MobileDownloads
MobileBiometricLogin
MobileOfflineLessons
MobileNativeFilePicker
```

This allows high-risk features to be disabled without affecting the rest of the application.

---

# 22. Observability

Monitor mobile usage separately from web usage.

Recommended signals:
- startup failures;
- login failures;
- lesson loading failures;
- WebView navigation errors;
- API errors;
- crash rate;
- app version distribution;
- operating system distribution.

Do not introduce analytics that collect student data unnecessarily.

---

# 23. Definition of Done — Android

Android is ready for production only when:

- [ ] mobile project builds reproducibly;
- [ ] production signing works;
- [ ] APK installs successfully;
- [ ] AAB builds successfully;
- [ ] login works;
- [ ] all student critical paths work;
- [ ] teacher/admin paths included in the app work;
- [ ] Arabic/English/Polish validated;
- [ ] file upload validated;
- [ ] external links validated;
- [ ] logout validated;
- [ ] session expiry validated;
- [ ] privacy policy published;
- [ ] Play listing complete;
- [ ] Data Safety complete;
- [ ] internal test passed;
- [ ] closed test passed where required;
- [ ] production release approved.

---

# 24. Definition of Done — iOS

iOS is ready for production only when:

- [ ] Xcode project builds;
- [ ] signing works;
- [ ] physical iPhone test passes;
- [ ] iPad test passes if supported;
- [ ] authentication works;
- [ ] lesson flows work;
- [ ] assessment flows work;
- [ ] file handling works;
- [ ] language switching works;
- [ ] RTL works;
- [ ] privacy permission strings are accurate;
- [ ] App Store privacy data completed;
- [ ] TestFlight build passes QA;
- [ ] App Review metadata complete;
- [ ] review credentials prepared;
- [ ] Apple review approved.

---

# 25. Explicit Out-of-Scope Items for Version 1

Unless specifically approved, version 1 should **not** include:
- a full native rewrite;
- offline synchronization of the entire curriculum;
- background downloading of all content;
- native payment/subscription architecture;
- independent mobile backend;
- separate mobile database;
- platform-specific duplicate business logic.

These can be considered after the initial store release.

---

# 26. Proposed Initial Mobile Release Scope

## Version 1.0

Include:
- secure authentication;
- current Edulytics UI;
- Student Portal;
- My Learning;
- lessons;
- assessments;
- Private Practice;
- progress;
- results;
- notifications page;
- multilingual support;
- responsive mobile navigation;
- secure file upload where needed;
- app icons;
- splash screen;
- Android release;
- iOS release.

Teacher/Supervisor/Admin access should be included only if their current mobile responsive experience passes the Phase 0 audit.

---

# 27. Recommended Repository Workflow

Every mobile change follows:

```text
issue / implementation task
        ↓
feature branch
        ↓
implementation
        ↓
web regression check
        ↓
mobile build check
        ↓
pull request
        ↓
review / CI
        ↓
merge
        ↓
staging mobile build
        ↓
QA
        ↓
store candidate
```

No direct production-only modification should be used as the development process.

---

# 28. Execution Order

## Milestone M1 — Foundation

- [ ] Create `/mobile`.
- [ ] Initialize Capacitor.
- [ ] Configure app identity.
- [ ] Add Android project.
- [ ] Add iOS project.
- [ ] Add staging configuration.

## Milestone M2 — Core Runtime

- [ ] Open Edulytics inside mobile runtime.
- [ ] Validate authentication.
- [ ] Validate cookies/session.
- [ ] Validate navigation.
- [ ] Validate logout.
- [ ] Validate deep links.

## Milestone M3 — Student QA

- [ ] Dashboard.
- [ ] My Learning.
- [ ] Lesson.
- [ ] Practice.
- [ ] Assessment.
- [ ] Results.
- [ ] Progress.
- [ ] Notifications.

## Milestone M4 — Device Integration

- [ ] Files.
- [ ] Camera if required.
- [ ] Downloads.
- [ ] External URLs.
- [ ] Native back behavior.
- [ ] Status bar.
- [ ] Splash screen.

## Milestone M5 — Android Candidate

- [ ] Release signing.
- [ ] APK.
- [ ] AAB.
- [ ] Internal QA.
- [ ] Google Play internal testing.

## Milestone M6 — iOS Candidate

- [ ] Signing.
- [ ] Archive.
- [ ] Physical device QA.
- [ ] TestFlight.

## Milestone M7 — Compliance

- [ ] Privacy policy update.
- [ ] Data Safety.
- [ ] App Privacy.
- [ ] Age rating.
- [ ] Review credentials.

## Milestone M8 — Store Launch

- [ ] Google Play submission.
- [ ] Google Play production.
- [ ] Apple submission.
- [ ] Apple approval.
- [ ] App Store release.

---

# 29. Go / No-Go Criteria

Proceed to store submission only if:

1. Production web remains unaffected.
2. No authentication regression exists.
3. No privileged data is exposed.
4. No secrets are inside the app bundle.
5. Student workflows pass mobile QA.
6. Crash/blocking-error rate is acceptable.
7. Privacy declarations match actual app behavior.
8. Release signing is reproducible.
9. Review accounts work.
10. Store assets accurately represent the released product.

---

# 30. Final Architectural Decision

The recommended mobile strategy for Edulytics is:

```text
KEEP:
- Current web frontend
- Current backend
- Current database
- Current authorization model
- Current production domain

ADD:
- Capacitor mobile shell
- Android native host
- iOS native host
- Mobile-specific integrations
- Store build/signing pipelines
- Store compliance assets
```

This approach converts Edulytics into a distributable mobile product **without replacing or destabilizing the existing web platform**.

The first implementation task after approval of this plan is:

> **Create the isolated `/mobile` workspace and complete the Mobile Readiness Audit before changing shared production behavior.**
