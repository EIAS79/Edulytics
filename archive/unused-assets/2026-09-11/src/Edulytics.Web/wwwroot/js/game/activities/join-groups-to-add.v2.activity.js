(function (global) {
    'use strict';

    global.EdulyticsGameActivities ||= {};

    global.EdulyticsGameActivities['join-groups-to-add-v2'] = {
        schemaVersion: 2,
        activityId: 'EDU-GAME-CAM-S1-JOIN-GROUPS-V2',
        title: 'Lantern Grove Adventure',
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
            guide: { name: 'Eddy' },
            student: { fallbackName: 'Student' }
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
            adaptive: { enabled: true, strongerHintAfter: 2 }
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
                studentFallback: 'Student',
                activityTitle: 'The Lantern Grove',
                round: (current, total) => `${current} / ${total}`,
                girlVoiceWord: 'Girl voice',
                boyVoiceWord: 'Boy voice',
                continue: 'Continue',
                skip: 'Skip',
                start: 'Start adventure',
                introTitle: 'The Lantern Grove',
                introSubtitle: 'Restore the glowing path by joining groups of fireflies.',
                readyLabel: 'Adventure briefing',
                readyPrompt: 'Listen to Eddy and get ready to help.',
                readyAnswer: 'The first mission starts when you are ready.',
                missionLabel: 'Lantern mission',
                taskTitle: 'Bring the two groups together',
                taskInstruction: q => `Move ${q.left} sunset fireflies and ${q.right} moonlight fireflies into the lantern.`,
                collectLabel: 'Join the groups',
                collectPrompt: 'Bring both groups into the lantern before choosing an answer.',
                answerLocked: 'Collect every firefly. Your stepping stones will appear here.',
                questionLabel: 'Solve to open the path',
                leftGroup: 'Sunset group',
                rightGroup: 'Moonlight group',
                dropHere: 'Lantern',
                fireflyLabel: 'Move firefly to the lantern',
                roundIntro: (q, ctx) => `${ctx.studentName}, bring ${q.left} fireflies from the sunset bush and ${q.right} from the moonlight bush into the lantern.`,
                answerInstruction: 'Choose the stepping stone with the total.',
                answerPrompt: q => `${q.left} plus ${q.right}. How many fireflies are glowing together? Choose the correct stepping stone.`,
                answerChoiceLabel: 'Answer',
                tryAgain: 'Almost. That stone did not open the path.',
                hints: [
                    () => 'Look inside the lantern and count each glowing light once.',
                    q => `Start with ${q.left}. Then count on ${q.right} more.`,
                    () => 'Watch each lantern light pulse. Count them slowly from one.'
                ],
                correct: q => `Excellent! ${q.left} plus ${q.right} equals ${q.sum}. The path is open!`,
                completeLabel: 'Adventure complete',
                complete: 'Lantern Grove restored!',
                completeCopy: (_q, ctx) => `${ctx.studentName}, you joined every group and restored the glowing path.`,
                completeSpeech: 'Amazing work! The whole Lantern Grove is glowing again.',
                rewardLabel: 'Your rewards',
                completeAnswer: 'All ten lantern missions are complete.',
                playAgain: 'Play again',
                introDialogue: [
                    { speaker: 'guide', text: ctx => `Welcome to Lantern Grove, ${ctx.studentName}! The path across the stream has gone dark.` },
                    { speaker: 'student', text: () => 'I can help. How do we bring the light back?' },
                    { speaker: 'guide', text: () => 'Join two groups of fireflies inside each lantern. Then choose the number stone that matches the total.' }
                ]
            },

            pl: {
                studentFallback: 'Uczeń',
                activityTitle: 'Gaj Lampionów',
                round: (current, total) => `${current} / ${total}`,
                girlVoiceWord: 'Głos dziewczynki',
                boyVoiceWord: 'Głos chłopca',
                continue: 'Dalej',
                skip: 'Pomiń',
                start: 'Rozpocznij przygodę',
                introTitle: 'Gaj Lampionów',
                introSubtitle: 'Przywróć świecącą ścieżkę, łącząc grupy świetlików.',
                readyLabel: 'Wprowadzenie',
                readyPrompt: 'Posłuchaj Eddy’ego i przygotuj się do pomocy.',
                readyAnswer: 'Pierwsza misja rozpocznie się, gdy będziesz gotowy.',
                missionLabel: 'Misja z lampionem',
                taskTitle: 'Połącz dwie grupy',
                taskInstruction: q => `Przenieś ${q.left} świetliki zachodu słońca i ${q.right} świetliki księżycowe do lampionu.`,
                collectLabel: 'Połącz grupy',
                collectPrompt: 'Przenieś obie grupy do lampionu, zanim wybierzesz odpowiedź.',
                answerLocked: 'Zbierz wszystkie świetliki. Kamienie z odpowiedziami pojawią się tutaj.',
                questionLabel: 'Rozwiąż i otwórz drogę',
                leftGroup: 'Grupa zachodu',
                rightGroup: 'Grupa księżyca',
                dropHere: 'Lampion',
                fireflyLabel: 'Przenieś świetlika do lampionu',
                roundIntro: (q, ctx) => `${ctx.studentName}, przenieś ${q.left} świetliki z krzaka zachodu i ${q.right} z krzaka księżyca do lampionu.`,
                answerInstruction: 'Wybierz kamień z właściwą sumą.',
                answerPrompt: q => `${q.left} plus ${q.right}. Ile świetlików świeci teraz razem? Wybierz właściwy kamień.`,
                answerChoiceLabel: 'Odpowiedź',
                tryAgain: 'Prawie. Ten kamień nie otworzył drogi.',
                hints: [
                    () => 'Spójrz do lampionu i policz każde świecące światełko tylko raz.',
                    q => `Zacznij od ${q.left}. Potem dolicz jeszcze ${q.right}.`,
                    () => 'Patrz na każde pulsujące światełko i licz powoli od jednego.'
                ],
                correct: q => `Świetnie! ${q.left} plus ${q.right} równa się ${q.sum}. Droga jest otwarta!`,
                completeLabel: 'Przygoda ukończona',
                complete: 'Gaj Lampionów znów świeci!',
                completeCopy: (_q, ctx) => `${ctx.studentName}, połączyłeś wszystkie grupy i przywróciłeś świecącą ścieżkę.`,
                completeSpeech: 'Wspaniała robota! Cały Gaj Lampionów znowu świeci.',
                rewardLabel: 'Twoje nagrody',
                completeAnswer: 'Wszystkie dziesięć misji zostało ukończonych.',
                playAgain: 'Zagraj ponownie',
                introDialogue: [
                    { speaker: 'guide', text: ctx => `Witaj w Gaju Lampionów, ${ctx.studentName}! Ścieżka przez strumień zgasła.` },
                    { speaker: 'student', text: () => 'Mogę pomóc. Jak przywrócimy światło?' },
                    { speaker: 'guide', text: () => 'Połącz dwie grupy świetlików w każdym lampionie. Potem wybierz kamień z liczbą równą sumie.' }
                ]
            },

            ar: {
                studentFallback: 'الطالب',
                activityTitle: 'بستان الفوانيس',
                round: (current, total) => `${current} / ${total}`,
                girlVoiceWord: 'صوت بنت',
                boyVoiceWord: 'صوت ولد',
                continue: 'متابعة',
                skip: 'تخطي',
                start: 'ابدأ المغامرة',
                introTitle: 'بستان الفوانيس',
                introSubtitle: 'أعد إضاءة الطريق عن طريق جمع مجموعات اليراعات.',
                readyLabel: 'بداية المغامرة',
                readyPrompt: 'استمع إلى Eddy واستعد للمساعدة.',
                readyAnswer: 'ستبدأ المهمة الأولى عندما تكون مستعدًا.',
                missionLabel: 'مهمة الفانوس',
                taskTitle: 'اجمع المجموعتين معًا',
                taskInstruction: q => `انقل ${q.left} من اليراعات الذهبية و${q.right} من اليراعات البنفسجية إلى الفانوس.`,
                collectLabel: 'اجمع المجموعتين',
                collectPrompt: 'انقل المجموعتين إلى الفانوس قبل اختيار الإجابة.',
                answerLocked: 'اجمع كل اليراعات. ستظهر أحجار الإجابة هنا.',
                questionLabel: 'حل وافتح الطريق',
                leftGroup: 'المجموعة الذهبية',
                rightGroup: 'المجموعة البنفسجية',
                dropHere: 'الفانوس',
                fireflyLabel: 'انقل اليراعة إلى الفانوس',
                roundIntro: (q, ctx) => `${ctx.studentName}، انقل ${q.left} يراعات من الشجيرة الذهبية و${q.right} من الشجيرة البنفسجية إلى الفانوس.`,
                answerInstruction: 'اختر حجر العبور الذي يحمل المجموع الصحيح.',
                answerPrompt: q => `${q.left} زائد ${q.right}. كم يراعة تضيء معًا الآن؟ اختر حجر العبور الصحيح.`,
                answerChoiceLabel: 'الإجابة',
                tryAgain: 'اقتربت. هذا الحجر لم يفتح الطريق.',
                hints: [
                    () => 'انظر داخل الفانوس وعدّ كل ضوء مرة واحدة فقط.',
                    q => `ابدأ بالعدد ${q.left}، ثم أضف ${q.right} بالعدّ إلى الأمام.`,
                    () => 'تابع كل ضوء عندما يومض، وعدّ ببطء من واحد.'
                ],
                correct: q => `ممتاز! ${q.left} زائد ${q.right} يساوي ${q.sum}. الطريق مفتوح الآن!`,
                completeLabel: 'اكتملت المغامرة',
                complete: 'أضاء بستان الفوانيس من جديد!',
                completeCopy: (_q, ctx) => `${ctx.studentName}، جمعت كل المجموعات وأعدت إضاءة الطريق.`,
                completeSpeech: 'عمل رائع! بستان الفوانيس كله مضيء من جديد.',
                rewardLabel: 'مكافآتك',
                completeAnswer: 'تم إكمال مهام الفوانيس العشر.',
                playAgain: 'العب مرة أخرى',
                introDialogue: [
                    { speaker: 'guide', text: ctx => `مرحبًا بك في بستان الفوانيس يا ${ctx.studentName}! الطريق فوق الجدول أصبح مظلمًا.` },
                    { speaker: 'student', text: () => 'أستطيع المساعدة. كيف نعيد الضوء؟' },
                    { speaker: 'guide', text: () => 'اجمع مجموعتين من اليراعات داخل كل فانوس، ثم اختر حجر الرقم الذي يساوي المجموع.' }
                ]
            }
        }
    };
}(window));