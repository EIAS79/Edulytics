(() => {
    const root = document.querySelector('[data-temporary-practice-preview]');
    if (!root) return;

    const tabs = Array.from(root.querySelectorAll('[data-preview-tab]'));
    const panels = Array.from(root.querySelectorAll('[data-preview-panel]'));
    const gameShell = root.querySelector('.kid-game-shell');
    if (!gameShell) return;

    const mascotAsset = '/images/public/edulytics-math-mascot-animation.png';
    const totalRounds = 10;
    let rounds = [];
    let roundIndex = 0;
    let movedCount = 0;
    let phase = 'collect';
    let locked = false;
    let soundOn = true;
    let audioContext = null;
    let currentSpeech = '';
    let preferredVoice = null;
    let speechLanguage = inferSpeechLanguage();
    let dragTokenId = null;
    let started = false;

    const strings = {
        en: {
            adventure: 'Edulytics Math Adventure',
            mission: 'Firefly Lantern Mission',
            counter: (i) => `${i + 1} of ${totalRounds}`,
            soundOn: '🔊 Sound on',
            soundOff: '🔇 Sound off',
            soundOnLabel: 'Turn game sounds off',
            soundOffLabel: 'Turn game sounds on',
            eyebrow: (i) => `Mission ${i + 1}`,
            heading: 'Help me light the lantern!',
            instruction: 'Move every firefly from both bushes into the lantern.',
            tip: 'Tap a firefly or drag it into the lantern.',
            left: 'Sunset bush',
            right: 'Moonlight bush',
            lantern: 'Lantern',
            joined: (moved, total) => `${moved} of ${total} joined`,
            firstSpeech: (q) => `Mission ${roundIndex + 1}. There are ${q.left} fireflies in one bush and ${q.right} in the other. Bring all of them into the lantern.`,
            collectSpeech: (remaining) => remaining === 1 ? 'One firefly left. Bring it to the lantern.' : `${remaining} fireflies are still waiting.`,
            ask: (q) => `${q.left} plus ${q.right}. How many fireflies are glowing together?`,
            question: (q) => `${q.left} + ${q.right} = ?`,
            answerTitle: 'How many are glowing together?',
            answerCopy: 'Count the fireflies inside the lantern, then choose your answer.',
            correct: (q) => `Brilliant! ${q.left} plus ${q.right} equals ${q.sum}. The lantern is shining!`,
            wrong: 'Good try. Count the glowing fireflies in the lantern once more.',
            check: 'Check my answer',
            clear: 'Clear',
            replay: 'Tap me to hear the mission again',
            completeTitle: 'Lantern adventure complete!',
            completeScore: '10 missions completed',
            completeCopy: 'You joined the groups and solved every addition mission.',
            playAgain: 'Play another round'
        },
        pl: {
            adventure: 'Edulytics Matematyczna Przygoda',
            mission: 'Misja ze świetlikami',
            counter: (i) => `${i + 1} z ${totalRounds}`,
            soundOn: '🔊 Dźwięk włączony',
            soundOff: '🔇 Dźwięk wyłączony',
            soundOnLabel: 'Wyłącz dźwięk gry',
            soundOffLabel: 'Włącz dźwięk gry',
            eyebrow: (i) => `Misja ${i + 1}`,
            heading: 'Pomóż mi rozświetlić lampion!',
            instruction: 'Przenieś wszystkie świetliki z obu krzaków do lampionu.',
            tip: 'Dotknij świetlika albo przeciągnij go do lampionu.',
            left: 'Krzak zachodu słońca',
            right: 'Krzak księżycowy',
            lantern: 'Lampion',
            joined: (moved, total) => `${moved} z ${total} połączonych`,
            firstSpeech: (q) => `Misja ${roundIndex + 1}. W jednym krzaku są ${q.left} świetliki, a w drugim ${q.right}. Przenieś wszystkie do lampionu.`,
            collectSpeech: (remaining) => remaining === 1 ? 'Został jeden świetlik. Przenieś go do lampionu.' : `Zostało ${remaining} świetlików.`,
            ask: (q) => `${q.left} plus ${q.right}. Ile świetlików świeci teraz razem?`,
            question: (q) => `${q.left} + ${q.right} = ?`,
            answerTitle: 'Ile świetlików świeci razem?',
            answerCopy: 'Policz świetliki w lampionie i wybierz odpowiedź.',
            correct: (q) => `Świetnie! ${q.left} plus ${q.right} równa się ${q.sum}. Lampion świeci!`,
            wrong: 'Dobra próba. Policz jeszcze raz świetliki w lampionie.',
            check: 'Sprawdź odpowiedź',
            clear: 'Wyczyść',
            replay: 'Dotknij mnie, aby usłyszeć misję ponownie',
            completeTitle: 'Przygoda z lampionem zakończona!',
            completeScore: '10 misji ukończonych',
            completeCopy: 'Połączyłeś grupy i rozwiązałeś wszystkie zadania z dodawania.',
            playAgain: 'Zagraj ponownie'
        }
    };

    function locale() {
        return speechLanguage.toLowerCase().startsWith('pl') ? strings.pl : strings.en;
    }

    function inferSpeechLanguage() {
        const htmlLanguage = (document.documentElement.lang || '').toLowerCase();
        if (htmlLanguage.startsWith('pl')) return 'pl-PL';
        const pageText = (root.textContent || '').toLowerCase();
        return /polish|polska|polski|podstawa/.test(pageText) ? 'pl-PL' : 'en-GB';
    }

    function createRounds() {
        const pairs = [];
        for (let left = 1; left <= 6; left++) {
            for (let right = 1; right <= 6; right++) {
                const sum = left + right;
                if (sum >= 3 && sum <= 10) pairs.push({ left, right, sum });
            }
        }
        shuffle(pairs);
        return pairs.slice(0, totalRounds);
    }

    function shuffle(items) {
        for (let i = items.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [items[i], items[j]] = [items[j], items[i]];
        }
        return items;
    }

    function installStyles() {
        if (document.getElementById('scenario-game-styles-v1')) return;
        const style = document.createElement('style');
        style.id = 'scenario-game-styles-v1';
        style.textContent = `
            .kid-game-shell.scenario-shell {
                --scene-x:0;
                --scene-y:0;
                position:relative;
                overflow:hidden;
                min-height:760px;
                border:6px solid #fff;
                border-radius:34px;
                background:#85d8f4;
                box-shadow:0 26px 72px rgba(24,76,99,.23);
                color:#17344f;
                isolation:isolate;
            }
            .scenario-world{position:absolute;inset:0;overflow:hidden;pointer-events:none;z-index:0;background:linear-gradient(180deg,#65c8f2 0%,#a9e9f9 42%,#ffe7a5 43%,#8ed171 59%,#4ba95e 100%)}
            .scenario-sky-glow{position:absolute;width:44vw;height:44vw;min-width:420px;min-height:420px;right:-12%;top:-22%;border-radius:50%;background:radial-gradient(circle,rgba(255,247,185,.95) 0 10%,rgba(255,218,112,.28) 29%,transparent 65%);animation:scenario-sun-pulse 7s ease-in-out infinite}
            .scenario-cloud{position:absolute;width:180px;height:56px;border-radius:999px;background:rgba(255,255,255,.88);filter:drop-shadow(0 10px 14px rgba(40,111,141,.11));}
            .scenario-cloud:before,.scenario-cloud:after{content:"";position:absolute;border-radius:50%;background:inherit}
            .scenario-cloud:before{width:74px;height:74px;left:35px;top:-34px}.scenario-cloud:after{width:92px;height:92px;right:24px;top:-46px}
            .scenario-cloud-a{top:15%;left:-210px;animation:scenario-cloud-a 22s linear infinite}
            .scenario-cloud-b{top:28%;right:-230px;transform:scale(.72);animation:scenario-cloud-b 27s linear infinite}
            .scenario-hill{position:absolute;bottom:8%;width:60%;height:220px;border-radius:50% 50% 0 0;background:rgba(49,139,77,.48);filter:blur(.1px)}
            .scenario-hill-a{left:-16%;animation:scenario-hill-a 8s ease-in-out infinite alternate}
            .scenario-hill-b{right:-18%;height:185px;background:rgba(35,126,73,.4);animation:scenario-hill-b 10s ease-in-out infinite alternate}
            .scenario-stars{position:absolute;inset:0;background-image:radial-gradient(circle,#fff8b0 0 2px,transparent 2.4px),radial-gradient(circle,#fff 0 1.4px,transparent 1.9px);background-size:94px 94px,143px 143px;background-position:0 0,38px 26px;opacity:.72;animation:scenario-stars-drift 16s linear infinite}
            .scenario-grass{position:absolute;left:-2%;right:-2%;bottom:-7px;height:90px;background:repeating-linear-gradient(86deg,transparent 0 13px,rgba(21,111,60,.32) 13px 17px,transparent 17px 28px);transform-origin:50% 100%;animation:scenario-grass-sway 3.6s ease-in-out infinite alternate}
            .scenario-topbar{position:relative;z-index:4;display:grid;grid-template-columns:minmax(0,1fr) auto;gap:18px;align-items:center;padding:22px 26px 10px}
            .scenario-brand{display:flex;gap:10px;align-items:center;flex-wrap:wrap}.scenario-kicker{margin:0;padding:7px 12px;border-radius:999px;background:rgba(255,255,255,.8);font-size:.78rem;font-weight:950;letter-spacing:.06em;text-transform:uppercase;color:#0e6c7b}
            .scenario-title{font-size:1.15rem;font-weight:950;color:#173d56}.scenario-actions{display:flex;gap:10px;align-items:center}.scenario-counter,.scenario-sound{min-height:44px;border:2px solid rgba(255,255,255,.82);border-radius:999px;background:rgba(255,255,255,.82);color:#174c60;font-weight:950;box-shadow:0 7px 20px rgba(41,102,124,.12)}
            .scenario-counter{padding:10px 15px;min-width:102px;text-align:center}.scenario-sound{padding:0 14px;cursor:pointer}
            .scenario-progress-wrap{position:relative;z-index:4;padding:0 26px 14px}.scenario-progress-track{height:15px;overflow:hidden;border:3px solid rgba(255,255,255,.86);border-radius:999px;background:rgba(255,255,255,.5)}
            .scenario-progress-fill{display:block;height:100%;width:0;border-radius:inherit;background:linear-gradient(90deg,#ffd15b,#ff9c55,#56ca8f);transition:width .5s ease;position:relative;overflow:hidden}.scenario-progress-fill:after{content:"";position:absolute;top:-3px;bottom:-3px;width:46px;left:-60px;background:linear-gradient(90deg,transparent,rgba(255,255,255,.92),transparent);transform:skewX(-18deg);animation:scenario-progress-shine 2.4s ease-in-out infinite}
            .scenario-stage{position:relative;z-index:3;display:grid;grid-template-columns:minmax(0,1fr) 265px;gap:20px;padding:10px 26px 28px;min-height:640px}
            .scenario-board{position:relative;overflow:hidden;min-height:610px;padding:20px;border:4px solid rgba(255,255,255,.86);border-radius:30px;background:rgba(255,255,255,.74);box-shadow:0 22px 48px rgba(30,83,105,.17);backdrop-filter:blur(3px)}
            .scenario-board:before{content:"";position:absolute;inset:auto 0 0;height:34%;background:linear-gradient(180deg,transparent,rgba(98,178,88,.15));pointer-events:none}
            .scenario-instruction{text-align:center;position:relative;z-index:3}.scenario-eyebrow{margin:0 0 5px;color:#168398;font-size:.82rem;font-weight:950;letter-spacing:.08em;text-transform:uppercase}.scenario-instruction h2{margin:0;color:#153f57;font-size:clamp(1.7rem,3vw,2.45rem);line-height:1.07}.scenario-instruction p{margin:7px 0 0;color:#58788c;font-weight:750}.scenario-tip{font-size:.88rem!important;color:#2e7f87!important}
            .scenario-playfield{position:relative;display:grid;grid-template-columns:minmax(0,1fr) 190px minmax(0,1fr);gap:16px;align-items:end;min-height:350px;margin-top:16px}
            .scenario-bush{position:relative;min-height:270px;border:0;border-radius:46% 46% 22px 22px;padding:45px 15px 16px;background:radial-gradient(circle at 50% 24%,#9be078 0 24%,#55b967 58%,#3a9857 100%);box-shadow:inset 0 -12px 0 rgba(26,107,61,.16),0 14px 26px rgba(31,91,66,.18);overflow:visible}
            .scenario-bush:before,.scenario-bush:after{content:"";position:absolute;border-radius:50%;background:#6bca6b;z-index:0}.scenario-bush:before{width:110px;height:96px;left:-13px;top:54px}.scenario-bush:after{width:118px;height:106px;right:-12px;top:45px}
            .scenario-bush-label{position:absolute;z-index:4;left:50%;top:10px;transform:translateX(-50%);white-space:nowrap;padding:7px 11px;border-radius:999px;background:#fff;color:#245a66;font-size:.78rem;font-weight:950;box-shadow:0 6px 14px rgba(35,82,94,.12)}
            .scenario-token-zone{position:relative;z-index:3;min-height:190px}
            .scenario-firefly{position:absolute;width:42px;height:42px;border:0;border-radius:50%;cursor:grab;background:radial-gradient(circle at 42% 38%,#fffbd0 0 13%,#ffe65d 15% 34%,#ffab4e 60%,#e66a42 100%);box-shadow:0 0 0 5px rgba(255,249,168,.23),0 0 26px rgba(255,223,81,.78);transition:opacity .18s ease,transform .16s ease;animation:scenario-firefly-float 2.1s ease-in-out infinite;touch-action:none}
            .scenario-firefly:before,.scenario-firefly:after{content:"";position:absolute;width:18px;height:10px;border-radius:50% 50% 45% 45%;background:rgba(255,255,255,.72);top:14px}.scenario-firefly:before{left:-11px;transform:rotate(-24deg)}.scenario-firefly:after{right:-11px;transform:rotate(24deg)}
            .scenario-firefly.is-right{background:radial-gradient(circle at 42% 38%,#fffbdc 0 13%,#d8c7ff 15% 34%,#9a82f4 60%,#6f54d9 100%);box-shadow:0 0 0 5px rgba(215,203,255,.23),0 0 26px rgba(153,128,244,.68)}
            .scenario-firefly:hover,.scenario-firefly:focus-visible{transform:scale(1.12);outline:3px solid rgba(255,255,255,.9);outline-offset:4px}.scenario-firefly.is-moving{opacity:0;pointer-events:none}
            .scenario-lantern-zone{align-self:center;position:relative;display:grid;place-items:center;min-height:300px;border-radius:32px;transition:transform .25s ease}
            .scenario-lantern-zone.is-drag-over{transform:scale(1.05)}.scenario-lantern{position:relative;width:150px;height:190px;border:7px solid #765133;border-radius:28px 28px 38px 38px;background:linear-gradient(180deg,rgba(255,248,198,.25),rgba(255,210,82,.2));box-shadow:inset 0 0 0 5px rgba(255,255,255,.4),0 18px 25px rgba(87,72,42,.2);overflow:hidden;transition:box-shadow .35s ease,background .35s ease,transform .35s ease}
            .scenario-lantern:before{content:"";position:absolute;width:74px;height:34px;border:8px solid #765133;border-bottom:0;border-radius:42px 42px 0 0;left:50%;top:-32px;transform:translateX(-50%)}.scenario-lantern:after{content:"";position:absolute;left:17px;right:17px;top:20px;bottom:20px;border:3px solid rgba(123,82,44,.32);border-radius:18px}
            .scenario-lantern.is-lit{background:radial-gradient(circle at 50% 56%,#fffad1 0 12%,#ffe16d 35%,rgba(255,165,54,.58) 72%,rgba(255,142,39,.28) 100%);box-shadow:inset 0 0 0 5px rgba(255,255,255,.5),0 0 42px rgba(255,215,69,.92),0 20px 28px rgba(87,72,42,.2);animation:scenario-lantern-glow 1.55s ease-in-out infinite}
            .scenario-lantern-fireflies{position:absolute;inset:18px;z-index:3}.scenario-lantern-dot{position:absolute;width:15px;height:15px;border-radius:50%;background:#fff8a5;box-shadow:0 0 16px #ffd74b;animation:scenario-lantern-dot 1.8s ease-in-out infinite}
            .scenario-lantern-label{position:absolute;bottom:10px;left:50%;transform:translateX(-50%);z-index:4;padding:6px 10px;border-radius:999px;background:rgba(255,255,255,.9);color:#674b30;font-size:.75rem;font-weight:950;white-space:nowrap}
            .scenario-join-count{position:absolute;top:8px;left:50%;transform:translateX(-50%);z-index:4;padding:7px 10px;border-radius:999px;background:#174c60;color:#fff;font-size:.74rem;font-weight:950;white-space:nowrap}
            .scenario-answer-panel{position:relative;z-index:4;margin-top:12px;padding:15px;border-radius:24px;background:rgba(255,255,255,.91);box-shadow:0 12px 25px rgba(39,83,99,.12);opacity:0;transform:translateY(18px) scale(.98);pointer-events:none;transition:opacity .35s ease,transform .35s ease}
            .scenario-answer-panel.is-ready{opacity:1;transform:none;pointer-events:auto}.scenario-answer-title{text-align:center}.scenario-answer-title strong{display:block;color:#18495f;font-size:1.05rem}.scenario-answer-title span{display:block;margin-top:3px;color:#628092;font-size:.88rem}
            .scenario-equation{display:flex;align-items:center;justify-content:center;gap:12px;margin:12px 0}.scenario-equation-text{font-size:clamp(2rem,4vw,3.2rem);font-weight:950;color:#173d56}.scenario-answer-input{width:108px;min-height:64px;border:4px solid #79c7d2;border-radius:18px;background:#fff;text-align:center;font-size:2rem;font-weight:950;color:#173d56}
            .scenario-keypad{display:grid;grid-template-columns:repeat(6,1fr);gap:8px}.scenario-key{min-height:48px;border:0;border-radius:14px;background:linear-gradient(180deg,#fff,#edf8fb);color:#1d556a;font-weight:950;box-shadow:0 5px 0 #c7e4ea;cursor:pointer}.scenario-key:active{transform:translateY(3px);box-shadow:0 2px 0 #c7e4ea}.scenario-key.is-tool{background:linear-gradient(180deg,#fff3d8,#ffe5ae);color:#875b1e}
            .scenario-check{width:100%;min-height:52px;margin-top:10px;border:0;border-radius:16px;background:linear-gradient(180deg,#36c58b,#1fa76f);color:#fff;font-weight:950;cursor:pointer;box-shadow:0 7px 0 #16815a}.scenario-check:disabled{opacity:.5;cursor:not-allowed;box-shadow:none}
            .scenario-feedback{min-height:46px;margin-top:10px;padding:11px 13px;border-radius:14px;font-weight:900;text-align:center}.scenario-feedback:empty{display:none}.scenario-feedback.is-correct{background:#dff9e9;color:#1b7c51}.scenario-feedback.is-wrong{background:#fff0d6;color:#9b621d}
            .scenario-mascot-column{position:relative;display:flex;flex-direction:column;justify-content:center;align-items:center;gap:12px;min-width:0}.scenario-speech{position:relative;width:100%;padding:16px;border:3px solid rgba(255,255,255,.92);border-radius:22px;background:rgba(255,255,255,.92);box-shadow:0 12px 25px rgba(29,78,97,.14);color:#234d60;font-weight:800;line-height:1.45}.scenario-speech:after{content:"";position:absolute;right:44px;bottom:-17px;width:28px;height:28px;background:#fff;transform:rotate(45deg);border-right:3px solid rgba(255,255,255,.92);border-bottom:3px solid rgba(255,255,255,.92)}
            .scenario-mascot-wrap{position:relative;width:min(285px,100%);height:390px;cursor:pointer}.scenario-mascot{position:relative;z-index:2;width:100%;height:100%;object-fit:contain;object-position:center bottom;filter:drop-shadow(0 18px 17px rgba(23,72,88,.18));user-select:none;-webkit-user-drag:none}
            .scenario-mouth{position:absolute;z-index:5;left:51%;top:40.5%;width:18px;height:7px;border-radius:50%;background:#4d2c35;opacity:0;transform:translate(-50%,-50%);pointer-events:none}.scenario-mascot-wrap.is-speaking .scenario-mouth{opacity:.9;animation:scenario-mouth-talk .18s ease-in-out infinite alternate}
            .scenario-talk-ring{position:absolute;z-index:1;left:50%;top:45%;width:175px;height:175px;border:4px solid rgba(27,155,180,.28);border-radius:50%;transform:translate(-50%,-50%) scale(.55);opacity:0}.scenario-mascot-wrap.is-speaking .scenario-talk-ring{animation:scenario-talk-ring 1.2s ease-out infinite}.scenario-mascot-wrap.is-speaking .scenario-mascot{animation:scenario-head-talk 1.1s ease-in-out infinite}
            .scenario-replay{font-size:.78rem;color:#2c6e7a;text-align:center;font-weight:800}
            .scenario-fly-clone{position:fixed;z-index:9999;width:42px;height:42px;border-radius:50%;pointer-events:none;box-shadow:0 0 28px rgba(255,224,83,.9);transition:transform .48s cubic-bezier(.2,.8,.2,1),opacity .48s ease}
            .scenario-burst{position:absolute;z-index:10;width:12px;height:12px;border-radius:50%;background:#fff4a2;box-shadow:0 0 16px #ffd44f;pointer-events:none;animation:scenario-burst .8s ease-out forwards}
            .scenario-finish{position:absolute;z-index:8;inset:0;display:grid;place-items:center;padding:24px;background:rgba(21,77,94,.28);backdrop-filter:blur(5px)}.scenario-finish-card{width:min(500px,100%);padding:26px;border:5px solid #fff;border-radius:30px;background:linear-gradient(180deg,#fff9dd,#fff);text-align:center;box-shadow:0 26px 60px rgba(24,70,89,.25);animation:scenario-finish-pop .65s cubic-bezier(.17,.89,.32,1.28)}.scenario-finish-card img{width:180px;height:180px;object-fit:contain}.scenario-finish-card h2{margin:8px 0;color:#173d56}.scenario-finish-score{font-size:1.4rem;font-weight:950;color:#0c8a72}.scenario-restart{min-height:50px;padding:0 22px;border:0;border-radius:16px;background:#0b8193;color:#fff;font-weight:950;cursor:pointer}
            @keyframes scenario-cloud-a{to{transform:translateX(calc(100vw + 470px))}}@keyframes scenario-cloud-b{to{transform:translateX(calc(-100vw - 470px)) scale(.72)}}@keyframes scenario-sun-pulse{0%,100%{transform:scale(1)}50%{transform:scale(1.08) translateY(10px)}}@keyframes scenario-stars-drift{to{background-position:94px 46px,181px 82px}}@keyframes scenario-hill-a{from{transform:translateX(-18px) scale(1)}to{transform:translateX(32px) scale(1.05)}}@keyframes scenario-hill-b{from{transform:translateX(20px) scale(1)}to{transform:translateX(-30px) scale(1.06)}}@keyframes scenario-grass-sway{from{transform:skewX(-1.4deg)}to{transform:skewX(1.4deg)}}@keyframes scenario-progress-shine{0%,35%{left:-60px}75%,100%{left:110%}}@keyframes scenario-firefly-float{0%,100%{translate:0 0;rotate:-3deg}50%{translate:0 -9px;rotate:5deg}}@keyframes scenario-lantern-glow{0%,100%{transform:scale(1);filter:brightness(1)}50%{transform:scale(1.025);filter:brightness(1.08)}}@keyframes scenario-lantern-dot{0%,100%{transform:scale(.7);opacity:.65}50%{transform:scale(1.2);opacity:1}}@keyframes scenario-mouth-talk{from{height:5px;width:17px}to{height:12px;width:14px}}@keyframes scenario-talk-ring{0%{opacity:.8;transform:translate(-50%,-50%) scale(.55)}100%{opacity:0;transform:translate(-50%,-50%) scale(1.35)}}@keyframes scenario-head-talk{0%,100%{transform:rotate(0deg)}35%{transform:rotate(-1.2deg)}70%{transform:rotate(1deg)}}@keyframes scenario-burst{from{opacity:1;transform:translate(0,0) scale(1)}to{opacity:0;transform:translate(var(--bx),var(--by)) scale(.2)}}@keyframes scenario-finish-pop{from{opacity:0;transform:translateY(30px) scale(.92)}to{opacity:1;transform:none}}
            @media(max-width:900px){.scenario-stage{grid-template-columns:minmax(0,1fr) 210px}.scenario-mascot-wrap{height:310px}.scenario-playfield{grid-template-columns:minmax(0,1fr) 150px minmax(0,1fr)}.scenario-lantern{width:128px;height:170px}}
            @media(max-width:720px){.kid-game-shell.scenario-shell{border-width:4px;border-radius:24px}.scenario-topbar{grid-template-columns:1fr;padding:16px 16px 9px}.scenario-actions{justify-content:space-between}.scenario-progress-wrap{padding:0 16px 12px}.scenario-stage{grid-template-columns:1fr;padding:8px 14px 18px}.scenario-board{min-height:0;padding:16px 10px}.scenario-mascot-column{display:grid;grid-template-columns:1fr 150px;gap:10px}.scenario-mascot-wrap{width:150px;height:190px}.scenario-playfield{grid-template-columns:1fr 120px 1fr;gap:8px;min-height:315px}.scenario-bush{min-height:230px;padding-inline:8px}.scenario-firefly{width:36px;height:36px}.scenario-lantern{width:112px;height:150px}.scenario-keypad{grid-template-columns:repeat(4,1fr)}}
            @media(max-width:510px){.scenario-title{width:100%}.scenario-playfield{grid-template-columns:1fr 92px 1fr;gap:5px}.scenario-bush{min-height:210px}.scenario-bush-label{font-size:.68rem;white-space:normal;text-align:center;width:92%}.scenario-firefly{width:31px;height:31px}.scenario-firefly:before,.scenario-firefly:after{width:13px;height:8px;top:10px}.scenario-firefly:before{left:-8px}.scenario-firefly:after{right:-8px}.scenario-lantern{width:84px;height:126px;border-width:5px}.scenario-lantern-label{font-size:.65rem}.scenario-join-count{font-size:.62rem}.scenario-mascot-column{grid-template-columns:1fr 120px}.scenario-mascot-wrap{width:120px;height:155px}.scenario-speech{font-size:.86rem;padding:12px}.scenario-answer-input{width:92px;min-height:56px;font-size:1.7rem}.scenario-key{min-height:45px}}
            @media(prefers-reduced-motion:reduce){.scenario-world *, .scenario-progress-fill:after, .scenario-firefly, .scenario-lantern.is-lit, .scenario-mascot-wrap.is-speaking .scenario-mouth, .scenario-mascot-wrap.is-speaking .scenario-talk-ring, .scenario-mascot-wrap.is-speaking .scenario-mascot{animation:none!important}}
        `;
        document.head.appendChild(style);
    }

    function buildGame() {
        const t = locale();
        gameShell.className = 'kid-game-shell scenario-shell';
        gameShell.innerHTML = `
            <div class="scenario-world" aria-hidden="true">
                <div class="scenario-stars"></div>
                <div class="scenario-sky-glow"></div>
                <div class="scenario-cloud scenario-cloud-a"></div>
                <div class="scenario-cloud scenario-cloud-b"></div>
                <div class="scenario-hill scenario-hill-a"></div>
                <div class="scenario-hill scenario-hill-b"></div>
                <div class="scenario-grass"></div>
            </div>
            <header class="scenario-topbar">
                <div class="scenario-brand">
                    <p class="scenario-kicker">✦ ${t.adventure}</p>
                    <span class="scenario-title">${t.mission}</span>
                </div>
                <div class="scenario-actions">
                    <div class="scenario-counter" data-scenario-counter>${t.counter(0)}</div>
                    <button class="scenario-sound" type="button" data-scenario-sound aria-pressed="true" aria-label="${t.soundOnLabel}">${t.soundOn}</button>
                </div>
            </header>
            <div class="scenario-progress-wrap">
                <div class="scenario-progress-track" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0" data-scenario-progress-track>
                    <span class="scenario-progress-fill" data-scenario-progress></span>
                </div>
            </div>
            <div class="scenario-stage">
                <main class="scenario-board">
                    <div class="scenario-instruction">
                        <p class="scenario-eyebrow" data-scenario-eyebrow>${t.eyebrow(0)}</p>
                        <h2>${t.heading}</h2>
                        <p>${t.instruction}</p>
                        <p class="scenario-tip">${t.tip}</p>
                    </div>
                    <div class="scenario-playfield">
                        <section class="scenario-bush" aria-label="${t.left}">
                            <span class="scenario-bush-label" data-scenario-left-label>${t.left}</span>
                            <div class="scenario-token-zone" data-scenario-left></div>
                        </section>
                        <div class="scenario-lantern-zone" data-scenario-dropzone>
                            <div class="scenario-join-count" data-scenario-join-count></div>
                            <div class="scenario-lantern" data-scenario-lantern>
                                <div class="scenario-lantern-fireflies" data-scenario-lantern-fireflies></div>
                                <span class="scenario-lantern-label">${t.lantern}</span>
                            </div>
                        </div>
                        <section class="scenario-bush" aria-label="${t.right}">
                            <span class="scenario-bush-label" data-scenario-right-label>${t.right}</span>
                            <div class="scenario-token-zone" data-scenario-right></div>
                        </section>
                    </div>
                    <section class="scenario-answer-panel" data-scenario-answer-panel aria-live="polite">
                        <div class="scenario-answer-title">
                            <strong>${t.answerTitle}</strong>
                            <span>${t.answerCopy}</span>
                        </div>
                        <div class="scenario-equation">
                            <span class="scenario-equation-text" data-scenario-equation></span>
                            <input class="scenario-answer-input" data-scenario-answer inputmode="numeric" maxlength="2" autocomplete="off" aria-label="${t.answerTitle}" />
                        </div>
                        <div class="scenario-keypad" data-scenario-keypad></div>
                        <button class="scenario-check" type="button" data-scenario-check disabled>${t.check}</button>
                        <div class="scenario-feedback" data-scenario-feedback aria-live="polite"></div>
                    </section>
                </main>
                <aside class="scenario-mascot-column" aria-label="Edulytics helper">
                    <div class="scenario-speech" data-scenario-speech aria-live="polite"></div>
                    <div class="scenario-mascot-wrap" data-scenario-mascot-wrap role="button" tabindex="0" aria-label="${t.replay}">
                        <div class="scenario-talk-ring" aria-hidden="true"></div>
                        <img class="scenario-mascot" src="${mascotAsset}" alt="Edulytics cartoon character" draggable="false" />
                        <span class="scenario-mouth" aria-hidden="true"></span>
                    </div>
                    <div class="scenario-replay">${t.replay}</div>
                </aside>
            </div>
            <div class="scenario-finish" data-scenario-finish hidden>
                <div class="scenario-finish-card">
                    <img src="${mascotAsset}" alt="Edulytics cartoon character celebrating" />
                    <h2>${t.completeTitle}</h2>
                    <div class="scenario-finish-score">${t.completeScore}</div>
                    <p>${t.completeCopy}</p>
                    <button class="scenario-restart" type="button" data-scenario-restart>${t.playAgain}</button>
                </div>
            </div>
        `;

        const keypad = gameShell.querySelector('[data-scenario-keypad]');
        for (let digit = 1; digit <= 9; digit++) {
            keypad.insertAdjacentHTML('beforeend', `<button class="scenario-key" type="button" data-scenario-key="${digit}">${digit}</button>`);
        }
        keypad.insertAdjacentHTML('beforeend', `<button class="scenario-key is-tool" type="button" data-scenario-key="clear">${t.clear}</button>`);
        keypad.insertAdjacentHTML('beforeend', `<button class="scenario-key" type="button" data-scenario-key="0">0</button>`);
        keypad.insertAdjacentHTML('beforeend', `<button class="scenario-key is-tool" type="button" data-scenario-key="backspace">⌫</button>`);

        bindGameEvents();
    }

    function bindGameEvents() {
        const soundButton = gameShell.querySelector('[data-scenario-sound]');
        const mascotWrap = gameShell.querySelector('[data-scenario-mascot-wrap]');
        const dropzone = gameShell.querySelector('[data-scenario-dropzone]');
        const answer = gameShell.querySelector('[data-scenario-answer]');
        const check = gameShell.querySelector('[data-scenario-check]');
        const restart = gameShell.querySelector('[data-scenario-restart]');

        soundButton.addEventListener('click', toggleSound);
        mascotWrap.addEventListener('click', () => speak(currentSpeech));
        mascotWrap.addEventListener('keydown', event => {
            if (event.key === 'Enter' || event.key === ' ') {
                event.preventDefault();
                speak(currentSpeech);
            }
        });

        gameShell.querySelectorAll('[data-scenario-key]').forEach(button => {
            button.addEventListener('click', () => {
                if (phase !== 'answer' || locked) return;
                const key = button.dataset.scenarioKey;
                if (key === 'clear') answer.value = '';
                else if (key === 'backspace') answer.value = answer.value.slice(0, -1);
                else if (answer.value.length < 2) answer.value += key;
                check.disabled = answer.value.length === 0;
                tone('tap');
                answer.focus();
            });
        });

        answer.addEventListener('input', () => {
            answer.value = answer.value.replace(/\D/g, '').slice(0, 2);
            check.disabled = answer.value.length === 0;
        });
        answer.addEventListener('keydown', event => {
            if (event.key === 'Enter' && !check.disabled) checkAnswer();
        });
        check.addEventListener('click', checkAnswer);
        restart.addEventListener('click', () => {
            rounds = createRounds();
            roundIndex = 0;
            gameShell.querySelector('[data-scenario-finish]').hidden = true;
            renderRound();
        });

        dropzone.addEventListener('dragover', event => {
            if (!dragTokenId || phase !== 'collect') return;
            event.preventDefault();
            dropzone.classList.add('is-drag-over');
        });
        dropzone.addEventListener('dragleave', () => dropzone.classList.remove('is-drag-over'));
        dropzone.addEventListener('drop', event => {
            event.preventDefault();
            dropzone.classList.remove('is-drag-over');
            if (dragTokenId) moveFireflyById(dragTokenId);
            dragTokenId = null;
        });
    }

    function fireflyPositions(count, side) {
        const left = [[12,18],[50,8],[72,30],[28,48],[61,58],[8,68],[43,78],[75,76],[20,88],[57,91]];
        const right = [[65,16],[26,8],[8,34],[48,43],[22,61],[70,61],[42,76],[7,81],[62,87],[31,92]];
        return (side === 'right' ? right : left).slice(0, count);
    }

    function renderRound() {
        const q = rounds[roundIndex];
        if (!q) return finishGame();

        const t = locale();
        movedCount = 0;
        phase = 'collect';
        locked = false;

        const leftZone = gameShell.querySelector('[data-scenario-left]');
        const rightZone = gameShell.querySelector('[data-scenario-right]');
        const lanternDots = gameShell.querySelector('[data-scenario-lantern-fireflies]');
        const lantern = gameShell.querySelector('[data-scenario-lantern]');
        const answerPanel = gameShell.querySelector('[data-scenario-answer-panel]');
        const answer = gameShell.querySelector('[data-scenario-answer]');
        const check = gameShell.querySelector('[data-scenario-check]');
        const feedback = gameShell.querySelector('[data-scenario-feedback]');

        leftZone.innerHTML = '';
        rightZone.innerHTML = '';
        lanternDots.innerHTML = '';
        lantern.classList.remove('is-lit');
        answerPanel.classList.remove('is-ready');
        answer.value = '';
        check.disabled = true;
        feedback.textContent = '';
        feedback.className = 'scenario-feedback';

        gameShell.querySelector('[data-scenario-counter]').textContent = t.counter(roundIndex);
        gameShell.querySelector('[data-scenario-eyebrow]').textContent = t.eyebrow(roundIndex);
        gameShell.querySelector('[data-scenario-left-label]').textContent = `${t.left} · ${q.left}`;
        gameShell.querySelector('[data-scenario-right-label]').textContent = `${t.right} · ${q.right}`;
        gameShell.querySelector('[data-scenario-equation]').textContent = t.question(q);
        updateProgress();
        updateJoinCount();

        renderFireflies(leftZone, q.left, 'left');
        renderFireflies(rightZone, q.right, 'right');

        setSpeech(t.firstSpeech(q), true);
    }

    function renderFireflies(zone, count, side) {
        const positions = fireflyPositions(count, side);
        positions.forEach((position, i) => {
            const button = document.createElement('button');
            button.type = 'button';
            button.className = `scenario-firefly${side === 'right' ? ' is-right' : ''}`;
            button.style.left = `${position[0]}%`;
            button.style.top = `${position[1]}%`;
            button.style.animationDelay = `${-(i * 0.23)}s`;
            button.dataset.fireflyId = `${side}-${i}`;
            button.dataset.side = side;
            button.draggable = true;
            button.setAttribute('aria-label', locale().tip);
            button.addEventListener('click', () => moveFirefly(button));
            button.addEventListener('dragstart', event => {
                dragTokenId = button.dataset.fireflyId;
                event.dataTransfer.effectAllowed = 'move';
                event.dataTransfer.setData('text/plain', dragTokenId);
            });
            button.addEventListener('dragend', () => {
                dragTokenId = null;
                gameShell.querySelector('[data-scenario-dropzone]').classList.remove('is-drag-over');
            });
            zone.appendChild(button);
        });
    }

    function moveFireflyById(id) {
        const escaped = window.CSS && CSS.escape ? CSS.escape(id) : id.replace(/[^a-zA-Z0-9_-]/g, '');
        const token = gameShell.querySelector(`[data-firefly-id="${escaped}"]`);
        if (token) moveFirefly(token);
    }

    function moveFirefly(token) {
        if (phase !== 'collect' || locked || token.classList.contains('is-moving')) return;
        token.classList.add('is-moving');
        animateToLantern(token);

        window.setTimeout(() => {
            token.remove();
            addLanternDot();
            movedCount++;
            updateJoinCount();
            tone('fly');

            const q = rounds[roundIndex];
            if (movedCount >= q.sum) {
                phase = 'answer';
                const lantern = gameShell.querySelector('[data-scenario-lantern]');
                lantern.classList.add('is-lit');
                burst(lantern, 16);
                const answerPanel = gameShell.querySelector('[data-scenario-answer-panel]');
                answerPanel.classList.add('is-ready');
                window.setTimeout(() => gameShell.querySelector('[data-scenario-answer]').focus(), 350);
                setSpeech(locale().ask(q), true);
                tone('join');
            } else if (movedCount === Math.ceil(q.sum / 2)) {
                setSpeech(locale().collectSpeech(q.sum - movedCount), false);
            }
        }, 430);
    }

    function animateToLantern(token) {
        const start = token.getBoundingClientRect();
        const lantern = gameShell.querySelector('[data-scenario-lantern]').getBoundingClientRect();
        const clone = document.createElement('div');
        clone.className = 'scenario-fly-clone';
        clone.style.left = `${start.left}px`;
        clone.style.top = `${start.top}px`;
        clone.style.background = token.classList.contains('is-right')
            ? 'radial-gradient(circle,#fffbdc 0 13%,#d8c7ff 15% 34%,#9a82f4 60%,#6f54d9 100%)'
            : 'radial-gradient(circle,#fffbd0 0 13%,#ffe65d 15% 34%,#ffab4e 60%,#e66a42 100%)';
        document.body.appendChild(clone);
        const dx = lantern.left + lantern.width / 2 - (start.left + start.width / 2);
        const dy = lantern.top + lantern.height / 2 - (start.top + start.height / 2);
        requestAnimationFrame(() => {
            clone.style.transform = `translate(${dx}px,${dy}px) scale(.45) rotate(240deg)`;
            clone.style.opacity = '.15';
        });
        window.setTimeout(() => clone.remove(), 520);
    }

    function addLanternDot() {
        const field = gameShell.querySelector('[data-scenario-lantern-fireflies]');
        const dot = document.createElement('span');
        dot.className = 'scenario-lantern-dot';
        const i = field.children.length;
        const positions = [[16,20],[51,13],[72,28],[30,43],[62,50],[12,61],[44,69],[75,68],[25,82],[59,85]];
        const p = positions[i % positions.length];
        dot.style.left = `${p[0]}%`;
        dot.style.top = `${p[1]}%`;
        dot.style.animationDelay = `${-(i * .17)}s`;
        field.appendChild(dot);
    }

    function updateJoinCount() {
        const q = rounds[roundIndex];
        if (!q) return;
        gameShell.querySelector('[data-scenario-join-count]').textContent = locale().joined(movedCount, q.sum);
    }

    function updateProgress() {
        const percent = Math.round((roundIndex / totalRounds) * 100);
        const fill = gameShell.querySelector('[data-scenario-progress]');
        const track = gameShell.querySelector('[data-scenario-progress-track]');
        fill.style.width = `${percent}%`;
        track.setAttribute('aria-valuenow', String(percent));
    }

    function checkAnswer() {
        if (phase !== 'answer' || locked) return;
        const q = rounds[roundIndex];
        const answer = gameShell.querySelector('[data-scenario-answer]');
        const feedback = gameShell.querySelector('[data-scenario-feedback]');
        const value = Number.parseInt(answer.value, 10);
        if (!Number.isFinite(value)) return;

        if (value === q.sum) {
            locked = true;
            phase = 'celebrate';
            feedback.textContent = `✓ ${locale().correct(q)}`;
            feedback.className = 'scenario-feedback is-correct';
            setSpeech(locale().correct(q), true);
            tone('correct');
            const lantern = gameShell.querySelector('[data-scenario-lantern]');
            burst(lantern, 28);
            updateProgressAfterCorrect();
            window.setTimeout(() => {
                roundIndex++;
                if (roundIndex >= totalRounds) finishGame();
                else renderRound();
            }, 1450);
        } else {
            feedback.textContent = locale().wrong;
            feedback.className = 'scenario-feedback is-wrong';
            answer.value = '';
            gameShell.querySelector('[data-scenario-check]').disabled = true;
            setSpeech(locale().wrong, true);
            tone('wrong');
            window.setTimeout(() => answer.focus(), 120);
        }
    }

    function updateProgressAfterCorrect() {
        const percent = Math.round(((roundIndex + 1) / totalRounds) * 100);
        const fill = gameShell.querySelector('[data-scenario-progress]');
        const track = gameShell.querySelector('[data-scenario-progress-track]');
        fill.style.width = `${percent}%`;
        track.setAttribute('aria-valuenow', String(percent));
    }

    function burst(target, count) {
        const board = gameShell.querySelector('.scenario-board');
        if (!board || !target) return;
        const boardRect = board.getBoundingClientRect();
        const rect = target.getBoundingClientRect();
        const x = rect.left - boardRect.left + rect.width / 2;
        const y = rect.top - boardRect.top + rect.height / 2;
        for (let i = 0; i < count; i++) {
            const particle = document.createElement('span');
            particle.className = 'scenario-burst';
            particle.style.left = `${x}px`;
            particle.style.top = `${y}px`;
            const angle = (Math.PI * 2 * i) / count + Math.random() * .25;
            const distance = 45 + Math.random() * 105;
            particle.style.setProperty('--bx', `${Math.cos(angle) * distance}px`);
            particle.style.setProperty('--by', `${Math.sin(angle) * distance}px`);
            board.appendChild(particle);
            window.setTimeout(() => particle.remove(), 900);
        }
    }

    function finishGame() {
        phase = 'finish';
        locked = true;
        updateProgressAfterCorrect();
        const finish = gameShell.querySelector('[data-scenario-finish]');
        finish.hidden = false;
        setSpeech(locale().completeCopy, true);
        tone('finish');
    }

    function setSpeech(text, readAloud) {
        currentSpeech = text;
        const bubble = gameShell.querySelector('[data-scenario-speech]');
        if (bubble) bubble.textContent = text;
        if (readAloud) speak(text);
    }

    function refreshVoices() {
        if (!('speechSynthesis' in window)) return;
        const voices = window.speechSynthesis.getVoices();
        const prefix = speechLanguage.slice(0, 2).toLowerCase();
        const candidates = voices.filter(voice => (voice.lang || '').toLowerCase().startsWith(prefix));
        const preferredNames = /pl/i.test(speechLanguage)
            ? [/zosia/i,/paulina/i,/ewa/i,/google.*pol/i,/microsoft.*pol/i]
            : [/samantha/i,/serena/i,/sonia/i,/google.*uk/i,/microsoft.*english.*united kingdom/i,/daniel/i];
        preferredVoice = candidates.find(v => preferredNames.some(pattern => pattern.test(v.name))) || candidates[0] || null;
    }

    function speak(text) {
        if (!soundOn || !text || !('speechSynthesis' in window)) return;
        window.speechSynthesis.cancel();
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = speechLanguage;
        utterance.rate = speechLanguage.toLowerCase().startsWith('pl') ? .88 : .92;
        utterance.pitch = 1.14;
        utterance.volume = 1;
        if (preferredVoice) utterance.voice = preferredVoice;
        const mascotWrap = gameShell.querySelector('[data-scenario-mascot-wrap]');
        utterance.onstart = () => mascotWrap?.classList.add('is-speaking');
        utterance.onend = () => mascotWrap?.classList.remove('is-speaking');
        utterance.onerror = () => mascotWrap?.classList.remove('is-speaking');
        window.speechSynthesis.speak(utterance);
    }

    function toggleSound() {
        soundOn = !soundOn;
        const t = locale();
        const button = gameShell.querySelector('[data-scenario-sound]');
        button.textContent = soundOn ? t.soundOn : t.soundOff;
        button.setAttribute('aria-pressed', String(soundOn));
        button.setAttribute('aria-label', soundOn ? t.soundOnLabel : t.soundOffLabel);
        if (!soundOn && 'speechSynthesis' in window) window.speechSynthesis.cancel();
        if (soundOn) {
            tone('tap');
            speak(currentSpeech);
        }
    }

    function ensureAudio() {
        if (!soundOn) return null;
        if (!audioContext) {
            const AudioCtor = window.AudioContext || window.webkitAudioContext;
            if (!AudioCtor) return null;
            audioContext = new AudioCtor();
        }
        if (audioContext.state === 'suspended') audioContext.resume();
        return audioContext;
    }

    function tone(kind) {
        if (!soundOn) return;
        const ctx = ensureAudio();
        if (!ctx) return;
        const now = ctx.currentTime;
        const patterns = {
            tap: [[420,.04,.05]],
            fly: [[620,.05,.06],[840,.06,.04]],
            join: [[460,.08,.07],[620,.09,.06],[820,.12,.05]],
            correct: [[520,.08,.08],[660,.09,.07],[820,.14,.06]],
            wrong: [[260,.09,.05],[215,.11,.04]],
            finish: [[440,.08,.07],[554,.08,.07],[659,.09,.07],[880,.18,.06]]
        };
        (patterns[kind] || patterns.tap).forEach((part, i) => {
            const osc = ctx.createOscillator();
            const gain = ctx.createGain();
            osc.type = kind === 'wrong' ? 'triangle' : 'sine';
            osc.frequency.value = part[0];
            gain.gain.setValueAtTime(0.0001, now + i * .07);
            gain.gain.exponentialRampToValueAtTime(part[2], now + i * .07 + .01);
            gain.gain.exponentialRampToValueAtTime(0.0001, now + i * .07 + part[1]);
            osc.connect(gain);
            gain.connect(ctx.destination);
            osc.start(now + i * .07);
            osc.stop(now + i * .07 + part[1] + .03);
        });
    }

    function activateTab(name) {
        tabs.forEach(tab => {
            const active = tab.dataset.previewTab === name;
            tab.classList.toggle('is-active', active);
            tab.setAttribute('aria-selected', String(active));
        });
        panels.forEach(panel => {
            panel.hidden = panel.dataset.previewPanel !== name;
        });
        if (name === 'practice' && !started) {
            started = true;
            rounds = createRounds();
            installStyles();
            buildGame();
            renderRound();
        }
    }

    tabs.forEach(tab => tab.addEventListener('click', () => activateTab(tab.dataset.previewTab)));

    if ('speechSynthesis' in window) {
        window.speechSynthesis.addEventListener?.('voiceschanged', refreshVoices);
        window.setTimeout(refreshVoices, 250);
    }
})();