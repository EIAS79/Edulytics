# CCSS 8.EE.A.1 — repair of official Outcome → Practice skill mismatch

## Source and defect
Grade 8 official mathematics standard 8.EE.A.1 requires knowing and applying **integer exponent properties** to form equivalent numerical expressions. The accepted source PDF example is 3² × 3⁻⁵ = 3⁻³ = 1/3³ = 1/27. PR #456 corrected the malformed official text and the SHA-guarded import state.

However, the checked-in official Practice map still routed `CCSS:8.EE.A.1` via `ccss-exponentials` to the combined `exponential-log` target, which permits `supporting.logarithms.evaluate`. Correct mathematical text must not be assessed by an unrelated logarithm family.

## Bounded fix
- Add a **higher-priority exact-code official route** for 8.EE.A.1 only, without weakening or editing any other CCSS standard's routing.
- Its only target family is `usccss.g8.integer_exponent_laws`, generating product, quotient and power-of-power calculations with negative, positive and zero integer exponents.
- The solver independently recomputes exact rational values from stored **original parameters**; equivalently reduced fractions are accepted. No supplied `CorrectAnswer` is trusted.
- Tests check official resolution and the materialized mapping agree; verify many samples across all three patterns, plus rejection of wrong values and absence of logarithms.
- This change does **not** independently certify the full 8.EE.A.1 lesson corpus or the other 120 child clauses, and it does not prove any publisher-specific pedagogical lesson is READY_VERIFIED. The academic NO-GO stays active.

## Validation and rollout
CI must pass .NET, mathematical intelligence, Common Core official closure and isolated American authenticated acceptance before merge. No changes to database structure, lesson IDs, student grades, existing published official-text hash chain or Render settings.
