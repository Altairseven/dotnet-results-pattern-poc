using FluentValidation;
using POC.ResultsPattern.Abstractions;

namespace POC.ResultsPattern.Features.PocCommandWithResult;

public record PocCommandVoid(
    string PropertyOne,
    string PropertyTwo
) : ICommand;

public class CommandVoidValidator : AbstractValidator<PocCommandVoid>
{
    public CommandVoidValidator()
    {
        RuleFor(c => c.PropertyOne).NotEmpty();
        RuleFor(c => c.PropertyTwo).NotEmpty();
    }
}

public class CommandVoidHandler : ICommandHandler<PocCommandVoid>
{
    public async Task<Result> Handle(PocCommandVoid request, CancellationToken cancellationToken)
    {
        await Task.Delay(100);
        return Result.Success();
    }
}
