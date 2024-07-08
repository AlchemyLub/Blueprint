namespace AlchemyLub.Blueprint.Generator.TestDecorators;

public interface IDecorator
{
    public Task InvokeAsync(OriginalMethod method, CancellationToken cancellationToken = default);
}
