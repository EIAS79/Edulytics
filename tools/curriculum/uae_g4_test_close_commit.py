import argparse,json,re,subprocess,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
P="artifacts/curriculum-delivery/UAE-MOE-MATH/G4"
def run(*args,capture=False):
    cmd=list(args)
    result=subprocess.run(cmd,cwd=ROOT,text=True,encoding="utf-8",errors="replace",
                          stdout=subprocess.PIPE if capture else None,
                          stderr=subprocess.STDOUT if capture else None)
    if capture:print(result.stdout,flush=True)
    if result.returncode!=0:raise RuntimeError("Command failed: "+" ".join(cmd))
    return result.stdout if capture else ""
def main():
    parser=argparse.ArgumentParser()
    parser.add_argument("spec",type=Path)
    parser.add_argument("--test-class",required=True)
    parser.add_argument("--test-file",required=True)
    parser.add_argument("--generator-file")
    parser.add_argument("--extra-file",action="append",default=[])
    args=parser.parse_args()
    spec=json.loads(args.spec.read_text(encoding="utf-8-sig"))
    ordinal,unit,index=(spec[k] for k in ("Ordinal","UnitIndex","LessonIndex"))
    code=f"PED:UAE-MOE-MATH:L4:COMMON:{unit:02}:{index:02}"
    assert spec["LessonCode"]==code
    assert spec["Ordinal"]<=spec.get("TargetLessons",132)
    branch=run("git","branch","--show-current",capture=True).strip()
    assert branch=="curriculum/uae-g4-arabic-a-b-c-20261111"
    assert (ROOT/args.test_file).is_file()
    before=run("git","status","--porcelain",capture=True)
    if "global.json" in before:raise RuntimeError("global.json already modified; abort")
    g=ROOT/"global.json"
    backup=g.read_bytes()
    try:
        g.write_text('{"sdk":{"version":"10.0.301"}}',encoding="utf-8")
        output=run("dotnet","test",str(ROOT/"tests/Edulytics.Tests/Edulytics.Tests.csproj"),
            "--no-restore","--filter",f"FullyQualifiedName~{args.test_class}",
            "--logger","console;verbosity=normal","-v","quiet",capture=True)
    finally:
        g.write_bytes(backup)
    pass_match=re.search(r"Passed:\s*(\d+)\s*",output)
    total_match=re.search(r"Total tests:\s*(\d+)\s*",output)
    assert "Test Run Successful." in output and pass_match and total_match, "No successful test result"
    passed,total=int(pass_match.group(1)),int(total_match.group(1))
    assert passed==total and passed>=3
    spec["PassedTests"]=passed
    args.spec.write_text(json.dumps(spec,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    run(sys.executable,str(ROOT/"tools/curriculum/uae_g4_close_from_spec.py"),str(args.spec))
    assert "global.json" not in run("git","status","--porcelain",capture=True)
    tracked=[
       "docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json",
       "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json",
       "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json",
       "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json",
       "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs",
       "src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs",
       f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u{unit:02}-l{index:02}-ar.rich-lesson-v2.json",
       args.test_file,
       str(args.spec.relative_to(ROOT)).replace("\\","/"),
    ]
    if args.generator_file:tracked.append(args.generator_file)
    tracked.extend(args.extra_file)
    for item in tracked:
        if (ROOT/item).exists():run("git","add","--",item)
    artifact_names=["full-grade-progress.json",f"lesson-{ordinal:03}-atomic-source-coverage.json",
                    f"lesson-{ordinal:03}-stage-a-c-checkpoint.json"]
    if spec.get("UnitClosed"):artifact_names.append(f"unit-{unit:03}-stage-a-c-closure.json")
    for name in artifact_names:
        path=f"{P}/{name}"
        assert (ROOT/path).exists(),path
        run("git","add","-f","--",path)
    run("git","diff","--cached","--check")
    leftovers=run("git","status","--porcelain",capture=True)
    assert not any(line.startswith("?? ") for line in leftovers.splitlines()),"Unexpected untracked files: "+leftovers
    run("git","-c","user.name=Elioo","-c","user.email=147754375+EIAS79@users.noreply.github.com",
        "commit","-m",f"feat(uae-g4): close U{unit:02} L{index:02} source-aligned ABC")
    run("git","push","origin","HEAD")
    assert not run("git","status","--porcelain",capture=True).strip()
    commit=run("git","rev-parse","--short","HEAD",capture=True).strip()
    print(f"CLOSED_AND_PUSHED lesson={ordinal} code={code} tests={passed}/{total} commit={commit}",flush=True)
if __name__=="__main__":main()
