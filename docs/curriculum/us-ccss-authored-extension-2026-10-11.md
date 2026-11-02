# US CCSS original standards extensions — 2026-10-11

## Provenance and scope

Two standards had verified numeric family implementations but lacked corresponding worked explanations in learner-visible lessons. This update adds **original Edulytics-authored enrichment**, not publisher excerpts, to existing lessons already linked to the **parent** Common Core outcomes. Official outcome graph nodes, all 1,560 lesson identities, lesson/outcome links, student grades and source fingerprints are unchanged. Neither missing child-level publisher evidence nor full academic certification is asserted.

| Standard child skill | Existing lesson | Added learner evidence | Exact Practice family |
| --- | --- | --- | --- |
| HSF-BF.A.1.c, composition of functions | Algebra 2 U05 L10, Combining Functions | Distinguishes function arithmetic from composition; evaluates `f(g(x))` and `g(f(x))`; weather-balloon altitude-to-temperature composition with consistent units | `usccss.hs.function_composition` |
| HSF-BF.B.4.d, creating an invertible function by restricting domain | Algebra 1 U04 L15, Inverse Functions | Demonstrates noninjective `q(x)=(x-3)^2`, then restricts to `x>=3`, obtains `q^-1(y)=3+sqrt(y)`; contrasts alternate branch | `usccss.hs.restricted_inverse` |

## Checked mathematics

- With `f(x)=2x+1`, `g(x)=3x-4`: `f(g(x))=6x-7`, `g(f(x))=6x-1`. At x=2 these are 5 and 11, respectively.
- A balloon height `h(t)=1000+2t` meters for t minutes and temperature `T(y)=20-0.006y` degrees Celsius for altitude y meters gives `T(h(t))=14-0.012t` degrees Celsius. `T(h(5))=13.94` degrees Celsius.
- `q(x)=(x-3)^2` on all real numbers has `q(1)=q(5)=4`, and hence is not injective. With domain restricted to `x>=3`, `q^-1(y)=3+sqrt(y)` for `y>=0`, so `q^-1(16)=7` and `q(7)=16`. With `x<=3`, the other inverse branch is `3-sqrt(y)`.

## Content and release gates

- Both appended extensions are explicitly described as **original Edulytics standards material** in JSON, not falsely attributed to IM/Kendall Hunt.
- The content repair manifest remains fail-closed with eight unique lesson identities and exact SHA-256 source proofs, adding a three-stage history (original body, prior corrected body, latest enhanced body) only for these two lessons.
- Numeric family verification and lesson Practice routing are subject to targeted and full .NET tests. A separate source-body parity check must reject any unlisted or provenance-altering edits.
- The 121 child-clause publisher-reference inventory stays **106 direct / 15 without direct child-code reference**. These original extensions improve actual teaching but do **not** fabricate publisher references or prove every graphical assessment.
- Remaining academic work: two-dimensional graph drawing evaluation, vector diagram construction and geometric direction interpretation, contextual constructed-response probability decisions, final clause-by-clause semantic signoff, then the user-requested authenticated learner/teacher acceptance test after curriculum completion.

**Release intent:** bounded incremental content improvement; **not** a 100% US CCSS academic certification.