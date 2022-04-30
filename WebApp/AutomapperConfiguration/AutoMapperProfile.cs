using Application.Dtos.Employee;
using Application.Dtos.Project;
using AutoMapper;
using TestProject.ViewModels.Employees;
using TestProject.ViewModels.Projects;

namespace TestProject.AutomapperConfiguration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddEmployeeDto, CreateEmployeeDto>();
        CreateMap<EditEmployeeDto, UpdateEmployeeDto>();

        CreateMap<AddProjectForm, CreateProjectDto>();
        CreateMap<EditProjectForm, UpdateProjectDto>();
    }
}