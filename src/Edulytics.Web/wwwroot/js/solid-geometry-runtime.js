(function () {
    'use strict';

    var root = document.querySelector('[data-curriculum-workspace-game]');
    if (!root) return;
    var mechanic = root.getAttribute('data-mechanic');
    if (mechanic !== 'VOLUME_BUILD' && mechanic !== 'SURFACE_AREA_BUILD') return;

    var locale = root.getAttribute('data-lesson-language') || 'en';
    var guide = '/images/game/v9/eddy-guide.webp';
    var isSurface = mechanic === 'SURFACE_AREA_BUILD';
    var state = { round: 0, rounds: 8, score: 0, soundOn: true, locked: false, timer: null };

    function t(en, pl, ar) { return locale === 'pl' ? pl : (locale === 'ar' ? ar : en); }
    function rnd(a, b) { return Math.floor(Math.random() * (b - a + 1)) + a; }
    function shuffle(a) { a = a.slice(); for (var i=a.length-1;i>0;i-=1){var j=Math.floor(Math.random()*(i+1)),x=a[i];a[i]=a[j];a[j]=x;} return a; }
    function narrate(text) { if(!state.soundOn||!window.speechSynthesis||!window.SpeechSynthesisUtterance)return; try{window.speechSynthesis.cancel();var u=new SpeechSynthesisUtterance(text);u.lang=locale==='pl'?'pl-PL':(locale==='ar'?'ar-AE':'en-GB');u.rate=.92;u.pitch=1.1;window.speechSynthesis.speak(u);}catch(e){} }

    root.innerHTML = '<section class="gw-game gw-solid-game">' +
        '<header class="gw-hud"><div class="gw-brand"><span class="gw-brand-mark">▱</span><span><strong>'+t('SOLID GEOMETRY LAB','LABORATORIUM BRYŁ','مختبر المجسمات')+'</strong><small>'+t('Build, inspect, calculate','Buduj, oglądaj, obliczaj','ابنِ وافحص واحسب')+'</small></span></div><div class="gw-hud-right"><div class="gw-pill">★ <span data-score>0</span></div><div class="gw-pill" data-round>1 / 8</div><button class="gw-icon" data-sound>🔊</button><button class="gw-icon" data-full>⛶</button></div></header>' +
        '<div class="gw-progress"><span data-progress></span></div>' +
        '<main class="gw-stage"><div class="gw-mission"><span>'+t('YOUR MISSION','TWOJE ZADANIE','مهمتك')+'</span><strong data-question></strong><small data-sub></small></div><div data-board></div></main>' +
        '<div class="gw-eddy"><img src="'+guide+'" alt="Eddy"><div><span>EDDY</span><p data-eddy></p></div></div>' +
        '<div class="gw-complete" data-complete hidden><div class="gw-complete-card"><img src="'+guide+'" alt="Eddy"><p>'+t('ADVENTURE COMPLETE','MISJA UKOŃCZONA','اكتملت المهمة')+'</p><h2>'+t('Solid geometry mastered!','Bryły opanowane!','أتقنت المجسمات!')+'</h2><p data-final></p><button class="gw-primary" data-replay>'+t('Play again','Zagraj ponownie','العب مرة أخرى')+'</button></div></div>' +
        '</section>';

    var game=root.querySelector('.gw-game'),board=root.querySelector('[data-board]'),q=root.querySelector('[data-question]'),sub=root.querySelector('[data-sub]'),eddy=root.querySelector('[data-eddy]'),score=root.querySelector('[data-score]'),round=root.querySelector('[data-round]'),progress=root.querySelector('[data-progress]'),complete=root.querySelector('[data-complete]');

    function setEddy(text,speak){eddy.textContent=text;if(speak)narrate(text);}
    function clearTimer(){if(state.timer){clearTimeout(state.timer);state.timer=null;}}
    function hint(text){clearTimer();state.timer=setTimeout(function(){if(!state.locked)setEddy(t('Hint: ','Wskazówka: ','تلميح: ')+text,true);},9000);}
    function prismHtml(l,w,h){
        var cells='',layers=Math.min(h,5),cols=Math.min(l,7),rows=Math.min(w,6);
        for(var z=0;z<layers;z+=1){cells+='<div class="gw-cube-layer" style="--layer:'+z+'">';for(var y=0;y<rows;y+=1){for(var x=0;x<cols;x+=1)cells+='<i></i>';}cells+='</div>';}
        return '<div class="gw-solid-model" style="--cols:'+cols+';--rows:'+rows+'">'+cells+'</div>';
    }
    function netHtml(l,w,h){return '<div class="gw-net"><span style="--a:'+l+';--b:'+w+'">'+l+'×'+w+'</span><span style="--a:'+l+';--b:'+h+'">'+l+'×'+h+'</span><span style="--a:'+w+';--b:'+h+'">'+w+'×'+h+'</span><span style="--a:'+l+';--b:'+w+'">'+l+'×'+w+'</span><span style="--a:'+l+';--b:'+h+'">'+l+'×'+h+'</span><span style="--a:'+w+';--b:'+h+'">'+w+'×'+h+'</span></div>';}
    function makeChoice(v){var b=document.createElement('button');b.type='button';b.className='gw-choice';b.textContent=v;return b;}
    function success(b,msg){clearTimer();state.locked=true;state.score+=200;b.classList.add('is-correct');score.textContent=state.score;setEddy(t('Correct! ','Dobrze! ','صحيح! ')+msg,true);setTimeout(next,900);}
    function fail(b,msg){b.classList.add('is-wrong');setEddy(t('Try again. ','Spróbuj ponownie. ','حاول مرة أخرى. ')+msg,true);setTimeout(function(){b.classList.remove('is-wrong');},650);}
    function render(){
        clearTimer();state.locked=false;board.innerHTML='';round.textContent=(state.round+1)+' / '+state.rounds;progress.style.width=((state.round/state.rounds)*100)+'%';
        var l=rnd(2,6),w=rnd(2,5),h=rnd(2,5),answer=isSurface?2*(l*w+l*h+w*h):l*w*h;
        q.textContent=isSurface?t('Find the surface area of the prism.','Oblicz pole powierzchni graniastosłupa.','احسب المساحة السطحية للمنشور.'):t('Find the volume of the prism.','Oblicz objętość prostopadłościanu.','احسب حجم المنشور.');
        sub.textContent=t('Dimensions: ','Wymiary: ','الأبعاد: ')+l+' × '+w+' × '+h;
        setEddy(isSurface?t('A rectangular prism has three pairs of equal faces.','Prostopadłościan ma trzy pary równych ścian.','للمنشور المستطيل ثلاثة أزواج من الأوجه المتساوية.'):t('Volume counts how many unit cubes fill the solid.','Objętość mówi, ile sześcianów jednostkowych wypełnia bryłę.','الحجم يساوي عدد المكعبات الوحدة التي تملأ المجسم.'),false);
        hint(isSurface?t('Find l×w, l×h and w×h, then double their sum.','Oblicz l×w, l×h i w×h, a potem podwój ich sumę.','احسب l×w وl×h وw×h ثم ضاعف مجموعها.'):t('Multiply length × width × height.','Pomnóż długość × szerokość × wysokość.','اضرب الطول × العرض × الارتفاع.'));
        var model=document.createElement('div');model.className='gw-solid-workspace';model.innerHTML=isSurface?netHtml(l,w,h):prismHtml(l,w,h);board.appendChild(model);
        var formula=document.createElement('div');formula.className='gw-solid-formula';formula.textContent=isSurface?'2('+l+'×'+w+' + '+l+'×'+h+' + '+w+'×'+h+') = ?':l+' × '+w+' × '+h+' = ?';board.appendChild(formula);
        var row=document.createElement('div');row.className='gw-choice-row';shuffle([answer,answer+(isSurface?l+w:h),Math.max(1,answer-(isSurface?w:h))]).forEach(function(v){var b=makeChoice(v);b.addEventListener('click',function(){if(state.locked)return;if(v===answer)success(b,isSurface?t('You counted every face exactly once.','Policzyłeś każdą ścianę dokładnie raz.','حسبت كل وجه مرة واحدة بالضبط.'):t('You multiplied all three dimensions.','Pomnożyłeś wszystkie trzy wymiary.','ضربت الأبعاد الثلاثة.'));else fail(b,isSurface?t('Check all six faces: two of each rectangle.','Sprawdź sześć ścian: po dwie z każdego prostokąta.','تحقق من الأوجه الستة: وجهان من كل مستطيل.'):t('Use all three dimensions, not just the base area.','Użyj wszystkich trzech wymiarów, nie tylko pola podstawy.','استخدم الأبعاد الثلاثة وليس مساحة القاعدة فقط.'));});row.appendChild(b);});board.appendChild(row);narrate(q.textContent);
    }
    function next(){state.round+=1;if(state.round>=state.rounds){progress.style.width='100%';root.querySelector('[data-final]').textContent=t('Stars: ','Gwiazdki: ','النجوم: ')+state.score;complete.hidden=false;narrate(t('Mission complete!','Misja ukończona!','اكتملت المهمة!'));return;}render();}
    root.querySelector('[data-sound]').addEventListener('click',function(){state.soundOn=!state.soundOn;this.textContent=state.soundOn?'🔊':'🔇';if(!state.soundOn&&speechSynthesis)speechSynthesis.cancel();});
    root.querySelector('[data-full]').addEventListener('click',function(){if(!document.fullscreenElement&&game.requestFullscreen)game.requestFullscreen();else if(document.exitFullscreen)document.exitFullscreen();});
    root.querySelector('[data-replay]').addEventListener('click',function(){state.round=0;state.score=0;score.textContent='0';complete.hidden=true;render();});
    render();
}());