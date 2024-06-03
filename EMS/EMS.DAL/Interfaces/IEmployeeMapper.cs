using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.DAL.Interfaces
{
    public interface IEmployeeMapper
    {
        public  List<EmployeeDto> ToEmployeeDto(IEnumerable<Employee> employees);
        public EmployeeDto? ToEmployeeDto(Employee employee);
        public  Employee ToEmployeeModel(EmployeeDto EmployeeDto);
    }
}