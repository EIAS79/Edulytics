# Grade 1 ordinal 84 — U04 L19 Make Two-digit Numbers — source-bound A–C authoring

**Result:** `READY_FOR_STAGE_D` for first-party source-matched teaching and gradeable mathematics, **not** academic approval. No .NET or GitHub Actions CI run, and no Stage D executed. Whole-grade `NO_GO`.

## Stage A — Original 2021 publisher source

Reference: https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-19/lesson.html (public, 2021 original teacher lesson).

The source includes a warm-up on four representations: A five tens and three ones (53); B thirty plus five (35); C three plus fifty (53); D a base-ten diagram showing five tens and three ones (53). A, C and D are equivalent in numerical value, unlike B. Actual original image artwork is not copied or asserted as pixel-exact.

Activity 1: make 65 using the provided bag's five existing ten-towers and loose ones, **without dismantling or constructing towers**; a mathematically valid representation is five tens plus fifteen ones. Activity 2: show the number 37 in multiple ways: besides 3 tens+7 ones and 2 tens+17 ones, the original teacher explicitly permits 36+1, 35+2 and other additive representations. Activity 3 offers real choice-time centers Greatest of Them All, Get Your Numbers in Order and Grab and Count; children's physical work/drawings remain teacher-assessed.

The five-minute nominal section D Checkpoint shown on the original page is labelled **0 minutes** and is behind teacher authentication; no protected assessment text is claimed or recreated.

## Stage B — Learner-facing source alignment

Reused the existing first-party canonical lesson teaching and preserved the important **65 no-break/no-new-tower** constraint and distinct **37 different ways** activity. Added the exact public warm-up values and the additional 37 addition-pair interpretation, without importing third-party images. Version moved from `us-ccss-g1-u04-l19-centers-and-regrouping-v2` to `...-v3` with an explicit old-version migration. Source and migration assertions authored but **not executed**.

Commit: `2ce021e2fbc964381345dce58c133855eba6cee1`. Teaching assertions commit: `a8e630a5937f7f461bfe0c1df22039b2697f0724`.

## Stage C — Practice grading

Previous modes 0–3 unchanged; 65 still requires `TENS=5;ONES=15`, and 37 tens-and-ones mode retains flexible student-chosen trades. Added first-party, source-grounded mode 4: accept **any two distinct unordered nonnegative integer addition pairs** that each sum to 37, in `PAIR1=x+y;PAIR2=z+w` notation. Reject a reordering of the same pair as the second example, malformed fields, wrong sums and out-of-grade operands. Written positive, wrong and generator coverage cases authored, **not run**.

Commit: `9421e961e9a3452d98f8361c1e1ce514675acf3d`. Test assertions: `efeb25e43ffa5b9e89a4f0d33056c31ac68edd6b`.

**A–C boundary:** Physical bag manipulation, drawn models, partner play, the original publisher artwork and the restricted checkpoint are not auto-certified by digital answers. The accessible mathematical objectives and source-specific numerical variants are authored. Exact-SHA tests, rendering, persistence, seeded compatibility and rights confirmation remain in deferred Stage D. No deployment, merge, academic approval or grade release was performed.
