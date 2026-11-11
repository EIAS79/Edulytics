import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
p=r/'src/Edulytics.Services/Mathematics/UaeGrade4Unit1ComparisonQuestions.cs'
s=p.read_text(encoding='utf-8-sig')
a='    internal const string Smaller="uae.g4.u01.l03.select_smaller";'
assert s.count(a)==1
s=s.replace(a,a+'''
    internal const string OrderAsc="uae.g4.u01.l04.order_three_ascending";
    internal const string OrderDesc="uae.g4.u01.l04.order_three_descending";
    internal const string Middle="uae.g4.u01.l04.middle_of_three";''')
a='family is Relation or Greater or Smaller;'
assert s.count(a)==1
s=s.replace(a,'family is Relation or Greater or Smaller or OrderAsc or OrderDesc or Middle;')
a='        int a=rng.Next(10_000,10_000_000),b=rng.Next(10_000,10_000_000);'
assert s.count(a)==1
s=s.replace(a,'''        if (family is OrderAsc or OrderDesc or Middle)
        {
            int x=rng.Next(10_000,10_000_000);
            int y=rng.Next(10_000,10_000_000);
            int z=rng.Next(10_000,10_000_000);
            while(x==y) y=rng.Next(10_000,10_000_000); while(y==z || x==z) z=rng.Next(10_000,10_000_000);
            var p3=new Dictionary<string,int>{{"a",x},{"b",y},{"c",z}};
            string prompt3=family switch
            {
                OrderAsc=>$"رتب الأعداد تصاعديًا، واكتبها مفصولة بفاصلة منقوطة: {x}، {y}، {z}.",
                OrderDesc=>$"رتب الأعداد تنازليًا، واكتبها مفصولة بفاصلة منقوطة: {x}، {y}، {z}.",
                _=>$"ما العدد الأوسط بعد ترتيب {x} و{y} و{z}؟"
            };
            return new Item(family,prompt3,
                "قارن عدد الخانات ثم المنازل من اليسار إلى اليمين ورتب القيم.",
                family==Middle?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p3);
        }
'''+a)
a='    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)\n    {'
assert s.count(a)==1
s=s.replace(a,'''    private static bool ValidThree(IReadOnlyDictionary<string,int> p,out int[] values)
    {
        values=[];
        if(p.Count!=3 || !p.TryGetValue("a",out int a) ||
           !p.TryGetValue("b",out int b) || !p.TryGetValue("c",out int c) ||
           new[]{a,b,c}.Any(x=>x<10_000||x>9_999_999) ||
           a==b || b==c || a==c) return false;
        values=[a,b,c]; Array.Sort(values); return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family is OrderAsc or OrderDesc or Middle)
        {
            if(!ValidThree(p,out var vals))throw new InvalidOperationException("Invalid G4 ordering");
            if(family==OrderDesc)Array.Reverse(vals);
            return family==Middle?vals[1].ToString(CultureInfo.InvariantCulture)
                :string.Join(";",vals);
        }''')
a='    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)\n    {'
assert s.count(a)==1
s=s.replace(a,'''    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family is OrderAsc or OrderDesc or Middle)
        {
            if(string.IsNullOrWhiteSpace(answer)||!ValidThree(p,out int[] vals))return false;
            if(family==OrderDesc)Array.Reverse(vals);
            string normalized=new(answer.Trim().Where(c=>c!='٬'&&c!=','&&c!=' '&&c!='\\u00a0')
                .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                          c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
            if(family==Middle)return normalized==vals[1].ToString(CultureInfo.InvariantCulture);
            return normalized==string.Join(";",vals);
        }''')
p.write_text(s,encoding='utf-8')
path=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=path.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
s=s.replace(marker,'''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:01:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:01:04",
            "number.place_value.order_to_millions",
            "ORDER_THREE_WHOLE_NUMBERS_ASC_DESC_MIDDLE",
            ["uae.g4.u01.l04.order_three_ascending",
             "uae.g4.u01.l04.order_three_descending",
             "uae.g4.u01.l04.middle_of_three"],
            "UAE_G4_STUDENT_04_01_PRINTED_PP29_36_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
'''+marker)
path.write_text(s,encoding='utf-8')
path=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=json.loads(path.read_text(encoding='utf-8-sig'))
for name,ans in [('order_three_ascending','short_text'),('order_three_descending','short_text'),('middle_of_three','exact_integer')]:
    ident='uae.g4.u01.l04.'+name
    assert not any(e.get('id')==ident for e in j['families'])
    t=next(e for e in j['families'] if e['id']=='uae.g4.u01.l03.select_greater')
    e=dict(t)
    e.update({'id':ident,'skillId':'number.place_value.order_to_millions','answerType':ans,'verificationPolicy':'g4-u01-l04-student-arabic-stage-d-not-run','representations':['number_line','symbolic'],'status':'ShadowVerified','productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(e)
path.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS: selected 3 number order families routed through reused comparison engine')
