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


def grade6_syntactically_valid_wrong_answer(correct, family=None):
    """Perturb a server-generated Grade 6 answer without breaking its numeric
    schema. The persisted grade is checked separately; this is not a publisher
    source oracle. Grade 1's custom multi-field wrong-answer rules are unchanged.
    """
    from decimal import Decimal
    if not isinstance(correct,str) or not correct.strip() or len(correct)>128:
        raise ValueError("Missing or unbounded Grade 6 answer")
    # Work for signed/unsigned integers, decimals, fractions, percentages,
    # coordinate pairs, numeric tables and labeled fields without inventing
    # an invalid-answer sentinel.
    number=re.search(r'(?<![A-Za-z0-9_.])[+-]?\d+(?:\.\d+)?(?![A-Za-z0-9_.])',correct)
    if number:
        old=number.group()
        candidate=correct[:number.start()]+format(Decimal(old)+Decimal(11),"f")+correct[number.end():]
    else:
        # These three families return explicit answer categories (not numeric
        # responses). Preserve the actual server's choice vocabulary when
        # submitting a syntactically valid but intentionally wrong answer.
        # Do not alter the production engine or any Grade 1/US contracts.
        family_options={
            "supporting.statistics.collection_method":("census","sample"),
            "supporting.statistics.scatter_correlation":("positive","negative","none"),
            "supporting.statistics.compare_range":("A","B","equal"),
        }.get(family)
        if family_options is not None:
            if correct.strip() not in family_options:
                raise ValueError("Unknown category for scoped statistical family")
            i=family_options.index(correct.strip())
            return family_options[(i+1)%len(family_options)]
        categories={
            "YES":"NO","NO":"YES","TRUE":"FALSE","FALSE":"TRUE",
            "GREATER":"LESS","LESS":"GREATER","EQUAL":"GREATER",
            "A":"B","B":"C","C":"A","D":"A",
            ">":"<","<":">","=":">",
            "ODD":"EVEN","EVEN":"ODD","POSITIVE":"NEGATIVE","NEGATIVE":"POSITIVE"
        }
        key=correct.strip().upper()
        if key in categories:
            candidate=categories[key]
        else:
            # Real UAE G6 mathematics uses Arabic categorical responses.
            # Choose a *different available category* from the same
            # mathematical question domain, never a malformed sentinel.
            # These are genuine response alternatives, not fake copies of
            # numbers or cosmetic suffixes. The runtime must independently
            # persist a zero score for the incorrect alternative.
            arabic_domains=(
                ("الأول","الثاني","الثالث","الرابع",
                 "المحور السيني","المحور الصادي","الأصل"),
                ("نعم","لا"),
                ("صحيح","خطأ"),
                ("يسار","يمين"),
                ("مفتوحة","مغلقة"),
                ("موجب","سالب","صفر"),
                ("الوسيط","المتوسط الحسابي","المنوال"),
                ("متماثل","منحرف إلى اليمين","منحرف إلى اليسار"),
                ("مخطط نقاط","مدرج تكراري","مخطط خطوط",
                 "مخطط أعمدة","مخطط الصندوق"),
                ("سم","كم","كجم","مل","م"),
                ("مسطرة","ميزان","كوب مدرج","شريط قياس"),
                ("الجمع","الطرح","الضرب","القسمة"),
                ("جمع","طرح","ضرب","قسمة"),
                ("أكبر","أصغر","يساوي"),
                ("متزايدة","متناقصة","ثابتة"),
                ("متزايد","متناقص","ثابت"),
                ("منتهٍ","دوري"),
                ("مجموع","فرق","حاصل ضرب","حاصل قسمة"),
                ("الأقواس","الأسس","الضرب والقسمة","الجمع والطرح"),
                ("التبديل","التجميع","المحايد","غير متكافئين"),
                ("مستقل","تابع"),
                # UAE G7 angle-type prompts return Arabic categorical answers.
                # Preserve the correct family schema for the graded wrong control.
                ("حادة","قائمة","منفرجة","مستقيمة"),
                ("حاد","قائم","منفرج","مستقيم"),
                ("حسابية","هندسية"),
                ("خطية","غير خطية"),
            )
            if family=="fractions.compare.unlike.order_three":
                # A source-valid ordering question expects exactly three
                # distinct labeled fractions and two comparison signs.
                m=re.fullmatch(r"([ABC])\s*([<>])\s*([ABC])\s*([<>])\s*([ABC])",correct.strip())
                if m is None or len({m[1],m[3],m[5]})!=3 or m[2]!=m[4]:
                    raise ValueError("Unsupported Grade 6 ordering answer schema")
                candidate=f"{m[1]} {m[2]} {m[5]} {m[4]} {m[3]}"
            elif family=="uae.g6.u05.l10.coordinate_region":
                domain=arabic_domains[0]
                if correct.strip() not in domain:
                    raise ValueError("Unknown UAE G6 coordinate region category")
                candidate=domain[(domain.index(correct.strip())+1)%len(domain)]
            else:
                domain=next(
                    (options for options in arabic_domains
                     if correct.strip() in options),None)
                if domain is None:
                    raise ValueError(
                        "Unknown Grade 6 answer category for valid-wrong probe: "
                        + str(family or "UNSCOPED"))
                candidate=domain[(domain.index(correct.strip())+1)%len(domain)]
    if candidate==correct or "B001_MALFORMED" in candidate:
        raise ValueError("Not distinct valid-wrong answer")
    return candidate


