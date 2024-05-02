using EMS.Common.Logging;
using EMS.DB.Context;
using EMS.DB.Models;
using EMS.DAL.DTO;
using EMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL;

public class EmployeeDAL : IEmployeeDAL
{
    private readonly EMSContext _context;
    private readonly string? _connectionString = "";
    private readonly ILogger _logger;

    public EmployeeDAL(EMSContext context, ILogger logger, string connectionString)
    {
        _context = context;
        _logger = logger;
        _connectionString = connectionString;
    }

    public int Insert(EmployeeDetails employee)
    {
        try
        {
            var newEmployee = new Employee
            {
                UID = employee.UID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Dob = employee.Dob,
                Email = employee.Email,
                MobileNumber = employee.MobileNumber,
                JoiningDate = employee.JoiningDate,
                LocationId = employee.LocationId,
                RoleId = employee.RoleId,
                DepartmentId = employee.DepartmentId,
                ProjectId = employee.ProjectId,
                ManagerId = employee.ManagerId
            };

            _context.Employee.Add(newEmployee);

            _context.SaveChanges();

            return newEmployee.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while inserting employee: {ex.Message}");
            throw;
        }
    }

    public List<EmployeeDetails> RetrieveAll()
    {
        try
        {
            var employees = _context.Employee
    .Include(e => e.Location)
    .Include(e => e.Role)
    .Include(e => e.Department)
    .Include(e => e.Project)
    .Select(e => new EmployeeDetails
    {
        Id = e.Id,
        Status = e.Status,
        FirstName = e.FirstName,
        LastName = e.LastName,
        UID = e.UID,
        Dob = e.Dob,
        Email = e.Email,
        MobileNumber = e.MobileNumber,
        JoiningDate = e.JoiningDate,
        LocationId = e.LocationId,
        LocationName = e.Location.Name,
        RoleId = e.RoleId,
        RoleName = e.Role.Name,
        DepartmentId = e.DepartmentId,
        DepartmentName = e.Department.Name,
        ProjectId = e.ProjectId,
        ProjectName = e.Project.Name,
        ManagerId = e.ManagerId,
        IsManager = e.IsManager,
        ManagerName = e.ManagerId != null ? e.Manager.FirstName + " " + e.Manager.LastName : null
    })
    .ToList();
            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving employee details: {ex.Message}");
            throw;
        }
    }

    public int Update(int id, EmployeeDetails employee)
    {
        try
        {
            var existingEmployee = _context.Employee.Find(id);

            if (existingEmployee == null)
            {
                return 0;
            }

            existingEmployee.FirstName = !string.IsNullOrEmpty(employee.FirstName) ? employee.FirstName : existingEmployee.FirstName;
            existingEmployee.LastName = !string.IsNullOrEmpty(employee.LastName) ? employee.LastName : existingEmployee.LastName;
            existingEmployee.Dob = employee.Dob ?? existingEmployee.Dob;
            existingEmployee.Email = !string.IsNullOrEmpty(employee.Email) ? employee.Email : existingEmployee.Email;
            existingEmployee.MobileNumber = !string.IsNullOrEmpty(employee.MobileNumber) ? employee.MobileNumber : existingEmployee.MobileNumber;
            existingEmployee.JoiningDate = employee.JoiningDate ?? existingEmployee.JoiningDate;
            existingEmployee.LocationId = employee.LocationId ?? existingEmployee.LocationId;
            existingEmployee.RoleId = employee.RoleId ?? existingEmployee.RoleId;
            existingEmployee.DepartmentId = employee.DepartmentId ?? existingEmployee.DepartmentId;
            existingEmployee.ManagerId = employee.ManagerId ?? existingEmployee.ManagerId;
            existingEmployee.ProjectId = employee.ProjectId ?? existingEmployee.ProjectId;

            int rowsAffected = _context.SaveChanges();

            return rowsAffected;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while updating employee: {ex.Message}");
            throw;
        }
    }

    public int Delete(int id)
    {
        try
        {
            var employeeToDelete = _context.Employee.Find(id);
            if (employeeToDelete == null)
            {
                return 0;
            }

            _context.Employee.Remove(employeeToDelete);
            int rowsAffected = _context.SaveChanges();
            return rowsAffected;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting employee: {ex.Message}");
            throw;
        }
    }

    public List<EmployeeDetails>? Filter(EmployeeFilters? filters)
    {
        if (filters == null)
        {
            return null;
        }

        try
        {
            var query = _context.Employee
            .Include(e => e.Location)
            .Include(e => e.Role)
            .Include(e => e.Department)
            .Include(e => e.Project)
            .Select(e => new EmployeeDetails
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                UID = e.UID,
                Dob = e.Dob,
                Email = e.Email,
                MobileNumber = e.MobileNumber,
                JoiningDate = e.JoiningDate,
                LocationId = e.LocationId,
                LocationName = e.Location.Name,
                RoleId = e.RoleId,
                RoleName = e.Role.Name,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name,
                ProjectId = e.ProjectId,
                ProjectName = e.Project.Name,
                ManagerId = e.ManagerId,
                IsManager = e.IsManager,
                ManagerName = e.ManagerId != null ? e.Manager.FirstName + " " + e.Manager.LastName : null
            });

            if (filters.Alphabet != null && filters.Alphabet.Count != 0)
            {
                query = query.Where(e =>
                    e.FirstName != null &&
                    filters.Alphabet.Any(alphabet =>
                        e.FirstName.Substring(0, 1).Equals(alphabet.ToString())));
            }

            if (filters.Locations != null && filters.Locations.Count != 0)
            {
                query = query.Where(e => e.LocationId.HasValue && filters.Locations.Contains(e.LocationId.Value));
            }

            if (filters.Departments != null && filters.Departments.Count != 0)
            {
                query = query.Where(e => e.DepartmentId.HasValue && filters.Departments.Contains(e.DepartmentId.Value));
            }

            if (filters.Status != null && filters.Status.Count != 0)
            {
                var statusBooleans = filters.Status.Select(s => s == 1).ToList();
                query = query.Where(e => statusBooleans.Contains(e.Status));
            }

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                query = query.Where(e =>
                    (e.FirstName != null && e.FirstName.Contains(filters.Search)) ||
                    (e.LastName != null && e.LastName.Contains(filters.Search)) ||
                    (e.UID != null && e.UID.Contains(filters.Search)) ||
                    e.Id.ToString().Contains(filters.Search));
            }

            var result = query.ToList();
            return result ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error Filtering: {ex.Message}");
            throw;
        }
    }

    public int Count()
    {
        try
        {
            int count = _context.Employee.Count();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error counting employees: {ex.Message}");
            throw;
        }
    }
}