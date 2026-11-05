#!/usr/bin/env python3
"""Representative actual Chromium G8 desktop/mobile visual and ARIA smoke.
NOT a complete accessibility audit or publisher graphic certification.
Run only inside the disposable GitHub Actions G8 HTTPS fixture.
"""
import json, os, re, hashlib
from pathlib import Path
from playwright.sync_api import sync_playwright
from g8_stage_d_authenticated_https import sql,require

BASE="https://localhost:5052"
OUT=Path("/tmp/g8-stage-d-visual")
OUT.mkdir(parents=True,exist_ok=True)
MAIN=Path("/tmp/g8-131-isolated-auth-https-evidence.json")
def main():
    require(os.environ.get("GITHUB_ACTIONS")=="true" and
        os.environ.get("EDULYTICS_CI_REHEARSAL_PROGRAMME")=="US-CCSS-G8",
        "visual test outside disposable G8 CI")
    source=json.loads(MAIN.read_text(encoding="utf-8"))
    require(source.get("status")=="G8_131_REAL_AUTHENTICATED_HTTPS_PASS" and
        source.get("candidateSha")==os.environ.get("G8_CANDIDATE_SHA") and
        len(source.get("lessons",[]))==131,"missing same-run authenticated source evidence")
    samples=source["lessons"]
    require([x["ordinal"] for x in samples]==list(range(1,132)),
        "visual browser lessons not in canonical order")
    first_in_unit={}
    for case in samples:
        first_in_unit.setdefault(case["unit"],case["lessonCode"])
    require(set(first_in_unit)==set(range(1,10)),"missing visual unit")
    shots=[]
    visual_checks=[]
    with sync_playwright() as p:
        browser=p.chromium.launch(headless=True)
        context=browser.new_context(ignore_https_errors=True,viewport={"width":1280,"height":900},
            locale="en-US",device_scale_factor=1)
        tab=context.new_page()
        tab.goto(BASE+"/account/login?culture=en",wait_until="domcontentloaded",timeout=40000)
        tab.locator('label[for="role-student"]').click()
        require(tab.locator("#role-student").is_checked(),
                "student role card did not select AccountType")
        tab.locator("#Email").fill("commoncore-middle.student.primary@edulytiks.com")
        tab.locator("#Password").fill(os.environ["Edulytics__MeetingDemo__Password"])
        tab.locator("#login-form button[type=submit]").click(timeout=30000)
        tab.wait_for_url(lambda url:"/account/login" not in url,timeout=30000)
        require("localhost:5052" in tab.url,"Chromium student did not authenticate")
        for case in samples:
            lesson=case["lessonCode"]
            ids=sql("""SELECT "Id"::text FROM "CurriculumPedagogicalLessons"
                WHERE "Code"='"""+lesson+"""';""").splitlines()
            require(len(ids)==1,"missing visual lesson "+lesson)
            for mode,width,height in (("desktop",1280,900),("mobile",390,844)):
                tab.set_viewport_size({"width":width,"height":height})
                tab.goto(BASE+"/student/learning/lesson/"+ids[0],
                    wait_until="domcontentloaded",timeout=45000)
                tab.locator(".rich-lesson-section").first.wait_for(state="visible",timeout=40000)
                require(tab.locator(".lesson-reader-hero h1").count()==1,
                    "missing visual lesson heading "+lesson)
                require(tab.locator(".lesson-source-aligned-activities").count()>=1,
                    "canonical G8 teaching absent in browser "+lesson)
                missing=tab.locator("img:not([alt])").count()
                require(missing==0,"unlabeled image "+lesson+" "+mode+" count="+str(missing))
                tab.keyboard.press("Tab")
                focused=tab.evaluate("document.activeElement && document.activeElement.tagName")
                require(focused not in (None,"BODY","HTML"),"keyboard focus lost "+lesson+" "+mode)
                horizontal=tab.evaluate("document.documentElement.scrollWidth - window.innerWidth")
                require(horizontal<=2,
                    "G8 mobile/desktop horizontal document overflow "+lesson+
                    " "+mode+" px="+str(horizontal))
                visual_checks.append({"ordinal":case["ordinal"],"unit":case["unit"],
                    "lessonCode":lesson,"viewport":mode,
                    "richV2":"PASS","canonicalFirstPartyTeaching":"PASS",
                    "heading":"PASS","imageAlt":"PASS","keyboardFocus":"PASS",
                    "horizontalOverflowPx":horizontal})
                if first_in_unit[case["unit"]]==lesson:
                    filename=f"g8-u{case['unit']:02d}-{mode}.png"
                    destination=OUT/filename
                    tab.screenshot(path=str(destination),full_page=True,animations="disabled")
                    require(destination.is_file() and destination.stat().st_size>1000,
                        "empty Chromium screenshot "+filename)
                    shots.append({"unit":case["unit"],"lessonCode":lesson,"viewport":mode,
                        "width":width,"height":height,"path":filename,
                        "sha256":hashlib.sha256(destination.read_bytes()).hexdigest(),
                        "missingImageAlt":0,"keyboardFocusVerified":True,
                        "horizontalOverflowPx":horizontal})
                print("G8_D_CHROMIUM_VISUAL_PASS",case["ordinal"],lesson,mode,flush=True)
        browser.close()
    require(len(shots)==18,"missing representative screenshots")
    require(len(visual_checks)==262 and
        [x["ordinal"] for x in visual_checks[::2]]==list(range(1,132)),
        "all-lesson visual audit incomplete")
    report={"recordType":"G8_STAGE_D_REAL_CHROMIUM_REPRESENTATIVE_VISUALS",
        "candidateSha":source["candidateSha"],"runId":source["runId"],
        "unitsVisited":9,"lessonSnapshots":9,"screenshots":18,
        "allLessonViewportChecks":262,"visuallyCheckedDistinctLessons":131,
        "sourceParity":"UNKNOWN","visualFull131":"262_BASELINE_DOM_AND_RESPONSIVE_CHECKS_131_LESSONS_NOT_FULL_VISUAL_CERTIFICATION",
        "automatedAccessibilityCertification":"NOT_CLAIMED",
        "teacherAcademicApproval":"NOT_CLAIMED","releaseDecision":"NO_GO_TECHNICAL",
        "shots":shots,"perLessonViewportChecks":visual_checks}
    (OUT/"g8-nine-unit-chromium-visual-evidence.json").write_text(
        json.dumps(report,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print("G8_CHROMIUM_131_LESSONS_262_VIEWPORTS_18_SCREENSHOTS_PASS",flush=True)
if __name__=="__main__":
    main()
