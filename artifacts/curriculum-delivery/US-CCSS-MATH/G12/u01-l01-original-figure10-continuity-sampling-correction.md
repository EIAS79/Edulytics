# Section 1.1 Figure 10: source graph sampling is not linear interpolation

Local 2017 original PDF page 29 (1-based) was visually inspected.
Original Figure 10 illustrates a continuous NON-MONOTONIC curve,
with two black sample points indicating f(0)=2 and f(6)=1.

Previous first-party graphic used the straight line f(x)=2−x/6;
this had the right two sample outputs but wrongly replaced the
source's rise-and-fall curve with a monotonic straight line.
It could therefore mislead learners into inferring a unique
curve from two sampled points.

First-party correction: a piecewise-linear, continuous but
non-monotonic function through the source's exact (0,2)
and (6,1) values, plus a local maximum and minimum.
The first-party diagram's shape is distinct from the original
publisher art. No source image was copied.

This independently closes the specific sampling/non-monotonicity
pedagogical gap; complete Section 1.1 source-atomic
parity and Stage D runtime remain separately unverified.
