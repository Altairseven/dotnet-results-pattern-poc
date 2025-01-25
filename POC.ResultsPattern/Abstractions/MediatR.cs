using MediatR;

namespace POC.ResultsPattern.Abstractions;

public interface IRequestBase {}
public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IRequestBase {}
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IRequestBase {}
public interface ICommand : IRequest<Result>, IRequestBase {}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
 where TQuery : IQuery<TResponse> {}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand {}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse> {}
