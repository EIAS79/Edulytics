'use strict';
const fs=require('fs'),assert=require('assert');
const file='src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs';
const family='usccss.g4.u06.l16.base_ten_equal_groups';
let s=fs.readFileSync(file,'utf8');
function add(before,after){assert(s.includes(before),'Missing source anchor');s=s.replace(before,after);}
add('            "usccss.g4.u06.l15.mural_complete_columns",','            "usccss.g4.u06.l15.mural_complete_columns",\n            "'+family+'",');
const v=[
'            "'+family+'" =>',
'                parameters.TryGetValue("dividend",out var n102) &&',
'                parameters.TryGetValue("divisor",out var d102) &&',
'                parameters.TryGetValue("tensAfterTrade",out var t102) &&',
'                parameters.TryGetValue("onesAfterTrade",out var o102) &&',
'                d102>=2 && d102<=9 && n102>=24 && n102<=999 &&',
'                t102==n102/10%10+10*((n102/100)%d102) &&',
'                o102==n102%10+10*(t102%d102) && o102%d102==0 &&',
'                n102%d102==0 && value>0 && (long)value*d102==n102,',''
].join('\n');
add('            "geometry.perimeter_area.rectangle_missing_side" =>',v+'\n            "geometry.perimeter_area.rectangle_missing_side" =>');
add('            "usccss.g4.u06.l15.mural_complete_columns" => BuildG4MuralMissingSide(random, true),',
'            "usccss.g4.u06.l15.mural_complete_columns" => BuildG4MuralMissingSide(random, true),\n            "'+family+'" => BuildG4BaseTenDivision(random),');
const method=[
'    private static ExactProblem BuildG4BaseTenDivision(Random random)',
'    {',
'        var d=random.Next(2,10);',
'        var q=random.Next(12,Math.Min(120,999/d)+1);',
'        var n=d*q;',
'        var tens=n/10%10+10*((n/100)%d);',
'        var ones=n%10+10*(tens%d);',
'        return Problem("'+family+'",',
'            "Share "+n+" using base-ten blocks into "+d+" equal groups.",',
'            "Exchange hundreds to tens and tens to ones. Each group receives "+q+". Check multiplication.",',
'            AssessmentItemType.Numeric,("dividend",n),("divisor",d),',
'            ("tensAfterTrade",tens),("onesAfterTrade",ones));',
'    }',''
].join('\n');
add('    private static ExactProblem BuildG4MuralMissingSide(Random random, bool withRemainder)',
    method+'\n    private static ExactProblem BuildG4MuralMissingSide(Random random, bool withRemainder)');
add('            "usccss.g4.u06.l15.mural_complete_columns" =>\n                (p["area"] / p["knownSide"]).ToString(CultureInfo.InvariantCulture),',
'            "usccss.g4.u06.l15.mural_complete_columns" =>\n                (p["area"] / p["knownSide"]).ToString(CultureInfo.InvariantCulture),\n            "'+family+'" => (p["dividend"]/p["divisor"]).ToString(CultureInfo.InvariantCulture),');
fs.writeFileSync(file,s);
console.log('Patched one source-specific family in existing exact engine.');
