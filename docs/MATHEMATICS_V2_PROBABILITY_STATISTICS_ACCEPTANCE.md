# Mathematics V2 — Probability / Statistics Acceptance

## Status

Shadow-only vertical slice. All registered question families are `ShadowVerified` with `productionRouting:false`.

No curriculum mappings, lesson mappings, standards, or OutcomeCodes are added by this slice.

## Supported exact operations

- Simple favourable-over-total probability with integer counts satisfying `0 <= favourable <= total` and `total > 0`.
- Complement probability `1 - p` for exact integer/rational `p` in `[0,1]`.
- Arithmetic mean for bounded nonempty vectors of exact integer/rational values.
- Frequency-table mean for bounded `[value, frequency]` rows, with exact values, nonnegative integer frequencies, and positive total frequency.

## Exactness and resource policy

- No floating-point approximation is used as an exact answer.
- Scalars are limited to the established 4096-bit exact rational budget.
- Intermediate arithmetic is preflighted against a bounded intermediate budget before exact operations are retained.
- Malformed rationals, invalid probability domains, invalid frequencies, unsupported shapes, and oversized values fail closed as `Unsupported` or `ResourceLimit`.

## Independent verification

Each verifier reconstructs the expected result from the original request. It does not trust solver trace data or the solver's reported answer. A solved result is accepted only when both `ExactResult` and the one-value `FiniteSolutionSet` equal the independently recomputed exact value.

## Deterministic generation

Four solver-grounded deterministic families are registered for difficulty bands 1–3:

- `probability.simple.favourable_over_total.exact`
- `probability.complement.exact`
- `statistics.mean.arithmetic.exact`
- `statistics.mean.frequency_table.exact`

A generated item must solve successfully and pass independent verification before the factory returns it.

## Explicit exclusions

This slice does not claim support for conditional probability, tree diagrams, permutations/combinations, probability distributions, variance/standard deviation, quartiles, regression, hypothesis testing, confidence intervals, or inferential statistics. Those remain unsupported until separately implemented and independently verified.
