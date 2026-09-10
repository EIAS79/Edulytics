(function (global) {
    'use strict';
    global.EdulyticsGameActivities ||= {};
    global.EdulyticsGameActivities['join-groups-to-add-v4'] = {
        schemaVersion: 6,
        activityId: 'EDU-GAME-CAM-S1-JOIN-GROUPS-V6',
        title: 'Lantern Isles',
        lessonLanguage: 'en', direction: 'ltr', voiceLocale: 'en-GB',
        curriculum: { frameworkCode:'CAMBRIDGE-INTL-MATH', curriculum:'British / Cambridge Primary', grade:'Cambridge Primary Stage 1', subject:'Mathematics', topic:'Addition, Subtraction and Doubles', lessonCode:'PED:CAMBRIDGE-INTL-MATH:S1:L10', lessonTitle:'Join Groups to Add', learningOutcomeCodes:['CAM:OUT:0096:1Ni.02'], skill:'Combining groups and counting on', difficulty:'Foundational' },
        assets: { guide:'/images/public/edulytics-math-mascot-animation.png', student:'/images/game/lantern-isles-v5-student.svg', world:'/images/game/lantern-isles-v6-world-master.webp' },
        generator: { pairs:[[2,1],[1,4],[3,2],[2,5],[4,3],[4,4],[5,2],[3,5],[5,4],[6,4]], maxSum:10 },
        progression: { rounds:10, strongerHintAfter:2 }, rewards:{ xpPerRound:10, starsPerRound:1 },
        world: { name:'Lantern Isles', mapNodes:[
            {id:1,name:'Meadow Gate',x:.18,y:.66},{id:2,name:'Glow Falls',x:.31,y:.50},{id:3,name:'Firefly Bend',x:.43,y:.34},{id:4,name:'Moonlit Ford',x:.55,y:.56},{id:5,name:'Lantern Grove',x:.67,y:.40},{id:6,name:'River Steps',x:.80,y:.30},{id:7,name:'Star Meadow',x:.77,y:.62},{id:8,name:'Whisper Bridge',x:.63,y:.76},{id:9,name:'Sunset Hollow',x:.45,y:.77},{id:10,name:'Beacon Hill',x:.29,y:.73}
        ]},
        copy: { studentFallback:'Student', worldTitle:'Lantern Isles', arrivalTitle:'A new path is waiting', mapPrompt:'Choose the glowing mission', missionTitle:'Bring the lights together',
            collectPrompt:q=>`Bring ${q.left} sunset fireflies and ${q.right} moonlight fireflies to the lantern.`, answerPrompt:q=>`${q.left} + ${q.right} = ?`, answerHelp:'Choose the stepping stone that lights the bridge.', correct:q=>`${q.left} plus ${q.right} equals ${q.sum}. The path is open!`, wrong:'That stone flickered. Try another one.',
            hints:[q=>'Count every light inside the lantern once.',q=>`Start at ${q.left}, then count on ${q.right} more.`,q=>`The two groups are ${q.left} and ${q.right}. Put them together and count the total.`],
            introDialogue:[{speaker:'guide',text:n=>`Welcome, ${n}. Lantern Isles is waking up, but the light paths are broken.`},{speaker:'student',text:()=> 'Can we bring the lights back?'},{speaker:'guide',text:()=> 'Yes. Join each pair of firefly groups, light the lantern, then choose the stepping stone with the total.'}],
            completeTitle:'Lantern Isles restored!', completeCopy:n=>`${n}, you powered all ten lantern paths.`, continue:'Continue', startMission:'Start mission', backToMap:'Back to map', nextMission:'Next mission', replay:'Play again', skip:'Skip' }
    };
}(window));