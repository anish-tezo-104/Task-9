using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.DAL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Interfaces
{
    public interface IDropDownController
    {
        public Task<IActionResult> GetLocationsAsync();
        public Task<IActionResult> GetDepartmentsAsync();
        public Task<IActionResult> GetManagersAsync();
        public Task<IActionResult> GetProjectsAsync();
    }
}