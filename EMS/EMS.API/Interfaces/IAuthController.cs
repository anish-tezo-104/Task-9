using EMS.DAL.DTO;
using EMS.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Interfaces;

public interface IAuthController
{
    public Task<IActionResult> LoginAsync(AuthenticateRequest model);
    public Task<IActionResult> RegisterAsync([FromBody] EmployeeDto employee);
}
