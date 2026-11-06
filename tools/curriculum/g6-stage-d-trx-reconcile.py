#!/usr/bin/env python3
"""Grade 6 real TRX attribution, not authenticated E2E or academic acceptance."""
import argparse, json
from pathlib import Path
from collections import Counter
from xml.etree import ElementTree as ET
N={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
S={"D1":(1,50),"D2":(51,100),"D3":(101,147)}
def must(ok,reason):
    if not ok: raise SystemExit("G6_TRX_FAIL_CLOSED: "+str(reason))
def read(path):
    r=ET.parse(path).getroot(); defs={}
    for node in r.findall(".//t:TestDefinitions/t:UnitTest",N):
        m=node.find("t:TestMethod",N)
        must(node.get("id") and m is not None,"bad test definition "+str(path))
        defs[node.get("id")]=m.get("className")
    results=r.findall(".//t:Results/t:UnitTestResult",N)
    c=r.find(".//t:Counters",N)
    must(results and c is not None,"empty TRX "+str(path))
    must(int(c.get("total",-1))==len(results) and int(c.get("executed",-1))==len(results),"missing test execution")
    must(int(c.get("failed",-1))==0 and int(c.get("passed",-1))==len(results),"failed or skipped cases")
    classes=Counter()
    for x in results:
        tid=x.get("testId")
        must(tid in defs and defs[tid] and x.get("outcome")=="Passed","unmapped or failing test "+str(tid))
        classes[defs[tid]]+=1
    return classes,len(results)
def main():
    p=argparse.ArgumentParser()
    for name in ("inventory","trx-dir","output"):
        p.add_argument("--"+name,required=True,type=Path)
    p.add_argument("--candidate-sha",required=True)
    p.add_argument("--run-id",required=True)
    a=p.parse_args()
    must(len(a.candidate_sha)==40 and all(x in "0123456789abcdef" for x in a.candidate_sha),"invalid SHA")
    must(a.run_id.isdigit() and int(a.run_id)>0,"bad Actions id")
    inv=json.loads(a.inventory.read_text(encoding="utf-8"))
    rows=inv.get("rows")
    must(inv.get("lessonCount")==147 and isinstance(rows,list) and len(rows)==147,"wrong denominator")
    must(inv.get("sourceParity")=="UNKNOWN","unproved parity")
    must([x.get("ordinal") for x in rows]==list(range(1,148)),"nonsequential lessons")
    must(len(set(x.get("code") for x in rows))==147,"duplicate codes")
    lessons=[]; summary={}
    for shard,(lo,hi) in S.items():
        fp=a.trx_dir/("g6-"+shard+".trx")
        must(fp.is_file(),"missing shard artifact: "+str(fp))
        classes,count=read(fp)
        selected=[x for x in rows if lo<=x["ordinal"]<=hi]
        must(len(selected)==hi-lo+1,"shard incomplete "+shard)
        expected=set()
        for x in selected:
            f=x.get("testClasses")
            must(isinstance(f,list) and f,"no suite: "+x["code"])
            for t in f:
                cls="Edulytics.Tests.MathematicsIntelligence."+t["className"]
                must(cls not in expected,"duplicate class: "+cls)
                expected.add(cls)
        must(set(classes)==expected,"unexpected/missing real classes "+shard+" missing="+str(sorted(expected-set(classes))[:8])+" extra="+str(sorted(set(classes)-expected)[:8]))
        for x in selected:
            n=sum(classes["Edulytics.Tests.MathematicsIntelligence."+t["className"]] for t in x["testClasses"])
            must(n>0,"zero real tests for "+x["code"])
            lessons.append({"lessonCode":x["code"],"ordinal":x["ordinal"],"unit":x["unit"],"lesson":x["lesson"],
                "selectedMathTestStatus":"PASS","passedCases":n,"failedCases":0,
                "testClasses":[t["className"] for t in x["testClasses"]],"trx":fp.name,
                "sourceParity":"UNKNOWN","authenticatedE2E":"NOT_RUN","dStatus":"D_PENDING"})
        summary[shard]={"lessons":len(selected),"passed":count,"failed":0}
    lessons.sort(key=lambda x:x["ordinal"])
    must([x["ordinal"] for x in lessons]==list(range(1,148)),"final reconciliation incomplete")
    o={"recordType":"G6_EXACT_RUN_TRX_MATH_ONLY_PER_LESSON_EVIDENCE",
        "candidateSha":a.candidate_sha,"actionsRunId":int(a.run_id),"ciRefKind":"branch-head",
        "canonicalLessonCount":147,"unitCount":9,"mathPassedLessons":147,
        "totalPassedCases":sum(v["passed"] for v in summary.values()),"mathShards":summary,
        "lessons":lessons,"sourceParity":"UNKNOWN","authenticatedE2E":"NOT_RUN",
        "databaseAuthTenantIsolation":"NOT_RUN","visualAccessibility":"NOT_RUN",
        "stageDVerified":0,"releaseDecision":"NO_GO_TECHNICAL",
        "meaning":"Actual Grade 6 selected Practice math test results per lesson; not a complete Stage D acceptance."}
    a.output.parent.mkdir(parents=True,exist_ok=True)
    a.output.write_text(json.dumps(o,indent=2,ensure_ascii=False)+"\n",encoding="utf-8")
    print("G6_TRX_ATTRIBUTION_PASS 147/147 lesson suites, "+str(o["totalPassedCases"])+" math cases. No E2E/approval.")
if __name__=="__main__": main()
