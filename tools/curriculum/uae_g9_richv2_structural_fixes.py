#!/usr/bin/env python3
"""Deterministic targeted Arabic RichV2 *structural* repairs for UAE G9 General.

Only 25 explicitly audited sidecars are eligible. The patch keeps existing
worked examples, answers, checks, concepts and drawings unchanged. Additional
worked examples are original mathematical examples written for Edulytics.
This does NOT certify publisher literal parity or auto-grader readiness.
Dry-run by default; apply only with --apply and review Git diffs before commit.
"""
from __future__ import annotations
import argparse
import copy
import json
from pathlib import Path
import re

ROOT=Path(__file__).resolve().parents[2]
PACKS=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/RichV2"
PREFIX="PED:UAE-MOE-MATH:L9:GENERAL:"

# Zero-based worked-example index: add the precise mathematical setup before
# the existing correct solution. A single one-step cube-root example also
# receives its own independent mathematical verification as a third step.
SETUP={
"u02-l07":{1:"نحدد أن القيمة الأصلية ١٧٢٢، وأن القيمة الجديدة ١٠٠٩ أقل منها، إذن الاتجاه نقصان.",
           2:"نحدّد السعر قبل الخصم ١٤٨٩ درهمًا ونحوّل نسبة ٤٠٪ إلى العدد العشري ٠٫٤."},
"u03-l01":{3:"نلاحظ أن ص ثابتة عند القيمة ٣ مهما تغيّرت السين، فيكون الميل صفرًا."},
"u08-l02":{0:"نطبّق قاعدة قسمة قوتين لهما الأساس غير الصفري نفسه بطرح أس المقام من أس البسط.",
           1:"بما أن س≠٠، نستخدم س^م÷س^ن=س^(م−ن) ثم نعيد كتابة الأس السالب.",
           2:"قاعدة قوة خارج القسمة تقتضي تربيع البسط والمقام، مع بقاء ص≠٠."},
"u08-l03":{0:"الأس ١/٣ يعني الجذر التكعيبي، لذا نبحث عن عدد مكعبه يساوي ٦٤.",
           1:"الأس ٢/٣ يعني حساب الجذر التكعيبي للعدد الموجب ثم تربيع الناتج."},
"u08-l05":{1:"لأن ١٨ و٢ غير سالبين، نستخدم خاصية ضرب الجذور √أ×√ب=√(أب).",
           2:"لتبسيط المقام نستخدم عاملًا مساويًا للواحد، وهو √٥/√٥ لأن √٥≠٠."},
"u08-l06":{3:"هذا حاصل ضرب مجموع حدين في فرقهما؛ نطبق الهوية (أ+ب)(أ−ب)=أ²−ب²."},
"u09-l02":{2:"نعرّف أ\\ب بأنه عناصر أ غير الموجودة في ب، وب\\أ بالعكس.",
           3:"تُحسب متممة أ بالنسبة إلى المجموعة الشاملة ش المعطاة فقط."},
"u09-l03":{1:"نحدد المجموعة الشاملة ش أولًا، ثم نبحث عن عناصرها التي ليست في أ."},
"u09-l04":{1:"نحسب حجم اتحاد النشاطين بطرح المشترك مرة واحدة، ثم نطرحه من إجمالي الطلاب."},
"u10-l01":{1:"لإيجاد الحد الخامس نتحقق أولًا من ثبات الفرق بين الحدود المتتالية."},
"u10-l02":{1:"النفي المنطقي ¬p يُبدّل قيمة الصدق للعبارة p نفسها ولا يغيّر عبارة أخرى."},
"u11-l03":{0:"لأن ب تقع بين أ وج، نستعمل مسلمة جمع أطوال القطع المستقيمة.",
           5:"نستبعد الأصفار السابقة لأول رقم غير صفري، ونعد الأصفار المحصورة والنهائية الموثّقة بالدقة."},
"u11-l04":{1:"الزاويتان المتجاورتان اللتان تشكلان خطًا مستقيمًا مجموعهما ١٨٠°."},
"u13-l02":{0:"مجموع قياسات زوايا أي مثلث في المستوى الإقليدي يساوي ١٨٠°.",
           1:"حسب نظرية الزاوية الخارجية، قياسها يساوي مجموع قياسي الزاويتين الداخليتين البعيدتين."},
"u13-l06":{0:"في المثلث متساوي الساقين تتساوى زاويتا القاعدة، ومجموع الزوايا الثلاث ١٨٠°."},
"u13-l07":{0:"قاعدة الانتقال أربع وحدات يمينًا وواحدة إلى أسفل هي (س،ص)↦(س+٤،ص−١).",
           1:"قاعدة الانعكاس حول محور السينات هي (س،ص)↦(س،−ص).",
           2:"قاعدة الدوران ٩٠° عكس عقارب الساعة حول الأصل هي (س،ص)↦(−ص،س)."},
"u13-l09":{1:"نستخدم مساحة المثلث = نصف حاصل ضرب القاعدة في ارتفاعها العمودي."}
}
EXTRA_LAST_STEP={
"u08-l03":{0:"نتحقق بالتكعيب: ٤×٤×٤=٦٤، لذلك ٦٤^(١/٣)=٤."}
}
METHODS={
"u02-l07":{
0:"قسمة الزيادة على القيمة الأصلية الموجبة وتحويل الناتج إلى نسبة مئوية.",
1:"إيجاد مقدار النقصان ثم قسمته على القيمة الأصلية.",
2:"ضرب السعر الأصلي في نسبة الخصم ثم طرح قيمة الخصم.",
3:"حساب الضريبة من السعر الأصلي، ثم تقريبها إلى أصغر وحدة نقدية مطلوبة وجمعها."
}}
NEW_EXAMPLE={
"u12-l01":{
"Title":"تمييز مستقيمين متوازيين في المستوى",
"Question":"في مستوى واحد، مستقيمان لا يلتقيان مهما امتدا. ما العلاقة بينهما؟",
"Method":"تطبيق تعريف التوازي مع شرط الاشتراك في المستوى.",
"Steps":[
"نحدّد أن المستقيمين يقعان في مستوى واحد وفق المعطيات.",
"نلاحظ أنهما لا يلتقيان عند امتدادهما في الاتجاهين.",
"وفق تعريف التوازي، نستنتج أنهما مستقيمان متوازيان."
],
"Answer":"مستقيمان متوازيان.",
"Check":"لو كانا متخالفين لما وقعا في مستوى واحد؛ لكن المسألة تنص على اشتراكهما في المستوى."
},
"u12-l05":{
"Title":"إثبات التوازي من زاويتين متحالفتين داخليًا",
"Question":"شكّل قاطع زاويتين داخليتين على جهة واحدة قياساهما ١١٥° و٦٥°. هل المستقيمان متوازيان؟",
"Method":"تطبيق عكس نظرية الزاويتين المتحالفتين داخليًا.",
"Steps":[
"نحدّد أن الزاويتين داخليتان وعلى الجهة نفسها من القاطع.",
"نحسب المجموع: ١١٥°+٦٥°=١٨٠°.",
"لأنهما متكاملتان، ينص عكس النظرية على أن المستقيمين متوازيان."
],
"Answer":"نعم، المستقيمان متوازيان.",
"Check":"لا يكفي تكامل زاويتين مجهولتي الموضع؛ العلاقة هنا مستندة إلى وضعهما الداخلي الصحيح."
},
"u13-l01":{
"Title":"تصنيف مثلث منفرج ومختلف الأضلاع",
"Question":"زوايا مثلث هي ١٠٠° و٥٠° و٣٠°. صنّفه بحسب زواياه وأضلاعه.",
"Method":"التحقق من مجموع الزوايا ثم الاستدلال على الأضلاع المقابلة.",
"Steps":[
"نجمع ١٠٠°+٥٠°+٣٠°=١٨٠°، وهو مجموع زوايا مثلث.",
"لأن إحدى الزوايا قياسها ١٠٠° وهي أكبر من ٩٠°، فالمثلث منفرج الزاوية.",
"الزوايا الثلاث مختلفة، فتختلف أطوال الأضلاع المقابلة لها؛ إذًا هو مختلف الأضلاع."
],
"Answer":"مثلث منفرج الزاوية ومختلف الأضلاع.",
"Check":"لا توجد زاويتان متساويتان، ومجموع الزوايا يساوي ١٨٠°."
},
"u13-l03":{
"Title":"التشابه لا يكفي لإثبات التطابق",
"Question":"مثلثان متساويا الأضلاع؛ طول ضلع الأول ٣ سم وطول ضلع الثاني ٥ سم. هل هما متطابقان؟",
"Method":"مقارنة أطوال الأضلاع المناظرة بعد ملاحظة تساوي الزوايا.",
"Steps":[
"في كل مثلث متساوي الأضلاع زواياه الثلاث ٦٠°، ولذلك زاوياتهما متساوية.",
"أطوال الأضلاع المناظرة مختلفة: ٣ سم في الأول و٥ سم في الثاني.",
"يشترط التطابق تساوي الأطوال المناظرة، فلا ينتج من تساوي الزوايا وحده."
],
"Answer":"لا، المثلثان متشابهان ولكنهما غير متطابقين.",
"Check":"نسبة ٣ إلى ٥ ليست ١؛ ومن ثم لا تتساوى الأجزاء المناظرة."
},
"u13-l04":{
"Title":"تمييز SSA غير الكافية من SAS",
"Question":"في مثلثين تساوى ضلعان متناظران ٦ و٨ سم، وزاوية مقدارها ٤٠° غير محصورة بينهما. هل تكفي المعطيات لإثبات التطابق بـSAS؟",
"Method":"تحديد موضع الزاوية بالنسبة إلى الضلعين قبل اختيار مسلمة التطابق.",
"Steps":[
"نراجع المعطيات: ضلعان متناظران متساويان في المثلثين.",
"الزاوية المعطاة ليست محصورة بين الضلعين، فلا تتحقق شروط SAS.",
"هذا نمط SSA، وهو لا يثبت التطابق بصورة عامة؛ نحتاج معطيات أخرى."
],
"Answer":"لا؛ لا يجوز الاستدلال بمسلمة SAS من زاوية غير محصورة.",
"Check":"تتطلب SAS تساوي الزاوية الواقعة بين الضلعين المذكورين."
},
"u13-l05":{
"Title":"تساوي ثلاث زوايا لا يثبت التطابق",
"Question":"مثلثان قائمان زواياهما ٣٠° و٦٠° و٩٠°، وطول الوتر في الأول ٥ سم وفي الثاني ١٠ سم. هل هما متطابقان؟",
"Method":"التمييز بين شرط التشابه AAA واشتراط الأطوال المناظرة للتطابق.",
"Steps":[
"الزوايا الثلاث مناظرة ومتساوية، فيثبت التشابه بقاعدة AAA.",
"طول الوتر في الأول ٥ سم وفي الثاني ١٠ سم، فلا تتساوى الأضلاع المناظرة.",
"بما أن التطابق يتطلب تساوي الأطوال أيضًا، فالمثلثان غير متطابقين."
],
"Answer":"لا؛ المثلثان متشابهان فقط وليسا متطابقين.",
"Check":"التشابه يسمح بمعامل تكبير ٢، أما التطابق فيتطلب معاملًا يساوي ١."
},
"u13-l06":{
"Title":"استخراج زاوية الرأس من زاويتي القاعدة",
"Question":"مثلث متساوي الساقين قياس كل من زاويتي قاعدته ٥٥°. ما قياس زاوية الرأس؟",
"Method":"استخدام تساوي زاويتي القاعدة ومجموع زوايا المثلث.",
"Steps":[
"نعرف أن زاويتي القاعدة في المثلث متساوي الساقين متساويتان، وكل منهما ٥٥°.",
"مجموع زاويتي القاعدة هو ٥٥°+٥٥°=١١٠°.",
"نطرح من ١٨٠° فنحصل على زاوية الرأس ٧٠°."
],
"Answer":"زاوية الرأس ٧٠°.",
"Check":"٧٠°+٥٥°+٥٥°=١٨٠°."
},
"u13-l08":{
"Title":"إثبات أن مثلثًا قائم باستعمال الإحداثيات",
"Question":"أ(٠،٠)، ب(٦،٠)، ج(٠،٤). أثبت أن المثلث أبج قائم الزاوية عند أ.",
"Method":"التحقق من تعامد ضلعين عبر موضعيهما على محوري الإحداثيات.",
"Steps":[
"القطعة أب تصل (٠،٠) بـ(٦،٠)، فتقع على محور السينات الأفقي.",
"القطعة أج تصل (٠،٠) بـ(٠،٤)، فتقع على محور الصادات الرأسي.",
"المحوران متعامدان؛ إذًا ∠بأج=٩٠° ويكون المثلث قائمًا عند أ."
],
"Answer":"المثلث أبج قائم الزاوية عند أ.",
"Check":"أب²+أج²=٣٦+١٦=٥٢، وهو يساوي بج²؛ فتتفق النتيجة مع عكس فيثاغورس."
}}
ADDITIONAL_CONCEPT={
"u06-l04":{
"Title":"تحويل معاملي متغير إلى عددين متعاكسين",
"Definition":"نضرب طرفي إحدى معادلتي النظام في عدد غير صفري حتى يصبح معاملا أحد المتغيرين متعاكسين.",
"Rule":"اضرب جميع حدود المعادلة والثابت في العدد نفسه، ثم اجمع المعادلتين لإلغاء المتغير.",
"Example":"س+٢ص=٧، ٢س−ص=٤؛ نضرب المعادلة الثانية في ٢ ونجمع: ٥س=١٥، إذًا س=٣ وص=٢."
}}
ADDITIONAL_MISTAKE={
"u13-l06":{
"Mistake":"الظن بأن تساوي زاويتي القاعدة يجعل زاوية الرأس مساوية لهما دائمًا.",
"WhyWrong":"تساوي زاويتي القاعدة يثبت تساوي الساقين، لكنه لا يجعل المثلث متساوي الأضلاع إلا إذا كانت زواياه الثلاث ٦٠°.",
"Correction":"احسب زاوية الرأس من ١٨٠° ناقص مجموع زاويتي القاعدة، ثم صنّف المثلث من القياسات الفعلية."
}}
EXPECTED=set(SETUP)|set(NEW_EXAMPLE)|set(ADDITIONAL_CONCEPT)|set(ADDITIONAL_MISTAKE)
assert len(EXPECTED)==25,f"Expected exactly 25 files, got {len(EXPECTED)}"

