const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const pack=require(path.join(root,'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.curriculum-pack.json'));
const authority=require(path.join(root,'docs/curriculum/us-ccss-authority-repair-candidates.json'));
const hash=x=>crypto.createHash('sha256').update(x,'utf8').digest('hex');
const codes=['5.G.A.1','5.NF.B.7','6.EE.A.2','7.NS.A.2'];
function officialText(code){
 const r=authority.rows.find(x=>x.code===code);
 if(!r)throw Error('Missing source authority '+code);
 let s=r.officialCombinedText
  .replace(/&divide;/g,'÷').replace(/&times;/g,'×').replace(/&plusmn;/g,'±')
  .replace(/&gt;/g,'>').replace(/&lt;/g,'<').replace(/&ne;/g,'≠')
  .replace(/\s+/g,' ').trim();
 if(code==='6.EE.A.2'){
  const needle='the formulas V = s and A = 6 s to find';
  if(!s.includes(needle))throw Error('Exponent source fragment missing');
  s=s.replace(needle,'the formulas V = s³ and A = 6s² to find');
 }
 if(code==='5.G.A.1')s=s.replace(/x -axis/g,'x-axis').replace(/x -coordinate/g,'x-coordinate').replace(/y -axis/g,'y-axis').replace(/y -coordinate/g,'y-coordinate');
 return {body:s,url:r.sourceUrl};
}
const replacements=codes.map(code=>{
 const n=pack.Nodes.find(x=>x.Code==='CCSS:'+code);
 if(!n)throw Error('Missing local node '+code);
 const source=officialText(code);
 const replacementHash=hash('EDULYTIKS-CCSS-TEXT-V1\n'+n.Code+'\n'+n.ContentHash+'\n'+source.body);
 return {code:n.Code,authorityUrl:source.url,expectedOldContentHash:n.ContentHash,
  expectedOldTextSha256:hash(n.OfficialText),correctedOfficialText:source.body,
  correctedTextSha256:hash(source.body),correctedContentHash:replacementHash};
});
const newDigest=hash('EDULYTIKS-CCSS-TEXT-V1\n'+pack.ContentDigest+'\n'+replacements.map(x=>x.code+':'+x.correctedContentHash).join('\n'));
const doc={schemaVersion:1,packCode:pack.PackCode,versionCode:pack.VersionCode,authority:'thecorestandards.org',baselineContentDigest:pack.ContentDigest,
  patchedContentDigest:newDigest,reason:'Restore incomplete source text and missing subordinate standard clauses without altering educational outcome identities.',
  replacements};
const dest=path.join(root,'src/Edulytics.Core/Curriculum/Packs/us-ccss-math.authority-text-repairs.json');
fs.writeFileSync(dest,JSON.stringify(doc,null,2)+'\n');
console.log('Manifest',dest,'Repairs',replacements.map(x=>x.code),'newDigest',newDigest);