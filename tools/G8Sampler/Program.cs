using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;
using Edulytics.Services.Mathematics;
var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
var auditFile=Path.Combine(root,"artifacts/curriculum-delivery/UAE-MOE-MATH/G8/general-practice-semantic-audit.json");
using var audit=JsonDocument.Parse(File.ReadAllText(auditFile));
var results=new List<object>();
int failed=0;
int correctAccepted=0, correctRejected=0, invalidAccepted=0, invalidRejected=0;
int totalNonArabicQuestions=0,totalEnglishQuestions=0,totalNonArabicSolutions=0,totalEnglishSolutions=0;
foreach(var row in audit.RootElement.GetProperty("lessons").EnumerateArray()){
    var code=row.GetProperty("lessonCode").GetString()!;
    var samples=new Dictionary<string,object>();
    var nonArabicExamples=new Dictionary<string,object>();
    var englishClauseExamples=new Dictionary<string,object>();
    var badSolutionExamples=new Dictionary<string,object>();
    int nonArabic=0, englishClauses=0, nonArabicSolutions=0, englishSolutionClauses=0;
    int count=0; string status="ready";
    if(!LessonPracticeContractRegistry.TryResolve(code,out var contract)||contract==null){
        status="missing-contract"; failed++;
    }else{
        for(int seed=1;seed<=64;seed++){
            try{
                var generated=new Stage18SkillContractPracticeEngine().GenerateComposed(
                    Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),
                    contract.ToLegacyStage18Contract(),StudentPrivatePracticeDifficulty.MyLevel,
                    3,20000+seed,[],Guid.NewGuid());
                foreach(var item in generated){
                    count++;
                    var family=item.GenerationFamily??"";
                    var prompt=item.Prompt??"";
                    if (!prompt.Any(c => c is >= 'ء' and <= 'ي'))
                    {
                        nonArabic++;
                        if(!nonArabicExamples.ContainsKey(family))
                            nonArabicExamples[family]=new {Family=family,Prompt=prompt};
                    }
                    if(System.Text.RegularExpressions.Regex.IsMatch(prompt,@"[A-Za-z]{4,}"))
                    {
                        englishClauses++;
                        if(!englishClauseExamples.ContainsKey(family))
                            englishClauseExamples[family]=new {Family=family,Prompt=prompt};
                    }
                    var solution=item.Solution??"";
                    var solutionHasArabic=solution.Any(c => c >= (char)0x0621 && c <= (char)0x064A);
                    var solutionHasEnglish=System.Text.RegularExpressions.Regex.IsMatch(
                        solution,@"[A-Za-z]{4,}");
                    if(!solutionHasArabic)nonArabicSolutions++;
                    if(solutionHasEnglish)englishSolutionClauses++;
                    if((!solutionHasArabic||solutionHasEnglish) &&
                       !badSolutionExamples.ContainsKey(family))
                        badSolutionExamples[family]=new {Family=family,Solution=solution};
                    try {
                        using var encoded=JsonDocument.Parse(item.GenerationParametersJson!);
                        var parameters=encoded.RootElement.GetProperty("parameters")
                            .EnumerateObject().ToDictionary(x=>x.Name,x=>x.Value.GetInt32());
                        if(ExactSkillContractQuestionEngine.Verify(family,parameters,item.CorrectAnswer))
                            correctAccepted++;
                        else correctRejected++;
                        if(ExactSkillContractQuestionEngine.Verify(family,parameters,"DefinitelyNotAValidMathAnswer"))
                            invalidAccepted++;
                        else invalidRejected++;
                    }catch(Exception){correctRejected++;}
                    if(!samples.ContainsKey(family))samples[family]=new {Family=family,Prompt=item.Prompt??"",CorrectAnswer=item.CorrectAnswer??""};
                }
            }catch(Exception e){status="error:"+e.GetType().Name+":"+e.Message;failed++;break;}
        }
    }
    if(contract is not null && samples.Count!=contract.AllowedQuestionFamilies.Count)
    {
        status="missing-configured-families";
        failed++;
    }
    totalNonArabicQuestions+=nonArabic;
    totalEnglishQuestions+=englishClauses;
    totalNonArabicSolutions+=nonArabicSolutions;
    totalEnglishSolutions+=englishSolutionClauses;
    results.Add(new {Ordinal=row.GetProperty("ordinal").GetInt32(),Title=row.GetProperty("arabicTitle").GetString(),
      LessonCode=code,Status=status,QuestionCount=count,Samples=samples.Values.ToArray(),
      NonArabicQuestionCount=nonArabic,EnglishClauseQuestionCount=englishClauses,
      NonArabicSolutionCount=nonArabicSolutions,EnglishSolutionClauseCount=englishSolutionClauses,
      NonArabicExamples=nonArabicExamples.Values.ToArray(),
      EnglishClauseExamples=englishClauseExamples.Values.ToArray(),
      BadSolutionExamples=badSolutionExamples.Values.ToArray()});
}
var output=Path.Combine(root,"artifacts/curriculum-delivery/UAE-MOE-MATH/G8/general-practice-76-runtime-sample.json");
File.WriteAllText(output,JsonSerializer.Serialize(results,new JsonSerializerOptions{WriteIndented=true,Encoder=System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}));
Console.WriteLine("DONE "+results.Count+" FAIL "+failed+" VERIFIED "+correctAccepted+" REJECTED_CORRECT "+correctRejected+" INVALID_REJECTED "+invalidRejected+" INVALID_ACCEPTED "+invalidAccepted+" "+output);
if(results.Count!=76 || failed!=0 || correctAccepted!=14592 ||
   invalidRejected!=14592 || correctRejected!=0 || invalidAccepted!=0 ||
   totalNonArabicQuestions!=0 || totalEnglishQuestions!=0 ||
   totalNonArabicSolutions!=0 || totalEnglishSolutions!=0)
{
    Console.Error.WriteLine($"G8 76-lesson fail-closed runtime gate: failures={failed}, Arabic-question violations={totalNonArabicQuestions+totalEnglishQuestions}, Arabic-solution violations={totalNonArabicSolutions+totalEnglishSolutions}");
    Environment.ExitCode=1;
}
