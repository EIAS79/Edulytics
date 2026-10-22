namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson34Answers
{
    public string? CartObservation{get;set;}
    public string? ProductA{get;set;}="Fruit";public string? ProductB{get;set;}="Cards";
    public string? ProductC{get;set;}="Books";
    public string? StockA{get;set;}="7";public string? StockB{get;set;}="42";
    public string? StockC{get;set;}="51";
    public string? ModelExplanation{get;set;}
    public string? ActualRoll1{get;set;} ;public string? ActualRoll2{get;set;} ;
    public string? ActualRoll3{get;set;} ;public string? RealSalesDescription{get;set;}
    public string? ExampleStockA{get;set;}="7";public string? ExampleStockB{get;set;}="42";
    public string? ExampleStockC{get;set;}="51";
    public string? ExampleSaleA{get;set;}="3";public string? ExampleSaleB{get;set;}="23";
    public string? ExampleSaleC{get;set;}="25";public string? ExampleLeftA{get;set;}
    public string? ExampleLeftB{get;set;} ;public string? ExampleLeftC{get;set;}
    public string? ExampleTotalSold{get;set;} ;public string? ExampleTotalLeft{get;set;}
    public string? ExampleRestock{get;set;} ;
    public string? RealSalesReport{get;set;} ;
    public string? ExitLeft{get;set;} ;public string? ExitRestock{get;set;}
}
public sealed record Grade2Lesson34Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson34Answers Answers,IReadOnlyList<Grade2Lesson34Feedback> Feedback);
public sealed record Grade2Lesson34Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
