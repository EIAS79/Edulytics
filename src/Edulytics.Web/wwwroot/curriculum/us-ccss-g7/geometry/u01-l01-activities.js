'use strict';
// G7 U01:L01 first-party formative matching aid. No accounts, persistence,
// automatic grades, or publisher artwork. Student drawings remain teacher-reviewed.
(() => {
  const root = document.querySelector('[data-g7-u01-l01]');
  if (!root) return;
  const cards = [
    ['A',[[0,0],[2,0],[2,3],[0,3]],'F'],
    ['B',[[0,0],[3,0],[0,2]],'I'],
    ['C',[[0,0],[3,0],[3,1],[1,1],[1,3],[0,3]],'H'],
    ['D',[[0,0],[4,0],[3,2],[1,2]],'G'],
    ['E',[[0,0],[2,0],[3,1],[2,3],[0,2]],'J'],
    ['F',[[0,0],[4,0],[4,6],[0,6]],'A'],
    ['G',[[0,0],[6,0],[4.5,3],[1.5,3]],'D'],
    ['H',[[0,0],[6,0],[6,2],[2,2],[2,6],[0,6]],'C'],
    ['I',[[0,0],[4.5,0],[0,3]],'B'],
    ['J',[[0,0],[4,0],[6,2],[4,6],[0,4]],'E']
  ];
  const byId = new Map(cards.map(([id, points, pair]) => [id, {points, pair}]));
  const list = root.querySelector('[data-g7-cards]');
  const message = root.querySelector('[data-g7-feedback]');
  const explanation = root.querySelector('[data-g7-explanation]');
  const chosen = [];
  const svgNS = 'http://www.w3.org/2000/svg';
  const make = (tag, attrs) => {
    const e = document.createElementNS(svgNS, tag);
    for (const [key, value] of Object.entries(attrs)) e.setAttribute(key, String(value));
    return e;
  };
  for (const [id, points] of cards) {
    const button = document.createElement('button');
    button.type = 'button';
    button.className = 'school-button';
    button.setAttribute('aria-pressed', 'false');
    button.setAttribute('aria-label', 'Select polygon card '+id);
    button.dataset.g7Card = id;
    const svg = make('svg', {viewBox:'0 0 110 110',width:'130',height:'130',role:'img',
      'aria-label':'Card '+id+'. Polygon on a uniform unit grid.'});
    for(let n=10;n<=100;n+=10){
      svg.appendChild(make('path',{d:'M'+n+' 10V100 M10 '+n+'H100',stroke:'#ddd',fill:'none'}));
    }
    const coords=points.map(([x,y])=>(14+10*x)+','+(96-10*y)).join(' ');
    svg.appendChild(make('polygon',{points:coords,stroke:'#164b63',fill:'#bce2f2',
      'stroke-width':'2','stroke-linejoin':'round'}));
    const name = document.createElement('span');
    name.textContent = ' Card '+id;
    button.append(svg,name);
    button.addEventListener('click',()=>{
      if (chosen.includes(id)){
        chosen.splice(chosen.indexOf(id),1);
        button.setAttribute('aria-pressed','false');
      } else if (chosen.length<2){
        chosen.push(id);
        button.setAttribute('aria-pressed','true');
      } else {
        message.textContent='Select two cards only; clear a selection first.';
      }
    });
    list.appendChild(button);
  }
  root.querySelector('[data-g7-check]').addEventListener('click',()=>{
    if(chosen.length!==2){message.textContent='Select exactly two cards.';return;}
    if(!explanation.value.trim()){message.textContent='Write a reason comparing corresponding lengths before checking.';return;}
    const match=byId.get(chosen[0]).pair===chosen[1];
    message.textContent=match
      ? 'The polygons have matching shapes and one common scale factor. Show corresponding lengths in your explanation; your teacher must review your reasoning and drawing.'
      : 'These are not one of the intended matching pairs. Compare all corresponding lengths and angles, then try another pair.';
  });
  root.querySelector('[data-g7-reset]').addEventListener('click',()=>{
    chosen.length=0;
    list.querySelectorAll('button').forEach(b=>b.setAttribute('aria-pressed','false'));
    message.textContent='Selections cleared. No answers or writing are saved.';
  });
  const t=root.querySelector('[data-g7-portrait-scale]');
  const uniform=root.querySelector('[data-g7-uniform]');
  const distorted=root.querySelector('[data-g7-distorted]');
  const metric=root.querySelector('[data-g7-portrait-result]');
  const update=()=>{
    const k=Number(t.value)/100;
    uniform.style.transform='scale('+k+')';
    distorted.style.transform='scale('+k+',1)';
    metric.textContent='Scale factors: uniform copy '+k.toFixed(2)+' by '+k.toFixed(2)+
      '; stretched copy '+k.toFixed(2)+' by 1.00. Which is a scaled copy for k not equal to 1?';
  };
  t.addEventListener('input',update);
  update();
  root.querySelector('[data-g7-cooldown-check]').addEventListener('click',()=>{
    const checked=root.querySelector('input[name="g7-l01-cooldown"]:checked');
    const feedback=root.querySelector('[data-g7-cooldown-feedback]');
    if(!checked){feedback.textContent='Choose A or B and prepare a sketch and written explanation.';return;}
    feedback.textContent=checked.value==='A'
      ? 'A has consistent side ratios of 2. This is only a classification check; the teacher must review your half-size drawing, angles and justification.'
      : 'B has factors 2, 2 and 1.8, so its lengths are not uniformly scaled. Recheck all side ratios. Your complete drawing still needs teacher review.';
  });
})();
