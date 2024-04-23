namespace MediateSharp.Generators;
/// <summary>
/// Decorate a class to create generate a request handler
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class EffectAttribute : Attribute
{
    /// <summary>
    /// Create an effect implementation
    /// </summary>
    /// <param name="actionType">The type of action being effected</param>
    /// <exception cref="ArgumentNullException">Thrown if either request or response type is null</exception>
    public EffectAttribute(Type actionType)
    {
        if (actionType is null)
        {
            throw new ArgumentNullException(nameof(actionType));
        }

        ActionType = actionType;
    }

    /// <summary>
    /// Gets the request type
    /// </summary>
    public Type ActionType { get; }
}