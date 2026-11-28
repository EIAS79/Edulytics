#!/usr/bin/env python3
"""UAE G10 General Arabic: authenticated Chromium visual/RTL smoke test.
148 real lesson pages in 1280px and 390px viewports in disposable GitHub CI.
No publisher item-parity or human screen-reader certification is asserted.
"""
import hashlib,json,os,urllib.request
from pathlib import Path
from playwright.sync_api import sync_playwright
from uae_g10_stage_d_authenticated_https import (
    required_environment,check_catalogue,login_actor,BASE,STUDENT
)

OUT=Path("/tmp/uae-g10-stage-d-browser-rtl")
SAMPLES={1,17,39,56,82,106,124,148}

def cookies_from_login(opener):
    for handler in opener.handlers:
        if isinstance(handler,urllib.request.HTTPCookieProcessor):
            result=[{"name":c.name,"value":c.value,"url":BASE,
                 "httpOnly":c.has_nonstandard_attr("HttpOnly"),
                 "secure":bool(c.secure)} for c in handler.cookiejar]
            assert result,"real G10 student session did not persist authentication"
            return result
    raise AssertionError("missing authenticated G10 cookie handler")

def inspect(page,ordinal,code,lesson_id,title,width,screenshot=False):
    response=page.goto(f"{BASE}/student/learning/lesson/{lesson_id}?culture=ar",
          wait_until="domcontentloaded",timeout=60000)
    assert response and response.status==200,("G10 browser HTTP not 200",ordinal)
    sections=page.locator(".lesson-learning-section")
    assert sections.count()>=6,("G10 student canonical section count",ordinal)
    for kind in ("explanation","concepts","examples","steps","mistakes","summary"):
        assert page.locator(".lesson-learning-section--"+kind).count()>=1,("G10 section",ordinal,kind)
    text=page.locator("body").inner_text()
    assert title in text,("G10 authored Arabic title missing",ordinal)
    assert any("\u0600"<=c<="\u06ff" for c in text),("G10 no Arabic",ordinal)
    body=page.locator("body")
    attrs=body.evaluate("""e=>{
      const s=getComputedStyle(e);
      return {direction:s.direction,clientWidth:e.clientWidth,
              scrollWidth:e.scrollWidth,language:e.lang,
              textLength:e.innerText.length};
    }""")
    root=page.locator(".lesson-learning-section--explanation").first
    context=root.evaluate("""e=>{const s=getComputedStyle(e);
      return {direction:s.direction,scrollWidth:e.scrollWidth,clientWidth:e.clientWidth}
    }""")
    if (context["direction"]!="rtl" or
        context["scrollWidth"]>context["clientWidth"]+16 or
        attrs["scrollWidth"]>attrs["clientWidth"]+16):
        # On failures only: inspect which *visible element* extends outside
        # the lesson section. Never weaken the viewport or RTL assertions.
        offenders=root.evaluate("""e=>{
          const a=e.getBoundingClientRect();
          return [...e.querySelectorAll('*')].map(x=>{
            const r=x.getBoundingClientRect(),s=getComputedStyle(x);
            return {tag:x.tagName,cls:String(x.className||'').slice(0,100),
              left:Math.round(r.left),right:Math.round(r.right),
              width:Math.round(r.width),scrollWidth:x.scrollWidth,
              clientWidth:x.clientWidth,overflowX:s.overflowX,
              snippet:(x.children.length? '':x.textContent||'').slice(0,90)}
          }).filter(x=>x.width>0 &&
              (x.left<a.left-16 || x.right>a.right+16)).slice(0,12)
        }""")
        raise AssertionError(
            f"G10 RTL / mobile overflow: {ordinal} {width} {context} {attrs} offenders={offenders}"
        )
    for forbidden in ("Concept model","Instructional diagram",
                      "YouTube discovery could not load right now"):
        assert forbidden not in text,("unapproved G10 generic fallback",ordinal,forbidden)
    result={"ordinal":ordinal,"lessonCode":code,"width":width,
       "bodyDirection":attrs["direction"],"contentDirection":context["direction"],
       "rtlComputed":True,"sixArabicSections":True,"noHorizontalOverflow":True,
       "textLength":len(text)}
    if screenshot:
        name=f"uae-g10-lesson-{ordinal:03d}-{width}px.png"
        dest=OUT/name
        page.screenshot(path=str(dest),full_page=True)
        result.update({"screenshotFile":name,
              "screenshotSha256":hashlib.sha256(dest.read_bytes()).hexdigest()})
    return result

def run():
    required_environment()
    if os.environ.get("GITHUB_EVENT_NAME") not in ("push","pull_request","workflow_dispatch"):
        raise RuntimeError("G10 Chromium test requires real GitHub Actions trigger")
    OUT.mkdir(parents=True,exist_ok=True)
    pairs,pack=check_catalogue()
    student=login_actor(STUDENT,"Student")
    cookies=cookies_from_login(student)
    records=[]
    with sync_playwright() as p:
        browser=p.chromium.launch(headless=True)
        try:
            for width,height in ((1280,900),(390,844)):
                ctx=browser.new_context(viewport={"width":width,"height":height},
                    locale="ar-AE",ignore_https_errors=True,reduced_motion="reduce")
                ctx.add_cookies(cookies)
                page=ctx.new_page()
                try:
                    for n,(code,lesson_id) in enumerate(pairs,1):
                        title=next(t["Title"] for t in pack[code]["Translations"] if t["CultureCode"]=="ar")
                        try:
                            result=inspect(page,n,code,lesson_id,title,width,
                                screenshot=n in SAMPLES)
                            result["status"]="PASS"
                        except Exception as exc:
                            result={"ordinal":n,"lessonCode":code,"width":width,
                                    "status":"FAIL","errorType":type(exc).__name__,
                                    "errorDetail":str(exc)[:500]}
                            print("G10_BROWSER_FAILURE",n,width,result["errorDetail"],flush=True)
                            try:
                                dest=OUT/f"uae-g10-failed-{n:03d}-{width}px.png"
                                page.screenshot(path=str(dest),full_page=False,timeout=10000)
                                result["screenshotFile"]=dest.name
                                result["screenshotSha256"]=hashlib.sha256(dest.read_bytes()).hexdigest()
                            except Exception:
                                pass
                        records.append(result)
                        print("G10_CHROMIUM",n,width,result["status"],flush=True)
                finally:
                    ctx.close()
        finally:
            version=browser.version
            browser.close()
    assert len(records)==296,"all 148 lessons must render on both real browser viewports"
    report={"candidateSha":os.environ["GITHUB_SHA"],
        "scope":"G10_148_REAL_CHROMIUM_ARABIC_DESKTOP_MOBILE_DISPOSABLE",
        "browserVersion":version,"studentAuthenticated":True,
        "rtlDesktopLessons":sum(x.get("status")=="PASS" and x["width"]==1280 for x in records),
        "rtlMobileLessons":sum(x.get("status")=="PASS" and x["width"]==390 for x in records),
        "screenshotsWritten":sum("screenshotSha256" in x for x in records),
        "sourcePublisherItemParity":"UNKNOWN","humanVisualCertification":"NOT_DONE",
        "productionWritten":False,"pages":records}
    (OUT/"report.json").write_text(json.dumps(report,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    assert report["rtlDesktopLessons"]==report["rtlMobileLessons"]==148,"G10 actual RTL page failures"
    print("UAE_G10_REAL_CHROMIUM_148_DESKTOP_148_MOBILE_PASS",flush=True)

if __name__=="__main__":run()
