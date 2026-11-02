// T001 lesson 8 chart construction UI. The server owns all answer verification.
(() => {
  "use strict";
  const locales = {
    en: ["Build your shape graph", "One mark = one card; count each shape once.",
      "Triangles", "Squares", "Circles", "Remove mark", "Add mark", "Build all three columns.", "Ready to check"],
    ar: ["ابنِ الرسم البياني بنفسك", "كل علامة تمثل شكلًا واحدًا، عُد كل شكل مرة.",
      "مثلثات", "مربعات", "دوائر", "احذف علامة", "أضف علامة", "أكمل الأعمدة الثلاثة.", "جاهز للتصحيح"],
    pl: ["Zbuduj wykres", "Jeden symbol = jedna figura. Policz każdą raz.",
      "Trójkąty", "Kwadraty", "Koła", "Usuń symbol", "Dodaj symbol", "Uzupełnij trzy kolumny.", "Gotowe do sprawdzenia"]
  };
  const make = (tag, cls, label) => {
    const e = document.createElement(tag);
    e.className = cls;
    if (label !== undefined) e.textContent = label;
    return e;
  };
  document.querySelectorAll("[data-t001-shape-chart]").forEach(root => {
    const labels = locales[(root.dataset.t001Locale || "en").split("-")[0]] || locales.en;
    const readonly = root.dataset.t001Readonly === "true";
    const initial = /^([0-5]),([0-5]),([0-5])$/.exec(root.dataset.t001Initial || "");
    const counts = initial ? [Number(initial[1]),Number(initial[2]),Number(initial[3])] : [0,0,0];
    const form = root.closest("form");
    const answer = form?.querySelector("[data-t001-shape-answer]");
    const submit = form?.querySelector("[data-t001-shape-submit]");
    root.replaceChildren();
    root.append(make("h2", "t001-chart-title", labels[0]));
    root.append(make("p", "t001-chart-help", labels[1]));
    const chart = make("div", "t001-chart-columns");
    chart.setAttribute("role", "group");
    chart.setAttribute("aria-label", labels[0]);
    root.append(chart);
    const state = make("p", "t001-chart-status");
    state.setAttribute("role", "status");
    state.setAttribute("aria-live", "polite");
    const controls = [];
    const shapes=["▲","■","●"];
    const update = () => {
      const total = counts.reduce((x,y)=>x+y,0);
      counts.forEach((count,index) => {
        const {bar,value,minus,plus} = controls[index];
        bar.replaceChildren();
        for(let i=0;i<count;i++) {
          const mark=make("span","t001-chart-mark",shapes[index]);
          mark.setAttribute("aria-hidden","true");
          bar.append(mark);
        }
        value.textContent=String(count);
        if(minus) minus.disabled=count===0;
        if(plus) plus.disabled=count>=5 || total>=10;
      });
      const valid=counts.every(x=>x>=1) && total<=10;
      if(answer) answer.value=counts.join(",");
      if(submit) submit.disabled=!valid;
      state.textContent=valid ? labels[8] : labels[7];
    };
    shapes.forEach((shape,index) => {
      const col=make("section","t001-chart-column");
      const name=make("h3","t001-chart-label",labels[index+2]);
      const bar=make("div","t001-chart-bar");
      bar.setAttribute("aria-hidden","true");
      const value=make("output","t001-chart-count");
      value.setAttribute("aria-label",labels[index+2]);
      col.append(name,bar,value);
      let minus=null,plus=null;
      if(!readonly) {
        const row=make("div","t001-chart-actions");
        minus=make("button","t001-chart-step","−");
        plus=make("button","t001-chart-step","+");
        minus.type="button"; plus.type="button";
        minus.setAttribute("aria-label",labels[5]+": "+labels[index+2]);
        plus.setAttribute("aria-label",labels[6]+": "+labels[index+2]);
        minus.addEventListener("click",()=>{if(counts[index]>0){counts[index]--;update();}});
        plus.addEventListener("click",()=>{
          if(counts[index]<5 && counts.reduce((x,y)=>x+y,0)<10){counts[index]++;update();}
        });
        row.append(minus,plus);col.append(row);
      }
      chart.append(col);controls.push({bar,value,minus,plus});
    });
    root.append(state);update();
  });
})();
