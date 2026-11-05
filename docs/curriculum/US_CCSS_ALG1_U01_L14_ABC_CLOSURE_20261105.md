# US-CCSS Algebra 1 U01:L14 — A–B–C closure

**Lesson:** Outliers
**Status:** `READY_FOR_STAGE_D`

Local source hashes: teacher preparation `7a9df33088a7c60bed23b0e73f1c78b16188ec0d44bd4695a3befae83cf98617`; teacher lesson `6e4edb32d0129b07f9971bedf73a86cb9230c08a474536431f2f7d8492db0cb9`; teacher practice `187a53b0a571ca894ad05c79326574c36e94a4a991eacfdbdc5d88903f2b1b79`; student lesson `2fd5b0e38345b41463be74bef212807c3c7f5b32d3dd912ee3f507cdb9f0cec0`; student practice `cd931e6f1c605bddadc3342bcd515781ad7ecca0033b84bbde1168810c17e50d`. AccessIM/v360 excluded.

A/B: Rich V2 is mathematically correct for the six-value example: median 11.5, Q1=10, Q3=13, IQR=3, lower fence 5.5, upper fence 17.5, mean 155/6≈25.83, and the keep/exclude decision is explicitly conditional on data quality.

C correction: the legacy authoring vector omitted the lower fence even though its prompt named both fences. The vector is corrected to `[11.5,10,13,3,5.5,17.5,25.833333333333332,11,11]`.

C implementation: **Reuse** mean/median/IQR and **Extend** with `statistics.outliers.iqr_fence` and `statistics.outliers.classify_iqr_flag`. Formal flagging is machine-verifiable; investigating whether an extreme is an error and deciding to retain/exclude it remain `TEACHER_REVIEW_REQUIRED`.

Tests are **AUTHORED_NOT_RUN**. No build, CI, Stage D, authenticated E2E, merge or deployment was run or claimed.
