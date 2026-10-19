using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party, accessible clock-face diagrams for original 2021 Grade 1
/// Unit 7 Lessons 13-15 only. Unlike a textual hour/minute statement this
/// permits visual reading of hour-hand position and the minute hand at 12/6.
/// Teacher-led construction, clock cards, and physical schedule remain
/// outside this digital Practice claim.
/// </summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneFullGradeClockFace(string family, JsonElement p)
    {
        int hour=GetInt(p,"a");
        int card=GetInt(p,"b");
        int c=GetInt(p,"c");
        int mode=GetInt(p,"mode");
        if(hour<1||hour>12||card<1||card>12||c!=0||mode<0||mode>3)
            throw new JsonException("Invalid original Grade 1 clock parameters.");
        int minute=family switch
        {
            "usccss.g1.u07.l13.hour_clock"=>0,
            "usccss.g1.u07.l14.half_hour_clock"=>mode==2?0:30,
            "usccss.g1.u07.l15.write_hours_halfhours"=>mode==0?0:30,
            "usccss.g1.u07.l16.daily_schedule_time"=>mode%2==0?0:30,
            _=>throw new JsonException("Family is not a Grade 1 analog clock assessment.")
        };
        const double cx=210,cy=130;
        var svg=SvgStart("Analog clock face with 12 numbered hours and two hands. Read their positions to tell the time.");
        svg.Append("<circle cx='210' cy='130' r='91' class='shape'/>");
        for(int number=1;number<=12;number++)
        {
            double angle=(number%12)*Math.PI/6-Math.PI/2;
            double x1=cx+Math.Cos(angle)*80;
            double y1=cy+Math.Sin(angle)*80;
            double x2=cx+Math.Cos(angle)*86;
            double y2=cy+Math.Sin(angle)*86;
            svg.Append($"<line x1='{F(x1)}' y1='{F(y1)}' x2='{F(x2)}' y2='{F(y2)}' stroke='currentColor' stroke-width='2'/>");
            // All 12 numerals are scale labels, not prefilled answers.
            double tx=cx+Math.Cos(angle)*67;
            double ty=cy+Math.Sin(angle)*67+5;
            svg.Append($"<text x='{F(tx)}' y='{F(ty)}' font-size='15' text-anchor='middle' fill='currentColor'>{number}</text>");
        }
        double hourAngle=((hour%12)+minute/60d)*Math.PI/6-Math.PI/2;
        double minuteAngle=minute*Math.PI/30-Math.PI/2;
        double hourEndX=cx+Math.Cos(hourAngle)*46;
        double hourEndY=cy+Math.Sin(hourAngle)*46;
        double minEndX=cx+Math.Cos(minuteAngle)*73;
        double minEndY=cy+Math.Sin(minuteAngle)*73;
        svg.Append($"<line class='clock-hour' x1='210' y1='130' x2='{F(hourEndX)}' y2='{F(hourEndY)}' stroke='currentColor' stroke-width='6' stroke-linecap='round'/>");
        svg.Append($"<line class='clock-minute' x1='210' y1='130' x2='{F(minEndX)}' y2='{F(minEndY)}' stroke='currentColor' stroke-width='3' stroke-linecap='round'/>");
        svg.Append("<circle cx='210' cy='130' r='5' fill='currentColor'/>");
        if(family=="usccss.g1.u07.l13.hour_clock"&&mode>=2)
        {
            int digitalHour=mode==2?hour:card;
            Text(svg,210,24,$"Digital card: {digitalHour}:00","hint");
        }
        Text(svg,210,244,"Read the short hand, then the long hand.","hint");
        return SvgEnd(svg);
    }
}
