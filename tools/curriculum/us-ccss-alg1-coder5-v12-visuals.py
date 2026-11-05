#!/usr/bin/env python3
"""Original, course-local SVG diagrams. Not automatically delivered until shared-view owner routes them."""
from pathlib import Path
from html import escape
import math
root=Path(__file__).resolve().parents[2]
out=root/'src/Edulytics.Web/wwwroot/images/curriculum/alg1-v12'
out.mkdir(parents=True,exist_ok=True)
def svg(title,desc,body):
    return ('<?xml version="1.0" encoding="utf-8"?>\n'
    '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 850 510" '
    'role="img" aria-labelledby="title desc"><title id="title">'+escape(title)+'</title>'
    '<desc id="desc">'+escape(desc)+'</desc>'
    '<rect width="850" height="510" fill="white"/><g font-family="Arial,sans-serif">'
    +body+'</g></svg>\n')
def txt(x,y,v,size=14,anchor='start'):
    return '<text x="'+str(x)+'" y="'+str(y)+'" font-size="'+str(size)+'" text-anchor="'+anchor+'">'+escape(str(v))+'</text>'
def line(x1,y1,x2,y2,color='#cad3df',w=1):
    return '<line x1="'+str(x1)+'" y1="'+str(y1)+'" x2="'+str(x2)+'" y2="'+str(y2)+'" stroke="'+color+'" stroke-width="'+str(w)+'"/>'
def dot(cx,cy,r=5,color='#33415c'):
    return '<circle cx="'+str(round(cx,2))+'" cy="'+str(round(cy,2))+'" r="'+str(r)+'" fill="'+color+'"/>'
spec=[]
D=[('A bell',[1,2,4,2,1]),('B uniform',[2,2,2,2,2]),('C right skew',[5,3,1,1,1]),('D bimodal',[4,1,1,1,4]),('E left skew',[1,1,1,3,5])]
body=txt(30,35,'Five first-party dot plots: use five frequency cards to match',20)
for i,(title,values) in enumerate(D):
    ox=30+(i%3)*270;oy=65+(i//3)*220
    body+=txt(ox+105,oy+16,title,16,'middle')+line(ox+20,oy+165,ox+230,oy+165,'#303a48',2)
    for n,count in enumerate(values):
        x=ox+34+n*43
        body+=txt(x,oy+183,n+1,12,'middle')
        for k in range(count):body+=dot(x,oy+153-k*22,7,'#4f61a8')
spec.append(('u01-l04-distribution-dotplots',svg('Five original distribution dot plots',
'Original fictional five distributions; values 1 through 5 have frequencies A 1,2,4,2,1; B 2,2,2,2,2; C 5,3,1,1,1; D 4,1,1,1,4; E 1,1,1,3,5. Create ten original cards and match them.',body)))
def chart(title,description,xrange,yrange,curves,points,xlabel,ylabel):
    x0,x1=xrange;y0,y1=yrange
    X=lambda x:64+(x-x0)/(x1-x0)*730
    Y=lambda y:445-(y-y0)/(y1-y0)*340
    body=txt(45,37,title,20)
    for x in range(math.ceil(x0),math.floor(x1)+1):
        px=round(X(x),2)
        body+=line(px,105,px,445)+txt(px,464,x,12,'middle')
    step=max(1,math.ceil((y1-y0)/14))
    for y in range(math.ceil(y0/step)*step,math.floor(y1)+1,step):
        py=round(Y(y),2)
        body+=line(64,py,794,py)+txt(54,py+4,y,12,'end')
    body+=line(64,445,794,445,'#333',2)
    for i,(name,func,color) in enumerate(curves):
        d=' '.join(('M' if k==0 else 'L')+format(X(x0+(x1-x0)*k/500),'.2f')+','+
                   format(Y(func(x0+(x1-x0)*k/500)),'.2f') for k in range(501))
        body+='<path d="'+d+'" fill="none" stroke="'+color+'" stroke-width="3" clip-path="url(#c)"/>'
        body+=txt(70+i*350,80,name,13)
    for x,y,label in points:body+=dot(X(x),Y(y))+txt(round(X(x)+8,2),round(Y(y)-7,2),label,12)
    if title.startswith('Original cost function'):
        # Red points belong to a SECOND relation, not to the bicycle curve.
        body+=dot(X(2),Y(7),7,'#c63c43')+dot(X(2),Y(9),7,'#c63c43')
        body+=txt(410,80,'Red: different relation, same input 2 has two outputs',11)
    body+=txt(785,490,xlabel,13)+txt(10,105,ylabel,13)
    clip='<defs><clipPath id="c"><rect x="64" y="105" width="730" height="340"/></clipPath></defs>'
    return svg(title,description,clip+body)
spec.append(('u04-l01-function-and-nonfunction',chart('Original cost function and a contrasting multi-output relation',
'Bike P(h)=4+3h gives points (0,4),(1,7),(2,10),(3,13). Other relation has both (2,7) and (2,9); this would fail the vertical-line test. Only the first graph is plotted as a curve. Separate student plot required.',
(0,5),(0,17),[('Bike P=4+3h',lambda h:4+3*h,'#6954bb')],
[(0,4,'0,4'),(1,7,'1,7'),(2,10,'2,10'),(3,13,'3,13')],'input h','output $')))
spec.append(('u05-l01-additive-vs-multiplicative',chart('Original linear and exponential rewards',
'Plan A 100+30t vs Plan B 10*2^t, for whole weeks 0 to 6. At week4 A220 B160; at week5 A250 B320, the first whole-week crossover.',
(0,6),(0,660),[('A(t)=100+30t',lambda t:100+30*t,'#6954bb'),('B(t)=10*2^t',lambda t:10*2**t,'#198467')],
[(4,220,'A4'),(4,160,'B4'),(5,250,'A5'),(5,320,'B5')],'week t','tokens')))
spec.append(('u06-l01-garden-area',chart('Garden with 24 m fencing: area rises then falls',
'Width 1..11 metres, complementary length12-w, areas11,20,27,32,35,36,35,32,27,20,11 square metres, vertex width6 area36.',
(0,12),(0,42),[('Area=w(12-w)',lambda w:w*(12-w),'#6954bb')],
[(1,11,'1,11'),(4,32,'4,32'),(6,36,'6,36'),(8,32,'8,32'),(11,11,'11,11')],'width m','area m2')))
body=txt(35,38,'Original geometric constraint: uniform picture border',21)
body+='<rect x="145" y="120" width="540" height="300" fill="#e0e9f5" stroke="#334054" stroke-width="3"/>'
body+='<rect x="200" y="162" width="430" height="215" fill="white" stroke="#334054" stroke-width="3"/>'
body+=txt(415,270,'Picture 8 by 5 inches',21,'middle')+txt(415,458,'Outer: (8+2x) by (5+2x); x=1 inch',18,'middle')
body+=txt(165,280,'x',18)+txt(650,280,'x',18)+txt(415,145,'x',18)+txt(415,405,'x',18)
body+=txt(115,489,'Border material = outer 70 square inches - picture 40 = 30.',14)
spec.append(('u07-l01-original-frame',svg('Original picture-frame geometry model',
'Picture 8 by 5 inches surrounded by a uniform border of thickness x; at x=1 outer dimensions10 by7 inches, outer area70, inner area40, frame area30 square inches.',body)))
for name,art in spec:
    p=out/(name+'.svg');p.write_text(art,encoding='utf-8')
    print(name,str(p.relative_to(root)).replace('\\','/'),'bytes',len(art.encode('utf-8')),flush=True)
