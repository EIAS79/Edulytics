#!/usr/bin/env python3
"""Bounded, disposable authenticated B001 lesson Practice end-to-end probe.

This verifies *system self-consistency*, NOT independent answer correctness or
pedagogical parity. Never run with production accounts or databases.
"""
from collections import Counter
import re
import subprocess
import urllib.error
import urllib.parse
import urllib.request

UUID=r"[0-9a-fA-F-]{36}"
def extract_csrf(html):
    found=re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',html)
    return found.group(1) if found else None

def extract_adoption(html):
    found=re.search(r'name="curriculumAdoptionId"[^>]*value="('+UUID+r')"',html)
    return found.group(1) if found else None

def probe_lesson(opener,base,lesson_id,adoption_id,csrf,sql):
    """Generate and self-grade one real question scoped to a single lesson."""
    if not re.fullmatch(UUID,lesson_id):
        return {"status":"BAD_LESSON_GUID"}
    if not re.fullmatch(UUID,adoption_id or "") or not csrf:
        return {"status":"NO_ADOPTION_OR_CSRF"}
    form={
        "curriculumAdoptionId":adoption_id,"scope":"Lesson",
        "lessonId":lesson_id,"difficulty":"AtClassLevel","questionCount":"1",
        "__RequestVerificationToken":csrf
    }
    try:
        with opener.open(urllib.request.Request(
                base+"/student/practice/generate",
                data=urllib.parse.urlencode(form).encode(),method="POST"),
                timeout=40) as response:
            final=urllib.parse.urlparse(response.url).path.lower()
            match=re.fullmatch(r"/student/practice/attempt/("+UUID+r")",final)
            if response.status!=200 or not match:
                return {"status":"GENERATION_ROUTE_NOT_ATTEMPT","httpStatus":response.status}
            attempt_id=match.group(1)
            page=response.read().decode("utf-8")
        # Real DB evidence prevents false success from HTTP 200 on an error page.
        found=sql("""
            SELECT p."CurriculumPedagogicalLessonId"::text,
                   pi."Id"::text, i."GenerationFamily", i."CorrectAnswer"
            FROM "PracticeAttempts" p
            JOIN "PracticeAttemptItems" pi ON pi."PracticeAttemptId"=p."Id"
            JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
            WHERE p."Id"='"""+attempt_id+"""'
            ORDER BY pi."Order";""").splitlines()
        if len(found)!=1:
            return {"status":"INCORRECT_GENERATED_ITEM_COUNT","count":len(found)}
        row=found[0].split("|",3)
        if len(row)!=4 or row[0].lower()!=lesson_id.lower() or not row[2] or not row[3].strip():
            return {"status":"QUESTION_FAMILY_OR_ANSWER_MISSING"}
        _,item_id,family,answer=row
        token=extract_csrf(page)
        item_match=re.search(r'name="attemptItemIds\[0\]"[^>]*value="('+UUID+r')"',page)
        if not token or not item_match or item_match.group(1).lower()!=item_id.lower():
            return {"status":"PERSONAL_TEST_FORM_NOT_AVAILABLE","family":family}
        payload=urllib.parse.urlencode([
            ("__RequestVerificationToken",token),
            ("attemptItemIds[0]",item_id),("answers[0]",answer)
        ]).encode()
        with opener.open(urllib.request.Request(
                base+"/student/practice/attempt/"+attempt_id+
                "/personal-test-submit",data=payload,method="POST"),
                timeout=40) as response:
            if response.status!=200:
                return {"status":"SUBMIT_NON_200","family":family}
            response.read()
        result=sql("""
            SELECT CASE WHEN p."SubmittedAtUtc" IS NULL THEN 0 ELSE 1 END,
                   p."Score"::text,
                   count(r."Id")::text
            FROM "PracticeAttempts" p
            JOIN "PracticeAttemptItems" ai ON ai."PracticeAttemptId"=p."Id"
            LEFT JOIN "PracticeResponses" r ON r."PracticeAttemptItemId"=ai."Id"
                 AND r."IsCorrect"=true
            WHERE p."Id"='"""+attempt_id+"""'
            GROUP BY p."Id",p."SubmittedAtUtc",p."Score";""").split("|")
        if len(result)!=3 or result[0]!="1" or float(result[1])!=1.0 or result[2]!="1":
            return {"status":"INCORRECT_GRADE_OR_PERSISTENCE","family":family}
        return {"status":"SELF_CONSISTENT_GRADE_PERSISTED","family":family}
    except urllib.error.HTTPError as exc:
        return {"status":"HTTP_ERROR","httpStatus":exc.code}
    except (ValueError,TypeError,IndexError,AssertionError) as exc:
        return {"status":"PROBE_ASSERTION_OR_PARSE_ERROR","errorType":type(exc).__name__}
    except (urllib.error.URLError,TimeoutError,ConnectionError,OSError,
            subprocess.CalledProcessError) as exc:
        return {"status":"NETWORK_OR_DB_ERROR","errorType":type(exc).__name__}

def summarize(rows):
    if len(rows)!=500 or len({x["lessonCode"] for x in rows})!=500:
        raise ValueError("B001 graded-attempt probe requires all 500 unique canonical codes")
    statuses=dict(Counter(x["status"] for x in rows))
    grade_passed=statuses.get("SELF_CONSISTENT_GRADE_PERSISTED",0)
    return {
        "batchId":"US-CCSS-B001","checked":500,"byStatus":statuses,
        "generationAndSelfGradingPassed":grade_passed,"notSelfGraded":500-grade_passed,
        "independentlyMathVerified":0,"teacherAcademicApproved":0,
        "productionAuthenticatedUatApproved":0,"releaseDecision":"NO_GO",
        "assurance":"Synthetic school HTTPS and internal stored CorrectAnswer only. Does not independently validate source objectives, the generated answer oracle, wrong-answer rejection, media accessibility, or real-student production UAT.",
        "lessons":rows
    }

if __name__=="__main__":
    import unittest
    class ProbeTests(unittest.TestCase):
        def test_parse_csrf_and_adoption(self):
            self.assertEqual(extract_csrf('<input name="__RequestVerificationToken" value="token123">'),"token123")
            self.assertIsNone(extract_csrf('<div>no csrf</div>'))
            self.assertEqual(extract_adoption('<input name="curriculumAdoptionId" value="12345678-abcd-1234-abcd-1234567890ab">'),
                             "12345678-abcd-1234-abcd-1234567890ab")
        def test_a_500_shape_cannot_forge_signoff(self):
            data=[{"lessonCode":f"L{i}","status":"SELF_CONSISTENT_GRADE_PERSISTED"} for i in range(500)]
            x=summarize(data)
            self.assertEqual(x["generationAndSelfGradingPassed"],500)
            self.assertEqual(x["releaseDecision"],"NO_GO")
            self.assertEqual(x["independentlyMathVerified"],0)
            self.assertRaises(ValueError,summarize,data[:499])
    unittest.main()
