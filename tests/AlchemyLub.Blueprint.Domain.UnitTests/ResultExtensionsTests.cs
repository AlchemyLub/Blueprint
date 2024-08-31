namespace AlchemyLub.Blueprint.Domain.UnitTests;

/// <summary>
/// Tests for the <see cref="ResultExtensions"/> class.
/// </summary>
public class ResultExtensionsTests
{
    private const int SuccessResult = 42;
    private const int FailureResult = 2;
    private const string FailureMessage = "Failure";

    /// <summary>
    /// Verifies that the Match method calls the onSuccess function when the result is successful.
    /// </summary>
    [Fact]
    public void Match_WithSuccessfulResult_CallsOnSuccessFunction()
    {
        Result result = Result.Success();

        int value = result.Match(
            () => SuccessResult,
            _ => 0);

        value.Should().Be(SuccessResult);
    }

    /// <summary>
    /// Verifies that the Match method calls the onFailure function when the result is failed.
    /// </summary>
    [Fact]
    public void Match_WithFailedResult_CallsOnFailureFunction()
    {
        Result result = Result.Failure(new(FailureMessage));

        int value = result.Match(
            () => 0,
            _ => FailureResult);

        value.Should().Be(FailureResult);
    }

    /// <summary>
    /// Verifies that the Match method calls the onSuccess function when the result is successful and has a value.
    /// </summary>
    [Fact]
    public void Match_WithSuccessfulGenericResult_CallsOnSuccessFunction()
    {
        Result<int> result = Result.Success(SuccessResult);

        int value = result.Match(
            successValue => successValue,
            _ => 0);

        value.Should().Be(SuccessResult);
    }

    /// <summary>
    /// Verifies that the Match method calls the onFailure function when the result is failed and has a value.
    /// </summary>
    [Fact]
    public void Match_WithFailedGenericResult_CallsOnFailureFunction()
    {
        Result<string> result = Result.Failure<string>(new(FailureMessage));

        int value = result.Match(
            _ => 0,
            _ => FailureResult);

        value.Should().Be(FailureResult);
    }

    /// <summary>
    /// Verifies that the Match method throws an ArgumentNullException when the onSuccess function is null.
    /// </summary>
    [Fact]
    public void Match_WithNullOnSuccessFunction_ThrowsArgumentNullException()
    {

        Result result = Result.Success();

        Func<Result, int> action = r => r.Match(
            null,
            _ => 0);

        result.Invoking(action)
            .Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that the Match method throws an ArgumentNullException when the onFailure function is null.
    /// </summary>
    [Fact]
    public void Match_WithNullOnFailureFunction_ThrowsArgumentNullException()
    {
        Result result = Result.Success();

        Func<Result, int> action = r => r.Match(
            () => 0,
            null);

        result.Invoking(action)
            .Should()
            .Throw<ArgumentNullException>();
    }
}
