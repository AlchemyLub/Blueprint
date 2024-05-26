namespace AlchemyLub.Blueprint.TestServices.UnitTests.TestEnums;

/// <summary>
/// Тестовая роль пользователя
/// </summary>
[Flags]
public enum UserRoles
{
    Unknown = 0,
    View = 1 << 0,
    Edit = 1 << 1,
    Delete = 1 << 2,
    Share = 1 << 3,
    Admin = (View | Edit | Delete | Share)
}

/// <summary>
/// Идентичная тестовая роль пользователя
/// </summary>
[Flags]
public enum SameUserRoles
{
    Unknown = 0,
    View = 1 << 0,
    Edit = 1 << 1,
    Delete = 1 << 2,
    Share = 1 << 3,
    Admin = (View | Edit | Delete | Share)
}

/// <summary>
/// Некорректная тестовая роль пользователя
/// </summary>
[Flags]
public enum WrongUserRoles
{
    Unknown = 0,
    Edit = 1 << 0,
    View = 1 << 1,
    Share = 1 << 2,
    Delete = 1 << 3,
    Admin = (View | Edit | Delete | Share)
}
