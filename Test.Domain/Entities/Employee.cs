namespace Test.Domain.Entities;

public class Employee : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public string Surname { get; set; } = null!;
    
    public string? Phone { get; set; }
    
    public int CompanyId { get; set; }
    
    public int? DepartmentId { get; set; }
    
    public Passport? Passport { get; set; }
    
    public Department? Department { get; set; }
}