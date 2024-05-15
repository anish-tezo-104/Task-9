using EMS.API.Helpers;
using EMS.API.Interfaces;
using EMS.BAL;
using EMS.BAL.Interfaces;
using EMS.DAL;
using EMS.DAL.Interfaces;
using EMS.DB.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[Route("/Api/DropDown")]
[ApiController]
public class DropDownController : IDropDownController
{
    private readonly EMSContext _context;
    private readonly IDropdownBAL _dropdownBal;
    private readonly IDropdownDAL _dropdownDal;
    private readonly Serilog.ILogger _logger;

    public DropDownController(EMSContext context, Serilog.ILogger logger)
    {
        _context = context;
        _logger = logger;
        _dropdownDal = new DropdownDAL(_context);
        _dropdownBal = new DropdownBAL(_dropdownDal,_logger);
    }

    [HttpGet("Departments")]
    [Authorize]
    public async Task<IActionResult> GetDepartmentsAsync()
    {
        try
        {
            var departments = await _dropdownBal.GetDepartmentsAsync();
            if(departments == null || departments.Count == 0)
            {
                return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.DEPARTMENTS_NOT_FOUND.ToString());
            }
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), departments);;
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.INTERNAL_SERVER_ERROR.ToString());
        }
    }

    [HttpGet("Locations")]
    [Authorize]
    public async Task<IActionResult>GetLocationsAsync()
    {
        try
        {
            var locations = await _dropdownBal.GetLocationsAsync();
            if(locations == null || locations.Count == 0)
            {
                return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.LOCATIONS_NOT_FOUND.ToString());
            }
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), locations);;
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.INTERNAL_SERVER_ERROR.ToString());
        }
    }

    [HttpGet("Managers")]
    [Authorize]
    public async Task<IActionResult>GetManagersAsync()
    {
        try
        {
            var managers = await _dropdownBal.GetManagersAsync();
            if(managers == null || managers.Count == 0)
            {
                return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.MANAGERS_NOT_FOUND.ToString());
            }
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), managers);;
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.INTERNAL_SERVER_ERROR.ToString());
        }
    }

    [HttpGet("Projects")]
    [Authorize]
    public async Task<IActionResult> GetProjectsAsync()
    {
        try
        {
            var projects = await _dropdownBal.GetProjectsAsync();
            if(projects == null || projects.Count == 0)
            {
                return ResponseHelper.WrapResponse(404, StatusMessage.ERROR.ToString(), null, ErrorCodes.PROJECTS_NOT_FOUND.ToString());
            }
            return ResponseHelper.WrapResponse(200, StatusMessage.SUCCESS.ToString(), projects);;
        }
        catch (Exception)
        {
            return ResponseHelper.WrapResponse(500, StatusMessage.FAILURE.ToString(), null, ErrorCodes.INTERNAL_SERVER_ERROR.ToString());
        }
    }
}
