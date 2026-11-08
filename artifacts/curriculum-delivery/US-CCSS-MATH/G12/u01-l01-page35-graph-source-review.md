# Grade 12 U01:L01 — source graph vertical-line review (PDF page 35)

Source: local read-only OpenStax Precalculus PDF, Section 1.1 page 35, existing recorded SHA-256 `f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d`. Figures were visually inspected in a locally rendered, non-distributed page. Only first-party semantic descriptions are retained here.

| Original exercise | Source visual reasoning | Vertical-line result |
|---|---|---|
| 40 | An x-monotone, cubic-like continuous curve with local maximum and minimum. Every vertical line intersects the curve at most once. The curve has exactly one plotted y at any x in its domain. | **FUNCTION** |
| 41 | Right-opening sideways parabola with two arms from the origin. A vertical line to the right of the origin meets upper and lower branches. For x>0, both upper and lower branches give distinct y-values. | **NOT A FUNCTION** |
| 42 | Reciprocal-like hyperbola with branches in quadrants I and III and the y-axis as a vertical asymptote. There is no plotted point at x=0. Every allowed nonzero x has one output; the asymptote is not another branch. | **FUNCTION** |
| 43 | Single connected piecewise rule: negative horizontal left ray, increasing middle line, positive horizontal right ray. Segments meet without contradictory outputs. Each x has one plotted y, including transition points. | **FUNCTION** |
| 44 | Small closed ellipse below and to the right of the origin. A vertical line through its interior intersects the upper and lower ellipse arcs. | **NOT A FUNCTION** |
| 45 | Single increasing S-like curve that becomes nearly horizontal around y=0 but never doubles back horizontally. Every input has exactly one output; a horizontal tangent does not violate the vertical-line test. | **FUNCTION** |
| 46 | Two horizontal rays: the upper ray at approximately y=3 begins just right of x=0 (open point) and extends right; lower ray at approximately y=−3 extends left up to an open point at x≈2. Their x-domains overlap for 0<x<2. For x=1, the plotted relation has BOTH y≈3 and y≈−3. It FAILS the vertical-line test despite the rays being spatially separated. | **NOT A FUNCTION** |
| 47 | Straight nonvertical line with negative slope. Each x gives one y on the line. | **FUNCTION** |
| 48 | Two crossing nonvertical diagonal lines forming an X. At almost any x other than the crossing x, the two lines give distinct y-values. | **NOT A FUNCTION** |
| 49 | Straight nonvertical line with positive slope. Each x gives one y on the line. | **FUNCTION** |
| 50 | Closed circle left of the y-axis touching the x-axis below. A vertical line through the circle interior intersects it at two y-values. | **NOT A FUNCTION** |
| 51 | Single sinusoidal/wave-like curve extending left and right. Each x lies on only one point of the wave, regardless of repeated heights at different x-values. | **FUNCTION** |

## Corrected defect

Original source route `G12-U01-L01-EXERCISE046` had said **passes**. This was mathematically incorrect: for an input such as `x=1` the image contains upper and lower horizontal rays giving different output values (approximately `3` and `−3`). Thus the figure **fails** the vertical-line test. The source routing and teacher-only answer key were corrected.

## Acceptance boundary

This establishes source **graph classification only** for exercises 40–51, not student-visible graph reproduction, digital grading or 1:1 delivery. All 12 contracts retain `SemanticParityStatus=UNMATCHED` and `DeliveryStatus=NOT_YET_IMPLEMENTED`. Figure rights/accessibility and complete section atom count remain outstanding; exercise 52(b) remains open. No tests, Stage D or CI intentionally run.
