(function (global) {
    'use strict';

    const rounds = [
        { a: 2, b: 1, zone: 0 },
        { a: 3, b: 2, zone: 0 },
        { a: 1, b: 4, zone: 0 },
        { a: 2, b: 3, zone: 1 },
        { a: 4, b: 1, zone: 1 },
        { a: 3, b: 3, zone: 1 },
        { a: 5, b: 2, zone: 2 },
        { a: 4, b: 3, zone: 2 },
        { a: 2, b: 5, zone: 2 },
        { a: 5, b: 4, zone: 2 }
    ].map((round, index) => {
        const sum = round.a + round.b;
        return {
            ...round,
            id: index + 1,
            sum,
            answers: [sum - 1, sum, sum + 1].filter(value => value > 0)
        };
    });

    const storyByZone = [
        round => `The trail at Discovery Peak is growing dim. Eddy spots ${round.a} glowing lights near the bridge and ${round.b} more by the lantern. Bring both groups together to relight the path.`,
        round => `The crystals inside Curiosity Caves are losing their glow. Eddy finds ${round.a} lights in one group and ${round.b} more nearby. Join both groups to wake the cave lantern.`,
        round => `You have reached Practice Point. The last lanterns need power. Eddy sees ${round.a} lights and ${round.b} more. Bring them together to finish the trail.`
    ];

    global.EdulyticsGameActivities = global.EdulyticsGameActivities || {};
    global.EdulyticsGameActivities['join-groups-to-add-v9'] = {
        id: 'join-groups-to-add-v9',
        lessonCode: 'PED:CAMBRIDGE-INTL-MATH:S1:L10',
        learningOutcome: 'CAM:OUT:0096:1Ni.02',
        lessonLanguage: 'en',
        title: 'Join Groups to Add',
        worldTitle: 'Lantern Isles',
        assets: {
            world: '/images/game/v9/lumen-trail-world.webp?v=0.9.2',
            student: '/images/game/v9/student-explorer.webp?v=0.9.2',
            guide: '/images/game/v9/eddy-guide.webp?v=0.9.2'
        },
        zones: [
            { name: 'Discovery Peak', focusX: 520, focusY: 330, zoom: 1.22, taskX: 610, taskY: 560 },
            { name: 'Curiosity Caves', focusX: 1180, focusY: 535, zoom: 1.26, taskX: 1160, taskY: 620 },
            { name: 'Practice Point', focusX: 690, focusY: 800, zoom: 1.32, taskX: 730, taskY: 760 }
        ],
        rounds,
        copy: {
            intro: name => `Welcome${name ? `, ${name}` : ''}. Eddy needs your help to relight the Lantern Isles. Join two groups, move every light into the lantern, then choose the total.`,
            collect: round => storyByZone[round.zone]?.(round) || `Bring the group of ${round.a} and the group of ${round.b} together.`,
            choose: round => `${round.a} + ${round.b} = ?`,
            question: round => `Great! You joined all the lights. ${round.a} plus ${round.b} equals what? Choose the total.`,
            hint1: 'Count every light in the joined group.',
            hint2: round => `Start with ${round.a}, then count on ${round.b} more.`,
            correct: 'Yes! The lantern glows brighter and the trail opens ahead.',
            complete: 'You restored the Lantern Isles. Brilliant work!'
        }
    };
}(window));
