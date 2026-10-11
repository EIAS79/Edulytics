# US Common Core rational-hole diagram viewport correction — 2026-10-11

## Confirmed visual defect

The generated family `usccss.hs.rational_hole_or_pole` can have a removable hole at an ordinate with absolute value above 8, for example a hole at y=+10 when removed=-2, pole=-1, root=8. The previous renderer fixed the graph to +/-8 and silently omitted the open circle if the hole fell outside that range. The question asks learners to distinguish a hole from a pole: hiding the marker undermined the claimed visual scaffold even though the text answer and verifier were mathematically correct.

## Bounded repair

- Compute the removable-hole limit value from the question's original integer parameters, without altering the problem or mathematical answer.
- Keep the previous +/-8 vertical scale for ordinary holes and pole-only graphs; for out-of-range holes, extend the scale just enough to keep the marker inside the SVG viewport.
- Use the same computed value for plotting and the open-circle mark. Do not alter student answer grading, lesson content, source artifacts, official codes, curriculum mapping, persisted data, or schema.
- Regression cases cover a hidden positive (+10) and negative (-10) hole; assert an actual on-screen open-circle with finite coordinates and rational branches.

## Academic boundary and verification

This is visual accuracy and graph interpretation support, **not** freehand graph grading, arbitrary graph construction, proof assessment, or academic certification of the 121 subordinate Common Core clauses. Merge only after focused and full .NET tests and required PR checks succeed. Verify exact main SHA and Render live/ready after deployment.
