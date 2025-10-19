using Test.Application.DTO;
using Test.Domain.Entities;

namespace Test.Application.Interfaces;

public interface IEmployeeService
{
    Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default);

    Task<bool> DeleteAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<Employee>> GetByCompanyAsync(int companyId, CancellationToken ct = default);

    Task<IEnumerable<Employee>> GetByCompanyDepartmentAsync(int companyId, int deptId, CancellationToken ct = default);

    Task<bool> UpdatePartialAsync(int id, EmployeeUpdateDto dto, CancellationToken ct = default);
}