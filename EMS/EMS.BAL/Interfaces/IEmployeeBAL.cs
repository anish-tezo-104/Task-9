using EMS.DAL.Models;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.BAL.Interfaces;

public interface IEmployeeBAL
{
    public Task<List<EmployeeDto>?> GetAllAsync(EmployeeFilters? filters);
    public Task<EmployeeDto> GetEmployeeByIdAsync(int? id);
    public Task<int> AddEmployeeAsync(EmployeeDto employee);
    public Task<int> DeleteEmployeeAsync(int id);
    public Task<int> UpdateEmployeeAsync(int id, EmployeeDto employee);
    public Task<List<EmployeeDto>?> FilterEmployeesAsync(EmployeeFilters? filters);
    public Task<int> CountEmployeesAsync();
    public  Task<List<EmployeeDto>?> GetEmployeeByDepartmentIdAsync(int? id);
    public Task<List<EmployeeDto>?> GetEmployeeByRoleAsync(int? id);
}