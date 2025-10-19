namespace Test.Application.DTO;

public class EmployeeDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Surname { get; set; } = null!;
    
    public string? Phone { get; set; }
    
    public int CompanyId { get; set; }
    
    public PassportDto? Passport { get; set; }
    
    public DepartmentDto? Department { get; set; }
}