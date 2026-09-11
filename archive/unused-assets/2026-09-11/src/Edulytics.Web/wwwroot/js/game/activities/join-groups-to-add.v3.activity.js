(function (global) {
    'use strict';

    global.EdulyticsGameActivities ||= {};

    global.EdulyticsGameActivities['join-groups-to-add-v3'] = {
        schemaVersion: 3,
        activityId: 'EDU-GAME-CAM-S1-JOIN-GROUPS-V3',
        title: 'Join Groups Adventure',
        lessonLanguage: 'en',
        direction: 'ltr',
        voiceLocale: 'en-GB',

        curriculum: {
            frameworkCode: 'CAMBRIDGE-INTL-MATH',
            curriculum: 'British / Cambridge Primary',
            grade: 'Cambridge Primary Stage 1',
            subject: 'Mathematics',
            topic: 'Addition, Subtraction and Doubles',
            lessonCode: 'PED:CAMBRIDGE-INTL-MATH:S1:L10',
            lessonTitle: 'Join Groups to Add',
            learningOutcomeCodes: ['CAM:OUT:0096:1Ni.02'],
            skill: 'Combining groups and counting on',
            difficulty: 'Foundational'
        },

        characters: {
            guide: { name: 'Eddy' },
            student: { name: null }
        },

        assets: {
            guide: '/images/public/edulytics-math-mascot-animation.png'
        },

        generator: {
            type: 'addition-pairs',
            minOperand: 1,
            maxOperand: 6,
            minSum: 3,
            maxSum: 10,
            unique: true
        },

        progression: {
            rounds: 10,
            adaptive: {
                enabled: true,
                strongerHintAfter: 2
            }
        },

        rewards: {
            xpPerRound: 10,
            starsPerRound: 1
        },

        copy: {
            activityTitle: 'The Lantern Path',
            girlVoice: 'Girl voice',
            boyVoice: 'Boy voice',
            sound: 'Sound',
            fullscreen: 'Full screen',
            replayGuide: 'Replay Eddy',
            continue: 'Continue',
            skip: 'Skip',
            start: 'Start adventure',
            missionLabel: 'Lantern mission',
            taskTitle: 'Bring the two groups together',
            taskInstruction: q => `Bring ${q.left} sunset fireflies and ${q.right} moonlight fireflies into the lantern.`,
            fireflyLabel: 'Move this firefly to the lantern',
            questionLabel: 'Open the bridge',
            collectFirst: 'Bring every firefly to the lantern. The number stones will rise from the river.',
            answerInstruction: 'Choose the stepping stone that opens the bridge.',
            studentFallback: 'Student',
            roundIntro: (q, ctx) => `${ctx.studentName}, the bridge needs more light. Bring ${q.left} fireflies from the sunset bush and ${q.right} from the moonlight bush into the lantern.`,
            answerPrompt: q => `${q.left} plus ${q.right}. How many fireflies are glowing together? Choose the stepping stone with the total.`,
            tryAgain: 'Almost. That stone did not wake the bridge.',
            hints: [
                () => 'Look inside the lantern and count each glowing light once.',
                q => `Start with ${q.left}. Then count on ${q.right} more.`,
                q => `The two groups are ${q.left} and ${q.right}. Say the counting sequence slowly while the lantern glows.`
            ],
            correct: q => `Yes! ${q.left} plus ${q.right} equals ${q.sum}. The bridge is awake!`,
            complete: 'Lantern Path complete!',
            completeCopy: 'You powered every lantern and opened the whole path across the meadow.',
            completeSpeech: 'Amazing work! You powered every lantern and completed the Lantern Path.',
            playAgain: 'Play again',
            introDialogue: [
                { speaker: 'guide', text: 'Welcome to Lantern Meadow. The bridge across the river has lost its light.' },
                { speaker: 'student', text: () => 'I can help, Eddy. Show me what to do.' },
                { speaker: 'guide', text: 'Bring the two firefly groups into each lantern. When it glows, choose the number stone that makes the bridge appear.' }
            ]
        }
    };
}(window));