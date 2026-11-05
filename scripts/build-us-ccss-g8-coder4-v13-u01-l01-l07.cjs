
'use strict';
const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const rich='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json';
const source=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json'),'utf8')).rows;
const prompts={
 L01:[
 'Source-aligned activity 1.1: show four original angle diagrams with one straight angle, one right angle, one acute angle and one obtuse angle. Invite multiple defensible odd-one-out explanations rather than require one universally correct selection.',
 'Source-aligned activity 1.2: create three original triangle-movement dance cards, privately choose A, B or C, describe the movement orally, and let a partner identify the intended sequence. Verify that the description is reproducible; this partner communication task is not automatically graded from a single number.'
 ],
 L03:[
 'Source-aligned activity 3.1: examine an isometric grid of equilateral triangles and ask the learner to notice parallel slanting lines and repeated 60° directions. The grid is not an ordinary Cartesian coordinate plane; do not imply its sloping basis vectors are perpendicular.',
 'Source-aligned activity 3.2: to translate an original triangle use the directed arrow from A to A′ as the vector and apply the same vector to every other vertex; then reset before a rotation or reflection. The source GeoGebra construction and its click/drag behavior remain a distinct interactive integration requirement.'
 ],
 L04:[
 'Source-aligned activity 4.1: display a completed reflected triangle for a brief timed glance, then hide it and ask students to reconstruct the image from one known point, the mirror line and equal perpendicular distances. A static first-party explanation alone does not implement the source timed-flash interaction.',
 'Source-aligned activities 4.2–4.3: one partner describes a triangle image that another sketches, followed by identifying a move from A to B and a second move from B to C on an isometric grid. Order of operations must be retained; the source drawing/GeoGebra interaction requires teacher or accessible geometric assessment.'
 ],
 L05:[
 'Source-aligned activity 5.1: for source-aligned triangle T with vertices (−3,0),(−2,−1),(−4,−3) and triangle U with vertices (1,2),(2,1),(0,−1), every correct translation has vector (+4,+2). Point pairs (−3,0)→(1,2) and (−4,−3)→(0,−1) describe the same translation; the reverse and the (1,2)→(2,1) option do not.',
 'Source-aligned activity 5.2: reflecting across the x-axis maps (13,10)→(13,−10), (13,−20)→(13,20), and (13,570)→(13,−570). Reflecting R(3,2) across the y-axis gives R′(−3,2). Learners should label original and image points and justify the rule.',
 'Source-aligned activity 5.3: create a segment, transform both endpoints by the same rigid rule, and label the image segment; human review of plotting and correct correspondence remains distinct from validating two numeric pairs.'
 ],
 L06:[
 'Source-aligned activity 6.1: determine a plausible center from an original polygon and its 90° counterclockwise image. A first-party example uses vertex P(2,1) mapped to P′(0,3) by rotation about C(0,1); check P−C=(2,0) and P′−C=(0,2) before concluding. One point pair alone may permit alternative motion types, so the full polygon must confirm the rotation.',
 'Source-aligned activity 6.2: simulate an information gap with separate problem and data cards. A learner must request missing center/angle/vertex information from a partner and then explain the transformation. Do not replace the communication and card exchange task with a fabricated automatically scored verbal interaction.'
 ],
 L07:[
 'Source-aligned activity 7.1: show first-party ruler segments with eighths and tenths and distinguish exact reading from rounding. Source benchmark examples include 4⅝=4.625 units to the nearest eighth, 4.7 units to the nearest tenth, and approximately 3¾=3.75 units to the nearest eighth. Record units and precision clearly.',
 'Source-aligned activity 7.2: measure corresponding sides and angles before and after translating, rotating and reflecting a triangle. Because rigid moves preserve lengths and angle measures, explain why changed measurements disprove a rigid mapping rather than trusting a sketch.',
 'Source-aligned activity 7.3: given different candidate images, identify which ones can be rigid images and justify rejection with a specific altered length or angle; drawings, ruler work and explanations must receive a human or accessible-construction assessment.'
 ]
};
let txt=fs.readFileSync(path.join(root,rich),'utf8');
const lessons=JSON.parse(txt).Lessons;
for(const [id,lines] of Object.entries(prompts)){
 const l=lessons.find(x=>x.LessonCode.endsWith(':'+id));
 if(!l)throw Error('Lesson absent '+id);
 if(txt.includes(lines[0]))continue;
 const anchor='        '+JSON.stringify(l.ExplanationParagraphs[1])+',';
 if(!txt.includes(anchor))throw Error('Missing exact insertion anchor '+id);
 txt=txt.replace(anchor,anchor+'\n'+lines.map(x=>'        '+JSON.stringify(x)+',').join('\n'));
}
fs.writeFileSync(path.join(root,rich),txt);
const hash=crypto.createHash('sha256').update(fs.readFileSync(path.join(root,rich))).digest('hex');
const evidence={schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',edition:'IM/Open Up Resources 2017–2019',scope:'U01 L01 L03-L07 course-owned source activity spot review; full parity, digital interaction, teacher score and authenticated runtime remain unverified',newContentHash:hash,lessons:Object.entries(prompts).map(([id,lines])=>({lessonCode:'PED:US-CCSS-MATH:G8:U01:'+id,sourceStudentUrl:'https://im.kendallhunt.com/MS/students/3/1/'+Number(id.slice(1))+'/index.html',sourceTeacherUrl:'https://im.kendallhunt.com/MS/teachers/3/1/'+Number(id.slice(1))+'/index.html',sourceActivityTitles:source.find(x=>x.lessonCode.endsWith('U01:'+id)).activities.map(x=>x.title),addedCourseOwnedTeaching:lines,sourceParityAccepted:false,practiceExactGeneratedPersisted:false,teacherRubricPersisted:false,interactiveEquivalentVerified:false})),figureCountAdded:0,existingOriginalSvgRetained:true,acceptance:{A:0,B:0,C:0,D:0}};
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-g8-coder4-v13-u01-l01-l07-source-evidence-20261029.json'),JSON.stringify(evidence,null,2)+'\n');
const coursePath=path.join(root,'docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
let course=JSON.parse(fs.readFileSync(coursePath));course.hashes.richCandidates[path.basename(rich)]=hash;fs.writeFileSync(coursePath,JSON.stringify(course,null,2)+'\n');
console.log('U01 early source-aligned lessons:',Object.keys(prompts),'hash:',hash);
