# Section 1.1 exercise 52: avoid a teacher grading false negative

Source: locally verified OpenStax Precalculus 2017, PDF p.36:
original graph approximately passes through (-1,1) and approaches y=3
near the edge of its displayed window. It has no stated algebraic rule.
An exact original numerical root for f(x)=3 is not demonstrable from
the finite image alone; accepting a graphical estimate is a teacher decision.

First-party task intentionally uses y=sqrt(x+2), with a fully specified
domain and drawn graph extending through the level y=3. On this **new**
student-assessed model, f(-1)=1 and f(x)=3 exactly when x=7.

Bug corrected: the teacher-only grading guidance previously mentioned only
the source's unresolved exact root, which risked rejecting an entirely valid
x=7 answer to the student-facing independent task. It now distinguishes
the original source graph from the Edulytics graph and explicitly accepts
x=7 for the latter.

Source image not copied. Source perfect pixel parity not claimed.
No Stage D, CI, E2E or automated tests run.
