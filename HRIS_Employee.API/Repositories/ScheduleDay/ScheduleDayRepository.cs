using HRIS_Employee.API.Repositories.Base;
using HRIS_Employee.Infrastructure.Persistence.DBContext;

namespace HRIS_Employee.API.Repositories.ScheduleDay
{
    public class ScheduleDayRepository(EmployeeDbContext dbContext): BaseRepository<Infrastructure.Persistence.Models.ScheduleDay>(dbContext), IScheduleDayRepository
    {
    }
}
