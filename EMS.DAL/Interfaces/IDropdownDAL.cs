using System.Data.SqlClient;
using EMS.DB.Models;
using EMS.DAL.DTO;

namespace EMS.DAL.Interfaces;

public interface IDropdownDAL
{
    List<Dropdown>? GetLocationsList();
    List<Dropdown>? GetDepartmentsList();
    List<Dropdown>? GetManagersList();
    List<Dropdown>? GetProjectsList();
    List<Role>? GetRolesList();
    Dictionary<int, string>? GetRoleNamesByDepartmentId(int? departmentId);
}