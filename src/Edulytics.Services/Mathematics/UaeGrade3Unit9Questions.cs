using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 3 Unit 9 extension to existing exact-question engine.
// No Stage D/CI is executed within A-B-C; test source WRITTEN_NOT_RUN.
internal static class UaeGrade3Unit9Questions
{
 internal const string Decomp="uae.g3.u09.l01.decompose_multiplication";
 internal const string Distrib="uae.g3.u09.l02.distributive_property";
 internal const string Three="uae.g3.u09.l03.multiply_three_factors";
 internal const string Assoc="uae.g3.u09.l04.associative_property";
 internal const string WriteExp="uae.g3.u09.l05.write_expression";
 internal const string EvalExp="uae.g3.u09.l06.evaluate_expression";
 internal const string Equation="uae.g3.u09.l07.write_equation";
 internal const string TwoStep="uae.g3.u09.l08.two_step_word_problem";
 internal const string Logic="uae.g3.u09.l09.logical_reasoning";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Decomp or Distrib or Three or Assoc or WriteExp or EvalExp or Equation or TwoStep or Logic;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported G3 U09 family");
  int a=rng.Next(2,10),b=rng.Next(2,10),m=rng.Next(2),result;
  string prompt,solution;
  switch(f)
  {
   case Decomp:
    result=a*(b+2);
    prompt=$"اكسر {b+2} إلى {b} و٢. ما قيمة {a} × ({b} + ٢) باستخدام مصفوفتين متجاورتين؟";
    solution="أجد جزأي حاصل الضرب ثم أجمعهما؛ الضرب في مجموع = مجموع ناتجي الضرب.";break;
   case Distrib:
    result=a*(b+2);
    prompt=$"احسب ({a} × {b}) + ({a} × ٢) واكتب قيمة العبارة المساوية {a} × ({b} + ٢).";
    solution="الضرب موزع على الجمع: أضرب العامل الثابت في كل جزء ثم أجمع.";break;
   case Three:
    result=a*b*2;
    prompt=$"احسب حاصل ضرب العوامل الثلاثة {a} × {b} × ٢ بعد ترتيب الحساب في مجموعتين.";
    solution="أضرب عاملين ثم أضرب الناتج في العامل الثالث، وأراجع بالترتيب الآخر.";break;
   case Assoc:
    result=a*b*2;
    prompt=$"ما قيمة ({a} × {b}) × ٢؟ هل تساوي {a} × ({b} × ٢)؟ اكتب القيمة المشتركة.";
    solution="تجميع العوامل لا يغير حاصل الضرب؛ احسب بأي التجميعين.";break;
   case WriteExp:
    result=a*b+2;
    prompt=$"اكتب ناتج عبارة: {a} مجموعات في كل مجموعة {b} قطع، ثم أضف قطعتين. ما قيمة التعبير {a} × {b} + ٢؟";
    solution="أترجم المجموعات المتساوية إلى الضرب ثم أضيف القطعتين.";break;
   case EvalExp:
    result=m==0?a*b+2:a*(b+2);
    prompt=m==0?$"أوجد قيمة التعبير {a} × {b} + ٢، مع مراعاة ترتيب العمليات.":
      $"أوجد قيمة التعبير {a} × ({b} + ٢)؛ ابدأ بما داخل القوسين.";
    solution="أحسب ما داخل الأقواس أولًا ثم الضرب قبل الجمع في التعبير غير المقوس.";break;
   case Equation:
    result=a*b;
    prompt=$"في {a} صفوف، في كل صف {b} أشياء. أكمل المعادلة {a} × {b} = □ بعد تمثيل الصفوف.";
    solution="أكتب علامة المساواة بين تعبير الضرب وعدد العناصر الكلي.";break;
   case TwoStep:
    result=a*b+2;
    prompt=$"لدى {a} أطفال {b} ملصقات لكل منهم، ثم وصل ملصقان آخران. ما العدد الكلي بعد خطوتين؟";
    solution="الخطوة الأولى: أضرب عدد الأطفال في الملصقات لكل طفل؛ الثانية: أضيف الملصقين.";break;
   case Logic:
    result=a*(b+2);
    prompt=$"أي عدد يساوي {a} × {b} + {a} × ٢؟ استعمل التفكير المنطقي وخاصية التوزيع.";
    solution="أخرج العامل المشترك وأكتب العدد {a} × ({b}+٢) ثم أحسب.";break;
   default:throw new InvalidOperationException("Unsupported G3 U09 family");
  }
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,
   new Dictionary<string,int>{{"a",a},{"b",b},{"mode",m},{"result",result}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("mode",out int m)||!p.TryGetValue("result",out int got)||
     a<2||a>9||b<2||b>9||m<0||m>1)return false;
  expected=f switch{
   Decomp or Distrib or Logic=>a*(b+2),
   Three or Assoc=>2*a*b,
   WriteExp or TwoStep=>a*b+2,
   EvalExp=>m==0?a*b+2:a*(b+2),
   Equation=>a*b,
   _=>throw new InvalidOperationException("Unsupported family")};
  return got==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid G3 U09 parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
   c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==expected;
 }
}
