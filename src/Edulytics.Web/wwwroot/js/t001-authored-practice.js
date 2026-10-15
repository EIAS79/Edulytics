// Source-scoped G1/T001 authored tasks for L02, L07, L09.
// The HTML form POST and server-side math kernel remain authoritative.
(() => {
 "use strict";
 const l = {
  en:{expression:"Write your own addition expression",left:"First card",right:"Second card",sum:"Total",tools:"Put each tool in its group",unsorted:"Not sorted",kind:"Group by tool kind",survey:"Collect real anonymous votes",surveyNote:"Ask a real participant for each vote. Do not record names or personal data. A teacher verifies authentic collection separately.",votes:"Votes entered",remove:"Undo last vote",build:"Build the graph from the votes",add:"Add mark",subtract:"Remove mark",incomplete:"Finish all parts before checking.",ready:"Your work is ready to check.",groups:["Counters","Pattern blocks","Inch tiles"],fruit:["Apple","Banana","Orange"],shapeLabel:"one mark = one vote",explain:"Tell your teacher how you chose the sorting rule."},
  ar:{expression:"اكتب عملية الجمع بنفسك",left:"البطاقة الأولى",right:"البطاقة الثانية",sum:"المجموع",tools:"صنّف كل أداة في مجموعتها",unsorted:"لم تُصنف",kind:"التصنيف حسب نوع الأداة",survey:"اجمع أصوات أشخاص حقيقيين بدون أسماء",surveyNote:"اسأل شخصًا حقيقيًا عن صوته في كل مرة، ولا تسجل أي بيانات شخصية. يتحقق المعلم من إجراء الاستبيان فعليًا.",votes:"الأصوات المسجلة",remove:"تراجع عن آخر صوت",build:"ابنِ الرسم من الأصوات",add:"أضف علامة",subtract:"احذف علامة",incomplete:"أكمل المهمة قبل التصحيح.",ready:"إجابتك جاهزة للتصحيح.",groups:["عدادات","قوالب الأنماط","بلاطات القياس"],fruit:["تفاح","موز","برتقال"],shapeLabel:"كل علامة تمثل صوتًا",explain:"اشرح للمعلم قاعدة التصنيف التي اخترتها."},
  pl:{expression:"Ułóż samodzielnie dodawanie",left:"Pierwsza karta",right:"Druga karta",sum:"Razem",tools:"Przyporządkuj każde narzędzie do grupy",unsorted:"Nieprzypisane",kind:"Grupuj według rodzaju",survey:"Zbierz anonimowe głosy prawdziwych osób",surveyNote:"Zapytaj osobę o jej głos. Nie zapisuj danych osobowych. Nauczyciel osobno potwierdza prawdziwy udział.",votes:"Zapisane głosy",remove:"Cofnij ostatni głos",build:"Narysuj wykres na podstawie głosów",add:"Dodaj symbol",subtract:"Usuń symbol",incomplete:"Ukończ zadanie przed sprawdzeniem.",ready:"Możesz sprawdzić odpowiedź.",groups:["Liczniki","Klocki wzorów","Płytki calowe"],fruit:["Jabłko","Banan","Pomarańcza"],shapeLabel:"jeden symbol to jeden głos",explain:"Wyjaś nauczycielowi zasadę podziału."}
 };
 const el=(tag,klass,value)=>{const x=document.createElement(tag);x.className=klass;if(value!==undefined)x.textContent=value;return x;};
 function button(text,fn,label){const b=el("button","t001-authored-btn",text);b.type="button";if(label)b.setAttribute("aria-label",label);b.addEventListener("click",fn);return b;}
 const names=["counter","pattern block","inch tile"];
 const symbols=["▲","■","●"];
 document.querySelectorAll("[data-t001-authored-task]").forEach(root=>{
  const kind=root.dataset.t001AuthoredTask;
  const tr=l[(root.dataset.t001Locale||"en").split("-")[0]]||l.en;
  const readonly=root.dataset.t001Readonly==="true";
  const stem=root.dataset.t001Source||"";
  const initial=root.dataset.t001Initial||"";
  const form=root.closest("form");
  const answer=form?.querySelector("[data-t001-authored-answer]");
  const submit=form?.querySelector("[data-t001-authored-submit]");
  root.replaceChildren();
  const status=el("p","t001-authored-status");
  status.setAttribute("role","status");status.setAttribute("aria-live","polite");
  let getAnswer=()=>"",valid=()=>false;
  const sync=()=>{const v=getAnswer();if(answer)answer.value=v;if(submit)submit.disabled=!valid();status.textContent=valid()?tr.ready:tr.incomplete;};
  const heading=t=>root.append(el("h2","t001-authored-title",t));
  if(kind==="expression"){
   const dots=/Dot cards:\s*(●+)\s*\|\s*(●+)\./.exec(stem);
   if(!dots){root.append(el("p","t001-authored-error","Invalid source-bound dot cards."));return;}
   heading(tr.expression);
   const cards=el("div","t001-authored-dotcards");
   for(const group of [dots[1],dots[2]]){
    const card=el("div","t001-authored-dotcard");
    for(const dot of group)card.append(el("span","t001-authored-dot",dot));
    cards.append(card);
   }
   root.append(cards);
   const line=el("div","t001-authored-expression-line");
   const labels=[tr.left,tr.right,tr.sum];
   const fields=[];
   const match=/^([0-9]{1,2})\+([0-9]{1,2})=([0-9]{1,2})$/.exec(initial);
   for(let i=0;i<3;i++){
    const wrap=el("label","t001-authored-field");
    wrap.append(el("span","",labels[i]));
    const field=el("input","t001-authored-number");
    field.type="number";field.min="0";field.max="10";field.step="1";field.inputMode="numeric";
    field.autocomplete="off";field.disabled=readonly;
    if(match)field.value=match[i+1];
    field.addEventListener("input",sync);
    wrap.append(field);line.append(wrap);fields.push(field);
    if(i<2)line.append(el("strong","t001-authored-symbol",i===0?"+":"="));
   }
   root.append(line);
   getAnswer=()=>fields.map(f=>f.value.trim()).join("~").replace(/^([0-9]{1,2})~([0-9]{1,2})~([0-9]{1,2})$/,"$1+$2=$3");
   valid=()=>fields.every(f=>f.value!==""&&/^\d{1,2}$/.test(f.value)&&Number(f.value)<=10);
  }else if(kind==="sort"){
   const match=/Items:\s*(.+?)\. Sort EVERY/.exec(stem);
   const tokens=match?match[1].split(", "):[];
   if(tokens.length<7||tokens.length>9||tokens.some(x=>!names.includes(x))){root.append(el("p","t001-authored-error","Invalid source-bound tool collection."));return;}
   heading(tr.tools);root.append(el("p","t001-authored-help",tr.kind));
   const saved=initial.split(",");
   const selected=tokens.map((_,i)=>/^[012]$/.test(saved[i]||"")?Number(saved[i]):-1);
   const choices=el("div","t001-authored-cards");
   const bins=el("div","t001-authored-bins");
   const buckets=tr.groups.map((name,i)=>{const b=el("section","t001-authored-bin");b.append(el("h3","",name));const content=el("div","t001-authored-bin-items");b.append(content);bins.append(b);return content;});
   const refresh=()=>{
    buckets.forEach(b=>b.replaceChildren());
    selected.forEach((group,i)=>{if(group>=0)buckets[group].append(el("span","t001-authored-chip",tokens[i]));});
    sync();
   };
   tokens.forEach((tool,i)=>{
    const row=el("label","t001-authored-card");
    row.append(el("span","t001-authored-card-name",tool));
    const selector=el("select","t001-authored-select");
    selector.setAttribute("aria-label",tool+" "+(i+1));
    for(const [value,label] of [[-1,tr.unsorted],...[0,1,2].map(j=>[j,tr.groups[j]])]){
     const opt=el("option","",label);opt.value=String(value);selector.append(opt);
    }
    selector.value=String(selected[i]);selector.disabled=readonly;
    selector.addEventListener("change",()=>{selected[i]=Number(selector.value);refresh();});
    row.append(selector);choices.append(row);
   });
   root.append(choices,bins,el("p","t001-authored-help",tr.explain));
   getAnswer=()=>selected.join(",");
   valid=()=>selected.every(x=>x>=0&&x<=2);
   refresh();
  }else if(kind==="survey"){
   heading(tr.survey);root.append(el("p","t001-authored-help",tr.surveyNote));
   const found=/^v:([0-2]{0,10})\|g:(\d{1,2}),(\d{1,2}),(\d{1,2})$/.exec(initial);
   const votes=found?Array.from(found[1],n=>Number(n)):[];
   const graph=found?[Number(found[2]),Number(found[3]),Number(found[4])]:[0,0,0];
   const voteBar=el("div","t001-authored-vote-buttons");
   const tape=el("div","t001-authored-vote-tape");
   const voteTitle=el("p","t001-authored-help");
   const graphTitle=el("h3","t001-authored-title",tr.build);
   const graphLayout=el("div","t001-authored-bins");
   const bars=[],values=[];
   const update=()=>{
    voteTitle.textContent=tr.votes+": "+votes.length+"/10";
    tape.replaceChildren();
    for(const v of votes)tape.append(el("span","t001-authored-chip",tr.fruit[v]));
    graph.forEach((count,i)=>{bars[i].replaceChildren();for(let n=0;n<count;n++)bars[i].append(el("span","t001-authored-tally","●"));values[i].textContent=String(count);});
    root.querySelectorAll("[data-vote-add]").forEach(b=>b.disabled=readonly||votes.length>=10);
    root.querySelectorAll("[data-graph-add]").forEach(b=>b.disabled=readonly||graph.reduce((a,b)=>a+b,0)>=10||graph[Number(b.dataset.graphAdd)]>=10);
    root.querySelectorAll("[data-graph-remove]").forEach(b=>b.disabled=readonly||graph[Number(b.dataset.graphRemove)]<=0);
    const undo=root.querySelector("[data-vote-undo]");if(undo)undo.disabled=readonly||votes.length===0;
    sync();
   };
   tr.fruit.forEach((fruit,i)=>{if(readonly)return;const b=button("+ "+fruit,()=>{if(votes.length<10){votes.push(i);update();}});b.dataset.voteAdd=String(i);voteBar.append(b);});
   if(!readonly){const undo=button(tr.remove,()=>{votes.pop();update();});undo.dataset.voteUndo="true";voteBar.append(undo);}
   root.append(voteBar,voteTitle,tape,graphTitle,el("p","t001-authored-help",tr.shapeLabel));
   tr.fruit.forEach((name,i)=>{
    const section=el("section","t001-authored-bin");section.append(el("h3","",name));
    const bar=el("div","t001-authored-tallybar");section.append(bar);bars.push(bar);
    const count=el("output","t001-authored-count");section.append(count);values.push(count);
    if(!readonly){const ctrl=el("div","t001-authored-controls");
     const less=button("−",()=>{if(graph[i]>0){graph[i]--;update();}},tr.subtract+" "+name);less.dataset.graphRemove=String(i);
     const more=button("+",()=>{if(graph.reduce((a,b)=>a+b,0)<10){graph[i]++;update();}},tr.add+" "+name);more.dataset.graphAdd=String(i);
     ctrl.append(less,more);section.append(ctrl);}
    graphLayout.append(section);
   });
   root.append(graphLayout);
   getAnswer=()=> "v:"+votes.join("")+"|g:"+graph.join(",");
   valid=()=>votes.length>=5&&votes.length<=10&&graph.reduce((a,b)=>a+b,0)===votes.length;
   update();
  }else {root.append(el("p","t001-authored-error","Unknown authored task."));return;}
  root.append(status);sync();
 });
})();
