using EMS.DB.Models;
using EMS.DAL.DTO;
using EMS.DAL.Interfaces;
using EMS.DB.Context;
using EMS.Common.Logging;
namespace EMS.DAL;

public class DropdownDAL : IDropdownDAL
{
    private readonly EMSContext _context;
    private readonly string? _connectionString = "";
    private readonly ILogger _logger;

    public DropdownDAL(EMSContext context, ILogger logger, string connectionString) 
    {
        _context = context;
        _logger = logger;
        _connectionString = connectionString;
    }

    public List<Dropdown> GetLocationsList()
    {
        try
        {
            var locations = _context.Location.ToList();
            var dropdownList = locations.Select(location => new Dropdown
            {
                Id = location.Id,
                Name = location.Name
            }).ToList();

            return dropdownList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching locations: {ex.Message}");
            throw;
        }
    }

    public List<Dropdown> GetDepartmentsList()
    {
        try
        {
            var departments = _context.Department.ToList();
            var dropdownList = departments.Select(department => new Dropdown
            {
                Id = department.Id,
                Name = department.Name
            }).ToList();

            return dropdownList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching departments: {ex.Message}");
            throw;
        }
    }

    public List<Dropdown> GetManagersList()
    {
        try
        {
            var employeesQuery = _context.Employee;

            var managersQuery = employeesQuery.Where(e => e.IsManager);

            var dropdownItems = managersQuery.Select(e => new Dropdown
            {
                Id = e.Id,
                Name = e.FirstName + " " + e.LastName
            });

            var managers = dropdownItems.ToList();
            return managers;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching managers: {ex.Message}");
            throw;
        }
    }

    public List<Dropdown> GetProjectsList()
    {
        try
        {
            var projectsQuery = _context.Project;

            var dropdownItems = projectsQuery.Select(p => new Dropdown
            {
                Id = p.Id,
                Name = p.Name
            });

            var projects = dropdownItems.ToList();

            return projects;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching projects: {ex.Message}");
            throw;
        }
    }

    public List<Role> GetRolesList()
    {
        try
        {
            var roles = _context.Role
                .ToList();

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching roles: {ex.Message}");
            throw;
        }
    }

    public Dictionary<int, string> GetRoleNamesByDepartmentId(int? departmentId)
    {
        try
        {
            var roleNames = _context.Role
                .Where(r => r.DepartmentId == departmentId)
                .ToDictionary(r => r.Id, r => r.Name);

            return roleNames;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching role names by department ID: {ex.Message}");
            throw;
        }
    }
}
