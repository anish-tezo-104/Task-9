using EMS.DB.Models;

namespace EMS.DAL.Interfaces;

public interface IRoleDAL
{
    public int Insert(Role role);
    public List<Role>? RetrieveAll();
}