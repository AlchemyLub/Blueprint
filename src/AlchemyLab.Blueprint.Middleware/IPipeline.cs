namespace AlchemyLab.Blueprint.Middleware;

public interface IPipeline<T> where T : class
{
    Task Invoke(T service, Func<T, Func<Task>> next);
}

public interface IPipeline<T, TResult> where T : class
{
    Task<TResult> Invoke(T service, Func<T, Func<T, TResult>, Task<TResult>> next);
}
