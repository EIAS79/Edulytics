(function (global) {
    'use strict';
    global.EdulyticsGameActivities ||= {};
    global.EdulyticsGameActivities['join-groups-to-add-v7'] = {
        schemaVersion: 7,
        activityId: 'EDU-GAME-CAM-S1-JOIN-GROUPS-V7',
        title: 'Lantern Isles',
        lessonLanguage: 'en', direction: 'ltr', voiceLocale: 'en-GB',
        curriculum: { frameworkCode:'CAMBRIDGE-INTL-MATH', curriculum:'British / Cambridge Primary', grade:'Cambridge Primary Stage 1', subject:'Mathematics', topic:'Addition, Subtraction and Doubles', lessonCode:'PED:CAMBRIDGE-INTL-MATH:S1:L10', lessonTitle:'Join Groups to Add', learningOutcomeCodes:['CAM:OUT:0096:1Ni.02'], skill:'Combining groups and counting on', difficulty:'Foundational' },
        assets: {
            guide:'/images/public/edulytics-math-mascot-animation.png',
            student:'/images/game/lantern-isles-v5-student.svg',
            background:'/images/game/v7/lantern-isles-background.svg',
            terrain:'/images/game/v7/lantern-isles-terrain.svg',
            foreground:'/images/game/v7/lantern-isles-foreground.svg'
        },
        generator: { pairs:[[2,1],[1,4],[3,2],[2,5],[4,3],[4,4],[5,2],[3,5],[5,4],[6,4]], maxSum:10 },
        progression: { rounds:10, strongerHintAfter:2 },
        rewards:{ xpPerRound:10, starsPerRound:1 },
        world: { name:'Lantern Isles', mapNodes:[
            {id:1,name:'Meadow Gate',x:.16,y:.62},
            {id:2,name:'Glow Falls',x:.37,y:.42},
            {id:3,name:'Crystal Grove',x:.57,y:.39},
            {id:4,name:'Lantern Grove',x:.49,y:.56},
            {id:5,name:'Beacon Ridge',x:.75,y:.48},
            {id:6,name:'River Steps',x:.81,y:.59},
            {id:7,name:'Star Meadow',x:.69,y:.72},
            {id:8,name:'Whisper Bridge',x:.55,y:.76},
            {id:9,name:'Sunset Hollow',x:.38,y:.72},
            {id:10,name:'Home Light',x:.24,y:.67}
        ]},
        copy: {
            studentFallback:'Student',
            worldTitle:'Lantern Isles',
            arrivalTitle:'A new path is waiting',
            mapPrompt:'Choose the glowing mission',
            missionTitle:'Bring the lights together',
            collectPrompt:q=>`Bring ${q.left} sunset fireflies and ${q.right} moonlight fireflies to the lantern.`,
            answerPrompt:q=>`${q.left} + ${q.right} = ?`,
            answerHelp:'Choose the stepping stone that lights the bridge.',
            correct:q=>`${q.left} plus ${q.right} equals ${q.sum}. The path is open!`,
            wrong:'That stone flickered. Try another one.',
            hints:[q=>'Count every light inside the lantern once.',q=>`Start at ${q.left}, then count on ${q.right} more.`,q=>`The two groups are ${q.left} and ${q.right}. Put them together and count the total.`],
            introDialogue:[
                {speaker:'guide',text:n=>`Welcome, ${n}. The Lantern Isles have gone dim.`},
                {speaker:'student',text:()=> 'How do we bring the light paths back?'},
                {speaker:'guide',text:()=> 'Join each pair of firefly groups. Every correct total lights the next path.'}
            ],
            completeTitle:'Lantern Isles restored!',
            completeCopy:n=>`${n}, you powered all ten lantern paths.`,
            continue:'Continue', startMission:'Start mission', backToMap:'Back to map', nextMission:'Next mission', replay:'Play again', skip:'Skip'
        }
    };
}(window));
