namespace FluentChaining;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LinkPriorityAttribute : Attribute
{
    public LinkPriorityAttribute(int value)
    {
        Value = value;
    }

    public int Value { get; }
}