using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    // Only the exact declared G10 visual families are accepted. Geometry is
    // calculated from the question parameters, not a generic decorative image.
    private static string? UaeG10Visual(string family, JsonElement p)
    {
        const string prefix = "uae.g10.general.";
        if (!family.StartsWith(prefix, StringComparison.Ordinal) || p.ValueKind != JsonValueKind.Object)
            return null;
        string name = family[prefix.Length..];
        int N(string key, int low, int high)
        {
            if (!p.TryGetProperty(key, out var value) || !value.TryGetInt32(out int n) || n < low || n > high)
                throw new JsonException("Invalid G10 diagram parameter: " + key);
            return n;
        }
        static double Tan(int a) => Math.Tan(a * Math.PI / 180);
        static (int a,int b,int c) Triple(int t) => t switch { 0 => (3,4,5), 1 => (5,12,13), 2 => (8,15,17), _ => (7,24,25) };
        static string Num(double n) => n.ToString("0.##", CultureInfo.InvariantCulture);
        var d = new G10Drawing();
        switch (name)
        {
            case "u02.l07.radical_graph_y_value":
            case "u02.l07.radical_graph_domain_start":
            {
                int a=N("a",-3,3),h=N("h",-4,4),k=N("k",-4,4);
                N("n",0,5);
                if(a==0)return null;
                return G10Plot(Math.Min(-2,h-2), Math.Max(6,h+25), x=>x<h?null:a*Math.Sqrt(x-h)+k, "تمثيل دالة الجذر التربيعي", h);
            }
            case "u04.l01.vertex_x":
            case "u04.l01.vertex_y":
            case "u04.l01.opens_up":
            case "u04.l01.value_at_x":
            case "u04.l02.real_root_count":
            case "u04.l02.left_intercept":
            case "u04.l02.right_intercept":
            {
                int a=N("a",-4,4), b=N("b",-40,40),c=N("c",-104,104);
                if(a==0)return null;
                double h=-b/(2d*a);
                if(name.StartsWith("u04.l01.",StringComparison.Ordinal))
                {
                    int probe=N("probe",-7,7);
                    if(h!=Math.Truncate(h)||Math.Abs(h)>4||Math.Abs(c-a*h*h)>8||Math.Abs(probe-h)>3)return null;
                }
                else
                {
                    int mode=N("mode",0,2), r1=N("r1",-5,5),r2=N("r2",-5,5),hh=N("h",-4,4),k=N("k",1,6);
                    if(mode==2)
                    {
                        if(name!="u04.l02.real_root_count"||b!=-2*a*hh||c!=a*(hh*hh+k))return null;
                    }
                    else if(b!=-a*(r1+r2)||c!=a*r1*r2||(mode==1)!=(r1==r2))return null;
                }
                return G10Plot(Math.Min(-6,h-3),Math.Max(6,h+3),x=>a*x*x+b*x+c,"تمثيل الدالة التربيعية",h);
            }
            case "u14.l01.ratio_six_exact":
            case "u14.l01.side_opposite_exact":
            case "u14.l01.side_adjacent_exact":
            case "u14.l01.side_hypotenuse_exact":
            case "u14.l01.angle_inverse_sine_rounded":
            case "u14.l01.angle_inverse_cosine_rounded":
            case "u14.l01.angle_inverse_tangent_rounded":
            case "u14.l01.special_angle_exact":
            case "u14.l01.elevation_height_rounded":
            case "u14.l01.depression_distance_rounded":
            {
                int t=N("triple",0,3),m=N("m",1,12),angle=N("angle",20,70),dist=N("distance",10,90),special=N("special",0,2);
                N("ratio",0,5);
                if(angle%5!=0)return null;
                var (a,b,c)=Triple(t);
                string op=name["u14.l01.".Length..];
                if(op=="elevation_height_rounded")
                    return G10Right(dist,dist*Tan(angle),Num(dist)+" m","?",null,Num(angle)+"°");
                if(op=="depression_distance_rounded")
                {
                    // Angle of depression is at the elevated point, measured
                    // from the horizontal parallel to the ground.
                    return G10Right(dist/Tan(angle),dist,"?",Num(dist)+" m",null,Num(angle)+"°",depression:true);
                }
                if(op=="special_angle_exact")
                {
                    if(special==0)return G10Right(Math.Sqrt(3)*m,m,null,"?",Num(2*m),"30°");
                    if(special==1)return G10Right(m,Math.Sqrt(3)*m,"?",null,Num(2*m),"60°");
                    return G10Right(m,m,Num(m),"?",null,"45°");
                }
                return G10Right(b*m,a*m,
                    op is "ratio_six_exact" or "angle_inverse_cosine_rounded" or "angle_inverse_tangent_rounded" ? Num(b*m) : op=="side_adjacent_exact"?"?":null,
                    op is "ratio_six_exact" or "side_hypotenuse_exact" or "angle_inverse_sine_rounded" or "angle_inverse_tangent_rounded" ? Num(a*m) : op=="side_opposite_exact"?"?":null,
                    op is "ratio_six_exact" or "side_opposite_exact" or "side_adjacent_exact" or "angle_inverse_sine_rounded" or "angle_inverse_cosine_rounded" ? Num(c*m):op=="side_hypotenuse_exact"?"?":null,
                    "θ");
            }
            case "u14.l15.mean_rounded":
            case "u14.l15.altitude_squared":
            case "u14.l15.leg_squared":
            case "u14.l15.projection_from_leg":
            case "u14.l16.triangle_classification":
            case "u14.l16.coordinate_distance_squared":
            {
                int a=N("a",2,12),b=N("b",2,12),k=N("k",0,5),t=N("t",0,3);
                var (x,y,h)=Triple(t);
                if(name=="u14.l16.triangle_classification")
                {
                    int[] sides=[a*x,a*y,a*(k%4==0?x+y:k%4==1?h-1:k%4==2?h:h+1)];
                    double scale=240d/sides.Max();
                    for(int i=0;i<3;i++){double yy=55+65*i;d.Line(42,yy,42+sides[i]*scale,yy);d.Label(42+sides[i]*scale/2,yy-12,Num(sides[i]));}
                    return d.End("مقارنة الأطوال المعطاة دون افتراض أنها تشكل مثلثًا");
                }
                if(name=="u14.l16.coordinate_distance_squared")
                {
                    double scale=Math.Min(240d/(a+b+2),150d/(t+k+2)),ox=50+(a+1)*scale,oy=25+(t+1)*scale;
                    d.Line(25,oy,335,oy,true);d.Line(ox,20,ox,225,true);d.Label(340,oy-5,"س");d.Label(ox+12,15,"ص");
                    double px=ox-a*scale,py=oy-t*scale,qx=ox+b*scale,qy=oy+k*scale;
                    d.Line(px,py,qx,qy);d.Dot(px,py);d.Dot(qx,qy);d.Label(px,py-12,$"P(−{a}, {t})");d.Label(qx,qy+18,$"Q({b}, −{k})");
                    return d.End("النقطتان المعطاتان على المستوى الإحداثي");
                }
                bool projection=name=="u14.l15.projection_from_leg";
                double length=projection?h*a:a+b, left=projection?(double)x*x*a/h:a;
                double height=Math.Sqrt(left*(length-left)),scale2=Math.Min(260/length,165/height);
                double l=50,r=50+length*scale2,foot=50+left*scale2,top=195-height*scale2;
                d.Line(l,195,r,195);d.Line(l,195,foot,top);d.Line(foot,top,r,195);d.Line(foot,top,foot,195,true);
                d.RightMark(foot,195,1,-1);
                if(projection){d.Label((l+r)/2,224,Num(length));d.Label((l+foot)/2-12,(195+top)/2-9,Num(x*a));d.Label((l+foot)/2,187,"?");}
                else {d.Label((l+foot)/2,217,Num(a));d.Label((foot+r)/2,217,Num(b));
                    if(name=="u14.l15.leg_squared")d.Label((l+foot)/2-12,(195+top)/2-8,"?");
                    else d.Label(foot+17,(195+top)/2,"?");}
                return d.End("مثلث قائم وارتفاعه على الوتر");
            }
            case "u14.l17.height_with_eye_rounded":
            case "u14.l17.two_station_height_rounded":
            case "u14.l17.line_of_sight_rounded":
            case "u14.l17.angle_from_rise_run_rounded":
            {
                int a=N("a",5,40),b=N("b",5,30),k=N("k",0,5),t=N("t",0,3);
                int alpha=20+5*k,beta=50+5*t;
                if(name=="u14.l17.line_of_sight_rounded")return G10Right(a/Tan(alpha),a,null,Num(a)+" m","?",Num(alpha)+"°");
                if(name=="u14.l17.angle_from_rise_run_rounded")return G10Right(b,a,Num(b)+" m",Num(a)+" m",null,"θ");
                if(name=="u14.l17.height_with_eye_rounded")
                {
                    double eye=(12+2*t)/10d, rise=a*Tan(alpha), scale=Math.Min(260d/a,165d/(eye+rise)),right=45+a*scale,yy=205-eye*scale,top=yy-rise*scale;
                    d.Line(45,205,right,205);d.Line(45,205,45,yy);d.Line(45,yy,right,yy,true);d.Line(45,yy,right,top);d.Line(right,205,right,top);
                    d.Label(24,(205+yy)/2,Num(eye));d.Label((45+right)/2,225,Num(a)+" m");d.Label(78,yy-12,Num(alpha)+"°");d.Label(right+19,(205+top)/2,"?");
                    return d.End("ارتفاع البرج بالنسبة إلى مستوى عين الراصد");
                }
                double near=b*Tan(alpha)/(Tan(beta)-Tan(alpha)),total=b+near,rise2=near*Tan(beta),sc=Math.Min(270/total,165/rise2),cx=40+b*sc,rx=40+total*sc,ty=205-rise2*sc;
                d.Line(40,205,rx,205);d.Line(40,205,rx,ty);d.Line(cx,205,rx,ty);d.Line(rx,205,rx,ty);d.RightMark(rx,205,-1,-1);
                d.Label((40+cx)/2,224,Num(b)+" m");d.Label(62,192,Num(alpha)+"°");d.Label(cx+12,180,Num(beta)+"°");d.Label(rx+17,(205+ty)/2,"?");
                return d.End("زاويتا ارتفاع من نقطتي رصد على المستوى نفسه");
            }
            case "u14.l18.special_length_squared":
            case "u14.l18.special_exact_choice":
            case "u14.l18.equilateral_altitude_squared":
            case "u14.l18.square_diagonal_squared":
            {
                int a=N("a",2,20),kind=N("kind",0,3),from=N("from",0,2),to=N("to",0,2);N("shift",0,2);
                if(from==to)return null;
                if(name=="u14.l18.equilateral_altitude_squared")
                {
                    d.Line(85,205,275,205);d.Line(85,205,180,205-95*Math.Sqrt(3));d.Line(275,205,180,205-95*Math.Sqrt(3));d.Line(180,205,180,205-95*Math.Sqrt(3),true);d.RightMark(180,205,1,-1);d.Label(180,226,Num(a));d.Label(197,124,"?");return d.End("ارتفاع المثلث المتساوي الأضلاع");
                }
                if(name=="u14.l18.square_diagonal_squared")
                {
                    d.Line(100,40,260,40);d.Line(260,40,260,200);d.Line(260,200,100,200);d.Line(100,200,100,40);d.Line(100,200,260,40,true);d.Label(180,222,Num(a));d.Label(184,110,"?");return d.End("قطر المربع");
                }
                bool equal=kind%2==0;
                double[] ratio=equal?[1,1,Math.Sqrt(2)]:[1,Math.Sqrt(3),2];
                string?[] labels=new string?[3];
                if(name=="u14.l18.special_exact_choice")
                {
                    from=kind switch{0=>0,1=>0,2=>2,_=>1};
                    to=kind switch{0=>2,1=>1,2=>0,_=>2};
                    labels[from]=kind switch{2=>Num(a)+"√2",3=>Num(a)+"√3",_=>Num(a)};
                }
                else labels[from]=Num(a);
                labels[to]="?";
                double scale=a/ratio[from];
                return G10Right(ratio[1]*scale,ratio[0]*scale,labels[1],labels[0],labels[2],equal?"45°":"30°");
            }
        }
        return null;
    }

    private static string G10Right(double width,double height,string? horizontal,string? vertical,string? hypotenuse,string angle,bool depression=false)
    {
        var d=new G10Drawing();
        double scale=Math.Min(252/width,154/height),left=48,right=48+width*scale,bottom=203,top=203-height*scale;
        d.Line(left,bottom,right,bottom);d.Line(right,bottom,right,top);d.Line(left,bottom,right,top);d.RightMark(right,bottom,-1,-1);
        if(horizontal!=null)d.Label((left+right)/2,bottom+20,horizontal);
        if(vertical!=null)d.Label(right+22,(bottom+top)/2,vertical);
        if(hypotenuse!=null)d.Label((left+right)/2-8,(bottom+top)/2-13,hypotenuse);
        if(depression){d.Line(left,top,right,top,true);d.Label(right-34,top+15,angle);}
        else d.Label(left+Math.Min(26,(right-left)/4),bottom-8,angle);
        return d.End("مثلث قائم مطابق للمعطيات");
    }

    private static string G10Plot(double xmin,double xmax,Func<double,double?> f,string title,double pivot)
    {
        var points=Enumerable.Range(0,321).Select(i=>xmin+(xmax-xmin)*i/320).Append(pivot).Distinct().OrderBy(x=>x)
            .Select(x=>(x,y:f(x))).Where(p=>p.y.HasValue).Select(p=>(p.x,y:p.y!.Value)).ToArray();
        double ymin=Math.Min(0,points.Min(p=>p.y)),ymax=Math.Max(0,points.Max(p=>p.y));
        double pad=Math.Max(1,(ymax-ymin)*0.07);ymin-=pad;ymax+=pad;
        double X(double x)=>45+280*(x-xmin)/(xmax-xmin);
        double Y(double y)=>210-178*(y-ymin)/(ymax-ymin);
        var d=new G10Drawing();
        double stepX=Math.Max(1,Math.Ceiling((xmax-xmin)/8)),stepY=Math.Max(1,Math.Ceiling((ymax-ymin)/6));
        for(double x=Math.Ceiling(xmin/stepX)*stepX;x<=xmax;x+=stepX){d.Line(X(x),32,X(x),210,true);d.Label(X(x),228,x.ToString("0",CultureInfo.InvariantCulture));}
        for(double y=Math.Ceiling(ymin/stepY)*stepY;y<=ymax;y+=stepY){d.Line(45,Y(y),325,Y(y),true);d.Label(24,Y(y)+4,y.ToString("0",CultureInfo.InvariantCulture));}
        d.Line(45,Y(0),325,Y(0));d.Line(X(0),32,X(0),210);d.Label(340,Y(0)-7,"س");d.Label(X(0)+10,20,"ص");
        d.Curve(points.Select(p=>(X(p.x),Y(p.y))));
        return d.End(title);
    }

    private sealed class G10Drawing
    {
        private readonly StringBuilder _body=new();
        private static string F(double v)=>v.ToString("0.###",CultureInfo.InvariantCulture);
        public void Line(double x,double y,double xx,double yy,bool guide=false)=>_body.Append($"<line x1='{F(x)}' y1='{F(y)}' x2='{F(xx)}' y2='{F(yy)}' stroke='{(guide?"#94a3b8":"#334155")}' stroke-width='{(guide?"1":"2")}' {(guide?"stroke-dasharray='4 4'":"")}/>");
        public void Label(double x,double y,string value)=>_body.Append($"<text x='{F(x)}' y='{F(y)}' text-anchor='middle' font-size='12' fill='#172554'>{WebUtility.HtmlEncode(value)}</text>");
        public void Dot(double x,double y)=>_body.Append($"<circle cx='{F(x)}' cy='{F(y)}' r='3.5' fill='#0369a1'/>");
        public void RightMark(double x,double y,int sx,int sy){Line(x+9*sx,y,x+9*sx,y+9*sy);Line(x+9*sx,y+9*sy,x,y+9*sy);}
        public void Curve(IEnumerable<(double x,double y)> points)=>_body.Append("<polyline fill='none' stroke='#0369a1' stroke-width='2.5' points='"+string.Join(" ",points.Select(p=>F(p.x)+","+F(p.y)))+"'/>");
        public string End(string title)=>"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 360 245' role='img' aria-label='"+WebUtility.HtmlEncode(title)+"'><rect width='360' height='245' fill='#ffffff'/>"+_body+"</svg>";
    }
}
