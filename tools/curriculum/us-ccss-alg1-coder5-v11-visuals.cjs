#!/usr/bin/env node
'use strict';
// Coder 5 course-owned original diagrams; no publisher assets or shared math engine.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const out=path.join(root,'src/Edulytics.Web/wwwroot/images/curriculum/alg1-v11');
fs.mkdirSync(out,{recursive:true});
const sx=(x,view)=>70+(x-view.xmin)/(view.xmax-view.xmin)*660;
const sy=(y,view)=>420-(y-view.ymin)/(view.ymax-view.ymin)*340;
const esc=s=>String(s).replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&apos;'}[c]));
const specs=[
  {id:'u06-l11-factored-graph',lesson:'U06:L11',
    title:'Two quadratic graphs with shared zeros and opposite opening',
    description:'The purple curve g(x)=-2(x-1)(x-5) opens downward with roots 1 and 5, vertex (3,8) and y-intercept (0,-10). The green curve h(x)=2(x-1)(x-5) opens upward with the same roots, vertex (3,-8) and y-intercept (0,10).',
    view:{xmin:-0.5,xmax:6.5,ymin:-13,ymax:13,dx:1,dy:2},
    curves:[
      {name:'g(x) = -2(x-1)(x-5)',color:'#6954bb',f:x=>-2*(x-1)*(x-5)},
      {name:'h(x) = 2(x-1)(x-5)',color:'#198467',f:x=>2*(x-1)*(x-5)}
    ],
    points:[{x:1,y:0,label:'(1,0)'},{x:5,y:0,label:'(5,0)'},{x:3,y:8,label:'(3,8)'},{x:3,y:-8,label:'(3,-8)'},{x:0,y:-10,label:'(0,-10)'}]},
  {id:'u06-l16-vertex-graph',lesson:'U06:L16',
    title:'Two quadratic graphs sharing vertex (-2,6)',
    description:'The purple curve g(x)=-0.5(x+2)^2+6 has maximum 6 at (-2,6) and y-intercept (0,4). The green curve q(x)=0.5(x+2)^2+6 has minimum 6 at the same vertex and y-intercept (0,8). Both are symmetric about x=-2.',
    view:{xmin:-6.5,xmax:2.5,ymin:-4,ymax:16,dx:1,dy:2},
    curves:[
      {name:'g(x) = -0.5(x+2)^2+6',color:'#6954bb',f:x=>-.5*Math.pow(x+2,2)+6},
      {name:'q(x) = 0.5(x+2)^2+6',color:'#198467',f:x=>.5*Math.pow(x+2,2)+6}
    ],
    points:[{x:-2,y:6,label:'(-2,6)'},{x:0,y:4,label:'(0,4)'},{x:0,y:8,label:'(0,8)'},{x:-4,y:4,label:'(-4,4)'},{x:-4,y:8,label:'(-4,8)'}]},
  {id:'u07-l24-quadratic-line',lesson:'U07:L24',
    title:'Height model and moving-platform line; shared heights at 1s and 4s',
    description:'The purple parabola H(t)=-t^2+6t+2 and green line P(t)=t+6 intersect at (1,7) and (4,10). The parabola has a maximum at (3,11). An intersection of heights alone is not evidence of a collision; horizontal position is not modeled.',
    view:{xmin:0,xmax:6.5,ymin:0,ymax:15,dx:1,dy:2},
    curves:[
      {name:'H(t) = -t^2+6t+2',color:'#6954bb',f:t=>-t*t+6*t+2},
      {name:'P(t) = t+6',color:'#198467',f:t=>t+6}
    ],
    points:[{x:1,y:7,label:'(1,7)'},{x:4,y:10,label:'(4,10)'},{x:3,y:11,label:'(3,11)'},{x:0,y:2,label:'(0,2)'}]}
];
function svgFor(spec){
  const v=spec.view;
  const lines=[];
  for(let x=Math.ceil(v.xmin/v.dx)*v.dx;x<=v.xmax;x+=v.dx){
    const px=sx(x,v);lines.push('<line x1="'+px.toFixed(2)+'" y1="80" x2="'+px.toFixed(2)+'" y2="420" stroke="#d9e0e7" stroke-width="1"/>');
    lines.push('<text x="'+px.toFixed(2)+'" y="443" font-size="12" text-anchor="middle" fill="#354052">'+x+'</text>');
  }
  for(let y=Math.ceil(v.ymin/v.dy)*v.dy;y<=v.ymax;y+=v.dy){
    const py=sy(y,v);lines.push('<line x1="70" y1="'+py.toFixed(2)+'" x2="730" y2="'+py.toFixed(2)+'" stroke="#d9e0e7" stroke-width="1"/>');
    lines.push('<text x="60" y="'+(py+4).toFixed(2)+'" font-size="12" text-anchor="end" fill="#354052">'+y+'</text>');
  }
  const axes=[];
  if(v.xmin<=0&&v.xmax>=0)axes.push('<line x1="'+sx(0,v).toFixed(2)+'" y1="80" x2="'+sx(0,v).toFixed(2)+'" y2="420" stroke="#222" stroke-width="2"/>');
  if(v.ymin<=0&&v.ymax>=0)axes.push('<line x1="70" y1="'+sy(0,v).toFixed(2)+'" x2="730" y2="'+sy(0,v).toFixed(2)+'" stroke="#222" stroke-width="2"/>');
  const curves=spec.curves.map(c=>{
    const pathData=[];
    for(let i=0;i<=600;i++){
      const x=v.xmin+(v.xmax-v.xmin)*i/600;
      pathData.push((i?'L':'M')+sx(x,v).toFixed(2)+','+sy(c.f(x),v).toFixed(2));
    }
    return '<path d="'+pathData.join(' ')+'" fill="none" stroke="'+c.color+'" stroke-width="3" stroke-linejoin="round" clip-path="url(#plotArea)"/>';
  });
  const pts=spec.points.map((p,i)=>{
    const px=sx(p.x,v),py=sy(p.y,v);
    const dy=(i%2===0)?-10:19;
    return '<circle cx="'+px.toFixed(2)+'" cy="'+py.toFixed(2)+'" r="5" fill="#1b2735" stroke="white" stroke-width="1.5"/>'+
    '<text x="'+(px+8).toFixed(2)+'" y="'+(py+dy).toFixed(2)+'" font-size="13" font-weight="700" fill="#1b2735">'+esc(p.label)+'</text>';
  });
  const legend=spec.curves.map((c,i)=>'<line x1="'+(80+i*328)+'" y1="67" x2="'+(103+i*328)+'" y2="67" stroke="'+c.color+'" stroke-width="4"/><text x="'+(110+i*328)+'" y="71" font-size="13" fill="#222">'+esc(c.name)+'</text>');
  return '<?xml version="1.0" encoding="UTF-8"?>\n'+
    '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 800 488" role="img" aria-labelledby="figureTitle figureDescription">'+
    '<title id="figureTitle">'+esc(spec.title)+'</title>'+
    '<desc id="figureDescription">'+esc(spec.description)+'</desc>'+
    '<rect width="800" height="488" rx="14" fill="#fff"/>'+
    '<defs><clipPath id="plotArea"><rect x="70" y="80" width="660" height="340"/></clipPath></defs>'+
    '<g font-family="Arial,sans-serif">'+legend.join('')+lines.join('')+axes.join('')+curves.join('')+pts.join('')+
    '<text x="745" y="445" font-size="14" fill="#222">'+(spec.lesson==='U07:L24'?'t (seconds)':'x')+'</text>'+
    '<text x="20" y="87" font-size="14" fill="#222">'+(spec.lesson==='U07:L24'?'height (m)':'y')+'</text>'+
    '</g></svg>\n';
}
for(const spec of specs){
  for(const c of spec.curves){
    if([spec.view.xmin,spec.view.xmax].some(x=>!Number.isFinite(c.f(x))))throw Error('Invalid plot model');
  }
  const file=path.join(out,spec.id+'.svg');
  const markup=svgFor(spec);
  if(!markup.includes('<title')||!markup.includes('<desc')||!markup.includes('<path')||!markup.includes('<circle'))
    throw Error('Unusable graph '+spec.id);
  fs.writeFileSync(file,markup,'utf8');
  console.log(JSON.stringify({lesson:spec.lesson,graph:path.relative(root,file).replace(/\\/g,'/'),curves:spec.curves.length,points:spec.points.length}));
}
