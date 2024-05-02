
using EMS.BAL.Interfaces;
using EMS.DB.Models;
using EMS.DAL.Interfaces;

namespace EMS.BAL;

public class RoleBAL : IRoleBAL
{
    private readonly IRoleDAL _roleDal;

    public RoleBAL(IRoleDAL roleDal)
    {
        _roleDal = roleDal;
    }

    public int AddRole(Role role)
    {
        return _roleDal.Insert(role);
    }

    public List<Role>? GetAll()
    {
        List<Role> roles;
        roles = _roleDal.RetrieveAll() ?? [];
        return roles;
    }
}
