using FluentValidation;
using POC.ResultsPattern.Abstractions;

namespace POC.ResultsPattern.Features;

public record PocQuery(
    string PropertyOne,
    string PropertyTwo
) : IQuery<PocQueryResponse>;


public class PocQueryValidator : AbstractValidator<PocQuery> {

    public PocQueryValidator()
    {
        RuleFor(c => c.PropertyOne).NotEmpty();
        RuleFor(c => c.PropertyTwo).NotEmpty();
    }
}

public record PocQueryResponse(
    List<string> Items    
);

public class PocQueryHandler : IQueryHandler<PocQuery, PocQueryResponse>
{
    public async Task<Result<PocQueryResponse>> Handle(PocQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(200);
        return Result<PocQueryResponse>.Success(
            new PocQueryResponse(["value1", "value2", "value3"])
        );
    }
}
