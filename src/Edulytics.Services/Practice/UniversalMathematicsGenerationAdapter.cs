using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Practice;

/// <summary>
/// Namespace-local adapter keeps Student Private Practice on the same universal
/// Mathematics engine as the Teacher Assessment Builder.
/// </summary>
internal sealed class MathematicsQuestionGenerationEngine
{
    private readonly UniversalMathematicsQuestionGenerationEngine _inner = new();

    public MathematicsGenerationBatch Generate(MathematicsGenerationRequest request) =>
        _inner.Generate(request);
}
