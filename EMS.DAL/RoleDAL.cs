using EMS.Common.Logging;
using EMS.DAL.Interfaces;
using EMS.DB.Context;
using EMS.DB.Models;

namespace EMS.DAL;

public class RoleDAL : IRoleDAL
{
    private readonly EMSContext _context;
    private readonly string? _connectionString = "";
    private readonly ILogger _logger;

    public RoleDAL(EMSContext context, ILogger logger, string connectionString)
    {
        _context = context;
        _logger = logger;
        _connectionString = connectionString;
    }

    public int Insert(Role role)
    {
        try
        {
            _context.Role.Add(role);
            _context.SaveChanges();

            return role.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while adding new role: {ex.Message}");
            throw;
        }
    }

    public List<Role>? RetrieveAll()
    {
        try
        {
            List<Role> roles = [.. _context.Role];
            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while retrieving all roles: {ex.Message}");
            throw;
        }
    }
}
