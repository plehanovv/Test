namespace Test.Infrastructure.Persistence.DTOs;

public class EmployeeQueryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Phone { get; set; }
    public int CompanyId { get; set; }
    public int? DepartmentId { get; set; }
    public string? PassportType { get; set; }
    public string? PassportNumber { get; set; }
    public string? DepartmentName { get; set; }
    public string? DepartmentPhone { get; set; }
}
