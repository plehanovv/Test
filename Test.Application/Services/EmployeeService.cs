using Test.Application.DTO;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Test.Domain.Interfaces.Repositories;

namespace Test.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;

    public EmployeeService(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default)
    {
        var employee = new Employee
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Phone = dto.Phone,
            CompanyId = dto.CompanyId,
            DepartmentId = dto.DepartmentId,
            Passport = dto.Passport != null
                ? new Passport
                {
                    Type = dto.Passport.Type,
                    Number = dto.Passport.Number
                }
                : null
        };

        return await _repo.AddAsync(employee, ct);
    }


    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _repo.DeleteAsync(id, ct);
    }

    public async Task<IEnumerable<Employee>> GetByCompanyAsync(int companyId, CancellationToken ct = default)
    {
        return await _repo.GetByCompanyAsync(companyId, ct);
    }

    public async Task<IEnumerable<Employee>> GetByCompanyDepartmentAsync(int companyId, int deptId,
        CancellationToken ct = default)
    {
        return await _repo.GetByCompanyDepartmentAsync(companyId, deptId, ct);
    }

    public async Task<bool> UpdatePartialAsync(int id, EmployeeUpdateDto dto, CancellationToken ct = default)
    {
        var employee = new Employee
        {
            Name = dto.Name ?? string.Empty,
            Surname = dto.Surname ?? string.Empty,
            Phone = dto.Phone,
            CompanyId = dto.CompanyId ?? 0,
            DepartmentId = dto.DepartmentId,
            Passport = dto.Passport != null
                ? new Passport
                {
                    Type = dto.Passport.Type,
                    Number = dto.Passport.Number
                }
                : null
        };

        return await _repo.UpdatePartialAsync(id, employee, ct);
    }
}