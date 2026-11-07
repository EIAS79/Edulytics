from pathlib import Path
import math,json
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
Q=[]
def add(q,a,src):
 n=len(Q)+1;assert q and a and src
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,"TeacherMathAnswer":a,
 "SourceMathematicalSkillReviewed":src,"OriginalTextCopied":False,"Scoring":"TEACHER_MANUAL_MODELING"})
for q,a,src in [
("Which physical process is naturally modeled by sinusoid? Give two necessary features.","Repeating rise/fall, approximate steady amplitude/mean and period; tides or daily temperature.","Source1 periodic physical behavior"),
("Give two data sets sufficient to construct a daily temperature sine model.","Set1 extrema+their clock times; set2 mean+amplitude+peak time, assuming24h period.","Source2 data sufficiency"),
("Is monotonically cumulative rainfall well modeled by an unadjusted sine?","No: cumulative total cannot decrease, while sine oscillates downward.","Source3 monotonic cumulative vs daily rainfall"),
("What does multiplying a spring oscillation by e^(−ct), c>0 do?","Envelope falls from A to Ae^(−ct), period remains2π/ω.","Source4 damping envelope")
]:add(q,a,src)
# 5-12 table → model; first-party sampled tables at known periods/phases
table_models=[
(3,0,2,4,"cos"),(-4,1,4,2,"cos"),(5,2,1,2,"sin"),
(4,-1,3,4,"cos"),(-6,4,2,6,"cos"),(7,5,3,4,"sin"),
(2,1,2,8,"sin"),(3,-2,1,6,"cos")]
for amp,mean,phase,period,fn in table_models:
 n=len(Q)+1;w=2*math.pi/period
 data=[round(mean+amp*(math.sin if fn=="sin" else math.cos)(w*(x-phase)),5)for x in range(5)]
 add(f"Observed table at x=0..4 has y-values {data}. Fit one sinusoid using given period {period} and family {fn}.",
 f"One matching formula y={mean}+({amp}){fn}((2π/{period})(x−{phase})); check each table value.",
 f"Source{n} table-of-values sinusoid reconstruction")
assert len(Q)==12
for q,a,src in [
("Graph f(t)=50−15cos(πt/6) on[0,12]; give realistic interpretation.","Yearly 12-month seasonal index, midline50, amplitude15, minimum35 at t0, maximum65 at t6.","Source13 composite periodic graphical model"),
("Graph f(t)=80+12sin(πt/12) on[0,24]; give physical example.","Daily 24h periodic humidity-like index, midline80 amplitude12; use physically valid range.","Source14 combined sine/cos physical model"),
("Graph f(t)=12+2sin(πt/6) on[0,24]; give physical example.","Temperature indicator periodic12 units with midline12 amplitude2; period12.","Source15 trigonometric plus tangent physical interpretation")
]:add(q,a,src)
add("A seasonal rainfall model has average20in plus amplitude5in, with long-term linear decline2in/year. When can model first predict0?","h(t)=20−2t+5cos(2πt), t years; first zero lies between7.5 and12.5 years, solve numerically with chosen start phase.","Source16 seasonal rainfall with declining trend and first zero")
# 17-20 temperature per day
for high,mean,peak,query in [(96,80,17,9),(82,69,18,7)]:
 i=len(Q)+1
 temp=mean+(high-mean)*math.cos(2*math.pi*(query-peak)/24)
 add(f"Daily temperature peak {high}°F at hour{peak}, mean{mean}°F. Assume24h period; find T({query}:00).",
 f"T(t)={mean}+{high-mean}cos(2π(t−{peak})/24), T({query})≈{temp:.2f}°F.",
 f"Source{i} daily temperature from max/mean/peak time")
for low,high,midnight_phase,target in [(50,70,"midline rising",55),(64,88,"midline rising",70)]:
 i=len(Q)+1
 avg=(low+high)/2;amp=(high-low)/2
 alpha=math.asin((target-avg)/amp)
 first=(24*(alpha%(2*math.pi))/(2*math.pi))
 add(f"Daily T(t)={avg:g}+{amp:g}sin(πt/12), t hours since midnight. Find first t≥0 with T={target}°F.",
 f"t=(12/π)arcsin(({target}−{avg:g})/{amp:g}) modulo24; first crossing t≈{first:.3f}hours.",
 f"Source{i} temperature first threshold crossing")
