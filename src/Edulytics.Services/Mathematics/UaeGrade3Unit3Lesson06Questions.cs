using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Extends existing exact engine for Grade3 4-digit subtraction; Stage D tests NOT_RUN.
internal static class UaeGrade3Unit3Lesson06Questions
{
 internal const string Family="uae.g3.u03.l06.four_digit_subtract_exact";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f==Family;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported G3 four-digit subtraction");
  int a=rng.Next(1000,10000),b=rng.Next(100,a),mode=rng.Next(3);
  int value=a-b;
  string prompt=mode==0?$"اطرح مع إعادة التجميع ثم تحقق بالجمع: {a} − {b} = ؟"
   :mode==1?$"في سجل {a} تذكرة، استُخدمت {b}. كم تذكرة بقيت؟"
   :$"استعمل مخطط المنازل الأربع وأوجد الفرق الدقيق بين {a} و{b}.";
  return new Item(f,prompt,"أحاذي المنازل وأبدأ بالآحاد، وأعيد التجميع ثم أتحقق بجمع الفرق والمطروح.",
   AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",value}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int result)
 {
  result=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||a is <1000 or >9999||
     !p.TryGetValue("b",out int b)||b<100||b>=a||
     !p.TryGetValue("mode",out int mode)||mode is <0 or >2||
     !p.TryGetValue("result",out int supplied))return false;
  result=a-b;return supplied==result;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int value))throw new InvalidOperationException("Invalid four-digit subtraction parameters");
  return value.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int value)||string.IsNullOrWhiteSpace(answer))return false;
  var normalized=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==value;
 }
}
