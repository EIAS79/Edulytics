# Grade 12 U01:L01 — source exercise 88: unit fidelity correction

Original local PDF: Section 1.1, PDF page 37 (same recorded source SHA-256 `f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d`). The original text identifies `G` in **tons per week** and population `p` in **thousands of people**.

The existing element ledger and Stage C exercise contract had incorrectly paraphrased the mass unit as **tonnes/week**. This would change the physical quantity and is not source-equivalent. Both source records are corrected consistently:
- `f(40)=13` means 40,000 people produce 13 **tons per week**.
- `f(5)=2` means 5,000 people produce 2 **tons per week**.

Do not infer a metric-tonne conversion or change the original units. This is a **source math/units correction**, not an assertion that Edulytics already delivers or grades the original exercise. Source exercise 88 retains `DeliveryStatus=NOT_YET_IMPLEMENTED` and `SemanticParityStatus=UNMATCHED`. U01:L01 remains `A_C_EXPLICIT_GAP`. No tests, E2E, CI or Stage D intentionally run.
