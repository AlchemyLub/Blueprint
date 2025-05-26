namespace AlchemyLab.Blueprint.MinimalControllers.Attributes;

/// <summary>
/// Указывает, что класс является API контроллером в стиле MinimalApi
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MinimalControllerAttribute : Attribute;
