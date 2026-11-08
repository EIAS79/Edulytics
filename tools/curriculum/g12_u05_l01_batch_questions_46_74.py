from pathlib import Path
import json,math,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
teacher_path=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u05-l01-firstparty-teacher-led.json"
t=json.loads(teacher_path.read_text(encoding="utf8"));Q=t["FirstPartyIndependentStudentMathQuestions"]
assert len(Q)==45
def add(q,answer,source):
 n=len(Q)+1
 assert q and answer and source
 Q.append({"OriginalSectionExercise":n,"IndependentStudentQuestion":q,"TeacherMathematicalAnswer":answer,
 "SourceSkillReviewed":source,"PublisherOriginalTextCopied":False,"Marking":"TEACHER_MANUAL_GEOMETRY"})
# 46-49 sector area r^2*theta/2
for i,(r,deg,unit,src) in enumerate([
(8,60,"cm","Source46 sector radius6cm 45°"),
(12,30,"cm","Source47 sector radius20cm 30°"),
(4,90,"ft","Source48 sector diameter10ft angleπ/2"),
(.9,180,"in","Source49 sector radius0.7in angleπ")],46):
 area=.5*r*r*math.radians(deg)
 add(f"Find area of circular sector, radius {r} {unit} and central angle {deg}°.",
 f"A=(1/2)r²θ≈{area:.4f} {unit}², θ in radians.",
 src+" and original requested four decimal places.")
# 50-53 degrees normalization
for i,(deg,src)in enumerate(zip([-80,-290,850,1130],[-40,-110,700,1400]),50):
 add(f"Find an angle in [0°,360°) coterminal with {deg}°.",
 f"{deg%360}°; add/subtract integer multiples of360°.",
 f"Source #{i} original {src}°, coterminal normalization.")
# 54-57 radians normalization
from fractions import Fraction
for i,(f,src)in enumerate(zip([Fraction(-2,9),Fraction(11,3),Fraction(17,6),Fraction(47,9)],
["−π/9","10π/3","13π/6","44π/9"]),54):
 norm=f%2
 text=(f"{norm.numerator}π/{norm.denominator}"if norm.denominator!=1 else f"{norm.numerator}π")
 add(f"Find coterminal radian angle in [0,2π) for {f.numerator}π/{f.denominator}.",
 f"{text} radians; reduce angle by whole 2π rotations.",
 f"Source #{i} original {src}, normalize mod 2π.")
# linear angular speed conversions
r_in=14
mph=20
v_in_min=mph*63360/60
omega=v_in_min/r_in
rpm=omega/(2*math.pi)
add(f"A bicycle wheel has diameter {r_in*2}in, moving {mph} mph. Find ω in rad/min and RPM.",
 f"v={v_in_min:.3f} in/min; ω=v/r≈{omega:.3f}rad/min; RPM≈{rpm:.3f}.",
 "Source #58 truck wheel diameter32in, truck speed60mph; convert circumference units and angular speed.")
r_in=10; mph=12;v_in_min=mph*63360/60;omega=v_in_min/r_in
add(f"A small wheel diameter20in travels {mph}mph. Find ω and RPM.",
 f"v={v_in_min:.2f}in/min; ω={omega:.3f}rad/min; RPM={omega/(2*math.pi):.3f}.",
 "Source #59 bicycle wheel diameter24in, speed15mph; angular speed and revolutions/min.")
r=6; degrees_per_s=30;omega=math.radians(degrees_per_s)
add(f"A wheel has radius {r}in rotating {degrees_per_s}°/s. Find linear in/s, RPM and rad/s.",
 f"ω=π/6rad/s≈{omega:.5f}, v=rω=πin/s≈{r*omega:.5f}, RPM={degrees_per_s*60/360:.3f}.",
 "Source #60 wheel radius8in rotates15°/s; convert into linear speed, rpm, rad/s.")
r=20;omega=0.6
add(f"A wheel radius20in spins at {omega}rad/s. Find linear speed, RPM and deg/s.",
 f"v=rω=12in/s; RPM=ω60/(2π)≈{omega*60/(2*math.pi):.4f}; angular deg/s≈{math.degrees(omega):.4f}.",
 "Source #61 wheel radius14in angular rate0.5rad/s, find linear speed, RPM, deg/s.")
