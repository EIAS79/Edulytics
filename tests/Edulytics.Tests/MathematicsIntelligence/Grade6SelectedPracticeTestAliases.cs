// Grade 6 selected Practice tests use the real, public alignment validator.
// Keep a single explicit alias instead of importing the Practice namespace globally,
// which would shadow unrelated mathematics generator types.
global using LessonPracticeAlignmentValidator = Edulytics.Services.Practice.LessonPracticeAlignmentValidator;
