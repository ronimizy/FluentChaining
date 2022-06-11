namespace FluentChaining;

public struct Unit
{
    public static Unit Value { get; } = new Unit();
    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(Value);
}