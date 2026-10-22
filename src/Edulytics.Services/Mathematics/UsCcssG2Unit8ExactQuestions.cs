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
  return mode switch{
   10=>$"FIRST-PARTY: {2*a} objects divided equally between two students. How many each?",
   11=>$"FIRST-PARTY: {2*a+1} objects divided equally into TWO groups. How many LEFT OVER?",
   12=>$"FIRST-PARTY: {2*a} objects in equal pairs. How many LEFT OVER?",
   13=>$"FIRST-PARTY: {2*a} objects make how many complete pairs?",
   20=>$"FIRST-PARTY: {2*a+1} objects. How many complete two-object pairs can be made?",
   21=>"FIRST-PARTY: How many student pairs can 18 pupils form?",
   22=>"FIRST-PARTY: How many student pairs can 20 pupils form?",
   23=>"FIRST-PARTY: 19 pupils form pairs; how many are unpaired?",
   30=>$"FIRST-PARTY: {2*a} objects are paired. How many leftover?",
   31=>$"FIRST-PARTY: {2*a+1} objects are paired. How many leftover?",
   32=>$"FIRST-PARTY: {2*a} split in TWO equal groups. How many per group?",
   33=>$"FIRST-PARTY: {2*a+1} objects form how many pairs?",
   40=>$"FIRST-PARTY: Two equal addends {a}+{a} give what EVEN number?",
   41=>$"FIRST-PARTY: Two equal addends plus ONE, {a}+{a}+1. Total?",
   42=>$"FIRST-PARTY: {2*a}={a}+{a}. What is one equal addend?",
   43=>$"FIRST-PARTY: {2*a+1} objects cannot split into two equal integer shares. Number leftover?",
   50=>$"FIRST-PARTY: Add 1 to {2*a} even objects; how many remain unpaired?",
   51=>$"FIRST-PARTY: Add 2 to {2*a} even objects; how many remain unpaired?",
   52=>$"FIRST-PARTY: What EVEN number follows {2*a} when adding 2?",
   53=>$"FIRST-PARTY: What ODD number follows {2*a-1} when adding 2?",
   60=>$"FIRST-PARTY: Skip count by2 after {2*a}; next value?",
   61=>$"FIRST-PARTY: Skip count by5 after {5*a}; next value?",
   62=>$"FIRST-PARTY: Skip count by10 after {10*a}; next value?",
   63=>$"FIRST-PARTY: Three +2 jumps after {2*a}; what value?",
   70=>"FIRST-PARTY: True array 2 rows by3 columns: total?",
   71=>"FIRST-PARTY: True array 3 rows by3 columns: total?",
   72=>"FIRST-PARTY: Can 7 dots form a rectangle of MORE THAN ONE row and column, no extras? Answer 1 yes,0 no.",
   73=>"FIRST-PARTY: True array 3 rows by4 columns: total?",
   80=>$"FIRST-PARTY: Array with {a} rows and {b} columns. Number objects?",
   81=>$"FIRST-PARTY: Array {a} rows by {b} columns. How many COLUMNS?",
   82=>$"FIRST-PARTY: Array {a} rows by {b} columns. How many ROWS?",
   83=>$"FIRST-PARTY: {b} columns with {a} counters each. What is the total?",
   90=>"FIRST-PARTY: Full waffle tray 5 rows by4 columns: spaces?",
   91=>$"FIRST-PARTY: {a} rows of {b}. Find equal-addends total?",
   92=>$"FIRST-PARTY: {a} rows of {b}. Number row addends?",
   93=>$"FIRST-PARTY: {a} rows of {b}. Number column addends?",
   100=>$"FIRST-PARTY: {a} equal row groups with {b} dots each. Total?",
   101=>$"FIRST-PARTY: {b} equal column groups with {a} dots each. Total?",
   102=>$"FIRST-PARTY: {a} rows. How many addends in repeated row-addition?",
   103=>$"FIRST-PARTY: {b} columns. How many addends in repeated column-addition?",
   110=>$"FIRST-PARTY: Rectangular tiles {a} rows, {b} columns. Total equal squares?",
   111=>$"FIRST-PARTY: Rectangle has {b} tile columns. How many columns?",
   112=>$"FIRST-PARTY: Rectangle has {a} tile rows. How many rows?",
   113=>$"FIRST-PARTY: {a} by {b} square tiles; ONE tile removed. How many remain?",
   120=>$"FIRST-PARTY: Partition an invented rectangle into {a} equal rows and {b} equal square columns. Total?",
   121=>$"FIRST-PARTY: {a} rows of equal squares need how many INTERIOR horizontal boundaries?",
   122=>$"FIRST-PARTY: {b} columns need how many INTERIOR vertical boundaries?",
   123=>$"FIRST-PARTY: {a} rows by {b} columns equal squares. Total?",
   130=>"FIRST-PARTY: Four 2s =4+4. What is their common total?",
   131=>"FIRST-PARTY: Three 2s =3+3. What is their common total?",
   132=>"FIRST-PARTY: Six 2s compared with5+5. What DIFFERENCE?",
   133=>$"FIRST-PARTY: Add2 to {2*a}. New number?",
   _=>throw new InvalidOperationException("Unknown grade2 U08 mode")
  };
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade2 Unit8 exact parameters");
  int a=p["a"],b=p["b"],m=p["mode"];
  int result=m switch{
   10=>a,11=>1,12=>0,13=>a,
   20=>a,21=>9,22=>10,23=>1,
   30=>0,31=>1,32=>a,33=>a,
   40=>2*a,41=>2*a+1,42=>a,43=>1,
   50=>1,51=>0,52=>2*a+2,53=>2*a+1,
   60=>2*a+2,61=>5*a+5,62=>10*a+10,63=>2*a+6,
   70=>6,71=>9,72=>0,73=>12,
   80=>a*b,81=>b,82=>a,83=>a*b,
   90=>20,91=>a*b,92=>a,93=>b,
   100=>a*b,101=>a*b,102=>a,103=>b,
   110=>a*b,111=>b,112=>a,113=>a*b-1,
   120=>a*b,121=>a-1,122=>b-1,123=>a*b,
   130=>8,131=>6,132=>2,133=>2*a+2,
   _=>throw new InvalidOperationException("Unknown grade2 U08 numeric mode")
  };
  return result.ToString(CultureInfo.InvariantCulture);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
   f is not null&&Modes.TryGetValue(f,out var mode)&&p.Count==5&&
   p.TryGetValue("mode",out var supplied)&&supplied==mode&&p.TryGetValue("lesson",out var lesson)&&lesson==mode/10&&
   p.TryGetValue("a",out var a)&&a>=2&&a<=5&&p.TryGetValue("b",out var b)&&b>=2&&b<=5&&
   p.TryGetValue("c",out var c)&&c>=1&&c<=5;
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)=>
   Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
}
