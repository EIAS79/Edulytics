const fs=require("fs"),path=require("path"),cp=require("child_process");
const root=path.resolve(__dirname,"..");
const pairs=[
["us-ccss-math-hs-vector-matrix-phase29-v1.lesson-content-pack.json","PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01"],
["us-ccss-math-hs-prob-decision-phase29-v1.lesson-content-pack.json","PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05"]
];
const rows=pairs.map(([file,code])=>{
 const rel="src/Edulytics.Core/Curriculum/LessonContent/Packs/"+file;
 const before=JSON.parse(cp.execFileSync("git",["show","origin/main:"+rel],{cwd:root,encoding:"utf8"}));
 const after=JSON.parse(fs.readFileSync(path.join(root,rel),"utf8"));
 const old=before.lessons.find(x=>x.lessonCode===code),now=after.lessons.find(x=>x.lessonCode===code);
 if(!old||!now||old.sourceSha256!==now.sourceSha256||old.canonicalBodySha256===now.canonicalBodySha256)throw Error(code);
 return {packCode:"US-CCSS-MATH",lessonCode:code,file:rel,expectedOriginalCanonicalBodySha256:old.canonicalBodySha256,expectedCorrectedCanonicalBodySha256:now.canonicalBodySha256,expectedUnchangedSourceSha256:old.sourceSha256};
});
const manifest={schemaVersion:1,description:"Exactly two publisher-sourced US CCSS body errors reviewed and repaired without modifying student records or official curriculum graph",allowUnlistedContentChanges:false,repairs:rows};
const target=path.join(root,"docs/curriculum/us-ccss-verified-body-repairs.v1.json");fs.writeFileSync(target,JSON.stringify(manifest,null,2)+"\n");console.log(JSON.stringify(manifest,null,2));