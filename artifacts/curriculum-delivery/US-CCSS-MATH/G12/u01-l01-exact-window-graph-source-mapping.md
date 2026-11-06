# Grade 12 U01:L01 — Exercises 76–87 exact window skill mappings

Reference: local OpenStax Precalculus (2017), Section 1.1, PDF page 37, *Technology*. Reading source was local only. The original explicitly requires graphing over the specified horizontal x viewing window and deriving corresponding ranges.

| Original exercise | Function | x window | Exact range |
|:--|:--|:--|:--|
| 76 | x² | [-0.1,0.1] | [0,0.01] |
| 77 | x² | [-10,10] | [0,100] |
| 78 | x² | [-100,100] | [0,10000] |
| 79 | x³ | [-0.1,0.1] | [-0.001,0.001] |
| 80 | x³ | [-10,10] | [-1000,1000] |
| 81 | x³ | [-100,100] | [-1000000,1000000] |
| 82 | √x | [0,0.01] | [0,0.1] |
| 83 | √x | [0,100] | [0,10] |
| 84 | √x | [0,10000] | [0,100] |
| 85 | ∛x | [-0.001,0.001] | [-0.1,0.1] |
| 86 | ∛x | [-1000,1000] | [-10,10] |
| 87 | ∛x | [-1000000,1000000] | [-100,100] |

Implementation: Grade12PrecalculusSection11WindowVisuals draws 65 normalized vertices, labels endpoints in physical units on both axes, reuses the existing typed SVG coordinate renderer, and retains its global 10,000 axis-span safety limit. Y-axis tick labels were added narrowly to CoordinatePlaneVisualSpec. Student Lesson.cshtml connects each existing teacher-review task 76–87 to its corresponding graph. No unrelated engine or grade was changed.

**Evidence:** Isolated targeted compilation of the modified visual renderer, lesson visuals and associated classes passed with zero C# warnings and errors. This is not a full Edulytics build, not an xUnit test, not E2E, and not Stage D. Independent source-figure visual parity, teacher submission runtime and all 92 source exercise equivalences still await acceptance; U01:L01 remains A_C_EXPLICIT_GAP.
