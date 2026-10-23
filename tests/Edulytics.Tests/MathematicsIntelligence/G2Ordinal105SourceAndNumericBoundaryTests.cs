using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal105SourceAndNumericBoundaryTests{
 [Fact] public void SourceOpenDiagramsAlwaysNeedTeacher(){
 var x=UsCcssG2Unit7OriginalActivities.Source(3,"351","116","601","460");
 Assert.True(x.Correct);Assert.True(x.TeacherReviewNeeded);
 Assert.False(UsCcssG2Unit7OriginalActivities.Source(3,"999","116","601","460").Correct);
 }
 [Fact] public void FirstPartyMathIsSeparateAndValidated(){
 var x=UsCcssG2Unit7OriginalActivities.NewMath(3,"101","130","340","70");
 Assert.True(x.Correct);Assert.False(x.TeacherReviewNeeded);
 Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(3,"999","130","340","70").Correct);
 }
}