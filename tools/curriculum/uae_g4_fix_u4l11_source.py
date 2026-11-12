import pathlib,json
r=pathlib.Path(__file__).resolve().parents[2]
p=r/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
j=json.loads(p.read_text('utf-8-sig'))
l=next(x for x in j['Lessons'] if x['LessonCode']=='PED:UAE-MOE-MATH:L4:COMMON:04:11')
l['SourceLocator']='04-01.pdf U04 L11 printed pp261-266 PDF284-289'
l['TitleSourceReference']=l['SourceLocator']
p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
p=r/'tools/curriculum/uae_g4_close_u4l11.py';s=p.read_text('utf-8')
s=s.replace('[261,262,263,264]','[261,262,263,264,265,266]').replace('[284,285,286,287]','[284,285,286,287,288,289]')
s=s.replace('printed 261-264','printed 261-266')
s=s.replace("'NextSourceLesson':'Check U04 L12 in student book'","'NextSourceLesson':'Unit 5 first numbered lesson after unit 4 review pp267-270'")
s=s.replace("'Note':'34 student-book A-B-C lessons closed; next U04 L12 after source confirmation; Stage D pending.'","'Note':'34 source-matched A-B-C lessons closed; Unit 4 has 11 numbered lessons; next Unit 5 after review. Stage D pending.'")
s=s.replace("print('PASS L34 closed; next U04 L12 after source verification')","print('PASS L34; Unit 4 numbered lessons finished')")
s=s.replace("'LastClosedLessonOrdinal':34,","'LastClosedLessonOrdinal':34,'ClosedUnitCount':4,")
p.write_text(s,encoding='utf-8')
print('PASS corrected source range and next unit')
