namespace Test.Domain.Entities;

public class Department :  BaseEntity
{
    public string Name { get; set; } = null!;
    
    public string? Phone { get; set; }
}