def grade7_syntactically_valid_wrong_answer(correct, family=None):
    """Preserve a *valid alternative mathematical category* for UAE Grade7
    angle-classification/angle-relation items, otherwise reuse G6's numeric
    perturbation. This is an E2E negative answer, not an engine/fallback change.
    """
    if family in (
        "supporting.geometry.angle_classify",
        "uae.g3.u13.l02.angle_types",
        "usccss.g7.u07.vertical_angle",
    ):
        # Each alternative is an existing mathematical angle/angle-pair type.
        # Preserve the source answer's language and letter case.
        domains = (
            ("acute", "obtuse", "right", "straight", "reflex"),
            ("acute angle", "obtuse angle", "right angle", "straight angle", "reflex angle"),
            ("vertical", "adjacent", "complementary", "supplementary"),
            ("vertical angles", "adjacent angles", "complementary angles", "supplementary angles"),
            ("حادة", "منفرجة", "قائمة", "مستقيمة", "منعكسة"),
            ("حادّة", "منفرجة", "قائمة", "مستقيمة", "منعكسة"),
            ("زاوية حادة", "زاوية منفرجة", "زاوية قائمة", "زاوية مستقيمة", "زاوية منعكسة"),
            ("متقابلتان بالرأس", "متجاورتان", "متتامتان", "متكاملتان"),
            ("متقابلتان", "متجاورتان", "متتامتان", "متكاملتان"),
        )
        clean = correct.strip()
        for group in domains:
            matches = [item.casefold() for item in group]
            if clean.casefold() in matches:
                idx = matches.index(clean.casefold())
                next_item = group[(idx + 1) % len(group)]
                if clean.isupper() and clean.isascii():
                    return next_item.upper()
                if clean.islower() and clean.isascii():
                    return next_item.lower()
                return next_item
    return grade6_syntactically_valid_wrong_answer(correct, family)


