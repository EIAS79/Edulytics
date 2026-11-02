const fs=require('fs');const p='C:/Users/khali/Edulytiks-US-CCSS-20261010/docs/curriculum/us-ccss-secondary-14-lesson-review.json';const data=JSON.parse(fs.readFileSync(p));const selected={
'G6:U06:L10':'TARGETED_DISTRIBUTIVE_PROPERTY',
'G6:U06:L11':'TARGETED_DISTRIBUTIVE_PROPERTY',
'G6:U06:L15':'TARGETED_GRADE6_EXPONENTS_REMOVED_LOGARITHMS',
'G6:U06:L19':'TARGETED_FUNCTION_TABLES',
'G7:U05:L08':'TARGETED_SIGNED_VELOCITY',
};
const accepted=[
['G6:U01:L05','KEEP_GEOMETRY_PRIMARY','Area formula/height of parallelogram is the lesson focus; general expression-code linkage does not require stand-alone coefficient questions'],
['G6:U01:L06','KEEP_GEOMETRY_PRIMARY','Area of parallelograms with variables/formulas'],
['G6:U01:L09','KEEP_GEOMETRY_PRIMARY','Triangle area formula and shape measurement'],
['G6:U01:L10','KEEP_GEOMETRY_PRIMARY','Bases and heights of triangles'],
['G6:U01:L18','KEEP_GEOMETRY_PRIMARY','Surface area of a cube uses area/expressions as supporting reasoning'],
['G6:U07:L10','KEEP_INEQUALITY_PRIMARY','Interpreting inequalities is the direct lesson focus and has its own bounded family'],
['G7:U05:L01','KEEP_SIGNED_NUMBER_CONCEPT_PRIMARY','Negative-number interpretation is the lesson focus, not all requirements of 7.NS.A.2.d'],
['G7:U08:L16','KEEP_STATISTICS_PRIMARY','Population-proportion estimation belongs to statistics; attached 7.NS.A.2.d is supplementary'],
['G7:U09:L04','KEEP_GEOMETRY_RATIO_PRIMARY','Restaurant floor-plan scale and geometric area are the lesson focus; recurring decimals are not the central target'],
];
for(const row of data.rows){const code=row.lessonCode.slice('PED:US-CCSS-MATH:'.length);if(selected[code]){row.decision=selected[code];row.runtimeMappingAdded=true;row.reason='Publisher lesson title and Addressing references support a narrower central question family; verified by runtime contract tests.';continue;}const item=accepted.find(x=>x[0]===code);if(!item)throw Error('Unreviewed lesson '+code);row.decision=item[1];row.runtimeMappingAdded=false;row.reason=item[2];}
data.summary={total:data.rows.length,explicitlyNarrowed:data.rows.filter(x=>x.runtimeMappingAdded).length,retainedWithPrimaryLessonEvidence:data.rows.filter(x=>!x.runtimeMappingAdded).length,limitations:'Title/reference cross-check is not independent certification of every a/b/c subclause or every contextual question.'};
fs.writeFileSync(p,JSON.stringify(data,null,2)+'\n');console.log(JSON.stringify(data.summary));