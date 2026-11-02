# Adaptive Practice V2 — C0 to C5 Closure Contract

This document is the authoritative learner-behaviour contract for the final
Adaptive Practice V2 closure. It supersedes any earlier implementation detail
that allowed unlimited same-item retries or treated infrastructure presence as
complete remediation behaviour.

## C0 — Canonical learner state machine

For every READY_VERIFIED learner-facing Practice item:

1. Correct on first attempt
   - record independent evidence;
   - no remediation lock is introduced;
   - select the next adaptive item through the decision engine.

2. Wrong #1
   - record the incorrect evidence;
   - classify a deterministic misconception when the submitted answer supports
     one; otherwise remain explicitly unclassified;
   - produce targeted Hint Level 1 from the item, submitted answer and exact
     verified answer;
   - keep the same item and same sequence open;
   - permit exactly one retry of that exact item.

3. Retry correct
   - record assisted success;
   - keep remediation locked;
   - assisted success must not count as independent progression evidence;
   - generate a fresh independent confirmation item.

4. Wrong #2 on the same item
   - close the original item as incorrect;
   - never show that exact item for another retry;
   - persist the second incorrect observation;
   - produce stronger scaffold/worked guidance;
   - generate a fresh remediation item in the same verified skill contract;
   - target the same or lower bounded complexity, never higher.

5. Remediation item correct
   - keep remediation locked;
   - request a fresh independent confirmation item.

6. Remediation item wrong
   - apply the same one-retry limit;
   - on its second wrong answer close it and generate another fresh recovery
     item with bounded complexity reduction;
   - prerequisite and representation evidence may further restrict the next
     approved family/representation.

7. Fresh independent confirmation correct
   - clear the remediation lock;
   - only now can normal progression resume.

8. Fresh independent confirmation wrong
   - reopen/continue remediation;
   - no complexity increase is allowed.

## C1 — Misconception diagnosis

Diagnosis is conservative and deterministic. A misconception label is emitted
only when the answer pattern proves it. The classifier receives the exact
AssessmentItem, including generation family, generation parameters, verified
answer and submitted learner answer.

The rounding family has explicit reviewed diagnoses:
- choosing the upper multiple when the lower multiple is correct;
- choosing the lower multiple when the upper multiple is correct;
- failing to zero lower place-value digits;
- choosing an adjacent multiple inconsistent with the rounding place.

Unknown wrong answers remain unclassified; the engine must never invent a
misconception merely to fill a field.

## C2 — Progressive remediation

Same-item retry limit is exactly one. Wrong #2 always closes the item and moves
to a fresh remediation item. The question budget counts generated items, not
retry attempts.

## C3 — Hint and scaffold engine

Guidance is server-generated and answer-aware. It may use:
- question family;
- exact generation parameters;
- learner submitted answer;
- verified answer;
- deterministic misconception classification;
- retry number.

Hint Level 1 must not reveal the final answer. Wrong #2 receives a stronger
scaffold/worked example that is independent of the current item's answer.

## C4 — Adaptive progression

Assisted success is not independent success. Complexity may increase only after
fresh independent evidence and while prerequisite/misconception locks are clear.
Repeated failure can reduce complexity only within the READY_VERIFIED lesson
contract; V2 does not invent or route to an unverified prerequisite lesson.

## C5 — Certification and V1 retirement

Inside the Unified Practice rollout:
- READY_VERIFIED lesson starts use Adaptive V2;
- V1 is historical/out-of-rollout compatibility only;
- eligible V2 runtime failure must not silently create a V1 session.

Certification must cover:
- correct first try;
- wrong then correct retry;
- wrong then wrong retry;
- remediation item correct;
- remediation item wrong;
- fresh confirmation correct;
- fresh confirmation wrong;
- repeated failure complexity non-increase;
- deterministic misconception/hint behaviour;
- same-item retry ceiling;
- semantic freshness of every generated next item.
