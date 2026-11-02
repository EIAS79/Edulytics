using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal106OriginalSourceGuardTests {
 [Fact] public void ShaAndSourceRichTeachingAreGuards(){
 var result=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
 d.Lessons.Where(x=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L04").Select(x=>(D:d,L:x))).Single();
 Assert.Equal("05139b3a1a6b7f9b49bad0ef50c0565ccbd38b3b9dcccb138deaabe586217dea",result.L.CanonicalBodySha256);
 Assert.Contains("IM2021 Grade2 Unit7 Lesson4",result.L.Translations.Single(x=>x.CultureCode=="en").Explanation);
 Assert.Equal(CommonCoreG2Unit7FirstFourCorrections.VersionFor(106),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(result.D,result.L));
 var original=result.L.CanonicalBodySha256;
 try{result.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7FirstFourCorrections.Apply(result.D));}
 finally{result.L.CanonicalBodySha256=original;}
 }
}