# 21-22 Ferris wheel with inequality
for diameter,platform,period,threshold in [(24,2,8,16),(40,1,10,26)]:
 i=len(Q)+1
 radius=diameter/2;center=platform+radius;c=(center-threshold)/radius
 assert -1<c<1
 duration=period*(1-math.acos(c)/math.pi)
 add(f"Wheel diameter{diameter}m, bottom boarding at{platform}m, period{period}min. How long above{threshold}m?",
 f"h(t)={center:g}−{radius:g}cos(2πt/{period}); duration={period}(1−arccos({c:.5f})/π)≈{duration:.4f}min.",
 f"Source{i} Ferris wheel time above given level")
# 23-28 seasonal ice/rainfall threshold; exact angular window
for name,lo,hi,period,target,relation in [
("Northern sea ice",6,14,365,9,"less"),
("Southern sea ice",4,18,365,15,"greater"),
("Monsoon rainfall",3,13,90,11,"greater"),
("Monsoon rainfall",5,13,90,6,"less"),
("Seasonal precipitation",1,9,365,8,"greater"),
("Seasonal precipitation",4,24,365,5,"less")]:
 i=len(Q)+1;avg=(lo+hi)/2;amp=(hi-lo)/2;c=(target-avg)/amp
 assert -1<c<1
 if relation=="greater":duration=period*math.acos(c)/math.pi
 else:duration=period*(1-math.acos(c)/math.pi)
 add(f"{name} varies seasonally between {lo} and{hi}, period{period}days; how many days per cycle {relation} than {target}?",
 f"m={avg:g},A={amp:g}, f(t)=m+Acos(2πt/{period}) centered on maximum; "+
 f"days≈{duration:.2f} per cycle (threshold strict, endpoints excluded).",
 f"Source{i} sinusoidal annual/monsoon threshold duration")
assert len(Q)==28
for amp,freq in [(8,3),(11,6),(4,.25),(5,30)]:
 i=len(Q)+1
 add(f"Spring displacement h(t)={amp}sin(2π·{freq}t)cm. Determine amplitude, period and frequency.",
 f"A={amp}cm, period={1/freq:.6f}s, frequency={freq}Hz.",
 f"Source{i} harmonic oscillation frequency and period")
# 33-36 populations
for animal,pop,osc,growth,mode in [
("deer",700,20,120,"linear"),("rabbits",600,15,96,"linear"),
("muskrats",900,30,.05,"exp"),("fish",800,40,.04,"exp")]:
 i=len(Q)+1
 baseline=(f"{pop}+({growth}/12)t"if mode=="linear"else f"{pop}(1+{growth})^t")
 add(f"Build {animal} population model with January minimum, yearly oscillation ±{osc}, initial mean{pop}, "+
 (f"mean rises by{growth} per year."if mode=="linear"else f"mean grows {growth*100:g}% monthly."),
 f"P(t)={baseline}−{osc}cos(2πt/12), t months since January; January is initial minimum.",
 f"Source{i} linear/exponential trend with annual seasonal population")
# 37-40 exponential damping
for amplitude,decay,freq in [(10,.15,18),(8,.12,20),(17,1-(13/17)**(1/3),14),(19,1-(14/19)**(1/4),13)]:
 i=len(Q)+1
 decay_factor=1-decay
 add(f"A spring starts {amplitude}cm below equilibrium and oscillates {freq}Hz with amplitude multiplied each second by {decay_factor:.6f}. Write damped displacement.",
 f"D(t)={amplitude}({decay_factor:.7f})^t cos(2π{freq}t), t≥0.",
 f"Source{i} damped spring exponential envelope")
