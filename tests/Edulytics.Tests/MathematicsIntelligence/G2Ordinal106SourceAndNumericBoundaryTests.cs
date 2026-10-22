using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal106SourceAndNumericBoundaryTests{
 [Fact] public void SourceOpenDiagramsAlwaysNeedTeacher(){
 var x=UsCcssG2Unit7OriginalActivities.Source(4,"113","124","266","778");
 Assert.True(x.Correct);Assert.True(x.TeacherReviewNeeded);
 Assert.False(UsCcssG2Unit7OriginalActivities.Source(4,"999","124","266","778").Correct);
 }
 [Fact] public void FirstPartyMathIsSeparateAndValidated(){
 var x=UsCcssG2Unit7OriginalActivities.NewMath(4,"50","250","120","300");
 Assert.True(x.Correct);Assert.False(x.TeacherReviewNeeded);
 Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(4,"999","250","120","300").Correct);
 }
}