using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit14PerimeterAreaQuestions
{
 internal const string PerimeterRect="uae.g4.u14.l01.perimeter_rectangle";
 internal const string PerimeterSquare="uae.g4.u14.l01.perimeter_square";
 internal const string SimpleArea="uae.g4.u14.l02.simple_area";
 internal const string SimplePerimeter="uae.g4.u14.l02.simple_perimeter";
 internal const string TileCount="uae.g4.u14.l03.area_tiles";
 internal const string RowsCols="uae.g4.u14.l03.area_rows_columns";
 internal const string AreaRect="uae.g4.u14.l04.area_rectangle";
 internal const string AreaSquare="uae.g4.u14.l04.area_square";
 internal const string SameArea="uae.g4.u14.l05.area_from_dimensions";
 internal const string SamePerimeter="uae.g4.u14.l05.perimeter_from_dimensions";
 internal static readonly HashSet<string> Families=[PerimeterRect,PerimeterSquare,SimpleArea,
 SimplePerimeter,TileCount,RowsCols,AreaRect,AreaSquare,SameArea,SamePerimeter];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f!=null&&Families.Contains(f);
 internal static Item Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported area/perimeter question family");
  int a=random.Next(2,16),b=random.Next(2,16);
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0}};
  string prompt=f switch
  {
    PerimeterRect=>$"مستطيل طوله {a} سم وعرضه {b} سم. احسب محيطه بالسنتيمتر.",
    PerimeterSquare=>$"مربع طول ضلعه {a} سم. احسب محيطه بالسنتيمتر.",
    SimpleArea=>$"قسم سطح مستطيل إلى {a} صفوف و{b} أعمدة متساوية. كم مربع وحدة يغطيه؟",
    SimplePerimeter=>$"حل المسألة الأبسط: مستطيل طوله {a} سم وعرضه {b} سم، ما محيطه؟",
    TileCount=>$"شبكة فيها {a} صفوف و{b} أعمدة من مربعات الوحدة. ما المساحة بمربعات الوحدة؟",
    RowsCols=>$"مستطيل فيه {a} صفوف من مربعات الوحدة، وفي كل صف {b} مربعات. كم مربع وحدة؟",
    AreaRect=>$"مستطيل طوله {a} سم وعرضه {b} سم. ما مساحته بالسم المربع؟",
    AreaSquare=>$"مربع طول ضلعه {a} سم. ما مساحته بالسم المربع؟",
    SameArea=>$"مستطيل طوله {a} سم وعرضه {b} سم. احسب مساحته بالسم المربع.",
    _=>$"مستطيل طوله {a} سم وعرضه {b} سم. احسب محيطه بالسنتيمتر."
  };
  return new Item(f,prompt,"المحيط طول حدود الشكل، والمساحة عدد مربعات الوحدة داخل الشكل. لمستطيل المحيط يساوي ٢×(الطول+العرض) والمساحة الطول×العرض.",
   AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
 {
  result=0;
  if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
    a is <1 or >100||b is <1 or >100||c!=0)return false;
  result=f switch
  {
    PerimeterRect or SimplePerimeter or SamePerimeter=>2*(a+b),
    PerimeterSquare=>4*a,
    AreaSquare=>a*a,
    _=>a*b
  };
  return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid perimeter/area parameters");
   return ans.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(c=>c switch
   {
     >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
     >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
   }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
   return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
