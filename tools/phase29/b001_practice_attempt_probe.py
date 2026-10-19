#!/usr/bin/env python3
"""Bounded, disposable authenticated B001 lesson Practice end-to-end probe.

This verifies *system self-consistency*, NOT independent answer correctness or
pedagogical parity. Never run with production accounts or databases.
"""
from collections import Counter
import json
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

def extract_idempotency_key(html):
    found=re.search(r'name="_idempotencyKey"[^>]*value="([^"]+)"',html)
    return found.group(1) if found else None

SURVEY_FAMILY = "usccss.t001.g1.collect_survey_graph"
SURVEY_SENTINEL = "survey-entry-protocol-v1"

def synthetic_survey_protocol(expected, parameters_json):
    """Exercise graph grading with synthetic CI-only votes; never claim real human respondents."""
    if expected != SURVEY_SENTINEL or not parameters_json:
        raise ValueError("Survey requires the server-owned, intentionally ungradeable sentinel")
    payload = json.loads(parameters_json)
    if not isinstance(payload,dict):
        raise ValueError("Survey must carry parameter object")
    p = payload.get("parameters")
    if not isinstance(p,dict):
        raise ValueError("Survey must have embedded server-owned parameters")
    minimum,maximum,topic = (p.get("minimumVotes"),p.get("maximumVotes"),p.get("topic"))
    if type(minimum) is not int or not (5<=minimum<=8) or maximum != 10 or type(topic) is not int or not (0<=topic<=5):
        raise ValueError("Invalid survey source parameters")
    # This vote sequence is intentionally fabricated CI fixture input. It tests
    # only graph tally consistency and deliberately cannot prove pupil interviews.
    votes = "".join(str(i%3) for i in range(minimum))
    tally = ",".join(str(votes.count(str(i))) for i in range(3))
    return "v:"+votes+"|g:"+tally


def syntactically_valid_wrong_answer(correct):
    """Create a real incorrect Grade 1 submission, preserving its answer
    field/schema; unlike a malformed sentinel, this exercises math grading.
    A full independent source oracle is tested separately in C#.
    """
    if not isinstance(correct,str) or "=" not in correct:
        raise ValueError("Expected answer with named fields")
    parts=correct.split(";")
    key,sep,value=parts[0].partition("=")
    if not sep or not key or not value:
        raise ValueError("No first answer field")
    number=re.search(r"\d+",value)
    if number:
        original=number.group()
        different=str(int(original)+1)
        replacement=value[:number.start()]+different+value[number.end():]
    else:
        # Alternatives are intentionally valid first-grade categorical
        # answers. Do NOT send the generic malformed-answer sentinel here.
        categories={
            "YES":"NO","NO":"YES",
            ">":"<","<":">","=":">",
            "RED":"BLUE","BLUE":"RED",
            "A":"B","B":"A",
            "EQUAL":"GREATER","GREATER":"LESS","LESS":"GREATER",
            "TRIANGLE":"SQUARE","SQUARE":"TRIANGLE",
            "CUBE":"SPHERE","SPHERE":"CUBE",
            "CYLINDER":"CONE","CONE":"CYLINDER",
            "RECTANGLE":"SQUARE",
            "HALF":"FOURTH","FOURTH":"HALF",
            "ADD":"SUBTRACT","SUBTRACT":"ADD",
        }
        replacement=categories.get(value.upper())
        if replacement is None and key.upper()=="NAME":
            replacement="one" if value.lower()!="one" else "two"
        if replacement is None:
            raise ValueError("Unrecognized Grade 1 categorical answer "+key)
    wrong=";".join([key+"="+replacement,*parts[1:]])
    if wrong.upper()==correct.upper() or not wrong or "B001_MALFORMED" in wrong:
        raise ValueError("Valid-wrong fixture not distinct from expected")
    return wrong

