using FluentValidation;
using POC.ResultsPattern.Abstractions;

namespace POC.ResultsPattern.Features;

public record PocCommand(string ClientId, string PropertyTwo) : ICommand<PocCommandResponse>;

public class PocCommandValidator : AbstractValidator<PocCommand>
{
    public PocCommandValidator()
    {
        RuleFor(c => c.ClientId).NotEmpty();
        RuleFor(c => c.PropertyTwo).NotEmpty();
    }
}

public record PocCommandResponse(string AchucuId);

public class PocCommandHandler : ICommandHandler<PocCommand, PocCommandResponse>
{
    //private readonly IValidator<PocCommand> _validator;

    //public PocCommandHandler(IValidator<PocCommand> validator)
    //{
    //    _validator = validator;
    //}

    public async Task<Result<PocCommandResponse>> Handle(PocCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(100);

        //We could run FluentValidations for checking that the request its valid
        //but we don't need to, because the validationBehavior its running those beforehand
        //var res = await _validator.ValidateAsync(request, cancellationToken);
        //if (!res.IsValid)
        //    return Result<PocCommandResponse>.Failure(res.Errors);


        //for scenarios where we check database or external APIs, and its response represent a flow failure
        //we use defined errors (this could belong to the use case, or the domain object).
        if (request.ClientId == "Value3")
            return Result<PocCommandResponse>.Failure(PocErrors.ClientNotExists);

        if (request.ClientId == "Value4")
            return Result<PocCommandResponse>.Failure(PocErrors.ClientDeactivated);

        return Result<PocCommandResponse>.Success(new PocCommandResponse("Success"));
    }
}

public static class PocErrors {

    public static readonly Error ClientNotExists = new(
        "Client.NotExists",
        "The client with the specified id was not found");

    public static readonly Error ClientDeactivated = new(
        "Client.NotActive",
        "The client account its not active");

}
