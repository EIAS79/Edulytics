(() => {
    const root = document.querySelector('[data-temporary-practice-preview]');
    if (!root) return;

    const tabs = Array.from(root.querySelectorAll('[data-preview-tab]'));
    const panels = Array.from(root.querySelectorAll('[data-preview-panel]'));
    const shell = root.querySelector('.kid-game-shell');
    if (!shell) return;

    const mascotAsset = '/images/public/edulytics-math-mascot-animation.png';
    const totalRounds = 10;
    let rounds = [];
    let roundIndex = 0;
    let collected = 0;
    let phase = 'intro';
    let locked = false;
    let soundOn = true;
    let speechLanguage = inferLanguage();
    let preferredVoice = null;
    let currentSpeech = '';
    let audioContext = null;
    let started = false;
    let dragId = null;

    const copy = {
        en: {
            title: 'Join Groups Adventure',
            introTitle: 'The Lantern Path',
            introCopy: 'Two groups of fireflies are waiting in the meadow. Bring them together, then choose the right stepping stone to help Eddy cross the stream.',
            start: 'Start adventure',
            mission: i => `Mission ${i + 1}`,
            counter: i => `${i + 1} / ${totalRounds}`,
            soundOn: '🔊 Sound on',
            soundOff: '🔇 Sound off',
            soundOnLabel: 'Turn sound off',
            soundOffLabel: 'Turn sound on',
            collectTitle: 'Bring the fireflies together',
            collectHint: 'Tap a firefly or drag it into the lantern.',
            groupA: 'Sunset group',
            groupB: 'Moonlight group',
            joined: (n, total) => `${n} of ${total} inside`,
            collectSpeech: q => `I found ${q.left} fireflies here and ${q.right} over there. Help me bring both groups into the lantern.`,
            remaining: n => n === 1 ? 'One firefly is still waiting.' : `${n} fireflies are still waiting.`,
            chooseTitle: 'Which stone opens the path?',
            chooseSpeech: q => `${q.left} plus ${q.right}. How many fireflies are together now? Choose the correct stepping stone.`,
            correct: q => `Yes! ${q.left} plus ${q.right} equals ${q.sum}. The path is open!`,
            wrong: 'That stone did not light up. Count the fireflies in the lantern and try again.',
            next: 'Next mission',
            complete: 'Adventure complete!',
            completeCopy: 'You brought every group together and helped Eddy cross the whole lantern path.',
            again: 'Play again',
            replay: 'Hear Eddy again'
        },
        pl: {
            title: 'Przygoda z dodawaniem',
            introTitle: 'Ścieżka lampionów',
            introCopy: 'Dwie grupy świetlików czekają na łące. Połącz je, a potem wybierz właściwy kamień, aby pomóc Eddy’emu przejść przez strumień.',
            start: 'Rozpocznij przygodę',
            mission: i => `Misja ${i + 1}`,
            counter: i => `${i + 1} / ${totalRounds}`,
            soundOn: '🔊 Dźwięk włączony',
            soundOff: '🔇 Dźwięk wyłączony',
            soundOnLabel: 'Wyłącz dźwięk',
            soundOffLabel: 'Włącz dźwięk',
            collectTitle: 'Połącz świetliki',
            collectHint: 'Dotknij świetlika albo przeciągnij go do lampionu.',
            groupA: 'Grupa zachodu słońca',
            groupB: 'Grupa księżycowa',
            joined: (n, total) => `${n} z ${total} w środku`,
            collectSpeech: q => `Znalazłem tutaj ${q.left} świetliki, a tam ${q.right}. Pomóż mi przenieść obie grupy do lampionu.`,
            remaining: n => n === 1 ? 'Został jeszcze jeden świetlik.' : `Zostały jeszcze ${n} świetliki.`,
            chooseTitle: 'Który kamień otworzy drogę?',
            chooseSpeech: q => `${q.left} plus ${q.right}. Ile świetlików jest teraz razem? Wybierz właściwy kamień.`,
            correct: q => `Tak! ${q.left} plus ${q.right} równa się ${q.sum}. Droga jest otwarta!`,
            wrong: 'Ten kamień się nie zaświecił. Policz świetliki w lampionie i spróbuj jeszcze raz.',
            next: 'Następna misja',
            complete: 'Przygoda zakończona!',
            completeCopy: 'Połączyłeś wszystkie grupy i pomogłeś Eddy’emu przejść całą ścieżkę lampionów.',
            again: 'Zagraj ponownie',
            replay: 'Posłuchaj Eddy’ego ponownie'
        }
    };

    function inferLanguage() {
        const lang = (document.documentElement.lang || '').toLowerCase();
        if (lang.startsWith('pl')) return 'pl-PL';
        const text = (root.textContent || '').toLowerCase();
        return /polish|polski|polska|podstawa/.test(text) ? 'pl-PL' : 'en-GB';
    }

    function t() {
        return speechLanguage.toLowerCase().startsWith('pl') ? copy.pl : copy.en;
    }

    function shuffle(items) {
        for (let i = items.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [items[i], items[j]] = [items[j], items[i]];
        }
        return items;
    }

    function createRounds() {
        const pairs = [];
        for (let a = 1; a <= 6; a++) {
            for (let b = 1; b <= 6; b++) {
                const sum = a + b;
                if (sum >= 3 && sum <= 10) pairs.push({ left: a, right: b, sum });
            }
        }
        return shuffle(pairs).slice(0, totalRounds);
    }

    function installStyles() {
        if (document.getElementById('immersive-mini-game-v1')) return;
        const style = document.createElement('style');
        style.id = 'immersive-mini-game-v1';
        style.textContent = `
            .kid-game-shell.imm-shell{position:relative;overflow:hidden;min-height:760px;border:0;border-radius:28px;background:#78cef2;color:#153a51;box-shadow:0 26px 70px rgba(28,74,96,.22);isolation:isolate}
            .imm-world{position:absolute;inset:0;overflow:hidden;z-index:0;pointer-events:none;background:linear-gradient(180deg,#69c9f2 0%,#a7e9fb 48%,#8bd36f 49%,#4ca461 100%)}
            .imm-world:after{content:"";position:absolute;left:0;right:0;bottom:0;height:28%;background:linear-gradient(180deg,transparent,rgba(18,106,59,.12))}
            .imm-sun{position:absolute;right:7%;top:7%;width:92px;height:92px;border-radius:50%;background:#ffe875;box-shadow:0 0 0 18px rgba(255,232,117,.2),0 0 50px rgba(255,220,84,.5);animation:imm-sun 5s ease-in-out infinite}
            .imm-cloud{position:absolute;width:180px;height:55px;border-radius:999px;background:rgba(255,255,255,.9);filter:drop-shadow(0 8px 12px rgba(46,105,128,.12))}.imm-cloud:before,.imm-cloud:after{content:"";position:absolute;border-radius:50%;background:inherit}.imm-cloud:before{width:80px;height:80px;left:34px;top:-36px}.imm-cloud:after{width:96px;height:96px;right:20px;top:-49px}.imm-cloud.one{top:16%;left:-220px;animation:imm-cloud-right 20s linear infinite}.imm-cloud.two{top:26%;right:-250px;transform:scale(.72);animation:imm-cloud-left 26s linear infinite}
            .imm-hill{position:absolute;bottom:18%;width:56%;height:190px;border-radius:50% 50% 0 0;background:#6fc477}.imm-hill.one{left:-15%;animation:imm-hill-a 7s ease-in-out infinite alternate}.imm-hill.two{right:-17%;height:165px;background:#5bb66d;animation:imm-hill-b 9s ease-in-out infinite alternate}
            .imm-stream{position:absolute;left:49%;top:46%;bottom:-8%;width:24%;transform:translateX(-50%) rotate(2deg);background:linear-gradient(90deg,#78d6ee,#b9f4ff 50%,#6bc8e5);border-radius:48% 46% 0 0;box-shadow:inset 14px 0 0 rgba(255,255,255,.18),inset -10px 0 0 rgba(35,145,183,.12);animation:imm-water 4s ease-in-out infinite alternate}
            .imm-stream:after{content:"";position:absolute;inset:0;background:repeating-linear-gradient(170deg,transparent 0 22px,rgba(255,255,255,.24) 22px 26px,transparent 26px 48px);animation:imm-water-lines 5s linear infinite}
            .imm-grass{position:absolute;left:0;right:0;bottom:0;height:90px;background:repeating-linear-gradient(86deg,transparent 0 12px,rgba(17,110,54,.26) 12px 16px,transparent 16px 27px);transform-origin:bottom;animation:imm-grass 2.8s ease-in-out infinite alternate}
            .imm-specks{position:absolute;inset:0;background-image:radial-gradient(circle,#fff8a8 0 2px,transparent 2.6px),radial-gradient(circle,#fff 0 1.5px,transparent 2px);background-size:105px 105px,151px 151px;background-position:0 0,33px 17px;opacity:.7;animation:imm-specks 13s linear infinite}
            .imm-hud{position:relative;z-index:7;display:flex;align-items:center;justify-content:space-between;gap:14px;padding:18px 20px}.imm-brand{display:flex;align-items:center;gap:10px;min-width:0}.imm-badge{padding:8px 12px;border-radius:999px;background:rgba(255,255,255,.86);font-size:.76rem;font-weight:950;text-transform:uppercase;letter-spacing:.07em;color:#0b7183}.imm-title{font-weight:950;color:#153a51}.imm-actions{display:flex;gap:8px}.imm-counter,.imm-sound{border:2px solid rgba(255,255,255,.8);border-radius:999px;background:rgba(255,255,255,.86);min-height:42px;padding:0 13px;font-weight:950;color:#174c60}.imm-sound{cursor:pointer}.imm-progress{position:relative;z-index:7;margin:0 20px;height:12px;border:3px solid rgba(255,255,255,.85);border-radius:999px;background:rgba(255,255,255,.48);overflow:hidden}.imm-progress>span{display:block;height:100%;width:0;border-radius:inherit;background:linear-gradient(90deg,#ffd159,#ff9357,#36c88d);transition:width .55s ease}
            .imm-stage{position:relative;z-index:4;min-height:675px;padding:18px 20px 24px}.imm-scene{position:relative;min-height:620px;border:4px solid rgba(255,255,255,.76);border-radius:30px;background:rgba(255,255,255,.08);overflow:hidden}
            .imm-intro,.imm-finish{position:absolute;inset:0;display:grid;grid-template-columns:minmax(0,1fr) minmax(280px,38%);align-items:center;gap:20px;padding:42px;transition:opacity .45s ease,transform .45s ease}.imm-intro.is-hidden,.imm-finish{opacity:0;pointer-events:none;transform:scale(1.03)}.imm-finish.is-visible{opacity:1;pointer-events:auto;transform:none}.imm-intro-copy,.imm-finish-copy{position:relative;z-index:2;max-width:620px;padding:26px;border-radius:28px;background:rgba(255,255,255,.88);box-shadow:0 18px 45px rgba(35,83,103,.18);backdrop-filter:blur(5px)}.imm-eyebrow{margin:0 0 8px;font-size:.78rem;font-weight:950;letter-spacing:.08em;text-transform:uppercase;color:#0d8191}.imm-intro h2,.imm-finish h2{margin:0;color:#123d56;font-size:clamp(2.2rem,5vw,4.3rem);line-height:.98;letter-spacing:-.04em}.imm-intro p,.imm-finish p{font-size:1.05rem;line-height:1.55;color:#53748a}.imm-primary{min-height:56px;padding:0 24px;border:0;border-radius:18px;background:linear-gradient(180deg,#ffae5a,#ff8147);color:#fff;font-weight:950;font-size:1.03rem;cursor:pointer;box-shadow:0 8px 0 #d65f36,0 14px 26px rgba(163,77,43,.2)}.imm-primary:active{transform:translateY(4px);box-shadow:0 4px 0 #d65f36}.imm-intro-mascot,.imm-finish-mascot{position:relative;z-index:2;width:min(380px,100%);justify-self:center;filter:drop-shadow(0 25px 24px rgba(22,64,78,.2));animation:imm-mascot-idle 3.6s ease-in-out infinite}
            .imm-mission{position:absolute;inset:0;opacity:0;pointer-events:none;transform:translateY(25px);transition:opacity .45s ease,transform .45s ease}.imm-mission.is-visible{opacity:1;pointer-events:auto;transform:none}.imm-mission-head{position:absolute;left:22px;top:18px;z-index:6;max-width:550px;padding:15px 18px;border-radius:22px;background:rgba(255,255,255,.88);box-shadow:0 10px 26px rgba(30,77,98,.14);backdrop-filter:blur(4px)}.imm-mission-head small{display:block;font-weight:950;text-transform:uppercase;letter-spacing:.08em;color:#0d8191}.imm-mission-head strong{display:block;margin-top:2px;font-size:1.35rem;color:#153d55}.imm-mission-head span{display:block;margin-top:4px;color:#57788b;font-weight:750;font-size:.9rem}
            .imm-actor{position:absolute;z-index:8;right:3%;bottom:8%;width:230px;height:320px;transition:transform .75s cubic-bezier(.22,.88,.28,1),right .75s ease,bottom .75s ease}.imm-actor img{position:absolute;inset:0;width:100%;height:100%;object-fit:contain;filter:drop-shadow(0 18px 16px rgba(25,70,84,.22));user-select:none;-webkit-user-drag:none}.imm-actor.is-entering{animation:imm-enter .8s cubic-bezier(.17,.86,.29,1.2)}.imm-actor.is-celebrating{animation:imm-celebrate .75s ease}.imm-actor.is-running{transform:translateX(-46vw) scale(.9)}
            .imm-mouth{position:absolute;z-index:9;left:49.5%;top:38%;width:17px;height:6px;border-radius:50%;background:#412934;opacity:0;transform:translate(-50%,-50%);pointer-events:none}.imm-actor.is-speaking .imm-mouth{opacity:.88;animation:imm-mouth .16s ease-in-out infinite alternate}.imm-voice-wave{position:absolute;z-index:7;left:18%;top:28%;width:90px;height:90px;border:4px solid rgba(18,149,174,.3);border-radius:50%;opacity:0}.imm-actor.is-speaking .imm-voice-wave{animation:imm-wave 1.1s ease-out infinite}
            .imm-speech{position:absolute;z-index:9;right:18%;bottom:42%;width:min(360px,36%);padding:16px 18px;border:3px solid rgba(255,255,255,.9);border-radius:22px;background:rgba(255,255,255,.94);box-shadow:0 12px 28px rgba(25,72,91,.17);font-weight:850;line-height:1.45;color:#244f62}.imm-speech:after{content:"";position:absolute;right:25px;bottom:-14px;width:25px;height:25px;background:#fff;transform:rotate(45deg)}.imm-replay{position:absolute;right:5%;bottom:5%;z-index:10;border:0;border-radius:999px;padding:8px 12px;background:rgba(255,255,255,.84);color:#17687a;font-size:.76rem;font-weight:900;cursor:pointer}
            .imm-playfield{position:absolute;left:3%;right:29%;top:22%;bottom:9%;z-index:5}.imm-patch{position:absolute;width:34%;height:43%;border-radius:48% 46% 28% 30%;background:radial-gradient(circle at 50% 35%,#9ce37f,#55b966 65%,#369457);box-shadow:inset 0 -12px 0 rgba(28,108,58,.13),0 13px 25px rgba(32,91,63,.18)}.imm-patch.left{left:0;top:10%}.imm-patch.right{right:0;top:10%;background:radial-gradient(circle at 50% 35%,#8fda82,#49ab67 65%,#337f59)}.imm-patch-label{position:absolute;top:8px;left:50%;transform:translateX(-50%);padding:6px 9px;border-radius:999px;background:#fff;color:#30606e;font-size:.72rem;font-weight:950;white-space:nowrap}.imm-token-zone{position:absolute;inset:42px 12px 12px}.imm-firefly{position:absolute;width:42px;height:42px;border:0;border-radius:50%;cursor:grab;touch-action:none;background:radial-gradient(circle at 40% 35%,#fffbd5 0 13%,#ffe661 15% 35%,#ffa74b 62%,#df6d3f 100%);box-shadow:0 0 0 5px rgba(255,248,165,.23),0 0 25px rgba(255,223,81,.78);animation:imm-firefly 1.8s ease-in-out infinite}.imm-firefly.right{background:radial-gradient(circle at 40% 35%,#fff 0 12%,#dccfff 15% 34%,#9d83f4 60%,#6f54d8 100%);box-shadow:0 0 0 5px rgba(225,215,255,.25),0 0 25px rgba(158,139,255,.68)}.imm-firefly:before,.imm-firefly:after{content:"";position:absolute;top:14px;width:17px;height:9px;border-radius:50%;background:rgba(255,255,255,.72)}.imm-firefly:before{left:-10px;transform:rotate(-25deg)}.imm-firefly:after{right:-10px;transform:rotate(25deg)}.imm-firefly:hover{scale:1.12}.imm-firefly.is-moving{opacity:.15;pointer-events:none}
            .imm-lantern-wrap{position:absolute;left:50%;top:35%;transform:translate(-50%,-50%);display:flex;flex-direction:column;align-items:center;gap:8px}.imm-lantern-count{padding:6px 9px;border-radius:999px;background:rgba(255,255,255,.88);font-size:.72rem;font-weight:950;color:#4b6775}.imm-lantern{position:relative;width:128px;height:170px;border:7px solid #74512e;border-radius:28px 28px 38px 38px;background:linear-gradient(180deg,rgba(255,248,187,.45),rgba(255,181,67,.18));box-shadow:inset 0 0 0 5px rgba(255,255,255,.26),0 15px 26px rgba(72,70,40,.18)}.imm-lantern:before{content:"";position:absolute;left:50%;top:-35px;width:66px;height:43px;border:7px solid #74512e;border-bottom:0;border-radius:38px 38px 0 0;transform:translateX(-50%)}.imm-lantern.is-lit{background:radial-gradient(circle at 50% 48%,#fffbd2 0 18%,#ffe36c 29%,#f2a943 70%);box-shadow:0 0 60px rgba(255,219,84,.95),inset 0 0 18px rgba(255,255,255,.7);animation:imm-lantern 1.2s ease-in-out infinite}.imm-lantern-dots{position:absolute;inset:18px 12px 20px}.imm-dot{position:absolute;width:17px;height:17px;border-radius:50%;background:#fff6a0;box-shadow:0 0 18px #ffd54d;animation:imm-dot 1.2s ease-in-out infinite}
            .imm-answer-zone{position:absolute;left:4%;right:32%;bottom:7%;z-index:9;display:flex;flex-direction:column;align-items:center;gap:10px;opacity:0;pointer-events:none;transform:translateY(16px);transition:.35s ease}.imm-answer-zone.is-visible{opacity:1;pointer-events:auto;transform:none}.imm-answer-title{padding:9px 14px;border-radius:18px;background:rgba(255,255,255,.9);font-weight:950;color:#17465d;box-shadow:0 8px 18px rgba(32,76,94,.12)}.imm-equation{font-size:clamp(2rem,4vw,3.3rem);font-weight:950;color:#173d56;text-shadow:0 2px 0 rgba(255,255,255,.8)}.imm-stones{display:flex;gap:14px;justify-content:center}.imm-stone{position:relative;width:92px;height:62px;border:0;border-radius:48% 52% 45% 55%;background:linear-gradient(180deg,#fff7dc,#d8c9a3);box-shadow:0 8px 0 #9d8a62,0 12px 20px rgba(58,72,65,.2);font-size:1.55rem;font-weight:950;color:#385063;cursor:pointer;transition:transform .2s ease,filter .2s ease}.imm-stone:hover{transform:translateY(-4px) scale(1.04)}.imm-stone.is-wrong{animation:imm-wrong .45s ease}.imm-stone.is-correct{background:linear-gradient(180deg,#fff8a3,#ffd95c);box-shadow:0 8px 0 #cf9d31,0 0 32px rgba(255,221,82,.8);animation:imm-correct .65s ease}
            .imm-burst{position:absolute;z-index:30;width:11px;height:11px;border-radius:50%;background:#fff39b;box-shadow:0 0 14px #ffd34c;pointer-events:none;animation:imm-burst .8s ease-out forwards}.imm-fly-clone{position:fixed;z-index:9999;width:42px;height:42px;border-radius:50%;pointer-events:none;box-shadow:0 0 28px rgba(255,225,88,.9);transition:transform .48s cubic-bezier(.2,.8,.2,1),opacity .48s ease}
            @keyframes imm-sun{0%,100%{transform:translateY(0) scale(1)}50%{transform:translateY(10px) scale(1.06)}}@keyframes imm-cloud-right{to{transform:translateX(calc(100vw + 520px))}}@keyframes imm-cloud-left{to{transform:translateX(calc(-100vw - 520px)) scale(.72)}}@keyframes imm-hill-a{from{transform:translateX(-18px)}to{transform:translateX(32px) scale(1.04)}}@keyframes imm-hill-b{from{transform:translateX(18px)}to{transform:translateX(-28px) scale(1.05)}}@keyframes imm-water{from{transform:translateX(-50%) rotate(1deg) scaleX(.96)}to{transform:translateX(-50%) rotate(3deg) scaleX(1.04)}}@keyframes imm-water-lines{to{background-position:0 80px}}@keyframes imm-grass{from{transform:skewX(-1.4deg)}to{transform:skewX(1.4deg)}}@keyframes imm-specks{to{background-position:105px 56px,184px 92px}}@keyframes imm-mascot-idle{0%,100%{transform:translateY(0)}50%{transform:translateY(-9px)}}@keyframes imm-enter{from{transform:translateX(140px);opacity:0}to{transform:none;opacity:1}}@keyframes imm-celebrate{0%,100%{transform:translateY(0) rotate(0)}35%{transform:translateY(-22px) rotate(-3deg)}70%{transform:translateY(-8px) rotate(3deg)}}@keyframes imm-mouth{from{height:5px;width:17px}to{height:12px;width:14px}}@keyframes imm-wave{0%{opacity:.75;transform:scale(.55)}100%{opacity:0;transform:scale(1.4)}}@keyframes imm-firefly{0%,100%{translate:0 0;rotate:-3deg}50%{translate:0 -10px;rotate:5deg}}@keyframes imm-lantern{0%,100%{filter:brightness(1)}50%{filter:brightness(1.12)}}@keyframes imm-dot{0%,100%{transform:scale(.72);opacity:.65}50%{transform:scale(1.2);opacity:1}}@keyframes imm-wrong{0%,100%{transform:translateX(0)}25%{transform:translateX(-10px)}75%{transform:translateX(10px)}}@keyframes imm-correct{0%{transform:scale(1)}50%{transform:scale(1.15)}100%{transform:scale(1)}}@keyframes imm-burst{from{opacity:1;transform:translate(0,0) scale(1)}to{opacity:0;transform:translate(var(--x),var(--y)) scale(.2)}}
            @media(max-width:900px){.imm-intro,.imm-finish{grid-template-columns:1fr 270px;padding:28px}.imm-actor{width:190px;height:270px}.imm-speech{right:17%;width:35%}.imm-playfield{right:26%}}
            @media(max-width:720px){.kid-game-shell.imm-shell{border-radius:22px}.imm-hud{align-items:flex-start;flex-direction:column;padding:14px}.imm-actions{width:100%;justify-content:space-between}.imm-progress{margin:0 14px}.imm-stage{padding:12px}.imm-scene{min-height:700px}.imm-intro,.imm-finish{grid-template-columns:1fr;padding:24px 16px}.imm-intro-mascot,.imm-finish-mascot{width:220px}.imm-mission-head{left:12px;right:12px;top:12px;max-width:none}.imm-playfield{left:3%;right:3%;top:25%;bottom:30%}.imm-patch{width:31%;height:44%}.imm-lantern{width:100px;height:140px}.imm-actor{width:150px;height:210px;right:2%;bottom:3%}.imm-speech{left:3%;right:auto;bottom:11%;width:58%}.imm-replay{right:3%;bottom:1.5%}.imm-answer-zone{left:3%;right:3%;bottom:26%}.imm-stone{width:76px;height:54px}.imm-title{font-size:.92rem}}
            @media(max-width:500px){.imm-scene{min-height:650px}.imm-patch{width:34%;height:41%}.imm-firefly{width:34px;height:34px}.imm-firefly:before,.imm-firefly:after{width:13px;height:8px;top:11px}.imm-firefly:before{left:-8px}.imm-firefly:after{right:-8px}.imm-lantern-wrap{top:40%}.imm-lantern{width:78px;height:115px;border-width:5px}.imm-lantern:before{width:48px;height:34px;top:-27px;border-width:5px}.imm-answer-zone{bottom:28%}.imm-equation{font-size:2rem}.imm-stone{width:68px;height:48px;font-size:1.25rem}.imm-speech{font-size:.83rem;padding:11px}.imm-actor{width:125px;height:180px}.imm-intro h2,.imm-finish h2{font-size:2.35rem}}
            @media(prefers-reduced-motion:reduce){.imm-world *, .imm-intro-mascot,.imm-finish-mascot,.imm-firefly,.imm-lantern.is-lit,.imm-dot,.imm-actor.is-speaking .imm-mouth,.imm-actor.is-speaking .imm-voice-wave{animation:none!important}}
        `;
        document.head.appendChild(style);
    }

    function buildGame() {
        const x = t();
        shell.className = 'kid-game-shell imm-shell';
        shell.innerHTML = `
            <div class="imm-world" aria-hidden="true"><div class="imm-specks"></div><div class="imm-sun"></div><div class="imm-cloud one"></div><div class="imm-cloud two"></div><div class="imm-hill one"></div><div class="imm-hill two"></div><div class="imm-stream"></div><div class="imm-grass"></div></div>
            <header class="imm-hud"><div class="imm-brand"><span class="imm-badge">Edulytics</span><span class="imm-title">${x.title}</span></div><div class="imm-actions"><span class="imm-counter" data-imm-counter>${x.counter(0)}</span><button class="imm-sound" type="button" data-imm-sound aria-pressed="true">${x.soundOn}</button></div></header>
            <div class="imm-progress"><span data-imm-progress></span></div>
            <div class="imm-stage"><div class="imm-scene">
                <section class="imm-intro" data-imm-intro><div class="imm-intro-copy"><p class="imm-eyebrow">Edulytics Math Adventure</p><h2>${x.introTitle}</h2><p>${x.introCopy}</p><button class="imm-primary" type="button" data-imm-start>${x.start}</button></div><img class="imm-intro-mascot" src="${mascotAsset}" alt="Edulytics cartoon character" /></section>
                <section class="imm-mission" data-imm-mission>
                    <div class="imm-mission-head"><small data-imm-mission-label></small><strong>${x.collectTitle}</strong><span>${x.collectHint}</span></div>
                    <div class="imm-playfield"><div class="imm-patch left"><span class="imm-patch-label" data-imm-left-label></span><div class="imm-token-zone" data-imm-left></div></div><div class="imm-lantern-wrap" data-imm-drop><span class="imm-lantern-count" data-imm-count></span><div class="imm-lantern" data-imm-lantern><div class="imm-lantern-dots" data-imm-dots></div></div></div><div class="imm-patch right"><span class="imm-patch-label" data-imm-right-label></span><div class="imm-token-zone" data-imm-right></div></div></div>
                    <div class="imm-answer-zone" data-imm-answer><div class="imm-answer-title">${x.chooseTitle}</div><div class="imm-equation" data-imm-equation></div><div class="imm-stones" data-imm-stones></div></div>
                    <div class="imm-speech" data-imm-speech></div><div class="imm-actor" data-imm-actor><div class="imm-voice-wave"></div><img src="${mascotAsset}" alt="Edulytics cartoon character" draggable="false" /><span class="imm-mouth"></span></div><button class="imm-replay" type="button" data-imm-replay>${x.replay}</button>
                </section>
                <section class="imm-finish" data-imm-finish><div class="imm-finish-copy"><p class="imm-eyebrow">Edulytics</p><h2>${x.complete}</h2><p>${x.completeCopy}</p><button class="imm-primary" type="button" data-imm-again>${x.again}</button></div><img class="imm-finish-mascot" src="${mascotAsset}" alt="Edulytics cartoon character celebrating" /></section>
            </div></div>`;
        bindEvents();
    }

    function bindEvents() {
        shell.querySelector('[data-imm-start]').addEventListener('click', startAdventure);
        shell.querySelector('[data-imm-again]').addEventListener('click', resetAdventure);
        shell.querySelector('[data-imm-sound]').addEventListener('click', toggleSound);
        shell.querySelector('[data-imm-replay]').addEventListener('click', () => speak(currentSpeech));
        const drop = shell.querySelector('[data-imm-drop]');
        drop.addEventListener('dragover', event => { if (dragId && phase === 'collect') event.preventDefault(); });
        drop.addEventListener('drop', event => { event.preventDefault(); if (dragId) moveById(dragId); dragId = null; });
    }

    function startAdventure() {
        ensureAudio();
        refreshVoices();
        rounds = createRounds();
        roundIndex = 0;
        shell.querySelector('[data-imm-intro]').classList.add('is-hidden');
        shell.querySelector('[data-imm-mission]').classList.add('is-visible');
        window.setTimeout(renderRound, 250);
    }

    function resetAdventure() {
        rounds = createRounds();
        roundIndex = 0;
        shell.querySelector('[data-imm-finish]').classList.remove('is-visible');
        shell.querySelector('[data-imm-mission]').classList.add('is-visible');
        renderRound();
    }

    function renderRound() {
        const q = rounds[roundIndex];
        if (!q) return finishAdventure();
        phase = 'collect';
        locked = false;
        collected = 0;
        const x = t();
        const left = shell.querySelector('[data-imm-left]');
        const right = shell.querySelector('[data-imm-right]');
        left.innerHTML = '';
        right.innerHTML = '';
        shell.querySelector('[data-imm-dots]').innerHTML = '';
        shell.querySelector('[data-imm-lantern]').classList.remove('is-lit');
        shell.querySelector('[data-imm-answer]').classList.remove('is-visible');
        shell.querySelector('[data-imm-stones]').innerHTML = '';
        shell.querySelector('[data-imm-counter]').textContent = x.counter(roundIndex);
        shell.querySelector('[data-imm-mission-label]').textContent = x.mission(roundIndex);
        shell.querySelector('[data-imm-left-label]').textContent = `${x.groupA} · ${q.left}`;
        shell.querySelector('[data-imm-right-label]').textContent = `${x.groupB} · ${q.right}`;
        shell.querySelector('[data-imm-equation]').textContent = `${q.left} + ${q.right} = ?`;
        updateCount();
        updateProgress();
        renderFireflies(left, q.left, 'left');
        renderFireflies(right, q.right, 'right');
        const actor = shell.querySelector('[data-imm-actor]');
        actor.className = 'imm-actor is-entering';
        setSpeech(x.collectSpeech(q), true);
        window.setTimeout(() => actor.classList.remove('is-entering'), 850);
    }

    function positions(count, side) {
        const a = [[12,12],[54,9],[73,33],[29,43],[59,57],[10,68],[40,78],[76,78],[22,88],[57,90]];
        const b = [[62,11],[24,8],[10,34],[48,42],[22,59],[71,60],[42,76],[8,82],[63,87],[31,91]];
        return (side === 'right' ? b : a).slice(0, count);
    }

    function renderFireflies(zone, count, side) {
        positions(count, side).forEach((p, i) => {
            const button = document.createElement('button');
            button.type = 'button';
            button.className = `imm-firefly ${side}`;
            button.style.left = `${p[0]}%`;
            button.style.top = `${p[1]}%`;
            button.style.animationDelay = `${-(i * .21)}s`;
            button.dataset.immId = `${side}-${i}`;
            button.draggable = true;
            button.setAttribute('aria-label', t().collectHint);
            button.addEventListener('click', () => moveFirefly(button));
            button.addEventListener('dragstart', event => { dragId = button.dataset.immId; event.dataTransfer.setData('text/plain', dragId); });
            button.addEventListener('dragend', () => { dragId = null; });
            zone.appendChild(button);
        });
    }

    function moveById(id) {
        const safe = window.CSS && CSS.escape ? CSS.escape(id) : id.replace(/[^a-zA-Z0-9_-]/g, '');
        const token = shell.querySelector(`[data-imm-id="${safe}"]`);
        if (token) moveFirefly(token);
    }

    function moveFirefly(token) {
        if (phase !== 'collect' || locked || token.classList.contains('is-moving')) return;
        token.classList.add('is-moving');
        flyToLantern(token);
        window.setTimeout(() => {
            token.remove();
            addDot();
            collected++;
            updateCount();
            tone('fly');
            const q = rounds[roundIndex];
            if (collected >= q.sum) revealAnswers();
            else if (collected === Math.ceil(q.sum / 2)) setSpeech(t().remaining(q.sum - collected), false);
        }, 430);
    }

    function flyToLantern(token) {
        const from = token.getBoundingClientRect();
        const to = shell.querySelector('[data-imm-lantern]').getBoundingClientRect();
        const clone = document.createElement('div');
        clone.className = 'imm-fly-clone';
        clone.style.left = `${from.left}px`;
        clone.style.top = `${from.top}px`;
        clone.style.background = token.classList.contains('right') ? 'radial-gradient(circle,#fff 0 12%,#dccfff 15% 34%,#9d83f4 60%,#6f54d8 100%)' : 'radial-gradient(circle,#fffbd5 0 13%,#ffe661 15% 35%,#ffa74b 62%,#df6d3f 100%)';
        document.body.appendChild(clone);
        const dx = to.left + to.width / 2 - (from.left + from.width / 2);
        const dy = to.top + to.height / 2 - (from.top + from.height / 2);
        requestAnimationFrame(() => { clone.style.transform = `translate(${dx}px,${dy}px) scale(.35) rotate(240deg)`; clone.style.opacity = '.12'; });
        window.setTimeout(() => clone.remove(), 520);
    }

    function addDot() {
        const field = shell.querySelector('[data-imm-dots]');
        const dot = document.createElement('span');
        dot.className = 'imm-dot';
        const spots = [[16,18],[51,12],[72,29],[29,43],[61,50],[12,61],[43,69],[75,68],[25,82],[59,84]];
        const p = spots[field.children.length % spots.length];
        dot.style.left = `${p[0]}%`;
        dot.style.top = `${p[1]}%`;
        field.appendChild(dot);
    }

    function updateCount() {
        const q = rounds[roundIndex];
        if (!q) return;
        shell.querySelector('[data-imm-count]').textContent = t().joined(collected, q.sum);
    }

    function revealAnswers() {
        phase = 'answer';
        const q = rounds[roundIndex];
        shell.querySelector('[data-imm-lantern]').classList.add('is-lit');
        burst(shell.querySelector('[data-imm-lantern]'), 16);
        const choices = shuffle([...new Set([q.sum, Math.max(1, q.sum - 1), Math.min(12, q.sum + 1), Math.max(1, q.sum - 2), Math.min(12, q.sum + 2)])]).slice(0, 3);
        if (!choices.includes(q.sum)) choices[0] = q.sum;
        shuffle(choices);
        const stones = shell.querySelector('[data-imm-stones]');
        choices.forEach(value => {
            const b = document.createElement('button');
            b.type = 'button';
            b.className = 'imm-stone';
            b.textContent = String(value);
            b.addEventListener('click', () => chooseStone(b, value));
            stones.appendChild(b);
        });
        shell.querySelector('[data-imm-answer]').classList.add('is-visible');
        setSpeech(t().chooseSpeech(q), true);
        tone('join');
    }

    function chooseStone(button, value) {
        if (phase !== 'answer' || locked) return;
        const q = rounds[roundIndex];
        if (value !== q.sum) {
            button.classList.remove('is-wrong');
            void button.offsetWidth;
            button.classList.add('is-wrong');
            setSpeech(t().wrong, true);
            tone('wrong');
            return;
        }
        locked = true;
        phase = 'celebrate';
        button.classList.add('is-correct');
        const actor = shell.querySelector('[data-imm-actor]');
        actor.classList.add('is-celebrating');
        burst(button, 28);
        setSpeech(t().correct(q), true);
        tone('correct');
        updateProgress(true);
        window.setTimeout(() => actor.classList.add('is-running'), 450);
        window.setTimeout(() => {
            roundIndex++;
            actor.className = 'imm-actor';
            if (roundIndex >= totalRounds) finishAdventure();
            else renderRound();
        }, 1650);
    }

    function updateProgress(afterCorrect = false) {
        const completed = afterCorrect ? roundIndex + 1 : roundIndex;
        shell.querySelector('[data-imm-progress]').style.width = `${Math.round((completed / totalRounds) * 100)}%`;
    }

    function finishAdventure() {
        phase = 'finish';
        locked = true;
        if ('speechSynthesis' in window) window.speechSynthesis.cancel();
        shell.querySelector('[data-imm-mission]').classList.remove('is-visible');
        shell.querySelector('[data-imm-finish]').classList.add('is-visible');
        shell.querySelector('[data-imm-progress]').style.width = '100%';
        currentSpeech = t().completeCopy;
        window.setTimeout(() => speak(currentSpeech), 250);
        tone('finish');
    }

    function burst(target, count) {
        const scene = shell.querySelector('.imm-scene');
        if (!scene || !target) return;
        const sr = scene.getBoundingClientRect();
        const tr = target.getBoundingClientRect();
        const cx = tr.left - sr.left + tr.width / 2;
        const cy = tr.top - sr.top + tr.height / 2;
        for (let i = 0; i < count; i++) {
            const p = document.createElement('span');
            p.className = 'imm-burst';
            p.style.left = `${cx}px`;
            p.style.top = `${cy}px`;
            const angle = (Math.PI * 2 * i) / count + Math.random() * .25;
            const distance = 45 + Math.random() * 110;
            p.style.setProperty('--x', `${Math.cos(angle) * distance}px`);
            p.style.setProperty('--y', `${Math.sin(angle) * distance}px`);
            scene.appendChild(p);
            window.setTimeout(() => p.remove(), 900);
        }
    }

    function setSpeech(text, aloud) {
        currentSpeech = text;
        const bubble = shell.querySelector('[data-imm-speech]');
        if (bubble) bubble.textContent = text;
        if (aloud) speak(text);
    }

    function refreshVoices() {
        if (!('speechSynthesis' in window)) return;
        const voices = window.speechSynthesis.getVoices();
        const lang = speechLanguage.toLowerCase();
        const prefix = lang.slice(0, 2);
        const candidates = voices.filter(v => (v.lang || '').toLowerCase().startsWith(prefix));
        const childPatterns = /child|kid|young|ana|junior|youth/i;
        const friendlyPatterns = /aria|jenny|zira|sonia|samantha|ava|emma|libby|mia|zosia|paulina|ewa|google/i;
        preferredVoice = candidates
            .map(v => ({ v, score: ((v.lang || '').toLowerCase() === lang ? 30 : 0) + (childPatterns.test(v.name) ? 120 : 0) + (friendlyPatterns.test(v.name) ? 55 : 0) + (/microsoft|google/i.test(v.name) ? 20 : 0) + (v.localService ? 5 : 0) }))
            .sort((a, b) => b.score - a.score)[0]?.v || candidates[0] || null;
    }

    function speak(text) {
        if (!soundOn || !text || !('speechSynthesis' in window)) return;
        window.speechSynthesis.cancel();
        refreshVoices();
        const u = new SpeechSynthesisUtterance(text);
        u.lang = speechLanguage;
        u.rate = speechLanguage.startsWith('pl') ? .9 : .94;
        u.pitch = 1.34;
        u.volume = 1;
        if (preferredVoice) u.voice = preferredVoice;
        const actor = shell.querySelector('[data-imm-actor]');
        u.onstart = () => actor?.classList.add('is-speaking');
        u.onend = () => actor?.classList.remove('is-speaking');
        u.onerror = () => actor?.classList.remove('is-speaking');
        window.speechSynthesis.speak(u);
    }

    function toggleSound() {
        soundOn = !soundOn;
        const x = t();
        const b = shell.querySelector('[data-imm-sound]');
        b.textContent = soundOn ? x.soundOn : x.soundOff;
        b.setAttribute('aria-pressed', String(soundOn));
        b.setAttribute('aria-label', soundOn ? x.soundOnLabel : x.soundOffLabel);
        if (!soundOn && 'speechSynthesis' in window) window.speechSynthesis.cancel();
        if (soundOn) { tone('tap'); speak(currentSpeech); }
    }

    function ensureAudio() {
        if (!soundOn) return null;
        if (!audioContext) {
            const Ctx = window.AudioContext || window.webkitAudioContext;
            if (!Ctx) return null;
            audioContext = new Ctx();
        }
        if (audioContext.state === 'suspended') audioContext.resume();
        return audioContext;
    }

    function tone(kind) {
        const ctx = ensureAudio();
        if (!ctx) return;
        const patterns = { tap:[[430,.05,.04]], fly:[[620,.05,.05],[850,.06,.04]], join:[[470,.07,.06],[650,.08,.05],[850,.1,.04]], correct:[[520,.07,.07],[660,.08,.06],[830,.14,.05]], wrong:[[260,.08,.04],[215,.1,.03]], finish:[[440,.08,.06],[554,.08,.06],[659,.09,.06],[880,.17,.05]] };
        const now = ctx.currentTime;
        (patterns[kind] || patterns.tap).forEach((part, i) => {
            const osc = ctx.createOscillator();
            const gain = ctx.createGain();
            osc.type = kind === 'wrong' ? 'triangle' : 'sine';
            osc.frequency.value = part[0];
            const at = now + i * .07;
            gain.gain.setValueAtTime(.0001, at);
            gain.gain.exponentialRampToValueAtTime(part[2], at + .01);
            gain.gain.exponentialRampToValueAtTime(.0001, at + part[1]);
            osc.connect(gain); gain.connect(ctx.destination); osc.start(at); osc.stop(at + part[1] + .03);
        });
    }

    function activateTab(name) {
        tabs.forEach(tab => { const active = tab.dataset.previewTab === name; tab.classList.toggle('is-active', active); tab.setAttribute('aria-selected', String(active)); });
        panels.forEach(panel => { panel.hidden = panel.dataset.previewPanel !== name; });
        if (name === 'practice' && !started) {
            started = true;
            installStyles();
            buildGame();
            refreshVoices();
        }
    }

    tabs.forEach(tab => tab.addEventListener('click', () => activateTab(tab.dataset.previewTab)));
    if ('speechSynthesis' in window) window.speechSynthesis.addEventListener?.('voiceschanged', refreshVoices);
})();
