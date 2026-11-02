#!/usr/bin/env python3
"""Read-only text extraction for original Grade 1 sources; no source files are copied."""
from html.parser import HTMLParser
from pathlib import Path
import sys
import re
class Reader(HTMLParser):
    def __init__(self):
        super().__init__(convert_charrefs=True)
        self.parts = []
        self.ignored = 0
    def handle_starttag(self, tag, attrs):
        if tag in ("script", "style", "noscript"):
            self.ignored += 1
        if not self.ignored and tag in ("p", "div", "h1", "h2", "h3", "h4", "li", "article", "section", "tr"):
            self.parts.append("\n")
    def handle_endtag(self, tag):
        if tag in ("script", "style", "noscript"):
            self.ignored = max(0, self.ignored - 1)
        if not self.ignored and tag in ("p", "div", "h1", "h2", "h3", "h4", "li", "article", "section", "tr"):
            self.parts.append("\n")
    def handle_data(self, data):
        if not self.ignored:
            self.parts.append(data)
root = Path(sys.argv[1])
for unit, lesson in [(2,21),(2,22),(3,1),(3,2),(3,3)]:
    for page in ("preparation", "lesson"):
        f=root/f"unit-{unit}"/f"lesson-{lesson}"/f"{page}.html"
        r=Reader()
        r.feed(f.read_text(encoding="utf-8"))
        raw="".join(r.parts)
        raw=re.sub(r"[ \t\r]+", " ", raw)
        raw=re.sub(r"\n(?:\s*\n)+", "\n", raw)
        raw=re.sub(r" ?\n ?", "\n", raw)
        print(f"\n=== {unit}.{lesson} {page} ===\n")
        print(raw[:int(sys.argv[2]) if len(sys.argv)>2 else 6500])
