using HRIS_Employee.API.Repositories.Base;

namespace HRIS_Employee.API.Repositories.Schedule
{
    public interface IScheduleRepository: IBaseRepository<Infrastructure.Persistence.Models.Schedule>
    {
        Task AddScheduleDaysAsync(List<Infrastructure.Persistence.Models.ScheduleDay> scheduleDays);
    }
}
