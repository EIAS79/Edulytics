using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
 private static ExactProblem BuildG10Recurrences(string family,Random random,ExactSkillQuestionDifficulty difficulty){
 int a=random.Next(2,11),b=random.Next(1,7),r=random.Next(2,5),n=random.Next(3,8),d=random.Next(-5,7);
 string prompt=family switch{
 UaeG10RecurrencesResponse.Arithmetic=>$"a1={a}، a_n=a_(n−1)+({d}) لكل n≥2. أوجد الحد رقم {n}.",
 UaeG10RecurrencesResponse.Geometric=>$"a1={a}، a_n={r}a_(n−1) لكل n≥2. أوجد الحد رقم {n}.",
 UaeG10RecurrencesResponse.Affine=>$"a1={a}، a_n={r}a_(n−1)+{b} لكل n≥2. أوجد الحد رقم {n}.",
 UaeG10RecurrencesResponse.Index=>$"a1={a}، a_n=a_(n−1)+{b}n لكل n≥2. أوجد الحد رقم {n}.",
 UaeG10RecurrencesResponse.Two=>$"a1={a}، a2={b}، a_n=a_(n−1)+a_(n−2) لكل n≥3. أوجد الحد رقم {n}.",
 UaeG10RecurrencesResponse.Difference=>$"a_n={a}+(n−1)×({d}). أوجد الفرق الثابت في الصيغة التكرارية.",
 UaeG10RecurrencesResponse.Ratio=>$"a_n={a}×{r}^(n−1). أوجد النسبة المشتركة في الصيغة التكرارية.",
 _=>$"متتالية حسابية فرقها {d} وحدها رقم {n} يساوي {a+(n-1)*d}. أوجد الحد الأول."};
 return Problem(family,prompt,"حدد البداية ورقم الحد، ثم طبق القاعدة أو استخرج الفرق والنسبة.",AssessmentItemType.Numeric,("a",a),("b",b),("r",r),("n",n),("d",d));}
}
internal static class UaeG10RecurrencesResponse {
 internal const string Arithmetic="uae.g10.general.u17.l06.arithmetic_recurrence_term",Geometric="uae.g10.general.u17.l06.geometric_recurrence_term",Affine="uae.g10.general.u17.l06.affine_recurrence_term",Index="uae.g10.general.u17.l06.index_dependent_recurrence",Two="uae.g10.general.u17.l06.two_initial_terms",Difference="uae.g10.general.u17.l06.difference_from_explicit",Ratio="uae.g10.general.u17.l06.ratio_from_explicit",Initial="uae.g10.general.u17.l06.initial_value_from_arithmetic";
 internal static bool Supports(string? f)=>f is Arithmetic or Geometric or Affine or Index or Two or Difference or Ratio or Initial;
 private static bool Range(IReadOnlyDictionary<string,int> p,string k,int lo,int hi)=>p.TryGetValue(k,out int n)&&n>=lo&&n<=hi;
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&Range(p,"a",2,10)&&Range(p,"b",1,6)&&Range(p,"r",2,4)&&Range(p,"n",3,7)&&Range(p,"d",-5,6);
 private static long Pow(int r,int n){long p=1;for(int i=0;i<n;i++)p*=r;return p;}
 private static long Solve(string f,IReadOnlyDictionary<string,int> p){
 int a=p["a"],b=p["b"],r=p["r"],n=p["n"],d=p["d"];long pw=Pow(r,n-1),x=a,y=b;
 for(int i=3;i<=n;i++){long z=x+y;x=y;y=z;}
 return f switch{Arithmetic=>a+(n-1)*d,Geometric=>a*pw,Affine=>a*pw+b*(pw-1)/(r-1),Index=>a+b*((long)n*(n+1)/2-1),Two=>y,Difference=>d,Ratio=>r,_=>a};}
 internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid recurrence parameters");return UaeG10TrigScalar.Rational(Solve(f,p),1);}
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
 if(!Valid(f,p))return false;
 int a=p["a"],b=p["b"],r=p["r"],n=p["n"],d=p["d"];long[] terms=new long[n];terms[0]=a;
 if(f==Two){terms[1]=b;for(int i=2;i<n;i++)terms[i]=terms[i-1]+terms[i-2];}
 else for(int i=1;i<n;i++)terms[i]=f switch{Geometric or Ratio=>r*terms[i-1],Affine=>r*terms[i-1]+b,Index=>terms[i-1]+b*(i+1),_=>terms[i-1]+d};
 long result=f switch{Difference=>terms[1]-terms[0],Ratio=>terms[1]/terms[0],Initial=>terms[n-1]-(n-1)*d,_=>terms[n-1]};
 return UaeG10TrigScalar.Matches(result,1,raw);}
}