def verify(doc, name):
    assert doc["SchemaVersion"]==1 and doc["PackCode"]=="UAE-MOE-MATH",name
    assert len(doc["Lessons"])==1,name
    l=doc["Lessons"][0]
    assert l["LessonCode"].startswith(PREFIX) and l["CultureCode"]=="ar",name
    assert len(l["ExplanationParagraphs"])>=2 and len(l["KeyConcepts"])>=3,name
    assert len(l["WorkedExamples"])>=3 and len(l["CommonMistakes"])>=2,name
    assert len(l["SummaryPoints"])>=3,name
    for ex in l["WorkedExamples"]:
        assert len(ex["Steps"])>=3,(name,ex["Title"])
        assert all(ex.get(k) for k in ("Title","Question","Method","Answer","Check")),(name,ex["Title"])

def main(apply):
    paths={re.search(r"(u\d\d-l\d\d)",f.name).group(1):f
           for f in sorted(PACKS.glob("uae-g9-general-*.rich-lesson-v2.json"))}
    assert len(paths)==82 and EXPECTED<=set(paths),"Source file set changed"
    updated=[]
    for key in sorted(EXPECTED):
        path=paths[key]
        doc=json.loads(path.read_text(encoding="utf-8"))
        original=copy.deepcopy(doc)
        lesson=doc["Lessons"][0]
        for i,method in METHODS.get(key,{}).items():
            assert not lesson["WorkedExamples"][i].get("Method"),(key,i,"Unexpected Method")
            lesson["WorkedExamples"][i]["Method"]=method
        for i,step in SETUP.get(key,{}).items():
            ex=lesson["WorkedExamples"][i]
            assert len(ex["Steps"]) in (1,2),(key,i,"Unexpected Step count")
            ex["Steps"].insert(0,step)
        for i,step in EXTRA_LAST_STEP.get(key,{}).items():
            ex=lesson["WorkedExamples"][i]
            assert len(ex["Steps"])==2,(key,i,"Unexpected extra step")
            ex["Steps"].append(step)
        if key in ADDITIONAL_CONCEPT:
            assert len(lesson["KeyConcepts"])==2,key
            lesson["KeyConcepts"].append(ADDITIONAL_CONCEPT[key])
        if key in NEW_EXAMPLE:
            assert len(lesson["WorkedExamples"])==2,key
            lesson["WorkedExamples"].append(NEW_EXAMPLE[key])
        if key in ADDITIONAL_MISTAKE:
            assert len(lesson["CommonMistakes"])==1,key
            lesson["CommonMistakes"].append(ADDITIONAL_MISTAKE[key])
        verify(doc,key)
        assert doc!=original,key
        # Status and publisher rights are untouched; this is not D_VERIFIED.
        assert doc["ContentVersion"]==original["ContentVersion"]
        updated.append((key,path,doc))
    if apply:
        for key,path,doc in updated:
            path.write_text(json.dumps(doc,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(json.dumps({"status":"STRUCTURAL_REPAIR_PREPARED",
       "applied":apply,"lessons":len(updated),
       "order":[x[0] for x in updated],
       "publisherLiteralParity":"UNKNOWN",
       "digitalPracticeContractsCertified":0},ensure_ascii=False,indent=2))

if __name__=="__main__":
    p=argparse.ArgumentParser()
    p.add_argument("--apply",action="store_true")
    args=p.parse_args()
    main(args.apply)
