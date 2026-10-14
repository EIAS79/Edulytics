# B01 release-specific independent rights review — REQUIRED / NO-GO

**Scope:** `US-CCSS-MATH`, `CCSSM-2010`, B01 exactly 500 lessons:
G1 146, G2 146, G3 143, G4 65. This is a reviewer request, **not** a rights approval.

**Current source evidence:** `B01-book-source-reference-index-500.json` identifies each of the 500 authorized Our-CS first-edition IM K–5 teacher preparation reference files by original relative path, byte length and SHA256. See also `01-edition-specific-licence-evidence.json` for source-licence policy and its current `INDEPENDENT_RIGHTS_SIGNOFF_PENDING` status.

**Known published terms to independently verify:** first-edition IM K–12 content ©2019–2021 is reported by the publisher as CC BY 4.0; see https://illustrativemathematics.org/terms-of-use/ and https://illustrativemathematics.org/site-ip-content/ . **Do not substitute** current 2024 v.360 CC BY-NC 4.0 sources; a website currently serving different material does not relicense the archived 2021 edition.

## Mandatory reviewer decisions

1. Independently identify the actual source edition of all G1–G4 B01 lesson/blueprint records, including nested sources and any drift from archived first-edition books. Cite corroborating publisher provenance.
2. Confirm the intended *commercial* copying, modification and redistribution rights for any source-protected content actually shipped by Edulytics. Verify that Edulytics-authored adaptations meet relevant first-edition licence terms and documentation.
3. Separately inventory any non-covered third-party artwork, diagrams, logos, screenshots, publisher-design/branding, assessments, or linked assets. Supply a specific licence or proof of independent Edulytics authorship for each shipped asset, or exclude it.
4. Confirm complete in-product attribution with author/publisher identification, CC BY 4.0 licence URL, source version/edition and adaptation disclosure where required. Disallow IM trademark/certification claims.
5. Provide an independent reviewer identity, dated evidence URL/ID and signed approval **specific to B01**; retain `NO-GO` for unknown/incompatible rights. No self-generated or synthetic approval.

## Required output

Record a verifiable signed decision bound to the final B01 source inventory and code/content candidate. Only an actual authorized reviewer may populate `independentReviewerId` and `rightsApprovalEvidenceId` in the authoritative rights evidence record (or an equally authoritative B01-specific signed decision consumed by release gates). A GitHub Actions test passing does not substitute for independent approval.

**Never certify, merge or publish B01 merely because 500 content records were indexed or structural tests passed.**
