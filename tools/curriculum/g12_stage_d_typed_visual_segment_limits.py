#!/usr/bin/env python3
"""Clamp original Grade12 first-party SVG numerical sampling to actual 128 segment contract.
Mathematical functions and domain exclusions remain unchanged."""
from pathlib import Path
import subprocess
ROOT=Path(__file__).resolve().parents[2]
BASE="src/Edulytics.Web/Presentation/Grade12PrecalculusSection"
changes={
"42GraphModels":[("for (var i=0; i<=120; i++)","for (var i=0; i<=120/Math.Max(1,functions.Length); i++)"),
 ("var x=xmin+(xmax-xmin)*i/120m;","var x=xmin+(xmax-xmin)*i/(120m/Math.Max(1,functions.Length));")],
"44GraphModels":[("for(var i=0;i<=220;i++)","for(var i=0;i<=120/Math.Max(1,funcs.Length);i++)"),
 ("var x=-7m+21m*i/220m;","var x=-7m+21m*i/(120m/Math.Max(1,funcs.Length));")],
"46GraphModels":[("for(var index=0; index<=200; index++)","for(var index=0; index<=120/Math.Max(1,funcs.Length); index++)"),
 ("var x=xmin+(xmax-xmin)*index/200m;","var x=xmin+(xmax-xmin)*index/(120m/Math.Max(1,funcs.Length));")],
"61SineCosineGraphs":[("for(int i=0;i<=240;i++)","for(int i=0;i<=120;i++)"),
 ("var x=-2*Math.PI+4*Math.PI*i/240d;","var x=-2*Math.PI+4*Math.PI*i/120d;")],
"62TrigGraphs":[("for(int i=0;i<=700;i++)","for(int i=0;i<=120;i++)"),
 ("var x=-2*Math.PI+4*Math.PI*i/700d;","var x=-2*Math.PI+4*Math.PI*i/120d;")],
"63InverseTrigGraphs":[("for(int i=0;i<=240;i++)","for(int i=0;i<=120;i++)"),
 ("var x=-2d+4d*i/240d;","var x=-2d+4d*i/120d;")]}
for name,pairs in changes.items():
 rel=BASE+name+".cs"
 p=ROOT/rel
 s=p.read_text(encoding="utf8") if p.exists() else subprocess.check_output(["git","show","HEAD:"+rel],cwd=ROOT).decode("utf8")
 for a,b in pairs:
  if s.count(a)!=1:raise ValueError((name,a,s.count(a)))
  s=s.replace(a,b,1)
 p.parent.mkdir(parents=True,exist_ok=True)
 p.write_text(s,encoding="utf8")
 print("CAPPED",name)
