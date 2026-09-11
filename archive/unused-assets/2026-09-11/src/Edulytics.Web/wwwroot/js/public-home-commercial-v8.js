(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  const mascotFamily = `
    <div class="ed-home-v6-cartoon" aria-label="Edulytics mascot family for mathematics learning">
      <svg viewBox="0 0 920 600" role="img" aria-label="A family of original Edulytics learning mascots with mathematics and progress elements">
        <defs>
          <linearGradient id="mf-bg" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#eef8ff"/><stop offset="1" stop-color="#f7f1ff"/></linearGradient>
          <linearGradient id="mf-blue" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#3b82f6"/><stop offset="1" stop-color="#1768f2"/></linearGradient>
          <linearGradient id="mf-purple" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#9a6cff"/><stop offset="1" stop-color="#7047ee"/></linearGradient>
          <linearGradient id="mf-green" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#66d79e"/><stop offset="1" stop-color="#2db779"/></linearGradient>
          <linearGradient id="mf-orange" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#ffb04a"/><stop offset="1" stop-color="#ff7a20"/></linearGradient>
        </defs>
        <rect width="920" height="600" fill="url(#mf-bg)"/>
        <circle cx="90" cy="88" r="28" fill="#ffd34d"/><rect x="782" y="74" width="54" height="54" rx="14" fill="#ff8a3d" transform="rotate(17 809 101)"/><circle cx="820" cy="490" r="28" fill="#52c98b"/>
        <rect x="565" y="88" width="280" height="188" rx="26" fill="#fff" stroke="#dce7f5" stroke-width="4"/>
        <text x="594" y="128" font-size="18" font-weight="900" fill="#2f66e8">EDULYTICS</text>
        <text x="594" y="163" font-size="28" font-weight="900" fill="#10243d">Math that adapts</text>
        <rect x="594" y="188" width="103" height="62" rx="16" fill="#eef5ff"/><text x="613" y="214" font-size="12" font-weight="800" fill="#62728a">MASTERY</text><text x="613" y="240" font-size="26" font-weight="900" fill="#16a56e">82%</text>
        <rect x="714" y="188" width="103" height="62" rx="16" fill="#fff4e9"/><text x="733" y="214" font-size="12" font-weight="800" fill="#a35f28">NEXT</text><text x="733" y="240" font-size="21" font-weight="900" fill="#10243d">Fractions</text>
        <ellipse cx="455" cy="500" rx="325" ry="36" fill="#dbe8f7"/>

        <g transform="translate(92 190)">
          <rect x="34" y="0" width="128" height="112" rx="42" fill="url(#mf-blue)"/>
          <rect x="54" y="24" width="88" height="58" rx="24" fill="#112d52"/>
          <circle cx="82" cy="53" r="8" fill="#77ecff"/><circle cx="116" cy="53" r="8" fill="#77ecff"/>
          <path d="M82 69c13 9 25 9 38 0" stroke="#77ecff" stroke-width="5" fill="none" stroke-linecap="round"/>
          <path d="M98 0v-24" stroke="#2f66e8" stroke-width="8" stroke-linecap="round"/><circle cx="98" cy="-30" r="8" fill="#ffcf4b"/>
          <path d="M45 100c-26 18-32 48-19 75" stroke="#2f66e8" stroke-width="18" fill="none" stroke-linecap="round"/><path d="M151 100c26 18 32 48 19 75" stroke="#2f66e8" stroke-width="18" fill="none" stroke-linecap="round"/>
          <rect x="52" y="102" width="93" height="118" rx="38" fill="url(#mf-blue)"/><circle cx="99" cy="152" r="20" fill="#fff" opacity=".95"/><text x="99" y="161" text-anchor="middle" font-size="26" font-weight="900" fill="#1768f2">e</text>
          <rect x="59" y="212" width="22" height="55" rx="11" fill="#1768f2"/><rect x="116" y="212" width="22" height="55" rx="11" fill="#1768f2"/>
        </g>

        <g transform="translate(290 188)">
          <ellipse cx="95" cy="54" rx="82" ry="70" fill="url(#mf-purple)"/>
          <circle cx="68" cy="50" r="12" fill="#fff"/><circle cx="121" cy="50" r="12" fill="#fff"/><circle cx="68" cy="52" r="6" fill="#1b2740"/><circle cx="121" cy="52" r="6" fill="#1b2740"/>
          <path d="M70 83c18 14 34 14 51 0" stroke="#fff" stroke-width="6" fill="none" stroke-linecap="round"/>
          <path d="M40 18 14 -10 50 0M150 18l27-28-5 36" fill="#8d5df5"/>
          <rect x="28" y="118" width="134" height="114" rx="46" fill="url(#mf-purple)"/>
          <circle cx="95" cy="156" r="18" fill="#fff" opacity=".95"/><text x="95" y="164" text-anchor="middle" font-size="22" font-weight="900" fill="#7047ee">π</text>
          <path d="M30 145c-35 13-44 39-35 69" stroke="#7047ee" stroke-width="18" fill="none" stroke-linecap="round"/><path d="M159 145c35 13 44 39 35 69" stroke="#7047ee" stroke-width="18" fill="none" stroke-linecap="round"/>
        </g>

        <g transform="translate(480 236)">
          <circle cx="80" cy="40" r="64" fill="url(#mf-green)"/>
          <circle cx="57" cy="35" r="10" fill="#fff"/><circle cx="103" cy="35" r="10" fill="#fff"/><circle cx="57" cy="37" r="5" fill="#1b2740"/><circle cx="103" cy="37" r="5" fill="#1b2740"/>
          <path d="M58 65c14 10 28 10 44 0" stroke="#fff" stroke-width="6" fill="none" stroke-linecap="round"/>
          <path d="M72 -18c-15-27-3-42 17-47M90 -20c19-25 9-42-9-52" stroke="#2db779" stroke-width="9" fill="none" stroke-linecap="round"/>
          <rect x="20" y="100" width="120" height="104" rx="44" fill="url(#mf-green)"/><text x="80" y="160" text-anchor="middle" font-size="30" font-weight="900" fill="#fff">%</text>
        </g>

        <g transform="translate(660 305)">
          <circle cx="60" cy="30" r="52" fill="url(#mf-orange)"/>
          <circle cx="42" cy="27" r="9" fill="#fff"/><circle cx="79" cy="27" r="9" fill="#fff"/><circle cx="42" cy="29" r="4" fill="#1b2740"/><circle cx="79" cy="29" r="4" fill="#1b2740"/>
          <path d="M43 53c11 8 22 8 34 0" stroke="#fff" stroke-width="5" fill="none" stroke-linecap="round"/>
          <rect x="10" y="78" width="100" height="88" rx="38" fill="url(#mf-orange)"/><text x="60" y="132" text-anchor="middle" font-size="27" font-weight="900" fill="#fff">3/4</text>
        </g>

        <g font-weight="900" fill="#10243d"><text x="112" y="520" font-size="38">x + 4</text><text x="760" y="330" font-size="44">%</text></g>
      </svg>
    </div>`;

  const replaceHeroMascot = () => {
    const visual = root.querySelector('.ed-home-v6-visual');
    if (!visual) return;
    if (visual.querySelector('.ed-home-v6-cartoon')) visual.innerHTML = mascotFamily;
  };

  const visual = root.querySelector('.ed-home-v6-visual');
  if (visual) {
    replaceHeroMascot();
    const observer = new MutationObserver(() => replaceHeroMascot());
    observer.observe(visual, { childList: true });
  }

  const schoolPhoto = root.querySelector('#schools .ed-home-v6-audience-photo');
  if (schoolPhoto) {
    schoolPhoto.style.backgroundImage = "url('https://images.unsplash.com/photo-1562774053-701939374585?auto=format&fit=crop&q=80&w=900')";
    schoolPhoto.setAttribute('aria-label', document.documentElement.lang.toLowerCase().startsWith('pl') ? 'Budynek szkoły' : 'School building');
  }
})();