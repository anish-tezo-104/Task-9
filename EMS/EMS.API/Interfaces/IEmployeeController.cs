using EMS.DAL.Models;
using EMS.DAL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Interfaces;

public interface IEmployeeController
{
    public Task<IActionResult> AddEmployee([FromBody] EmployeeDto employee);
    public Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employee);
    public Task<IActionResult> DeleteEmployee(int id);
    public Task<IActionResult> GetEmployees([FromQuery] EmployeeFilters? filters = null);
    public Task<IActionResult> CountEmployees();

}
