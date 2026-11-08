from pathlib import Path
import json
from collections import Counter
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
path=base/"u01-l01-element-coverage.json"
ledger=json.loads(path.read_text(encoding="utf-8"))
rows=[]
for x in ledger["Elements"]:
    cat=x["Category"]
    if cat not in ("FIG","TABLE","EXERCISE","MEDIA"):
        continue
    if cat=="FIG":
        assert x["SourceImageCopied"] is False and x["FirstPartyAlternativeAuthored"] is True
        x["EdulyticsReuseRightsStatus"]="FIRST_PARTY_GRAPHIC_OR_DESCRIPTION; ORIGINAL_IMAGE_NOT_EMBEDDED"
        x["OriginalSourceIllustrationRightsStatus"]="SEPARATELY_REVIEW_IF_PUBLISHER_IMAGE_IS_EVER_REUSED"
        x["SourceFigureSemanticParityStillRequired"]=True
    elif cat=="TABLE":
        assert x["FirstPartyTableRepresentationAuthored"] is True
        x["EdulyticsReuseRightsStatus"]="INDEPENDENT_EDUCATIONAL_TABLE_REPRESENTATION; SOURCE_BITMAP_NOT_EMBEDDED"
        x["OriginalSourceIllustrationRightsStatus"]="SEPARATELY_REVIEW_IF_ORIGINAL_IMAGE_IS_EVER_REUSED"
        x["SourceTableNumericsSemanticParityStillRequired"]=True
    elif cat=="EXERCISE":
        if x["RightsStatus"]!="SOURCE_PDF_GRAPH_RIGHTS_REQUIRE_SEPARATE_REVIEW":
            continue
        assert x["OriginalImageCopied"] is False
        x["EdulyticsReuseRightsStatus"]="INDEPENDENT_FIRST_PARTY_EXERCISE_GRAPH; ORIGINAL_IMAGE_NOT_EMBEDDED"
        x["OriginalSourceIllustrationRightsStatus"]="SEPARATELY_REVIEW_IF_ORIGINAL_IMAGE_EVER_REUSED"
        x["OriginalGraphImageSemanticParityStillRequired"]=True
    elif cat=="MEDIA":
        assert "NO_SOURCE_MEDIA_EMBEDDED" in x["RightsStatus"]
        x["EdulyticsReuseRightsStatus"]="INDEPENDENT_FIRST_PARTY_TEXT_ONLY; EXTERNAL_MEDIA_NOT_COPIED"
        x["OriginalSourceExternalMediaRightsStatus"]="NOT_GRANTED; NO_RIGHT_TO_REPUBLISH_EXTERNAL_ASSET"
        x["OriginalMediaSkillSemanticParityStillRequired"]=True
    rows.append({
        "OriginalElement":x["SourceElementId"],
        "OriginalSourceRightsStatus":x["RightsStatus"],
        "EdulyticsOwnAssetReuseStatus":x["EdulyticsReuseRightsStatus"],
        "OriginalAssetEmbedded":False,
        "OriginalAssetRightsClearanceClaim":False,
        "SkillAndScaleSourceMatchIndependentlyCertified":False
    })
assert len(rows)==38,len(rows)
audit={
"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":1,
"OriginalVisualAndMediaAnchorsExamined":38,
"OriginalFigureAnchorCount":15,
"OriginalTableAnchorCount":14,
"OriginalExerciseGraphSpecialRightsCount":3,
"OriginalExternalMediaCount":6,
"EmbeddedPublisherOrThirdPartyImageCount":0,
"OriginalSourceImageReproductionRightsClaimed":False,
"FirstPartyDerivedPedagogicContentAttribution":"SOURCE_PROVENANCE_IN_MANIFEST_CC_BY_4_REQUIREMENTS_RETAINED",
"FirstPartyAlternativeAssetsHaveIndependentReuseRightsDescription":True,
"OriginalSourceGraphicOrExternalMediaSemanticParityCertified":False,
"Gate":"SOURCE_ORIGINAL_ASSET_RIGHTS_NOT_NEEDED_WHEN_NOT_REUSING; SOURCE_PEDAGOGIC_MATCH_STILL_REQUIRED",
"Rows":rows
}
(base/"u01-l01-independent-visuals-rights-boundary.json").write_text(
    json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
ledger["FirstPartyOriginalVisualAndMediaReuseRightsSeparationCount"]=38
ledger["OriginalPublisherOrExternalMediaCopiedCount"]=0
ledger["OriginalVisualSemanticParityCertified"]=False
ledger["SourceAtomicDenominatorVerified"]=False
path.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cpath=base/"u01-l01-abc-checkpoint.json"
c=json.loads(cpath.read_text(encoding="utf-8"))
c["SourceVisualAssetRightsBoundaryAudit"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-independent-visuals-rights-boundary.json"
c["OriginalPublisherVisualsOrExternalMediaCopied"]=0
c["OriginalPublisherVisualsAndExternalMediaRightsAssumed"]=False
c["FirstPartyVisualAndMediaRightsBoundaryReviewedCount"]=38
c["OriginalVisualSemanticParity"]="NOT_INDEPENDENTLY_CERTIFIED"
c["ReadyForStageD"]=False
cpath.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("38_SOURCE_FIGURE_TABLE_EXERCISE_GRAPH_AND_MEDIA_RIGHTS_BOUNDARIES_REVIEWED")
