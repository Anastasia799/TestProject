using Domain.Enums;

namespace Domain.Entities;

public class TaskModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    public TaskStatusEnum Status { get; set; }
    public int Priority { get; set; }
    public string Comment { get; set; }

    public int AuthorId { get; set; }
    public int ExecutorId { get; set; }


    public Employee Author { get; set; }
    public Employee Executor { get; set; }
}