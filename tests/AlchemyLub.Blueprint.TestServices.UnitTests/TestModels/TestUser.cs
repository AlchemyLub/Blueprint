namespace AlchemyLub.Blueprint.TestServices.UnitTests.TestModels;

public sealed class TestUser(Guid id, DateTime birthday, int age, TimeSpan timeSpan, City city, Work work)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = string.Empty;
    public DateTime Birthday { get; set; } = birthday;
    public int Age { get; set; } = age;
    public TimeSpan TimeSpan { get; set; } = timeSpan;
    public City City { get; set; } = city;
    public Work Work { get; set; } = work;
}

public sealed class City
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class Work
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}

public sealed class SameTestUser(Guid id, DateTime birthday, int age, TimeSpan timeSpan, SameCity city, SameWork work)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = string.Empty;
    public DateTime Birthday { get; set; } = birthday;
    public int Age { get; set; } = age;
    public TimeSpan TimeSpan { get; set; } = timeSpan;
    public SameCity City { get; set; } = city;
    public SameWork Work { get; set; } = work;
}

public sealed class SameCity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class SameWork
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}


public sealed class WrongTestUser(long id, long birthdayUnix, int age, TimeSpan timeSpan, WrongCity city, WrongWork work)
{
    public long Id { get; set; } = id;
    public string FullName { get; set; } = string.Empty;
    public long BirthdayUnix { get; set; } = birthdayUnix;
    public int Age { get; set; } = age;
    public TimeSpan TimeSpan { get; set; } = timeSpan;
    public WrongCity Hometown { get; set; } = city;
    public WrongWork Job { get; set; } = work;
}

public sealed class WrongCity
{
    public long CityId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class WrongWork
{
    public long WrongWorkId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public double Money { get; set; }
}
