(function () {
    'use strict';

    var root = document.querySelector('[data-curriculum-workspace-game]');
    if (!root) return;

    var guide = '/images/game/v9/eddy-guide.webp';
    var workspace = root.getAttribute('data-workspace') || 'REASONING_MODELING';
    var mechanic = root.getAttribute('data-mechanic') || 'MODEL_SELECT';
    var locale = root.getAttribute('data-lesson-language') || 'en';
    var lessonTitle = root.getAttribute('data-lesson-title') || '';
    var unitTitle = root.getAttribute('data-unit-title') || '';

    var copy = {
        en: {
            brand: 'EDULYTICS PRACTICE', mission: 'YOUR MISSION', round: 'Round', score: 'Stars',
            check: 'Check', next: 'Next', again: 'Play again', complete: 'Mission complete!',
            completeCopy: 'You used the right mathematical tool for this lesson.', listen: 'Listen',
            hint: 'Try this:', correct: 'Correct!', retry: 'Try again.',
            fullscreen: 'Fullscreen', sound: 'Sound'
        },
        pl: {
            brand: 'ĆWICZENIA EDULYTICS', mission: 'TWOJE ZADANIE', round: 'Runda', score: 'Gwiazdki',
            check: 'Sprawdź', next: 'Dalej', again: 'Zagraj ponownie', complete: 'Misja ukończona!',
            completeCopy: 'Użyłeś odpowiedniego narzędzia matematycznego do tego tematu.', listen: 'Posłuchaj',
            hint: 'Wskazówka:', correct: 'Dobrze!', retry: 'Spróbuj ponownie.',
            fullscreen: 'Pełny ekran', sound: 'Dźwięk'
        },
        ar: {
            brand: 'تدريب EDULYTICS', mission: 'مهمتك', round: 'الجولة', score: 'النجوم',
            check: 'تحقق', next: 'التالي', again: 'العب مرة أخرى', complete: 'اكتملت المهمة!',
            completeCopy: 'استخدمت الأداة الرياضية المناسبة لهذا الدرس.', listen: 'استمع',
            hint: 'تلميح:', correct: 'صحيح!', retry: 'حاول مرة أخرى.',
            fullscreen: 'ملء الشاشة', sound: 'الصوت'
        }
    }[locale] || null;
    if (!copy) copy = {
        brand: 'EDULYTICS PRACTICE', mission: 'YOUR MISSION', round: 'Round', score: 'Stars',
        check: 'Check', next: 'Next', again: 'Play again', complete: 'Mission complete!',
        completeCopy: 'You used the right mathematical tool for this lesson.', listen: 'Listen',
        hint: 'Try this:', correct: 'Correct!', retry: 'Try again.', fullscreen: 'Fullscreen', sound: 'Sound'
    };

    var names = {
        OBJECT_COUNTING: ['Counting Garden', 'Ogród liczenia', 'حديقة العد'],
        NUMBER_SYSTEM: ['Number Navigation Lab', 'Laboratorium liczb', 'مختبر الأعداد'],
        OPERATIONS: ['Operations Workshop', 'Warsztat działań', 'ورشة العمليات'],
        FRACTION_DECIMAL_PERCENT: ['Fraction Studio', 'Studio ułamków', 'استوديو الكسور'],
        GEOMETRY: ['Geometry Workshop', 'Pracownia geometrii', 'ورشة الهندسة'],
        MEASUREMENT: ['Measurement Lab', 'Laboratorium pomiarów', 'مختبر القياس'],
        TIME_MONEY: ['Time & Money World', 'Świat czasu i pieniędzy', 'عالم الوقت والمال'],
        DATA_STATISTICS: ['Data Lab', 'Laboratorium danych', 'مختبر البيانات'],
        RATIO_ALGEBRA: ['Pattern & Balance Lab', 'Laboratorium wzorów i równań', 'مختبر الأنماط والمعادلات'],
        REASONING_MODELING: ['Reasoning Studio', 'Studio rozumowania', 'استوديو التفكير'],
        COMPOSITE_SESSION: ['Math Quest', 'Misja matematyczna', 'مهمة الرياضيات']
    };

    var languageIndex = locale === 'pl' ? 1 : (locale === 'ar' ? 2 : 0);
    function workspaceName() {
        var value = names[workspace] || names.REASONING_MODELING;
        return value[languageIndex];
    }

    var state = {
        roundIndex: 0,
        roundCount: 8,
        score: 0,
        soundOn: true,
        locked: false,
        hintTimer: null,
        selected: null,
        data: null
    };

    function rnd(min, max) { return Math.floor(Math.random() * (max - min + 1)) + min; }
    function shuffle(values) {
        var a = values.slice();
        for (var i = a.length - 1; i > 0; i -= 1) {
            var j = Math.floor(Math.random() * (i + 1));
            var t = a[i]; a[i] = a[j]; a[j] = t;
        }
        return a;
    }
    function gcd(a, b) { while (b) { var t = b; b = a % b; a = t; } return Math.abs(a); }
    function esc(value) {
        return String(value).replace(/[&<>'"]/g, function (c) { return {'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','"':'&quot;'}[c]; });
    }

    root.innerHTML = '' +
        '<section class="gw-game gw-universal" data-gw-game data-workspace="' + esc(workspace) + '">' +
            '<header class="gw-hud">' +
                '<div class="gw-brand"><span class="gw-brand-mark" aria-hidden="true">◆</span><span><strong>' + esc(workspaceName().toUpperCase()) + '</strong><small>' + esc(unitTitle) + '</small></span></div>' +
                '<div class="gw-hud-right"><div class="gw-pill">★ <span data-gw-score>0</span></div><div class="gw-pill"><span data-gw-round>1 / 8</span></div><button class="gw-icon" type="button" data-gw-sound aria-label="' + esc(copy.sound) + '">🔊</button><button class="gw-icon" type="button" data-gw-fullscreen aria-label="' + esc(copy.fullscreen) + '">⛶</button></div>' +
            '</header>' +
            '<div class="gw-progress"><span data-gw-progress></span></div>' +
            '<main class="gw-stage">' +
                '<div class="gw-mission"><span>' + esc(copy.mission) + '</span><strong data-gw-question></strong><small data-gw-subquestion></small></div>' +
                '<div class="gw-runtime-board" data-gw-board></div>' +
            '</main>' +
            '<div class="gw-eddy"><img src="' + guide + '" alt="Eddy" /><div><span>EDDY</span><p data-gw-eddy></p></div></div>' +
            '<div class="gw-complete" data-gw-complete hidden><div class="gw-complete-card"><img src="' + guide + '" alt="Eddy" /><p>' + esc(copy.brand) + '</p><h2>' + esc(copy.complete) + '</h2><p>' + esc(copy.completeCopy) + '</p><strong data-gw-final-score></strong><br/><button class="gw-primary" type="button" data-gw-replay>' + esc(copy.again) + '</button></div></div>' +
        '</section>';

    var game = root.querySelector('[data-gw-game]');
    var board = root.querySelector('[data-gw-board]');
    var question = root.querySelector('[data-gw-question]');
    var subquestion = root.querySelector('[data-gw-subquestion]');
    var eddy = root.querySelector('[data-gw-eddy]');
    var scoreNode = root.querySelector('[data-gw-score]');
    var roundNode = root.querySelector('[data-gw-round]');
    var progress = root.querySelector('[data-gw-progress]');
    var soundButton = root.querySelector('[data-gw-sound]');
    var fullscreenButton = root.querySelector('[data-gw-fullscreen]');
    var complete = root.querySelector('[data-gw-complete]');
    var finalScore = root.querySelector('[data-gw-final-score]');

    function localized(en, pl, ar) { return locale === 'pl' ? pl : (locale === 'ar' ? ar : en); }
    function narrate(text) {
        if (!state.soundOn || !window.speechSynthesis || !window.SpeechSynthesisUtterance) return;
        try {
            window.speechSynthesis.cancel();
            var u = new window.SpeechSynthesisUtterance(text);
            u.lang = locale === 'pl' ? 'pl-PL' : (locale === 'ar' ? 'ar-AE' : 'en-GB');
            u.rate = 0.92; u.pitch = 1.12;
            window.speechSynthesis.speak(u);
        } catch (ignore) { }
    }
    function setEddy(text, speak) { eddy.textContent = text; if (speak) narrate(text); }
    function clearHint() { if (state.hintTimer) { window.clearTimeout(state.hintTimer); state.hintTimer = null; } }
    function hintAfter(text) {
        clearHint();
        state.hintTimer = window.setTimeout(function () { if (!state.locked) setEddy(copy.hint + ' ' + text, true); }, 9000);
    }
    function updateHud() {
        roundNode.textContent = (state.roundIndex + 1) + ' / ' + state.roundCount;
        scoreNode.textContent = state.score;
        progress.style.width = ((state.roundIndex / state.roundCount) * 100) + '%';
    }
    function makeChoice(label, value, extraClass) {
        var button = document.createElement('button');
        button.type = 'button'; button.className = 'gw-choice' + (extraClass ? ' ' + extraClass : '');
        button.textContent = label; button.setAttribute('data-runtime-answer', value);
        return button;
    }
    function choices(values, correctValue, feedback) {
        var row = document.createElement('div'); row.className = 'gw-choice-row gw-runtime-choices';
        shuffle(values).forEach(function (item) {
            var value = typeof item === 'object' ? item.value : item;
            var label = typeof item === 'object' ? item.label : item;
            row.appendChild(makeChoice(label, value));
        });
        row.addEventListener('click', function (event) {
            if (state.locked) return;
            var button = event.target.closest('[data-runtime-answer]'); if (!button) return;
            var value = button.getAttribute('data-runtime-answer');
            if (String(value) === String(correctValue)) succeed(button, feedback);
            else fail(button, localized('Look at the model again and compare carefully.', 'Spójrz jeszcze raz na model i porównaj uważnie.', 'انظر إلى النموذج مرة أخرى وقارن بعناية.'));
        });
        board.appendChild(row);
    }
    function succeed(button, message) {
        clearHint(); state.locked = true; state.score += 200;
        if (button) button.classList.add('is-correct'); updateHud();
        setEddy(copy.correct + ' ' + message, true);
        window.setTimeout(nextRound, 900);
    }
    function fail(button, message) {
        if (button) button.classList.add('is-wrong');
        setEddy(copy.retry + ' ' + message, true);
        window.setTimeout(function () { if (button) button.classList.remove('is-wrong'); }, 650);
    }
    function nextRound() {
        state.roundIndex += 1;
        if (state.roundIndex >= state.roundCount) {
            progress.style.width = '100%'; finalScore.textContent = copy.score + ': ' + state.score;
            complete.hidden = false; narrate(copy.complete); return;
        }
        renderRound();
    }
    function setMission(q, sub, eddyText, hint) {
        question.textContent = q; subquestion.textContent = sub || '';
        setEddy(eddyText, false); narrate(q); if (hint) hintAfter(hint);
    }

    function renderObjectCounting() {
        var count = rnd(3, 10), tapped = 0;
        setMission(
            localized('Touch each star once. How many are there?', 'Dotknij każdej gwiazdki jeden raz. Ile ich jest?', 'المس كل نجمة مرة واحدة. كم عددها؟'),
            localized('Count carefully without touching the same star twice.', 'Licz uważnie i nie dotykaj tej samej gwiazdki dwa razy.', 'عد بعناية ولا تلمس النجمة نفسها مرتين.'),
            localized('Give every star one count number.', 'Nadaj każdej gwiazdce jeden numer podczas liczenia.', 'أعط كل نجمة رقم عد واحداً.'),
            localized('Touch one object, say one number, then move to the next.', 'Dotknij jednego obiektu, powiedz jedną liczbę i przejdź dalej.', 'المس شيئاً واحداً وقل رقماً واحداً ثم انتقل للتالي.')
        );
        var field = document.createElement('div'); field.className = 'gw-object-field';
        for (var i = 0; i < count; i += 1) {
            var item = document.createElement('button'); item.type = 'button'; item.className = 'gw-count-object'; item.textContent = '✦';
            item.style.left = rnd(7, 87) + '%'; item.style.top = rnd(8, 76) + '%';
            item.addEventListener('click', function () {
                if (state.locked || this.classList.contains('is-counted')) return;
                tapped += 1; this.classList.add('is-counted'); this.setAttribute('data-count-label', tapped);
                if (tapped === count) {
                    setEddy(localized('Great. Now choose the total.', 'Świetnie. Teraz wybierz liczbę wszystkich gwiazdek.', 'رائع. الآن اختر العدد الكلي.'), true);
                }
            });
            field.appendChild(item);
        }
        board.appendChild(field);
        var answers = [count, Math.max(0, count - 1), count + 1];
        var row = document.createElement('div'); row.className = 'gw-choice-row gw-runtime-choices';
        shuffle(answers).forEach(function (value) {
            var btn = makeChoice(String(value), value); row.appendChild(btn);
        });
        row.addEventListener('click', function (event) {
            if (state.locked) return;
            var btn = event.target.closest('[data-runtime-answer]'); if (!btn) return;
            if (tapped !== count) { fail(btn, localized('Touch every star before choosing the total.', 'Najpierw dotknij wszystkich gwiazdek.', 'المس كل النجوم أولاً قبل اختيار العدد.')); return; }
            if (Number(btn.getAttribute('data-runtime-answer')) === count) succeed(btn, localized('Every object was counted exactly once.', 'Każdy obiekt został policzony dokładnie raz.', 'تم عد كل عنصر مرة واحدة بالضبط.'));
            else fail(btn, localized('Use the count labels to check your last number.', 'Sprawdź ostatni numer przy policzonych gwiazdkach.', 'راجع آخر رقم ظهر أثناء العد.'));
        });
        board.appendChild(row);
    }

    function renderNumberSystem() {
        var target = rnd(12, 96), start = Math.max(0, target - rnd(4, 9)), end = target + rnd(4, 9);
        var mode = mechanic.indexOf('PLACE_VALUE') >= 0 || mechanic === 'SYMBOL_VALUE_MATCH' ? 'place' : 'line';
        if (mode === 'place') {
            var tens = Math.floor(target / 10), ones = target % 10;
            setMission(
                localized('Build ' + target + ' using tens and ones.', 'Zbuduj liczbę ' + target + ' z dziesiątek i jedności.', 'كوّن العدد ' + target + ' باستخدام العشرات والآحاد.'),
                localized('Choose the matching place-value model.', 'Wybierz pasujący model wartości miejscowej.', 'اختر نموذج القيمة المكانية الصحيح.'),
                localized('Think about how many full tens and extra ones are needed.', 'Pomyśl, ile pełnych dziesiątek i dodatkowych jedności potrzeba.', 'فكر في عدد العشرات الكاملة والآحاد الإضافية.'),
                localized('The first digit tells the tens. The second digit tells the ones.', 'Pierwsza cyfra to dziesiątki, druga to jedności.', 'الرقم الأول يمثل العشرات والثاني يمثل الآحاد.')
            );
            var models = [[tens, ones], [Math.max(0,tens-1), ones], [tens, (ones+2)%10]];
            var grid = document.createElement('div'); grid.className = 'gw-model-grid';
            shuffle(models).forEach(function (m) {
                var card = document.createElement('button'); card.type='button'; card.className='gw-model-card';
                card.setAttribute('data-model-answer', m[0] === tens && m[1] === ones ? 'yes' : 'no');
                card.innerHTML = '<div class="gw-place-model"><span>' + new Array(m[0] + 1).join('▮') + '</span><b>' + new Array(m[1] + 1).join('●') + '</b></div><strong>' + m[0] + ' tens + ' + m[1] + ' ones</strong>';
                grid.appendChild(card);
            });
            grid.addEventListener('click', function(e){ var b=e.target.closest('[data-model-answer]'); if(!b||state.locked)return; if(b.getAttribute('data-model-answer')==='yes')succeed(b,localized('That model has the right tens and ones.','Ten model ma właściwe dziesiątki i jedności.','هذا النموذج يحتوي على العدد الصحيح من العشرات والآحاد.'));else fail(b,localized('Count the tens first, then the ones.','Najpierw policz dziesiątki, potem jedności.','عد العشرات أولاً ثم الآحاد.')); });
            board.appendChild(grid);
        } else {
            setMission(
                localized('Place ' + target + ' on the number line.', 'Umieść ' + target + ' na osi liczbowej.', 'ضع ' + target + ' على خط الأعداد.'),
                localized('Move the marker to the correct value.', 'Przesuń znacznik na właściwą wartość.', 'حرّك المؤشر إلى القيمة الصحيحة.'),
                localized('Use the labelled endpoints and count the intervals.', 'Użyj opisanych końców i policz odcinki.', 'استخدم النهايات المعلّمة وعد الفواصل.'),
                localized('Each step changes the value by one.', 'Każdy krok zmienia wartość o jeden.', 'كل خطوة تغيّر القيمة بمقدار واحد.')
            );
            var wrap = document.createElement('div'); wrap.className='gw-slider-card';
            wrap.innerHTML='<div class="gw-number-line"><span>'+start+'</span><input type="range" min="'+start+'" max="'+end+'" value="'+Math.floor((start+end)/2)+'" step="1" data-runtime-range/><span>'+end+'</span></div><output data-runtime-output></output><button type="button" class="gw-primary" data-runtime-check>'+esc(copy.check)+'</button>';
            board.appendChild(wrap);
            var slider=wrap.querySelector('[data-runtime-range]'), out=wrap.querySelector('[data-runtime-output]'); out.textContent=slider.value;
            slider.addEventListener('input',function(){out.textContent=slider.value;});
            wrap.querySelector('[data-runtime-check]').addEventListener('click',function(){if(Number(slider.value)===target)succeed(this,localized('The marker is on the target number.','Znacznik jest na właściwej liczbie.','المؤشر على العدد المطلوب.'));else fail(this,localized('Count the intervals from the nearest labelled value.','Policz odcinki od najbliższej opisanej liczby.','عد الفواصل من أقرب قيمة معلّمة.'));});
        }
    }

    function renderOperations() {
        var op = mechanic;
        var a, b, answer, symbol, visual;
        if (op === 'EQUAL_GROUPS_ARRAY' || op === 'FACTOR_MULTIPLE_ARRAY') { a=rnd(2,6); b=rnd(2,6); answer=a*b; symbol='×'; visual=groupsHtml(a,b); }
        else if (op === 'SHARE_DIVIDE' || op === 'SHARE_REMAINDER') { b=rnd(2,5); answer=rnd(2,6); a=b*answer; symbol='÷'; visual=groupsHtml(b,answer); }
        else if (op === 'TAKE_AWAY') { a=rnd(7,20); b=rnd(2,a-2); answer=a-b; symbol='−'; visual=dotsHtml(a,b); }
        else { a=rnd(2,12); b=rnd(2,12); answer=a+b; symbol='+'; visual=groupsHtml(2,0,a,b); }
        setMission(
            localized('Use the model to solve ' + a + ' ' + symbol + ' ' + b + '.', 'Użyj modelu, aby obliczyć ' + a + ' ' + symbol + ' ' + b + '.', 'استخدم النموذج لحل ' + a + ' ' + symbol + ' ' + b + '.'),
            localized('The objects show the mathematical action.', 'Obiekty pokazują działanie matematyczne.', 'تُظهر العناصر العملية الرياضية.'),
            localized('Read the action in the model before calculating.', 'Najpierw odczytaj działanie z modelu, potem oblicz.', 'اقرأ العملية في النموذج قبل الحساب.'),
            localized('Group, join, remove or share the objects to match the operation.', 'Połącz, usuń lub podziel obiekty zgodnie z działaniem.', 'اجمع أو أزل أو قسّم العناصر بما يطابق العملية.')
        );
        var model=document.createElement('div');model.className='gw-operation-model';model.innerHTML=visual+'<div class="gw-equation">'+a+' '+symbol+' '+b+' = ?</div>';board.appendChild(model);
        choices([answer, Math.max(0,answer-1), answer+1], answer, localized('The model and equation agree.','Model i działanie dają ten sam wynik.','النموذج والمعادلة يعطيان النتيجة نفسها.'));
    }

    function groupsHtml(groups, each, left, right) {
        var html='<div class="gw-groups">';
        if (left !== undefined) {
            html+='<div class="gw-group">'+new Array(left+1).join('<i>●</i>')+'</div><span class="gw-op-sign">+</span><div class="gw-group">'+new Array(right+1).join('<i>●</i>')+'</div>';
        } else {
            for(var g=0;g<groups;g+=1) html+='<div class="gw-group">'+new Array(each+1).join('<i>●</i>')+'</div>';
        }
        return html+'</div>';
    }
    function dotsHtml(total, removed) { return '<div class="gw-dot-strip">'+Array.from({length:total},function(_,i){return '<i class="'+(i>=total-removed?'is-removed':'')+'">●</i>';}).join('')+'</div>'; }

    function renderFraction() {
        var denom = shuffle([2,3,4,5,6,8])[0], num = rnd(1, denom-1), factor = shuffle([2,3])[0];
        var targetNum=num*factor,targetDen=denom*factor;
        var mode = mechanic === 'PERCENT_MODEL' ? 'percent' : (mechanic.indexOf('DECIMAL')>=0 ? 'decimal' : 'fraction');
        if(mode==='percent') { denom=100; num=shuffle([10,20,25,50,75])[0]; targetNum=num; targetDen=100; }
        setMission(
            mode==='percent' ? localized('Shade ' + num + '% of the bar.', 'Zaznacz ' + num + '% paska.', 'ظلّل ' + num + '% من الشريط.') : localized('Find a model equal to ' + num + '/' + denom + '.', 'Znajdź model równy ' + num + '/' + denom + '.', 'اختر نموذجاً يساوي ' + num + '/' + denom + '.'),
            localized('Compare the size of the shaded part, not only the numbers.', 'Porównaj wielkość zacieniowanej części, nie tylko liczby.', 'قارن حجم الجزء المظلّل وليس الأرقام فقط.'),
            localized('Equal fractions cover the same share of a whole.', 'Ułamki równoważne zajmują taką samą część całości.', 'الكسور المتكافئة تغطي الجزء نفسه من الكل.'),
            localized('Multiply or divide numerator and denominator by the same number.', 'Pomnóż lub podziel licznik i mianownik przez tę samą liczbę.', 'اضرب أو اقسم البسط والمقام على العدد نفسه.')
        );
        var target=document.createElement('div');target.className='gw-fraction-target';target.innerHTML='<div class="gw-frac-label">'+num+'/'+denom+'</div>'+fractionBar(num,denom);board.appendChild(target);
        var opts=document.createElement('div');opts.className='gw-fraction-options';
        var candidates=mode==='percent'?[[num,100],[Math.max(0,num-10),100],[Math.min(100,num+10),100]]:[[targetNum,targetDen],[Math.max(1,targetNum-1),targetDen],[Math.min(targetDen-1,targetNum+1),targetDen]];
        shuffle(candidates).forEach(function(c){var b=document.createElement('button');b.type='button';b.className='gw-fraction-option';var equal=(c[0]*denom===num*c[1]);b.setAttribute('data-frac-correct',equal?'yes':'no');b.innerHTML='<div class="gw-frac-label">'+c[0]+'/'+c[1]+'</div>'+fractionBar(c[0],c[1]);opts.appendChild(b);});
        opts.addEventListener('click',function(e){var b=e.target.closest('[data-frac-correct]');if(!b||state.locked)return;if(b.getAttribute('data-frac-correct')==='yes')succeed(b,localized('Both models cover the same share of the whole.','Oba modele pokazują tę samą część całości.','النموذجان يغطيان الجزء نفسه من الكل.'));else fail(b,localized('Compare the shaded proportion of each bar.','Porównaj proporcję zacieniowanej części każdego paska.','قارن نسبة الجزء المظلّل في كل شريط.'));});board.appendChild(opts);
    }
    function fractionBar(n,d){var cells='';for(var i=0;i<d;i+=1)cells+='<span class="gw-frac-segment '+(i<n?'filled':'')+'"></span>';return '<div class="gw-frac-bar" style="grid-template-columns:repeat('+d+',1fr)">'+cells+'</div>';}

    function renderGeometry() {
        if (mechanic === 'PERIMETER_TRACE' || mechanic === 'AREA_TILE' || mechanic === 'SURFACE_AREA_BUILD' || mechanic === 'VOLUME_BUILD') { renderGeometryMeasure(); return; }
        if (mechanic === 'ANGLE_LAB') { renderAngleMini(); return; }
        var shapeSet=[{name:localized('triangle','trójkąt','مثلث'),sides:3,html:'△'},{name:localized('quadrilateral','czworokąt','شكل رباعي'),sides:4,html:'◇'},{name:localized('pentagon','pięciokąt','خماسي'),sides:5,html:'⬠'}];
        var target=shuffle(shapeSet)[0];
        setMission(localized('Find the shape with ' + target.sides + ' sides.', 'Znajdź figurę o ' + target.sides + ' bokach.', 'اختر الشكل الذي له ' + target.sides + ' أضلاع.'),localized('Use geometric properties, not the shape’s size or colour.','Użyj własności geometrycznych, a nie wielkości lub koloru.','استخدم خصائص الشكل الهندسية وليس حجمه أو لونه.'),localized('Count straight sides and corners carefully.','Policz uważnie proste boki i wierzchołki.','عد الأضلاع المستقيمة والزوايا بعناية.'),localized('Trace around the boundary and count each side once.','Obejdź brzeg figury i policz każdy bok jeden raz.','تتبع حدود الشكل وعد كل ضلع مرة واحدة.'));
        var gallery=document.createElement('div');gallery.className='gw-shape-gallery';shuffle(shapeSet).forEach(function(s){var b=document.createElement('button');b.type='button';b.className='gw-shape-card';b.innerHTML='<span>'+s.html+'</span><strong>'+esc(s.name)+'</strong>';b.setAttribute('data-shape-sides',s.sides);gallery.appendChild(b);});gallery.addEventListener('click',function(e){var b=e.target.closest('[data-shape-sides]');if(!b||state.locked)return;if(Number(b.getAttribute('data-shape-sides'))===target.sides)succeed(b,localized('You used the number of sides as a defining property.','Użyłeś liczby boków jako własności figury.','استخدمت عدد الأضلاع كخاصية للشكل.'));else fail(b,localized('Count the straight sides again.','Policz proste boki jeszcze raz.','عد الأضلاع المستقيمة مرة أخرى.'));});board.appendChild(gallery);
    }

    function renderAngleMini(){var target=shuffle([30,45,60,90,120,135,150])[0];setMission(localized('Set the ray to ' + target + '°.', 'Ustaw promień na ' + target + '°.', 'اضبط الشعاع على ' + target + '°.'),localized('Use the slider like a protractor control.','Użyj suwaka jak kątomierza.','استخدم شريط التمرير مثل المنقلة.'),localized('Watch how the opening changes as the angle grows.','Obserwuj, jak zmienia się rozwarcie kąta.','لاحظ كيف يتغير اتساع الزاوية.'),localized('Compare with 90° as a landmark.','Porównaj z kątem 90° jako punktem odniesienia.','قارن مع 90° كنقطة مرجعية.'));var card=document.createElement('div');card.className='gw-angle-mini';card.innerHTML='<div class="gw-mini-angle"><i></i><b data-mini-ray></b></div><input type="range" min="10" max="170" step="5" value="45" data-angle-range/><output data-angle-out>45°</output><button class="gw-primary" type="button" data-angle-check>'+esc(copy.check)+'</button>';board.appendChild(card);var r=card.querySelector('[data-angle-range]'),out=card.querySelector('[data-angle-out]'),ray=card.querySelector('[data-mini-ray]');function draw(){out.textContent=r.value+'°';ray.style.transform='rotate(-'+r.value+'deg)';}r.addEventListener('input',draw);draw();card.querySelector('[data-angle-check]').addEventListener('click',function(){if(Math.abs(Number(r.value)-target)<=5)succeed(this,localized('Your ray is on the target angle.','Promień jest ustawiony na właściwy kąt.','الشعاع مضبوط على الزاوية المطلوبة.'));else fail(this,localized('Adjust the opening and compare the degree reading.','Popraw rozwarcie i porównaj odczyt stopni.','عدّل اتساع الزاوية وقارن قراءة الدرجات.'));});}

    function renderGeometryMeasure(){var w=rnd(3,8),h=rnd(2,6),answer=mechanic==='PERIMETER_TRACE'?2*(w+h):w*h;var label=mechanic==='PERIMETER_TRACE'?localized('perimeter','obwód','المحيط'):localized('area','pole','المساحة');setMission(localized('Find the ' + label + ' of the rectangle.', 'Oblicz ' + label + ' prostokąta.', 'احسب ' + label + ' للمستطيل.'),localized('The rectangle is ' + w + ' by ' + h + ' units.', 'Prostokąt ma wymiary ' + w + ' na ' + h + ' jednostek.', 'أبعاد المستطيل ' + w + ' × ' + h + ' وحدات.'),localized('Use the boundary for perimeter and the inside tiles for area.','Dla obwodu użyj brzegu, a dla pola płytek wewnątrz.','استخدم الحدود للمحيط والبلاطات الداخلية للمساحة.'),localized('Perimeter goes around. Area covers the inside.','Obwód biegnie wokół figury. Pole pokrywa jej wnętrze.','المحيط حول الشكل، والمساحة تغطي داخله.'));var grid=document.createElement('div');grid.className='gw-area-grid';grid.style.gridTemplateColumns='repeat('+w+',1fr)';for(var i=0;i<w*h;i+=1){var c=document.createElement('span');c.className='gw-area-cell';grid.appendChild(c);}var card=document.createElement('div');card.className='gw-geometry-measure';card.innerHTML='<div class="gw-dimension gw-dim-top">'+w+'</div><div class="gw-dimension gw-dim-side">'+h+'</div>';card.appendChild(grid);board.appendChild(card);choices([answer,answer+w,Math.max(1,answer-h)],answer,localized('The model matches the calculation.','Model zgadza się z obliczeniem.','النموذج يطابق الحساب.'));}

    function renderMeasurement(){var target=rnd(2,18), max=20;setMission(localized('Measure the object to ' + target + ' units.', 'Zmierz obiekt do ' + target + ' jednostek.', 'قِس العنصر إلى ' + target + ' وحدات.'),localized('Align the marker with the scale.', 'Wyrównaj znacznik ze skalą.', 'حاذِ المؤشر مع التدريج.'),localized('Start from zero and read the end point.', 'Zacznij od zera i odczytaj punkt końcowy.', 'ابدأ من الصفر واقرأ نقطة النهاية.'),localized('Count intervals, not just tick marks.','Licz odcinki, nie tylko kreski podziałki.','عد الفواصل وليس علامات التدريج فقط.'));var card=document.createElement('div');card.className='gw-ruler-card';var ticks='';for(var i=0;i<=max;i+=1)ticks+='<span><i></i><b>'+(i%5===0?i:'')+'</b></span>';card.innerHTML='<div class="gw-ruler">'+ticks+'</div><input type="range" min="0" max="'+max+'" step="1" value="5" data-measure-range/><output data-measure-out>5</output><button class="gw-primary" type="button" data-measure-check>'+esc(copy.check)+'</button>';board.appendChild(card);var r=card.querySelector('[data-measure-range]'),out=card.querySelector('[data-measure-out]');r.addEventListener('input',function(){out.textContent=r.value;});card.querySelector('[data-measure-check]').addEventListener('click',function(){if(Number(r.value)===target)succeed(this,localized('The marker is aligned with the correct measurement.','Znacznik wskazuje właściwy pomiar.','المؤشر بمحاذاة القياس الصحيح.'));else fail(this,localized('Start at zero and count each interval to the marker.','Zacznij od zera i policz odcinki do znacznika.','ابدأ من الصفر وعد كل فاصل حتى المؤشر.'));});}

    function renderTimeMoney(){if(mechanic.indexOf('MONEY')>=0||mechanic==='MAKE_AMOUNT'||mechanic==='CHANGE_TRANSACTION'){renderMoney();return;}var hour=rnd(1,12),minute=shuffle([0,15,30,45])[0];setMission(localized('Set the clock to ' + hour + ':' + String(minute).padStart(2,'0') + '.', 'Ustaw zegar na ' + hour + ':' + String(minute).padStart(2,'0') + '.', 'اضبط الساعة على ' + hour + ':' + String(minute).padStart(2,'0') + '.'),localized('Move the minute hand by quarter-hour steps.', 'Przesuwaj wskazówkę minutową co kwadrans.', 'حرّك عقرب الدقائق بخطوات ربع ساعة.'),localized('The short hand shows hours; the long hand shows minutes.','Krótka wskazówka pokazuje godziny, długa minuty.','العقرب القصير للساعات والطويل للدقائق.'),localized('15 minutes is a quarter turn around the clock.','15 minut to ćwierć obrotu zegara.','15 دقيقة تساوي ربع دورة حول الساعة.'));var card=document.createElement('div');card.className='gw-clock-card';card.innerHTML='<div class="gw-clock"><i class="gw-clock-hour" data-clock-hour></i><i class="gw-clock-minute" data-clock-minute></i><b>12</b><em>3</em><strong>6</strong><small>9</small></div><div class="gw-clock-controls"><input type="range" min="1" max="12" value="6" data-hour/><input type="range" min="0" max="45" step="15" value="0" data-minute/><output data-clock-out></output><button class="gw-primary" data-clock-check>'+esc(copy.check)+'</button></div>';board.appendChild(card);var hr=card.querySelector('[data-hour]'),mn=card.querySelector('[data-minute]'),out=card.querySelector('[data-clock-out]'),hh=card.querySelector('[data-clock-hour]'),mh=card.querySelector('[data-clock-minute]');function draw(){out.textContent=hr.value+':'+String(mn.value).padStart(2,'0');hh.style.transform='rotate('+(Number(hr.value)*30+Number(mn.value)/2)+'deg)';mh.style.transform='rotate('+(Number(mn.value)*6)+'deg)';}hr.addEventListener('input',draw);mn.addEventListener('input',draw);draw();card.querySelector('[data-clock-check]').addEventListener('click',function(){if(Number(hr.value)===hour&&Number(mn.value)===minute)succeed(this,localized('Both hands show the target time.','Obie wskazówki pokazują właściwy czas.','العقربان يظهران الوقت المطلوب.'));else fail(this,localized('Check the hour hand and minute hand separately.','Sprawdź osobno wskazówkę godzinową i minutową.','تحقق من عقرب الساعات وعقرب الدقائق كلٌ على حدة.'));});}
    function renderMoney(){var target=shuffle([20,30,40,50,60,75,90])[0],coins=[5,10,20,25,50],total=0;setMission(localized('Make ' + target + ' using the coins.', 'Ułóż kwotę ' + target + ' za pomocą monet.', 'كوّن المبلغ ' + target + ' باستخدام العملات.'),localized('Tap coins to add them to your purse.', 'Dotykaj monet, aby dodać je do portfela.', 'اضغط العملات لإضافتها إلى محفظتك.'),localized('Combine coin values until the total matches the target.','Łącz wartości monet, aż suma będzie równa celowi.','اجمع قيم العملات حتى يساوي المجموع الهدف.'),localized('You can use the same coin value more than once.','Możesz użyć tej samej wartości monety więcej niż raz.','يمكنك استخدام قيمة العملة نفسها أكثر من مرة.'));var card=document.createElement('div');card.className='gw-money-card';var tray=document.createElement('div');tray.className='gw-coin-tray';coins.forEach(function(v){var b=document.createElement('button');b.type='button';b.className='gw-coin';b.textContent=v;b.addEventListener('click',function(){if(state.locked)return;total+=v;sum.textContent=total;});tray.appendChild(b);});var sum=document.createElement('output');sum.className='gw-money-sum';sum.textContent='0';var reset=document.createElement('button');reset.type='button';reset.className='gw-secondary';reset.textContent='↺';reset.addEventListener('click',function(){total=0;sum.textContent='0';});var check=document.createElement('button');check.type='button';check.className='gw-primary';check.textContent=copy.check;check.addEventListener('click',function(){if(total===target)succeed(this,localized('Your coin values add to the target amount.','Wartości monet dają właściwą sumę.','مجموع قيم العملات يساوي المبلغ المطلوب.'));else fail(this,localized('Add the coin values and compare with the target.','Dodaj wartości monet i porównaj z celem.','اجمع قيم العملات وقارنها بالمبلغ المطلوب.'));});card.appendChild(tray);card.appendChild(sum);card.appendChild(reset);card.appendChild(check);board.appendChild(card);}

    function renderData(){var values=[rnd(2,7),rnd(2,7),rnd(2,7)],labels=locale==='ar'?['أ','ب','ج']:['A','B','C'];var max=Math.max.apply(Math,values),targetIndex=values.indexOf(max);setMission(localized('Which category has the greatest value?', 'Która kategoria ma największą wartość?', 'أي فئة لها أكبر قيمة؟'),localized('Read the height of each bar.', 'Odczytaj wysokość każdego słupka.', 'اقرأ ارتفاع كل عمود.'),localized('A taller bar represents a larger value.','Wyższy słupek oznacza większą wartość.','العمود الأعلى يمثل قيمة أكبر.'),localized('Compare each bar against the same scale.','Porównaj każdy słupek na tej samej skali.','قارن كل عمود على المقياس نفسه.'));var chart=document.createElement('div');chart.className='gw-bar-chart';values.forEach(function(v,i){var col=document.createElement('button');col.type='button';col.className='gw-bar-column';col.setAttribute('data-bar-index',i);col.innerHTML='<span style="height:'+(v*30)+'px"><b>'+v+'</b></span><strong>'+labels[i]+'</strong>';chart.appendChild(col);});chart.addEventListener('click',function(e){var b=e.target.closest('[data-bar-index]');if(!b||state.locked)return;if(Number(b.getAttribute('data-bar-index'))===targetIndex)succeed(b,localized('You compared all bars on the same scale.','Porównałeś wszystkie słupki na tej samej skali.','قارنت جميع الأعمدة على المقياس نفسه.'));else fail(b,localized('Look for the highest bar and check its value.','Znajdź najwyższy słupek i sprawdź jego wartość.','ابحث عن أعلى عمود وتحقق من قيمته.'));});board.appendChild(chart);}

    function renderRatioAlgebra(){if(mechanic==='RATIO_SCALE'||mechanic==='DOUBLE_NUMBER_LINE'){var a=rnd(2,5),b=rnd(2,5),mult=rnd(2,4),ans=b*mult;setMission(localized('Scale the ratio ' + a + ':' + b + ' by ' + mult + '.', 'Powiększ proporcję ' + a + ':' + b + ' ' + mult + ' razy.', 'كبّر النسبة ' + a + ':' + b + ' بمقدار ' + mult + ' مرات.'),localized('Both parts must be multiplied by the same factor.','Obie części trzeba pomnożyć przez ten sam współczynnik.','يجب ضرب الجزأين في العامل نفسه.'),localized('A ratio stays equivalent when both quantities scale together.','Proporcja pozostaje równoważna, gdy obie wielkości zmieniają się razem.','تبقى النسبة مكافئة عندما تتغير الكميتان معاً.'),localized('Multiply both sides by ' + mult + '.', 'Pomnóż obie części przez ' + mult + '.', 'اضرب الطرفين في ' + mult + '.'));var model=document.createElement('div');model.className='gw-ratio-model';model.innerHTML='<div><span>'+a+'</span><b>:</b><span>'+b+'</span></div><i>× '+mult+'</i><div><span>'+(a*mult)+'</span><b>:</b><span>?</span></div>';board.appendChild(model);choices([ans,ans+b,Math.max(1,ans-b)],ans,localized('Both quantities were scaled by the same factor.','Obie wielkości pomnożono przez ten sam współczynnik.','تم تكبير الكميتين بالعامل نفسه.'));return;}var x=rnd(2,9),add=rnd(2,8),total=x+add;setMission(localized('Balance the equation: x + ' + add + ' = ' + total + '.', 'Zrównoważ równanie: x + ' + add + ' = ' + total + '.', 'وازن المعادلة: x + ' + add + ' = ' + total + '.'),localized('Find the value that makes both sides equal.', 'Znajdź wartość, która sprawia, że obie strony są równe.', 'أوجد القيمة التي تجعل الطرفين متساويين.'),localized('A balanced equation has the same value on both sides.','W zrównoważonym równaniu obie strony mają tę samą wartość.','المعادلة المتوازنة لها القيمة نفسها في الطرفين.'),localized('Undo the addition by subtracting ' + add + ' from ' + total + '.', 'Cofnij dodawanie, odejmując ' + add + ' od ' + total + '.', 'اعكس الجمع بطرح ' + add + ' من ' + total + '.'));var balance=document.createElement('div');balance.className='gw-balance-model';balance.innerHTML='<div>x + '+add+'</div><span>⚖</span><div>'+total+'</div>';board.appendChild(balance);choices([x,x+1,Math.max(0,x-1)],x,localized('Both sides now have equal value.','Obie strony mają teraz tę samą wartość.','الطرفان الآن لهما القيمة نفسها.'));}

    function renderReasoning(){var a=rnd(3,9),b=rnd(2,7),answer=a+b;setMission(localized('Choose the model that matches the story.', 'Wybierz model pasujący do sytuacji.', 'اختر النموذج الذي يطابق القصة.'),localized('There are ' + a + ' items, then ' + b + ' more join.', 'Jest ' + a + ' elementów, potem dołącza jeszcze ' + b + '.', 'يوجد ' + a + ' عناصر ثم ينضم ' + b + ' أخرى.'),localized('Decide what changes in the story before choosing a calculation.', 'Najpierw ustal, co zmienia się w sytuacji, potem wybierz działanie.', 'حدد ما الذي يتغير في القصة قبل اختيار العملية.'),localized('If more items join, the total becomes larger.','Gdy dochodzą elementy, suma rośnie.','عندما تنضم عناصر إضافية يزداد المجموع.'));var models=[{label:a+' + '+b,value:'join'},{label:a+' − '+b,value:'leave'},{label:a+' × '+b,value:'groups'}];choices(models,'join',localized('The join model matches the change in the story.','Model dodawania pasuje do zmiany w sytuacji.','نموذج الجمع يطابق التغير في القصة.'));}

    function renderComposite(){var options=['OPERATIONS','FRACTION_DECIMAL_PERCENT','GEOMETRY'];var pick=options[state.roundIndex%options.length];var oldWorkspace=workspace;workspace=pick;if(pick==='OPERATIONS')renderOperations();else if(pick==='FRACTION_DECIMAL_PERCENT')renderFraction();else renderGeometry();workspace=oldWorkspace;}

    function renderRound(){clearHint();state.locked=false;state.selected=null;board.innerHTML='';updateHud();switch(workspace){case'OBJECT_COUNTING':renderObjectCounting();break;case'NUMBER_SYSTEM':renderNumberSystem();break;case'OPERATIONS':renderOperations();break;case'FRACTION_DECIMAL_PERCENT':renderFraction();break;case'GEOMETRY':renderGeometry();break;case'MEASUREMENT':renderMeasurement();break;case'TIME_MONEY':renderTimeMoney();break;case'DATA_STATISTICS':renderData();break;case'RATIO_ALGEBRA':renderRatioAlgebra();break;case'COMPOSITE_SESSION':renderComposite();break;default:renderReasoning();break;}}

    soundButton.addEventListener('click',function(){state.soundOn=!state.soundOn;soundButton.textContent=state.soundOn?'🔊':'🔇';if(!state.soundOn&&window.speechSynthesis)window.speechSynthesis.cancel();});
    fullscreenButton.addEventListener('click',function(){if(!document.fullscreenElement&&game.requestFullscreen)game.requestFullscreen();else if(document.exitFullscreen)document.exitFullscreen();});
    root.querySelector('[data-gw-replay]').addEventListener('click',function(){state.roundIndex=0;state.score=0;complete.hidden=true;renderRound();});

    renderRound();
}());