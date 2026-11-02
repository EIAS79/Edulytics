'use strict';
/*
 * Compatibility entrypoint for the old Windows-only clause extraction tool.
 * Old historical publisher report was generated with a case-sensitive high
 * school suffix comparison (82 direct, 39 missing), which is now documented
 * as stale legacy evidence. This tool deliberately delegates to the new
 * repository-relative verifier (106 direct, 15 missing), and writes only
 * the new, separately provenance-labelled report. It never overwrites the
 * historical 121-clause text or invents publisher alignments.
 */
require('./us-ccss-publisher-clause-reconciliation.cjs');
