using EMS.DAL.DTO;
using EMS.DAL.Models;

namespace EMS.DAL.Interfaces;

public interface IEmployeeDAL
{
    public Task<int> InsertAsync(EmployeeDto employee);
    public Task<List<EmployeeDto>> RetrieveAllAsync(EmployeeFilters? filters);
    public Task<EmployeeDto?> RetrieveByIdAsync(int? id);
    public Task<List<EmployeeDto>?> RetrieveByDepartmentIdAsync(int? id);
    public Task<int> UpdateAsync(int id, EmployeeDto updatedEmployee);
    public Task<int> DeleteAsync(int id);
    public Task<List<EmployeeDto>?> FilterAsync(EmployeeFilters? filters);
    public Task<int> CountAsync();
    public Task<List<EmployeeDto>?> RetrieveByRoleIdAsync(int? id);
}

