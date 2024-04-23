using System.Diagnostics.CodeAnalysis;

using MediatR;

namespace ReduxSharp.Core;

public sealed class StateChangedRequestHandler<TAction, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]TState> 
    : IRequestHandler<StateChangeRequest<TAction, TState>>
{
    private readonly StateStore _stateStore;

    public StateChangedRequestHandler(StateStore stateStore)
    {
        _stateStore = stateStore;
    }

    public async Task Handle(StateChangeRequest<TAction, TState> request, CancellationToken cancellationToken)
    {
        var currentState = await _stateStore.GetStateAsync<TState>(cancellationToken).ConfigureAwait(false);
        await _stateStore.ReplaceState(request.Reduce(currentState), cancellationToken).ConfigureAwait(false);
    }
}