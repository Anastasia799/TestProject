using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace TestProject.ViewModels.Employees;

public class AddEmployeeDto
{
    [Required] [MaxLength(50)] public string FirstName { get; set; }

    [Required] [MaxLength(50)] public string LastName { get; set; }

    [MaxLength(50)] public string MiddleName { get; set; }

    [EmailAddress]
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
}