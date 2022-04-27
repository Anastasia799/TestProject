using Application.Dtos.Employee;
using AutoMapper;
using TestProject.ViewModels.Employees;

namespace TestProject.AutomapperConfiguration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<EditEmployeeDto, UpdateEmployeeDto>();
        CreateMap<AddEmployeeDto, CreateEmployeeDto>();
    }
}