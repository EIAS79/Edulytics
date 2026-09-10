(function(global){'use strict';
const rounds=[
 {a:2,b:1,zone:0,objects:'fireflies'},{a:3,b:2,zone:0,objects:'fireflies'},{a:1,b:4,zone:0,objects:'fireflies'},
 {a:2,b:3,zone:1,objects:'lantern-seeds'},{a:4,b:1,zone:1,objects:'lantern-seeds'},{a:3,b:3,zone:1,objects:'lantern-seeds'},
 {a:5,b:2,zone:2,objects:'glow-stones'},{a:4,b:3,zone:2,objects:'glow-stones'},{a:2,b:5,zone:2,objects:'glow-stones'},{a:5,b:4,zone:2,objects:'glow-stones'}
].map((r,i)=>({...r,sum:r.a+r.b,id:i+1,answers:[r.a+r.b-1,r.a+r.b,r.a+r.b+1].filter(v=>v>0)}));
global.EdulyticsGameActivities=global.EdulyticsGameActivities||{};
global.EdulyticsGameActivities['join-groups-to-add-v8']={
 id:'join-groups-to-add-v8',lessonCode:'PED:CAMBRIDGE-INTL-MATH:S1:L10',learningOutcome:'CAM:OUT:0096:1Ni.02',lessonLanguage:'en',title:'Join Groups to Add',worldTitle:'Lumen Trail',
 assets:{background:'/images/game/v8/lumen-trail-background.svg',terrain:'/images/game/v8/lumen-trail-terrain.svg',foreground:'/images/game/v8/lumen-trail-foreground.svg',student:'/images/game/v8/student-explorer.svg',guide:'/images/game/v8/eddy-guide.svg'},
 zones:[{name:'Whispering Grove',camera:0},{name:'Moonbridge Crossing',camera:720},{name:'Lantern Gate',camera:1440}],rounds,
 copy:{intro:name=>`Welcome${name?`, ${name}`:''}. Eddy found a broken light trail. Join each pair of groups to restore it.`,collect:r=>`Bring ${r.a} and ${r.b} together.`,choose:r=>`${r.a} + ${r.b} = ?`,correct:'The path is brighter. Keep going!',complete:'You restored the Lumen Trail!'}
};
}(window));