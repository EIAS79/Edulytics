using System.Text.Json;
using Edulytics.Core.Curriculum;
namespace Edulytics.Data.Seeding;
/// <summary>Exactly Grade 3 ordinals 45-143. First-party source-index intake candidates only:
/// original PDF/figures, teacher-led work and publisher Cool-down NOT verified; never academic approval.</summary>
internal static class CommonCoreG3RemainingSourceIntakeRichV2
{
    private const string Prefix="Edulytics.Data.Grade3.SourceIntakeUnit";
    private static readonly Lazy<IReadOnlyDictionary<string,JsonElement>> Records=new(Load);
    internal static bool AppliesTo(string code)=>Records.Value.ContainsKey(code);
    internal static void Apply(CanonicalLessonContentPackLesson lesson)
    {
        if(!Records.Value.TryGetValue(lesson.LessonCode,out var source))return;
        var en=lesson.Translations.Single(x=>string.Equals(x.CultureCode,"en",StringComparison.Ordinal));
        en.Title=Text(source,"Title");
        en.Explanation=Join(source,"ExplanationParagraphs");
        en.KeyConceptsAndRules=string.Join("\n\n",source.GetProperty("KeyConcepts").EnumerateArray()
          .Select(x=>Text(x,"Title")+"\n"+Text(x,"Definition")+"\nRule: "+Text(x,"Rule")+"\nExample: "+Text(x,"Example")));
        en.WorkedExamples=string.Join("\n\n",source.GetProperty("WorkedExamples").EnumerateArray()
          .Select(x=>Text(x,"Title")+"\nProblem: "+Text(x,"Question")+"\nMethod: "+Text(x,"Method")
            +"\nSteps: "+string.Join("; ",x.GetProperty("Steps").EnumerateArray().Select(t=>t.GetString()))
            +"\nAnswer: "+Text(x,"Answer")+"\nCheck: "+Text(x,"Check")));
        en.StepByStepSolutions=string.Join("\n\n",source.GetProperty("WorkedExamples").EnumerateArray()
          .Select(x=>Text(x,"Title")+"\n"+string.Join("\n",x.GetProperty("Steps").EnumerateArray().Select((t,i)=>"Step "+(i+1)+": "+t.GetString()))));
        en.CommonMistakes=string.Join("\n\n",source.GetProperty("CommonMistakes").EnumerateArray()
          .Select(x=>Text(x,"Mistake")+"\nWhy incorrect: "+Text(x,"WhyWrong")+"\nCorrection: "+Text(x,"Correction")));
        en.QuickSummary=Join(source,"SummaryPoints");
    }
    private static string Join(JsonElement e,string key)=>string.Join("\n\n",e.GetProperty(key).EnumerateArray().Select(x=>x.GetString()));
    private static string Text(JsonElement e,string key)=>e.GetProperty(key).GetString()??throw new InvalidOperationException("Missing source intake "+key);
    private static IReadOnlyDictionary<string,JsonElement> Load()
    {
        var map=new Dictionary<string,JsonElement>(StringComparer.Ordinal);
        for(int unit=3;unit<=8;unit++)
        {
            var name=Prefix+unit;
            using var stream=typeof(CommonCoreG3RemainingSourceIntakeRichV2).Assembly.GetManifestResourceStream(name)
              ??throw new InvalidOperationException("Missing source intake resource "+name);
            using var doc=JsonDocument.Parse(stream);
            var root=doc.RootElement;
            if(root.GetProperty("SchemaVersion").GetInt32()!=1||root.GetProperty("Unit").GetInt32()!=unit)
              throw new InvalidOperationException("Source intake identity mismatch");
            foreach(var row in root.GetProperty("Lessons").EnumerateArray())
            {
                var ordinal=row.GetProperty("GradeOrdinal").GetInt32();
                var code=Text(row,"LessonCode");
                if(ordinal<45||ordinal>143||!code.StartsWith("PED:US-CCSS-MATH:G3:U",StringComparison.Ordinal)
                   ||!map.TryAdd(code,row.Clone()))
                  throw new InvalidOperationException("Unexpected source-intake lesson "+code);
            }
        }
        if(map.Count!=99)throw new InvalidOperationException("Expected 99 remaining G3 source-intake candidates");
        return map;
    }
}
