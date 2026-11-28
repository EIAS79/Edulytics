"""Reconcile G10 lesson 001's original assessment supplements with the Arabic canonical fallback.
Run only against the audited pre-repair lesson state. No production release claims.
"""
import json, hashlib, pathlib, copy
root = pathlib.Path(__file__).resolve().parents[2]
g = root / 'artifacts/curriculum-delivery/UAE-MOE-MATH/G10'
cp_path = g / 'lesson-001-stage-d-checkpoint.json'
ev_path = g / 'lesson-001-stage-d-page-026-review.json'
pack_path = root / 'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json'
rich_path = root / 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l01-ar.rich-lesson-v2.json'
def read(p): return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p, x): p.write_text(json.dumps(x, ensure_ascii=False, indent=2)+'\n', encoding='utf8')
pack=read(pack_path); before=copy.deepcopy(pack); rich=read(rich_path); x=rich['Lessons'][0]
ev=read(ev_path); cp=read(cp_path)
l=next(i for i in pack['Lessons'] if i['LessonCode']==x['LessonCode'])
ar=next(t for t in l['Translations'] if t['CultureCode']=='ar')
assert len(pack['Lessons'])==148 and x['CultureCode']=='ar' and len(x['WorkedExamples'])==20
assert ev['physicalPdfPage']==26 and [row['sourceQuestionNumber'] for row in ev['rows']]==list(range(66,92))
assert l['CanonicalBodySha256']=='b379fabb9a42546097aa8a36ac59ebfb5addc07dde262b24b8b53555b070121d'
# Direct visual review: printed page 13 has Q66-69 standardized assessment, Q70-91 cumulative/skill review.
for row in ev['rows']:
    n=row['sourceQuestionNumber']
    row['sourceSection']='STANDARDIZED_ASSESSMENT' if n<=69 else ('CUMULATIVE_REVIEW' if n<=83 else 'PREREQUISITE_SKILL_REVIEW')
    if n in (68,69): row['role']='STANDARDIZED_CROSS_SKILL_ASSESSMENT'
ev['primaryItemCount']=2
ev['crossSkillStandardizedItemCount']=2
ev['cumulativeReviewItemCount']=22
# Replace a publisher-identical mathematical error scenario on PDF25 with a genuinely different original equivalent.
old=next(e for e in x['WorkedExamples'] if e['Title']=='كشف خطأ قلب الإشارات في طرح كثيرة حدود')
assert '٢س²−س' in old['Question']
old.update(Question='بسّط (٤س²+٢س−٥)−(−٢س²+٧س−٣). كتب طالب الناتج ٢س²+٩س−٨. فسّر الخطأ وأعط الناتج الصحيح.',
           Method='اضرب كل حد في المطروح في −١ قبل التجميع.',
           Steps=['نبدأ من (٤س²+٢س−٥)−(−٢س²+٧س−٣).','نوزع السالب: ٤س²+٢س−٥+٢س²−٧س+٣.','نجمع معاملات س² ثم س والثابت: ٤+٢=٦، ٢−٧=−٥، −٥+٣=−٢.'],
           Answer='الناتج الصحيح ٦س²−٥س−٢؛ يجب عكس جميع الإشارات في القوس الثاني.',
           Check='عند س=٢: الأولى ١٥ والثانية ٣، والفرق ١٢؛ والناتج ٢٤−١٠−٢=١٢.')
v=next(v for v in x['Visuals'] if v['Title']=='تحقيق إشارة كل حد في المطروح')
v['Items']=['(٤س²+٢س−٥)−(−٢س²+٧س−٣)','٤س²+٢س−٥+٢س²−٧س+٣','٦س²−٥س−٢','عند س=٢: ١٢ في التعبيرين']
for s in range(-10,11): assert (4*s*s+2*s-5)-(-2*s*s+7*s-3)==6*s*s-5*s-2
# Preserve the existing canonical text, then append the exact authored assessment groups, not source wording.
assert 'تقويم أصلي: مجموع أعداد متتالية' not in ar['WorkedExamples']
supp=x['WorkedExamples'][9:]
assert len(supp)==11
ar['Explanation']+='\n'+x['ExplanationParagraphs'][-1]
ar['WorkedExamples']+='\n\n'+ '\n\n'.join(e['Title']+'\n'+e['Question']+'\n'+'؛ '.join(e['Steps'])+'\n'+e['Answer']+'\n'+e['Check'] for e in [old]+supp)
ar['StepByStepSolutions']+='\n'+ 'يبدأ التقويم الإضافي بمسألتين في جمع الحدود؛ أما تقويم العد والمتباينات والمراجعة التراكمية فلا يُحتسب تلقائيًا في تقييم مهارة جمع كثيرات الحدود. راجع الرسوم والتعليل مع المعلم.'
ar['QuickSummary']+='\n'+ 'راجع أسئلة التقويم الإضافي والمراجعة التراكمية في الأمثلة المحلولة، مع المحافظة على فصل المهارات غير المرتبطة مباشرة بموضوع الدرس.'
l['SourceLocator']=l['SourceLocator'].replace('PDF pages 20–25 / printed pages 7–12','PDF pages 20–26 / printed pages 7–13')
assert '20–26' in l['SourceLocator']
l['AdaptationStatus']+=' Stage D repair: original source page 26 (printed 13) supplements synchronized into Arabic fallback; former historical page range superseded. Source-wide atomic parity and runtime verification remain pending.'
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(ar,sort_keys=True,ensure_ascii=False,separators=(',',':')).encode('utf8')).hexdigest()
for a,b in zip(pack['Lessons'],before['Lessons']):
    if a['LessonCode']!=l['LessonCode']: assert a==b
assert next(t for t in l['Translations'] if t['CultureCode']=='en')==next(t for t in before['Lessons'][0]['Translations'] if t['CultureCode']=='en')
ev['limitations']=[z for z in ev['limitations'] if not z.startswith('Canonical fallback content pack still references')]
ev['canonicalFallbackReconciliation']={'status':'SYNCED_ORIGINAL_ASSESSMENT_GROUPS','canonicalBodySha256':l['CanonicalBodySha256'],'digestScheme':'sha256(utf8(JSON Arabic translation, sorted keys, compact separators, literal Unicode))','oldCanonicalBodySha256':before['Lessons'][0]['CanonicalBodySha256'],'sourceLocator':l['SourceLocator']}
ev['rightsCorrection']={'sourcePhysicalPage':25,'sourceQuestionNumber':61,'originalEquivalentTitle':old['Title'],'status':'REPLACED_WITH_INDEPENDENT_ORIGINAL_NUMBERS_AND_STRUCTURE'}
cp['sourcePdfPagesDirectlyReviewedInThisStage']=list(range(20,27))
cp['sourcePrintedPagesDirectlyReviewedInThisStage']=list(range(7,14))
cp['openDefects']=[z for z in cp['openDefects'] if z!='CANONICAL_PACK_FALLBACK_RECONCILIATION_REQUIRED']
cp['fallbackReconciliation']='SYNCED_PENDING_RUNTIME'
cp['canonicalBodySha256']=l['CanonicalBodySha256']
cp['rightsStatus']='ORIGINAL_ALTERNATIVES_AUTHORED_AND_ONE_SOURCE_IDENTICAL_EXAMPLE_REPLACED_FULL_REVIEW_PENDING'
cp['status']='D_PENDING_A_C_EVIDENCE'
save(pack_path,pack); save(rich_path,rich); save(ev_path,ev); save(cp_path,cp)
print('G10_L001_FALLBACK_SYNC',l['CanonicalBodySha256'],len(supp),'groups; 147 unrelated lessons preserved')
