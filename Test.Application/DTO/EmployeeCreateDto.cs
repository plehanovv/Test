namespace Test.Application.DTO;

public class EmployeeCreateDto
{
    public string Name { get; set; } = null!;
    
    public string Surname { get; set; } = null!;
    
    public string? Phone { get; set; }
    
    public int CompanyId { get; set; }
    
    public PassportDto? Passport { get; set; }
    
    public int? DepartmentId { get; set; }
}