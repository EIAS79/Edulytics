(function(global){
'use strict';
const TAU=Math.PI*2;
const clamp=(v,a,b)=>Math.max(a,Math.min(b,v));
const lerp=(a,b,t)=>a+(b-a)*t;
const ease=t=>1-Math.pow(1-clamp(t,0,1),3);
class Renderer{
  constructor(game){
    this.g=game;
    this.img={};
    const sources={background:game.config.assets?.background,terrain:game.config.assets?.terrain,foreground:game.config.assets?.foreground,student:game.config.assets?.student,guide:game.config.assets?.guide};
    Object.entries(sources).forEach(([k,src])=>{if(!src)return;const im=new Image();im.decoding='async';im.src=src;this.img[k]=im;});
  }
  rr(c,x,y,w,h,r){c.beginPath();c.roundRect(x,y,w,h,r);}
  crop(scene){
    if(scene==='arrival') return {x:0,y:180,w:1050,h:720};
    if(scene==='mission') return {x:130,y:235,w:1340,h:665};
    return {x:0,y:0,w:1600,h:900};
  }
  drawLayer(c,im,scene){
    if(!im?.complete||!im.naturalWidth)return false;
    const g=this.g,r=this.crop(scene),srcAR=r.w/r.h,dstAR=g.w/g.h;
    let sx=r.x,sy=r.y,sw=r.w,sh=r.h;
    if(srcAR>dstAR){const nw=sh*dstAR;sx+=(sw-nw)/2;sw=nw;}else{const nh=sw/dstAR;sy+=(sh-nh)/2;sh=nh;}
    c.drawImage(im,sx,sy,sw,sh,0,0,g.w,g.h);
    return true;
  }
  world(c,scene){
    c.fillStyle='#0a7fa8';c.fillRect(0,0,this.g.w,this.g.h);
    this.drawLayer(c,this.img.background,scene);
    this.drawLayer(c,this.img.terrain,scene);
    this.drawLayer(c,this.img.foreground,scene);
  }
  vignette(c,a=.2){const g=this.g,r=c.createRadialGradient(g.w*.5,g.h*.46,Math.min(g.w,g.h)*.16,g.w*.5,g.h*.5,Math.max(g.w,g.h)*.72);r.addColorStop(0,'rgba(0,0,0,0)');r.addColorStop(1,`rgba(2,18,31,${a})`);c.fillStyle=r;c.fillRect(0,0,g.w,g.h);}
  glow(c,x,y,r,color='255,224,93',strength=.8){const gr=c.createRadialGradient(x,y,0,x,y,r);gr.addColorStop(0,`rgba(${color},${strength})`);gr.addColorStop(.26,`rgba(${color},${strength*.38})`);gr.addColorStop(1,`rgba(${color},0)`);c.fillStyle=gr;c.beginPath();c.arc(x,y,r,0,TAU);c.fill();}
  drawCharacter(c,im,x,y,h,t,flip=false){if(!im?.complete||!im.naturalWidth)return;const bob=Math.sin(t*2.2+x*.01)*4;const w=h*(im.naturalWidth/im.naturalHeight);c.save();c.translate(x,y+bob);if(flip)c.scale(-1,1);c.shadowColor='rgba(0,26,42,.38)';c.shadowBlur=18;c.drawImage(im,-w/2,-h,w,h);c.restore();}
  lantern(c,x,y,total){const g=this.g,gl=clamp(g.collected/Math.max(1,total),0,1);this.glow(c,x,y,110,'255,222,91',.18+.72*gl);c.save();c.translate(x,y);c.shadowColor='#ffd95c';c.shadowBlur=18+45*gl;c.strokeStyle='#5c3f31';c.lineWidth=8;c.beginPath();c.arc(0,-45,29,Math.PI,TAU);c.stroke();const body=c.createLinearGradient(0,-48,0,60);body.addColorStop(0,'#8e6749');body.addColorStop(1,'#493027');c.fillStyle=body;this.rr(c,-42,-49,84,108,19);c.fill();const light=c.createLinearGradient(0,-36,0,42);light.addColorStop(0,`rgba(255,242,153,${.3+.7*gl})`);light.addColorStop(1,`rgba(255,199,69,${.18+.72*gl})`);c.fillStyle=light;this.rr(c,-31,-37,62,79,14);c.fill();c.shadowBlur=0;c.fillStyle='#694632';this.rr(c,-50,48,100,17,8);c.fill();c.fillStyle='rgba(7,34,47,.83)';this.rr(c,-34,70,68,28,14);c.fill();c.fillStyle='#fff4b8';c.font='900 14px Inter,system-ui';c.textAlign='center';c.textBaseline='middle';c.fillText(`${g.collected} / ${total}`,0,84);c.restore();}
  firefly(c,x,y,color,t,active=false){c.save();const pulse=1+Math.sin(t*4+x*.012)*.08;c.translate(x,y);c.scale(pulse,pulse);c.shadowColor=color;c.shadowBlur=active?30:20;c.fillStyle='rgba(255,255,255,.88)';c.beginPath();c.ellipse(-12,-1,15,7,-.45,0,TAU);c.ellipse(12,-1,15,7,.45,0,TAU);c.fill();c.fillStyle=color;c.beginPath();c.arc(0,2,10,0,TAU);c.fill();c.fillStyle='#fffbd2';c.beginPath();c.arc(-2,-1,3.5,0,TAU);c.fill();c.restore();}
  groupHalo(c,x,y,count,kind){const col=kind==='left'?'255,207,76':'188,163,255';this.glow(c,x,y,110,col,.14);c.save();c.strokeStyle=`rgba(${col},.72)`;c.lineWidth=3;c.setLineDash([10,8]);c.beginPath();c.ellipse(x,y,112,68,0,0,TAU);c.stroke();c.setLineDash([]);c.fillStyle='rgba(7,34,47,.86)';this.rr(c,x-31,y-96,62,33,16);c.fill();c.fillStyle='#fff4bc';c.font='950 18px Inter,system-ui';c.textAlign='center';c.textBaseline='middle';c.fillText(String(count),x,y-79);c.restore();}
  prompt(c,q){const g=this.g,w=g.w,h=g.h;c.save();c.fillStyle='rgba(6,30,46,.72)';this.rr(c,w*.31,h*.105,w*.38,h*.074,22);c.fill();c.strokeStyle='rgba(255,220,104,.58)';c.lineWidth=2;c.stroke();c.fillStyle='#ffe68f';c.font=`900 ${Math.max(15,Math.min(22,w*.015))}px Inter,system-ui`;c.textAlign='center';c.fillText(g.config.copy.missionTitle,w*.5,h*.135);c.fillStyle='#f4ffff';c.font=`800 ${Math.max(11,Math.min(14,w*.0102))}px Inter,system-ui`;c.fillText(g.config.copy.collectPrompt(q),w*.5,h*.161);c.restore();}
  answers(c,q,t){const g=this.g,slots=g.answerSlots(q.answers.length);c.save();c.fillStyle='rgba(6,30,46,.72)';this.rr(c,g.w*.38,g.h*.61,g.w*.24,40,20);c.fill();c.fillStyle='#fff1ab';c.font=`950 ${Math.max(19,Math.min(29,g.w*.020))}px Inter,system-ui`;c.textAlign='center';c.textBaseline='middle';c.fillText(g.config.copy.answerPrompt(q),g.w*.5,g.h*.635);slots.forEach((s,i)=>{const v=q.answers[i],hover=v===g.hoverAnswer,y=s.y-Math.sin(t*2+i)*2;c.save();c.translate(s.x,y);this.glow(c,0,0,64,'255,217,83',hover?.9:.34);const grd=c.createLinearGradient(0,-30,0,32);grd.addColorStop(0,hover?'#fff4aa':'#ecd8a8');grd.addColorStop(1,hover?'#e2a33b':'#9b7249');c.fillStyle=grd;c.beginPath();c.ellipse(0,0,49,29,0,0,TAU);c.fill();c.strokeStyle=hover?'#fff7b7':'#5f4937';c.lineWidth=4;c.stroke();c.fillStyle='#183846';c.font='950 25px Inter,system-ui';c.fillText(String(v),0,2);c.restore();});c.restore();}
  bridge(c,t){const g=this.g,geo=g.geometry(),segments=9,startY=g.h*.57,endY=g.h*.88;for(let i=0;i<segments;i++){const k=clamp(t*segments-i,0,1);if(k<=0)continue;const y=lerp(startY,endY,i/(segments-1)),x=geo.riverX+Math.sin(i*.68)*7;c.save();c.globalAlpha=ease(k);c.translate(x,y);this.glow(c,0,0,42,'255,218,83',.38);c.shadowColor='rgba(0,0,0,.35)';c.shadowBlur=10;c.fillStyle='#d9ad68';this.rr(c,-52,-13,104,26,9);c.fill();c.strokeStyle='#62442f';c.lineWidth=3;c.stroke();c.restore();}}
  particles(c){for(const p of this.g.particles){c.save();c.globalAlpha=clamp(p.life,0,1);c.fillStyle=p.color;c.beginPath();c.arc(p.x,p.y,p.size,0,TAU);c.fill();c.restore();}}
  node(c,p,label,state,t){const active=state==='active',done=state==='done';if(active)this.glow(c,p.x,p.y,50,'255,225,88',.78+.12*Math.sin(t*4));c.save();c.translate(p.x,p.y);c.fillStyle=done?'#7ee1a1':active?'#ffd858':'rgba(9,40,53,.82)';c.strokeStyle=active?'#fff5af':'rgba(255,255,255,.72)';c.lineWidth=active?4:2.5;c.beginPath();c.arc(0,0,active?21:16,0,TAU);c.fill();c.stroke();c.fillStyle=done?'#205845':active?'#4a3716':'#d8e6e9';c.font='950 12px Inter,system-ui';c.textAlign='center';c.textBaseline='middle';c.fillText(done?'✓':active?String(label):'•',0,0);if(active){c.fillStyle='rgba(5,30,44,.88)';this.rr(c,-66,29,132,31,15);c.fill();c.fillStyle='#fff2b5';c.font='850 11px Inter,system-ui';c.fillText(label,p.x?0:0,45);}c.restore();}
  mapNodes(c,t){const g=this.g;g.config.world.mapNodes.forEach((n,i)=>{const p=g.mapPoint(n),state=i<g.roundIndex?'done':i===g.roundIndex?'active':'locked';this.node(c,p,i+1,state,t);if(state==='active'){c.save();c.fillStyle='rgba(5,30,44,.88)';this.rr(c,p.x-70,p.y+29,140,31,15);c.fill();c.fillStyle='#fff2b5';c.font='850 11px Inter,system-ui';c.textAlign='center';c.textBaseline='middle';c.fillText(n.name,p.x,p.y+45);c.restore();}});}
  arrival(c,t){const g=this.g;this.world(c,'arrival');this.vignette(c,.24);this.drawCharacter(c,this.img.student,g.w*.16,g.h*.84,g.h*.42,t,false);this.drawCharacter(c,this.img.guide,g.w*.33,g.h*.78,g.h*.25,t,true);const x=g.w*.61,y=g.h*.51;this.lantern(c,x,y,1);this.glow(c,x,y,90,'255,226,91',.48+.08*Math.sin(t*3));this.particles(c);}
  map(c,t){const g=this.g;this.world(c,'map');this.vignette(c,.12);this.mapNodes(c,t);this.drawCharacter(c,this.img.student,g.w*.10,g.h*.88,g.h*.25,t,false);this.drawCharacter(c,this.img.guide,g.w*.90,g.h*.87,g.h*.18,t,true);this.particles(c);}
  mission(c,t){const g=this.g,q=g.rounds[g.roundIndex],geo=g.geometry();this.world(c,'mission');c.fillStyle='rgba(3,18,29,.12)';c.fillRect(0,0,g.w,g.h);this.drawCharacter(c,this.img.student,g.w*.10,g.h*.89,g.h*.25,t,false);this.drawCharacter(c,this.img.guide,g.w*.90,g.h*.86,g.h*.18,t,true);this.groupHalo(c,geo.leftBush.x,geo.leftBush.y,q.left,'left');this.groupHalo(c,geo.rightBush.x,geo.rightBush.y,q.right,'right');this.lantern(c,geo.lantern.x,geo.lantern.y,q.sum);g.getLiveFlies().forEach((f,i)=>{if(f.alive)this.firefly(c,f.x,f.y,f.color,t+i*.3,i===g.hoverFly)});g.flyAnimations.forEach(a=>{const k=ease(a.t),arc=Math.sin(Math.PI*clamp(a.t,0,1))*-70;this.firefly(c,lerp(a.sx,a.ex,k),lerp(a.sy,a.ey,k)+arc,a.color,t,true)});this.prompt(c,q);if(g.answerVisible||g.phase==='success')this.answers(c,q,t);if(g.bridgeT>0)this.bridge(c,g.bridgeT);this.vignette(c,.08);this.particles(c);}
  draw(c,t){c.clearRect(0,0,this.g.w,this.g.h);if(this.g.scene==='arrival')this.arrival(c,t);else if(this.g.scene==='map'||this.g.scene==='complete')this.map(c,t);else this.mission(c,t);}
}
global.EdulyticsGameV7Renderer=Renderer;
// V4 engine currently instantiates this global. Alias it intentionally for isolated preview compatibility.
global.EdulyticsGameV4Renderer=Renderer;
}(window));
