using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C authored, NOT executed. Source WODB rationale teacher-only.
public sealed class Grade4Ordinal148NumberStringsTests
{
 [Fact] public void SourceNumberTalkStringsShowDistinctPatterns()
 {
  int[] first=[0,4,8,12,16]; int[] second=[3,6,9,12,15];
  for(int i=1;i<5;i++){Assert.Equal(4,first[i]-first[i-1]);Assert.Equal(3,second[i]-second[i-1]);}
  Assert.Contains(12,first);Assert.Contains(12,second);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u07.l02.geometry_cards_number_talk"));
 }
}
