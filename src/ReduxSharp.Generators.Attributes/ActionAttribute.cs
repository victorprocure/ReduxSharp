namespace MediateSharp.Generators;
/// <summary>
/// Decorate a class to create generate a request handler
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ActionAttribute : Attribute
{
    /// <summary>
    /// Create a handler implementation
    /// </summary>
    /// <param name="stateType">The type of request being handled</param>
    /// <exception cref="ArgumentNullException">Thrown if either request or response type is null</exception>
    public ActionAttribute(Type stateType)
    {
        if (stateType is null)
        {
            throw new ArgumentNullException(nameof(stateType));
        }

        StateType = stateType;
    }

    /// <summary>
    /// Gets the request type
    /// </summary>
    public Type StateType { get; }
}