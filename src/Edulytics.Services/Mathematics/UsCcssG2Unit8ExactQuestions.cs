using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>52 first-party exact Unit8 math families. Never grades publisher images, physical pairing, student arrays or center outcomes.</summary>
internal static class UsCcssG2Unit8ExactQuestions
{
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static readonly string[][] Names=new string[][]{
["equal_share_count","share_leftover","even_leftover","pair_count"],
["pairs_made","pairs_eighteen","pairs_twenty","one_unpaired"],
["even_no_left","odd_one_left","two_equal_groups","odd_pairs"],
["even_double","odd_double_plus_one","half_even","odd_remainder"],
["plus_one_parity","plus_two_parity","next_even","next_odd"],
["skip_two","skip_five","skip_ten","skip_two_three_jumps"],
["six_array","nine_array","seven_not_rectangular","three_by_four"],
["columns_total","columns_count","rows_count","column_skip_sum"],
["waffle_tray_total","array_sum","row_addends","column_addends"],
["row_repeated_sum","column_repeated_sum","number_rows","number_columns"],
["tile_rectangle_total","tile_columns","tile_rows","tiles_after_one_removed"],
["partition_total","horizontal_boundaries","vertical_boundaries","equal_squares"],
["four_pairs_sum","three_pairs_sum","compare_equal_addends","skip_two_next"]};
 private static readonly IReadOnlyDictionary<string,int> Modes=CreateModes();
 private static IReadOnlyDictionary<string,int> CreateModes()
 {
  var d=new Dictionary<string,int>(StringComparer.Ordinal);
  for(int lesson=1;lesson<=13;lesson++)
   for(int i=0;i<4;i++)d["usccss.g2.u08.l"+lesson.ToString("D2",CultureInfo.InvariantCulture)+"."+Names[lesson-1][i]]=10*lesson+i;
  return d;
 }
 public static bool Supports(string? f)=>f is not null&&Modes.ContainsKey(f);
 public static Problem Build(string family,Random random)
 {
  if(!Modes.TryGetValue(family,out var mode))throw new InvalidOperationException("Unknown Grade2 U08 first-party family");
  int a=random.Next(2,6),b=random.Next(2,6),c=random.Next(1,6);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"lesson",mode/10},{"mode",mode},{"a",a},{"b",b},{"c",c}};
  return new(family,Prompt(mode,a,b,c),"Independently authored numeric practice. Original source physical representations and explanations require teacher.",AssessmentItemType.Numeric,p);
 }
 private static string Prompt(int mode,int a,int b,int c)
 {
  int lesson=mode/10,kind=mode%10;
  if(lesson<=5)
  {
   int total=2*a+1;
   return kind switch{
    0=>$"FIRST-PARTY — {total} objects: how many complete PAIRS?",
    1=>$"FIRST-PARTY — {total} objects shared between TWO: how many LEFT OVER?",
    2=>$"FIRST-PARTY — {2*a} objects shared evenly with TWO: how many EACH?",
    _=>$"FIRST-PARTY — {a}+{a} objects in two equal groups: total?"};
  }
  if(lesson==6)return kind switch{
   0=>$"FIRST-PARTY — Next when counting by 2 from {2*a}?",
   1=>$"FIRST-PARTY — Next when counting by 5 from {5*a}?",
   2=>$"FIRST-PARTY — Next when counting by 10 from {10*a}?",
   _=>$"FIRST-PARTY — Three jumps of 2 from {2*a}, what number?"};
  if(lesson==7)return kind switch{
   0=>"FIRST-PARTY — 2 rows with 3 columns: how many dots?",
   1=>"FIRST-PARTY — 3 rows with 3 columns: how many dots?",
   2=>"FIRST-PARTY — Can 7 dots form a true rectangular array with MORE THAN 1 row and MORE THAN 1 column? Answer 1=yes 0=no.",
   _=>"FIRST-PARTY — Array 3 rows and 4 columns: total dots?"};
  if(lesson<=12)return kind switch{
   0=>$"FIRST-PARTY — Rectangular array {a} equal rows of {b} objects: total?",
   1=>$"FIRST-PARTY — A rectangle has {a} rows of equal tiles: how many rows?",
   2=>$"FIRST-PARTY — A rectangle has {b} columns of equal tiles: how many columns?",
   _=>lesson==12?$"FIRST-PARTY — {a} equally spaced rows of square tiles need how many INTERNAL horizontal dividing lines?"
                  :$"FIRST-PARTY — {b} equal columns of {a} objects: total?"};
  return kind switch{
    0=>"FIRST-PARTY — Four 2s equal 4+4. What common total?",
    1=>"FIRST-PARTY — Three 2s equal 3+3. What common total?",
    2=>"FIRST-PARTY — Six 2s compared to 5+5. What is the difference?",
    _=>$"FIRST-PARTY — Add 2 to the even number {2*a}: new value?"};
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade2 Unit8 exact family parameters");
  int a=p["a"],b=p["b"],lesson=p["lesson"],kind=p["mode"]%10;
  int v;
  if(lesson<=5)v=kind switch{0=>a,1=>1,2=>a,_=>2*a};
  else if(lesson==6)v=kind switch{0=>2*a+2,1=>5*a+5,2=>10*a+10,_=>2*a+6};
  else if(lesson==7)v=kind switch{0=>6,1=>9,2=>0,_=>12};
  else if(lesson<=12)v=kind switch{0=>a*b,1=>a,2=>b,_=>lesson==12?a-1:a*b};
  else v=kind switch{0=>8,1=>6,2=>2,_=>2*a+2};
  return v.ToString(CultureInfo.InvariantCulture);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
   f is not null&&Modes.TryGetValue(f,out var mode)&&p.Count==5&&
   p.TryGetValue("mode",out var supplied)&&supplied==mode&&p.TryGetValue("lesson",out var lesson)&&lesson==mode/10&&
   p.TryGetValue("a",out var a)&&a>=2&&a<=5&&p.TryGetValue("b",out var b)&&b>=2&&b<=5&&
   p.TryGetValue("c",out var c)&&c>=1&&c<=5;
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)=>
   Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
}
