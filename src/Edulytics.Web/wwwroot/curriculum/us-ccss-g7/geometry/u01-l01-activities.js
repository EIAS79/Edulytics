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
  // These are the lesson's seven first-party F table rows, not the shared
  // polygon verifier. The check provides formative measurements only.
  const fCandidateRows = [
    ["A",[6,3,1.5],"upright"],
    ["B",[8,4,2],"upright"],
    ["C",[4,4,3],"upright"],
    ["D",[4,2,1],"sheared (angles changed)"],
    ["E",[6,3,2],"upright"],
    ["F",[2,1,1],"upright"],
    ["G",[12,6,3],"upright"]
  ];
  const fCandidates = new Map(fCandidateRows.map(([id, lengths, condition]) =>
    [id, {lengths, condition}]));
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
    // Screen readers need the actual figure, not only its letter: coordinates
    // make correspondence and scale factors independently inspectable.
    const vertices = points.map(([x, y], index) =>
      'vertex '+(index+1)+' ('+x+', '+y+')').join('; ');
    button.setAttribute('aria-label',
      'Polygon card '+id+', '+points.length+' vertices on a unit grid: '+
      vertices+'. Select this card to compare scaled copies.');
    button.dataset.g7Card = id;
    // The labeled button exposes the geometry; hide its duplicate SVG subtree.
    const svg = make('svg', {viewBox:'0 0 110 110',width:'130',height:'130',
      'aria-hidden':'true',focusable:'false'});
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
  // Reference evidence for the ten fixed cards. This is NOT a verifier for
  // student-uploaded shapes: the shared exact polygon helper belongs to PR #608.
  const referenceSideFactors = (fromId, toId) => {
    const source = byId.get(fromId).points;
    const target = byId.get(toId).points;
    return source.map((point, index) => {
      const next = (index + 1) % source.length;
      const sourceSide = Math.hypot(source[next][0]-point[0], source[next][1]-point[1]);
      const targetSide = Math.hypot(target[next][0]-target[index][0],
        target[next][1]-target[index][1]);
      return targetSide / sourceSide;
    });
  };
  const readableFactor = factor => Number(factor.toFixed(3)).toString();
  root.querySelector('[data-g7-check]').addEventListener('click',()=>{
    if(chosen.length!==2){message.textContent='Select exactly two cards.';return;}
    if(!explanation.value.trim()){message.textContent='Write a reason comparing corresponding lengths before checking.';return;}
    const match=byId.get(chosen[0]).pair===chosen[1];
    if (!match) {
      message.textContent='This is not one of the intended matching card pairs. Compare every corresponding side and angle; your written reason has not been graded.';
      return;
    }
    const factors = referenceSideFactors(chosen[0], chosen[1]);
    const consistent = factors.every(k => Number.isFinite(k) && k>0 &&
      Math.abs(k-factors[0]) < 1e-9);
    message.textContent = consistent
      ? 'Reference cards '+chosen[0]+' to '+chosen[1]+': corresponding side ratios (copy/original) are '+
        factors.map(readableFactor).join(', ')+'. The common factor is '+
        readableFactor(factors[0])+'. Matching angles remain equal. Only the intended card match was checked; your written explanation and drawing still require teacher review.'
      : 'The reference side ratios disagree. Ask the teacher to review this card data; no answer has been graded.';
  });
  root.querySelector('[data-g7-reset]').addEventListener('click',()=>{
    chosen.length=0;
    list.querySelectorAll('button').forEach(b=>b.setAttribute('aria-pressed','false'));
    explanation.value='';
    message.textContent='Card selections and draft explanation cleared. No answers or writing are saved.';
  });
  // Source table self-check: compare the three stated measurements and the
  // supplied angle condition. Do not infer that a three-number match grades a
  // freehand F drawing, or that rotating the entire F invalidates scaling.
  root.querySelector('[data-g7-f-check]').addEventListener('click',()=>{
    const id = root.querySelector('[data-g7-f-choice]').value;
    const feedback = root.querySelector('[data-g7-f-feedback]');
    const candidate = fCandidates.get(id);
    if (!candidate) {
      feedback.textContent='Choose an F candidate A through G before checking.';
      return;
    }
    const ratios = candidate.lengths.map((v,i)=>v/[4,2,1][i]);
    const uniform = ratios.every(k=>Number.isFinite(k) && k>0 &&
      Math.abs(k-ratios[0])<1e-9);
    const angles = candidate.condition==='upright';
    const outcome = uniform && angles
      ? 'The listed lengths share a factor and the stated angle condition is preserved.'
      : (!uniform
        ? 'The three corresponding length ratios are not all equal.'
        : 'The listed lengths share a factor, but the sheared F changes corresponding angles.');
    feedback.textContent='F candidate '+id+': ratios (spine, top bar, short bar) are '+
      ratios.map(readableFactor).join(', ')+'. '+outcome+
      ' This checks reference measurements only; a teacher must inspect the full outline, all corresponding angles, and your written reasoning. No grade is recorded.';
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
      '; horizontal-only copy '+k.toFixed(2)+' by 1.00. '+
      (k===1
        ? 'At factor 1 both figures match the original; use another slider value to distinguish them.'
        : 'Only the uniformly scaled figure preserves both dimensions by one factor. Check corresponding angles and lengths.');
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
