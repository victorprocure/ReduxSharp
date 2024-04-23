using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace ReduxSharp.Core;

public class StateStore
{
    private readonly ConcurrentDictionary<Type, object> _states = [];

    public ValueTask<TState> GetStateAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]TState>(CancellationToken _ = default)
    {
        var state = (TState)_states.GetOrAdd(typeof(TState), static ([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]tState) =>
        {
            var defaultState = Activator.CreateInstance(tState);
            return (TState)defaultState!;
        });

        return ValueTask.FromResult(state);
    }

    internal async Task ReplaceState<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]TState>(TState newState, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(newState);

        var currentState = await GetStateAsync<TState>(cancellationToken).ConfigureAwait(false);
        if(!_states.TryUpdate(typeof(TState), newState, currentState!))
        {
            throw new InvalidOperationException("Unable to update state");
        }
    }
}