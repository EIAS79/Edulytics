(() => {
    'use strict';

    window.EdulyticsGameActivities ??= {};

    window.EdulyticsGameActivities['count-touch-and-check-v1'] = {
        id: 'count-touch-and-check-v1',
        lessonCode: 'PED:CAMBRIDGE-INTL-MATH:S1:L01',
        lessonLanguage: 'en',
        world: 'Counting Grove',
        profile: {
            workspace: 'OBJECT_COUNTING',
            primaryMechanic: 'COUNT_TOUCH',
            secondaryMechanics: ['REARRANGE_RECOUNT'],
            interaction: ['tap', 'keyboard'],
            roundCount: 10
        },
        countPool: [3, 4, 5, 6, 7, 8, 9, 10],
        visualVariants: ['sunbug', 'leafbug', 'berrybug'],
        assets: {
            guide: '/images/game/v9/eddy-guide.webp'
        },
        copy: {
            introTitle: 'Wake the Counting Grove',
            introBody: 'Touch each glowbug once and count as you go. Then a breeze will move the same glowbugs. Count them again to check whether the total changed.',
            start: 'Start counting',
            firstPrompt: 'Touch every glowbug once. Count as you go.',
            firstCheck: 'How many glowbugs did you count? Choose the number stone.',
            breeze: 'Whoosh! The same glowbugs moved to new places. None flew away.',
            recountPrompt: 'Count the same glowbugs again. Touch each one once.',
            secondCheck: 'Did the total change? Choose the number stone.',
            duplicate: 'That glowbug is already counted. Find one without a number.',
            wrong: 'Not quite. Look at the numbered glowbugs and count them again.',
            hint: 'Try one glowbug at a time. Each glowbug should get exactly one number.',
            firstSuccess: count => `Yes — ${count}. Now watch what happens when they move.`,
            roundSuccess: count => `Exactly ${count}. Moving the glowbugs did not change how many there are.`,
            completeTitle: 'Counting Grove restored!',
            completeBody: 'You counted carefully and checked that changing the arrangement does not change the total.',
            next: 'Next grove',
            replay: 'Play again',
            soundOn: 'Sound on',
            soundOff: 'Sound off'
        }
    };
})();
