using EMS.API.Helpers;
using EMS.API.Interfaces;
using EMS.BAL;
using EMS.BAL.Interfaces;
using EMS.DAL;
using EMS.DAL.Interfaces;
using EMS.DB.Context;
using EMS.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EMS.DAL.Models;

namespace EMS.API.Controllers;

[Route("/Api/Employees")]
[ApiController]
public class EmployeeController : ControllerBase, IEmployeeController
{
    private readonly EMSContext _context;
    private readonly IEmployeeBAL _employeeBal;
    private readonly IEmployeeDAL _employeeDal;
    private readonly Serilog.ILogger _logger;

    public EmployeeController(EMSContext context, Serilog.ILogger logger)
    {
        _context = context;
        _logger = logger;
        _employeeDal = new EmployeeDAL(_context);
        _employeeBal = new EmployeeBAL(_logger, _employeeDal);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employee)
    {
        try
        {
            var result = await _employeeBal.AddEmployeeAsync(employee);
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), result);
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.FAILED_TO_ADD_EMPLOYEE.ToString());
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employee)
    {
        try
        {
            var result = await _employeeBal.UpdateEmployeeAsync(id, employee);
            if (result == 0)
            {
                return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.EMPLOYEE_NOT_FOUND.ToString());
            }
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), result);
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.FAILED_TO_UPDATE_EMPLOYEE.ToString());
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var result = await _employeeBal.DeleteEmployeeAsync(id);
            if (result == 0)
            {
                return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.EMPLOYEE_NOT_FOUND.ToString());
            }
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), result);
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.FAILED_TO_DELETE_EMPLOYEE.ToString());
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetEmployees([FromQuery] EmployeeFilters? filters)
    {
        try
        {
            if (filters != null)
            {
                if (filters.RoleId != null)
                {
                    var employeesByRole = await _employeeBal.GetEmployeeByRoleAsync(filters.RoleId);
                    if (employeesByRole == null || employeesByRole.Count == 0)
                    {
                        return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.EMPLOYEES_NOT_FOUND.ToString());
                    }
                    return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), employeesByRole);
                }
                if (filters.DepartmentId != null)
                {
                    var employeesByDept = await _employeeBal.GetEmployeeByDepartmentIdAsync(filters.DepartmentId);
                    if (employeesByDept == null || employeesByDept.Count == 0)
                    {
                        return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.EMPLOYEES_NOT_FOUND.ToString());
                    }
                    return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), employeesByDept);
                }
                if (filters.EmployeeId != null)
                {
                    var employee = await _employeeBal.GetEmployeeByIdAsync(filters.EmployeeId);
                    if (employee == null)
                    {
                        return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.EMPLOYEE_NOT_FOUND.ToString());
                    }
                    return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), employee);
                }

                var employees = await _employeeBal.FilterEmployeesAsync(filters);
                if (employees == null || employees.Count == 0)
                {
                    return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.FAILED_TO_FILTER_EMPLOYEES.ToString());
                }
                return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), employees);

            }
            else
            {
                var employees = await _employeeBal.GetAllAsync(filters!);
                if (employees == null)
                {
                    return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.EMPLOYEES_NOT_FOUND.ToString());
                }
                return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), employees);
            }
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.INTERNAL_SERVER_ERROR.ToString());
        }
    }

    [HttpGet("Count")]
    [Authorize]
    public async Task<IActionResult> CountEmployees()
    {
        try
        {
            var count = await _employeeBal.CountEmployeesAsync();
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), count);
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.FAILED_TO_COUNT_EMPLOYEES.ToString());
        }
    }

    private AuthenticateResponse? GetCurrentEmployee()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var claims = identity.Claims;
            return new AuthenticateResponse
            {
                Id = int.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out int id) ? id : -1,
                FirstName = claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
                LastName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
                UID = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };
        }
        return null;
    }
}