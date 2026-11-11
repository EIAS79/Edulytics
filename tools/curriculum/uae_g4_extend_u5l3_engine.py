import pathlib,json,hashlib
r=pathlib.Path(__file__).resolve().parents[2]
p=r/'src/Edulytics.Services/Mathematics/UaeGrade4Unit5Lesson02EstimateProductsQuestions.cs'
s=p.read_text(encoding='utf-8-sig')
s=s.replace('internal sealed record Item(','''internal const string AreaProduct="uae.g4.u05.l03.area_two_digit_product";
    internal const string TensPartial="uae.g4.u05.l03.area_tens_partial";
    internal const string OnesPartial="uae.g4.u05.l03.area_ones_partial";
    internal sealed record Item(''')
s=s.replace('f is Estimate or Compare;', 'f is Estimate or Compare or AreaProduct or TensPartial or OnesPartial;')
s=s.replace('string prompt=f==Estimate?$"قرّب {a} و{b} إلى أقرب عشرة ثم قدّر حاصل الضرب {a} × {b}.":\n            $"قرّب {a} و{b} لأقرب عشرة ثم قارن التقدير بالناتج الدقيق: اكتب ١ إذا كان أكبر أو مساويًا، و٠ إذا كان أصغر.";','''string prompt=f switch
        {
            Estimate=>$"قرّب {a} و{b} إلى أقرب عشرة ثم قدّر حاصل الضرب {a} × {b}.",
            Compare=>$"قرّب {a} و{b} لأقرب عشرة ثم قارن التقدير بالناتج الدقيق: اكتب ١ إذا كان أكبر أو مساويًا، و٠ إذا كان أصغر.",
            AreaProduct=>$"مثّل مستطيلًا طوله {a} وعرضه {b}، ثم أوجد ناتج الضرب باستخدام خاصية التوزيع.",
            TensPartial=>$"في نموذج المساحة لـ {a} × {b}، ما حاصل ضرب {a} في عشرات {b} فقط؟",
            _=>$"في نموذج المساحة لـ {a} × {b}، ما حاصل ضرب {a} في آحاد {b} فقط؟"
        };''')
s=s.replace('expected=family==Estimate?roundedA*roundedB:(roundedA*roundedB>=a*b?1:0);','''expected=family switch
        {
            Estimate=>roundedA*roundedB,
            Compare=>roundedA*roundedB>=a*b?1:0,
            AreaProduct=>a*b,
            TensPartial=>a*(b/10*10),
            _=>a*(b%10)
        };''')
p.write_text(s,encoding='utf-8')
print('PASS extended exact engine with area product and partial factors')
