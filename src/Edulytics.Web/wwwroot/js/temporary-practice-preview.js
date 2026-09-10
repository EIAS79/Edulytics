(() => {
    const root = document.querySelector('[data-temporary-practice-preview]');
    if (!root) return;

    const tabs = Array.from(root.querySelectorAll('[data-preview-tab]'));
    const panels = Array.from(root.querySelectorAll('[data-preview-panel]'));
    const questionText = root.querySelector('[data-preview-question]');
    const questionNumber = root.querySelector('[data-preview-question-number]');
    const counter = root.querySelector('[data-preview-counter]');
    const progress = root.querySelector('[data-preview-progress]');
    const answer = root.querySelector('[data-preview-answer]');
    const feedback = root.querySelector('[data-preview-feedback]');
    const check = root.querySelector('[data-preview-check]');
    const joinButton = root.querySelector('[data-preview-join]');
    const gameStage = root.querySelector('[data-preview-game-stage]');
    const answerDock = root.querySelector('[data-preview-answer-dock]');
    const finish = root.querySelector('[data-preview-finish]');
    const finalScore = root.querySelector('[data-preview-final-score]');
    const finalDetail = root.querySelector('[data-preview-final-detail]');
    const restart = root.querySelector('[data-preview-restart]');
    const soundToggle = root.querySelector('[data-preview-sound]');
    const speech = root.querySelector('[data-preview-speech]');
    const mascot = root.querySelector('[data-preview-mascot]');
    const dockTitle = root.querySelector('[data-preview-dock-title]');
    const dockCopy = root.querySelector('[data-preview-dock-copy]');
    const leftLabel = root.querySelector('[data-preview-left-label]');
    const rightLabel = root.querySelector('[data-preview-right-label]');
    const leftGroup = root.querySelector('[data-preview-left-group]');
    const rightGroup = root.querySelector('[data-preview-right-group]');
    const joinedGroup = root.querySelector('[data-preview-joined-group]');
    const leftCard = root.querySelector('[data-preview-left-card]');
    const rightCard = root.querySelector('[data-preview-right-card]');
    const joinedCard = root.querySelector('[data-preview-joined-card]');
    const keyButtons = Array.from(root.querySelectorAll('[data-practice-key]'));
    const gameShell = root.querySelector('.kid-game-shell');
    const playBoard = root.querySelector('.kid-play-board');
    const mascotWrap = root.querySelector('.kid-mascot-wrap');
    const finishMascot = root.querySelector('.kid-finish-mascot');

    if (!questionText || !answer || !check || !joinButton || !gameShell) return;

    const mascotAsset = '/images/public/edulytics-math-mascot-animation.png';
    let questions = [];
    let index = 0;
    let correct = 0;
    let locked = false;
    let joined = false;
    let soundOn = true;
    let audioContext = null;
    let speechLanguage = inferSpeechLanguage();
    let currentSpeechText = '';
    let preferredVoice = null;

    const copy = {
        en: {
            countJoin: current => `Question ${index + 1}. Bring the two groups together. Count ${current.left} plus ${current.right}, then tap Join the groups.`,
            afterJoin: current => `${current.left} plus ${current.right}. How many are there altogether?`,
            correct: current => `Brilliant! ${current.left} plus ${current.right} equals ${current.sum}.`,
            incorrect: current => `Good try. Count the joined group once more. There are ${current.sum} altogether.`,
            finish: percent => `Math adventure complete. You scored ${percent} percent. Great work!`
        },
        pl: {
            countJoin: current => `Pytanie ${index + 1}. Połącz dwie grupy. Policz ${current.left} plus ${current.right}, a potem naciśnij przycisk połącz grupy.`,
            afterJoin: current => `${current.left} plus ${current.right}. Ile jest razem?`,
            correct: current => `Świetnie! ${current.left} plus ${current.right} równa się ${current.sum}.`,
            incorrect: current => `Dobra próba. Policz połączoną grupę jeszcze raz. Razem jest ${current.sum}.`,
            finish: percent => `Przygoda matematyczna zakończona. Twój wynik to ${percent} procent. Świetna robota!`
        }
    };

    installScene();
    installMotionStyles();
    prepareMascot();
    refreshVoices();

    function inferSpeechLanguage() {
        const htmlLanguage = (document.documentElement.lang || '').toLowerCase();
        if (htmlLanguage.startsWith('pl')) return 'pl-PL';
        const chips = Array.from(root.querySelectorAll('.preview-chip'))
            .map(chip => (chip.textContent || '').toLowerCase())
            .join(' ');
        return /polish|polska|polski|podstawa/.test(chips) ? 'pl-PL' : 'en-GB';
    }

    function activeCopy() {
        return speechLanguage.toLowerCase().startsWith('pl') ? copy.pl : copy.en;
    }

    function installScene() {
        if (!gameShell.querySelector('.kid-motion-world')) {
            const world = document.createElement('div');
            world.className = 'kid-motion-world';
            world.setAttribute('aria-hidden', 'true');
            world.innerHTML = `
                <div class="kid-aurora kid-aurora-a"></div>
                <div class="kid-aurora kid-aurora-b"></div>
                <div class="kid-orbit kid-orbit-a"></div>
                <div class="kid-orbit kid-orbit-b"></div>
                <div class="kid-float-symbol kid-float-1">+</div>
                <div class="kid-float-symbol kid-float-2">2</div>
                <div class="kid-float-symbol kid-float-3">=</div>
                <div class="kid-float-symbol kid-float-4">5</div>
                <div class="kid-float-symbol kid-float-5">+</div>
                <div class="kid-float-symbol kid-float-6">3</div>
                <div class="kid-sparkle kid-sparkle-1">✦</div>
                <div class="kid-sparkle kid-sparkle-2">✦</div>
                <div class="kid-sparkle kid-sparkle-3">✦</div>
                <div class="kid-sparkle kid-sparkle-4">✦</div>
                <div class="kid-ground-track"><span></span><span></span><span></span><span></span></div>`;
            gameShell.prepend(world);
        }

        if (mascotWrap && !mascotWrap.querySelector('.kid-voice-halo')) {
            const halo = document.createElement('div');
            halo.className = 'kid-voice-halo';
            halo.setAttribute('aria-hidden', 'true');
            halo.innerHTML = '<span></span><span></span><span></span>';
            mascotWrap.prepend(halo);
        }

        gameShell.addEventListener('pointermove', event => {
            if (window.matchMedia('(prefers-reduced-motion: reduce)').matches || window.innerWidth < 760) return;
            const bounds = gameShell.getBoundingClientRect();
            const x = ((event.clientX - bounds.left) / bounds.width - 0.5) * 2;
            const y = ((event.clientY - bounds.top) / bounds.height - 0.5) * 2;
            gameShell.style.setProperty('--scene-x', x.toFixed(3));
            gameShell.style.setProperty('--scene-y', y.toFixed(3));
        });
        gameShell.addEventListener('pointerleave', () => {
            gameShell.style.setProperty('--scene-x', '0');
            gameShell.style.setProperty('--scene-y', '0');
        });
    }

    function installMotionStyles() {
        if (document.getElementById('temporary-practice-motion-styles-v2')) return;
        const style = document.createElement('style');
        style.id = 'temporary-practice-motion-styles-v2';
        style.textContent = `
            .kid-game-shell {
                --scene-x: 0;
                --scene-y: 0;
                isolation: isolate;
                background:
                    radial-gradient(circle at 18% 12%, rgba(255,255,255,.9) 0 2.5%, transparent 2.8%),
                    radial-gradient(circle at 84% 22%, rgba(255,246,172,.85) 0 2%, transparent 2.3%),
                    linear-gradient(180deg,#6ed3ff 0%,#a9eaff 46%,#e8fbff 52%,#83d46d 53%,#57b85e 100%);
                background-size: 120% 120%;
                animation: preview-world-breathe 8s ease-in-out infinite alternate;
            }
            .kid-motion-world { position:absolute; inset:0; overflow:hidden; pointer-events:none; z-index:-1; }
            .kid-aurora { position:absolute; width:58%; height:45%; border-radius:50%; filter:blur(3px); opacity:.34; mix-blend-mode:screen; }
            .kid-aurora-a { left:-12%; top:12%; background:radial-gradient(circle,rgba(255,255,255,.95),rgba(140,235,255,.18) 55%,transparent 72%); animation:aurora-a 10s ease-in-out infinite alternate; }
            .kid-aurora-b { right:-18%; top:28%; background:radial-gradient(circle,rgba(255,242,155,.72),rgba(255,181,115,.12) 56%,transparent 72%); animation:aurora-b 12s ease-in-out infinite alternate; }
            .kid-orbit { position:absolute; border:2px dashed rgba(255,255,255,.28); border-radius:50%; }
            .kid-orbit-a { width:470px; height:470px; left:-220px; top:180px; animation:orbit-spin 28s linear infinite; }
            .kid-orbit-b { width:390px; height:390px; right:-190px; top:40px; animation:orbit-spin-reverse 24s linear infinite; }
            .kid-float-symbol { position:absolute; display:grid; place-items:center; width:58px; height:58px; border:3px solid rgba(255,255,255,.84); border-radius:18px; background:rgba(255,255,255,.72); color:#156b7a; font-size:1.55rem; font-weight:950; box-shadow:0 12px 28px rgba(35,91,112,.12); backdrop-filter:blur(4px); }
            .kid-float-1 { left:4%; top:31%; animation:float-one 8.8s ease-in-out infinite; }
            .kid-float-2 { left:15%; top:64%; width:48px; height:48px; border-radius:50%; animation:float-two 10.2s ease-in-out infinite -2.1s; }
            .kid-float-3 { right:5%; top:38%; animation:float-three 9.6s ease-in-out infinite -.8s; }
            .kid-float-4 { right:18%; top:69%; width:50px; height:50px; border-radius:50%; animation:float-four 11s ease-in-out infinite -3s; }
            .kid-float-5 { left:43%; top:9%; width:44px; height:44px; border-radius:50%; animation:float-two 8s ease-in-out infinite -1s; }
            .kid-float-6 { right:34%; top:17%; width:42px; height:42px; border-radius:14px; animation:float-one 9.4s ease-in-out infinite -4s; }
            .kid-sparkle { position:absolute; color:#fff9b0; font-size:1.45rem; text-shadow:0 0 14px rgba(255,240,107,.9); animation:sparkle-pulse 2.4s ease-in-out infinite; }
            .kid-sparkle-1 { left:24%; top:20%; }
            .kid-sparkle-2 { right:24%; top:31%; animation-delay:-.7s; }
            .kid-sparkle-3 { left:9%; top:78%; animation-delay:-1.2s; }
            .kid-sparkle-4 { right:7%; top:78%; animation-delay:-1.8s; }
            .kid-ground-track { position:absolute; left:-5%; right:-5%; bottom:10px; height:88px; transform:translate3d(calc(var(--scene-x) * -8px),calc(var(--scene-y) * -2px),0); transition:transform .2s ease-out; }
            .kid-ground-track span { position:absolute; bottom:10px; width:64px; height:26px; border-radius:50%; background:rgba(255,255,255,.19); animation:ground-drift 7s linear infinite; }
            .kid-ground-track span:nth-child(1){left:8%;animation-delay:-1s}.kid-ground-track span:nth-child(2){left:31%;animation-delay:-4s}.kid-ground-track span:nth-child(3){left:61%;animation-delay:-2.2s}.kid-ground-track span:nth-child(4){left:83%;animation-delay:-5.3s}
            .kid-cloud-one { animation:cloud-drift-a 15s ease-in-out infinite alternate; filter:drop-shadow(0 10px 12px rgba(74,134,159,.09)); }
            .kid-cloud-two { animation:cloud-drift-b 18s ease-in-out infinite alternate; filter:drop-shadow(0 10px 12px rgba(74,134,159,.08)); }
            .kid-sun { box-shadow:0 0 0 14px rgba(255,230,109,.22),0 0 48px rgba(255,220,91,.42); animation:sun-orbit 7s ease-in-out infinite; }
            .kid-game-shell::before { animation:hill-a 10s ease-in-out infinite alternate; }
            .kid-game-shell::after { animation:hill-b 12s ease-in-out infinite alternate; }
            .kid-game-topbar,.kid-progress-wrap,.kid-stage,.kid-answer-dock,.kid-finish { position:relative; z-index:2; }
            .kid-game-topbar { animation:ui-drop .6s cubic-bezier(.2,.8,.2,1) both; }
            .kid-progress-wrap { animation:ui-drop .65s .08s cubic-bezier(.2,.8,.2,1) both; }
            .kid-progress-fill { position:relative; overflow:hidden; }
            .kid-progress-fill::after { content:""; position:absolute; inset:-3px auto -3px -42px; width:42px; background:linear-gradient(90deg,transparent,rgba(255,255,255,.9),transparent); transform:skewX(-18deg); animation:progress-shine 2.8s ease-in-out infinite; }
            .kid-play-board { transform:perspective(900px) rotateX(calc(var(--scene-y) * -0.5deg)) rotateY(calc(var(--scene-x) * 0.7deg)); transition:transform .22s ease-out,box-shadow .22s ease-out; animation:board-enter .7s .12s cubic-bezier(.2,.85,.2,1) both; box-shadow:0 24px 48px rgba(38,95,116,.17),inset 0 1px 0 rgba(255,255,255,.55); }
            .kid-instruction h2 { text-shadow:0 2px 0 rgba(255,255,255,.75); }
            .kid-group-card { animation:group-float 3.8s ease-in-out infinite; }
            .kid-group-card.is-right { animation-delay:-1.8s; }
            .kid-group-card.is-joined { animation:joined-pop .52s cubic-bezier(.17,.89,.32,1.28) both !important; }
            .kid-token { animation:token-live 2.1s ease-in-out infinite; will-change:transform; }
            .kid-token:nth-child(2n){animation-delay:-.36s}.kid-token:nth-child(3n){animation-delay:-.72s}.kid-token:nth-child(5n){animation-delay:-1.08s}
            .kid-plus { animation:plus-pulse 2.2s ease-in-out infinite; }
            .kid-join-button { position:relative; overflow:hidden; animation:join-breathe 2.4s ease-in-out infinite; }
            .kid-join-button::after { content:""; position:absolute; top:-35%; bottom:-35%; left:-34%; width:24%; background:rgba(255,255,255,.48); transform:rotate(18deg); animation:button-shine 2.6s ease-in-out infinite; }
            .kid-equation { animation:equation-float 3s ease-in-out infinite; }
            .kid-answer-box:not(:disabled) { animation:answer-ready 1.8s ease-in-out infinite; }
            .kid-mascot-column { z-index:3; }
            .kid-mascot-wrap { width:min(285px,100%); height:405px; }
            .kid-mascot { width:100%; max-height:400px; object-fit:contain; object-position:center bottom; clip-path:none !important; filter:drop-shadow(0 18px 17px rgba(23,72,88,.18)); transform:none !important; animation:none !important; transition:filter .25s ease,opacity .25s ease; }
            .kid-mascot.is-celebrating { filter:drop-shadow(0 18px 17px rgba(23,72,88,.18)) drop-shadow(0 0 24px rgba(255,220,77,.72)); }
            .kid-voice-halo { position:absolute; z-index:0; left:50%; top:47%; width:190px; height:190px; transform:translate(-50%,-50%); pointer-events:none; opacity:0; transition:opacity .2s ease; }
            .kid-voice-halo span { position:absolute; inset:50%; border:3px solid rgba(28,154,177,.35); border-radius:50%; transform:translate(-50%,-50%) scale(.3); opacity:0; }
            .kid-mascot-wrap.is-speaking .kid-voice-halo { opacity:1; }
            .kid-mascot-wrap.is-speaking .kid-voice-halo span { animation:voice-ring 1.65s ease-out infinite; }
            .kid-mascot-wrap.is-speaking .kid-voice-halo span:nth-child(2){animation-delay:.45s}.kid-mascot-wrap.is-speaking .kid-voice-halo span:nth-child(3){animation-delay:.9s}
            .kid-speech { transform-origin:85% 100%; transition:transform .2s ease,box-shadow .2s ease; }
            .kid-speech.is-speaking { animation:speech-talk .62s ease-in-out infinite alternate; box-shadow:0 13px 30px rgba(40,90,112,.2),0 0 0 5px rgba(255,255,255,.28); }
            .kid-key { transition:transform .12s ease,filter .12s ease,box-shadow .12s ease; }
            .kid-key:not(:disabled):hover { transform:translateY(-3px) scale(1.03); filter:brightness(1.08); }
            .kid-check-button:not(:disabled) { animation:check-ready 2.1s ease-in-out infinite; }
            .kid-scene-burst { position:absolute; z-index:5; pointer-events:none; width:14px; height:14px; border-radius:50%; animation:burst-pop .72s ease-out forwards; }
            .kid-question-enter { animation:question-enter .48s cubic-bezier(.2,.8,.2,1) both; }
            .kid-finish-card { animation:finish-pop .7s cubic-bezier(.17,.89,.32,1.28) both; }
            .kid-finish-mascot { content:url('${mascotAsset}'); filter:drop-shadow(0 14px 15px rgba(27,75,89,.18)); animation:finish-float 3s ease-in-out infinite; }
            @keyframes preview-world-breathe { 0%{background-position:50% 0%}100%{background-position:48% 12%} }
            @keyframes aurora-a { from{transform:translate3d(-3%,0,0) scale(1)}to{transform:translate3d(18%,8%,0) scale(1.16)} }
            @keyframes aurora-b { from{transform:translate3d(6%,-4%,0) scale(.94)}to{transform:translate3d(-13%,12%,0) scale(1.14)} }
            @keyframes orbit-spin { to{transform:rotate(360deg)} }
            @keyframes orbit-spin-reverse { to{transform:rotate(-360deg)} }
            @keyframes float-one { 0%,100%{transform:translate3d(0,0,0) rotate(-7deg)}50%{transform:translate3d(34px,-25px,0) rotate(8deg)} }
            @keyframes float-two { 0%,100%{transform:translate3d(0,0,0) rotate(5deg)}50%{transform:translate3d(-28px,-34px,0) rotate(-8deg)} }
            @keyframes float-three { 0%,100%{transform:translate3d(0,0,0) rotate(8deg)}50%{transform:translate3d(-38px,24px,0) rotate(-6deg)} }
            @keyframes float-four { 0%,100%{transform:translate3d(0,0,0) rotate(-4deg)}50%{transform:translate3d(22px,-30px,0) rotate(9deg)} }
            @keyframes sparkle-pulse { 0%,100%{transform:scale(.6) rotate(0);opacity:.35}50%{transform:scale(1.35) rotate(28deg);opacity:1} }
            @keyframes ground-drift { 0%{transform:translateX(-20px) scale(.8);opacity:.15}50%{opacity:.4}100%{transform:translateX(85px) scale(1.25);opacity:.05} }
            @keyframes cloud-drift-a { from{transform:translate3d(-40px,0,0) scale(.92)}to{transform:translate3d(150px,-12px,0) scale(1.06)} }
            @keyframes cloud-drift-b { from{transform:translate3d(48px,0,0) scale(.72)}to{transform:translate3d(-140px,14px,0) scale(.86)} }
            @keyframes sun-orbit { 0%,100%{transform:translateY(0) rotate(0) scale(1)}50%{transform:translateY(-13px) rotate(8deg) scale(1.08)} }
            @keyframes hill-a { from{transform:translateX(-18px) rotate(-7deg) scale(1)}to{transform:translateX(28px) rotate(-4deg) scale(1.06)} }
            @keyframes hill-b { from{transform:translateX(18px) rotate(8deg) scale(1)}to{transform:translateX(-26px) rotate(5deg) scale(1.055)} }
            @keyframes ui-drop { from{opacity:0;transform:translateY(-16px)}to{opacity:1;transform:none} }
            @keyframes board-enter { from{opacity:0;transform:perspective(900px) translateY(28px) scale(.975)}to{opacity:1;transform:perspective(900px) translateY(0) scale(1)} }
            @keyframes progress-shine { 0%,40%{left:-48px}75%,100%{left:110%} }
            @keyframes group-float { 0%,100%{transform:translateY(0) rotate(-.35deg)}50%{transform:translateY(-7px) rotate(.35deg)} }
            @keyframes token-live { 0%,100%{transform:translateY(0) scale(1)}50%{transform:translateY(-7px) scale(1.05)} }
            @keyframes plus-pulse { 0%,100%{transform:scale(1) rotate(0)}50%{transform:scale(1.08) rotate(3deg)} }
            @keyframes join-breathe { 0%,100%{transform:translateY(0) scale(1)}50%{transform:translateY(-2px) scale(1.025)} }
            @keyframes button-shine { 0%,30%{left:-38%}65%,100%{left:128%} }
            @keyframes joined-pop { 0%{opacity:0;transform:scale(.78) rotate(-2deg)}65%{transform:scale(1.05) rotate(1deg)}100%{opacity:1;transform:scale(1)} }
            @keyframes equation-float { 0%,100%{transform:translateY(0)}50%{transform:translateY(-4px)} }
            @keyframes answer-ready { 0%,100%{box-shadow:0 7px 0 #168091,0 0 0 0 rgba(40,166,186,.08)}50%{box-shadow:0 7px 0 #168091,0 0 0 8px rgba(40,166,186,.13)} }
            @keyframes voice-ring { 0%{transform:translate(-50%,-50%) scale(.25);opacity:.65}100%{transform:translate(-50%,-50%) scale(1.15);opacity:0} }
            @keyframes speech-talk { from{transform:translateY(0) scale(1)}to{transform:translateY(-3px) scale(1.012)} }
            @keyframes check-ready { 0%,100%{filter:brightness(1);transform:translateY(0)}50%{filter:brightness(1.08);transform:translateY(-2px)} }
            @keyframes burst-pop { from{transform:translate(0,0) scale(.2);opacity:1}to{transform:translate(var(--burst-x),var(--burst-y)) scale(1.1);opacity:0} }
            @keyframes question-enter { from{opacity:0;transform:translateY(16px) scale(.985)}to{opacity:1;transform:none} }
            @keyframes finish-pop { from{opacity:0;transform:scale(.82) translateY(28px)}to{opacity:1;transform:none} }
            @keyframes finish-float { 0%,100%{transform:translateY(0)}50%{transform:translateY(-8px)} }
            @media (max-width:760px) {
                .kid-float-symbol{opacity:.55;transform:scale(.78)}
                .kid-float-1,.kid-float-2{left:1%}.kid-float-3,.kid-float-4{right:1%}
                .kid-mascot-wrap{width:190px;height:230px}.kid-voice-halo{width:135px;height:135px}
            }
            @media (max-width:520px) {
                .kid-float-2,.kid-float-4,.kid-float-5,.kid-float-6,.kid-orbit{display:none}
                .kid-float-symbol{width:38px;height:38px;border-radius:12px;font-size:1rem;opacity:.4}
                .kid-mascot-wrap{width:145px;height:185px}.kid-voice-halo{width:110px;height:110px}
            }
            @media (prefers-reduced-motion:reduce) {
                .kid-game-shell,.kid-motion-world *, .kid-cloud,.kid-sun,.kid-game-shell::before,.kid-game-shell::after,
                .kid-game-topbar,.kid-progress-wrap,.kid-play-board,.kid-progress-fill::after,.kid-group-card,.kid-token,.kid-plus,
                .kid-join-button,.kid-join-button::after,.kid-equation,.kid-answer-box,.kid-speech,.kid-voice-halo span,
                .kid-check-button,.kid-finish-card,.kid-finish-mascot { animation:none !important; transition:none !important; }
                .kid-play-board { transform:none !important; }
            }
        `;
        document.head.appendChild(style);
    }

    function prepareMascot() {
        if (mascot) {
            mascot.src = mascotAsset;
            mascot.dataset.mascotIdle = mascotAsset;
            mascot.dataset.mascotCorrect = mascotAsset;
            mascot.dataset.mascotTry = mascotAsset;
            mascot.setAttribute('role', 'button');
            mascot.setAttribute('tabindex', '0');
            mascot.setAttribute('aria-label', 'Read the current question aloud');
            mascot.title = 'Tap the character to hear the question again';
        }
        if (finishMascot) finishMascot.src = mascotAsset;
    }

    function setTab(name) {
        tabs.forEach(tab => {
            const active = tab.dataset.previewTab === name;
            tab.classList.toggle('is-active', active);
            tab.setAttribute('aria-selected', active ? 'true' : 'false');
        });
        panels.forEach(panel => { panel.hidden = panel.dataset.previewPanel !== name; });
    }

    function ensureAudioContext() {
        if (!soundOn) return null;
        const Context = window.AudioContext || window.webkitAudioContext;
        if (!Context) return null;
        if (!audioContext) audioContext = new Context();
        if (audioContext.state === 'suspended') audioContext.resume().catch(() => {});
        return audioContext;
    }

    function tone(frequency, duration = 0.08, delay = 0, volume = 0.045, type = 'sine') {
        const context = ensureAudioContext();
        if (!context) return;
        const oscillator = context.createOscillator();
        const gain = context.createGain();
        const start = context.currentTime + delay;
        const end = start + duration;
        oscillator.type = type;
        oscillator.frequency.setValueAtTime(frequency, start);
        gain.gain.setValueAtTime(0.0001, start);
        gain.gain.exponentialRampToValueAtTime(volume, start + 0.012);
        gain.gain.exponentialRampToValueAtTime(0.0001, end);
        oscillator.connect(gain);
        gain.connect(context.destination);
        oscillator.start(start);
        oscillator.stop(end + 0.02);
    }

    const playClick = () => tone(520, 0.045, 0, 0.025, 'triangle');
    const playJoin = () => { tone(330, .08, 0, .035, 'triangle'); tone(440, .08, .07, .035, 'triangle'); tone(550, .1, .14, .04, 'triangle'); };
    const playCorrect = () => { tone(523.25, .13, 0, .045, 'triangle'); tone(659.25, .13, .09, .045, 'triangle'); tone(783.99, .18, .18, .05, 'triangle'); };
    const playIncorrect = () => { tone(330, .11, 0, .03, 'sine'); tone(277, .14, .12, .028, 'sine'); };
    const playFinish = () => { tone(523.25,.14,0,.045,'triangle'); tone(659.25,.14,.11,.045,'triangle'); tone(783.99,.14,.22,.05,'triangle'); tone(1046.5,.25,.34,.055,'triangle'); };

    function updateSoundToggle() {
        if (!soundToggle) return;
        soundToggle.textContent = soundOn ? '🔊 Sound on' : '🔇 Sound off';
        soundToggle.setAttribute('aria-pressed', soundOn ? 'true' : 'false');
        soundToggle.setAttribute('aria-label', soundOn ? 'Turn game sounds and voice off' : 'Turn game sounds and voice on');
    }

    function refreshVoices() {
        if (!('speechSynthesis' in window)) return;
        preferredVoice = chooseFriendlyVoice(window.speechSynthesis.getVoices(), speechLanguage);
    }

    function chooseFriendlyVoice(voices, language) {
        if (!voices || voices.length === 0) return null;
        const prefix = language.toLowerCase().slice(0, 2);
        const matching = voices.filter(voice => (voice.lang || '').toLowerCase().startsWith(prefix));
        const pool = matching.length ? matching : voices;
        const preferredNames = prefix === 'pl'
            ? ['zofia','paulina','agnieszka','ewa','marek','google polski','polish']
            : ['sonia','libby','ava','jenny','samantha','hazel','google uk english female','google english'];
        return pool
            .map(voice => {
                const name = (voice.name || '').toLowerCase();
                let score = 0;
                preferredNames.forEach((preferred, position) => { if (name.includes(preferred)) score += 40 - position; });
                if (/natural|neural|enhanced|premium/.test(name)) score += 20;
                if (/female/.test(name)) score += 6;
                if ((voice.lang || '').toLowerCase() === language.toLowerCase()) score += 10;
                if (voice.localService) score += 2;
                return { voice, score };
            })
            .sort((a,b) => b.score - a.score)[0]?.voice || pool[0] || null;
    }

    function stopSpeaking() {
        if ('speechSynthesis' in window) window.speechSynthesis.cancel();
        mascot?.classList.remove('is-talking');
        mascotWrap?.classList.remove('is-speaking');
        speech?.classList.remove('is-speaking');
    }

    function speak(text) {
        currentSpeechText = text || '';
        if (!soundOn || !currentSpeechText || !('speechSynthesis' in window)) return;
        stopSpeaking();
        refreshVoices();
        const utterance = new SpeechSynthesisUtterance(currentSpeechText);
        utterance.lang = speechLanguage;
        utterance.rate = speechLanguage.toLowerCase().startsWith('pl') ? .88 : .9;
        utterance.pitch = 1.16;
        utterance.volume = .96;
        if (preferredVoice) utterance.voice = preferredVoice;
        utterance.onstart = () => {
            mascot?.classList.add('is-talking');
            mascotWrap?.classList.add('is-speaking');
            speech?.classList.add('is-speaking');
        };
        const done = () => {
            mascot?.classList.remove('is-talking');
            mascotWrap?.classList.remove('is-speaking');
            speech?.classList.remove('is-speaking');
        };
        utterance.onend = done;
        utterance.onerror = done;
        window.speechSynthesis.speak(utterance);
    }

    function repeatCurrentSpeech() {
        if (soundOn && currentSpeechText) speak(currentSpeechText);
    }

    function buildQuestions() {
        const result = [];
        const seen = new Set();
        let guard = 0;
        while (result.length < 10 && guard < 700) {
            guard += 1;
            const difficulty = result.length < 3 ? 4 : result.length < 7 ? 6 : 8;
            const left = Math.floor(Math.random() * difficulty) + 1;
            const right = Math.floor(Math.random() * difficulty) + 1;
            const sum = left + right;
            if (sum > 15) continue;
            const key = `${left}:${right}`;
            if (seen.has(key)) continue;
            seen.add(key);
            result.push({ left, right, sum });
        }
        while (result.length < 10) {
            const left = (result.length % 5) + 1;
            const right = ((result.length + 2) % 5) + 1;
            result.push({ left, right, sum: left + right });
        }
        return result;
    }

    function createTokens(container, count, className) {
        container.replaceChildren();
        for (let i = 0; i < count; i += 1) {
            const token = document.createElement('span');
            token.className = `kid-token ${className}`;
            token.style.setProperty('--token-index', String(i));
            token.setAttribute('aria-hidden', 'true');
            container.appendChild(token);
        }
    }

    function createJoinedTokens(current) {
        joinedGroup.replaceChildren();
        for (let i = 0; i < current.sum; i += 1) {
            const token = document.createElement('span');
            token.className = 'kid-token kid-token-joined';
            token.setAttribute('aria-hidden', 'true');
            token.style.background = i < current.left
                ? 'linear-gradient(145deg,#ffbd59,#ff8e4f)'
                : 'linear-gradient(145deg,#a795ff,#745fe7)';
            joinedGroup.appendChild(token);
        }
    }

    function burst(centerElement, count = 12) {
        if (!centerElement || !gameShell) return;
        const shellRect = gameShell.getBoundingClientRect();
        const rect = centerElement.getBoundingClientRect();
        const originX = rect.left - shellRect.left + rect.width / 2;
        const originY = rect.top - shellRect.top + rect.height / 2;
        const colors = ['#ffd95d','#ff8f6b','#7f72ef','#47c891','#ffffff'];
        for (let i = 0; i < count; i += 1) {
            const particle = document.createElement('span');
            particle.className = 'kid-scene-burst';
            const angle = (Math.PI * 2 * i) / count + Math.random() * .35;
            const distance = 55 + Math.random() * 80;
            particle.style.left = `${originX}px`;
            particle.style.top = `${originY}px`;
            particle.style.background = colors[i % colors.length];
            particle.style.setProperty('--burst-x', `${Math.cos(angle) * distance}px`);
            particle.style.setProperty('--burst-y', `${Math.sin(angle) * distance}px`);
            gameShell.appendChild(particle);
            window.setTimeout(() => particle.remove(), 800);
        }
    }

    function setMascot(state) {
        if (!mascot) return;
        mascot.src = mascotAsset;
        mascot.classList.remove('is-celebrating');
        if (state === 'correct') {
            void mascot.offsetWidth;
            mascot.classList.add('is-celebrating');
            burst(mascotWrap || mascot, 16);
        }
    }

    function setInputEnabled(enabled) {
        answer.disabled = !enabled;
        check.disabled = !enabled;
        keyButtons.forEach(button => { button.disabled = !enabled; });
    }

    function replayQuestionEntrance() {
        if (!playBoard) return;
        playBoard.classList.remove('kid-question-enter');
        void playBoard.offsetWidth;
        playBoard.classList.add('kid-question-enter');
    }

    function renderQuestion() {
        const current = questions[index];
        if (!current) {
            showFinish();
            return;
        }

        stopSpeaking();
        locked = false;
        joined = false;
        answer.value = '';
        feedback.hidden = true;
        feedback.className = 'kid-feedback';
        joinedCard.style.display = 'none';
        leftCard.style.display = '';
        rightCard.style.display = '';
        leftCard.style.opacity = '1';
        rightCard.style.opacity = '1';
        leftCard.style.transform = '';
        rightCard.style.transform = '';
        joinButton.hidden = false;
        joinButton.disabled = false;
        setInputEnabled(false);
        setMascot('idle');
        replayQuestionEntrance();

        leftLabel.textContent = String(current.left);
        rightLabel.textContent = String(current.right);
        createTokens(leftGroup, current.left, 'kid-token-left');
        createTokens(rightGroup, current.right, 'kid-token-right');
        joinedGroup.replaceChildren();
        questionText.textContent = `${current.left} + ${current.right} =`;
        questionNumber.textContent = `Question ${index + 1}`;
        counter.textContent = `${index + 1} of ${questions.length}`;
        speech.textContent = 'Count each group, then tap “Join the groups!” to bring them together.';
        dockTitle.textContent = 'First, join the groups';
        dockCopy.textContent = 'Then use the number buttons to enter your answer.';
        currentSpeechText = activeCopy().countJoin(current);

        const percent = Math.round(index * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));

        const practicePanel = panels.find(panel => panel.dataset.previewPanel === 'practice');
        if (practicePanel && !practicePanel.hidden && soundOn) window.setTimeout(() => speak(currentSpeechText), 180);
    }

    function joinGroups() {
        if (locked || joined || !questions[index]) return;
        joined = true;
        const current = questions[index];
        stopSpeaking();
        playJoin();
        burst(joinButton, 14);
        createJoinedTokens(current);
        leftCard.style.opacity = '.18';
        rightCard.style.opacity = '.18';
        leftCard.style.transform = 'translateX(34px) scale(.9) rotate(2deg)';
        rightCard.style.transform = 'translateX(-34px) scale(.9) rotate(-2deg)';
        joinButton.disabled = true;

        window.setTimeout(() => {
            leftCard.style.display = 'none';
            rightCard.style.display = 'none';
            joinedCard.style.display = 'block';
            burst(joinedCard, 18);
            joinButton.hidden = true;
            setInputEnabled(true);
            speech.textContent = 'Nice! Now count how many are together and choose your answer.';
            dockTitle.textContent = 'How many altogether?';
            dockCopy.textContent = 'Use the number buttons, then check your answer.';
            currentSpeechText = activeCopy().afterJoin(current);
            answer.focus();
            speak(currentSpeechText);
        }, 360);
    }

    function showFinish() {
        stopSpeaking();
        gameStage.hidden = true;
        answerDock.hidden = true;
        finish.hidden = false;
        progress.style.width = '100%';
        progress.parentElement.setAttribute('aria-valuenow', '100');
        counter.textContent = 'Complete!';
        const percent = Math.round(correct * 100 / questions.length);
        finalScore.textContent = `${percent}%`;
        finalDetail.textContent = `${correct} correct out of ${questions.length}. Great work joining groups!`;
        currentSpeechText = activeCopy().finish(percent);
        burst(root.querySelector('.kid-finish-card'), 24);
        playFinish();
        window.setTimeout(() => speak(currentSpeechText), 420);
    }

    function submitAnswer() {
        if (locked || !joined || !questions[index]) return;
        const value = answer.value.trim();
        if (value === '') {
            answer.focus();
            return;
        }

        stopSpeaking();
        locked = true;
        const current = questions[index];
        const isCorrect = Number(value) === current.sum;
        if (isCorrect) correct += 1;
        setInputEnabled(false);
        feedback.hidden = false;
        feedback.className = `kid-feedback ${isCorrect ? 'is-correct' : 'is-incorrect'}`;

        if (isCorrect) {
            feedback.textContent = `Yes! ${current.left} + ${current.right} = ${current.sum}.`;
            speech.textContent = 'Brilliant! You joined the groups and counted them correctly!';
            currentSpeechText = activeCopy().correct(current);
            setMascot('correct');
            burst(feedback, 18);
            playCorrect();
        } else {
            feedback.textContent = `Almost! Together there are ${current.sum}.`;
            speech.textContent = `Good try! Count the joined group once more — there are ${current.sum} altogether.`;
            currentSpeechText = activeCopy().incorrect(current);
            setMascot('try');
            playIncorrect();
        }

        window.setTimeout(() => speak(currentSpeechText), 140);
        const completed = index + 1;
        const percent = Math.round(completed * 100 / questions.length);
        progress.style.width = `${percent}%`;
        progress.parentElement.setAttribute('aria-valuenow', String(percent));
        window.setTimeout(() => {
            index += 1;
            renderQuestion();
        }, isCorrect ? 1900 : 2350);
    }

    function resetGame() {
        stopSpeaking();
        questions = buildQuestions();
        index = 0;
        correct = 0;
        gameStage.hidden = false;
        answerDock.hidden = false;
        finish.hidden = true;
        renderQuestion();
    }

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            playClick();
            const target = tab.dataset.previewTab;
            setTab(target);
            if (target === 'practice' && soundOn && questions[index]) {
                currentSpeechText = joined ? activeCopy().afterJoin(questions[index]) : activeCopy().countJoin(questions[index]);
                window.setTimeout(() => speak(currentSpeechText), 140);
            } else if (target !== 'practice') {
                stopSpeaking();
            }
        });
    });

    joinButton.addEventListener('click', joinGroups);
    keyButtons.forEach(button => {
        button.addEventListener('click', () => {
            if (locked || answer.disabled) return;
            playClick();
            const key = button.dataset.practiceKey;
            if (key === 'clear') answer.value = '';
            else if (key === 'backspace') answer.value = answer.value.slice(0, -1);
            else if (/^\d$/.test(key) && answer.value.length < 2) answer.value += key;
            answer.focus();
        });
    });

    check.addEventListener('click', submitAnswer);
    answer.addEventListener('keydown', event => {
        if (event.key === 'Enter') {
            event.preventDefault();
            submitAnswer();
        }
    });
    restart.addEventListener('click', () => { playClick(); resetGame(); });

    soundToggle?.addEventListener('click', () => {
        soundOn = !soundOn;
        updateSoundToggle();
        if (soundOn) {
            ensureAudioContext();
            window.setTimeout(playClick, 15);
            const practicePanel = panels.find(panel => panel.dataset.previewPanel === 'practice');
            if (practicePanel && !practicePanel.hidden) window.setTimeout(repeatCurrentSpeech, 120);
        } else {
            stopSpeaking();
        }
    });

    if ('speechSynthesis' in window) window.speechSynthesis.addEventListener?.('voiceschanged', refreshVoices);
    mascot?.addEventListener('click', repeatCurrentSpeech);
    mascot?.addEventListener('keydown', event => {
        if (event.key === 'Enter' || event.key === ' ') {
            event.preventDefault();
            repeatCurrentSpeech();
        }
    });

    updateSoundToggle();
    resetGame();
    setTab('lesson');
})();
