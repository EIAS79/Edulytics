# Stage 22 — Game runtime migration

## Status

**COMPLETE**

Stage 22 moves all currently migrated READY_VERIFIED exact Mathematics game routes
to a server-authoritative runtime.

The governing rule is now:

```text
server owns mathematics
browser owns interaction/rendering
```

For exact Mathematics V2 routes, the browser no longer generates the problem,
stores the authoritative answer, or decides whether the student's answer is correct.

## Scope

Stage 22 covers every exact lesson SkillContract already approved by Stage 18:

- 14 exact lesson mappings;
- 5 exact mechanics;
- TWO_UNKNOWNS;
- SCALE_READING;
- FRACTION_COMPARE_UNLIKE;
- FRACTION_EQUIVALENT;
- UNIT_RATE.

No new lesson mapping or official OutcomeCode is invented by this stage.

## Server-authoritative flow

```text
Authenticated student
→ approved exact lesson
→ SkillContract
→ allowed question family
→ ExactSkillContractQuestionEngine
→ solver
→ independent verifier
→ protected opaque round token
→ browser renderer
→ student interaction
→ answer + protected token returned to server
→ server verification
→ correctness response
```

The public round payload contains:

- prompt;
- hint;
- answer choices;
- SkillId;
- mechanic;
- question family;
- opaque protected round token.

It does **not** contain the authoritative answer.

## Security and scope

The protected round token is bound to:

- student identity;
- curriculum adoption;
- lesson;
- exact lesson code;
- SkillContract;
- mechanic;
- allowed question family;
- generated parameters;
- round index;
- issue time.

Tokens expire after 30 minutes.

Tampering, cross-student reuse, cross-lesson reuse, cross-curriculum reuse,
disallowed families, mechanic mismatch and expired state fail closed.

Both runtime endpoints require:

- StudentPortal authorization;
- anti-forgery validation;
- interactive request timeout controls;
- heavy-write concurrency rate limiting.

## Browser contract

The Stage 22 browser client:

- renders server-provided problem data;
- captures the student's interaction;
- submits the selected answer;
- displays the server's `isCorrect` result;
- updates presentation-only stars/progress.

It does not:

- call `Math.random()` to create mathematics;
- receive `CorrectAnswer`;
- compare a selected answer with an authoritative local value;
- fall back to browser grading when the server is unavailable.

## Legacy compatibility

Non-migrated game routes may keep their existing browser interaction until they obtain
an approved exact SkillContract. They are not Stage 22 server-verified mathematics and
their browser feedback must not be treated as authoritative mastery evidence.

When the Mathematics V2 production rollout is enabled, an exact route is not allowed
to silently fall back to the old browser-authoritative exact runtime.

## Source of truth

Manifest:

`src/Edulytics.Core/Mathematics/Curriculum/stage22-game-runtime-migration-manifest.v1.json`

Server runtime:

`src/Edulytics.Web/GameRouting/Stage22ExactGameRuntime.cs`

Authenticated endpoints:

`src/Edulytics.Web/Controllers/StudentPracticeController.cs`

Interaction-only client:

`src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-stage22.js`

CI closure:

`tools/math_intelligence/stage22_game_runtime_closure_audit.py`

Acceptance tests:

`tests/Edulytics.Tests/MathematicsIntelligence/Stage22GameRuntimeMigrationTests.cs`

## Exit criteria

Stage 22 is complete only when:

- all Stage 18 exact lessons enter the server-authoritative game runtime;
- exact problem generation uses the shared SkillContract kernel;
- solver output is verified before a round is issued;
- the authoritative answer is absent from the browser payload;
- correctness is decided only on the server;
- protected round scope cannot be forged;
- exact routes fail closed if server verification is unavailable;
- the Stage 22 closure audit is green;
- all previous Mathematics Intelligence closure gates remain green;
- full repository CI is green;
- the merged commit is live on Render;
- external liveness, readiness and homepage checks pass.

## Next programme stage

After this stage is live, execution proceeds to **Programme Stage 23 — IGCSE Extended gate**.