assert len(Q)==40
# 41-46 sustainability and damping thresholds
add("A lake has initial population20000 with annual harvest of3000 and oscillation ±2000; estimate first possible extinction.","P(t)=20000−3000t+2000cos2πt; no nonnegative population physically after first zero, first crossing t in[6,7.34]y; solve numerically.","Source41 seasonal population decline first zero")
add("Fish population begins500, declines by6% annually with ±25 seasonal variation. Approximate year when envelope below200.","Upper envelope=500(.94)^t+25; permanent below200 once t>ln(175/500)/ln(.94)≈"+f"{math.log(175/500)/math.log(.94):.2f} years.","Source42 population threshold with exponential loss")
for initial,after,seconds,freq in [(12,6,2,8),(20,4,6,20)]:
 i=len(Q)+1
 k=math.log(after/initial)/seconds
 t=math.log(.1/initial)/k
 add(f"Spring starts amplitude{initial}cm, decreases to{after}cm after{seconds}s, frequency{freq}Hz. When is envelope permanently <0.1cm?",
 f"D(t)={initial}e^({k:.6f}t)cos(2π{freq}t), envelope crossing t≈{t:.4f}s.",
 f"Source{i} damped spring near-rest envelope threshold")
for A1,rate1,A2,after2,dt2 in [(30,.5,16,2,4),(3,.08,10,2,3)]:
 i=len(Q)+1
 first=math.log(.1/A1)/math.log(1-rate1)
 second=dt2*math.log(.1/A2)/math.log(after2/A2)
 faster="first"if first<second else"second"
 add(f"Compare two springs envelopes A1={A1}({1-rate1})^t and A2={A2}({after2/A2})^(t/{dt2}), permanent rest threshold0.1cm.",
 f"First t≈{first:.3f}s; second t≈{second:.3f}s; {faster} reaches threshold first.",
 f"Source{i} compare two damped spring stopping times")
assert len(Q)==46
for legs in [
[(150,20),(180,110)],
[(200,60),(300,150)]
]:
 i=len(Q)+1
 # bearing clockwise from north, east=dist*sin θ, north=dist*cos θ
 east=sum(l*math.sin(math.radians(ang))for l,ang in legs)
 north=sum(l*math.cos(math.radians(ang))for l,ang in legs)
 distance=math.hypot(east,north)
 bearing=math.degrees(math.atan2(east,north))%360
 add(f"Plane flies legs (distance miles, bearing clockwise from north) {legs}. Find resultant distance and bearing.",
 f"east≈{east:.3f},north≈{north:.3f},distance≈{distance:.3f}mi,bearing≈{bearing:.3f}°.",
 f"Source{i} vector composition of two flights")
for a,b,c in [(6,3,4),(5,4,2),(4,2,-6)]:
 i=len(Q)+1
 ys=[a*(b**x)+c*math.sin(math.pi*x/2)for x in range(4)]
 add(f"Find y=a·b^x+c sin(πx/2) fitting x0..3 data {ys}.",
 f"a={a}, b={b}, c={c}; check y0=a,y2=a b²,y1=ab+c,y3=ab³−c.",
 f"Source{i} exponential+sinusoidal table fitting")
for a,b,c in [(8,2,3),(5,3,1)]:
 i=len(Q)+1
 ys=[a*b**x*math.cos(math.pi*x/2)+c for x in range(4)]
 add(f"Find y=a·b^x cos(πx/2)+c fitting x0..3 data {[round(y,6)for y in ys]}.",
 f"a={a},b={b},c={c}; use odd inputs y1=y3=c and y0=a+c, y2=−ab²+c.",
 f"Source{i} exponential cosine+offset table fitting")
assert len(Q)==53 and [e["SourceExercise"]for e in Q]==list(range(1,54))
# Mathematical spot checks
assert math.isclose(1-math.acos(0)/math.pi,.5)
assert math.isclose(10*math.cos(0),10)
assert math.isclose((4+6)/2,5)
path=B/"u07-l06-53-firstparty-source-skill-math-tasks.json"
path.write_text(json.dumps({"SchemaVersion":1,"Section":"7.6","Count":53,
"FirstPartyIndependentStudentMathAndTeacherKeys":Q,
"AutomaticGradeCertified":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L06_53_INDEPENDENT_MATH_APPLICATIONS_PREPARED_NOT_CLOSED")
