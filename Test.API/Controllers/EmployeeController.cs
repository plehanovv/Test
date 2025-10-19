using Microsoft.AspNetCore.Mvc;
using Test.Application.DTO;
using Test.Application.Services;

namespace Test.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeesController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreateDto dto)
    {
        var id = await _employeeService.CreateAsync(dto);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var success = await _employeeService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetByCompany(int companyId)
    {
        var employees = await _employeeService.GetByCompanyAsync(companyId);
        return Ok(employees);
    }

    [HttpGet("company/{companyId}/department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(int companyId, int departmentId)
    {
        var employees = await _employeeService.GetByCompanyDepartmentAsync(companyId, departmentId);
        return Ok(employees);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto dto)
    {
        var success = await _employeeService.UpdatePartialAsync(id, dto);
        if (!success) return NotFound();
        return NoContent();
    }
}