namespace AlchemyLub.Blueprint.Generator.TestDecorators;

public class Decorator : IDecorator
{
    public async Task InvokeAsync(OriginalMethod method, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SOME TEXT");

        await method.Invoke();

        Console.WriteLine("ANOTHER TEXT");
        Console.WriteLine("ANOTHER TEXT");

        Console.WriteLine("ANOTHER TEXT");
    }
}
