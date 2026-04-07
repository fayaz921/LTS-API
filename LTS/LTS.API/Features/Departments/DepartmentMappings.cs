using LTS.API.Domain.Entities;
using LTS.API.Features.Departments.DTOs;
namespace LTS.API.Features.Departments.Mappings;
public static class DepartmentMappings
{
    public static GetAllDepartmentsDto ToDto(this Department department)
    {
        return new GetAllDepartmentsDto
        {
            Id = department.Id,
            DepartmentName = department.DepartmentName,
            AddressContact = department.AddressContact,
            IsActive = department.IsActive
        };
    }
}