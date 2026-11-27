#!/usr/bin/env python3
"""Read-only evidence for the disposable catalogue rehearsal, not a production migration."""
import json, os, pathlib, subprocess, sys
ROOT=pathlib.Path(__file__).resolve().parents[2]
SCOPE="PED:UAE-MOE-MATH:L10:%"
assert os.environ.get("GITHUB_ACTIONS")=="true"
assert os.environ.get("EDULYTICS_CI_REHEARSAL")=="true"
assert os.environ.get("EDULYTICS_CI_REHEARSAL_PROGRAMME")=="UAE-G10"
assert os.environ.get("EDULYTICS_CLEAN_DATABASE_NAME")=="edulytics_clean_ci"
assert os.environ.get("ASPNETCORE_ENVIRONMENT")=="Staging"
def sql(query):
    return subprocess.check_output(["psql","-X","-h","127.0.0.1","-U","postgres","-d","edulytics_clean_ci","-Atq","-v","ON_ERROR_STOP=1","-c",query],text=True).strip()
def rows(query):
    return json.loads(sql("SELECT COALESCE(json_agg(row_to_json(q)),'[]') FROM ("+query+") q"))
def snapshot():
    target=rows('SELECT "Id"::text AS id,"Code" AS code,"Title" AS title,"Pathway" AS pathway,"SortOrder" AS ordinal,"UnitKey" AS unit FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE \''+SCOPE+'\' ORDER BY "Code"')
    meta=rows('SELECT c."Id"::text AS id,p."Code" AS code,c."Status" AS status,c."ContentVersion" AS version FROM "CurriculumLessonContents" c JOIN "CurriculumPedagogicalLessons" p ON p."Id"=c."LessonNodeId" WHERE p."Code" LIKE \''+SCOPE+'\' ORDER BY p."Code"')
    unrelated={}
    for table,where in [
      ("CurriculumPedagogicalLessons",'"Code" NOT LIKE \''+SCOPE+'\''),
      ("CurriculumLessonContents",'"LessonNodeId" NOT IN (SELECT "Id" FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE \''+SCOPE+'\')'),
      ("CurriculumPedagogicalLessonOutcomes",'"PedagogicalLessonId" NOT IN (SELECT "Id" FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE \''+SCOPE+'\')'),
      ("CurriculumPackContentNodes","true"),("CurriculumPackNodeLinks","true"),
      ("CurriculumPackImportStates","true"),("CurriculumFrameworks","true"),("CurriculumFrameworkVersions","true")]:
        unrelated[table]=rows('SELECT COUNT(*) AS count,md5(COALESCE(string_agg(to_jsonb(t)::text,chr(10) ORDER BY to_jsonb(t)::text),\'\')) AS digest FROM "'+table+'" t WHERE '+where)[0]
    assert len(target)==len(meta) and len({x["id"] for x in target})==len(target)
    assert {x["code"] for x in meta}=={x["code"] for x in target}
    assert all(x["status"]==3 for x in meta)
    return dict(candidateSha=os.environ["GITHUB_SHA"],target=target,metadata=meta,unrelated=unrelated,
                environment="GITHUB_ACTIONS_LOCAL_POSTGRESQL",productionWritten=False,tenantMigrationTested=False)
mode=sys.argv[1]
if mode=="replay":
    a,b=(json.loads(pathlib.Path(x).read_text()) for x in sys.argv[2:])
    assert a==b,"G10 replay changed catalogue identity, metadata marker or unrelated records"
    print("G10_ISOLATED_CATALOGUE_REPLAY_PASS")
    sys.exit(0)
out=pathlib.Path(sys.argv[2])
report=snapshot()
out.write_text(json.dumps(report,ensure_ascii=False,indent=2)+"\n")
if mode=="before":
    scope=json.loads((ROOT/"artifacts/curriculum-delivery/UAE-MOE-MATH/G10/grade10-legacy-rehearsal-scope.json").read_text())
    assert len(report["target"])==100
    assert {x["code"]:x["title"] for x in report["target"]}=={x["code"]:x["title"] for x in scope["legacyLessons"]}
    assert sum(x["pathway"]=="General" for x in report["target"])==48
    assert sum(x["pathway"]=="Advanced" for x in report["target"])==52
elif mode=="after":
    before=json.loads(pathlib.Path(sys.argv[3]).read_text())
    bp=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json").read_text())
    expected={x["LessonCode"]:x for x in bp["Lessons"]}
    actual={x["code"]:x for x in report["target"]}
    assert len(actual)==len(expected)==148 and set(actual)==set(expected)
    assert len({x["unit"] for x in actual.values()})==18
    assert all(x["pathway"]=="General" and x["title"]==expected[k]["Title"] and x["ordinal"]==expected[k]["SortOrder"] for k,x in actual.items())
    assert all(actual[x["code"]]["id"]==x["id"] for x in before["target"] if x["pathway"]=="General")
    assert report["unrelated"]==before["unrelated"],"Unrelated curriculum reference records changed"
else:
    raise ValueError("Invalid snapshot phase")
print("G10_ISOLATED_CATALOGUE_"+mode.upper()+"_PASS")
