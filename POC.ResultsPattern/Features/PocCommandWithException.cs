using FluentValidation;
using POC.ResultsPattern.Abstractions;
using POC.ResultsPattern.Exceptions;

namespace POC.ResultsPattern.Features;

/// <summary>
/// this Commands demonstrates what happens when an actual failure happens, not for flow control, but for something like 
/// an infrastructure or connection fault 
/// </summary>
/// <param name="PropertyOne"></param>
/// <param name="PropertyTwo"></param>
public record PocCommandWithException(
    string PropertyOne,
    string PropertyTwo
) : ICommand<PocCommandResponse>;

public class PocCommandWithExceptionHandler : ICommandHandler<PocCommandWithException, PocCommandResponse>
{

    public async Task<Result<PocCommandResponse>> Handle(PocCommandWithException request, CancellationToken cancellationToken)
    {
        await Task.Delay(100);

        throw new InfrastructureException("Remote Api connection failed");
    }
}