def syntactically_valid_wrong_answer(correct, family=None):
    """Create a real incorrect Grade 1 submission, preserving its answer
    field/schema; unlike a malformed sentinel, this exercises math grading.
    A full independent source oracle is tested separately in C#.
    """
    if isinstance(family,str) and (
        family.startswith("usccss.g6.") or
        family.startswith("uae.g2.") or
        family == "algebra.linear.ax_plus_b_equals_c"
    ):
        # Grade 8 uses the existing linear-equation numeric answer family,
        # not a Grade 1 semicolon-separated named-field response.
        return grade6_syntactically_valid_wrong_answer(correct)
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
            "AGREE":"DISAGREE","DISAGREE":"AGREE",
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
        if key.upper()=="AGREE" and family=="usccss.g1.u04.l07.read.two_digit":
            # The original learner-facing question names only Clare and Diego.
            if value.upper() not in ("CLARE","DIEGO"):
                raise ValueError("Unexpected original agreement claimant")
            replacement="CLARE" if value.upper()=="DIEGO" else "DIEGO"
        if key.upper()=="CURVED" and family=="usccss.g1.u07.l03.sort_flat_shapers":
            # A triangle is a genuine available source-category shape, but
            # incorrectly categorizing it as the curved shape must score zero.
            if value.upper()!="CIRCLE":
                raise ValueError("Unexpected original curved-shape answer")
            replacement="TRIANGLE"
        if key.upper()=="SHORTER" and value.upper() in ("MAI","CLARE","EQUAL"):
            # Original U06 L03 compares the two school routes; inventing
            # GREATER is not a well-formed answer to a shorter-walk question.
            replacement={"MAI":"CLARE","CLARE":"MAI","EQUAL":"MAI"}[value.upper()]
        if key.upper()=="LONGER" and value.upper()=="EQUAL":
            # SOURCE comparison uses two admissible label systems.
            replacement="RED" if family=="usccss.g1.u06.l01.compare_lengths" else "A"
        if key.upper()=="SORT" and value.upper() in ("EQUAL","UNEQUAL"):
            # G1 U07 L10 physically sorts equally/unequally divided shapes.
            # Both are the only source-valid labels; "GREATER" is not a sort.
            replacement="UNEQUAL" if value.upper()=="EQUAL" else "EQUAL"
        if replacement is None and key.upper()=="TRIANGLES":
            # U07 L05 asks for the three triangular cards among six original
            # labeled drawings. An erroneous three-card selection must still
            # contain THREE DISTINCT available letters, never an invented
            # shape label or a malformed list.
            labels=[part.strip().upper() for part in value.split(",")]
            if len(labels)!=3 or len(set(labels))!=3 or not set(labels)<=set("ABCDEF"):
                raise ValueError("Unexpected source triangle-card selection")
            substitute=next((letter for letter in "ABCDEF" if letter not in labels),None)
            if substitute is None:
                raise ValueError("No alternate triangle-card letter")
            replacement=",".join([labels[0],labels[1],substitute])
        if replacement is None and key.upper()=="ORDER":
            # Original G1 U06 L01 has THREE distinct measured strips; its
            # answer is a comma-separated permutation, not a scalar/category.
            # Reverse their order while preserving exactly the same labels,
            # keeping the negative answer syntactically valid and genuinely wrong.
            labels=[part.strip().upper() for part in value.split(",")]
            if len(labels)==3 and set(labels)=={"A","B","C"}:
                replacement=",".join(reversed(labels))
            else:
                raise ValueError("Unexpected source three-strip order format")
        if key.upper()=="MATCH" and family=="usccss.g1.u04.l08.match.tens_ones_expression" and value.upper()=="J,S":
            # Real source cards J and S show 32; card E shows 23. Include E
            # instead of S to form a well-formed two-card but wrong selection.
            replacement="J,E"
        if key.upper()=="MATCH" and value.upper() in ("A","B","C"):
            # Original U04 L02 asks which one of THREE written number-name
            # cards matches the base-ten representation: a single A/B/C,
            # unlike U06 L14's TWO distinct equations (comma-separated).
            replacement={"A":"B","B":"C","C":"A"}[value.upper()]
        if replacement is None and key.upper()=="MATCH":
            # The ORIGINAL U06 L14 source asks the child to circle exactly
            # TWO distinct equations. Keep the letter-pair answer format
            # intact while submitting a genuinely wrong pair.
            candidate=value.upper()
            if candidate not in ("A,B","A,C","B,C"):
                raise ValueError("Unexpected two-equation matching format")
            replacement="A,B" if candidate!="A,B" else "B,C"
        if replacement is None and key.upper()=="NAME":
            replacement="one" if value.lower()!="one" else "two"
        if replacement is None:
            raise ValueError("Unrecognized Grade 1 categorical answer "+key)
    wrong=";".join([key+"="+replacement,*parts[1:]])
    if wrong.upper()==correct.upper() or not wrong or "B001_MALFORMED" in wrong:
        raise ValueError("Valid-wrong fixture not distinct from expected")
    return wrong

