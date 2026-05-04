using LTS.API.Domain.Entities;
using LTS.API.Features.Departments.DTOs;
namespace LTS.API.Features.Departments.Mappings;
    public static class DepartmentMappings
    {
        public static DepartmentDto ToDto(this Department department)
        {
            return new DepartmentDto
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                AddressContact = department.AddressContact,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
            };
        }
    }