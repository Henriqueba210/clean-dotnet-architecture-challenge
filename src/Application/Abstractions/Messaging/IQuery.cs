using Ardalis.Result;

using MediatR;

namespace Location.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;