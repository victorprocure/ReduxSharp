namespace ReduxSharp.Core;

public interface IStateActionReducer<TAction, TState>
{
    public TState Reduce(TState currentState, TAction action);
}