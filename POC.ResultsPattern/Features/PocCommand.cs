using FluentValidation;
using POC.ResultsPattern.Abstractions;

namespace POC.ResultsPattern.Features;

public record PocCommand(
    string PropertyOne,
    string PropertyTwo
) : ICommand<PocCommandResponse>;

public class PocCommandValidator : AbstractValidator<PocCommand>
{
    public PocCommandValidator()
    {
        RuleFor(c => c.PropertyOne).NotEmpty();
        RuleFor(c => c.PropertyTwo).NotEmpty();
    }
}

public record PocCommandResponse(string AchucuId);

public class PocCommandHandler : ICommandHandler<PocCommand, PocCommandResponse>
{
    //private readonly IValidator<CommandWithResult> _validator;

    //public PocCommandHandler(IValidator<CommandWithResult> validator)
    //{
    //    _validator = validator;
    //}

    public async Task<Result<PocCommandResponse>> Handle(PocCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(100);

        ////I there was no Validator Behavior, i would need to validate like this:
        //var res = await _validator.ValidateAsync(request, cancellationToken);
        //if (!res.IsValid)
        //    return Result<CreateAchucuResult>.Failure(res.Errors);


        return Result<PocCommandResponse>.Success(new PocCommandResponse("Success"));
    }
}
