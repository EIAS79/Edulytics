using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// First-party U05 L06: modeling and multistep division; extends existing ExactSkillContractQuestionEngine.
internal static class UaeGrade3Unit5Lesson06Questions
{
 internal const string Family="uae.g3.u05.l06.modeled_division_word_problem";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? family)=>family==Family;
 public static Item Build(string family,Random random)
 {
  if(!Supports(family))throw new InvalidOperationException("Unsupported G3 model-word family");
  int groups=random.Next(2,9),each=random.Next(2,10),remaining=groups*each;
  int used=random.Next(1,11),start=remaining+used,mode=random.Next(3);
  int answer=mode==2?groups:each;
  string prompt=mode switch
  {
   0=>$"في صندوق {start} بطاقة، استُخدمت {used} منها، ثم وُزّع الباقي بالتساوي على {groups} مجموعات. كم بطاقة في كل مجموعة؟ ارسم شريطًا يمثل الكل والأجزاء.",
   1=>$"في مخزن {remaining} كرة، وُزّعت على {groups} فرق بالتساوي. كم كرة لكل فريق؟ مثّلها في مجموعات متساوية.",
   _=>$"معك {remaining} قطعة، وضعت {each} قطع في كل علبة متساوية. كم علبة استخدمت؟"
  };
  return new Item(family,prompt,"أحدد الكل وأطرح المستخدم إن وجد، ثم أقسم المتبقي بالتساوي وأتحقق بالضرب.",
    AssessmentItemType.Numeric,new Dictionary<string,int>{{"groups",groups},{"each",each},{"remaining",remaining},{"used",used},{"start",start},{"mode",mode},{"result",answer}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int answer)
 {
  answer=0;
  if(!Supports(f)||p.Count!=7||
    !p.TryGetValue("groups",out int groups)||groups is <2 or >8||
    !p.TryGetValue("each",out int each)||each is <2 or >9||
    !p.TryGetValue("remaining",out int rem)||rem!=groups*each||
    !p.TryGetValue("used",out int used)||used is <1 or >10||
    !p.TryGetValue("start",out int start)||start!=rem+used||
    !p.TryGetValue("mode",out int mode)||mode is <0 or >2||
    !p.TryGetValue("result",out int supplied))return false;
  answer=mode==2?groups:each;return supplied==answer;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int value))throw new InvalidOperationException("Invalid modeled division parameters");
  return value.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int value)||string.IsNullOrWhiteSpace(answer))return false;
  string digits=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
    c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)&&n==value;
 }
}
