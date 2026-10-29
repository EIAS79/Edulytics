'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,ideas,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U05:L'+String(n).padStart(2,'0'),CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,'Original first-party Edulytiks instruction informed by Open Up Resources / Illustrative Mathematics Grade 8, 2017–2019 (CC BY 4.0), source https://im.kendallhunt.com/MS/teachers/3/5/'+n+'/preparation.html . No publisher figures or branded assessment tasks are reused.'],
 KeyConcepts:ideas.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(11,'Filling Containers',[
 'Goal: describe how water volume and surface height are related as different shapes of containers are filled, attending to cross-sectional area.',
 'A straight-sided container with constant horizontal cross-section has volume proportional to water height. For a container that widens upward, each additional centimeter of height needs more water than the preceding centimeter.',
 'Original comparison: pour equal 100-mL portions into a tall narrow rectangular container and a wider one. Record water heights and discuss where a linear graph is justified. Teacher-scored diagrams and observations matter; predictions cannot replace actual measurements.'
],[
 ['Cross-sectional area','Area of a horizontal slice of the container.','For a uniform prism, volume=A·h.','A 20 cm² base fills at 5 cm height with 100 cm³.'],
 ['Constant-fill area','Equal volume increments raise water by equal heights.','Δh=ΔV/A in a constant-area vessel.','Adding 40 cm³ to base 20 cm² raises water 2 cm.'],
 ['Variable area','Area changes as height changes.','Height-versus-volume graph may be curved.','A widening vessel rises more slowly at high fill levels.']
],[
 ['Prism fill height','Rectangular container base 20 cm² holds 100 cm³. Find water height.','Use V=Ah.', ['V=100 cm³ and A=20 cm².','Height h=V/A=100/20.','h=5 cm.'],'5 cm.','20×5=100 cm³.'],
 ['Extra water','Same container receives 40 cm³ extra. Height rise?','Use Δh=ΔV/A.', ['Added volume 40 cm³.','Constant base area 20 cm².','Height increases by 40/20=2 cm.'],'2 cm.','Added 2-cm slice occupies 40 cm³.'],
 ['Compare widths','Two rectangular vessels have base areas 20 and 40 cm². Pour 80 cm³ into each. Which water height is larger?','Divide volume by area.', ['Narrow vessel height 80/20=4 cm.','Wide vessel height 80/40=2 cm.','The narrower vessel fills higher.'],'4 cm versus 2 cm; narrow vessel is higher.','The equal volumes occupy different heights.']
],[
 ['Assuming all containers fill at constant height rate.','Changing cross-sectional area changes height response.','Inspect shape at each level.'],
 ['Confusing volume with height units.','cm³ and cm are different dimensions.','Use volume/base area.']
],['Fill rate depends on cross-sectional area.','Uniform prisms give linear volume-height relations.','Varying shapes may yield nonlinear graphs.']),
lesson(12,'How Much Will Fit?',[
 'Goal: estimate and compare capacities of solids using cubic units and known prism geometry, explaining which measurement determines how much fits.',
 'Count unit cubes in a rectangular prism as layers: base area times number of layers. If physical dimensions are measured in centimeters, capacity is in cubic centimeters; 1 cubic centimeter equals 1 milliliter.',
 'Construct two small paper containers with the same height but different bases. Predict capacities first and then verify with unit cubes or water where safe. Teacher review assesses the physical representation.'
],[
 ['Unit cube','A cube with edge length one unit.','One 1-cm cube occupies 1 cm³.','A 2×3×4 prism has 24 such cubes.'],
 ['Prism capacity','Volume from base area and height.','V=length×width×height.','2×3×4=24 cm³.'],
 ['Capacity units','Cubic centimeters relate to milliliters.','1 cm³=1 mL.','250 cm³ corresponds to 250 mL.']
],[
 ['Rectangular volume','Find volume of a 2 by 3 by 4 cm prism.','Multiply three dimensions.', ['Base area is 2×3=6 cm².','There are 4 cm of height.','Volume 6×4=24 cm³.'],'24 cm³.','Count as four layers of six cubes.'],
 ['Compare containers','One prism is 2×3×4 cm; another is 3×3×4 cm. Which holds more?','Compute both volumes.', ['First is 24 cm³.','Second is 3×3×4=36 cm³.','Second exceeds first by 12 cm³.'],'Second container by 12 cm³.','36>24.'],
 ['Convert capacity','A prism occupies 250 cm³. How many milliliters?','Use volume equivalence.', ['One cm³ equals one mL.','Multiply 250 by 1 mL/cm³.','Capacity 250 mL.'],'250 mL.','Units cancel correctly.']
],[
 ['Adding dimensions rather than multiplying them.','Volume counts 3D unit cubes.','Use base area times height.'],
 ['Reporting square units for volume.','Capacity is three-dimensional.','Use cm³ or mL.']
],['Volume measures 3D capacity.','Prism layers multiply base area by height.','Convert cm³ to mL when appropriate.']),
lesson(13,'The Volume of a Cylinder',[
 'Goal: develop and apply V=πr²h for a right circular cylinder using circular base area and height.',
 'A cylinder is a stack of congruent circles. The circular base has area πr², so multiplying that area by vertical height gives volume. Radius is half diameter; never substitute diameter directly as r.',
 'Construct an original paper cylinder with marked radius and height and discuss the relationship between a circle drawing and a 3D solid. A teacher checks labeling and reasoning; numeric formula substitution is a separate graded task.'
],[
 ['Cylinder base','A circular disk with radius r.','Area A=πr².','Radius 3 cm gives base area 9π cm².'],
 ['Cylinder volume','Base area times perpendicular height.','V=πr²h.','r=3,h=5 gives 45π cm³.'],
 ['Radius versus diameter','Radius is half of a diameter.','r=d/2.','Diameter 8 cm implies radius 4 cm.']
],[
 ['Compute exact cylinder','Radius 3 cm, height 5 cm. Find exact volume.','Apply πr²h.', ['r²=9 cm².','Multiply by h=5 to get 45.','Include π and cubic units.'],'45π cm³.','≈141.37 cm³.'],
 ['Diameter input','Diameter 8 cm, height 2 cm. What is volume?','Halve diameter first.', ['Radius r=8/2=4 cm.','Base area=π×16.','Multiply height 2: V=32π cm³.'],'32π cm³.','The diameter must not be squared as the radius.'],
 ['Compare heights','Two cylinders have same r and heights 3 and 9. Volume ratio?','Base area cancels.', ['Both share πr².','Volumes are 3πr² and 9πr².','Ratio second/first=9/3=3.'],'3 to 1.','Tripling h triples volume.']
],[
 ['Using d² instead of r².','Diameter is twice radius and produces fourfold volume error.','Divide diameter by 2 first.'],
 ['Reporting πrh rather than πr²h.','Base area is quadratic in radius.','Square r before multiplying height.']
],['Cylinder volume is πr²h.','Identify radius and perpendicular height.','Use cubic units.']),
lesson(14,'Finding Cylinder Dimensions',[
 'Goal: use a known cylinder volume to solve for an unknown height or radius, distinguishing exact algebraic values from rounded physical measurements.',
 'Rearrange V=πr²h. For known radius, h=V/(πr²). For known height, r=√(V/(πh)), taking the nonnegative root because physical radius cannot be negative.',
 'Use a first-party container-design task in which a required capacity and fixed radius determine height. Explain how a change in diameter affects required height. Teacher-scored diagrams complement numerical algebra.'
],[
 ['Solve for height','Rearrange cylinder volume with known r.','h=V/(πr²).','V=72π, r=3 gives h=8.'],
 ['Solve for radius','Isolate squared radius first.','r=√(V/(πh)) with r≥0.','V=50π,h=2 gives r=5.'],
 ['Physical dimensions','Radius and height are nonnegative lengths.','Reject negative root physically.','r=−5 is not a radius.']
],[
 ['Find height','Cylinder volume 72π cm³ and radius 3 cm. Find h.','Divide by base area.', ['Base area is π×3²=9π.','h=72π/(9π).','h=8 cm.'],'8 cm.','9π×8=72π.'],
 ['Find radius','Volume 50π cm³ and height 2 cm. Find radius.','Solve r².', ['50π=πr²×2.','Divide by 2π: r²=25.','Take physical root r=5 cm.'],'5 cm.','π×25×2=50π.'],
 ['Change radius at fixed volume','A cylinder radius doubles and volume stays fixed. How must height change?','Compare πr²h.', ['Doubling r multiplies base area by 4.','To preserve total volume, multiply height by 1/4.','New height is one quarter of original.'],'Height becomes 1/4.','4×1/4=1 for volume factor.']
],[
 ['Taking a negative square root as physical radius.','Length cannot be negative.','Use nonnegative root.'],
 ['Ignoring radius squared when solving.','Area scales quadratically with r.','Isolate r² first.']
],['Rearrange before substituting.','Heights divide by base area.','Radii come from nonnegative square roots.']),
lesson(15,'The Volume of a Cone',[
 'Goal: connect a cone with a cylinder of equal base radius and perpendicular height to justify V=(1/3)πr²h.',
 'Three ideal equal-base equal-height cones fill the volume of the matching cylinder. The slant height is not the perpendicular height needed for the volume formula.',
 'Draw a cone and its circumscribing cylinder, mark the same vertical height, and compare capacity through an original measure-and-pour investigation. Teacher assessment checks visual reasoning; formula calculations alone do not prove construction parity.'
],[
 ['Cone volume','One-third of same-base, same-height cylinder.','V=πr²h/3.','r=3,h=6 gives 18π.'],
 ['Perpendicular height','Distance straight from apex to base plane.','Use this h, not side slant.','A slant of 5 and radius 3 gives height 4.'],
 ['Cylinder comparison','Same r,h cylinder is three times the cone volume.','Vcone=Vcylinder/3.','Cylinder 54π corresponds cone 18π.']
],[
 ['Compute cone','Cone radius 3 cm and height 6 cm. Find volume.','Use one-third cylinder formula.', ['Base area 9π.','Matching cylinder volume 9π×6=54π.','Cone volume 54π/3=18π cm³.'],'18π cm³.','Three such cones total 54π.'],
 ['Slant to height','Right cone radius 3 cm and slant height 5 cm. Find perpendicular h.','Use right-triangle geometry.', ['Slant²=radius²+height².','h²=25−9=16.','Physical height h=4 cm.'],'4 cm.','3²+4²=5².'],
 ['Compare equal cylinders','Cylinder and cone same base and height; cylinder 60 cm³. Cone volume?','Divide by 3.', ['Equal r,h condition holds.','Cone is one-third of matching cylinder.','60/3=20 cm³.'],'20 cm³.','20×3=60.']
],[
 ['Using slant height in volume formula.','Formula requires perpendicular height.','Calculate vertical height separately.'],
 ['Forgetting one third.','Cone fills one-third the matching cylinder.','Divide by 3.']
],['Cone volume is πr²h/3.','Use perpendicular height.','Same-base cylinder comparison provides the factor.']),
lesson(16,'Finding Cone Dimensions',[
 'Goal: solve the cone-volume equation for an unknown height or radius given other dimensions, with exact expressions and physical constraints.',
 'From V=πr²h/3, solve h=3V/(πr²). For known height solve r=√(3V/(πh)), selecting the nonnegative root. Respect cubic versus linear units.',
 'Design a cone-shaped container of a specified capacity and one fixed dimension. A teacher reviews dimensional interpretation and geometry while exact number checks verify algebra.'
],[
 ['Cone height inverse','Solve for h.','h=3V/(πr²).','V=24π,r=2 gives h=18.'],
 ['Cone radius inverse','Solve for r.','r=√(3V/(πh)).','V=12π,h=9 gives r=2.'],
 ['Physical domain','Valid solid dimensions are nonnegative.','Reject negative radius and height.','Use positive square root for r.']
],[
 ['Find cone height','V=24π cm³ and r=2 cm. Find h.','Rearrange volume formula.', ['24π=(π×4×h)/3.','Multiply 3:72π=4πh.','Divide 4π: h=18 cm.'],'18 cm.','(π×4×18)/3=24π.'],
 ['Find cone radius','V=12π cm³, h=9 cm. Find r.','Isolate r².', ['12π=πr²×9/3=3πr².','Divide 3π: r²=4.','Take r=2 cm.'],'2 cm.','π×4×9/3=12π.'],
 ['Compare dimensions','If cone r is doubled and height unchanged, what happens to volume?','Inspect r².', ['Radius factor is 2.','Base area factor is 2²=4.','Height and one-third factor unchanged.'],'Volume quadruples.','(2r)²=4r².']
],[
 ['Leaving the factor of three on the wrong side.','Cone inverse differs from cylinder inverse.','Multiply volume by 3 before dividing base area.'],
 ['Taking negative physical radius.','Geometry uses a nonnegative length.','Select the positive square root.']
],['Solve cone volume algebraically.','Radius is quadratic, height linear.','Verify results by substitution.']),
lesson(17,'Scaling One Dimension',[
 'Goal: predict volume change when one dimension of a prism, cylinder or similar cross-section solid changes while others remain fixed.',
 'When only height doubles, volume doubles for a fixed base. If only a rectangular length doubles, volume doubles. When only radius of a cylinder doubles, volume quadruples because it changes two base-area directions.',
 'Use original rectangular-box models with colored unit-cube layers to distinguish scaling one linear height from scaling a circular radius. A teacher reviews the diagram and explanation of unchanged dimensions.'
],[
 ['One linear dimension','A prism dimension multiplied by k while others fixed.','V new/V old=k.','2×3×4 becomes 2×3×8, doubling volume.'],
 ['Cylinder height','Only h changes.','V=πr²h scales linearly in h.','h→3h gives V→3V.'],
 ['Cylinder radius','Only r changes but two-dimensional base area changes.','r→kr gives V→k²V.','r→2r gives V→4V.']
],[
 ['Double box height','Box 2×3×4 doubles height from 4 to 8. New volume?','Multiply unchanged base by new h.', ['Original volume is 2×3×4=24.','New height 8 gives 2×3×8=48.','Factor 48/24=2.'],'48 cubic units.','Only one linear dimension doubled.'],
 ['Triple cylinder height','Cylinder volume 20π grows to three times height, same radius. New volume?','Scale h.', ['Original V=20π.','Height factor is 3.','New V=3×20π=60π.'],'60π cubic units.','Base area unchanged.'],
 ['Double cylinder radius','Cylinder originally 20π, same height but radius doubles. New volume?','Scale r².', ['New radius squared is 4 times original.','Height unchanged.','New V=4×20π=80π.'],'80π cubic units.','Not merely 40π.']
],[
 ['Assuming radius acts as one independent dimension in πr².','Radius changes two area dimensions.','Square its factor.'],
 ['Changing an unmentioned dimension.','The task asks only one dimension to change.','List what is held fixed.']
],['Volume response depends on which dimension changes.','Prism height scales linearly.','Cylinder radius has squared effect.']),
lesson(18,'Scaling Two Dimensions',[
 'Goal: reason about volume when two dimensions vary independently, multiplying their factors correctly.',
 'For a rectangular prism with length factor a and width factor b but fixed height, volume factor is ab. For a cylinder, scaling radius by a and height by b produces factor a²b.',
 'Compare independently constructed boxes and cylinders before and after scaling. A teacher reviews which dimension labels were changed and why factors multiply.'
],[
 ['Prism scale product','Scale two independent linear dimensions.','V factor=a×b.','Length ×2 and width ×3 produce volume ×6.'],
 ['Cylinder scale product','Radius factor a and height factor b.','V factor=a²b.','r×2 and h×3 produce V×12.'],
 ['Uniform 3D scaling','All linear dimensions scaled by same k.','Volume factor=k³.','A similar solid doubled in all dimensions has eightfold volume.']
],[
 ['Two box dimensions','Box 2×3×4: double length and triple width, same height. New V?','Scale each independent dimension.', ['New dimensions are 4×9×4.','Multiply 4×9×4=144.','Old volume 24; ratio 144/24=6.'],'144 cubic units, factor 6.','Factors 2×3=6.'],
 ['Radius and height','Cylinder original volume 5π; double radius and triple height. New volume?','Apply a²b.', ['Radius change gives area factor 4.','Height change gives factor 3.','Total volume factor 12, so new V=60π.'],'60π cubic units.','5π×12=60π.'],
 ['All dimensions doubled','Solid volume 7 cm³ undergoes uniform scale factor 2. New volume?','Use cubic factor.', ['Each of three independent dimensions doubles.','Volume factor is 2³=8.','New volume 7×8=56 cm³.'],'56 cm³.','A scale factor of 2 is not a volume factor of 2.']
],[
 ['Adding instead of multiplying scale factors.','Independent dimensions multiply in volume.','Use product of dimension factors.'],
 ['Forgetting radius square in cylinder scaling.','Circular base area depends on r².','Use a²b.']
],['Volume factors multiply across dimensions.','A cylinder radius factor is squared.','Uniform 3D scale k changes volume by k³.']),
lesson(19,'Estimating a Hemisphere',[
 'Goal: estimate the volume of a hemisphere using comparisons with familiar cylindrical and conical volumes and explain the limitations of estimates.',
 'A hemisphere is half a sphere. A careful comparison can bound its volume using a surrounding cylinder, but numerical experiments alone do not constitute a proof of the exact sphere formula.',
 'Draw a hemisphere of radius 3 fitting inside a cylinder with radius 3 and height 3. Estimate an upper bound of 27π cm³, then discuss that the exact hemisphere volume is 18π cm³. A teacher checks the conceptual diagram and assumptions.'
],[
 ['Hemisphere','Half of a sphere cut through its center.','Vhemi=Vsphere/2.','For radius 3, Vhemi=18π cm³.'],
 ['Containing cylinder','A hemisphere radius r fits inside a cylinder radius r, height r.','Cylinder volume πr³ is an upper bound.','For r=3, bound 27π.'],
 ['Exact hemisphere formula','Half of 4πr³/3.','V=2πr³/3.','r=3 gives 18π.']
],[
 ['Cylinder bound','Radius 3 hemisphere inside same-radius height-3 cylinder. What cylinder volume?','Use πr²h.', ['Radius squared is 9.','Height is 3.','Cylinder volume 9π×3=27π.'],'27π cm³, an upper bound.','The hemisphere fits inside the cylinder.'],
 ['Exact hemisphere volume','Find hemisphere volume when r=3.','Use 2πr³/3.', ['Cube radius: 3³=27.','Multiply by 2π: 54π.','Divide by 3: 18π.'],'18π cm³.','It is below the 27π cylinder bound.'],
 ['Compare volumes','What fraction of the containing radius-r height-r cylinder does the hemisphere occupy?','Divide exact volumes.', ['Hemisphere V=2πr³/3.','Cylinder V=πr³.','Ratio is (2/3).'],'2/3.','Same r cancels.']
],[
 ['Treating the bounding cylinder volume as exact hemisphere volume.','A hemisphere leaves unfilled corner regions.','Use bound only as a comparison.'],
 ['Calling an experimental fill ratio a rigorous proof.','Empirical measurements have error.','Separate estimate from exact formula.']
],['A hemisphere is half a sphere.','Cylinder comparisons help bound capacity.','Exact hemisphere formula is 2πr³/3.']),
lesson(20,'The Volume of a Sphere',[
 'Goal: develop and apply the relationship between sphere radius and volume, including the cubic effect of scaling radius.',
 'Sphere volume is V=4πr³/3. Distinguish radius from diameter and compare with the hemisphere volume. Doubling radius multiplies sphere volume by eight, not two.',
 'Use a first-party diagram of a sphere and its central great circle. Label r and calculate volume at r=3; teacher review checks spatial reasoning and measurement annotation.'
],[
 ['Sphere volume','The volume inside a sphere of radius r.','V=4πr³/3.','r=3 yields 36π.'],
 ['Cubic radius scaling','Radius enters as r³.','Scale r by k, V changes by k³.','r×2 gives V×8.'],
 ['Diameter relation','The diameter is twice radius.','r=d/2.','Diameter 10 means r=5.']
],[
 ['Volume at radius 3','Sphere radius 3 cm. Find exact volume.','Substitute 4πr³/3.', ['r³=27.','Multiply 4π×27=108π.','Divide by 3: V=36π cm³.'],'36π cm³.','Hemisphere would be 18π cm³.'],
 ['Radius from diameter','Sphere diameter 10 cm. Find its exact volume.','Convert to radius.', ['Radius 10/2=5.','Cube 5³=125.','Volume (4π/3)×125=500π/3.'],'500π/3 cm³.','Using diameter directly would be eight times too large.'],
 ['Double radius','Sphere volume 12π at radius r. Radius doubles. New volume?','Use k³.', ['Linear scale factor is 2.','Volume factor 2³=8.','New volume 12π×8=96π.'],'96π cubic units.','Sphere volume is cubic in radius.']
],[
 ['Squaring radius instead of cubing it.','Sphere volume has three dimensions.','Use r³.'],
 ['Substituting diameter for radius.','Diameter is double and changes volume eightfold.','Halve diameter first.']
],['Sphere volume is 4πr³/3.','Radius scales volume cubically.','Use cubic units and correct radius.']),
lesson(21,'Cylinders, Cones, and Spheres',[
 'Goal: select and compare the appropriate volume formulas for cylinders, cones and spheres, including composite or missing-capacity situations.',
 'For a cylinder and cone with the same r,h, the cone has one third the cylinder volume. A sphere of radius r requires a cubic radius term and does not depend on an independent cylinder height.',
 'Create a labeled table with one solid of each type, compute volumes and justify the formula selected. Teacher review checks diagrams, units and how the chosen measurements correspond to each solid.'
],[
 ['Cylinder formula','Prism-like circular stack.','Vcyl=πr²h.','r=2,h=6 gives 24π.'],
 ['Cone formula','One-third same-r and same-h cylinder.','Vcone=πr²h/3.','r=2,h=6 gives 8π.'],
 ['Sphere formula','Round 3D solid depending on radius.','Vsphere=4πr³/3.','r=3 gives 36π.']
],[
 ['Cylinder calculation','Cylinder r=2,h=6. What volume?','Use πr²h.', ['r²=4.','Multiply height 6 to get 24.','Include π and cubic units.'],'24π cubic units.','4π×6=24π.'],
 ['Cone calculation','Cone r=2,h=6. What volume?','Divide matching cylinder volume by 3.', ['Matching cylinder volume is 24π.','Divide by 3.','Cone volume 8π.'],'8π cubic units.','Cone is one-third the same-base cylinder.'],
 ['Sphere calculation','Sphere radius 3. What volume?','Use 4πr³/3.', ['r³=27.','4×27/3=36.','Volume 36π.'],'36π cubic units.','Matches previous sphere example.']
],[
 ['Using cylinder formula on a cone.','Cone volume is one third.','Identify the solid type before selecting a formula.'],
 ['Equating sphere radius to an unrelated cylinder height.','Sphere has no independent axial height in formula.','Use radius only for sphere.']
],['Choose volume formula from solid geometry.','Check r, h and units.','Compare volumes through exact ratios.']),
lesson(22,'Volume As a Function of . . .',[
 'Goal: compare volume as a function of water height or shape dimensions, explaining which relationships are linear and which are nonlinear.',
 'For a cylinder with fixed radius, water volume V(h)=πr²h grows linearly with height. For a cone filled from the narrow tip upward, the water surface radius increases proportionally with water height; the filled volume grows as h³ when the container shape is fixed.',
 'Sketch volume-height graphs for a cylinder and an upright cone with the same total height. Explain why equal height increments need different volume increments in the cone. Teacher review is essential for diagram orientation, modeling assumptions and graph shape.'
],[
 ['Cylinder volume versus height','Fixed cross-sectional area produces a linear function.','V(h)=πr²h.','At r=2, V=4πh.'],
 ['Cone from tip versus height','Similar cross-sections grow with water height.','Filled volume proportional to h³ for fixed cone proportions.','Doubling water height from tip multiplies filled volume by 8, while still below brim.'],
 ['Interpreting graph slope','Rate of volume added per height increase equals cross-sectional area.','For widening shape, slope increases with height.','Near cone tip, tiny area gives slow volume increase.']
],[
 ['Cylinder volume function','Cylinder radius 2 cm. Express V at water height h and find V(3).','Use fixed circular base.', ['Base area is π×2²=4π.','V(h)=4πh.','At h=3, V=12π cm³.'],'V(h)=4πh; V(3)=12π cm³.','Volume-height graph is a line through origin.'],
 ['Cone fill scaling','In a conical vessel filled from tip, similar filled volumes at heights h and 2h have what ratio?','Use uniform 3D similarity.', ['Radius scales with height for similar cross-sections.','Both radius and height double.','Volume factor is 2³=8.'],'8 to 1.','Assumes 2h is within vessel height.'],
 ['Find volume at higher height','Cone filled from tip has volume 3 cm³ at height 2 cm. Predict volume at height 4 cm within the same cone.','Apply cubic scaling for similar filled cones.', ['Height scale factor is 4/2=2.','Volume factor is 2³=8.','New filled volume 3×8=24 cm³.'],'24 cm³.','Valid only for a cone with tip at bottom and height 4 within its walls.']
],[
 ['Assuming any container produces a linear volume-height relation.','Changing cross-section makes slope variable.','Check vessel shape.'],
 ['Using h³ for a cylinder of fixed radius.','Its area does not vary with water height.','Use linear V=Ah for fixed area.']
],['Volume-height functions depend on container geometry.','Fixed-area cylinders are linear in h.','Tip-up conical fill volumes scale cubically.'])
];
if(rows.length!==12||rows[0].LessonCode!=='PED:US-CCSS-MATH:G8:U05:L11'||rows.at(-1).LessonCode!=='PED:US-CCSS-MATH:G8:U05:L22')throw Error('Unit5 volume count');
for(const l of rows)if(l.ExplanationParagraphs.length<2||l.KeyConcepts.length<3||l.WorkedExamples.length<3||l.CommonMistakes.length<2||l.SummaryPoints.length<3)throw Error('Rich contract invalid: '+l.LessonCode);
const file=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u05-l11-l22-2017.rich-lesson-v2.json');
fs.writeFileSync(file,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u05-l11-l22-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U05_VOLUME '+JSON.stringify({lessons:rows.length,examples:rows.reduce((n,x)=>n+x.WorkedExamples.length,0),file}));
