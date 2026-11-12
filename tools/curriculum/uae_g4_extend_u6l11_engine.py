import pathlib
r=pathlib.Path(__file__).resolve().parents[2];p=r/'src/Edulytics.Services/Mathematics/UaeGrade4Unit5Lesson05MultiStepQuestions.cs'
s=p.read_text('utf-8')
s=s.replace('internal const string Days="uae.g4.u05.l06.table_number_of_days";','internal const string Days="uae.g4.u05.l06.table_number_of_days";\n    internal const string Weeks="uae.g4.u06.l11.weeks_to_save_target";')
s=s.replace('f is Net or Missing or Table or Days;', 'f is Net or Missing or Table or Days or Weeks;')
s=s.replace('Table=>$"ارسم جدولًا لـ {n} أيام، في كل يوم {a} نشاطًا. كم العدد التراكمي للأنشطة في اليوم الأخير؟",','Table=>$"ارسم جدولًا لـ {n} أيام، في كل يوم {a} نشاطًا. كم العدد التراكمي للأنشطة في اليوم الأخير؟",\n            Weeks=>$"يبدأ طالب من صفر مدخرات، يحصل كل أسبوع على {a} درهمًا ويصرف {b} دراهم. بعد كم أسبوع يجمع {(a-b)*n} درهمًا؟",')
s=s.replace('result=f switch {Net=>(a-b)*n,Missing=>a+b,Table=>a*n,_=>n};','result=f switch {Net=>(a-b)*n,Missing=>a+b,Table=>a*n,Weeks=>n,_=>n};')
p.write_text(s,encoding='utf-8')
print('PASS extended existing multi-step engine with reverse savings period')
