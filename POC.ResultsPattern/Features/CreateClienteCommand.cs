using FluentValidation;
using POC.ResultsPattern.Abstractions;

namespace POC.ResultsPattern.Features;

public record CreateClienteCommand(string Nombre) : ICommand<CreateClienteResponse>;

public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand> {
    public CreateClienteCommandValidator()
    {
        RuleFor(c => c.Nombre)
            .NotEmpty()
            .MinimumLength(4);
    }
}

public record CreateClienteResponse(string clientId);

public record CreateClienteHandler : ICommandHandler<CreateClienteCommand, CreateClienteResponse>
{
    public async Task<Result<CreateClienteResponse>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {




        await Task.Delay(100);
        return Result<CreateClienteResponse>.Success(new CreateClienteResponse("asdasd"));
    }
}


