using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit13GraphsQuestions
{
 internal const string PlotValue="uae.g4.u13.l01.plot_read_value";
 internal const string PlotDifference="uae.g4.u13.l01.plot_difference";
 internal const string LineMax="uae.g4.u13.l02.line_graph_max";
 internal const string LineIncrease="uae.g4.u13.l02.line_graph_increase";
 internal const string PieCategory="uae.g4.u13.l03.pie_category_percent";
 internal const string PieRest="uae.g4.u13.l03.pie_rest_percent";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f is PlotValue or PlotDifference or LineMax or LineIncrease or PieCategory or PieRest || IsG5Map(f) || IsG5Coordinates(f) || IsG5GraphPattern(f);

 // UAE G5 U06 L09: exact numeric subskills of mapping routes on a
 // unit grid. Reuse shared G4 graph engine; freehand sketch teacher-reviewed.
 internal const string G5MapHorizontal="uae.g5.u06.l09.map_horizontal_steps";
 internal const string G5MapVertical="uae.g5.u06.l09.map_vertical_steps";
 internal const string G5MapRoute="uae.g5.u06.l09.map_shortest_grid_route";
 private static bool IsG5Map(string? f)=>f is G5MapHorizontal or G5MapVertical or G5MapRoute;
 private static Item BuildG5Map(string f,Random random)
 {
     int sx=random.Next(0,5),sy=random.Next(0,5),tx=random.Next(2,8),ty=random.Next(2,8);
     if(sx==tx&&sy==ty)tx=7;
     var p=new Dictionary<string,int>{{"sx",sx},{"sy",sy},{"tx",tx},{"ty",ty}};
     string start=$"({sx}، {sy})",goal=$"({tx}، {ty})";
     string prompt=f switch
     {
         G5MapHorizontal=>$"في خريطة شبكة وحدية، تقع المدرسة عند {start} وتقع المكتبة عند {goal}. كم خطوة أفقية، يمينًا أو يسارًا، تفصل الموقعين؟",
         G5MapVertical=>$"في خريطة شبكة وحدية، تقع المدرسة عند {start} وتقع الحديقة عند {goal}. كم خطوة رأسية، أعلى أو أسفل، تفصل الموقعين؟",
         _=>$"على شبكة وحدية، انطلق من {start} إلى {goal} بالسير أفقيًا ورأسيًا دون حركة قطرية. كم خطوة على أقصر طريق شبكي؟"
     };
     return new Item(f,prompt,"أرسم الخريطة، وأحسب الفارق الأفقي والرأسي من مواقع النقطتين دون الحركة القطرية.",
         AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Map(string f,IReadOnlyDictionary<string,int> p,out int expected)
 {
     expected=0;
     if(!IsG5Map(f)||p.Count!=4||
        !p.TryGetValue("sx",out int sx)||!p.TryGetValue("sy",out int sy)||
        !p.TryGetValue("tx",out int tx)||!p.TryGetValue("ty",out int ty)||
        new[]{sx,sy,tx,ty}.Any(v=>v is <0 or >7)||
        (sx==tx&&sy==ty))return false;
     int dx=Math.Abs(tx-sx),dy=Math.Abs(ty-sy);
     expected=f switch {G5MapHorizontal=>dx,G5MapVertical=>dy,_=>dx+dy};
     return true;
 }
 private static string SolveG5Map(string f,IReadOnlyDictionary<string,int> p)
 {
     if(!ValidG5Map(f,p,out int v))throw new InvalidOperationException("Invalid Grade5 map data");
     return v.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Map(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
     if(!ValidG5Map(f,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>20)return false;
     string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
        .Select(ch=>ch switch
        {
           >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
           >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
           _=>ch
        }).ToArray());
     if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual))return false;
     int dx=Math.Abs(p["tx"]-p["sx"]),dy=Math.Abs(p["ty"]-p["sy"]);
     return actual==(f==G5MapHorizontal?dx:f==G5MapVertical?dy:dx+dy);
 }


 // UAE G5 U06 L10: coordinate reading on first-quadrant grid, shared graph engine.
 // Short-answer ordered pair has structured-entry routing; drawings are manual.
 internal const string G5CoordX="uae.g5.u06.l10.coordinate_x";
 internal const string G5CoordY="uae.g5.u06.l10.coordinate_y";
 internal const string G5CoordPair="uae.g5.u06.l10.write_ordered_pair";
 private static bool IsG5Coordinates(string? f)=>f is G5CoordX or G5CoordY or G5CoordPair;
 private static Item BuildG5Coordinates(string f,Random rng)
 {
     int x=rng.Next(1,8),y=rng.Next(1,8);
     var p=new Dictionary<string,int>{{"x",x},{"y",y}};
     string q=f switch
     {
         G5CoordX=>$"على شبكة في الربع الأول، النقطة أ عند ({x}، {y}). ما الإحداثي الأفقي x؟",
         G5CoordY=>$"على شبكة في الربع الأول، النقطة أ عند ({x}، {y}). ما الإحداثي الرأسي y؟",
         _=>$"من نقطة الأصل تحرك {x} وحدات يمينًا و{y} وحدات لأعلى. اكتب الزوج المرتب (x,y) للنقطة."
     };
     return new Item(f,q,
         "العدد الأول هو الإحداثي الأفقي x والعدد الثاني هو الإحداثي الرأسي y.",
         f==G5CoordPair?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Coordinates(string f,IReadOnlyDictionary<string,int> p,
     out int x,out int y)
 {
     x=y=0;
     return IsG5Coordinates(f)&&p.Count==2&&p.TryGetValue("x",out x)&&
            p.TryGetValue("y",out y)&&x is >=1 and <=7 && y is >=1 and <=7;
 }
 private static string SolveG5Coordinates(string f,IReadOnlyDictionary<string,int> p)
 {
     if(!ValidG5Coordinates(f,p,out int x,out int y))
         throw new InvalidOperationException("Invalid G5 coordinate problem");
     return f==G5CoordX?x.ToString(CultureInfo.InvariantCulture):
            f==G5CoordY?y.ToString(CultureInfo.InvariantCulture):
            $"({x},{y})";
 }
 private static bool VerifyG5Coordinates(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
     if(!ValidG5Coordinates(f,p,out int x,out int y)||
        string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
     string s=new(answer.Trim().Select(ch=>ch switch
       {
          >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
          >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
          '،'=>',',
          _=>ch
       }).Where(ch=>!char.IsWhiteSpace(ch)).ToArray());
     if(f==G5CoordPair)
     {
         if(s.Length<5||s[0]!='('||s[^1]!=')')return false;
         var parts=s[1..^1].Split(',');
         return parts.Length==2&&
             int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int px)&&
             int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int py)&&
             px==x&&py==y;
     }
     if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual))return false;
     return actual==(f==G5CoordX?x:y);
 }


 // G5 U06 L11: numeric subskills of linearly increasing table/point graphs.
 // Reuse G4 shared graph engine; plotting and graph choice stay teacher-reviewed.
 internal const string G5GraphValue="uae.g5.u06.l11.graph_value_at_hour";
 internal const string G5GraphCompare="uae.g5.u06.l11.compare_linear_rentals";
 internal const string G5GraphIncrease="uae.g5.u06.l11.graph_linear_increase";
 private static bool IsG5GraphPattern(string? f)=>f is G5GraphValue or G5GraphCompare or G5GraphIncrease;
 private static Item BuildG5GraphPattern(string f,Random random)
 {
     if(f==G5GraphCompare)
     {
         int a=random.Next(2,8),b=random.Next(a+1,16),h=random.Next(2,5);
         return new Item(f,$"خطة أ لاستئجار دراجة تكلف {a} دراهم للساعة، وخطة ب تكلف {b} درهمًا للساعة. بعد {h} ساعات، كم يزيد ثمن الخطة ب عن أ؟",
           "كون جدول الساعات ١–٤ ثم ارسم النقطتين على مخطط وانظر إلى الفرق الرأسي.",
           AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"h",h}});
     }
     int rate=random.Next(2,15);
     if(f==G5GraphValue)
     {
         int h=random.Next(2,5);
         return new Item(f,$"على تمثيل بياني يربط الساعات بالتكلفة: النقاط (١،{rate})، (٢،{2*rate})، (٣،{3*rate}). ما الإحداثي الرأسي للتكلفة عند الساعة {h}؟",
          "ألاحظ أن كل ساعة ترفع التكلفة بقيمة ثابتة، وأطبق القاعدة.",
          AssessmentItemType.Numeric,new Dictionary<string,int>{{"rate",rate},{"h",h}});
     }
     int from=random.Next(1,3),to=random.Next(3,5);
     return new Item(f,$"توضح نقاط نمط خطي تكلفة بواقع {rate} دراهم للساعة. ما مقدار زيادة التكلفة من الساعة {from} إلى الساعة {to}؟",
      "أحسب التغير في الساعات ثم أضرب في معدل التكلفة لكل ساعة.",
      AssessmentItemType.Numeric,new Dictionary<string,int>{{"rate",rate},{"from",from},{"to",to}});
 }
 private static bool ValidG5GraphPattern(string f,IReadOnlyDictionary<string,int> p,out int expected)
 {
     expected=0;
     if(!IsG5GraphPattern(f))return false;
     if(f==G5GraphCompare)
     {
         if(p.Count!=3||!p.TryGetValue("a",out int a)||
            !p.TryGetValue("b",out int b)||!p.TryGetValue("h",out int h)||
            a is <2 or >7||b<=a||b>15||h is <2 or >4)return false;
         expected=(b-a)*h;return true;
     }
     if(f==G5GraphValue)
     {
         if(p.Count!=2||!p.TryGetValue("rate",out int rate)||
            !p.TryGetValue("h",out int h)||rate is <2 or >14||h is <2 or >4)
            return false;
         expected=rate*h;return true;
     }
     if(p.Count!=3||!p.TryGetValue("rate",out int r)||
        !p.TryGetValue("from",out int a0)||!p.TryGetValue("to",out int a1)||
        r is <2 or >14||a0 is <1 or >2||a1 is <3 or >4)return false;
     expected=r*(a1-a0);return true;
 }
 private static string SolveG5GraphPattern(string f,IReadOnlyDictionary<string,int> p)
 {
     if(!ValidG5GraphPattern(f,p,out int expected))
         throw new InvalidOperationException("Invalid Grade5 U6 L11 linear graph parameters");
     return expected.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5GraphPattern(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
     if(!ValidG5GraphPattern(f,p,out _)||
        string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
     string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
       .Select(ch=>ch switch
       {
         >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
         >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
         _=>ch
       }).ToArray());
     if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual))return false;
     int expected=f switch
     {
         G5GraphCompare=>(p["b"]-p["a"])*p["h"],
         G5GraphValue=>p["rate"]*p["h"],
         _=>p["rate"]*(p["to"]-p["from"])
     };
     return actual==expected;
 }

 private static bool IsPie(string f)=>f is PieCategory or PieRest;
 internal static Item Build(string f,Random random)
 {
   if(!Supports(f))throw new InvalidOperationException("Unsupported graph family");
   if(IsG5GraphPattern(f))return BuildG5GraphPattern(f,random);
   if(IsG5Coordinates(f))return BuildG5Coordinates(f,random);
   if(IsG5Map(f))return BuildG5Map(f,random);
   Dictionary<string,int> p;
   if(IsPie(f))
   {
      int a=random.Next(10,40),b=random.Next(10,35),c=random.Next(10,90-a-b);
      p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",0},{"e",0}};
      string data=$"في تمثيل دائري: القراءة {a}%، الرياضة {b}%، الرسم {c}%، وبقية الوقت لأنشطة أخرى.";
      string question=f==PieCategory?
       $"{data} ما النسبة المئوية للرياضة؟":
       $"{data} ما النسبة المئوية لبقية الأنشطة؟";
      return new Item(f,question,"اجمع نسب الأجزاء المعطاة، والقطاع الكامل يساوي ١٠٠٪.",
        AssessmentItemType.Numeric,p);
   }
   int a0=random.Next(12,32),a1=a0+random.Next(2,7),a2=a1+random.Next(1,7),
       a3=a2+random.Next(1,7),a4=a3+random.Next(1,7);
   p=new Dictionary<string,int>{{"a",a0},{"b",a1},{"c",a2},{"d",a3},{"e",a4}};
   string table=$"قيم مخطط من خمس نقاط متتابعة هي {a0}، {a1}، {a2}، {a3}، {a4}.";
   string prompt=f switch
   {
       PlotValue=>$"{table} ما قيمة النقطة الثالثة؟",
       PlotDifference=>$"{table} ما مقدار الزيادة من النقطة الأولى إلى الخامسة؟",
       LineMax=>$"{table} ما أعلى قيمة في التمثيل البياني الخطي؟",
       _=>$"{table} كم ارتفعت القيمة من بداية الخط إلى نهايته؟"
   };
   return new Item(f,prompt,"اقرأ القيم من التدريج على محور البيانات؛ ولإيجاد التغير اطرح القيمة الأولى من الأخيرة.",
     AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int ans)
 {
   ans=0;
   if(!Supports(f)||p.Count!=5||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
      !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d)||!p.TryGetValue("e",out int e))return false;
   if(IsPie(f))
   {
       if(a is <1 or >80||b is <1 or >80||c is <1 or >80||a+b+c>100||d!=0||e!=0)return false;
       ans=f==PieCategory?b:100-a-b-c;return true;
   }
   if(a<0||b<a||c<b||d<c||e<d||e>150)return false;
   ans=f switch
   {
       PlotValue=>c,PlotDifference or LineIncrease=>e-a,_=>e
   };
   return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(IsG5GraphPattern(f))return SolveG5GraphPattern(f,p);
   if(IsG5Coordinates(f))return SolveG5Coordinates(f,p);
   if(IsG5Map(f))return SolveG5Map(f,p);
   if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid graph data");
   return result.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(IsG5GraphPattern(f))return VerifyG5GraphPattern(f,p,answer);
   if(IsG5Coordinates(f))return VerifyG5Coordinates(f,p,answer);
   if(IsG5Map(f))return VerifyG5Map(f,p,answer);
   if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(c=>c switch
   {
      >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
      >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
   }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
   return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
