using System.Diagnostics.CodeAnalysis;

namespace ReduxSharp.Core;

public readonly struct VoidState : IEquatable<VoidState>, IComparable<VoidState>, IComparable
{
    private static readonly VoidState s_value;
    public static ref readonly VoidState Value => ref s_value;

    public int CompareTo(VoidState other) => 0;

    public int CompareTo(object? obj) => 0;

    public bool Equals(VoidState other) => CompareTo(other) == 0;

    public override int GetHashCode() => 0;
    public override bool Equals([NotNullWhen(true)] object? obj) => CompareTo(obj) == 0;
    public override string ToString() => string.Empty;

    public static bool operator ==(VoidState first, VoidState second) => first.Equals(second);
    public static bool operator !=(VoidState first, VoidState second) => !first.Equals(second);
    public static bool operator <(VoidState first, VoidState second) => !first.Equals(second);
    public static bool operator <=(VoidState first, VoidState second) => first.Equals(second);
    public static bool operator >(VoidState first, VoidState second) => !first.Equals(second);
    public static bool operator >=(VoidState first, VoidState second) => first.Equals(second);

}