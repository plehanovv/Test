using Dapper;
using Test.Domain.Entities;
using Test.Domain.Interfaces;
using Test.Domain.Interfaces.Repositories;
using Test.Infrastructure.Persistence.DTOs;

namespace Test.Infrastructure.Persistence.Repositories;

public class EmployeeRepository :  IEmployeeRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    private const string BaseEmployeeQuery = @"
        SELECT e.id, e.name, e.surname, e.phone, e.company_id AS CompanyId, e.department_id AS DepartmentId,
               p.type AS PassportType, p.number AS PassportNumber,
               d.name AS DepartmentName, d.phone AS DepartmentPhone
        FROM employees e
        LEFT JOIN passports p ON e.id = p.employee_id
        LEFT JOIN departments d ON e.department_id = d.id";

    private const string InsertEmployeeSql = @"
        INSERT INTO employees (name, surname, phone, company_id, department_id)
        VALUES (@Name, @Surname, @Phone, @CompanyId, @DepartmentId)
        RETURNING id";

    private const string InsertPassportSql = @"
        INSERT INTO passports (employee_id, type, number)
        VALUES (@EmployeeId, @Type, @Number)";

    private const string DeleteEmployeeSql = "DELETE FROM employees WHERE id = @Id";

    public EmployeeRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<int> AddAsync(Employee employee, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        var employeeId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(InsertEmployeeSql, new { 
            employee.Name, 
            employee.Surname, 
            employee.Phone, 
            employee.CompanyId, 
            DepartmentId = employee.DepartmentId
        }, cancellationToken: ct));

        if (employee.Passport != null)
        {
            await connection.ExecuteAsync(
                new CommandDefinition(InsertPassportSql, new
                {
                    EmployeeId = employeeId,
                    Type = employee.Passport.Type,
                    Number = employee.Passport.Number
                }, cancellationToken: ct)
            );
        }

        return employeeId;
    }


    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var affected = await connection.ExecuteAsync(new CommandDefinition(DeleteEmployeeSql, new { Id = id }, cancellationToken: ct));

        return affected > 0;
    }

    public async Task<IEnumerable<Employee>> GetByCompanyAsync(int companyId, CancellationToken ct = default)
    {
        var sql = $"{BaseEmployeeQuery} WHERE e.company_id = @CompanyId";

        using var connection = _dbConnectionFactory.CreateConnection();
        var result = await connection.QueryAsync<EmployeeQueryDto>(new CommandDefinition(sql, new { CompanyId = companyId }, cancellationToken: ct));

        return result.Select(MapToEmployee);
    }

    public async Task<IEnumerable<Employee>> GetByCompanyDepartmentAsync(int companyId, int departmentId, CancellationToken ct = default)
    {
        var sql = $"{BaseEmployeeQuery} WHERE e.company_id = @CompanyId AND e.department_id = @DepartmentId";

        using var connection = _dbConnectionFactory.CreateConnection();
        var result = await connection.QueryAsync<EmployeeQueryDto>(new CommandDefinition(sql, new { CompanyId = companyId, DepartmentId = departmentId }, cancellationToken: ct));

        return result.Select(MapToEmployee);
    }
    
    public async Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var sql = $"{BaseEmployeeQuery} WHERE e.id = @Id";

        using var connection = _dbConnectionFactory.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<EmployeeQueryDto>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

        return result == null ? null : MapToEmployee(result);
    }

    public async Task<bool> UpdatePartialAsync(int id, Employee employee, CancellationToken ct = default)
    {
        var updates = new List<string>();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        if (employee.Name != null) { updates.Add("name = @Name"); parameters.Add("Name", employee.Name); }
        if (employee.Surname != null) { updates.Add("surname = @Surname"); parameters.Add("Surname", employee.Surname); }
        if (employee.Phone != null) { updates.Add("phone = @Phone"); parameters.Add("Phone", employee.Phone); }
        if (employee.CompanyId != 0) { updates.Add("company_id = @CompanyId"); parameters.Add("CompanyId", employee.CompanyId); }
        if (employee.DepartmentId.HasValue) { updates.Add("department_id = @DepartmentId"); parameters.Add("DepartmentId", employee.DepartmentId); }

        if (!updates.Any()) return false;

        var sql = $"UPDATE employees SET {string.Join(", ", updates)} WHERE id = @Id";

        using var connection = _dbConnectionFactory.CreateConnection();

        var affected = await connection.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));
        
        if (employee.Passport != null)
        {
            var passportUpdates = new List<string>();
            var passportParams = new DynamicParameters();
            passportParams.Add("EmployeeId", id);

            if (employee.Passport.Type != null) { passportUpdates.Add("type = @PassportType"); passportParams.Add("PassportType", employee.Passport.Type); }
            if (employee.Passport.Number != null) { passportUpdates.Add("number = @PassportNumber"); passportParams.Add("PassportNumber", employee.Passport.Number); }

            if (passportUpdates.Any())
            {
                var passportSql = $"UPDATE passports SET {string.Join(", ", passportUpdates)} WHERE employee_id = @EmployeeId";
                await connection.ExecuteAsync(new CommandDefinition(passportSql, passportParams, cancellationToken: ct));
            }
        }

        return affected > 0;
    }
    
    private static Employee MapToEmployee(EmployeeQueryDto dto)
    {
        return new Employee
        {
            Id = dto.Id,
            Name = dto.Name,
            Surname = dto.Surname,
            Phone = dto.Phone,
            CompanyId = dto.CompanyId,
            DepartmentId = dto.DepartmentId,
            Passport = !string.IsNullOrEmpty(dto.PassportType) || !string.IsNullOrEmpty(dto.PassportNumber)
                ? new Passport { Type = dto.PassportType, Number = dto.PassportNumber } 
                : null,
            Department = !string.IsNullOrEmpty(dto.DepartmentName) || !string.IsNullOrEmpty(dto.DepartmentPhone)
                ? new Department { Name = dto.DepartmentName, Phone = dto.DepartmentPhone } 
                : null
        };
    }

    private static Passport? CreatePassport(EmployeeQueryDto dto)
    {
        return !string.IsNullOrEmpty(dto.PassportType) || !string.IsNullOrEmpty(dto.PassportNumber)
            ? new Passport { Type = dto.PassportType, Number = dto.PassportNumber } 
            : null;
    }

    private static Department? CreateDepartment(EmployeeQueryDto dto)
    {
        return !string.IsNullOrEmpty(dto.DepartmentName) || !string.IsNullOrEmpty(dto.DepartmentPhone)
            ? new Department { Name = dto.DepartmentName, Phone = dto.DepartmentPhone } 
            : null;
    }
}