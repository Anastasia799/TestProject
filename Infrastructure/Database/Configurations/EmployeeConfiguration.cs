using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.MiddleName).HasMaxLength(50);
        
        builder.Property(x => x.Email).HasMaxLength(50)
            .IsRequired();
    }
}