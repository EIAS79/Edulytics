import json,pathlib
r=pathlib.Path(r'C:\Users\khali\Edulytics-UAE-G4-ABC-20261111')
p=r/'tools/curriculum/uae_g4_u6l2_spec.json'
j=json.loads(p.read_text('utf-8'))
j['Ordinal']=42
j['SkillChecks']=['التقدير باستخدام الأعداد المتوافقة','تحديد عدد قريب سهل القسمة','تقدير ناتج قسمة عدد ثلاثي على رقم واحد','تقدير ناتج قسمة عدد رباعي على رقم واحد','التأكد من التقريب بالضرب','اختيار الأعداد المتوافقة في مسائل الأسعار','تقدير قيمة مجهولة في معادلة قسمة','تفسير معقولية الناتج التقديري']
j['CoolDown']={'prompt':'قدّر ٦٤٢÷٨ باستخدام عدد متوافق ثم تحقق بالضرب.','expected':'٦٤٠÷٨=٨٠، ٨٠×٨=٦٤٠','teacherReview':'اختيار العدد المتوافق'}
j['NextLesson']='Unit6 lesson 3 according to student book'
p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS spec closure metadata')
