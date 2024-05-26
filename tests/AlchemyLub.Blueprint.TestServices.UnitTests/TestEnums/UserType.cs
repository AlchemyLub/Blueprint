namespace AlchemyLub.Blueprint.TestServices.UnitTests.TestEnums;

/// <summary>
/// Тестовый тип пользователя
/// </summary>
public enum UserType
{
    Unknown = 0,
    Manager = 1,
    User = 2,
    Admin = 3
}

/// <summary>
/// Идентичный тестовый тип пользователя
/// </summary>
public enum SameUserType
{
    Unknown = 0,
    Manager = 1,
    User = 2,
    Admin = 3
}

/// <summary>
/// Некорректный тестовый тип пользователя
/// </summary>
public enum WrongUserType
{
    User = 0,
    Admin = 1,
    Manager = 2
}
