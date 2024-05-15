using EMS.DAL.DTO;
using EMS.DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Interfaces;

public interface IRoleController
{
    public Task<IActionResult> AddRoleAsync([FromBody] Role role);
    public Task<IActionResult> GetRolesAsync(RoleFilters filters);
}
