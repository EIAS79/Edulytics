# Grade 10 General U03L02 — Original teacher rubric for systems of linear inequalities

**Lesson:** «حل أنظمة المتباينات بالتمثيل البياني». **Original source:** owner `01متقدم_.pdf`, Advanced Arabic 2025–2026, PDF33–39 / printed16–22, graph calculator lab PDF40/printed23, followed by a distinct linear programming lesson on PDF41. Source SHA-256 `0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815`. This rubric and examples are original to Edulytics, not publisher replicas.

| Assessment criterion | 2 = fully correct | 1 = partly correct | 0 = incorrect |
|---|---|---|---|
| Rewriting boundaries | Correct line for each inequality, each slope/intercept, same coordinate axes and scales | One graph slope/axis inaccuracy | Lines not derivable from original inequalities |
| Solid vs dashed | Solid for ≤ / ≥, dashed for < / >, explained by equality inclusion | Correct type without explanation | Reversed type or claims all boundaries included |
| Correct half-plane | Test point used to select the correct side for EACH inequality | One correct side, uncertain second | Shades wrong sides |
| Feasible intersection | Only region satisfying both inequalities highlighted and explained | Partial or mislabeled overlap | Uses union or line intersection rather than shared region |
| Boundary and empty-region cases | Identifies strict boundary exclusions and possible no-overlap case | One omitted condition | Accepts a boundary point on strict inequality |
| Source context | Correct model for time/money/production limits, nonnegative domain and units | Correct inequality with incomplete units | Violates actual limits or claims an optimization answer |
| Written explanation and technology | Drawn graph reasoning and independent substitution; graphing-device output checked against both equations | Incomplete check | Trusts screenshot or single inequality uncritically |

**Original point example:** For `ص≥س−١` and `ص<−س+٥`, point `(٠،٠)` passes because `٠≥−١` and `٠<٥`. Point `(٢،٣)` fails the second inequality because `٣<٣` is false, although it lies on that line. The first boundary is solid and the second is dashed.

**Different systems:** `ص>س+٣` and `ص<س+١` cannot both hold because this implies `س+٣<ص<س+١`, impossible. In `−١<ص≤٣`, the upper horizontal line is included and the lower excluded.

**Applied context:** If `٢س+٣ص≤١٢`, `س≥٠`, `ص≥٠` and س وص count units, `(٣،٢)` costs 12 and is allowed, whereas `(٤،٢)` costs 14 and is not. Learner draws the feasible budget triangle and explains whole-number units if needed.

**Exact digital/teacher boundary:** the two bounded first-party families score an original-coordinate pair's **membership** in both inequalities and whether a sampled boundary point is **included**, returning integer 0 or 1. They do not score colored shading, plotted lines, non-integer slopes, real-world programming/optimization, calculator keypresses or written proof. Those outcomes remain `TEACHER_REVIEW_REQUIRED`. `productionRouting=false`, Stage D tests **WRITTEN_NOT_RUN**.