def probe_lesson(opener,base,lesson_id,adoption_id,csrf,idempotency_key,sql,answer_mode="correct",wrong_answer_profile=None):
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
                # A 200 response can be the application's redirect to
                # the practice index after a genuine generation failure.
                # Surface only the user-facing alert, never auth tokens.
                response_body=response.read().decode("utf-8",errors="replace")
                import html
                alert=re.search(
                    r'<div[^>]*class="[^"]*school-alert-error[^"]*"[^>]*>(.*?)</div>',
                    response_body, flags=re.IGNORECASE|re.DOTALL)
                failure_message=(re.sub(r'<[^>]+>',' ',html.unescape(alert.group(1)))
                                 if alert else '').strip()
                return {"status":"GENERATION_ROUTE_NOT_ATTEMPT",
                        "httpStatus":response.status,"redirectPath":final,
                        "failureMessage":failure_message[:250]}
            attempt_id=match.group(1)
            page=response.read().decode("utf-8")
        # Fail closed if a Grade 6 attempt page embeds answer keys/validator
        # metadata in a machine-readable field before the learner submits.
        # Visible teaching examples are not answer-key leakage.
        if re.search(r'(?i)(?:"correctAnswer"\s*:|data-(?:correct|expected)-answer\s*=|name="correctAnswer")',page):
            return {"status":"G6_PRE_SUBMISSION_ANSWER_KEY_LEAK"}
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
        # Original 2021 G1 U03 L14 Stage 2: only a genuinely generated
        # two-card mode must show the first-party two-blank SVG in the
        # authenticated student's real Practice page. This is a targeted
        # V1 Stage-D learner-rendering check, not publisher artwork copying.
        l51_card_mode = None
        l51_board_rendered = False
        if family == "usccss.g1.u03.l14.center.add_subtract":
            try:
                payload=json.loads(parameter_json)
                params=payload.get("parameters",payload)
                if not isinstance(params,dict):
                    raise ValueError("Missing server-owned parameters")
                l51_card_mode=params.get("mode")
                if type(l51_card_mode) is not int or l51_card_mode not in (0,1,2,3):
                    raise ValueError("Invalid original Number Puzzles mode")
            except (ValueError,TypeError,AttributeError,json.JSONDecodeError):
                return {"status":"G1_L51_GENERATION_PARAMETER_INTEGRITY_FAILED",
                        "family":family}
            if l51_card_mode>=2:
                # Renderer creates a first-party accessible SVG with exactly
                # two blank card positions; answers must NOT be prefilled.
                import html
                student_page=html.unescape(page)
                l51_board_rendered=(
                    "<svg " in student_page and
                    "practice-math-visual" in student_page and
                    "Each card may only be used once" in student_page and
                    "Different cards, no repetition." in student_page)
                if not l51_board_rendered:
                    return {"status":"G1_L51_TWO_CARD_BOARD_NOT_IN_STUDENT_PRACTICE",
                            "family":family,"mode":l51_card_mode}
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
            # G2 reuses many first-party/shared numeric and category families.
            # Opt-in only: never change existing US or G1 negative controls.
            if wrong_answer_profile == "uae-g7":
                answer=grade7_syntactically_valid_wrong_answer(answer,family)
            elif wrong_answer_profile == "uae-g8":
                answer=grade7_syntactically_valid_wrong_answer(answer,family)
            elif wrong_answer_profile in ("uae-g2", "uae-g3", "uae-g4", "uae-g5", "uae-g6"):
                answer=grade6_syntactically_valid_wrong_answer(answer,family)
            else:
                answer=syntactically_valid_wrong_answer(answer,family)
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
        result={"status":"SELF_CONSISTENT_GRADE_PERSISTED","family":family}
        if l51_card_mode is not None:
            result["numberPuzzleMode"]=l51_card_mode
            result["numberPuzzleBoardRendered"]=l51_board_rendered
        return result
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
        def test_g6_well_formed_wrong_is_numeric_and_g1_does_not_change(self):
            self.assertEqual(grade6_syntactically_valid_wrong_answer("-4"),"7")
            self.assertEqual(syntactically_valid_wrong_answer("7","uae.g2.u01.l01.commutative_add"),"18")
            self.assertEqual(syntactically_valid_wrong_answer("0","uae.g2.u12.l05.dirham_fils"),"11")
            self.assertEqual(grade6_syntactically_valid_wrong_answer("2.50"),"13.50")
            self.assertEqual(grade6_syntactically_valid_wrong_answer("1/3"),"12/3")
            self.assertEqual(grade6_syntactically_valid_wrong_answer("(3,-2)"),"(14,-2)")
            self.assertEqual(grade6_syntactically_valid_wrong_answer("YES"),"NO")
            self.assertEqual(syntactically_valid_wrong_answer("n=2","usccss.g1.u01.l01.example"),"n=3")
            self.assertEqual(syntactically_valid_wrong_answer(
                "7","algebra.linear.ax_plus_b_equals_c"),"18")
            self.assertEqual(syntactically_valid_wrong_answer(
                "-2.5","algebra.linear.ax_plus_b_equals_c"),"8.5")
            self.assertRaises(ValueError,grade6_syntactically_valid_wrong_answer,"")
        def test_g7_angle_categories_preserve_valid_labels(self):
            self.assertEqual(
                grade7_syntactically_valid_wrong_answer(
                    "ACUTE", "supporting.geometry.angle_classify"), "OBTUSE")
            self.assertEqual(
                grade7_syntactically_valid_wrong_answer(
                    "زاوية قائمة", "uae.g3.u13.l02.angle_types"),
                "زاوية مستقيمة")
            self.assertEqual(
                grade7_syntactically_valid_wrong_answer(
                    "90", "supporting.geometry.angle_classify"), "101")
            self.assertRaises(ValueError,
                grade7_syntactically_valid_wrong_answer, "UNKNOWN_LABEL",
                "supporting.geometry.angle_classify")
        def test_g6_arabic_categories_are_legitimate_wrong_alternatives(self):
            domains=[
                ("الأول","uae.g6.u05.l10.coordinate_region"),
                ("المحور السيني","uae.g6.u05.l10.coordinate_region"),
                ("الأصل","uae.g6.u05.l10.coordinate_region"),
                ("نعم","uae.g6.u11.l01.statistical_question"),
                ("مفتوحة","uae.g6.u08.l06.endpoint_inclusion"),
                ("يسار","uae.g6.u08.l06.graph_direction"),
                ("الوسيط","uae.g6.u11.l07.choose_centre"),
                ("مخطط نقاط","uae.g6.u12.l08.choose_display"),
                ("كجم","uae.g6.u12.l09.choose_unit"),
                ("ميزان","uae.g6.u12.l09.choose_tool"),
                ("المنحرف","uae.g6.invalid-sentinel"),
            ]
            domains.extend([
                ("A < C < B","fractions.compare.unlike.order_three"),
                ("دوري","uae.g6.u05.l08.classify_fraction_decimal"),
                ("مجموع","uae.g6.u06.l01.classify_outer_operation"),
                ("الأسس","uae.g6.u06.l03.first_operation"),
                ("فرق","uae.g6.u06.l05.operation_type"),
                ("التجميع","uae.g6.u06.l08.classify_property"),
                ("مستقل","uae.g6.u08.l01.classify_variable"),
                ("حسابية","uae.g6.u08.l02.classify"),
                ("خطية","uae.g6.u08.l03.linear_vs_square")
            ])
            for value,family in domains:
                if family=="uae.g6.invalid-sentinel":
                    continue
                wrong=grade6_syntactically_valid_wrong_answer(value,family)
                self.assertNotEqual(value,wrong)
                self.assertNotIn("B001_MALFORMED",wrong)
            self.assertRaises(ValueError,
                grade6_syntactically_valid_wrong_answer,
                "المنحرف","uae.g6.invalid-sentinel")
        def test_g7_angle_classification_wrong_answer_uses_valid_category(self):
            for expected in ("حادة","قائمة","منفرجة","مستقيمة",
                             "حاد","قائم","منفرج","مستقيم"):
                wrong=grade6_syntactically_valid_wrong_answer(
                    expected,"supporting.geometry.angle_classify")
                self.assertNotEqual(wrong,expected)
                self.assertIn(wrong,("حادة","قائمة","منفرجة","مستقيمة",
                                     "حاد","قائم","منفرج","مستقيم"))
        def test_g7_real_statistical_family_wrong_categories(self):
            scenarios={
                "supporting.statistics.collection_method":("census","sample"),
                "supporting.statistics.scatter_correlation":("positive","negative","none"),
                "supporting.statistics.compare_range":("A","B","equal"),
            }
            for family,choices in scenarios.items():
                for answer in choices:
                    wrong=grade6_syntactically_valid_wrong_answer(answer,family)
                    self.assertIn(wrong,choices)
                    self.assertNotEqual(wrong,answer)
            self.assertRaises(ValueError,grade6_syntactically_valid_wrong_answer,
                              "unknown","supporting.statistics.collection_method")
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
        def test_source_valid_wrong_shorter_walk_and_equal_strip_comparison(self):
            self.assertEqual(syntactically_valid_wrong_answer(
                "SHORTER=MAI","usccss.g1.u06.l03.choose_indirect_comparison"),
                "SHORTER=CLARE")
            self.assertEqual(syntactically_valid_wrong_answer(
                "SHORTER=CLARE","usccss.g1.u06.l03.choose_indirect_comparison"),
                "SHORTER=MAI")
            self.assertEqual(syntactically_valid_wrong_answer(
                "SHORTER=EQUAL","usccss.g1.u06.l03.choose_indirect_comparison"),
                "SHORTER=MAI")
            self.assertEqual(syntactically_valid_wrong_answer(
                "LONGER=EQUAL","usccss.g1.u06.l01.compare_lengths"),
                "LONGER=RED")
            self.assertEqual(syntactically_valid_wrong_answer(
                "LONGER=EQUAL","usccss.g1.u06.l03.choose_indirect_comparison"),
                "LONGER=A")
            self.assertEqual(syntactically_valid_wrong_answer(
                "FITS=YES","usccss.g1.u06.l03.choose_indirect_comparison"),
                "FITS=NO")

        def test_single_original_tens_word_matching_card_remains_a_valid_distractor(self):
            for original,wrong in (
                ("MATCH=A;TENS=3;NUMBER=30","MATCH=B;TENS=3;NUMBER=30"),
                ("MATCH=B;TENS=4;NUMBER=40","MATCH=C;TENS=4;NUMBER=40"),
                ("MATCH=C;TENS=5;NUMBER=50","MATCH=A;TENS=5;NUMBER=50")):
                self.assertEqual(syntactically_valid_wrong_answer(original),wrong)

        def test_u04_l06_source_collection_claim_uses_well_formed_wrong_agreement(self):
            family="usccss.g1.u04.l06.collection.tens_ones"
            correct="DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50"
            wrong=syntactically_valid_wrong_answer(correct,family)
            self.assertEqual(
                wrong,
                "DECISION=AGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50")
            self.assertNotEqual(wrong,correct)

        def test_source_named_clare_diego_and_matching_card_distractors(self):
            self.assertEqual(syntactically_valid_wrong_answer(
                "AGREE=DIEGO;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six",
                "usccss.g1.u04.l07.read.two_digit"),
                "AGREE=CLARE;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six")
            self.assertEqual(syntactically_valid_wrong_answer(
                "MATCH=J,S;NUMBER=32;OTHER=23",
                "usccss.g1.u04.l08.match.tens_ones_expression"),
                "MATCH=J,E;NUMBER=32;OTHER=23")
            self.assertEqual(syntactically_valid_wrong_answer(
                "CURVED=CIRCLE;THREE_SIDES=TRIANGLE;FOUR_SIDES=SQUARE,RECTANGLE",
                "usccss.g1.u07.l03.sort_flat_shapers"),
                "CURVED=TRIANGLE;THREE_SIDES=TRIANGLE;FOUR_SIDES=SQUARE,RECTANGLE")

        def test_original_u07_l05_triangle_cards_keep_three_distinct_source_labels(self):
            family="usccss.g1.u07.l05.defining_triangles"
            self.assertEqual(syntactically_valid_wrong_answer(
                "TRIANGLES=A,B,C;NONTRIANGLES=D,E,F",family),
                "TRIANGLES=A,B,D;NONTRIANGLES=D,E,F")
            for invalid in ("TRIANGLES=A,A,B;NONTRIANGLES=D,E,F",
                            "TRIANGLES=A,B;NONTRIANGLES=D,E,F",
                            "TRIANGLES=X,Y,Z;NONTRIANGLES=D,E,F"):
                self.assertRaises(ValueError,
                                  syntactically_valid_wrong_answer,invalid,family)

        def test_original_g1_equal_and_unequal_partition_sort_stays_valid(self):
            self.assertEqual(
                syntactically_valid_wrong_answer("SORT=EQUAL;PIECES=4"),
                "SORT=UNEQUAL;PIECES=4")
            self.assertEqual(
                syntactically_valid_wrong_answer("SORT=UNEQUAL;PIECES=2"),
                "SORT=EQUAL;PIECES=2")

        def test_original_u06_l01_three_strip_order_stays_a_permutation(self):
            for original in ("ORDER=A,B,C","ORDER=B,A,C","ORDER=C,B,A",
                             "ORDER=A,C,B","ORDER=B,C,A","ORDER=C,A,B"):
                wrong=syntactically_valid_wrong_answer(original)
                self.assertNotEqual(wrong,original)
                self.assertTrue(wrong.startswith("ORDER="))
                self.assertEqual(set(wrong.split("=",1)[1].split(",")),
                                 {"A","B","C"})
            for invalid in ("ORDER=A,A,B","ORDER=A,B","ORDER=RED,BLUE,RED"):
                self.assertRaises(ValueError,syntactically_valid_wrong_answer,invalid)

        def test_well_formed_wrong_original_grade1_answers_are_distinct(self):
            for correct in ("RESULT=18","NAME=seventy-two","SYMBOL=>",
                            "SHAPE=RECTANGLE","TIME=7:30","ORDER=2,3,6",
                            "CANDIDATES=34,44,54","MATCH=10+8;TOTAL=18",
                            "CARDS=4,8","ALT_ONES=24;WAYS=4","ANSWER=YES",
                            "MATCH=A,C;TOTAL=20","MATCH=B,C;TOTAL=18",
                            "MATCH=A;TENS=3;NUMBER=30",
                            "MATCH=B;TENS=4;NUMBER=40",
                            "MATCH=C;TENS=5;NUMBER=50","ORDER=A,B,C","ORDER=C,B,A",
                            "SORT=EQUAL;PIECES=4","SORT=UNEQUAL;PIECES=2"):
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
