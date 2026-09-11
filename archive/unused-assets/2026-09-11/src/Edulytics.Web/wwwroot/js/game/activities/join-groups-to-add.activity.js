(function (global) {
    'use strict';

    global.EdulyticsGameActivities ||= {};

    global.EdulyticsGameActivities['join-groups-to-add'] = {
        schemaVersion: 1,
        activityId: 'EDU-GAME-CAM-S1-JOIN-GROUPS-V1',
        title: 'Join Groups Adventure',
        defaultLanguage: 'en',

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
            guide: { name: 'Eddy', avatar: 'E' },
            student: { name: 'Nova', avatar: '★' }
        },

        assets: {
            guide: '/images/public/edulytics-math-mascot-animation.png'
        },

        interaction: {
            type: 'grouping',
            answerMode: 'stepping-stones'
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

        voiceLocales: {
            en: 'en-GB',
            pl: 'pl-PL',
            ar: 'ar-EG'
        },

        locales: {
            en: {
                activityTitle: 'The Lantern Path',
                round: (current, total) => `${current} / ${total}`,
                girlVoice: '👧 Girl voice',
                boyVoice: '👦 Boy voice',
                continue: 'Continue',
                skip: 'Skip',
                start: 'Start adventure',
                missionLabel: 'Lantern mission',
                taskTitle: 'Bring the two groups together',
                taskInstruction: q => `Move ${q.left} sunset fireflies and ${q.right} moonlight fireflies into the lantern.`,
                fireflyLabel: 'Move firefly to the lantern',
                roundIntro: q => `Nova, we need both groups together. Bring ${q.left} fireflies from this bush and ${q.right} from the other bush into the lantern.`,
                answerInstruction: 'Choose the stepping stone that opens the path',
                answerPrompt: q => `${q.left} plus ${q.right}. How many fireflies are glowing together? Choose the correct stepping stone.`,
                tryAgain: 'Almost. That stone did not open the path.',
                hints: [
                    () => 'Look inside the lantern and count each glowing light once.',
                    q => `Start with ${q.left}. Then count on ${q.right} more.`,
                    () => 'Touch each glowing light with your eyes as it pulses. Count slowly from one.'
                ],
                correct: q => `Excellent! ${q.left} plus ${q.right} equals ${q.sum}. The path is open!`,
                complete: 'Lantern Path complete!',
                completeCopy: 'You joined every pair of groups and helped Nova cross the meadow.',
                completeSpeech: 'Amazing work! You completed the Lantern Path.',
                playAgain: 'Play again',
                introDialogue: [
                    { speaker: 'guide', text: 'Welcome to the Lantern Meadow! Our path across the stream has gone dark.' },
                    { speaker: 'student', text: 'I can help. What do we need to do?' },
                    { speaker: 'guide', text: 'Bring two groups of fireflies together inside each lantern. Then choose the number stone that matches the total.' }
                ]
            },

            pl: {
                activityTitle: 'Ścieżka lampionów',
                round: (current, total) => `${current} / ${total}`,
                girlVoice: '👧 Głos dziewczynki',
                boyVoice: '👦 Głos chłopca',
                continue: 'Dalej',
                skip: 'Pomiń',
                start: 'Rozpocznij przygodę',
                missionLabel: 'Misja z lampionem',
                taskTitle: 'Połącz dwie grupy',
                taskInstruction: q => `Przenieś ${q.left} świetliki z zachodu słońca i ${q.right} świetliki księżycowe do lampionu.`,
                fireflyLabel: 'Przenieś świetlika do lampionu',
                roundIntro: q => `Nova, połączmy obie grupy. Przenieś ${q.left} świetliki z tego krzaka i ${q.right} z drugiego do lampionu.`,
                answerInstruction: 'Wybierz kamień, który otworzy drogę',
                answerPrompt: q => `${q.left} plus ${q.right}. Ile świetlików świeci razem? Wybierz właściwy kamień.`,
                tryAgain: 'Prawie. Ten kamień nie otworzył drogi.',
                hints: [
                    () => 'Spójrz do lampionu i policz każde świecące światełko tylko raz.',
                    q => `Zacznij od ${q.left}. Potem dolicz jeszcze ${q.right}.`,
                    () => 'Patrz na każde światełko, gdy pulsuje, i licz powoli od jednego.'
                ],
                correct: q => `Świetnie! ${q.left} plus ${q.right} równa się ${q.sum}. Droga jest otwarta!`,
                complete: 'Ścieżka lampionów ukończona!',
                completeCopy: 'Połączyłeś wszystkie grupy i pomogłeś Novie przejść przez łąkę.',
                completeSpeech: 'Wspaniała robota! Ukończyłeś Ścieżkę lampionów.',
                playAgain: 'Zagraj ponownie',
                introDialogue: [
                    { speaker: 'guide', text: 'Witaj na Łące Lampionów! Nasza ścieżka przez strumień zgasła.' },
                    { speaker: 'student', text: 'Mogę pomóc. Co trzeba zrobić?' },
                    { speaker: 'guide', text: 'Połącz dwie grupy świetlików w każdym lampionie. Potem wybierz kamień z liczbą równą sumie.' }
                ]
            },

            ar: {
                activityTitle: 'طريق الفوانيس',
                round: (current, total) => `${current} / ${total}`,
                girlVoice: '👧 صوت بنت',
                boyVoice: '👦 صوت ولد',
                continue: 'متابعة',
                skip: 'تخطي',
                start: 'ابدأ المغامرة',
                missionLabel: 'مهمة الفانوس',
                taskTitle: 'اجمع المجموعتين معًا',
                taskInstruction: q => `انقل ${q.left} من اليراعات الذهبية و${q.right} من اليراعات البنفسجية إلى الفانوس.`,
                fireflyLabel: 'انقل اليراعة إلى الفانوس',
                roundIntro: q => `يا نوفا، نحتاج إلى جمع المجموعتين. انقل ${q.left} يراعات من هذه الشجيرة و${q.right} من الشجيرة الأخرى إلى الفانوس.`,
                answerInstruction: 'اختر حجر العبور الذي يفتح الطريق',
                answerPrompt: q => `${q.left} زائد ${q.right}. كم يراعة تضيء معًا الآن؟ اختر حجر العبور الصحيح.`,
                tryAgain: 'اقتربت. هذا الحجر لم يفتح الطريق.',
                hints: [
                    () => 'انظر داخل الفانوس وعدّ كل ضوء مرة واحدة فقط.',
                    q => `ابدأ بالعدد ${q.left}، ثم عدّ ${q.right} أعداد أخرى.`,
                    () => 'تابع كل ضوء عندما يومض، وعدّ ببطء من واحد.'
                ],
                correct: q => `ممتاز! ${q.left} زائد ${q.right} يساوي ${q.sum}. الطريق مفتوح الآن!`,
                complete: 'أكملت طريق الفوانيس!',
                completeCopy: 'جمعت كل المجموعات وساعدت نوفا على عبور المرج.',
                completeSpeech: 'عمل رائع! لقد أكملت طريق الفوانيس.',
                playAgain: 'العب مرة أخرى',
                introDialogue: [
                    { speaker: 'guide', text: 'مرحبًا بك في مرج الفوانيس! الطريق فوق النهر أصبح مظلمًا.' },
                    { speaker: 'student', text: 'أستطيع المساعدة. ماذا علينا أن نفعل؟' },
                    { speaker: 'guide', text: 'اجمع مجموعتين من اليراعات داخل كل فانوس، ثم اختر حجر الرقم الذي يساوي المجموع.' }
                ]
            }
        }
    };
}(window));