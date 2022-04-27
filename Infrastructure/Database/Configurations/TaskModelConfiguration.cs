using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class TaskModelConfiguration : IEntityTypeConfiguration<TaskModel>
{
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasConversion(x => (int)x,
                intValue => (TaskStatusEnum)intValue);

        builder.HasOne(x => x.Executor)
            .WithMany(x => x.TasksInExecution)
            .HasForeignKey(x => x.ExecutorId);

        builder.HasOne(x => x.Author)
            .WithMany(x => x.AuthoredTasks)
            .HasForeignKey(x => x.AuthorId);
    }
}