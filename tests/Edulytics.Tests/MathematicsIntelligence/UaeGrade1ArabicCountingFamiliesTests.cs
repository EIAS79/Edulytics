using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN in Stage A–B–C.
public sealed class UaeGrade1ArabicCountingFamiliesTests
{
 [Theory]
 [InlineData("uae.g1.u01.l01.read_write_1_5", 1, 5)]
 [InlineData("uae.g1.u01.l02.zero_cardinality", 0, 0)]
 [InlineData("uae.g1.u01.l03.read_write_6_8", 6, 8)]
 [InlineData("uae.g1.u01.l04.read_write_9_10", 9, 10)]
 [InlineData("uae.g1.u01.l05.read_write_11_15", 11, 15)]
 [InlineData("uae.g1.u01.l06.read_write_16_20", 16, 20)]
 public void Counts_AreExactAndRangeConstrained(string family, int min, int max)
 {
   foreach (var count in new[] { min, max })
   {
     var p = new Dictionary<string,int> { ["count"]=count, ["scene"]=1 };
     Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,count.ToString()));
     Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(count+1).ToString()));
   }
   var invalid = new Dictionary<string,int> { ["count"]=max+1, ["scene"]=1 };
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,(max+1).ToString()));
 }
 [Fact]
 public void Zero_AcceptsArabicIndicNumeral()
 {
    var p=new Dictionary<string,int> { ["count"]=0, ["scene"]=0 };
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g1.u01.l02.zero_cardinality",p,"٠"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g1.u01.l02.zero_cardinality",p,"١"));
 }
}
