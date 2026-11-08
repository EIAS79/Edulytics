#!/usr/bin/env python3
"""GitHub-only Chromium layout gate: 102 real authenticated Arabic G1 learner pages.

Screenshots are actual browser pixels; machine assertions are a smoke test,
not a human academic/rights/visual approval. Only disposable CI PostgreSQL.
"""
import hashlib
import json
import os
from pathlib import Path
import urllib.request
from playwright.sync_api import sync_playwright
from uae_g1_stage_d_authenticated_https import (
    required_environment, check_catalogue, login_actor, BASE, STUDENT
)

OUT = Path("/tmp/uae-g1-stage-d-browser-rtl")
SAMPLES = {1, 43, 74, 102}


def cookies_from_urllib(opener):
    for handler in opener.handlers:
        if isinstance(handler, urllib.request.HTTPCookieProcessor):
            cookies = [{
                "name": cookie.name,
                "value": cookie.value,
                "url": BASE,
                "httpOnly": cookie.has_nonstandard_attr("HttpOnly"),
                "secure": bool(cookie.secure),
            } for cookie in handler.cookiejar]
            if not cookies:
                raise AssertionError("No authenticated session cookies from real Grade1 login")
            return cookies
    raise AssertionError("Missing HTTP cookie handler from Grade1 login")


def inspect(page, ordinal, code, lesson_id, width, screenshot=False):
    url = f"{BASE}/student/learning/lesson/{lesson_id}?culture=ar"
    response = page.goto(url, wait_until="domcontentloaded", timeout=60000)
    if not response or response.status != 200:
        raise AssertionError(f"UAE Grade1 browser non-200 ordinal={ordinal}, status={response.status if response else 'none'}")
    root = page.locator(".uae-g1-original-arabic-lesson")
    if root.count() != 1:
        raise AssertionError(f"UAE Grade1 browser Arabic canonical body not found ordinal={ordinal} code={code}")
    attrs = root.evaluate("""e => {
      const r=e.getBoundingClientRect();
      const style=getComputedStyle(e);
      const kinds=['explanation','concepts','examples','steps','mistakes','summary'];
      const missing=kinds.filter(k=>!e.querySelector('.lesson-learning-section--'+k));
      const text=e.innerText || '';
      return {
        direction:style.direction,language:e.lang,display:style.display,
        width:r.width,clientWidth:e.clientWidth,scrollWidth:e.scrollWidth,
        missing:missing,hasArabic:/[\u0600-\u06FF]/.test(text),textLength:text.length
      };
    }""")
    if (attrs["direction"] != "rtl" or attrs["language"] != "ar" or
        attrs["width"] < 230 or attrs["missing"] or
        not attrs["hasArabic"] or attrs["textLength"] < 100):
        raise AssertionError(f"UAE Grade1 browser visual DOM invalid ordinal={ordinal}, attrs={attrs}")
    if attrs["scrollWidth"] > attrs["clientWidth"] + 16:
        raise AssertionError(f"UAE Grade1 Arabic content horizontally overflows width={width}, ordinal={ordinal}, attrs={attrs}")
    actual_headers = [
        h.strip() for h in root.locator(
            ".lesson-learning-section__header h2"
        ).all_inner_texts()
    ]
    expected_headers = [
        "الشرح", "المفاهيم والقواعد", "أمثلة محلولة",
        "الحل خطوة بخطوة", "الأخطاء الشائعة", "ملخص الدرس"
    ]
    if actual_headers != expected_headers:
        raise AssertionError(
            f"UAE Grade1 lesson heading localization invalid at {ordinal}: {actual_headers}"
        )
    body_text = root.inner_text()
    for placeholder in ("Concept model", "Understand", "Represent",
                        "Instructional diagram"):
        if placeholder in body_text:
            raise AssertionError(
                f"UAE Grade1 unapproved English placeholder {placeholder} at {ordinal}"
            )
    if page.locator(".lesson-mode-tab--active").count() and (
        page.locator(".lesson-mode-tab--active").inner_text().strip() != "الدرس"
    ):
        raise AssertionError(
            f"UAE Grade1 lesson navigation not localized at {ordinal}"
        )
    if "YouTube discovery could not load right now" in page.content():
        raise AssertionError(
            f"UAE Grade1 unconfigured YouTube widget leaked into canonical lesson {ordinal}"
        )
    hero = page.locator(".lesson-reader-hero")
    if (
        hero.locator(".lesson-library-eyebrow").inner_text().strip() != "محتوى الدرس" or
        hero.locator(".student-subject-code").inner_text().strip() != "رياضيات" or
        hero.locator(".lesson-reader-hero__unit").count() != 0 or
        hero.locator("h1").get_attribute("lang") != "ar" or
        hero.evaluate("e => getComputedStyle(e).direction") != "rtl"
    ):
        raise AssertionError(
            f"UAE Grade1 hero still has non-Arabic/duplicated metadata at {ordinal}"
        )
    row = {
        "lessonOrdinalSorted":ordinal,"lessonCode":code,"viewportWidth":width,
        "browserRtlComputedStyle":True,"arabicActualDOM":True,
        "allSixCanonicalSections":True,"noArabicBodyOverflow":True,
        "allSixSectionTitlesArabic":True,
        "noGenericEnglishInstructionalDiagram":True,
        "noUnconfiguredVideoWidget":True,
        "localizedHeroMetadata":True,
        "contentTextLength":attrs["textLength"],
    }
    if screenshot:
        path = OUT / f"uae-g1-lesson-{ordinal:03d}-{width}px.png"
        page.screenshot(path=str(path), full_page=True)
        row["screenshotSha256"] = hashlib.sha256(path.read_bytes()).hexdigest()
        row["screenshotFile"] = path.name
    return row


