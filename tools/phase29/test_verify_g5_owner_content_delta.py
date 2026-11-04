import copy
import unittest
from verify_g5_owner_content_delta import validate_pair


class Grade5OwnerContentDeltaNegativeControls(unittest.TestCase):
    def setUp(self):
        self.original = {"packCode": "US-CCSS-MATH",
                         "lessons": [
                             {"lessonCode": f"PED:US-CCSS-MATH:G5:U01:L{i:03}",
                              "sourceSha256": "source-fixed",
                              "canonicalBodySha256": "canonical-fixed",
                              "translations": [{"cultureCode": "en", "title": "old"}]}
                             for i in range(148)]}
        self.candidate = copy.deepcopy(self.original)
        for row in self.candidate["lessons"][:107]:
            row["translations"][0]["title"] = "corrected"

    def test_pinned_107_authorized_only(self):
        self.assertEqual(len(validate_pair(self.original, self.candidate)), 107)

    def test_unapproved_publisher_source_change_is_rejected(self):
        self.candidate["lessons"][0]["sourceSha256"] = "forged"
        with self.assertRaises(ValueError):
            validate_pair(self.original, self.candidate)

    def test_exact_grade5_content_version_transition_is_allowed(self):
        self.original["contentVersion"] = "phase30-common-core-rich-content-v1"
        self.candidate["contentVersion"] = "phase30-common-core-rich-content-v1-g5-u01-l01-source-20261030"
        self.assertEqual(len(validate_pair(self.original, self.candidate)), 107)

    def test_unknown_content_version_or_pack_metadata_is_rejected(self):
        self.original["contentVersion"] = "phase30-common-core-rich-content-v1"
        self.candidate["contentVersion"] = "forged-version"
        with self.assertRaises(ValueError):
            validate_pair(self.original, self.candidate)
        self.candidate["contentVersion"] = "phase30-common-core-rich-content-v1-g5-u01-l01-source-20261030"
        self.candidate["sourceAuthority"] = "forged"
        with self.assertRaises(ValueError):
            validate_pair(self.original, self.candidate)

    def test_extra_108th_change_is_rejected(self):
        self.candidate["lessons"][107]["translations"][0]["title"] = "extra"
        with self.assertRaises(ValueError):
            validate_pair(self.original, self.candidate)

    def test_original_order_must_match(self):
        self.candidate["lessons"][5], self.candidate["lessons"][6] = (
            self.candidate["lessons"][6], self.candidate["lessons"][5])
        with self.assertRaises(ValueError):
            validate_pair(self.original, self.candidate)

    def test_non_teaching_field_is_rejected(self):
        self.candidate["lessons"][0]["translations"][0]["unapproved"] = "value"
        with self.assertRaises(ValueError):
            validate_pair(self.original, self.candidate)


if __name__ == "__main__":
    unittest.main()
