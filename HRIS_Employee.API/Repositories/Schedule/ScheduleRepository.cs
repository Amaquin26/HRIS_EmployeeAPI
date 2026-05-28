using HRIS_Employee.API.Repositories.Base;
using HRIS_Employee.Infrastructure.Persistence.DBContext;

namespace HRIS_Employee.API.Repositories.Schedule
{
    public class ScheduleRepository(EmployeeDbContext dbContext) : BaseRepository<Infrastructure.Persistence.Models.Schedule>(dbContext), IScheduleRepository
    {
        public async Task AddScheduleDaysAsync(List<Infrastructure.Persistence.Models.ScheduleDay> scheduleDays)
        {
            await dbContext.ScheduleDays.AddRangeAsync(scheduleDays);
        }
    }
}
