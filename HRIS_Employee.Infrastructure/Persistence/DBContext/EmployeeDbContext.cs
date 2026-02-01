using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.Infrastructure.Persistence.DBContext
{
    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmployeeStatus> EmployeeStatuses => Set<EmployeeStatus>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(p => p.EntraObjectId)
                      .IsUnique();

                entity.HasIndex(p => p.EmployeeNumber)
                      .IsUnique();

                entity.HasOne(e => e.EmployeeStatus)
                      .WithMany(es => es.Employees)
                      .HasForeignKey(e => e.EmployeeStatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmployeeStatus>(entity =>
            {
                entity.HasIndex(es => es.Name)
                      .IsUnique();
            });
        }
    }
}