def main():
    required_environment()
    if os.environ.get("GITHUB_EVENT_NAME") not in ("push", "pull_request", "workflow_dispatch"):
        raise RuntimeError("Actual GitHub Actions invocation required")
    OUT.mkdir(parents=True, exist_ok=True)
    pairs, _ = check_catalogue()
    opener = login_actor(STUDENT, "Student")
    auth_cookies = cookies_from_urllib(opener)
    results = []
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        try:
            for width, height in ((1280, 860), (390, 844)):
                context = browser.new_context(
                    viewport={"width":width, "height":height},
                    locale="ar-AE",
                    ignore_https_errors=True,
                    reduced_motion="reduce",
                )
                context.add_cookies(auth_cookies)
                page = context.new_page()
                try:
                    for ordinal, (code, lesson_id) in enumerate(pairs, 1):
                        record = inspect(page, ordinal, code, lesson_id,
                                         width, screenshot=ordinal in SAMPLES)
                        results.append(record)
                        if ordinal % 20 == 0 or ordinal == 102:
                            print("UAE_G1_CHROMIUM_RTL_CHECK",
                                  width, ordinal, flush=True)
                finally:
                    context.close()
        finally:
            version = browser.version
            browser.close()
    if len(results) != 204:
        raise AssertionError(f"UAE G1 Chromium RTL page count {len(results)} != 204")
    report = {
        "scope":"UAE_G1_REAL_CHROMIUM_RTL_102_LESSONS_DESKTOP_AND_MOBILE_CI_NOT_HUMAN_APPROVAL",
        "candidateSha":os.getenv("GITHUB_SHA"),
        "browserVersion":version,
        "studentAuthenticated":True,
        "rtlDesktopLessons":102,
        "rtlMobileLessons":102,
        "screenshotsWritten":len([r for r in results if "screenshotSha256" in r]),
        "humanScreenshotReview":"NOT_DONE",
        "sourceAcademicParity":"NOT_VERIFIED",
        "rightsReview":"NOT_VERIFIED",
        "pages":results,
    }
    (OUT/"report.json").write_text(
        json.dumps(report,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print("UAE_G1_CHROMIUM_RTL_102_DESKTOP_102_MOBILE_PASS",flush=True)


if __name__=="__main__":
    main()