for i,(diam,rpm,src)in enumerate([(100,300,"Source #62 CD 120mm at200RPM"),(130,3600,"Source #63 CD 120mm at4800RPM")],62):
 v=math.pi*diam*rpm
 add(f"A disc has diameter{diam}mm and spins {rpm}rpm. Find rim linear speed in m/s.",
 f"v=π·d·rpm/60000≈{v/60000:.5f}m/s.",
 src+"; circumference times rotations with unit conversion.")
R=6370; v=2*math.pi*R/24
add("Assume Earth equatorial radius6370km and24h rotation. Find surface linear speed and angular speed.",
 f"ω=2π/24=π/12rad/h; v=2π(6370)/24≈{v:.2f}km/h.",
 "Source #64 Earth radius3960mi, equator, Earth rotation24h.")
for n,minutes in [(65,6),(66,8)]:
 deg=minutes/60;theta=math.radians(deg)
 add(f"At Earth radius6370km, find arc length corresponding to {minutes} arcminutes of latitude.",
 f"θ=({minutes}/60)°=π·{minutes}/10800 rad, s≈{6370*theta:.3f}km.",
 f"Source #{n} Earth radius3960mi; {5 if n==65 else 7} arcminutes.")
add("A clock minute hand sweeps through 35 minutes. Find directed angle CCW magnitude in degrees.",
 "35/60·360=210°, or clockwise rotation −210° depending on convention.",
 "Source #67 minute hand moves through20min =120° clockwise.")
add("Two cities lie at same longitude, latitudes12°N and 33°N, Earth radius6370km. Find north-south arc distance.",
 f"Difference21°; s=6370·21π/180≈{6370*math.radians(21):.2f}km.",
 "Source #68 latitude9°N and30°N radius3960mi, same longitude 21° difference.")
for n,lat in [(69,35),(70,65)]:
 speed=2*math.pi*6370*math.cos(math.radians(lat))/24
 add(f"At latitude{lat}°N on a sphere radius6370km rotating in24h, find linear eastward speed.",
 f"Parallel radius6370cos({lat}°), v=2πr/24≈{speed:.2f}km/h.",
 f"Source #{n} latitude{40 if n==69 else 75}°N, radius3960mi; use cos latitude.")
R=385000;days=27;vel=2*math.pi*R/(days*24)
add(f"Assume circular moon orbit radius{R}km and period{days}days. Find orbital speed km/h.",
 f"v=2πr/(24·days)≈{vel:.2f}km/h.",
 "Source #71 moon orbit distance239000mi, period28days, speed in mi/h.")
diam=26; rpm=150; mph=math.pi*diam*rpm*60/63360
add(f"A bicycle has diameter{diam}in wheels rotating {rpm}RPM. Find speed mph.",
 f"v=πd×rpm×60/63360≈{mph:.3f}mph.",
 "Source #72 bicycle diameter28in,180RPM, find speed.")
miles=4;revolutions=3520;rad=miles*63360/(revolutions*2*math.pi)
add(f"A vehicle goes{miles}miles in{revolutions} wheel revolutions. Find tire radius inches.",
 f"r=(miles·63360)/(revolutions·2π)≈{rad:.3f}in.",
 "Source #73 car3mi,2640 revolutions, find radius in inches.")
miles=5;diam=30;revs=miles*63360/(math.pi*diam)
add(f"A wheel diameter{diam}in rolls {miles}mi. Find approximate revolutions.",
 f"Revolutions=distance/(πd)={miles}·63360/(30π)≈{revs:.1f}.",
 "Source #74 tractor wheel24in diameter rolls4miles, find revolutions.")
assert len(Q)==74 and [x["OriginalSectionExercise"]for x in Q]==list(range(1,75))
assert math.isclose(25*math.pi/3,26.1799387799,rel_tol=1e-10)
assert math.isclose(60*63360/60,63360)
t["FirstPartyIndependentStudentMathQuestions"]=Q
t["OriginalExercisesReviewed"]=74
teacher_path.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U05L01_ALL74_INDEPENDENT_SOURCE_SKILL_QUESTION_KEYS_PREPARED")
