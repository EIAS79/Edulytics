from pathlib import Path
import json,math,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json";p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==28 and p["FirstIncompleteLessonOrdinal"]==29
Q=[]
def add(question,key,skill):
 n=len(Q)+1
 assert question and key and skill
 Q.append({"OriginalSectionExercise":n,"IndependentStudentQuestion":question,"TeacherMathematicalAnswer":key,
 "SourceSkillReviewed":skill,"PublisherOriginalTextCopied":False,"Marking":"TEACHER_MANUAL_GEOMETRY"})
for q,a,s in [
("Describe a standard-position angle and its labeled rays.","Vertex at origin; initial side on positive x-axis; terminal side reached by rotation.","Source #1 standard position sketch labels vertex and initial/terminal rays."),
("Explain why one angle has infinitely many coterminal measures.","θ+360°k or θ+2πk, k any integer.","Source #2 infinitely many full-turn rotations."),
("Explain positive versus negative orientation.","Counterclockwise positive, clockwise negative.","Source #3 direction of rotation."),
("Explain one radian and how it relates to degrees.","One radian subtends arc length equal to circle radius; 180°=π radians.","Source #4 radian arc/radius relation."),
("Compare angular and linear speed on a circle.","ω=θ/t in rad per time; v=s/t=rω in distance per time.","Source #5 angle rate vs tangent/arc travel speed.")]:add(q,a,s)
# 6–21: independent standard-angle sketches; target source signs and quadrants
for i,(deg,src)in enumerate(zip(
[45,315,-60,150,-135,120,330,210,90,-36,405,-150,-300,420,-45,-240],
["30°","300°","−80°","135°","−150°","2π/3","7π/4","5π/6","π/2","−π/10","415°","−120°","−315°","22π/3","−π/6","−4π/3"]),6):
 normalized=deg%360; quadrant=("positive x-axis"if normalized==0 else
 "positive y-axis"if normalized==90 else "negative x-axis"if normalized==180 else
 "negative y-axis"if normalized==270 else "quadrant I"if normalized<90 else
 "quadrant II"if normalized<180 else "quadrant III"if normalized<270 else "quadrant IV")
 add(f"Sketch standard-position angle {deg}°, indicating rotation and terminal quadrant.",
 f"Rotate {'CCW'if deg>=0 else 'CW'} through {abs(deg)}°; terminal {normalized}° ({quadrant}).",
 f"Source #{i} angle {src}, need directed terminal ray in standard position.")
assert len(Q)==21
# Original source figure25/26 skills and independent scale
add("Find arc length for radius5 cm and central120°.",f"s=5(2π/3)=10π/3≈{10*math.pi/3:.2f} cm.","Source #22 uses source Fig25 radius3in and central140°, s=3(7π/9).")
add("Find sector area for radius5 cm and central120°.",f"A=(1/2)·25·(2π/3)=25π/3≈{25*math.pi/3:.2f} cm².","Source #23 source Fig25 area=(1/2)·3²·(7π/9).")
add("Find arc length for radius4 cm and central3π/5.",f"s=4(3π/5)=12π/5≈{12*math.pi/5:.2f} cm.","Source #24 source Fig26 r4.5cm angle2π/5.")
add("Find sector area for radius4 cm and central3π/5.",f"A=(1/2)·16·(3π/5)=24π/5≈{24*math.pi/5:.2f} cm².","Source #25 source Fig26 sector for r4.5cm theta2π/5.")
# Radian to degree source26-32
angles=[(2,3),(1,6),(-3,4),(5,6),(-8,3),(-7,12),(7,6)]
sources=["3π/4","π/9","−5π/4","π/3","−7π/3","−5π/12","11π/6"]
for i,((numerator,denom),src)in enumerate(zip(angles,sources),26):
 deg=180*numerator/denom
 add(f"Convert {numerator}π/{denom} radians to degrees.",f"{deg:g}° (multiply radians by180/π).",f"Source #{i} convert {src} radians to degrees.")
# degrees to radians 33-39
degrees=[60,135,-450,-75,270,-225,240]
sources=["90","100","−540","−120","180","−315","150"]
from fractions import Fraction
for i,(deg,src)in enumerate(zip(degrees,sources),33):
 f=Fraction(deg,180)
 val=(f"{f.numerator}π/{f.denominator}"if f.denominator!=1 else f"{f.numerator}π")
 add(f"Convert {deg}° to exact radians.",val+ " radians.",f"Source #{i} convert {src}° to radians.")
# Arc lengths 40-45 independent using r, theta degrees or pi fractions
spec=[(8,math.pi/3,"cm","r12in, θπ/4"),(6,math.pi/6,"mi","r5.02mi, θπ/3"),
(7,2*math.pi/3,"m","diameter14m θ5π/6"),(12,math.pi/4,"cm","r10cm θ50°"),
(6,7*math.pi/6,"in","r5in θ220°"),(10,math.pi/3,"m","diameter12m θ63°")]
for i,(rad,theta,unit,source)in enumerate(spec,40):
 add(f"Determine circular arc length with radius {rad} {unit} and central angle {theta/math.pi:g}π radians.",
 f"s=rθ≈{rad*theta:.2f} {unit}; angle measured in radians.",
 f"Source #{i} original {source}, solve s=rθ with correct radius and units.")
assert len(Q)==45
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u05-l01-firstparty-teacher-led.json"
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L01",
"FirstPartyIndependentStudentMathQuestions":Q,"SourceExerciseSkillsReviewed":45,"StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/teacher_rel,teacher)
print("U05L01_FIRST45_MATH_TASKS_PREPARED")
