using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Reuses shared exact engine; written in A-B-C, runtime test deferred.
internal static class UaeGrade3Unit3Lesson03Questions
{
 internal const string Family="uae.g3.u03.l03.choose_estimate_or_exact";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f==Family;
 public static Item Build(string f,Random r)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported grade-3 precision choice");
  int a=r.Next(200,1600),b=r.Next(100,a),mode=r.Next(2),context=r.Next(2);
  string prompt=mode==0
   ? context==0?$"كان لدى المكتبة {a} بطاقة، واستخدمت {b}. كم بطاقة بقيت بالضبط؟ هل الإجابة دقيقة أم تقديرية؟"
               :$"ثمن سلعة {a} درهمًا وقد دفعت {b}. كم درهمًا بقي بالضبط؟ هل الإجابة دقيقة أم تقديرية؟"
   : context==0?$"حضر نحو {a} زائر أمس ونحو {b} اليوم. كم تقريبًا الفرق؟ هل الإجابة دقيقة أم تقديرية؟"
               :$"قطعت حافلة نحو {a} كيلومتر وأخرى نحو {b}. كم تقريبًا الفرق؟ هل الإجابة دقيقة أم تقديرية؟";
  return new Item(f,prompt,"نوع الإجابة يتوقف على كلمة تقريبًا أو بالضبط وسياق المسألة.",
   AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"context",context},{"result",mode}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int mode)
 {
  mode=0;
  return Supports(f)&&p.Count==5&&p.TryGetValue("a",out int a)&&a is >=200 and <=1599&&
   p.TryGetValue("b",out int b)&&b>=100&&b<a&&
   p.TryGetValue("mode",out mode)&&mode is 0 or 1&&
   p.TryGetValue("context",out int context)&&context is 0 or 1&&
   p.TryGetValue("result",out int result)&&result==mode;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int mode))throw new InvalidOperationException("Bad precision-choice parameters");
  return mode==0?"دقيقة":"تقديرية";
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int mode)||string.IsNullOrWhiteSpace(answer))return false;
  string s=answer.Trim();
  return mode==0?s is "دقيقة" or "دقيق" or "exact" or "Exact":
                    s is "تقديرية" or "تقديري" or "estimate" or "Estimate";
 }
}
