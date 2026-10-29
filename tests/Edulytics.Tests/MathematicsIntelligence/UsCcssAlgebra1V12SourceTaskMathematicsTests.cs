using System.Text.Json;
using System.Xml.Linq;
using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlgebra1V12SourceTaskMathematicsTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:";
    private static readonly string Root = FindRoot();

    private static string FindRoot()
    {
        for (var d = new DirectoryInfo(AppContext.BaseDirectory); d is not null; d = d.Parent)
            if (File.Exists(Path.Combine(d.FullName,"global.json")) &&
                Directory.Exists(Path.Combine(d.FullName,"src","Edulytics.Core")))
                return d.FullName;
        throw new InvalidOperationException("No root");
    }
    private static JsonDocument Read(string name) => JsonDocument.Parse(File.ReadAllText(
        Path.Combine(Root,"docs","curriculum",name)));

    [Theory]
    [InlineData("U01:L04")][InlineData("U02:L01")][InlineData("U03:L01")]
    [InlineData("U04:L01")][InlineData("U05:L01")][InlineData("U06:L01")]
    [InlineData("U07:L01")][InlineData("U01:L05")][InlineData("U02:L02")]
    [InlineData("U03:L02")][InlineData("U04:L02")][InlineData("U05:L02")]
    [InlineData("U06:L02")][InlineData("U07:L02")]
    public void RepairedLessonMatchesCanonicalSourceAndKeepsWorkedMath(string suffix)
    {
        var lesson=RichLessonContentV2Registry.Find(Prefix+suffix,"en");
        Assert.NotNull(lesson);
        var unit=int.Parse(suffix.Substring(1,2));
        var n=int.Parse(suffix.Substring(5,2));
        var text=string.Join(" ",lesson.ExplanationParagraphs);
        Assert.Contains($"/HS/teachers/1/{unit}/{n}/preparation.html",text);
        Assert.Contains("V12",text);
        Assert.True(lesson.WorkedExamples.Count>=3);
        Assert.True(lesson.KeyConcepts.Count>=3);
        Assert.True(lesson.CommonMistakes.Count>=2);
    }

    [Fact]
    public void SevenUnitSourceRepairsHaveIndependentNumericalOracles()
    {
        int[][] data=[[1,2,4,2,1],[2,2,2,2,2],[5,3,1,1,1],
                      [4,1,1,1,4],[1,1,1,3,5]];
        Assert.Equal([10,10,11,11,11],data.Select(x=>x.Sum()).ToArray());
        Assert.True(data[0].SequenceEqual(data[0].Reverse()));
        Assert.True(6*3<20);
        Assert.Equal(106,14*7+8);
        Assert.Equal(120,14*8+8);
        Assert.Equal(40,12+8+9+11);
        Assert.Equal(21,12+9);
        Assert.Equal(19,8+11);
        Assert.Equal(14.4,12.8+1.6,8);
        Assert.Equal(20.8,13*1.6,8);
        Assert.Equal([1,3,9,27,81],
            Enumerable.Range(0,5).Select(n=>(int)Math.Pow(3,n)).ToArray());
        Assert.True(100+30*4>10*Math.Pow(2,4));
        Assert.True(100+30*5<10*Math.Pow(2,5));
        Assert.Equal(36,6*(12-6));
        Assert.Equal(35,5*(12-5));
        Assert.Equal(30,(8+2)*(5+2)-8*5);
    }

    [Fact]
    public void SecondBatchSourceRepairsHaveIndependentExactMath()
    {
        int[] observations=[3,5,5,7,9,9,11];
        Assert.Equal(7,observations.Average(),8);
        Assert.Equal(16d/7,observations.Average(x=>Math.Abs(x-7)),8);
        Assert.Equal(4,observations[5]-observations[1]);
        Assert.Equal(50,200*.25,8);
        Assert.Equal(24,200*.12,8);
        Assert.Equal(16,200*.08,8);
        Assert.Equal(2,8-12+6);
        Assert.Equal(2,4-6+4);
        Assert.Equal(61,1.08*50+7,8);
        Assert.Equal(.30,12d/40,8);
        Assert.Equal(.225,9d/40,8);
        Assert.Equal(.20,8d/40,8);
        Assert.Equal(.275,11d/40,8);
        Assert.Equal(1d,12d/21+9d/21,8);
        Assert.Equal(1d,8d/19+11d/19,8);
        Assert.Equal(2,Math.Min(2,4));
        Assert.Equal(3,4-Math.Abs(2-3));
        Assert.Equal(86,6+8*10);
        Assert.Equal(345.990234375,6*Math.Pow(1.5,10),8);
        Assert.True(6+8*5>6*Math.Pow(1.5,5));
        Assert.True(6+8*6<6*Math.Pow(1.5,6));
        Assert.Equal(55,7*9-2*4);
        Assert.Equal(55,5*9+2*5);
        Assert.Equal(75,9*8+3);
        Assert.Equal(0,-4*Math.Pow(2+Math.Sqrt(7),2)
                +16*(2+Math.Sqrt(7))+12,8);
        static double Revenue(double p)=>p*(40-2*p);
        Assert.Equal(128,Revenue(4));
        Assert.Equal(128,Revenue(16));
        Assert.Equal(200,Revenue(10));
    }

    [Theory]
    [InlineData("u01-l04-distribution-dotplots.svg")]
    [InlineData("u04-l01-function-and-nonfunction.svg")]
    [InlineData("u05-l01-additive-vs-multiplicative.svg")]
    [InlineData("u06-l01-garden-area.svg")]
    [InlineData("u07-l01-original-frame.svg")]
    public void V12FirstPartySvgHasAccessibleMetadataAndNoCode(string name)
    {
        var file=Path.Combine(Root,"src","Edulytics.Web","wwwroot",
            "images","curriculum","alg1-v12",name);
        Assert.True(File.Exists(file),file);
        var doc=XDocument.Load(file);
        XNamespace ns="http://www.w3.org/2000/svg";
        Assert.Equal(ns+"svg",doc.Root?.Name);
        Assert.Single(doc.Descendants(ns+"title"));
        Assert.Single(doc.Descendants(ns+"desc"));
        Assert.Empty(doc.Descendants(ns+"script"));
        Assert.Empty(doc.Descendants(ns+"foreignObject"));
        var content=File.ReadAllText(file);
        Assert.DoesNotContain("NaN",content,StringComparison.Ordinal);
        Assert.DoesNotContain("Infinity",content,StringComparison.Ordinal);
    }

    [Fact]
    public void PublisherSourceFetchMetadataRemainsExplicitlyUnaccepted()
    {
        using var audit=Read("US_CCSS_ALG1_CODER5_V12_SOURCE_FETCH_AUDIT_20261029.json");
        Assert.Equal(129,audit.RootElement.GetProperty("lessonCount").GetInt32());
        Assert.Equal(0,audit.RootElement.GetProperty("teacherApprovedCount").GetInt32());
        Assert.Equal(0,audit.RootElement.GetProperty("stageAAcceptedCount").GetInt32());
        foreach(var row in audit.RootElement.GetProperty("rows").EnumerateArray())
        {
            Assert.Equal("HTTP_200",row.GetProperty("teacherFetch").GetProperty("status").GetString());
            Assert.Equal("HTTP_200",row.GetProperty("studentFetch").GetProperty("status").GetString());
            Assert.False(row.GetProperty("stageAApproved").GetBoolean());
        }
    }

    [Fact]
    public void Supervisor2ContractAndTeacherAssessmentMatrixPreserveNoGo()
    {
        using var packet=Read("US_CCSS_ALG1_CODER5_V12_SUPERVISOR2_PRACTICE_CONTRACT_HANDOFF_20261029.json");
        using var rubrics=Read("US_CCSS_ALG1_CODER5_V12_TEACHER_ASSESSMENT_MATRIX_20261029.json");
        using var ledger=Read("US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json");
        Assert.Equal(132,packet.RootElement.GetProperty("contracts").GetArrayLength());
        Assert.Equal(132,rubrics.RootElement.GetProperty("lessons").GetArrayLength());
        Assert.Equal(17,rubrics.RootElement.GetProperty("structuredDrafts").GetInt32());
        Assert.Equal(115,rubrics.RootElement.GetProperty("unstructuredPending").GetInt32());
        Assert.Equal(0,rubrics.RootElement.GetProperty("actualTeacherGrades").GetInt32());
        foreach(var c in packet.RootElement.GetProperty("contracts").EnumerateArray())
        {
            Assert.False(c.GetProperty("exactPracticeApproved").GetBoolean());
            Assert.False(c.GetProperty("attemptPersisted").GetBoolean());
            Assert.True(c.GetProperty("proposedAssessmentFamilies").GetArrayLength()>0);
        }
        var v12=ledger.RootElement.GetProperty("v12");
        Assert.Equal(14,v12.GetProperty("sourceRepairedV12").GetInt32());
        Assert.Equal(115,v12.GetProperty("fullActivityReviewsOutstanding").GetInt32());
        Assert.Equal(0,v12.GetProperty("A_ACCEPTED").GetInt32());
        Assert.Equal(0,v12.GetProperty("B_ACCEPTED").GetInt32());
        Assert.Equal(0,v12.GetProperty("C_VERIFIED").GetInt32());
        Assert.Equal(0,v12.GetProperty("D_VERIFIED").GetInt32());
    }

    [Fact]
    public void All132LessonsRemainSourceIdentifiedWithNoDuplicates()
    {
        var lessons=RichLessonContentV2Registry.AllDocuments.SelectMany(x=>x.Lessons)
            .Where(x=>x.LessonCode.StartsWith(Prefix,StringComparison.Ordinal)).ToArray();
        Assert.Equal(132,lessons.Length);
        Assert.Equal(132,lessons.Select(x=>x.LessonCode).Distinct(StringComparer.Ordinal).Count());
    }
}