def probe_lesson(opener,base,lesson_id,adoption_id,csrf,idempotency_key,sql,answer_mode="correct"):
    """Generate, submit and check one real scoped Practice question."""
    if answer_mode not in ("correct","malformed","well_formed_wrong"):
        return {"status":"INVALID_PROBE_ANSWER_MODE"}
    if not re.fullmatch(UUID,lesson_id):
        return {"status":"BAD_LESSON_GUID"}
    if not re.fullmatch(UUID,adoption_id or "") or not csrf:
        return {"status":"NO_ADOPTION_OR_CSRF"}
    if not re.fullmatch(r"[0-9a-fA-F]{32}",idempotency_key or ""):
        return {"status":"MISSING_PER_FORM_IDEMPOTENCY_NONCE"}
    form={
        "curriculumAdoptionId":adoption_id,"scope":"Lesson",
        "lessonId":lesson_id,"difficulty":"AtClassLevel","questionCount":"1",
        "__RequestVerificationToken":csrf,"_idempotencyKey":idempotency_key
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
                   pi."Id"::text, i."GenerationFamily", i."CorrectAnswer",
                   COALESCE(i."GenerationParametersJson"::text,'')
            FROM "PracticeAttempts" p
            JOIN "PracticeAttemptItems" pi ON pi."PracticeAttemptId"=p."Id"
            JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
            WHERE p."Id"='"""+attempt_id+"""'
            ORDER BY pi."Order";""").splitlines()
        if len(found)!=1:
            return {"status":"INCORRECT_GENERATED_ITEM_COUNT","count":len(found)}
        row=found[0].split("|",4)
        if len(row)!=5 or row[0].lower()!=lesson_id.lower() or not row[2] or not row[3].strip():
            return {"status":"QUESTION_FAMILY_OR_ANSWER_MISSING"}
        _,item_id,family,answer,parameter_json=row
        synthetic_survey = family == SURVEY_FAMILY
        if synthetic_survey:
            try:
                answer = synthetic_survey_protocol(answer,parameter_json)
            except (ValueError,TypeError,KeyError,json.JSONDecodeError) as exc:
                return {"status":"SURVEY_SYNTHETIC_FIXTURE_INVALID","family":family,
                        "errorType":type(exc).__name__}
        if answer_mode=="well_formed_wrong":
            # A syntactically correct but mathematically wrong answer tests
            # the real ASP.NET grading and persisted zero-score response.
            # A survey requiring human participants cannot be auto-tested.
            if synthetic_survey:
                return {"status":"SURVEY_REQUIRES_HUMAN_NOT_VALID_WRONG_FIXTURE","family":family}
            answer=syntactically_valid_wrong_answer(answer)
        if answer_mode=="malformed":
            # Deliberately invalid for numbers, expressions, category lists and
            # survey tally protocols alike. The source generation family
            # remains server-owned; this is a negative control, not a math oracle.
            if answer=="B001_MALFORMED_RESPONSE_INVALID":
                return {"status":"MALFORMED_FIXTURE_COLLIDES_WITH_CORRECT_ANSWER"}
            answer="B001_MALFORMED_RESPONSE_INVALID"
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
                   COUNT(r."Id") FILTER (WHERE r."IsCorrect"=true)::text,
                   COUNT(r."Id")::text,
                   COALESCE(MAX(r."Answer"),'')
            FROM "PracticeAttempts" p
            JOIN "PracticeAttemptItems" ai ON ai."PracticeAttemptId"=p."Id"
            LEFT JOIN "PracticeResponses" r ON r."PracticeAttemptItemId"=ai."Id"
            WHERE p."Id"='"""+attempt_id+"""'
            GROUP BY p."Id",p."SubmittedAtUtc",p."Score";""").split("|",4)
        if len(result)!=5 or result[0]!="1" or result[3]!="1" or result[4]!=answer:
            return {"status":"MISSING_SUBMITTED_RESPONSE_OR_ANSWER_MISMATCH",
                    "family":family,"answerMode":answer_mode}
        if answer_mode=="malformed":
            if float(result[1])!=0.0 or result[2]!="0":
                return {"status":"MALFORMED_ANSWER_INCORRECTLY_ACCEPTED","family":family}
            return {"status":"MALFORMED_ANSWER_REJECTED_AND_PERSISTED","family":family}
        if answer_mode=="well_formed_wrong":
            if float(result[1])!=0.0 or result[2]!="0":
                return {"status":"WELL_FORMED_WRONG_ANSWER_INCORRECTLY_ACCEPTED","family":family}
            return {"status":"WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED","family":family}
        if float(result[1])!=1.0 or result[2]!="1":
            return {"status":"INCORRECT_GRADE_OR_PERSISTENCE","family":family}
        if synthetic_survey:
            return {"status":"SYNTHETIC_SURVEY_TALLY_PERSISTED_NEEDS_REAL_TEACHER_VERIFICATION",
                    "family":family,"humanParticipantsVerified":False}
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
    grade_passed=(statuses.get("SELF_CONSISTENT_GRADE_PERSISTED",0)
        +statuses.get("SYNTHETIC_SURVEY_TALLY_PERSISTED_NEEDS_REAL_TEACHER_VERIFICATION",0))
    return {
        "batchId":"US-CCSS-B001","checked":500,"byStatus":statuses,
        "generationAndSelfGradingPassed":grade_passed,"notSelfGraded":500-grade_passed,
        "malformedAnswersRejectedAndPersisted":sum(r.get("negativeStatus")=="MALFORMED_ANSWER_REJECTED_AND_PERSISTED" for r in rows),
        "malformedAnswersNotVerified":sum(r.get("negativeStatus")!="MALFORMED_ANSWER_REJECTED_AND_PERSISTED" for r in rows),
        "independentlyMathVerified":0,"teacherAcademicApproved":0,
        "productionAuthenticatedUatApproved":0,"releaseDecision":"NO_GO",
        "syntheticSurveyCount":statuses.get("SYNTHETIC_SURVEY_TALLY_PERSISTED_NEEDS_REAL_TEACHER_VERIFICATION",0),
        "realSurveyParticipantsVerified":0,
        "assurance":"Synthetic school HTTPS: correct-answer self-grading plus a deliberately malformed submitted-answer rejection for every lesson, not independent source mathematics or real-student UAT. No human survey respondents were consulted.",
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
            self.assertEqual(extract_idempotency_key('<input name="_idempotencyKey" value="0123456789abcdef0123456789abcdef">'),"0123456789abcdef0123456789abcdef")
        def test_survey_is_synthetic_and_cannot_use_the_solver_sentinel(self):
            p=json.dumps({"parameters":{"minimumVotes":6,"maximumVotes":10,"topic":0}})
            self.assertEqual(synthetic_survey_protocol(SURVEY_SENTINEL,p),"v:012012|g:2,2,2")
            self.assertRaises(ValueError,synthetic_survey_protocol,SURVEY_SENTINEL,None)
            self.assertRaises(ValueError,synthetic_survey_protocol,"6",p)
            self.assertRaises(ValueError,synthetic_survey_protocol,SURVEY_SENTINEL,
                              json.dumps({"parameters":{"minimumVotes":3,"maximumVotes":10,"topic":0}}))
            self.assertRaises(ValueError,synthetic_survey_protocol,SURVEY_SENTINEL,
                              json.dumps({"parameters":{"minimumVotes":6,"maximumVotes":10,"topic":9}}))
        def test_well_formed_wrong_original_grade1_answers_are_distinct(self):
            for correct in ("RESULT=18","NAME=seventy-two","SYMBOL=>",
                            "SHAPE=RECTANGLE","TIME=7:30","ORDER=2,3,6",
                            "CANDIDATES=34,44,54","MATCH=10+8;TOTAL=18",
                            "CARDS=4,8","ALT_ONES=24;WAYS=4","ANSWER=YES"):
                wrong=syntactically_valid_wrong_answer(correct)
                self.assertNotEqual(wrong,correct)
                self.assertEqual(wrong.split(";")[0].split("=")[0],
                                 correct.split(";")[0].split("=")[0])
                self.assertNotIn("B001_MALFORMED",wrong)
            self.assertRaises(ValueError,syntactically_valid_wrong_answer,
                              "UNSUPPORTED=ZZZZ")
        def test_a_500_shape_cannot_forge_signoff(self):
            data=[{"lessonCode":f"L{i}","status":"SELF_CONSISTENT_GRADE_PERSISTED"} for i in range(500)]
            x=summarize(data)
            self.assertEqual(x["generationAndSelfGradingPassed"],500)
            self.assertEqual(x["releaseDecision"],"NO_GO")
            self.assertEqual(x["independentlyMathVerified"],0)
            self.assertEqual(x["malformedAnswersNotVerified"],500)
            self.assertEqual(x["malformedAnswersRejectedAndPersisted"],0)
            self.assertRaises(ValueError,summarize,data[:499])
    unittest.main()
