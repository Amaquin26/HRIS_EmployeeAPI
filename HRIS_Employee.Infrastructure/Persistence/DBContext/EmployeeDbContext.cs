using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.Infrastructure.Persistence.DBContext
{
    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<EmployeeStatus> EmployeeStatuses => Set<EmployeeStatus>();

        public DbSet<Schedule> Schedules => Set<Schedule>();

        public DbSet<ScheduleDay> ScheduleDays => Set<ScheduleDay>();

        public DbSet<ScheduleOverride> ScheduleOverrides => Set<ScheduleOverride>();

        public DbSet<ShiftRecord> ShiftRecords => Set<ShiftRecord>();

        public DbSet<ShiftAdjustment> ShiftAdjustments => Set<ShiftAdjustment>();

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

                entity.HasOne(e => e.Schedule)
                      .WithMany(es => es.Employees)
                      .HasForeignKey(e => e.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmployeeStatus>(entity =>
            {
                entity.HasIndex(es => es.Name)
                      .IsUnique();
            });

            modelBuilder.Entity<ScheduleDay>(entity =>
            {
                entity.HasOne(e => e.Schedule)
                      .WithMany(es => es.ScheduleDays)
                      .HasForeignKey(e => e.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_ScheduleDay_DayOfWeek",
                    "\"DayOfWeek\" BETWEEN 0 AND 6"));
            });

            modelBuilder.Entity<ScheduleOverride>(entity =>
            {
                entity.HasOne(e => e.Schedule)
                      .WithMany(es => es.ScheduleOverrides)
                      .HasForeignKey(e => e.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ShiftRecord>(entity =>
            {
                entity.HasOne(e => e.Employee)
                      .WithMany(es => es.ShiftRecords)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_ShiftRecord_Status",
                    "\"Status\" BETWEEN 0 AND 3"));
            });

            modelBuilder.Entity<ShiftAdjustment>(entity =>
            {
                entity.HasOne(e => e.Shift)
                      .WithMany(es => es.ShiftAdjustments)
                      .HasForeignKey(e => e.ShiftId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AdjustBy)
                      .WithMany(es => es.ShiftAdjustments)
                      .HasForeignKey(e => e.AdjustedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
