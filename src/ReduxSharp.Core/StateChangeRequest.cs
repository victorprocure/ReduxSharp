using MediatR;

namespace ReduxSharp.Core;
public sealed record StateChangeRequest<TAction, TState> : IRequest
{
    internal Action<TState, TAction> ReduceState { get; init; } = default!;
}