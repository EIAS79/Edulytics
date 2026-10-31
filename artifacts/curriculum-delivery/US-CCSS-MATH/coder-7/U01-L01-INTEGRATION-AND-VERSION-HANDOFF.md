# U01:L01 — A/B evidence, prepared C, shared integration and version request

**Course:** US-CCSS-MATH:HS:TRAD:ALG2:IM-1E
**Lesson:** PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01 — *A Towering Sequence*
**PR:** #588; **baseline HEAD at first review:** `8ed112e5a3dccc086c306f171f87157b9e2e485c`
**Original edition:** IM Algebra 2 First Edition 2019–2021, CC BY 4.0. This is a course-owned adaptation, not publisher-approved solutions.

## Stage A/B and local sources

Opened the actual existing learner content pack and these five original local files (sha256 recorded verbatim in `algebra2-120-lesson-progress.json`):

- `Common Core/11_HS_Algebra_2/1/1/preparation.html`
- `Common Core/11_HS_Algebra_2/1/1/index.html`
- `Common Core/11_HS_Algebra_2/1/1/practice.html`
- `Common Core/11_HS_Algebra_2_Student/1/1/index.html`
- `Common Core/11_HS_Algebra_2_Student/1/1/practice.html`

The local originals show lesson goals (sequence, recursive rule and contextual generation), warm-up 1.1, Hanoi 1.2, **optional** checker swap 1.3, 64-disc extension, and the four original Practice prompts. Preserved correctly authored Hanoi teaching and added only missing hands-on framing, optional checker activity, extension and worked answers/explanations for the four source Practice exercises. This preserves source lesson IDs, baseline hash fields, and existing teaching work. Original publisher Cool-down remains teacher-login-protected; its text was **not accessed**. A distinct, originally authored, teacher-only assessment candidate with rubric is stored in the contract as `ADAPTED_COOLDOWN_ORIGINAL_UNVERIFIED`, **not** shipped in the student content pack.

**One focused Node command**, `node --test tests/coder-7/UsCcssAlg2U01L01SourcePracticeContract.test.cjs`, gave **5 PASS / 0 FAIL** on the pre-commit working tree based on baseline HEAD. It independently explores legal Hanoi moves (n=1..4), legal checker swaps (n=1..4), checks BigInt n=64 and source recurrence/reversal/rational examples. This is **not** runtime generator/grader, .NET, auth/E2E or CI verification. Academic approval remains NOT_VERIFIED.

## Stage C blocker — Supervisor 3 Shared Integration Owner

Current static Practice trace has **no explicit lessonCode mapping** for U01:L01. The separate `U01-L01-ABC-SOURCE-PRACTICE-CONTRACT.json` specifies three precise proposed families (Hanoi minimum moves, recursive sequence, optional checker swap) with input domains, mathematical oracles, wrong/malformed/equivalent examples and narrative rubrics; all are `PROPOSED_NOT_REGISTERED`. Shared owner must select existing suitable engines before extending, then register a lesson-scoped family/verifier and implement/verify teacher scoring and authenticated attempt persistence for this **exact lesson**. Issue **#453** additionally blocks adopted-school `Traditional` / `ALG2`, course selection and two-school isolation. No claim of runtime C acceptance or Stage D is made.

**Gate outcome:** `A_CLOSED`, `B_CLOSED`, `C_PREPARED`, `BLOCKED_BY_INTEGRATION`, `ABC_READY_FOR_D=false`. Next curriculum cursor **remains ordinal 1**; do not advance to U01:L02 merely because the blocker is documented.

## Content lineage/version — designated academic/version owner

The locally changed first lesson's source hash field remains `11ed0bf897ac0d0a3346603a80e47f8e7dc991911d0a120e2c065d32b9a0360c`; historical `canonicalBodySha256` remains `9e7aa655c4441095780fd17c07c4d53612b59a5becc907ab55541e224aabeff7`. These are **historical metadata**, not a claim that the newly authored translation body matches that digest. Request the designated version owner to derive the canonical-body successor digest under the authorized hashing algorithm, approve the predecessor/successor pair and update materialization and academic-version authority before production. No historical digest is overwritten. Existing P0 lineage requests for U02:L18 and U05:L10 remain in place.

**Execution times:** exact per-lesson start timestamp was not captured at start, so ledger records it as unknown rather than inventing a number; end timestamp is recorded from device UTC clock. Test results are for an inspected pre-commit working tree; a future committed-SHA recheck must be labeled separately if run.
