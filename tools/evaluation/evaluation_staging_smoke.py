#!/usr/bin/env python3
from __future__ import annotations

import argparse
import ssl
from http.cookiejar import CookieJar
from urllib.error import HTTPError
from urllib.parse import urljoin, urlparse
from urllib.request import HTTPCookieProcessor, HTTPSHandler, Request, build_opener

LOCKED_HOST = "staging.edulytiks.com"


def fail(message: str) -> None:
    raise SystemExit("FAIL: " + message)


def validate_base(base: str) -> None:
    parsed = urlparse(base)
    if parsed.scheme != "https":
        fail("evaluation staging smoke requires HTTPS")
    if (parsed.hostname or "").lower() != LOCKED_HOST:
        fail(f"evaluation staging smoke is locked to {LOCKED_HOST}")


class Browser:
    def __init__(self, base: str) -> None:
        self.base = base.rstrip("/") + "/"
        self.jar = CookieJar()
        self.opener = build_opener(
            HTTPCookieProcessor(self.jar),
            HTTPSHandler(context=ssl.create_default_context()),
        )

    def request(self, path: str, method: str = "GET"):
        request = Request(
            urljoin(self.base, path),
            method=method,
            headers={
                "User-Agent": "Edulytics-Evaluation-Staging-Smoke/1.0",
                "Accept": "text/html,application/json",
            },
        )
        try:
            with self.opener.open(request, timeout=30) as response:
                body = response.read(128 * 1024).decode("utf-8", errors="replace")
                return response.status, response.geturl(), body
        except HTTPError as exc:
            return (
                exc.code,
                exc.geturl(),
                exc.read(128 * 1024).decode("utf-8", errors="replace"),
            )


def assert_protected(browser: Browser, path: str) -> None:
    status, final_url, body = browser.request(path)
    lowered = body.lower()
    if status == 200 and "/account/login" not in final_url.lower():
        fail(f"protected path was anonymously readable: {path}")
    if "studentnumber" in lowered or "currentmasterypercentage" in lowered:
        fail(f"protected path leaked evaluation data: {path}")
    print(f"PASS: anonymous access blocked for {path}")


def run(base: str) -> None:
    validate_base(base)
    browser = Browser(base)

    for path in ("/health/live", "/health/ready"):
        status, _, _ = browser.request(path)
        if status != 200:
            fail(f"{path} returned HTTP {status}")
        print(f"PASS: {path} = 200")

    assert_protected(browser, "/school/analytics")
    assert_protected(browser, "/student/progress")

    status, final_url, body = browser.request(
        "/school/analytics/student/00000000-0000-0000-0000-000000000001/intervention-check",
        method="POST",
    )
    if status in (200, 201, 202):
        fail("anonymous intervention POST unexpectedly succeeded")
    if "personalized intervention check created" in body.lower():
        fail("anonymous intervention POST returned success content")
    print(
        "PASS: anonymous intervention POST blocked "
        f"(status={status}, final={final_url})"
    )

    print("EVALUATION_STAGING_SMOKE_PASS")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--base-url")
    parser.add_argument("--self-test", action="store_true")
    args = parser.parse_args()

    if args.self_test:
        good = "https://staging.edulytiks.com"
        validate_base(good)
        for bad in (
            "http://staging.edulytiks.com",
            "https://edulytiks.com",
            "https://localhost",
            "https://example.com",
        ):
            try:
                validate_base(bad)
            except SystemExit:
                continue
            fail(f"self-test expected rejection for {bad}")
        print("EVALUATION_STAGING_SMOKE_SELF_TEST_PASS")
        return

    if not args.base_url:
        fail("--base-url is required")

    run(args.base_url)


if __name__ == "__main__":
    main()
