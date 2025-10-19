using Test.Domain.Entities;

namespace Test.Domain.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<int> AddAsync(Employee employee, CancellationToken ct = default);
    
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    
    Task<IEnumerable<Employee>> GetByCompanyAsync(int companyId, CancellationToken ct = default);
    
    Task<IEnumerable<Employee>> GetByCompanyDepartmentAsync(int companyId, int departmentId, CancellationToken ct = default);
    
    Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default);
    
    Task<bool> UpdatePartialAsync(int id, Employee employee, CancellationToken ct = default);
}