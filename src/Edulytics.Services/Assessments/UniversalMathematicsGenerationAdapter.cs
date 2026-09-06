using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Namespace-local adapter keeps the Assessment Builder call site stable while
/// routing all generation through the shared universal Mathematics engine.
/// </summary>
internal sealed class MathematicsQuestionGenerationEngine
{
    private readonly UniversalMathematicsQuestionGenerationEngine _inner = new();

    public MathematicsGenerationBatch Generate(MathematicsGenerationRequest request) =>
        _inner.Generate(request);
}
